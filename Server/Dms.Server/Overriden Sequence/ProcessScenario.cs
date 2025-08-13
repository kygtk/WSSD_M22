using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Sequence;
using Dms.Device;
using System.Windows.Forms;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ProcessScenario
    {
        #region Fields
        protected static ServerManager m_Server = ServerManager.Instance;
        protected static GenInfoHandler m_GenInfos = GenInfoHandler.Instance;
        #endregion

        #region Common ProcessCondition
        protected static bool IsInterlock(ProcessUnit unit) // 11.02.01 minhan
        {
            #region 제거
            //bool interlock = false;
            //int glassNo = 0;
            ////runCond &= (m_Server.JobCond.HeavyInterlock & ~HeavyInterlock.Door) > 0 ;	//DoorInterlock을 제외하는경우
            ////2010.06.29 kimgun leak는 세정구간에 glass가 있으면 일단 정상으로 취급한다.유저요청.
            //for (int i = eqpTransferUnits._RB_CvUnit.DataMatchingKey(0); i <= eqpTransferUnits._UL_CvUnit.DataMatchingKey(0); i++) // minhan
            //{
            //    if (m_Server.GlassData.IsExist(i)) glassNo++;
            //}
            //if (glassNo > 0)
            //    interlock &= ((m_Server.JobCond.HeavyInterlock & ~HeavyInterlock.Leak) > 0) ;
            //else
            //{
            //    interlock |= (m_Server.JobCond.HeavyInterlock > 0) ;
            //    interlock |= GlobalVar.HighLevelDetectInterlock;//2010.09.09 kimgun 세정구간의 glass는 일단 빼고 ld부만 출발 안 시킨다.
            //}
            //interlock |= eqpTankUnits._FR_Unit_Tank.IfFlag.TankLevelFault;//2010.09.09 kimgun 무조건 정지
            ////interlock |= ((m_Server.JobCond.HeavyInterlock & HeavyInterlock.Leak) > 0) ;
            //return interlock;
            #endregion

            bool interlock = true;
            interlock &= m_Server.JobCond.HeavyInterlock > 0;

            return interlock;
        }

        protected static bool IsRunEnable(ProcessUnit unit)
        {
            bool run = true;
            run &= m_Server.JobCond.ProcessMode;

            return run;
        }

        protected static bool IsAutoRunCondition(ProcessUnit unit)
        {
            bool run = true;
            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= m_GenInfos.DiStart;
            return run;
        }
        private static int GetGlassCount() // minhan
        {
            int glassNo = 0;

            for (int i = eqpTransferUnits._LD_CvUnit.DataMatchingKey(0); i <= eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0); i++) // 11.04.22 minhan
            {
                if (m_Server.GlassData.IsExist(i)) glassNo++;
            }

            return glassNo;
        }
        #endregion

        #region Parts Life time Control
        public static bool GetPartsCheckCondition(int partsNo) // 11.02.01 minhan
        {
            bool check = true;

            if (partsNo == eqpPartsItems._RB_AC__Ionizer_Filter.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
            }
            else if (partsNo == eqpPartsItems._RB1_Up_Brush.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= (eqpRbMotors._RB_Unit_Up_RbMotor1.IsTurnCw() || eqpRbMotors._RB_Unit_Up_RbMotor1.IsTurnCcw());
            }
            else if (partsNo == eqpPartsItems._RB1_Lo_Brush.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= (eqpRbMotors._RB_Unit_Lo_RbMotor1.IsTurnCw() || eqpRbMotors._RB_Unit_Lo_RbMotor1.IsTurnCcw());
            }
            else if (partsNo == eqpPartsItems._RB2_Up_Brush.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= (eqpRbMotors._RB_Unit_Up_RbMotor2.IsTurnCw() || eqpRbMotors._RB_Unit_Up_RbMotor2.IsTurnCcw());
            }
            else if (partsNo == eqpPartsItems._RB2_Lo_Brush.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= (eqpRbMotors._RB_Unit_Lo_RbMotor2.IsTurnCw() || eqpRbMotors._RB_Unit_Lo_RbMotor2.IsTurnCcw());
            }
            else if (partsNo == eqpPartsItems._RB_MJ_CDA_Filter.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= eqpAutoValves._RB_MJ_CDA_In_Valve.IsOpen();
            }
            else if (partsNo == eqpPartsItems._RB_Shw_Pump_Filter.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= eqpPumps._RB_Shower_Pump.IsRun();
            }
            else if (partsNo == eqpPartsItems._FR_Shw_DI_Filter.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= eqpAutoValves._FR_SHW_DI_In_Valve.IsOpen();
            }
            else if (partsNo == eqpPartsItems._AK_CDA_Filter1.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= eqpAutoValves._AK_UP_CDA_In_Valve.IsOpen();
            }
            else if (partsNo == eqpPartsItems._AK_CDA_Filter2.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= eqpAutoValves._AK_LO_CDA_In_Valve.IsOpen();
            }
            else if (partsNo == eqpPartsItems._EUV_N2_IN_Filter.Id)
            {
                check &= m_Server.JobCond.ProcessMode;
                check &= m_Server.JobCond.EuvUse(eqpUshioEuvUnits._Ushio_EUV_Unit_);
                check &= (eqpEuvLamps._EuvUnit_Lamp1.IsOn());
            }

            return check;
        }
        #endregion

        #region Air Knife Control
        public static ProcessCondition AirKnifeProcess(ProcessUnit unit, bool previousAutoMode, bool previousSemiAuto)
        {
            bool interlock = false;
            //bool isAkMotorRun = eqpTransferUnits._AK_CvUnit.MotorControl.IsFw(Logic.OR);
            //bool glassDetect = m_Server.GlassData.IsExist(eqpCvUnits._AK_CvUnit.DataMatchingKey(0));//zhangliang
            //glassDetect |= m_Server.GlassData.IsExist(eqpCvUnits._AK_CvUnit.DataMatchingKey(1));
            //glassDetect |= eqpCvUnits._AK_CvUnit.GlsInSensor.IsDetected(Logic.OR);
            //glassDetect |= eqpCvUnits._AK_CvUnit.GlsOutSensor.IsDetected(Logic.OR);
            // Ak는 Heavy Inerlock무시
            // interlock = IsInterlock(unit);
            // 11.02.01 minhan 공정문제로 인하여 나이프를 off 하는 경우도 있는데, 이럴경우는 데이터와 센서 모터의 움직을 보고 수정해야함
            ProcessCondition refProcess = ProcessCondition.Run;

            if (interlock)
            {
                refProcess = ProcessCondition.Stop;
            }
            else if (m_GenInfos.AutoMode)
            {
                refProcess = ProcessCondition.Run;
            }
            else
            {
                if (unit.OwnerUnit == null)
                {
                }
                else
                {
                    bool isMotorRun = unit.OwnerUnit.MotorControl.IsFw(Logic.OR);
                    if (isMotorRun)
                    {
                        refProcess = ProcessCondition.Run;
                    }
                    else
                    {
                        if (m_GenInfos.DiStart)
                        {
                            refProcess = ProcessCondition.Run;
                        }
                        else if (previousAutoMode == true)
                        {   // AutoMode에서 ManualMode로 바뀌는 시점
                            // AK의 경우에는 Auto -> Manual 되어도 On
                            refProcess = ProcessCondition.Run;
                        }
                        else
                        {	// AutoValve Manual Control은 가능해야겠기에
                            refProcess = ProcessCondition.DontCare;
                        }
                    }
                }
            }

            return refProcess;
        }
        #endregion

        #region RB MJ Control
        public static ProcessCondition RbMJProcess(ProcessUnit unit, bool previousAutoMode, bool previousSemiAuto)
        {
            bool interlock = IsInterlock(unit);
            bool enable = IsRunEnable(unit);
            bool autoRun = IsAutoRunCondition(unit);
            autoRun &= (GetGlassCount() > 0 || (m_GenInfos.IdleRunning && GetGlassCount() == 0));
            autoRun &= m_Server.JobCond.CurrentRecipe.MjUse; //Recipe MJ 사용 모드에서만

            ProcessCondition refProcess = ProcessCondition.Stop;
            if (interlock)
            { // Heavy interlock 조건이면 무조건 stop
                refProcess = ProcessCondition.Stop;
            }
            else if (!enable)
            { // Run 불가능한 상태이면 무조건 stop
                refProcess = ProcessCondition.Stop;
            }
            else if (m_GenInfos.AutoMode)
            { // 현재 Auto Mode 일때
                if (!autoRun || GlobalVar.UlTimeOut) // 11.04.27 minhan
                //if(autoRun) // 11.03.25 minhan
                {
                    refProcess = ProcessCondition.Stop;
                }
                else
                {
                    refProcess = ProcessCondition.Run;
                }
            }
            else if (!m_GenInfos.AutoMode)// Manual Mode 일때
            {
                if (m_GenInfos.DiStart)
                { // DiStart이면 Run
                    refProcess = ProcessCondition.Run;
                }
                else
                { // DiStop이면 Accept Manual Operation
                    if (previousAutoMode == true)
                    { //  AutoMode에서 ManualMode로 바뀌는 시점
                        refProcess = ProcessCondition.Stop;
                    }
                    else if (previousSemiAuto == true)
                    { //  DiStart에서 DiStop으로 바뀌는 시점
                        refProcess = ProcessCondition.Stop;
                    }
                    else
                    {
                        refProcess = ProcessCondition.DontCare;
                    }
                }
            }

            return refProcess;
        }
        #endregion

        #region FR Shower Control
        public static ProcessCondition FrShowerProcess(ProcessUnit unit, bool previousAutoMode, bool previousSemiAuto)
        {
            bool interlock = IsInterlock(unit);
            bool enable = IsRunEnable(unit);
            bool autoRun = IsAutoRunCondition(unit);
            autoRun &= (GetGlassCount() > 0 || (m_GenInfos.IdleRunning && GetGlassCount() == 0));

            ProcessCondition refProcess = ProcessCondition.Stop;
            if (interlock)
            { // Heavy interlock 조건이면 무조건 stop
                refProcess = ProcessCondition.Stop;
            }
            else if (!enable)
            { // Run 불가능한 상태이면 무조건 stop
                refProcess = ProcessCondition.Stop;
            }
            else if (m_GenInfos.AutoMode)
            { // 현재 Auto Mode 일때
                if (autoRun)
                { // Auto Run 상태이면 Run
                    refProcess = ProcessCondition.Run;
                }
                else
                {
                    refProcess = ProcessCondition.Stop;
                }
            }
            else if (!m_GenInfos.AutoMode)// Manual Mode 일때
            {
                if (m_GenInfos.DiStart)
                { // DiStart이면 Run
                    refProcess = ProcessCondition.Run;
                }
                else
                { // DiStop이면 Accept Manual Operation
                    if (previousAutoMode == true)
                    { //  AutoMode에서 ManualMode로 바뀌는 시점
                        refProcess = ProcessCondition.Stop;
                    }
                    else if (previousSemiAuto == true)
                    { //  DiStart에서 DiStop으로 바뀌는 시점
                        refProcess = ProcessCondition.Stop;
                    }
                    else
                    { // AutoValve Manual Control은 가능해야겠기에
                        refProcess = ProcessCondition.DontCare;
                    }
                }
            }

            return refProcess;
        }
        #endregion

        #region Gauge Interlock Condition Control
        public static bool GetGaugeInterlockCheckCondition(Gauge gauge) // 11.02.01 minhan 수정함.
        {
            bool check = true;
            string name = gauge.Name;

            if ((name == eqpGauges._Driving_Air_Pressure_Gauge_Name) ||
                (name == eqpGauges._RB_Unit_Up_AC_Gauge_Name) ||
                (name == eqpGauges._RB_Unit_Lo_AC_Gauge_Name))
            {
                check = (gauge.InterlockEnable && gauge.SetupInterlock.Use);
                return check; // 11.04.07 minhan
            }
            //else if ((name == eqpGauges._AP_Unit_CDA_Pressure_Gauge.Name) ||
            //        (name == eqpGauges._AP_Unit_N2_Pressure_Gauge.Name)) // 11.04.07 minhan
            //{
            //    check = (gauge.InterlockEnable && gauge.SetupInterlock.Use);
            //    check &= m_Server.JobCond.CurrentRecipe.ApUse;
            //    check &= eqpSeAps._SE_AP_Plasma_Unit.IsPowerOn();
            //    return check;
            //}
            //else if ((name == eqpGauges._AP_Unit_PCW_Out_Gauge.Name) ||
            //       (name == eqpGauges._AP_Unit_PCW_Pressure_Gauge.Name))
            //{
            //    check = (gauge.InterlockEnable && gauge.SetupInterlock.Use);
            //    check &= m_Server.JobCond.CurrentRecipe.ApUse;
            //    check &= eqpAutoValves._AP_PCW_In_Valve.IsOpen();
            //    return check;
            //}
            //else if (name == eqpGauges._AP_Unit_Power_Gauge.Name)
            //{
            //    check = (gauge.InterlockEnable && gauge.SetupInterlock.Use);
            //    check &= m_Server.JobCond.CurrentRecipe.ApUse;
            //    check &= eqpSeAps._SE_AP_Plasma_Unit.IsPowerOn();
            //    return check;
            //}

            check &= m_GenInfos.AutoMode;
            check &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            check &= m_Server.JobCond.ProcessMode;
            check &= (gauge.InterlockEnable && gauge.SetupInterlock.Use);
            check &= m_GenInfos.EqpInitComp;

            if (name == eqpGauges._RB_Unit_MJ_CDA_Pressure_Gauge_Name)
            {
                check &= m_GenInfos.DiStart;
                check &= eqpAutoValves._RB_MJ_CDA_In_Valve.IsOpen();
            }
            else if ((name == eqpGauges._RB_Unit_MJ_DI_Flow_Gauge_Name) ||
                        (name == eqpGauges._RB_Unit_AQK_Shower__Flow_Gauge_Name) ||
                     (name == eqpGauges._RB_Unit_AQK_Shower__Flow_Gauge_Name))
            {
                check &= m_GenInfos.DiStart;
                check &= eqpPumps._RB_Shower_Pump.SetupPumpUse.GetValue<bool>();
                check &= eqpPumps._RB_Shower_Pump.IsRun();
            }
            else if ((name == eqpGauges._FR_Unit_AQK_Flow_Gauge_Name) ||
           (name == eqpGauges._FR_Unit_Shower_Up_Flow_Gauge_Name) || (name == eqpGauges._FR_Unit_Shower_Low_Flow_Gauge_Name) || (name == eqpGauges._FR_Unit_Water_Resister_Gauge_Name))
            {
                check &= m_GenInfos.DiStart;
                check &= eqpAutoValves._FR_SHW_DI_In_Valve.IsOpen();
            }
            else if ((name == eqpGauges._AK_Unit_Up_Air_Knife_Flowrate_Gauge_Name) ||
                     (name == eqpGauges._AK_Unit_Up_Air_Knife_Pressure_Gauge_Name))
            {
                check &= eqpAutoValves._AK_UP_CDA_In_Valve.IsOpen();
            }
            else if ((name == eqpGauges._AK_Unit_Lo_Air_Knife_Flowrate_Gauge_Name) ||
                     (name == eqpGauges._AK_Unit_Lo_Air_Knife_Pressure_Gauge_Name))
            {
                check &= eqpAutoValves._AK_LO_CDA_In_Valve.IsOpen();
            }

            return check;
        }
        #endregion

        #region ServoUnit Interlock Condtion
        public static bool IsServoUnitInterlock(_ServoUnit unit, int targetPosition)
        {
            //kang Tr Robot Interrupt 추가
            bool interlockCondition = false;
            //bool DoorAlarm = false; // 11.03.02 minhan 
            //_GenericCollection<DoorSensor> m_DoorUnits = m_Server.ComponentContainer.GetCollection<DoorSensor>(); // 11.03.02 minhan


            if ((unit.Name == eqpServoUnits._TR_Servo_Unit_Name) &&
                eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected())
            {
                interlockCondition = true;
                //return interlockCondition; // 11.04.27 minhan
            }

            //foreach (DoorSensor door in m_DoorUnits) // 11.04.27 minhan
            //{
            //    if (door.Id != eqpDoorSensors._AP_Unit_Front__Mid_Door1_Sensor.Id ||
            //        door.Id != eqpDoorSensors._AP_Unit_Front__Mid_Door2_Sensor.Id ||
            //        door.Id != eqpDoorSensors._AP_Unit_Rear_Mid_Door1_Sensor.Id ||
            //        door.Id != eqpDoorSensors._AP_Unit_Rear_Mid_Door2_Sensor.Id
            //        //door.Id != eqpDoorSensors._AP_Unit_Spare_Door1_Sensor.Id ||
            //        /*door.Id != eqpDoorSensors._AP_Unit_Spare_Door2_Sensor.Id*/) // 11.03.07 minhan
            //    {
            //        DoorAlarm |= door.IsAlarm;
            //    }
            //}

            //if (DoorAlarm)
            //{
            //    interlockCondition = true;
            //    return interlockCondition;
            //}

            return interlockCondition;
        }
        #endregion

        //#region Ap InterLock
        //public static bool IsApInterlock(_Plasma unit) // 10.12.21 minhan
        //{
        //    SeAp Ap = eqpSeAps._SE_AP_Plasma_Unit;

        //    int alarm = 0;

        //    bool interlock = false;
        //    bool Houseclose = false;
        //    Houseclose = Ap.ActuatorUnit.IsNegative();
        //    Houseclose &= Ap.DiHouseClose.GetState();

        //    bool heavyInterlock = (m_Server.JobCond.HeavyInterlock > 0) ;

        //    if (!Ap.IsUse())
        //    {
        //        MessageBox.Show("Parameter Error! Please Plasma Use Check! ", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if (heavyInterlock)
        //    {
        //        MessageBox.Show("Heavy Interlock.. Please Check Ineterlock", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if (!Ap.PcwInValve.IsOpen())
        //    {
        //        MessageBox.Show("Please PCW SOL OPEN!", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if (!Houseclose)
        //    {
        //        MessageBox.Show("Please Please House Close", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if (((GlobalVar.ApManualN2Set < 700) || (GlobalVar.ApManualN2Set > 1500)) ||
        //        ((GlobalVar.ApManualCDASet < 0) || (GlobalVar.ApManualCDASet > 10)) ||
        //       ((GlobalVar.ApManualVolSet < 7) || (GlobalVar.ApManualVolSet > 13))) // 11.04.20 minhan
        //    {
        //        MessageBox.Show("Setting Value Error! Check Setting Value.", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if ((eqpGauges._AP_Unit_N2_Flow_Gauge.CurValue < 700) || 
        //        (eqpGauges._AP_Unit_CDA_Pressure_Gauge.CurValue < 0) ||
        //        (eqpGauges._AP_Unit_PCW_Out_Gauge.CurValue <= 3.5)) // 11.03.26 minhan
        //    {
        //        MessageBox.Show("N2, CDA Recv Current Valve Error!", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if (Ap.DiPCWFlowLowLimitAlarm.GetState() ||
        //        Ap.DiN2FlowLowLimitAlarm.GetState())
        //    {
        //        MessageBox.Show("Interlock Alarm! Please Plasma Status Check!", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    if ((eqpGauges._AP_Unit_CDA_Pressure_Gauge.IsAlarm) ||
        //       (eqpGauges._AP_Unit_N2_Pressure_Gauge.IsAlarm) ||
        //       (eqpGauges._AP_Unit_PCW_Pressure_Gauge.IsAlarm) ||
        //       (eqpGauges._AP_Unit_PCW_Out_Gauge.IsAlarm) ||
        //       (eqpGauges._AP_Unit_Power_Gauge.IsAlarm)) // Setup 에서 사용하는 게이지들 알람 체크
        //    {
        //        MessageBox.Show("Gauge Alarm! Please Gauge Check!", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }
        //    if (((alarm = Ap.GetStatusAlarm()) != (int)SeApAlarmIndex.plasmaNoAlm) || Ap.IsAlarm)
        //    {
        //        MessageBox.Show("Plasma Alarm! Please Plasma Status Check!", "WSSD", MessageBoxButtons.OK);
        //        interlock = true;
        //        return interlock;
        //    }

        //    return interlock;
        //}
        //#endregion
    }
}
