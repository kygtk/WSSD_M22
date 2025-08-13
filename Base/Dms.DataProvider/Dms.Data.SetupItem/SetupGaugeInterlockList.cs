using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupGaugeInterlockList
    {
        #region Fields
        private static object m_LockKey = new object();
        private List<TagGaugeInterlock> m_Items = new List<TagGaugeInterlock>();
        #endregion

        #region Properties
        public List<TagGaugeInterlock> Items
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
        public SetupGaugeInterlockList()
        {
        }

        public bool InitItem(TagGaugeInterlock info)
        {
            lock(m_LockKey)
            {
                bool find = false;
                foreach (TagGaugeInterlock item in m_Items)
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


        public bool GetInfo(string name, ref TagGaugeInterlock info)
        {
            lock(m_LockKey)
            {
                foreach (TagGaugeInterlock item in m_Items)
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


        public bool UpdateToList(TagGaugeInterlock info)
        {
            lock(m_LockKey)
            {
                foreach (TagGaugeInterlock item in m_Items)
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
                SetupGaugeInterlockAdapter adapter = new SetupGaugeInterlockAdapter();
                adapter.SaveToDB(this);
            }
        }

        public void LoadFromDB()
        {
            lock(m_LockKey)
            {

                SetupGaugeInterlockAdapter adapter = new SetupGaugeInterlockAdapter();
                adapter.LoadFromDB(this);
            }
        }


        public void LoadFromXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupGaugeInterlockList");

                FileInfo fileInfo = new FileInfo(fileName);
                if (true == fileInfo.Exists)
                {
                    StreamReader sr = new StreamReader(fileName);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(SetupGaugeInterlockList));

                    SetupGaugeInterlockList list = xmlSer.Deserialize(sr) as SetupGaugeInterlockList;

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
                string fileName = string.Format("{0}\\{1}.xml", dirName, "SetupGaugeInterlockList");

                StreamWriter sw = new StreamWriter(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(SetupGaugeInterlockList));
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

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
