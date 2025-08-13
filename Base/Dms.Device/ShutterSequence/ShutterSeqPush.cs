using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    class SeqShutterPush : XSeqFunction
    {
        public SeqShutterPush()
        {
        }
        public SeqShutterPush(ShutterUnit unit)
        {
            Unit = unit;
        }
        private ShutterUnit Unit;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        Unit.ShutterPushCylinder.SetAct(ActuatorAct.Pos);

                        StartTicks = XFunc.GetTickCount();
                        
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    {
                        if (Unit.ShutterPushCylinder.IsActStatus(ActuatorAct.Pos))
                        {
                            nSeqNo = 0;
                            result = 0;
                        }
                        else if (Unit.ShutterPushCylinder.MoveTimeout >= GetElapsedTicks())
                        {
                            Unit.ShutterUpCylinder.DiFwSensor.SetState(true);
                            Unit.ShutterUpCylinder.DiBwSensor.SetState(false);
                        }
                    }
                    break;
            }

            this.SeqNo = nSeqNo;

            return result;
        }

    }
}
