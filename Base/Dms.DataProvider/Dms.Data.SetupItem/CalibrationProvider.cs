using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class CalibrationProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private CalibrationList m_List = null;
        private CalibrationListAdapter m_Adapter = null;
        #endregion

        #region Properties
        public CalibrationList List
        {
            get { return m_List; }
        }
        public CalibrationListAdapter Adapter
        {
            get { return m_Adapter; }
        }
        #endregion

        #region Singleton code...
        public static readonly CalibrationProvider Instance = new CalibrationProvider();
        #endregion

        #region Constructor
        private CalibrationProvider()
        {
            m_Adapter = new CalibrationListAdapter();
            m_List = new CalibrationList();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void Create()
        {

        }

        public void DeleteDbGarbage()
        {
            lock (m_LockKey)
            {
                m_Adapter.DeleteGarbage(m_List);
            }
        }


        public bool InitFromDB(TagCalibrationInfo info)
        {
            lock (m_LockKey)
            {
                m_Adapter.InitFromDB(info);
                m_List.InitItem(info);
            }

            return true;
        }

        //public bool LoadFromDB()
        //{
        //    lock(m_LockKey)
        //    {
        //        return m_Adapter.LoadFromDB(m_List);
        //    }
        //}

        public bool UpdateFromDB(string name)
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFromDB();

                TagCalibrationInfo info = new TagCalibrationInfo();
                if (m_Adapter.UpdateFromDB(name, info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public bool UpdateToDB(TagCalibrationInfo info)
        {
            lock (m_LockKey)
            {
                if (m_Adapter.UpdateToDB(info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public void UpdateToDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.SaveToDB();
                m_Adapter.UpdateFromDB(m_List);
            }
        }

        public TagCalibrationInfo GetInfo(string name)
        {
            TagCalibrationInfo info = new TagCalibrationInfo();
            m_List.GetInfo(name, ref info);

            return info;
        }
        #endregion
    }
}
