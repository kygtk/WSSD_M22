using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Dms.Device;
using Dms.Sequence;
using Dms.Data;

namespace Dms.Server
{
    //public delegate bool GetInterlockCheckConditionDelegate(Gauge gauge);

    public class ThreadGaugeBOE_G8_DHDC : ThreadGauge
    {
        #region Fields
        //protected static IServerManager m_Server = null;
        //protected static _GenericCollection<Gauge> m_Gauges;
        // public GetInterlockCheckConditionDelegate GetInterlockCheckCondition;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Gauge device in m_Gauges)
            {
                RegisterSequence(new SeqGaugeInterlock(this, device));
            }
            RegisterSequence(new SeqAkReverseInterlock(this));
        }
        #endregion

        #region Constructor
        public ThreadGaugeBOE_G8_DHDC(int scanTime, IServerManager server, GetInterlockCheckConditionDelegate checkCondition)
            : base(scanTime, server, checkCondition)
        {
            //m_Server = server; 
            //m_Gauges = m_Server.ComponentContainer.GetCollection<Gauge>();
            //GetInterlockCheckCondition = checkCondition;

            // RegisterSequences();
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

                // 값을 갱신하고
                foreach (Gauge device in m_Gauges)
                {
                    device.UpdateValue();
                }

                //foreach (SeqGaugeInterlock seq in SeqFunctions)
                //{
                //    if (seq.SeqFunName == "A/K Reverse INTR") 
                //        continue;
                //    seq.Do();
                //}
                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Static Methods
        //public static _GenericCollection<Gauge> Units
        //{
        //    get
        //    {
        //        if (m_Gauges == null) m_Gauges = new _GenericCollection<Gauge>();
        //        return m_Gauges;
        //    }
        //}
        #endregion

        #region override Methods

        #endregion

        #region General Methods

        #endregion
    }

    public class SeqGaugeInterlock : XSeqFunction
    {
        #region Fields
        private enum IntrState
        {
            LowAlarm, UpAlarm, LowWarning, UpWarning, Noop
        }

        private Gauge m_Gauge;
        protected static IEqpManager m_EqpManager;
        //protected static IServerManager m_Server;
        protected static ServerManager m_Server; // 09.11.02 minhan
        protected static ThreadGauge m_Control;
        //private double m_RecheckDelayTime = 1000;
        private double m_RecheckDelayTime = 0; // 09.11.02 minhan
        private double m_FirstDelayTime;// 10.12.21 minhan
        private IntrState m_IntrState = IntrState.Noop;
        private bool[] m_IsAlarmSet;
        private string m_Msg = "";
        private string m_Date = ""; // 10.12.21 minhan
        private string m_AlarmData = ""; // 10.12.21 minhan
        private bool m_PcwPressureUpAlarm; // 11.03.20 minhan
        private bool m_PcwFlowUpAlarm; // 11.04.07 minhan
        private bool m_MjPressureLowAlarm; // 11.04.08 minhan
        #endregion

