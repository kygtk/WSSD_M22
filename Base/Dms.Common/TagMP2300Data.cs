using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    [Flags]
    public enum AxisEventMP2300
    {
        NoEvent = 0x0000,
        ServoOn = 0x0001,
        Ready = 0x0002,
        Warning = 0x0004,
        Alarm = 0x0008
    }

    [Flags]
    public enum AxisSourceMP2300
    {
        None = 0x0000,
        PosLimit = 0x0001,
        NegLimit = 0x0002,
        HomeSwitch = 0x0004,
        Ext1Switch = 0x0008,
        Ext2Switch = 0x0010,
        Ext3Switch = 0x0020,
        BrakeUnit = 0x0040,
        HomeBusy = 0x0080,
        HomeComp = 0x0100,
        ActBusy = 0x0200,
        ActComp = 0x0400,
        ActReject = 0x0800
    }

    public enum ControlType
    {
        CtrlNone,
        CtrlPosition,
        CtrlSpeed,
        CtrlTorque
    }
}
