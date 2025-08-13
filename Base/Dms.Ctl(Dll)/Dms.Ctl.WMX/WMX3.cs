using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dms.Common;
using Dms.Util.IODefine;
using System.Threading;

namespace Dms.Ctl
{
    public class WMX3 : ICtlDevice_Adv
    {
        #region Singleton
        public static readonly WMX3 Instance = new WMX3();
        #endregion

        #region Fields
        private Simul m_Simul;
        private bool m_SimulateController = false;

        private bool m_Initialized = false;
        private bool m_Uninitialized = false;

        private ActiveState m_ActiveState = ActiveState.UnKnown;
        private string m_ControllerState = "";
        private IoDefines m_ModuleConfig = new IoDefines(FieldBusType.MovensysEtherCAT);
        private Dictionary<EcSlave, WMX.Slave> m_SlavePairs = new Dictionary<EcSlave, WMX.Slave>();

        private List<SeqInitSlave> m_SeqInitSlave = new List<SeqInitSlave>();

        private int m_ServoCount = 0;
        private int m_BldcCount = 0;
        private int m_InverterCount = 0;
        private int m_DiCount = 0;
        private int m_DoCount = 0;
        private int m_AiCount = 0;
        private int m_AoCount = 0;
        private int m_ApCount = 0;

        private List<EcSlaveItem_Servo> m_ServoItemList = new List<EcSlaveItem_Servo>();
        private List<EcSlaveItem_BLDC> m_BldcItemList = new List<EcSlaveItem_BLDC>();
        private List<EcSlaveItem_Inverter> m_InverterItemList = new List<EcSlaveItem_Inverter>();
        private List<EcSlaveItem_DI> m_DiItemList = new List<EcSlaveItem_DI>();
        private List<EcSlaveItem_DO> m_DoItemList = new List<EcSlaveItem_DO>();
        private List<EcSlaveItem_AI> m_AiItemList = new List<EcSlaveItem_AI>();
        private List<EcSlaveItem_AO> m_AoItemList = new List<EcSlaveItem_AO>();
        private List<EcSlaveItem_AP> m_ApItemList = new List<EcSlaveItem_AP>();

        private XLog m_Log = new XLog("WMX", XLog.LogStampType.UseStamp);

        #endregion

        #region Properties
        public Simul Simul
        {
            get { return m_Simul; }
        }

        public IoDefines ModuleConfig
        {
            get { return m_ModuleConfig; }
            set { m_ModuleConfig = value; }
        }

        public int ServoCount
        {
            get { return m_ServoCount; }
            set { m_ServoCount = value; }
        }

        public int BldcCount
        {
            get { return m_BldcCount; }
            set { m_BldcCount = value; }
        }
        public int InverterCount
        {
            get { return m_InverterCount; }
            set { m_InverterCount = value; }
        }

        public int DiCount
        {
            get { return m_DiCount; }
            set { m_DiCount = value; }
        }

        public int DoCount
        {
            get { return m_DoCount; }
            set { m_DoCount = value; }
        }

        public int AiCount
        {
            get { return m_AiCount; }
            set { m_AiCount = value; }
        }

        public int AoCount
        {
            get { return m_AoCount; }
            set { m_AoCount = value; }
        }

        public int ApCount
        {
            get { return m_ApCount; }
            set { m_ApCount = value; }
        }

        public bool IsAdvDevice { get { return true; } }
        #endregion

        #region Constructor
        private WMX3()
        {
            m_Simul = AppConfig.Instance.Simul;
        }
        #endregion

        #region EventHandler
        #endregion

        #region Methods
        public DmsErrors Initialize(IoDefines iodefines)
        {
            m_SimulateController = m_Simul.Device || iodefines.SimulateController;

            bool ok = InitializeMaster(iodefines);

            if (ok)
            {
                CreateModuleInfo(iodefines);
                //CreateBuffer();
            }

            m_Initialized = ok;
            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }

        private bool InitializeMaster(IoDefines iodefines)
        {
            if (iodefines.BusType != FieldBusType.MovensysEtherCAT)
                return false;
            else
            {
                if (iodefines.Count > 0)
                    WMX.Initialize();
                else
                    return false;

                return true;
            }
        }

