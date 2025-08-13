using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class CurrentAlarmsProvider
    {
        #region Fields
        //private CurrentAlarms m_List = null;
        private static object m_LockKey = new object();
        private CurrentAlarmsAdapter m_Adapter = null;
        private ViewCurrentAlarms m_Viewer = null;
        private int m_CurrentAlarmCount = 0;
        private int m_CurrentWarningCount = 0;
        #endregion

        #region Properties
        //public CurrentAlarms List
        //{
        //    get { return m_List; }
        //}
        public int CurrentAlarmCount
        {
            get { return m_CurrentAlarmCount; }
        }
        public int CurrentWarningCount
        {
            get { return m_CurrentWarningCount; }
        }

        public CurrentAlarmsAdapter Adapter
        {
            get { return m_Adapter; }
        }

        public ViewCurrentAlarms Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly CurrentAlarmsProvider Instance = new CurrentAlarmsProvider();
        #endregion

        #region Constructor
        private CurrentAlarmsProvider()
        {
            m_Adapter = new CurrentAlarmsAdapter();
        }
        #endregion

        #region Methods
        public void Create()
        {

        }

        public bool IsAlarm(TagAlarm alarm)
        {
            lock (m_LockKey)
            {
                return m_Adapter.IsAlarm(alarm);
            }
        }

        // Call by sequence
        public void Add(TagAlarm alarm)
        {
            lock (m_LockKey)
            {
                if (!IsAlarm(alarm))
                {
                    if (m_Viewer != null)
                    {
                        m_Viewer.AddAlarm(alarm);
                    }
                    else
                    {
                        InvokeAdd(alarm);
                    }

                    if (alarm.Level == AlarmLevel.S) m_CurrentAlarmCount++;
                    else m_CurrentWarningCount++;
                }
            }
        }

        // Call by viewer
        public void InvokeAdd(TagAlarm alarm)
        {
            m_Adapter.Add(alarm);
        }

        // Call by sequence
        public void Remove(TagAlarm alarm)
        {
            lock (m_LockKey)
            {
                if (m_Viewer != null)
                {
                    m_Viewer.RemoveAlarm(alarm);
                }
                else
                {
                    InvokeRemove(alarm);
                }

                if (alarm.Level == AlarmLevel.S)
                {
                    m_CurrentAlarmCount--;
                    if (m_CurrentAlarmCount < 0) m_CurrentAlarmCount = 0;
                }
                else
                {
                    m_CurrentWarningCount--;
                    if (m_CurrentWarningCount < 0) m_CurrentWarningCount = 0;
                }
            }
        }

        // Call by viewer
        public void InvokeRemove(TagAlarm alarm)
        {
            m_Adapter.Remove(alarm.Id);
        }

        //private void Remove(int alarmId)
        //{
        //    lock(m_LockKey)
        //    {
        //        m_Adapter.Remove(alarmId);
        //    }
        //}

        public void ResetAllAlarm()
        {
            lock (m_LockKey)
            {
                m_Adapter.ClearDB();

                m_CurrentWarningCount = 0;
                m_CurrentWarningCount = 0;
            }
        }

        public void LoadFromDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFromDB();
            }
        }

        public void UpdateFromDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.UpdateFromDB();
            }
        }

        public void UpdateToDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.UpdateToDB();
            }
        }

        public List<int> GetCurrentAlarmIds()
        {
            lock (m_LockKey)
            {
                return m_Adapter.GetCurrentAlarmIds();
            }
        }
        #endregion
    }
}
