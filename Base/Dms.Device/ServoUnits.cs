///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Collection of CtlServoUnit
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.18 - jemoon : code review - GenericCollecion 상속
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectConfig), typeof(UITypeEditor))]
    public class ServoUnits : _GenericCollection<ServoUnit>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ServoUnits()
        {
        } 
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void SyncInstance(IComponentContainer componentContainer)
        {
            base.SyncInstance(componentContainer);

            foreach (ServoUnit cfg in m_Items)
            {
                if (cfg.Sync)
                {
                    cfg.SyncInfo.Master = cfg.Axis[cfg.SyncInfo.Master.Name];
                    cfg.SyncInfo.Slave = cfg.Axis[cfg.SyncInfo.Slave.Name];

                    if (cfg.SyncInfo.Master == null ||
                        cfg.SyncInfo.Slave == null)
                    { // jemoon : Master & slave 정보가 정확하지 않다면

                        cfg.SyncInfo.Master = null;
                        cfg.SyncInfo.Slave = null;
                        cfg.Sync = false;
                    }
                }
            }
        }
        #endregion
    } 
}
