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
    [Editor(typeof(UIEditorIOSelect), typeof(UITypeEditor))]
    [Serializable()]
    public class IoAnalogInput : _DeviceIo
    {
        #region Fields
        private IoItem m_IoInfo = new IoItem(IoType.AI);
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
        public IoAnalogInput()
        {
            this.Initialized = false;
        }

        public IoAnalogInput(IoItem item, ICtlDevice ctlDevice)
        {
            m_CtlDevice = ctlDevice;

            IoInfo = item;
            this.Id = IoInfo.Id;
            this.Name = IoInfo.Name;
            this.Description = IoInfo.Description;

            Initialize();
        }

        public IoAnalogInput(int id, ICtlDevice ctlDevice)
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
            return m_CtlDevice.Read(m_Id, m_IoInfo.Channel, m_IoInfo.Terminal, m_IoInfo.Node);
        }

        public void SetStateSync(object value)
        {
            m_CtlDevice.Write(m_Id, value, m_IoInfo.Channel, m_IoInfo.Terminal, m_IoInfo.Node);
        }
        #endregion

        #region Override
        public override _DeviceIo Clone()
        {
            IoAnalogInput io = new IoAnalogInput();

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
