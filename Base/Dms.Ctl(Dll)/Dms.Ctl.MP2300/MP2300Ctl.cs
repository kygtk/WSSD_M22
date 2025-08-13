using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Windows.Forms;
using System.Threading;

namespace Dms.Ctl
{
    public class MP2300Ctl : MP2300
    {
        #region Fields
        private bool m_Initialized = false;
        private ThreadMP2300 m_Thread = null;
        private ThreadMP2300Monitor m_MonitorThread = null;
        //For simulation
        private const int m_TimerInterval = 100;
        private bool[] m_IsCmdDone;
        private bool[] m_HomeStart;
        private int[] m_CurPositionPulse;
        private int[] m_TargetPosPulse;
        private int[] m_VelPulse;
        private bool[] m_StartPosMove;
        private bool[] m_ServoOn;
        private System.Threading.Timer m_ThreadingTimer = null; // jemoon : FormTimer에서 ThreadingTimer로 변경
        private bool[] m_IsHomeSwitchDetect;
        #endregion

        #region Singleton code...
        public static readonly MP2300Ctl Instance = new MP2300Ctl();
        #endregion

        #region Constructor
        private MP2300Ctl()
        {
            m_Simulate = AppConfig.Instance.Simul.Motion;
        }
        #endregion

        #region Definition of Input/Output
        private bool mpIN_HEART_BIT()
        {
            return GetRecvBit(0, 0);
        }
        private bool mpIN_SERVO_ON(short ax)
        {
            return GetRecvBit(ax, 1);
        }
        private bool mpIN_SERVO_READY(short ax)
        {
            return GetRecvBit(ax, 2);
        }
        private bool mpIN_WARNING(short ax)
        {
            return GetRecvBit(ax, 3);
        }
        private bool mpIN_ALARM(short ax)
        {
            return GetRecvBit(ax, 4);
        }
        private bool mpIN_LIMIT_POS(short ax)
        {
            return GetRecvBit(ax, 5);
        }
        private bool mpIN_LIMIT_NEG(short ax)
        {
            return GetRecvBit(ax, 6);
        }
        private bool mpIN_SWITCH_DEC(short ax)
        {
            return GetRecvBit(ax, 7);
        }
        private bool mpIN_SWITCH_EXT1(short ax)
        {
            return GetRecvBit(ax, 8);
        }
        private bool mpIN_SWITCH_EXT2(short ax)
        {
            return GetRecvBit(ax, 9);
        }
        private bool mpIN_SWITCH_EXT3(short ax)
        {
            return GetRecvBit(ax, 10);
        }
        private bool mpIN_BRAKE_UNIT(short ax)
        {
            return GetRecvBit(ax, 11);
        }
        private bool mpIN_ORG_BUSY(short ax)
        {
            return GetRecvBit(ax, 12);
        }
        private bool mpIN_ORG_COMP(short ax)
        {
            return GetRecvBit(ax, 13);
        }
        private bool mpIN_ACT_BUSY(short ax)
        {
            return GetRecvBit(ax, 14);
        }
        private bool mpIN_ACT_COMP(short ax)
        {
            return GetRecvBit(ax, 15);
        }
        private bool mpIN_ACT_REJECT(short ax)
        {
            return GetRecvBit(ax, 16);
        }
        private int mpIN_CUR_POSITION(short ax)
        {
            return GetRecvDWord(ax, 150);
        }
        private int mpIN_WARNING_CODE(short ax)
        {
            return GetRecvDWord(ax, 152);
        }
        private int mpIN_ALARM_CODE(short ax)
        {
            return GetRecvDWord(ax, 154);
        }
        private int mpIN_LOAD_RATIO(short ax)
        {
            return GetRecvDWord(ax, 250);
        }
        private int mpOUT_HEART_BIT(bool state)
        {
            return SetSendBit(0, 0, state);
        }
        private int mpOUT_SERVO_ON(short ax, bool state)
        {
            return SetSendBit(ax, 1, state);
        }
        private int mpOUT_ORG_START(short ax, bool state)
        {
            return SetSendBit(ax, 2, state);
        }
        private int mpOUT_ORG_STOP(short ax, bool state)
        {
            return SetSendBit(ax, 3, state);
        }
        private int mpOUT_JOG_POS(short ax, bool state)
        {
            return SetSendBit(ax, 4, state);
        }
        private int mpOUT_JOG_NEG(short ax, bool state)
        {
            return SetSendBit(ax, 5, state);
        }
        private int mpOUT_ACT_START_POINT(short ax, bool state)
        {
            return SetSendBit(ax, 6, state);
        }
        private int mpOUT_ACT_START_REF(short ax, bool state)
        {
            return SetSendBit(ax, 7, state);
        }
        private int mpOUT_ACT_PAUSE(short ax, bool state)
        {
            return SetSendBit(ax, 8, state);
        }
        private int mpOUT_ACT_STOP(short ax, bool state)
        {
            return SetSendBit(ax, 9, state);
        }
        private int mpOUT_ACT_ESTOP(short ax, bool state)
        {
            return SetSendBit(ax, 10, state);
        }
        private int mpOUT_ALARM_RESET(short ax, bool state)
        {
            return SetSendBit(ax, 11, state);
        }
        private int mpOUT_ORG_KEEP(short ax, bool state)
        {
            return SetSendBit(ax, 12, state);
        }
        //20090911 eun 각 축에 대한 비트 Command처리시 쓰레드에 대한 안정성 확보를 위해 비트버퍼를 따로 쓰게됨.
        //실제 주소는 248(249)번이지만 비트버퍼를 사용하기 위해 14(15)로 변경. (13은 Spare)
        //Write할 때 248(249)번지에 쓰도록함.
        private int mpOUT_ORG_COMP_REQ(short ax, bool state)
        {
            //return SetSendBit(ax, 248, state);
            return SetSendBit(ax, 14, state);
        }
        private int mpOUT_SET_POS_REQ(short ax, bool state)
        {
            //return SetSendBit(ax, 249, state);
            return SetSendBit(ax, 15, state);
        }
        private bool mpsOUT_HEART_BIT(short ax)
        {
            return GetSendBit(0, 0);
        }
        private bool mpsOUT_SERVO_ON(short ax)
        {
            return GetSendBit(ax, 1);
        }
        private bool mpsOUT_ORG_START(short ax)
        {
            return GetSendBit(ax, 2);
        }
        private bool mpsOUT_ORG_STOP(short ax)
        {
            return GetSendBit(ax, 3);
        }
        private bool mpsOUT_JOG_POS(short ax)
        {
            return GetSendBit(ax, 4);
        }
        private bool mpsOUT_JOG_NEG(short ax)
        {
            return GetSendBit(ax, 5);
        }
        private bool mpsOUT_ACT_START_POINT(short ax)
        {
            return GetSendBit(ax, 6);
        }
        private bool mpsOUT_ACT_START_REF(short ax)
        {
            return GetSendBit(ax, 7);
        }
        private bool mpsOUT_ACT_PAUSE(short ax)
        {
            return GetSendBit(ax, 8);
        }
        private bool mpsOUT_ACT_STOP(short ax)
        {
            return GetSendBit(ax, 9);
        }
        private bool mpsOUT_ACT_ESTOP(short ax)
        {
            return GetSendBit(ax, 10);
        }
        private bool mpsOUT_ALARM_RESET(short ax)
        {
            return GetSendBit(ax, 11);
        }
        private bool mpsOUT_ORG_KEEP(short ax)
        {
            return GetSendBit(ax, 12);
        }
        private bool mpsOUT_ORG_COMP_REQ(short ax)
        {
            return GetSendBit(ax, 248);
        }
        private bool mpsOUT_SET_POS_REQ(short ax)
        {
            return GetSendBit(ax, 249);
        }
        private int mpOUT_CTRL_TYPE(short ax, int data)
        {
            return SetSendWord(ax, 40, data);
        }
        private int mpOUT_POINT_POS(short ax, int data)
        {
            return SetSendWord(ax, 41, data);
        }
        private int mpOUT_POINT_SPD(short ax, int data)
        {
            return SetSendWord(ax, 42, data);
        }
        private int mpOUT_POINT_ACC(short ax, int data)
        {
            return SetSendWord(ax, 43, data);
        }
        private int mpOUT_POINT_DEC(short ax, int data)
        {
            return SetSendWord(ax, 44, data);
        }
        private int mpOUT_REF_POS(short ax, int data)
        {
            return SetSendDWord(ax, 45, data);
        }
        private int mpOUT_REF_SPD(short ax, int data)
        {
            return SetSendDWord(ax, 47, data);
        }
        private int mpOUT_REF_ACC(short ax, int data)
        {
            return SetSendDWord(ax, 49, data);
        }
        private int mpOUT_JOG_SPEED(short ax, int data)
        {
            return SetSendDWord(ax, 51, data);
        }
        private int mpOUT_SET_POSITION(short ax, int data)
        {
            return SetSendDWord(ax, 250, data);
        }
        #endregion

