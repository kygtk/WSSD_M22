using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class AlarmListProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private AlarmList m_List = null;
        private AlarmListAdapter m_Adapter = null;
        #endregion

        #region Properties
        public AlarmList List
        {
            get { return m_List; }
        }
        public AlarmListAdapter Adapter
        {
            get { return m_Adapter; }
        }
        #endregion

        #region Singleton code...
        public static readonly AlarmListProvider Instance = new AlarmListProvider();
        #endregion

        #region Constructor
        private AlarmListProvider()
        {
            m_List = new AlarmList();
            m_Adapter = new AlarmListAdapter();
        }
        #endregion

        #region Methods
        public void Create()
        {
            Alarm alarm = new Alarm(m_List);
        }

        public void Create(int baseIndex)
        {
            Alarm alarm = new Alarm(m_List);
            alarm.SetBaseIndex(baseIndex);            
        }

        public bool GetAlarm(int id, TagAlarm alarm)
        {
            lock(m_LockKey)
            {
                return m_List.GetAlarm(id, alarm);
            }
        }

        public void LoadFromDB()
        {
            lock(m_LockKey)
            {
                m_Adapter.LoadFromDB(m_List);
            }
        }

        public void SaveToDB()
        {
            lock(m_LockKey)
            {
                m_Adapter.SaveToDB(m_List);
            }
        }
        #endregion
    }
}
