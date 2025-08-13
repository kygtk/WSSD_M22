using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Sequence;
using Dms.Device;
using Dms.Common;
using Dms.Data;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadIntefaceControl : XSequence
    {
        #region Fields
        protected static ServerManager m_Server;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqGlassRecv(eqpInterfaceSteps._Recv_Step)); // 10.12.21 minhan
            RegisterSequence(new SeqGlassSend(eqpInterfaceSteps._Send_Step)); // 11.06.10 minhan
            RegisterSequence(new SeqMonitorIFInterlock()); // 10.12.21 minhan
                                                           // RegisterSequence(new SeqManualGlassRecv(this)); //메뉴얼은 일단 나중에 요청이 있을 경우 수정하자.... 
                                                           // RegisterSequence(new SeqManualGlassSend(this));

        }
        #endregion

        #region Constructor
        public ThreadIntefaceControl(int scanTime, ServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            RegisterSequences();
        }
        #endregion

        #region
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

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
    }

    public class SeqGlassRecv : XSeqFunction // 10.12.21 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_Eqp;
        private CvUnit m_CvUnit = null;
        private ActuatorUnit m_LdTiltingUnit = null;
        private ActuatorUnit m_LdAligner = null;
        private ActuatorUnit m_LdIdleRoller = null;
        //private short m_GantryWaitPosition;
        private GenInfoHandler m_GenInfo;
        private BOELoaderInterface m_BoeLoaderinterface;
        private InterfaceStep m_Step;
        //private GantryUnit m_GantryUnit;
        private Alarm ALM_LoaderReadyOff;
        private bool IfRecvCond;
        private bool CheckGlassDetect;
        private bool IF_Cancel;
        //private int CurGtPosition;
        private int m_TimeOut;
        //private bool CheckGtPosOk;
        #endregion

        #region Constructor
        public SeqGlassRecv(InterfaceStep step)
        {
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_GenInfo = GenInfoHandler.Instance;
            m_LdTiltingUnit = eqpActuatorUnits._LD_Tilting_Unit;
            m_LdAligner = eqpActuatorUnits._LD_Align;
            m_LdIdleRoller = eqpActuatorUnits._LD_Idle_Roller;
            m_CvUnit = eqpTransferUnits._LD_CvUnit;
            //m_GantryWaitPosition = eqpPos_TR_Servo_Unit.Wait_Position;
            //m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_BoeLoaderinterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_Step = step;

            ALM_LoaderReadyOff = new Alarm("INTERFACE RECV" + "Loader Power Off Alarm ", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.18 minhan

            this.m_SeqFunName = "INTERFACE RECV";

        }
        #endregion

        private bool RecvCond()
        {
            bool recvCond = true;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = ((heavy & HeavyInterlock.Emo) > 0);

            recvCond &= (m_GenInfo.EQPGlassCount < m_Server.SetupMaxGlassNo.GetValue<int>());
            recvCond &= !m_GenInfo.CleanOut;
            recvCond &= !m_GenInfo.CycleStop;
            recvCond &= !m_GenInfo.Pause;
            recvCond &= m_GenInfo.AutoMode;
            recvCond &= m_GenInfo.EqpInitComp;
            recvCond &= !interlock;
            recvCond &= !m_Server.SetupSingleMode.GetValue<bool>();
            recvCond &= GlobalVar.rcvREQ;
            recvCond &= GlobalVar.LoaderReady;
            recvCond &= !eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected();
            //recvCond &= !GlobalVar.rcvReportComp; // 11.03.07 minhan
            //recvCond &= !GlobalVar.NoSubstrate; // 11.02.08 minhan
            recvCond &= m_LdTiltingUnit.IsNegative();
            recvCond &= m_LdAligner.IsNegative();
            recvCond &= m_LdIdleRoller.IsNegative();
            //recvCond &= CheckGtPosOk;           
            // recvCond &= !GlobalVar.ExchnageReq; // 11.06.10 minhan
            return recvCond;
        }

        private bool InterfaceCancel() // 인터페이스 상에서 중요힌 내용으로 cancel 해야 하는 내용만.
        {
            bool Cancel = false;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            Cancel = ((heavy & HeavyInterlock.Emo) > 0);
            Cancel |= !GlobalVar.LoaderReady;
            Cancel |= !m_LdAligner.IsNegative();
            Cancel |= !m_LdTiltingUnit.IsNegative(); // Align 이거는 글쎄..
            Cancel |= !m_LdIdleRoller.IsNegative();
            // Cancel |= eqpSensors._LD_Robot_Hand_Interlock_Sensor.IsDetected();//zhangliang 130826
            return Cancel;
        }

        private bool GlassCheck() // 더 추가할 내용 추가하삼.
        {
            bool GlassDetect = false;

            GlassDetect |= m_CvUnit.GlsInSensor.IsDetected();
            GlassDetect |= m_Server.GlassData.IsExist(m_CvUnit.DataMatchingKey(0));

            return GlassDetect;
        }

        #region Sequence
        public override int Do()
        {
            if (GlobalVar.rcvIfResetReq) // 11.04.24 minhan
            {
                GlobalVar.rcvIfResetReq = false;
                GlobalVar.rcvCANCEL = false;
                GlobalVar.rcvON_IF2 = false;
                GlobalVar.rcvCOMP = false;
                GlobalVar.rcvReportComp = false;
                GlobalVar.LdIFStatus = "--";
                GlobalVar.rcvReportCancel = true; // 11.02.09 minhan
                m_Step.StepNo = 0;
                this.m_SeqNo = 0;
                return -1;
            }

            if (!GenInfoHandler.Instance.EqpInitComp) return -1;

            if (GenInfoHandler.Instance.Pause || !GenInfoHandler.Instance.AutoMode)
            {
                m_StartTicks = XFunc.GetTickCount(); // 아니면 타이머를 사용해야한다.
                return -1;
            }

            int seqNo = this.m_SeqNo;
            int nRv = -1;

            //CurGtPosition = m_GantryUnit.Servo.GetCurPointId();

            //if((CurGtPosition == (int)m_GantryWaitPosition) &&
            //    m_GantryUnit.diWait_Pos_Sensor.IsDetected() &&
            //    !m_GantryUnit.diSend_Pos_Sensor.IsDetected() &&
            //    !m_GantryUnit.diRecv_Pos_Sensor.IsDetected())
            //{
            //    CheckGtPosOk = true;
            //}
            //else CheckGtPosOk = false;

            IfRecvCond = RecvCond();
            CheckGlassDetect = GlassCheck();

            switch (seqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        //if(GlobalVar.ExchnageReq) // 11.06.10 minhan
                        //{
                        //    return -1;
                        //}

                        if (m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState()) // 시퀀스 0으로 돌아 왔다는 건 다시 한다는 말인데.
                        {
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                        }

                        if (m_Step.StepNo != 0) m_Step.StepNo = 0;

                        if (IfRecvCond && !CheckGlassDetect)
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                            //GlobalVar.rcvCOMP = false; // 10.12.21 minhan 여기서 하면 잘못하면 Tr seq랑 따로 놀수가 있을텐데 검토해야함.
                            GlobalVar.rcvON_IF2 = true; // 10.12.21 minhan 만족하는 조건이 들어 같다면, 여기서 시작이다.
                            GlobalVar.LdIFStatus = "LR"; // 11.02.09 minhan
                            m_Step.StepNo++;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Start(case 0)");
                            seqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (!IfRecvCond)
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_Step.StepNo = 0;
                            GlobalVar.rcvCANCEL = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel(case 10)");
                            seqNo = 0;
                        }
                        else if (CheckGlassDetect)
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_Step.StepNo = 0;
                            GlobalVar.rcvCANCEL = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel(case 10)");
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Detected(case 10)");
                            seqNo = 0;
                        }
                        else if (IfRecvCond && !CheckGlassDetect)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Condition OK(case 10)");
                            if (m_Simul.Melsec) m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!IfRecvCond)
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_Step.StepNo = 0;
                            GlobalVar.rcvCANCEL = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel(case 20)");
                            seqNo = 0;
                            break;
                        }
                        else if (CheckGlassDetect)
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_Step.StepNo = 0;
                            GlobalVar.rcvCANCEL = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel(case 20)");
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Detected(case 20)");
                            seqNo = 0;
                            break;
                        }
                        else if (m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                                 m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState())
                        {
                            GlobalVar.rcvREQ = false;
                            //m_GenInfo.InterfaceStatus = "Loading Start";
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                            m_Step.StepNo++;
                            m_Step.StepNo++;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Transfer Ready to Cleaner(HDC<-LOADER)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 30;
                            break;
                        }
                        else if (m_Simul.Melsec && (GetElapsedTicks() > 2000))
                        {
                            m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Load_Request.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                        }
                    }
                    break;
                case 30:
                    {
                        IF_Cancel = InterfaceCancel();

                        m_TimeOut = m_Server.SetupIfTimeoutL3.GetValue<int>() * 1000;

                        if (IF_Cancel)
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);

                            if (!GlobalVar.LoaderReady)
                            {
                                m_Step.StepNo = 0;
                                GlobalVar.rcvCANCEL = true;
                                GlobalVar.rcvON_IF2 = false;
                                GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady off (case 30)");
                                seqNo = 0;
                                break;
                            }
                            else
                            {
                                m_Step.StepNo = 0;
                                GlobalVar.rcvCANCEL = true;
                                GlobalVar.rcvON_IF2 = false;
                                GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel(case 30)");
                                seqNo = 0;
                                break;
                            }
                        }
                        else if (!m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState())
                        // 10.12.21 minhan 사양에 없는데.. 이런 상황이라면 알람을 발생하지 말고 리턴
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                            m_Step.StepNo = 0;
                            GlobalVar.rcvCANCEL = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Transfer Ready to Cleaner Signal Off(case 30)");
                            seqNo = 0;
                            break;
                        }
                        else if (m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                                 m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                                 m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState()) // 위의 알람을 사용한다면 이렇게 봐야 할 것이다.
                        {
                            if (m_Simul.Device)
                            {
                                m_CvUnit.GlsInSensor.SetState(true);
                            }
                            GlobalVar.rcvReportComp = false; // 11.04.22 minhan
                            GlobalVar.rcvReport = true; // 11.03.02 minhan siti 알람 시나리오 때문에 여기서 살림.
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Check the Robot Accessing to Cleaner signal On(case 30)");
                            m_Step.StepNo++;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 40;
                            break;
                        }
                        else if (GetElapsedTicks() > m_TimeOut)
                        {
                            m_AlarmId = m_Server.ALM_IfTimeoutL3.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Time Out Occur(case 30)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        else if (m_Simul.Melsec && (GetElapsedTicks() > 2000))
                        {
                            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Load_Request.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                        }
                    }
                    break;
                case 40: // 10.12.21 minhan 여기부분은 사양이 좀 그래서 추후 협의가 필요하다.
                    {
                        IF_Cancel = InterfaceCancel();

                        m_TimeOut = m_Server.SetupIfTimeoutL4.GetValue<int>() * 1000;

                        bool checkSensor = m_CvUnit.GlsInSensor.IsDetected();//zhangliang

                        // 이걸보고 mibTransfer_Readyto_Cleaner_Load_Stage 를 죽인다는 것인가?
                        if (!checkSensor) m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                        else if (checkSensor && !m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState())
                        {
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mobSubstrate_Present_at_Cleaner_Load_Stage signal On(case 40)");
                        }

                        if (IF_Cancel)
                        {
                            if (!GlobalVar.LoaderReady || !m_BoeLoaderinterface.mibLoader_Power_ON.GetState()) // Loaser Power 시그널이 죽었다면..11.02.17 minhan
                            {
                                m_AlarmId = ALM_LoaderReadyOff.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Power Off(case 40)");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 3000;
                                break;
                            }
                            else
                            {
                                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                                m_Step.StepNo = 0;
                                GlobalVar.rcvCANCEL = true;
                                GlobalVar.rcvON_IF2 = false;
                                GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel(case 40)");
                                seqNo = 0;
                                break;
                            }
                        }
                        else if (m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                                 !m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                                 !m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            //GlobalVar.rcvReport = true; // 11.03.02 minhan 
                            // 그래 일단 살리고 글래스 데이터는 따로 처리하고 인터페이스는 가자... 나중에 데이터주었는데 우리가 못 받았다고 난리 칠 것이다.
                            m_Step.StepNo++;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Transfer Ready to Cleaner Signal Off (HDC<-LOADER)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 50;
                        }
                        else if (GetElapsedTicks() > m_TimeOut)
                        {
                            m_AlarmId = m_Server.ALM_IfTimeoutL4.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Time Out Occur(case 40)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        else if (m_Simul.Melsec && (GetElapsedTicks() > 2000))
                        {
                            m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.SetState(false);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Load_Request.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                        }
                    }
                    break;
                case 50: // 다시 확인한다.
                    {
                        bool checkSensor = m_CvUnit.GlsInSensor.IsDetected();

                        if (checkSensor)
                        {
                            //GlobalVar.rcvReport = true; //실제 글래스가 들어오지 않았는데... 이걸 살린다면.... 

                            if (!m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState()) // 11.06.10 minhan
                            {
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(true);
                            }

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Sensor Check OK");
                            m_Step.StepNo++;
                            m_StartTicks = XFunc.GetTickCount(); // 10.12.27 minhan
                            seqNo = 60;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            if (m_Simul.Device)
                            {
                                m_CvUnit.GlsInSensor.SetState(true);
                                break;
                            }
                            m_AlarmId = m_CvUnit.ALM_GlsDataSensing.Id;//zhangliang
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_ReturnSeqNo = seqNo;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass is Not Detected(case 50)");
                            seqNo = 2000;
                        }
                    }
                    break;
                case 60:
                    {
                        // 다른 건 볼필요가 없이 시퀀스 처리를 끝내는 것이 맞다고 생각함 나머지 인터락은 tr Seq에서 처리하는게 좋지 않을까. 
                        // 아래 플래그가 살지 않을 경우의 조치가 있어야 할 것이다.
                        m_TimeOut = m_Server.DataCompTimeout.GetValue<int>() * 1000;

                        if (//m_BoeLoaderinterface.mibCleaner_Recived_Data_Read_Request.GetState() &&
                           (m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState())) // 11.02.18 minhan
                                                                                                         // Loaer 다운시 사양때문에 그런데...Loaer가 이 시그널을 살린다는 내용은 아래 시그널을 보지 않는다는 조건 아래에서 
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Recv Signal off(case 60)");
                        }
                        else if (GlobalVar.rcvReportComp) // 글래스 데이터가 생성이 완료 되었다면...
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                            GlobalVar.rcvCOMP = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.rcvReportComp = false;
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Step.StepNo++;
                            //m_GenInfo.InterfaceStatus = "Loading Complete";
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Recv Complete(case 60)");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > m_TimeOut) // 무언정지 가능성으로 추가.
                        {
                            m_AlarmId = m_Server.ALM_IfDataCompTimeout.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Time Out Occur(case 60)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                    }
                    break;
                case 500:
                    {
                        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                        bool cancelCond = InterfaceCancel();

                        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                        recoveryCond &= m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();
                        recoveryCond &= m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState();

                        if (alarmReset || cancelCond || recoveryCond)
                        {
                            if (alarmReset) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (Reset)");
                            else if (cancelCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (Interface Cancel)");
                            else if (recoveryCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (I/F)");

                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 600:
                    {
                        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                        bool cancelCond = InterfaceCancel();

                        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                        recoveryCond &= !m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();
                        recoveryCond &= !m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState();

                        if (alarmReset || cancelCond || recoveryCond)
                        {
                            if (alarmReset) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Timeout Reset (Reset)");
                            else if (cancelCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Timeout Reset (Interface Cancel)");
                            else if (recoveryCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Timeout Reset (I/F)");

                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 1000: // 타임 아웃 발생시 유저는 recovery 할 것인가 아님 초기화 할 것인가 선택하여 진행하게 처리
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        if (DialogResult.No == MessageBox.Show("Recv Interface Time out! Yes: Error Recovery No: SoftReset(DANGER))", "WSSD", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                        {
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel : case 1000");
                            GlobalVar.rcvCANCEL = true;
                            GlobalVar.rcvON_IF2 = false;
                            GlobalVar.rcvReport = false; // 10.12.21 minhan
                            GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                            m_Step.StepNo = 0;
                            m_ReturnSeqNo = 0;
                            seqNo = 0;
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery(1000)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 2000: // 글래스 센서가 감지 되지 않을 경우.
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        bool checkSensor = m_CvUnit.GlsInSensor.IsDetected();

                        if (checkSensor)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery : case 2000 Sensor Check Ok");
                            seqNo = 60;
                        }
                        else
                        {
                            if (DialogResult.No == MessageBox.Show("Glass Not Detect! Yes: Error Recovery No: SoftReset(DANGER)", "WSSD", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                            {
                                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel : case 2000");
                                GlobalVar.rcvCANCEL = true;
                                GlobalVar.rcvON_IF2 = false;
                                GlobalVar.rcvReport = false; // 10.12.21 minhan
                                GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_ReturnSeqNo = 0;
                                seqNo = 0;
                            }
                            else
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery(2000)");
                                m_StartTicks = XFunc.GetTickCount();
                                seqNo = m_ReturnSeqNo;
                            }
                        }
                    }
                    break;
                case 3000: // 사양서가 좀 그런데.. 이런 상황은 Loader가 먼저 무조건 살아야 알람해지가 가능하다. 아마 나중에 유저요청이 있으면 메세지 처리하자.
                    if (m_Eqp.AlarmResetSwitchPushed) // 11.02.17 minhan
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        bool checkSensor = m_CvUnit.GlsInSensor.IsDetected();

                        IF_Cancel = InterfaceCancel();

                        if (checkSensor && !IF_Cancel) // 글래스가 있는 상황이고, 인터락에 문제가 없는 상황이라면...문제가 된다면 60번으로 가는 건 않 좋은 것 같다.
                        {
                            //GlobalVar.rcvReport = true; // 11.03.02 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery : case 3000 Sensor or Interlock Check Ok");
                            m_ReturnSeqNo = 0;
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.17 minhan
                            seqNo = 60;
                        }
                        else
                        {
                            if (DialogResult.No == MessageBox.Show("Glass not Detected or Interlock Error! Yes: Error Recovery No: SoftReset(DANGER)", "WSSD", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                            // 유저가 선택하도록 하자.
                            {
                                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Interface Cancel : case 3000");
                                GlobalVar.rcvCANCEL = true;
                                GlobalVar.rcvON_IF2 = false;
                                GlobalVar.LdIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_ReturnSeqNo = 0;
                                seqNo = 0;
                            }
                            else
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery(3000)");
                                m_StartTicks = XFunc.GetTickCount();
                                seqNo = m_ReturnSeqNo;
                            }
                        }
                    }
                    break;
            }
            this.m_SeqNo = seqNo;
            return nRv;
        }
        #endregion
    }

    public class SeqGlassSend : XSeqFunction // 10.12.21 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_Eqp;
        private short m_GantrySendPosition;
        private GenInfoHandler m_GenInfo;
        private BOELoaderInterface m_BoeLoaderinterface;
        private InterfaceStep m_Step;
        // private InterfaceStep m_ExStep;
        private GantryUnit m_GantryUnit;
        private EGiSInterface m_EgisInterface; // 11.03.02 minhan
        private Alarm ALM_LoaderReadyOff;
        private bool IfSendCond;
        //private bool CheckGlassDetect;
        private bool IF_Cancel;
        private int CurGtPosition;
        private int m_TimeOut;
        private bool CheckGtPosOk;
        #endregion

        #region Constructor
        public SeqGlassSend(InterfaceStep step) // 11.06.10 minhan
        {
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_GenInfo = GenInfoHandler.Instance as GenInfoHandler;
            m_GantrySendPosition = eqpPos_TR_Servo_Unit.Send;
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_BoeLoaderinterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface; // 11.03.02 minhan
            m_Step = step;
            //m_ExStep = exstep;

            ALM_LoaderReadyOff = new Alarm("INTERFACE SEND" + "Loader Power Off Alarm ", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.18 minhan

            this.m_SeqFunName = "INTERFACE SEND";
        }
        #endregion

        private bool SendCond()
        {
            bool sendCond = true;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = ((heavy & HeavyInterlock.Emo) > 0);

            sendCond &= m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
            sendCond &= m_GantryUnit.IsGlassExist();
            sendCond &= !m_GenInfo.Pause;
            sendCond &= m_GenInfo.AutoMode;
            sendCond &= m_GenInfo.EqpInitComp;
            sendCond &= !interlock;
            sendCond &= !m_Server.SetupSingleMode.GetValue<bool>();
            sendCond &= GlobalVar.sndREQ;
            sendCond &= GlobalVar.LoaderReady;
            //sendCond &= !GlobalVar.sndReportComp; // 11.04.22 minhan
            sendCond &= m_GantryUnit.IsAlignBw();
            //sendCond &= CheckGtPosOk;
            //sendCond &= !eqpSensors._TR_Robot_Hand_Interlock_Sensor.IsDetected();

            if (m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>()) // 11.06.01 minhan 
            {
                sendCond &= !GlobalVar.CrackOutCheckNG;
            }
            return sendCond;
        }

        private bool InterfaceCancel() // 인터페이스 상에서 중요힌 내용으로 cancel 해야 하는 내용만.
        {
            bool Cancel = false;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            Cancel = ((heavy & HeavyInterlock.Emo) > 0);
            Cancel |= !GlobalVar.LoaderReady;
            Cancel |= !CheckGtPosOk; // TR Position
            //Cancel |= !m_GantryUnit.IsAlignBw(); // Align 이거는 글쎄..
            return Cancel;
        }

        //private bool GlassCheck() // 더 추가할 내용 추가하삼.
        //{
        //    bool GlassDetect = false;

        //    GlassDetect |= m_GantryUnit.IsGlassExist();
        //    GlassDetect |= m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));

        //    return GlassDetect;
        //}

        #region Sequence
        public override int Do()
        {
            if (GlobalVar.sndIfResetReq) // 11.04.24 minhan
            {
                GlobalVar.sndIfResetReq = false;
                GlobalVar.sndCANCEL = false;
                GlobalVar.sndON_IF2 = false;
                GlobalVar.sndCOMP = false;
                GlobalVar.sndREQ = false;
                GlobalVar.sndReportComp = false;
                GlobalVar.UlIFStatus = "--";
                GlobalVar.sndReportCancel = true; // 11.02.09 minhan
                //GlobalVar.ExchnageReq = false; // 11.06.10 minhan
                m_Step.StepNo = 0;
                //m_ExStep.StepNo = 0; // 11.06.10 minhan
                this.m_SeqNo = 0;
                return -1;
            }

            if (!GenInfoHandler.Instance.EqpInitComp) return -1;

            if (GenInfoHandler.Instance.Pause || !GenInfoHandler.Instance.AutoMode)
            {
                m_StartTicks = XFunc.GetTickCount(); // 아니면 타이머를 사용해야한다.
                return -1;
            }

            int seqNo = this.m_SeqNo;
            int nRv = -1;

            CurGtPosition = m_GantryUnit.Servo.GetCurPointId();

            if ((CurGtPosition == (int)m_GantrySendPosition) &&
                (!eqpServoMotors._TR_Master_Servo_Motor.GetHomeSwitch() || m_Simul.Device) &&
                m_GantryUnit.diSend_Pos_Sensor.IsDetected() &&
                !m_GantryUnit.diRecv_Pos_Sensor.IsDetected() &&
                !m_GantryUnit.diWait_Pos_Sensor.IsDetected()) // 11.02.08 minhan
            {
                CheckGtPosOk = true;
            }
            else CheckGtPosOk = false;

            IfSendCond = SendCond();
            //CheckGlassDetect = GlassCheck();

            switch (seqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;

                        }

                        //if(GlobalVar.ExchnageReq) GlobalVar.ExchnageReq = false; // 11.06.10 minhan

                        if (//m_BoeLoaderinterface.mobUnload_Wait.GetState() || //11.06.21 minhan
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.GetState() ||
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.GetState()) // 11.02.18 minhan 시퀀스 0으로 돌아 왔다는 건 다시 한다는 말인데.
                        {
                            //m_BoeLoaderinterface.mobUnload_Wait.SetState(false); // 11.06.21 minhan
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                        }


                        if (GlobalVar.UlIFStatus == "UW") // 11.06.10 minhan
                        {
                            if ((m_GenInfo.EQPGlassCount == 0) || m_Server.SetupSingleMode.GetValue<bool>())
                            {
                                GlobalVar.UlIFStatus = "--";
                                m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            }
                            else if (!m_BoeLoaderinterface.mobUnload_Wait_U.GetState())
                            {
                                m_BoeLoaderinterface.mobUnload_Wait_U.SetState(true);
                            }
                        }
                        else
                        {
                            if (m_BoeLoaderinterface.mobUnload_Wait_U.GetState())
                            {
                                m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            }
                        }

                        if (m_Step.StepNo != 0) m_Step.StepNo = 0;
                        //if(m_ExStep.StepNo != 0) m_ExStep.StepNo = 0; // 11.06.10 minhan

                        if (IfSendCond)
                        {
                            //if(m_Simul.Device) // 11.06.08 minhan
                            //{
                            //    if(GlobalVar.SimulExReq)
                            //    {
                            //        m_BoeLoaderinterface.mibExchange_Req.SetState(true);
                            //    }
                            //    else
                            //    {
                            //        m_BoeLoaderinterface.mibExchange_Req.SetState(false);
                            //    }
                            //}

                            //Unload Wait & Unload Stage Interlock 부분을 살려 놓자....
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(true);
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(true);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false); // 11.06.10 minhan
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Condition OK(case 0)");
                            seqNo = 3;
                        }
                    }
                    break;
                case 3: // 11.06.10 minhan
                    {
                        //if(m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                        //   !GlobalVar.NoSubstrate &&
                        //   !m_GenInfo.CleanOut && !m_GenInfo.CycleStop)
                        //{
                        //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Case(case 3)");
                        //    GlobalVar.ExchnageReq = true;
                        //    seqNo = 100;
                        //}
                        //else
                        //{
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Unload Case(case 3)");
                        //GlobalVar.ExchnageReq = false;
                        seqNo = 5;
                        //}
                    }
                    break;
                #region Exchange
                case 5:
                    {
                        if (!IfSendCond)
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel(case 5)");
                            //GlobalVar.ExchnageReq = false; // 11.06.11 minhan
                            GlobalVar.sndCANCEL = true;
                            GlobalVar.sndON_IF2 = false;
                            GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                            seqNo = 0;
                        }
                        //else if(m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                        //         !GlobalVar.NoSubstrate &&
                        //         !m_GenInfo.CleanOut && !m_GenInfo.CycleStop) // 11.06.10 minhan
                        //{
                        //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Send Interface Cancel and Exchange Case(case 5)");
                        //    GlobalVar.ExchnageReq = true;
                        //    seqNo = 100;
                        //}
                        else
                        {
                            m_Step.StepNo++;
                            GlobalVar.sndON_IF2 = true;
                            //GlobalVar.ExchnageReq = false;
                            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(true);
                            GlobalVar.UlIFStatus = "UR"; // 11.02.09 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Start(case 5)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        m_TimeOut = m_Server.SetupIfTimeoutU1.GetValue<int>() * 1000;

                        if (!IfSendCond)
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel(case 10)");
                            //GlobalVar.ExchnageReq = false; // 11.06.10 minhan
                            GlobalVar.sndCANCEL = true;
                            GlobalVar.sndON_IF2 = false;
                            GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                            m_Step.StepNo = 0;
                            seqNo = 0;
                            break;
                        }
                        //else if(m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                        //         !GlobalVar.NoSubstrate &&
                        //         !m_GenInfo.CleanOut && !m_GenInfo.CycleStop) // 11.06.10 minhan
                        //{
                        //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Send Interface Cancel and Exchange Case(case 10)");
                        //    GlobalVar.ExchnageReq = true;
                        //    m_Step.StepNo = 0;
                        //    seqNo = 100;
                        //    break;
                        //}
                        else if (m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                                 m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState()) // 11.06.21
                        {
                            GlobalVar.sndREQ = false;
                            //GlobalVar.ExchnageReq = false; // 11.06.11 minhan
                            //m_GenInfo.InterfaceStatus = "Unloading Start";

                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(true);
                            m_Step.StepNo++;
                            m_Step.StepNo++;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Transfer Ready to Cleaner(HDC<-LOADER)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                            break;
                        }
                        else if (GetElapsedTicks() > m_TimeOut)
                        {
                            m_AlarmId = m_Server.ALM_IfTimeoutU1.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T1 Time Out Occur(case 10)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 500;
                            break;
                        }
                        else if (m_Simul.Melsec && (GetElapsedTicks() > 2000))
                        {
                            m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobUnload_Wait_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Unload_Request_U.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(true);
                        }
                    }
                    break;
                case 20:
                    {
                        IF_Cancel = InterfaceCancel();

                        m_TimeOut = m_Server.SetupIfTimeoutU3.GetValue<int>() * 1000;

                        if (IF_Cancel)
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);

                            if (!GlobalVar.LoaderReady)
                            {
                                GlobalVar.sndCANCEL = true;
                                GlobalVar.sndON_IF2 = false;
                                GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady off (case 20)");
                                seqNo = 0;
                                break;
                            }
                            else
                            {
                                GlobalVar.sndCANCEL = true;
                                GlobalVar.sndON_IF2 = false;
                                GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel(case 20)");
                                seqNo = 0;
                                break;
                            }
                        }
                        else if (!m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState())
                        // 10.12.21 minhan 사양에 없는데.. 이런 상황이라면 알람을 발생하지 말고 리턴
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            GlobalVar.sndCANCEL = true;
                            GlobalVar.sndON_IF2 = false;
                            GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                            m_Step.StepNo = 0;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Transfer Ready to Cleaner Signal Off(case 20)");
                            seqNo = 0;
                            break;
                        }
                        else if (m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                                 m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState() &&
                                 m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage_U.GetState()) // 위의 알람을 사용한다면 이렇게 봐야 할 것이다.
                        {
                            if (m_Simul.Device)
                            {
                                m_GantryUnit.GlassExistSensor.SetState(false);
                            }

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Check the Robot Accessing to Cleaner signal On(case 20)");
                            m_Step.StepNo++;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 30;
                            break;
                        }
                        else if (GetElapsedTicks() > m_TimeOut)
                        {
                            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                            m_AlarmId = m_Server.ALM_IfTimeoutU3.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Time Out Occur(case 30)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        else if (m_Simul.Melsec && (GetElapsedTicks() > 2000))
                        {
                            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobUnload_Wait_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Unload_Request_U.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(true);
                        }
                    }
                    break;
                case 30:
                    {
                        IF_Cancel = InterfaceCancel();

                        m_TimeOut = m_Server.SetupIfTimeoutU4.GetValue<int>() * 1000;

                        bool checkSensor = m_GantryUnit.IsGlassExist();

                        if (checkSensor) m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(true);
                        else if (!checkSensor && m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);

                            if (m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>()) // 11.06.17 minhan
                            {
                                GlobalVar.CrackOutCheckReq = true;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mobSubstrate_Present_at_Cleaner_Unload_Stage_U signal Off(case 30 Crack Use)");
                            }
                            else
                            {
                                //GlobalVar.CrackOutCheckReq = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mobSubstrate_Present_at_Cleaner_Unload_Stage_U signal Off(case 30 Creak No Use)");
                            }
                        }

                        if (IF_Cancel)
                        {
                            if (!GlobalVar.LoaderReady || !m_BoeLoaderinterface.mibLoader_Power_ON.GetState()) // Loaser Power 시그널이 죽었다면..11.02.17 minhan
                            {
                                m_AlarmId = ALM_LoaderReadyOff.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Power Off(case 30)");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 3000;
                                break;
                            }
                            else
                            {
                                m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                                m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                                GlobalVar.sndCANCEL = true;
                                GlobalVar.sndON_IF2 = false;
                                GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel(case 30)");
                                seqNo = 0;
                                break;
                            }
                        }
                        else if (m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                                 !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState() &&
                                 !m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                            if (m_Simul.Device)
                            {
                                m_GantryUnit.GlassExistSensor.SetState(false);
                            }
                            m_Step.StepNo++;
                            GlobalVar.sndReportComp = false; // 11.06.01 minhan
                            GlobalVar.sndReport = true;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mibRobot_Accessing_at_Cleaner_Unload_Stage_U Signal Off (HDC<-LOADER)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 40;
                        }
                        else if (GetElapsedTicks() > m_TimeOut)
                        {
                            m_AlarmId = m_Server.ALM_IfTimeoutU4.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Time Out Occur(case 30)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        else if (m_Simul.Melsec && (GetElapsedTicks() > 2000))
                        {
                            m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage_U.SetState(false);
                        }

                        if (!m_BoeLoaderinterface.mobUnload_Wait_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Unload_Request_U.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(true);
                        }

                        if (!m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(true);
                        }
                    }
                    break;
                case 40:
                    {
                        bool checkSensor = m_GantryUnit.IsGlassExist();

                        if (!checkSensor)
                        {
                            if (m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState()) // 혹시나. 
                            {
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            }

                            m_Step.StepNo++;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Sensor Check OK");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.17 minhan
                            seqNo = 50; // 11.06.10 minhan
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            if (m_Simul.Device)
                            {
                                m_GantryUnit.GlassExistSensor.SetState(false);
                                break;
                            }
                            m_AlarmId = m_GantryUnit.ALM_GlassExist.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_ReturnSeqNo = seqNo;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass is Detected(case 40)"); // 11.04.22 minhan
                            seqNo = 2000;
                        }
                    }
                    break;
                case 50:
                    {
                        m_TimeOut = m_Server.DataCompTimeout.GetValue<int>() * 1000; // 11.02.17 minhan

                        if (m_BoeLoaderinterface.mobUnload_Wait_U.GetState() ||
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.GetState() ||
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.GetState())
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Recv Signal off(case 50)");
                        }
                        else if (GlobalVar.sndReportComp)
                        {
                            m_Server.GlassData.Delete(eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0)); // 11.01.27 minhan
                            GlobalVar.sndCOMP = true;
                            GlobalVar.sndON_IF2 = false;
                            //m_GenInfo.InterfaceStatus = "UnLoading Complete";
                            GlobalVar.sndReportComp = false;
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                            m_Step.StepNo++;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Send Complete(case 50)");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > m_TimeOut) // 11.02.17 minhan
                        {
                            m_AlarmId = m_Server.ALM_IfDataCompTimeout.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Time Out Occur(case 50)");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                    }
                    break;
                #endregion

                //#region Exchange
                //case 100: // 11.06.10 minhan
                //    {
                //        if(!IfSendCond)
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel(case 100)");
                //            GlobalVar.sndCANCEL = true;
                //            GlobalVar.sndON_IF2 = false;
                //            GlobalVar.ExchnageReq = false;
                //            GlobalVar.IFStatus = "--";
                //            seqNo = 0;
                //        }
                //        else if(!m_BoeLoaderinterface.mibExchange_Req.GetState() ||
                //                 GlobalVar.NoSubstrate ||
                //                 m_GenInfo.CleanOut || m_GenInfo.CycleStop)
                //        {
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel mibExchange_Req off(case 100)");
                //            GlobalVar.ExchnageReq = false;
                //            seqNo = 5;
                //        }
                //        else
                //        {
                //            m_ExStep.StepNo++;
                //            m_ExStep.StepNo++;
                //            GlobalVar.ExchnageReq = true; // 11.06.10 minhan
                //            GlobalVar.sndON_IF2 = true;
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(true);
                //            GlobalVar.IFStatus = "ER";
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Start(case 100)");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 110;
                //        }
                //    }
                //    break;
                //case 110: // 11.06.10 minhan
                //    {
                //        //m_TimeOut = m_Server.SetupIfTimeoutE1.GetValue<int>() * 1000;

                //        if(!IfSendCond)
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel(case 110)");
                //            GlobalVar.sndCANCEL = true;
                //            GlobalVar.sndON_IF2 = false;
                //            GlobalVar.ExchnageReq = false;
                //            GlobalVar.IFStatus = "--";
                //            m_ExStep.StepNo = 0;
                //            seqNo = 0;
                //            break;
                //        }
                //        else if(!m_BoeLoaderinterface.mibExchange_Req.GetState() ||
                //                 GlobalVar.NoSubstrate ||
                //                 m_GenInfo.CleanOut || m_GenInfo.CycleStop)
                //        {
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel mibExchange_Req off(case 110)");
                //            GlobalVar.ExchnageReq = false;
                //            m_ExStep.StepNo = 0;
                //            seqNo = 5;
                //            break;
                //        }
                //        else if(m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                //                 m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                //                 m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                //                 m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState())
                //        {
                //            GlobalVar.sndREQ = false;
                //            GlobalVar.ExchnageReq = true; // 11.06.11 minhan

                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(true);
                //            m_ExStep.StepNo++;
                //            m_ExStep.StepNo++;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Transfer Ready to Cleaner(HDC<-LOADER)");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 120;
                //            break;
                //        }
                //        //else if (GetElapsedTicks() > m_TimeOut)
                //        //{
                //        //    AlarmId = m_Server.ALM_IfTimeoutE1.Id;
                //        //    m_Eqp.SetAlarm(AlarmId);
                //        //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T1 Time Out Occur(case 110)");
                //        //    ReturnSeqNo = seqNo;
                //        //    seqNo = 4000;
                //        //    break;
                //        //}
                //        else if(m_Simul.Melsec && (GetElapsedTicks() > 2000))
                //        {
                //            m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.SetState(true);
                //            m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.SetState(true);
                //        }

                //        if(!m_BoeLoaderinterface.mobUnload_Wait.GetState() ||
                //            !m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.GetState() ||
                //             m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Unload_Request.GetState())
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(true);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(true);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(true);
                //        }
                //    }
                //    break;
                //case 120:
                //    {
                //        IF_Cancel = InterfaceCancel();

                //        m_TimeOut = m_Server.SetupIfTimeoutE3.GetValue<int>() * 1000;

                //        if(IF_Cancel)
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);

                //            if(!GlobalVar.LoaderReady)
                //            {
                //                GlobalVar.sndCANCEL = true;
                //                GlobalVar.sndON_IF2 = false;
                //                GlobalVar.ExchnageReq = false;
                //                GlobalVar.IFStatus = "--";
                //                m_ExStep.StepNo = 0;
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange LoaderReady off (case 120)");
                //                seqNo = 0;
                //                break;
                //            }
                //            else
                //            {
                //                GlobalVar.sndCANCEL = true;
                //                GlobalVar.sndON_IF2 = false;
                //                GlobalVar.ExchnageReq = false;
                //                GlobalVar.IFStatus = "--";
                //                m_ExStep.StepNo = 0;
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel(case 120)");
                //                seqNo = 0;
                //                break;
                //            }
                //        }
                //        else if(!m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState() ||
                //                 !m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState()) // 이런 경우도 cancel 이라고 봐야 하겠지
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);

                //            GlobalVar.sndCANCEL = true;
                //            GlobalVar.sndON_IF2 = false;
                //            GlobalVar.ExchnageReq = false;
                //            GlobalVar.IFStatus = "--";
                //            m_ExStep.StepNo = 0;

                //            if(!m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                //                !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState())
                //            {
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Load and unload Transfer Ready to Cleaner Signal Off(case 120)");
                //            }
                //            else if(!m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState())
                //            {
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Load Transfer Ready to Cleaner Signal Off(case 120)");
                //            }
                //            else
                //            {
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange unload Transfer Ready to Cleaner Signal Off(case 120)");
                //            }

                //            seqNo = 0;
                //            break;
                //        }
                //        else if(m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                //                 m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                //                 m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                //                 m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState() &&
                //                 m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.GetState() &&
                //                 m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState())
                //        {
                //            if(m_Simul.Device)
                //            {
                //                m_GantryUnit.GlassExistSensor.DiSensor.SetState(false);
                //            }

                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Check the Robot Accessing to Cleaner signal On(case 120)");
                //            m_ExStep.StepNo++;
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 130;
                //            break;
                //        }
                //        else if(GetElapsedTicks() > m_TimeOut)
                //        {
                //            AlarmId = m_Server.ALM_IfTimeoutE3.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T3 Time Out Occur(case 120)");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 4000;
                //            break;
                //        }
                //        else if(m_Simul.Melsec && (GetElapsedTicks() > 2000))
                //        {
                //            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.SetState(true);
                //            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.SetState(true);
                //        }

                //        if(!m_BoeLoaderinterface.mobUnload_Wait.GetState() ||
                //            !m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.GetState() ||
                //             m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Unload_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(true);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(true);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(true);
                //        }
                //    }
                //    break;
                //case 130:
                //    {
                //        IF_Cancel = InterfaceCancel();

                //        m_TimeOut = m_Server.SetupIfTimeoutE4.GetValue<int>() * 1000;

                //        bool checkSensor = m_GantryUnit.IsGlassExist();

                //        if(checkSensor) m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(true);
                //        else if(!checkSensor && m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);

                //            if(m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>())
                //            {
                //                GlobalVar.CrackOutCheckReq = true;
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange mobSubstrate_Present_at_Cleaner_Unload_Stage signal Off(case 130 Crack Use)");
                //            }
                //            else
                //            {
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange mobSubstrate_Present_at_Cleaner_Unload_Stage signal Off(case 130 Creak No Use)");
                //            }
                //        }

                //        if(IF_Cancel)
                //        {
                //            if(!GlobalVar.LoaderReady || !m_BoeLoaderinterface.mibLoader_Power_ON.GetState())
                //            {
                //                AlarmId = ALM_LoaderReadyOff.Id;
                //                m_Eqp.SetAlarm(AlarmId);
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Loader Power Off(case 130)");
                //                seqNo = 5000;
                //                break;
                //            }
                //            else
                //            {
                //                m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //                m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);

                //                GlobalVar.sndCANCEL = true;
                //                GlobalVar.sndON_IF2 = false;
                //                GlobalVar.ExchnageReq = false;
                //                GlobalVar.IFStatus = "--"; // 11.02.09 minhan
                //                m_ExStep.StepNo = 0;
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel(case 130)");
                //                seqNo = 0;
                //                break;
                //            }
                //        }
                //        else if(m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                //                 m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                //                 m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                //                 !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState() &&
                //                 m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState() &&
                //                 m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.GetState())
                //        {
                //            if(m_Simul.Device)
                //            {
                //                m_GantryUnit.GlassExistSensor.DiSensor.SetState(false);
                //            }

                //            m_ExStep.StepNo++;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, " Exchange mibTransfer_Ready_to_Cleaner_Unload_Stage_L Signal Off (case 130)");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 140;
                //        }
                //        else if(GetElapsedTicks() > m_TimeOut)
                //        {
                //            AlarmId = m_Server.ALM_IfTimeoutE4.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T4 Time Out Occur(case 130)");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 4000;
                //            break;
                //        }
                //        else if(m_Simul.Melsec && (GetElapsedTicks() > 2000))
                //        {
                //            m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.SetState(false);
                //        }

                //        if(!m_BoeLoaderinterface.mobUnload_Wait.GetState() ||
                //            !m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.GetState() ||
                //             m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                //            //!m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Unload_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(true);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(true);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(true);
                //        }
                //    }
                //    break;
                //case 140:
                //    {
                //        bool checkSensor = m_GantryUnit.IsGlassExist();

                //        if(!checkSensor)
                //        {
                //            if(m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState())
                //            {
                //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            }

                //            m_Server.GlassData.Delete(eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0));
                //            GlobalVar.ExchnageReq = true; // 11.06.10 minhan 혹시나
                //            GlobalVar.rcvCOMP = false;
                //            GlobalVar.rcvON_IF2 = true;
                //            GlobalVar.sndON_IF2 = false;
                //            GlobalVar.sndCOMP = true;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Glass Send Complete(case 140)");

                //            //if (m_Simul.Device) // 11.06.10 minhan
                //            //{
                //            //    m_GantryUnit.GlassExistSensor.DiSensor.SetState(true);
                //            //}

                //            m_ExStep.StepNo++;
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 150;
                //        }
                //        else if(GetElapsedTicks() > 2000)
                //        {
                //            if(m_Simul.Device)
                //            {
                //                m_GantryUnit.GlassExistSensor.DiSensor.SetState(false);
                //                break;
                //            }
                //            AlarmId = m_GantryUnit.ALM_GlassExist.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_ReturnSeqNo = seqNo;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Glass is Detected(case 140)");
                //            seqNo = 6000;
                //        }
                //    }
                //    break;
                //case 150: // Recv Check Point
                //    {
                //        IF_Cancel = InterfaceCancel();

                //        m_TimeOut = m_Server.SetupIfTimeoutE4.GetValue<int>() * 1000;

                //        bool checkSensor = m_GantryUnit.IsGlassExist();

                //        if(!checkSensor) m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //        else if(checkSensor && !m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(true);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange mobSubstrate_Present_at_Cleaner_Load_Stage signal On(case 150)");
                //        }

                //        if(IF_Cancel)
                //        {
                //            if(!GlobalVar.LoaderReady || !m_BoeLoaderinterface.mibLoader_Power_ON.GetState())
                //            {
                //                AlarmId = ALM_LoaderReadyOff.Id;
                //                m_Eqp.SetAlarm(AlarmId);
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Loader Power Off(case 150)");
                //                seqNo = 5500;
                //                break;
                //            }
                //            else
                //            {
                //                m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //                m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //                GlobalVar.rcvCANCEL = true;
                //                GlobalVar.rcvON_IF2 = false;
                //                GlobalVar.ExchnageReq = false;
                //                GlobalVar.IFStatus = "--";
                //                m_ExStep.StepNo = 0;
                //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel(case 150)");
                //                seqNo = 0;
                //                break;
                //            }
                //        }
                //        else if(m_BoeLoaderinterface.mibLoader_Power_ON.GetState() &&
                //            //!m_BoeLoaderinterface.mibExchange_Req.GetState() &&
                //                 !m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState() &&
                //                 !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState() &&
                //                 !m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState() &&
                //                 !m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.GetState()) // 11.06.14 minhan
                //        {
                //            GlobalVar.rcvReportComp = false;
                //            GlobalVar.rcvReport = true;

                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);

                //            if(m_Simul.Device)
                //            {
                //                m_GantryUnit.GlassExistSensor.DiSensor.SetState(true);
                //            }

                //            m_ExStep.StepNo++;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, " Exchange mibTransfer_Readyto_Cleaner_Load_Stage Signal Off (HDC<-LOADER)");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 160;
                //        }
                //        else if(GetElapsedTicks() > m_TimeOut)
                //        {
                //            AlarmId = m_Server.ALM_IfTimeoutE4.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T4 Time Out Occur(case 150)");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 4500;
                //            break;
                //        }
                //        else if(m_Simul.Melsec && (GetElapsedTicks() > 2000))
                //        {
                //            m_BoeLoaderinterface.mibExchange_Req.SetState(false);
                //            m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.SetState(false);
                //            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.SetState(false);
                //            m_GantryUnit.GlassExistSensor.DiSensor.SetState(true);
                //        }

                //        if(!m_BoeLoaderinterface.mobUnload_Wait.GetState() ||
                //            !m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.GetState() ||
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                //             m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Unload_Request.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                //            !m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(true);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(true);
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(true);
                //        }
                //    }
                //    break;
                //case 160:
                //    {
                //        bool checkSensor = m_GantryUnit.IsGlassExist();

                //        if(checkSensor)
                //        {
                //            if(!m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState()) // 11.06.10 minhan
                //            {
                //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(true);
                //            }

                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Glass Sensor Check OK(case 160)");
                //            m_ExStep.StepNo++;
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 170;
                //        }
                //        else if(GetElapsedTicks() > 2000)
                //        {
                //            if(m_Simul.Device)
                //            {
                //                m_GantryUnit.GlassExistSensor.DiSensor.SetState(true);
                //                break;
                //            }
                //            AlarmId = m_GantryUnit.ALM_GlassExist.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_ReturnSeqNo = seqNo;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Glass is Not Detected(case 160)");
                //            seqNo = 6500;
                //        }
                //    }
                //    break;
                //case 170: // 아래부터가 데이터 처리부분
                //    {
                //        m_TimeOut = m_Server.DataCompTimeout.GetValue<int>() * 1000;

                //        if(m_BoeLoaderinterface.mobUnload_Wait.GetState() ||
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.GetState() ||
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                //        }
                //        else if(GlobalVar.rcvReportComp)
                //        {
                //            GlobalVar.rcvReportComp = false;
                //            GlobalVar.sndReportComp = false;
                //            GlobalVar.sndReport = true;
                //            m_ExStep.StepNo++;
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Glass Data Recv Complete(case 170)");
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Glass Data Send Report(case 170)");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 180;
                //        }
                //        else if(GetElapsedTicks() > m_TimeOut) // 무언정지 가능성으로 추가.
                //        {
                //            AlarmId = m_Server.ALM_IfDataCompTimeout.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Time Out Occur(case 170)");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 4500;
                //            break;
                //        }
                //    }
                //    break;
                //case 180:
                //    {
                //        m_TimeOut = m_Server.DataCompTimeout.GetValue<int>() * 1000;

                //        if(m_BoeLoaderinterface.mobUnload_Wait.GetState() ||
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.GetState() ||
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState() ||
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.GetState() ||
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.GetState())
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            //m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                //        }
                //        else if(GlobalVar.sndReportComp)
                //        {
                //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);

                //            GlobalVar.sndCOMP = false;
                //            GlobalVar.rcvON_IF2 = false;
                //            GlobalVar.sndON_IF2 = false;
                //            GlobalVar.rcvReportComp = false;
                //            GlobalVar.sndReportComp = false;
                //            GlobalVar.ExchnageReq = false;
                //            GlobalVar.rcvCOMP = true;
                //            GlobalVar.IFStatus = "--";
                //            m_ExStep.StepNo++;

                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Glass Data Send Complete(case 180)");
                //            seqNo = 0;
                //        }
                //        else if(GetElapsedTicks() > m_TimeOut)
                //        {
                //            AlarmId = m_Server.ALM_IfDataCompTimeout.Id;
                //            m_Eqp.SetAlarm(AlarmId);
                //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Time Out Occur(case 180)");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 4500;
                //            break;
                //        }
                //    }
                //    break;
                //#endregion

                #region T1 Timeout Recovery
                case 500:
                    {
                        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                        bool sendCancel = !IfSendCond;

                        //bool exchangeCond = m_BoeLoaderinterface.mibExchange_Req.GetState();
                        //exchangeCond &= !GlobalVar.NoSubstrate;
                        //exchangeCond &= !m_GenInfo.CleanOut;
                        //exchangeCond &= !m_GenInfo.CycleStop;

                        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                        recoveryCond &= m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState();
                        //recoveryCond &= !m_BoeLoaderinterface.mibExchange_Req.GetState();

                        if (alarmReset || sendCancel || recoveryCond)
                        {
                            if (alarmReset) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T1 Timeout Reset (Reset)");
                            else if (sendCancel) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T1 Timeout Reset (SendCancel)");
                            //else if(exchangeCond) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "T1 Timeout Reset (Exchange Condition)");
                            else if (recoveryCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T1 Timeout Reset (I/F)");

                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                #endregion

                #region T3 Timeout Recovery
                case 600:
                    {
                        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                        bool cancelCond = InterfaceCancel();

                        bool transReadyUlOff = !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState();

                        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                        recoveryCond &= m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState();
                        recoveryCond &= m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage_U.GetState();

                        if (alarmReset || cancelCond || transReadyUlOff || recoveryCond)
                        {
                            if (alarmReset) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (Reset)");
                            else if (cancelCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (Interface Cancel)");
                            else if (transReadyUlOff) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (TransferReadyUl Off)");
                            else if (recoveryCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T3 Timeout Reset (I/F)");

                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                #endregion

                #region T4 Timeout Recovery
                case 700:
                    {
                        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                        bool cancelCond = InterfaceCancel();

                        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                        recoveryCond &= !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_U.GetState();
                        recoveryCond &= !m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage_U.GetState();

                        if (alarmReset || cancelCond || recoveryCond)
                        {
                            if (alarmReset) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Timeout Reset (Reset)");
                            else if (cancelCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Timeout Reset (Interface Cancel)");
                            else if (recoveryCond) m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "T4 Timeout Reset (I/F)");

                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                #endregion

                //#region T3(Ex) Timeout Recovery
                //case 800:
                //    {
                //        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                //        bool cancelCond = InterfaceCancel();

                //        bool transReadyOff = !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState();
                //        transReadyOff |= !m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();

                //        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibExchange_Req.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState();

                //        if(alarmReset || cancelCond || transReadyOff || recoveryCond)
                //        {
                //            if(alarmReset) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T3 Timeout Reset (Reset)");
                //            else if(cancelCond) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T3 Timeout Reset (Interface Cancel)");
                //            else if(transReadyOff) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T3 Timeout Reset (TransferReadyEx Off)");
                //            else if(recoveryCond) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T3 Timeout Reset (I/F)");

                //            m_Eqp.ResetAlarm(AlarmId);
                //            AlarmId = 0;
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = ReturnSeqNo;
                //        }
                //    }
                //    break;
                //#endregion

                //#region T4(Ex) Timeout Recovery
                //case 900:
                //    {
                //        bool alarmReset = m_Eqp.AlarmResetSwitchPushed;

                //        bool cancelCond = InterfaceCancel();

                //        bool recoveryCond = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibExchange_Req.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();
                //        recoveryCond &= !m_BoeLoaderinterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState();
                //        recoveryCond &= m_BoeLoaderinterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.GetState();

                //        if(alarmReset || cancelCond || recoveryCond)
                //        {
                //            if(alarmReset) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T4 Timeout Reset (Reset)");
                //            else if(cancelCond) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T4 Timeout Reset (Interface Cancel)");
                //            else if(recoveryCond) m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange T4 Timeout Reset (I/F)");

                //            m_Eqp.ResetAlarm(AlarmId);
                //            AlarmId = 0;
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = ReturnSeqNo;
                //        }
                //    }
                //    break;
                //#endregion

                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        if (DialogResult.No == MessageBox.Show("Send Interface Time out! Yes: Error Recovery No: SoftReset(DANGER))", "WSSD", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                        {
                            m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                            m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                            GlobalVar.sndCANCEL = true;
                            GlobalVar.sndON_IF2 = false;
                            GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                            m_Step.StepNo = 0;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel : case 1000");
                            m_ReturnSeqNo = 0;
                            seqNo = 0;
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery(1000)");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 2000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        bool checkSensor = m_GantryUnit.IsGlassExist();

                        if (!checkSensor)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery : case 2000 Sensor Check Ok");
                            m_ReturnSeqNo = 0;
                            seqNo = 40; // 11.06.01 minhan
                        }
                        else
                        {
                            if (DialogResult.No == MessageBox.Show("Glass Not Detect! Yes: Error Recovery No: SoftReset(DANGER)", "WSSD", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                            {
                                m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                                m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                                GlobalVar.sndCANCEL = true;
                                GlobalVar.sndON_IF2 = false;
                                GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel : case 2000");
                                m_ReturnSeqNo = 0;
                                seqNo = 0;
                            }
                            else
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery(2000)");
                                m_StartTicks = XFunc.GetTickCount();
                                seqNo = m_ReturnSeqNo;
                            }
                        }
                    }
                    break;
                case 3000:
                    if (m_Eqp.AlarmResetSwitchPushed) // 11.02.17 minhan
                    {
                        bool checkSensor = m_GantryUnit.IsGlassExist();

                        IF_Cancel = InterfaceCancel();

                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        if (!checkSensor && !IF_Cancel)
                        {
                            //GlobalVar.sndReport = true; // 11.06.01 minhan
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery : case 3000 Sensor or Interlock Check Ok");
                            m_ReturnSeqNo = 0;
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.17 minhan
                            seqNo = 40; // 11.06.01 minhan
                        }
                        else
                        {
                            if (DialogResult.No == MessageBox.Show("Glass not Detected or Interlock Error! Yes: Error Recovery No: SoftReset(DANGER)", "WSSD", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                            {
                                m_BoeLoaderinterface.mobUnload_Wait_U.SetState(false);
                                m_BoeLoaderinterface.mobUnload_Stage_Inter_lock_U.SetState(false);
                                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Unload_Request_U.SetState(false);
                                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                                GlobalVar.sndCANCEL = true;
                                GlobalVar.sndON_IF2 = false;
                                GlobalVar.UlIFStatus = "--"; // 11.02.09 minhan
                                m_Step.StepNo = 0;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Send Interface Cancel : case 3000");
                                m_ReturnSeqNo = 0;
                                seqNo = 0;
                            }
                            else
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery(3000)");
                                m_StartTicks = XFunc.GetTickCount();
                                seqNo = m_ReturnSeqNo;
                            }
                        }
                    }
                    break;
                    //case 4000: // 11.06.10 minhan
                    //    if(m_Eqp.AlarmResetSwitchPushed)
                    //    {
                    //        m_Eqp.ResetAlarm(AlarmId);
                    //        AlarmId = 0;

                    //        if(DialogResult.No == MessageBox.Show("Exchange Interface Time out! Yes: Error Recovery No: SoftReset(DANGER))", "WSSD", MessageBoxButtons.YesNo,
                    //            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                    //        {
                    //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                    //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                    //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                    //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                    //            GlobalVar.sndCANCEL = true;
                    //            GlobalVar.sndON_IF2 = false;
                    //            GlobalVar.ExchnageReq = false;
                    //            GlobalVar.IFStatus = "--";
                    //            m_ExStep.StepNo = 0;
                    //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel : case 4000");
                    //            ReturnSeqNo = 0;
                    //            seqNo = 0;
                    //        }
                    //        else
                    //        {
                    //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Error Recovery(4000)");
                    //            m_StartTicks = XFunc.GetTickCount();
                    //            seqNo = ReturnSeqNo;
                    //        }
                    //    }
                    //    break;
                    //case 4500: // 11.06.10 minhan
                    //    if(m_Eqp.AlarmResetSwitchPushed)
                    //    {
                    //        m_Eqp.ResetAlarm(AlarmId);
                    //        AlarmId = 0;

                    //        if(DialogResult.No == MessageBox.Show("Exchange Interface Time out! Yes: Error Recovery No: SoftReset(DANGER))", "WSSD", MessageBoxButtons.YesNo,
                    //            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                    //        {
                    //            m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                    //            m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                    //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                    //            m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                    //            m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                    //            GlobalVar.rcvCANCEL = true;
                    //            GlobalVar.rcvON_IF2 = false;
                    //            GlobalVar.ExchnageReq = false;
                    //            GlobalVar.IFStatus = "--";
                    //            m_ExStep.StepNo = 0;
                    //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel : case 4500");
                    //            ReturnSeqNo = 0;
                    //            seqNo = 0;
                    //        }
                    //        else
                    //        {
                    //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Error Recovery(4500)");
                    //            m_StartTicks = XFunc.GetTickCount();
                    //            seqNo = ReturnSeqNo;
                    //        }
                    //    }
                    //    break;
                    //case 5000: // 11.06.10 minhan
                    //    if(m_Eqp.AlarmResetSwitchPushed)
                    //    {
                    //        m_Eqp.ResetAlarm(AlarmId);
                    //        AlarmId = 0;

                    //        m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                    //        m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                    //        m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                    //        m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                    //        GlobalVar.sndCANCEL = true;
                    //        GlobalVar.sndON_IF2 = false;
                    //        GlobalVar.ExchnageReq = false;
                    //        GlobalVar.IFStatus = "--";
                    //        m_ExStep.StepNo = 0;
                    //        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel : case 5000");
                    //        ReturnSeqNo = 0;
                    //        seqNo = 0;
                    //    }
                    //    break;
                    //case 5500: // 11.06.10 minhan
                    //    if(m_Eqp.AlarmResetSwitchPushed)
                    //    {
                    //        m_Eqp.ResetAlarm(AlarmId);
                    //        AlarmId = 0;

                    //        m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                    //        m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                    //        m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                    //        m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                    //        m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                    //        GlobalVar.rcvCANCEL = true;
                    //        GlobalVar.rcvON_IF2 = false;
                    //        GlobalVar.ExchnageReq = false;
                    //        GlobalVar.IFStatus = "--";
                    //        m_ExStep.StepNo = 0;
                    //        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel : case 5500");
                    //        ReturnSeqNo = 0;
                    //        seqNo = 0;
                    //    }
                    //    break;
                    //case 6000: // 11.06.10 minhan
                    //    if(m_Eqp.AlarmResetSwitchPushed)
                    //    {
                    //        m_Eqp.ResetAlarm(AlarmId);
                    //        AlarmId = 0;

                    //        bool checkSensor = m_GantryUnit.IsGlassExist();

                    //        if(!checkSensor)
                    //        {
                    //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Error Recovery : case 6000 Sensor Check Ok");
                    //            seqNo = ReturnSeqNo;
                    //        }
                    //        else
                    //        {
                    //            if(DialogResult.No == MessageBox.Show("Exchange Glass Detect! Yes: Error Recovery No: SoftReset(DANGER)", "WSSD", MessageBoxButtons.YesNo,
                    //                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                    //            {
                    //                m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                    //                m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                    //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                    //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                    //                GlobalVar.sndCANCEL = true;
                    //                GlobalVar.sndON_IF2 = false;
                    //                GlobalVar.ExchnageReq = false;
                    //                GlobalVar.IFStatus = "--";
                    //                m_ExStep.StepNo = 0;
                    //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel : case 6000");
                    //                ReturnSeqNo = 0;
                    //                seqNo = 0;
                    //            }
                    //            else
                    //            {
                    //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Error Recovery(6000)");
                    //                m_StartTicks = XFunc.GetTickCount();
                    //                seqNo = ReturnSeqNo;
                    //            }
                    //        }
                    //    }
                    //    break;
                    //case 6500: // 11.06.10 minhan
                    //    if(m_Eqp.AlarmResetSwitchPushed)
                    //    {
                    //        m_Eqp.ResetAlarm(AlarmId);
                    //        AlarmId = 0;

                    //        bool checkSensor = m_GantryUnit.IsGlassExist();

                    //        if(checkSensor)
                    //        {
                    //            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Error Recovery : case 6500 Sensor Check Ok");
                    //            seqNo = ReturnSeqNo;
                    //        }
                    //        else
                    //        {
                    //            if(DialogResult.No == MessageBox.Show("Exchange Glass Not Detect! Yes: Error Recovery No: SoftReset(DANGER)", "WSSD", MessageBoxButtons.YesNo,
                    //                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                    //            {
                    //                m_BoeLoaderinterface.mobUnload_Wait.SetState(false);
                    //                m_BoeLoaderinterface.mobUnload_Stage_Inter_Lock.SetState(false);
                    //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Load_Stage.SetState(false);
                    //                m_BoeLoaderinterface.mobSubstrate_Present_at_Cleaner_Unload_Stage.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Load_Request.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Unload_Request.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                    //                m_BoeLoaderinterface.mobCleaner_Access_Possible_Unload_Stage.SetState(false);
                    //                GlobalVar.rcvCANCEL = true;
                    //                GlobalVar.rcvON_IF2 = false;
                    //                GlobalVar.ExchnageReq = false;
                    //                GlobalVar.IFStatus = "--";
                    //                m_ExStep.StepNo = 0;
                    //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Interface Cancel : case 6500");
                    //                ReturnSeqNo = 0;
                    //                seqNo = 0;
                    //            }
                    //            else
                    //            {
                    //                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Error Recovery(6500)");
                    //                m_StartTicks = XFunc.GetTickCount();
                    //                seqNo = ReturnSeqNo;
                    //            }
                    //        }
                    //    }
                    //    break;
            }
            this.m_SeqNo = seqNo;
            return nRv;
        }
        #endregion
    }

    public class SeqMonitorIFInterlock : XSeqFunction // 10.12.21 minhan
    {
        #region fields
        private static BOELoaderInterface m_BoeLoaderinterface;
        private static ServerManager m_Server;
        private static GenInfoHandler m_Geninfo;
        private Simul m_Simul;
        private bool LoaderReady;
        private bool m_FirstlogReady; // 11.02.08 minhsn
        private bool m_FirstlogNoSub; // 11.02.08 minhan
        //private bool m_FirstlogExReq; // 11.06.10 minhan
        private bool NoSubstrate; // 11.02.08 minhan
        private string buf1; // 11.02.09 minhan
        private string buf2;
        #endregion

        #region Constructor
        public SeqMonitorIFInterlock()
        {
            m_Server = ServerManager.Instance;
            m_Geninfo = GenInfoHandler.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_BoeLoaderinterface = eqpBOELoaderInterfaces._LoaderInterface;
            LoaderReady = false;
            m_FirstlogReady = false;
            m_FirstlogNoSub = false;
            //m_FirstlogExReq = false; // 11.06.10 minhan
            NoSubstrate = false;
            //m_ExchangeReq = false; // 11.06.10 minhan
            buf1 = "";
            buf2 = "";
            this.m_SeqFunName = "INTERFACE Monitor";
        }
        #endregion

        private void InterlockCheck()
        {
            //if (m_Simul.Melsec) // 11.06.10 minhan
            //{
            //    m_BoeLoaderinterface.mibLoader_Power_ON.SetState(true);
            //}

            LoaderReady = m_BoeLoaderinterface.mibLoader_Power_ON.GetState();
            NoSubstrate = m_BoeLoaderinterface.mibNoSubstarate_to_Cleaner.GetState();
            //m_ExchangeReq = m_BoeLoaderinterface.mibExchange_Req.GetState(); // 11.06.10 minhan
        }
        #region Sequence
        public override int Do()
        {
            InterlockCheck();

            if (LoaderReady)
            {
                if (!GlobalVar.LoaderReady) GlobalVar.LoaderReady = true;
                GenInfoHandler.Instance.UpStreamState = "Connected";
                GenInfoHandler.Instance.HostReady = true;

                if (!m_FirstlogReady)
                {
                    m_FirstlogReady = true;
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Power On");
                }
            }
            else
            {
                if (GlobalVar.LoaderReady) GlobalVar.LoaderReady = false;
                GenInfoHandler.Instance.UpStreamState = "Disconnected";
                GenInfoHandler.Instance.HostReady = false;

                if (m_FirstlogReady)
                {
                    m_FirstlogReady = false;
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Power Off");
                }
            }

            if (NoSubstrate) // 11.02.08 minhan
            {
                if (!GlobalVar.NoSubstrate) GlobalVar.NoSubstrate = true;

                if (!m_FirstlogNoSub)
                {
                    m_FirstlogNoSub = true;
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "NoSubstrate is True");
                }
            }
            else
            {
                if (GlobalVar.NoSubstrate) GlobalVar.NoSubstrate = false;

                if (m_FirstlogNoSub)
                {
                    m_FirstlogNoSub = false;
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "NoSubstrate is False");
                }
            }

            //if (m_ExchangeReq) // 11.06.10 minhan
            //{
            //    //if (!GlobalVar.ExchnageReq) GlobalVar.ExchnageReq = true;

            //    if (!m_FirstlogExReq)
            //    {
            //        m_FirstlogExReq = true;
            //        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Req is True");
            //    }
            //}
            //else
            //{
            //    //if (GlobalVar.ExchnageReq) GlobalVar.ExchnageReq = false;

            //    if (m_FirstlogExReq)
            //    {
            //        m_FirstlogExReq = false;
            //        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Exchange Req is False");
            //    }
            //}

            if (m_Geninfo.CleanOut || m_Geninfo.CycleStop) // 11.06.11 minhan
            {
                if (!m_BoeLoaderinterface.mobCleanOut_Mode.GetState())
                {
                    m_BoeLoaderinterface.mobCleanOut_Mode.SetState(true);

                    if (m_Geninfo.CleanOut)
                    {
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mobCleanOut_Mode On(CleanOut Mode)");
                    }
                    else
                    {
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mobCleanOut_Mode On(Cycle Stop)");
                    }
                }
            }
            else
            {
                if (m_BoeLoaderinterface.mobCleanOut_Mode.GetState())
                {
                    m_BoeLoaderinterface.mobCleanOut_Mode.SetState(false);
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "mobCleanOut_Mode Off");
                }
            }
            buf1 = GlobalVar.LdIFStatus; // 11.02.09 minhan
            m_Geninfo.LdInterfaceStatus = buf1;
            buf2 = GlobalVar.UlIFStatus; // 11.02.09 minhan
            m_Geninfo.UlInterfaceStatus = buf2;

            return -1;

        }

        #endregion
    }

}
