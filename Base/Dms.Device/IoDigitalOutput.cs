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
    public class IoDigitalOutput : _DeviceIo
    {
        #region Fields
        private IoItem m_IoInfo = new IoItem(IoType.DO);
        #endregion

        #region Properties
        [XmlIgnore()]
        public IoItem IoInfo
        {
            get { return m_IoInfo; }
            set { m_IoInfo = value; }
        }
        #endregion

        #region Constructor
        public IoDigitalOutput()
        {
            this.Initialized = false;
        }
        public IoDigitalOutput(IoItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            IoInfo = item;
            this.Id = IoInfo.Id;
            this.Name = IoInfo.Name;
            this.Description = IoInfo.Description;
            
            Initialize();
        }
        public IoDigitalOutput(int id, ICtlDevice ctlDevice)
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
            Thread thread = new Thread(delegate() { Pulse(on, msec); });
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
            return m_CtlDevice.Read(m_Id, m_IoInfo.Channel, m_IoInfo.Terminal, m_IoInfo.Node);
        }

        public void SetStateSync(object value)
        {
            //m_CtlDevice.WriteSync(this.Id, value, IoType.DO);
            //m_CtlDevice.Write(this.Id, value, IoType.DO);
            m_CtlDevice.Write(this.Id, value, m_IoInfo.Channel, m_IoInfo.Terminal, m_IoInfo.Node);
        }
        #endregion

        #region Override
        public override _DeviceIo Clone()
        {
            IoDigitalOutput io = new IoDigitalOutput();

            io.Id = this.Id;
            io.Description = this.Description;
            io.Name = m_Name;
            io.m_CtlDevice = m_CtlDevice;
            io.m_Initialized = m_Initialized;

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
