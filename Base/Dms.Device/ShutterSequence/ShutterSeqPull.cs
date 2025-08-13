using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    class SeqShutterPull : XSeqFunction
    {
        public SeqShutterPull()
        {
        }
        public SeqShutterPull(ShutterUnit unit)
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
                        Unit.ShutterPushCylinder.SetAct(ActuatorAct.Neg);
                        
                        StartTicks = XFunc.GetTickCount();
                        
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    {
                        if (Unit.ShutterPushCylinder.IsActStatus(ActuatorAct.Neg))
                        {
                            nSeqNo = 0;
                            result = 0;
                        }
                        else if (Unit.ShutterPushCylinder.MoveTimeout >= GetElapsedTicks())
                        {
                            
                        }
                    }
                    break;
            }

            this.SeqNo = nSeqNo;

            return result;
        }

    }
}
