using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using Dms.Common;

namespace Dms.Device
{
    [XmlInclude(typeof(PartsItem))]
    [XmlInclude(typeof(PartsItems))]
    public class PartsItemsHandler
    {
        #region Fields
        private static string m_FileName = null;
        private Mutex m_Mutex = new Mutex();
        private static PartsItems m_PartsItemsFormat = null;
        private PartsItems m_Items;
        private IServerManager m_Server;
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        private bool m_UseGlassCount = true; //eun 20091007 GlassCount는 인터락체크에서 제외하고자 할때 false시킨다. UsedTime만 체크하고 싶을 때 사용
        #endregion

        //public event PartsLifeTimeChangeEventHandler OnPartsLifeTimeChange;

        #region Properties
        public PartsItems Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public bool UseGlassCount
        {
            get { return m_UseGlassCount; }
            set { m_UseGlassCount = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly PartsItemsHandler Instance = new PartsItemsHandler();
        #endregion

        #region Constructor
        private PartsItemsHandler()
        {
        }
        #endregion

        #region Methods
        public bool CreateHandler(IServerManager server)
        {
            m_Server = server;
            _GenericCollection<PartsItem> collection = m_Server.ComponentContainer.GetCollection<PartsItem>();
            
            //Items must be added
            collection.Add(new PartsItem("Hard Disk"));
            collection.Add(new PartsItem("RAID"));
            collection.Add(new PartsItem("FAN"));
            collection.Add(new PartsItem("CMOS Battery"));
            collection.UpdateItemId();

            //Initialize only added items
            int count = collection.Count;
            for (int i = count - 4; i < count; i++)
            {
                if (collection[i].Initialize() != DmsErrors.Success)
                {
                    return false;
                }
            }

            if (collection.Count == 0) return true;

            if (m_PartsItemsFormat == null)
            {
                m_PartsItemsFormat = new PartsItems(collection);
            }

            return this.ReadXml();
            //this.SyncTags(m_Server.TagContainer);
            //this.DeleteGarbage(m_Server.GlassData);
        }

        private void CheckPath()
        {
            //AppConfig appConfig = AppConfig.Instance;
            m_AppConfig.ReadXml();

            //jemoon : 090929 - use default path option
            string filePath = m_AppConfig.PartsItemPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Parts Item Handler Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Parts Item File Folder";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.PartsItemPath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                m_CheckPath = 1;
            }
        }

        public bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                return ReadXml(m_FileName);
            }
            else
            {
                MessageBox.Show(this.GetType().Name + " : " + "The specified path is invalid. Check the path to the file and try again.");
                return false;
            }
        }

        private bool ReadXml(string fileName)
        {
            // jemoon : 081024 아래의 케이스를 고려해야 되겠지
            // 1. 파일이 없을때 : 그냥 만들면 되고
            // 2. 파일이 있는데 구조가 바뀌었을때
            //    - 파일에는 아이템이 있는데 구조에는 빠졌을때
            //    - 구조에는 아이템이 추가 되었는데 파일에는 없을때

            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                //make a new file
                if (m_Items == null)
                {
                    m_Items = new PartsItems();
                    m_Items.Clone(m_PartsItemsFormat);
                }
            }
            else
            {
                m_FileName = fileName;

                StreamReader sr = new StreamReader(m_FileName);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());
                PartsItemsHandler storedHandler;
                storedHandler = xmlSer.Deserialize(sr) as PartsItemsHandler;
                sr.Close();

                PartsItems storedItems = storedHandler.Items;
                PartsItems items = new PartsItems();

                //저장된 구조에서 PartsItem를 하나씩 가져와서 비교한다.
                foreach (PartsItem itemFormat in m_PartsItemsFormat.Items)
                {
                    PartsItem item = new PartsItem();
                    item = itemFormat.Clone();
                    foreach (PartsItem storedItem in storedItems.Items)
                    {
                        if (itemFormat.Name == storedItem.Name)
                        {
                            item.CurGlsCount = storedItem.CurGlsCount;
                            item.MaxGlsCount = storedItem.MaxGlsCount;
                            item.CurUsedTime = storedItem.CurUsedTime;
                            item.MaxUsedTime = storedItem.MaxUsedTime;
                            item.LifeTimeOver = storedItem.LifeTimeOver;
                            break;
                        }
                    }

                    items.Add(item);
                }

                m_Items = items;

            }

            return WriteXml();
        }

        public bool WriteXml()
        {
            m_Mutex.WaitOne();

            if (m_CheckPath == 1 && !string.IsNullOrEmpty(m_FileName))
            {
                bool rv = WriteXml(m_FileName);
                m_Mutex.ReleaseMutex();
                return rv;
            }
            else
            {
                m_Mutex.ReleaseMutex();
                return false;
            }
        }

        private bool WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());

            try
            {   // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();

                return false;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.CopyTo(fileName + ".old", true);
                } 

                sw = new StreamWriter(fileName);
                xmlSer.Serialize(sw, this);
                sw.Close();

                m_FileName = fileName;

                return true;
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());
                return false;
            }
        }

        public PartsItems GetItems()
        {
            PartsItems items = new PartsItems();
            items.Clone(m_Items);
            return items;
        }

        public string GetCurTime(int id)
        {
            return m_Items[id].CurUsedTime.ToString();
        }

        public string GetCurTime(string name)
        {
            return m_Items[name].CurUsedTime.ToString();
        }

        public string GetCurCount(int id)
        {
            return m_Items[id].CurGlsCount.ToString();
        }

        public string GetCurCount(string name)
        {
            return m_Items[name].CurGlsCount.ToString();
        }

        public void IncreaseCount(int id)
        {
            m_Items[id].CurGlsCount++;

            WriteXml();
        }

        public void IncreaseTime(int id)
        {
            m_Items[id].CurUsedTime++;

            WriteXml();
        }

        public void SetMaxCount(int id, int newValue)
        {
            foreach (PartsItem item in m_Items.Items)
            {
                if (item.Id == id)
                {
                    item.MaxGlsCount = newValue;
                    break;
                }
            }

            WriteXml();
        }

        public void SetMaxTime(int id, int newValue)
        {
            foreach (PartsItem item in m_Items.Items)
            {
                if (item.Id == id)
                {
                    item.MaxUsedTime = newValue;
                    break;
                }
            }

            WriteXml();
        }

        public void Reset(int id)
        {
            foreach (PartsItem item in m_Items.Items)
            {
                if (item.Id == id)
                {
                    item.CurGlsCount = 0;
                    item.CurUsedTime = 0;
                    break;
                }
            }

            WriteXml();
        }

        public bool GetLifeTimeOver(int id)
        {
            if (m_UseGlassCount &&
                ((m_Items[id].CurGlsCount > m_Items[id].MaxGlsCount) ||
                (m_Items[id].CurUsedTime > m_Items[id].MaxUsedTime)))
            {
                m_Items[id].LifeTimeOver = true;
            }
            else if (!m_UseGlassCount &&
                     (m_Items[id].CurUsedTime > m_Items[id].MaxUsedTime))
            {
                m_Items[id].LifeTimeOver = true;
            }
            else
            {
                m_Items[id].LifeTimeOver = false;
            }

            return m_Items[id].LifeTimeOver;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.GetType().Name;
        }
        #endregion
    }
}
