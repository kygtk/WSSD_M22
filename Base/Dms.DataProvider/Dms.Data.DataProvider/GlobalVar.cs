using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dms.Common;
using System.Threading;

namespace Dms.Data
{
    public class GlobalVar : BaseGlobalVar
    {
        public static bool rcvREQ;
        public static bool sndREQ;
        public static bool rcvCOMP;
        public static bool sndCOMP;
        public static bool IsRecvGlassData;
        public static bool IsRecvGlassData1;
        public static bool UlHandGlsSendCondition;
        public static bool GlassProcessed;
        public static bool TrGlsRecvReady;
        public static bool TransferErAble;
        public static bool ArGlsComplete;
        public static bool LdGlsComplete;
        public static bool sndCANCEL;
        public static bool rcvCANCEL_REQ;
        //public static bool sndEXCANCEL_REQ;
        public static bool sndCANCEL_REQ;
        //public static bool sndEXCHANGE_REQ;
        public static bool UnloadDataSet;
        public static bool UwGlsReq;
        public static bool UlGlsReq;
        public static bool TrGlsSendReady;
        public static bool UlGlsComplete;
        public static bool UlWait;
        public static bool ModeChangeComp;
        public static bool EqpStatusChange;
        public static bool ForceUnloading;
        public static bool RecvDataUnMatch;
        public static bool UlGlsExist;
        public static bool UlStatus;
        public static int PreReq;
        public static char OpMode;
        public static char ReqOpMode;
        public static char CtlMode;
        public static char IonizerStatus1;
        public static char IonizerStatus2;
        public static char IonizerStatus3;
        public static bool IsNextGlsDataRecv;
        //public static bool ExReqCancel;
        public static bool LdReqCancel;
        public static bool UlReqCancel;
        public static bool UlIng;
        public static bool RestartCancel30;
        public static bool GlsCodeReport;
        public static bool PreERStateSetting;
        public static bool NotUwReportAbnormal;
        public static bool RestartCancel60;
        public static bool LdReq;
        public static bool UlFishHandGlsSendCondition;
        public static bool GlsRecvInterlockAlarm;
        public static bool GlsSendInterlockAlarm;
        public static bool LdIng;
        public static bool WaitLdCompCancel;
        public static bool WaitUlCompCancel;
        public static bool WaitERCompCancel;
        public static bool LdInterlockRelease;
        public static bool LdRetryEcsInterface;
        public static bool UlInterlockRelease;
        public static bool UlRetryEcsInterface;
        //public static bool ExInterlockRelease;
        //public static bool ExRetryEcsInterface;
        public static bool UlReq;
        public static bool UlComp;
        public static bool ScrapUnloadCancel;
        public static bool ManualUnloadGlsDataReq;
        public static bool ManualUnloadGlsDataSend;
        public static bool ManualLoadGlsDataReq;//2009.08.24 kimgun
                                                //public static bool DesireGlsData;	//변수명이 불분명하여 변경함 : LostGlassRequest
                                                //public static int requestPositionId;//변수명이 불분명하여 변경함 : LostGlassRequestPositionId
        public static bool LostGlassRequest;
        public static int LostGlassRequestPositionId;//2009.08.25 kimgun
        //public static bool GlsExChangeInterlockAlarm;
        //public static char EqpState;
        //public static bool ExReq;
        public static bool AllGlsPosDataIng;
        public static bool LdComp;
        public static bool UlWaitComp;
        public static bool GlsApdDataReq;
        public static bool GlsApdDataComp;
        public static bool AllGlsPosDataReq;
        //public static bool ExBegin;
        //public static bool ExIng;
        public static bool OpModeChangeReq;
        public static bool OpModeChangeAccept;
        public static bool OpModeChangeComp;
        public static bool IonizerStatusChangeReq;
        public static bool CtlModeChangeReq;
        public static bool CtlModeChangeComp;
        public static bool EcsRestartOpModeChange;
        public static char CimOpMode;
        public static bool CycleStopReq;
        public static bool CycleStartReq;
        //public static bool EqpStatusChangeReq;
        public static int AlarmSetCode;
        public static bool RecipeIdChangeReport;
        public static bool RecipeAddReport;
        public static bool RecipeDeleteReport;
        public static bool RecipeCopyReport;
        public static bool GlassScrapReport;
        public static int ScrapUnitNo;
        public static bool GlassDataChangeReport;
        public static int GalssDataChangePositionId;
        public static bool GlassScrapReportCancel;
        public static bool GlassScrapUnloadCancel;
        public static bool ApdReportReq;
        public static bool ApdReportComp;
        public static bool AlarmSetReq;
        public static int AlarmCode;
        public static bool Holding;
        public static string HostReady;
        public static bool Recv1Complete;
        public static char EqpCtlMode;
        public static bool UnloadGlassDataRequest;
        public static bool HpmjBuzzerOff; // 11.01.31 minhan 
        public static bool UlHnadAlarm;//2009.08.25 kimgun
        public static bool RecipeNG; //2009.09.14 kimgun setrecipejobcond가 boo형식으로 바뀌면 필요없는 flag.그때 보고 삭제하셩.
        //public static bool LdBroken;//2009.09.21 kimgun 11.02.01 minhan 
        //public static bool EUVBroken;//2009.09.21 kimgun 11.02.01 minhan
        public static bool HpmjPumpInterlock;//2009.09.21 kimgun // 11.02.01 minhan//lkl 150929
        public static bool Powercut;//2009.09.21 kimgun
        public static bool RcvHostMsg;//2009.10.14 kimgun
        public static bool s6f67ChangeReq;//2009.09.24 kimgun
        public static double ApManualN2Set; // 09.10.08 minhan
        public static double ApManualCDASet; // 09.10.08 minhan
        public static double ApManualVolSet; // 09.10.08 minhan
        public static List<string> CurInterfaceStatus = new List<string>();//2009.10.26 kimugn
        public static List<string> CurGaugeAlarm = new List<string>(); // 10.12.21 minhan
        public static List<string> ApCurGaugeAlarm = new List<string>(); // 10.12.21 minhan
        public static List<string> CurGlassData = new List<string>(); // 10.12.21 minhan
        public static List<string> CurSendGlassData = new List<string>(); // 11.06.07 minhan
        public static string ChangedRecipeId; // 10.12.25 minhan
        public static string SourceRecipeId; // 10.12.25 minhan
        public static string RecipeBodyId; // 10.12.25 minhan
        public static bool SndRecipeReq;// 10.12.25 minhan
        public static TagRecipe ChangeRecipe; // 10.12.25 minhan

