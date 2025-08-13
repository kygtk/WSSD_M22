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
    public class SlaveAnalogOutput : _DeviceSlave
    {
        #region Fields
        private EcSlaveItem_AO m_SlaveInfo = new EcSlaveItem_AO();
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_AO SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveAnalogOutput()
        {
            this.Initialized = false;
        }
        public SlaveAnalogOutput(EcSlaveItem_AO item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            Initialize();
        }
        public SlaveAnalogOutput(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            this.Id = id;
            Initialize();
        }
        #endregion

        #region Methods
        public ushort GetState()
        {
            return m_CtlDevice.ReadAoAsync(this.Id);
        }

        public void SetState(ushort state)
        {
            m_CtlDevice.WriteAoAsync(this.Id, state);
        }

        public ushort[] GetStates(int size)
        {
            ushort[] values = new ushort[size];

            for (int i = 0; i < size; i++)
            {
                values[i] = m_CtlDevice.ReadAoAsync(m_Id + i);
            }

            return values;
        }

        public void SetStates(ushort[] values, int startIndex, int size)
        {
            for (int i = 0; i < size; i++)
            {
                m_CtlDevice.WriteAoSync(m_Id + i, values[startIndex + i]);
            }
        }

        public object GetStateAs()
        {
            return m_CtlDevice.Read(m_Id, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }

        public void SetStateSync(object value)
        {
            //m_CtlDevice.WriteSync(this.Id, value, IoType.AO);
            //m_CtlDevice.Write(this.Id, value, IoType.AO);
            m_CtlDevice.Write(m_Id, value, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveAnalogOutput slave = new SlaveAnalogOutput();

            slave.m_Id = this.Id;
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