        private void CreateModuleInfo(IoDefines iodefines)
        {
            //if (m_Simul.Device)
            //{
            //    //  Simulation Mode 추후 작성
            //    m_DiCount = 16 * 256;   //  DI16 256EA 기준
            //    m_DoCount = 16 * 256;   //  DO16 256EA 기준
            //    m_AiCount = 8 * 256;    //  AI8 256EA 기준
            //    m_AoCount = 8 * 256;    //  AO8 256EA 기준
            //    m_ApCount = 16 * 256;   //  AP16 256EA 기준;
            //}
            //else
            {
                for (int nodeid = 0; nodeid < iodefines.Count; nodeid++)
                {
                    IoNodeMovensysEcMaster ecmaster = new IoNodeMovensysEcMaster();
                    IoNodeMovensysEcMaster definedecmaster = iodefines.Container[nodeid] as IoNodeMovensysEcMaster;

                    int slaveCount = definedecmaster.Count;
                    for (int slaveid = 0; slaveid < slaveCount; slaveid++)
                    {
                        EcSlave slave = definedecmaster.Slaves[slaveid];
                        //EcSlave slave = definedslave.Copy();

                        //slave.CreateChannels();
                        UpdateChannelId(ref slave);
                        UpdateSlaveInfo(ref slave);

                        ecmaster.Slaves.Add(slave);
                    }

                    m_ModuleConfig.Container.Add(ecmaster);

                    m_SeqInitSlave.Add(new SeqInitSlave(ecmaster));
                }
            }
        }