        //jemoon : 여기 부터 새로 만든것
        //public static DataReportCEID DataReporting = DataReportCEID.None;10.12.21 minhan
        //public static RecipeVariousCEID RecipeVariouReporting = RecipeVariousCEID.None; 10.12.21 minhan
        public static ushort EqpSpecificUnitNumber = 200;   //jemoon : 2010.02.05 - 200으로 고정
                                                            //public static ProductionType ProductionType; 10.12.21 minhan
        public static bool SetupChangeReportReq;

        //jemoon : melsec 사양적용으로 인해 일단 새로만듬
        //위에 변수들이랑 중복되는것이 있다면 나중에 삭제 하자
        public static bool UnloadGlassDataRequestTriggerred;
        public static bool UnloadGlassDataReport;
        public static bool UnloadGlassDataReportOn;
        public static bool LoadGlassDataRequest;
        public static bool LoadGlassDataRequestOn;
        public static bool LoadGlassDataReceived;
        //public static GlassReceiveAck LoadGlassDataReceiveAck; 10.12.21 minhan
        public static bool LoadGlassDataExistFlag;
        //public static GlassMoveStatus GlassMoveReq; 10.12.21 minhan
        //public static GlassMoveStatus GlassMoveStatusConfirm; 10.12.21 minhan
        public static bool RobotEndOn = false;
        //   public static string[][] CheckUnitCond;//2010.09.23 KIMGUN
        //kang: Glass 출발 조건 변경으로....
        public static bool LdCvStartCondition;
        public static bool HighLevelDetectInterlock;//2010.09.09 kimgun

        //2010.11.04 Kang Hpmj Interface 
        public static bool HpmjOnline;
        public static bool HpmjReady;
        public static bool HpmjPumpRunErr; // 11.01.31 minhan
        public static bool HpmjRemote;
        public static bool HpmjPPIDReq;
        public static bool HpmjInterlockParaSet;
        public static bool HpmjAlarmReset; // 11.01.31 minhan
        public static bool HpmjRunningCountSet; // 11.01.31 minhan
        public static bool HpmjRunningCountReset; // 11.01.31 minhan
        public static bool HpmjPackingCountSet; // 11.01.31 minhan
        public static bool HpmjPackingCountReset; // 11.01.31 minhan
        public static bool HpmjFilterCountSet; // 11.01.31 minhan
        public static bool HpmjFilterCountReset; // 11.01.31 minhan

