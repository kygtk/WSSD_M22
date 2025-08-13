using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqHomeMP2300 : XSeqFunction
    {
        private ServoMotorMp2300 m_ServoMotor;

        public SeqHomeMP2300()
        {
        }

        public SeqHomeMP2300(ServoMotorMp2300 servo)
        {
            m_ServoMotor = servo;
        }

        public override int Do()
        {
            int result = -1;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (true == m_ServoMotor.GetHomeStop()) return (int)MP2300Error.errHomeStop;
                    else
                    {
                        m_ServoMotor.SetHomeStart(true);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if ((true == m_ServoMotor.GetHomeBusy()) &&
                        (!m_ServoMotor.GetHomeComp()))
                    {
                        m_ServoMotor.SetHomeStart(false);
                        nSeqNo = 20;
                    }
                    else if (GetElapsedTicks() > 3000)
                    {
                        int nRv = -1;
                        if ((nRv = m_ServoMotor.GetAxisErr()) > 0) result = nRv;
                        else if (true == m_ServoMotor.GetHomeStop()) result = (int)MP2300Error.errHomeStop;
                        else if (!m_ServoMotor.GetServoOnState()) result = (int)MP2300Error.errServoOn;
                        else result = (int)MP2300Error.errTimeOver;
                        if (result > 0)
                        {
                            m_ServoMotor.SetHomeStart(false);
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 20:
                    if (true == m_ServoMotor.GetHomeComp())
                    {
                        //Servo.SetPosition(0.0);
                        m_ServoMotor.HomeComp = true;
                        result = 0;
                        nSeqNo = 0;
                    }
                    else
                    {
                        int nRv = -1;
                        if ((nRv = m_ServoMotor.GetAxisErr()) > 0) result = nRv;
                        else if (true == m_ServoMotor.GetHomeStop()) result = (int)MP2300Error.errHomeStop;
                        else if (!m_ServoMotor.GetServoOnState()) result = (int)MP2300Error.errServoOn;
                        if (result > 0) nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