        private void UpdateChannelId(ref EcSlave ecslave)
        {
            switch (ecslave.SlaveType)
            {
                case SlaveType.Servo:
                    {
                        EcSlave_Servo _servo = ecslave as EcSlave_Servo;
                        for (int ch = 0; ch < _servo.ServoCount; ch++)
                        {
                            _servo.Servos[ch].Id = m_ServoCount++;
                            m_ServoItemList.Add(_servo.Servos[ch]);
                        }
                    }
                    break;
                case SlaveType.BLDC:
                    {
                        EcSlave_BLDC _bldc = ecslave as EcSlave_BLDC;
                        for (int ch = 0; ch < _bldc.BLDCCount; ch++)
                        {
                            _bldc.BLDCs[ch].Id = m_BldcCount++;
                            m_BldcItemList.Add(_bldc.BLDCs[ch]);
                        }
                    }
                    break;
                case SlaveType.Inverter:
                    {
                        EcSlave_Inverter _inverter = ecslave as EcSlave_Inverter;
                        for (int ch = 0; ch < _inverter.InverterCount; ch++)
                        {
                            _inverter.Inverters[ch].Id = m_InverterCount++;
                            m_InverterItemList.Add(_inverter.Inverters[ch]);
                        }
                    }
                    break;
                case SlaveType.DI:
                    {
                        EcSlave_DI _di = ecslave as EcSlave_DI;
                        for (int ch = 0; ch < _di.InChannelCount; ch++)
                        {
                            _di.InChannels[ch].Id = m_DiCount++;
                            m_DiItemList.Add(_di.InChannels[ch]);
                        }
                    }
                    break;
                case SlaveType.DIO:
                    {
                        EcSlave_DIO _dio = ecslave as EcSlave_DIO;
                        for (int ch = 0; ch < _dio.InChannelCount; ch++)
                        {
                            _dio.InChannels[ch].Id = m_DiCount++;
                            m_DiItemList.Add(_dio.InChannels[ch]);
                        }
                        for (int ch = 0; ch < _dio.OutChannelCount; ch++)
                        {
                            _dio.OutChannels[ch].Id = m_DoCount++;
                            m_DoItemList.Add(_dio.OutChannels[ch]);
                        }
                    }
                    break;
                case SlaveType.DO:
                    {
                        EcSlave_DO _do = ecslave as EcSlave_DO;
                        for (int ch = 0; ch < _do.OutChannelCount; ch++)
                        {
                            _do.OutChannels[ch].Id = m_DoCount++;
                            m_DoItemList.Add(_do.OutChannels[ch]);
                        }
                    }
                    break;
                case SlaveType.AI:
                    {
                        EcSlave_AI _ai = ecslave as EcSlave_AI;
                        for (int ch = 0; ch < _ai.InChannelCount; ch++)
                        {
                            _ai.InChannels[ch].Id = m_AiCount++;
                            m_AiItemList.Add(_ai.InChannels[ch]);
                        }
                    }
                    break;
                case SlaveType.AIO:
                    {
                        EcSlave_AIO _aio = ecslave as EcSlave_AIO;
                        for (int ch = 0; ch < _aio.InChannelCount; ch++)
                        {
                            _aio.InChannels[ch].Id = m_AiCount++;
                            m_AiItemList.Add(_aio.InChannels[ch]);
                        }
                        for (int ch = 0; ch < _aio.OutChannelCount; ch++)
                        {
                            _aio.OutChannels[ch].Id = m_AoCount++;
                            m_AoItemList.Add(_aio.OutChannels[ch]);
                        }
                    }
                    break;
                case SlaveType.AO:
                    {
                        EcSlave_AO _ao = ecslave as EcSlave_AO;
                        for (int ch = 0; ch < _ao.OutChannelCount; ch++)
                        {
                            _ao.OutChannels[ch].Id = m_AoCount++;
                            m_AoItemList.Add(_ao.OutChannels[ch]);
                        }
                    }
                    break;
                case SlaveType.AP:
                    {
                        EcSlave_AP _ap = ecslave as EcSlave_AP;
                        for (int ch = 0; ch < _ap.ChannelCount; ch++)
                        {
                            _ap.Channels[ch].Id = m_ApCount++;
                            m_ApItemList.Add(_ap.Channels[ch]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void UpdateSlaveInfo(ref EcSlave ecslave)
        {
            int AliasNo = ecslave.AliasNo;
            uint VendorId = ecslave.VendorId;
            int ProductCode = ecslave.ProductCode;

            WMX.Slave slave = WMX.EcCtl.GetSlave(AliasNo, VendorId, ProductCode);

            if (slave != null)
            {
                ecslave.SlaveNo = slave.SlaveNo;
                ecslave.VendorName = slave.VendorName;
                ecslave.ProductName = slave.ProductName;
            }

            m_SlavePairs.Add(ecslave, slave);
        }

        public void Uninitialize()
        {
            m_Uninitialized = true;

            if (m_Simul.Device)
            {
                m_Initialized = false;
            }
            else
            {
                ClearAllNode();

                WMX.Uninitialize();
            }
        }

        private void ClearAllNode()
        {
            for (int i = 0; i < m_DoCount; i++)
            {
                WriteDoAsync(i, false);
            }

            for (int i = 0; i < m_AoCount; i++)
            {
                WriteAoAsync(i, (ushort)0);
            }

            //UpdateIoStatus();
        }

        public void WriteLog(string msg)
        {
            m_Log.TextOut(msg);
        }

        private WMX.Slave_Servo GetSlaveServo(int index)
        {
            if (m_ServoItemList.Count <= index)
                return null;
            EcSlaveItem item = m_ServoItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.Servo)
                return null;

            return slave as WMX.Slave_Servo;
        }
        private WMX.Slave_BLDC GetSlaveBLDC(int index)
        {
            if (m_BldcItemList.Count <= index)
                return null;
            EcSlaveItem item = m_BldcItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.BLDC)
                return null;

            return slave as WMX.Slave_BLDC;
        }
        private WMX.Slave_Inverter GetSlaveInverter(int index)
        {
            if (m_BldcItemList.Count <= index)
                return null;
            EcSlaveItem item = m_InverterItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.Inverter)
                return null;

            return slave as WMX.Slave_Inverter;
        }
        private WMX.Slave_DIO GetSlaveDI(int index)
        {
            if (m_DiItemList.Count <= index)
                return null;
            EcSlaveItem item = m_DiItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.DIO)
                return null;

            return slave as WMX.Slave_DIO;
        }
        private WMX.Slave_DIO GetSlaveDO(int index)
        {
            if (m_DoItemList.Count <= index)
                return null;
            EcSlaveItem item = m_DoItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.DIO)
                return null;

            return slave as WMX.Slave_DIO;
        }
        private WMX.Slave_AIO GetSlaveAI(int index)
        {
            if (m_AiItemList.Count <= index)
                return null;
            EcSlaveItem item = m_AiItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.AIO)
                return null;

