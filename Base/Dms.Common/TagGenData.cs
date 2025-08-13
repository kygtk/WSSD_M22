using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum IoInOutType
    {
        In,
        Out
    }

    public enum IoDataType
    {
        Digital,
        Analog,
        Mix
    }

    public enum IoType
    {
        DI,
        DO,
        AI,
        AO,
        Mix
    }

    public enum ActiveType
    {
        A,
        B
    }

    public enum ServerMode
    {
        Local,
        Remoting
    }

    public enum OpMode
    {
        Local,
        Remote
    }

    public enum InitState
    {
        Noop, Init, Fail, Comp
    }

    public enum IoUpdateMode
    {
        Polling,
        EventDriven
    }

    // Io Status Watch 할때 어떤 기반으로 돌릴것인지 정의
    public enum WatchMode
    {
        Thread,
        FormTimer,
        SystemTimer,
        ThreadingTimer
    }

    public enum DeviceNetState
    {
        Invalid = 0,
        Idle = 1,
        Reset = 2,
        Init = 3,
        Start = 4,
        Run = 5,
        Stop = 6,
        SaveConfig = 7,
        LoadConfig = 8,
        PowerFailure = 9,
        PowerGood = 10,
        Error = 11,
        Shutdown = 12,
        Suspend = 13,
        Resume = 14,
        Config = 15
    }

    public enum GlassFlowDirection
    {
        RightToLeft,
        LeftToRight
    }

    public enum UnitType
    {
        None,
        lpm,
        kPa,
        Pa,
        rpm,
        Bar,
        mmps,
        Mohm,
        mW,
        V,
        A,
        L,
        Hz,
        Celsius,
        hour,
        min,
        sec,
        msec,
        mm,
        EA,
        Percent,
        kV,
        mmpm,
        Times,
        Torr,
        SCCM,
        pH,
        ohm,
        KW,
        W,
        ccm,
        Sec,
        MB,
        Degree
    }

    public struct TagUnit
    {
        private UnitType unit;
        public UnitType Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public string Name
        {
            get
            {
                string name = "";
                switch (unit)
                {
                    case UnitType.None:
                        name = "";
                        break;
                    case UnitType.mmps:
                        name = "mm/s";
                        break;
                    case UnitType.mmpm:
                        name = "mm/min";
                        break;
                    case UnitType.Percent:
                        name = "%";
                        break;
                    case UnitType.Celsius:
                        name = "'C"; //℃는 폰트에 따라 깨짐
                        break;
                    case UnitType.Degree:
                        name = "˚";
                        break;
                    default:
                        name = unit.ToString();
                        break;
                }

                return name;
            }
        }

        public TagUnit(UnitType unit)
        {
            this.unit = unit;
        }
    }

    public enum Lamp
    {
        Off,
        On,
        Toggle
    }

    public enum Melody
    {
        Off,
        On,
    }

    public enum ActiveState
    {
        UnKnown,
        Initialized,
        Run,        // Sequence는 Run일때만 돌아야 한다.
        Stop,
        Uninitialized,
        Error
    }

    public enum UserLevels
    {
        Operator,
        Technician,
        Engineer,
        Administrator
    }

    public enum Logic
    {
        AND,
        OR
    }

    public enum Format
    {
        ASCII,
        NUMBER,
        BCD,
        HEX
    }

    public enum PortNo
    {
        COM1,
        COM2,
        COM3,
        COM4
    }

    public enum CommType
    {
        Analog,
        RS232,
        AnalogRS232
    }

    public enum BaudRate
    {
        Low = 9600,
        Middle = 12400,
        Fast = 15200
    }

    public enum DesignerMode
    {
        Design,
        OnlyView,
        OnlySetting
    }

    public enum ModelType
    {
        Atomic,
        Coupled
    }

    public enum CalibrationType
    {
        ByDesigner,
        ByDataBase
    }

    public enum StorageType
    {
        DataBase,
        Xml,
        Text
    }

    public enum HostControlMode
    {
        Offline,
        Control,
        Monitor,
    }

    public enum OperateMode
    {
        Manual,
        Auto
    }

    public enum AlarmCondition
    {
        NoAlarm,
        Alarm
    }

    public static class InitCheckState
    {
        public const string NotReady = "Not Ready";
        public const string Checking = "Checking...";
        public const string ServoHoming = "Homing...";
        public const string ServoEStop = "E-Stop";
        public const string ServoReset = "Reset";
        public const string OK = "OK";
        public const string NG = "NG";
        public const string NoUse = "No Use";
    }

    public enum ProgressAct
    {
        PROGRESS_START, PROGRESS_SET, PROGRESS_END
    }

    public enum RelationalLogic
    {
        Noop,
        Equal,          // =
        NotEqual,       // !=
        Greater,        // >
        GreaterOrEqual, // >=
        Less,           // <
        LessOrEqual     // <=
    }

    public enum PcSystemType
    {
        General,
        APC, //B&R Automation PC Series
    }

    // for MultipleMode(멀티플렉서 모드) - 1:1 or 1:N
    public enum MuxMode
    {
        Single,
        MultiDrop
    }
    public enum GridViewMode
    {
        OnlyRead,
        ReadWrite,
    }
    public enum TimeUnit
    {
        Sec = 1,
        Min = 60,
        Hour = 3600
    }
    public enum ProcessControlBy
    {
        Setup,
        Recipe
    }
    public enum ProcessCondition
    {
        Stop,
        Run,
        DontCare
    }

    public enum VarType
    {
        None,
        In,
        Out,
        Di,
        Do,
        Ai,
        Ao
    }

    public enum StructureType
    {
        NodeTerminal,
        MasterSlave,
    }

    public enum SlaveType
    {
        Servo,
        BLDC,
        Inverter,
        DI,
        DO,
        DIO,
        AI,
        AO,
        AIO,
        AP,
        Serial,
        Junction,
    }

    public enum EcSlaveItemType
    {
        Servo,
        BLDC,
        Inverter,
        DI,
        DO,
        AI,
        AO,
        AP,

        Null,
    }

    public enum PeerType
    {
        Null = 0x00,

        DIW = 0x01,             //  IO
        CDA = 0x02,             //  IO
        SmartDamper = 0x03,     //  Serial
        XrayIonizer = 0x04,     //  Serial + IO
        D40A = 0x05,            //  IO
        D4SL = 0x06,            //  IO
        TIC = 0x07,             //  Serial
        LFC = 0x08,             //  IO
        Manometer = 0x09,       //  IO
        LCT = 0x0A,           //  IO

        //  pt0bSpare = 0x0b,
        //  pt0cSpare = 0x0c,
        //  pt0dSpare = 0x0d,
        //  pt0eSpare = 0x0e,
        //  pt0fSpare = 0x0f,

        //DI = 0x11,              //  IO
        //DO = 0x12,              //  IO
        //AI = 0x13,              //  IO
    }

    public enum PairingState
    {
        Unpaired = 0,
        Pairing = 1,
        Paired = 2,
        Unpairing = 3,
    }

    public enum SmartDamperMode
    {
        Auto = 0,
        SemiAuto = 1,
        Manual = 2,
        Test = 3,

        Error = 99,
    }
}
