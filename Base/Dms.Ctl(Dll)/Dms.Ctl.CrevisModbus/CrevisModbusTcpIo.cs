using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Threading;

namespace Dms.Ctl
{
    public class CrevisModbusTcpIo : ICtlDevice
    {
        #region Singleton
        public static readonly CrevisModbusTcpIo Instance = new CrevisModbusTcpIo();
        #endregion

        #region Fields
        private bool m_Initialized = false;
        private bool m_Uninitializing = false;

        private ActiveState m_DeviceState;

        //private List<ActiveState> m_DeviveStateList;

        private string m_ControllerState = "";
        private Simul m_Simul;
        private XLog m_BrModbusLog = new XLog("BrModbusLog", XLog.LogStampType.UseStamp);

        private int m_MasterCount;
        private int m_DiCount = 0;
        private int m_DoCount = 0;
        private int m_AiCount = 0;
        private int m_AoCount = 0;

        private List<IoTerminal> m_X20TeminalTypes = null;
        private List<IoNode> m_Nodes;
        private List<MasterCrevis> m_Masters = new List<MasterCrevis>();
        private List<string> m_MasterIP = new List<string>();
        private List<ushort> m_MasterPortNo = new List<ushort>();
        private List<bool> m_MasterBCexceptionRegisterd = new List<bool>();
        //Buffer for sequence
        private bool[] m_DiStatus = null;
        private int[] m_AiStatus = null;
        private bool[] m_DoCmdBuf = null;
        private int[] m_AoCmdBuf = null;

        //Watch를 위한 변수들
        private ushort m_PollRefresh = 1;
        private int m_WatchCycleTime = 10;
        private WatchMode m_WatchMode = WatchMode.FormTimer;
        private List<ThreadCrevisModbusWatch> m_ThreadWatchs = null;
        private List<System.Windows.Forms.Timer> m_FormTimers = null;
        //private List<System.Timers.Timer> m_SystemTimers = null;
        //private List<System.Threading.Timer> m_ThreadingTimers = null;

        private ushort m_ExcCode = 0;
        #endregion

        #region Properties
        public Simul Simul
        {
            get { return m_Simul; }
        }
        public List<IoNode> Nodes
        {
            get { return m_Nodes; }
        }
        public List<MasterCrevis> Masters
        {
            get { return m_Masters; }
        }
        public List<string> IP
        {
            get { return m_MasterIP; }
        }
        public ushort ExcCode
        {
            get { return m_ExcCode; }
            set { m_ExcCode = value; }
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
        public int WatchUpdateCycle
        {
            get { return m_WatchCycleTime; }
            set { m_WatchCycleTime = value; }
        }
        public WatchMode WatchUpdateMode
        {
            get { return m_WatchMode; }
            set { m_WatchMode = value; }
        }
        #endregion

        #region Constructor
        private CrevisModbusTcpIo()
        {
            m_Simul = AppConfig.Instance.Simul;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create new master, Default PortNo : 502
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
        public bool AddMaster(string ip, ushort portNo)
        {
            if (!m_Simul.IoController)
            {
                // Create new master instance
                // Default PortNo = 502;
                // new MasterBR(ip, portNo); 로 생성하면 생성자 내부에서 connect를 호출하게 되므로 비추
                //MasterBR master = new MasterBR(ip, portNo);
                MasterCrevis master = new MasterCrevis();
                master.MasterCrevisInit();
                m_MasterIP.Add(ip);
                m_MasterPortNo.Add(portNo);
                m_Masters.Add(master);
                m_MasterCount++;
               // m_MasterBCexceptionRegisterd.Add(false);
                //m_DeviveStateList.Add(ActiveState.UnKnown);
            }

            return true;
        }

        public void SetLog(string text)
        {
            string logText = "";

            logText = string.Format("[{0}]{1}{2}", "BrModbusTcpIo", '\t', text);
            m_BrModbusLog.TextOut(logText);
        }

        private bool CreateMasterInfo(IoDefines iodefines)
        {
            if (iodefines.BusType != FieldBusType.CrevisModbusTcp)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < iodefines.Container.Count; i++)
                {
                    IoNodeCrevisModbus node = (IoNodeCrevisModbus)iodefines.Container[i];
                    AddMaster(node.IpAddress, node.PortNo);
                }

                return true;
            }
        }

