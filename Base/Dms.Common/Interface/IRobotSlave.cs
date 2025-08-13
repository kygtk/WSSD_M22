using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{

    public delegate void RobotTransferEventHandler(RobotActionType act, int portId, int slotId);

    public interface IRobotSlave
    {
        event RobotTransferEventHandler OnRobotTransferEvent;
    }
}
