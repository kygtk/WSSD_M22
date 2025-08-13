using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Data
{
    public class Alarm
    {
        #region Fields
        //private static int m_Count;
        private static object m_LockKey = new object();
        private static AlarmList m_List = null;
        private TagAlarm m_Item = new TagAlarm();
        private static int m_BaseIndex = 0;
        #endregion

        #region Properties
        [XmlIgnore()]
        //public int Count
        //{
        //    get { return m_Count; }
        //    set { m_Count = value; }
        //}
        public TagAlarm Item
        {
            get { return m_Item; }
        }

        public int Id
        {
            get { return m_Item.Id; }
            set { m_Item.Id = value; }
        }
        public AlarmLevel Level
        {
            get { return m_Item.Level; }
            set { m_Item.Level = value; }
        }
        public AlarmCode Code
        {
            get { return m_Item.Code; }
            set { m_Item.Code = value; }
        }
        public string Name
        {
            get { return m_Item.Name; }
            set { m_Item.Name = value; }
        }

        public override string ToString()
        {
            return m_Item.Name;
        }
        #endregion

        #region Constructor
        public Alarm()
        {
        }

        public void SetBaseIndex(int baseIndex)
        {
            m_BaseIndex = baseIndex;
        }

        public Alarm(AlarmList alarmList)
        {
            m_List = alarmList;
        }

        public Alarm(string name, AlarmLevel level, AlarmCode code)
        {
            if (m_List == null)
            {
                MessageBox.Show("Alarm List is not created!");
            }

            lock (m_LockKey)
            {
                //int id = ++Count;
                int id = m_BaseIndex + (m_List.Count + 1);
                m_Item.Id = id;
                m_Item.Level = level;
                m_Item.Code = code;
                m_Item.Name = name;

                m_List.Items.Add(m_Item);
            }
        }
        #endregion

        #region Destructor
        ~Alarm()
        {
        }
        #endregion
    }
}