        #region Methods
        public short Initialize()
        {
            if (m_Initialized)
            {
                return 0;
            }
            else
            {
                short err;
                InitParameter();

                if (m_Simulate)
                {
                    MessageBox.Show("System run in MP2300 Simulation mode!!");

                    m_IsCmdDone = new bool[m_MaxAxisNo];
                    m_HomeStart = new bool[m_MaxAxisNo];
                    m_CurPositionPulse = new int[m_MaxAxisNo];
                    m_TargetPosPulse = new int[m_MaxAxisNo];
                    m_VelPulse = new int[m_MaxAxisNo];
                    m_StartPosMove = new bool[m_MaxAxisNo];
                    m_ServoOn = new bool[m_MaxAxisNo];
                    m_ThreadingTimer = new System.Threading.Timer(new TimerCallback(CheckState), null, 0, m_TimerInterval);
                    m_IsHomeSwitchDetect = new bool[m_MaxAxisNo];

                    m_Connected = true;
                    m_IsLinked = true;

                    m_Initialized = true;

                    return 0;
                }
                else
                {
                    if (1 == InitSocket())
                    {
                        m_Initialized = true;
                        m_Thread = new ThreadMP2300(30, this);
                        m_MonitorThread = new ThreadMP2300Monitor(30, this);
                        m_MonitorThread.Start();
                        m_Thread.Start();
                        err = 0;
                    }
                    else
                    {
                        MessageBox.Show("MP2300 init failed!!!");
                        err = 1;
                    }
                }
                return err;
            }
        }
        public void UnInitialize()
        {
            if (m_Thread != null)
            {
                m_Thread.Pause();
            }
            if (m_MonitorThread != null)
            {
                m_MonitorThread.Pause();
            }
            CloseSocket();
            ClearBit();
            m_Initialized = false;
        }
        public void SetMaxAxis(short maxAxisNo)
        {
            m_MaxAxisNo = maxAxisNo;
        }
        private void CheckState(Object stateInfo)
        {
            UpdateSimulationData();
        }
        private void UpdateSimulationData()
        {
            if (m_Initialized)
            {
                for (int i = 0; i < m_MaxAxisNo; i++)
                {
                    if (m_StartPosMove[i])
                    {
                        if (Math.Abs(m_TargetPosPulse[i] - m_CurPositionPulse[i]) < m_VelPulse[i])
                        {
                            //m_StartPosMove[i] = false;2009.08.28 kimgun 이중이다.SeqRbtMovePointMP2300에서 busy 체크후 false시키니까 여기선 할 필요없다.
                            m_CurPositionPulse[i] = m_TargetPosPulse[i];
                            m_IsCmdDone[i] = true;
                        }
                        else
                        {
                            int dir = 1;

                            if (m_CurPositionPulse[i] > m_TargetPosPulse[i]) dir = -1;

                            m_CurPositionPulse[i] += m_VelPulse[i] * dir;
                        }
                    }
                }
            }
        }

