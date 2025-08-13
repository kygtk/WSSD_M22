using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum eqpSolarStatus
    {
        eqpSolarNone = 0,
        eqpSolarNonScheduled = 1,
        eqpSolarUnSheduledDowntimeState = 2,
        eqpSolarScheduledDowntimeState = 3,
        eqpSolarEngineeringState = 4,
        eqpSolarStandbyState = 5,
        eqpSolarProductiveState = 6,
    }

    public enum eqpSolarUnitType
    {
        Normal_Unit,
        Process_Unit,
        Transfer_Unit,
        Loading_Unit,
        Unloading_Unit,
    }
}
