using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Dms.Common;
using System.Threading;

namespace Dms.Ctl
{
    public class Mmc : IMotionControl
    {
        #region MMC Native DLL Import
        private const string _DllName = "MMCWHP13.dll";
        [DllImport(_DllName)]
        static extern short set_sync_control_ax(short ax, short enable, short master_ax, int gain);
        [DllImport(_DllName)]
        unsafe static extern short mmc_initx(short noOfBoard, long* pAddr);
        [DllImport(_DllName)]
        unsafe static extern short error_message(short code, char* pMsg);
        [DllImport(_DllName)]
        static extern short get_mmc_error();
        [DllImport(_DllName)]
        static extern short set_sync_map_axes(short master, short slave);
        [DllImport(_DllName)]
        static extern short clear_status(short ax);
        [DllImport(_DllName)]
        static extern short axis_done(short ax);
        [DllImport(_DllName)]
        static extern short frames_clear(short ax);
        [DllImport(_DllName)]
        static extern short set_amp_enable(short ax, short enable);
        [DllImport(_DllName)]
        static extern short set_e_stop(short ax);
        [DllImport(_DllName)]
        static extern short axis_state(short ax);
        [DllImport(_DllName)]
        static extern short axis_source(short ax);
        [DllImport(_DllName)]
        static extern short home_switch(short ax);
        [DllImport(_DllName)]
        static extern short in_motion(short ax);
        [DllImport(_DllName)]
        static extern short pos_switch(short ax);
        [DllImport(_DllName)]
        static extern short neg_switch(short ax);
        [DllImport(_DllName)]
        unsafe static extern short get_negative_sw_limit(short ax, double* pLimit, short* pAction);
        [DllImport(_DllName)]
        static extern short set_negative_sw_limit(short ax, double limit, short action);
        [DllImport(_DllName)]
        unsafe static extern short get_positive_sw_limit(short ax, double* pLimit, short* pAction);
        [DllImport(_DllName)]
        static extern short set_positive_sw_limit(short ax, double limit, short action);
        [DllImport(_DllName)]
        static extern short set_home(short ax, short action);
        [DllImport(_DllName)]
        static extern short set_negative_limit(short ax, short action);
        [DllImport(_DllName)]
        static extern short set_positive_limit(short ax, short action);
        [DllImport(_DllName)]
        static extern short set_stop_rate(short ax, short acc);
        [DllImport(_DllName)]
        static extern short set_index_required(short ax, short enable);
        [DllImport(_DllName)]
        static extern short v_move(short ax, double vel, short acc);
        [DllImport(_DllName)]
        static extern short start_r_move(short ax, double distance, double vel, short acc);
        [DllImport(_DllName)]
        static extern short set_position(short ax, double position);
        [DllImport(_DllName)]
        unsafe static extern short start_move_all(short no, short* pAxes, double* pPosPulse, double pVelPulse, short* pAccPulse);
        [DllImport(_DllName)]
        unsafe static extern short map_axes(short no, short* pAxes);
        [DllImport(_DllName)]
        static extern short set_move_speed(double vel);
        [DllImport(_DllName)]
        static extern short set_move_accel(short maxAcc);
        [DllImport(_DllName)]
        static extern short move_4(double posPulse1, double posPulse2, double posPulse3, double posPulse4);
        [DllImport(_DllName)]
        static extern short start_s_move(short ax, double posPulse, double velPulse, short acc);
        [DllImport(_DllName)]
        static extern short smove_4(double posPulse1, double posPulse2, double posPulse3, double posPulse4);
        [DllImport(_DllName)]
        static extern short start_rs_move(short ax, double posPulse, double velPulse, short acc);
        [DllImport(_DllName)]
        static extern short v_move_stop(short ax);
        [DllImport(_DllName)]
        unsafe static extern short get_position(short ax, double* pPos);
        [DllImport(_DllName)]
        unsafe static extern short get_amp_enable(short ax, short* pEnable);
        [DllImport(_DllName)]
        unsafe static extern short get_encoder_direction(short ax, short* pEncorderDir);
        [DllImport(_DllName)]
        unsafe static extern short start_t_move(short ax, double posPulse, double velPulse, short acc, short dec);
        [DllImport(_DllName)]
        unsafe static extern short get_command(short ax, double* pPos);
        [DllImport(_DllName)]
        static extern short set_e_stop_rate(short ax, short rate);
        [DllImport(_DllName)]
        static extern short get_act_velocity(short ax); //2010.12.29 kang :실제 속도 가져오기 위해서 사용
        #endregion