        private void ClearBit()
        {
            mpOUT_HEART_BIT(false);

            for (short i = 0; i < m_MaxAxisNo; i++)
            {
                mpOUT_SERVO_ON(i, false);
                mpOUT_ORG_START(i, false);
                mpOUT_ORG_STOP(i, false);
                mpOUT_JOG_POS(i, false);
                mpOUT_JOG_NEG(i, false);
                mpOUT_ACT_START_POINT(i, false);
                mpOUT_ACT_START_REF(i, false);
                mpOUT_ACT_PAUSE(i, false);
                mpOUT_ACT_STOP(i, false);
                mpOUT_ACT_ESTOP(i, false);
                mpOUT_ALARM_RESET(i, false);
                mpOUT_ORG_COMP_REQ(i, false);
                mpOUT_SET_POS_REQ(i, false);
            }
        }

        public bool ClearBit(short ax, bool servoOff)
        {
            if (servoOff) mpOUT_SERVO_ON(ax, false);
            mpOUT_ORG_START(ax, false);
            mpOUT_ORG_STOP(ax, false);
            mpOUT_JOG_POS(ax, false);
            mpOUT_JOG_NEG(ax, false);
            mpOUT_ACT_START_POINT(ax, false);
            mpOUT_ACT_START_REF(ax, false);
            mpOUT_ACT_STOP(ax, false);
            mpOUT_ACT_ESTOP(ax, false);
            mpOUT_ALARM_RESET(ax, false);
            mpOUT_ORG_COMP_REQ(ax, false);
            mpOUT_SET_POS_REQ(ax, false);

            return true;
        }