        public DmsErrors Intialize(IoDefines iodefines)
        {
            bool ok = true;
            m_Nodes = new List<IoNode>();

            ok &= CreateMasterInfo(iodefines);
            CreateModuleInfo(iodefines);
            CreateBuffer();

            if (ok)
            {
                ok &= Intialize() == DmsErrors.Success;
            }

            return (ok == true) ? DmsErrors.Success : DmsErrors.InternalError;
        }

        public DmsErrors Intialize()
        {
            //m_X20TeminalTypes = IoDefines.GetTerminalTypes(Maker.BR);
           

            bool ok = true;

            for (int i = 0; i < m_MasterCount; i++)
            {
                ok &= Connect(i);
            }

            if (ok)
            {
                ////Master로 부터 Terminal 정보를 읽어와서 Io 구성정보를 만듬
                //CreateModuleInfo();

                //Sequece에서 참조할 Memory Buffer 생성
               

                //Update 구조 생성
                if (m_Simul.IoController)
                {
                    SetLog("BrModbusTcpIo Start...");
                    MessageBox.Show(string.Format("System run in {0} Simulation mode!!", this.GetType().Name));
                }
                else
                {
                    switch (m_WatchMode)
                    {
                        case WatchMode.Thread:
                            { // Modbus Master에 Connect 하고 나서 Watch Thread Start 하기 까지 약간 Delay 필요함
                                Thread.Sleep(500);

                                m_ThreadWatchs = new List<ThreadCrevisModbusWatch>();
                                for (int i = 0; i < m_MasterCount; i++)
                                {
                                    ThreadCrevisModbusWatch thread = new ThreadCrevisModbusWatch(m_WatchCycleTime, this, i);
                                    thread.Start();
                                    m_ThreadWatchs.Add(thread);
                                }
                            }
                            break;
                        case WatchMode.FormTimer:
                        case WatchMode.SystemTimer:
                        case WatchMode.ThreadingTimer:
                            {
                                //m_FormTimers = new List<System.Windows.Forms.Timer>();
                                //for (int i = 0; i < m_MasterCount; i++)
                                //{
                                //    System.Windows.Forms.Timer formTimer = new System.Windows.Forms.Timer();
                                //    formTimer.Tag = i;
                                //    formTimer.Interval = m_WatchCycleTime;
                                //    formTimer.Tick += new EventHandler(FormTimerTick);
                                //    formTimer.Enabled = true;
                                //    m_FormTimers.Add(formTimer);
                                //}
                            }
                            break;
                    }
                }

                this.DeviceState = ActiveState.Run;
            }

            m_Initialized = ok;

            return (ok == true) ? DmsErrors.Success : DmsErrors.InternalError;
        }

        public void Uninitialize()
        {
            if (true == this.Initialized)
            {
                m_Uninitializing = true;

                if (!m_Simul.IoController)
                {
                    switch (m_WatchMode)
                    {
                        case WatchMode.Thread:
                            {
                                foreach (ThreadCrevisModbusWatch thread in m_ThreadWatchs)
                                {
                                    if (thread != null) thread.Pause();
                                }
                            }
                            break;
                        case WatchMode.FormTimer:
                        case WatchMode.SystemTimer:
                        case WatchMode.ThreadingTimer:
                            {
                                foreach (System.Windows.Forms.Timer timer in m_FormTimers)
                                {
                                    if (timer != null) timer.Dispose();
                                }
                            }
                            break;
                    }

                    Disconnect();
                }

                SetLog("CrevisModbusTcpIo End...");
            }
        }

        public bool[] Connect()
        {
            bool[] ok = new bool[m_MasterCount];
            for (int i = 0; i < m_MasterCount; i++)
            {
                ok[i] = Connect(i);
            }

            return ok;
        }

