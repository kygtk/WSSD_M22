using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Util.IODefine;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Serializable()]
    abstract public class _DeviceIo : _Device
    {
        #region Fields 
        protected ICtlDevice m_CtlDevice = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public ICtlDevice CtlDevice
        {
            get { return m_CtlDevice; }
        }
        #endregion

        #region Methods
        abstract public IoItem GetIoInfo();
        abstract public string GetIoStateString();
        abstract public void UpdateState();
        abstract public void UpdateState(IoStateEventArgs e);
        abstract public _DeviceIo Clone();

        public virtual void HandleEvent(object sender, IoStateEventArgs e)
        {
            //if (this.Initialized == false) return;

            //IoItem ioInfo = GetIoInfo();
            //if (e.Type == ioInfo.IoType && e.Id == ioInfo.Id)
            //{
            //    ioInfo.State = GetIoStateString();
            //}

            UpdateState(e);
        }

        public virtual void SetSubscriber()
        {
            //m_CtlDevice.OnIoStateChange += new IoStateChangeEventHandler(HandleEvent);
        }
        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            SetSubscriber();

            //Io현재상태값 갱신
            //IoItem ioInfo = GetIoInfo();
            //ioInfo.State = GetIoStateString();

            //jemoon : 100106 - 이전에는 초기값을 위해 필요했지만 지금은 필요없음
            //UpdateState();

            this.Initialized = true;
            return DmsErrors.Success;
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        }
        #endregion
    }
}