        #region Fields
        private bool m_Simulate = true;
        private bool m_Initialized;
        private int m_NumOfMmc;

        /// Simulation ///
        private int m_MaxAxisNo;
        private double[] m_CurPositionPulse;
        private double[] m_TargetPosPulse;
        private double[] m_VelPulse;
        private bool[] m_StartSMove;
        private bool[] m_IsCmdDone;
        private bool[] m_ServoOn;
        //private System.Windows.Forms.Timer m_Timer = null;
        private System.Threading.Timer m_ThreadingTimer = null; // jemoon : FormTimer에서 ThreadingTimer로 변경
        const int m_TimerInterval = 100;
        //////////////////
        #endregion

        #region Properties
        public bool Simulate
        {
            get { return m_Simulate; }
            set { m_Simulate = value; }
        }
        public bool Initialized
        {
            get { return m_Initialized; }
            set { m_Initialized = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// numOfMmc:MMC보드의 수, maxAxisNum:MMC보드의 모든 축수 
        /// </summary>
        /// <param name="numOfMmc"></param>
        /// <param name="maxAxisNum"></param>
        public Mmc(int numOfMmc, int maxAxisNum)
        {
            AppConfig app = AppConfig.Instance;
            m_Simulate = app.Simul.Motion;

            m_NumOfMmc = numOfMmc;
            m_MaxAxisNo = maxAxisNum;
        }
        #endregion

        #region Methods
        public short Initialize()
        {
            return Initialize(m_NumOfMmc);
        }

        unsafe private short Initialize(int noOfMmc)
        {
            if (m_Initialized)
            {
                return 0;
            }
            else
            {
                if (m_Simulate)
                {
                    MessageBox.Show("System run in MMC Simulation mode!!");


                    m_CurPositionPulse = new double[m_MaxAxisNo];
                    m_TargetPosPulse = new double[m_MaxAxisNo];
                    m_VelPulse = new double[m_MaxAxisNo];
                    m_StartSMove = new bool[m_MaxAxisNo];
                    m_IsCmdDone = new bool[m_MaxAxisNo];
                    m_ServoOn = new bool[m_MaxAxisNo];

                    //m_Timer = new Timer();
                    //m_Timer.Interval = 100;
                    //m_Timer.Tick += new EventHandler(Timer_Tick);
                    //m_Timer.Start();

                    m_ThreadingTimer = new System.Threading.Timer(new TimerCallback(CheckState), null, 0, m_TimerInterval);

                    m_Initialized = true;

                    return 0;
                }
                else
                {
                    short err;
                    long[] addr = new long[noOfMmc];
                    for (int i = 0; i < noOfMmc; i++)
                    {
                        addr[i] = 0xd8000000 + i;   // PCI Type에서는 Address 는 의미없음.
                    }

                    fixed (long* pAddr = addr)
                    {
                        err = mmc_initx((short)noOfMmc, pAddr);
                        if (0 == err)
                        {
                            this.m_Initialized = true;
                        }
                        else
                        {
                            MessageBox.Show("MMC init failed!!!");
                            //Application.Exit();
                        }
                    }

                    return err;
                }
            }
        }

        public void Uninitialize()
        {
            m_Initialized = false;
        }

        private void UpdateSimulationData()
        {
            if (m_Initialized)
            {
                for (int i = 0; i < m_MaxAxisNo; i++)
                {
                    if (m_StartSMove[i])
                    {
                        if (Math.Abs(m_TargetPosPulse[i] - m_CurPositionPulse[i]) < m_VelPulse[i])
                        {
                            m_StartSMove[i] = false;
                            m_CurPositionPulse[i] = m_TargetPosPulse[i];
                            m_IsCmdDone[i] = true;
                        }
                        else
                        {
                            double dir = 1.0;

                            if (m_CurPositionPulse[i] > m_TargetPosPulse[i]) dir = -1.0;

                            m_CurPositionPulse[i] += m_VelPulse[i] * dir;
                        }
                    }
                }
            }
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    UpdateSimulationData();
        //}

        public void CheckState(Object stateInfo)
        {
            UpdateSimulationData();
        }

        #endregion

        #region IMotionConrol
        public short GetError()
        {
            if (m_Simulate) return 0;

            return get_mmc_error();
        }
        public short ServoOn(short ax, bool enable)
        {
            if (m_Simulate)
            {
                m_ServoOn[ax] = enable;

                if (!enable)
                {
                    m_StartSMove[ax] = false;
                }
                return 0;
            }

            short on = (short)((enable) ? 1 : 0);
            return set_amp_enable(ax, on);
        }
        unsafe public short GetServoOnState(short ax, ref short enable)
        {
            fixed (short* result = &enable)
            {
                if (m_Simulate)
                {
                    *result = 1;
                    return 0;
                }
                else
                {
                    return get_amp_enable(ax, result);
                }
            }
        }
        public short ServoEstop(short ax)
        {
            short rValue = 0;
            if (!m_Simulate)
            {
                rValue = set_e_stop(ax);
            }
            else
            {
                m_IsCmdDone[ax] = true;
            }

            return rValue;
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
                return axis_done(ax) != 0;
            }
        }
        public bool SetSyncControl(short masterAx, short slaveAx, bool enable)
        {
            if (m_Simulate) return true;

            set_sync_map_axes(masterAx, slaveAx);
            short nEnable = ((enable) ? (short)1 : (short)0);
            return set_sync_control_ax(slaveAx, nEnable, masterAx, 0) != 0;
        }
        public bool ClearStatus(short ax)
        {
            if (m_Simulate)
            {
                m_IsCmdDone[ax] = true;
                return true;
            }
            else
            {
                return clear_status(ax) != 0;
            }
        }
        public bool ClearFrames(short ax)
        {
            if (m_Simulate)
            {
                m_IsCmdDone[ax] = true;
                return true;
            }
            return frames_clear(ax) != 0;
        }
        public AxisEvent GetAxisState(short ax)
        {
            if (m_Simulate)
            {
                return AxisEvent.NoEvent;
            }
            else
            {
                return (AxisEvent)axis_state(ax);
            }
        }
        public AxisSource GetAxisSource(short ax)
        {
            if (m_Simulate)
            {
                return AxisSource.StNone;
            }
            else
            {
                return (AxisSource)axis_source(ax);
            }
        }
        public short SetPosition(short ax, double position)
        {
            if (m_Simulate)
            {
                m_CurPositionPulse[ax] = position;
                return 0;
            }

            return set_position(ax, position);
        }

        unsafe public short GetNegSwLimitAct(short ax, ref double limit, ref AxisEvent action)
        {
            if (m_Simulate) return 0;

            fixed (double* pLimit = &limit)
            {
                short act;
                short rv = get_negative_sw_limit(ax, pLimit, &act);
                action = (AxisEvent)act;
                return rv;
            }
        }
        public short SetNegSwLimitAct(short ax, double limit, AxisEvent action)
        {
            if (m_Simulate) return 0;

            return set_negative_sw_limit(ax, limit, (short)action);
        }
        unsafe public short GetPosSwLimitAct(short ax, ref double limit, ref AxisEvent action)
        {
            if (m_Simulate) return 0;

            fixed (double* pLimit = &limit)
            {
                short act;
                short rv = get_positive_sw_limit(ax, pLimit, &act);
                action = (AxisEvent)act;
                return rv;
            }
        }
        public short SetPosSwLimitAct(short ax, double limit, AxisEvent action)
        {
            if (m_Simulate) return 0;

            return set_positive_sw_limit(ax, limit, (short)action);
        }
        public short SetHomeAct(short ax, AxisEvent action)
        {
            if (m_Simulate) return 0;

            return set_home(ax, (short)action);
        }
        public short SetNegLimitAct(short ax, AxisEvent action)
        {
            if (m_Simulate) return 0;

            return set_negative_limit(ax, (short)action);
        }
        public short SetPosLimitAct(short ax, AxisEvent action)
        {
            if (m_Simulate) return 0;

            return set_positive_limit(ax, (short)action);
        }
        public short SetStopRate(short ax, short acc)
        {
            if (m_Simulate) return 0;

            return set_stop_rate(ax, acc);
        }
        public short SetIndexRequired(short ax, bool enable)
        {
            if (m_Simulate) return 0;

            short nEnable = ((enable) ? (short)1 : (short)0);
            return set_index_required(ax, nEnable);
        }
        public short StartVmove(short ax, double vel, short acc)
        {
            if (m_Simulate)
            {
                return 0;
            }
            else
            {
                return v_move(ax, vel, acc);
            }
        }
        public short StopVmove(short ax)
        {
            if (m_Simulate) return 0;

            return v_move_stop(ax);
        }
        public short StartRmove(short ax, double distance, double vel, short acc)
        {
            if (m_Simulate) return 0;

            return start_r_move(ax, distance, vel, acc);
        }
        public short StartSmove(short ax, double posPulse, double velPulse, short acc)
        {
            if (m_Simulate)
            {
                m_StartSMove[ax] = true;
                m_TargetPosPulse[ax] = posPulse;
                m_VelPulse[ax] = velPulse / (1000 / m_TimerInterval);

                return 0;
            }

            return start_s_move(ax, posPulse, velPulse, acc);
        }
        public short StartATmove(short ax, double posPulse, double velPulse, short acc, short dec)
        {
            if (m_Simulate) return 0;

            return start_t_move(ax, posPulse, velPulse, acc, dec);
        }
        /// <summary>
        /// return pulse of current position
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="rpos"></param>
        /// <returns>false if success to get current position, otherwise true</returns>
        unsafe public bool GetPosition(short ax, ref double rpos)
        {
            fixed (double* pPos = &rpos)
            {
                if (m_Simulate)
                {
                    *pPos = m_CurPositionPulse[ax];
                    return false;
                }
                else
                {
                    return get_position(ax, pPos) != 0;
                }
            }
        }
        /// <summary>
        /// return pulse of command(target position)
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="rpos"></param>
        /// <returns>false if success to get target position, otherwise true</returns>
        unsafe public bool GetCommand(short ax, ref double rpos)
        {
            fixed (double* pPos = &rpos)
            {
                if (m_Simulate)
                {
                    *pPos = 123456.7;
                    return false;
                }
                else
                {
                    return get_command(ax, pPos) != 0;
                }
            }
        }
        public bool GetHomeSwitch(short ax)
        {
            if (m_Simulate)
            {
                return true;
            }
            else
            {
                return home_switch(ax) != 0;
            }
        }
        public bool GetInMotion(short ax)
        {
            if (m_Simulate)
            {
                return false;
            }
            else
            {
                return in_motion(ax) != 0;
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
                return neg_switch(ax) != 0;
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
                return pos_switch(ax) != 0;
            }
        }
        unsafe public short GetEncoderDir(short ax, ref short dir)
        {
            fixed (short* result = &dir)
            {
                if (m_Simulate)
                {
                    *result = 0;
                    return 0;
                }
                else
                {
                    return get_encoder_direction(ax, result);
                }
            }
        }

        public short SetServoEstopRate(short ax, short rate)
        {
            return set_e_stop_rate(ax, rate);
        }

        public short GetActVelocity(short ax) //2010.12.28 kang
        {
            if (m_Simulate) return 0;

            else return get_act_velocity(ax);
        }
        #endregion
    }
}
