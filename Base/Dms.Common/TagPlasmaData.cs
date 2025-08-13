using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum SeApAlarmIndex
    {
        plasmaAlmInterlockErr = 0,
        plasmaAlmInverterErr = 1,
        plasmaAlmLowVoltageErr = 2,
        plasmaAlmHighVoltageErr = 3,
        plasmaAlmArcFault = 4,
        plasmaAlmLocalModeRunErr = 5,
        plasmaAlmOnFaultErr = 6,
        plasmaNoAlm = 7,
    }

    public enum ApAlarmStatus
    {
        none,
        status0 = 0x01,// 1 << 0, // 1,
        status1 = 0x02,// 1 << 1, // 2
        status2 = 0x04, //1 << 2, // 4
        normal = 0x07,
    }

    public enum PSMApAlarmIndex
    {
        errIntrOpen = 0, 
        errNotDefined, 
        errLowVoltage, 
        errSystem, 
        errOutput, 
        errLocal, 
        errARC, 
        errNone
    }
}
