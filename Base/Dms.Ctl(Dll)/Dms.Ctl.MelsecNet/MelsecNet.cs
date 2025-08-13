///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.18
// Author       : jemoon
// Description  : Melsec Net Controller
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Diagnostics;
using Microsoft.VisualBasic;
using Dms.Common;
using Dms.Util.IODefine;
using System.Threading;

namespace Dms.Ctl
{
    public class MelsecNet : ICtlDevice
    {
        #region Singleton
        public static readonly MelsecNet Instance = new MelsecNet();
        #endregion

        #region Fields
        private Simul m_Simul;
        private bool m_Initialized = false;
        private bool m_Uninitializing = false;
        private ActiveState m_ActiveState = ActiveState.UnKnown;
        private string m_ControllerState = "";
        private List<MelsecMaster> m_Masters = new List<MelsecMaster>();
        private IoDefines m_ModuleConfig = new IoDefines(FieldBusType.MitsubishiMelsecNet);
        private int m_MasterCount;
        private int m_DiCount = 0;
        private int m_DoCount = 0;
        private int m_AiCount = 0;
        private int m_AoCount = 0;

        //Buffer for sequence
        private bool[] m_DiStatus = null;
        private int[] m_AiStatus = null;
        private bool[] m_DoCmdBuf = null;
        private int[] m_AoCmdBuf = null;

        //Watch를 위한 변수들
        private int m_WatchCycleTime = 10;
        private WatchMode m_WatchMode = WatchMode.FormTimer;
        private List<ThreadMelsecNetWatch> m_ThreadWatchs = null;
        private List<System.Windows.Forms.Timer> m_FormTimers = null;
        //private System.Timers.Timer m_SystemTimer = null;
        //private System.Threading.Timer m_ThreadingTimer = null;

        //for test
        //private uint m_OldTick = 0;
        #endregion

        #region Properties
        public Simul Simul
        {
            get { return m_Simul; }
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

        public bool IsAdvDevice { get { return false; } }
        #endregion

        #region Constructor
        private MelsecNet()
        {
            m_Simul = AppConfig.Instance.Simul;
        }
        #endregion

        #region Event
        #endregion

        #region Methods
        private bool AddMaster(IoNodeMelsec node)
        {
            MelsecMaster master = new MelsecMaster(node);
            master.Simulate = m_Simul.Melsec;
            m_Masters.Add(master);
            m_MasterCount = m_Masters.Count;

            return true;
        }

        private bool CreateMasterInfo(IoDefines iodefines)
        {
            if (iodefines.BusType != FieldBusType.MitsubishiMelsecNet)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < iodefines.Container.Count; i++)
                {
                    IoNodeMelsec node = (IoNodeMelsec)iodefines.Container[i];
                    AddMaster(node);
                }

                return true;
            }
        }

        private bool OpenMasters()
        {
            bool ok = true;
            for (int i = 0; i < m_MasterCount; i++)
            {
                MelsecMaster master = m_Masters[i];
                ok &= master.Open();
            }

            return ok;
        }


        public DmsErrors Initialize(IoDefines iodefines)
        {
            bool ok = true;

            ok &= CreateMasterInfo(iodefines);
            ok &= OpenMasters();

            if (ok)
            {
                //Terminal 정보를 읽어와서 Io 구성정보를 만듬
                CreateModuleInfo(iodefines);

                //Io 구성정보로부터 Update에서 사용할 Melsec device info를 만듬
                //CreateMelsecDeviceInfo(m_ModuleConfig);

                //Sequece에서 참조할 Memory Buffer 생성
                CreateBuffer();

                //Update 구조 생성
                if (!m_Simul.Melsec)
                {
                    switch (m_WatchMode)
                    {
                        case WatchMode.Thread:
                            {
                                m_ThreadWatchs = new List<ThreadMelsecNetWatch>();
                                for (int i = 0; i < m_MasterCount; i++)
                                {
                                    ThreadMelsecNetWatch thread = new ThreadMelsecNetWatch(m_WatchCycleTime, this, i);
                                    thread.Start();
                                    m_ThreadWatchs.Add(thread);
                                }
                            }
                            break;
                        case WatchMode.FormTimer:
                        case WatchMode.SystemTimer:
                        case WatchMode.ThreadingTimer:
                            {
                                m_FormTimers = new List<System.Windows.Forms.Timer>();
                                for (int i = 0; i < m_MasterCount; i++)
                                {
                                    System.Windows.Forms.Timer formTimer = new System.Windows.Forms.Timer();
                                    formTimer.Tag = i;
                                    formTimer.Interval = m_WatchCycleTime;
                                    formTimer.Tick += new EventHandler(FormTimerTick);
                                    formTimer.Enabled = true;
                                    m_FormTimers.Add(formTimer);
                                }
                            }
                            break;
                    }
                }

                this.DeviceState = ActiveState.Run;
            }

            m_Initialized = ok;

            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }

