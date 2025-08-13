///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : AutoValve Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AutoValve2 : Cylinder_Io
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public AutoValve2()
        {
            this.Name = "__ Valve";
        }
        #endregion

        #region Methods
        public void Open()
        {
            if (IsClose() == true) SetFw();
        }

        public void Close()
        {
            if (IsOpen() == true) SetBw();
        }

        public bool IsOpen()
        {
            return IsFwSensingOnly();
        }

        public bool IsClose()
        {
            return IsBwSensingOnly();
        }

        public bool IsProcess()
        {
            return IsOpen();
        }
        #endregion

        #region Override

        #endregion
    }
}