        public bool Connect(int masterId)
        {
            try
            {
                bool connected = true;
                int i = masterId;

                //for (int i = 0; i < m_MasterCount; i++)
                {
                    MasterCrevis masterCrevis = m_Masters[i];

                    if (masterCrevis.IsConnected == false)
                    {
                        //if (m_MasterBCexceptionRegisterd[masterId] == false)
                        //{
                        //    m_MasterBCexceptionRegisterd[masterId] = true;
                        //    masterBR.OnBCexception += new ModbusTCPBR.MasterBR.BCexception(OnBCexception);
                        //}

                        masterCrevis.Connect(m_MasterIP[i], m_MasterPortNo[i]);

                        //masterBR.BCinfo.com_x2x = (ushort)BrX2XCycleTime.C7_0_5ms;
                        //masterBR.PollRefresh = m_PollRefresh;

                        //SetWatchDogMode(i, BrModbusWatchDogMode.TriggeredEach);
                    }
                    connected &= masterCrevis.IsConnected;
                }
                return connected;
            }
            catch (Exception err)
            {
                //SetLog("BrModbusTcpIo Connection failed!");
                return false;
            }
        }

        public void Disconnect()
        {
            //if (m_ThreadWatch.IsStarted) m_ThreadWatch.Pause();

            for (int i = 0; i < m_MasterCount; i++)
            {
                Disconnect(i);
            }

            SetLog("CrevisModbusTcpIo DisConnection");
        }

        public void Disconnect(int masterId)
        {
            //m_Masters[masterId].Disconnect();
            //SetLog(string.Format("BrModbusTcpIo {0} DisConnection", masterId.ToString()));
        }

        #region Definitions for Modbus Function
        private const int CMD_ADDR_WATCHDOG_MODE = 0x1043;
        public enum BrModbusWatchDogMode
        {
            Disabled = 0xC0,
            TriggeredEach = 0xC1,
            TriggeredWrite = 0xC2
        }

        public enum BrModbusWatchDogStatus
        {
            NotInOperation = 0xC0,
            Armed = 0xC1,
            TimeOut = 0xC2
        }

        //0xC0 4 ms     Max. 253 I/O modules, max. 1400 bytes of sync. data
        //0xC1 3.5 ms   Max. 253 I/O modules, max. 1150 bytes of sync. data
        //0xC2 3 ms     Max. 253 I/O modules, max. 900 bytes of sync. data
        //0cX3 2.5 ms   Max. 200 I/O modules, max. 800 bytes of sync. data
        //0xC4 2 ms     Max. 200 I/O modules, max. 500 bytes of sync. data
        //0xC5 1.5 ms   Max. 100 I/O modules, max. 450 bytes of sync. data
        //0xC6 1 ms     Max. 80 I/O modules, max. 300 bytes of sync. data
        //0xC7 0.5 ms   Max. 40 I/O modules, max. 120 bytes of sync. data
        public enum BrX2XCycleTime
        {
            C0_4_0ms = 0xC0,
            C1_3_5ms = 0xC1,
            C2_3_0ms = 0xC2,
            C3_2_5ms = 0xC3,
            C4_2_0ms = 0xC4,
            C5_1_5ms = 0xC5,
            C6_1_0ms = 0xC6,
            C7_0_5ms = 0xC7,
        }

        // misc_status
        public enum ModbusBrOperationStatus
        {
            Already = 0x0001,
            MasterConnection = 0x0002,
            InitializationActive = 0x0004,
            Wait = 0x0008,
        }

        // misc_status_error
        public enum BusControllerErrorStaus
        {
            WatchdogTimeOut = 0x0001,
            FlashMemoryReadError = 0x0002,
            FaultyModule = 0x0004,
            MissingModule = 0x0008,
            IncorrectModule = 0x0010,
            FaultyIoModuleConfigData = 0x0020,
            IpAddressConflict = 0x0040,
        }
        #endregion

        public void SetWatchDogMode(int masterId, BrModbusWatchDogMode mode)
        {
            //jemoon : vesion up 되면서 MBmaster가 없어져 버렸다.
            //m_Masters[masterId].MBmaster.WriteSingleRegister(0, CMD_ADDR_WATCHDOG_MODE, new byte[] { 0x00, (byte)mode });
            //m_Masters[masterId].BCinfo.watchdog_threshold = 2000; //2secs
            //m_Masters[masterId].BCinfo.watchdog_reset();
            //m_Masters[masterId].BCinfo.watchdog_mode = (byte)mode;
            //m_Masters[masterId].BCinfo.watchdog_threshold = 2000; //2secs
            //m_Masters[masterId].BCinfo.watchdog_reset();
        }