        #region Contructor
        public SeqGaugeInterlock(ThreadGauge control, Gauge gauge)
        {
            m_Gauge = gauge;
            //m_Server = m_Gauge.ServerManager;
            m_Server = ServerManager.Instance; // 09.11.02 minhan
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", m_Gauge.Name);
            m_IsAlarmSet = new bool[(int)IntrState.Noop];
            m_FirstDelayTime = 5000;
            m_PcwPressureUpAlarm = false; // 11.03.20 minhan
            m_PcwFlowUpAlarm = false; // 11.04.07 minhan
            m_MjPressureLowAlarm = false; // 11.04.08 minhan
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;   //TODO:자신의 Init을 보도록 
            // 11.02.01 minhan HPMJ의 컨셉이 초기 게발시 어떻게 가지고 진행 되었지 모르겠지만, 알람을 발생히는 것은 plc에서 하므로, hpmj 관련 내용은 보지 않기로 함. 컨셉변경시 수정하시도록 하세요
            // 단 Set값은 줘야하는데, 의미가 있나..
            int nSeqNo = this.m_SeqNo;
            bool checkCond = true;
            checkCond = m_Control.GetInterlockCheckCondition(m_Gauge);

            if ((m_Gauge.Id == eqpGauges._FR_Unit_HPMJ_CO2_Pressure_Gauge.Id) ||
               (m_Gauge.Id == eqpGauges._FR_Unit_HPMJ_DI_Resistance_Gauge.Id)
               || (m_Gauge.Id == eqpGauges._FR_Unit_HPMJ_Main_DI_Pressure_Gauge.Id)) // 11.05.03 minhan
            {
                checkCond = false;
            }

            //if (m_Gauge.Id == eqpGauges._AP_Unit_PCW_Pressure_Gauge.Id) // 11.03.20 minhan
            //{
            //    if (m_Gauge.IsAlarm)
            //    {
            //        if (m_PcwPressureUpAlarm) GlobalVar.PcwPressureUpAlarm = true;
            //    }
            //    else
            //    {
            //        if (m_PcwPressureUpAlarm) m_PcwPressureUpAlarm = false;
            //        if (GlobalVar.PcwPressureUpAlarm) GlobalVar.PcwPressureUpAlarm = false;
            //    }
            //}
            //else if (m_Gauge.Id == eqpGauges._AP_Unit_PCW_Out_Gauge.Id) // 11.04.07 minhan
            //{
            //    if (m_Gauge.IsAlarm)
            //    {
            //        if (m_PcwFlowUpAlarm) GlobalVar.PcwFlowUpAlarm = true;
            //    }
            //    else
            //    {
            //        if (m_PcwFlowUpAlarm) m_PcwFlowUpAlarm = false;
            //        if (GlobalVar.PcwFlowUpAlarm) GlobalVar.PcwFlowUpAlarm = false;
            //    }
            //}
            else if (m_Gauge.Id == eqpGauges._RB_Unit_MJ_CDA_Pressure_Gauge.Id) // 11.04.08 minhan
            {
                if (m_Gauge.IsAlarm)
                {
                    if (m_MjPressureLowAlarm) GlobalVar.MjPressureLowAlarm = true;
                }
                else
                {
                    if (m_MjPressureLowAlarm) m_MjPressureLowAlarm = false;
                    if (GlobalVar.MjPressureLowAlarm) GlobalVar.MjPressureLowAlarm = false;
                }
            }

            if (m_Gauge.SetupInterlock != null) // 11.02.17 minhan
            {
                m_RecheckDelayTime = m_Gauge.SetupInterlock.DelayTime * 1000; // 11.01.27 minhan
            }
            else
            {
                m_RecheckDelayTime = 0;
            }
            //try
            //{
            //    m_RecheckDelayTime = m_Gauge.SetupInterlock.DelayTime * 1000; // 11.01.27 minhan
            //}
            //catch
            //{
            //    m_RecheckDelayTime = 0;
            //}

            switch (nSeqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (m_IsAlarmSet[(int)IntrState.LowAlarm])
                        {
                            m_IsAlarmSet[(int)IntrState.LowAlarm] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerAlarm.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.UpAlarm])
                        {
                            m_IsAlarmSet[(int)IntrState.UpAlarm] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperAlarm.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.LowWarning])
                        {
                            m_IsAlarmSet[(int)IntrState.LowWarning] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerWarning.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.UpWarning])
                        {
                            m_IsAlarmSet[(int)IntrState.UpWarning] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperWarning.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }

