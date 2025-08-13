using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    abstract public class _Sensor : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorSensor tagDescriptor = new TagDescriptorSensor();
        #endregion

        #region Methods
        abstract public bool IsDetected(); 
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(_Sensor); }
        }
        #endregion
    }
}