        public void SetHeartBit(bool state)
        {
            mpOUT_HEART_BIT(state);
        }
        public bool IsSetHeartBit()
        {
            return mpsOUT_HEART_BIT(0);
        }
        public bool GetHeartBit()
        {
            return mpIN_HEART_BIT();
        }
        public void ServoOn(short ax, bool enable)
        {
            mpOUT_SERVO_ON(ax, enable);
            if (m_Simulate)
            {
                m_ServoOn[ax] = enable;
            }
        }
        public bool GetServoOnState(short ax)
        {
            if (m_Simulate)
            {
                return m_ServoOn[ax];
            }
            else
            {
                return mpIN_SERVO_ON(ax);
            }
        }
        //public bool ClearFrames(short ax)
        //{
        //    mpOUT_ORG_START(ax, false);
        //    mpOUT_ACT_START_POINT(ax, false);
        //    mpOUT_ACT_START_REF(ax, false);
        //    return true;
        //}
        public void SetEstop(short ax, bool state)
        {
            mpOUT_ACT_ESTOP(ax, state);

            if (m_Simulate && state)
            {
                m_IsCmdDone[ax] = true;
                if (m_ServoOn[ax])
                {
                    m_ServoOn[ax] = false;
                }
            }
        }
        public bool IsCmdDone(short ax)
        {
            if (m_Simulate)
            {
                if (m_IsCmdDone[ax])
                {
                    m_IsCmdDone[ax] = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (!mpIN_ACT_COMP(ax) || mpIN_ACT_BUSY(ax)) return false;
            }

            return true;
        }
        public bool ClearStatus(short ax, bool state)
        {
            bool result = true;
            result &= (mpOUT_ACT_STOP(ax, state) == 0);
            result &= (mpOUT_ALARM_RESET(ax, state) == 0);
            return result;
        }
        public AxisEventMP2300 GetAxisState(short ax)
        {
            int nRv = 0;
            int count = Enum.GetNames(typeof(AxisEventMP2300)).Length;    //이렇게 바꿔주어야 하나?
            for (int i = 0; i < count - 1; i++)
            {
                nRv |= (((m_InBuf[1 + i] >> (int)ax) & 0x01) << i);
            }
            return (AxisEventMP2300)nRv;
        }
        public AxisSourceMP2300 GetAxisSource(short ax)
        {
            int nRv = 0;
            int count = Enum.GetNames(typeof(AxisSourceMP2300)).Length;   //이렇게 바꿔주어야 하나?
            for (int i = 0; i < count - 1; i++)
            {
                nRv |= (((m_InBuf[5 + i] >> (int)ax) & 0x01) << i);
            }
            return (AxisSourceMP2300)nRv;
        }
        public int GetPosition(short ax)
        {
            if (m_Simulate)
            {
                return m_CurPositionPulse[ax];
            }
            else
            {
                return mpIN_CUR_POSITION(ax);
            }
        }
        public int GetError(short ax)
        {
            if (mpIN_ALARM(ax))
            {
                int code = (int)mpIN_ALARM_CODE(ax);
                return code;
            }
            else if (mpIN_WARNING(ax))
            {
                int code = (int)mpIN_WARNING_CODE(ax);
                return code;
            }
            return 0;
        }
        public bool GetHomeSwitch(short ax)
        {
            if (m_Simulate)
            {
                //return true;
                return m_IsHomeSwitchDetect[ax];
            }
            else
            {
                return (mpIN_SWITCH_DEC(ax) | mpIN_SWITCH_EXT1(ax));
            }
        }

        /// <summary>
        /// only for simulation
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="state"></param>
        public void SetHomeSwitch(short ax, bool state)
        {
            if (m_Simulate)
            {
                m_IsHomeSwitchDetect[ax] = state;
            }
        }
        public bool GetNegSwitch(short ax)
        {
            if (m_Simulate)
            {
                return false;
            }
            else
            {
                return mpIN_LIMIT_NEG(ax);
            }
        }
        public bool GetPosSwitch(short ax)
        {
            if (m_Simulate)
            {
                return false;
            }
            else
            {
                return mpIN_LIMIT_POS(ax);
            }
        }
        public void SetHomeStart(short ax, bool state)
        {
            mpOUT_ORG_START(ax, state);
            if (state) mpOUT_ORG_KEEP(ax, state);   // ljy_090825 : E-Stop 후 Servo On 상태에서 Org Comp 유지.
            if (m_Simulate)
            {
                m_HomeStart[ax] = state;
                m_IsHomeSwitchDetect[ax] = !state;
            }
        }
        public bool GetHomeBusy(short ax)
        {
            if (m_Simulate)
            {
                if (m_HomeStart[ax])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return mpIN_ORG_BUSY(ax);
            }
        }
        public bool GetHomeComp(short ax)
        {
            if (m_Simulate)
            {
                if (m_HomeStart[ax])
                {
                    m_IsHomeSwitchDetect[ax] = false;
                    return false;
                }
                else
                {
                    m_IsHomeSwitchDetect[ax] = true;
                    return true;
                }
            }
            else
            {
                return mpIN_ORG_COMP(ax);
            }
        }
        public void SetControlType(short ax, ControlType type)
        {
            mpOUT_CTRL_TYPE(ax, (int)type);
        }
        public void SetPoint(short ax, int posNo)
        {
            mpOUT_POINT_POS(ax, posNo);
            mpOUT_POINT_SPD(ax, posNo);
            mpOUT_POINT_ACC(ax, posNo);
            mpOUT_POINT_DEC(ax, posNo);
        }
        public void SetActStartPoint(short ax, bool state)
        {
            mpOUT_ACT_START_POINT(ax, state);
        }
        public bool GetActBusy(short ax)
        {
            if (m_Simulate)
            {
                return m_StartPosMove[ax];
            }
            else
            {
                return mpIN_ACT_BUSY(ax);
            }
        }
        public void SetRefPosition(short ax, int pos)
        {
            mpOUT_REF_POS(ax, pos);

            if (m_Simulate)
            {
                m_TargetPosPulse[ax] = pos;
            }
        }
        public void SetRefSpeed(short ax, int speed)
        {
            mpOUT_REF_SPD(ax, speed);

            if (m_Simulate)
            {
                m_VelPulse[ax] = speed / (1000 / m_TimerInterval);
            }
        }
        public void SetRefAcceleration(short ax, int acc)
        {
            mpOUT_REF_ACC(ax, acc);
        }
        public void SetActStartReference(short ax, bool state)
        {
            mpOUT_ACT_START_REF(ax, state);

            if (m_Simulate && state)
            {
                m_StartPosMove[ax] = true;
            }
        }
        public void SetJogPlus(short ax, bool state)
        {
            mpOUT_JOG_POS(ax, state);
        }
        public void SetJogMinus(short ax, bool state)
        {
            mpOUT_JOG_NEG(ax, state);
        }
        public void SetJogSpeed(short ax, int speed)
        {
            mpOUT_JOG_SPEED(ax, speed);
        }
        public void SetActStop(short ax, bool state)
        {
            mpOUT_ACT_STOP(ax, state);
        }
        public void SetAlarmReset(short ax, bool state)
        {
            mpOUT_ALARM_RESET(ax, state);
        }
        public void SetHomeStop(short ax, bool state)
        {
            mpOUT_ORG_STOP(ax, state);
        }
        public bool GetHomeStop(short ax)
        {
            return mpsOUT_ORG_STOP(ax);
        }
        public void SetPause(short ax, bool state)
        {
            mpOUT_ACT_PAUSE(ax, state);
        }
        public bool GetServoReady(short ax)
        {
            if (m_Simulate)
            {
                return true;
            }
            else
            {
                return mpIN_SERVO_READY(ax);
            }
        }
        public void SetPosition(short ax, int data)
        {
            mpOUT_SET_POSITION(ax, data);
        }
        public void SetPositionRequest(short ax, bool state)
        {
            mpOUT_SET_POS_REQ(ax, state);
        }
        public void SetHomeCompRequest(short ax, bool state)
        {
            mpOUT_ORG_COMP_REQ(ax, state);
            if (state) mpOUT_ORG_KEEP(ax, state);   // ljy_090825 : E-Stop 후 Servo On 상태에서 Org Comp 유지.
        }

        private void SetHomeInfo(short ax, ushort[] buf)
        {
            uint startAddr = (uint)(40000 + ax * 20);
            Write(startAddr, (uint)buf.Length, buf);
        }
        public void SetHomeInfo(ushort[] buf)
        {
            Write((uint)40000, (uint)ORG_NUM, buf);
        }
        public void SetPointInfo(ushort[][] buf)
        {
            Write((uint)50000, (uint)POINT_NUM, buf[0]);
            Write((uint)50000 + (uint)POINT_NUM, (uint)POINT_NUM, buf[1]);
        }
        public ushort[] GetHomeInfo()
        {
            Read((uint)40000, (uint)ORG_NUM, m_OrgBuf);
            return m_OrgBuf;
        }
        public ushort[][] GetPointInfo()
        {
            Read((uint)50000, (uint)POINT_NUM, m_PointBuf[0]);
            Read((uint)50000 + (uint)POINT_NUM, (uint)POINT_NUM, m_PointBuf[1]);
            return m_PointBuf;
        }
        public float GetLoadRatio(short ax)//100617 LeeChungWon
        {
            double ndata = mpIN_LOAD_RATIO(ax);
            float fdata = (float)(ndata / 1000);
            if (fdata < 0) fdata = 0;
            else if (fdata > 100) fdata = 100;
            return fdata;
        }
        #endregion
    }
}
