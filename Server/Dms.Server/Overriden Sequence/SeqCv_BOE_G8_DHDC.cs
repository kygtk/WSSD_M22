using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadCvBOE_G8_DHDC : ThreadCvControl
    {
        #region enum
        public enum CheckCond // 11.03.07 minhan
        {
            CVDriverCond, GaugeCond, HpmjCond, InIonizerCond, EuvIonizerCond, UlIonizerCond, ApCond, PumpCond, TankCond, AkHepacond, RbCond, FfuCond, CheckCondCnt,
        };
        #endregion

        #region Fields
        private _GenericCollection<RbUnit> m_RbUnits;
        protected static _GenericCollection<TransferUnit> m_TransferUnits;
        public string[][] CheckUnitCond;
        private CheckUnitCondForm m_CheckCondView;
        public Alarm AlarmApdReportError; // 11.02.01 minhan
        public EventMessageQueue m_MsgQueue; // 11.05.17 minhan
        public Alarm AlarmLdRobotInterlock = new Alarm("LD Unit Robot Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.06.17 minhan
        public Alarm AlarmUlRobotInterlock = new Alarm("UL Unit Robot Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.06.17 minhan
        #endregion

        #region Constructor
        public ThreadCvBOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {
            m_RbUnits = DmsComponents.Instance.ComponentContainer.GetCollection<RbUnit>();
            m_TransferUnits = DmsComponents.Instance.ComponentContainer.GetCollection<TransferUnit>();
            m_MsgQueue = EventMessageQueue.Instance; // 11.05.17 minhan
            CheckUnitCond = new string[m_TransferUnits.Count][];

            foreach (TransferUnit device in m_TransferUnits)
            {
                CheckUnitCond[device.Id] = new string[(int)CheckCond.CheckCondCnt];
            }
            m_CheckCondView = new CheckUnitCondForm();
            m_CheckCondView.Initialize(this);
            m_CheckCondView.Show();
            AlarmApdReportError = new Alarm("Apd Item Set Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.02.01 minhan
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqCvManualControlInterlock(this, m_CvUnits));

            foreach (CvUnit device in m_CvUnits)
            {
                if (device.Name == eqpTransferUnits._LD_CvUnit_Name)
                {
                    RegisterSequence(new SeqUnitLdCv1(this, device));
                    RegisterSequence(new SeqDualOutSensor(this, device)); // 11.02.01 minhan
                }
                else if (device.Name == eqpTransferUnits._UL_CvUnit_Name)
                {
                    RegisterSequence(new SeqUnitUlCv1(this, device));
                    RegisterSequence(new SeqUnitUlCv2(this, device));
                }
                else
                {
                    RegisterSequence(new SeqDualInSensor(this, device));
                    RegisterSequence(new SeqDualOutSensor(this, device));
                }

                RegisterSequence(new SeqCvMotor(this, device));
                RegisterSequence(new SeqCvMotorCondition(this, device));
                RegisterSequence(new SeqCvMotorSpeedControl(this, device));
            }

            foreach (ActuatorUnit device in m_ActuatorUnits)
            {
                if (device.Name == eqpActuatorUnits._LD_Align_Name ||
                    device.Name == eqpActuatorUnits._LD_Idle_Roller_Name ||
                    device.Name == eqpActuatorUnits._LD_Tilting_Unit_Name)
                {
                    RegisterSequence(new SeqUnitActuatorInterlock(this, device));
                }
            }
            RegisterSequence(new SeqAkManualInterlockCheck(this, m_CvUnits));

            m_Server.AddSeqInitFunction(new SeqInitCv(this, m_Server));
        }
        #endregion

        #region Override
        public override bool CheckUnitCondition(CvUnit cv)
        {
            bool ok = true;
            bool check = true;
            string CheckName = string.Format("{0} CheckUnitCondition", cv.Name);
            check = GetCvCond(cv);

            if (!check && CheckUnitCond[cv.Id][(int)CheckCond.CVDriverCond] == null)
            {//2010.09.23 kimgun
                CheckUnitCond[cv.Id][(int)CheckCond.CVDriverCond] = string.Format("CP Off or Driver Alarm");
                cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.CVDriverCond]);
            }
            else if (check)
            {
                CheckUnitCond[cv.Id][(int)CheckCond.CVDriverCond] = null;
            }
            ok &= check;
            check = true;
            //ok &= m_Server.JobCond.GetGaugeCond(cv); // 09.12.08 minhan

            if (cv.Id == eqpTransferUnits._LD_CvUnit.Id)
            {
                foreach (CvUnit unit in ThreadCvControl.Units) // 09.12.08 minhan LD 구간에서는 모든 게이지 알람을 체크한다.
                {
                    check &= m_Server.JobCond.GetGaugeCond(unit);
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.GaugeCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.GaugeCond] = string.Format("Gauge Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.GaugeCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.GaugeCond] = null;
                }

                ok &= check;
                check = true;

                // HPMJ
                if (m_Server.JobCond.HpmjUse(eqpHpmjs._HPMJ_Unit) &&
                    //eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>() &&
                    m_Server.JobCond.ProcessMode) // 11.02.01 minhan
                {
                    check &= eqpHpmjs._HPMJ_Unit.IfFlag.Ready;
                    check &= !eqpHpmjs._HPMJ_Unit.IfFlag.PIDError;
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] = string.Format("Hpmj Not Ready");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] = null;
                }

                ok &= check;
                check = true;
                // IN ionizer zhangliang
                if (eqpIonizers._IN_Unit_Ionizer.SetupIonizerInterlock.Use &&
                   m_Server.JobCond.ProcessMode)
                {
                    check &= !eqpIonizers._IN_Unit_Ionizer.IsAlarm;
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.InIonizerCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.InIonizerCond] = string.Format("IN Ionizer Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.InIonizerCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.InIonizerCond] = null;
                }
                ok &= check;
                check = true;

                // EUV ionizer
                if (eqpIonizers._EUV_Unit_Ionizer.SetupIonizerInterlock.Use &&
                   m_Server.JobCond.ProcessMode)
                {
                    check &= !eqpIonizers._EUV_Unit_Ionizer.IsAlarm;
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.EuvIonizerCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.EuvIonizerCond] = string.Format("EUV Ionizer Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.EuvIonizerCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.EuvIonizerCond] = null;
                }
                ok &= check;
                check = true;

                // AP PLASMA
                //if (m_Server.JobCond.ApUse(eqpSeAps._SE_AP_Plasma_Unit))
                //{
                //    check &= (m_Server as ServerManager).ThreadHandler.ApControl.GetApCond(eqpSeAps._SE_AP_Plasma_Unit);
                //}
                //if (!check && CheckUnitCond[cv.Id][(int)CheckCond.ApCond] == null)
                //{//2010.09.23 kimgun
                //    CheckUnitCond[cv.Id][(int)CheckCond.ApCond] = string.Format("Ap Not Ready");
                //    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.ApCond]);
                //}
                //else if (check)
                //{
                //    CheckUnitCond[cv.Id][(int)CheckCond.ApCond] = null;
                //}

                //ok &= check;
                //check = true;

                // Ul Ionizer  zhangliang
                if (eqpIonizers._UL_Unit_Ionizer1.SetupIonizerInterlock.Use &&
                  m_Server.JobCond.ProcessMode)
                {
                    check &= !eqpIonizers._UL_Unit_Ionizer1.IsAlarm;
                }
                if (eqpIonizers._UL_Unit_Ionizer2.SetupIonizerInterlock.Use &&
                  m_Server.JobCond.ProcessMode)
                {
                    check &= !eqpIonizers._UL_Unit_Ionizer2.IsAlarm;
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.UlIonizerCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.UlIonizerCond] = string.Format("UL Ionizer Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.UlIonizerCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.UlIonizerCond] = null;
                }
                ok &= check;
                check = true;

                // tank
                if ((m_Server.JobCond.ProcessMode) && eqpPumpUnits._RB_SHW_Pump_Unit.Pump.IsUse) // 11.02.01 minhan 탱크가 중요한게 아니라 펌프겠지.
                {
                    check &= !eqpTankUnits._FR_Unit_Tank.IfFlag.TankLevelFault;
                    check &= !GlobalVar.HighLevelDetectInterlock;
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.TankCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.TankCond] = string.Format("Tank Level Ng");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.TankCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.TankCond] = null;
                }
                ok &= check;
                check = true; // 11.02.09 minhan

                // pump
                if ((m_Server.JobCond.ProcessMode) && eqpPumpUnits._RB_SHW_Pump_Unit.Pump.IsUse) // 11.02.09 minhan
                {
                    check &= (eqpPumpUnits._RB_SHW_Pump_Unit.RefAct == PumpAct.Run);
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] == null)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] = string.Format("Pump Not Ready");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.PumpCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] = null;
                }
                ok &= check;
                check = true;

                // hepa
                bool Use = eqpHepaFilters._AK_Unit_Hepa_Filter.SetupHepaInterlock.Use;

                if (Use) check &= !eqpHepaFilters._AK_Unit_Hepa_Filter.IsAlarm; // 09.12.08 minhan ak 구간 hepa 알람을 ld에서 본다.

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.AkHepacond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.AkHepacond] = string.Format("Ak Hepa Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.AkHepacond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.AkHepacond] = null;
                }
                ok &= check;
                check = true;

                // rb
                Use = true;
                foreach (RbUnit unit in m_RbUnits)
                {
                    Use &= m_Server.JobCond.RbUse(unit);
                    if (Use)
                        check &= ThreadRollBrush_BOE_G8_DHDC.Instance.GetRbCond(unit);
                }

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.RbCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.RbCond] = string.Format("RB Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.RbCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.RbCond] = null;
                }
                ok &= check; // 11.02.01 minhan
                check = true;

                //Ffu
                Use = eqpFanFilterControls._LD_Unit_Fan_Filter_Control.SetupFfuControlIntr.GetValue<bool>(); // 11.03.07 minhan
                if (Use) check &= !eqpFanFilterControls._LD_Unit_Fan_Filter_Control.IsAlarm;

                if (!check && CheckUnitCond[cv.Id][(int)CheckCond.FfuCond] == null)
                {//2010.09.23 kimgun
                    CheckUnitCond[cv.Id][(int)CheckCond.FfuCond] = string.Format("FFU Control Alarm");
                    cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.FfuCond]);
                }
                else if (check)
                {
                    CheckUnitCond[cv.Id][(int)CheckCond.FfuCond] = null;
                }
                ok &= check;
            }
            //else if (cv.Id == eqpTransferUnits._AP_CvUnit.Id) //11.11.16 ld 구간을 제외하고 C/V stop 금지(유저요청)
            //{//2009.08.13 kimgun
            //    //bool bUse = true;

            //    //bUse &= m_Server.JobCond.ProcessMode; // 09.09.11 minhan

            //    // HPMJ
            //    if (m_Server.JobCond.HpmjUse(eqpHpmjs._HPMJ_Unit) &&
            //        //eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>() &&
            //        m_Server.JobCond.ProcessMode)// 09.12.08 minhan
            //    {
            //        check &= eqpHpmjs._HPMJ_Unit.IfFlag.Ready;
            //        check &= !eqpHpmjs._HPMJ_Unit.IfFlag.PIDError;
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] = string.Format("Hpmj Not Ready");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] = null;
            //    }
            //    ok &= check;
            //    check = true;

            //    // AP PLASMA
            //    if (m_Server.JobCond.ApUse(eqpSeAps._SE_AP_Plasma_Unit))
            //    {
            //        check &= (m_Server as ServerManager).ThreadHandler.ApControl.GetApCond(eqpSeAps._SE_AP_Plasma_Unit);
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.ApCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.ApCond] = string.Format("Ap Not Ready");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.ApCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.ApCond] = null;
            //    }
            //    ok &= check;
            //    check = true;

            //    // rb
            //    bool bUse = true;
            //    foreach (RbUnit unit in m_RbUnits)
            //    {
            //        bUse &= m_Server.JobCond.RbUse(unit);
            //        if (bUse)
            //            check &= ThreadRollBrush_BOE_G8_DHDC.Instance.GetRbCond(unit);

            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.RbCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.RbCond] = string.Format("RB Alarm");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.RbCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.RbCond] = null;
            //    }
            //    ok &= check;
            //    check = true;

            //    // pump, tank
            //    if ((m_Server.JobCond.ProcessMode) && eqpPumpUnits._RB_SHW_Pump_Unit.Pump.IsUse) // 11.02.01 minhan 탱크가 중요한게 아니라 펌프겠지.
            //    {
            //        check &= !eqpTankUnits._FR_Unit_Tank.IfFlag.TankLevelFault;
            //        check &= !GlobalVar.HighLevelDetectInterlock;
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.TankCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.TankCond] = string.Format("Tank Level Ng");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.TankCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.TankCond] = null;
            //    }
            //    ok &= check;
            //    check = true; // 11.02.09 minhan

            //    // pump
            //    if ((m_Server.JobCond.ProcessMode) && eqpPumpUnits._RB_SHW_Pump_Unit.Pump.IsUse) // 11.02.09 minhan
            //    {
            //        check &= (eqpPumpUnits._RB_SHW_Pump_Unit.RefAct == PumpAct.Run) ;
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] == null)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] = string.Format("Pump Not Ready");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.PumpCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] = null;
            //    }
            //    ok &= check;

            //    //check = true; // 11.02.01 minhan
            //}
            //else if (cv.Id == eqpTransferUnits._RB_CvUnit.Id) // 11.01.27 minhan 일단 다 정지하는 것으로 하고 차 후 디버깅하자.
            //{
            //    // HPMJ
            //    if (m_Server.JobCond.HpmjUse(eqpHpmjs._HPMJ_Unit) &&
            //        //eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>() &&
            //        m_Server.JobCond.ProcessMode) // 11.02.01 minhan
            //    {
            //        check &= eqpHpmjs._HPMJ_Unit.IfFlag.Ready;
            //        check &= !eqpHpmjs._HPMJ_Unit.IfFlag.PIDError;
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] = string.Format("Hpmj Not Ready");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.HpmjCond] = null;
            //    }
            //    ok &= check;
            //    check = true;

            //    // rb
            //    bool bUse = true;
            //    foreach (RbUnit unit in m_RbUnits)
            //    {
            //        bUse &= m_Server.JobCond.RbUse(unit);
            //        if (bUse)
            //            check &= ThreadRollBrush_BOE_G8_DHDC.Instance.GetRbCond(unit);

            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.RbCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.RbCond] = string.Format("RB Alarm");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.RbCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.RbCond] = null;
            //    }
            //    ok &= check;
            //    check = true;

            //    // pump,tank
            //    if ((m_Server.JobCond.ProcessMode) && eqpPumpUnits._RB_SHW_Pump_Unit.Pump.IsUse) // 11.02.01 minhan 탱크가 중요한게 아니라 펌프겠지.
            //    {
            //        check &= !eqpTankUnits._FR_Unit_Tank.IfFlag.TankLevelFault;
            //        check &= !GlobalVar.HighLevelDetectInterlock;
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.TankCond] == null)
            //    {//2010.09.23 kimgun
            //        CheckUnitCond[cv.Id][(int)CheckCond.TankCond] = string.Format("Tank Level Ng");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.TankCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.TankCond] = null;
            //    }
            //    ok &= check;
            //    check = true; // 11.02.09 minhan

            //    // pump
            //    if ((m_Server.JobCond.ProcessMode) && eqpPumpUnits._RB_SHW_Pump_Unit.Pump.IsUse) // 11.02.09 minhan
            //    {
            //        check &= (eqpPumpUnits._RB_SHW_Pump_Unit.RefAct == PumpAct.Run) ;
            //    }

            //    if (!check && CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] == null)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] = string.Format("Pump Not Ready");
            //        cv.SetLog(CheckName, 0, 0, CheckUnitCond[cv.Id][(int)CheckCond.PumpCond]);
            //    }
            //    else if (check)
            //    {
            //        CheckUnitCond[cv.Id][(int)CheckCond.PumpCond] = null;
            //    }
            //    ok &= check;
            //}
            else if (cv.Id == eqpTransferUnits._AK_CvUnit.Id) // 11.01.27 minhan 이거는 Cond가 필요없을 것 같네 뭐 알람 발생하는데.
            {
                ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge.IsAlarm;
                ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge.IsAlarm;
                ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge.IsAlarm;
                ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge.IsAlarm;

            }
            else if (cv.Id == eqpTransferUnits._UL_CvUnit.Id) // 11.01.27 minhan
            {
                bool GlassDetect = true;
                GlassDetect &= m_Server.GlassData.IsExist(eqpTransferUnits._UL_CvUnit.DataMatchingKey(0));
                GlassDetect &= eqpTransferUnits._UL_CvUnit.GlsInSensor.IsDetected(Logic.OR);
                GlassDetect &= eqpTransferUnits._AK_CvUnit.GlsOutSensor.IsDetected(Logic.OR);

                if (GlassDetect)
                {
                    ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge.IsAlarm;
                    ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge.IsAlarm;
                    ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge.IsAlarm;
                    ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge.IsAlarm;
                }
            }

            if (cv.NextCv == null)
            {
                ok &= true;
            }

            return ok;
        }
        public override bool IsRunCondition(CvUnit cv)
        {
            bool run = false;

            run |= cv.IfFlag.InReady;
            run |= cv.IfFlag.OutReady;
            run |= !cv.IfFlag.OutComp;


            if (cv.PrevCv == null)
            {
                //run |= GlsInSensor.IsDetected();
            }
            else
            {
                run |= cv.PrevCv.IfFlag.OutReady;
                run |= !cv.PrevCv.IfFlag.OutComp;
                run |= m_Server.GlassData.IsExist(cv.DataMatchingKey(0));
            }

            if (cv.NextCv != null)
            {
                run |= !cv.NextCv.IfFlag.InComp;
            }
            else
            {
                if (cv.IfFlag.OutReady && !cv.IfFlag.OutComp) run = false;
                if (eqpTransferUnits._UL_Fish_Hand.IfFlag.InComp && !cv.IfFlag.OutReady) run = false;
            }

            if (cv.PrevCv != null) // 09.09.11 minhan
            {
                bool glassDetect = false;
                bool nextglassDetect = false; // 11.05.14 minhan

                if (cv.Id == eqpTransferUnits._UL_CvUnit.Id) // 11.04.27 minhan
                {
                    glassDetect = m_Server.GlassData.IsExist(cv.DataMatchingKey(1));
                    glassDetect |= cv.GlsOutSensor.IsDetected(Logic.OR);
                    //if (cv.NextCv != null) glassDetect |= m_Server.GlassData.IsExist(cv.NextCv.DataMatchingKey(0)); // 09.09.23 minhan kimx 검토하고 kimx는 지워라.
                    if (!glassDetect)
                    {
                        if (!cv.PrevCv.IfFlag.RecoveryReq)
                        {
                            run &= !cv.PrevCv.IfFlag.InError;
                            run &= !cv.PrevCv.IfFlag.OutError;
                        }
                    }
                }
                else
                {
                    if (cv.NextCv != null)
                    {
                        nextglassDetect = m_Server.GlassData.IsExist(cv.NextCv.DataMatchingKey(0)); // 11.05.14 minhan
                        nextglassDetect |= cv.NextCv.GlsInSensor.IsDetected(Logic.OR);
                    }

                    if (!nextglassDetect) // 11.04.27 minhan
                    {
                        if (!cv.PrevCv.IfFlag.RecoveryReq)
                        {
                            run &= !cv.PrevCv.IfFlag.InError;
                            run &= !cv.PrevCv.IfFlag.OutError;
                        }
                    }
                }
            }

            if (cv.Id != eqpTransferUnits._UL_CvUnit.Id) // 11.02.01 minhan 검토해야함.
            {
                //bool glassExist = cv.GlsOutSensor.IsDetected();
                //run &= !glassExist;
                run &= !GlobalVar.UlTimeOut;

            }
            else if (cv.Id == eqpTransferUnits._UL_CvUnit.Id) // 11.06.08 minhan
            {
                run &= !GlobalVar.UlcvStop;
            }
            //else if (cv.Id == eqpTransferUnits._UL_CvUnit.Id) // 09.09.11 minhan
            //{
            //    bool glassExist = cv.GlsOutSensor.IsDetected();
            //    if (glassExist) run = false;
            //}

            run &= m_GenInfos.EqpInitComp;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.AutoMode;
            //run &= !GlobalVar.UlTimeOut;// !m_Server.SeqFlag.UlTimeOut; // 09.09.11 minhan

            return run;
        }
        public override bool IsSeqRunCondition(CvUnit cv)
        {
            bool run = true;
            run &= !IsInterlock(cv);
            run &= !CvStopCondition(cv.NextCv);
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.AutoMode;
            return run;
        }
        public override bool CvStopCondition(CvUnit cv)
        {
            bool cvStop = false;
            CvUnit temp = cv;

            while (temp != null)
            {
                TagCvIfFlag flag = temp.IfFlag;
                cvStop |= flag.InError;
                cvStop |= flag.OutError;
                if (cv.Id != eqpTransferUnits._UL_CvUnit.Id &&
                   (cv.Id != eqpTransferUnits._AK_CvUnit.Id))
                {
                    cvStop |= GlobalVar.UlTimeOut;
                    cvStop |= GlobalVar.UlHnadAlarm;//2009.08.25 kimgun
                }

                //if (temp.Id == eqpTransferUnits._LD_CvUnit.Id) // 11.02.01 minhan 없다.
                //{
                //    cvStop |= GlobalVar.LdBroken;
                //}

                //if(temp.Id == eqpTransferUnits._AP_CvUnit.Id)
                //{
                //    //cvStop |= GlobalVar.EUVBroken;
                //    bool glassExist = m_Server.GlassData.IsExist(eqpTransferUnits._AP_CvUnit.DataMatchingKey(1));
                //         glassExist |= eqpTransferUnits._AP_CvUnit.GlsOutSensor.IsDetected(Logic.OR);
                //    if (!glassExist) cvStop |= GlobalVar.LdBroken;
                //}

                //if (m_Server.JobCond.ProcessMode) // 11.02.01 minhan 검토해야함.
                //{//2010.09.09 kimgun
                //    cvStop |= eqpTankUnits._FR_Unit_Tank.IfFlag.TankLevelFault;
                //}

                if (cvStop) break;

                temp = temp.NextCv;
            }

            return cvStop;
        }
        public override void InitParameter() // 09.09.22 minhan
        {
            foreach (CvUnit unit in m_CvUnits)
            {
                if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();
                if (unit.Sequence[1] != null) unit.Sequence[1].InitSeq();
                unit.IfFlag.Reset();
                unit.AutoAct = CvMotorAct.Stop;

                int motorCount = unit.Motors.Count;
                for (int i = 0; i < motorCount; i++)
                {
                    unit.ManualAct[i] = CvMotorAct.Stop;
                    unit.ManualSpeed[i] = 0;
                }
            }

            foreach (TransferUnit device in m_TransferUnits) // 11.02.19 minhan
            {
                for (int j = 0; j < (int)CheckCond.CheckCondCnt; j++)
                {
                    CheckUnitCond[device.Id][j] = null;
                }
            }

            GlobalVar.GlassOutComp1 = true;
            GlobalVar.UlTimeOut = false;
            GlobalVar.MsgSkip = false; // 11.05.17 minhan
            GlobalVar.MsgBrokenComp = false;
            GlobalVar.MsgRequest = false;
            GlobalVar.MsgULSkip = false; // 11.06.08 minhan
            GlobalVar.MsgULBrokenComp = false;
            GlobalVar.UlcvStop = false;
            //BaseGlobalVar.LdTimeOut = false; // 11.03.02 minhan
            //GlobalVar.LdBroken = false; // 11.02.01 minhan 제거
            //GlobalVar.EUVBroken = false;
        }
        public override void TimerControl(CvUnit cv, _GSS type, params XTimer[] timers) // 11.03.02 minhan
        {
            int count = timers.Length;

            if (cv.AutoAct == CvMotorAct.Fw)
            {
                bool timerResume = false;

                for (int i = 0; i < count; i++)
                {
                    timerResume |= timers[i].IsPaused;
                }
                if (timerResume)
                {
                    foreach (XTimer timer in timers)
                    {
                        if (timer != null)
                            timer.Resume();
                    }
                    // cv.TimerPause[(int)type] = false;
                }
            }
            else if (cv.AutoAct == CvMotorAct.Stop)
            {
                bool timerPused = true;

                for (int i = 0; i < count; i++)
                {
                    timerPused &= timers[i].IsPaused;

                }

                if (!timerPused)
                {
                    foreach (XTimer timer in timers)
                    {
                        if (timer != null)
                            timer.Pause();
                    }
                    // cv.TimerPause[(int)type] = true;
                }
            }
        }
        #endregion
    }

    public class SeqUnitLdCv1 : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        //private FishHand m_LdHandUnit = null;
        private FishHand m_UlHandUnit = null;
        private GantryUnit m_GantryUnit = null;//2009.08.27 kimgun
        private Simul m_Simul;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private ActuatorUnit m_LdTilting;
        private ActuatorUnit m_LdIdeRoller;
        private Alarm m_AlarmLdTiltingUP;
        private Alarm m_AlarmLdTiltingDW;
        //private Alarm m_WarningLdTiltingUP;//2010.09.09 kimgun
        //private Alarm m_WarningLdTiltingDW;//2010.09.09 kimgun
        private ActuatorUnit m_Aligner;
        private Alarm m_AlarmAlignFw;
        private Alarm m_AlarmAlignBw;
        private Alarm m_AlarmLdIdeRollerFw;//2013.11.12 fan
        private Alarm m_AlarmLdIdeRollerBw;//2013.11.12 fan
        //private Alarm m_WarningAlignFw;//2010.09.09 kimgun
        //private Alarm m_WarningAlignBw;//2010.09.09 kimgun
        private int m_TiltingSeqNo = 0;

        private int TiltingRetryCnt = 0;//kang
        public int TiltingUpTimeoutAlarm = 0;
        public int TiltingDownTimeoutAlarm = 0;
        private Sensor m_LdRobotHandDetectSensor;
        private string GlassRecipeId;
        private TagGlassData m_RecvGlassData = new TagGlassData();
        private ThreadCvBOE_G8_DHDC m_Control;
        private string m_Msg;
        private GenInfoHandler m_GenInfo;
        private int LdTiltingUPTimeoutVal = 0; // 09.09.30 minhan
        private int LdTiltingDWTimeoutVal = 0; // 09.09.30 minhan
        private int ldAlignFwTimeoutVal = 0; // 09.09.30 minhan
        private int ldAlignBwTimeoutVal = 0; // 09.09.30 minhan
        private int LdIdleRollerFwTimeoutVal = 0;//2013.11.12 fan
        private int LdIdleRollerBwTimeoutVal = 0;//2013.11.12 fan
        //private int AlignRetryCnt = 0;//2010.09.09 kimgun //LSB
        private bool glassInDataExist;
        private bool glassOutDataExist;
        private bool glassInDetected;
        private bool glassOutDetected;
        private bool simulation;
        #endregion

        #region Constructor
        public SeqUnitLdCv1(ThreadCvBOE_G8_DHDC control, CvUnit cv) // 11.02.01 minhan
        {
            Cv = cv;
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            // m_LdHandUnit = eqpTransferUnits._LD_Fish_Hand;
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand;
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_SeqFunName = Cv.GlsInSensor.Name;
            m_LdRobotHandDetectSensor = eqpSensors._LD_Robot_Hand_Interlock_Sensor;
            //sOldRecipeId = m_GenInfo.CurRecipeId; // 10.12.25 minhan

            m_Timer1 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Timeout");
            m_Timer2 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Off Timeout");
            m_Timer3 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : On Time warning");

            m_AlarmOnTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmOffTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmLdTiltingUP = new Alarm(Cv.Name + " : Tilting UP Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmLdTiltingDW = new Alarm(Cv.Name + " : Tilting DW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            // m_WarningLdTiltingUP = new Alarm(Cv.Name + " : Tilting UP Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);//2010.09.09 kimgun
            //m_WarningLdTiltingDW = new Alarm(Cv.Name + " : Tilting DW Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);//2010.09.09 kimgun

            m_AlarmAlignFw = new Alarm(Cv.Name + " : Align FW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmAlignBw = new Alarm(Cv.Name + " : Align BW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            //m_WarningAlignFw = new Alarm(Cv.Name + " : Align FW Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);//2010.09.09 kimgun
            //m_WarningAlignBw = new Alarm(Cv.Name + " : Align BW Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);//2010.09.09 kimgun
            m_AlarmLdIdeRollerFw = new Alarm(Cv.Name + " : IdleRoller FW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);//2013.11.12 fan
            m_AlarmLdIdeRollerBw = new Alarm(Cv.Name + " : IdleRoller BW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);//2013.11.12 fan

            m_Aligner = eqpActuatorUnits._LD_Align;
            m_LdTilting = eqpActuatorUnits._LD_Tilting_Unit;
            m_GenInfo = GenInfoHandler.Instance;
            m_LdIdeRoller = eqpActuatorUnits._LD_Idle_Roller;
            glassInDataExist = false;
            glassOutDataExist = false;
            glassInDetected = false;
            glassOutDetected = false;
            simulation = false;
        }
        #endregion

        #region Sequence
        #region enum
        public class TiltingCommand
        {
            public enum ULTiming
            {
                _TiltingUp = 0,
                _TiltingDown
            }
        }
        #endregion
        #region Methods
        public void FlagSet() // 11.02.01 minhan
        {
            simulation = m_Simul.Device;
            m_Server.GlassData.GetData(Cv.DataMatchingKey(0), ref m_RecvGlassData);
            m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
            m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            glassInDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(0));
            glassOutDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(1));
            glassInDetected = Cv.GlsInSensor.IsDetected(Logic.OR);
            glassOutDetected = Cv.GlsOutSensor.IsDetected(Logic.OR);

        }

        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.InError)
            {
                Cv.IfFlag.InError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }
        #endregion

        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp) return -1;

            if (Cv.Sequence[0] == null) Cv.Sequence[0] = this;

            m_Control.TimerControl(Cv, _GSS.ssIN, m_Timer1, m_Timer2, m_Timer3);

            if (m_Eqp.AlarmResetSwitchPushed) // 11.01.27 minhan 참 컨셉이 어떻게 가는지 모르겠네. 참 짜 맞추는 것도 힘드네.
            {
                m_Eqp.ResetAlarm(m_AlarmLdTiltingUP.Id);
                m_Eqp.ResetAlarm(m_AlarmLdTiltingDW.Id);
                //m_Eqp.ResetAlarm(m_WarningAlignBw.Id);
                //m_Eqp.ResetAlarm(m_WarningAlignFw.Id);
            }

            if (!m_Control.IsSeqRunCondition(Cv)
                //&& m_LdTilting.IsPositive()
                && m_Aligner.IsPositive())
            {
                m_StartTicks = XFunc.GetTickCount(); // 11.03.02 minhan
                return -1; // 11.02.01 minhan
            }
            //bool test1 = m_Control.IsSeqRunCondition(Cv);
            //bool test2 = m_LdTilting.IsPositive();
            //bool test3 = m_Aligner.IsPositive();
            m_GenInfo.TimeOutMargin = string.Format("{0:F2}", Cv.SetupTimeoutMargin.GetValue<double>() / ((double)m_Server.JobCond.CurrentRecipe.CvSpeed / 60));//2009.10.05 LeeChungWon : User 요청 사항

            int nSeqNo = this.m_SeqNo;
            //            int Rv = -1;
            FlagSet(); // 11.02.01 minhan

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        if (simulation && glassInDataExist)
                        {
                            Cv.GlsInSensor.SetState(true, Logic.AND);
                        }

                        if (glassInDataExist)
                        {
                            Cv.IfFlag.InComp = false;

                            nSeqNo = 45; // Align Forward and Check Next CV Cond.
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data exist and Sensor is detected(case0)");
                        }
                        else
                        {
                            if (glassInDetected)
                            {
                                Cv.IfFlag.InComp = false;
                                nSeqNo = 130;
                            }
                            else
                            {
                                nSeqNo = 10;
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data is not exist and Sensor is not detected(Case0)");
                            }
                        }
                    }
                    break;
                case 10:
                    if (!Cv.GlsInSensor.IsDetected(Logic.AND) &&
                        !Cv.GlsOutSensor.IsDetected(Logic.AND) &&
                        (Cv.AutoAct == CvMotorAct.Stop))
                    {
                        if (!m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_LdTilting.SetNegativeAct();
                            m_Aligner.SetNegativeAct();
                            m_LdIdeRoller.SetNegativeAct();
                            m_StartTicks = XFunc.GetTickCount();
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Ld Unit Tilt : DW");
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "LD  Align : BW");
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "LD IdleRoller : BW");
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 10;
                            nSeqNo = 1000;
                        }
                        //m_Aligner.SetNegativeAct();
                        //AlignRetryCnt = 0; // 11.02.01 minhan //LSB
                        //AlignRetryCnt++;//2010.09.09 kimgun  //LSB
                        //m_LdTilting.SetNegativeAct();
                        //m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting : DW");
                        //ldAlignBwTimeoutVal = m_Server.SetupLdAlignBwTimeoutAlarm.GetValue<int>(); // 10.12.25 minhan
                        //m_StartTicks = XFunc.GetTickCount();


                        //nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        LdTiltingDWTimeoutVal = m_Server.SetupLdTiltingDWTimeoutAlarm.GetValue<int>(); // 10.12.25 minhan
                        //ldTiltingDWTimeoutVal=m_Server.setupl
                        if (m_LdTilting.IsNegative())
                        {

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting : DW OK");
                            nSeqNo = 21;
                        }
                        else if (m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            m_LdTilting.SetStopAct();
                            m_Aligner.SetStopAct();
                            m_LdIdeRoller.SetStopAct();

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 10;
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > LdTiltingDWTimeoutVal * 1000)
                        {

                            m_AlarmId = m_AlarmLdTiltingDW.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting : DW Alarm");
                            m_ReturnSeqNo = 10;

                            nSeqNo = 5000;
                        }

                    }
                    break;
                case 21://2013.11.12 fan
                    {
                        ldAlignBwTimeoutVal = m_Server.SetupLdAlignBwTimeoutAlarm.GetValue<int>();
                        //ldTiltingDWTimeoutVal=m_Server.setupl
                        if (m_Aligner.IsNegative())
                        {

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align  : BW OK");
                            nSeqNo = 22;
                        }
                        else if (m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            m_LdTilting.SetStopAct();
                            m_Aligner.SetStopAct();
                            m_LdIdeRoller.SetStopAct();

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 10;
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > ldAlignBwTimeoutVal * 1000)
                        {

                            m_AlarmId = m_AlarmAlignBw.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align: BW Alarm");
                            m_ReturnSeqNo = 10;

                            nSeqNo = 5000;
                        }

                    }
                    break;
                case 22://2013.11.12 fan
                    {
                        LdIdleRollerBwTimeoutVal = m_Server.SetupLdIdleRollerBwTimeoutAlarm.GetValue<int>();
                        //LdIdleRollerBwTimeoutVal = m_Server.SetupLdIdleRollerBwTimeoutAlarm.GetValue<int>(); 
                        //ldTiltingDWTimeoutVal=m_Server.setupl
                        if (m_LdIdeRoller.IsNegative())
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD IdleRoller  : BW OK");
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Interface : Ready");

                            //Cv.IfFlag.InReady = true; 

                            nSeqNo = 30;
                        }
                        else if (m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            m_LdTilting.SetStopAct();
                            m_Aligner.SetStopAct();
                            m_LdIdeRoller.SetStopAct();

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 10;
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > LdIdleRollerBwTimeoutVal * 1000)
                        {
                            //if (AlignRetryCnt > 1)//2010.09.09 kimgun //LSB
                            //{
                            //    AlignRetryCnt = 0;                       
                            m_AlarmId = m_AlarmLdIdeRollerBw.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true; // 11.01.27 minhan
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD IdleRoller: BW Alarm");
                            m_ReturnSeqNo = 10;
                            nSeqNo = 5000;
                        }
                    }
                    break;
                case 30:
                    {
                        bool RecvCond = !m_LdRobotHandDetectSensor.IsDetected();
                        RecvCond &= m_GenInfo.AutoMode & !m_GenInfo.CycleStop & !m_GenInfo.CleanOut & !m_GenInfo.Pause;
                        RecvCond &= m_LdTilting.IsNegative();
                        RecvCond &= m_LdIdeRoller.IsNegative();
                        RecvCond &= m_Aligner.IsNegative();
                        RecvCond &= !Cv.GlsInSensor.IsDetected(Logic.AND);

                        if (RecvCond && !glassInDataExist)
                        {
                            GlobalVar.rcvREQ = true;
                            Cv.SetLog(m_SeqFunName, 0, 0, "LD Unit Glass Recv Request");
                            nSeqNo = 40;
                        }
                        else if ((m_Aligner.GetRefAct() != ActuatorAct.Neg) && !m_Aligner.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                        {
                            m_Aligner.SetNegativeAct();
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align : BW");
                        }
                        //else if ((m_LdTilting.GetRefAct() != ActuatorAct.Neg) && !m_LdTilting.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                        //{
                        //    m_LdTilting.SetNegativeAct();
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting : DW");
                        //}
                        //else if ((m_LdIdeRoller.GetRefAct() != ActuatorAct.Neg) && !m_LdIdeRoller.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                        //{
                        //    m_LdIdeRoller.SetNegativeAct();
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD IdleRoller : BW");
                        //}
                        else if (glassInDataExist && glassInDetected)
                        {
                            nSeqNo = 40;
                        }

                    }
                    break;
                // case 40:
                //if (m_LdHandUnit.IfFlag.OutComp) // LD Fish가 After Recv Pos 이동 후 날려주도록 하자.
                //{
                //    m_LdHandUnit.IfFlag.OutComp = false;

                //    m_Timer1.Start(3000);
                //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Hand Out Complete : OK");
                //    nSeqNo = 50;
                //}
                //if ((m_LdTilting.GetRefAct() != ActuatorAct.Neg) && !m_LdTilting.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                //{
                //    m_LdTilting.SetNegativeAct();
                //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting : DW");
                //    nSeqNo = 50;
                //}
                //break;
                case 40:
                    {
                        if (GlobalVar.rcvCOMP)
                        {
                            GlobalVar.rcvCOMP = false;
                            Cv.IfFlag.InReady = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Unit Glass In Complete");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 45;
                        }
                        else if (GlobalVar.rcvCANCEL)
                        {
                            GlobalVar.rcvCANCEL = false;
                            GlobalVar.rcvREQ = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Load Cancel");
                            nSeqNo = 0;
                        }
                        //if (glassInDetected && glassInDataExist)
                        //{
                        //    //m_Server.GlassData.Move(m_LdHandUnit.DataMatchingKey(0), Cv.DataMatchingKey(0));

                        //    Cv.IfFlag.InComp = true; // minhan ld hand 는 바로 접고 올라가야 하지 않을까...
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Detected Complete");

                        //    GlobalVar.rcvREQ = false;
                        //    ClearErrorConditionFlags(); // 09.10.15 minhan
                        //    nSeqNo = 45;
                        //}
                        else if ((m_Aligner.GetRefAct() != ActuatorAct.Neg) && !m_Aligner.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                        {
                            m_Aligner.SetNegativeAct();
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align : BW");
                        }
                        //else if ((m_LdTilting.GetRefAct() != ActuatorAct.Neg) && !m_LdTilting.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                        //{
                        //    m_LdTilting.SetNegativeAct();
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting : DW");
                        //}
                        //else if ((m_LdIdeRoller.GetRefAct() != ActuatorAct.Neg) && !m_LdIdeRoller.IsNegative() && m_GenInfo.AutoMode) // 09.09.23 minhan
                        //{
                        //    m_LdIdeRoller.SetNegativeAct();
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD IdleRoller : BW");
                        //}
                        //else if (m_Timer1.Over) // 09.10.15 minhan  //zhangliang 130826
                        //{
                        //    AlarmId = m_AlarmOnTimeOut.Id;
                        //    m_Eqp.SetAlarm(AlarmId);
                        //    Cv.IfFlag.InError = true;
                        //    Cv.IfFlag.RecoveryReq = false;
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");
                        //    ReturnSeqNo = nSeqNo;
                        //    nSeqNo = 2000;
                    }
                    break;
                case 45:
                    {
                        if (GetElapsedTicks() > 100)
                        {
                            string date = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.02.09 minhan
                            m_Server.ApdItemsHandler.SetData(Cv.DataMatchingKey(0), eqpApdItems._GLS_START.Id, date);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Start Time : " + date); // 10.12.29 minhan
                            TagGlassData data = new TagGlassData();
                            m_Server.GlassData.GetData(Cv.DataMatchingKey(0), ref data);

                            data.Processed = true;

                            m_Server.GlassData.Update(Cv.DataMatchingKey(0), data);
                            //m_Aligner.SetPositiveAct();
                            //m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 50;
                        }
                    }
                    break;
                case 50:
                    {
                        if (!m_Aligner.IsPositive())
                        {
                            m_Aligner.SetPositiveAct();
                            m_StartTicks = XFunc.GetTickCount();
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align FW Start");
                            nSeqNo = 52;
                        }
                        else if (m_Aligner.IsPositive())
                        {

                            nSeqNo = 60;
                        }
                    }
                    break;
                case 52:
                    {
                        ldAlignFwTimeoutVal = m_Server.SetupLdAlignFwTimeoutAlarm.GetValue<int>(); // 10.12.25 minhan

                        if (m_Aligner.IsPositive())
                        {
                            //if (GetElapsedTicks() > (ldAlignFwTimeoutVal * 0.9 * 1000))
                            //{//2010.09.09 kimgun
                            //    m_Eqp.SetAlarm(m_WarningAlignFw.Id);
                            //}
                            //else m_Eqp.ResetAlarm(m_WarningAlignFw.Id);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align : FW OK");
                            nSeqNo = 60;
                        }
                        else if (m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            m_LdTilting.SetStopAct();
                            m_Aligner.SetStopAct();
                            m_LdIdeRoller.SetStopAct();

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 50;
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > ldAlignFwTimeoutVal * 1000)
                        {

                            m_AlarmId = m_AlarmAlignFw.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true; // 11.01.27 minhan
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align : FW Alarm");
                            m_ReturnSeqNo = 45;
                            nSeqNo = 5000;

                        }
                    }
                    break;
                case 60:
                    {
                        bool tiltUpCond = true;
                        tiltUpCond &= !eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected();
                        tiltUpCond &= m_Aligner.IsPositive();
                        tiltUpCond &= glassInDataExist && !glassOutDataExist;
                        tiltUpCond &= glassInDetected && !glassOutDetected;

                        if (tiltUpCond)
                        {
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Ld Unit Tilt Move Up Condition OK");
                            //m_LdTilting.SetPositiveAct();
                            //AlignRetryCnt = 0; // 11.02.01 minhan //LSB
                            //AlignRetryCnt++;//2010.09.09 kimgun //LSB
                            //m_StartTicks = XFunc.GetTickCount();
                            //ldAlignFwTimeoutVal = m_Server.SetupLdAlignFwTimeoutAlarm.GetValue<int>(); // 10.12.25 minhan
                            nSeqNo = 71;
                        }
                    }
                    break;
                case 71:
                    {
                        m_LdTilting.SetPositiveAct();
                        m_LdIdeRoller.SetPositiveAct();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 72;

                    }
                    break;
                case 72:
                    {
                        LdTiltingUPTimeoutVal = m_Server.SetupLdTiltingUPTimeoutAlarm.GetValue<int>(); // 10.12.25 minhan

                        if (m_LdTilting.IsPositive())
                        {
                            //if (GetElapsedTicks() > (LdTiltingUPTimeoutVal * 0.9 * 1000))
                            //{//2010.09.09 kimgun
                            //    m_Eqp.SetAlarm(m_WarningLdTiltingUP.Id);
                            //}
                            //else m_Eqp.ResetAlarm(m_WarningLdTiltingUP.Id);
                            //AlignRetryCnt = 0; // 11.03.02 minhan //LSB

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting: UP OK");
                            nSeqNo = 73;
                        }
                        else if (m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            m_LdTilting.SetStopAct();
                            m_Aligner.SetStopAct();
                            m_LdIdeRoller.SetStopAct();

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 71;
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > LdTiltingUPTimeoutVal * 1000)
                        {
                            //if (AlignRetryCnt > 1)//2010.09.09 kimgun // LSB
                            //{
                            m_AlarmId = m_AlarmLdTiltingUP.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true; // 11.01.27 minhan
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Tilting: UP Alarm");
                            m_ReturnSeqNo = 71;
                            nSeqNo = 5000;
                        }
                        //} //LSB
                        //else
                        //{
                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align : FW Alarm(Skip)");
                        //    nSeqNo = 60; // 10.12.21 minhan
                        //}

                    }
                    break;
                case 73: //2013.11.29 fan 
                    {
                        LdIdleRollerFwTimeoutVal = m_Server.SetupLdIdleRollerFwTimeoutAlarm.GetValue<int>();

                        if (m_LdIdeRoller.IsPositive())
                        {
                            //if (GetElapsedTicks() > (LdTiltingUPTimeoutVal * 0.9 * 1000))
                            //{//2010.09.09 kimgun
                            //    m_Eqp.SetAlarm(m_WarningLdTiltingUP.Id);
                            //}
                            //else m_Eqp.ResetAlarm(m_WarningLdTiltingUP.Id);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Idle Roller: UP OK");
                            nSeqNo = 75;
                        }
                        else if (m_LdRobotHandDetectSensor.IsDetected())
                        {
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_Eqp.SetAlarm(m_AlarmId);

                            m_LdTilting.SetStopAct();
                            m_Aligner.SetStopAct();
                            m_LdIdeRoller.SetStopAct();

                            //SetLog
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Robot Hand Sensing Error");
                            m_ReturnSeqNo = 71;
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > LdIdleRollerFwTimeoutVal * 1000)
                        {

                            m_AlarmId = m_AlarmLdIdeRollerFw.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD IdleRoller: UP Alarm");
                            m_ReturnSeqNo = 71;
                            nSeqNo = 5000;

                        }
                    }
                    break;
                case 75: // 10.12.25 minhan recipe 체크
                    {
                        TagGlassData data = new TagGlassData();
                        TagRecipe recipe = new TagRecipe();
                        m_Server.GlassData.GetData(Cv.DataMatchingKey(0), ref data);
                        GlassRecipeId = data.RecipeID;

                        if (!m_Server.DataProvider.RecipeProvider.Adapter.IsExist(GlassRecipeId))
                        {
                            m_AlarmId = Cv.ALM_GlsDataError.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Recipe ID Error");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                        }
                        else if (!m_Server.DataProvider.RecipeProvider.GetRecipe(GlassRecipeId, ref recipe) ||
                                 !TagRecipe.CheckRecipeItem(recipe))
                        {
                            m_AlarmId = Cv.ALM_GlsDataError.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Recipe Para Error");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                        }
                        else
                        {
                            ClearErrorConditionFlags();
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Recipe Check : OK");
                            nSeqNo = 80;
                        }
                    }
                    break;
                case 80:
                    if (GlassRecipeId == m_GenInfo.CurRecipeId) // 09.11.17 minhan
                    {
                        nSeqNo = 100;
                    }
                    else
                    {
                        nSeqNo = 90;
                    }
                    break;
                case 90: // 10.12.25 minhan
                    {
                        bool glassCount = false;
                        for (int i = eqpTransferUnits._LD_CvUnit.DataMatchingKey(1); i <= eqpTransferUnits._AK_CvUnit.DataMatchingKey(1); i++)
                        {
                            glassCount |= m_Server.GlassData.IsExist(i);
                        }

                        if (!glassCount)
                        {
                            m_GenInfo.CurRecipeId = GlassRecipeId;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Recipe Change!");
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 100:
                    {
                        m_Server.JobCond.SetRecipe2JobCond(m_GenInfo.CurRecipeId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Recipe ID : " + m_GenInfo.CurRecipeId); // 10.12.25 minhan

                        if (m_Server.JobCond.CurrentRecipe.HpmjUse) // 11.02.01 minhan
                        {
                            if (eqpHpmjs._HPMJ_Unit.IfFlag.PressureSet != m_Server.JobCond.CurrentRecipe.HpmjPressure)
                            {
                                eqpHpmjs._HPMJ_Unit.IfFlag.PPIDChangeRequest = true;
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "HPMJ PPID Change Request");
                            }
                        }

                        //if (m_Server.JobCond.ApUse(eqpSeAps._SE_AP_Plasma_Unit)) // 10.12.25 minhan AP가 반응이 느려서 여기서 줘야 할 것이다.
                        //{
                        //    double setN2Flow = m_Server.JobCond.ApN2Flow(eqpSeAps._SE_AP_Plasma_Unit);
                        //    double setCDAFlow = m_Server.JobCond.ApCDAFlow(eqpSeAps._SE_AP_Plasma_Unit);
                        //    double setVoltage = m_Server.JobCond.ApVoltage(eqpSeAps._SE_AP_Plasma_Unit);
                        //    string m_Msg = "";

                        //    eqpSeAps._SE_AP_Plasma_Unit.SetN2Flow(setN2Flow);
                        //    m_Msg = string.Format("N2 Flow set : {0}", setN2Flow.ToString());
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, m_Msg);

                        //    eqpSeAps._SE_AP_Plasma_Unit.SetCDAFlow(setCDAFlow);
                        //    m_Msg = string.Format("CDA Flow set : {0}", setCDAFlow);
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, m_Msg);

                        //    eqpSeAps._SE_AP_Plasma_Unit.SetVoltage(setVoltage);
                        //    m_Msg = string.Format("Plasma Power : {0}", setVoltage);
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, m_Msg);
                        //}

                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    {
                        bool ok = true;
                        foreach (CvUnit unit in ThreadCvControl.Units)
                        {
                            ok &= m_Control.CheckUnitCondition(unit);
                        }

                        if (ok)
                        {
                            if (m_Server.SetupMaxGlassNo.GetValue<int>() < 4)//2009.08.27 kimgun
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unit Condition Check : OK");
                                nSeqNo = 120;
                            }
                            else // 4매짜리
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unit Condition Check : OK(max Glass 4)");
                                nSeqNo = 115;
                            }
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unit Condition Check : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 4000;
                        }
                    }
                    break;
                case 115:
                    {
                        int glsCount = 0;
                        foreach (CvUnit unit in ThreadCvControl.Units)
                        {
                            if (unit.Id != eqpTransferUnits._LD_CvUnit.Id &&
                                (m_Server.GlassData.IsExist(unit.DataMatchingKey(0)) || m_Server.GlassData.IsExist(unit.DataMatchingKey(1))))
                            {
                                glsCount++;
                            }
                        }

                        if (m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0)))
                        {
                            glsCount++;
                        }

                        if (m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0)))
                        {
                            int posId = m_GantryUnit.DataMatchingKey(0);
                            TagGlassData data = new TagGlassData();
                            m_Server.GlassData.GetData(posId, ref data);
                            if (data.Processed)
                            {
                                glsCount++;
                            }
                        }

                        // dspcrassus - 111130 : C/V Unit & UL Hand 상에 2매 이상의 Glass가 있을 때는 TR이 Send 또는 Recv 위치로 이동할 때 LD Unit의 Glass를 출발시킨다.
                        // TR이 Send 또는 Recv 위치로 이동함은 Loader와의 I/F 중이 아니므로, Loader로 인한 Unloading 정체가 발생하지 않음
                        if (glsCount >= 3)
                        {
                            // dspcrassus - 111223 : 정체발생 가능 대기 Case
                        }
                        else if (glsCount <= 2) // LSB
                        {
                            // dspcrassus - 111223 : 정체 미발생 Case
                            m_Msg = string.Format("Advanced Glass = {0}", glsCount);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                            nSeqNo = 120;
                        }
                        else
                        {
                            if (GlobalVar.TrMoveLoadDir || GlobalVar.TrMoveUnloadDir)
                            {
                                m_Msg = string.Format("Advanced Glass = {0}", glsCount);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                                nSeqNo = 120;
                            }
                        }
                    }
                    break;
                // 11.03.02 minhan
                //{//2009.08.27 kimgun max glass 4매이하 짜리용 
                //    int nCnt = 0;
                //    //int nLdafterGlassCnt = 0;
                //    int TrposId = m_GantryUnit.DataMatchingKey(0);

                //    foreach (CvUnit unit in ThreadCvControl.Units)
                //    {
                //        if ((unit.Id != eqpTransferUnits._LD_CvUnit.Id) &&
                //            (m_Server.GlassData.IsExist(unit.DataMatchingKey(0)) ||
                //            m_Server.GlassData.IsExist(unit.DataMatchingKey(1))))
                //        {
                //            nCnt++;
                //            //nLdafterGlassCnt++;
                //        }
                //    }

                //    if (m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0)))
                //    {
                //        nCnt++;
                //        //nLdafterGlassCnt++;
                //    }

                //    if (m_GenInfo.EQPGlassCount > 3)
                //    {
                //        if (!GlobalVar.TrMoveLoadDir && m_Server.GlassData.IsExist(TrposId))
                //            nCnt++;
                //    }

                //    if (nCnt < 3) // 11.03.02 minhan
                //    {
                //        m_Msg = string.Format("Advanced Glass = {0}", nCnt);
                //        m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                //        nSeqNo = 120;
                //    }

                //    #region 제거
                //    //if (((m_GenInfo.EQPGlassCount == 3) && (nLdafterGlassCnt > 1)) &&
                //    //        m_UlHandUnit.IfFlag.OutReady &&
                //    //        m_GantryUnit.IfFlag.InReady/* && !GlobalVar.LdIng*/)
                //    //{
                //    //    if ((nCnt < 3) && eqpTransferUnits._UL_CvUnit.IfFlag.OutComp/* && !GlobalVar.LdIng*/) // 11.02.01 minhan 여기좀 다시보자.
                //    //    {
                //    //        m_Msg = string.Format("Advanced Glass = {0}", nCnt);
                //    //        m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                //    //        nSeqNo = 120;
                //    //    }

                //    //}
                //    //else if ((m_GenInfo.EQPGlassCount != 3) || (nLdafterGlassCnt < 2))
                //    //{
                //    //    if ((nCnt < 3) && (nLdafterGlassCnt < 2))
                //    //    {
                //    //        m_Msg = string.Format("Advanced Glass = {0}", nCnt);
                //    //        m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                //    //        nSeqNo = 120;
                //    //    }
                //    //}
                //    #endregion
                //}
                //break;
                case 120:
                    if (GlobalVar.TimeOver &&
                        !GlobalVar.StopCount1 &&
                        !GlobalVar.UlTimeOut)
                    {
                        if (!Cv.NextCv.GlsInSensor.IsDetected(Logic.AND) &&
                            //!Cv.NextCv.GlsOutSensor.IsDetected(Logic.AND) &&    // dspcrassus - 111223 : Glass 간격 추가
                            !m_Server.GlassData.IsExist(Cv.NextCv.DataMatchingKey(0)) &&
                            !m_Server.GlassData.IsExist(Cv.NextCv.DataMatchingKey(1)) &&
                            !m_Server.GlassData.IsExist(Cv.NextCv.NextCv.DataMatchingKey(0))) // 11.02.09 minhan
                        {
                            GlobalVar.TimeOver = false;
                            m_Msg = string.Format("Tact Time = {0}", m_GenInfo.TactTime);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                            nSeqNo = 130;
                        }
                    }
                    else
                    {//2009.08.13 kimgun tact 대기동안 앞쪽 상태 체크하자.
                        foreach (CvUnit unit in ThreadCvControl.Units)
                        {
                            if (!m_Control.CheckUnitCondition(unit))
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unit Condition Check : NG");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 4000;
                                break;
                            }
                        }

                    }
                    break;
                case 130:
                    if (Cv.IfFlag.OutComp)
                    {
                        Cv.IfFlag.OutComp = false;
                        Cv.IfFlag.InReady = true;

                        m_Server.JobCond.SetTactTime(TactTimeAct.ttCOUNT_START);

                        int count = m_Server.PartsItemsHandler.GetItems().Count;
                        for (int i = 0; i < count; i++)
                        {
                            m_Server.PartsItemsHandler.IncreaseCount(i);
                        }

                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Skip");

                        // Timer Set
                        int distance = Cv.SetupDistance.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        int LdIndistance = distance - glassSize;
                        LdIndistance = (LdIndistance < 0) ? 0 : LdIndistance;

                        int speed = m_Server.JobCond.CvProcessSpeed(Cv.Id); // 09.12.20 minhan

                        m_Control.GetTimeOutParameter(Cv, LdIndistance, glassSize, speed, ref m_Tm1, ref m_Tm2, ref m_Tm3); // 09.12.20 minhan
                        m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.02.01 minhan

                        m_Timer1.Start(m_Tm1);

                        nSeqNo = 140;
                    }
                    break;
                case 140:
                    {
                        bool glassDetected = false;
                        bool Tiltingcheck = (m_LdTilting.IsPositive() && m_LdIdeRoller.IsPositive()); // 11.03.02 minhan

                        bool Aligncheck = m_Aligner.IsPositive();

                        if (!simulation)
                        {
                            glassDetected = glassOutDetected;
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        foreach (CvUnit unit in ThreadCvControl.Units) // 09.09.22 minhan 
                        {
                            if (!m_Control.CheckUnitCondition(unit))
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unit Condition Check : NG");
                                Cv.IfFlag.InError = true;
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 4000;
                                break;
                            }
                        }

                        if (!Tiltingcheck) // 11.03.02 minhan
                        {
                            m_AlarmId = m_AlarmLdTiltingUP.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false; // 09.10.15 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Tilting UP Error");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 5000;
                            break;
                        }
                        if (!Aligncheck) // 11.03.02 minhan
                        {
                            m_AlarmId = m_AlarmAlignFw.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false; // 09.10.15 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Align FW Error");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 5000;
                            break;
                        }
                        if (glassDetected)
                        {
                            // Clear error condition
                            ClearErrorConditionFlags(); // 10.12.25 minhan 이거 왜 않하는데..

                            if (simulation)
                            {
                                Cv.GlsInSensor.SetState(false, Logic.AND);
                                Cv.GlsOutSensor.SetState(true, Logic.AND);
                            }
                            m_Timer2.Start(m_Tm2);
                            //nSeqNo = 150; 
                            nSeqNo = 145; // 09.09.22 minhan
                        }
                        else if (m_Timer1.Over)
                        {
                            if (simulation) // 11.02.09 minhan
                            {
                                Cv.GlsInSensor.SetState(false, Logic.AND);
                                Cv.GlsOutSensor.SetState(true, Logic.AND);
                                m_Timer2.Start(m_Tm2);
                                nSeqNo = 145;
                            }
                            else
                            {
                                m_AlarmId = m_AlarmOnTimeOut.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                Cv.IfFlag.InError = true;
                                Cv.IfFlag.RecoveryReq = false; // 09.10.15 minhan
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 145: // 09.09.22 minhan
                    {
                        bool glassDetected = true;
                        if (!simulation)
                        {
                            glassDetected = glassInDetected;
                        }
                        else
                        {
                            glassDetected = m_Timer2.Over;
                        }

                        if (!glassDetected) // 11.03.02 minhan
                        {
                            if (simulation)
                            {
                                Cv.GlsInSensor.SetState(false, Logic.AND);
                            }

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "IN Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 150;
                        }
                        else if (m_Timer2.Over)
                        {
                            bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                nSeqNo = 150;
                            }
                            else
                            {
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                Cv.IfFlag.InError = true;
                                Cv.IfFlag.RecoveryReq = false; // 09.10.15 minhan
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 150:
                    {
                        bool glassDetected = false;

                        if (!simulation)
                        {
                            glassDetected = glassOutDetected; // 11.03.02 minhan
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        if (!glassDetected) // 11.02.01 minhan 
                        {
                            // Clear error condition
                            //ClearErrorConditionFlags();
                            m_StartTicks = XFunc.GetTickCount();
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor OFF Confirm(1st)");
                            nSeqNo = 160;

                        }
                    }
                    break;
                case 160:
                    if (GetElapsedTicks() > 200)
                    {
                        if (!glassOutDetected || m_Simul.Device) // 11.03.02 minhan
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor OFF Confirm(2nd)");
                            nSeqNo = 180;
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor OFF Re-Check");
                            nSeqNo = 150;
                        }
                    }
                    break;
                case 180:
                    if (!Cv.MotorControl.IsFw(Logic.OR))
                    {
                        nSeqNo = 10;
                    }
                    break;

                case 1000: //Timeout Error --> Timer 돌리자
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        ClearErrorConditionFlags();
                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000: // 09.10.15 minhan
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        ClearErrorConditionFlags();
                        m_Timer1.Start(3000);

                        Cv.IfFlag.RecoveryReq = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;

                case 3000: // Gerneral Error 
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;

                case 4000: //Check Condtion Error
                    {
                        bool ok = true;
                        foreach (CvUnit unit in ThreadCvControl.Units)
                        {
                            ok &= m_Control.CheckUnitCondition(unit);
                        }

                        if (ok)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unit Condition Check - Recheck : OK");
                            ClearErrorConditionFlags(); // 09.09.22 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 5000: // Cylinder Error --> TickCount
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        if (m_LdTilting.IsPositive() && m_AlarmId == m_AlarmLdTiltingUP.Id)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery : Tilting Error");
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            ClearErrorConditionFlags(); // 11.01.27 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else if (m_Aligner.IsPositive() && m_AlarmId == m_AlarmAlignFw.Id)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery : Align Error");
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            ClearErrorConditionFlags(); // 11.01.27 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else if (m_LdIdeRoller.IsPositive() && m_AlarmId == m_AlarmLdIdeRollerFw.Id)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery : IdleRoller  Error");
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            ClearErrorConditionFlags(); // 11.01.27 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else if (m_LdTilting.IsNegative() && m_AlarmId == m_AlarmLdTiltingDW.Id)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery : Tilting Error");
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            ClearErrorConditionFlags(); // 11.01.27 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else if (m_LdIdeRoller.IsNegative() && m_AlarmId == m_AlarmLdIdeRollerBw.Id)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery : IdleRoller Error");
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            ClearErrorConditionFlags(); // 11.01.27 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else if (m_Aligner.IsNegative() && m_AlarmId == m_AlarmAlignBw.Id)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery : Align Error");
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            ClearErrorConditionFlags(); // 11.01.27 minhan
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
        #region Methods
        public int SeqTilting(int type)  //kang 강제로 시간 설정
        {

            int tiltingSeqNo = this.m_TiltingSeqNo;
            int ReturnVal = -1;

            switch (tiltingSeqNo)
            {
                case 0:
                    if (type == (int)TiltingCommand.ULTiming._TiltingUp)
                    {

                        m_LdTilting.SetAct(ActuatorAct.Pos);
                        Cv.SetLog(m_SeqFunName, 0, 0, "Tiling Up Start");

                        TiltingUpTimeoutAlarm = 5;
                        m_StartTicks = XFunc.GetTickCount();
                        TiltingRetryCnt++;
                        tiltingSeqNo = 10;
                    }
                    else
                    {
                        m_LdTilting.SetAct(ActuatorAct.Neg);
                        Cv.SetLog(m_SeqFunName, 0, 0, "Tiling Down Start");
                        TiltingDownTimeoutAlarm = 5;
                        m_StartTicks = XFunc.GetTickCount();
                        TiltingRetryCnt++;
                        tiltingSeqNo = 100;
                    }
                    break;
                case 10:
                    if (m_LdTilting.IsPositive())
                    {
                        //if(GetElapsedTicks() > (TiltingUpTimeoutAlarm * 0.9 * 1000))
                        //{
                        //    m_Eqp.SetAlarm(m_WarningTiltingUp.Id);
                        //}
                        //else m_Eqp.ResetAlarm(m_WarningTiltingUp.Id);
                        TiltingRetryCnt = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        tiltingSeqNo = 200;
                    }
                    else if (GetElapsedTicks() > TiltingUpTimeoutAlarm * 1000)
                    {
                        if (TiltingRetryCnt > 1)
                        {
                            TiltingRetryCnt = 0;
                            ReturnVal = 1;
                            tiltingSeqNo = 0;
                        }
                        else tiltingSeqNo = 0;
                    }
                    break;
                case 100:
                    if (m_LdTilting.IsNegative())
                    {
                        //if(GetElapsedTicks() > (TiltingDownTimeoutAlarm * 0.9 * 1000))
                        //{
                        //    m_Eqp.SetAlarm(m_WarningTiltingDown.Id);
                        //}
                        //else m_Eqp.ResetAlarm(m_WarningTiltingDown.Id);
                        TiltingRetryCnt = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        tiltingSeqNo = 200;
                    }
                    else if (GetElapsedTicks() > TiltingDownTimeoutAlarm * 1000)
                    {
                        if (TiltingRetryCnt > 1)
                        {
                            TiltingRetryCnt = 0;
                            ReturnVal = 1;
                            tiltingSeqNo = 0;
                        }
                    }
                    break;
                case 200:
                    if (GetElapsedTicks() > 500)
                    {
                        ReturnVal = 0;
                        tiltingSeqNo = 0;
                    }
                    break;
            }
            this.m_TiltingSeqNo = tiltingSeqNo;
            return ReturnVal;
        }
        #endregion
    }
    public class SeqUnitUlCv1 : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        private Simul m_Simul;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private ThreadCvBOE_G8_DHDC m_Control; // 11.02.01 minhan
        private GenInfoHandler m_GenInfo;
        private TagGlassData m_RecvGlassData = new TagGlassData();//2010.08.30 kimgun
        private FishHand m_UlHandUnit; // 11.02.01 minhan
        private bool simulation;
        private bool glassInDataExist;
        private bool glassOutDataExist;
        private bool glassInDetected;
        private bool glassOutDetected;
        private string m_Msg; // 11.02.01 minhan
        //private Alarm m_AlarmDecSensor; // 11.02.01 minhan
        #endregion

        #region Constructor
        public SeqUnitUlCv1(ThreadCvBOE_G8_DHDC control, CvUnit cv)
        {
            Cv = cv;
            m_Server = ServerManager.Instance;
            m_GenInfo = GenInfoHandler.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand;
            m_SeqFunName = Cv.GlsInSensor.Name;

            m_Timer1 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Timeout");
            m_Timer2 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Off Timeout");
            m_Timer3 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Time warning");

            m_AlarmOnTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmOffTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            //m_AlarmDecSensor = new Alarm(Cv.GlsInSensor.Name + "DEC Sensor Error", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.01 minhan
            m_GenInfo = m_GenInfo as GenInfoHandler;

            simulation = false;
            glassInDataExist = false;
            glassOutDataExist = false;
            glassInDetected = false;
            glassOutDetected = false;
            m_Msg = "";
        }
        #endregion

        #region Methods
        public void FlagSet()
        {
            simulation = m_Simul.Device;
            glassInDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(0));
            glassOutDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(1));
            glassInDetected = Cv.GlsInSensor.IsDetected(Logic.OR);
            glassOutDetected = Cv.GlsOutSensor.IsDetected(Logic.OR);
            m_Server.GlassData.GetData(Cv.DataMatchingKey(0), ref m_RecvGlassData);
            m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
            m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
        }

        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.InError)
            {
                Cv.IfFlag.InError = false;
                //Cv.IfFlag.OutError = false; // 11.06.08 minhan
                Cv.IfFlag.RecoveryReq = false;
            }
            //if (Cv.IfFlag.OutError) // 11.06.08 minhan
            //{
            //    Cv.IfFlag.OutError = false;
            //    Cv.IfFlag.RecoveryReq = false;
            //}
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp) return -1;
            if (Cv.Sequence[0] == null) Cv.Sequence[0] = this;

            m_Control.TimerControl(Cv, _GSS.ssIN, m_Timer1, m_Timer2, m_Timer3);

            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = this.m_SeqNo;

            FlagSet(); // 11.02.01 minhan

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        if (glassInDataExist)
                        {
                            Cv.PrevCv.IfFlag.OutReady = true;
                            Cv.IfFlag.InComp = false;

                            if (!glassOutDataExist)
                            {
                                GlobalVar.GlassOutComp1 = true;
                            }

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data exist and Sensor is detected(case0)");
                            nSeqNo = 10; // Align Forward and Check Next CV Cond.
                        }
                        else
                        {
                            if (!glassInDetected || !glassOutDataExist) // 11.06.08 minhan
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data is not exist and Sensor is not detected(Case0)");
                                nSeqNo = 10;
                            }
                            else if (glassInDetected)
                            {
                                nSeqNo = 500;
                            }

                        }
                    }
                    break;

                case 10:
                    {
                        if (Cv.PrevCv.IfFlag.OutReady &&
                            !Cv.PrevCv.IfFlag.OutError &&
                            glassInDetected)
                        {
                            Cv.PrevCv.IfFlag.OutReady = false;

                            m_Server.GlassData.Move(Cv.PrevCv.DataMatchingKey(1), Cv.DataMatchingKey(0));
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In Sensor is ON");
                            //if (!GlobalVar.AllGlsPosDataIng) // 11.02.01 minhan 제거
                            //{
                            //    GlobalVar.AllGlsPosDataReq = true;
                            //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Position Data Report");
                            //}
                            //Dual Sensor Check 2010.06.16 kimgun
                            //sensor가 감지 안 될 경우///////////////////////////////////////////////
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20: // 11.02.01 minhan
                    {
                        bool UlHandCheck = eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                        UlHandCheck &= (eqpCylinders._UL_Fish_Hand_Cylinder_OP.IsActStatus(ActuatorAct.Pos) || eqpCylinders._UL_Fish_Hand_Cylinder_MT.IsActStatus(ActuatorAct.Pos));

                        if (GlobalVar.GlassOutComp1 &&
                            !glassOutDataExist &&
                            !glassOutDetected &&
                            !UlHandCheck)
                        {
                            //bool CheckDecSensor = Cv.FwDecelSensor.IsDetected(); // 11.02.01 minhan 생각을 해봐도 다시 진행하는 상황일 경우 알람이 발생하므로 일단 삭제.

                            GlobalVar.GlassOutComp1 = false;

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unload Out No Glass Confirm");

                            // Timer Set
                            int distance = Cv.SetupDistance.GetValue<int>();
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, m_Server.JobCond.CvProcessSpeed(Cv.Id), ref m_Tm1, ref m_Tm2, ref m_Tm3);
                            int TimeOutMargin = m_Server.ULOutSensorOnTimeMargin.GetValue<int>() * 1000; // 11.02.01 minhan

                            m_Tm1 += TimeOutMargin; // 11.02.01 minhan 기본 0초로 하며 설정해서 사용하길.
                            m_Tm2 += TimeOutMargin;

                            m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.02.01 minhan
                            m_Timer1.Start(m_Tm1);
                            m_Timer2.Start(m_Tm2);
                            m_Timer3.Start(m_Tm3);

                            //if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.06.08 minhan
                            //{
                            //    Cv.BrokenDetectScanType.Reset(true);
                            //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                            //}
                            //else
                            //{
                            //    if (Cv.BrokenDetectScanType != null) Cv.BrokenDetectScanType.Reset(false);
                            //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Skip");
                            //}

                            //if (!GlobalVar.AllGlsPosDataIng)
                            //{
                            //    GlobalVar.AllGlsPosDataReq = true;
                            //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Position Data Report");
                            //}

                            nSeqNo = 30;
                            //}
                            //else
                            //{
                            //    AlarmId = m_AlarmDecSensor.Id;
                            //    m_Eqp.SetAlarm(AlarmId);
                            //    Cv.IfFlag.InError = true;
                            //    Cv.IfFlag.RecoveryReq = false;
                            //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DEC Sensor Error");
                            //    ReturnSeqNo = nSeqNo;
                            //    nSeqNo = 2000;
                            //}
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unloading TimeOut : Set");

                            GlobalVar.UlTimeOut = true;
                            GlobalVar.UlcvStop = true; // 11.06.08 minhan
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 30:
                    {
                        if (simulation)
                        {
                            glassInDetected = !m_Timer2.Over;
                        }

                        if (!glassInDetected)
                        {
                            if (simulation)
                            {
                                Cv.GlsInSensor.SetState(false);
                            }

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags(); // 09.06.11 minhan
                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 40;
                        }
                        else if (glassOutDetected) // kimx 검토해봐.알람아이디도 새로 생각해보고09.10.20 minhan out sensor 가 감지 되면 알람 특정한 알람을 찾지 못함.
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false; // 09.10.20 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor On Timeout Error");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                        else if (m_Timer2.Over) // 09.06.24 minhan
                        {
                            bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 40;
                            }
                            else
                            {
                                Cv.IfFlag.InError = true;
                                Cv.IfFlag.RecoveryReq = false; // 09.06.11 minhan
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_Eqp.SetAlarm(m_AlarmId);

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                        else if (!m_Control.CheckUnitCondition(Cv)) // 11.03.07 minhan
                        {
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                        }
                    }
                    break;
                case 40:
                    if (glassOutDetected)// 09.09.17 minhan
                    {
                        Cv.IfFlag.InComp = true;
                        Cv.IfFlag.InReady = true;
                        Cv.IfFlag.InError = false; // 20130116 wang
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor On");
                        nSeqNo = 10;
                    }
                    else if (m_Timer1.Over)
                    {
                        if (simulation)
                        {
                            Cv.IfFlag.InComp = true;
                            Cv.IfFlag.InReady = true;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor On");
                            Cv.GlsInSensor.SetState(false);
                            Cv.GlsOutSensor.SetState(true);
                            nSeqNo = 10;
                        }
                        else
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false; // 09.10.20 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor On Timeout Error");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 100: // 11.02.01 minhan
                    {
                        bool UlHandCheck = eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                        UlHandCheck &= (eqpCylinders._UL_Fish_Hand_Cylinder_OP.IsActStatus(ActuatorAct.Pos) || eqpCylinders._UL_Fish_Hand_Cylinder_MT.IsActStatus(ActuatorAct.Pos));

                        if (GlobalVar.GlassOutComp1 &&
                            !glassOutDataExist &&
                            !glassOutDetected &&
                            !UlHandCheck) // 10.01.07 minhan
                        {
                            GlobalVar.UlTimeOut = false;
                            GlobalVar.UlcvStop = false; // 11.06.08 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unloading Timeout Release");
                            nSeqNo = 20;
                        }
                        else // 11.06.08 minhan
                        {
                            if (!GlobalVar.UlTimeOut) GlobalVar.UlTimeOut = true;
                            if (!GlobalVar.UlcvStop) GlobalVar.UlcvStop = true;
                        }
                    }
                    break;
                case 500:
                    {
                        m_Timer2.Start(m_Tm2);
                        nSeqNo = 30;
                    }
                    break;
                case 1000: //Timeout Error --> Timer 돌리자
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        //ClearErrorConditionFlags(); // 11.06.08 minhan
                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000: // 11.02.01 minhan
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        ClearErrorConditionFlags();

                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000: // 11.03.07 minhan
                    if (m_Control.CheckUnitCondition(Cv))
                    {
                        Cv.IfFlag.InError = false;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqUnitUlCv2 : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        private FishHand m_UlHandUnit;
        private Simul m_Simul;
        private CvUnit Cv;
        //private XTimer m_Timer1; // 11.02.01 minhan
        //private XTimer m_Timer2;
        //private XTimer m_Timer3;
        //private Alarm m_AlarmOnTimeOut;
        //private Alarm m_AlarmOffTimeOut;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        //private Alarm m_AlarmRbtInterlock; // 11.02.01 minhan 제거
        private TagGlassData m_RecvGlassData = new TagGlassData();
        //private DualGlsSensor m_DualOutSensor;
        private ThreadCvBOE_G8_DHDC m_Control; // 11.02.01 minhan
        //private float fSkipDelayTime; // 11.02.01 minhan
        private bool glassInDataExist;
        private bool glassOutDataExist;
        private bool glassInDetected;
        private bool glassOutDetected;
        private bool simulation;
        private Alarm m_AlarmDecSensorOn; // 11.02.01 minhan
        private Alarm m_AlarmDecSensorOff; // 11.02.01 minhan
        #endregion

        #region Constructor
        public SeqUnitUlCv2(ThreadCvBOE_G8_DHDC control, CvUnit cv)
        {
            Cv = cv;
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand;
            m_SeqFunName = Cv.GlsOutSensor.Name;
            //m_DualOutSensor = (Cv.GlsOutSensor as DualGlsSensor); // 11.02.01 minhan
            //m_Timer1 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Timeout");
            //m_Timer2 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Off Timeout");
            //m_Timer3 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Time warning");

            //m_AlarmOnTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.01 minhan
            //m_AlarmOffTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            //m_AlarmRbtInterlock = new Alarm(Cv.Name + " Robot Hand Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.01 minhan
            m_AlarmDecSensorOn = new Alarm(Cv.GlsInSensor.Name + "DEC Sensor On Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.01 minhan
            m_AlarmDecSensorOff = new Alarm(Cv.GlsInSensor.Name + "DEC Sensor Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.01 minhan
            glassInDataExist = false;
            glassOutDataExist = false;
            glassInDetected = false;
            glassOutDetected = false;
            simulation = false;
        }
        #endregion

        #region Methods
        public void FlagSet()
        {
            simulation = m_Simul.Device;
            m_PortNo = 0;
            m_SlotNo = 0;
            //2010.08.30 kimgun
            m_Server.GlassData.GetData(Cv.DataMatchingKey(1), ref m_RecvGlassData);
            m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
            m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            glassInDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(0));
            glassOutDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(1));
            glassInDetected = Cv.GlsInSensor.IsDetected(Logic.OR);
            glassOutDetected = Cv.GlsOutSensor.IsDetected(Logic.OR);
            //fSkipDelayTime = m_Server.DualSkipDelayTime.GetValue<float>();//2006.06.16 kimgun
        }

        private void ClearErrorConditionFlags() // 11.02.01 minhan
        {
            if (Cv.IfFlag.OutError)
            {
                Cv.IfFlag.OutError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;
            if (Cv.Sequence[1] == null) Cv.Sequence[1] = this;

            //m_Control.TimerControl(Cv, _GSS.ssOUT, m_Timer1, m_Timer2, m_Timer3); // 11.02.01 minhan
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = this.m_SeqNo;

            FlagSet(); // 11.02.01 minhan

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        if (glassOutDataExist)
                        {
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.OutComp = false;
                            GlobalVar.GlassOutComp1 = false;

                            if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.06.08 minhan
                            {
                                Cv.BrokenDetectScanType.Reset(false);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data exist and Sensor is detected(case0)");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 5;
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : No use");
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data exist and Sensor is detected(case0)");
                                nSeqNo = 10; // Align Forward and Check Next CV Cond.
                            }
                        }
                        else
                        {
                            if (glassOutDetected && !glassInDataExist) // 11.06.08 minhan
                            {
                                Cv.IfFlag.OutComp = false;
                                Cv.IfFlag.OutReady = true; // 11.02.01 minhan
                                GlobalVar.GlassOutComp1 = false;

                                //if (simulation)   //  wzy 13.06.03
                                //{
                                //    Cv.FwDecelSensor.DiSensor.SetState(true);
                                //}

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass In,Out is not exist and Sensor is detected(case0)");
                                nSeqNo = 500;
                            }
                            else
                            {
                                if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.06.08 minhan
                                {
                                    Cv.BrokenDetectScanType.Reset(false);
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data is not exist and Sensor is not detected(Case0)");
                                    m_StartTicks = XFunc.GetTickCount();
                                    nSeqNo = 5;
                                }
                                else
                                {
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : No use");
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Data is not exist and Sensor is not detected(Case0)");
                                    nSeqNo = 10; // Align Forward and Check Next CV Cond.
                                }
                            }
                        }
                    }
                    break;
                case 5: // 11.06.08 minhan
                    {
                        if (GetElapsedTicks() > 300)
                        {
                            if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use))
                            {
                                Cv.BrokenDetectScanType.Reset(true);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Start");
                            }

                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if ((Cv.BrokenDetectScanType != null) &&
                           (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.06.08 minhan
                        {
                            Cv.BrokenDetectScanType.Reset(true);
                        }

                        if (Cv.IfFlag.InReady &&
                            //!glassInDetected && //--> InSensor OFF 조건땜시 cleanout 안되는 경우가 있삼. 나중에 생각해보자//LeeChungWon : 이 조건으로 gls 정체시 seq 정지
                            glassOutDetected)
                        {
                            Cv.IfFlag.InReady = false;
                            Cv.IfFlag.OutComp = false;

                            m_Server.GlassData.Move(Cv.DataMatchingKey(0), Cv.DataMatchingKey(1));
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor is ON");

                            ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), Cv.Name); // 2010.06.25 kimgun

                            if (simulation)
                            {
                                //Cv.FwDecelSensor.DiSensor.SetState(true);//  wzy 13.06.03
                                m_StartTicks = XFunc.GetTickCount();
                            }

                            //m_StartTicks = XFunc.GetTickCount(); // 11.02.01 minhan
                            nSeqNo = 15; // 11.06.08 minhan
                        }
                    }
                    break;
                case 15: // 11.06.08 minhan
                    {
                        if ((Cv.BrokenDetectScanType == null) ||
                           (!Cv.BrokenDetectScanType.SetupBrokenInterlock.Use) ||
                           (!Cv.BrokenDetectScanType.IsError()))
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : No Error");
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Error");
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 20: // 11.02.01 minhan 
                    {
                        bool CheckDecSensorOn = Cv.GlsOutSensor.IsDetected();//  wzy 13.06.03

                        if (CheckDecSensorOn)
                        {
                            Cv.IfFlag.OutComp = false;
                            Cv.IfFlag.OutReady = true; // 11.02.01 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "CV STOP");
                            nSeqNo = 40; // 11.02.01 minhan
                        }
                        else
                        {
                            if (!simulation)
                            {
                                m_AlarmId = m_AlarmDecSensorOn.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false;
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DEC Sensor On Error");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                            else if (GetElapsedTicks() > 2000)
                            {
                                m_AlarmId = m_AlarmDecSensorOn.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false;
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DEC Sensor On Error");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 40:
                    {
                        if (m_UlHandUnit.IfFlag.InComp &&
                           //Cv.GlsOutSensor.IsDetected() &&  --> UL Fish가 들어올렸을때 glass 감지 센서가 감지되는 경우땜시..--> teaching point를 조금 높이면 됨. 글라스감지유무는 확인해야함.
                           !Cv.GlsOutSensor.IsDetected(Logic.AND) &&
                           !glassOutDataExist)
                        {
                            Cv.IfFlag.OutReady = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass not detected");
                            nSeqNo = 50;
                        }
                    }
                    break;
                case 50:
                    if (!m_UlHandUnit.IfFlag.InComp)
                    {
                        GlobalVar.GlassOutComp1 = true;
                        Cv.IfFlag.OutReady = false;
                        Cv.IfFlag.OutComp = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass out complete");

                        if (simulation)
                        {
                            //Cv.FwDecelSensor.DiSensor.SetState(false);//  wzy 13.06.03
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        nSeqNo = 60; // 11.02.01 minhan
                    }
                    break;
                case 60: // 11.02.01 minhan
                    {
                        bool CheckDecSensorOff = Cv.GlsOutSensor.IsDetected();//  wzy 13.06.03

                        if (!CheckDecSensorOff)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DEC Sensor Off");

                            if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.06.08 minhan
                            {
                                Cv.BrokenDetectScanType.Reset(false);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 5;
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : No use");
                                nSeqNo = 10;
                            }

                        }
                        else
                        {
                            if (!simulation)
                            {
                                m_AlarmId = m_AlarmDecSensorOff.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false;
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DEC Sensor Off Error");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                            else if (GetElapsedTicks() > 2000)
                            {
                                m_AlarmId = m_AlarmDecSensorOff.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false;
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DEC Sensor Off Error");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 100: // 11.06.08 minhan
                    if (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use && Cv.BrokenDetectScanType.IsError())
                    {
                        Cv.IfFlag.OutError = true;
                        Cv.IfFlag.RecoveryReq = false;
                        m_AlarmId = Cv.BrokenDetectScanType.ALM_BrokenDetect.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Set Alarm");
                        m_ReturnSeqNo = 15;
                        nSeqNo = 2000;
                    }
                    else
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : No Alarm");
                        nSeqNo = 15;
                    }
                    break;
                case 500:
                    {
                        nSeqNo = 40;
                    }
                    break;
                case 1000: // 11.02.01 minhan
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        ClearErrorConditionFlags();

                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000: // 11.06.08 minhan
                    {
                        if (Cv.IfFlag.RecoveryReq) Cv.IfFlag.RecoveryReq = false;// 11.06.08 minhan

                        if (m_Eqp.AlarmResetSwitchPushed)
                        {
                            if (!GlobalVar.MsgRequest)
                            {
                                string Msg = "ULBroken";
                                m_Control.m_MsgQueue.EnqueueRecvMsg(Msg);
                                GlobalVar.MsgRequest = true;
                                GlobalVar.MsgULSkip = false;
                                GlobalVar.MsgULBrokenComp = false;
                            }
                        }
                        else if (GlobalVar.MsgULBrokenComp)
                        {
                            if (GlobalVar.MsgULSkip)
                            {
                                Cv.BrokenDetectScanType.Reset(false);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Reset");
                                nSeqNo = 2010;
                            }

                            GlobalVar.MsgULSkip = false;
                            GlobalVar.MsgULBrokenComp = false;
                            GlobalVar.MsgRequest = false;
                        }
                    }
                    break;
                case 2010: // 11.06.08 minhan
                    if (!Cv.BrokenDetectScanType.IsError())
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Reset Alarm");
                        ClearErrorConditionFlags();
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCvManualControlInterlock : XSeqFunction
    {
        #region Fields
        private IServerManager m_Server = null;
        private GenInfoHandler m_GenInfo = null;
        private _GenericCollection<CvUnit> m_CvUnits = null;
        private CvUnit m_LdCvUnit = null;
        private CvUnit m_EuvCvUnit = null;
        private CvUnit m_UlCvUnit = null;
        private ActuatorUnit m_LdAlign = null;
        private ActuatorUnit m_LdTilting = null;//zhangliang 
        private ActuatorUnit m_LdIdleRoller = null;
        //private FishHand m_LdHandUnit = null;
        private FishHand m_UlHandUnit = null;
        //private Sensor m_LdSend1PositionSensor = null;
        private Sensor m_UlRecv2PositionSensor = null;
        private ThreadCvControl m_Control;
        #endregion

        #region Constructor
        public SeqCvManualControlInterlock(ThreadCvControl control, _GenericCollection<CvUnit> units)
        {
            m_CvUnits = units;
            m_Server = ServerManager.Instance;
            m_GenInfo = GenInfoHandler.Instance;
            m_Control = control;

            m_LdCvUnit = eqpTransferUnits._LD_CvUnit;
            m_EuvCvUnit = eqpTransferUnits._EUV_CvUnit;
            m_UlCvUnit = eqpTransferUnits._UL_CvUnit;
            m_LdTilting = eqpActuatorUnits._LD_Tilting_Unit;//zhangliang
            m_LdAlign = eqpActuatorUnits._LD_Align;
            m_LdIdleRoller = eqpActuatorUnits._LD_Idle_Roller;
            // m_LdHandUnit = eqpTransferUnits._LD_Fish_Hand;
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand;
            //m_LdSend1PositionSensor = eqpSensors._LD_Hand_Unit_Send1_Position_Sensor;
            m_UlRecv2PositionSensor = eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (!m_GenInfo.AutoMode)
                    {
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_GenInfo.AutoMode)
                        {
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 1000) // 11.02.01 minhan
                        {
                            // LD Unit에서 Glass In 감지된 상태에서 후진 하려고 할때
                            bool stopCond = false;
                            //ld
                            stopCond |= (m_LdCvUnit.GlsInSensor.IsDetected() &&
                                         m_LdCvUnit.MotorControl.IsBw(Logic.OR));

                            stopCond |= (m_LdCvUnit.GlsOutSensor.IsDetected() &&
                                         m_LdCvUnit.MotorControl.IsFw(Logic.OR) &&
                                         !m_LdTilting.IsPositive());
                            //ap
                            stopCond |= (m_EuvCvUnit.MotorControl.IsBw(Logic.OR) &&
                                       m_EuvCvUnit.GlsInSensor.IsDetected(Logic.OR) &&
                                       !m_LdTilting.IsPositive());
                            stopCond |= (m_UlCvUnit.GlsOutSensor.IsDetected() && m_UlCvUnit.MotorControl.IsFw(Logic.OR));
                            stopCond |= ((((short)m_UlHandUnit.Servo.GetCurPointId() == eqpPos_UL_Hand_Servo_Unit.Recv2) ||
                                         m_UlRecv2PositionSensor.IsDetected() || ((short)m_UlHandUnit.Servo.GetCurPointId() == -1)) &&
                                        ((m_UlHandUnit.FrontRib.GetCurAct() == ActuatorAct.Pos) ||
                                        (m_UlHandUnit.RearRib.GetCurAct() == ActuatorAct.Pos)));

                            //stopCond |= ((!m_LdCvUnit.MotorControl.IsStop(Logic.AND)) && (m_LdAlign.IsNegative()));
                            //stopCond |= (m_LdTilting.IsNegative() && !m_LdTilting.IsPositive()) || (!m_LdTilting.IsNegative() && !m_LdTilting.IsPositive());
                            //stopCond |= (m_LdIdleRoller.IsNegative() && !m_LdIdleRoller.IsPositive()) || (!m_LdIdleRoller.IsNegative() && !m_LdIdleRoller.IsPositive());
                            if (stopCond)
                            {
                                int unitCount = m_CvUnits.Count;
                                CvUnit unit;
                                for (int i = 0; i < unitCount; i++)
                                {
                                    unit = m_CvUnits[i];
                                    int motorCount = unit.Motors.Count;
                                    for (int j = 0; j < motorCount; j++)
                                    {
                                        unit.ManualAct[j] = CvMotorAct.Stop;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqUnitActuatorInterlock : XSeqFunction
    {
        #region Fields
        private ActuatorUnit m_ActuatorUnit = null;
        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server;
        private ThreadCvBOE_G8_DHDC m_Control;
        private Simul m_Simul;
        private GenInfoHandler m_GenInfo;
        #endregion

        #region Constructor
        public SeqUnitActuatorInterlock(ThreadCvBOE_G8_DHDC control, ActuatorUnit actuator)
        {
            m_ActuatorUnit = actuator;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_SeqFunName = m_ActuatorUnit.Name;
            m_GenInfo = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfo.EqpInitComp) return -1; // 11.02.25 minhan

            int nSeqNo = this.m_SeqNo;
            int Rv = -1;

            bool Interlock = true;
            Interlock &= eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected();

            switch (nSeqNo)
            {
                case 0:
                    if (!m_GenInfo.AutoMode && m_ActuatorUnit.GetCurAct() != ActuatorAct.Stop) // 11.02.25 minhan
                    {
                        if (Interlock)
                        {
                            m_ActuatorUnit.SetStopAct();
                            m_AlarmId = m_Control.AlarmLdRobotInterlock.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Moving Interlock Status");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            nSeqNo = 1000; // 11.02.25 minhan
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed && !Interlock)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Interlock Status Error Recovery(1000)");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return Rv;
        }
        #endregion
    }

    public class SeqDualInSensor : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static GenInfoHandler m_GenInfos;
        protected static ThreadCvBOE_G8_DHDC m_Control; // 11.02.01 minhan
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private Alarm m_InOPWarning;
        private Alarm m_InOOPWarning;
        private Alarm m_OutOPWarning;
        private Alarm m_OutOOPWarning;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private bool m_Tm3Warning;
        //private bool m_DualCheckSkip; //2009.07.14 kimgun 11.02.01 minhan
        private bool glassDetected;
        private TagGlassData m_RecvGlassData = new TagGlassData();
        private DualGlsSensor m_DualInSensor;
        private DualGlsSensor m_DualOutSensor;
        private float fSkipDelayTime; // 11.02.01 minhan
        private bool simulation;
        private string m_Msg; // 11.02.01 minhan
        #endregion

        #region Constructor
        public SeqDualInSensor(ThreadCvBOE_G8_DHDC control, CvUnit cv)
        {
            Cv = cv;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_DualInSensor = (Cv.GlsInSensor as DualGlsSensor);
            m_DualOutSensor = (Cv.GlsOutSensor as DualGlsSensor);

            m_SeqFunName = Cv.GlsInSensor.Name;

            m_Timer1 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Timeout");
            m_Timer2 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Off Timeout");
            m_Timer3 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Time warning");

            m_AlarmOnTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmOffTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_OutOPWarning = new Alarm(Cv.GlsOutSensor.Name + " : OP Out Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.02.01 minhan
            m_OutOOPWarning = new Alarm(Cv.GlsOutSensor.Name + " : OOP Out Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_InOPWarning = new Alarm(Cv.GlsInSensor.Name + " : OP In Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_InOOPWarning = new Alarm(Cv.GlsInSensor.Name + " : OOP In Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);

            fSkipDelayTime = 0;
            simulation = false;
            m_Msg = "";
            //m_DualCheckSkip = false; // 11.02.01 minhan
            glassDetected = false;
        }
        #endregion

        #region Methods
        public void FlagSet() // 11.02.01 minhan
        {
            simulation = m_Simul.Device;
            fSkipDelayTime = m_Server.DualSkipDelayTime.GetValue<float>();//2006.06.16 kimgun
            //2010.08.30 kimgun
            if (m_Server.GlassData.GetData(Cv.DataMatchingKey(0), ref m_RecvGlassData)) // 11.03.02 minhan
            {
                m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
                m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            if ((m_AlarmId == m_OutOOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_OutOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_InOOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_InOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
        }

        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.InError)
            {
                Cv.IfFlag.InError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            if (Cv.Sequence[0] == null) Cv.Sequence[0] = this;

            m_Control.TimerControl(Cv, _GSS.ssIN, m_Timer1, m_Timer2, m_Timer3);
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = m_SeqNo;

            FlagSet(); // 11.02.01 minhan

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        bool glassDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(0));
                        bool glassDataExistPrev = m_Server.GlassData.IsExist(Cv.PrevCv.DataMatchingKey(1));//2009.07.14 kimgun

                        if (!glassDataExist)
                        {
                            if (!Cv.GlsInSensor.IsDetected(Logic.OR) || glassDataExistPrev)//2009.07.14 kimgun
                            {
                                if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.05.17 minhan
                                {
                                    Cv.BrokenDetectScanType.Reset(false);
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                                    m_StartTicks = XFunc.GetTickCount();
                                    nSeqNo = 1;
                                }
                                else
                                {
                                    nSeqNo = 2; // 11.05.17 minhan
                                }
                            }
                            else if (Cv.GlsInSensor.IsDetected(Logic.OR))
                            {
                                if ((Cv.BrokenDetectScanType != null) &&
                                   (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.06.08 minhan
                                {
                                    Cv.BrokenDetectScanType.Reset(true);
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Start");
                                }

                                Cv.IfFlag.InComp = false;
                                nSeqNo = 500;
                            }
                        }
                        else
                        {
                            if (Cv.GlsInSensor.IsDetected(Logic.OR))
                            {
                                Cv.IfFlag.InComp = false;

                                if (Cv.PrevCv != null)
                                {
                                    Cv.PrevCv.IfFlag.OutReady = true;
                                    if (simulation)
                                    {
                                        //Cv.PrevCv.GlsOutSensor.DiSensor.SetState(true);
                                        Cv.PrevCv.GlsOutSensor.SetState(true, Logic.AND);
                                        Cv.PrevCv.Sequence[1].InitSeq();
                                    }
                                }
                                //if (Cv.GlsOutSensor.IsDetected(Logic.OR)) // 11.02.01 minhan
                                //{
                                //    m_DualCheckSkip = true;//2009.07.14 kimgun
                                //}

                                if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.05.17 minhan
                                {
                                    Cv.BrokenDetectScanType.Reset(false);
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                                    m_StartTicks = XFunc.GetTickCount();
                                    nSeqNo = 1;
                                }
                                else
                                {
                                    nSeqNo = 2; // 11.05.17 minhan
                                }
                            }
                            else if (Cv.GlsOutSensor.IsDetected(Logic.OR))
                            {
                                // Glass Data Move
                                if (Cv.PrevCv == null)
                                {
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                                }
                                else
                                {
                                    Cv.PrevCv.IfFlag.OutReady = false;

                                    m_Server.GlassData.Move(Cv.PrevCv.DataMatchingKey(1), Cv.DataMatchingKey(0));
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                                }

                                // TODO : Glass animation control

                                // Timer Set
                                int distance = Cv.SetupDistance.GetValue<int>();
                                int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                                m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                                m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.02.01 minhan
                                m_Timer1.Start(m_Tm1);
                                m_Timer2.Start(m_Tm2);
                                m_Timer3.Start(m_Tm3);

                                m_Tm3Warning = false;
                                //  m_DualCheckSkip = true;//2009.07.14 kimgun
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor is detected(go case 20)");
                                nSeqNo = 20;
                            }
                            else
                            {
                                // Timer Set
                                int distance = Cv.SetupDistance.GetValue<int>();
                                int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                                m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                                m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.02.01 minhan
                                m_Timer1.Start(m_Tm1);
                                m_Timer2.Start(m_Tm2);
                                m_Timer3.Start(m_Tm3);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In/Out Sensor is no detected(go case 30)");
                                Cv.IfFlag.OutLogicalCheckIgnore = true;
                                nSeqNo = 30;
                            }
                        }
                    }
                    break;
                case 1: // 11.05.17 minhan
                    {
                        if (GetElapsedTicks() > 300)
                        {
                            if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use))
                            {
                                Cv.BrokenDetectScanType.Reset(true);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Start");
                            }

                            nSeqNo = 2;
                        }
                    }
                    break;
                case 2: // 11.05.17 minhan
                    {
                        // set flag and sensor condtion
                        bool prevOutReady;
                        bool prevOutError;
                        bool glassExist;

                        if (Cv.PrevCv == null)
                        {
                            prevOutReady = true;
                            prevOutError = false;
                        }
                        else
                        {
                            prevOutReady = Cv.PrevCv.IfFlag.OutReady;
                            prevOutError = Cv.PrevCv.IfFlag.OutError;
                        }

                        if ((Cv.BrokenDetectScanType != null) &&
                            (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.05.17 minhan
                        {
                            Cv.BrokenDetectScanType.Reset(true);
                            //Cv.SetLog(SeqFunName, m_PortNo, m_SlotNo, "Broken Scan : Start");
                        }

                        glassExist = Cv.GlsInSensor.IsDetected(Logic.OR);

                        // check flag and sensor condtion
                        if (glassExist && prevOutReady && !prevOutError)
                        {
                            // Glass Data Move
                            if (Cv.PrevCv == null)
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                            }
                            else
                            {
                                Cv.PrevCv.IfFlag.OutReady = false;

                                m_Server.GlassData.Move(Cv.PrevCv.DataMatchingKey(1), Cv.DataMatchingKey(0));


                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                            }

                            // TODO : Glass animation control
                            //if (!GlobalVar.AllGlsPosDataIng) // 11.02.01 minhan
                            //{
                            //    GlobalVar.AllGlsPosDataReq = true;
                            //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Position Data Report");
                            //}

                            // Timer Set
                            int distance = Cv.SetupDistance.GetValue<int>();
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                            m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.02.01 minhan
                            m_Timer1.Start(m_Tm1);
                            m_Timer2.Start(m_Tm2);
                            m_Timer3.Start(m_Tm3);

                            m_Tm3Warning = false;
                            //Dual Sensor Check 2010.06.16 kimgun
                            //sensor가 감지 안 될 경우///////////////////////////////////////////////
                            if ((m_DualInSensor != null) && m_DualInSensor.UseOp &&
                                m_DualInSensor.UseOop)
                            {
                                if (m_DualInSensor.IsOpDetected() &&
                                    !m_DualInSensor.IsOopDetected()) nSeqNo = 5;//Op Sensor On
                                else if (m_DualInSensor.IsOopDetected() &&
                                    !m_DualInSensor.IsOpDetected()) nSeqNo = 8;//Oop Sensor On
                                else nSeqNo = 10;
                            }
                            else nSeqNo = 10;
                            m_StartTicks = XFunc.GetTickCount();
                            //////////////////////////////////////////////////////////////////////////
                        }
                    }
                    break;
                case 5:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        !m_DualInSensor.IsOopDetected())
                    {
                        m_DualInSensor.UseOop = false;
                        m_AlarmId = m_InOOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualInSensor Warning Oop");
                        nSeqNo = 10;
                    }
                    else if (m_DualInSensor.IsOopDetected())
                    {
                        m_Msg = string.Format("{0} SUB Sensor ON[5]", Cv.GlsInSensor.Name); // 11.02.01 minhan
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 10;
                    }
                    break;

                case 8:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        !m_DualInSensor.IsOpDetected())
                    {
                        m_DualInSensor.UseOp = false;
                        m_AlarmId = m_InOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualInSensor Warning Op");
                        nSeqNo = 10;
                    }
                    else if (m_DualInSensor.IsOpDetected())
                    {
                        m_Msg = string.Format("{0} Main Sensor ON[8]", Cv.GlsInSensor.Name); // 11.02.01 minhan
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 10;
                    }
                    break;
                //==>2010.04.15 kimgun
                case 10:
                    //다음 sensor가 감지 되어 있을 경우 //////////////////////////////////////
                    if ((m_DualOutSensor != null) &&
                        m_DualOutSensor.UseOp && m_DualOutSensor.UseOop)
                    {
                        if (Cv.GlsOutSensor.IsDetected(Logic.AND) || !Cv.GlsOutSensor.IsDetected(Logic.OR))//두개 다 감지되던지 두개 다 감지되지 않으면 정상으로 판단.
                            nSeqNo = 20;
                        else
                        {
                            if (m_DualOutSensor.IsOpDetected() &&
                                !m_DualOutSensor.IsOopDetected()) nSeqNo = 11;//Main Sensor ON
                            else if (!m_DualOutSensor.IsOpDetected() &&
                                m_DualOutSensor.IsOopDetected()) nSeqNo = 13;//Sub Sensor ON
                            m_StartTicks = XFunc.GetTickCount(); // 11.03.02 minhan
                        }
                    }
                    else nSeqNo = 20;
                    //////////////////////////////////////////////////////////////////////////
                    break;
                case 11:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualOutSensor.IsOpDetected())
                    {
                        m_DualOutSensor.UseOp = false;
                        m_AlarmId = m_OutOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualOutSensor Warning Op");
                        nSeqNo = 20;
                    }
                    else if (!m_DualOutSensor.IsOpDetected())
                    {
                        m_Msg = string.Format("{0}  MAIN Sensor OFF[11]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 20;
                    }
                    break;
                case 13:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualOutSensor.IsOopDetected())
                    {
                        m_DualOutSensor.UseOop = false;
                        m_AlarmId = m_OutOOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualOutSensor Warning Oop");
                        nSeqNo = 20;
                    }
                    else if (!m_DualOutSensor.IsOopDetected())
                    {
                        m_Msg = string.Format("{0}  MAIN Sensor OFF[13]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 20;
                    }
                    break;

                //<==2010.04.15 kimgun
                case 20:
                    {
                        // Check the out complete
                        if (Cv.IfFlag.OutComp)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Complete OK");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;

                        }
                        else if (m_Timer3.Over)
                        {
                            Cv.IfFlag.InLogicalCheckIgnore = true;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Complete NG");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        if (!simulation)
                        {
                            glassDetected = Cv.GlsOutSensor.IsDetected(Logic.OR);
                        }
                        else
                        {
                            if (!glassDetected)
                            {
                                glassDetected = m_Timer1.Over;
                            }
                        }

                        if (glassDetected && (m_Timer3.Over || Cv.IfFlag.InLogicalCheckIgnore))
                        {
                            if (simulation)
                            {
                                //Cv.GlsOutSensor.DiSensor.SetState(true);
                                Cv.GlsOutSensor.SetState(true, Logic.AND);
                                glassDetected = false;
                            }

                            // Flag set / reset
                            Cv.IfFlag.OutComp = false;
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.InLogicalCheckIgnore = false;

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor is ON");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 100;
                        }
                        //else if (glassDetected && !m_Tm3Warning)//2009.08.15 kimgun
                        //{
                        //    Cv.IfFlag.OutComp = false;

                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor is NG(1st)");
                        //    //StartTime = DateTime.Now;
                        //    m_StartTicks = XFunc.GetTickCount();

                        //    ReturnSeqNo = nSeqNo;
                        //    nSeqNo = 40;
                        //}
                        else if (m_Timer1.Over && !glassDetected)//2010.09.07 kimgun Timer1 alarm은 sensor가 on되지 않았을 때만 C++참조// || eqpTransferUnits._AK_CvUnit.Id == Cv.Id)
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false; // 09.10.26 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                        else if (!m_Control.CheckUnitCondition(Cv)) // 09.12.08 minhan
                        {
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false; // 11.02.01 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                        }
                        else if ((Cv.BrokenDetectScanType != null) &&
                                  Cv.BrokenDetectScanType.SetupBrokenInterlock.Use &&
                                  Cv.BrokenDetectScanType.IsError()) // 11.05.17 minhan
                        {
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_AlarmId = Cv.BrokenDetectScanType.ALM_BrokenDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Set Alarm");
                            m_ReturnSeqNo = 30;
                            nSeqNo = 4000;
                        }
                    }
                    break;

                #region 제거
                //case 40:
                //    if (GetElapsedTicks() > 500)
                //    {
                //        //if (Cv.GlsOutSensor.IsDetected() && !m_Tm3Warning)
                //        if (Cv.GlsOutSensor.IsDetected(Logic.OR) && !m_Tm3Warning)
                //        {
                //            m_Tm3Warning = true;
                //            //   Cv.IfFlag.OutSick = true;2009.08.15 kimgun
                //            m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor is NG(2nd)");
                //            nSeqNo = ReturnSeqNo;
                //        }
                //        else
                //        {
                //            nSeqNo = ReturnSeqNo;
                //        }
                //    }
                //    //else if (!Cv.GlsOutSensor.IsDetected())
                //    else if (!Cv.GlsOutSensor.IsDetected(Logic.AND))
                //    {
                //        m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor is Off"); //2009.08.15 kimgun
                //        nSeqNo = ReturnSeqNo;
                //    }
                //    break;
                #endregion

                case 100:
                    {
                        // bool glassDetected = false;
                        if (!simulation)
                        {
                            glassDetected = Cv.GlsInSensor.IsDetected(Logic.AND);
                        }
                        else
                        {
                            glassDetected = !(m_Timer2.Over || !Cv.GlsInSensor.IsDetected(Logic.AND));
                        }

                        if (!glassDetected)
                        {
                            if (simulation)
                            {
                                //Cv.GlsInSensor.DiSensor.SetState(false);
                                Cv.GlsInSensor.SetState(false, Logic.AND);
                            }

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            m_StartTicks = XFunc.GetTickCount();
                            if ((m_DualInSensor != null) &&
                                (!m_DualInSensor.UseOp && !m_DualInSensor.UseOop))
                            {
                                if (!m_DualInSensor.IsOpDetected() &&
                                    !m_DualInSensor.UseOop) nSeqNo = 101;
                                else if (!m_DualInSensor.IsOopDetected() &&
                                    !m_DualInSensor.UseOp) nSeqNo = 102;
                                else nSeqNo = 110;
                            }
                            else nSeqNo = 110;
                        }
                        else if (m_Timer2.Over && glassDetected)//2010.09.07 kimgun Timer2 alarm은 sensor가 off되지 않았을 때만 C++참조
                        {
                            bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();

                                nSeqNo = 110;
                            }
                            else
                            {
                                Cv.IfFlag.InError = true;
                                Cv.IfFlag.RecoveryReq = false; // 09.10.26 minhan
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                        else if ((Cv.BrokenDetectScanType != null) &&
                                  Cv.BrokenDetectScanType.SetupBrokenInterlock.Use &&
                                  Cv.BrokenDetectScanType.IsError()) // 11.05.17 minhan
                        {
                            Cv.IfFlag.InError = true;
                            Cv.IfFlag.RecoveryReq = false;
                            m_AlarmId = Cv.BrokenDetectScanType.ALM_BrokenDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Set Alarm");
                            m_ReturnSeqNo = 100;
                            nSeqNo = 4000;
                        }
                    }
                    break;
                case 101:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualInSensor.IsOopDetected())
                    {
                        m_DualInSensor.UseOop = false;
                        m_AlarmId = m_InOOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} SUB Sensor Auto Skip![101]", Cv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    else if (!m_DualInSensor.IsOopDetected())
                    {
                        m_Msg = string.Format("{0} SUB Sensor OFF[101]", Cv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    break;
                case 102:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualInSensor.IsOpDetected())
                    {
                        m_DualInSensor.UseOp = false;
                        m_AlarmId = m_InOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Main Sensor Auto Skip![102]", Cv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    else if (!m_DualInSensor.IsOpDetected())
                    {
                        m_Msg = string.Format("{0} MAIN Sensor OFF[102]", Cv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    if (GetElapsedTicks() > 1000)
                    {
                        glassDetected = Cv.GlsInSensor.IsDetected(Logic.OR);

                        bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();

                        if (glassDetected && timeoutUse)
                        {
                            nSeqNo = 100;
                        }
                        else if (!glassDetected || !timeoutUse)
                        {
                            if (!glassDetected)
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is OFF(2nd)");
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error(2nd) : Skip");
                            }

                            if ((Cv.BrokenDetectScanType == null) ||
                                (!Cv.BrokenDetectScanType.SetupBrokenInterlock.Use) ||
                                (!Cv.BrokenDetectScanType.IsError()))
                            {
                                // Flag set
                                Cv.IfFlag.InComp = true;

                                // Clear error condition
                                ClearErrorConditionFlags();

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In Sensor : Finish");

                                if ((Cv.BrokenDetectScanType != null) && (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use)) // 11.05.17 minhan
                                {
                                    Cv.BrokenDetectScanType.Reset(false);
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Broken Scan : Reset");
                                    m_StartTicks = XFunc.GetTickCount();
                                    nSeqNo = 1;
                                }
                                else
                                {
                                    nSeqNo = 2; // 11.05.17 minhan
                                }
                            }
                            else
                            {
                                nSeqNo = 120;

                            }
                        }
                    }
                    break;
                case 120:
                    if (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use && Cv.BrokenDetectScanType.IsError())
                    {
                        Cv.IfFlag.InError = true;
                        Cv.IfFlag.RecoveryReq = false; // 11.06.08 minhan
                        m_AlarmId = Cv.BrokenDetectScanType.ALM_BrokenDetect.Id; // 11.05.17 minhan
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Set Alarm");
                        m_ReturnSeqNo = 110;
                        nSeqNo = 4000;
                    }
                    else
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : No Alarm");
                        nSeqNo = 110;
                    }
                    break;
                case 500:
                    {
                        int distance = Cv.SetupDistance.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                        m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.03.02 minhan
                        //if (simulation) // 11.03.02 minhan
                        //{
                        //    m_Tm2 = (int)(m_Tm2 / 3);
                        //}

                        m_Timer2.Start(m_Tm2);

                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "In Sensor Detect : Timer2 Start(case500)");

                        nSeqNo = 100;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000:
                    if (m_Control.CheckUnitCondition(Cv))
                    {
                        Cv.IfFlag.InError = false;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                //case 3500: // 11.03.02 minhan
                //    {
                //        bool ok = true;
                //        ok &= m_Control.GetCvCond(Cv);
                //        if (ok)
                //        {
                //            Cv.IfFlag.InError = false;
                //            m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                //            nSeqNo = ReturnSeqNo;
                //        }
                //    }
                //    break;
                case 4000:
                    //if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        //if (DialogResult.Yes == MessageBox.Show("Glass Broken Scan Alarm. Do you really want to reset", "Broken Scan Alarm",
                        //    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)) // 09.12.28 minhan
                        //{
                        //    Cv.BrokenDetectScanType.Reset(false);
                        //    nSeqNo = 4010;
                        //}

                        if (m_EqpManager.AlarmResetSwitchPushed) // 11.05.17 minhan
                        {
                            if (!GlobalVar.MsgRequest)
                            {
                                string Msg = "Broken";
                                m_Control.m_MsgQueue.EnqueueRecvMsg(Msg);
                                GlobalVar.MsgRequest = true;
                                GlobalVar.MsgSkip = false;
                                GlobalVar.MsgBrokenComp = false;
                            }
                        }
                        else if (GlobalVar.MsgBrokenComp)
                        {
                            if (GlobalVar.MsgSkip)
                            {
                                Cv.BrokenDetectScanType.Reset(false);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Reset"); // 11.05.17 minhan
                                nSeqNo = 4010;
                            }

                            GlobalVar.MsgSkip = false;
                            GlobalVar.MsgBrokenComp = false;
                            GlobalVar.MsgRequest = false;
                        }
                    }
                    break;
                case 4010:
                    if (!Cv.BrokenDetectScanType.IsError())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId); // 11.05.17 minhan
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Glass Broken Scan : Reset Alarm");
                        if (Cv.IfFlag.InError)
                        {
                            Cv.IfFlag.InError = false;
                        }
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqDualOutSensor : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadCvBOE_G8_DHDC m_Control; // 11.02.01 minhan
        protected static GenInfoHandler m_GenInfos;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private Alarm m_NextInOPWarning;
        private Alarm m_NextInOOPWarning;
        private Alarm m_OutOPWarning;
        private Alarm m_OutOOPWarning;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private bool m_Tm3Warning;
        private bool m_DualCheckSkip;
        private bool glassDetected;
        private FishHand m_UlHandUnit; // 10.01.07 minhan
        private DualGlsSensor m_DualNextInSensor;
        private DualGlsSensor m_DualOutSensor;
        private TagGlassData m_RecvGlassData = new TagGlassData();//2010.08.30 kimgun
        private float fSkipDelayTime; // 11.02.01 minhan
        private bool simulation;
        private string m_Msg; // 11.02.01 minhan
        private int m_ApdAlarmId; // 11.02.01 minhan
        private string m_OldMsg; // 11.02.01 minhan
        #endregion

        #region Constructor
        public SeqDualOutSensor(ThreadCvBOE_G8_DHDC control, CvUnit cv)
        {
            Cv = cv;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            if (Cv.NextCv != null)
                m_DualNextInSensor = (Cv.NextCv.GlsInSensor as DualGlsSensor);
            m_DualOutSensor = (Cv.GlsOutSensor as DualGlsSensor);
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand; // 10.01.07 minhan
            m_SeqFunName = Cv.GlsOutSensor.Name;
            m_DualCheckSkip = false;
            glassDetected = false;
            fSkipDelayTime = 0;
            simulation = false;
            m_Msg = "";
            m_ApdAlarmId = 0;
            m_OldMsg = "";

            if (Cv.NextCv == null)
            {
                m_Timer2 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Off Timeout");
                m_AlarmOffTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            }
            else
            {
                m_Timer1 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Next On Timeout");
                m_Timer2 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Off Timeout");
                m_Timer3 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Next On Time warning");
                m_AlarmOnTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
                m_AlarmOffTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
                m_NextInOPWarning = new Alarm(Cv.NextCv.GlsInSensor.Name + " : OP In Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.02.01 minhan
                m_NextInOOPWarning = new Alarm(Cv.NextCv.GlsInSensor.Name + " : OOP In Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                m_OutOPWarning = new Alarm(Cv.GlsOutSensor.Name + " : OP Out Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                m_OutOOPWarning = new Alarm(Cv.GlsOutSensor.Name + " : OOP Out Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            }
        }
        #endregion

        #region Methods
        public void FlagSet() // 11.02.01 minhan
        {
            simulation = m_Simul.Device;
            //2010.08.30 kimgun

            if (m_Server.GlassData.GetData(Cv.DataMatchingKey(1), ref m_RecvGlassData)) // 11.03.02 minhan
            {
                m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
                m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            if ((m_AlarmId == m_NextInOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_NextInOOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_OutOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_OutOOPWarning.Id) && m_EqpManager.AlarmResetSwitchPushed) m_EqpManager.ResetAlarm(m_AlarmId);
            if ((m_ApdAlarmId != 0) && m_EqpManager.AlarmResetSwitchPushed) // 11.02.01 minhan
            {
                m_EqpManager.ResetAlarm(m_ApdAlarmId);
                m_ApdAlarmId = 0;
                m_OldMsg = "";
            }

            fSkipDelayTime = m_Server.DualSkipDelayTime.GetValue<float>();//2006.06.16 kimgun
        }

        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.OutError)
            {
                Cv.IfFlag.OutError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }

        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            if (Cv.Sequence[1] == null) Cv.Sequence[1] = this;

            m_Control.TimerControl(Cv, _GSS.ssOUT, m_Timer1, m_Timer2, m_Timer3);
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = this.m_SeqNo;

            FlagSet(); // 11.02.01 minhan

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        //2009.08.15 kimgun
                        bool glassDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(1));
                        if (!glassDataExist)
                        {
                            if (Cv.GlsOutSensor.IsDetected(Logic.OR) && Cv.NextCv.GlsInSensor.IsDetected(Logic.OR))
                            {
                                Cv.IfFlag.OutComp = false;
                                nSeqNo = 500;
                            }
                            else
                            {
                                nSeqNo = 1;
                            }
                        }
                        else
                        {
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.OutComp = false;
                            m_DualCheckSkip = true;//2009.07.14 kimgun
                            nSeqNo = 1;
                        }

                    }
                    break;
                case 1:
                    {
                        // set flag and sensor condtion
                        bool curInReady = Cv.IfFlag.InReady;
                        bool curInError = Cv.IfFlag.InError;
                        bool glassExist = Cv.GlsOutSensor.IsDetected(Logic.OR);

                        // check flag and sensor condtion
                        if (glassExist && curInReady && !curInError)
                        {
                            // Set Flags
                            Cv.IfFlag.InReady = false;

                            // Glass Data Move
                            m_Server.GlassData.Move(Cv.DataMatchingKey(0), Cv.DataMatchingKey(1));
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Cur Sensor is ON");

                            ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), Cv.Name); // 09.11.16 minhan

                            if (Cv.Id == eqpTransferUnits._FR_CvUnit.Id) //11.02.01 minhan 참 그렇네.
                            {
                                try
                                {
                                    string apditem = "";

                                    //Co2
                                    apditem = string.Format("{0}", (double)eqpHpmjs._HPMJ_Unit.CO2InPress.CurAdc / 1000);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_CO2_PRS.Id, apditem);
                                    //Resistivity
                                    apditem = "";
                                    apditem = string.Format("{0}", (double)eqpHpmjs._HPMJ_Unit.Resistivity.CurAdc / 100);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_RES.Id, apditem);
                                    //Diffrence
                                    //apditem = "";
                                    //apditem = string.Format("{0}", (double)eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge.CurAdc / 10);
                                    //ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_PRS_DEV.Id, apditem);
                                    //MainDiPress
                                    apditem = "";
                                    apditem = string.Format("{0}", (double)eqpHpmjs._HPMJ_Unit.MainDiPress.CurAdc / 1000);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_DI_PRS.Id, apditem);
                                    //FilterInPress
                                    apditem = "";
                                    apditem = string.Format("{0}", (double)eqpHpmjs._HPMJ_Unit.FilterInPress.CurAdc / 10);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_IN_PRS.Id, apditem);
                                    //FilterOutPress
                                    apditem = "";
                                    apditem = string.Format("{0}", (double)eqpHpmjs._HPMJ_Unit.FilterOutPress.CurAdc / 10);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_OUT_PRS.Id, apditem);
                                    //HpmjFlow
                                    apditem = "";
                                    apditem = string.Format("{0}", (double)eqpHpmjs._HPMJ_Unit.HpmjFlow.CurAdc / 100);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_DI_FLW.Id, apditem);

                                    m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "HPMJ Apd Report OK");
                                }
                                catch (Exception err)
                                {
                                    string msg = err.ToString();

                                    if (msg != m_OldMsg)
                                    {
                                        m_Server.WriteExceptionLog(msg);
                                        m_OldMsg = msg;
                                        m_ApdAlarmId = m_Control.AlarmApdReportError.Id;
                                        m_EqpManager.SetAlarm(m_ApdAlarmId);
                                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "HPMJ Apd Report NG");
                                    }

                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_CO2_PRS.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_RES.Id, "0");
                                    //ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_PRS_DEV.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_DI_PRS.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_IN_PRS.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_OUT_PRS.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._HJ_DI_FLW.Id, "0");
                                }
                            }

                            if (Cv.Id == eqpTransferUnits._RB_CvUnit.Id)//zhangliang 关于APD中显示RB Speed
                            {
                                try
                                {
                                    string apditem = "";

                                    //RB Up1 
                                    apditem = string.Format("{0}", (double)JobCondition.Instance.CurrentRecipe.RB1UpSpeed);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB1_SPD.Id, apditem);

                                    //RB Lo1 
                                    apditem = string.Format("{0}", (double)JobCondition.Instance.CurrentRecipe.RB1LoSpeed);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB2_SPD.Id, apditem);

                                    //RB Up2
                                    apditem = string.Format("{0}", (double)JobCondition.Instance.CurrentRecipe.RB2UpSpeed);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB3_SPD.Id, apditem);

                                    //RB Lo2
                                    apditem = string.Format("{0}", (double)JobCondition.Instance.CurrentRecipe.RB2LoSpeed);
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB4_SPD.Id, apditem);
                                }
                                catch (Exception err)
                                {
                                    string msg = err.ToString();

                                    if (msg != m_OldMsg)
                                    {
                                        m_Server.WriteExceptionLog(msg);
                                        m_OldMsg = msg;
                                        m_ApdAlarmId = m_Control.AlarmApdReportError.Id;
                                        m_EqpManager.SetAlarm(m_ApdAlarmId);
                                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "RB Apd Report NG");
                                    }

                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB1_SPD.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB2_SPD.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB3_SPD.Id, "0");
                                    ApdItemsHandler.Instance.SetData(Cv.DataMatchingKey(1), eqpApdItems._RB_RB4_SPD.Id, "0");

                                }

                            }
                            // Timer Set
                            int distance = 0;
                            if (Cv.NextCv != null)
                            {
                                distance = Cv.SetupDistanceNext.GetValue<int>();
                            }
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                            m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.02.01 minhan

                            if (Cv.NextCv == null)
                            {
                                m_Timer2.Start(m_Tm2);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Timer Set:tm2");//2009.09.14 kimgun
                            }
                            else
                            {
                                m_Timer1.Start(m_Tm1);
                                m_Timer2.Start(m_Tm2);
                                m_Timer3.Start(m_Tm3);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Timer Set:tm1,tm2,tm3");//2009.09.14 kimgun
                            }

                            m_Tm3Warning = false;
                            if ((m_DualOutSensor != null) &&
                                m_DualOutSensor.UseOp && m_DualOutSensor.UseOop)
                            {
                                if (m_DualOutSensor.IsOpDetected() &&
                                    !m_DualOutSensor.IsOopDetected()) nSeqNo = 5;//Main Sensor ON
                                else if (m_DualOutSensor.IsOopDetected() &&
                                    !m_DualOutSensor.IsOpDetected()) nSeqNo = 8;//Sub Sensor ON
                                else nSeqNo = 10;
                            }
                            else nSeqNo = 10;
                            m_StartTicks = XFunc.GetTickCount();
                        }
                    }
                    break;
                case 5:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        !m_DualOutSensor.IsOopDetected())
                    {
                        m_DualOutSensor.UseOop = false;
                        m_AlarmId = m_OutOOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} SUB Sensor Auto Skip![5]", Cv.GlsOutSensor.Name); // 11.02.01 minhan
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 10;
                    }
                    else if (m_DualOutSensor.IsOopDetected())
                    {
                        string sMsg;
                        sMsg = string.Format("{0} SUB Sensor ON[5]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, sMsg);
                        nSeqNo = 10;
                    }
                    break;

                case 8:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        !m_DualOutSensor.IsOpDetected())
                    {
                        m_DualOutSensor.UseOp = false;
                        m_AlarmId = m_OutOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Main Sensor Auto Skip![8]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 10;
                    }
                    else if (m_DualOutSensor.IsOpDetected())
                    {
                        m_Msg = string.Format("{0} MAIN Sensor ON[8]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 10;
                    }
                    break;
                //==>2010.04.15 kimgun
                case 10:
                    //다음 sensor가 감지 되어 있을 경우 //////////////////////////////////////
                    if ((m_DualNextInSensor != null) &&
                        m_DualNextInSensor.UseOp && m_DualNextInSensor.UseOop)
                    {
                        if (m_DualNextInSensor.IsDetected(Logic.AND) || !m_DualNextInSensor.IsDetected(Logic.OR))
                            nSeqNo = 15;
                        else
                        {
                            if (m_DualNextInSensor.IsOpDetected() &&
                                !m_DualNextInSensor.IsOopDetected()) nSeqNo = 11;//Main Sensor ON
                            else if (!m_DualNextInSensor.IsOpDetected() &&
                                m_DualNextInSensor.IsOopDetected()) nSeqNo = 13;//Sub Sensor ON 
                            m_StartTicks = XFunc.GetTickCount(); // 11.03.02 minhan
                        }
                    }
                    else nSeqNo = 15;
                    //////////////////////////////////////////////////////////////////////////
                    break;
                case 11:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualNextInSensor.IsOpDetected())
                    {
                        m_DualNextInSensor.UseOp = false;
                        m_AlarmId = m_NextInOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Main Sensor Auto Skip![11]", Cv.NextCv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 15;
                    }
                    else if (!m_DualNextInSensor.IsOpDetected())
                    {
                        m_Msg = string.Format("{0} MAIN Sensor OFF[11]", Cv.NextCv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 15;
                    }
                    break;
                case 13:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualNextInSensor.IsOopDetected())
                    {
                        m_DualNextInSensor.UseOop = false;
                        m_AlarmId = m_NextInOOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Sub Sensor Auto Skip![13]", Cv.NextCv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 15;
                    }
                    else if (!m_DualNextInSensor.IsOopDetected())
                    {
                        m_Msg = string.Format("{0} MAIN Sensor OFF[13]", Cv.NextCv.GlsInSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        nSeqNo = 15;
                    }
                    break;
                case 15:
                    if (Cv.Id == eqpTransferUnits._AK_CvUnit.Id)
                    {
                        nSeqNo = 18;//2009.08.25 kimgun
                    }
                    else
                    {
                        nSeqNo = 20;
                    }
                    break;
                case 18://2009.08.25 kimgun
                    {
                        bool UlHandCheck = eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                        UlHandCheck &= (eqpCylinders._UL_Fish_Hand_Cylinder_OP.IsActStatus(ActuatorAct.Pos) || eqpCylinders._UL_Fish_Hand_Cylinder_MT.IsActStatus(ActuatorAct.Pos));
                        //UlHandCheck |= m_UlHandUnit.IfFlag.InComp;

                        if (/*!Cv.NextCv.GlsInSensor.IsDetected(Logic.AND) && */!Cv.NextCv.GlsOutSensor.IsDetected(Logic.AND) &&
                           !m_Server.GlassData.IsExist(Cv.NextCv.DataMatchingKey(0)) &&
                           !m_Server.GlassData.IsExist(Cv.NextCv.DataMatchingKey(1)) &&
                           !UlHandCheck) // 11.05.24 minhan
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Ul Unit Empty : OK");
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unloading TimeOut : Set");

                            GlobalVar.UlTimeOut = true;
                            nSeqNo = 400;
                        }
                    }
                    break;
                case 20:
                    {
                        if (Cv.Id == eqpTransferUnits._LD_CvUnit.Id) // 09.11.17 minhan
                        {

                            foreach (CvUnit unit in ThreadCvControl.Units)
                            {
                                if (!m_Control.CheckUnitCondition(unit))
                                {
                                    Cv.IfFlag.OutError = true;
                                    Cv.IfFlag.RecoveryReq = false; // 11.03.02 minhan
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                                    m_ReturnSeqNo = nSeqNo;
                                    nSeqNo = 4000;
                                    break;
                                }
                            }

                            if (!eqpActuatorUnits._LD_Align.IsPositive()) // 11.03.02 minhan
                            {
                                m_AlarmId = eqpActuatorUnits._LD_Align.ALM_ActuatorPositive.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false;

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align FW Error");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 5000;
                                break; // 11.03.02 minhan
                            }
                        }
                        else if (Cv.Id == eqpTransferUnits._RB_CvUnit.Id) // 11.02.01 minhan
                        {
                            bool ok = true;
                            ok &= m_Control.CheckUnitCondition(Cv);
                            ok &= m_Control.GetCvCond(Cv.NextCv);
                            ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge.IsAlarm;  //ak 건조 불량시 out에서 hpmj, rb 정지하고 대기.
                            ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge.IsAlarm;
                            ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge.IsAlarm;
                            ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge.IsAlarm;
                            if (!ok)
                            {
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false; // 11.02.01 minhan
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 3500;
                                break;
                            }
                        }
                        else if (!m_Control.CheckUnitCondition(Cv) || !m_Control.GetCvCond(Cv.NextCv)) // 09.12.08 minhan 자신의 컨디션과 다음 컨베어의 fbl 상태
                        {
                            Cv.IfFlag.OutError = true;
                            Cv.IfFlag.RecoveryReq = false; // 11.02.01 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                            break;
                        }

                        if (Cv.NextCv == null)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "This is last Out Sensor");

                            nSeqNo = 100;
                        }
                        else if (Cv.NextCv.IfFlag.InComp)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Next Complete OK");
                            if (m_DualNextInSensor != null) m_StartTicks = XFunc.GetTickCount();//Dual Sensor Check
                            nSeqNo = 30;
                        }
                        else if (m_Timer3.Over)
                        {
                            Cv.IfFlag.OutLogicalCheckIgnore = true;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Complete NG");
                            if (m_DualNextInSensor != null) m_StartTicks = XFunc.GetTickCount();//Dual Sensor Check
                            nSeqNo = 30;
                        }
                    }
                    break;
                #region 제거
                //case 25:
                //    {
                //        bool glassExist = Cv.NextCv.GlsInSensor.IsDetected();

                //        if (m_DualNextInSensor != null && !m_DualCheckSkip)//2009.07.14 kimgun
                //        {
                //            if (GetElapsedTicks() > 500 && glassExist)
                //            {
                //                bool glassExistOp = m_DualNextInSensor.DiSensorOp.GetState();
                //                bool glassExistOop = m_DualNextInSensor.DiSensor.GetState();

                //                if (glassExistOp && m_DualNextInSensor.UseOp && m_DualNextInSensor.UseOop)
                //                {
                //                    m_DualNextInSensor.UseOp = false;
                //                    AlarmId = m_NextInOPWarning.Id;
                //                    m_EqpManager.SetAlarm(AlarmId);
                //                    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualNextInSensor Warning Op");
                //                }
                //                else if (glassExistOop && m_DualNextInSensor.UseOp && m_DualNextInSensor.UseOop)
                //                {
                //                    m_DualNextInSensor.UseOop = false;
                //                    AlarmId = m_NextInOOPWarning.Id;
                //                    m_EqpManager.SetAlarm(AlarmId);
                //                    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualNextInSensor Warning Oop");
                //                }
                //                nSeqNo = 30;
                //            }
                //            else if (!glassExist)
                //            {
                //                m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualNextInSensor Condition OK");
                //                nSeqNo = 30;
                //            }
                //        }
                //        else
                //        {
                //            m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "DualNextInSensor No Use ");
                //            m_DualCheckSkip = false;//2009.07.14 kimugn
                //            nSeqNo = 30;
                //        }
                //    }
                //    break;
                #endregion
                case 30:
                    {
                        // bool glassDetected = false;

                        if (Cv.Id == eqpTransferUnits._LD_CvUnit.Id) // 09.11.17 minhan
                        {

                            foreach (CvUnit unit in ThreadCvControl.Units)
                            {
                                if (!m_Control.CheckUnitCondition(unit))
                                {
                                    Cv.IfFlag.OutError = true;
                                    Cv.IfFlag.RecoveryReq = false; // 11.03.02 minhan
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                                    m_ReturnSeqNo = nSeqNo;
                                    nSeqNo = 4000;
                                    break;
                                }
                            }

                            if (!eqpActuatorUnits._LD_Align.IsPositive()) // 11.03.02 minhan
                            {
                                m_AlarmId = eqpActuatorUnits._LD_Align.ALM_ActuatorPositive.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false;

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "LD Align FW Error");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 5000;
                                break; // 11.03.02 minhan
                            }
                        }
                        else if (Cv.Id == eqpTransferUnits._RB_CvUnit.Id) // 11.02.01 minhan
                        {
                            bool ok = true;
                            ok &= m_Control.CheckUnitCondition(Cv);
                            ok &= m_Control.GetCvCond(Cv.NextCv);
                            ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge.IsAlarm;  //ak 건조 불량시 out에서 hpmj, rb 정지하고 대기.
                            ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge.IsAlarm;
                            ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge.IsAlarm;
                            ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge.IsAlarm;
                            if (!ok)
                            {
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false; // 11.02.01 minhan
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 3500;
                                break;
                            }
                        }
                        else if (!m_Control.CheckUnitCondition(Cv) || !m_Control.GetCvCond(Cv.NextCv)) // 09.12.08 minhan 자신의 컨디션과 다음 컨베어의 fbl 상태
                        {
                            Cv.IfFlag.OutError = true;
                            Cv.IfFlag.RecoveryReq = false; // 11.02.01 minhan
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                            break;
                        }

                        if (!simulation)
                        {
                            glassDetected = Cv.NextCv.GlsInSensor.IsDetected(Logic.OR);
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        if (glassDetected && (m_Timer3.Over || Cv.IfFlag.OutLogicalCheckIgnore))
                        {
                            if (simulation)
                            {
                                Cv.NextCv.GlsInSensor.SetState(true, Logic.AND);
                                glassDetected = false;
                            }

                            // Flag set / reset
                            Cv.NextCv.IfFlag.InComp = false;
                            Cv.IfFlag.OutReady = true;
                            Cv.IfFlag.OutLogicalCheckIgnore = false;
                            if (!m_Tm3Warning) Cv.NextCv.IfFlag.InSick = false; // 2009.08.15 kimgun
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Next Sensor is ON");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 100;
                        }
                        //else if (glassDetected && !m_Tm3Warning)//2009.08.15 kimgun
                        //{
                        //    Cv.NextCv.IfFlag.InComp = false;

                        //    m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor is NG(1st)");
                        //    m_StartTicks = XFunc.GetTickCount();

                        //    ReturnSeqNo = nSeqNo;
                        //    nSeqNo = 40;
                        //}
                        else if (m_Timer1.Over)//Cv.Id == eqpTransferUnits._RB_CvUnit.Id)// m_Timer1.Over)
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            Cv.IfFlag.OutError = true;
                            Cv.IfFlag.RecoveryReq = false; // 09.10.26 minhan

                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }

                    }
                    break;
                #region 제거
                //case 40:
                //    if (GetElapsedTicks() > 500)
                //    {
                //        //if (Cv.NextCv.GlsInSensor.IsDetected() && !m_Tm3Warning)
                //        if (Cv.NextCv.GlsInSensor.IsDetected(Logic.OR) && !m_Tm3Warning)
                //        {
                //            m_Tm3Warning = true;
                //            //  Cv.NextCv.IfFlag.InSick = true;//2009.08.15 kimgun
                //            m_Control.SetLog(SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Next Sensor is NG(2nd)");
                //            nSeqNo = ReturnSeqNo;
                //        }
                //        else
                //        {
                //            nSeqNo = ReturnSeqNo;
                //        }
                //    }
                //    //else if (!Cv.NextCv.GlsInSensor.IsDetected())
                //    else if (!Cv.NextCv.GlsInSensor.IsDetected(Logic.OR))
                //    {
                //        nSeqNo = ReturnSeqNo;
                //    }
                //    break;
                #endregion
                case 100:
                    {
                        // bool glassDetected = false;
                        if (!simulation)
                        {
                            //glassDetected = Cv.GlsOutSensor.IsDetected();
                            glassDetected = Cv.GlsOutSensor.IsDetected(Logic.AND);
                        }
                        else
                        {
                            glassDetected = !m_Timer2.Over;
                        }

                        if (!glassDetected)
                        {
                            if (simulation)
                            {
                                //Cv.GlsOutSensor.DiSensor.SetState(false);
                                Cv.GlsOutSensor.SetState(false, Logic.AND);
                            }
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            if ((m_DualOutSensor != null) &&
                                m_DualOutSensor.UseOp && m_DualOutSensor.UseOop)
                            {
                                if (!m_DualOutSensor.IsOpDetected() &&
                                    m_DualOutSensor.UseOop) nSeqNo = 101;
                                else if (!m_DualOutSensor.IsOopDetected() &&
                                    m_DualOutSensor.UseOp) nSeqNo = 102;
                                else nSeqNo = 110;
                            }
                            else nSeqNo = 110;
                            m_StartTicks = XFunc.GetTickCount(); // 11.03.02 minhan

                        }
                        else if (m_Timer2.Over && glassDetected)//2010.09.07 kimgun Timer2 alarm은 sensor가 off되지 않았을 때만 C++참조
                        {
                            bool timeoutUse = Cv.SetupOutOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();

                                nSeqNo = 110;
                            }
                            else
                            {
                                Cv.IfFlag.OutError = true;
                                Cv.IfFlag.RecoveryReq = false; // 09.10.26 minhan
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);

                                m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 101:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualOutSensor.IsOopDetected())
                    {
                        m_DualOutSensor.UseOop = false;
                        m_AlarmId = m_OutOOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0}SUB Sensor Auto Skip![101]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    else if (!m_DualOutSensor.IsOopDetected())
                    {
                        m_Msg = string.Format("{0}SUB Sensor OFF[101]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    break;

                case 102:
                    if ((GetElapsedTicks() > (1000 * fSkipDelayTime)) &&
                        m_DualOutSensor.IsOpDetected())
                    {
                        m_DualOutSensor.UseOp = false;
                        m_AlarmId = m_OutOPWarning.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0}Main Sensor Auto Skip![102]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    else if (!m_DualOutSensor.IsOpDetected())
                    {
                        m_Msg = string.Format("{0}Main Sensor OFF[102]", Cv.GlsOutSensor.Name);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    {
                        if (GetElapsedTicks() > 1000)
                        {
                            glassDetected = Cv.GlsOutSensor.IsDetected(Logic.AND);

                            bool timeoutUse = Cv.SetupOutOffUse.GetValue<bool>();
                            if (glassDetected && timeoutUse)
                            {
                                nSeqNo = 100;
                            }
                            else if (!glassDetected || !timeoutUse)
                            {

                                if (!glassDetected)
                                {
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor is OFF(2nd)");
                                }
                                else
                                {
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error(2nd) : Skip");
                                }

                                // Flag set
                                Cv.IfFlag.OutComp = true;

                                // Clear error condition
                                ClearErrorConditionFlags();

                                if (Cv.NextCv == null)
                                {
                                    m_Server.GlassData.Delete(Cv.DataMatchingKey(1)); // 09.11.16 minhan
                                }

                                nSeqNo = 1;
                            }
                        }
                    }
                    break;
                case 400:
                    {
                        bool UlHandCheck = eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                        UlHandCheck &= (eqpCylinders._UL_Fish_Hand_Cylinder_OP.IsActStatus(ActuatorAct.Pos) || eqpCylinders._UL_Fish_Hand_Cylinder_MT.IsActStatus(ActuatorAct.Pos));

                        if (/*!Cv.NextCv.GlsInSensor.IsDetected(Logic.AND) &&*/ !Cv.NextCv.GlsOutSensor.IsDetected(Logic.AND) &&
                           !m_Server.GlassData.IsExist(Cv.NextCv.DataMatchingKey(0)) &&
                           !m_Server.GlassData.IsExist(Cv.NextCv.DataMatchingKey(1)) &&
                           !UlHandCheck) // 11.05.24 minhan
                        {//2009.08.25 kimgun
                            GlobalVar.UlTimeOut = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Unloading Timeout Release");
                            nSeqNo = 20;
                        }
                        else
                        {
                            if (!GlobalVar.UlTimeOut) GlobalVar.UlTimeOut = true; // 11.03.02 minhan
                        }
                    }
                    break;
                case 500:
                    {
                        int distance = Cv.SetupDistanceNext.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                        m_Msg = string.Format("{0} : {1} : {2} : {3}", Cv.AutoSpeed, m_Tm1, m_Tm2, m_Tm3);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, m_Msg); // 11.03.02 minhan

                        //if (simulation)2009.08.15 kimgun
                        //{
                        //    m_Tm2 = (int)(m_Tm2 / 3);
                        //}

                        m_Timer2.Start(m_Tm2);

                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Out Sensor Detect : Timer2 Start(case500)"); // 2009.08.15 kimgun
                        nSeqNo = 100;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000:
                    if (m_Control.CheckUnitCondition(Cv) && m_Control.CheckUnitCondition(Cv.NextCv))
                    {
                        Cv.IfFlag.OutError = false;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3500: // 11.02.01 minhan
                    {
                        bool ok = true;
                        ok &= m_Control.CheckUnitCondition(Cv);
                        ok &= m_Control.GetCvCond(Cv.NextCv);
                        ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge.IsAlarm;
                        ok &= !eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge.IsAlarm;
                        ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge.IsAlarm;
                        ok &= !eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge.IsAlarm;
                        if (ok)
                        {
                            Cv.IfFlag.OutError = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 4000: // 09.11.17 minhan
                    {
                        bool ok = true;
                        foreach (CvUnit unit in ThreadCvControl.Units)
                        {
                            ok &= m_Control.CheckUnitCondition(unit);
                        }

                        if (ok)
                        {
                            Cv.IfFlag.OutError = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 5000: // 11.03.02 minhan
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        ClearErrorConditionFlags();
                        m_Control.SetLog(m_SeqFunName, nSeqNo, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    class SeqEuvProcessCondition : XSeqFunction
    {
        #region fields
        private IServerManager m_Server = null;
        private GenInfoHandler m_GenInfo = null;
        private Alarm EuvRunCheckError = null;
        private Alarm EuvStopCheckError = null;
        protected static IEqpManager m_EqpManager;
        private bool lampSelecte;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public SeqEuvProcessCondition(ThreadCvBOE_G8_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_GenInfo = GenInfoHandler.Instance;
            EuvRunCheckError = new Alarm("Euv Run Check Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            EuvStopCheckError = new Alarm("Euv Stop Check Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_EqpManager = m_Server.EqpStateManager;
            m_SeqFunName = "EUV Condition Check";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfo.AutoMode) return -1;
            if (AppConfig.Instance.Simul.Device) return -1;
            //if (GlobalVar.PvgModeChange) return -1;

            int nSeqNo = this.m_SeqNo;

            bool EuvCheckCondition = true;
            EuvCheckCondition &= eqpUshioEuvUnits._Ushio_EUV_Unit_.Cv.GlsInSensor.IsDetected();
            EuvCheckCondition &= eqpUshioEuvUnits._Ushio_EUV_Unit_.Cv.MotorControl.IsFw(Logic.OR);

            bool EuvRunCondition = true;
            EuvRunCondition &= m_Server.JobCond.EuvUse(eqpUshioEuvUnits._Ushio_EUV_Unit_);
            lampSelecte = false;
            for (int i = 0; i < eqpUshioEuvUnits._Ushio_EUV_Unit_.Lamps.Count; i++)
            {
                lampSelecte |= m_Server.JobCond.EuvLampUse(eqpUshioEuvUnits._Ushio_EUV_Unit_.Lamps[i]);
            }
            EuvRunCondition &= lampSelecte;

            switch (nSeqNo)
            {
                case 0:
                    if (EuvCheckCondition)
                    {
                        nSeqNo = 5;
                    }
                    break;
                case 5:
                    if (EuvRunCondition)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 10:
                    if (!EuvRunCondition || !EuvCheckCondition)
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        if (eqpUshioEuvUnits._Ushio_EUV_Unit_.DiULAC.GetState())
                        {
                            nSeqNo = 0;
                        }
                        else
                        {
                            eqpUshioEuvUnits._Ushio_EUV_Unit_.IsAlarm = true;
                            m_AlarmId = EuvRunCheckError.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            string log = string.Format("alarm Set : Euv Run Check Error", m_SeqFunName);
                            m_Server.Log(log);
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20:
                    if (EuvRunCondition || !EuvCheckCondition)
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        if (!eqpUshioEuvUnits._Ushio_EUV_Unit_.DiULAC.GetState())
                        {
                            nSeqNo = 0;
                        }
                        else
                        {
                            eqpUshioEuvUnits._Ushio_EUV_Unit_.IsAlarm = true;
                            m_AlarmId = EuvStopCheckError.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            string log = string.Format("Alarm Set :  Euv Stop Check Error", m_SeqFunName);
                            m_Server.Log(log);
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        eqpUshioEuvUnits._Ushio_EUV_Unit_.IsAlarm = false;
                        string log = string.Format("Alarm Reset : Euv Condition Error Reset", m_SeqFunName);
                        m_Server.Log(log);
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
        #endregion
    }
    public class SeqAkManualInterlockCheck : XSeqFunction // 09.05.30 minhan
    {
        #region Fields
        protected static ServerManager m_Server = null;
        protected static EqpManager m_Eqp = null;
        protected static ThreadCvControl m_Control;
        private List<Gauge> m_List = new List<Gauge>();
        private int m_Count;
        private bool[,] m_IsAlarm;
        private bool[,] m_IsWarning;
        bool[] use;
        double[] curVal;
        double[] lowAlarm;
        double[] lowWarning;
        double[] upAlarm;
        double[] upWarning;
        private _GenericCollection<CvUnit> m_CvUnits;


        #endregion

        #region Constructor
        public SeqAkManualInterlockCheck(ThreadCvControl control, _GenericCollection<CvUnit> units)
        {
            m_Server = ServerManager.Instance;
            m_Eqp = EqpManager.Instance;
            m_Control = control;
            m_CvUnits = units;

            m_List.Add(eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge);
            m_List.Add(eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge);
            m_List.Add(eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge);
            m_List.Add(eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge);
            m_Count = m_List.Count;
            m_IsAlarm = new bool[m_Count, 2];
            m_IsWarning = new bool[m_Count, 2];

            use = new bool[m_Count];
            curVal = new double[m_Count];
            lowAlarm = new double[m_Count];
            lowWarning = new double[m_Count];
            upAlarm = new double[m_Count];
            upWarning = new double[m_Count];

        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool checkCondition = true;
            checkCondition &= !GenInfoHandler.Instance.AutoMode;
            checkCondition &= (eqpTransferUnits._AK_CvUnit.MotorControl.IsFw(Logic.OR) || eqpTransferUnits._AK_CvUnit.MotorControl.IsBw(Logic.OR));
            checkCondition &= (eqpAutoValves._AK_LO_CDA_In_Valve.IsOpen() || eqpAutoValves._AK_UP_CDA_In_Valve.IsOpen()); // 11.03.02 minhan 하나라도 열리면.

            switch (nSeqNo)
            {
                case 0:
                    if (checkCondition == true)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (checkCondition == false)
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 3000)
                    {
                        for (int i = 0; i < m_Count; i++)
                        {
                            if (m_List[i].InterlockMethod == Gauge.interlockMethod.PC) // 11.04.11 minhan
                            {
                                use[i] = m_List[i].SetupInterlock.Use;
                                curVal[i] = m_List[i].CurValue;
                                lowAlarm[i] = m_List[i].SetupInterlock.LowAlarm;
                                lowWarning[i] = m_List[i].SetupInterlock.LowWarning;
                                upAlarm[i] = m_List[i].SetupInterlock.HighAlarm;
                                upWarning[i] = m_List[i].SetupInterlock.HighWarning;
                            }
                            else if (m_List[i].InterlockMethod == Gauge.interlockMethod.PLC)
                            {
                                use[i] = m_List[i].SetupInterlock.Use;
                                curVal[i] = m_List[i].CurValue;
                                lowAlarm[i] = m_List[i].SetupInterlock.SettingValue - m_List[i].SetupInterlock.LowAlarm;
                                lowWarning[i] = m_List[i].SetupInterlock.SettingValue - m_List[i].SetupInterlock.LowWarning;
                                upAlarm[i] = m_List[i].SetupInterlock.SettingValue + m_List[i].SetupInterlock.HighAlarm;
                                upWarning[i] = m_List[i].SetupInterlock.SettingValue + m_List[i].SetupInterlock.HighWarning;
                            }
                        }

                        for (int j = 0; j < m_Count; j++) // 11.04.11 minhan
                        {
                            if (checkCondition && use[j])
                            {
                                if (lowAlarm[j] >= curVal[j]) m_IsAlarm[j, 0] = true;
                                else if ((lowAlarm[j] < curVal[j]) && (lowWarning[j] >= curVal[j])) m_IsWarning[j, 0] = true;
                                else if (upAlarm[j] <= curVal[j]) m_IsAlarm[j, 1] = true;
                                else if ((upAlarm[j] > curVal[j]) && (upWarning[j] <= curVal[j])) m_IsWarning[j, 1] = true;
                            }
                        }

                        bool ConditionErr = false;
                        for (int k = 0; k < m_Count; k++)
                        {
                            for (int l = 0; l < 2; l++)
                            {
                                ConditionErr = m_IsAlarm[k, l];
                                //ConditionErr |= m_IsWarning[k, l]; // 09.12.08 minhan 알람만 정지하도록
                            }
                            if (ConditionErr) break;
                        }

                        if (ConditionErr)
                        {
                            int unitCount = m_CvUnits.Count;
                            CvUnit unit;
                            for (int i = 0; i < unitCount; i++)
                            {
                                unit = m_CvUnits[i];
                                int motorCount = unit.Motors.Count;
                                for (int j = 0; j < motorCount; j++)
                                {
                                    unit.ManualAct[j] = CvMotorAct.Stop;
                                }
                            }

                            //m_bAkManualAlarm = TRUE;
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        for (int i = 0; i < m_Count; i++)
                        {
                            Gauge gauge = m_List[i];
                            if (m_IsAlarm[i, 0]) m_Eqp.SetAlarm(gauge.ALM_LowerAlarm.Id);
                            if (m_IsAlarm[i, 1]) m_Eqp.SetAlarm(gauge.ALM_UpperAlarm.Id);
                            if (m_IsWarning[i, 0]) m_Eqp.SetAlarm(gauge.ALM_LowerWarning.Id);
                            if (m_IsWarning[i, 1]) m_Eqp.SetAlarm(gauge.ALM_UpperWarning.Id);
                        }
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        for (int i = 0; i < m_Count; i++)
                        {
                            Gauge gauge = m_List[i];

                            if (m_IsAlarm[i, 0])
                            {
                                m_Eqp.ResetAlarm(gauge.ALM_LowerAlarm.Id);
                                m_IsAlarm[i, 0] = false;
                            }
                            if (m_IsAlarm[i, 1])
                            {
                                m_Eqp.ResetAlarm(gauge.ALM_UpperAlarm.Id);
                                m_IsAlarm[i, 1] = false;
                            }
                            if (m_IsWarning[i, 0])
                            {
                                m_Eqp.ResetAlarm(gauge.ALM_LowerWarning.Id);
                                m_IsWarning[i, 0] = false;
                            }
                            if (m_IsWarning[i, 1])
                            {
                                m_Eqp.ResetAlarm(gauge.ALM_UpperWarning.Id);
                                m_IsWarning[i, 1] = false;
                            }
                        }

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



