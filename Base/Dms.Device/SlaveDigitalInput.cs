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
    public class SlaveDigitalInput : _DeviceSlave
    {
        #region Fields
        private ActiveType m_ActiveType = ActiveType.A;
        private EcSlaveItem_DI m_SlaveInfo = new EcSlaveItem_DI();
        #endregion

        #region Properties
        [XmlIgnore(), ReadOnly(true)]
        public ActiveType ActiveType
        {
            get { return m_ActiveType; }
            set { m_ActiveType = value; }
        }
        [XmlIgnore()]
        public EcSlaveItem_DI SlaveInfo
        {
            get { return m_SlaveInfo; }
            set { m_SlaveInfo = value; }
        }
        #endregion

        #region Constructor
        public SlaveDigitalInput()
        {
            this.Initialized = false;
        }

        public SlaveDigitalInput(EcSlaveItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            m_SlaveInfo = item as EcSlaveItem_DI;
            this.Id = m_SlaveInfo.Id;
            this.Name = m_SlaveInfo.Name;
            this.Description = m_SlaveInfo.Description;
            m_ActiveType = m_SlaveInfo.ActiveType;

            Initialize();
        }

        public SlaveDigitalInput(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            m_SlaveInfo = new EcSlaveItem_DI();
            this.Id = id;
            Initialize();
        }
        #endregion

        #region Methods
        public bool GetState()
        {
            bool state = m_CtlDevice.ReadDiAsync(this.Id);
            if (m_SlaveInfo.ActiveType == ActiveType.B)
            {
                state = !state;
            }

            return state;
        }

        public void SetState(bool state)
        {
            if (m_SlaveInfo.ActiveType == ActiveType.B)
            {
                state = !state;
            }

            m_CtlDevice.WriteDiSync(this.Id, state);
        }

        public void SetPulse(bool state, int msec)
        {
            Thread thread = new Thread(delegate () { Pulse(state, msec); });
            thread.Start();
        }

        protected void Pulse(bool state, int msec)
        {
            m_CtlDevice.WriteDiSync(this.Id, state);

            if (msec < 100) msec = 100;
            Thread.Sleep(msec);

            m_CtlDevice.WriteDiSync(this.Id, !state);
        }

        public bool[] GetStates(int size)
        {
            bool[] values = new bool[size];

            for (int i = 0; i < size; i++)
            {
                values[i] = m_CtlDevice.ReadDiAsync(m_Id + i);
            }

            return values;
        }

        public void SetStates(bool[] values, int startIndex, int size)
        {
            for (int i = 0; i < size; i++)
            {
                m_CtlDevice.WriteDiSync(m_Id + i, values[startIndex + i]);
            }
        }

        public object GetStateAs()
        {
            return m_CtlDevice.Read(this.Id, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }

        public void SetStateSync(object value)
        {
            m_CtlDevice.Write(m_Id, value, m_SlaveInfo.Channel, m_SlaveInfo.SlaveNo, m_SlaveInfo.AliasNo);
        }

        #endregion

        #region Override
        public override _DeviceSlave Clone()
        {
            SlaveDigitalInput slave = new SlaveDigitalInput();

            slave.Id = m_Id;
            slave.Description = this.Description;
            slave.Name = m_Name;
            slave.m_CtlDevice = m_CtlDevice;
            slave.m_Initialized = m_Initialized;
            slave.ActiveType = m_ActiveType;

            slave.m_SlaveInfo = m_SlaveInfo.Clone();

            return slave;
        }

        public override EcSlaveItem GetSlaveInfo()
        {
            return m_SlaveInfo as EcSlaveItem;
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
