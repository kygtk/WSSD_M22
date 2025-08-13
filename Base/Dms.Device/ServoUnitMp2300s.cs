using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectConfig), typeof(UITypeEditor))]
    public class ServoUnitMp2300s : _GenericCollection<ServoUnitMp2300>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ServoUnitMp2300s()
        {
        } 
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void SyncInstance(IComponentContainer componentContainer)
        {
            base.SyncInstance(componentContainer);

            //foreach (ServoUnitMp2300 cfg in m_Items)
            //{
            //    if (cfg.Sync)
            //    {
            //        cfg.SyncInfo.Master = cfg.Axis[cfg.SyncInfo.Master.Name];
            //        cfg.SyncInfo.Slave = cfg.Axis[cfg.SyncInfo.Slave.Name];

            //        if (cfg.SyncInfo.Master == null ||
            //            cfg.SyncInfo.Slave == null)
            //        { // jemoon : Master & slave 정보가 정확하지 않다면

            //            cfg.SyncInfo.Master = null;
            //            cfg.SyncInfo.Slave = null;
            //            cfg.Sync = false;
            //        }
            //    }
            //}
        }
        #endregion
    }
}