                        //if (m_Gauge.Id != eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge.Id &&
                        //   m_Gauge.Id != eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id &&
                        //   m_Gauge.Id != eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge.Id) // 11.02.01 minhan 
                        //{
                        if (m_Gauge.IsAlarm) // 11.02.09 minhan
                        {
                            m_Gauge.IsAlarm = false;
                        }
                        //}
                    }
                    break;
                //case 5: // 11.04.07 minhan
                //    {
                //        if ((m_Gauge.Id == eqpGauges._AP_Unit_CDA_Pressure_Gauge.Id) ||
                //            (m_Gauge.Id == eqpGauges._AP_Unit_N2_Pressure_Gauge.Id) ||
                //            (m_Gauge.Id == eqpGauges._AP_Unit_PCW_Out_Gauge.Id) ||
                //            (m_Gauge.Id == eqpGauges._AP_Unit_PCW_Pressure_Gauge.Id) ||
                //            (m_Gauge.Id == eqpGauges._AP_Unit_Power_Gauge.Id))
                //        {

                //            nSeqNo = 10;
                //        }
                //        else
                //        {
                //            if (GetElapsedTicks() > m_FirstDelayTime)
                //            {
                //                nSeqNo = 10;
                //            }
                //        }
                //    }
                //    break;
                case 10:
                    if (!checkCond)
                    {
                        nSeqNo = 0;
                    }
                    else
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal = m_Gauge.CurValue;
                        double lowAlarm = 0.0;
                        double upAlarm = 0.0;
                        double lowWarning = 0.0;
                        double upWarning = 0.0;

                        double targetValue = m_Gauge.SetupInterlock.SettingValue; // 11.01.27 minhan
                        if (m_Gauge.InterlockMethod == Gauge.interlockMethod.PC)
                        {
                            lowAlarm = m_Gauge.SetupInterlock.LowAlarm;
                            upAlarm = m_Gauge.SetupInterlock.HighAlarm;
                            lowWarning = m_Gauge.SetupInterlock.LowWarning;
                            upWarning = m_Gauge.SetupInterlock.HighWarning;
                        }
                        else if (m_Gauge.InterlockMethod == Gauge.interlockMethod.PLC)
                        {
                            lowAlarm = targetValue - m_Gauge.SetupInterlock.LowAlarm;
                            upAlarm = targetValue + m_Gauge.SetupInterlock.HighAlarm;
                            lowWarning = targetValue - m_Gauge.SetupInterlock.LowWarning;
                            upWarning = targetValue + m_Gauge.SetupInterlock.HighWarning;
                        }

                        if (curVal < lowAlarm)
                        {
                            m_IntrState = IntrState.LowAlarm;
                        }
                        else if (curVal > upAlarm)
                        {
                            m_IntrState = IntrState.UpAlarm;
                        }
                        else if (curVal < lowWarning)
                        {
                            m_IntrState = IntrState.LowWarning;
                        }
                        else if (curVal > upWarning)
                        {
                            m_IntrState = IntrState.UpWarning;
                        }

                        if (m_IntrState != IntrState.Noop)
                        {
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "ReCheck Process.");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            if (m_Gauge.IsAlarm) // 11.02.09 minhan
                            {
                                m_Gauge.IsAlarm = false;
                            }
                        }
                    }
                    break;
                case 20:
                    if (!checkCond)
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > m_RecheckDelayTime)
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal = m_Gauge.CurValue;
                        double lowAlarm = 0.0;
                        double upAlarm = 0.0;
                        double lowWarning = 0.0;
                        double upWarning = 0.0;

                        double targetValue = m_Gauge.SetupInterlock.SettingValue; // 11.01.27 minhan
                        if (m_Gauge.InterlockMethod == Gauge.interlockMethod.PC)
                        {
                            lowAlarm = m_Gauge.SetupInterlock.LowAlarm;
                            upAlarm = m_Gauge.SetupInterlock.HighAlarm;
                            lowWarning = m_Gauge.SetupInterlock.LowWarning;
                            upWarning = m_Gauge.SetupInterlock.HighWarning;
                        }
                        else if (m_Gauge.InterlockMethod == Gauge.interlockMethod.PLC)
                        {
                            lowAlarm = targetValue - m_Gauge.SetupInterlock.LowAlarm;
                            upAlarm = targetValue + m_Gauge.SetupInterlock.HighAlarm;
                            lowWarning = targetValue - m_Gauge.SetupInterlock.LowWarning;
                            upWarning = targetValue + m_Gauge.SetupInterlock.HighWarning;
                        }

                        if (curVal < lowAlarm)
                        {
                            m_IntrState = IntrState.LowAlarm;
                        }
                        else if (curVal > upAlarm)
                        {
                            m_IntrState = IntrState.UpAlarm;
                        }
                        else if (curVal < lowWarning)
                        {
                            m_IntrState = IntrState.LowWarning;
                        }
                        else if (curVal > upWarning)
                        {
                            m_IntrState = IntrState.UpWarning;
                        }

                        if (m_IntrState == IntrState.LowAlarm)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.LowAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.LowAlarm] = true;

                                if (m_Gauge.Id == eqpGauges._RB_Unit_MJ_CDA_Pressure_Gauge.Id) // 11.04.08 minhan
                                {
                                    m_MjPressureLowAlarm = true;
                                }

                                m_EqpManager.SetAlarm(m_Gauge.ALM_LowerAlarm.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4}, {5})", curVal, targetValue, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);

                                m_Date = DateTime.Now.ToString("yyyyMMddHHmmss"); // 10.12.15 minhan

                                m_AlarmData = string.Format("{0}/{1}/{2:F2}/({3}, {4}, {5}, {6}, {7})", m_Date, m_Gauge.Name, curVal, lowAlarm, lowWarning, targetValue, upWarning, upAlarm);

                                if (GlobalVar.CurGaugeAlarm.Count > 50)
                                {
                                    GlobalVar.CurGaugeAlarm.Clear();
                                    GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                                }
                                else GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                            }
                            if (m_IsAlarmSet[(int)IntrState.LowWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.LowWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerWarning.Id);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.LowAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.LowAlarm] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerAlarm.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Lower Alarm");
                            }
                        }

                        if (m_IntrState == IntrState.UpAlarm)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.UpAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.UpAlarm] = true;

                                //if (m_Gauge.Id == eqpGauges._AP_Unit_PCW_Pressure_Gauge.Id) // 11.03.20 minhan
                                //{
                                //    m_PcwPressureUpAlarm = true;
                                //}
                                //else if (m_Gauge.Id == eqpGauges._AP_Unit_PCW_Out_Gauge.Id) // 11.04.07 minhan
                                //{
                                //    m_PcwFlowUpAlarm = true;
                                //}

                                m_EqpManager.SetAlarm(m_Gauge.ALM_UpperAlarm.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4}, {5})", curVal, targetValue, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);

                                m_Date = DateTime.Now.ToString("yyyyMMddHHmmss"); // 10.12.15 minhan

                                m_AlarmData = string.Format("{0}/{1}/{2:F2}/({3}, {4}, {5}, {6}, {7})", m_Date, m_Gauge.Name, curVal, lowAlarm, lowWarning, targetValue, upWarning, upAlarm);

                                if (GlobalVar.CurGaugeAlarm.Count > 50)
                                {
                                    GlobalVar.CurGaugeAlarm.Clear();
                                    GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                                }
                                else GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                            }
                            if (m_IsAlarmSet[(int)IntrState.UpWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.UpWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperWarning.Id);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.UpAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.UpAlarm] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperAlarm.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Upper Alarm");
                            }
                        }

                        if (m_IntrState == IntrState.LowWarning)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.LowWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.LowWarning] = true;
                                m_EqpManager.SetAlarm(m_Gauge.ALM_LowerWarning.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4}, {5})", curVal, targetValue, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);

                                m_Date = DateTime.Now.ToString("yyyyMMddHHmmss"); // 10.12.15 minhan

                                m_AlarmData = string.Format("{0}/{1}/{2:F2}/({3}, {4}, {5}, {6}, {7})", m_Date, m_Gauge.Name, curVal, lowAlarm, lowWarning, targetValue, upWarning, upAlarm);

                                if (GlobalVar.CurGaugeAlarm.Count > 50)
                                {
                                    GlobalVar.CurGaugeAlarm.Clear();
                                    GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                                }
                                else GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.LowWarning] && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_IsAlarmSet[(int)IntrState.LowWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerWarning.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Lower Warning");
                            }
                        }

                        if (m_IntrState == IntrState.UpWarning)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.UpWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.UpWarning] = true;
                                m_EqpManager.SetAlarm(m_Gauge.ALM_UpperWarning.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4}, {5})", curVal, targetValue, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);

                                m_Date = DateTime.Now.ToString("yyyyMMddHHmmss"); // 10.12.15 minhan

                                m_AlarmData = string.Format("{0}/{1}/{2:F2}/({3}, {4}, {5}, {6}, {7})", m_Date, m_Gauge.Name, curVal, lowAlarm, lowWarning, targetValue, upWarning, upAlarm);

                                if (GlobalVar.CurGaugeAlarm.Count > 50)
                                {
                                    GlobalVar.CurGaugeAlarm.Clear();
                                    GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                                }
                                else GlobalVar.CurGaugeAlarm.Add(m_AlarmData);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.UpWarning] && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_IsAlarmSet[(int)IntrState.UpWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperWarning.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Upper Warning");
                            }
                        }

                        if (!m_IsAlarmSet[(int)IntrState.LowAlarm] && !m_IsAlarmSet[(int)IntrState.UpAlarm])
                        {
                            //if (m_IsAlarm)
                            if (m_Gauge.IsAlarm)
                            {
                                //m_IsAlarm = false;
                                m_Gauge.IsAlarm = false;
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "OK : Recovery");
                            }

                            if (!m_IsAlarmSet[(int)IntrState.LowWarning] && !m_IsAlarmSet[(int)IntrState.UpWarning])
                            {
                                nSeqNo = 10;
                            }
                        }
                        else
                        {
                            if (!m_Gauge.IsAlarm)
                            {
                                m_Gauge.IsAlarm = true;
                            }
                            else
                            {
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "NG : Recovery");
                            }

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Check Recovery");
                        //m_RecheckDelayTime = m_FirstDelayTime; // 09.11.02 minhan
                        //StartTime = DateTime.Now;
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
    public class SeqAkReverseInterlock : XSeqFunction
    {
        #region Fields
        private Gauge m_AkUpFlow;
        private Gauge m_AkLoFlow;
        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server; // 09.11.02 minhan
        protected static ThreadGauge m_Control;
        private Alarm m_AkReverseWaring;
        private double m_RecheckDelayTime = 1000; // 09.11.02 minhan
        private double m_FirstDelayTime = 5000;
        // private bool[] m_IsAlarmSet;
        private string m_Msg = "";
        #endregion

        #region Contructor
        public SeqAkReverseInterlock(ThreadGauge control)
        {
            m_Server = ServerManager.Instance; // 09.11.02 minhan
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_AkUpFlow = eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge;
            m_AkLoFlow = eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge;
            m_AkReverseWaring = new Alarm("A/K Reverse Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_SeqFunName = string.Format("A/K Reverse INTR");
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;   //TODO:자신의 Init을 보도록 

            int nSeqNo = this.m_SeqNo;
            int nDiff = m_Server.SetupAkReverseDiff.GetValue<int>();
            bool checkCond = true;
            checkCond &= m_Control.GetInterlockCheckCondition(m_AkLoFlow);
            checkCond &= m_Control.GetInterlockCheckCondition(m_AkUpFlow);

            switch (nSeqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > m_FirstDelayTime) nSeqNo = 20;
                    break;
                case 20:
                    if (!checkCond) nSeqNo = 0;
                    else if ((m_AkLoFlow.CurValue - m_AkUpFlow.CurValue) > nDiff)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (!checkCond) nSeqNo = 0;
                    else if (GetElapsedTicks() > m_RecheckDelayTime)
                    {
                        m_AlarmId = m_AkReverseWaring.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("Up:{0:F2} Lo:{1:F2})", m_AkUpFlow.CurValue, m_AkLoFlow.CurValue);
                        m_AkUpFlow.SetLog(this.m_SeqFunName, 0, 0, m_Msg);
                        nSeqNo = 1000;
                    }
                    else if ((m_AkLoFlow.CurValue - m_AkUpFlow.CurValue) <= nDiff)
                    {
                        nSeqNo = 20; ;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed ||
                        (m_AkLoFlow.CurValue - m_AkUpFlow.CurValue) <= nDiff)
                    {
                        m_AkUpFlow.SetLog(this.m_SeqFunName, 0, 0, "Check Recovery");
                        m_EqpManager.ResetAlarm(m_AkReverseWaring.Id);
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
        #endregion
    }
}