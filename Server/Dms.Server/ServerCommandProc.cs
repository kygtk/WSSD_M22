using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Dms.Common;
using System.Threading;
using Dms.Data;
using Dms.Device;
using Dms.Remoting;
using Dms.Sequence;
using Dms.Server;
using Dms.ServerCommon;

namespace Dms.Server
{
    public partial class ServerManager
    {
        #region field
        TagRecipe CurRecipe;//2009.09.16 kimgun
        ThreadTr_BOE_G8_DHDC trControl = null;
        //ThreadLd_Hand_BOE_G8_DHDC ldHandControl = null;
        ThreadUl_Hand_BOE_G8_DHDC ulHandControl = null;
        private GantryUnit m_GantryUnit = null;
        private ServerManager m_Server = null;
        #endregion

        #region CommandProc
        //params 0: DeviceTag's ID, 1: each unit's activity
        public void CommandProc(Command cmd, params Object[] para)
        {
            if (trControl == null) trControl = ThreadHandler.TrControl;
            //if (ldHandControl == null) ldHandControl = ThreadHandler.LdHandControl;
            if (ulHandControl == null) ulHandControl = ThreadHandler.UlHandControl;
            m_Server = ServerManager.Instance;
            try
            {
                switch (cmd)
                {
                    case Command.Auto:
                        {
                            if (BaseGlobalVar.Manualcompulsion) // 11.02.23 minhan device 다운시는 auto 모드로 전환되는 않도록.훗 아니라면 날리삼.
                            {
                                MessageBox.Show("Device Down!", "WSSD", MessageBoxButtons.OK);
                                return;
                            }
                            if (m_GenInfos.AutoMode) return;
                            if (MessageBox.Show("Do you want to change to auto mode?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            //if (false) // 11.02.01 minhan
                            //{
                            //    if (DialogResult.No == MessageBox.Show("현재 BYPASS 신호가 On 상태입니다. 사고의 우려가 있어 OFF 해 주십시요. 그래도 진행 하시겠습니까?", "WSSD",
                            //       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)) // 09.12.28 minhan
                            //    {
                            //        return;
                            //    }

                            //    string msg = "BYPASS ON SKIP AND AUTO MODE RUN!";
                            //    Log(msg);
                            //}

                            if (eqpTransferUnits._TR_Gantry_Unit.IsAlignFw() || !eqpTransferUnits._TR_Gantry_Unit.IsAlignBw()) // 09.11.12 minhan
                            {
                                MessageBox.Show("TR Align send to backward.", "WSSD", MessageBoxButtons.OK);
                                return;
                            }
                            if (!m_Server.GlassData.IsExist(eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0)) && eqpSensors._TR_Unit_Wait_Position_Check_Sensor.IsDetected())
                            {
                                MessageBox.Show("please check glass exist on TR unit", "WSSD", MessageBoxButtons.OK);
                                return;
                            }
                            //else if //(!eqpSRUs._Servo_SRU.SruByPass.GetState() && !this.Simul.Motion && daebak
                            //        //(!eqpSRUs._Servo_SRU.SruAct.GetState() ||
                            //        (((eqpServoUnitMp2300s._TR_Servo_Unit.GetRbtError() != 0) || 
                            //        (eqpServoUnitMp2300s._LD_Hand_Servo_Unit.GetRbtError() != 0) ||
                            //        (eqpServoUnitMp2300s._UL_Hand_Servo_Unit.GetRbtError() != 0))) // 09.11.12 minhan
                            //{
                            //    MessageBox.Show("서보 구동시 정상적인 상태가 아니므로, 서보나 Door의 상태 및 SRU 상태를 확인하세요.", "WSSD", MessageBoxButtons.OK);
                            //    return;
                            //}

                            short TRpos = (short)eqpServoUnits._TR_Servo_Unit.GetCurPointId(); // 09.12.01 minhan
                            //short Ldpos = (short)eqpServoUnits._LD_Hand_Servo_Unit.GetCurPointId();
                            short ULpos = (short)eqpServoUnits._UL_Hand_Servo_Unit.GetCurPointId();
                            bool checkSensor = true;

                            if ((TRpos == -1) || (ULpos == -1))
                            {
                                MessageBox.Show("Check the Servo position.", "WSSD", MessageBoxButtons.OK);
                                return;
                            }

                            if (!trControl.IsPositionConfirmed(TRpos)) checkSensor = false;
                            //if (!ldHandControl.IsPositionConfirmed(Ldpos)) checkSensor = false;
                            if (!ulHandControl.IsPositionConfirmed(ULpos)) checkSensor = false;

                            if (!checkSensor)
                            {
                                MessageBox.Show("Check the Servo position sensor.", "WSSD", MessageBoxButtons.OK);
                                return;
                            }

                            bool singleMode = this.SetupSingleMode.GetValue<bool>();

                            if (!m_GenInfos.CleanOut && !m_GenInfos.CycleStop && !singleMode && !GlobalVar.NoSubstrate && GlobalVar.LoaderReady) // 11.05.14 minhan
                            {
                                if ((TRpos == eqpPos_TR_Servo_Unit.Recv) && (ULpos == eqpPos_UL_Hand_Servo_Unit.Send3))
                                {
                                    MessageBox.Show("Check the TR Servo position. Please Wait or Home Move", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                            }

                            if (GlassData.Count > 0) m_GenInfos.Pause = true;
                            m_GenInfos.AutoMode = true;
                            GlobalVar.EqpStatusChangeReq = true;//2009.08.19 kimgun
                            //현재 사용중인 recipe로 복귀 2009.09.16 kimgun
                            if ((m_JobCondition.CurrentRecipe.Id != CurRecipe.Id) && (this.GlassData.Count > 0)) // 09.12.20 minhan
                            {
                                m_JobCondition.SetRecipe2JobCond(CurRecipe.Id);
                            }
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //2009.08.15 kimgun servo unit들 auto로 바뀌면 seqno를 0으로 되돌리자
                            ThreadHandler.TrControl.InitParameter();
                            //ThreadHandler.LdHandControl.InitParameter(); 
                            ThreadHandler.UlHandControl.InitParameter();
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                        break;
                    case Command.Manual:
                        {
                            if (!BaseGlobalVar.Manualcompulsion) // 11.02.23 minhan
                            {
                                if (!m_GenInfos.AutoMode) return;

                                if (this.m_GenInfos.LdInterfaceStatus == "LR" || this.m_GenInfos.UlInterfaceStatus == "UR") // 11.02.17 minhan
                                {
                                    if (MessageBox.Show("Now is Loader Interfacing!..Do you want to change to manual mode?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        string msg = "Loader Interfacing.. manual Change!";
                                        Log(msg);
                                    }
                                }
                                else
                                {
                                    if (MessageBox.Show("Do you want to change to manual mode?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                                }
                                m_GenInfos.AutoMode = false;

                                GlobalVar.EqpStatusChangeReq = true;//2009.08.19 kimgun
                                CurRecipe = m_JobCondition.CurrentRecipe;//2009.09.16 kimgun
                            }
                            else
                            {
                                string msg = "";

                                if (!m_GenInfos.AutoMode)
                                {
                                    //BaseGlobalVar.Manualcompulsion = false; // 이 플래그는 시스템 시퀀스에서 알람 해제시 false 하도록 하자.
                                    msg = "Manual Compulsion!";
                                    Log(msg);
                                    return;
                                }

                                //BaseGlobalVar.Manualcompulsion = false;

                                m_GenInfos.AutoMode = false;

                                GlobalVar.EqpStatusChangeReq = true;
                                CurRecipe = m_JobCondition.CurrentRecipe;

                                msg = "Manual Compulsion!";
                                Log(msg);
                            }
                        }
                        break;
                    case Command.CycleStart:
                        {
                            if (!m_GenInfos.CycleStop) return;
                            if (m_GenInfos.CleanOut)
                            {
                                MessageBox.Show("Can not change to cycle start. Now cleanout mode");
                                return;
                            }
                            if (MessageBox.Show("Do you want to change to cycle start?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            EquipmentType.CimProtocolType cimType = eqpEquipmentTypes._Equipment_Type.CimProtocol;
                            if (cimType == EquipmentType.CimProtocolType.Hsms)
                            {
                                GlobalVar.CycleStartReq = true;
                            }
                            else if (cimType == EquipmentType.CimProtocolType.Melsec)
                            {
                                m_GenInfos.CycleStop = false;
                            }
                        }
                        break;
                    case Command.CycleStop:
                        {
                            if (m_GenInfos.CycleStop) return;
                            //if (GlobalVar.rcvON_IF2 || GlobalVar.sndON_IF2) return; // 11.04.17 minhan
                            if (MessageBox.Show("Do you want to change to cycle stop?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            EquipmentType.CimProtocolType cimType = eqpEquipmentTypes._Equipment_Type.CimProtocol;
                            if (cimType == EquipmentType.CimProtocolType.Hsms)
                            {
                                GlobalVar.CycleStopReq = true;
                            }
                            else if (cimType == EquipmentType.CimProtocolType.Melsec)
                            {
                                m_GenInfos.CycleStop = true;
                            }
                        }
                        break;
                    case Command.Pause:
                        {
                            if (m_GenInfos.Pause)
                            {
                                if (MessageBox.Show("Do you want to restart?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                                if (m_GenInfos.CleanOut || SetupSingleMode.GetValue<bool>())    //cleanout일때 LD의 글라스는 택타임이 지나야 출발한다.
                                {
                                    //SeqFlag.TimeOver = false;
                                    JobCond.SetTactTime(TactTimeAct.ttCOUNT_START);
                                }
                                m_GenInfos.Pause = false;
                            }
                            else
                            {
                                if (MessageBox.Show("Do you want to pause?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                                m_GenInfos.Pause = true;
                            }
                        }
                        break;
                    case Command.OnlineControl:
                        {
                            if (this.SetupSingleMode.GetValue<bool>() == true)
                            {
                                MessageBox.Show("Can not change to Online Mode. Now Single Mode.", "WSSD", MessageBoxButtons.OK);
                                return;
                            }

                            if (GlobalVar.EqpCtlMode == '0') return;
                            if (MessageBox.Show("Do you want to change Online?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            EquipmentType.CimProtocolType cimType = eqpEquipmentTypes._Equipment_Type.CimProtocol;
                            if (cimType == EquipmentType.CimProtocolType.Hsms)
                            {   //HSMS type
                                GlobalVar.CtlMode = '0';
                                GlobalVar.CtlModeChangeReq = true;
                            }
                            else if (cimType == EquipmentType.CimProtocolType.Melsec)
                            {
                                //Melsec type
                                //jemoon : MelsecType은 여기서 바로 처리
                                GlobalVar.EqpCtlMode = '0';
                                m_GenInfos.OnlineMode = true;
                                m_GenInfos.OfflineMode = false;
                                GlobalVar.ModeChangeComp = true;
                            }
                        }
                        break;
                    case Command.OnlineLocal:
                        {
                        }
                        break;
                    case Command.Offline:
                        {
                            if (GlobalVar.EqpCtlMode == '2') return;
                            if (MessageBox.Show("Do you want to change Offline?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            EquipmentType.CimProtocolType cimType = eqpEquipmentTypes._Equipment_Type.CimProtocol;
                            if (cimType == EquipmentType.CimProtocolType.Hsms)
                            {   //HSMS type
                                GlobalVar.CtlMode = '2';
                                GlobalVar.CtlModeChangeReq = true;
                            }
                            else if (cimType == EquipmentType.CimProtocolType.Melsec)
                            {
                                //Melsec type
                                //jemoon : MelsecType은 여기서 바로 처리
                                GlobalVar.EqpCtlMode = '2';
                                m_GenInfos.OnlineMode = false;
                                m_GenInfos.OfflineMode = true;
                                GlobalVar.ModeChangeComp = false;
                            }
                        }
                        break;
                    case Command.NormalMode:
                        {
                            if (GlobalVar.OpMode == '1') return;
                            if (MessageBox.Show("Do you want to change Normal Mode?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            GlobalVar.ReqOpMode = '1';
                            GlobalVar.OpModeChangeReq = true;
                        }
                        break;
                    case Command.RecoveryMode:
                        {
                            if (GlobalVar.OpMode == '3') return;
                            if (MessageBox.Show("Do you want to change Recovery Mode?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            GlobalVar.ReqOpMode = '3';
                            GlobalVar.OpModeChangeReq = true;
                        }
                        break;
                    case Command.ParticleMode:
                        {
                            if (GlobalVar.OpMode == '6') return;
                            if (MessageBox.Show("Do you want to change Particle Mode?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                            GlobalVar.ReqOpMode = '6';
                            GlobalVar.OpModeChangeReq = true;
                        }
                        break;
                    case Command.LoopBack:
                        {
                            m_GenInfos.LoopBackTest = true;
                        }
                        break;
                    case Command.DiStart:
                        {
                            if (m_GenInfos.AutoMode || m_GenInfos.DiStart) return;

                            if (MessageBox.Show("Do you want to DI Start?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                            m_GenInfos.DiStart = true;
                        }
                        break;
                    case Command.DiStop:
                        {
                            if (m_GenInfos.AutoMode || !m_GenInfos.DiStart) return;

                            if (MessageBox.Show("Do you want to DI Stop?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                            m_GenInfos.DiStart = false;
                        }
                        break;
                    case Command.Resume:
                        {
                            string msg = string.Format("request resume");
                            m_GenInfos.Pause = false;
                        }
                        break;
                    case Command.Ready:
                        {
                            if (MessageBox.Show("Do you want to initialize the equipment?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                            m_GenInfos.EqpInitReq = true;
                            if (m_InitForm.IsDisposed)
                            {
                                m_InitForm = new FormInitStatus();
                            }
                            if (!m_InitForm.Initialized)
                            {
                                m_InitForm.Initialize(ThreadHandler.MainControl.SeqInitFunctions);
                            }

                            m_InitForm.Show();
                        }
                        break;
                    case Command.CurGlassCountReset:
                        {
                            m_GenInfos.CurGlassCount = 0;
                        }
                        break;
                    case Command.ManualIFrecv:
                        {
                            if (m_GenInfos.ManualIFAct == "" || m_GenInfos.ManualIFAct == "0")
                            {
                                if (DialogResult.Yes == MessageBox.Show("Do you want to start Recv I/F?", "Manual I/F", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification))
                                    m_GenInfos.ManualIFAct = "Manual Recv";
                            }
                            else if (m_GenInfos.ManualIFAct == "Manual Recv")
                            {
                                if (DialogResult.Yes == MessageBox.Show("Do you want to cancel Recv I/F?", "Manual I/F", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification))
                                    m_GenInfos.ManualIFAct = "Manual Recv Cancel";
                            }
                        }
                        break;
                    case Command.ManualIFsend:
                        {
                            if (m_GenInfos.ManualIFAct == "" || m_GenInfos.ManualIFAct == "0")
                            {
                                if (DialogResult.Yes == MessageBox.Show("Do you want to start Send I/F ?", "Manual I/F", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification))
                                    m_GenInfos.ManualIFAct = "Manual Send";
                            }
                            else if (m_GenInfos.ManualIFAct == "Manual Send")
                            {
                                if (DialogResult.Yes == MessageBox.Show("Do you want to cancel Send I/F ?", "Manual I/F", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification))
                                    m_GenInfos.ManualIFAct = "Manual Send Cancel";
                            }
                        }
                        break;
                    case Command.CvMotorManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            CvMotorAct act = (CvMotorAct)para[1];
                            int speed = (int)para[2];
                            SetCvMotorAct(deviceName, act, speed);
                        }
                        break;
                    case Command.PumpManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            PumpAct act = (PumpAct)para[1];

                            SetPumpAct(deviceName, act);
                        }
                        break;
                    case Command.RbMotorManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            RbMotorAct act = (RbMotorAct)para[1];
                            int speed = (int)para[2];

                            SetRbMotorAct(deviceName, act, speed);
                        }
                        break;
                    case Command.ActuatorManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            GantryUnit GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
                            //FishHand LdFishHand = eqpTransferUnits._LD_Fish_Hand;
                            FishHand UlFishHand = eqpTransferUnits._UL_Fish_Hand;
                            CvUnit LdCvUnit = eqpTransferUnits._LD_CvUnit;
                            CvUnit UlCvUnit = eqpTransferUnits._UL_CvUnit;
                            //Cylinder Ld_Aligner = eqpCylinders._LD_Align_Cylinder1;

                            short CurPositionIdTr = (short)GantryUnit.Servo.GetCurPointId();
                            //short CurPositionIdLd = (short)LdFishHand.Servo.GetCurPointId();
                            short CurPositionIdUl = (short)UlFishHand.Servo.GetCurPointId();

                            #region GlassExist GlassData
                            bool GlassExistTr = GantryUnit.IsGlassExist(Logic.OR);
                            //bool GlassExistLdFh = LdFishHand.IsGlassExist(Logic.OR);
                            bool GlassExistUlFh = UlFishHand.IsGlassExist(Logic.OR);
                            bool GlassExistLdIn = LdCvUnit.GlsInSensor.IsDetected(Logic.OR);
                            bool GlassExistLdOut = LdCvUnit.GlsOutSensor.IsDetected(Logic.OR);
                            bool GlassExistUlIn = UlCvUnit.GlsInSensor.IsDetected(Logic.OR);
                            bool GlassExistUlOut = UlCvUnit.GlsOutSensor.IsDetected(Logic.OR);

                            bool GlassDataTr = GlassData.IsExist(GantryUnit.DataMatchingKey(0));
                            //bool GlassDataLdFh = GlassData.IsExist(LdFishHand.DataMatchingKey(0));
                            bool GlassDataUlFh = GlassData.IsExist(UlFishHand.DataMatchingKey(0));
                            bool GlassDataLdIn = GlassData.IsExist(LdCvUnit.DataMatchingKey(0));
                            bool GlassDataLdOut = GlassData.IsExist(LdCvUnit.DataMatchingKey(1));
                            bool GlassDataUlIn = GlassData.IsExist(UlCvUnit.DataMatchingKey(0));
                            bool GlassDataUlOut = GlassData.IsExist(UlCvUnit.DataMatchingKey(1));
                            #endregion

                            bool LdCvMotorStop = (LdCvUnit.ManualAct[0] == CvMotorAct.Stop); // cv motor가 1개 이므로, 0
                            bool UlCvMotorStop = (UlCvUnit.ManualAct[0] == CvMotorAct.Stop); // cv motor가 1개 이므로, 0
                            string deviceName = (string)para[0];
                            ActuatorAct act = (ActuatorAct)para[1];

                            #region cylinder, align interlock
                            if ((deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_OP1_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_OP2_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_OP3_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_OP4_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_MT1_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_MT2_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_MT3_Name) ||
                                    (deviceName == eqpCylinders._LD_Idle_Roller_Cylinder_MT4_Name)) // 10.02.01 minhan
                            {
                                if (act == ActuatorAct.Neg)
                                {
                                    eqpActuatorUnits._LD_Idle_Roller.SetNegativeAct();
                                }
                                else if (act == ActuatorAct.Pos)
                                {
                                    eqpActuatorUnits._LD_Idle_Roller.SetPositiveAct();
                                }
                            }
                            if ((deviceName == eqpCylinders._LD_Tilting_Cylinder1_Name) ||
                                   (deviceName == eqpCylinders._LD_Tilting_Cylinder2_Name)) // 10.02.01 minhan
                            {
                                bool CheckSensor = this.GlassData.IsExist(eqpTransferUnits._LD_CvUnit.DataMatchingKey(1));
                                CheckSensor |= eqpTransferUnits._LD_CvUnit.GlsOutSensor.IsDetected(Logic.OR);

                                bool CheckEuvGlass = this.GlassData.IsExist(eqpTransferUnits._EUV_CvUnit.DataMatchingKey(0));
                                CheckEuvGlass |= eqpTransferUnits._EUV_CvUnit.GlsInSensor.IsDetected(Logic.OR);


                                bool RobotDetect = eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected();

                                if (eqpGauges._Driving_Air_Pressure_Gauge.IsAlarm) // 10.04.29 minhan
                                {
                                    MessageBox.Show("Driving Air Press Alarm!", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else if (CheckSensor)
                                {
                                    MessageBox.Show("In Cv Unit Out Sensor Glass Exist! Danger!", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else if ((CheckEuvGlass) /*&& ThreadCv_BOE_G6_DHDC.INSendPosComp*/) // 10.04.01 minhan
                                {
                                    MessageBox.Show("EUV CvUnit IN Glass Exist! Danger!", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else if (RobotDetect)
                                {
                                    MessageBox.Show("Loader Sensing. Danger !", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else
                                {
                                    _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                                    device.SetAct(act);
                                }
                                return;
                            }

                            //if ((act == ActuatorAct.Pos) && (deviceName == eqpCylinders._LD_Tilting_Cylinder_OP_Name |
                            //        deviceName == eqpCylinders._LD_Tilting_Cylinder_MT_Name))
                            //{
                            //    _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                            //    device.SetAct(act);
                            //}
                            //else if ((act == ActuatorAct.Neg) && (deviceName == eqpCylinders._LD_Tilting_Cylinder_OP_Name |
                            //        deviceName == eqpCylinders._LD_Tilting_Cylinder_MT_Name))
                            //{
                            //    _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                            //    device.SetAct(act);

                            //}
                            //if (deviceName == eqpCylinders._LD_Fish_Hand_Cylinder_OP_Name |
                            //    deviceName == eqpCylinders._LD_Fish_Hand_Cylinder_MT_Name)
                            //{

                            //    #region 공용 인터락
                            //    if (CurPositionIdTr == -1)
                            //    // TR 의 위치를 모를 경우
                            //    {
                            //        string Msg = "TR servo position is imprecise.Do you move still? (Yes:Move,No:Stop)?";
                            //        if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                            //        return;
                            //    }
                            //    if (CurPositionIdLd == -1)
                            //    // LD hand 위치를 모를 경우.
                            //    {
                            //        string Msg = "LD Fish Hand's position is imprecise.Do you move still? (Yes:Move,No:Stop)";
                            //        if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                            //        return;

                            //    }
                            //    if (GlassExistLdFh && (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Send2_Position) &&
                            //        (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Home_Position)) 
                            //    {
                            //        string Msg = "LD Fish Hand Glass Checking.";
                            //        MessageBox.Show(Msg, "WSSD" ,MessageBoxButtons.OK);
                            //        return;

                            //    }
                            //    if ((GlassExistLdIn | GlassDataLdIn | GlassExistLdOut | GlassDataLdOut) && 
                            //        (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Recv1_Position) &&
                            //        (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Recv2_Position) &&
                            //        (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
                            //        (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Send2_Position) &&
                            //        (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Home_Position))
                            //    // LD OUT Glass Check
                            //    {
                            //        if (GlassExistLdOut | GlassDataLdOut)
                            //        {
                            //            string Msg = "Glass exists to [LD C/V Out].Is no there interference?";
                            //            if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo)) return;
                            //        }
                            //        else
                            //        {
                            //            string Msg = "Please LD Out Glass Checking";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }
                            //    }
                            //    if (((CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) | trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Send_Position)) &
                            //       ((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv1_Position) |
                            //       (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv2_Position) |
                            //       (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position)))
                            //    {
                            //        string Msg = "Please  TR and Ld Hand Servo Position Interlock Checking. ";
                            //        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //        return;
                            //    } 
                            //    #endregion

                            //    if ((act == ActuatorAct.Pos) && 
                            //        (deviceName == eqpCylinders._LD_Fish_Hand_Cylinder_OP_Name |
                            //        deviceName == eqpCylinders._LD_Fish_Hand_Cylinder_MT_Name))
                            //    {

                            //        #region LD Front HAND UP INTERLOCK
                            //        if ( trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Send_Position) &
                            //            ((ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv1_Position) | 
                            //            (ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv2_Position) | 
                            //             ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv3_Position)))))
                            //        // 다시 센서를 체킹.               
                            //        {
                            //            string Msg = "Please  TR and Ld Hand Servo Position Interlock Checking.";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }

                            //        if ((LdFishHand.FrontRib.GetCurAct() == ActuatorAct.Pos |
                            //            LdFishHand.RearRib.GetCurAct() == ActuatorAct.Pos) &
                            //            !Simul.Device)
                            //        // 현재 Rib가 up 된 상황이면 리턴.
                            //        {
                            //            string Msg = "Now Rib Status is Up";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }
                            //        else if ((LdFishHand.FrontRib.DoFwSolenoid.GetState() == true &
                            //                 !LdFishHand.IsHandUp()) &
                            //                !Simul.Device)
                            //        // 출력은 나가고 있는 상태에서 센서가 on 되지 않을 경우.
                            //        {
                            //            string Msg = "Rib Output is On But Please Sensor Checking";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }
                            //        else
                            //        {
                            //         _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                            //           device.SetAct(act);
                            //        }
                            //        #endregion
                            //    }
                            //    else if (act == ActuatorAct.Neg &&
                            //        (deviceName == eqpCylinders._LD_Fish_Hand_Cylinder_OP_Name |
                            //        deviceName == eqpCylinders._LD_Fish_Hand_Cylinder_MT_Name))
                            //    {
                            //        #region LD Front HAND DOWN INTERLOCK
                            //        if ( trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Send_Position) &
                            //            ((ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv1_Position) |
                            //            (ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv2_Position) |
                            //            ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv3_Position)))))
                            //        // 다시 센서를 체킹.               
                            //        {
                            //            string Msg = "Please  TR and Ld Hand Servo Position Interlock Checking.";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }
                            //        if ((LdFishHand.FrontRib.GetCurAct() == ActuatorAct.Neg |
                            //            LdFishHand.RearRib.GetCurAct() == ActuatorAct.Neg) &
                            //            !Simul.Device)
                            //        // 현재 Rib가 Down 된 상황이면 리턴.
                            //        {
                            //            string Msg = "Now Rib Status is Down";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }
                            //        else if (((LdFishHand.FrontRib.DoBwSolenoid.GetState() == true ) &
                            //                  !LdFishHand.IsHandDown()) &
                            //                !Simul.Device)
                            //        // 출력은 나가고 있는 상태에서 센서가 on 되지 않을 경우.
                            //        {
                            //            string Msg = "Rib Output is On But Please Sensor Checking";
                            //            MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                            //            return;
                            //        }
                            //        else
                            //        {
                            //            _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                            //            device.SetAct(act);
                            //        }
                            //        #endregion
                            //    }

                            //}

                            if (deviceName == eqpCylinders._UL_Fish_Hand_Cylinder_OP_Name |
                                   deviceName == eqpCylinders._UL_Fish_Hand_Cylinder_MT_Name)
                            {
                                #region 공용 인터락
                                if (CurPositionIdTr == -1)
                                // TR 의 위치를 모를 경우
                                {
                                    string Msg = "TR's position is imprecise.Do you move still? (Yes:Move,No:Stop)";
                                    if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                                        return;
                                }
                                if (CurPositionIdUl == -1)
                                // LD hand 위치를 모를 경우.
                                {
                                    string Msg = "UL Fish Hand's position is imprecise.Do you move still? (Yes:Move,No:Stop)";
                                    if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                                        return;

                                }
                                if (GlassExistUlFh && (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Recv1) &&
                                   (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Home))

                                // glass is exist
                                {
                                    string Msg = "Ul Fish Hand Glass Checking.";
                                    MessageBox.Show(Msg, "WSSD");
                                    return;

                                }
                                if ((GlassExistUlOut | GlassExistUlIn | GlassDataUlIn | GlassDataUlOut) &&
                                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Send1) &&
                                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Send2) &&
                                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Send3) &&
                                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Recv1) &&
                                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Home))
                                // LD OUT Glass Check
                                {
                                    if (GlassExistUlIn | GlassDataUlIn)
                                    {
                                        string Msg = "Glass exists to [UL C/V IN].Is no there interference??";
                                        if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo)) return;
                                    }
                                    else
                                    {
                                        string Msg = "Please UL OUT Glass Checking";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                }
                                if (((CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) | trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Recv)) &
                                   ((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send1) |
                                   (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send2) |
                                   (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3)))

                                {
                                    string Msg = "Please  TR and Ul Hand Servo Position Interlock Checking. ";
                                    MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                #endregion

                                if ((act == ActuatorAct.Pos) && (deviceName == eqpCylinders._UL_Fish_Hand_Cylinder_OP_Name |
                                    deviceName == eqpCylinders._UL_Fish_Hand_Cylinder_MT_Name))
                                {

                                    #region UL Front HAND UP INTERLOCK
                                    if ((trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Recv)) &
                                        (ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Send1) |
                                        ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Send2) |
                                        ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Send3)))

                                    // 다시 센서를 체킹.               
                                    {
                                        string Msg = "TR servo position is receive position.and UL Fish Hand's position is imprecise.";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                    else if ((UlFishHand.FrontRib.GetCurAct() == ActuatorAct.Pos |
                                        UlFishHand.RearRib.GetCurAct() == ActuatorAct.Pos) &
                                        !AppConfig.Instance.Simul.Device)
                                    // 현재 Rib가 up 된 상황이면 리턴.
                                    {
                                        string Msg = "Now Rib Status is Up";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                    else if (((UlFishHand.FrontRib.IsSetFw() == true |
                                             UlFishHand.RearRib.IsSetFw() == true) &
                                             !UlFishHand.IsHandUp()) & !AppConfig.Instance.Simul.Device)
                                    // 출력은 나가고 있는 상태에서 센서가 on 되지 않을 경우.
                                    {
                                        string Msg = "Rib Output is On But Please Sensor Checking";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                    else
                                    {
                                        _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                                        device.SetAct(act);
                                    }
                                    #endregion
                                }

                                else if (act == ActuatorAct.Neg && (deviceName == eqpCylinders._UL_Fish_Hand_Cylinder_OP_Name |
                                    deviceName == eqpCylinders._UL_Fish_Hand_Cylinder_MT_Name))
                                {
                                    #region UL Front HAND DOWN INTERLOCK
                                    if ((trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Recv)) &
                                        (ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Send1) |
                                        ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Send2) |
                                        ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Send3)))
                                    {
                                        string Msg = "TR servo position is receive position.and UL Fish Hand's position is imprecise..";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                    else if ((UlFishHand.FrontRib.GetCurAct() == ActuatorAct.Neg |
                                            UlFishHand.FrontRib.GetCurAct() == ActuatorAct.Neg) &
                                            !AppConfig.Instance.Simul.Device)
                                    // 현재 Rib가 Down 된 상황이면 리턴.
                                    {
                                        string Msg = "Now Rib Status is Down";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                    else if ((UlFishHand.FrontRib.IsSetBw() |
                                              UlFishHand.RearRib.IsSetBw()) &
                                              !UlFishHand.IsHandDown() & !AppConfig.Instance.Simul.Device)
                                    // 출력은 나가고 있는 상태에서 센서가 on 되지 않을 경우.
                                    {
                                        string Msg = "Rib Output is On But Please Sensor Checking";
                                        MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                        return;
                                    }
                                    else
                                    {
                                        _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                                        device.SetAct(act);
                                    }
                                    #endregion
                                }

                            }
                            else if ((deviceName == eqpCylinders._LD_Align_Cylinder_OP1_Name) ||
                                (deviceName == eqpCylinders._LD_Align_Cylinder_OP2_Name) ||
                                (deviceName == eqpCylinders._LD_Align_Cylinder_OP3_Name) ||
                                (deviceName == eqpCylinders._LD_Align_Cylinder_MT1_Name) ||
                                (deviceName == eqpCylinders._LD_Align_Cylinder_MT2_Name) ||
                                (deviceName == eqpCylinders._LD_Align_Cylinder_MT3_Name)) // 09.09.11 minhan  
                            {
                                #region LD Align INTERLOCK

                                bool RobotDetect = eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected();
                                if (!LdCvMotorStop)
                                // LD Cv Check
                                {
                                    string Msg = "Please LD Cv Checking";
                                    MessageBox.Show(Msg, "WSSD", MessageBoxButtons.OK);
                                    return;
                                }

                                if (eqpGauges._Driving_Air_Pressure_Gauge.IsAlarm) // 10.04.29 minhan
                                {
                                    MessageBox.Show("Driving Air Press Alarm!", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else if (RobotDetect)
                                {
                                    MessageBox.Show("Loader Sensing. Danger !", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else
                                {
                                    _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                                    device.SetAct(act);
                                }
                                #endregion
                            }
                            else if ((deviceName == eqpCylinders._TR_Align_Cylinder1_Name) ||
                                    (deviceName == eqpCylinders._TR_Align_Cylinder2_Name) ||
                                    (deviceName == eqpCylinders._TR_Align_Cylinder3_Name) ||
                                    (deviceName == eqpCylinders._TR_Align_Cylinder4_Name)) // 09.10.19 minhan 
                            {
                                if ((CurPositionIdTr == -1) ||
                                    CurPositionIdTr == eqpPos_TR_Servo_Unit.Home ||
                                    (trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Home)))

                                // TR 의 위치를 모를 경우
                                {
                                    string Msg;
                                    if (CurPositionIdTr == -1)
                                        Msg = "TR's position is imprecise.Do you move still? (Yes:Move,No:Stop)";
                                    else if (act == ActuatorAct.Pos)
                                        Msg = "Do not move at the Tr servo Home Position.";
                                    else
                                        Msg = "Do not move at the Tr servo Home Position. Do you move still? (Yes:Move,No:Stop)?";
                                    if (act == ActuatorAct.Neg)
                                    {
                                        if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            Log(Msg);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(Msg);
                                        return;
                                    }
                                }

                                if (GantryUnit.IsRobotInterlock())
                                {
                                    string Msg = "Loader hand is interfered. Do you want to act still?(Yes:Move,No:Stop)?";
                                    if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Log(Msg);
                                    }
                                }

                                if (GlassExistTr || GlassDataTr) // 11.02.25 minhan
                                {
                                    string Msg = "Glass exist on TR Unit. Do Act Align !! Do you want to act still?(Yes:Move,No:Stop) ?";
                                    if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Log(Msg);
                                    }
                                }

                                //if (GlobalVar.GlsRecvInterlockAlarm || GlobalVar.GlsSendInterlockAlarm || GlobalVar.GlsExChangeInterlockAlarm) // 인터락 알람이 발생한 경우
                                //{// 11.02.01 minhan 현재는 사용하지 않는다.
                                //    string Msg = "Loader Interface alarm occurred. Do you want to act still?(Yes:Move,No:Stop)?";
                                //    if (DialogResult.No == MessageBox.Show(Msg, "WSSD", MessageBoxButtons.YesNo))
                                //    {
                                //        return;
                                //    }
                                //    else
                                //    {
                                //        Log(Msg);
                                //    }
                                //}
                                _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                                device.SetAct(act);
                            }
                            else
                            {
                                _Actuator device = m_DmsComponents[deviceName] as _Actuator;
                                device.SetAct(act);
                            }
                            #endregion
                        }
                        break;
                    case Command.ActuatorTurnManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            ActuatorAct act = (ActuatorAct)para[1];

                            _ActuatorTurn device = m_DmsComponents[deviceName] as _ActuatorTurn;

                            if (act == ActuatorAct.SetRefPosition)
                            {
                                int positionId = (int)para[2];
                                device.SetRefPoint(positionId);
                            }
                            else
                            {
                                device.SetAct(act);
                            }
                        }
                        break;
                    case Command.AutoValveManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            AutoValveAct act = (AutoValveAct)para[1];
                            _GenericCollection<AutoValve> units = m_DmsComponents.ComponentContainer.GetCollection<AutoValve>();
                            //units[deviceName].SetAutoValveAct(act); // 11.02.08 minhan

                            #region Fields

                            TankUnit m_TankUnit;
                            PumpUnit m_PumpUnit;

                            #endregion

                            m_TankUnit = eqpTankUnits._FR_Unit_Tank;
                            m_PumpUnit = eqpPumpUnits._RB_SHW_Pump_Unit;

                            #region Interlock

                            if (deviceName == m_TankUnit.AutoValve.Name) // 11.02.01 minhan
                            {
                                if (m_PumpUnit.Pump.IsAlarm() && (act == AutoValveAct.Open))
                                {
                                    if (DialogResult.No == MessageBox.Show("Now is PUMP Alarm Do you want Running?  ", "WSSD", MessageBoxButtons.YesNo))
                                        return;
                                }

                                if (m_TankUnit.AutoValve.IsOpen() && (act == AutoValveAct.Open))
                                {
                                    MessageBox.Show("Now Value is Open.", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                                else if ((m_TankUnit.SupplyStop || m_TankUnit.TopLevel) && (act == AutoValveAct.Open))
                                {
                                    MessageBox.Show("Tank Level H or HH. Can not Value Open!", "WSSD", MessageBoxButtons.OK);
                                    return;
                                }
                            }
                            #endregion

                            units[deviceName].SetAutoValveAct(act);
                        }
                        break;
                    case Command.GlassDataCreate:
                        {
                            #region GlassData Create Update
                            bool EnableCreate = false;
                            int position = (int)para[0];

                            _GenericCollection<GantryUnit> GantryUnits = ThreadGantryControl.Units; // minhan
                            _GenericCollection<FishHand> FishHandUnits = ThreadHandControl.Units; // minhan
                            _GenericCollection<CvUnit> cvUnits = ThreadCvControl.Units;

                            if (!EnableCreate)
                            {
                                foreach (GantryUnit unit in GantryUnits) // minhan
                                {
                                    if (position == unit.DataMatchingKey(0))
                                    {

                                        if (unit.IsGlassExist(Logic.OR) || eqpSensors._TR_Unit_Wait_Position_Check_Sensor.IsDetected() || eqpSensors._TR_Unit_Recv_Position_Check_Sensor.IsDetected()) EnableCreate = true;
                                        if (AppConfig.Instance.Simul.Device) unit.GlassExistSensor.SetState(true);
                                    }
                                }
                            }
                            if (!EnableCreate)
                            {
                                foreach (FishHand unit in FishHandUnits)
                                {

                                    if (position == unit.DataMatchingKey(0))
                                    {
                                        if (unit.IsGlassExist(Logic.OR)) EnableCreate = true;
                                        if (AppConfig.Instance.Simul.Device) unit.GlassExistSensor.SetState(true, Logic.AND);
                                    }
                                }
                            }

                            if (!EnableCreate)
                            {
                                foreach (CvUnit unit in cvUnits)
                                {
                                    if ((position == unit.DataMatchingKey(0)) || (position == unit.DataMatchingKey(1)))
                                    {
                                        if (position == unit.DataMatchingKey(0))
                                        {
                                            if (unit.GlsInSensor.IsDetected(Logic.OR))
                                            {
                                                EnableCreate = true;
                                            }
                                            if (AppConfig.Instance.Simul.Device)
                                            {
                                                if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                                    unit.Id == eqpTransferUnits._FR_CvUnit.Id ||
                                                    unit.Id == eqpTransferUnits._AK_CvUnit.Id)
                                                {
                                                    unit.GlsInSensor.SetState(true, Logic.AND); // 09.09.08 minhan
                                                }
                                                else unit.GlsInSensor.SetState(true); // 09.09.08 minhan
                                            }

                                        }
                                        else
                                        {
                                            if (unit.GlsOutSensor.IsDetected(Logic.OR))
                                            {
                                                EnableCreate = true;
                                            }
                                            if (AppConfig.Instance.Simul.Device)
                                            {
                                                if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                                    unit.Id == eqpTransferUnits._FR_CvUnit.Id)
                                                {
                                                    unit.GlsOutSensor.SetState(true, Logic.AND); // 09.09.08 minhan
                                                }
                                                else unit.GlsOutSensor.SetState(true); // 09.09.08 minhan

                                                if (unit.Id == eqpTransferUnits._UL_CvUnit.Id) eqpTransferUnits._UL_CvUnit.FwDecelSensor.SetState(true); // 11.02.01 minhan
                                            }

                                        }
                                    }
                                }
                            }

                            if (AppConfig.Instance.Simul.Device)
                            {
                                EnableCreate = true;
                            }


                            if (EnableCreate)
                            {
                                //TODO : 이 파라미터들도 다 넘겨받도록.
                                TagGlassData glsData = new TagGlassData();
                                glsData.PositionId = position;
                                //glsData.RecipeId = "1";
                                //glsData.Item.GlassId = "GLS";
                                //glsData.Item.GlassNumberCode.LotNo = 1;
                                //glsData.Item.GlassNumberCode.SlotNo = 1;
                                //glsData.Item.UpdateData(glsData.Item);  //for CIM Test
                                m_GlassData.Create(glsData);

                                //if (this.Simul.Device) // minhan
                                //{
                                //    foreach (CvUnit unit in cvUnits)
                                //    {
                                //        if (unit.GlsInSensor.Id == position) unit.GlsInSensor.DiSensor.SetState(true);
                                //        else if (unit.GlsOutSensor.Id == position) unit.GlsOutSensor.DiSensor.SetState(true);
                                //    }
                                //}

                                if (position == eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0)) // 11.02.09 minhan
                                {
                                    GlobalVar.TrGlsSendReady = false;
                                    GlobalVar.LdIFStatus = "--";
                                    GlobalVar.UlIFStatus = "--";
                                    GlobalVar.rcvIfResetReq = true;
                                    GlobalVar.sndIfResetReq = true;
                                    GlobalVar.TrCrackResetReq = true; // 11.06.01 minhan
                                    GlobalVar.LoaderCrackResetReq = true;
                                }

                                ThreadHandler.CvControl.InitParameter(); // 09.10.26 minhan

                                _GenericCollection<GlsSensor> sensors = m_DmsComponents.ComponentContainer.GetCollection<GlsSensor>();
                                string msg = string.Format("Glass data Create  : {0}", sensors[position].Name); // 11.02.01 minhan
                                Log(msg);
                            }
                            else
                            {
                                MessageBox.Show("Create Failed : Glass isn't sensing");
                            }
                            #endregion
                        }
                        break;
                    case Command.GlassDataDeleteConfirm: // 11.02.01 minhan 사용하지 않음
                        {
                            #region 제거
                            //int position = (int)para[0];

                            ////이건뭐지?
                            ////GlobalVar.LostGlassId = position;// this.SeqFlag.LostGlassId = position;
                            ////GlobalVar.GlassDataLostReq = true;// this.SeqFlag.GlassDataLostReq = true;

                            //if (position == eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0))
                            //{
                            //    GlobalVar.TrGlsSendReady = false;
                            //}

                            ////Delete GlassData;
                            //m_GlassData.Delete(position);

                            //_GenericCollection<CvUnit> cvUnits = ThreadCvControl.Units;

                            //if (this.Simul.Device)
                            //{
                            //    DualGlsSensor GlsInsensor;
                            //    DualGlsSensor GlsOutSensor;
                            //    foreach (CvUnit unit in cvUnits)
                            //    {
                            //        GlsInsensor = unit.GlsInSensor as DualGlsSensor;
                            //        GlsOutSensor = unit.GlsOutSensor as DualGlsSensor;
                            //        GlsInsensor.DiSensor.SetState(false);
                            //        GlsInsensor.DiSensorOp.SetState(false);
                            //        GlsOutSensor.DiSensor.SetState(false);
                            //        GlsOutSensor.DiSensorOp.SetState(false);
                            //        //unit.GlsInSensor.DiSensor.SetState(false);
                            //        //unit.GlsOutSensor.DiSensor.SetState(false);
                            //    }

                            //    eqpTransferUnits._TR_Gantry_Unit.GlassExistSensor.SetState(false, Logic.AND);
                            //    eqpTransferUnits._LD_Fish_Hand.GlassExistSensor.SetState(false, Logic.AND);
                            //    eqpTransferUnits._UL_Fish_Hand.GlassExistSensor.SetState(false, Logic.AND);
                            //}

                            //ThreadHandler.CvControl.InitParameter();
                            //ThreadHandler.TrControl.InitParameter();
                            //ThreadHandler.LdHandControl.InitParameter();
                            //ThreadHandler.UlHandControl.InitParameter();
                            #endregion
                        }
                        break;
                    case Command.GlassDataDelete: // 11.02.01 minhan
                        {
                            bool EnableDelete = true;

                            m_JobCondition.SetTactTime(TactTimeAct.ttCOUNT_END);
                            m_JobCondition.SetTactTime(TactTimeAct.ttCOUNT_START);
                            //DualGlsSensor dualGlsInSensor;
                            //DualGlsSensor dualGlsOutSensor;
                            //GlsSensor GlsInSensor;
                            //GlsSensor GlsOutSensor;

                            int position = (int)para[0];

                            //if (position == eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0) ||
                            //    position == eqpTransferUnits._LD_Fish_Hand.DataMatchingKey(0) ||
                            //    position == eqpTransferUnits._UL_Fish_Hand.DataMatchingKey(0))
                            //{
                            //    GlobalVar.ScrapUnitNo = position;
                            //}
                            //else
                            //{
                            _GenericCollection<GantryUnit> gantryUnits = ThreadGantryControl.Units;

                            foreach (GantryUnit unit in gantryUnits) // 11.02.01 minhan
                            {
                                if (position == unit.DataMatchingKey(0))
                                {
                                    if (unit.GlassExistSensor.IsDetected(Logic.OR) ||
                                        eqpSensors._TR_Unit_Recv_Position_Check_Sensor.IsDetected() ||
                                        eqpSensors._TR_Unit_Wait_Position_Check_Sensor.IsDetected())
                                    {
                                        EnableDelete = false;
                                        MessageBox.Show("glass exist,can not delete glass data");
                                    }
                                    if (!unit.diRecv_Pos_Sensor.IsDetected() &&
                                        !unit.diSend_Pos_Sensor.IsDetected() &&
                                        !unit.diWait_Pos_Sensor.IsDetected())
                                    {
                                        EnableDelete = false;
                                        MessageBox.Show("please check TR position1");
                                    }
                                }
                            }
                            _GenericCollection<FishHand> handUnits = ThreadHandControl.Units;

                            foreach (FishHand unit in handUnits)
                            {
                                if (position == unit.DataMatchingKey(0))
                                {
                                    if (unit.GlassExistSensor.IsDetected(Logic.OR))
                                    {
                                        EnableDelete = false;
                                    }
                                }
                            }

                            _GenericCollection<CvUnit> cvUnits = ThreadCvControl.Units;

                            foreach (CvUnit unit in cvUnits)
                            {
                                if (position == unit.DataMatchingKey(0))
                                {
                                    ////GlobalVar.ScrapUnitNo = position; // 11.02.01 minhan
                                    //if (cvUnits.Name == eqpTransferUnits._RB_CvUnit_Name ||
                                    //   cvUnits.Name == eqpTransferUnits._FR_CvUnit_Name ||
                                    //   cvUnits.Name == eqpTransferUnits._AK_CvUnit_Name)
                                    //{
                                    //    dualGlsInSensor = unit.GlsInSensor as DualGlsSensor;
                                    //    if (dualGlsInSensor.IsDetected(Logic.AND))
                                    //    {
                                    //        EnableDelete = false;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    GlsInSensor = unit.GlsInSensor as GlsSensor;
                                    //    if (GlsInSensor.IsDetected(Logic.OR))
                                    //    {
                                    //        EnableDelete = false;
                                    //    }
                                    //} 
                                    if (unit.GlsInSensor.IsDetected(Logic.OR))
                                    {
                                        EnableDelete = false;
                                    }

                                }
                                else if (position == unit.DataMatchingKey(1))
                                {
                                    ////GlobalVar.ScrapUnitNo = position;
                                    //if (cvUnits.Name == eqpTransferUnits._RB_CvUnit_Name ||
                                    //   cvUnits.Name == eqpTransferUnits._FR_CvUnit_Name)
                                    //{
                                    //    dualGlsOutSensor = unit.GlsOutSensor as DualGlsSensor;
                                    //    if (dualGlsOutSensor.IsDetected(Logic.AND))
                                    //    {
                                    //        EnableDelete = false;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    GlsOutSensor = unit.GlsOutSensor as GlsSensor;
                                    //    if (GlsOutSensor.IsDetected(Logic.OR))
                                    //    {
                                    //        EnableDelete = false;
                                    //    }
                                    //}
                                    if (unit.GlsInSensor.IsDetected(Logic.OR))
                                    {
                                        EnableDelete = false;
                                    }
                                }
                            }
                            //}

                            if (AppConfig.Instance.Simul.Device)
                            {
                                EnableDelete = true;
                            }

                            if (EnableDelete)
                            {
                                //TagGlassData glassData = new TagGlassData();
                                //if (m_GlassData.GetData(position, ref glassData))
                                //{
                                //    glassData.GlassTypeNum |= (int)GlassTypeFlags.GlassDataScraped;
                                //    m_GlassData.Update(position, glassData);
                                //}
                                //GlobalVar.GlassScrapReport = true; // 11.02.01 minhan
                                //Delete GlassData;

                                m_GlassData.Delete(position);

                                if (position == eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0)) // 11.02.01 minhan
                                {
                                    GlobalVar.TrGlsSendReady = false;
                                    GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                                    GlobalVar.UlIFStatus = "--";
                                    GlobalVar.rcvIfResetReq = true;
                                    GlobalVar.sndIfResetReq = true;
                                    GlobalVar.TrCrackResetReq = true; // 11.06.01 minhan
                                    GlobalVar.LoaderCrackResetReq = true;
                                }

                                ThreadHandler.CvControl.InitParameter(); // 11.02.01 minhan
                                ThreadHandler.TrControl.InitParameter();
                                // ThreadHandler.LdHandControl.InitParameter();
                                ThreadHandler.UlHandControl.InitParameter();


                                #region Simul
                                if (AppConfig.Instance.Simul.Device) // 11.02.01 minhan
                                {
                                    foreach (GantryUnit unit in gantryUnits)
                                    {
                                        if (position == unit.DataMatchingKey(0))
                                        {
                                            if (unit.GlassExistSensor.IsDetected())
                                            {
                                                unit.GlassExistSensor.SetState(false);
                                            }
                                        }
                                    }

                                    foreach (FishHand unit in handUnits)
                                    {
                                        if (position == unit.DataMatchingKey(0))
                                        {
                                            if (unit.GlassExistSensor.IsDetected(Logic.OR))
                                            {
                                                unit.GlassExistSensor.SetState(false, Logic.AND);
                                            }
                                        }
                                    }

                                    foreach (CvUnit unit in cvUnits)
                                    {
                                        if (!this.GlassData.IsExist(unit.DataMatchingKey(0)))
                                        {
                                            if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                                unit.Id == eqpTransferUnits._FR_CvUnit.Id ||
                                                unit.Id == eqpTransferUnits._AK_CvUnit.Id)
                                            {
                                                unit.GlsInSensor.SetState(false, Logic.AND); // 09.09.08 minhan
                                            }
                                            else unit.GlsInSensor.SetState(false); // 09.09.08 minhan
                                        }

                                        if (!this.GlassData.IsExist(unit.DataMatchingKey(1)))
                                        {
                                            if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                               unit.Id == eqpTransferUnits._FR_CvUnit.Id)
                                            {
                                                unit.GlsOutSensor.SetState(false, Logic.AND); // 09.09.08 minhan
                                            }
                                            else unit.GlsOutSensor.SetState(false); // 09.09.08 minhan

                                            if (unit.FwDecelSensor != null) unit.FwDecelSensor.SetState(false);
                                            if (unit.BwDecelSensor != null) unit.BwDecelSensor.SetState(false);
                                        }
                                    }
                                }
                                #endregion 

                                _GenericCollection<GlsSensor> sensors = m_DmsComponents.ComponentContainer.GetCollection<GlsSensor>();
                                string msg = string.Format("Glass data delete  : {0}", sensors[position].Name); // 11.02.01 minhan
                                Log(msg);
                            }
                            else
                            {
                                MessageBox.Show("Delete Failed : Glass is still sensing");
                            }
                        }
                        break;
                    case Command.GlassDataEdit: // 11.02.09 minhan
                        {
                            try
                            {
                                int positionID = (int)para[0];
                                bool Ng = false;
                                //int i = 0;

                                Dictionary<string, string> m_DisplayedItem = (Dictionary<string, string>)para[1];

                                TagGlassData tagGlassData = new TagGlassData();

                                foreach (KeyValuePair<string, string> item in m_DisplayedItem)
                                {
                                    if (string.Equals(item.Key, "Port No", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tagGlassData.PortID = Convert.ToInt16((string)item.Value).ToString();
                                    }
                                    else if (string.Equals(item.Key, "Slot No", StringComparison.OrdinalIgnoreCase))
                                    {
                                        int nbuf;

                                        if (int.TryParse((string)item.Value, out nbuf))
                                        {
                                            tagGlassData.SlotID = (string)item.Value;
                                        }
                                        else Ng = true;
                                    }
                                    else if (string.Equals(item.Key, "Recipe ID", StringComparison.OrdinalIgnoreCase))
                                    {

                                        int m_value = 0;

                                        m_value = Convert.ToInt16(item.Value);
                                        string m_recipe = string.Format("{0:d4}", m_value);

                                        if (this.DataProvider.RecipeProvider.Adapter.IsExist(m_recipe) == false)
                                        {
                                            Ng = true;
                                        }
                                        else
                                        {
                                            tagGlassData.RecipeID = m_recipe;
                                        }
                                    }
                                    else if (string.Equals(item.Key, "Processed", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tagGlassData.Processed = Convert.ToBoolean((string)item.Value);
                                    }
                                    else if (string.Equals(item.Key, "Glass ID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tagGlassData.GlassID = item.Value;
                                    }
                                    else if (string.Equals(item.Key, "Cst ID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tagGlassData.CstID = item.Value;
                                    }
                                    else if (string.Equals(item.Key, "Lot ID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tagGlassData.LotID = item.Value;
                                    }
                                    else if (string.Equals(item.Key, "TRCrackAlarm", StringComparison.OrdinalIgnoreCase)) // 11.06.01 minhan
                                    {
                                        tagGlassData.TRCrackAlarm = Convert.ToBoolean((string)item.Value);
                                    }
                                    else if (string.Equals(item.Key, "LoaderCrackAlarm", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tagGlassData.LoaderCrackAlarm = Convert.ToBoolean((string)item.Value);
                                    }

                                }

                                if (Ng)
                                {
                                    string msg = "Glass Data Format Error!";
                                    MessageBox.Show(msg);
                                }
                                else
                                {
                                    if (positionID == eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0)) // 11.06.07 minhan
                                    {
                                        GlobalVar.TrGlsSendReady = false;
                                        GlobalVar.LdIFStatus = "--";
                                        GlobalVar.UlIFStatus = "--";
                                        GlobalVar.rcvIfResetReq = true;
                                        GlobalVar.sndIfResetReq = true;
                                        GlobalVar.TrCrackResetReq = true;
                                        GlobalVar.LoaderCrackResetReq = true;
                                    }

                                    tagGlassData.PositionId = positionID;
                                    tagGlassData.Clone(tagGlassData); // 10.05.19 minhan
                                    this.GlassData.Update(positionID, tagGlassData);

                                    string msg = string.Format("Glass data Edit : {0} ", positionID.ToString());
                                    Log(msg);
                                }
                            }
                            catch (Exception err)
                            {
                                string msg = "Glass Data Format Error!";
                                MessageBox.Show(msg);
                                this.WriteExceptionLog(err.ToString());
                            }

                            //// para[0] - positionId : int 11.02.09 minhan
                            //// para[1] - editData : Dictionary<string, string>
                            //int positionId = (int)para[0];
                            ////TagGlassData editData = para[1] as TagGlassData;
                            //Dictionary<string, string> editData = (Dictionary<string, string>)para[1];

                            //TagGlassData oldGlassData = new TagGlassData();
                            //TagGlassData newGlassData = new TagGlassData();
                            //m_GlassData.GetData(positionId, ref oldGlassData);
                            //newGlassData.Clone(oldGlassData);

                            //newGlassData.Item.SetTagGlassDatabyDisplayedItem(newGlassData, editData);

                            //newGlassData.GlassTypeNum |= (int)GlassTypeFlags.GlassDataUpdated;

                            //if (newGlassData.GlassID != oldGlassData.GlassID)
                            //{
                            //    newGlassData.GlassTypeNum |= (int)GlassTypeFlags.GlassIdUpdated;
                            //}

                            //m_GlassData.Update(positionId, newGlassData);

                            //GlobalVar.GlassDataChangeReport = true;
                            //GlobalVar.GalssDataChangePositionId = positionId;
                        }
                        break;

                    case Command.GlassDataMove:
                        {
                            bool MoveEnable = true;
                            int from = (int)para[0];
                            int to = (int)para[1];


                            if (m_GlassData.IsExist(to))
                            {
                                MessageBox.Show("Move Failed : There is a glass data at target position");
                                return;
                            }

                            _GenericCollection<GantryUnit> GantryUnits = ThreadGantryControl.Units; // minhan
                            _GenericCollection<FishHand> FishHandUnits = ThreadHandControl.Units; // minhan
                            _GenericCollection<CvUnit> cvUnits = ThreadCvControl.Units;

                            if (MoveEnable)
                            {
                                foreach (GantryUnit unit in GantryUnits) // minhan
                                {
                                    if (to == unit.DataMatchingKey(0))
                                    {
                                        if (AppConfig.Instance.Simul.Device) unit.GlassExistSensor.SetState(true);
                                        if (!unit.IsGlassExist(Logic.OR) && !eqpSensors._TR_Unit_Recv_Position_Check_Sensor.IsDetected() && !eqpSensors._TR_Unit_Wait_Position_Check_Sensor.IsDetected())
                                            MoveEnable = false;

                                    }

                                }
                            }

                            if (MoveEnable)
                            {
                                foreach (FishHand unit in FishHandUnits) // minhan
                                {
                                    //fishhand는 home위치에서는 glass data가 존재 할 수 없다. 2009.06.04 kimgun 

                                    //fishhand가 home을 잡을 수 없는 상황에서 glass data move를 할 수 없다. LeeChungWon 090822
                                    //short CurPositionId = (short)unit.Servo.GetCurPointId();

                                    //if (to == unit.DataMatchingKey(0))
                                    //{
                                    //    if ((unit.Name == eqpTransferUnits._LD_Fish_Hand_Name) &&
                                    //    (CurPositionId == eqpPos_LD_Hand_Servo_Unit.Send2_Position ||
                                    //     CurPositionId == eqpPos_LD_Hand_Servo_Unit.Home_Position)) MoveEnable = false;
                                    //    if ((unit.Name == eqpTransferUnits._UL_Fish_Hand_Name) &&
                                    //        (CurPositionId == eqpPos_UL_Hand_Servo_Unit.Recv1_Position ||
                                    //         CurPositionId == eqpPos_UL_Hand_Servo_Unit.Home_Position)) MoveEnable = false;
                                    //    if (this.Simul.Device && MoveEnable) unit.GlassExistSensor.DiSensor.SetState(true);
                                    //    if (!unit.IsGlassExist()) MoveEnable = false;

                                    //}

                                    //fishhand가 home을 잡을 수 없는 상황에서 glass data move를 할 수 없다. LeeChungWon 090822
                                    if (to == unit.DataMatchingKey(0))
                                    {
                                        //if ((unit.Name == eqpTransferUnits._LD_Fish_Hand_Name) &&
                                        //((ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Send2_Position) && !Simul.Motion) ||
                                        // (eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch() && !Simul.Motion)))
                                        //{
                                        //    MoveEnable = false; // 11.01.27 minhan
                                        //}
                                        //if ((unit.Name == eqpTransferUnits._LD_Fish_Hand_Name) &&
                                        //    (((ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Send2_Position) && !Simul.Motion) ||
                                        //    (eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch() && !Simul.Motion)) ||
                                        //    ((ldHandControl.IsPositionConfirmed(eqpPos_LD_Hand_Servo_Unit.Recv2_Position) && !Simul.Motion) &&
                                        //     (trControl.IsPositionConfirmed(eqpPos_TR_Servo_Unit.Send_Position) && !Simul.Motion))))
                                        //{
                                        //    MoveEnable = false; // 12.11.28 wang 뎠TR瞳Send貫零谿珂LD瞳Recv2貫零珂，쐐岺盧땡Glassdata
                                        //}
                                        if ((unit.Name == eqpTransferUnits._UL_Fish_Hand_Name) &&
                                            ((ulHandControl.IsPositionConfirmed(eqpPos_UL_Hand_Servo_Unit.Recv1) && !AppConfig.Instance.Simul.Motion) ||
                                             (eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch() && !AppConfig.Instance.Simul.Motion)))
                                        {
                                            MoveEnable = false; // 11.01.27 minhan
                                        }
                                        if (AppConfig.Instance.Simul.Device && MoveEnable) unit.GlassExistSensor.SetState(true, Logic.AND);
                                        if (!unit.IsGlassExist(Logic.OR)) MoveEnable = false;

                                    }
                                }
                            }

                            if (MoveEnable)
                            {
                                foreach (CvUnit unit in cvUnits)
                                {
                                    if ((to == unit.DataMatchingKey(0)) || (to == unit.DataMatchingKey(1)))
                                    {
                                        if (to == unit.DataMatchingKey(0))
                                        {
                                            if (!unit.GlsInSensor.IsDetected(Logic.OR)) MoveEnable = false;
                                            if (AppConfig.Instance.Simul.Device)
                                            {
                                                if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                                    unit.Id == eqpTransferUnits._FR_CvUnit.Id ||
                                                    unit.Id == eqpTransferUnits._AK_CvUnit.Id)
                                                {
                                                    unit.GlsInSensor.SetState(true, Logic.AND); // 09.09.08 minhan
                                                }
                                                else unit.GlsInSensor.SetState(true); // 09.09.08 minhan
                                            }

                                        }
                                        else
                                        {
                                            if (!unit.GlsOutSensor.IsDetected(Logic.OR)) MoveEnable = false;
                                            if (AppConfig.Instance.Simul.Device)
                                            {
                                                if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                                    unit.Id == eqpTransferUnits._FR_CvUnit.Id)
                                                {
                                                    unit.GlsOutSensor.SetState(true, Logic.AND); // 09.09.08 minhan
                                                }
                                                else unit.GlsOutSensor.SetState(true); // 09.09.08 minhan

                                                if (unit.Id == eqpTransferUnits._UL_CvUnit.Id) eqpTransferUnits._UL_CvUnit.FwDecelSensor.SetState(true); // 11.02.01 minhan
                                            }

                                        }

                                        if (unit.Name == eqpTransferUnits._LD_CvUnit_Name)
                                        {
                                            m_JobCondition.SetTactTime(TactTimeAct.ttCOUNT_END);
                                            m_JobCondition.SetTactTime(TactTimeAct.ttCOUNT_START);
                                        }
                                    }
                                }
                            }

                            if (AppConfig.Instance.Simul.Device)
                            {
                                MoveEnable = true;
                            }

                            if (MoveEnable)
                            {
                                m_GlassData.Move(from, to);

                                //  ThreadHandler.CvControl.InitParameter(); // 09.10.20 minhan 이중으로 중복 됨.

                                if (from == eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0)) // 11.02.01 minhan
                                {
                                    GlobalVar.TrGlsSendReady = false;
                                    GlobalVar.LdIFStatus = "--";
                                    GlobalVar.UlIFStatus = "--";// 11.02.09 minhan
                                    GlobalVar.rcvIfResetReq = true;
                                    GlobalVar.sndIfResetReq = true;
                                    GlobalVar.TrCrackResetReq = true; // 11.06.01 minhan
                                    GlobalVar.LoaderCrackResetReq = true;
                                }

                                if (AppConfig.Instance.Simul.Device)
                                {
                                    foreach (GantryUnit unit in GantryUnits)
                                    {
                                        if (from == unit.DataMatchingKey(0)) unit.GlassExistSensor.SetState(false);
                                    }
                                    foreach (FishHand unit in FishHandUnits)
                                    {
                                        if (from == unit.DataMatchingKey(0)) unit.GlassExistSensor.SetState(false, Logic.AND);
                                    }
                                    foreach (CvUnit unit in cvUnits)
                                    {
                                        if (!this.GlassData.IsExist(unit.DataMatchingKey(0)))
                                        {
                                            if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                                unit.Id == eqpTransferUnits._FR_CvUnit.Id ||
                                                unit.Id == eqpTransferUnits._AK_CvUnit.Id)
                                            {
                                                unit.GlsInSensor.SetState(false, Logic.AND); // 09.09.08 minhan
                                            }
                                            else unit.GlsInSensor.SetState(false); // 09.09.08 minhan
                                        }

                                        if (!this.GlassData.IsExist(unit.DataMatchingKey(1)))
                                        {
                                            if (unit.Id == eqpTransferUnits._RB_CvUnit.Id ||
                                               unit.Id == eqpTransferUnits._FR_CvUnit.Id)
                                            {
                                                unit.GlsOutSensor.SetState(false, Logic.AND); // 09.09.08 minhan
                                            }
                                            else unit.GlsOutSensor.SetState(false); // 09.09.08 minhan

                                            if (unit.FwDecelSensor != null) unit.FwDecelSensor.SetState(false);
                                            if (unit.BwDecelSensor != null) unit.BwDecelSensor.SetState(false);
                                        }
                                    }
                                }
                                ThreadHandler.CvControl.InitParameter();
                                ThreadHandler.TrControl.InitParameter(); // minhan
                                //    ThreadHandler.HandControl.InitParameter(); // minhan
                                //ThreadHandler.LdHandControl.InitParameter(); // 09.05.30 minhan
                                ThreadHandler.UlHandControl.InitParameter(); // 09.05.30 minhan
                                _GenericCollection<GlsSensor> sensors = m_DmsComponents.ComponentContainer.GetCollection<GlsSensor>();
                                string msg = string.Format("Glass data move from {0} to {1}", sensors[from].Name, sensors[to].Name);
                                Log(msg);
                            }
                            else
                            {
                                MessageBox.Show("Move Failed : There is no glass at target position");
                            }

                        }
                        break;
                    case Command.GlassDataRecovery:
                        {
                            int glassNo = (int)para[0];
                            int glassPos = (int)para[1];
                            GlobalVar.RecoveryGlassPos = glassPos;// this.SeqFlag.RecoveryGlassPos = glassPos;
                            GlobalVar.RecoveryGlassNo = glassNo;// this.SeqFlag.RecoveryGlassNo = glassNo;
                            GlobalVar.GlassDataRecoveryReq = true;// this.SeqFlag.GlassDataRecoveryReq = true;
                        }
                        break;
                    case Command.GlassDataRequest:  //2009.06.29 Youngsik 사용하지 않음.
                        {
                            #region 제거
                            //int positionID = (int)para[0];
                            //string glassID = "";
                            //ushort lotNo = 0;
                            //ushort slotNo = 0;
                            //ushort glassCode = 0;

                            //DlgGlassDataRequest dlg = new DlgGlassDataRequest();
                            //string option = "";

                            //dlg.TopMost = true;
                            //dlg.ShowDialog();
                            //option = dlg.Option;

                            //bool glassIDValid = true;
                            //bool lotNoValid = true;
                            //bool slotNoValid = true;
                            //Dictionary<string, string> m_DisplayedItem = (Dictionary<string, string>)para[1];
                            //foreach (KeyValuePair<string, string> item in m_DisplayedItem)
                            //{
                            //    if (string.Equals(item.Key, "Glass ID", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        glassID = (string)item.Value;
                            //        if (string.IsNullOrEmpty(glassID))
                            //        {
                            //            glassIDValid = false;
                            //        }
                            //    }
                            //    else if (string.Equals(item.Key, "Lot No", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        if (UInt16.TryParse((string)item.Value, out lotNo) != true)
                            //        {
                            //            lotNoValid = false;
                            //        }
                            //    }
                            //    else if (string.Equals(item.Key, "Slot No", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        if (UInt16.TryParse((string)item.Value, out slotNo) != true)
                            //        {
                            //            slotNoValid = false;
                            //        }
                            //    }
                            //    else if (string.Equals(item.Key, "Glass Code", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        if (UInt16.TryParse((string)item.Value, out glassCode) != true)
                            //        {//2009.09.11 kimgun
                            //            MessageBox.Show("Item Error");
                            //            CommandProc(Command.GlassDataDeleteConfirm, positionID);
                            //            return;
                            //        }
                            //    }
                            //}

                            //switch (option)
                            //{
                            //    case "C": //by glass code
                            //        {
                            //            if (!lotNoValid || !slotNoValid)
                            //            {
                            //                MessageBox.Show("Item Error");
                            //                CommandProc(Command.GlassDataDeleteConfirm, positionID);
                            //            }
                            //            else
                            //            {
                            //                //GlassNumberCode code = new GlassNumberCode(lotNo, slotNo);
                            //                //glassCode = code.Code;
                            //                //EcsInfo.Instance.GetGlassId = "0"; // 10.12.21 minhan
                            //                //EcsInfo.Instance.GetGlassCode =  glassCode;
                            //                //EcsInfo.Instance.GetReqOption = option;
                            //            }
                            //        }
                            //        break;
                            //    case "I": //by glass id
                            //        {
                            //            if (!glassIDValid)
                            //            {
                            //                MessageBox.Show("Item Error");
                            //                CommandProc(Command.GlassDataDeleteConfirm, positionID);
                            //            }
                            //            else
                            //            {
                            //                //EcsInfo.Instance.GetGlassId = glassID; // 10.12.21 minhan
                            //                //EcsInfo.Instance.GetGlassCode = 0;
                            //                //EcsInfo.Instance.GetReqOption = option;
                            //            }
                            //        }
                            //        break;
                            //    case "A": //by code and id
                            //        {
                            //            if (!lotNoValid || !slotNoValid || !glassIDValid)
                            //            {
                            //                MessageBox.Show("Item Error");
                            //                CommandProc(Command.GlassDataDeleteConfirm, positionID);
                            //            }
                            //            else
                            //            {
                            //                //GlassNumberCode code = new GlassNumberCode(lotNo, slotNo);
                            //                //glassCode = code.Code;
                            //                //EcsInfo.Instance.GetGlassId = glassID; // 10.12.21 minhan
                            //                //EcsInfo.Instance.GetGlassCode = glassCode;
                            //                //EcsInfo.Instance.GetReqOption = option;
                            //            }
                            //        }
                            //        break;
                            //}

                            //GlobalVar.LostGlassRequestPositionId = positionID;
                            //GlobalVar.LostGlassRequest = true;
                            #endregion
                        }
                        break;
                    case Command.GlassDataReport:   //2009.06.29 Youngsik
                        {
                            #region 제거
                            //int positionID = (int)para[0];
                            //Dictionary<string, string> m_DisplayedItem = (Dictionary<string, string>)para[1];

                            //TagGlassData tagGlassData = new TagGlassData();
                            //m_DataProvider.GlassDataProvider.GetData(positionID, ref tagGlassData);

                            //foreach (KeyValuePair<string, string> item in m_DisplayedItem)
                            //{
                            //    if (string.Equals(item.Key, "Glass ID", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        tagGlassData.GlassID = (string)item.Value;
                            //    }
                            //    else if (string.Equals(item.Key, "Glass Code", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        //tagGlassData.GlassCode.Code = Convert.ToUInt16((string)item.Value);//2009.08.19 kimgun2010.06.20 kimgun 삭제
                            //        tagGlassData.GlassCode = Convert.ToUInt16((string)item.Value);//2010.07.20 kimgun 
                            //    }
                            //    else if (string.Equals(item.Key, "Recipe ID", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        tagGlassData.RecipeID = (string)item.Value;
                            //    }
                            //}

                            //GantryUnit gantryUnit = eqpTransferUnits._TR_Gantry_Unit;
                            //tagGlassData.PositionId = gantryUnit.Id;
                            //m_DataProvider.GlassDataProvider.Update(gantryUnit.Id, tagGlassData);
                            ////EcsInfo.Instance.UnloadGlassInfo.Clone(tagGlassData); // 10.12.21 minhan

                            //GlobalVar.ManualUnloadGlsDataSend = true;
                            #endregion
                        }
                        break;
                    case Command.RecipeAddReq:
                        {
                            m_DataProvider.RecipeProvider.ViewerHold(true);
                            //2009.06.18 Youngsik
                            GlobalVar.ChangeRecipe = (TagRecipe)para[0]; // 10.12.25 minhan
                            GlobalVar.RecipeAddReport = true;
                        }
                        break;
                    case Command.RecipeAdd:
                        {   //para[0] - newRecipe :TagRecipe
                            TagRecipe newRecipe = (TagRecipe)para[0];
                            AddRecipe(newRecipe);
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeCopyReq:
                        {
                            m_DataProvider.RecipeProvider.ViewerHold(true);
                            GlobalVar.SourceRecipeId = para[0] as string; // 10.12.25 minhan
                            GlobalVar.ChangedRecipeId = para[1] as string;
                            GlobalVar.RecipeCopyReport = true;   //2009.06.18 Youngsik.
                        }
                        break;
                    case Command.RecipeCopy:
                        {   //para[0] - sourceRecipeId : string
                            //para[1] - targetRecipeId : string
                            string sourceId = para[0] as string;
                            string targetId = para[1] as string;
                            CopyRecipe(sourceId, targetId);
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeRemoveReq:
                        {
                            m_DataProvider.RecipeProvider.ViewerHold(true);
                            string recipeId = para[0] as string;
                            GlobalVar.RecipeDeleteReport = true;
                        }
                        break;
                    case Command.RecipeRemove:
                        {   //para[0] - recipeId : string
                            string recipeId = para[0] as string;
                            RemoveRecipe(recipeId);
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeSaveReq:
                        {
                            m_DataProvider.RecipeProvider.ViewerHold(true);
                            GlobalVar.RecipeIdChangeReport = true; //10.12.25 minhan
                        }
                        break;
                    case Command.RecipeSave:
                        {   //No para

                            SaveRecipe();
                            //SetCurrentRecipe(m_JobCondition.CurrentRecipe.Id);
                            m_JobCondition.SetRecipe2JobCond(m_JobCondition.CurrentRecipe.Id);
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeAddFail:
                        {
                            string recipeId = para[0] as string;
                            RemoveRecipe(recipeId); // 10.12.25 minhan 
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeRemoveFail:
                        {
                            string recipeId = para[0] as string;
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeCopyFail:
                        {
                            string targetId = para[1] as string;
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeSaveFail:
                        {
                            RecipeProvider.Instance.RejectChanges();
                            m_DataProvider.RecipeProvider.ViewerHold(false);
                        }
                        break;
                    case Command.RecipeSelect:
                        {   //para[0] - recipeId : string
                            string recipeId = (string)para[0];
                            m_DataProvider.RecipeProvider.SetCurrentRecipe(recipeId);
                            m_JobCondition.SetRecipe2JobCond(recipeId);
                        }
                        break;
                    case Command.SetupSave:
                        {   //No Para
                            SaveSetupInfos();
                        }
                        break;
                    case Command.CalibrationSave:
                        { //para[0] - calibarionInfo : TagCalibrationInfo
                            TagCalibrationInfo info = para[0] as TagCalibrationInfo;
                            SaveCalibrationInfo(info);
                        }
                        break;
                    case Command.SetIoState:
                        {
                            if (m_GenInfos.AutoMode) break;
                            IoStateEventArgs e = (IoStateEventArgs)para[0];
                            switch (e.Type)
                            {
                                case IoType.DO:
                                    {
                                        int id = e.Id;
                                        bool curValue = m_IoController.ReadDoAsync(id);
                                        m_IoController.WriteDoAsync(id, !curValue);
                                    }
                                    break;
                            }
                        }
                        break;
                    case Command.DualGlsSensorManual:
                        {
                            string deviceName = (string)para[0];
                            DualGlsSensorType type = (DualGlsSensorType)para[1];
                            bool value = (bool)para[2];
                            _GenericCollection<GlsSensor> units = m_DmsComponents.ComponentContainer.GetCollection<GlsSensor>();
                            DualGlsSensor sensor = units[deviceName] as DualGlsSensor;
                            if (type == DualGlsSensorType.Op)
                            {
                                if (value == false)
                                {
                                    if (sensor.UseOop == false)
                                    {
                                        MessageBox.Show("Oop sensor is NOUSE status.");
                                    }
                                    else
                                    {
                                        sensor.UseOp = value;
                                    }
                                }
                                else
                                {
                                    sensor.UseOp = value;
                                }
                            }
                            else if (type == DualGlsSensorType.Oop)
                            {
                                if (value == false)
                                {
                                    if (sensor.UseOp == false)
                                    {
                                        MessageBox.Show("Op sensor is NOUSE status.");
                                    }
                                    else
                                    {
                                        sensor.UseOop = value;
                                    }
                                }
                                else
                                {
                                    sensor.UseOop = value;
                                }
                            }
                        }
                        break;
                    case Command.ServoManual:
                        {
                            ServoManualAct act = (ServoManualAct)(para[0]);
                            ServoMotor servoMotor = null;
                            ServoUnit servoUnit = new ServoUnit();

                            if (act == ServoManualAct.JogPlusStart ||
                                act == ServoManualAct.JogPlusStop ||
                                act == ServoManualAct.JogMinusStart ||
                                act == ServoManualAct.JogMinusStop)
                            {
                                servoMotor = (ServoMotor)(para[1]);
                            }
                            else if (act != ServoManualAct.ResestManualAct &&
                                    act != ServoManualAct.EstopAll)
                            {
                                servoUnit = (ServoUnit)(para[1]);
                            }

                            #region Servo Act
                            switch (act)
                            {
                                case ServoManualAct.PosSend:
                                    {
                                        int point = (int)(para[2]);
                                        RbtPos curPos = new RbtPos(servoUnit.Axis.Count);
                                        servoUnit.GetCurPosition(ref curPos);

                                        servoUnit.SetTeachPointPos((short)point, curPos);
                                    }
                                    break;
                                case ServoManualAct.PosSave:
                                    {
                                        servoUnit.SaveTeachPosToFile();
                                    }
                                    break;
                                case ServoManualAct.PosRead:
                                    {
                                        servoUnit.ReadTeachPosFromFile();
                                    }
                                    break;
                                case ServoManualAct.Move:
                                    {
                                        int point = (int)(para[2]);
                                        servoUnit.SelectedPointId = (short)point;
                                        //servoUnit.ManualActionCmd = (int)RbtAction.Move;

                                        if (!ServoManualInterlock(servoUnit, (short)point))
                                        {
                                            servoUnit.ManualActionCmd = (int)RbtAction.Move;

                                            #region simul 전용 setup 완료후 삭제해도 무방
                                            if (AppConfig.Instance.Simul.Motion)
                                            {
                                                if (servoUnit.Id == eqpServoUnits._TR_Servo_Unit.Id)
                                                {
                                                    trControl.SetPositionConfirmed(servoUnit.SelectedPointId);
                                                }
                                                //else if (servoUnit.Id == eqpServoUnits._LD_Hand_Servo_Unit.Id)
                                                //{
                                                //    ldHandControl.SetPositionConfirmed(servoUnit.SelectedPointId);
                                                //}
                                                else if (servoUnit.Id == eqpServoUnits._UL_Hand_Servo_Unit.Id)
                                                {
                                                    ulHandControl.SetPositionConfirmed(servoUnit.SelectedPointId);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    break;
                                case ServoManualAct.RepeatStart:
                                    {
                                        int point = (int)(para[2]);
                                        servoUnit.SelectedPointId = (short)point;
                                        //servoUnit.ManualActionCmd = (int)RbtAction.MoveRepeat;
                                        if (!ServoManualInterlock(servoUnit, (short)point))
                                            servoUnit.ManualActionCmd = (int)RbtAction.MoveRepeat;
                                    }
                                    break;
                                case ServoManualAct.RepeatStop:
                                    {
                                        servoUnit.ManualActionCmd = (int)RbtAction.None;
                                    }
                                    break;
                                case ServoManualAct.ServoOn:
                                    {
                                        servoUnit.ManualActionCmd = (int)RbtAction.ServoOn;
                                    }
                                    break;
                                case ServoManualAct.Home:
                                    {
                                        GlobalVar.ServoHomeChecked = true;
                                        if (!ServoManualInterlock(servoUnit, 0))
                                            servoUnit.ManualActionCmd = (int)RbtAction.Home;
                                    }
                                    break;
                                case ServoManualAct.Estop:
                                    {
                                        servoUnit.ManualActionCmd = (int)RbtAction.Estop;
                                    }
                                    break;
                                case ServoManualAct.EstopAll:
                                    {
                                        ServoUnits units = m_DmsComponents.ComponentContainer.GetCollection<ServoUnit>() as ServoUnits;
                                        foreach (ServoUnit unit in units)
                                        {
                                            unit.ManualActionCmd = (int)RbtAction.Estop;
                                        }
                                    }
                                    break;
                                case ServoManualAct.JogPlusStart:
                                    {
                                        double vel = Convert.ToDouble((string)(para[2]));
                                        double pulse = servoMotor.Len2Pulse(vel);
                                        bool interlockCondition = true;
                                        int ServoError = servoMotor.GetControllerError(); // 11.02.01 minhan

                                        if ((servoMotor.Id == eqpServoMotors._RB_Up_Servo_Motor1.Id) ||
                                           (servoMotor.Id == eqpServoMotors._RB_Lo_Servo_Motor1.Id) ||
                                           (servoMotor.Id == eqpServoMotors._RB_Up_Servo_Motor2.Id) ||
                                           (servoMotor.Id == eqpServoMotors._RB_Lo_Servo_Motor2.Id))
                                        {
                                            HeavyInterlock heavy = this.JobCond.HeavyInterlock;
                                            interlockCondition = ((heavy & HeavyInterlock.Emo) > 0 ? false : true);
                                            interlockCondition = ((heavy & HeavyInterlock.Cover) > 0 ? false : true);
                                        }
                                        else
                                        {
                                            HeavyInterlock heavy = this.JobCond.HeavyInterlock;
                                            interlockCondition = ((heavy & HeavyInterlock.Emo) > 0 ? false : true);
                                            interlockCondition = ((heavy & HeavyInterlock.Door) > 0 ? false : true);
                                        }

                                        if (!interlockCondition)
                                        {
                                            MessageBox.Show("Please Emo or Door or Cover Interlock Checking.", "WSSD", MessageBoxButtons.OK);
                                            return;
                                        }

                                        if (ServoError != 0)
                                        {
                                            MessageBox.Show("Servo Status Error! Please Servo Checking.", "WSSD", MessageBoxButtons.OK);
                                            return;
                                        }

                                        servoMotor.StartVelMove(pulse);
                                    }
                                    break;
                                case ServoManualAct.JogPlusStop:
                                    {
                                        servoMotor.StopVelMove();
                                    }
                                    break;
                                case ServoManualAct.JogMinusStart:
                                    {
                                        double vel = Convert.ToDouble((string)(para[2]));
                                        double pulse = servoMotor.Len2Pulse(vel * -1);
                                        bool interlockCondition = true;
                                        int ServoError = servoMotor.GetControllerError(); // 11.02.01 minhan

                                        if ((servoMotor.Id == eqpServoMotors._RB_Up_Servo_Motor1.Id) ||
                                           (servoMotor.Id == eqpServoMotors._RB_Lo_Servo_Motor1.Id) ||
                                           (servoMotor.Id == eqpServoMotors._RB_Up_Servo_Motor2.Id) ||
                                           (servoMotor.Id == eqpServoMotors._RB_Lo_Servo_Motor2.Id))
                                        {
                                            HeavyInterlock heavy = this.JobCond.HeavyInterlock;
                                            interlockCondition = ((heavy & HeavyInterlock.Emo) > 0 ? false : true);
                                            interlockCondition = ((heavy & HeavyInterlock.Cover) > 0 ? false : true);
                                        }
                                        else
                                        {
                                            HeavyInterlock heavy = this.JobCond.HeavyInterlock;
                                            interlockCondition = ((heavy & HeavyInterlock.Emo) > 0 ? false : true);
                                            interlockCondition = ((heavy & HeavyInterlock.Door) > 0 ? false : true);
                                        }

                                        if (!interlockCondition)
                                        {
                                            MessageBox.Show("Please Emo or Door or Cover Interlock Checking.", "WSSD", MessageBoxButtons.OK);
                                            return;
                                        }

                                        if (ServoError != 0)
                                        {
                                            MessageBox.Show("Servo Status Error! Please Servo Checking.", "WSSD", MessageBoxButtons.OK);
                                            return;
                                        }

                                        servoMotor.StartVelMove(pulse);
                                    }
                                    break;
                                case ServoManualAct.JogMinusStop:
                                    {
                                        servoMotor.StopVelMove();
                                    }
                                    break;
                                case ServoManualAct.ChangeVelRatio:
                                    {
                                        double velRatio = Convert.ToDouble((string)(para[2])) / 100;
                                        servoUnit.SetVelRatio(velRatio);
                                    }
                                    break;
                                case ServoManualAct.ChangeRepeatWaitTime:
                                    {
                                        int waitTime = Convert.ToInt32(para[2]);
                                        servoUnit.RepeatWaitTime = waitTime;
                                    }
                                    break;
                                case ServoManualAct.ResestManualAct:
                                    {
                                        ServoUnits units = m_DmsComponents.ComponentContainer.GetCollection<ServoUnit>() as ServoUnits;
                                        foreach (ServoUnit unit in units)
                                        {
                                            unit.SetVelRatio(1.0);
                                            unit.ManualActionCmd = (int)RbtAction.None;
                                            unit.InitSequenceParameter();
                                        }
                                    }
                                    break;
                            }
                            #endregion
                        }
                        break;

                    //case Command.HeaterManual:
                    //    {
                    //        HeaterAct act = (HeaterAct)para[0];

                    //        switch (act)
                    //        {
                    //            case HeaterAct.On:
                    //                {
                    //                    eqpHeaterUnits._Dev_Heater_Unit.ManualAct = HeaterAct.On;
                    //                }
                    //                break;

                    //            case HeaterAct.Off:
                    //                {
                    //                    eqpHeaterUnits._Dev_Heater_Unit.ManualAct = HeaterAct.Off;
                    //                }
                    //                break;

                    //            case HeaterAct.Set:
                    //                {
                    //                    int address = (int)para[1];
                    //                    int temp = Convert.ToInt32(para[2]);

                    //                    eqpHeaterUnits._Dev_Heater_Unit.ManualTemp = temp * 10;
                    //                }
                    //                break;
                    //        }
                    //    }
                    //    break;
                    #region Euv Act
                    case Command.UshioEuvManual:
                        {
                            UshioEuvUnit euv = (UshioEuvUnit)para[0];
                            UshioEuvAct act = (UshioEuvAct)(para[1]);
                            bool bOn = (bool)(para[2]);
                            switch (act)
                            {
                                case UshioEuvAct.UDLC:
                                    {
                                        euv.LampLocalLock(bOn);
                                        //euv.DoUDLC.SetState(bOn);
                                    }
                                    break;
                                case UshioEuvAct.UEMG:
                                    {
                                        euv.LampEmergency(bOn);
                                        //euv.DoUEMG.SetState(bOn);
                                    }
                                    break;
                                case UshioEuvAct.UERT:
                                    {
                                        euv.EmoReset(bOn);
                                    }
                                    break;
                                case UshioEuvAct.UICR:
                                    {
                                        euv.SetInitialCount(bOn);
                                        //euv.DoUICR.SetState(bOn);
                                    }
                                    break;
                                case UshioEuvAct.ULST:
                                    {
                                        euv.LampUseTimeReset(bOn);
                                    }
                                    break;
                                case UshioEuvAct.UTRE:
                                    {
                                        euv.LampRemote(bOn);
                                        //euv.DoUTRE.SetState(bOn);
                                    }
                                    break;
                                case UshioEuvAct.ULON:
                                    {
                                        euv.LampControl(bOn);
                                        //euv.DoULON.SetState(bOn);
                                    }
                                    break;
                                case UshioEuvAct.EuvUpdnUp:
                                    {
                                        if (bOn)
                                            eqpActuatorUnits._EUV_Unit_Updn.SetPositiveAct();
                                        else
                                            eqpActuatorUnits._EUV_Unit_Updn.SetStopAct();
                                    }
                                    break;
                                case UshioEuvAct.EuvUpdnDn:
                                    {
                                        if (bOn)
                                            eqpActuatorUnits._EUV_Unit_Updn.SetNegativeAct();
                                        else
                                            eqpActuatorUnits._EUV_Unit_Updn.SetStopAct();
                                    }
                                    break;
                                case UshioEuvAct.N2InValve:
                                    {
                                        if (bOn)
                                            euv.N2Valve.SetAutoValveAct(AutoValveAct.Open);
                                        else
                                            euv.N2Valve.SetAutoValveAct(AutoValveAct.Close);
                                    }
                                    break;
                                case UshioEuvAct.CdaInValve:
                                    {
                                        if (bOn)
                                            euv.CdaValve.SetAutoValveAct(AutoValveAct.Open);
                                        else
                                            euv.CdaValve.SetAutoValveAct(AutoValveAct.Close);
                                    }
                                    break;
                                case UshioEuvAct.PcwInValve:
                                    {
                                        if (bOn)
                                            euv.PCWInValve.SetAutoValveAct(AutoValveAct.Open);
                                        else
                                            euv.PCWInValve.SetAutoValveAct(AutoValveAct.Close);
                                    }
                                    break;
                                case UshioEuvAct.PcwOutValve:
                                    {
                                        if (bOn)
                                            euv.PCWOutValve.SetAutoValveAct(AutoValveAct.Open);
                                        else
                                            euv.PCWOutValve.SetAutoValveAct(AutoValveAct.Close);
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion
                    //case Command.ServoMP2300Manaul:
                    //    {
                    //        ServoManualAct act = (ServoManualAct)(para[0]);
                    //        ServoMotorMp2300 servoMotor = new ServoMotorMp2300();
                    //        ServoUnitMp2300 servoUnit = new ServoUnitMp2300();

                    //        if (act == ServoManualAct.JogPlusStart ||
                    //            act == ServoManualAct.JogPlusStop ||
                    //            act == ServoManualAct.JogMinusStart ||
                    //            act == ServoManualAct.JogMinusStop)
                    //        {
                    //            servoMotor = (ServoMotorMp2300)(para[1]);
                    //        }
                    //        else if (act != ServoManualAct.ResestManualAct &&
                    //                act != ServoManualAct.EstopAll)
                    //        {
                    //            servoUnit = (ServoUnitMp2300)(para[1]);
                    //        }

                    //        #region Servo Mp2300 Act
                    //        switch (act)
                    //        {
                    //            case ServoManualAct.PosSend:
                    //                {
                    //                    int point = (int)(para[2]);
                    //                    RbtPos curPos = new RbtPos(servoUnit.Axis.Count);
                    //                    servoUnit.GetCurPosition(ref curPos);

                    //                    servoUnit.SetTeachPointPos((short)point, curPos);
                    //                }
                    //                break;
                    //            case ServoManualAct.PosSave:
                    //                {
                    //                    servoUnit.SaveTeachPosToFile();
                    //                }
                    //                break;
                    //            case ServoManualAct.PosRead:
                    //                {
                    //                    servoUnit.ReadTeachPosFromFile();
                    //                }
                    //                break;
                    //            case ServoManualAct.Move:
                    //                {
                    //                    int point = (int)(para[2]);
                    //                    servoUnit.SelectedPointId = (short)point;
                    //                    if (!ServoManualInterlock(servoUnit, (short)point))
                    //                    {
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.Move;

                    //                    #region simul 전용 setup 완료후 삭제해도 무방
                    //                    if (Simul.Motion)
                    //                    {
                    //                        if (servoUnit.Id == eqpServoUnits._TR_Servo_Unit.Id)
                    //                        {
                    //                            trControl.SetPositionConfirmed(servoUnit.SelectedPointId);
                    //                        }
                    //                        else if (servoUnit.Id == eqpServoUnits._LD_Hand_Servo_Unit.Id)
                    //                        {
                    //                            ldHandControl.SetPositionConfirmed(servoUnit.SelectedPointId);
                    //                        }
                    //                        else if (servoUnit.Id == eqpServoUnits._UL_Hand_Servo_Unit.Id)
                    //                        {
                    //                            ulHandControl.SetPositionConfirmed(servoUnit.SelectedPointId);
                    //                        }
                    //                      }
                    //                    #endregion
                    //                    }
                    //                }
                    //                break;
                    //            case ServoManualAct.RepeatStart:
                    //                {
                    //                    int point = (int)(para[2]);
                    //                    servoUnit.SelectedPointId = (short)point;
                    //                    if (!ServoManualInterlock(servoUnit, (short)point))
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.MoveRepeat;
                    //                }
                    //                break;
                    //            case ServoManualAct.RepeatStop:
                    //                {
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.None;
                    //                }
                    //                break;
                    //            case ServoManualAct.ServoOn:
                    //                {
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.ServoOn;
                    //                }
                    //                break;
                    //            case ServoManualAct.Home:
                    //                {
                    //                    if (!ServoManualInterlock(servoUnit, 0))
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.Home;
                    //                }
                    //                break;
                    //            case ServoManualAct.Estop:
                    //                {
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.Estop;
                    //                }
                    //                break;
                    //            case ServoManualAct.EstopAll:
                    //                {
                    //                    ServoUnitMp2300s units = ComponentContainer.GetCollection<ServoUnitMp2300>() as ServoUnitMp2300s;
                    //                    foreach (ServoUnitMp2300 unit in units)
                    //                    {
                    //                        unit.ManualActionCmd = (int)RbtAction.Estop;
                    //                    }
                    //                }
                    //                break;
                    //            case ServoManualAct.JogPlusStart:
                    //                {
                    //                    double vel = Convert.ToDouble((string)(para[2]));
                    //                    int speed = servoMotor.Len2Pulse<int>(vel);

                    //                    //servoMotor.StartVelMove(pulse);
                    //                    servoMotor.SetJogSpeed(speed);
                    //                    servoMotor.SetJogPlus(true);

                    //                }
                    //                break;
                    //            case ServoManualAct.JogPlusStop:
                    //                {
                    //                    //servoMotor.StopVelMove();
                    //                    servoMotor.SetJogPlus(false);
                    //                }
                    //                break;
                    //            case ServoManualAct.JogMinusStart:
                    //                {
                    //                    double vel = Convert.ToDouble((string)(para[2]));
                    //                    int speed = servoMotor.Len2Pulse<int>(vel);
                    //                    //double pulse = servoMotor.Len2Pulse(vel * -1);
                    //                    //if (!ServoManualInterlock(servoUnit, 0))
                    //                    {
                    //                    //servoMotor.StartVelMove(pulse);
                    //                    servoMotor.SetJogSpeed(speed);
                    //                    servoMotor.SetJogMinus(true);
                    //                    }
                    //                }
                    //                break;
                    //            case ServoManualAct.JogMinusStop:
                    //                {
                    //                    //servoMotor.StopVelMove();
                    //                    servoMotor.SetJogMinus(false);
                    //                }
                    //                break;
                    //            case ServoManualAct.ChangeVelRatio:
                    //                {
                    //                    double velRatio = Convert.ToDouble((string)(para[2])) / 100;
                    //                    servoUnit.SetVelRatio(velRatio);
                    //                }
                    //                break;
                    //            case ServoManualAct.ChangeRepeatWaitTime:
                    //                {
                    //                    int waitTime = Convert.ToInt32(para[2]);
                    //                    servoUnit.RepeatWaitTime = waitTime;
                    //                }
                    //                break;
                    //            case ServoManualAct.ResestManualAct:
                    //                {
                    //                    ServoUnitMp2300s units = ComponentContainer.GetCollection<ServoUnitMp2300>() as ServoUnitMp2300s;
                    //                    foreach (ServoUnitMp2300 unit in units)
                    //                    {
                    //                        unit.SetVelRatio(1.0);
                    //                        unit.ManualActionCmd = (int)RbtAction.None;
                    //                        unit.InitSequenceParameter();
                    //                    }
                    //                }
                    //                break;
                    //            case ServoManualAct.FindHome:
                    //                {
                    //                    servoUnit.ManualActionCmd = (int)RbtAction.FindHome;
                    //                }
                    //                break;
                    //            case ServoManualAct.SaveFindHome:
                    //                {
                    //                    for (int i = 0; i < servoUnit.AxisCount; i++)
                    //                    {
                    //                        servoUnit.Axis[i].AxisHomeAngle = servoUnit.Theta;
                    //                    }
                    //                }
                    //                break;
                    //        }
                    //        #endregion

                    //    }
                    //    break;
                    /* case Command.ApPlasmaManual:
                         {
                             //PSMAp ap = (PSMAp)para[0];
                             _Plasma ap = (_Plasma)para[0];
                             ApPlasmaAct act = (ApPlasmaAct)(para[1]);
                             switch (act)
                             {
                                 case ApPlasmaAct.SetN2Flow:
                                     {
                                         double value = double.Parse((string)para[2]);
                                         //ap.SetMfcFlow(ap.MfcN2.Name, value);
                                         if (value < 700 || value > 1500) // 10.12.21 minhan
                                         {
                                             MessageBox.Show("Wrong Plasma N2 Setting value .", "WSSD", MessageBoxButtons.OK);
                                             return;    
                                         }
                                         else if (ap.IsPowerOn()) // 09.10.08 minhan
                                         {
                                             MessageBox.Show("Please AP power Off. Now Plasma On status.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         ap.SetN2Flow(value);
                                     }
                                     break;
                                 case ApPlasmaAct.SetCDAFlow:
                                     {
                                         double value = double.Parse((string)para[2]);
                                         //ap.SetMfcFlow(ap.MfcCDA.Name, value);
                                         if (value < 0 || value > 20) // 10.12.21 minhan
                                         {
                                             MessageBox.Show("Wrong Plasma CDA Setting value.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         else if (ap.IsPowerOn()) // 09.10.08 minhan
                                         {
                                             MessageBox.Show("Please AP power Off. Now Plasma On status.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         ap.SetCDAFlow(value);
                                     }
                                     break;
                                 case ApPlasmaAct.SetVoltage:
                                     {
                                         double value = double.Parse((string)para[2]);
                                         if (value < 7 || value > 15) // 10.12.21 minhan
                                         {
                                             MessageBox.Show("Wrong Plasma Voltage Setting value.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         else if (ap.IsPowerOn()) // 09.10.08 minhan
                                         {
                                             MessageBox.Show("Please AP power Off. Now Plasma On status.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         ap.SetVoltage(value);
                                     }
                                     break;
                                 case ApPlasmaAct.PowerOnOff:
                                     {
                                         if (ap.IsPowerOn())
                                         {
                                             ap.PowerOff();
                                         }
                                         else
                                         {
                                             if (ap.IsInterlockCondition())
                                             {
                                                 MessageBox.Show("AP Unit is interlock condition!!");
                                             }
                                             else
                                             {
                                                 ap.PowerOn();
                                             }
                                         }
                                     }
                                     break;
                                 case ApPlasmaAct.ResetN2Flow:
                                     {
                                         //ap.SetMfcFlow(ap.MfcN2.Name, 0);
                                         if (ap.IsPowerOn()) // 09.09.28 minhan
                                         {
                                             MessageBox.Show("Please AP power Off. Now Plasma On status.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         ap.SetN2Flow(0.0);
                                     }
                                     break;
                                 case ApPlasmaAct.ResetCDAFlow:
                                     {
                                         //ap.SetMfcFlow(ap.MfcCDA.Name, 0);
                                         if (ap.IsPowerOn()) // 09.09.28 minhan
                                         {
                                             MessageBox.Show("Please AP power Off. Now Plasma On status.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         ap.SetCDAFlow(0.0);
                                     }
                                     break;
                                 case ApPlasmaAct.ResetVoltage:
                                     {
                                         if (ap.IsPowerOn()) // 09.09.28 minhan
                                         {
                                             MessageBox.Show("Please AP power Off. Now Plasma On status.", "WSSD", MessageBoxButtons.OK);
                                             return;
                                         }
                                         ap.SetVoltage(0);
                                     }
                                     break;
                                 case ApPlasmaAct.HouseUp: // 11.02.01 minhan
                                     {
                                         bool pos = bool.Parse((string)para[2]);
                                         bool DoorCheck = eqpDoorSensors._AP_Unit_Front__Mid_Door1_Sensor.IsAlarm;
                                         DoorCheck |= eqpDoorSensors._AP_Unit_Front__Mid_Door2_Sensor.IsAlarm;
                                         DoorCheck |= eqpDoorSensors._AP_Unit_Rear_Mid_Door1_Sensor.IsAlarm;
                                         DoorCheck |= eqpDoorSensors._AP_Unit_Rear_Mid_Door2_Sensor.IsAlarm;

                                         bool CvStop = eqpTransferUnits._EUV_CvUnit.MotorControl.IsStop(Logic.AND);

                                         if (pos == true)
                                         {
                                             if (!CvStop)
                                             {
                                                 MessageBox.Show("Please AP Cv Stop!", "WSSD", MessageBoxButtons.OK);
                                                 return;
                                             }

                                             if (ap.IsPowerOn()) // 10.02.01 minhan
                                             {
                                                 MessageBox.Show("Now is Plasma ON Please Plasma Off", "WSSD", MessageBoxButtons.OK);
                                                 return;
                                             }
                                             else if (DoorCheck)
                                             {
                                                 MessageBox.Show("Please AP MID DOOR Alarm Check ", "WSSD", MessageBoxButtons.OK);
                                                 return;
                                             }
                                         }

                                         if (pos == true)
                                         {
                                             ap.ActuatorUnit.SetPositiveAct();
                                         }
                                         else
                                         {
                                             ap.ActuatorUnit.SetStopAct();
                                         }
                                     }
                                     break;
                                 case ApPlasmaAct.HouseDown: // 11.02.01 minhan
                                     {
                                         bool neg = bool.Parse((string)para[2]);
                                         bool DoorCheck = eqpDoorSensors._AP_Unit_Front__Mid_Door1_Sensor.IsAlarm;
                                         DoorCheck |= eqpDoorSensors._AP_Unit_Front__Mid_Door2_Sensor.IsAlarm;
                                         DoorCheck |= eqpDoorSensors._AP_Unit_Rear_Mid_Door1_Sensor.IsAlarm;
                                         DoorCheck |= eqpDoorSensors._AP_Unit_Rear_Mid_Door2_Sensor.IsAlarm;

                                         bool CvStop = eqpTransferUnits._EUV_CvUnit.MotorControl.IsStop(Logic.AND);

                                         if (neg == true)
                                         {
                                             if (!CvStop)
                                             {
                                                 MessageBox.Show("Please AP Cv Stop!", "WSSD", MessageBoxButtons.OK);
                                                 return;
                                             }

                                             if (ap.IsPowerOn()) // 10.02.01 minhan
                                             {
                                                 MessageBox.Show("Now is Plasma ON Please Plasma Off", "WSSD", MessageBoxButtons.OK);
                                                 return;
                                             }
                                             else if (DoorCheck)
                                             {
                                                 MessageBox.Show("Please AP MID DOOR Alarm Check ", "WSSD", MessageBoxButtons.OK);
                                                 return;
                                             }
                                         }

                                         if (neg == true)
                                         {
                                             ap.ActuatorUnit.SetNegativeAct();
                                         }
                                         else
                                         {
                                             ap.ActuatorUnit.SetStopAct();
                                         }
                                     }
                                     break;
                             }
                         }
                         break;*/
                    case Command.D4SLManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            D4SLAct act = (D4SLAct)para[1];
                            int index;
                            _GenericCollection<D4SL> units = m_DmsComponents.ComponentContainer.GetCollection<D4SL>();
                            D4SL unit = units[deviceName];

                            #region Interlock
                            #endregion

                            switch (act)
                            {
                                case D4SLAct.LockAll:
                                    unit.SetLockAll(true);
                                    break;
                                case D4SLAct.UnlockAll:
                                    unit.SetLockAll(false);
                                    break;
                                case D4SLAct.LockOne:
                                    index = (int)para[2];
                                    unit.SetLock(index, true);
                                    break;
                                case D4SLAct.UnlockOne:
                                    index = (int)para[2];
                                    unit.SetLock(index, false);
                                    break;
                            }
                        }
                        break;
                    case Command.DoorLockManual:
                        {
                            if (m_GenInfos.AutoMode) return;

                            string deviceName = (string)para[0];
                            DoorLockAct act = (DoorLockAct)para[1];
                            _GenericCollection<DoorLockSensor> units = m_DmsComponents.ComponentContainer.GetCollection<DoorLockSensor>();
                            DoorLockSensor unit = units[deviceName];

                            #region Interlock
                            bool opened = unit.IsDetected();
                            if (opened)
                            {
                                MessageBox.Show("Door is Open.", "WSSD", MessageBoxButtons.OK);
                                return;
                            }
                            #endregion

                            switch (act)
                            {
                                case DoorLockAct.Lock:
                                    unit.SetLock(true);
                                    break;
                                case DoorLockAct.Unlock:
                                    unit.SetLock(false);
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region CommandProcHelper
        public void SetCvMotorAct(string deviceName, CvMotorAct act, int speed)
        {
            if (m_GenInfos.AutoMode) return;

            _GenericCollection<CvUnit> cvUnits = ThreadCvControl.Units;

            int cvUnitCnt = cvUnits.Count;

            //bool checkCond = false;
            //foreach(CvUnit unit in cvUnits)
            //{
            //    checkCond |= ((unit.Cylinder != null) );
            //}

            for (int unitId = 0; unitId < cvUnitCnt; unitId++)
            {
                CvUnit cvUnit = cvUnits[unitId];
                int motorCnt = cvUnit.Motors.Count;

                for (int motorId = 0; motorId < motorCnt; motorId++)
                {
                    if (deviceName == cvUnit.Motors[motorId].Name)
                    {
                        if (cvUnit.ManualAct[motorId] != act) cvUnit.ManualAct[motorId] = act;
                        //if (act == CvMotorAct.Stop) return;
                        if (cvUnit.ManualSpeed[motorId] != speed) cvUnit.ManualSpeed[motorId] = speed;
                        //return;
                    }
                }
            }
        }

        //public bool CvManualActEnable(bool condition, CvMotorAct act)
        //{
        //    if (!condition) return false;
        //    else
        //    {
        //        if (act == CvMotorAct.Bw)
        //        { }
        //    }
        //}

        public void SetPumpAct(string deviceName, PumpAct act) // 11.02.01 minhan
        {
            if (m_GenInfos.AutoMode) return;

            _GenericCollection<PumpUnit> pumpUnits = ThreadPumpControl.Units;
            //_GenericCollection<CirPumpUnit> cirPumpUnits = ThreadCirPumpControl.Units;
            //_GenericCollection<DevPumpUnit> devPumpUnits = ThreadDevPumpControl.Units;

            int pumpUnitCnt = pumpUnits.Count;
            //int cirPumpUnitCnt = cirPumpUnits.Count;
            //int devPumpUnitCnt = devPumpUnits.Count;

            bool checkMj = eqpAutoValves._RB_MJ_CDA_In_Valve.IsOpen(); // 11.03.25 minhan
            checkMj &= !eqpGauges._RB_Unit_MJ_CDA_Pressure_Gauge.IsAlarm;

            for (int unitId = 0; unitId < pumpUnitCnt; unitId++)
            {
                if (deviceName == pumpUnits[unitId].Pump.Name)
                {
                    bool CheckCover = eqpCoverSensors._RB_Unit_Cover_Sensor_OP1.IsAlarm;
                    CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_OP2.IsAlarm;
                    CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_OP3.IsAlarm;
                    CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_MT1.IsAlarm;
                    CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_MT2.IsAlarm;
                    CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_MT3.IsAlarm;

                    if ((pumpUnits[unitId].Tank.LevelFault) && (act == PumpAct.Run))
                    {
                        MessageBox.Show("Please Tank Level Sensor Check", "WSSD", MessageBoxButtons.OK);
                        return;
                    }
                    else if ((!pumpUnits[unitId].Tank.IsRunEnableLevel()) && (act == PumpAct.Run))
                    {
                        MessageBox.Show("Please Tank Status Check", "WSSD", MessageBoxButtons.OK);
                        return;
                    }
                    else if ((CheckCover) && (act == PumpAct.Run))
                    {
                        MessageBox.Show("Now is RB Cover Alarm ", "WSSD", MessageBoxButtons.OK);
                        return;
                    }
                    else if (!checkMj && (act == PumpAct.Run)) // 11.03.25 minhan
                    {
                        MessageBox.Show("Please MJ CDA Unit Check ", "WSSD", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        pumpUnits[unitId].ManualAct = act;
                    }
                }
            }

            //for (int unitId = 0; unitId < cirPumpUnitCnt; unitId++)
            //{
            //    if (deviceName == cirPumpUnits[unitId].Pump.Name)
            //        cirPumpUnits[unitId].ManualAct = act;
            //}

            //for (int unitId = 0; unitId < devPumpUnitCnt; unitId++)
            //{
            //    if (deviceName == devPumpUnits[unitId].Pump.Name)
            //        devPumpUnits[unitId].ManualAct = act;
            //}
        }

        public void SetRbMotorAct(string deviceName, RbMotorAct act, int speed) // 11.02.01 minhan
        {
            if (m_GenInfos.AutoMode) return;

            _GenericCollection<RbUnit> rbUnits = ThreadRbControl.Units;

            int rbUnitCnt = rbUnits.Count;

            bool checkGlass = eqpTransferUnits._RB_CvUnit.GlsInSensor.IsDetected(Logic.OR);
            checkGlass |= eqpTransferUnits._RB_CvUnit.GlsOutSensor.IsDetected(Logic.OR);

            bool CheckCover = eqpCoverSensors._RB_Unit_Cover_Sensor_OP1.IsAlarm;
            CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_OP2.IsAlarm;
            CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_OP3.IsAlarm;
            CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_MT1.IsAlarm;
            CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_MT2.IsAlarm;
            CheckCover |= eqpCoverSensors._RB_Unit_Cover_Sensor_MT3.IsAlarm;

            for (int unitId = 0; unitId < rbUnitCnt; unitId++)
            {
                if (deviceName == rbUnits[unitId].Motor.Name)
                {
                    if (rbUnits[unitId].ManualSpeed != speed)
                    {
                        if ((act == RbMotorAct.Cw) || (act == RbMotorAct.Ccw))
                        {
                            if (checkGlass)
                            {
                                if (DialogResult.No == MessageBox.Show("Now is RB Glass Exist. Do you want Running?", "WSSD", MessageBoxButtons.YesNo))
                                {
                                    rbUnits[unitId].ManualSpeed = 0;
                                    return;
                                }
                            }

                            if (CheckCover)
                            {
                                if (DialogResult.No == MessageBox.Show("Now is RB Cover Open. Do you want Running?", "WSSD", MessageBoxButtons.YesNo))
                                {
                                    rbUnits[unitId].ManualSpeed = 0;
                                    return;
                                }
                                else
                                {
                                    rbUnits[unitId].ManualSpeed = (double)speed;
                                }
                            }
                            else rbUnits[unitId].ManualSpeed = (double)speed;
                        }
                        else
                        {
                            rbUnits[unitId].ManualSpeed = 0;
                        }
                    }
                }
            }
        }

        public void AddRecipe(TagRecipe recipe)
        {
            m_DataProvider.RecipeProvider.AddRecipe(recipe);
        }
        public void CopyRecipe(string sourceId, string targetId)
        {
            if (sourceId == null || targetId == null) return;
            m_DataProvider.RecipeProvider.CopyRecipe(sourceId, targetId);
        }
        public void RemoveRecipe(string recipeId)
        {
            if (recipeId == null) return;
            m_DataProvider.RecipeProvider.RemoveRecipe(recipeId);
        }
        public void SaveRecipe()
        {
            m_DataProvider.RecipeProvider.SaveRecipe();
        }
        //public void SetCurrentRecipe(string recipeId)
        //{
        //    if (recipeId == null || recipeId == "") return;
        //    if (recipeId == m_JobCondition.CurrentRecipe.Id) return;

        //    m_DataProvider.RecipeProvider.SetCurrentRecipe(recipeId);
        //    TagRecipe recipe = new TagRecipe();
        //    m_DataProvider.RecipeProvider.GetCurrentRecipe(ref recipe);
        //    m_JobCondition.CurrentRecipe = recipe;
        //    GenInfos.CurRecipeId = m_JobCondition.CurrentRecipe.Id;
        //}
        public void SaveSetupInfos()
        {
            m_DataProvider.SaveSetupInfos();
        }
        public void SaveCalibrationInfo(TagCalibrationInfo info)
        {
            if (info == null) return;
            m_DataProvider.CalibrationProvider.UpdateToDB(info);
        }
        public bool ServoManualInterlock(_ServoUnit unit, int targetPosition)
        {
            bool interlockCondition = false;
            GantryUnit TRUnit = eqpTransferUnits._TR_Gantry_Unit;
            //FishHand LdFishHand = eqpTransferUnits._LD_Fish_Hand;
            FishHand UlFishHand = eqpTransferUnits._UL_Fish_Hand;
            CvUnit LdCvUnit = eqpTransferUnits._LD_CvUnit;
            CvUnit UlCvUnit = eqpTransferUnits._UL_CvUnit;

            short CurPositionIdTr = (short)TRUnit.Servo.GetCurPointId();//2010.09.02 kimgun
            //short CurPositionIdLd = (short)LdFishHand.Servo.GetCurPointId();//2010.09.02 kimgun
            short CurPositionIdUl = (short)UlFishHand.Servo.GetCurPointId();//2010.09.02 kimgun
            CurPositionIdTr = GetCurPointId(eqpTransferUnits._TR_Gantry_Unit.Id, CurPositionIdTr);
            //CurPositionIdLd = GetCurPointId(eqpTransferUnits._LD_Fish_Hand.Id, CurPositionIdLd);
            CurPositionIdUl = GetCurPointId(eqpTransferUnits._UL_Fish_Hand.Id, CurPositionIdUl);
            bool interlock = false;

            #region GlassExist GlassData
            bool GlassExistTr = TRUnit.IsGlassExist(Logic.AND);
            //bool GlassExistLdFh = LdFishHand.IsGlassExist(Logic.AND);
            bool GlassExistUlFh = UlFishHand.IsGlassExist(Logic.AND);
            bool GlassExistLdIn = LdCvUnit.GlsInSensor.IsDetected(Logic.OR);
            bool GlassExistLdOut = LdCvUnit.GlsOutSensor.IsDetected(Logic.OR);
            bool GlassExistUlIn = UlCvUnit.GlsInSensor.IsDetected(Logic.OR);
            bool GlassExistUlOut = UlCvUnit.GlsOutSensor.IsDetected(Logic.OR);

            #endregion

            bool LdCvMotorStop = (LdCvUnit.ManualAct[0] == CvMotorAct.Stop); // cv motor가 1개 이므로, 0
            bool UlCvMotorStop = (UlCvUnit.ManualAct[0] == CvMotorAct.Stop); // cv motor가 1개 이므로, 0

            interlock |= ((JobCond.HeavyInterlock & HeavyInterlock.Door) > 0); // 11.01.27 minhan

            if (interlock)
            {
                MessageBox.Show("Door Alarm! Can Not Move");
                interlockCondition = true;
                return interlockCondition;
            }

            // Unit별 Interlock 조건 기술
            #region TR Interlock
            if (unit.Id == eqpServoUnits._TR_Servo_Unit.Id)
            {
                // if tr align is not bw, tr don't move
                if (!TRUnit.IsAlignBw())
                {
                    MessageBox.Show("TR Align send to backward..");
                    interlockCondition |= true;
                }
                // if Loader Hand is detected, tr don't move
                else if (eqpTransferUnits._TR_Gantry_Unit.IsRobotInterlock())
                {
                    MessageBox.Show("Loader hand is interfered.");
                    interlockCondition |= true;
                }
                //else if ((GlobalVar.ServoHomeChecked) &&
                //        (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv2_Position ||
                //        CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position || CurPositionIdLd == -1)) // 11.02.08 minhan 
                //{
                //    if (CurPositionIdLd == -1)
                //        MessageBox.Show("LD Hand servo move to teaching position.");
                //    else
                //        MessageBox.Show("LD Hand servo is interfered.");
                //    //GlobalVar.ServoHomeChecked = false; //taegoo
                //    interlockCondition |= true;
                //}
                else if ((GlobalVar.ServoHomeChecked) &&
                         (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send2 ||
                         CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3 || CurPositionIdUl == -1)) // 11.02.08 minhan
                {
                    if (CurPositionIdUl == -1)
                        MessageBox.Show("UL Hand servo move to teaching position.");
                    else
                        MessageBox.Show("UL Hand servo is interfered.");
                    //GlobalVar.ServoHomeChecked = false; //taegoo
                    interlockCondition |= true;
                }
                //TR이 SEND POS가 아니고 TR이 목적지가 SEND POS이고 LD FISH가 RECV1,RECV3일 경우 RIB는 UP위치여야 한다.
                //TR이 SEND POS가 아니고 TR의 목적지가 SEND POS일 경우 LD FISH가 RECV2이면 TR은 움직일 수 없다. 
                //TR이 SEND POS가 아니고 TR의 목적지가 SEND POS일 경우 LD FISH의 위치가 불확실 할 때 TR은 움직일 수 없다.
                //else if ((CurPositionIdTr != eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (targetPosition == eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv1_Position ||
                //        CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
                //        !LdFishHand.IsHandUp()) ||
                //        (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv2_Position ||
                //        CurPositionIdLd == -1)))
                //{
                //    if(CurPositionIdLd == -1)
                //        MessageBox.Show("LD Hand servo move to teaching position.");
                //    else
                //        MessageBox.Show("LD Hand servo is interfered.");
                //    interlockCondition |= true;
                //}
                //TR이 RECV POS가 아니고 TR이 목적지가 RECV POS이고 UL FISH가 SEND1,SEND3일 경우 RIB는 UP위치여야 한다.
                //TR이 RECV POS가 아니고 TR의 목적지가 RECV POS가 일 경우 UL FISH가 SEND2이면 TR은 움직일 수 없다.
                //TR이 RECV POS가 아니고 TR의 목적지가 RECV POS가 일 경우 UL FISH의 위치가 불확실 할 때 TR은 움직일 수 없다.
                else if ((CurPositionIdTr != eqpPos_TR_Servo_Unit.Recv) &&
                        (targetPosition == eqpPos_TR_Servo_Unit.Recv) &&
                        (((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send1 ||
                        CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        !UlFishHand.IsHandUp()) ||
                        (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send2 ||
                        CurPositionIdUl == -1)))
                {
                    if (CurPositionIdUl == -1)
                        MessageBox.Show("UL Hand servo move to teaching position.");
                    else
                        MessageBox.Show("UL Hand servo is interfered.");
                    interlockCondition |= true;
                }
                //TR이 SEND POS이고 TR의 목적지가 SEND POS가 아니고 LD FISH가 RECV1,RECV3일 경우 RIB는 UP위치여야 한다.
                //TR이 SEND POS이고 LD FISH가 RECV2이면 TR은 움직일 수 없다.
                //TR이 SEND POS이고 LD FISH의 위치가 불확실 할 때 TR은 움직일 수 없다.
                //else if ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (targetPosition != eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv1_Position ||
                //        CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
                //        !LdFishHand.IsHandUp()) ||
                //        (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv2_Position ||
                //        CurPositionIdLd == -1)))
                //{
                //    if(CurPositionIdLd == -1)
                //        MessageBox.Show("LD Hand servo move to teaching position.");
                //    else
                //        MessageBox.Show("LD Hand servo is interfered.");
                //    interlockCondition |= true;
                //}
                //TR이 RECV POS이고 TR의 목적지가 RECV POS가 아니고 UL FISH가 SEND1,SEND3일 경우 RIB는 UP위치여야 한다.
                //TR이 RECV POS이고 UL FISH가 SEND2이면 TR은 움직일 수 없다.
                //TR이 RECV POS이고 UL FISH의 위치가 불확실 할 때 TR은 움직일 수 없다.
                else if ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) &&
                        (targetPosition != eqpPos_TR_Servo_Unit.Recv) &&
                        (((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send1 ||
                        CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        !UlFishHand.IsHandUp()) ||
                        (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send2 ||
                        CurPositionIdUl == -1)))
                {
                    if (CurPositionIdUl == -1)
                        MessageBox.Show("UL Hand servo move to teaching position.");
                    else
                        MessageBox.Show("UL Hand servo is interfered.");
                    interlockCondition |= true;
                }
                // LD Hand가 Recv1 에 있고, Glass가 있을 경우 Tr이 Send Position 이동시 
                //else if ((CurPositionIdTr != eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (targetPosition == eqpPos_TR_Servo_Unit.Send_Position) &&
                //        ((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv1_Position) && GlassExistLdFh) )
                //{
                //    if (CurPositionIdLd == -1)
                //        MessageBox.Show("LD Hand servo move to teaching position.");
                //    else
                //        MessageBox.Show("LD Hand Glass is exist.");
                //    interlockCondition |= true;
                //}

                // UL Hand가 Send1 에 있고, Glass가 있을 경우 Tr이 Recv Position 이동시 
                else if ((CurPositionIdTr != eqpPos_TR_Servo_Unit.Recv) &&
                        (targetPosition == eqpPos_TR_Servo_Unit.Recv) &&
                        ((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send1) && GlassExistUlFh))
                {
                    if (CurPositionIdUl == -1)
                        MessageBox.Show("UL Hand servo move to teaching position.");
                    else
                        MessageBox.Show("UL Hand Glass is exist.");
                    interlockCondition |= true;
                }
                //tr이 glass를 가지고 ld hand가 recv3에 위치해 있을때 send pos 이동 불가
                //else if (GlassExistTr && (CurPositionIdTr != eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (targetPosition == eqpPos_TR_Servo_Unit.Send_Position) &&
                //        (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position))
                //{
                //    MessageBox.Show("TR servo with glass, Do not move to send position at Ld Hand servo position is recv3.");
                //    interlockCondition |= true;
                //}
                //tr이 glass를 가지고 ul hand가 send3에 위치해 있을때 send pos 이동 불가
                else if (GlassExistTr && (CurPositionIdTr != eqpPos_TR_Servo_Unit.Recv) &&
                        (targetPosition == eqpPos_TR_Servo_Unit.Recv) &&
                        (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3))
                {
                    MessageBox.Show("TR servo with glass, Do not move to receive position at UL Hand servo position is Send3.");
                    interlockCondition |= true;
                }
                //tr의 위치를 모를때 LD FISH가 RECV1,RECV3일 경우 RIB는 UP위치여야 한다.
                //TR의 위치를 모를때 UL FISH가 SEND1,SEND3일 경우 RIB는 UP위치여야 한다.
                //TR의 위치를 모를때 LD FISH와 UL FISH가 하나라도 RECV2또는 SEND2일 경우 TR은 움직일 수 없다.
                else if ((CurPositionIdTr == -1) &&
                        (/*(((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv1_Position ||
                        CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
                        !LdFishHand.IsHandUp()) ||
                        (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv2_Position ||
                        CurPositionIdLd == -1)) ||*/
                        (((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send1 ||
                        CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        !UlFishHand.IsHandUp()) ||
                        (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send2 ||
                        CurPositionIdUl == -1))))
                {
                    MessageBox.Show("Please Hand Rib Checking. Move to no interfere teaching position");
                    interlockCondition |= true;
                }
            }
            #endregion

            //#region LD Fish Interlock
            // else if (unit.Id == eqpServoUnits._LD_Hand_Servo_Unit.Id)
            // {
            //     // LD CV에 glass가 존재하고 LD hand가 rib가 접힌상태가 아니라면 send2로 이동 금지
            //     if (((targetPosition <= eqpPos_LD_Hand_Servo_Unit.Send2_Position) ||
            //         (targetPosition == eqpPos_LD_Hand_Servo_Unit.Home_Position)) &&
            //         //((unit.ManualActionCmd == (int)RbtAction.Home) &&
            //         (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Send2_Position) &&
            //         (CurPositionIdLd != eqpPos_LD_Hand_Servo_Unit.Home_Position) &&//kimx
            //         (GlassExistLdIn || GlassExistLdOut) &&
            //         (!GlassExistLdFh || CurPositionIdLd >= eqpPos_LD_Hand_Servo_Unit.Wait_Position) &&
            //         !LdFishHand.IsHandDown())
            //     {
            //         MessageBox.Show("LD C/V Unit has a glass.");
            //         interlockCondition |= true;
            //     }
            //     //ld fish가 home이나 send1에서 send2로 이동시 ld in에 glass가 감지 안 되고 ld out에 glass가 감지되면 이동불가
            //     else if ((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Home_Position ||
            //             CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Send2_Position) &&
            //             (targetPosition != eqpPos_LD_Hand_Servo_Unit.Send2_Position) &&
            //             (targetPosition != eqpPos_LD_Hand_Servo_Unit.Home_Position) &&
            //             (!GlassExistLdIn && GlassExistLdOut) && !LdFishHand.IsHandDown())
            //     {
            //         MessageBox.Show("Glass is exist at the AP C/V in sensor. this status do not lifting a glass ");
            //         interlockCondition |= true;
            //     }

            //     //rib가 펴진것도 접힌것도 아닌 상태라면 일단 이동 금지
            //     else if (!LdFishHand.IsHandUp() && !LdFishHand.IsHandDown())
            //     {
            //         MessageBox.Show("LD Hand's Rib state is imperfect.");
            //         interlockCondition |= true;
            //     }
            //     //ld가 recv1보다 낮고 tr이 send pos이고 ld hand의 목적지가 recv1,2,3로 이동 할 수 없다.               
            //     else if ((CurPositionIdLd < eqpPos_LD_Hand_Servo_Unit.Recv1_Position) &&
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv1_Position ||
            //              targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv2_Position ||
            //              targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
            //              ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) ||
            //              CurPositionIdTr == -1))
            //     {
            //         MessageBox.Show("TR servo move to home position or wait position.");
            //         interlockCondition |= true;
            //     }
            //     else if ((CurPositionIdLd == -1) &&
            //             ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) ||
            //               CurPositionIdTr == -1)) // 11.05.14 minhan
            //     {
            //         MessageBox.Show("Don't move. Please LD Hand or TR Servo Position Check.");
            //         interlockCondition |= true;

            //     }
            //     else if (GlobalVar.ServoHomeChecked &&
            //             ((CurPositionIdLd > eqpPos_LD_Hand_Servo_Unit.Wait_Position) &&
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Home_Position) &&
            //             (CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position)) ||
            //             CurPositionIdTr == -1) // 11.02.08 minhan
            //     {
            //         if (CurPositionIdTr == -1)
            //         {
            //             MessageBox.Show("Don't move. TR Servo position is obscured.");

            //             interlockCondition |= true;
            //         }
            //         else if (CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position)
            //         {
            //             MessageBox.Show("TR servo move to home position or wait position.");
            //             interlockCondition |= true;
            //         }
            //     }
            //     //fishbone이 접힌채 올라 갈 수 있는 최대 높이는 recv1이다.
            //     //이때 tr은 send pos가 아니여야한다.
            //     else if ((CurPositionIdLd <= eqpPos_LD_Hand_Servo_Unit.Recv1_Position) &&
            //             (targetPosition > eqpPos_LD_Hand_Servo_Unit.Recv1_Position) &&
            //             LdFishHand.IsHandDown())
            //     {
            //         MessageBox.Show("Ld Hand's rib is fold. Move to Maximum position is recv1 ");
            //         interlockCondition |= true;
            //     }
            //     //tr이 send pos에 있고 LD hand가 recv1보다 낮은 위치에서 recv1,recv2,recv3으로 이동 금지
            //     else if (((targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) ||
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv2_Position) ||
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv1_Position)) &&
            //             ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) ||
            //             CurPositionIdTr == -1) &&
            //         // (GlassExistLdFh /*|| CurPositionIdUl >= eqpPos_UL_Hand_Servo_Unit.Wait_Position*/) &&//2009.09.22 kimgun ul은 왜 보는거야..
            //             CurPositionIdLd < eqpPos_LD_Hand_Servo_Unit.Recv1_Position)
            //     {
            //         if (CurPositionIdTr == -1)
            //         {
            //             MessageBox.Show("Don't move. TR Servo position is obscured.");
            //             interlockCondition |= true;
            //         }
            //         else if (CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position)
            //         {
            //             MessageBox.Show("TR servo move to home position or wait position.");
            //             interlockCondition |= true;
            //         }
            //     }
            //     else if (((targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) ||
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv2_Position) ||
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv1_Position)) &&
            //             (CurPositionIdTr == -1))
            //     {
            //         MessageBox.Show("Don't move. TR Servo position is obscured.");
            //         interlockCondition |= true;
            //     }
            //     //ld의 위치가 recv1보다 높고 tr이 send이거나 -1이면 ld hand는 recv1보다 아래로 내려 갈 수 없다.

            //     else if ((CurPositionIdLd >= eqpPos_LD_Hand_Servo_Unit.Recv1_Position) &&
            //             (CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position ||
            //             CurPositionIdTr == -1) &&
            //             (targetPosition < eqpPos_LD_Hand_Servo_Unit.Recv1_Position))
            //     {
            //         if (CurPositionIdTr == -1)
            //             MessageBox.Show("Don't move. TR Servo position is obscured.");
            //         else
            //             MessageBox.Show("TR servo move to home position or wait position.");
            //         interlockCondition |= true;
            //     }
            //     ////TR이 send에 있고 LD Hand가 Recv3에 있으면 TR에 glass 유무를 확인하라. kimx
            //     else if ((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
            //             (CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) &&
            //             (targetPosition <= eqpPos_LD_Hand_Servo_Unit.Recv2_Position) &&
            //             (!LdFishHand.IsHandDown()))
            //     {
            //         if (DialogResult.Yes == MessageBox.Show("Is exist glass in TR servo?", "WSSD", MessageBoxButtons.YesNo))
            //             interlockCondition |= true;
            //         else if (unit.VelRatio > 0.2)
            //         {
            //             MessageBox.Show("Lower speed ratio by 20 lows.");
            //             interlockCondition |= true;
            //         }
            //     }
            //     // TR에서 LD Hand로 glass인계시 속도 20이하 설정 kimx
            //     else if ((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Recv2_Position) &&
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Recv3_Position) &&
            //             (CurPositionIdTr == eqpPos_TR_Servo_Unit.Send_Position) &&
            //             GlassExistLdFh && !LdFishHand.IsHandDown() &&
            //             (unit.VelRatio > 0.2))
            //     {
            //         MessageBox.Show("Lower speed ratio by 20 lows.");
            //         interlockCondition |= true;
            //     }
            //     // LD Hand에서 LD CV로 glass인계시 속도 20이하 설정
            //     else if (((targetPosition == eqpPos_LD_Hand_Servo_Unit.Send2_Position) ||
            //             (targetPosition == eqpPos_LD_Hand_Servo_Unit.Home_Position)) &&
            //             GlassExistLdFh && !LdFishHand.IsHandDown() &&
            //             (unit.VelRatio > 0.2))
            //     {
            //         MessageBox.Show("Lower speed ratio by 20 lows.");
            //         interlockCondition |= true;
            //     }
            //     // LD CV에서 LD Hand로 glass인계시 속도 20이하 설정 kimx
            //     else if (((CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Send2_Position) ||
            //            (CurPositionIdLd == eqpPos_LD_Hand_Servo_Unit.Home_Position)) &&
            //            (GlassExistLdIn || GlassExistLdOut) &&
            //            (unit.VelRatio > 0.2) && !LdFishHand.IsHandDown())//&&
            //     //  targetPosition != -1)
            //     {
            //         MessageBox.Show("Lower speed ratio by 20 lows.");
            //         interlockCondition |= true;
            //     }
            //     else if (((targetPosition == eqpPos_LD_Hand_Servo_Unit.Send2_Position) ||
            //        (targetPosition == eqpPos_LD_Hand_Servo_Unit.Home_Position)) &&
            //        GlassExistLdFh && !LdFishHand.IsHandDown() &&
            //        !eqpActuatorUnits._LD_Align.IsNegative()) // 11.04.14 minhan
            //     {
            //         MessageBox.Show("Please LD CV Align BW");
            //         interlockCondition |= true;
            //     }


            // }
            // #endregion

            #region UL Fish Interlock
            else if (unit.Id == eqpServoUnits._UL_Hand_Servo_Unit.Id)
            {

                // UL CV에 glass가 존재하고 UL hand가 rib가 접힌상태가 아니라면 home,Recv1,2로 이동 금지
                if (((targetPosition <= eqpPos_UL_Hand_Servo_Unit.Recv2) ||
                    (unit.ManualActionCmd == (int)RbtAction.Home)) &&
                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Recv1) &&
                    (CurPositionIdUl != eqpPos_UL_Hand_Servo_Unit.Home) &&//kimx
                    (GlassExistUlIn || GlassExistUlOut) &&
                    (!GlassExistUlFh || CurPositionIdUl >= eqpPos_UL_Hand_Servo_Unit.Wait) &&
                    !UlFishHand.IsHandDown())
                {
                    MessageBox.Show("UL C/V Unit has a glass.");
                    interlockCondition |= true;
                }
                //ul fish가 home이나 recv1에서 recv2로 이동시 ul in에 glass가 감지 되고 ul out에 glass가 감지되면 이동불가
                else if ((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Home ||
                        CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Recv1) &&
                        ((targetPosition != eqpPos_UL_Hand_Servo_Unit.Recv1) &&
                        (targetPosition != eqpPos_UL_Hand_Servo_Unit.Home)) &&
                        (GlassExistUlIn && !GlassExistUlOut) && !UlFishHand.IsHandDown())
                {
                    MessageBox.Show("Glass is exist at the UL C/V Out sensor. this status do not lifting a glass ");
                    interlockCondition |= true;
                }

                //rib가 펴진것도 접힌것도 아닌 상태라면 일단 이동 금지
                else if (!UlFishHand.IsHandUp() && !UlFishHand.IsHandDown())
                {
                    MessageBox.Show("UL Hand's Rib state is imperfect.");
                    interlockCondition |= true;
                }
                else if (GlobalVar.ServoHomeChecked &&
                      ((CurPositionIdUl > eqpPos_UL_Hand_Servo_Unit.Wait) &&
                      (targetPosition == eqpPos_UL_Hand_Servo_Unit.Home) &&
                      (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv)) ||
                      CurPositionIdTr == -1)
                {
                    if (CurPositionIdTr == -1)
                    {
                        MessageBox.Show("Don't move. TR Servo position is obscured.");
                        interlockCondition |= true;
                    }
                    else if (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv)
                    {
                        MessageBox.Show("TR servo move to home position or wait position.");
                        interlockCondition |= true;
                    }
                }
                //UL가 Send1보다 낮고 tr이 recv pos이고 Ul hand의 목적지가 send 1,2,3로 이동 할 수 없다.               
                else if (CurPositionIdUl < eqpPos_UL_Hand_Servo_Unit.Send1 &&
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Send1 ||
                        targetPosition == eqpPos_UL_Hand_Servo_Unit.Send2 ||
                        targetPosition == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) ||
                         CurPositionIdTr == -1))
                {
                    MessageBox.Show("TR servo move to home position or wait position.");
                    interlockCondition |= true;
                }
                else if ((CurPositionIdUl == -1) &&
                        ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) ||
                        CurPositionIdTr == -1)) // 11.05.14 minhan
                {
                    MessageBox.Show("Don't move. Please UL Hand or TR Servo Position Check.");
                    interlockCondition |= true;
                }
                //fishbone이 접힌채 올라 갈 수 있는 최대 높이는 send1이다.
                //이때 tr은 send pos가 아니여야한다.
                else if ((CurPositionIdUl < eqpPos_UL_Hand_Servo_Unit.Send1) &&
                         (targetPosition > eqpPos_UL_Hand_Servo_Unit.Send1) &&
                         UlFishHand.IsHandDown())
                {
                    MessageBox.Show("UL Hand's rib is fold. Move to Maximum position is Send1 ");
                    interlockCondition |= true;
                }

                //tr이 Recv pos에 있고 UL hand가 send1보다 낮은 위치에서 send1,send2,send3으로 이동 금지
                else if (((targetPosition == eqpPos_UL_Hand_Servo_Unit.Send3) ||
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Send2) ||
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Send1)) &&
                        ((CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) ||
                        CurPositionIdTr == -1) &&
                        /*GlassExistUlFh &&*/
                        CurPositionIdUl < eqpPos_UL_Hand_Servo_Unit.Send1)
                {
                    if (CurPositionIdTr == -1) MessageBox.Show("Don't move. TR Servo position is obscured.");
                    else if (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv)
                        MessageBox.Show("TR servo move to home position or wait position.");
                    interlockCondition |= true;
                }
                //
                else if (((targetPosition == eqpPos_UL_Hand_Servo_Unit.Send3) ||
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Send2) ||
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Send1)) &&
                        (CurPositionIdTr == -1))
                {
                    MessageBox.Show("Don't move. TR Servo position is obscured.");
                    interlockCondition |= true;
                }
                //UL의 위치가 send1보다 높고 tr이 recv이거나 -1이면 Ul hand는 send1보다 아래로 내려 갈 수 없다.
                else if ((CurPositionIdUl >= eqpPos_UL_Hand_Servo_Unit.Send1) &&
                        (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv ||
                        CurPositionIdTr == -1) &&
                        (targetPosition < eqpPos_UL_Hand_Servo_Unit.Send1))
                {
                    if (CurPositionIdTr == -1)
                        MessageBox.Show("Don't move. TR Servo position is obscured.");
                    else
                        MessageBox.Show("TR servo move to home position or wait position.");
                    interlockCondition |= true;
                }

                //TR이 Recv에 있고 UL Hand가 Send3에 있으면 TR에 glass 유무를 확인하라.
                else if ((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) &&
                        (targetPosition <= eqpPos_UL_Hand_Servo_Unit.Send2) &&
                        (!UlFishHand.IsHandDown()))
                {
                    if (DialogResult.Yes == MessageBox.Show("Is exist glass in TR servo?", "WSSD", MessageBoxButtons.YesNo))
                        interlockCondition |= true;
                    else if (unit.VelRatio > 0.2)
                    {
                        MessageBox.Show("Lower speed ratio by 20 lows.");
                        interlockCondition |= true;
                    }
                }
                // UL Hand에서 TR로 glass인계시 속도 30이하 설정
                else if ((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        (targetPosition <= eqpPos_UL_Hand_Servo_Unit.Send2) &&
                        (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) &&
                        GlassExistUlFh &&
                        (!UlFishHand.IsHandDown()) &&
                        (unit.VelRatio > 0.2))
                {
                    MessageBox.Show("Lower speed ratio by 20 lows.");
                    interlockCondition |= true;
                }
                // TR에서 UL Hand로 glass인계시 속도 20이하 설정 kimx
                else if ((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Send2) &&
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Send3) &&
                        (CurPositionIdTr == eqpPos_TR_Servo_Unit.Recv) &&
                        GlassExistUlFh && !UlFishHand.IsHandDown() &&
                        (unit.VelRatio > 0.2))
                {
                    MessageBox.Show("Lower speed ratio by 20 lows.");
                    interlockCondition |= true;
                }
                // UL Hand에서 UL CV로 glass인계시 속도 30이하 설정
                else if (((targetPosition == eqpPos_UL_Hand_Servo_Unit.Recv1) ||
                        (targetPosition == eqpPos_UL_Hand_Servo_Unit.Home)) &&
                        GlassExistUlFh &&
                        (unit.VelRatio > 0.2))
                {
                    MessageBox.Show("Lower speed ratio by 20 lows.");
                    interlockCondition |= true;
                }
                // UL CV에서 UL Hand로 glass인계시 속도 20이하 설정 kimx
                else if (((CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Recv1) ||
                        (CurPositionIdUl == eqpPos_UL_Hand_Servo_Unit.Home)) &&
                        (GlassExistUlIn || GlassExistUlOut) &&
                        (unit.VelRatio > 0.2) && !UlFishHand.IsHandDown())// &&
                //      targetPosition != -1)
                {
                    MessageBox.Show("Lower speed ratio by 20 lows.");
                    interlockCondition |= true;
                }
            }
            GlobalVar.ServoHomeChecked = false;
            return interlockCondition;
            #endregion  
        }
        //현재 위치의 Sensor를 제외하고 나머지 센서가 감지되면 -1을 return함으로써 각 서보 unit의 위치를 위치 모름으로 처리한다.
        //TR 추락사고때문에 추가된 사항.정말 필요없을것 같지만..
        //2010.09.02 kimgun
        public short GetCurPointId(int RbtId, short posId)
        {
            bool PosSensorOk = true;
            if (posId == -1) return posId;

            if (RbtId == eqpTransferUnits._TR_Gantry_Unit.Id)
            {
                if (posId == eqpPos_TR_Servo_Unit.Home)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        //PosSensorOk &= eqpServoMotorMp2300s ._TR_Master_Servo_Motor.GetHomeSwitch();
                        PosSensorOk &= eqpServoMotors._TR_Master_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !(eqpSensors._TR_Unit_Wait_Position_Sensor.IsDetected());
                    PosSensorOk &= !(eqpSensors._TR_Unit_Recv_Position_Sensor.IsDetected());
                    PosSensorOk &= !(eqpSensors._TR_Unit_Send_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_TR_Servo_Unit.Wait)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._TR_Master_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= eqpSensors._TR_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._TR_Unit_Recv_Position_Sensor.IsDetected());
                    PosSensorOk &= !(eqpSensors._TR_Unit_Send_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_TR_Servo_Unit.Recv)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._TR_Master_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !(eqpSensors._TR_Unit_Wait_Position_Sensor.IsDetected());
                    PosSensorOk &= eqpSensors._TR_Unit_Recv_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._TR_Unit_Send_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_TR_Servo_Unit.Send)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._TR_Master_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !(eqpSensors._TR_Unit_Wait_Position_Sensor.IsDetected());
                    PosSensorOk &= !(eqpSensors._TR_Unit_Recv_Position_Sensor.IsDetected());
                    PosSensorOk &= eqpSensors._TR_Unit_Send_Position_Sensor.IsDetected();
                }
            }
            //else if (RbtId == eqpTransferUnits._LD_Fish_Hand.Id)
            //{
            //    if (posId == eqpPos_LD_Hand_Servo_Unit.Home_Position || posId == eqpPos_LD_Hand_Servo_Unit.Send2_Position)
            //    {
            //        if (!this.Simul.Motion)
            //        PosSensorOk &= eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected() );
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected() );
            //    }
            //    else if (posId == eqpPos_LD_Hand_Servo_Unit.Send1_Position)
            //    {
            //        if (!this.Simul.Motion)
            //        PosSensorOk &= !eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
            //        PosSensorOk &= eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected() );
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected() );
            //    }
            //    else if (posId == eqpPos_LD_Hand_Servo_Unit.Wait_Position)
            //    {
            //        if (!this.Simul.Motion)
            //            PosSensorOk &= !eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected();
            //        PosSensorOk &= eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected() );
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected() );
            //    }
            //    else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv1_Position)
            //    {
            //        if (!this.Simul.Motion)
            //            PosSensorOk &= !eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            //        PosSensorOk &= eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected() ;
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected() );
            //    }
            //    else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv2_Position)
            //    {
            //        if (!this.Simul.Motion)
            //            PosSensorOk &= !eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected() );
            //        PosSensorOk &= eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected() );
            //    }
            //    else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv3_Position)
            //    {
            //        if (!this.Simul.Motion)
            //            PosSensorOk &= !eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected();
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            //        PosSensorOk &= !(eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected() );
            //        PosSensorOk &= !eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            //        PosSensorOk &= eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected() ;
            //    }
            //}
            else if (RbtId == eqpTransferUnits._UL_Fish_Hand.Id)
            {
                if (posId == eqpPos_UL_Hand_Servo_Unit.Home || posId == eqpPos_UL_Hand_Servo_Unit.Recv1)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send1_Position_Sensor.IsDetected());
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Send2_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send3_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_UL_Hand_Servo_Unit.Recv2)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send1_Position_Sensor.IsDetected());
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Send2_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send3_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_UL_Hand_Servo_Unit.Wait)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                    PosSensorOk &= eqpSensors._UL_Hand_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send1_Position_Sensor.IsDetected());
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Send2_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send3_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_UL_Hand_Servo_Unit.Send1)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= eqpSensors._UL_Hand_Unit_Send1_Position_Sensor.IsDetected();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Send2_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send3_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_UL_Hand_Servo_Unit.Send2)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send1_Position_Sensor.IsDetected());
                    PosSensorOk &= eqpSensors._UL_Hand_Unit_Send2_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send3_Position_Sensor.IsDetected());
                }
                else if (posId == eqpPos_UL_Hand_Servo_Unit.Send3)
                {
                    if (!AppConfig.Instance.Simul.Motion)
                        PosSensorOk &= !eqpServoMotors._UL_Hand_Servo_Motor.GetHomeSwitch();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Recv2_Position_Sensor.IsDetected();
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Wait_Position_Sensor.IsDetected();
                    PosSensorOk &= !(eqpSensors._UL_Hand_Unit_Send1_Position_Sensor.IsDetected());
                    PosSensorOk &= !eqpSensors._UL_Hand_Unit_Send2_Position_Sensor.IsDetected();
                    PosSensorOk &= eqpSensors._UL_Hand_Unit_Send3_Position_Sensor.IsDetected();
                }
            }
            if (PosSensorOk) return posId;
            else return -1;

        }
        #endregion
    }
}
