using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dms.Common;
using Dms.Util.IODefine;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Serializable()]
    abstract public class _DeviceSlave : _Device
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
        abstract public EcSlaveItem GetSlaveInfo();
        abstract public string GetSlaveStateString();
        abstract public void UpdateState();
        abstract public void UpdateState(SlaveStateEventArgs e);
        abstract public _DeviceSlave Clone();

        public virtual void HandleEvent(object sender, SlaveStateEventArgs e)
        {
            UpdateState(e);
        }

        public virtual void SetSubscriber()
        {

        }
        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            SetSubscriber();

            this.m_Initialized = true;
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
