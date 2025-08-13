using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupCvInfoList
    {
        #region Fields
        private static object m_LockKey = new object();
        private List<TagSetupCvInfo> m_Items = new List<TagSetupCvInfo>();
        #endregion

        #region Properties
        public List<TagSetupCvInfo> Items
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
        public SetupCvInfoList()
        {
        }

        public bool InitItem(TagSetupCvInfo info)
        {
            lock (m_LockKey)
            {
                bool find = false;
                foreach (TagSetupCvInfo item in m_Items)
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


        public bool GetInfo(string name, ref TagSetupCvInfo info)
        {
            lock (m_LockKey)
            {
                foreach (TagSetupCvInfo item in m_Items)
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


        public bool UpdateToList(TagSetupCvInfo info)
        {
            lock (m_LockKey)
            {
                foreach (TagSetupCvInfo item in m_Items)
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
            lock (m_LockKey)
            {
                SetupCvInfoAdapter adapter = new SetupCvInfoAdapter();
                adapter.SaveToDB(this);
            }
        }

        public void LoadFromDB()
        {
            lock (m_LockKey)
            {

                SetupCvInfoAdapter adapter = new SetupCvInfoAdapter();
                adapter.LoadFromDB(this);
            }
        }


        public void LoadFromXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupCvInfoList");

                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    StreamReader sr = new StreamReader(fileName);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(SetupCvInfoList));

                    SetupCvInfoList list = xmlSer.Deserialize(sr) as SetupCvInfoList;

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
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupCvInfoList");

                StreamWriter sw = new StreamWriter(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SetupCvInfoList));
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
                string fileName = string.Format("{0}\\{1}.txt", dirName, "SetupCvInfoList");

                Directory.CreateDirectory(dirName);
                StreamWriter sw = File.CreateText(fileName);
                sw.AutoFlush = true;

                string txt = "ID\tName\t\tVel\tGear\tDiameter";
                sw.WriteLine(txt);
                sw.WriteLine("================================================================");

                int id = 1;
                foreach (TagSetupCvInfo info in m_Items)
                {
                    txt = string.Format("{0:d2}\t{1}\t{2}\t{3}\t{4}", id++, info.Name, info.VelRatio, info.GearRatio, info.DiaMeter);
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
