///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.03.09
// Author       : 
// Description  : ApdItemsHandler
//-------------------------------------------------------------------------
// Revison History
// * 2010.03.09 - jemoon : mutex 보완
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Threading;
using Dms.Common;

namespace Dms.Device
{
    [XmlInclude(typeof(ApdItem))]
    [XmlInclude(typeof(ApdItems))]
    public class ApdItemsHandler
    {
        #region Fields
        private static string m_FileName = null;
        private Mutex m_Mutex = new Mutex();
        private static ApdItems m_ApditemsFormat = null;
        private List<ApdItems> m_Items = new List<ApdItems>();
        private short[] m_ReportData;
        private IServerManager m_Server;
        private static int m_CheckPath = -1;
        private static readonly AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        public List<ApdItems> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        #endregion

        #region Events
        public event ApdValueChangeEventHandler OnApdValueChange;
        #endregion

        #region Singleton code...
        public static readonly ApdItemsHandler Instance = new ApdItemsHandler();
        #endregion

        #region Constructor
        private ApdItemsHandler()
        {
        }
        #endregion

        #region Methods
        public bool CreateHandler(IServerManager server)
        {
            m_Server = server;
            _GenericCollection<ApdItem> collection = m_Server.ComponentContainer.GetCollection<ApdItem>();
            if (collection.Count == 0) return true;

            if (m_ApditemsFormat == null)
            {
                m_ApditemsFormat = new ApdItems(collection);
            }

            if (this.ReadXml() == false)
            {
                return false;
            }

            this.SyncTags(m_Server.TagContainer);
            this.DeleteGarbage(m_Server.GlassData);

            int count = 0;
            foreach (ApdItem item in m_ApditemsFormat.Items)
            {
                count += item.WordCount;
            }
            m_ReportData = new short[count];

            return true;
        }

        private void DeleteGarbage(IGlassDataHandler glassDataHandler)
        {
            //File에는 존재하지만 Glass Data가 없는 Item은 삭제
            int count = m_Items.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if ((((ApdItems)m_Items[i]).PositionId != -1) &&
                    (!glassDataHandler.IsExist(((ApdItems)m_Items[i]).PositionId)))
                {
                    m_Items.RemoveAt(i);
                }
            }

            //File에는 없지만 Glass Data가 있는 Item은 추가
            int[] glassIds;
            glassDataHandler.GetAllPositionId(out glassIds);
            count = glassIds.Length;

            for (int i = 0; i < count; i++)
            {
                if (!IsExist(glassIds[i]))
                {
                    AddNewItem(glassIds[i]);
                }
            }

            WriteXml();
        }