        public bool[] WatchDogReset()
        {
            bool[] ok = new bool[m_MasterCount];
            //for (int i = 0; i < m_Masters.Count; i++)
            //{
            //    ok[i] = WatchDogReset(i);
            //}

            return ok;
        }

        public bool WatchDogReset(int masterId)
        {
            try
            {
                bool watchdogReset = true;
                int i = masterId;

                //for (int i = 0; i < m_Masters.Count; i++)
                //{
                //    if (m_Masters[i].IsConnected == false) watchdogReset = false;
                //    else if ((m_Masters[i].BCinfo.misc_status_error & 0x0001) == 0x0001)
                //    {
                //        m_Masters[i].BCinfo.watchdog_reset();
                //    }
                //}

                return watchdogReset;
            }
            catch
            {
                return false;
            }
        }

        // Bus Coupler에서 구성된 Module 정보를 읽어 온다.
        //private void CreateModuleInfo()
        //{
        //    if (m_Simul.IoController)
        //    {
        //        //Simulation Mode 이면 가상의 Node를 한개 잡는다.
        //        //m_Nodes.Clear();
        //        //m_Nodes.Add(new IoNode());
        //        //m_MasterCount = 1;

        //        m_DiCount = 512;
        //        m_DoCount = 512;
        //        m_AiCount = 128;
        //        m_AoCount = 128;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < m_MasterCount; i++)
        //        {
        //            m_Nodes.Add(new IoNode());
        //        }

        //        int totalModuleId = 0;
        //        for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //        {
        //            m_Nodes[nodeId].Id = nodeId;

        //            // 구성된 모듈의 정보를 가져온다.
        //           // ModbusTCPBR.MasterBR_MDinfo[] moduleInfos = m_Masters[nodeId].MDinfo;

        //            //if (moduleInfos != null)
        //            {
        //                totalModuleId = -1; // 만약 Node가 바뀌어도 ModuleId는 그대로 증가한다면 초기화 금지
        //                int moduleInfosCount = moduleInfos.Length;
        //                for (int moduleId = 0; moduleId < moduleInfosCount; moduleId++)
        //                {
        //                    totalModuleId++; // Power module은 Skip하지만 BR Modbus에서 Id는 할당되어야 한다.
        //                    IoTerminal terminal = GetTerminal(moduleInfos[moduleId]);
        //                    if (terminal != null)
        //                    {
        //                        // Terminal 정보 생성
        //                        #region Make Terminal Info
        //                        terminal.Id = totalModuleId;
        //                        #endregion

        //                        // 각 Channel의 Id는 Io Type별 전체 서수와 맞아야 한다.
        //                        #region Make Channel Info
        //                        int channelCount = terminal.ChannelCount;
        //                        for (int id = 0; id < channelCount; id++)
        //                        {
        //                            IoItem io = terminal.Channels[id];
        //                            io.Node = nodeId;
        //                            io.Terminal = terminal.Id;
        //                            switch (io.IoType)
        //                            {
        //                                case IoType.DI:
        //                                    io.Id = m_DiCount++;
        //                                    break;
        //                                case IoType.DO:
        //                                    io.Id = m_DoCount++;
        //                                    break;
        //                                case IoType.AI:
        //                                    io.Id = m_AiCount++;
        //                                    break;
        //                                case IoType.AO:
        //                                    io.Id = m_AoCount++;
        //                                    break;
        //                            }
        //                        }
        //                        #endregion

        //                        m_Nodes[nodeId].Terminals.Add(terminal);
        //                    }
        //                }
        //            }

        //            //m_Nodes[nodeId].UpdateIoCountByType();
        //        }
        //    }
        //}

