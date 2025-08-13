using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum Command
    {
        //CommandButton
        Noop,
        Auto,
        Manual,
        Ready,
        CurGlassCountReset,
        CycleStart,
        CycleStop,
        Pause,
        DiStart,
        DiStop,
        Resume,
        OnlineControl,
        OnlineLocal,
        Offline,
        NormalMode,
        RecoveryMode,
        ParticleMode,
        LoopBack,
        CTPumpResultSave,
        //Device Manual Control
        SetIoState,
        CvMotorManual,
        ServoManual,
        ServoMP2300Manaul,
        PumpManual,
        AutoValveManual,
        ActuatorManual,
        ActuatorTurnManual,
        RbMotorManual,
        DBMotorManual,
        ShinkoDryCleanerManual,
        HpmjManual,
        ApPlasmaManual,
        HeaterManual,
        MfcManual,
        VirtualAlarmReset,
        UshioEuvManual,
        EyeEuvManual,
        //Data Provider Control
        RecipeAdd,
        RecipeCopy,
        RecipeRemove,
        RecipeSave,
        RecipeAddReq,       //2009.06.18 Youngsik
        RecipeCopyReq,      //2009.06.18 Youngsik
        RecipeRemoveReq,    //2009.06.18 Youngsik
        RecipeSaveReq,      //2009.06.18 Youngsik
        RecipeAddFail,      //2009.06.18 Youngsik
        RecipeCopyFail,     //2009.06.18 Youngsik
        RecipeRemoveFail,   //2009.06.18 Youngsik
        RecipeSaveFail,     //2009.06.18 Youngsik
        RecipeSelect,
        GlassDataCreate,
        GlassDataDelete,
        GlassDataEdit,
        GlassDataMove,
        GlassDataRecovery,
        GlassDataDeleteConfirm, //2009.06.23 Youngsik
        GlassDataRequest,       //2009.06.29 Youngsik
        GlassDataReport,        //2009.06.29 Youngsik
        SetupSave,
        CalibrationSave,
        // ETC...
        DualGlsSensorManual,
        GlsSensorManual,//2010.01.27 Kang
        DownStreamJobProcess,
        DownStreamJobBypass,
        ApcManual,
        ShutterManual,
        RfUnitManual,
        RfgManual,
        RfTunerManual,
        RfTuner,
        IntegralFlowReset,
        StepRun,
        ManualIFrecv,//2010.06.21 kimgun Manual 상태에서 I/F를 할 수 있게하는 버튼
        ManualIFsend,//2010.06.21 kimgun Manual 상태에서 I/F를 할 수 있게하는 버튼
        // for CIM
        CstLDRQ,        //2009.09.29 Youngsik
        CstLDCM,        //2009.09.29 Youngsik
        CstULRQ,        //2009.09.29 Youngsik
        CstULCM,        //2009.09.29 Youngsik
        CstCANCEL,      //2009.09.29 Youngsik
        CstABORT,       //2009.09.29 Youngsik
        CstSTART,       //2009.09.29 Youngsik
        PmRequest,
        ScrapRequest,
        OnlineRequest,  //2009.11.27 molnic
        OfflineRequest, //2009.11.27 molnic
        YaskawaRobotManual,
        // For JCB CIM
        OriginalPoint,      // 2009.09.15 Sangseo
        Prepare,            // 2009.09.15 Sangseo
        InitializeGlassData,// 2009.09.15 Sangseo
        BuzzerStop,         // 2009.09.15 Sangseo 
        AlarmReset,         // 2009.09.15 Sangseo
        CimMessage,         // 2009.09.15 Sangseo
        RemoteAlertMessage, // 2009.09.15 Sangseo
        AlarmSave,          // 2009.09.15 Sangseo
        AlarmHistory,       // 2009.09.15 Sangseo
        GlassApdHistory,    // 2009.09.15 Sangseo
        DateTimeReq,        // 2009.09.15 Sangseo
        OperatorLotStart,
        QTimeErrorLotStart,
        HostCommandOperatorLotStart,
        OfflineLotStart,
        LotStart,
        LotCancel,
        LotAbort,
        LotReject,         // 2010.11.5 Sangseo
        MappingOnOff, // 2010.11.5 Sangseo
        TrsModeChange,
        OpCallOn,
        OpCallOff,
        HeatExchangerManual,
        ReChuck,
        PressLog,
        GasControl,
        ForceHandshakeSendInit,
        ForceHandshakeSendComp,
        ForceHandshakeRecvInit,
        ForceHandshakeRecvComp,
        OnlineProcess,      //2010.08.26 Youngsik for LGE Solar
        OnlineTest,         //2010.08.26 Youngsik for LGE Solar
        OfflineProcess,     //2010.08.26 Youngsik for LGE Solar    
        OfflineTest,        //2010.08.26 Youngsik for LGE Solar

        D4SLManual,
        DoorLockManual,
    }

    public enum ActuatorType
    {
        Align,
        Simple,
        Chuck,
        Lift,
        OpenClose,
        UserDefinedText
    }

    public enum ActuatorAct
    {
        Noop,
        Pos,    // Fw, Cw, Open, Lock, Up
        Neg,    // Bw, Ccw, Close, Unlock, Down
        Stop,
        EStop,
        SetRefPosition,
        AlarmReset,
        Initialize
    }

    public enum ActuatorInitAct
    {
        NoAction,
        Positive,
        Negative,
    }

    public enum CvMotorAct
    {
        Noop,
        Fw,
        Bw,
        Stop,
        On,
        Off,
    }

    public enum RbMotorAct
    {
        Noop,
        Cw,
        Ccw,
        Stop,
    }

    public enum DBMotorAct
    {
        Noop,
        Cw,
        Ccw,
        Stop,
    }

    public enum PumpAct
    {
        Noop,
        Run,
        Stop,
    }

    public enum HeaterAct
    {
        Noop,
        On,
        Off,
        McOn,
        McOff,
        Reset,
        Set,
    }

    public enum AutoValveAct
    {
        Noop,
        Open,
        Close
    }

    public enum ShinkoDryCleanerAct
    {
        Noop,
        PowerOn,
        PowerOff,
        Run,
        Stop,
        EStop,
        EStopRelease
    }

    public enum HpmjAct
    {
        Noop,
        Remote,
        Local,
        PumpRun,
        PumpStop
    }

    public enum ApPlasmaAct
    {
        Noop,
        SetN2Flow,
        SetCDAFlow,
        SetVoltage,
        HouseUp,
        HouseDown,
        PowerOnOff,
        ResetN2Flow,
        ResetCDAFlow,
        ResetVoltage
    }

    public enum ServoManualAct
    {
        Noop,
        PosSend,
        PosSave,
        PosRead,
        Move,
        RepeatStart,
        RepeatStop,
        ServoOn,
        Home,
        Estop,
        EstopAll,
        JogPlusStart,
        JogPlusStop,
        JogMinusStart,
        JogMinusStop,
        ChangeVelRatio,
        ChangeRepeatWaitTime,
        ResestManualAct,
        FindHome,
        SaveFindHome
    }

    public enum DualGlsSensorType
    {
        Op,
        Oop
    }

    public enum ShutterAct
    {
        Noop,
        Open,
        Close,
        Push,
        Pull,
        Up,
        Down
    }
    public enum MFCAct
    {
        ON,
        OFF,
        SETPOINT
    }

    public enum ApcAct
    {
        Noop,
        Open,
        Close,
        Hold,
        SelectPointA,
        SelectPointB,
        SelectPointC,
        SelectPointD,
        SelectPointE,
        SetPointAPressure,
        SetPointBPressure,
        SetPointCPressure,
        SetPointDPressure,
        SetPointEPressure,
    }

    public enum UshioEuvAct
    {
        Noop,
        N2InValve,
        CdaInValve,
        PcwInValve,
        PcwOutValve,
        EuvUpdnUp,
        EuvUpdnDn,
        UDLC,
        UTRE,
        UEMG,
        UERT,
        UICR,
        ULST,
        ULON
    }

    public enum EyeEuvAct
    {//2009.11.24 KIMGUN
        Noop,
        N2InValve,
        CdaInValve,
        PcwInValve,
        PcwOutValve,
        EuvUpdnUp,
        EuvUpdnDn,
        REMOTE,
        STANBY,
        WATERFLOWOK,
        WATERLEAKOK,
        COLLINGCDAOK,
        EMOGENCYSTOP,
        RESET,
        LAMP1ON,
        LAMP2ON
    }
    public enum RfAct
    {
        Noop,
        PowerOn,
        PowerOff,
        InterlockSet,
        InterlockRelease,
        PowerSet,
        TunePresetSetting,
        LoadPresetSetting,
    }

    public enum RfTunerAct
    {
        Noop,
        TunePresetSetting,
        LoadPresetSetting,
        //AutoTunningSet,
        //AutoTunningRelease,
        //InterlockSet,
        //InterlockRelease,
    }

    public enum StepRunAct
    {
        Noop,
        Initialize,
        ChamberVent,
        InsertTray,
        SlowPumping,
        FastPumping,
        ProcessStep1,
        ProcessStep2,
        ProcessStep3,
        ProcessStep4,
        ProcessStep5,
        ChamberPurge,
        RemoveTray,
        CyclePurge,
        SeqReset,
    }

    public enum RobotSelectedMode
    {
        Unknown,
        RemoteMode,
        PlayMode,
        TeachMode,
    }

    public enum YaskawaNX100Act
    {
        Noop,
        //Remote,
        //Play,
        //Teach,
        AlignX,
        AlignY,
        VirtualMode,
        ServoOn,
        ServoOff,
        RobotHome,
        RobotHold,
        ExternalStart,
        AlarmReset,
        MotionStrobe,
    }

    public enum HeatExchangerAct
    {
        Noop,
        HeatExchangerRun,
        HeatExchangerStop,
        HeatExchangerSetTemp,
        HeatExchangerRemoteMode,
    }

    public enum D4SLAct
    {
        Noop,
        LockAll,
        UnlockAll,
        LockOne,
        UnlockOne,
    }

    public enum DoorLockAct
    {
        Noop,
        Lock,
        Unlock,
    }
}
