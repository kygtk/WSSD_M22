using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Device
{
    #region Enum
    [Flags]
    public enum HeavyInterlock
    {
        None,
        Emo = 1 << 0, // 1
        Leak = 1 << 1, // 2
        Cover = 1 << 2, // 4
        Door = 1 << 3, // 8
        Area = 1 << 4  // 16  
    }
    #endregion

    public class TagInterlockCondition
    {
        #region Fields
        private int m_Alarm = 0;
        #endregion

        #region Properties
        public bool IsAlarm
        {
            get { return m_Alarm > 0; }
        }
        #endregion

        #region Constructor
        public TagInterlockCondition()
        {
        }
        #endregion

        #region Methods
        public void SetAlarm(int deviceId)
        {
            m_Alarm |= (0x01 << deviceId);
        }
        public void ResetAlarm(int deviceId)
        {
            m_Alarm &= ~(0x01 << deviceId);
        }
        #endregion
    }

    public class TagInterlock
    {
        private IServerManager m_Server;
        public TagInterlockCondition EmoCondition = EmoSensor.InterlockCondition;
        public TagInterlockCondition LeakCondition = LeakSensor.InterlockCondition;
        public TagInterlockCondition DoorCondition = DoorSensor.InterlockCondition;
        public TagInterlockCondition CoverCondition = CoverSensor.InterlockCondition;
        public TagInterlockCondition AreaCondition = AreaSensor.InterlockCondition;

        public HeavyInterlock Heavy
        {
            get
            {
                HeavyInterlock heavy = 0;
                if (EmoCondition != null) heavy |= EmoCondition.IsAlarm ? HeavyInterlock.Emo : HeavyInterlock.None;
                if (LeakCondition != null) heavy |= LeakCondition.IsAlarm ? HeavyInterlock.Leak : HeavyInterlock.None;
                if (CoverCondition != null) heavy |= CoverCondition.IsAlarm ? HeavyInterlock.Cover : HeavyInterlock.None;
                if (DoorCondition != null) heavy |= DoorCondition.IsAlarm ? HeavyInterlock.Door : HeavyInterlock.None;
                if (AreaCondition != null) heavy |= AreaCondition.IsAlarm ? HeavyInterlock.Area : HeavyInterlock.None;
                return heavy;
            }
        }

        public TagInterlock(IServerManager server)
        {
            m_Server = server;
        }
    }
}
