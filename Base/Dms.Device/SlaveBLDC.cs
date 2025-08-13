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
    public class SlaveBLDC : _DeviceSlave
    {
        #region Fields
        private EcSlaveItem_BLDC m_SlaveInfo = new EcSlaveItem_BLDC();
        private ICtlDevice_Adv m_CtlDevice_Adv;
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_BLDC SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveBLDC()
        {
            this.Initialized = false;
        }

        public SlaveBLDC(EcSlaveItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item as EcSlaveItem_BLDC;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            Initialize();
        }

        public SlaveBLDC(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            m_SlaveInfo = new EcSlaveItem_BLDC();
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

        public double GetLoadFactor()
        {
            if (m_CtlDevice_Adv == null) return 0;
            return m_CtlDevice_Adv.BldcGetLoadFactor(this.Id);
        }

        public short GetCurrentRPM()
        {
            if (m_CtlDevice_Adv == null) return 0;
            return m_CtlDevice_Adv.BldcGetCurrentRPM(this.Id);
        }

        public bool IsTurnFw()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.BldcGetIsTurnFw(this.Id);
        }

        public bool IsTurnBw()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.BldcGetIsTurnBw(this.Id);
        }

        public bool IsAlarm()
        {
            if (m_CtlDevice_Adv == null) return false;
            return m_CtlDevice_Adv.BldcGetIsAlarm(this.Id);
        }

        public void SetRotateFw(bool op)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.BldcSetRotateFw(this.Id, op);
        }

        public void SetRotateBw(bool op)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.BldcSetRotateBw(this.Id, op);
        }

        public void SetAlarmReset(bool op)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.BldcSetAlarmReset(this.Id, op);
        }

        public void SetAccTime(ushort val)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.BldcSetAccTime(this.Id, val);
        }

        public void SetDecTime(ushort val)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.BldcSetDecTime(this.Id, val);
        }

        public void SetTargetRPM(ushort val)
        {
            if (m_CtlDevice_Adv == null) return;
            m_CtlDevice_Adv.BldcSetTargetRPM(this.Id, val);
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveBLDC slave = new SlaveBLDC();

            slave.m_Id = m_Id;
            slave.m_Description = this.Description;
            slave.m_Name = m_Name;
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
            else if (IsTurnBw() == IsTurnFw()) return "Stopped";
            else if (IsTurnFw()) return "FW";
            else return "BW";
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
