using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqClearAxisErrMP2300 : XSeqFunction
    {
        private ServoMotorMp2300 m_ServoMotor;

        public SeqClearAxisErrMP2300()
        {
        }

        public SeqClearAxisErrMP2300(ServoMotorMp2300 servo)
        {
            m_ServoMotor = servo;
        }

        public override int Do()
        {
            int nRv = -1;
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_ServoMotor.ClearBit(false))
                    {
                        m_ServoMotor.ClearStatus(true);

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if ((nRv = m_ServoMotor.GetAxisErr()) == 0)
                    {
                        m_ServoMotor.ClearStatus(false);
                        result = 0;
                        nSeqNo = 0;
                    }
                    else if ((GetElapsedTicks() > 1000) &&
                             (nRv > 0))
                    {
                        m_ServoMotor.ClearStatus(false);
                        result = nRv;
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