            return slave as WMX.Slave_AIO;
        }
        private WMX.Slave_AIO GetSlaveAO(int index)
        {
            if (m_AoItemList.Count <= index)
                return null;
            EcSlaveItem item = m_AoItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.AIO)
                return null;

            return slave as WMX.Slave_AIO;
        }
        private WMX.Slave_AP GetSlaveAP(int index)
        {
            if (m_ApItemList.Count <= index)
                return null;
            EcSlaveItem item = m_ApItemList[index];

            if (WMX.EcCtl.Slaves.Count <= item.SlaveNo || item.SlaveNo < 0)
                return null;
            WMX.Slave slave = WMX.EcCtl.Slaves[item.SlaveNo];

            if (slave.SlaveType != WMX.SlaveType.AP)
                return null;

            return slave as WMX.Slave_AP;
        }
        private WMX.Peer GetPeer(int index)
        {
            WMX.Slave_AP ap = GetSlaveAP(index);
            if (ap == null) return null;

            WMX.Peer peer = ap.Peers[m_ApItemList[index].Channel];

            return peer;
        }
        private bool CheckPeerState(WMX.Peer peer, PeerType type)
        {
            if (peer == null) return false;
            if (peer.PeerType != type) return false;
            if (!peer.IsPaired) return false;
            return true;
        }

        private WMX.Slave GetPair(EcSlave slave)
        {
            return m_SlavePairs[slave];
        }

        private EcSlave GetPair(WMX.Slave slave)
        {
            return m_SlavePairs.FirstOrDefault(key => key.Value == slave).Key;
        }
        #endregion

        #region ICtlDevice Properties
        public bool Initialized
        {
            get { return m_Initialized; }
            set { m_Initialized = value; }
        }

        public bool Uninitializing
        {
            get { return m_Uninitialized; }
            set { m_Uninitialized = value; }
        }

        public ActiveState DeviceState
        {
            get
            {
                return m_ActiveState;
            }
            set
            {
                m_ActiveState = value;
                m_ControllerState = m_ActiveState.ToString();
            }
        }

        public string ControllerState
        {
            get { return m_ControllerState; }
        }
        #endregion

        #region ICtlDevice Methods

        #region DI
        public bool ReadDiSync(int index)
        {
            return ReadDiAsync(index);
        }

        public bool ReadDiAsync(int index)
        {
            WMX.Slave_DIO dio = GetSlaveDI(index);
            if (dio == null) return false;

            return dio.GetDi(m_DiItemList[index].Channel);
        }

        public void WriteDiSync(int index, bool val)
        {
            if (m_Simul.Device)
            {
                WMX.Slave_DIO dio = GetSlaveDI(index);
                if (dio == null) return;

                dio.SetDi(m_DiItemList[index].Channel, val);
            }
        }
        #endregion

        #region DO
        public bool ReadDoSync(int index)
        {
            return ReadDoAsync(index);
        }

        public bool ReadDoAsync(int index)
        {
            WMX.Slave_DIO dio = GetSlaveDO(index);
            if (dio == null) return false;

            return dio.GetDo(m_DoItemList[index].Channel);
        }

        public void WriteDoSync(int index, bool val)
        {
            WriteDoAsync(index, val);
        }

        public void WriteDoAsync(int index, bool val)
        {
            WMX.Slave_DIO dio = GetSlaveDO(index);
            if (dio == null) return;

            dio.SetDo(m_DoItemList[index].Channel, val);
        }
        #endregion

        #region AI
        public short ReadAiSync(int index)
        {
            return ReadAiAsync(index);
        }

        public short ReadAiAsync(int index)
        {
            WMX.Slave_AIO aio = GetSlaveAI(index);
            if (aio == null) return 0;

            return (short)aio.GetAi(m_AiItemList[index].Channel);
        }

        public void WriteAiSync(int index, short val)
        {
            if (m_Simul.Device)
            {
                WMX.Slave_AIO aio = GetSlaveAI(index);
                if (aio == null) return;

                aio.SetAi(m_AiItemList[index].Channel, (ushort)val);
            }
        }
        #endregion

        #region AO
        public ushort ReadAoSync(int index)
        {
            return ReadAoAsync(index);
        }

        public ushort ReadAoAsync(int index)
        {
            WMX.Slave_AIO aio = GetSlaveAO(index);
            if (aio == null) return 0;

            return aio.GetAo(m_AoItemList[index].Channel);
        }

        public void WriteAoSync(int index, ushort val)
        {
            WriteAoAsync(index, val);
        }

        public void WriteAoAsync(int index, ushort val)
        {
            WMX.Slave_AIO aio = GetSlaveAO(index);
            if (aio == null) return;

            aio.SetAo(m_AoItemList[index].Channel, val);
        }
        #endregion
        #endregion

        #region ICtlDevice Undefined
        public event IoStateChangeEventHandler OnIoStateChange;

        public object Read(int index, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object Read(int index, IoType type, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(int index, object val, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(int index, object val, IoType type, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WriteSync(int index, object val, IoType type)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICtlDevice_Adv Methods
        #region Servo Control Methods
        public int ServoOn(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.ServoOn();
        }
        public int ServoOff(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.ServoOff();
        }

        public bool ServoIsOn(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return false;

            return servo.IsOnline && servo.IsOn;
        }
        public bool ServoIsDone(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return false;

            return servo.IsDone;
        }
        public bool ServoGetHomeSwitch(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return false;

            return servo.IsHome;
        }
        public bool ServoGetLimitPSwitch(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return false;

            return servo.IsLimitP;
        }
        public bool ServoGetLimitMSwitch(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return false;

            return servo.IsLimitM;
        }
        public AxisEvent ServoGetAxisState(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return AxisEvent.NoEvent;

            return servo.GetAxisState();
        }

        public int ServoMove_R(int index, double dist, double vel, double acc)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.Move((int)dist, (int)vel, (int)acc, (int)acc, WMX.ServoCtl.ProfileType.Trapezoidal, true);
        }
        public int ServoMove_S(int index, double pos, double vel, double acc)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.Move((int)pos, (int)vel, (int)acc, (int)acc, WMX.ServoCtl.ProfileType.SCurve, false);
        }
        public int ServoMove_T(int index, double pos, double vel, double acc, double dec)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.Move((int)pos, (int)vel, (int)acc, (int)dec, WMX.ServoCtl.ProfileType.Trapezoidal, false);
        }
        public int ServoMove_V(int index, double spd, double acc, double dec)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.Run((int)spd, (int)acc, (int)dec);
        }

        public int ServoHoming(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.Homing();
        }

        public int ServoStop(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.Stop();
        }
        public int ServoEStop(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.EStop();
        }
        public int ServoEStopRelease(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.EStopRelease();
        }


        public bool ServoIsAlarm(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return false;

            return servo.IsAlarm;
        }
        public int ServoAlarmReset(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.AlarmReset();
        }

        public int ServoSetPosition(int index, double pos)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.SetPosition(pos);
        }
        public double ServoGetPosition(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return double.NaN;

            return servo.CurrentPosition;
        }
        public double ServoGetVelocity(int index)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return double.NaN;

            return servo.CurrentVelocity;
        }

        public int ServoSetAxisCommandMode(int index, int mode)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            return servo.SetAxisCommandMode(mode);
        }

        public int ServoSetSync(int index, int slaveindex)
        {
            WMX.Slave_Servo servo = GetSlaveServo(index);
            if (servo == null) return -1;

            WMX.Slave_Servo servo_slave = GetSlaveServo(slaveindex);
            if (servo_slave == null) return -2;

            return servo.SetSync(servo_slave);
        }
        public int ServoUnsync(int slaveindex)
        {
            WMX.Slave_Servo servo_slave = GetSlaveServo(slaveindex);
            if (servo_slave == null) return -1;

            return servo_slave.SetUnsync();
        }
        #endregion

        #region BLDC Control
        public double BldcGetLoadFactor(int index)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return 0;

            return bldc.GetLoadFactor();
        }

        public short BldcGetCurrentRPM(int index)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return 0;

            return bldc.GetCurrentRPM();
        }

        public bool BldcGetIsTurnFw(int index)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return false;

            return bldc.IsRotateFw();
        }

        public bool BldcGetIsTurnBw(int index)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return false;

            return bldc.IsRotateBw();
        }

        public bool BldcGetIsAlarm(int index)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return false;

            return bldc.IsAlarm();
        }

        public void BldcSetRotateFw(int index, bool op)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return;

            bldc.SetRotateFw(op);
        }

        public void BldcSetRotateBw(int index, bool op)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return;

            bldc.SetRotateBw(op);
        }

        public void BldcSetAlarmReset(int index, bool op)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return;

            bldc.SetAlarmReset(op);
        }

        public void BldcSetAccTime(int index, ushort val)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return;

            bldc.SetAccTime(val);
        }

        public void BldcSetDecTime(int index, ushort val)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return;

            bldc.SetDecTime(val);
        }

        public void BldcSetTargetRPM(int index, ushort val)
        {
            WMX.Slave_BLDC bldc = GetSlaveBLDC(index);
            if (bldc == null) return;

            bldc.SetTargetRPM(val);
        }
        #endregion

        #region Inverter Control
        public bool InverterGetIsAlarm(int index)
        {
            WMX.Slave_Inverter inverter = GetSlaveInverter(index);
            if (inverter == null) return false;

            return inverter.IsAlarm();
        }

        public bool InverterGetIsRun(int index)
        {
            WMX.Slave_Inverter inverter = GetSlaveInverter(index);
            if (inverter == null) return false;

            return inverter.IsRun();
        }

        public double InverterGetCurrentFrequency(int index)
        {
            WMX.Slave_Inverter inverter = GetSlaveInverter(index);
            if (inverter == null) return 0;

            return inverter.GetCurrentFrequency();
        }

        public void InverterSetAlarmReset(int index, bool op)
        {
            WMX.Slave_Inverter inverter = GetSlaveInverter(index);
            if (inverter == null) return;

            inverter.SetAlarmResetBit(op);
        }

        public void InverterSetRun(int index, bool op)
        {
            WMX.Slave_Inverter inverter = GetSlaveInverter(index);
            if (inverter == null) return;

            inverter.SetStopBit(!op);
            inverter.SetRunBit(op);
        }

        public void InverterSetTargetFrequency(int index, double val)
        {
            WMX.Slave_Inverter inverter = GetSlaveInverter(index);
            if (inverter == null) return;

            inverter.SetTargetFrequency(val);
        }
        #endregion

        #region AP Control Methods
        public void ApSetPeer(int index, int channel, PeerType type, int peerid)
        {
            WMX.Slave_AP ap = GetSlaveAP(index);
            if (ap == null) return;

            ap.MakePeer(channel, type, peerid);
        }

        public PairingState ApGetPairingState(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (peer == null) return PairingState.Unpaired;

            return peer.PairingState;
        }

        #region DIW
        public short ApDiwReadFlowValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.DIW)) return 0;

            return ((WMX.Peer_DIW)peer).FlowValue;
        }
        public short ApDiwReadPressValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.DIW)) return 0;

            return ((WMX.Peer_DIW)peer).PressValue;
        }
        #endregion
        #region CDA
        public short ApCdaReadFlowValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.CDA)) return 0;

            return ((WMX.Peer_CDA)peer).FlowValue;
        }
        public short ApCdaReadPressValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.CDA)) return 0;

            return ((WMX.Peer_CDA)peer).PressValue;
        }
        #endregion

        #region SmartDamper
        public SmartDamperMode ApSmartDamperReadSmartDamperMode(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return 0;

            return ((WMX.Peer_SmartDamper)peer).PV_CurrentMode;
        }
        public ushort ApSmartDamperReadTargetPressure(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return 0;

            return ((WMX.Peer_SmartDamper)peer).PV_TargetPressure;
        }
        public ushort ApSmartDamperReadTargetPressureHysteresis(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return 0;

            return ((WMX.Peer_SmartDamper)peer).PV_TargetPressureHysteresis;
        }
        public ushort ApSmartDamperReadCurrentValveAngle(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return 0;

            return ((WMX.Peer_SmartDamper)peer).PV_CurrentValveAngle;
        }
        public ushort ApSmartDamperReadCurrentPressure(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return 0;

            return ((WMX.Peer_SmartDamper)peer).PV_CurrentPressure;
        }
        public ushort ApSmartDamperReadAlarmCode(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return 0;

            return ((WMX.Peer_SmartDamper)peer).PV_AlarmCode;
        }
        public bool ApSmartDamperWriteSmartDamperMode(int index, SmartDamperMode mode)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return false;

            ((WMX.Peer_SmartDamper)peer).SV_SmartDamperMode = mode;
            return true;
        }
        public bool ApSmartDamperWriteTargetPressure(int index, ushort value)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return false;

            ((WMX.Peer_SmartDamper)peer).SV_TargetPressure = value;
            return true;
        }
        public bool ApSmartDamperWriteTargetPressureHysteresis(int index, ushort value)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return false;

            ((WMX.Peer_SmartDamper)peer).SV_TargetPressureHysteresis = value;
            return true;
        }
        public bool ApSmartDamperWriteTargetValveAngle(int index, ushort value)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.SmartDamper)) return false;

            ((WMX.Peer_SmartDamper)peer).SV_TargetValveAngle = value;
            return true;
        }
        #endregion

        #region D40A
        public ushort ApD40AReadState(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.D40A)) return 0;

            return ((WMX.Peer_D40A)peer).SensorData;
        }
        #endregion

        #region D4SL
        public bool ApD4SLIsOpened(int index, int channel)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.D4SL)) return false;

            return ((WMX.Peer_D4SL)peer).IsOpened[channel];
        }
        public bool ApD4SLIsLocked(int index, int channel)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.D4SL)) return false;

            return ((WMX.Peer_D4SL)peer).IsLocked[channel];
        }
        public void ApD4SLSetLock(int index, int channel, bool op)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.D4SL)) return;

            ((WMX.Peer_D4SL)peer).Lock[channel] = op;
        }
        public void ApD4SLSetLockAll(int index, bool op)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.D4SL)) return;

            ((WMX.Peer_D4SL)peer).LockAll = op;
        }
        #endregion

        #region LFC
        public ushort ApLfcReadFlowValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LFC)) return 0;

            return ((WMX.Peer_LFC)peer).PV_FlowValue;
        }
        public ushort ApLfcReadPressValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LFC)) return 0;

            return ((WMX.Peer_LFC)peer).PV_PressValue;
        }
        public ushort ApLfcReadCurrentOpenRate(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LFC)) return 0;

            return ((WMX.Peer_LFC)peer).PV_CurrentOpenRate;
        }
        public bool ApLfcWriteTargetOpenRate(int index, ushort value)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LFC)) return false;

            ((WMX.Peer_LFC)peer).SV_TargetOpenRate = value;
            return true;
        }
        #endregion

        #region Manometer
        public short ApManometerReadExhaustValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.Manometer)) return 0;

            return ((WMX.Peer_Manometer)peer).ExhaustValue;
        }
        #endregion

        #region LCT
        public ushort ApLctReadLevel1Value(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LCT)) return 0;

            return ((WMX.Peer_LCT)peer).PV_Level1Value;
        }
        public ushort ApLctReadLevel2Value(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LCT)) return 0;

            return ((WMX.Peer_LCT)peer).PV_Level2Value;
        }
        public ushort ApLctReadConsistenceValue(int index)
        {
            WMX.Peer peer = GetPeer(index);
            if (!CheckPeerState(peer, PeerType.LCT)) return 0;

            return ((WMX.Peer_LCT)peer).PV_ConsistanceValue;
        }
        #endregion
        #endregion
        #endregion
    }

    public class SeqInitSlave : XSeqFunction
    {
        #region Fields
        private IoNodeMovensysEcMaster m_EcMaster;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public SeqInitSlave(IoNodeMovensysEcMaster master)
        {
            m_EcMaster = master;
        }
        #endregion

        #region Methods
        #endregion
    }
}
