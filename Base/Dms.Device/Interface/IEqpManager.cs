using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public interface IEqpManager
    {
        EqpUnit EqpUnit { get; set;}
        int AlarmCount { get; }
        bool AlarmResetSwitchPushed { get; set; }
        bool BuzzerOffSwitchPushed { get; set; }
        bool IsAlarmState { get; }
        bool IsWarningState { get; }
        void ResetAlarm(int alarmId);
        void ResetAllAlarm();
        void SetAlarm(int alarmId);
        int WarningCount { get; }
    }
}