        private void CreateModuleInfo(IoDefines ioDefines)
        {
            if (m_Simul.Melsec)
            {
                ////Simulation Mode 이면 가상의 Node를 한개 잡는다.
                //m_ModuleConfig.Container.Add(new IoNode());
                //m_MasterCount = 1;

                m_DiCount = 1024;
                m_DoCount = 1024;
                m_AiCount = 512;
                m_AoCount = 512;
            }
            else    //Io define 정보를 이용해서 Crevis 구성정보를 만들어 내야 한다.        
            {
                for (int nodeId = 0; nodeId < ioDefines.Count; nodeId++)
                {
                    IoNodeCrevisModbus node = new IoNodeCrevisModbus();
                    IoNodeCrevisModbus definedNode = ioDefines.Container[nodeId] as IoNodeCrevisModbus;


                    m_Nodes.Add(definedNode);

                    //CClinkStaionInfo nextInfo = new CClinkStaionInfo();

                    //nextInfo.AddressRx = definedNode.AddressRx;
                    //nextInfo.AddressRy = definedNode.AddressRy;
                    //nextInfo.AddressRWr = definedNode.AddressRWr;
                    //nextInfo.AddressRWw = definedNode.AddressRWw;

                    //int totalIoIndex = 0;
                    //int totalTeminalId = 0;
                    int terminalCount = definedNode.Terminals.Count;
                    for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                    {
                        IoTerminal definedTerminal = definedNode.Terminals[terminalId];
                        //CClinkStation station = definedTerminal.CreateObject() as CClinkStation;
                        //station.Info.MasterNo = nodeId + 1;



                        int channelCount = definedTerminal.ChannelCount;
                        for (int id = 0; id < channelCount; id++)
                        {
                            IoItem io = definedTerminal.Channels[id];
                            io.Node = nodeId;
                            io.Terminal = definedTerminal.Id;
                            switch (io.IoType)
                            {
                                case IoType.DI:
                                    io.Id = m_DiCount++;
                                    break;
                                case IoType.DO:
                                    io.Id = m_DoCount++;
                                    break;
                                case IoType.AI:
                                    io.Id = m_AiCount++;
                                    break;
                                case IoType.AO:
                                    io.Id = m_AoCount++;
                                    break;
                            }
                        }
                        #endregion



                        //m_Nodes[nodeId].Terminals.Add(definedTerminal);


                        //if (definedTerminal.ProductMaker == Maker.CrevisCCLink)
                        //{
                        //    station.Info.StationId = terminalId;
                        //    station.Info.StationNo = terminalId + 1;

                        //    MakeStationInfo(definedTerminal, ref station, ref nextInfo);

                        //    station.Id = terminalId;
                        //}
                        //else if (definedTerminal.ProductMaker == Maker.CrevisCCLink)
                        //{
                        //    MakeCrevisStationInfo(definedTerminal, ref station, ref nextInfo);

                        //}

                        //station.CreateChannels();
                        //IoTerminal terminal = station as IoTerminal;
                        //MakeTerminalInfo(ref terminal);
                        //node.Terminals.Add(station);
                    }

                    
                    //m_ModuleConfig.Container.Add(node);

                    //m_SeqInitStation.Add(new SeqInitStation(m_Masters[nodeId], node));
                }
            }
        }
       


        private void CreateBuffer()
        {
            // 이 함수는 필히 CreateModuleInfo() 이후에 Call
            m_DiStatus = new bool[m_DiCount];
            m_AiStatus = new int[m_AiCount];

            m_DoCmdBuf = new bool[m_DoCount];
            m_AoCmdBuf = new int[m_AoCount];
        }

        //BR Modbus 에서는 사용하지 않음
        private void OnDeviceNotification()
        {
            // BR Modbus에서는 event driven은 무리가 있음
            // Fire event...
            IoStateChangeEventHandler eHandle = OnIoStateChange;
            if (eHandle != null)
            {
                OnIoStateChange(this, new IoStateEventArgs());
            }
        }

