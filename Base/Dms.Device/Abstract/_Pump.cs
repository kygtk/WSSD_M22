using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    abstract public class _Pump : _DeviceAsm, ICtlPiping
    {
        #region Tag Descriptor
        protected static TagDescriptorPump tagDescriptor = new TagDescriptorPump();
        #endregion

        #region Methods
        abstract public void Run();
        abstract public void Stop();
        abstract public bool IsAlarm();
        abstract public bool IsRun();
        abstract public bool IsStop();
        abstract public bool IsProcess(); 
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        } 
        #endregion
    }
}
