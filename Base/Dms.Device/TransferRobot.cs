using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Dms.Common;


namespace Dms.Device
{
    abstract public class TransferRobot : TransferUnit
    {
        #region Fields
        protected _ServoUnit m_Servo = null;
        #endregion

        #region Properties
        public abstract _ServoUnit Servo { get; set; }
        #endregion

        #region Constructor
        public TransferRobot()
        {
            this.Name = "__ Unit";
        }
        #endregion

        #region _DeviceAsm override
        public override void CreateTag(DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        }

        public override DmsErrors Initialize()
        {
            return DmsErrors.Success;
        }
        #endregion
    }
}