        private void CreateModuleInfo(IoDefines ioDefines)
        {
            if (m_Simul.Melsec)
            {
                ////Simulation Mode 이면 가상의 Node를 한개 잡는다.
                //m_ModuleConfig.Container.Add(new IoNode());
                //m_MasterCount = 1;

                m_DiCount = 0xFFFF;
                m_DoCount = 0xFFFF;
                m_AiCount = 0xFFFF;
                m_AoCount = 0xFFFF;
            }
            else    //Io define 정보를 이용해서 Network 구성정보를 만들어 내야 한다.        
            {
                for (int nodeId = 0; nodeId < ioDefines.Count; nodeId++)
                {
                    IoNodeMelsec node = new IoNodeMelsec();
                    IoNodeMelsec definedNode = (IoNodeMelsec)ioDefines.Container[nodeId];
                    //node.PollMethod = definedNode.PollMethod;

                    int terminalCount = definedNode.Terminals.Count;
                    for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                    {
                        MelsecTerminal definedTerminal = (MelsecTerminal)definedNode.Terminals[terminalId];
                        MelsecTerminal terminal = definedTerminal.Clone();

                        for (int channelId = 0; channelId < terminal.ChannelCount; channelId++)
                        {
                            IoItem ioItem = terminal.Channels[channelId];

                            // Master address, offset + channel index.                            
                            ioItem.Channel = definedTerminal.OffsetDec + channelId;

                            // Buffer address, Type별로 Node 구분없이 순차 증가한다.
                            switch (terminal.IoType)
                            {
                                case IoType.DI:
                                    ioItem.Id = m_DiCount++;
                                    break;
                                case IoType.DO:
                                    ioItem.Id = m_DoCount++;
                                    break;
                                case IoType.AI:
                                    ioItem.Id = m_AiCount++;
                                    break;
                                case IoType.AO:
                                    ioItem.Id = m_AoCount++;
                                    break;
                            }
                        }

                        node.Terminals.Add(terminal);
                    }

                    m_ModuleConfig.Container.Add(node);
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

        private void FormTimerTick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer formTimer = (System.Windows.Forms.Timer)sender;
            int id = (int)formTimer.Tag;

            UpdateIoStatus(id);
            ActiveStateMonitor(id);
        }

        private void SystemTimerTick(object source, System.Timers.ElapsedEventArgs e)
        {
        }

        private void ThreadingTimerTick(Object stateInfo)
        {
        }

        public void UpdateIoStatus()
        {
            for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
            {
                UpdateIoStatus(nodeId);
            }
        }

        private bool _tarce = false;
        private DateTime[] _startTime = new DateTime[20];
        private int _commCount = 0;

        public void UpdateIoStatus(int masterId)
        {
            if (_tarce) _commCount = 0;

            int nodeId = masterId;
            MelsecMaster master = m_Masters[nodeId];
            IoNode node = m_ModuleConfig.Container[nodeId];

            short maxSize = MelsecTerminal._MaxWordSizeBlockRW;
            int terminalCount = node.Terminals.Count;
            for (int terminalId = 0; terminalId < terminalCount; terminalId++)
            {
                MelsecTerminal terminal = (MelsecTerminal)node.Terminals[terminalId];
                int channelCount = terminal.ChannelCount;

                short stationNo = terminal.StationNo;
                short networkNo = terminal.NetworkNo;
                devTYPE deviceType = terminal.DeviceType;

                switch (terminal.IoType)
                {
                    case IoType.DI:
                        {
                            if (m_Simul.Loader && terminal.LinkMemoryDivision == LinkMemoryDivision.LOADER) break;
                            if (m_Simul.Eqp && terminal.LinkMemoryDivision == LinkMemoryDivision.EQP) break;

                            short byteSize = (short)((channelCount - 1) / 8 + 1);
                            short shortSize = (short)((byteSize - 1) / 2 + 1);
                            short blockCount = (short)((shortSize - 1) / maxSize + 1);
                            short curByteSize = byteSize;
                            short curShortSize = shortSize;

                            for (short blockId = 0; blockId < blockCount; blockId++)
                            {
                                curShortSize = (shortSize - maxSize) > 0 ? maxSize : shortSize;
                                shortSize -= curShortSize;
                                byteSize -= (short)(curShortSize * 2);
                                curByteSize = byteSize < 0 ? (short)((curShortSize * 2) - 1) : (short)(curShortSize * 2);
                                short[] readData = new short[curShortSize];
                                int blockStartIndex = blockId * maxSize;
                                int startAddress = terminal.Channels[blockStartIndex].Channel;

                                if (_tarce) _commCount++;

                                short rv = master.MdReceive(stationNo, networkNo, deviceType, startAddress, ref curByteSize, ref readData);
                                if (rv == 0)
                                {
                                    for (int i = 0; i < curByteSize * 8; i++)
                                    {
                                        int channelIndex = blockStartIndex + i;
                                        if (channelIndex >= channelCount)
                                        {
                                            break;
                                        }
                                        int index = i / 16;
                                        bool val = ((readData[index] >> (i % 16)) & 0x01) > 0;
                                        int ioAddress = terminal.Channels[channelIndex].Id;
                                        m_DiStatus[ioAddress] = val;
                                    }
                                }
                            }
                        }
                        break;
                    case IoType.DO:
                        {
                            short byteSize = (short)((channelCount - 1) / 8 + 1);
                            short shortSize = (short)((byteSize - 1) / 2 + 1);
                            short blockCount = (short)((shortSize - 1) / maxSize + 1);
                            short curByteSize = byteSize;
                            short curShortSize = shortSize;

                            for (short blockId = 0; blockId < blockCount; blockId++)
                            {
                                curShortSize = (shortSize - maxSize) > 0 ? maxSize : shortSize;
                                shortSize -= curShortSize;
                                byteSize -= (short)(curShortSize * 2);
                                curByteSize = byteSize < 0 ? (short)((curShortSize * 2) - 1) : (short)(curShortSize * 2);
                                short[] writeData = new short[curShortSize];
                                int blockStartIndex = blockId * maxSize;
                                int startAddress = terminal.Channels[blockStartIndex].Channel;

                                for (int i = 0; i < curByteSize * 8; i++)
                                {
                                    if (i < terminal.Channels.Count)
                                    {
                                        int ioAddress = terminal.Channels[blockStartIndex + i].Id;
                                        short val = m_DoCmdBuf[ioAddress] ? (short)1 : (short)0;
                                        int index = i / 16;
                                        writeData[index] |= (short)(val << (i % 16));
                                    }
                                }

                                if (_tarce) _commCount++;

                                short rv = master.MdSend(stationNo, networkNo, deviceType, startAddress, ref curByteSize, ref writeData);
                            }
                        }
                        break;
                    case IoType.AI:
                        {
                            if (m_Simul.Loader && terminal.LinkMemoryDivision == LinkMemoryDivision.LOADER) break;
                            if (m_Simul.Eqp && terminal.LinkMemoryDivision == LinkMemoryDivision.EQP) break;

                            short byteSize = (short)(channelCount * 2);
                            short shortSize = (short)(byteSize / 2);
                            short blockCount = (short)((shortSize - 1) / maxSize + 1);
                            short curByteSize = byteSize;
                            short curShortSize = shortSize;

                            for (short blockId = 0; blockId < blockCount; blockId++)
                            {
                                curShortSize = (shortSize - maxSize) > 0 ? maxSize : shortSize;
                                shortSize -= curShortSize;
                                curByteSize = (short)(curShortSize * 2);
                                short[] readData = new short[curShortSize];
                                int blockStartIndex = blockId * maxSize;
                                int startAddress = terminal.Channels[blockStartIndex].Channel;

                                if (_tarce) _commCount++;

                                short rv = master.MdReceive(stationNo, networkNo, deviceType, startAddress, ref curByteSize, ref readData);
                                if (rv == 0)
                                {
                                    for (int i = 0; i < curShortSize; i++)
                                    {
                                        int index = i;
                                        short val = readData[index];
                                        int ioAddress = terminal.Channels[blockStartIndex + i].Id;
                                        m_AiStatus[ioAddress] = val;
                                    }
                                }
                            }
                        }
                        break;
                    case IoType.AO:
                        {
                            short byteSize = (short)(channelCount * 2);
                            short shortSize = (short)(byteSize / 2);
                            short blockCount = (short)((shortSize - 1) / maxSize + 1);
                            short curByteSize = byteSize;
                            short curShortSize = shortSize;

                            for (short blockId = 0; blockId < blockCount; blockId++)
                            {
                                curShortSize = (shortSize - maxSize) > 0 ? maxSize : shortSize;
                                shortSize -= curShortSize;
                                curByteSize = (short)(curShortSize * 2);
                                short[] writeData = new short[curShortSize];
                                int blockStartIndex = blockId * maxSize;
                                int startAddress = terminal.Channels[blockStartIndex].Channel;

                                for (int i = 0; i < curShortSize; i++)
                                {
                                    int index = i;
                                    short val = (short)m_AoCmdBuf[terminal.Channels[blockStartIndex + index].Id];
                                    writeData[index] = val;
                                }

                                if (_tarce) _commCount++;

                                short rv = master.MdSend(stationNo, networkNo, deviceType, startAddress, ref curByteSize, ref writeData);
                            }
                        }
                        break;
                }
            }

            if (_tarce)
            {
                DateTime endTime = DateTime.Now;
                TimeSpan diff = endTime - _startTime[masterId];
                string trace = string.Format("{0}-UpdateIoStatus:{1}-{2}", masterId.ToString(), diff.TotalMilliseconds.ToString(), _commCount.ToString());
                Trace.WriteLine(trace);
                _startTime[masterId] = DateTime.Now;
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

            UpdateIoStatus();
        }

        public void ActiveStateMonitor()
        {
            for (int i = 0; i < m_MasterCount; i++)
            {
                ActiveStateMonitor(i);
            }
        }

        public void ActiveStateMonitor(int masterId)
        {
            this.DeviceState = m_Masters[masterId].IsOpened ? ActiveState.Run : ActiveState.Stop;

            if (!m_Masters[masterId].IsOpened)
            {
                m_Masters[masterId].Connect();
            }
        }

        #endregion

        #region ICtlDevice 멤버

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

        public bool ReadDiSync(int index)
        {
            return ReadDiAsync(index);
        }

        public bool ReadDiAsync(int index)
        {
            //MelsecDeviceInfo info = m_AddressMap[(int)IoType.DI].Items[index];
            //if (((IoNodeMelsec)m_ModuleConfig.Container[info.NodeId]).PollMethod == IoNodeMelsec.PollingMethod.Random)
            //{
            //    return m_MelDevBitReadBuffer[info.NodeId][info.TerminalId][info.ChannelId];
            //}
            //else
            {
                return m_DiStatus[index];
            }
        }

        public bool ReadDoSync(int index)
        {
            return ReadDoAsync(index);
        }

        public bool ReadDoAsync(int index)
        {
            //MelsecDeviceInfo info = m_AddressMap[(int)IoType.DO].Items[index];
            //if (((IoNodeMelsec)m_ModuleConfig.Container[info.NodeId]).PollMethod == IoNodeMelsec.PollingMethod.Random)
            //{
            //    return m_MelDevBitWriteCommand[info.NodeId][info.TerminalId][info.ChannelId] == 1 ;
            //}
            //else
            {
                return m_DoCmdBuf[index];
            }
        }

        public short ReadAiSync(int index)
        {
            return ReadAiAsync(index);
        }

        public short ReadAiAsync(int index)
        {
            return (short)m_AiStatus[index];
        }

        public ushort ReadAoSync(int index)
        {
            return ReadAoAsync(index);
        }

        public ushort ReadAoAsync(int index)
        {
            //MelsecDeviceInfo info = m_AddressMap[(int)IoType.AO].Items[index];
            //if (((IoNodeMelsec)m_ModuleConfig.Container[info.NodeId]).PollMethod == IoNodeMelsec.PollingMethod.Random)
            //{
            //    return (ushort)m_MelDevWordWriteCommand[info.NodeId][info.TerminalId][info.ChannelId];
            //}
            //else
            {
                return (ushort)m_AoCmdBuf[index];
            }
        }

        public void WriteDiSync(int index, bool val)
        {
            if (m_Simul.Melsec || m_Simul.Loader || m_Simul.Eqp)
            {
                m_DiStatus[index] = val;
            }
        }

        public void WriteDoSync(int index, bool val)
        {
            WriteDoAsync(index, val);
        }

        public void WriteDoAsync(int index, bool val)
        {
            //MelsecDeviceInfo info = m_AddressMap[(int)IoType.DO].Items[index];
            //if (((IoNodeMelsec)m_ModuleConfig.Container[info.NodeId]).PollMethod == IoNodeMelsec.PollingMethod.Random)
            //{
            //    m_MelDevBitWriteCommand[info.NodeId][info.TerminalId][info.ChannelId] = (short)(val ? 1 : 0);
            //}
            //else
            {
                m_DoCmdBuf[index] = val;
            }
        }

        public void WriteAiSync(int index, short val)
        {
            if (m_Simul.Melsec || m_Simul.Loader || m_Simul.Eqp)
            {
                m_AiStatus[index] = val;
            }
        }

        public void WriteAoSync(int index, ushort val)
        {
            WriteAoAsync(index, val);
        }

        public void WriteAoAsync(int index, ushort val)
        {
            //MelsecDeviceInfo info = m_AddressMap[(int)IoType.AO].Items[index];
            //if (((IoNodeMelsec)m_ModuleConfig.Container[info.NodeId]).PollMethod == IoNodeMelsec.PollingMethod.Random)
            //{
            //    m_MelDevWordWriteCommand[info.NodeId][info.TerminalId][info.ChannelId] = (short)val;
            //}
            //else
            {
                m_AoCmdBuf[index] = val;
            }
        }

        public void Uninitialize()
        {
            m_Uninitializing = true;

            if (m_Simul.Melsec)
            {
                m_Initialized = false;
            }
            else if (m_Initialized)
            {
                switch (m_WatchMode)
                {
                    case WatchMode.Thread:
                        {
                            foreach (ThreadMelsecNetWatch thread in m_ThreadWatchs)
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

                //Watch thread가 중지할 수 있는 여유를 줘야 한다.
                System.Threading.Thread.Sleep(500);

                //모든 출력값을 Reset 한다.
                ClearAllNode();

                for (int i = 0; i < m_MasterCount; i++)
                {
                    m_Masters[i].Close();
                }
            }
        }

        #region 사용하지 않음
        public event IoStateChangeEventHandler OnIoStateChange;
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
        #endregion

        #region 사용하지 않음
        //private List<MelsecDeviceInfos>[] m_MelDevBitReadInfo; //polling 해야할 대상, 노드별로, device block을 등록
        //private List<MelsecDeviceInfos>[] m_MelDevWordReadInfo; //polling 해야할 대상, 노드별로, device block을 등록
        //private List<MelsecDeviceRandomInfos>[] m_MelDevBitWriteInfo; //polling 해야할 대상, 노드별로, device block을 등록
        //private List<MelsecDeviceRandomInfos>[] m_MelDevWordWriteInfo; //polling 해야할 대상, 노드별로, device block을 등록
        //private List<bool[]>[] m_MelDevBitReadBuffer; //polling한 결과를 저장할 Buffer
        //private List<short[]>[] m_MelDevWordReadBuffer; //polling한 결과를 저장할 Buffer
        //private List<short[]>[] m_MelDevBitWriteCommand; //command를 저장할 Buffer
        //private List<short[]>[] m_MelDevWordWriteCommand; //command를 저장할 Buffer
        //private MelsecDeviceInfos[] m_AddressMap;	// R/W index 와 buffer를 연관 시켜줄 정보, IoType별로 
        //private MelsecDeviceInfos m_MelsecDevices;

        //private void CreateMelsecDeviceInfo(IoDefines ioDefines)
        //{
        //    //저장소를 만들고
        //    int nodeCount = ioDefines.Count;
        //    m_MelDevBitReadInfo = new List<MelsecDeviceInfos>[nodeCount];
        //    m_MelDevWordReadInfo = new List<MelsecDeviceInfos>[nodeCount];
        //    m_MelDevBitWriteInfo = new List<MelsecDeviceRandomInfos>[nodeCount];
        //    m_MelDevWordWriteInfo = new List<MelsecDeviceRandomInfos>[nodeCount];
        //    m_MelDevBitReadBuffer = new List<bool[]>[nodeCount];
        //    m_MelDevWordReadBuffer = new List<short[]>[nodeCount];
        //    m_MelDevBitWriteCommand = new List<short[]>[nodeCount];
        //    m_MelDevWordWriteCommand = new List<short[]>[nodeCount];
        //    m_AddressMap = new MelsecDeviceInfos[4];
        //    m_MelsecDevices = new MelsecDeviceInfos();

        //    Array ioTypes = Enum.GetValues(typeof(IoType));

        //    for (int nodeId = 0; nodeId < nodeCount; nodeId++)
        //    {
        //        //Memory 할당
        //        m_MelDevBitReadInfo[nodeId] = new List<MelsecDeviceInfos>();
        //        m_MelDevWordReadInfo[nodeId] = new List<MelsecDeviceInfos>();
        //        m_MelDevBitWriteInfo[nodeId] = new List<MelsecDeviceRandomInfos>();
        //        m_MelDevWordWriteInfo[nodeId] = new List<MelsecDeviceRandomInfos>();
        //        m_MelDevBitReadBuffer[nodeId] = new List<bool[]>();
        //        m_MelDevWordReadBuffer[nodeId] = new List<short[]>();
        //        m_MelDevBitWriteCommand[nodeId] = new List<short[]>();
        //        m_MelDevWordWriteCommand[nodeId] = new List<short[]>();

        //        //Node에 등록된 Melsec Device를 IoType별로 가져온다
        //        IoNode node = ioDefines.Container[nodeId];
        //        MelsecDeviceInfos nodeInfos = MelsecDeviceInfos.GetMelsecDeviceInfos(node);

        //        foreach (object io in ioTypes)
        //        {
        //            //IoType 이 일치하는 정보만 가져옴
        //            IoType ioType = (IoType)io;
        //            MelsecDeviceInfos infosByIoType = MelsecDeviceInfos.GetItemsBy(nodeInfos, ioType);
        //            MelsecDeviceInfos.SortByDevType(infosByIoType);

        //            //Make Buffer
        //            switch (ioType)
        //            {
        //                case IoType.DI:
        //                case IoType.AI:
        //                    CreateMelsecInputDeviceInfo(nodeId, ioType, infosByIoType, nodeInfos);
        //                    break;
        //                case IoType.DO:
        //                case IoType.AO:
        //                    CreateMelsecOutputDeviceInfo(nodeId, ioType, infosByIoType, nodeInfos);
        //                    break;
        //            }
        //        }

        //        //Make AddressMap : Io index와 polling buffer address를 매핑
        //        MelsecDeviceInfos.SortByIoType(nodeInfos);
        //        foreach (object obj in ioTypes)
        //        {
        //            IoType ioType = (IoType)obj;
        //            MelsecDeviceInfos registeredInfos = MelsecDeviceInfos.GetItemsBy(nodeInfos, ioType);
        //            MelsecDeviceInfos temp = new MelsecDeviceInfos();

        //            //Find PollingInfo
        //            foreach (MelsecDeviceInfo info in registeredInfos.Items)
        //            {
        //                MelsecDeviceInfo findItem = MelsecDeviceInfos.GetItemsBy(m_MelsecDevices, info.DeviceType, info.AddressDec, info.InOutType);
        //                if (findItem != null)
        //                {
        //                    temp.Items.Add(findItem);
        //                }
        //            }

        //            m_AddressMap[(int)ioType] = temp;
        //        }

        //        m_MelsecDevices.Items.Clear();
        //    }
        //}

        //private void CreateMelsecInputDeviceInfo(int nodeId, IoType ioType, MelsecDeviceInfos infosByIoType, MelsecDeviceInfos source)
        //{
        //    int diBlockIndex = 0;
        //    int aiBlockIndex = 0;
        //    short maxSize = 0;
        //    if (ioType == IoType.DI)
        //    {
        //        maxSize = MelsecTerminal._MaxBitSizeBlockRW;
        //    }
        //    else
        //    {
        //        maxSize = MelsecTerminal._MaxWordSizeBlockRW;
        //    }

        //    Array devTypes = Enum.GetValues(typeof(devTYPE));
        //    foreach (object obj in devTypes)
        //    {
        //        MelsecDeviceInfos infosByDevType = MelsecDeviceInfos.GetItemsBy(infosByIoType, (devTYPE)obj);
        //        int count = infosByDevType.Items.Count;
        //        if (count > 0)
        //        {
        //            // 해당 type의 첫번째 address와 마지막 address 사이의 모든 메모리를 등록한다.
        //            // maxSize 만큼씩 잘라서 buffer를 생성한다.
        //            // 해당영역중 실제 사용 등록된 device가 하나도 없다면 제외
        //            MelsecDeviceInfo firstInfo = infosByDevType.Items[0];
        //            int devCount = count;
        //            int firstAddress = firstInfo.AddressDec;
        //            int lastAddress = infosByDevType.Items[devCount - 1].AddressDec;
        //            int channelCount = (firstAddress == lastAddress) ? 1 : ((lastAddress - firstAddress) + 1);
        //            IoDataType ioDataType = firstInfo.IoDataType;
        //            devTYPE devType = (devTYPE)obj;

        //            short blockCount = (short)((channelCount - 1) / maxSize + 1);
        //            int targetCount = channelCount;
        //            for (int blockId = 0; blockId < blockCount; blockId++)
        //            {
        //                MelsecDeviceInfos infos = new MelsecDeviceInfos();
        //                int curCount = (targetCount - maxSize) > 0 ? maxSize : targetCount; //읽어야할 size
        //                targetCount -= curCount; //남은 size
        //                int startAddress = blockId * maxSize + firstAddress;
        //                // 해당영역중 실제 사용 등록된 device가 하나도 없다면 제외
        //                bool used = false;
        //                for (int i = 0; i < curCount; i++)
        //                {
        //                    MelsecDeviceInfo info = new MelsecDeviceInfo();
        //                    info.NodeId = nodeId;
        //                    info.TerminalId = ioDataType == IoDataType.Digital ? diBlockIndex : aiBlockIndex;
        //                    info.ChannelId = i;
        //                    info.AddressDec = startAddress + i;
        //                    info.DeviceType = devType;
        //                    info.InOutType = IoInOutType.In;
        //                    infos.Items.Add(info);//block별 list에 등록		

        //                    MelsecDeviceInfo find = MelsecDeviceInfos.GetItemsBy(source, info.DeviceType, info.AddressDec, info.InOutType);

        //                    used |= (find != null) ;
        //                }

        //                //if (used)
        //                {
        //                    foreach (MelsecDeviceInfo dev in infos.Items)
        //                    {
        //                        m_MelsecDevices.Items.Add(dev);//전체 list에도 등록							
        //                    }

        //                    if (ioType == IoType.DI)
        //                    {
        //                        m_MelDevBitReadInfo[nodeId].Add(infos);
        //                        m_MelDevBitReadBuffer[nodeId].Add(new bool[channelCount]);
        //                        diBlockIndex++;
        //                    }
        //                    else
        //                    {
        //                        m_MelDevWordReadInfo[nodeId].Add(infos);
        //                        m_MelDevWordReadBuffer[nodeId].Add(new short[channelCount]);
        //                        aiBlockIndex++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void CreateMelsecOutputDeviceInfo(int nodeId, IoType ioType, MelsecDeviceInfos infosByIoType, MelsecDeviceInfos source)
        //{
        //    int doBlockIndex = 0;
        //    int aoBlockIndex = 0;
        //    short maxSize = 0;
        //    if (ioType == IoType.DO)
        //    {
        //        maxSize = MelsecTerminal._MaxBitSizeRandomWrite;
        //    }
        //    else
        //    {
        //        maxSize = MelsecTerminal._MaxWordSizeRandomWrite;
        //    }

        //    int count = infosByIoType.Items.Count;
        //    int curId = 0;

        //    MelsecDeviceRandomInfos infos = new MelsecDeviceRandomInfos((short)(count > maxSize ? maxSize : count));
        //    for (int i = 0; i < count; i++)
        //    {
        //        // maxSize 만큼씩 잘라서 buffer를 생성한다.
        //        MelsecDeviceInfo devInfo = infosByIoType.Items[i];
        //        IoDataType ioDataType = devInfo.IoDataType;
        //        devTYPE devType = devInfo.DeviceType;

        //        //block별 list에 등록
        //        infos.DeviceTypes[curId] = devType;
        //        infos.Indexes[curId] = devInfo.AddressDec;
        //        infos.Size[curId] = 1;

        //        MelsecDeviceInfo info = new MelsecDeviceInfo();
        //        info.NodeId = nodeId;
        //        info.TerminalId = ioDataType == IoDataType.Digital ? doBlockIndex : aoBlockIndex;
        //        info.ChannelId = curId;
        //        info.AddressDec = devInfo.AddressDec;
        //        info.DeviceType = devType;
        //        info.InOutType = IoInOutType.Out;
        //        m_MelsecDevices.Items.Add(info);//전체 list에도 등록


        //        if (curId >= maxSize || i == count - 1)
        //        {
        //            if (ioType == IoType.DO)
        //            {
        //                m_MelDevBitWriteInfo[nodeId].Add(infos);
        //                m_MelDevBitWriteCommand[nodeId].Add(new short[curId + 1]);
        //                doBlockIndex++;
        //            }
        //            else
        //            {
        //                m_MelDevWordWriteInfo[nodeId].Add(infos);
        //                m_MelDevWordWriteCommand[nodeId].Add(new short[curId + 1]);
        //                aoBlockIndex++;
        //            }

        //            int size = (count - i); //남은size						
        //            if (size > 0)
        //            {
        //                infos = new MelsecDeviceRandomInfos((short)(size > maxSize ? maxSize : size));
        //            }
        //        }

        //        curId++;
        //    }
        //}

        //public void ReadDi()
        //{
        //    DateTime startTime = DateTime.Now;
        //    int commCount = 0;

        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        MelsecMaster master = m_Masters[nodeId];
        //        List<MelsecDeviceInfos> node = m_MelDevBitReadInfo[nodeId];

        //        int terminalCount = node.Count;
        //        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //        {
        //            MelsecDeviceInfos terminals = node[terminalId];
        //            MelsecDeviceInfo firstInfo = terminals.Items[0];
        //            devTYPE deviceType = firstInfo.DeviceType;
        //            IoDataType ioDataType = firstInfo.IoDataType;
        //            int channelCount = terminals.Items.Count;
        //            short byteSize = (short)((channelCount - 1) / 8 + 1);
        //            short shortSize = (short)((byteSize - 1) / 2 + 1);
        //            int startAddress = firstInfo.AddressDec;
        //            short stationNo = 255;// terminal.StationNo; //일단 고정으로
        //            short networkNo = 0;// terminal.NetworkNo; //일단 고정으로

        //            short[] readData = new short[shortSize];
        //            commCount++;
        //            short rv = master.MdReceive(stationNo, networkNo, deviceType, (short)startAddress, byteSize, ref readData);
        //            if (rv == 0)
        //            {
        //                bool[] bits = new bool[channelCount];
        //                for (int i = 0; i < channelCount; i++)
        //                {
        //                    int index = i / 16;
        //                    bool val = ((readData[index] >> (i % 16)) & 0x01) > 0 ;
        //                    {
        //                        bits[i] = val;
        //                    }
        //                }

        //                Array.Copy(bits, m_MelDevBitReadBuffer[nodeId][terminalId], channelCount);
        //            }
        //        }
        //    }

        //    DateTime endTime = DateTime.Now;
        //    TimeSpan diff = endTime - startTime;
        //    string trace = string.Format("ReadDi : {0} - {1}", diff.TotalMilliseconds.ToString(), commCount.ToString());
        //    Trace.WriteLine(trace);
        //}

        //public void ReadAi()
        //{
        //    DateTime startTime = DateTime.Now;
        //    int commCount = 0;

        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        MelsecMaster master = m_Masters[nodeId];
        //        List<MelsecDeviceInfos> node = m_MelDevWordReadInfo[nodeId];

        //        int terminalCount = node.Count;
        //        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //        {
        //            MelsecDeviceInfos terminals = node[terminalId];
        //            MelsecDeviceInfo firstInfo = terminals.Items[0];
        //            devTYPE deviceType = firstInfo.DeviceType;
        //            IoDataType ioDataType = firstInfo.IoDataType;
        //            int channelCount = terminals.Items.Count;
        //            short byteSize = (short)(channelCount * 2);
        //            short shortSize = (short)(channelCount);
        //            int startAddress = firstInfo.AddressDec;
        //            short stationNo = 255;// terminal.StationNo; //일단 고정으로
        //            short networkNo = 0;// terminal.NetworkNo; //일단 고정으로

        //            short[] readData = new short[shortSize];
        //            commCount++;
        //            short rv = master.MdReceive(stationNo, networkNo, deviceType, (short)startAddress, byteSize, ref readData);
        //            if (rv == 0)
        //            {
        //                Array.Copy(readData, m_MelDevWordReadBuffer[nodeId][terminalId], channelCount);
        //            }
        //        }
        //    }

        //    DateTime endTime = DateTime.Now;
        //    TimeSpan diff = endTime - startTime;
        //    string trace = string.Format("ReadAi : {0} - {1}", diff.TotalMilliseconds.ToString(), commCount.ToString());
        //    Trace.WriteLine(trace);
        //}

        //public void WriteDo()
        //{
        //    DateTime startTime = DateTime.Now;
        //    int commCount = 0;

        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        MelsecMaster master = m_Masters[nodeId];
        //        List<MelsecDeviceRandomInfos> node = m_MelDevBitWriteInfo[nodeId];

        //        int terminalCount = node.Count;
        //        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //        {
        //            MelsecDeviceRandomInfos terminals = node[terminalId];
        //            int channelCount = terminals.Count;
        //            short stationNo = 255;// terminal.StationNo; //일단 고정으로
        //            short networkNo = 0;// terminal.NetworkNo; //일단 고정으로

        //            //Make buffer
        //            devTYPE[] devTypes = new devTYPE[channelCount];
        //            int[] indexes = new int[channelCount];
        //            short[] size = new short[channelCount];
        //            short[] writeData = new short[channelCount];

        //            //Make data
        //            Array.Copy(terminals.DeviceTypes, devTypes, channelCount);
        //            Array.Copy(terminals.Indexes, indexes, channelCount);
        //            Array.Copy(terminals.Size, size, channelCount);
        //            Array.Copy(m_MelDevBitWriteCommand[nodeId][terminalId], writeData, channelCount);

        //            //Send data
        //            commCount++;
        //            short rv = master.MdBitRandSend(stationNo, networkNo, devTypes, indexes, size, writeData);
        //        }
        //    }

        //    DateTime endTime = DateTime.Now;
        //    TimeSpan diff = endTime - startTime;
        //    string trace = string.Format("WriteDo : {0} - {1}", diff.TotalMilliseconds.ToString(), commCount.ToString());
        //    Trace.WriteLine(trace);
        //}

        //public void WriteAo()
        //{
        //    DateTime startTime = DateTime.Now;
        //    int commCount = 0;

        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        MelsecMaster master = m_Masters[nodeId];
        //        List<MelsecDeviceRandomInfos> node = m_MelDevWordWriteInfo[nodeId];

        //        int terminalCount = node.Count;
        //        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //        {
        //            MelsecDeviceRandomInfos terminals = node[terminalId];
        //            int channelCount = terminals.Count;
        //            short stationNo = 255;// terminal.StationNo; //일단 고정으로
        //            short networkNo = 0;// terminal.NetworkNo; //일단 고정으로

        //            //Make buffer
        //            devTYPE[] devTypes = new devTYPE[channelCount];
        //            int[] indexes = new int[channelCount];
        //            short[] size = new short[channelCount];
        //            short[] writeData = new short[channelCount];

        //            //Make data
        //            Array.Copy(terminals.DeviceTypes, devTypes, channelCount);
        //            Array.Copy(terminals.Indexes, indexes, channelCount);
        //            Array.Copy(terminals.Size, size, channelCount);
        //            Array.Copy(m_MelDevWordWriteCommand[nodeId][terminalId], writeData, channelCount);

        //            //Send data
        //            commCount++;
        //            short rv = master.MdWordRandSend(stationNo, networkNo, devTypes, indexes, size, writeData);
        //        }
        //    }

        //    DateTime endTime = DateTime.Now;
        //    TimeSpan diff = endTime - startTime;
        //    string trace = string.Format("WriteAo : {0} - {1}", diff.TotalMilliseconds.ToString(), commCount.ToString());
        //    Trace.WriteLine(trace);
        //}
        #endregion

        #endregion

        #region ICtlDevice 멤버


        public object Read(int index, IoType type, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICtlDevice 멤버


        public void Write(int index, object val, IoType type, int group, int dataType, int node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICtlDevice 멤버
        #endregion

        #region ICtlDevice 멤버
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
