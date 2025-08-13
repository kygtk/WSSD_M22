using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqSetHomeCompMP2300 : XSeqFunction
    {
        private ServoMotorMp2300 m_Motor;

        public SeqSetHomeCompMP2300()
        { }

        public SeqSetHomeCompMP2300(ServoMotorMp2300 motor)
        {
            m_Motor = motor;
        }

        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            int result = -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Motor.GetHomeComp() == true)
                        {
                            result = 0;
                            break;
                        }

                        m_Motor.SetHomeCompRequest(true);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Motor.GetHomeComp())
                    {
                        m_Motor.SetHomeCompRequest(false);
                        nSeqNo = 0;
                        result = 0;
                    }
                    else if (GetElapsedTicks() > 3000)
                    {
                        m_Motor.SetHomeCompRequest(false);
                        nSeqNo = 0;
                        result = 1;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
