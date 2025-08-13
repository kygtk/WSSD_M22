using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    abstract public class _Heater : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorHeater tagDescriptor = new TagDescriptorHeater();
        #endregion

        #region Methods
        abstract public void On();
        abstract public void Off();
        abstract public bool IsAlarm();
        abstract public bool IsOn();
        abstract public bool IsOff(); 
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion

    }
}
