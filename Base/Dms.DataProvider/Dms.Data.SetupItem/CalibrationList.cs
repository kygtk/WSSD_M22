using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class CalibrationList
    {
        #region Fields
        private static object m_LockKey = new object(); 
        private List<TagCalibrationInfo> m_Items = new List<TagCalibrationInfo>();
        #endregion

        #region Properties
        public List<TagCalibrationInfo> Items
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
        public CalibrationList()
        {       
        }

        public bool InitItem(TagCalibrationInfo info)
        {
            lock(m_LockKey)
            {
                bool find = false;
                foreach (TagCalibrationInfo item in m_Items)
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


        public bool GetInfo(string name, ref TagCalibrationInfo info)
        {
            lock(m_LockKey)
            {
                foreach (TagCalibrationInfo item in m_Items)
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


        public bool UpdateToList(TagCalibrationInfo info)
        {
            lock(m_LockKey)
            {
                foreach (TagCalibrationInfo item in m_Items)
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
                CalibrationListAdapter adapter = new CalibrationListAdapter();
                adapter.SaveToDB(this);
            }
        }

        //public void LoadFromDB()
        //{
        //    lock(m_LockKey)
        //    {

        //        CalibrationListAdapter adapter = new CalibrationListAdapter();
        //        adapter.LoadFromDB(this);
        //    }
        //}
    
        public void LoadFromXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "CalibraionList");

                StreamReader sr = new StreamReader(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(CalibrationList));

                CalibrationList list = xmlSer.Deserialize(sr) as CalibrationList;

                m_Items.Clear();
                m_Items = list.Items;
                sr.Close();
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
                string fileName = string.Format("{0}\\{1}.xml", dirName, "CalibraionList");

                StreamWriter sw = new StreamWriter(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(CalibrationList));
                xmlSer.Serialize(sw, this);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    #endregion
    }
}