        private bool IsExist(int positionId)
        {
            foreach (ApdItems items in m_Items)
            {
                if (items.PositionId == positionId)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckPath()
        {
            //jemoon : 090929 - use default path option
            string filePath = m_AppConfig.ApdDataPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("APD Item Handler Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "APD Item File Folder";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.ApdDataPath.SelectedFolder = filePath;
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
            //    - 아이템은 일치 하는데 구조의 내용이 바뀌었다면?

            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                // Make new file
                // 마지막 History 저장용 items 생성
                ApdItems items = new ApdItems();
                items.Clone(m_ApditemsFormat);
                items.PositionId = -1;
                m_Items.Add(items);

                //ApdItems tpdItems = new ApdItems();
                //tpdItems.Clone(m_ApditemsFormat);
                //tpdItems.PositionId = -2;
                //m_Items.Add(tpdItems);

                //ApdItems lpdItems = new ApdItems();
                //lpdItems.Clone(m_ApditemsFormat);
                //lpdItems.PositionId = -3;
                //m_Items.Add(lpdItems);
            }
            else
            {
                m_FileName = fileName;

                StreamReader sr = new StreamReader(m_FileName);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());
                ApdItemsHandler storedHandler;
                storedHandler = xmlSer.Deserialize(sr) as ApdItemsHandler;
                sr.Close();

                //저장된 구조에서 ApdItems를 하나씩 가져와서 비교한다.
                foreach (ApdItems storedItems in storedHandler.Items)
                {
                    ApdItems items = new ApdItems();
                    items.PositionId = storedItems.PositionId;
                    foreach (ApdItem itemFormat in m_ApditemsFormat.Items)
                    {
                        ApdItem item = itemFormat.Clone();
                        foreach (ApdItem storedItem in storedItems.Items)
                        {
                            if (itemFormat.Name == storedItem.Name)
                            {
                                item.Value = storedItem.Value;
                                break;
                            }
                        }

                        items.Add(item);
                    }
                    m_Items.Add(items);
                }
            }

            return WriteXml();
        }

        public bool WriteXml()
        {
            m_Mutex.WaitOne();//Lock은 WriteXml()한곳에서만 하게끔.2009.08.24 kimgun

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
            {
                // m_Mutex.WaitOne();2009.08.24 

                // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();

                // m_Mutex.ReleaseMutex();2009.08.24 
            }
            catch (Exception err)
            {
                //string msg = err.ToString();
                //System.Windows.Forms.MessageBox.Show(err.ToString());
                XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);

                if (sw != null) sw.Close();
                // m_Mutex.ReleaseMutex();2009.08.24 

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

                // m_Mutex.WaitOne();2009.08.24 
                {
                    sw = new StreamWriter(fileName);
                    xmlSer.Serialize(sw, this);
                    sw.Close();

                    m_FileName = fileName;
                }
                //m_Mutex.ReleaseMutex();2009.08.24 
                return true;
            }
            catch (Exception err)
            {
                //  m_Mutex.ReleaseMutex();2009.08.24 
                //string msg = err.ToString();
                //System.Windows.Forms.MessageBox.Show(err.ToString());
                XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);

                return false;
            }
        }

        public void AddNewItem(int positionId)
        {
            ApdItems items = new ApdItems();
            items.Clone(m_ApditemsFormat);
            items.PositionId = positionId;
            this.Add(items);
        }

        public void Add(ApdItems obj)
        {
            m_Mutex.WaitOne();
            m_Items.Add(obj);
            m_Mutex.ReleaseMutex();

            WriteXml();
        }

        public void Move(int from, int to)
        {
            m_Mutex.WaitOne();
            if (m_Items.Count == 0)
            {
                m_Mutex.ReleaseMutex();
                return;
            }

            foreach (ApdItems item in m_Items)
            {
                if (item.PositionId == from)
                {
                    item.PositionId = to;
                    break;
                }
            }
            m_Mutex.ReleaseMutex();

            WriteXml();
        }

        public void Delete(int positionId)
        {
            m_Mutex.WaitOne();
            if (m_Items.Count == 0)
            {
                m_Mutex.ReleaseMutex();
                return;
            }

            foreach (ApdItems item in m_Items)
            {
                if (item.PositionId == positionId)
                {
                    m_Items.Remove(item);
                    break;
                }
            }
            m_Mutex.ReleaseMutex();

            WriteXml();
        }

        public void SetData(int positionId, int itemId, string value)
        {
            m_Mutex.WaitOne();
            ApdItems items = GetItems_Ref(positionId);
            if (items == null)
            {
                m_Mutex.ReleaseMutex();
                return;
            }

            items[itemId].Value = value;

            m_Mutex.ReleaseMutex();

            WriteXml();
        }

        public void SetData(int positionId, string ownerName)
        {
            m_Mutex.WaitOne();

            ApdItems items = GetItems_Ref(positionId);
            if (items == null)
            {
                m_Mutex.ReleaseMutex();
                return;
            }

            foreach (ApdItem item in items.Items)
            {
                if (item.OwnerUnitName == ownerName)
                {
                    item.UpdateTag();
                    //break;
                }
            }

            m_Mutex.ReleaseMutex();

            WriteXml();
        }

        public void SetTpdData()
        {
            m_Mutex.WaitOne();

            ApdItems items = GetItems_Ref(-2);

            if (items == null)
            {
                m_Mutex.ReleaseMutex();
                return;
            }

            foreach (ApdItem item in items.Items)
            {
                if (item.TpdReportEnable == true)
                {
                    item.UpdateTag();
                }
            }

            m_Mutex.ReleaseMutex();
        }

        public ApdItems GetItems(int positionId)
        {
            m_Mutex.WaitOne();

            foreach (ApdItems apdItems in m_Items)
            {
                if (apdItems.PositionId == positionId)
                {
                    ApdItems item = new ApdItems();
                    item.Clone(apdItems);
                    m_Mutex.ReleaseMutex();
                    return item;
                    //return apdItems;
                }
            }
            m_Mutex.ReleaseMutex();
            return null;
        }

        private ApdItems GetItems_Ref(int positionId)
        {
            m_Mutex.WaitOne();
            foreach (ApdItems apdItems in m_Items)
            {
                if (apdItems.PositionId == positionId)
                {
                    m_Mutex.ReleaseMutex();
                    return apdItems;
                }
            }
            m_Mutex.ReleaseMutex();
            return null;
        }

        public void SetLastData(int positionId)
        {
            m_Mutex.WaitOne();

            ApdItems items = GetItems(positionId);

            //jemoon : 091130 - 각 position별 기록 당시의 data 유지를 위해서는 아래부분을 수행하면 안됨
            //if (items != null)
            //{
            //    foreach (ApdItem item in items.Items)
            //    {
            //        if ((item.ReferenceTag.DeviceName != "") /*&&
            //            (item.Value == "" || item.Value == null)*/)
            //        {
            //            item.UpdateTag();
            //        }
            //    }
            //}

            ApdItems itemsLast = GetItems_Ref(-1);
            //items.Clone(GetItems(positionId));
            if (itemsLast != null) itemsLast.Clone(items);

            m_Mutex.ReleaseMutex();

            WriteXml();

            ApdValueChangeEventHandler ehandle = OnApdValueChange;
            if (ehandle != null)
            {
                OnApdValueChange();
            }
        }
        public void SetLotData()
        {//2009.09.02 kimgun
            m_Mutex.WaitOne();

            ApdItems items = GetItems(-1);

            if (items == null)
            {
                m_Mutex.ReleaseMutex();
                return;
            }

            ApdItems itemsLot = GetItems_Ref(-3);
            //items.Clone(GetItems(positionId));
            if (itemsLot != null) itemsLot.Clone(items);

            //jemoon : 091130 - 각 position별 기록 당시의 data 유지를 위해서는 아래부분을 
            //ApdItems itemsLot = GetItems(-3); 이후, itemsLot를 이용해야 함
            //foreach (ApdItem item in itemsLot.Items)
            //{
            //    if (item.LpdReportEnable == true)
            //    {
            //        item.UpdateTag();
            //    }
            //}

            m_Mutex.ReleaseMutex();
        }
        public void SyncTags(DeviceTags tagContainer)
        {
            foreach (ApdItems apdItems in m_Items)
            {
                apdItems.SyncRefDeviceTag(tagContainer);
            }
        }

        public string GetData(int positionId, int itemId)
        {
            m_Mutex.WaitOne();
            foreach (ApdItems apdItems in m_Items)
            {
                if (apdItems.PositionId == positionId)
                {
                    string value = ((ApdItem)apdItems.Items[itemId]).Value;
                    m_Mutex.ReleaseMutex();
                    return (value != "" ? value : "0");
                }
            }
            m_Mutex.ReleaseMutex();
            return "0";
        }

        public void GetReportData(int positionId, ref short[] reportData)
        {
            m_Mutex.WaitOne();
            foreach (ApdItems apdItems in m_Items)
            {
                if (apdItems.PositionId == positionId)
                {
                    MakeReportData(apdItems, ref reportData);
                }
            }
            m_Mutex.ReleaseMutex();

            //return m_ReportData;
        }

        public void GetReportData(int positionId, ref ushort[] reportData)
        {
            m_Mutex.WaitOne();
            foreach (ApdItems apdItems in m_Items)
            {
                if (apdItems.PositionId == positionId)
                {
                    MakeReportData(apdItems, ref reportData);
                }
            }
            m_Mutex.ReleaseMutex();
        }

        private void MakeReportData(ApdItems items, ref ushort[] reportData)
        {
            int count = 0;
            string temp;
            foreach (ApdItem item in items.Items)
            {
                if (item.ReportEnable)
                {
                    string itemValue = item.Value;
                    int wordCount = item.WordCount;
                    if (wordCount > 1)
                    {
                        int len = (itemValue.Length >= wordCount) ? itemValue.Length / wordCount : 1;
                        for (int i = 0; i < wordCount; i++)
                        {
                            temp = (itemValue != "") ? itemValue.Substring(i * len, len) : "";
                            reportData[count] = (ushort)(ConvertbyFormat(item, temp));// * (short)(1.0/item.Rate));
                            count++;
                        }
                    }
                    else
                    {
                        reportData[count] = (ushort)(ConvertbyFormat(item, itemValue));// * (short)(1.0/item.Rate));
                        count++;
                    }
                }
            }
        }

        private void MakeReportData(ApdItems items, ref short[] reportData)
        {
            int count = 0;
            string temp;
            foreach (ApdItem item in items.Items)
            {
                if (item.ReportEnable)
                {
                    string itemValue = item.Value;
                    int wordCount = item.WordCount;
                    if (wordCount > 1)
                    {
                        int len = (itemValue.Length >= wordCount) ? itemValue.Length / wordCount : 1;
                        for (int i = 0; i < wordCount; i++)
                        {
                            temp = (itemValue != "") ? itemValue.Substring(i * len, len) : "";
                            reportData[count] = (short)(ConvertbyFormat(item, temp));// * (short)(1.0/item.Rate));
                            count++;
                        }
                    }
                    else
                    {
                        reportData[count] = (short)(ConvertbyFormat(item, itemValue));// * (short)(1.0/item.Rate));
                        count++;
                    }
                }
            }
        }

        private short ConvertbyFormat(ApdItem item, string value)
        {
            if (value == "" || value == null || value == "true" || value == "false") return 0;

            short rv = 0;
            if (item.Rate < 1)
            {
                value = ((short)(Convert.ToDouble(value) * (1.0 / item.Rate))).ToString();
            }

            switch (item.Format)
            {
                case Format.ASCII:

                    rv = XFunc.ConvertToAscii(value);

                    break;
                case Format.BCD:
                    {
                        if (value != "" && value != null)
                            rv = XFunc.ConvertIntToBcd((int)Convert.ToInt16(value));
                    }
                    break;
                case Format.HEX:
                    break;
                case Format.NUMBER:
                    try
                    {
                        if (value != "" && value != null)
                            rv = Convert.ToInt16(value);
                    }
                    catch //(Exception err)
                    {
                    }
                    break;
            }
            return rv;
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
