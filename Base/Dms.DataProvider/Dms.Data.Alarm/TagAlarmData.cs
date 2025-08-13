using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public enum AlarmLevel
    {
        L = 0,
        S = 1
    }
    public enum AlarmCode
    {
        NotUsed = 0,
        PersonalSafety = 1,
        EquipmentSafety = 2,
        ParameterControlWarning = 3,
        ParameterControlError = 4,
        IrrecoverableError = 5,
        EquipmentStatusWarning = 6,
        AttentionFlags = 7,
        DataIntegrity = 8
    }
    public enum AlarmReportType
    { 
        Reset,
        Set
    }

    [Serializable()]
    public class TagAlarm
    {
        private int id;
        private string name;
        private AlarmLevel level;
        private AlarmCode code;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public AlarmLevel Level
        {
            get { return level; }
            set { level = value; }
        }
        public AlarmCode Code
        {
            get { return code; }
            set { code = value; }
        }

        public TagAlarm()
        { 
        
        }

        public TagAlarm(int id, string name, AlarmLevel level, AlarmCode code)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.code = code;
        }

        public void Clone(TagAlarm alarm)
        {
            this.id = alarm.id;
            this.name = alarm.name;
            this.level = alarm.level;
            this.code = alarm.code;            
        }
    }


    [Serializable()]
    public class TagCurrentAlarm
    {
        private int id;
        private string name;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public TagCurrentAlarm()
        { 
        
        }
        
        public TagCurrentAlarm(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void Clone(TagCurrentAlarm alarm)
        {
            this.id = alarm.id;
            this.name = alarm.name;
        }
    }

    [Serializable()]
    public class TagAlarmHistory
    {
        private TagAlarm alarm = new TagAlarm();
        private string time;

        public TagAlarm Alarm
        {
            get { return alarm; }
            set { alarm = value; }
        }
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        public int Id
        {
            get { return alarm.Id; }
        }
        public AlarmLevel Level
        {
            get { return alarm.Level; }
        }
        public string Name
        {
            get { return alarm.Name; }
        }

        public TagAlarmHistory()
        { 
        
        }

        private const string _DateTimeFormat = "yyyy-MM-dd HH:mm:ss:fff";
        private static string m_OldDateTime = DateTime.Now.ToString(_DateTimeFormat);
        public TagAlarmHistory(TagAlarm alarm, DateTime time)
        {
            DateTime temp = time;
            string currentTime = temp.ToString(_DateTimeFormat);
            if (m_OldDateTime == currentTime)
            {
                temp = temp.AddMilliseconds(1);
                currentTime = temp.ToString(_DateTimeFormat);
            }
            m_OldDateTime = currentTime;

            this.alarm.Clone(alarm);
            this.time = currentTime;
        }

        public TagAlarmHistory(TagAlarm alarm, string time)
        {
            this.alarm.Clone(alarm);
            this.time = time;
        }


        public void Clone(TagAlarmHistory history)
        {
            this.alarm.Clone(history.alarm);
            this.time = history.time;
        }
    }

    [Serializable()]
    public class TagAlarmReport
    {
        #region Fields
		private AlarmReportType m_ReportType;
        private TagAlarm m_AlarmInfo; 
	    #endregion

        #region Properties
        public AlarmReportType ReportType
        {
            get { return m_ReportType; }
        }
        public TagAlarm AlarmInfo
        {
            get { return m_AlarmInfo; }
        }
        #endregion

        #region Constructor
        public TagAlarmReport()
        { 
        }
        public TagAlarmReport(AlarmReportType reportType, TagAlarm alarmInfo)
        {
            m_ReportType = reportType;
            m_AlarmInfo = alarmInfo;
        }
        #endregion
    }

    [Serializable()]
    public class AlarmReportQueue
    {
        #region Fields
        private static object m_LockKey = new object();
        private Queue<TagAlarmReport> m_ReportCollection = new Queue<TagAlarmReport>();
        private bool m_UseCimReport;
        #endregion

        #region Singleton code...
        public static readonly AlarmReportQueue Instance = new AlarmReportQueue();
        #endregion

        #region Properties
        public int Count
        {
            get 
            {
                lock (m_LockKey)
                {
                    return m_ReportCollection.Count;
                }
            }
        }
        public bool UseCimReport
        {
            get { return m_UseCimReport; }
            set { m_UseCimReport = value; }
        }
        #endregion

        #region Constructor
        private AlarmReportQueue()
        {            
        }
        #endregion

        #region Methods
        public TagAlarmReport Dequeue()
        {
            lock (m_LockKey)
            {
                return m_ReportCollection.Dequeue();
            }
        }

        public void Enqueue(TagAlarmReport item)
        {
            if (m_UseCimReport)
            {
                lock (m_LockKey)
                {
                    m_ReportCollection.Enqueue(item);
                }
            }
        }

        public void Clear()
        {
            lock (m_LockKey)
            {
                m_ReportCollection.Clear();
            }
        }
        #endregion

    }
}
