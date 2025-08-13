using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    abstract public class _Cylinder : _DeviceAsm
    {
        #region Properties
        public override Type FamilyType
        {
            get { return typeof(_Cylinder); }
        }
        #endregion

        #region Methods
        abstract public void SetFw();
        abstract public void SetBw();
        abstract public bool IsFw();
        abstract public bool IsBw(); 
        #endregion
    }
}
