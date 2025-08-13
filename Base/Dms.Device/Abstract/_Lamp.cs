using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    abstract public class _Lamp : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorLamp tagDescriptor = new TagDescriptorLamp();
        #endregion

        #region Methods
        abstract public void On();
        abstract public void Off();
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
