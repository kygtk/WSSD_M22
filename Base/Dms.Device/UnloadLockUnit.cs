using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class UnloadLockUnit : _DeviceAsm
    {
        #region Fields
        private Mfc m_MfcGN2 = new Mfc();
        private ServoUnit m_LoadLockHandUnit;
        private Cylinder m_PinUpdown;
        #endregion

        #region Properties
        public ServoUnit LoadLockHandUnit
        {
            get { return m_LoadLockHandUnit; }
            set { m_LoadLockHandUnit = value; }
        }

        public Cylinder PinUpdown
        {
            get { return m_PinUpdown; }
            set { m_PinUpdown = value; }
        }
        #endregion
        
        // M
        // VS
        // CG

        public UnloadLockUnit()
        {

        }

        public override DmsErrors Initialize()
        {
            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }

        public override void CreateTag(DeviceTags tagContainer)
        {

        }

        public override void UpdateTag()
        {

        }

    }
}
