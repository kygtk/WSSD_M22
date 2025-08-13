using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Util.IODefine;
using System.Threading;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorSlaveSelect), typeof(UITypeEditor))]
    [Serializable()]
    public class SlaveServo : _DeviceSlave
    {
        #region Enums
        public enum AxisCommandMode
        {
            Position = 0,
            Velocity = 1
        }
        #endregion

        #region Fields
        private EcSlaveItem_Servo m_SlaveInfo = new EcSlaveItem_Servo();
        private int m_AxisIndex;
        private ICtlDevice_Adv m_CtlDevice_Adv;
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_Servo SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }

        [XmlIgnore()]
        public int AxisIndex
        {
            get { return m_AxisIndex; }
            set { m_AxisIndex = value; }
        }
        #endregion

        #region Constructor
        public SlaveServo()
        {
            this.Initialized = false;
        }

        public SlaveServo(EcSlaveItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item as EcSlaveItem_Servo;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            this.AxisIndex = m_SlaveInfo.AxisIndex;

            Initialize();
        }

        public SlaveServo(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            m_SlaveInfo = new EcSlaveItem_Servo();
            this.Id = id;
            Initialize();
        }
        #endregion

        #region Methods
        public void SetAdvController()
        {
            if ((m_CtlDevice as ICtlDevice_Adv) != null)
                m_CtlDevice_Adv = m_CtlDevice as ICtlDevice_Adv;
        }

        public int ServoOn()
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoOn(m_Id);
        }
        public int ServoOff()
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoOff(m_Id);
        }

        public int SetPosition(double pos)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoSetPosition(m_Id, pos);
        }
        public int AlarmReset()
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoAlarmReset(m_Id);
        }

        public bool IsHomeSwitchDetected()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.ServoGetHomeSwitch(m_Id);
        }
        public bool IsPositiveLimitSwitchDetected()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.ServoGetLimitPSwitch(m_Id);
        }
        public bool IsNegativeLimitSwitchDetected()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.ServoGetLimitMSwitch(m_Id);
        }
        public bool IsOn()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.ServoIsOn(m_Id);
        }
        public bool IsDone()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.ServoIsDone(m_Id);
        }
        public bool IsAlarm()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.ServoIsAlarm(m_Id);
        }

        public double CurrentPosition()
        {
            if (m_CtlDevice_Adv == null) return double.NaN;
            return m_CtlDevice_Adv.ServoGetPosition(m_Id);
        }
        public double CurrentVelocity()
        {
            if (m_CtlDevice_Adv == null) return double.NaN;
            return m_CtlDevice_Adv.ServoGetVelocity(m_Id);
        }

        public int SetAxisCommandMode(AxisCommandMode mode)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoSetAxisCommandMode(m_Id, (int)mode);
        }

        public int StartMove_R(double dist, double vel, double acc)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoMove_R(m_Id, dist, vel, acc);
        }
        public int StartMove_S(double pos, double vel, double acc)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoMove_S(m_Id, pos, vel, acc);
        }
        public int StartMove_T(double pos, double vel, double acc, double dec)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoMove_T(m_Id, pos, vel, acc, dec);
        }
        public int StartMove_V(double vel, double acc, double dec)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoMove_V(m_Id, vel, acc, dec);
        }

        public int StartHoming()
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoHoming(m_Id);
        }

        public int Stop()
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoStop(m_Id);
        }
        public int EStop()
        {
            if (m_CtlDevice_Adv == null) return -100;
            return m_CtlDevice_Adv.ServoEStop(m_Id);
        }

        public int SetSync(short slaveid, bool enable)
        {
            if (m_CtlDevice_Adv == null) return -100;
            return enable ? m_CtlDevice_Adv.ServoSetSync(m_Id, slaveid)
                          : m_CtlDevice_Adv.ServoUnsync(slaveid);
        }

        public AxisEvent AxisState()
        {
            if (m_CtlDevice_Adv == null) return AxisEvent.NoEvent;
            return m_CtlDevice_Adv.ServoGetAxisState(m_Id);
        }
        public short ControllerError()
        {
            if (m_CtlDevice_Adv == null) return -100;
            switch (m_CtlDevice_Adv.DeviceState)
            {
                case ActiveState.UnKnown:
                case ActiveState.Run:
                    return 0;
                default:
                    return -1;
            }
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveServo slave = new SlaveServo();

            slave.Id = m_Id;
            slave.Description = this.Description;
            slave.Name = m_Name;
            slave.m_CtlDevice = m_CtlDevice;
            slave.m_Initialized = m_Initialized;

            slave.m_SlaveInfo = m_SlaveInfo.Clone();
            slave.m_AxisIndex = m_AxisIndex;

            return slave;
        }

        public override EcSlaveItem GetSlaveInfo()
        {
            return m_SlaveInfo;
        }

        public string GetState()
        {
            return "";
        }

        public override string GetSlaveStateString()
        {
            return GetState();
        }

        public override void UpdateState()
        {
            m_SlaveInfo.State = GetState();
        }

        public override void UpdateState(SlaveStateEventArgs e)
        {
            if (this.Initialized == false) return;

            if (e.Type == m_SlaveInfo.SlaveItemType && e.Id == m_SlaveInfo.AliasNo)
            {
                UpdateState();
            }
        }
        #endregion
    }
}