        // 논리적 Io index로 부터 Module 구성정보를 가져온다
        // 이렇게 되어 있으면 마지막 Id에 대해서는 성능저하 예상됨
        // jemoon : Test 결과 전체 Poll refresh 하는 것보다 아래 검색 시간이 더 많이 소요됨
        // 결론 : 아래 방식은 사용하지 않음
        //#region 사용하지 않음
        //protected IoConfigInfo GetIoConfigInfo(IoType ioType, int index)
        //{
        //    IoConfigInfo info = null; //null 이면 구성 없음

        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        List<IoTerminal> terminals = m_Nodes[nodeId].Terminals;
        //        int terminalCount = terminals.Count;
        //        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //        {
        //            IoTerminal terminal = terminals[terminalId];
        //            if (terminal.IoType != ioType) continue;

        //            int channelCount = terminal.ChannelCount;
        //            for (int id = 0; id < channelCount; id++)
        //            {
        //                IoItem io = terminal.Channels[id];
        //                if (io.Id == index)
        //                {
        //                    info = new IoConfigInfo();
        //                    info.NodeId = (byte)nodeId;
        //                    info.ModuleId = (byte)(terminal.Id);
        //                    info.ChannelId = (byte)(io.Channel);

        //                    return info;
        //                }
        //            }
        //        }
        //    }

        //    return info;
        //}
        //#endregion

        // BR Master로 부터는 Terminal 의 Product Id만 제공받는다.
        // IoDefine에 정의되어 있는 Terminal List에서 Product Id를 비교해서 
        // 완전한 Terminal의 Instance를 생성한다.
        //protected IoTerminal GetTerminal(ModbusTCPBR.MasterBR_MDinfo moduleInfo)
        //{
        //    IoTerminal terminal = null; // null 이면 정보없음
        //    int terminalInfoCount = m_X20TeminalTypes.Count;

        //    for (int i = 0; i < terminalInfoCount; i++)
        //    {
        //        if (m_X20TeminalTypes[i].ProductId == moduleInfo.id)
        //        {
        //            terminal = m_X20TeminalTypes[i].CreateObject();
        //            terminal.CreateChannels();
        //            break;
        //        }
        //    }

        //    return terminal;
        //}

        // ------------------------------------------------------------------------
        // BC0087 controller exception
        private void OnBCexception(ushort reason)
        {
            // 예외처리 기준이 필요하다!     
            this.DeviceState = ActiveState.Stop;

            m_ExcCode = reason;

            //switch (reason)
            //{
            //    case MasterBR.excWatchdog:
            //        {
            //            SetLog("Watchdog Exception");
            //        }
            //        break;

            //    case MasterBR.excConnection:
            //        {
            //            SetLog("Connection Exception");
            //        }
            //        break;

            //    case MasterBR.excTimeout:
            //        {
            //            SetLog("Time Out Exception");
            //        }
            //        break;

            //    case MasterBR.excDataEmptyAnswer:
            //        {
            //            SetLog("Data Empty Answer Exception");
            //        }
            //        break;

            //    case MasterBR.excDataRange:
            //        {
            //            SetLog("Data Range Exception");
            //        }
            //        break;

            //    case MasterBR.excDataSize:
            //        {
            //            SetLog("Data Size Exception");
            //        }
            //        break;

            //    case MasterBR.excNoAnaInData:
            //        {
            //            SetLog("No Analog In Exception");
            //        }
            //        break;

            //    case MasterBR.excNoAnaOutData:
            //        {
            //            SetLog("No Analog Ot Exception");
            //        }
            //        break;

            //    case MasterBR.excNoDigInData:
            //        {
            //            SetLog("No Digital In Exception");
            //        }
            //        break;

            //    case MasterBR.excNoDigOutData:
            //        {
            //            SetLog("No Digital Out Exception");
            //        }
            //        break;

            //    case MasterBR.excNoModule:
            //        {
            //            SetLog("No Module Exception");
            //        }
            //        break;

            //    case MasterBR.excUnhandled:
            //        {
            //            SetLog("Unhandled Exception");
            //        }
            //        break;

            //    case MasterBR.excWrongEthernetFormat:
            //        {
            //            SetLog("Wrong Ethernet Format Exception");
            //        }
            //        break;

            //    case MasterBR.excWrongRegData:
            //        {
            //            SetLog("Wrong Register Data Exception");
            //        }
            //        break;
            //}
        }
       

