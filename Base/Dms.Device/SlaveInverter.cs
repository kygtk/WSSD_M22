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
    public class SlaveInverter : _DeviceSlave
    {
        #region Fields
        private EcSlaveItem_Inverter m_SlaveInfo = new EcSlaveItem_Inverter();
        private ICtlDevice_Adv m_CtlDevice_Adv;
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_Inverter SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveInverter()
        {
            this.Initialized = false;
        }

        public SlaveInverter(EcSlaveItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item as EcSlaveItem_Inverter;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            Initialize();
        }

        public SlaveInverter(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            m_SlaveInfo = new EcSlaveItem_Inverter();
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

        public bool IsAlarm()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.InverterGetIsAlarm(this.Id);
        }

        public bool IsRun()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.InverterGetIsRun(this.Id);
        }

        public void SetAlarmReset(bool op)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.InverterSetAlarmReset(this.Id, op);
        }

        public void SetRun(bool op)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.InverterSetRun(this.Id, op);
        }

        public void SetTargetFrequency(double frequency)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.InverterSetTargetFrequency(this.Id, frequency);
        }

        public double GetCurrentFrequency()
        {
            if (m_CtlDevice_Adv == null) return 0;
            return m_CtlDevice_Adv.InverterGetCurrentFrequency(this.Id);
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveInverter slave = new SlaveInverter();

            slave.Id = m_Id;
            slave.Description = this.Description;
            slave.Name = m_Name;
            slave.m_CtlDevice = m_CtlDevice;
            slave.m_Initialized = m_Initialized;

            slave.m_SlaveInfo = m_SlaveInfo.Clone();

            return slave;
        }

        public override EcSlaveItem GetSlaveInfo()
        {
            return m_SlaveInfo;
        }

        public string GetState()
        {
            if (IsAlarm()) return "Alarm";
            else return "OK";
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
