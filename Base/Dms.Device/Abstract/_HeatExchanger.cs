using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    abstract public class _HeatExchanger : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorHeatExchanger tagDescriptor = new TagDescriptorHeatExchanger();
        #endregion

        #region Methods
        abstract public void On();
        abstract public void Off();
        abstract public bool IsAlarm();
        abstract public bool IsOn();
        abstract public bool IsOff();
        abstract public double GetTemp(int channelNo);
        abstract public void SetTemp(int channelNo, int setTemp);
        abstract public void SetOperationMode(OpMode opMode);
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion

    }
}