        #region ICtlDevice 멤버
        public string ControllerState
        {
            get { return m_ControllerState; }
        }

        public ActiveState DeviceState
        {
            get
            {
                return m_DeviceState;
            }
            set
            {
                m_DeviceState = value;
                m_ControllerState = m_DeviceState.ToString();
            }
        }

        public bool Initialized
        {
            get
            {
                return m_Initialized;
            }
            set
            {
                m_Initialized = value;
            }
        }

        public bool Uninitializing
        {
            get
            {
                return m_Uninitializing;
            }
            set
            {
                m_Uninitializing = value;
            }
        }

        public event IoStateChangeEventHandler OnIoStateChange;

        public short ReadAiAsync(int index)
        {
            // ModbusTCPBR에서 이미 Poll Refresh 되고 있음
            return (short)m_AiStatus[index];
        }

        public ushort ReadAoAsync(int index)
        {
            // ModbusTCPBR에서 이미 Poll Refresh 되고 있음
            return (ushort)m_AoCmdBuf[index];
        }

        public bool ReadDiAsync(int index)
        {
            // ModbusTCPBR에서 이미 Poll Refresh 되고 있음
            return m_DiStatus[index];
        }

        public bool ReadDoAsync(int index)
        {
            // ModbusTCPBR에서 이미 Poll Refresh 되고 있음
            return m_DoCmdBuf[index];
        }

        public void WriteAoAsync(int index, ushort val)
        {
            m_AoCmdBuf[index] = val;
        }

        public void WriteDoAsync(int index, bool val)
        {
            m_DoCmdBuf[index] = val;
        }

        public short ReadAiSync(int index)
        {
            return (short)m_AiStatus[index];

            //if (m_Simul.IoController)
            //{
            //    return (short)m_AiStatus[index];
            //}
            //else
            //{
            //    short value = 0;

            //    IoConfigInfo io = GetIoConfigInfo(IoType.AI, index);
            //    MasterBR master = m_Masters[io.NodeId];
            //    if (master.IsConnected)
            //    {
            //        int[] values = master.ReadAnalogInputs(io.ModuleId, io.ChannelId, 1);
            //        if (values != null)
            //        {
            //            value = (short)values[0];
            //        }
            //    }

            //    return value;
            //}
        }

        public ushort ReadAoSync(int index)
        {
            //Br Modbus 는 Read output State 는 지원하지 않음
            return (ushort)m_AoCmdBuf[index];
        }

        public bool ReadDiSync(int index)
        {
            return m_DiStatus[index];

            //if (m_Simul.IoController)
            //{
            //    return m_DiStatus[index];
            //}
            //else
            //{
            //    bool value = false;

            //    IoConfigInfo io = GetIoConfigInfo(IoType.DI, index);
            //    MasterBR master = m_Masters[io.NodeId];
            //    if (master.IsConnected)
            //    {
            //        bool[] values = master.ReadDigitalInputs(io.ModuleId, io.ChannelId, 1);
            //        if (values != null)
            //        {
            //            value = (bool)values[0];
            //        }
            //    }

            //    return value;
            //}
        }

        public bool ReadDoSync(int index)
        {
            //Br Modbus 는 Read output State 는 지원하지 않음
            return m_DoCmdBuf[index];
        }

        public void WriteAiSync(int index, short val)
        {
            //Br Modbus 는 write input State 는 지원하지 않음
            if (m_Simul.IoController)
            {
                m_AiStatus[index] = val;
            }
        }

        public void WriteAoSync(int index, ushort val)
        {
            m_AoCmdBuf[index] = val;

            //IoConfigInfo io = GetIoConfigInfo(IoType.AO, index);
            //MasterBR master = m_Masters[io.NodeId];
            //if (!m_Simul.IoController && master.IsConnected)
            //{
            //    master.WriteAnalogOutputs(io.ModuleId, io.ChannelId, (new int[] { val }));
            //}
        }

        public void WriteDiSync(int index, bool val)
        {
            //Br Modbus 는 write input State 는 지원하지 않음
            if (m_Simul.IoController)
            {
                m_DiStatus[index] = val;
            }
        }

