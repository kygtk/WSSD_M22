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
    public class SlaveDigitalOutput : _DeviceSlave
    {
        #region Fields
        private EcSlaveItem_DO m_SlaveInfo = new EcSlaveItem_DO();
        #endregion

        #region Properties
        [XmlIgnore()]
        public EcSlaveItem_DO SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveDigitalOutput()
        {
            this.Initialized = false;
        }
        public SlaveDigitalOutput(EcSlaveItem_DO item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;

            Initialize();
        }
        public SlaveDigitalOutput(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            this.Id = id;
            Initialize();
        }
        #endregion

        #region Methods
        public bool GetState()
        {
            return m_CtlDevice.ReadDoAsync(this.Id);
        }

        public void SetState(bool state)
        {
            m_CtlDevice.WriteDoAsync(this.Id, state);
        }

        public void SetPulse(bool on, int msec)
        {
            Thread thread = new Thread(delegate () { Pulse(on, msec); });
            thread.Start();
        }

        protected void Pulse(bool on, int msec)
        {
            m_CtlDevice.WriteDoSync(this.Id, on);

            if (msec < 100) msec = 100;
            Thread.Sleep(msec);

            m_CtlDevice.WriteDoSync(this.Id, !on);
        }

        public bool[] GetStates(int size)
        {
            bool[] values = new bool[size];

            for (int i = 0; i < size; i++)
            {
                values[i] = m_CtlDevice.ReadDoAsync(m_Id + i);
            }

            return values;

        }
        public void SetStates(bool[] values, int startIndex, int size)
        {
            for (int i = 0; i < size; i++)
            {
                m_CtlDevice.WriteDoSync(m_Id + i, values[startIndex + i]);
            }
        }

        public object GetStateAs()
        {
            return m_CtlDevice.Read(m_Id, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }

        public void SetStateSync(object value)
        {
            //m_CtlDevice.WriteSync(this.Id, value, IoType.DO);
            //m_CtlDevice.Write(this.Id, value, IoType.DO);
            m_CtlDevice.Write(this.Id, value, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }
        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveDigitalOutput slave = new SlaveDigitalOutput();

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
