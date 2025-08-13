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
    [Editor(typeof(UIEditorIOSelect), typeof(UITypeEditor))]
    [Serializable()]
    public class IoDigitalInput : _DeviceIo
    {
        #region Fields
        private ActiveType m_ActiveType = ActiveType.A;
        private IoItemDI m_IoInfo = new IoItemDI(IoType.DI);
        #endregion

        #region Properties
        [XmlIgnore(), ReadOnly(true)]
        public ActiveType ActiveType
        {
            get { return m_ActiveType; }
            set { m_ActiveType = value; }
        }
        [XmlIgnore()]
        public IoItemDI IoInfo
        {
            get { return m_IoInfo; }
            set { m_IoInfo = value; }
        }
        #endregion

        #region Constructor
        public IoDigitalInput()
        {
            this.Initialized = false;
        }
        public IoDigitalInput(IoItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            
            IoInfo = item as IoItemDI;
            this.Id = IoInfo.Id;
            this.Name = IoInfo.Name;
            this.Description = IoInfo.Description;
            m_ActiveType = IoInfo.ActiveType;

            Initialize();
        }
        public IoDigitalInput(int id, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;
            m_IoInfo = new IoItemDI();
            this.Id = id;
            Initialize();
        }
        #endregion

        #region Methods
        public bool GetState()
        {
            bool state = m_CtlDevice.ReadDiAsync(this.Id);
            if (m_IoInfo.ActiveType == ActiveType.B)
            {
                state = !state;
            }

            return state;
        }

        public void SetState(bool state)
        {
            if (m_IoInfo.ActiveType == ActiveType.B)
            {
                state = !state;
            }

            m_CtlDevice.WriteDiSync(this.Id, state);
        }

        public void SetPulse(bool state, int msec)
        {
            Thread thread = new Thread(delegate() { Pulse(state, msec); });
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
            return m_CtlDevice.Read(this.Id, m_IoInfo.Channel, m_IoInfo.Terminal, m_IoInfo.Node);
        }

        public void SetStateSync(object value)
        {
            m_CtlDevice.Write(m_Id, value, m_IoInfo.Channel, m_IoInfo.Terminal, m_IoInfo.Node);
        }

        #endregion

        #region Override
        public override _DeviceIo Clone()
        {
            IoDigitalInput io = new IoDigitalInput();

            io.Id = m_Id;
            io.Description = this.Description;
            io.Name = m_Name;
            io.m_CtlDevice = m_CtlDevice;
            io.m_Initialized = m_Initialized;
            io.ActiveType = m_ActiveType;

            io.IoInfo = m_IoInfo.Clone();

            return io;
        }

        public override IoItem GetIoInfo()
        {
            return m_IoInfo as IoItem;
        }

        public override string GetIoStateString()
        {
            return GetState().ToString();
        }

        public override void UpdateState()
        {
            m_IoInfo.State = GetState().ToString();
        }

        public override void UpdateState(IoStateEventArgs e)
        {
            if (this.Initialized == false) return;

            if (e.Type == m_IoInfo.IoType && e.Id == m_IoInfo.Id)
            {
                UpdateState();
            }
        }
        #endregion
    }
}
