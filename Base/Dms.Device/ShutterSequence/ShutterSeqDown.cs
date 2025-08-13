using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    class SeqShutterDown : XSeqFunction
    {
        public SeqShutterDown()
        {
        }
        public SeqShutterDown(ShutterUnit unit)
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
                        Unit.ShutterUpCylinder.SetAct(ActuatorAct.Neg);
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    {
                        if (Unit.ShutterUpCylinder.IsActStatus(ActuatorAct.Neg))
                        {
                            nSeqNo = 0;
                            result = 0;
                        }
                        else if (Unit.ShutterUpCylinder.MoveTimeout >= GetElapsedTicks())
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
