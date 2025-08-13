using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Util.IODefine;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorSlaveSelect), typeof(UITypeEditor))]
    [Serializable()]
    public class SlaveAP : _DeviceSlave
    {
        #region Fields
        private EcSlaveItem_AP m_SlaveInfo = new EcSlaveItem_AP();
        private ICtlDevice_Adv m_CtlDevice_Adv;
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_AP SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveAP()
        {
            this.Initialized = false;
        }

        public SlaveAP(EcSlaveItem_AP item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            Initialize();
        }

        public SlaveAP(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            m_SlaveInfo = new EcSlaveItem_AP();
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

        public void SetPeer(PeerType type)
        {
            if (m_CtlDevice_Adv == null) return;

            m_CtlDevice_Adv.ApSetPeer(m_Id, m_SlaveInfo.Channel, type, m_SlaveInfo.PeerId);
        }

        public PairingState GetPairingState()
        {
            if (m_CtlDevice_Adv == null) return PairingState.Unpaired;

            return m_CtlDevice_Adv.ApGetPairingState(m_Id);
        }

        #region DIW
        public short DIW_GetFlowValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApDiwReadFlowValue(m_Id);
        }
        public short DIW_GetPressValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApDiwReadPressValue(m_Id);
        }
        #endregion

        #region CDA
        public short CDA_GetFlowValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApCdaReadFlowValue(m_Id);
        }
        public short CDA_GetPressValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApCdaReadPressValue(m_Id);
        }
        #endregion

        #region SmartDamper
        public SmartDamperMode SmartDamper_GetSmartDamperMode()
        {
            if (m_CtlDevice_Adv == null) return SmartDamperMode.Error;

            return m_CtlDevice_Adv.ApSmartDamperReadSmartDamperMode(m_Id);
        }
        public ushort SmartDamper_GetTargetPressure()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApSmartDamperReadTargetPressure(m_Id);
        }
        public ushort SmartDamper_GetTargetPressureHysteresis()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApSmartDamperReadTargetPressureHysteresis(m_Id);
        }
        public ushort SmartDamper_GetCurrentValveAngle()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApSmartDamperReadCurrentValveAngle(m_Id);
        }
        public ushort SmartDamper_GetCurrentPressure()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApSmartDamperReadCurrentPressure(m_Id);
        }
        public ushort SmartDamper_GetAlarmCode()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApSmartDamperReadAlarmCode(m_Id);
        }
        public bool SmartDamper_SetSmartDamperMode(SmartDamperMode mode)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApSmartDamperWriteSmartDamperMode(m_Id, mode);
        }
        public bool SmartDamper_SetTargetPressure(ushort value)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApSmartDamperWriteTargetPressure(m_Id, value);
        }
        public bool SmartDamper_SetTargetPressureHysteresis(ushort value)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApSmartDamperWriteTargetPressureHysteresis(m_Id, value);
        }
        public bool SmartDamper_SetTargetValveAngle(ushort value)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApSmartDamperWriteTargetValveAngle(m_Id, value);
        }
        #endregion

        #region D40A
        public ushort D40A_GetState()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApD40AReadState(m_Id);
        }
        #endregion

        #region D4SL
        public bool D4SL_IsOpened(int channel)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApD4SLIsOpened(m_Id, channel);
        }
        public bool D4SL_IsLocked(int channel)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApD4SLIsLocked(m_Id, channel);
        }
        public void D4SL_SetLock(int channel, bool op)
        {
            if (m_CtlDevice_Adv == null) return;

            m_CtlDevice_Adv.ApD4SLSetLock(m_Id, channel, op);
        }
        public void D4SL_SetLockAll(bool op)
        {
            if (m_CtlDevice_Adv == null) return;

            m_CtlDevice_Adv.ApD4SLSetLockAll(m_Id, op);
        }
        #endregion

        #region LFC
        public ushort LFC_GetFlowValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApLfcReadFlowValue(m_Id);
        }

        public ushort LFC_GetPressValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApLfcReadPressValue(m_Id);
        }

        public ushort LFC_GetCurrentOpenRate()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApLfcReadCurrentOpenRate(m_Id);
        }

        public bool LFC_SetTargetOpenRate(ushort value)
        {
            if (m_CtlDevice_Adv == null) return false;

            return m_CtlDevice_Adv.ApLfcWriteTargetOpenRate(m_Id, value);
        }
        #endregion

        #region Manometer
        public short Manometer_GetExhaustValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApManometerReadExhaustValue(m_Id);
        }
        #endregion

        #region LCT
        public ushort LCT_GetLevel1Value()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApLctReadLevel1Value(m_Id);
        }
        public ushort LCT_GetLevel2Value()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApLctReadLevel2Value(m_Id);
        }
        public ushort LCT_GetConsistenceValue()
        {
            if (m_CtlDevice_Adv == null) return 0;

            return m_CtlDevice_Adv.ApLctReadConsistenceValue(m_Id);
        }
        #endregion

        public short GetState()
        {
            return 0;
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveAP slave = new SlaveAP();

            slave.Id = this.Id;
            slave.Description = this.Description;
            slave.Name = m_Name;
            slave.m_CtlDevice = m_CtlDevice;
            slave.m_Initialized = m_Initialized;

            slave.SlaveInfo = m_SlaveInfo.Clone();

            return slave;
        }

        public override EcSlaveItem GetSlaveInfo()
        {
            return m_SlaveInfo;
        }

        public override string GetSlaveStateString()
        {
            return GetState().ToString();
        }

        public override void UpdateState()
        {
            m_SlaveInfo.State = GetState().ToString();
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
