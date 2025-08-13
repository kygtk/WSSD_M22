using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    class SeqShutterOpen : XSeqFunction
    {
        public SeqShutterOpen()
        {
        }
        public SeqShutterOpen(ShutterUnit unit)
        {
            Unit = unit;
        }
        private ShutterUnit Unit;

        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            int result = -1;
            int rv = -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (0 == (rv = Unit.ShutterCylinder.SetAct(ActuatorAct.Pos)))
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else if (0 < rv)
                        {
                            result = rv;
                            nSeqNo = 0;
                        }
                    }
                    break;

                case 10:
                    {
                        if (Unit.IsOpen())
                        {
                            result = 0;
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > Unit.ShutterCylinder.MoveTimeout * 1000)
                        {
                            result = 100;
                            nSeqNo = 0;
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }

    }
}