        public static int ThreadEUVCount;
        public static bool rcvReport;
        public static bool rcvReportComp;
        public static bool rcvReportCancel; // 11.02.09 minhan
        public static bool sndReport;
        public static bool sndReportComp;
        public static bool sndReportCancel; // 11.02.09 minhan

        public static bool ServoHomeChecked;

        public static bool CrackInCheckReq;
        public static bool CrackInCheckOK;
        public static bool CrackInCheckNG; // 11.06.01 minhan
        public static bool CrackInCheckWarning; // 12.06.13 wang
        public static bool CrackOutCheckReq;
        public static bool CrackOutCheckOK;
        public static bool CrackOutCheckNG; // 11.06.01 minhan
        public static bool CrackOutCheckWarning; // 12.06.13 wang
        //public static bool CrackINPassReq; // 11.06.01 minhan
        //public static bool CrackOUTPassReq; // 11.06.01 minhan
        //public static bool CrackReportReq; // 11.06.01 minhan

        public static bool RbServoParaErr; // 10.05.19 minhan 
        public static bool Rb1UpServoErr; // 10.10.15 minhan
        public static bool Rb1LoServoErr; // 10.10.15 minhan
        public static bool Rb2UpServoErr; // 10.10.15 minhan
        public static bool Rb2LoServoErr; // 10.10.15 minhan
        public static bool Rb1UpServoMoveErr; // 10.10.15 minhan
        public static bool Rb1LoServoMoveErr; // 10.10.15 minhan
        public static bool Rb2UpServoMoveErr; // 10.10.15 minhan
        public static bool Rb2LoServoMoveErr; // 10.10.15 minhan

        public static string LdIFStatus;
        public static string UlIFStatus;// 11.02.09 minhan

        public static bool rcvIfResetReq; // 11.02.09 minhan
        public static bool sndIfResetReq; // 11.02.09 minhan
        public static bool TrCrackResetReq; // 11.06.01 minhan
        public static bool LoaderCrackResetReq; // 11.06.01 minhan 
        public static bool EgisAlive; // 11.02.25 minhan
        public static bool EgisVelScan; // 11.02.25 minhan
        //public static bool EgisVelScanOut; // 11.06.01 minhan
        public static bool FfuControlComErr; // 11.03.07 minhan
        public static bool PcwPressureUpAlarm; // 11.03.20 minhan
        public static bool PcwFlowUpAlarm; // 11.04.07 minhan
        public static bool MjPressureLowAlarm; // 11.04.08 minhan
        public static bool UlcvStop; // 11.06.08 minhan

        // EventMessage 
        public static bool MsgRequest; // 11.05.17 minhan
        public static bool MsgSkip;
        public static bool MsgBrokenComp;
        public static bool MsgULSkip; // 11.06.08 minhan
        public static bool MsgULBrokenComp; // 11.06.08 minhan

        //simul
        //public static bool SimulExReq; // 11.06.10 minhan

        public static string ScreenLockPassword;

        public static bool EqpRunSwrDiStop; //13.01.21 wzy//lkl 150929

