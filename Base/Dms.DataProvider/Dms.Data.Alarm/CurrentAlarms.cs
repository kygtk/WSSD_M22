using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class CurrentAlarms
    {
        #region Fields
        private static object m_LockKey = new object();
        private List<TagCurrentAlarm> m_Items = new List<TagCurrentAlarm>();
        #endregion

        #region Properties
        public List<TagCurrentAlarm> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion

        #region Constructor
        public CurrentAlarms()
        {
        }
        #endregion

        #region Methods
        public bool IsAlarm(int id, ref int index)
        {
            bool exist = false;

            lock (m_LockKey)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (id == m_Items[i].Id)
                    {
                        index = i;
                        exist = true;
                        break;
                    }
                }
            }

            return exist;
        }


        public bool IsAlarm(int id)
        {
            bool exist = false;

            lock (m_LockKey)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (id == m_Items[i].Id)
                    {
                        exist = true;
                        break;
                    }
                }
            }

            return exist;
        }


        public void Add(TagAlarm alarm)
        {
            lock (m_LockKey)
            {
                if (!IsAlarm(alarm.Id))
                {
                    TagCurrentAlarm currentAlarm = new TagCurrentAlarm(alarm.Id, alarm.Name);
                    m_Items.Add(currentAlarm);
                }
            }
        }


        public void Add(TagCurrentAlarm alarm)
        {
            lock (m_LockKey)
            {
                if (!IsAlarm(alarm.Id))
                {
                    m_Items.Add(alarm);
                }
            }
        }

        public void Remove(int alarmId)
        {
            lock (m_LockKey)
            {
                int index = 0;
                if (IsAlarm(alarmId, ref index))
                {
                    m_Items.RemoveAt(index);
                }
            }
        }

        public void LoadFromDB()
        {
            CurrentAlarmsAdapter adapter = new CurrentAlarmsAdapter();
            adapter.LoadFromDB(this);
        }

        public void UpdateFromDB()
        {
            CurrentAlarmsAdapter adapter = new CurrentAlarmsAdapter();
            adapter.UpdateFromDB(this);
        }
        #endregion
    }
}
