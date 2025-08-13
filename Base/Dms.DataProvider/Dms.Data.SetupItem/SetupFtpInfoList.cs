using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupFtpInfoList
    {
        #region Fields
        private static object m_LockKey = new object();
        private List<TagSetupInfo> m_Items = new List<TagSetupInfo>();
        #endregion

        #region Properties
        public List<TagSetupInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion

        #region Methods
        public SetupFtpInfoList()
        {
        }

        public bool InitItem(TagSetupInfo info)
        {
            lock(m_LockKey)
            {
                bool find = false;
                foreach (TagSetupInfo item in m_Items)
                {
                    if (info.Name == item.Name)
                    {
                        info.Clone(info);
                        find = true;
                        break;
                    }
                }

                if (find == false)
                {
                    m_Items.Add(info);
                }

                return true;
            }
        }


        public bool FtpInfo(string name, ref TagSetupInfo info)
        {
            lock(m_LockKey)
            {
                foreach (TagSetupInfo item in m_Items)
                {
                    if (name == item.Name)
                    {
                        info = item;
                        return true;
                    }
                }

                return false;
            }
        }


        public bool UpdateToList(TagSetupInfo info)
        {
            lock(m_LockKey)
            {
                foreach (TagSetupInfo item in m_Items)
                {
                    if (info.Name == item.Name)
                    {
                        item.Clone(info);
                        return true;
                    }
                }

                return false;
            }
        }

        public void SaveToDB()
        {
            lock(m_LockKey)
            {
                SetupFtpInfoAdapter adapter = new SetupFtpInfoAdapter();
                adapter.SaveToDB(this);
            }
        }

        public void LoadFromDB()
        {
            lock(m_LockKey)
            {

                SetupFtpInfoAdapter adapter = new SetupFtpInfoAdapter();
                adapter.LoadFromDB(this);
            }
        }


        public void LoadFromXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupFtpInfoList");

                FileInfo fileInfo = new FileInfo(fileName);
                if (true == fileInfo.Exists)
                {
                    StreamReader sr = new StreamReader(fileName);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(SetupFtpInfoList));

                    SetupFtpInfoList list = xmlSer.Deserialize(sr) as SetupFtpInfoList;

                    m_Items.Clear();
                    m_Items = list.Items;
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveToXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupFtpInfoList");

                StreamWriter sw = new StreamWriter(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SetupFtpInfoList));
                xmlSer.Serialize(sw, this);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveToTxt()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.txt", dirName, "SetupFtpInfoList");

                Directory.CreateDirectory(dirName);
                StreamWriter sw = File.CreateText(fileName);
                sw.AutoFlush = true;

                string txt = "ID\tName\t\tValue\tUnit";
                sw.WriteLine(txt);
                sw.WriteLine("================================================================");

                int id = 1;
                foreach (TagSetupInfo info in m_Items)
                {
                    txt = string.Format("{0:d2}\t{1}\t{2}\t{3}", id++, info.Name, info.Val, info.Unit.Name);
                    sw.WriteLine(txt);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
