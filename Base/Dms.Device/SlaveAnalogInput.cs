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
    public class SlaveAnalogInput : _DeviceSlave
    {
        #region Fields
        private EcSlaveItem_AI m_SlaveInfo = new EcSlaveItem_AI();
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_AI SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveAnalogInput()
        {
            this.Initialized = false;
        }

        public SlaveAnalogInput(EcSlaveItem_AI item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            Initialize();
        }

        public SlaveAnalogInput(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            this.Id = id;
            Initialize();
        }
        #endregion

        #region Methods
        public short GetState()
        {
            return m_CtlDevice.ReadAiAsync(this.Id);
        }

        public void SetState(short state)
        {
            m_CtlDevice.WriteAiSync(this.Id, state);
        }

        public short[] GetStates(int size)
        {
            short[] values = new short[size];

            for (int i = 0; i < size; i++)
            {
                values[i] = m_CtlDevice.ReadAiAsync(m_Id + i);
            }

            return values;
        }

        public void SetStates(short[] values, int startIndex, int size)
        {
            for (int i = 0; i < size; i++)
            {
                m_CtlDevice.WriteAiSync(m_Id + i, values[startIndex + i]);
            }
        }

        public object GetStateAs()
        {
            return m_CtlDevice.Read(m_Id, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }

        public void SetStateSync(object value)
        {
            m_CtlDevice.Write(m_Id, value, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveAnalogInput slave = new SlaveAnalogInput();

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
