using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class EqpManager : IEqpManager
    {
        // Fields
        private static object m_LockKey = new object(); // 10.12.21 minhan
        private bool m_Initialized = false;
        private ServerManager m_Server = null;
        private AlarmListProvider m_AlarmListProvider = null;
        private CurrentAlarmsProvider m_CurrentAlarmProvider = null;
        private AlarmHistoryProvider m_AlarmHistoryProvider = null;
        private AlarmResetSwitchControl m_AlarmResetSwitches = null;
        private BuzzerControl m_Buzzers = null;
        private Simul m_Simul;
        private EqpUnit m_EqpUnit;
        private AlarmReportQueue m_AlarmReportQueue = null;

        #region Properties
        public EqpUnit EqpUnit
        {
            get { return m_EqpUnit; }
            set { m_EqpUnit = value; }
        }
        public CurrentAlarmsProvider AlarmProvider
        {
            get { return m_CurrentAlarmProvider; }
        }
        public bool IsAlarmState
        {
            get
            {
                return (m_CurrentAlarmProvider.CurrentAlarmCount > 0);
            }
        }
        public bool IsWarningState
        {
            get
            {
                return (m_CurrentAlarmProvider.CurrentWarningCount > 0);
            }
        }
        public int AlarmCount
        {
            get { return m_CurrentAlarmProvider.CurrentAlarmCount; }
        }
        public int WarningCount
        {
            get { return m_CurrentAlarmProvider.CurrentWarningCount; }
        }

        public bool AlarmResetSwitchPushed
        {
            get
            {
                if (m_Initialized == false ||
                    m_AlarmResetSwitches == null)
                {
                    return false;
                }
                else
                {
                    return m_AlarmResetSwitches.AlarmResetSwitchPushed;
                }
            }
            set
            {
                if (m_Initialized == false ||
                    m_AlarmResetSwitches == null)
                {
                    ;
                }
                else
                {
                    m_AlarmResetSwitches.AlarmResetSwitchPushed = value;
                }
            }
        }
        public bool BuzzerOffSwitchPushed
        {
            get
            {
                if (m_Initialized == false ||
                    m_Buzzers == null)
                {
                    return false;
                }
                else
                {
                    return m_Buzzers.BuzzerOffSwitchPushed;
                }
            }
            set
            {
                if (m_Initialized == false ||
                    m_Buzzers == null)
                {
                    ;
                }
                else
                {
                    m_Buzzers.BuzzerOffSwitchPushed = value;
                }
            }
        }
        #endregion

        public static readonly EqpManager Instance = new EqpManager();

        private EqpManager()
        {
        }


        public void Initialize(ServerManager server, DmsComponents components)
        {
            m_Server = server;
            m_Simul = AppConfig.Instance.Simul;

            m_AlarmListProvider = m_Server.DataProvider.AlarmList;
            m_CurrentAlarmProvider = m_Server.DataProvider.CurrentAlarms;
            m_AlarmHistoryProvider = m_Server.DataProvider.AlarmHistory;

            m_AlarmReportQueue = AlarmReportQueue.Instance;
            m_AlarmReportQueue.UseCimReport = true;

            IComponentContainer componentContainer = components.ComponentContainer;
            m_AlarmResetSwitches = new AlarmResetSwitchControl(componentContainer.GetCollection<AlarmResetSwitch>());
            m_Buzzers = new BuzzerControl(componentContainer.GetCollection<Buzzer>());

            m_Initialized = true;
        }

        public void SetAlarm(int alarmId)
        {
            lock (m_LockKey) // 10.12.21 minhan
            {
                TagAlarm alarm = new TagAlarm();
                bool existInAlarmList = m_AlarmListProvider.GetAlarm(alarmId, alarm);
                bool occured = m_CurrentAlarmProvider.IsAlarm(alarm);

                if (existInAlarmList && !occured)
                {
                    TagAlarmHistory history = new TagAlarmHistory(alarm, DateTime.Now);
                    m_CurrentAlarmProvider.Add(alarm);
                    m_AlarmHistoryProvider.Add(history);

                    //jemoon : 090609 - for CIM report
                    AlarmReportQueue.Instance.Enqueue(new TagAlarmReport(AlarmReportType.Set, alarm));
                }
            }
        }

        public void ResetAlarm(int alarmId)
        {
            lock (m_LockKey) // 10.12.21 minhan
            {
                TagAlarm alarm = new TagAlarm();
                bool existInAlarmList = m_AlarmListProvider.GetAlarm(alarmId, alarm);
                bool occured = m_CurrentAlarmProvider.IsAlarm(alarm);
                if (existInAlarmList && occured)
                {
                    m_CurrentAlarmProvider.Remove(alarm);

                    //jemoon : 090609 - for CIM report
                    AlarmReportQueue.Instance.Enqueue(new TagAlarmReport(AlarmReportType.Reset, alarm));
                }
            }
        }

        public void ResetAllAlarm()
        {
            m_CurrentAlarmProvider.ResetAllAlarm();
        }
    }
}
