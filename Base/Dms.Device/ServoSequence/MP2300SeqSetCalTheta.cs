using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqSetCalThetaMP2300 : XSeqFunction
    {
        private ServoUnitMp2300 m_Unit;
        private ServoMotorMp2300 m_Motor;
        private Simul m_simul = null;
        private double m_theta = 0.0;

        public SeqSetCalThetaMP2300()
        {
        }

        public double Theta
        {
            get { return m_theta; }
        }

        public SeqSetCalThetaMP2300(ServoUnitMp2300 unit)
        {
            m_Unit = unit;
            m_Motor = (m_Unit.AxisCount != 0) ? m_Unit.Axis[0] : null;
            m_simul = m_Unit.Simul;
            m_SeqFunName = "CalcTheta";
        }

        public override int Do()
        {
            if (m_Motor == null) return 2000;

            int nSeqNo = this.m_SeqNo;
            int nRv = -1;
            int result = -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        nRv = m_Unit.SetCurPosition(0);
                        if (nRv == 0)
                        {
                            m_Unit.SetLog(m_Unit.Name, m_SeqFunName, 0, 0, "Calc Theta : Set Position 0.0");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else if (nRv > 0)
                        {
                            m_ReturnSeqNo = 0;
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 10:
                    if (m_Unit.RbtReset())
                    {
                        m_Unit.SetLog(m_Unit.Name, m_SeqFunName, 0, 0, "Calc Theta : Servo ON");
                        nSeqNo = 20;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_ReturnSeqNo = 10;
                        nSeqNo = 100;
                    }
                    break;
                case 20:
                    {
                        m_Motor.SetJogSpeed(m_Motor.Len2Pulse<int>(1.0));
                        m_Motor.SetJogMinus(true);

                        m_Unit.SetLog(m_Unit.Name, m_SeqFunName, 0, 0, "Calc Theta : Jog Start");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (m_Motor.GetHomeSwitch() && m_Unit.IsRbtSensorOk())
                    {
                        m_Motor.SetJogMinus(false);
                        m_Unit.SetLog(m_Unit.Name, m_SeqFunName, 0, 0, "Calc Theta : Find Home");
                        nSeqNo = 40;
                    }
                    else if (!m_Unit.IsRbtSensorOk() || GetElapsedTicks() > 10000)
                    {
                        m_Motor.SetJogMinus(false);
                        if (!m_Unit.IsRbtSensorOk())
                        {
                            nRv = (int)MP2300Error.errMoveSensor;
                        }
                        else
                        {
                            nRv = (int)MP2300Error.errTimeOver;
                        }
                        m_ReturnSeqNo = 30;
                        nSeqNo = 100;
                    }
                    break;
                case 40:
                    {
                        int pulse = m_Motor.GetRawPosition();
                        uint encoder = m_Motor.AxisEncoder;
                        //AxisEncoder가 2바퀴에 대한 값이기 때문에 360*2 = 720
                        //현재 position을 0으로 setting후 minus로 이동하기 때문에 pulse값은 -일것이다. 그래서 * -1.0
                        double degree = 90 - (720 * (-1.0 * pulse) / encoder);
                        m_theta = (degree / 180.0) * Math.PI;
                        m_Unit.SetLog(m_Unit.Name, m_SeqFunName, 0, 0, string.Format("Calc Theta : Find Home Completed [Pulse:{0}, Theta:{1}]", pulse, m_theta));
                        result = 0;
                        nSeqNo = 0;
                    }
                    break;
                case 100:
                    {
                        m_Unit.SetLog(m_Unit.Name, "CalTheta", 0, 0, string.Format("Find Home : Fail [Step:{0}, Code:{1}", m_ReturnSeqNo, nRv));
                        m_theta = -100;
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
