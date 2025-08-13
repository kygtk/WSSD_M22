using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Device;
using Dms.Sequence;
using System.Threading;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadHandler
    {
        private List<XSequence> m_Thread = new List<XSequence>();
        public ThreadSystemControl SystemControl; // 11.02.23 minhan
        public ThreadCvBOE_G8_DHDC CvControl;
        public ThreadTr_BOE_G8_DHDC TrControl;
        //public ThreadLd_Hand_BOE_G8_DHDC LdHandControl;
        public ThreadUl_Hand_BOE_G8_DHDC UlHandControl;
        public SeqCimInterface_BOE_DOHC CimControl;
        public ThreadIntefaceControl InterfaceControl;
        public ThreadHpmjIntefaceControl HpmjControl;
        public ThreadMain MainControl;
        public ThreadSeqTowerLamp_BOE_G8_DHDC TowerLampControl;
        public ThreadSeqIonizer_BOE_G8_DHDC IonizerControl; // 11.03.25 minhan
        //public ThreadForcedExhaust_P8E ForceExhaustControl; // 10.12.21 minhan
        //public ThreadSeApBOE_G8_DHDC ApControl;
        public ThreadUshioEuv_BOE_G8_DHDC EuvControl;
        public ThreadEmoInterlock EmoControl;
        //public ThreadDoorInterlockLTPS_G4_5_DHDC DoorControl;
        public ThreadDoorInterlock DoorControl;
        public ThreadGaugeBOE_G8_DHDC GaugeControl;//2009.09.30 kimgun
        public ThreadHepaInterlockBOE_G8_DHDC HepaControl; // 10.12.21 minhan
        public ThreadRollBrush_BOE_G8_DHDC RbControl; //2010.06.23 Taegoo
        public ThreadPumpControl_BOE_G8_DHDC pumpControl;//2010.06.29 kimgun
        //public SeqSharedMem seqShareMem;//2010.07.20 kimgun;
        public ThreadTankControl_BOE_G8_DHDC SeqTank;//2010.09.09 kimgun
        //public ThreadLeakInterlock_P8E seqLeak;//2010.09.09 kimgun
        public ThreadFfuControl FfuControl; // 11.03.07 minhan
        public SeqCrackEqpInterface_BOE_DHDC SeqCrack; //kang
        public EventmessageProc EventMsgControl; // 11.05.17 minhan
                                                 //public ThreadTraceReport_BOE_G8_DHDC SeqTraceReport;

        public ThreadHandler()
        {
        }

        public void Initialize()
        {
            ServerManager serverManager = ServerManager.Instance;
            IComponentContainer components = DmsComponents.Instance.ComponentContainer;

            // Message Control
            EventMsgControl = new EventmessageProc(10, serverManager);      //  25.08.06 bm
            AddThread(EventMsgControl);

            // System Control Sequence
            SystemControl = new ThreadSystemControl(1000, serverManager);   //  25.08.07 bm
            AddThread(SystemControl);

            // Cim Sequence
            CimControl = new SeqCimInterface_BOE_DOHC(20, serverManager);
            AddThread(CimControl);

            // Main Sequence
            MainControl = new ThreadMain(20, serverManager);                //  25.08.07 bm
            AddThread(MainControl);

            //Buzzer Control Sequence
            AddThread(new ThreadBuzzerControl(50, serverManager));          //  25.08.07 bm

            //Alarm Reset Switch Sequecne
            AddThread(new ThreadAlarmResetSwitch(50, serverManager));

            //// SignalTower
            TowerLampControl = new ThreadSeqTowerLamp_BOE_G8_DHDC(10, serverManager);
            AddThread(TowerLampControl);

            //// Ionizer Sequence
            IonizerControl = new ThreadSeqIonizer_BOE_G8_DHDC(100, serverManager); // 11.03.25 minhan
            AddThread(IonizerControl);

            //// Cv Sequence
            CvControl = new ThreadCvBOE_G8_DHDC(10, serverManager);
            AddThread(CvControl);

            //// Tr Sequence
            TrControl = new ThreadTr_BOE_G8_DHDC(20, serverManager);
            AddThread(TrControl);

            //// LD Hand Sequence
            //LdHandControl = new ThreadLd_Hand_BOE_G8_DHDC(20, serverManager);
            //AddThread(LdHandControl);

            //// UL Hand Sequence
            UlHandControl = new ThreadUl_Hand_BOE_G8_DHDC(20, serverManager);
            AddThread(UlHandControl);

            //// Interface Sequence
            InterfaceControl = new ThreadIntefaceControl(20, serverManager);
            AddThread(InterfaceControl);

            //// Emo Interlock Sequence
            EmoControl = new ThreadEmoInterlock(100, serverManager);        //  25.08.12 bm
            AddThread(EmoControl);

            //// Door Interlock Sequence
            DoorControl = new ThreadDoorInterlock(100, serverManager);
            AddThread(DoorControl);

            //// Cover Interlock Sequence
            AddThread(new ThreadCoverInterlock(100, serverManager));

            //// Leak Sensor
            // seqLeak = new ThreadLeakInterlock_P8E(100, serverManager);
            // AddThread(seqLeak);
            AddThread(new ThreadLeakInterlock(100, serverManager));

            // FFU Control
            FfuControl = new ThreadFfuControl(100, serverManager); // 11.03.07 minhan
            AddThread(FfuControl);

            // HEPA
            HepaControl = new ThreadHepaInterlockBOE_G8_DHDC(100, serverManager);
            AddThread(HepaControl);

            //// Forced Exhaust
            ////AddThread(new ThreadForcedExhaust(100, serverManager));
            //ForceExhaustControl = new ThreadForcedExhaust_P8E(100, serverManager);
            //AddThread(ForceExhaustControl);

            //// Gauge Interlock
            GaugeControl = new ThreadGaugeBOE_G8_DHDC(100, serverManager, ProcessScenario.GetGaugeInterlockCheckCondition);//2009.09.30 kimgun
            AddThread(GaugeControl);

            //// Parts Life time
            AddThread(new ThreadPartsItems(100, serverManager, serverManager.PartsItemsHandler, ProcessScenario.GetPartsCheckCondition));// 11.06.16 minhan

            //// Tank Sequence
            SeqTank = new ThreadTankControl_BOE_G8_DHDC(10, serverManager);
            AddThread(SeqTank);
            ////AddThread(new ThreadTankControl(10, serverManager));

            //// Pump Sequence
            //AddThread(new ThreadPumpControl(10, serverManager));
            pumpControl = new ThreadPumpControl_BOE_G8_DHDC(10, serverManager);
            AddThread(pumpControl);

            ////Hpmj Sequence
            HpmjControl = new ThreadHpmjIntefaceControl(50, serverManager);
            AddThread(HpmjControl);

            //// Process Unit Sequence
            AddThread(new ThreadProcessUnitLTPS_G8_DHDC(10, serverManager));

            //// Roll Brush Sequence
            RbControl = new ThreadRollBrush_BOE_G8_DHDC(10, serverManager);
            AddThread(RbControl);

            SeqCrack = new SeqCrackEqpInterface_BOE_DHDC(10, serverManager);
            AddThread(SeqCrack);

            // Ap Sequence
            //ApControl = new ThreadSeApBOE_G8_DHDC(10, serverManager);
            //AddThread(ApControl);
            // Euv Sequence
            EuvControl = new ThreadUshioEuv_BOE_G8_DHDC(100, serverManager);
            AddThread(EuvControl);

            //SeqTraceReport = new ThreadTraceReport_BOE_G8_DHDC(100, serverManager);
            //AddThread(SeqTraceReport);

            //_Plasma.SetInterlockScenario(ProcessScenario.IsApInterlock); // 11.03.26 minhan

            //// TPD Sequence
            //TpdControl = new ThreadTpd_LTPS_G4_5_DHDC(10, serverManager);
            //AddThread(TpdControl);
            ////SharedMemory
            //seqShareMem = new SeqSharedMem(10, serverManager);//2010.07.20 kimgun   
            //AddThread(seqShareMem);

            //// Servo Unit Interlock Scenario
            ServoUnit.SetInterlockScenario(ProcessScenario.IsServoUnitInterlock);
        }

        private void AddThread(XSequence thread)
        {
            if (thread.SeqFunctionCount > 0)
            {
                m_Thread.Add(thread);
            }
        }

        public void Start()
        {
            foreach (XSequence thread in m_Thread)
            {
                if (thread.SeqFunctionCount > 0)
                {
                    thread.Start();
                }
            }
        }

        public void Pause()
        {
            foreach (XSequence thread in m_Thread)
            {
                thread.Pause();
            }

            Thread.Sleep(1000);
        }
    }
}
