using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Data
{
    public class SetupHsmsCeidInfoList
    {
        #region Fields
        private static object m_LockKey = new object();
        private List<TagHsmsSetupInfo> m_Items = new List<TagHsmsSetupInfo>();
        #endregion

        #region Properties
        public List<TagHsmsSetupInfo> Items
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
        public SetupHsmsCeidInfoList()
        {
        }

        public bool InitItem(TagHsmsSetupInfo info)
        {
            lock(m_LockKey)
            {
                bool find = false;
                foreach (TagHsmsSetupInfo item in m_Items)
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


        public List<string> GetValueList(bool enable)
        {
            List<string> list = new List<string>();

            lock(m_LockKey)
            {
                foreach (TagHsmsSetupInfo item in m_Items)
                {
                    if (enable)
                    {
                        if (item.Val == OptionFormat.Enable.ToString())
                        {
                            list.Add(item.ID.ToString());
                        }
                    }
                    else
                    {
                        if (item.Val != OptionFormat.Enable.ToString())
                        {
                            list.Add(item.ID.ToString());
                        }
                    }
                }
            }

            return list;
        }

        public bool GetCeidEnable(string ceid)
        {
            bool bRv = true;

            foreach (TagHsmsSetupInfo item in m_Items)
            {
                if (item.ID.ToString() == ceid)
                {
                    if (item.Val == OptionFormat.Enable.ToString()) bRv = true;
                    else bRv = false;
                    break;
                }
            }

            return bRv;
        }

        public bool UpdateToList(TagHsmsSetupInfo info)
        {
            lock(m_LockKey)
            {
                foreach (TagHsmsSetupInfo item in m_Items)
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
                SetupHsmsCeidInfoAdapter adapter = new SetupHsmsCeidInfoAdapter();
                adapter.SaveToDB(this);
            }
        }

        public void LoadFromDB()
        {
            lock(m_LockKey)
            {

                SetupHsmsCeidInfoAdapter adapter = new SetupHsmsCeidInfoAdapter();
                adapter.LoadFromDB(this);
            }
        }


        public void LoadFromXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupHsmsCeidInfoList");

                FileInfo fileInfo = new FileInfo(fileName);
                if (true == fileInfo.Exists)
                {
                    StreamReader sr = new StreamReader(fileName);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(SetupHsmsEcidInfoList));

                    SetupHsmsEcidInfoList list = xmlSer.Deserialize(sr) as SetupHsmsEcidInfoList;

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
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupHsmsCeidInfoList");

                StreamWriter sw = new StreamWriter(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SetupHsmsEcidInfoList));
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
                string fileName = string.Format("{0}\\{1}.txt", dirName, "SetupHsmsCeidInfoList");

                Directory.CreateDirectory(dirName);
                StreamWriter sw = File.CreateText(fileName);
                sw.AutoFlush = true;

                string txt = "ID\tName\t\tValue\tUnit";
                sw.WriteLine(txt);
                sw.WriteLine("================================================================");

                int id = 1;
                foreach (TagHsmsSetupInfo info in m_Items)
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
