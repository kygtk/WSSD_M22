using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dms.Common;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        #region Enums
        #endregion

        public class Peer
        {
            #region Fields
            protected PeerType m_PeerType;

            protected int m_Id;
            protected Slave_AP m_AP;
            protected int m_Channel;

            protected PairingState m_PairingState;
            #endregion

            #region Properties
            public PeerType PeerType
            {
                get { return m_PeerType; }
            }

            public int Id
            {
                get { return m_Id; }
            }

            public int Channel
            {
                get { return m_Channel; }
            }

            public PairingState PairingState
            {
                get { return m_PairingState; }
                set { m_PairingState = value; }
            }

            public bool IsPaired
            {
                get { return m_PairingState == PairingState.Paired; }
            }

            public string Code
            {
                get
                {
                    string st = "x";
                    st += ((int)m_PeerType).ToString("X2");
                    st += m_Id.ToString("X2");
                    return st;
                }
            }
            #endregion

            #region Constructor
            public Peer(Slave_AP Ap, int Channel)
            {
                m_AP = Ap;
                m_Channel = Channel;

                m_PairingState = PairingState.Unpaired;
            }
            #endregion

            #region Virtuals
            public virtual int MaxPage_IoRead { get { return 0; } }
            public virtual int MaxPage_IoWrite { get { return 0; } }
            public virtual int MaxPage_Serial { get { return 0; } }

            public virtual void ReadIoData(ushort IoData)
            {

            }

            public virtual ushort WriteIoData()
            {
                return 0x0000;
            }

            public virtual void ReadSerialData(ushort[] SerialData)
            {

            }

            public virtual ushort[] WriteSerialData(int Page)
            {
                return new ushort[7] { 0, 0, 0, 0, 0, 0, 0 };
            }
            #endregion
        }

        public class Peer_DIW : Peer
        {
            #region Fields
            private int m_LastReadPage;

            private ushort m_R1_FlowData;
            private ushort m_R2_PressData;
            #endregion

            #region Properties
            public short FlowValue
            { get { return (short)m_R1_FlowData; } }

            public short PressValue
            { get { return (short)m_R2_PressData; } }
            #endregion

            #region Constructor
            public Peer_DIW(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.DIW;
                m_Id = Id;
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 2; } }

            public override void ReadIoData(ushort IoData)
            {
                bool isInData = (IoData & (0x01 << 15)) == 0;
                if (!isInData) return;

                ushort page = (ushort)((IoData & 0x7000) >> 12);
                ushort data = (ushort)((IoData & 0x0FFF) >> 00);

                switch (page)
                {
                    case 1:
                        m_R1_FlowData = data;
                        break;
                    case 2:
                        m_R2_PressData = data;
                        break;
                }
            }

            public override ushort WriteIoData()
            {
                ushort IoData = 0x00;

                int page = m_LastReadPage + 1;
                if (page > MaxPage_IoRead) page = 1;

                IoData |= (ushort)(page << 12);
                m_LastReadPage = page;

                return IoData;
            }
            #endregion
        }

        public class Peer_CDA : Peer
        {
            #region Fields
            private int m_LastReadPage;

            private ushort m_R1_FlowData;
            private ushort m_R2_PressData;
            #endregion

            #region Properties
            public short FlowValue
            { get { return (short)m_R1_FlowData; } }

            public short PressValue
            { get { return (short)m_R2_PressData; } }
            #endregion

            #region Constructor
            public Peer_CDA(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.CDA;
                m_Id = Id;
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 2; } }

            public override void ReadIoData(ushort IoData)
            {
                bool isInData = (IoData & (0x01 << 15)) == 0;
                if (!isInData) return;

                ushort page = (ushort)((IoData & 0x7000) >> 12);
                ushort data = (ushort)((IoData & 0x0FFF) >> 00);

                switch (page)
                {
                    case 1:
                        m_R1_FlowData = data;
                        break;
                    case 2:
                        m_R2_PressData = data;
                        break;
                }
            }

            public override ushort WriteIoData()
            {
                ushort IoData = 0x00;

                int page = m_LastReadPage + 1;
                if (page > MaxPage_IoRead) page = 1;

                IoData |= (ushort)(page << 12);
                m_LastReadPage = page;

                return IoData;
            }
            #endregion
        }

        public class Peer_SmartDamper : Peer
        {
            #region Fields
            private SmartDamperMode m_W1_InitMode;
            private ushort m_W2_TargetPressureValue;
            private ushort m_W3_TargetPressureHysterisys;
            private ushort m_W4_TargetValveAngle;

            private SmartDamperMode m_R1_CurrentMode;
            private ushort m_R2_TargetPressureValue;
            private ushort m_R3_TargetPressureHysteresis;
            private ushort m_R4_CurrentValveAngle;
            private ushort m_R5_CurrentPressureRate;
            private ushort m_R6_ServoAlarmCode;
            #endregion

            #region Properties
            public SmartDamperMode PV_CurrentMode
            {
                get { return m_R1_CurrentMode; }
            }
            public ushort PV_TargetPressure
            {
                get { return m_R2_TargetPressureValue; }
            }
            public ushort PV_TargetPressureHysteresis
            {
                get { return m_R3_TargetPressureHysteresis; }
            }
            public ushort PV_CurrentValveAngle
            {
                get { return m_R4_CurrentValveAngle; }
            }
            public ushort PV_CurrentPressure
            {
                get { return m_R5_CurrentPressureRate; }
            }
            public ushort PV_AlarmCode
            {
                get { return m_R6_ServoAlarmCode; }
            }

            public SmartDamperMode SV_SmartDamperMode
            {
                set { m_W1_InitMode = value; }
            }
            public ushort SV_TargetPressure
            {
                set { m_W2_TargetPressureValue = value; }
            }
            public ushort SV_TargetPressureHysteresis
            {
                set { m_W3_TargetPressureHysterisys = value; }
            }
            public ushort SV_TargetValveAngle
            {
                set { m_W4_TargetValveAngle = value; }
            }
            #endregion

            #region Constructor
            public Peer_SmartDamper(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.SmartDamper;
                m_Id = Id;
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_Serial { get { return 1; } }

            public override ushort[] WriteSerialData(int Page = 1)
            {
                ushort[] SerialData = new ushort[7];

                SerialData[0] = (ushort)m_W1_InitMode;
                SerialData[1] = m_W2_TargetPressureValue;
                SerialData[2] = m_W3_TargetPressureHysterisys;
                SerialData[3] = m_W4_TargetValveAngle;
                SerialData[4] = 0;
                SerialData[5] = 0;
                SerialData[6] = 0;

                return SerialData;
            }
            #endregion
        }

        public class Peer_XrayIonizer : Peer
        {
            #region Fields
            private int m_DeviceCount;

            private bool[] m_R0_0_Mode;
            private bool[] m_R0_1_Interlock;
            private bool[] m_R0_2_Power;
            private bool[] m_R0_3_OverTime;
            private bool[] m_R0_4_Alarm;

            private uint[][] m_RS_TubeLifeTime;

            private bool[] m_W0_0_Mode;
            private bool[] m_W0_1_Interlock;

            private bool flag_Write;
            #endregion

            #region Properties
            #endregion

            #region Constructor
            public Peer_XrayIonizer(Slave_AP Ap, int Channel, int Id, int DeviceCount) : base(Ap, Channel)
            {
                m_PeerType = PeerType.XrayIonizer;
                m_Id = Id;

                m_DeviceCount = DeviceCount;

                InitializeArray();
            }
            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_R0_0_Mode = new bool[3];
                m_R0_1_Interlock = new bool[3];
                m_R0_2_Power = new bool[3];
                m_R0_3_OverTime = new bool[3];
                m_R0_4_Alarm = new bool[3];

                m_RS_TubeLifeTime = new uint[3][];
                m_RS_TubeLifeTime[0] = new uint[m_DeviceCount];
                m_RS_TubeLifeTime[1] = new uint[m_DeviceCount];
                m_RS_TubeLifeTime[2] = new uint[m_DeviceCount];

                m_W0_0_Mode = new bool[3];
                m_W0_1_Interlock = new bool[3];
            }

            private ushort SetIoData_Write()
            {
                ushort IoData = 0x00;

                IoData |= 0x8000;       //  0b 1000 0000 0000 0000
                IoData |= 1 << 12;      //  Page 1 고정

                if (m_W0_0_Mode[0]) IoData |= 1 << 0;
                if (m_W0_0_Mode[1]) IoData |= 1 << 2;
                if (m_W0_0_Mode[2]) IoData |= 1 << 4;

                if (m_W0_1_Interlock[0]) IoData |= 1 << 1;
                if (m_W0_1_Interlock[1]) IoData |= 1 << 3;
                if (m_W0_1_Interlock[2]) IoData |= 1 << 5;

                return IoData;
            }

            private ushort SetIoData_Read()
            {
                ushort IoData = 0x00;

                IoData |= 1 << 12;      //  Page 1 고정

                return IoData;
            }
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 1; } }
            public override int MaxPage_IoWrite { get { return 1; } }
            public override int MaxPage_Serial { get { return m_DeviceCount; } }

            public override void ReadIoData(ushort IoData)
            {
                bool isInData = (IoData & (0x01 << 15)) == 0;

                if (isInData)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        bool mode = (IoData & 0x0001 << (i * 5)) != 0;
                        bool interlock = (IoData & 0x0002 << (i * 5)) != 0;
                        bool power = (IoData & 0x0004 << (i * 5)) != 0;
                        bool overtime = (IoData & 0x0008 << (i * 5)) != 0;
                        bool alarm = (IoData & 0x0010 << (i * 5)) != 0;

                        m_R0_0_Mode[i] = mode;
                        m_R0_1_Interlock[i] = interlock;
                        m_R0_2_Power[i] = power;
                        m_R0_3_OverTime[i] = overtime;
                        m_R0_4_Alarm[i] = alarm;
                    }
                }
                else
                {
                    if (flag_Write)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            bool mode = (IoData & 0x0001 << (i * 2)) != 0;
                            bool interlock = (IoData & 0x0002 << (i * 2)) != 0;

                            m_W0_0_Mode[0] = mode;
                            m_W0_1_Interlock[0] = interlock;
                        }

                        flag_Write = false;
                    }
                }
            }

            public override ushort WriteIoData()
            {
                if (flag_Write)
                    return SetIoData_Write();
                else
                    return SetIoData_Read();
            }
            #endregion
        }

        public class Peer_D40A : Peer
        {
            #region Fields
            private ushort m_R0_SensorData;
            #endregion

            #region Properties
            public ushort SensorData
            {
                get { return m_R0_SensorData; }
            }
            #endregion

            #region Constructor
            public Peer_D40A(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.D40A;
                m_Id = Id;
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 1; } }

            public override void ReadIoData(ushort IoData)
            {
                m_R0_SensorData = IoData;
            }
            #endregion
        }

        public class Peer_D4SL : Peer
        {
            #region Fields
            private bool[] m_R0_0_Opened;
            private bool[] m_R0_1_Locked;

            private bool[] m_W0_0_Lock;
            private bool m_W0_1_AllLock;
            #endregion

            #region Properties
            public bool[] IsOpened
            {
                get { return m_R0_0_Opened; }
            }
            public bool[] IsLocked
            {
                get { return m_R0_1_Locked; }
            }

            public bool[] Lock
            {   //  Array 형태이므로, Get만 가능하게 해도 값 수정이 가능함
                get { return m_W0_0_Lock; }
            }
            public bool LockAll
            {
                set { m_W0_1_AllLock = value; }
            }
            #endregion

            #region Constructor
            public Peer_D4SL(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.D4SL;
                m_Id = Id;

                InitializeArray();
            }
            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_R0_0_Opened = new bool[8];
                m_R0_1_Locked = new bool[8];

                m_W0_0_Lock = new bool[8];
            }
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 1; } }
            public override int MaxPage_IoWrite { get { return 1; } }

            public override void ReadIoData(ushort IoData)
            {
                for (int i = 0; i < 8; i++)
                {
                    bool opened = (IoData & (0x0001 << (i * 2))) != 0;
                    bool locked = (IoData & (0x0002 << (i * 2))) != 0;

                    m_R0_0_Opened[i] = opened;
                    m_R0_1_Locked[i] = locked;
                }
            }

            public override ushort WriteIoData()
            {
                ushort IoData = 0x00;

                for (int i = 0; i < 8; i++)
                    if (m_W0_0_Lock[i])
                        IoData |= (ushort)(0x01 << i);

                if (m_W0_1_AllLock)
                    IoData |= 0x01 << 8;

                return IoData;
            }
            #endregion
        }

        public class Peer_TIC : Peer
        {
            #region Structs
            public struct TIC_DataSet
            {
                public ushort R1_CurrentValue;
                public ushort R2_TargetValue;
                public ushort R3_PID_P;
                public ushort R4_PID_I;
                public ushort R5_PID_D;
                public ushort R6_AlValue2;

                public ushort W1_TargetValue;
                public ushort W2_AlValue1;
                public ushort W3_PID_P;
                public ushort W4_PID_I;
                public ushort W5_PID_D;
                public ushort W6_AlValue2;
            }
            #endregion

            #region Fields
            private int m_DeviceCount;
            private TIC_DataSet[] m_TICs;
            #endregion

            #region Properties
            public Peer_TIC(Slave_AP Ap, int Channel, int Id, int DeviceCount) : base(Ap, Channel)
            {
                m_PeerType = PeerType.TIC;
                m_Id = Id;

                m_DeviceCount = DeviceCount;

                InitializeArray();
            }
            #endregion

            #region Constructor
            private void InitializeArray()
            {
                m_TICs = new TIC_DataSet[m_DeviceCount];
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_Serial { get { return m_DeviceCount; } }

            public override ushort[] WriteSerialData(int Page)
            {
                int idxTIC = Page - 1;

                ushort[] SerialData = new ushort[7];

                SerialData[0] = m_TICs[idxTIC].W1_TargetValue;
                SerialData[1] = m_TICs[idxTIC].W2_AlValue1;
                SerialData[2] = m_TICs[idxTIC].W3_PID_P;
                SerialData[3] = m_TICs[idxTIC].W4_PID_I;
                SerialData[4] = m_TICs[idxTIC].W5_PID_D;
                SerialData[5] = m_TICs[idxTIC].W6_AlValue2;
                SerialData[6] = 0;

                return SerialData;
            }
            #endregion
        }

        public class Peer_LFC : Peer
        {
            #region Fields
            private int m_LastReadPage;

            private ushort m_R1_FlowData;
            private ushort m_R2_PressData;
            private ushort m_R3_TargetOpenRate;
            private ushort m_R4_CurrentOpenRate;

            private ushort m_W3_TargetOpenRate;

            private bool flag_Write;
            #endregion

            #region Properties
            public ushort PV_FlowValue
            {
                get { return m_R1_FlowData; }
            }
            public ushort PV_PressValue
            {
                get { return m_R2_PressData; }
            }
            public ushort PV_CurrentOpenRate
            {
                get { return m_R4_CurrentOpenRate; }
            }

            public ushort SV_TargetOpenRate
            {
                set { m_W3_TargetOpenRate = value; }
            }
            #endregion

            #region Constructor
            public Peer_LFC(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.LFC;
                m_Id = Id;
            }
            #endregion

            #region Methods

            private ushort SetIoData_Write()
            {
                ushort IoData = 0x00;

                IoData |= 0x8000;       //  0b 1000 0000 0000 0000
                IoData |= 3 << 12;      //  Page 3 고정

                IoData |= m_W3_TargetOpenRate;

                return IoData;
            }

            private ushort SetIoData_Read()
            {
                ushort IoData = 0x00;

                int page = m_LastReadPage + 1;
                if (page > MaxPage_IoRead) page = 1;

                IoData |= (ushort)(page << 12);
                m_LastReadPage = page;

                return IoData;
            }
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 4; } }
            public override int MaxPage_IoWrite { get { return 1; } }

            public override void ReadIoData(ushort IoData)
            {
                bool isInData = (IoData & (0x01 << 15)) == 0;
                ushort page = (ushort)((IoData & 0x7000) >> 12);
                ushort data = (ushort)((IoData & 0x0FFF) >> 00);

                if (isInData)
                {
                    switch (page)
                    {
                        case 1:
                            m_R1_FlowData = data;
                            break;
                        case 2:
                            m_R2_PressData = data;
                            break;
                        case 3:
                            m_R3_TargetOpenRate = data;
                            break;
                        case 4:
                            m_R4_CurrentOpenRate = data;
                            break;
                    }
                }
                else
                {
                    if (flag_Write && page == 3)
                    {
                        m_W3_TargetOpenRate = data;

                        flag_Write = false;
                    }
                }
            }

            public override ushort WriteIoData()
            {
                if (flag_Write)
                    return SetIoData_Write();
                else
                    return SetIoData_Read();
            }
            #endregion
        }

        public class Peer_Manometer : Peer
        {
            #region Fields
            private ushort m_R1_ExhaustData;
            #endregion

            #region Properties
            public short ExhaustValue
            {
                get { return (short)m_R1_ExhaustData; }
            }
            #endregion

            #region Constructor
            public Peer_Manometer(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.Manometer;
                m_Id = Id;
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 1; } }

            public override void ReadIoData(ushort IoData)
            {
                bool isInData = (IoData & (0x01 << 15)) == 0;
                if (!isInData) return;

                ushort page = (ushort)((IoData & 0x7000) >> 12);
                ushort data = (ushort)((IoData & 0x0FFF) >> 00);

                switch (page)
                {
                    case 1:
                        m_R1_ExhaustData = data;
                        break;
                }
            }

            public override ushort WriteIoData()
            {
                ushort IoData = 0x00;

                IoData |= 1 << 12;      //  Page 1 고정

                return IoData;
            }
            #endregion
        }

        public class Peer_LCT : Peer
        {
            #region Fields
            private int m_LastReadPage;

            private ushort m_R1_LevelData1;
            private ushort m_R2_LevelData2;
            private ushort m_R3_Consistance;
            #endregion

            #region Properties
            public ushort PV_Level1Value
            {
                get { return m_R1_LevelData1; }
            }
            public ushort PV_Level2Value
            {
                get { return m_R2_LevelData2; }
            }
            public ushort PV_ConsistanceValue
            {
                get { return m_R3_Consistance; }
            }
            #endregion

            #region Constructor
            public Peer_LCT(Slave_AP Ap, int Channel, int Id) : base(Ap, Channel)
            {
                m_PeerType = PeerType.LCT;
                m_Id = Id;
            }
            #endregion

            #region Methods
            #endregion

            #region Overrides
            public override int MaxPage_IoRead { get { return 3; } }

            public override void ReadIoData(ushort IoData)
            {
                bool isInData = (IoData & (0x01 << 15)) == 0;
                if (!isInData) return;

                ushort page = (ushort)((IoData & 0x7000) >> 12);
                ushort data = (ushort)((IoData & 0x0FFF) >> 00);

                switch (page)
                {
                    case 1:
                        m_R1_LevelData1 = data;
                        break;
                    case 2:
                        m_R2_LevelData2 = data;
                        break;
                    case 3:
                        m_R3_Consistance = data;
                        break;
                }
            }

            public override ushort WriteIoData()
            {
                ushort IoData = 0x00;

                int page = m_LastReadPage + 1;
                if (page > MaxPage_IoRead) page = 1;

                IoData |= (ushort)(page << 12);
                m_LastReadPage = page;

                return IoData;
            }
            #endregion
        }

        /* Sample
         * 
            #region Fields
            #endregion

            #region Properties
            #endregion

            #region Constructor
            #endregion

            #region Methods
            #endregion

            #region Overrides
            #endregion
         *
         */
    }
}
