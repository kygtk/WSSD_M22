using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public abstract class _ServoMotor : _DeviceAsm
    {
        #region Abstracts
        public abstract short AxisAcc { get; set; }
        public abstract short AxisDec { get; set; }
        public abstract short AxisId { get; set; }
        public abstract double AxisRatio { get; set; }
        public abstract double AxisVel { get; set; }
        public abstract double Len2Pulse(double len);
        public abstract double Pulse2Len(double pulse);
        public abstract void ServoOn(bool on);
        public abstract int StartVelMove(double velPulse);
        public abstract int StopVelMove();
        public abstract bool GetServoOnState();

        public abstract bool GetHomeSwitch();
        #endregion

        #region _DeviceAsm
        public override Type FamilyType
        {
            get { return typeof(_ServoMotor); }
        }
        #endregion
    }
}
