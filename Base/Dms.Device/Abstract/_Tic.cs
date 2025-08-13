using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectConfig), typeof(UITypeEditor))]
    abstract public class _Tic : _DeviceAsm
    {
        #region Methods
        abstract public double GetPvValue(int address);
        abstract public void SetSvValue(int address, int temp);
        abstract public bool IsAlarm(); 
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion

    }
}
