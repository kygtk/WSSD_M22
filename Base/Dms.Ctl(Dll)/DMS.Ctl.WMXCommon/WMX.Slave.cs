using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        #region Enums
        public enum SlaveType
        {
            Null = 0,
            Junction = 1,

            Servo = 10,
            BLDC = 11,
            Inverter = 12,

            DIO = 20,
            AIO = 21,

            AP = 30,

            StoE = 40,

            ESD = 50,
        }
        #endregion

        public class Slave
        {
            #region Fields
            protected int m_SlaveNo;
            protected int m_AliasNo;
            protected int m_VendorId;
            protected string m_VendorName;
            protected int m_ProductCode;
            protected string m_ProductName;
            protected SlaveType m_SlaveType;
            protected EcStateMachine m_SlaveState;

            protected readonly byte[] m_InvalidFrameError = new byte[4];
            protected readonly byte[] m_RxError = new byte[4];
            protected readonly byte[] m_ForwardedRxError = new byte[4];
            protected byte m_ProcessingUnitError = 0;
            protected readonly byte[] m_LostLinkError = new byte[4];
            #endregion

            #region Properties
            [Category("Slave Info")]
            [DisplayName("Slave No")]
            public int SlaveNo
            {
                get { return this.m_SlaveNo; }
            }

            [Category("Slave Info")]
            [DisplayName("Alias No")]
            public int AliasNo
            {
                get { return this.m_AliasNo; }
            }

            [Category("Slave Info")]
            [DisplayName("Vendor ID")]
            public int VendorId
            {
                get { return this.m_VendorId; }
            }

            [Category("Slave Info")]
            [DisplayName("Vendor Name")]
            public string VendorName
            {
                get { return this.m_VendorName; }
            }

            [Category("Slave Info")]
            [DisplayName("Product Code")]
            public int ProductCode
            {
                get { return this.m_ProductCode; }
            }

            [Category("Slave Info")]
            [DisplayName("Product Name")]
            public string ProductName
            {
                get { return this.m_ProductName; }
            }

            [Category("Slave Info")]
            [DisplayName("Slave Type")]
            public SlaveType SlaveType
            {
                get { return this.m_SlaveType; }
            }

            [Browsable(false)]
            public EcStateMachine SlaveState
            {
                get { return m_SlaveState; }
            }
            #endregion

            #region Constructor
            public Slave(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName)
            {
                m_SlaveNo = SlaveNo;
                m_AliasNo = AliasNo;
                m_VendorId = VendorId;
                m_VendorName = VendorName;
                m_ProductCode = ProductCode;
                m_ProductName = ProductName;
            }
            #endregion

            #region Methods
            public void UpdateState(EcMasterInfo MasterInfo)
            {
                m_SlaveState = MasterInfo.Slaves[m_SlaveNo].State;
            }

            public void UpdateESCReg()
            {
                byte[] IFnRE = EcCtl.ReadRegister(m_SlaveNo, 0x0300, 8);
                byte[] FRE = EcCtl.ReadRegister(m_SlaveNo, 0x0308, 4);
                byte[] PUE = EcCtl.ReadRegister(m_SlaveNo, 0x030C, 1);
                byte[] LL = EcCtl.ReadRegister(m_SlaveNo, 0x0310, 4);

                for (int i = 0; i < 4; i++)
                {
                    m_InvalidFrameError[i] = IFnRE[i * 2];
                    m_RxError[i] = IFnRE[i * 2 + 1];
                    m_ForwardedRxError[i] = FRE[i];
                    m_LostLinkError[i] = LL[i];
                }
                m_ProcessingUnitError = PUE[0];
            }

            #region SDO Basic Method
            public int SetSDO(int idx, int sub, int size, int value)
            {
                return EcCtl.SetSDO(m_SlaveNo, idx, sub, size, value);
            }
            public int GetSDO(int idx, int sub, out byte[] value, out uint actualsize)
            {
                return EcCtl.GetSDO(m_SlaveNo, idx, sub, out value, out actualsize);
            }
            #endregion

            #region PDO Basic Method
            protected int SetPDO(int idx, int sub, int size, int value)
            {
                return EcCtl.TxPDO(m_SlaveNo, idx, sub, size, value);
            }
            protected int SetPDO(int idx, int sub, int size, uint value)
            {
                return EcCtl.TxPDO(m_SlaveNo, idx, sub, size, value);
            }
            protected int GetPDO(int idx, int sub, int size, out byte[] value)
            {
                return EcCtl.RxPDO(m_SlaveNo, idx, sub, size, out value);
            }
            #endregion
            #endregion

            #region Virtuals
            public virtual void Update() { }
            #endregion
        }

        public class Slave_Junction : Slave
        {
            #region 생성자
            public Slave_Junction(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName)
                : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.Junction;
            }
            #endregion
        }

        public class Slave_Servo : Slave
        {
            #region Feilds
            private CoreMotionAxisStatus m_AxisStatus;
            private int m_AxisIdx;

            private bool m_IsOnline;
            private bool m_IsOn;
            private bool m_IsHome;
            private bool m_IsLimitP;
            private bool m_IsLimitM;
            private bool m_IsStopped;
            private bool m_IsAlarm;
            private int m_AlarmCode;

            private double m_Position;
            private double m_Velocity;

            private OperationState m_OperationState;
            private AxisCommandMode m_CommandMode;

            private double m_GearRatio_Num;
            private double m_GearRatio_Den;

            private double m_HomingFastVel;
            private double m_HomingFastAcc;
            private double m_HomingFastDec;

            private double m_HomingSlowVel;
            private double m_HomingSlowAcc;
            private double m_HomingSlowDec;

            private double m_HomingShiftPos;
            private double m_HomingShiftVel;
            private double m_HomingShiftAcc;
            private double m_HomingShiftDec;

            #endregion

            #region Properties
            public bool IsOnline
            { get { return m_IsOnline; } }
            public bool IsOn
            { get { return m_IsOn; } }
            public bool IsDone
            {
                get { return m_OperationState == OperationState.Idle; }
            }
            public bool IsStopped
            {
                get { return m_IsStopped; }
            }
            public bool IsAlarm
            {
                get { return m_IsAlarm; }
            }

            public bool IsHome
            {
                get { return m_IsHome; }
            }
            public bool IsLimitP
            {
                get { return m_IsLimitP; }
            }
            public bool IsLimitM
            {
                get { return m_IsLimitM; }
            }


            public double CurrentPosition
            { get { return m_Position; } }

            public double CurrentVelocity
            { get { return m_Velocity; } }

            public OperationState OperationState
            {
                get { return m_OperationState; }
            }
            #endregion

            #region Constructor
            public Slave_Servo(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName,
                               int AxisIdx) : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.Servo;

                m_AxisIdx = AxisIdx;

                m_AxisStatus = new CoreMotionAxisStatus();
                m_OperationState = OperationState.Idle;
            }
            #endregion

            #region Methods
            private void UpdateStatus()
            {
                GetServoStatus();
                GetGearRatio();
            }

            private void GetServoStatus()
            {
                int eCode = ServoCtl.GetServoStatus(m_AxisIdx, out m_AxisStatus);

                if (eCode == 0)
                {
                    m_IsOnline = !m_AxisStatus.ServoOffline;
                    m_IsOn = m_AxisStatus.ServoOn;
                    m_IsHome = m_AxisStatus.HomeSwitch;
                    m_IsLimitP = m_AxisStatus.PositiveLS;
                    m_IsLimitM = m_AxisStatus.NegativeLS;
                    m_IsStopped = m_AxisStatus.CmdDistributionEnd;
                    m_IsAlarm = m_AxisStatus.AmpAlarm;
                    m_AlarmCode = m_AxisStatus.AmpAlarmCode;

                    m_Position = m_AxisStatus.ActualPos;
                    m_Velocity = m_AxisStatus.ActualVelocity;

                    m_OperationState = m_AxisStatus.OpState;
                    m_CommandMode = m_AxisStatus.AxisCommandMode;
                }
            }

            private void GetGearRatio()
            {
                int eCode = ServoCtl.GetGearRatio(m_AxisIdx, out double GearRatio_Num, out double GearRatio_Den);

                if (eCode == 0)
                {
                    m_GearRatio_Num = GearRatio_Num;
                    m_GearRatio_Den = GearRatio_Den;
                }
            }

            public int GetAxisCommandMode()
            {
                int eCode = ServoCtl.GetServoAxisCommandMode(m_AxisIdx, out m_CommandMode);

                return eCode;
            }

            public int SetAxisCommandMode(int CommandMode)
            {
                int eCode = SetAxisCommandMode((AxisCommandMode)CommandMode);

                return eCode;
            }
            private int SetAxisCommandMode(AxisCommandMode CommandMode)
            {
                int eCode = ServoCtl.SetServoAxsisCommandMode(m_AxisIdx, CommandMode);

                return eCode;
            }

            public int ServoOn()
            {
                int eCode = ServoCtl.ServoOn(m_AxisIdx);
                return eCode;
            }

            public int ServoOff()
            {
                int eCode = ServoCtl.ServoOff(m_AxisIdx);
                return eCode;
            }

            public int SetGearRatio(double numerator, double denominator)
            {
                int eCode = ServoCtl.SetGearRatio(m_AxisIdx, numerator, denominator);
                return eCode;
            }

            public int SetPosition(double pos)
            {
                int eCode = ServoCtl.SetPosition(m_AxisIdx, pos);
                return eCode;
            }

            public int Move(int pos, int vel, int acc, int dec, ServoCtl.ProfileType type, bool isRelative)
            {
                if (m_CommandMode != AxisCommandMode.Position) return -1;

                int eCode = isRelative ? ServoCtl.Start_Relative(m_AxisIdx, pos, vel, acc, dec, type)
                                       : ServoCtl.Start_Absolute(m_AxisIdx, pos, vel, acc, dec, type);

                return eCode;
            }

            public int Run(int vel, int acc, int dec)
            {
                if (m_CommandMode != AxisCommandMode.Velocity) return -1;

                int eCode = ServoCtl.Start_Velocity(m_AxisIdx, vel, acc, dec);

                return eCode;
            }

            public int Jog(int vel, int acc, int dec, bool isPositive)
            {
                int eCode = ServoCtl.Jog(m_AxisIdx, isPositive ? vel : -vel, acc, dec);
                return eCode;
            }

            public int Stop()
            {
                int eCode = -1;
                switch (m_CommandMode)
                {
                    case AxisCommandMode.Position:
                        eCode = ServoCtl.Stop_P(m_AxisIdx);
                        break;
                    case AxisCommandMode.Velocity:
                        eCode = ServoCtl.Stop_V(m_AxisIdx);
                        break;
                }
                return eCode;
            }

            public int EStop()
            {
                int eCode = ServoCtl.EStop();
                return eCode;
            }

            public int EStopRelease()
            {
                int eCode = ServoCtl.EStopRelease();
                return eCode;
            }

            public int Stop(int dec)
            {
                int eCode = ServoCtl.Stop(m_AxisIdx, dec);
                return eCode;
            }

            public int AlarmReset()
            {
                int eCode = ServoCtl.AlarmReset(m_AxisIdx);
                return eCode;
            }

            public int HomingParamInit()
            {
                int eCode = ServoCtl.HomingParamInit(m_AxisIdx, Config.HomeType.HS, Config.HomeDirection.Negative,
                                                     m_HomingFastVel, m_HomingFastAcc, m_HomingFastDec,
                                                     m_HomingSlowVel, m_HomingSlowAcc, m_HomingSlowDec,
                                                     m_HomingShiftPos, m_HomingShiftVel, m_HomingShiftAcc, m_HomingShiftDec);
                return eCode;
            }

            public int Homing()
            {
                int eCode = ServoCtl.Homing(m_AxisIdx);
                return eCode;
            }

            public int SetSync(Slave_Servo SyncSlave)
            {
                int eCode = ServoCtl.SetSync(m_AxisIdx, SyncSlave.m_AxisIdx);
                return eCode;
            }

            public int SetUnsync()
            {
                int eCode = ServoCtl.SetUnsync(m_AxisIdx);
                return eCode;
            }

            public AxisEvent GetAxisState()
            {
                if (m_OperationState == OperationState.Stop)
                    return AxisEvent.StopEvent;
                else
                    return AxisEvent.NoEvent;
            }
            #endregion

            #region Overrides
            public override void Update()
            {
                UpdateStatus();
            }
            #endregion
        }

        public class Slave_BLDC : Slave
        {
            #region Enums
            public enum BldcType
            {
                YDIIT,
                Fastech,
            }
            #endregion

            #region Feilds
            private BldcType m_BldcType;
            private bool m_LastReadTypeLF;

            private int m_InByteSize;
            private int m_OutByteSize;
            private int m_InStartByteAddr;
            private int m_OutStartByteAddr;

            private int m_InWordSize;
            private int m_OutWordSize;

            private byte[] m_InDataByte;
            private byte[] m_OutDataByte;
            private ushort[] m_InData;
            private ushort[] m_OutData;

            private object lock_InData = new object();
            private object lock_OutData = new object();

            private ushort m_R_LoadFactor;
            private short m_R_CurrentRPM;
            private uint m_R_Status;
            private bool m_R_RotateFW;
            private bool m_R_RotateBW;
            private bool m_R_Alarm;
            private ushort m_R_CurrentSpd;              //  YDIIT BMC 기능

            private bool m_W_RotateFW;
            private bool m_W_RotateBW;
            private bool m_W_AlarmReset;
            private ushort m_W_AccTime;
            private ushort m_W_DecTime;
            private ushort m_W_TargetRPM;
            private ushort m_W_TargetSpd;               //  YDIIT BMC 기능
            private bool m_W_ExternalControl = true;    //  YDIIT BMC 기능
            private bool m_W_SlowStop = true;           //  YDIIT BMC 기능
            #endregion

            #region Properties
            #endregion

            #region Constructor
            public Slave_BLDC(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName,
                              int InByteSize, int OutByteSize, int InStartByteAddr, int OutStartByteAddr, BldcType BldcType)
                : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.BLDC;

                m_InByteSize = InByteSize;
                m_OutByteSize = OutByteSize;
                m_InStartByteAddr = InStartByteAddr;
                m_OutStartByteAddr = OutStartByteAddr;

                m_InWordSize = m_InByteSize / 2;
                m_OutWordSize = m_OutByteSize / 2;

                InitializeArray();

                m_BldcType = BldcType;
            }

            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_InDataByte = new byte[m_InByteSize];
                m_OutDataByte = new byte[m_OutByteSize];

                lock (lock_InData) m_InData = new ushort[m_InWordSize];
                lock (lock_OutData) m_OutData = new ushort[m_OutWordSize];
            }

            private void Read()
            {
                if (m_InWordSize <= 0) return;

                IoCtl.GetInBytes(m_InByteSize, ref m_InDataByte, m_InStartByteAddr);

                SetWordArrayFromByteArray();
            }

            private void Write()
            {
                if (m_OutWordSize <= 0) return;

                SetByteArrayFromWordArray();

                IoCtl.SetOutBytes(m_OutByteSize, m_OutDataByte, m_OutStartByteAddr);
            }

            private void SetWordArrayFromByteArray()
            {
                ushort val = 0x0000;
                for (int idxByte = 0; idxByte < m_InByteSize; idxByte++)
                {
                    if (idxByte % 2 == 0)
                    {
                        val = 0x0000;
                        val |= m_InDataByte[idxByte];
                    }
                    else // if (idxByte % 2 == 1)
                    {
                        val |= (ushort)(m_InDataByte[idxByte] << 8);
                        lock (lock_InData) m_InData[idxByte / 2] = val;
                    }
                }
            }

            private void SetByteArrayFromWordArray()
            {
                byte valLow = 0x00;
                byte valHigh = 0x00;
                for (int idxWord = 0; idxWord < m_OutWordSize; idxWord++)
                {
                    lock (lock_OutData)
                    {
                        valLow = (byte)(m_OutData[idxWord] & 0x00FF);
                        valHigh = (byte)((m_OutData[idxWord] & 0xFF00) >> 8);
                    }

                    m_OutDataByte[idxWord * 2] = valLow;
                    m_OutDataByte[idxWord * 2 + 1] = valHigh;
                }

            }

            private void ReadDataInterpretation()
            {
                switch (m_BldcType)
                {
                    case BldcType.YDIIT:
                        Interpretation_YDIIT();
                        break;
                    case BldcType.Fastech:
                        Interpretation_Fastech();
                        break;
                }
            }

            private void Interpretation_YDIIT()
            {
                if (m_BldcType != BldcType.YDIIT) return;

                lock (lock_OutData)
                {
                    m_LastReadTypeLF = (m_OutData[0] & 0x0020) != 0;
                }

                lock (lock_InData)
                {
                    m_R_CurrentSpd = (ushort)(m_InData[0] & 0x03FF);        //  하위 10개 Bit
                    m_R_Status = (uint)((m_InData[0] & 0x0C00) >> 10);      //  10,11 Bit
                    m_R_RotateFW = (m_InData[0] & 0x0400) == 0x1000;        //  12 Bit
                    m_R_RotateBW = (m_InData[0] & 0x0800) == 0x2000;        //  13 Bit
                    m_R_Alarm = (m_InData[0] & 0x8000) != 0;                //  15 Bit

                    if (m_LastReadTypeLF)
                        m_R_LoadFactor = m_InData[1];
                    else
                        m_R_CurrentRPM = (short)m_InData[1];
                }
            }

            private void Interpretation_Fastech()
            {
                if (m_BldcType != BldcType.Fastech) return;

                lock (lock_InData)
                {
                    m_R_LoadFactor = m_InData[0];
                    m_R_CurrentRPM = (short)m_InData[1];
                    m_R_Status = (uint)(m_InData[2] + (m_InData[3] * 0x0100));
                    m_R_RotateFW = (m_InData[2] & 0x01) == 0x01;
                    m_R_RotateBW = (m_InData[2] & 0x02) == 0x02;
                    m_R_Alarm = (m_InData[3] & 0x40) == 0x40;
                }
            }

            private void SetWriteData()
            {
                switch (m_BldcType)
                {
                    case BldcType.YDIIT:
                        SetWriteData_YDIIT();
                        break;
                    case BldcType.Fastech:
                        SetWriteData_Fastech();
                        break;
                }
            }
            private void SetWriteData_YDIIT()
            {
                if (m_BldcType != BldcType.YDIIT) return;

                lock (lock_OutData)
                {
                    ushort outdata = 0x0000;

                    if (m_W_ExternalControl) outdata |= 0x0001;
                    if (m_W_RotateFW) outdata |= 0x0002;
                    if (m_W_RotateBW) outdata |= 0x0004;
                    if (m_W_AlarmReset) outdata |= 0x0008;
                    if (m_W_SlowStop) outdata |= 0x0010;
                    if (!m_LastReadTypeLF) outdata |= 0x0020;

                    outdata |= (ushort)((m_W_DecTime & 0x000F) << 08);
                    outdata |= (ushort)((m_W_AccTime & 0x000F) << 12);

                    m_OutData[0] = outdata;

                    m_OutData[1] = m_W_TargetSpd;
                }
            }
            private void SetWriteData_Fastech()
            {
                if (m_BldcType != BldcType.Fastech) return;

                lock (lock_OutData)
                {
                    ushort outdata = 0x0000;

                    if (m_W_RotateFW) outdata |= 0x0001;
                    if (m_W_RotateBW) outdata |= 0x0002;
                    if (m_W_AlarmReset) outdata |= 0x0100;

                    m_OutData[0] = m_W_AccTime;
                    m_OutData[1] = m_W_DecTime;
                    m_OutData[2] = m_W_TargetRPM;
                    m_OutData[3] = outdata;
                }
            }

            public double GetLoadFactor()
            {
                return m_R_LoadFactor / 10.0;
            }

            public short GetCurrentRPM()
            {
                return m_R_CurrentRPM;
            }

            public bool IsRotateFw()
            {
                return m_R_RotateFW;
            }

            public bool IsRotateBw()
            {
                return m_R_RotateBW;
            }

            public bool IsAlarm()
            {
                return m_R_Alarm;
            }

            public void SetRotateFw(bool op)
            {
                m_W_RotateFW = op;
            }

            public void SetRotateBw(bool op)
            {
                m_W_RotateBW = op;
            }

            public void SetAlarmReset(bool op)
            {
                m_W_AlarmReset = op;
            }

            public void SetAccTime(ushort val)
            {
                m_W_AccTime = val;
            }

            public void SetDecTime(ushort val)
            {
                m_W_DecTime = val;
            }

            public void SetTargetRPM(ushort val)
            {
                switch (m_BldcType)
                {
                    case BldcType.YDIIT:
                        m_W_TargetSpd = (ushort)(1023.0 * val / 3000);
                        break;
                    case BldcType.Fastech:
                        m_W_TargetRPM = val;
                        break;
                }
            }
            #endregion

            #region Overrides
            public override void Update()
            {
                Read();
                ReadDataInterpretation();

                SetWriteData();
                Write();
            }
            #endregion
        }

        public class Slave_Inverter : Slave
        {
            #region Feilds
            private int m_InByteSize;
            private int m_OutByteSize;
            private int m_InStartByteAddr;
            private int m_OutStartByteAddr;

            private int m_InWordSize;
            private int m_OutWordSize;

            private byte[] m_InDataByte;
            private byte[] m_OutDataByte;
            private ushort[] m_InData;
            private ushort[] m_OutData;

            private object lock_InData = new object();
            private object lock_OutData = new object();

            private double m_R_OutputCurrent;
            private double m_R_OutputFrequency;
            private ushort m_R_DriveStatus;
            private ushort m_R_TripInfomation;
            private double m_R_AccTime;
            private double m_R_DecTime;
            private double m_R_TargetFrequency;
            private ushort m_R_DriveCommand;

            private double m_W_AccTime;
            private double m_W_DecTime;
            private double m_W_TargetFrequency;
            private ushort m_W_DriveCommand;
            #endregion

            #region Properties
            #endregion

            #region Constructor
            public Slave_Inverter(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName,
                                  int InByteSize, int OutByteSize, int InStartByteAddr, int OutStartByteAddr)
                : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.BLDC;

                m_InByteSize = InByteSize;
                m_OutByteSize = OutByteSize;
                m_InStartByteAddr = InStartByteAddr;
                m_OutStartByteAddr = OutStartByteAddr;

                m_InWordSize = m_InByteSize / 2;
                m_OutWordSize = m_OutByteSize / 2;

                InitializeArray();
            }
            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_InDataByte = new byte[m_InByteSize];
                m_OutDataByte = new byte[m_OutByteSize];

                lock (lock_InData) m_InData = new ushort[m_InWordSize];
                lock (lock_OutData) m_OutData = new ushort[m_OutWordSize];
            }

            private void Read()
            {
                if (m_InWordSize <= 0) return;

                IoCtl.GetInBytes(m_InByteSize, ref m_InDataByte, m_InStartByteAddr);

                SetWordArrayFromByteArray();
            }

            private void Write()
            {
                if (m_OutWordSize <= 0) return;

                SetByteArrayFromWordArray();

                IoCtl.SetOutBytes(m_OutByteSize, m_OutDataByte, m_OutStartByteAddr);
            }

            private void SetWordArrayFromByteArray()
            {
                ushort val = 0x0000;
                for (int idxByte = 0; idxByte < m_InByteSize; idxByte++)
                {
                    if (idxByte % 2 == 0)
                    {
                        val = 0x0000;
                        val |= m_InDataByte[idxByte];
                    }
                    else // if (idxByte % 2 == 1)
                    {
                        val |= (ushort)(m_InDataByte[idxByte] << 8);
                        lock (lock_InData) m_InData[idxByte / 2] = val;
                    }
                }
            }

            private void SetByteArrayFromWordArray()
            {
                byte valLow = 0x00;
                byte valHigh = 0x00;
                for (int idxWord = 0; idxWord < m_OutWordSize; idxWord++)
                {
                    lock (lock_OutData)
                    {
                        valLow = (byte)(m_OutData[idxWord] & 0x00FF);
                        valHigh = (byte)((m_OutData[idxWord] & 0xFF00) >> 8);
                    }

                    m_OutDataByte[idxWord * 2] = valLow;
                    m_OutDataByte[idxWord * 2 + 1] = valHigh;
                }

            }

            private void ReadDataInterpretation()
            {
                lock (lock_InData)
                {
                    m_R_OutputCurrent = m_InData[2] * 0.1;
                    m_R_OutputFrequency = m_InData[3] * 0.01;
                    m_R_DriveStatus = m_InData[4];
                    m_R_TripInfomation = m_InData[5];
                    m_R_AccTime = m_InData[6] * 0.1;
                    m_R_DecTime = m_InData[7] * 0.1;
                    m_R_TargetFrequency = m_InData[8] * 0.01;
                    m_R_DriveCommand = m_InData[9];
                }
            }

            private void SetWriteData()
            {
                lock (lock_OutData)
                {
                    m_OutData[2] = (ushort)(m_W_AccTime * 10);
                    m_OutData[3] = (ushort)(m_W_DecTime * 10);
                    m_OutData[4] = (ushort)(m_W_TargetFrequency * 100);
                    m_OutData[5] = m_W_DriveCommand;
                }
            }

            public bool IsAlarm()
            {
                return m_R_TripInfomation != 0;
            }

            public bool IsRun()
            {
                return (m_R_DriveCommand & 0x0003) != 0;    //  0b 0000 0000 0000 0110
            }

            public void SetStopBit(bool op)
            {
                if (op)
                    m_W_DriveCommand |= 0x0001; //  0b 0000 0000 0000 0001
                else
                    m_W_DriveCommand &= 0xFFFE; //  0b 1111 1111 1111 1110
            }

            public void SetRunBit(bool op)
            {
                if (op)
                    m_W_DriveCommand |= 0x0002; //  0b 0000 0000 0000 0010
                else
                    m_W_DriveCommand &= 0xFFFD; //  0b 1111 1111 1111 1101
            }

            public void SetAlarmResetBit(bool op)
            {
                if (op)
                    m_W_DriveCommand |= 0x0008; //  0b 0000 0000 0000 1000
                else
                    m_W_DriveCommand &= 0xFFF7; //  0b 1111 1111 1111 0111
            }

            public void SetTargetFrequency(double frequency)
            {
                m_W_TargetFrequency = frequency;
            }

            public double GetCurrentFrequency()
            {
                return m_R_OutputFrequency;
            }
            #endregion

            #region Overrides
            public override void Update()
            {
                Read();
                ReadDataInterpretation();

                SetWriteData();
                Write();
            }
            #endregion
        }

        public class Slave_DIO : Slave
        {
            #region Fields
            private int m_InByteSize;
            private int m_OutByteSize;
            private int m_InStartByteAddr;
            private int m_OutStartByteAddr;

            private byte[] m_InData;
            private byte[] m_OutData;

            private object lock_InData = new object();
            private object lock_OutData = new object();
            #endregion

            #region Properties
            public int InByteSize
            {
                get { return m_InByteSize; }
            }
            public int OutByteSize
            {
                get { return m_OutByteSize; }
            }
            public int InStartByteAddr
            {
                get { return m_InStartByteAddr; }
            }
            public int OutStartByteAddr
            {
                get { return m_OutStartByteAddr; }
            }

            public byte[] InData
            {
                get { return m_InData; }
            }
            public byte[] OutData
            {
                get { return m_OutData; }
            }
            #endregion

            #region Constructor
            public Slave_DIO(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName,
                             int InByteSize, int OutByteSize, int InStartByteAddr, int OutStartByteAddr)
                : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.DIO;

                m_InByteSize = InByteSize;
                m_OutByteSize = OutByteSize;
                m_InStartByteAddr = InStartByteAddr;
                m_OutStartByteAddr = OutStartByteAddr;

                InitializeArray();
            }
            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_InData = new byte[m_InByteSize];
                m_OutData = new byte[m_OutByteSize];
            }

            private void Read()
            {
                if (m_InByteSize <= 0) return;

                lock (lock_InData)
                    IoCtl.GetInBytes(m_InByteSize, ref m_InData, m_InStartByteAddr);
            }

            private void Write()
            {
                if (m_OutByteSize <= 0) return;

                lock (lock_OutData)
                    IoCtl.SetOutBytes(m_OutByteSize, m_OutData, m_OutStartByteAddr);
            }

            public bool GetDi(int channel)
            {
                int idxByte = channel / 8;
                int idxBit = channel % 8;

                bool op = false;

                lock (lock_InData)
                    op = (m_InData[idxByte] & (0x01 << idxBit)) != 0;

                return op;
            }

            public bool GetDo(int channel)
            {
                int idxByte = channel / 8;
                int idxBit = channel % 8;

                bool op = false;

                lock (lock_OutData)
                    op = (m_OutData[idxByte] & (0x01 << idxBit)) != 0;

                return op;
            }

            public void SetDi(int channel, bool op)
            {
                int idxByte = channel / 8;
                int idxBit = channel % 8;

                if (op)
                    lock (lock_InData)
                        m_InData[idxByte] |= (byte)(0x01 << idxBit);
                else
                    lock (lock_InData)
                        m_InData[idxByte] &= (byte)~(0x01 << idxBit);
            }

            public void SetDo(int channel, bool op)
            {
                int idxByte = channel / 8;
                int idxBit = channel % 8;

                if (op)
                    lock (lock_OutData)
                        m_OutData[idxByte] |= (byte)(0x01 << idxBit);
                else
                    lock (lock_OutData)
                        m_OutData[idxByte] &= (byte)~(0x01 << idxBit);
            }
            #endregion

            #region Overrides
            public override void Update()
            {
                Read();
                Write();
            }
            #endregion
        }

        public class Slave_AIO : Slave
        {
            #region Fields
            private int m_InByteSize;
            private int m_OutByteSize;
            private int m_InStartByteAddr;
            private int m_OutStartByteAddr;

            private int m_InWordSize;
            private int m_OutWordSize;

            private byte[] m_InDataByte;
            private byte[] m_OutDataByte;
            private ushort[] m_InData;
            private ushort[] m_OutData;

            private object lock_InData = new object();
            private object lock_OutData = new object();
            #endregion

            #region Properties
            public int InByteSize
            {
                get { return m_InByteSize; }
            }
            public int OutByteSize
            {
                get { return m_OutByteSize; }
            }
            public int InStartByteAddr
            {
                get { return m_InStartByteAddr; }
            }
            public int OutStartByteAddr
            {
                get { return m_OutStartByteAddr; }
            }

            public ushort[] InData
            {
                get { return m_InData; }
            }
            public ushort[] OutData
            {
                get { return m_OutData; }
            }
            #endregion

            #region Constructor
            public Slave_AIO(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName,
                             int InByteSize, int OutByteSize, int InStartByteAddr, int OutStartByteAddr)
                : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.AIO;

                m_InByteSize = InByteSize;
                m_OutByteSize = OutByteSize;
                m_InStartByteAddr = InStartByteAddr;
                m_OutStartByteAddr = OutStartByteAddr;

                m_InWordSize = m_InByteSize / 2;
                m_OutWordSize = m_OutWordSize / 2;

                InitializeArray();
            }
            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_InDataByte = new byte[m_InByteSize];
                m_OutDataByte = new byte[m_OutByteSize];

                m_InData = new ushort[m_InWordSize];
                m_OutData = new ushort[m_OutWordSize];
            }

            private void Read()
            {
                if (m_InWordSize <= 0) return;

                IoCtl.GetInBytes(m_InByteSize, ref m_InDataByte, m_InStartByteAddr);

                SetWordArrayFromByteArray();
            }

            private void Write()
            {
                if (m_OutWordSize <= 0) return;

                SetByteArrayFromWordArray();

                IoCtl.SetOutBytes(m_OutByteSize, m_OutDataByte, m_OutStartByteAddr);
            }

            private void SetWordArrayFromByteArray()
            {
                ushort val = 0x0000;
                for (int idxByte = 0; idxByte < m_InByteSize; idxByte++)
                {
                    if (idxByte % 2 == 0)
                    {
                        val = 0x0000;
                        val |= m_InDataByte[idxByte];
                    }
                    else // if (idxByte % 2 == 1)
                    {
                        val |= (ushort)(m_InDataByte[idxByte] << 8);
                        lock (lock_InData) m_InData[idxByte / 2] = val;
                    }
                }
            }

            private void SetByteArrayFromWordArray()
            {
                byte valLow = 0x00;
                byte valHigh = 0x00;
                for (int idxWord = 0; idxWord < m_OutWordSize; idxWord++)
                {
                    lock (lock_OutData)
                    {
                        valLow = (byte)(m_OutData[idxWord] & 0x00FF);
                        valHigh = (byte)((m_OutData[idxWord] & 0xFF00) >> 8);
                    }

                    m_OutDataByte[idxWord * 2] = valLow;
                    m_OutDataByte[idxWord * 2 + 1] = valHigh;
                }

            }

            public ushort GetAi(int channel)
            {
                ushort val = 0;

                lock (lock_InData)
                    val = m_InData[channel];

                return val;
            }

            public ushort GetAo(int channel)
            {
                ushort val = 0;

                lock (lock_OutData)
                    val = m_OutData[channel];

                return val;
            }

            public void SetAi(int channel, ushort val)
            {
                lock (lock_InData)
                    m_InData[channel] = val;
            }

            public void SetAo(int channel, ushort val)
            {
                lock (lock_OutData)
                    m_OutData[channel] = val;
            }
            #endregion

            #region Overrides
            public override void Update()
            {
                Read();
                Write();
            }
            #endregion
        }

        public class Slave_AP : Slave
        {
            #region Constants
            private const int c_PeerCountMax = 16;
            #endregion

            #region Fields
            private int m_InByteSize;
            private int m_OutByteSize;
            private int m_InStartByteAddr;
            private int m_OutStartByteAddr;

            private int m_InWordSize;
            private int m_OutWordSize;

            private byte[] m_InDataByte;
            private byte[] m_OutDataByte;
            private ushort[] m_InData;
            private ushort[] m_OutData;

            private object lock_InData = new object();
            private object lock_OutData = new object();

            private Peer[] m_Peers;

            private object lock_Peers = new object();

            private int m_LastSerialChannel = 13;
            private int m_LastSerialPage = 1;
            #endregion

            #region Properties
            public Peer[] Peers
            {
                get { return m_Peers; }
            }
            #endregion

            #region Constructor
            public Slave_AP(int SlaveNo, int AliasNo, int VendorId, string VendorName, int ProductCode, string ProductName,
                            int InByteSize, int OutByteSize, int InStartByteAddr, int OutStartByteAddr)
                : base(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName)
            {
                m_SlaveType = SlaveType.AP;

                m_InByteSize = InByteSize;
                m_OutByteSize = OutByteSize;
                m_InStartByteAddr = InStartByteAddr;
                m_OutStartByteAddr = OutStartByteAddr;

                m_InWordSize = m_InByteSize / 2;
                m_OutWordSize = m_OutByteSize / 2;

                InitializeArray();
            }
            #endregion

            #region Methods
            private void InitializeArray()
            {
                m_InDataByte = new byte[m_InByteSize];
                m_OutDataByte = new byte[m_OutByteSize];

                m_InData = new ushort[m_InWordSize];
                m_OutData = new ushort[m_OutWordSize];

                m_Peers = new Peer[c_PeerCountMax];
                for (int i = 0; i < c_PeerCountMax; i++)
                    m_Peers[i] = new Peer(this, i + 1);
            }

            public void MakePeer(int channel, PeerType type, int id)
            {
                Peer peer = Peers[channel];
                if (peer.PeerType == type && peer.Id == id) return;

                switch (type)
                {
                    case PeerType.DIW:
                        Peers[channel] = new Peer_DIW(this, channel, id);
                        break;
                    case PeerType.CDA:
                        Peers[channel] = new Peer_CDA(this, channel, id);
                        break;
                    case PeerType.SmartDamper:
                        Peers[channel] = new Peer_SmartDamper(this, channel, id);
                        break;
                    case PeerType.XrayIonizer:
                        Peers[channel] = new Peer_XrayIonizer(this, channel, id, 10);
                        break;
                    case PeerType.D40A:
                        Peers[channel] = new Peer_D40A(this, channel, id);
                        break;
                    case PeerType.D4SL:
                        Peers[channel] = new Peer_D4SL(this, channel, id);
                        break;
                    case PeerType.TIC:
                        Peers[channel] = new Peer_TIC(this, channel, id, 10);
                        break;
                    case PeerType.LFC:
                        Peers[channel] = new Peer_LFC(this, channel, id);
                        break;
                    case PeerType.Manometer:
                        Peers[channel] = new Peer_Manometer(this, channel, id);
                        break;
                    case PeerType.LCT:
                        Peers[channel] = new Peer_LCT(this, channel, id);
                        break;
                    default:
                        Peers[channel] = new Peer(this, channel);
                        break;
                }
            }

            private void Read()
            {
                if (m_InWordSize <= 0) return;

                IoCtl.GetInBytes(m_InByteSize, ref m_InDataByte, m_InStartByteAddr);

                SetWordArrayFromByteArray();
            }

            private void Write()
            {
                if (m_OutWordSize <= 0) return;

                SetByteArrayFromWordArray();

                IoCtl.SetOutBytes(m_OutByteSize, m_OutDataByte, m_OutStartByteAddr);
            }

            private void SetWordArrayFromByteArray()
            {
                ushort val = 0x0000;
                for (int idxByte = 0; idxByte < m_InByteSize; idxByte++)
                {
                    if (idxByte % 2 == 0)
                    {
                        val = 0x0000;
                        val |= m_InDataByte[idxByte];
                    }
                    else // if (idxByte % 2 == 1)
                    {
                        val |= (ushort)(m_InDataByte[idxByte] << 8);
                        lock (lock_InData) m_InData[idxByte / 2] = val;
                    }
                }
            }

            private void SetByteArrayFromWordArray()
            {
                byte valLow = 0x00;
                byte valHigh = 0x00;
                for (int idxWord = 0; idxWord < m_OutWordSize; idxWord++)
                {
                    lock (lock_OutData)
                    {
                        valLow = (byte)(m_OutData[idxWord] & 0x00FF);
                        valHigh = (byte)((m_OutData[idxWord] & 0xFF00) >> 8);
                    }

                    m_OutDataByte[idxWord * 2] = valLow;
                    m_OutDataByte[idxWord * 2 + 1] = valHigh;
                }

            }

            private void PairingStateUpdate()
            {
                for (int channel = 1; channel <= c_PeerCountMax; channel++)
                {
                    lock (lock_Peers)
                    {
                        int idx = channel - 1;

                        Peer peer = m_Peers[idx];
                        PeerType pType = peer.PeerType;
                        int pID = peer.Id;

                        //  Pairing 상태 업데이트
                        bool isPaired;
                        lock (lock_InData) isPaired = (m_InData[0] & (0x01 << idx)) != 0x00;       // In Data로 획득한 Pairing 상태

                        bool isPairingSignalOn;
                        lock (lock_OutData) isPairingSignalOn = (m_OutData[0] & (0x01 << idx)) != 0x00;       //  현재 Out Data의 Pairing Bit 상태

                        //  Null일 경우
                        if (pType == PeerType.Null)
                        {
                            if (isPaired)
                                UnpairingSignalOn(channel);
                            else
                            {
                                if (isPairingSignalOn)
                                    PairingSignalOff(channel);
                            }

                            continue;
                        }
                        //  이외의 경우 Signal On Off Sq
                        else
                        {
                            //  Pairing 신호가 On 되어있을 경우
                            if (isPaired)
                            {
                                switch (peer.PairingState)
                                {
                                    case PairingState.Unpaired:
                                        UnpairingSignalOn(channel);
                                        break;
                                    case PairingState.Pairing:
                                        PairingSignalOff(channel);
                                        peer.PairingState = PairingState.Paired;
                                        break;
                                    case PairingState.Paired:
                                        if (isPairingSignalOn) PairingSignalOff(channel);
                                        break;
                                    case PairingState.Unpairing:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            //  Pairing 신호가 Off 되어있을 경우
                            else // if(!isPaired)
                            {
                                switch (peer.PairingState)
                                {
                                    case PairingState.Unpaired:
                                        PairingSignalOn(channel);
                                        break;
                                    case PairingState.Pairing:
                                        PairingSignalOn(channel);
                                        break;
                                    case PairingState.Paired:
                                        peer.PairingState = PairingState.Unpaired;
                                        break;
                                    case PairingState.Unpairing:
                                        PairingSignalOff(channel);
                                        peer.PairingState = PairingState.Unpaired;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            private void PairingSignalOn(int channel)
            {
                ushort PairingOnSign = 0;
                ushort PairingData = 0;

                lock (lock_Peers)
                {
                    int idx = channel - 1;
                    Peer peer = m_Peers[idx];

                    PeerType pType = peer.PeerType;
                    int pID = peer.Id;

                    PairingOnSign |= (ushort)(0x01 << idx);

                    ushort pData = 0;
                    pData |= (ushort)((ushort)pType << 8);
                    pData |= (ushort)pID;

                    PairingData = pData;

                    peer.PairingState = PairingState.Pairing;
                }

                //  Word Array에 저장
                lock (lock_OutData)
                {
                    m_OutData[0] |= PairingOnSign;
                    m_OutData[channel] = PairingData;
                }
            }

            private void PairingSignalOff(int channel)
            {
                ushort PairingOffSign = 0;
                ushort PairingData = 0;

                lock (lock_Peers)
                {
                    int idx = channel - 1;
                    Peer peer = m_Peers[idx];

                    PeerType pType = peer.PeerType;
                    int pID = peer.Id;

                    PairingOffSign = (ushort)~(0x01 << idx);

                    PairingData = 0;
                }

                //  Word Array에 저장
                lock (lock_OutData)
                {
                    m_OutData[0] &= PairingOffSign;
                    m_OutData[channel] = PairingData;
                }
            }

            private void UnpairingSignalOn(int channel)
            {
                ushort PairingOnSign = 0;
                ushort PairingData = 0;

                lock (lock_Peers)
                {
                    int idx = channel - 1;
                    Peer peer = m_Peers[idx];

                    PeerType pType = peer.PeerType;
                    int pID = peer.Id;

                    PairingOnSign = (ushort)(0x01 << idx);

                    PairingData = 0;

                    if (pType != PeerType.Null)
                        peer.PairingState = PairingState.Unpairing;
                }

                //  Word Array에 저장
                lock (lock_OutData)
                {
                    m_OutData[0] |= PairingOnSign;
                    m_OutData[channel] = PairingData;
                }
            }

            private void ReadIoData()
            {
                lock (lock_Peers)
                {
                    foreach (Peer peer in m_Peers)
                    {
                        if (!peer.IsPaired) continue;

                        int index = peer.Channel + 1;

                        lock (lock_InData)
                            peer.ReadIoData(m_InData[index]);
                    }
                }
            }

            private void WriteIoData()
            {
                lock (lock_Peers)
                {
                    foreach (Peer peer in m_Peers)
                    {
                        if (!peer.IsPaired) continue;

                        int index = peer.Channel + 1;

                        lock (lock_OutData)
                            m_OutData[index] = peer.WriteIoData();
                    }
                }
            }

            private void ReadSerialData()
            {
                ushort SerialCommandData;
                lock (lock_InData) SerialCommandData = m_InData[17];

                int ch = (SerialCommandData & 0x00F0) >> 4;
                int page = SerialCommandData & 0x000F;

                if (ch == 0 || page == 0) return;

                ch += 12;
                if (ch > c_PeerCountMax) return;

                lock (lock_Peers)
                {
                    Peer peer = m_Peers[ch - 1];

                    if (!peer.IsPaired) return;

                    ushort[] SerialData = new ushort[7];
                    lock (lock_InData)
                        Array.Copy(m_InData, 18, SerialData, 0, 7);

                    peer.ReadSerialData(SerialData);
                }
            }

            private void WriteSerialData()
            {
                //  Serial Peer 중 Pairing 된 Peer가 없으면 종료
                lock (lock_Peers)
                {
                    bool isPairedSerialPeerExist = false;
                    for (int ch = 13; ch <= c_PeerCountMax; ch++)
                    {
                        Peer peer = m_Peers[ch - 1];
                        if (peer.IsPaired) isPairedSerialPeerExist = true;
                    }
                    if (!isPairedSerialPeerExist) return;
                }

                //  Command
                {
                    int ch = m_LastSerialChannel;
                    int page = m_LastSerialPage + 1;

                    while (true)
                    {
                        if (ch == m_LastSerialChannel && page == m_LastSerialPage) break;

                        lock (lock_Peers)
                        {
                            Peer peer = m_Peers[ch - 1];
                            if (!peer.IsPaired || page > peer.MaxPage_Serial)
                            {
                                ch = ch >= c_PeerCountMax ? 13 : ch + 1;
                                page = 1;
                                continue;
                            }
                        }

                        break;
                    }

                    ushort[] SerialData;
                    lock (lock_Peers)
                    {
                        Peer peer = m_Peers[ch - 1];
                        SerialData = peer.WriteSerialData(page);
                    }

                    m_LastSerialChannel = ch;
                    m_LastSerialPage = page;

                    ushort SerialCommandData = (ushort)(((ch - 12) << 12) | (page << 8)
                                                      | ((ch - 12) << 04) | (page << 0));


                    //  OutData에 기록
                    lock (lock_OutData)
                    {
                        m_OutData[17] = SerialCommandData;
                        Array.Copy(SerialData, 0, m_OutData, 18, 7);
                    }
                }
            }
            #endregion

            #region Overrides
            public override void Update()
            {
                Read();

                PairingStateUpdate();

                ReadIoData();
                ReadSerialData();

                WriteIoData();
                WriteSerialData();

                Write();
            }
            #endregion
        }
    }
}
