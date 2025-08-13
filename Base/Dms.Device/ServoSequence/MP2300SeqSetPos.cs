using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqSetPosMP2300 : XSeqFunction
    {
        private ServoMotorMp2300 m_Motor;

        public SeqSetPosMP2300()
        {
        }

        public SeqSetPosMP2300(ServoMotorMp2300 motor)
        {
            m_Motor = motor;
        }

        public int Do(double pos)
        {
            int nRv = -1;
            int result = -1;
            int nSeqNo = this.m_SeqNo;
            uint nTimeover = 5000;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_Motor.ServoOn(false);
                        nSeqNo = 10;
                        m_StartTicks = XFunc.GetTickCount();
                    }
                    break;
                case 10:
                    if (!m_Motor.GetServoOnState())
                    {
                        nSeqNo = 20;
                    }
                    else if (GetElapsedTicks() > nTimeover)
                    {
                        nSeqNo = 0;
                        result = (int)MP2300Error.errServoOn;
                    }
                    break;
                case 20:
                    if ((nRv = m_Motor.ClearAxisErr()) == 0)
                    {
                        int position = m_Motor.Len2Pulse<int>(pos);
                        m_Motor.SetPosition(position);
                        nSeqNo = 30;
                    }
                    else if (nRv > 0)
                    {
                        result = nRv;
                        nSeqNo = 0;
                    }
                    break;
                case 30:
                    {
                        m_Motor.SetPositionRequest(true);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 40;
                    }
                    break;
                case 40:
                    {
                        double getPos = 0.0;
                        if (m_Motor.GetPosition(ref getPos))
                        {
                            if (getPos == pos)
                            {
                                m_Motor.SetPositionRequest(false);
                                nSeqNo = 0;
                                result = 0;
                            }
                            else if (GetElapsedTicks() > 3000)
                            {
                                m_Motor.SetPositionRequest(false);
                                nSeqNo = 0;
                                result = (int)MP2300Error.errSetPosition;
                            }
                        }
                        else
                        {
                            m_Motor.SetPositionRequest(false);
                            nSeqNo = 0;
                            result = (int)MP2300Error.errSetPosition;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