        public static void InitParameter()
        {
            rcvON_IF2 = false;
            sndON_IF2 = false;
            rcvREQ = false;
            sndREQ = false;
            rcvCOMP = false;
            sndCOMP = false;
            IsRecvGlassData = false;
            IsRecvGlassData1 = false;
            TrMoveUnloadDir = false;
            TrMoveLoadDir = false;
            UlHandGlsSendCondition = false;
            GlassProcessed = false;
            LdHandAirInterlock = false;
            UlHandAirInterlock = false;
            TrGlsRecvReady = false;
            TransferErAble = false;
            ArGlsComplete = false;
            LdGlsComplete = false;
            rcvCANCEL = false;
            sndEXCANCEL = false;
            sndCANCEL = false;
            rcvCANCEL_REQ = false;
            //sndEXCANCEL_REQ = false;
            sndCANCEL_REQ = false;
            //sndEXCHANGE_REQ = false;
            UnloadDataSet = false;
            UwGlsReq = false;
            UlGlsReq = false;
            TrGlsSendReady = false;
            UlGlsComplete = false;
            UlWait = false;
            ModeChangeComp = false;
            EqpStatusChange = true;
            ForceUnloading = false;
            RecvDataUnMatch = false;
            UlGlsExist = false;
            UlStatus = false;
            PreReq = 0;
            OpMode = '0';
            CtlMode = '2';
            IonizerStatus1 = 'A';
            IonizerStatus2 = 'A';
            IonizerStatus3 = 'A';
            IsNextGlsDataRecv = false;
            //ExReqCancel = false;
            LdReqCancel = false;
            UlReqCancel = false;
            UlIng = false;
            RestartCancel30 = false;
            GlsCodeReport = false;
            PreERStateSetting = false;
            NotUwReportAbnormal = false;
            RestartCancel60 = false;
            LdReq = false;
            UlHandGlsSendCondition = false;
            GlsRecvInterlockAlarm = false;
            GlsSendInterlockAlarm = false;
            LdIng = false;
            WaitLdCompCancel = false;
            WaitUlCompCancel = false;
            WaitERCompCancel = false;
            LdInterlockRelease = false;
            LdRetryEcsInterface = false;
            UlInterlockRelease = false;
            UlRetryEcsInterface = false;
            // ExInterlockRelease = false;
            // ExRetryEcsInterface = false;
            UlReq = false;
            UlComp = false;
            ScrapUnloadCancel = false;
            ManualUnloadGlsDataReq = false;
            ManualUnloadGlsDataSend = false;
            ManualLoadGlsDataReq = false;//2009.08.24 kimgun
            //GlsExChangeInterlockAlarm = false;
            EqpState = 'A';
            //ExReq = false;
            AllGlsPosDataIng = false;
            LdComp = false;
            UlWaitComp = false;
            GlsApdDataReq = false;
            GlsApdDataComp = false;
            AllGlsPosDataReq = false;
            //ExBegin = false;
            //ExIng = false;
            OpModeChangeReq = false;
            OpModeChangeAccept = false;
            OpModeChangeComp = false;
            IonizerStatusChangeReq = false;
            CtlModeChangeReq = false;
            CtlModeChangeComp = false;
            EcsRestartOpModeChange = false;
            CimOpMode = '0';
            CycleStopReq = false;
            CycleStartReq = true;//2009.08.20 kimgun
            EqpStatusChangeReq = false;
            AlarmSetCode = 0;
            RecipeIdChangeReport = false;
            GlassScrapReport = false;
            ScrapUnitNo = 0;
            GlassScrapReportCancel = false;
            GlassScrapUnloadCancel = false;
            HostReady = "OFFLINE";
            EqpCtlMode = '2';
            UlHnadAlarm = false; //2009.08.25 kimgun
            LostGlassRequestPositionId = 0;//2009.08.25 kimgun
            RecipeNG = false;//2009.09.14 kimgun
            //LdBroken = false;//2009.09.21 kimgun 11.02.01 minhan
            //EUVBroken = false;//2009.09.21 kimgun 11.02.01 minhan
            //HpmjPumpInterlock = false;//2009.09.21 kimgun 11.02.01 minhan
            Powercut = false;//2009.09.21 kimgun
            RcvHostMsg = false; //2009.10.14 kimgun
            s6f67ChangeReq = false;//2009.09.24 kimgun
            ApManualN2Set = 0; // 09.10.08 minhan
            ApManualCDASet = 0; // 09.10.08 minhan
            ApManualVolSet = 0; // 09.10.08 minhan
            ChangedRecipeId = ""; // 10.12.25 minhan
            SourceRecipeId = ""; // 10.12.25 minhan 
            RecipeBodyId = "";
            SndRecipeReq = false;
            //InterfaceHistoryClear = false;//091020 LeeChungWon
            CurInterfaceStatus.Add("No Interface Processing"); //2009.10.26 kimgun//091015 LeeChungWon 
            SetupChangeReportReq = false;

            DailyDiTotalUse = GeneralData.GetIntValue("GlobalVar", DailyDiTotalUseKey, 0);
            DailyGlassProcessCount = GeneralData.GetIntValue("GlobalVar", DailyGlassProcessCountKey, 0);
            SavedTime = GeneralData.GetStringValue("GlobalVar", DailyGlassSavedTimeKey, "0");

            LdCvStartCondition = false; //2010.07.23
            HighLevelDetectInterlock = false;//2010.09.09 kimgun

            //2010.09.23 kimgun
            //CheckUnitCond = new string[eqpTransferUnits._Items.Count][];
            //foreach (TransferUnit device in eqpTransferUnits._Items)
            //{
            //    CheckUnitCond[device.Id] = new string[(int)ThreadCvBOE_G8_DHDC.CheckCond.CheckCondCnt];
            //    for(int i = 0; i<(int)ThreadCvBOE_G8_DHDC.CheckCond.CheckCondCnt; i++)
            //    {
            //        CheckUnitCond[device.Id][i] = null;
            //    }
            //}

            LoaderReady = false;
            NoSubstrate = false; // 11.02.08 minhan 
            //ExchnageReq = false; // 11.06.10 minhan
            HpmjOnline = false;
            HpmjReady = false;
            HpmjPumpRunErr = false;
            HpmjRemote = false;
            HpmjPPIDReq = false;
            HpmjAlarmReset = false;
            HpmjInterlockParaSet = false;
            HpmjRunningCountSet = false; // 11.01.31 minhan
            HpmjRunningCountReset = false; // 11.01.31 minhan
            HpmjPackingCountSet = false; // 11.01.31 minhan
            HpmjPackingCountReset = false; // 11.01.31 minhan
            HpmjFilterCountSet = false; // 11.01.31 minhan
            HpmjFilterCountReset = false; // 11.01.31 minhan
            ThreadEUVCount = 0;

            rcvReport = false;
            sndReport = false;
            rcvReportComp = false;
            sndReportComp = false;
            rcvReportCancel = false; // 11.02.09 minhan
            sndReportCancel = false;

            ServoHomeChecked = false;

            CrackInCheckReq = false;
            CrackInCheckOK = false;
            CrackInCheckNG = false; // 11.06.01 minhan
            CrackInCheckWarning = false; // 12.06.13 wang
            CrackOutCheckReq = false;
            CrackOutCheckOK = false;
            CrackOutCheckNG = false; // 11.06.01 minhan
            CrackOutCheckWarning = false; // 12.06.13 wang
            //CrackINPassReq = false; // 11.06.01 minhan
            //CrackOUTPassReq = false; // 11.06.01 minhan
            //CrackReportReq = false; // 11.06.01 minhan

            RbServoParaErr = false; // 10.05.19 minhan
            Rb1UpServoErr = false; // 10.10.15 minhan
            Rb1LoServoErr = false; // 10.10.15 minhan
            Rb2UpServoErr = false; // 10.10.15 minhan
            Rb2LoServoErr = false; // 10.10.15 minhan
            Rb1UpServoMoveErr = false; // 10.10.15 minhan
            Rb1LoServoMoveErr = false; // 10.10.15 minhan
            Rb2UpServoMoveErr = false; // 10.10.15 minhan
            Rb2LoServoMoveErr = false; // 10.10.15 minhana

            rcvIfResetReq = false; // 11.02.09 minhan
            sndIfResetReq = false;
            TrCrackResetReq = false; // 11.06.01 minhan
            LoaderCrackResetReq = false;
            EgisAlive = false; // 11.02.25 minhan
            EgisVelScan = false; // 11.02.25 minhan
            //EgisVelScanOut = false; // 11.06.01 minhan
            FfuControlComErr = false; // 11.03.07 minhan
            PcwPressureUpAlarm = false; // 11.03.20 minhan
            PcwFlowUpAlarm = false; // 11.04.07 minhan
            MjPressureLowAlarm = false; // 11.04.08 minhan
            UlcvStop = false; // 11.06.08 minhan

            HpmjBuzzerOff = false;

            MsgRequest = false; // 11.05.17 minhan
            MsgSkip = false;
            MsgBrokenComp = false;
            MsgULSkip = false; // 11.06.08 minhan
            MsgULBrokenComp = false;

            //SimulExReq = false; // 11.06.10 minhan

            ScreenLockPassword = ""; //12.01.16

            LdIFStatus = "--"; // 11.02.09 minhan
            UlIFStatus = "--";
        }
    }
}