        public void WriteDoSync(int index, bool val)
        {
            m_DoCmdBuf[index] = val;

            //IoConfigInfo io = GetIoConfigInfo(IoType.DO, index);
            //MasterBR master = m_Masters[io.NodeId];
            //if (!m_Simul.IoController && master.IsConnected)
            //{
            //    master.WriteDigitalOutputs(io.ModuleId, io.ChannelId, (new bool[] { val }));
            //}
        }
        #endregion

        //private void FormTimerTick(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.Timer formTimer = (System.Windows.Forms.Timer)sender;
        //    int id = (int)formTimer.Tag;

        //    UpdateIoStatus(id);
        //}

        //private void SystemTimerTick(object source, System.Timers.ElapsedEventArgs e)
        //{
        //}

        //private void ThreadingTimerTick(Object stateInfo)
        //{
        //}

        //public void UpdateIoStatus()
        //{
        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        UpdateIoStatus(nodeId);
        //    }
        //}

        public void UpdateIoStatus(int masterId)
        {
            try
            {
                if (DeviceState != ActiveState.Run) return;

                int nodeId = masterId;
                int terminals = m_Nodes[nodeId].Terminals.Count;

                for (int i = 0; i < terminals; i++)
                {
                    if (DeviceState != ActiveState.Run) return;

                    IoTerminal terminal = m_Nodes[nodeId].Terminals[i];

                    switch (terminal.IoType)
                    {
                        case IoType.DI:
                            if (DeviceState != ActiveState.Run) return;
                            {
                                bool[] values = m_Masters[nodeId].ReadDigitalInputs((byte)terminal.Id, (byte)terminal.ChannelCount);
                                Array.Copy(values, 0, m_DiStatus, terminal.Channels[0].Id, terminal.ChannelCount);
                            }
                            break;
                        case IoType.DO:
                            if (DeviceState != ActiveState.Run) return;
                            {
                                bool[] values = new bool[terminal.ChannelCount];
                                Array.Copy(m_DoCmdBuf, terminal.Channels[0].Id, values, 0, terminal.ChannelCount);

                                m_Masters[nodeId].WriteDigitalOutputs((byte)terminal.IdByIoType, values);
                            }
                            break;
                        case IoType.AI:
                            if (DeviceState != ActiveState.Run) return;
                            {
                                byte count = (byte)terminal.ChannelCount;

                                int[] values = m_Masters[nodeId].ReadAnalogInputs((byte)terminal.IdByIoType, (byte)terminal.ChannelCount);
                                Array.Copy(values, 0, m_AiStatus, terminal.Channels[0].Id, count);
                            }
                            break;
                        case IoType.AO:
                            if (DeviceState != ActiveState.Run) return;
                            {
                                int[] values = new int[terminal.ChannelCount];
                                Array.Copy(m_AoCmdBuf, terminal.Channels[0].Id, values, 0, terminal.ChannelCount);
                                m_Masters[nodeId].WriteAnalogOutputs((byte)terminal.IdByIoType, values);
                            }
                            break;
                    }
                }
            }
            catch
            {
                SetLog("UpdateIoStatus Exception!!!");
            }
        }


       

        //public void ActiveStateMonitor(int masterId)
        //{
        //    if (m_ExcCode == MasterBR.excConnection || m_ExcCode == MasterBR.excTimeout)
        //    //|| m_ExcCode == MasterBR.excDataEmptyAnswer || m_ExcCode == MasterBR.excUnhandled)
        //    {
        //        if (Connect(masterId) == true)
        //        {
        //            m_ExcCode = 0;

        //            this.DeviceState = ActiveState.Run;
        //        }
        //    }

        //    if (m_ExcCode == MasterBR.excWatchdog)
        //    {
        //        if (WatchDogReset(masterId) == true)
        //        {
        //            m_ExcCode = 0;
        //            this.DeviceState = ActiveState.Run;
        //        }
        //    }
        //}

        #region ICtlDevice 멤버


        public object Read(int index, IoType type, int group, int dataType, int node)
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

        public object Read(int index, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(int index, object val, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
