///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.18
// Author       : jemoon
// Description  : Melsec CC Link Controller
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
using Microsoft.VisualBasic;
using Dms.Common;
using Dms.Util.IODefine;
using System.Threading;

namespace Dms.Ctl
{
    public class MelsecCCLink : ICtlDevice
    {
        #region Singleton
        public static readonly MelsecCCLink InstanceM = new MelsecCCLink();
        public static readonly MelsecCCLink InstanceC = new MelsecCCLink(); //  BM : Crevvis CClink 대응을 위한 instance 분리 및 개별 관리
        #endregion

        #region Fields
        private Simul m_Simul = AppConfig.Instance.Simul;
        private bool m_SimulateController = false;
        private bool m_Initialized = false;
        private bool m_Uninitializing = false;
        private ActiveState m_ActiveState = ActiveState.UnKnown;
        private string m_ControllerState = "";
        private List<MelsecMaster> m_Masters = new List<MelsecMaster>(); // jemoon : 2011.03.21 recover
        private XLog m_Log = new XLog("MelsecCClink", XLog.LogStampType.UseStamp); // 11.02.20 minhan
        private List<SeqInitStation> m_SeqInitStation = new List<SeqInitStation>();
        private IoDefines m_ModuleConfig = new IoDefines(FieldBusType.MitsubishiCClink);
        private int m_MasterCount;
        private int m_DiCount = 0;
        private int m_DoCount = 0;
        private int m_AiCount = 0;
        private int m_AoCount = 0;
        private int m_CrevisErrCode = 0; // 11.02.23 minhan
        // Auto Address Setting

        private int m_CurrentStationNo = 0;
        private int m_NodeStationNo = 0;

        private int m_DiPointCount = 0;
        private int m_DoPointCount = 0;
        private int m_AiChannelCount = 0;
        private int m_AoChannelCount = 0;

        private int m_NodeNo = 0;


        //Buffer for sequence
        private bool[] m_DiStatus = null;
        private bool[] m_DoStatus = null;
        private int[] m_AiStatus = null;
        private int[] m_AoStatus = null;
        private bool[] m_DoCmdBuf = null;
        private int[] m_AoCmdBuf = null;

        //Watch를 위한 변수들
        private int m_WatchCycleTime = 10;
        private WatchMode m_WatchMode = WatchMode.FormTimer;
        private ThreadCCLinkWatch m_ThreadWatch = null;
        private System.Windows.Forms.Timer m_FormTimer = null;
        private System.Timers.Timer m_SystemTimer = null;
        private System.Threading.Timer m_ThreadingTimer = null;

        //CC link Staion Special Infomation
        public const int _CC_REMOTE_IO_SIZE = 32;
        public const int _CC_REMOTE_REG_SIZE = 4;
        public const int _CC_ADDRES_REMOTE_READY = 0x1B;
        public const int _CC_ADDRES_STATION_INIT = 0x19;
        public const int _CC_ADDRES_AI_USAGE = 0x00;
        public const int _CC_ADDRES_AI_RANGE_TYPE = 0x01;
        public const int _CC_ADDRES_AI_MOV_AVG = 0x02;
        public const int _CC_ADDRES_AI_MOV_AVG_START = 0x00;
        public const int _CC_ADDRES_AO_USAGE = 0x02;
        public const int _CC_ADDRES_AO_RANGE_TYPE = 0x03;
        public const int _CC_ADDRES_AO_ENABLE = 0x00;
        public const int _CC_ADDRES_MASTER_ERROR = 0x00;
        public const int _CC_ADDRES_MASTER_LINK_OK = 0x01;
        public const int _CC_ADDRES_MASTER_READY = 0x0F;
        public const int _CC_ADDRES_STATION_LINK_STATE = 0x80;
        //public const int _CC_ADDRES_STATION_READY = 0x7FF; // 11.02.23 minhan Crevis
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
        private MelsecCCLink()
        {
        }
        #endregion

        #region Event
        #endregion

        #region Methods
        public void WriteLog(string msg) // 11.02.20 minhan
        {
            m_Log.TextOut(msg);
        }

        private bool AddMaster(IoNodeMelsec node)
        {
            if (!m_Simul.IoController)
            {
                MelsecMaster master = new MelsecMaster(node); //jemoon : 2011.03.21 recover
                master.Simulate = m_Simul.Melsec;//20130530
                m_Masters.Add(master);
                m_MasterCount = m_Masters.Count;
            }

            return true;
        }

        private bool CreateMasterInfo(IoDefines iodefines)
        {
            if ((iodefines.BusType != FieldBusType.MitsubishiCClink) &&
                (iodefines.BusType != FieldBusType.CrevisCClink))
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
            if (!m_SimulateController)
            {
                for (int i = 0; i < m_MasterCount; i++)
                {
                    MelsecMaster master = m_Masters[i]; //jemoon : 2011.03.21 recover
                    ok &= master.Open();
                }
            }

            return ok;
        }


        public DmsErrors Initialize(IoDefines iodefines)
        {
            m_SimulateController = m_Simul.Device || iodefines.SimulateController;

            bool ok = true;

            ok &= CreateMasterInfo(iodefines);
            ok &= OpenMasters();

            if (ok)
            {
                //Master로 부터 Terminal 정보를 읽어와서 Io 구성정보를 만듬
                CreateModuleInfo(iodefines);

                //Sequece에서 참조할 Memory Buffer 생성
                CreateBuffer();

                //Update 구조 생성
                if (!m_SimulateController)
                {
                    switch (m_WatchMode)
                    {
                        case WatchMode.Thread:
                            { // Modbus Master에 Connect 하고 나서 Watch Thread Start 하기 까지 약간 Delay 필요함
                                m_ThreadWatch = new ThreadCCLinkWatch(m_WatchCycleTime, this);
                                m_ThreadWatch.Start();
                            }
                            break;
                        case WatchMode.FormTimer:
                            {
                                m_FormTimer = new System.Windows.Forms.Timer();
                                m_FormTimer.Interval = m_WatchCycleTime;
                                m_FormTimer.Tick += new EventHandler(FormTimerTick);
                                m_FormTimer.Enabled = true;
                            }
                            break;
                        case WatchMode.SystemTimer:
                            {
                                m_SystemTimer = new System.Timers.Timer();
                                m_SystemTimer.Interval = m_WatchCycleTime;
                                m_SystemTimer.Elapsed += new System.Timers.ElapsedEventHandler(SystemTimerTick);
                                m_SystemTimer.Enabled = true;
                            }
                            break;
                        case WatchMode.ThreadingTimer:
                            {
                                m_ThreadingTimer = new System.Threading.Timer(new TimerCallback(ThreadingTimerTick), null, 0, m_WatchCycleTime);
                            }
                            break;
                    }

                }

                this.DeviceState = ActiveState.Run;
            }

            m_Initialized = ok;

            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }

        //private void CreateModuleInfo(IoDefines ioDefines)
        //{
        //    if (m_Simul.Melsec)
        //    {   
        //        ////Simulation Mode 이면 가상의 Node를 한개 잡는다.
        //        //m_ModuleConfig.Container.Add(new IoNode());
        //        //m_MasterCount = 1;

        //        m_DiCount = 1024;
        //        m_DoCount = 1024;
        //        m_AiCount = 512;
        //        m_AoCount = 512;
        //    }
        //    else    //Io define 정보를 이용해서 CC Link 구성정보를 만들어 내야 한다.        
        //    {
        //        for (int nodeId = 0; nodeId < ioDefines.Count; nodeId++)
        //        {
        //            IoNode node = new IoNode();
        //            IoNode definedNode = ioDefines.Container[nodeId];

        //            int totalIoIndex = 0;
        //            int totalTeminalId = 0;
        //            int terminalCount = definedNode.Terminals.Count;
        //            for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //            {
        //                IoTerminal definedTerminal = definedNode.Terminals[terminalId];
        //                CCSpecialDeivce special = definedTerminal as CCSpecialDeivce;

        //                if (special == null)
        //                {
        //                    IoTerminal terminal = definedTerminal.CreateObject();
        //                    terminal.Id = totalTeminalId++;
        //                    terminal.CreateChannels();
        //                    MakeTerminalInfo(ref terminal, ref totalIoIndex);
        //                    node.Terminals.Add(terminal);
        //                }
        //                else
        //                {
        //                    IoTerminal terminal = special.CreateTerminal(0);
        //                    terminal.Id = totalTeminalId++;
        //                    MakeTerminalInfo(ref terminal, ref totalIoIndex);
        //                    node.Terminals.Add(terminal);

        //                    if (special.AddressAllocType == AddressAllocType.Inclusive)
        //                    {
        //                        totalIoIndex -= terminal.ChannelCount;
        //                    }

        //                    terminal = special.CreateTerminal(1);
        //                    terminal.Id = totalTeminalId++;
        //                    MakeTerminalInfo(ref terminal, ref totalIoIndex);
        //                    node.Terminals.Add(terminal);
        //                }
        //            }

        //            m_ModuleConfig.Container.Add(node);
        //        }
        //    }
        //}

        private void CreateModuleInfo(IoDefines ioDefines)
        {
            if (m_Simul.Device)
            {
                ////Simulation Mode 이면 가상의 Node를 한개 잡는다.
                //m_ModuleConfig.Container.Add(new IoNode());
                //m_MasterCount = 1;

                m_DiCount = 1024;
                m_DoCount = 1024;
                m_AiCount = 512;
                m_AoCount = 512;
            }
            else    //Io define 정보를 이용해서 CC Link 구성정보를 만들어 내야 한다.        
            {
                for (int nodeId = 0; nodeId < ioDefines.Count; nodeId++)
                {
                    IoNodeMelsec node = new IoNodeMelsec();
                    IoNodeMelsec definedNode = ioDefines.Container[nodeId] as IoNodeMelsec;

                    node.ChannelNo = definedNode.ChannelNo; // 11.02.24 minhan 새로 생성하되 이 정보가 업데이트가 되지 않아서 추가.
                    node.AddressBase = definedNode.AddressBase;
                    node.AddressRWr = definedNode.AddressRWr;
                    node.AddressRWw = definedNode.AddressRWw;
                    node.AddressRx = definedNode.AddressRx;
                    node.AddressRy = definedNode.AddressRy;
                    node.AddressSb = definedNode.AddressSb;
                    node.AddressSw = definedNode.AddressSw;
                    node.CcLinkMasterInfo = definedNode.CcLinkMasterInfo;
                    //node.ReadyBit = definedNode.ReadyBit;
                    node.DevTypeRWr = definedNode.DevTypeRWr;
                    node.DevTypeRWw = definedNode.DevTypeRWw;
                    node.DevTypeRx = definedNode.DevTypeRx;
                    node.DevTypeRy = definedNode.DevTypeRy;
                    node.DevTypeSw = definedNode.DevTypeSw;
                    CClinkStaionInfo nextInfo = new CClinkStaionInfo();

                    nextInfo.AddressRx = definedNode.AddressRx;
                    nextInfo.AddressRy = definedNode.AddressRy;
                    nextInfo.AddressRWr = definedNode.AddressRWr;
                    nextInfo.AddressRWw = definedNode.AddressRWw;

                    //int totalIoIndex = 0;
                    //int totalTeminalId = 0;
                    int terminalCount = definedNode.Terminals.Count;
                    for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                    {
                        IoTerminal definedTerminal = definedNode.Terminals[terminalId];
                        CClinkStation station = definedTerminal.CreateObject() as CClinkStation;
                        station.Info.MasterNo = nodeId + 1;


                        if (definedTerminal.ProductMaker == Maker.MitsubishiCCLink)
                        {
                            station.Info.StationId = terminalId;
                            station.Info.StationNo = terminalId + 1;

                            MakeStationInfo(definedTerminal, ref station, ref nextInfo);

                            station.Id = terminalId;
                        }
                        else if (definedTerminal.ProductMaker == Maker.CrevisCCLink)
                        {
                            MakeCrevisStationInfo(definedTerminal, ref station, ref nextInfo);

                        }

                        station.CreateChannels();
                        IoTerminal terminal = station as IoTerminal;
                        MakeTerminalInfo(ref terminal);
                        node.Terminals.Add(station);
                    }

                    m_ModuleConfig.Container.Add(node);

                    m_SeqInitStation.Add(new SeqInitStation(m_Masters[nodeId], node));
                }
            }
        }

        private void MakeStationInfo(IoTerminal def, ref IoTerminal current, ref CClinkStaionInfo next)
        {
            CClinkStation tempCur = current as CClinkStation;
            MakeStationInfo(def, ref tempCur, ref next);
        }



        private void MakeCrevisStationInfo(IoTerminal def, ref CClinkStation current, ref CClinkStaionInfo next)
        {
            //Make Crevis CC Link Station Info
            if (current != null)
            {
                CClinkStation tempDef = def as CClinkStation;

                string productName = tempDef.PartName.ToString();

                if (productName.StartsWith("AT") || productName.StartsWith("NA"))
                {
                    m_CurrentStationNo += 1;

                    m_NodeStationNo = 1;

                    m_NodeNo++;

                    tempDef.StationNo = m_CurrentStationNo;


                    m_DiPointCount = 0;
                    m_DoPointCount = 0;
                    m_AiChannelCount = 0;
                    m_AoChannelCount = 0;



                    current.Info.AddressRx = 32 * (tempDef.StationNo - 1);
                    current.Info.AddressRy = 32 * (tempDef.StationNo - 1);

                    next.AddressRx = 32 * (tempDef.StationNo - 1);
                    next.AddressRy = 32 * (tempDef.StationNo - 1);

                    next.AddressRWr = 4 * (tempDef.StationNo - 1);
                    next.AddressRWw = 4 * (tempDef.StationNo - 1);
                }
                else
                {
                    current.Info.AddressRx = next.AddressRx;
                    current.Info.AddressRy = next.AddressRy;
                    current.Info.AddressRWr = next.AddressRWr;
                    current.Info.AddressRWw = next.AddressRWw;
                }

                // Test
                current.Info.StationId = m_CurrentStationNo - 1;
                current.Info.StationNo = m_CurrentStationNo;
                current.Id = m_NodeNo;
                //next.AddressRx = next.AddressRx + current.Info.Points;
                //next.AddressRy = next.AddressRy + current.Info.Points;
                //next.AddressRWr = next.AddressRWr + current.Info.Points;
                //next.AddressRWw = next.AddressRWw + current.Info.Points;

                int points = current.Info.Points;

                switch (current.Info.TerminalType)
                {
                    case CclinkTerminalType.DI:
                        current.Info.IndexDiBuf = m_DiCount;
                        next.AddressRx = current.Info.AddressRx + current.Info.Points;

                        m_DiPointCount += current.Info.Points;

                        break;
                    case CclinkTerminalType.DO:
                        current.Info.IndexDoBuf = m_DoCount;
                        next.AddressRy = current.Info.AddressRy + current.Info.Points;

                        m_DoPointCount += current.Info.Points;

                        break;
                    case CclinkTerminalType.DIO:
                        current.Info.IndexDiBuf = m_DiCount;
                        current.Info.IndexDoBuf = m_DoCount;

                        next.AddressRx = current.Info.AddressRx + current.Info.Points;
                        next.AddressRy = current.Info.AddressRy + current.Info.Points;

                        m_DiPointCount += current.Info.Points;
                        m_DoPointCount += current.Info.Points;
                        break;

                    case CclinkTerminalType.AI:
                        {
                            current.Info.IndexAiBuf = m_AiCount;

                            next.AddressRWr = current.Info.AddressRWr + current.ChannelCount;

                            m_AiChannelCount += current.ChannelCount;
                        }
                        break;
                    case CclinkTerminalType.AO:
                        {
                            current.Info.IndexAoBuf = m_AoCount;

                            next.AddressRWw = current.Info.AddressRWw + current.ChannelCount;

                            m_AoChannelCount += current.ChannelCount;
                        }
                        break;

                    case CclinkTerminalType.BLDC:
                        //  Crevis Station용 BLDC 제어는 아직 개발되지 않음
                        break;
                    case CclinkTerminalType.Inverter:
                        //  Crevis Station용 Inverter 제어는 아직 개발되지 않음
                        break;
                }

                int a = 0;
                int b = 0;

                a = Math.DivRem(m_DiPointCount, 32, out b);

                int DiStationNo = GetStationNo(m_DiPointCount, 32);
                int DoStationNo = GetStationNo(m_DoPointCount, 32);
                int AiStationNo = GetStationNo(m_AiChannelCount, 4);
                int AoStationNo = GetStationNo(m_AoChannelCount, 4);

                int maxStationNo = Math.Max(Math.Max(DiStationNo, DoStationNo), Math.Max(AiStationNo, AoStationNo));    //20130530

                if (m_NodeStationNo < maxStationNo)
                {
                    m_NodeStationNo = maxStationNo;
                    m_CurrentStationNo += 1;
                }

                //if (m_DiPointCount > m_NodeStationNo * 32 ||
                //    m_DoPointCount > m_NodeStationNo * 32 ||
                //    m_AiChannelCount > m_NodeStationNo * 4 ||
                //    m_AoChannelCount > m_NodeStationNo * 4)              
            }
        }

        public int GetStationNo(int count, int size)
        {
            int rv = 0;

            int a = 0;
            int b = 0;

            a = Math.DivRem(count, size, out b);


            if (a <= 0)
            {
                rv = 1;
            }
            else if (b > 0)
            {
                rv = a + 1;
            }
            else rv = a;

            return rv;
        }

        private void MakeStationInfo(IoTerminal def, ref CClinkStation current, ref CClinkStaionInfo next)
        {
            //Make CC Link Station Info
            if (current != null)
            {
                CClinkStation tempDef = def as CClinkStation;

                current.Info.AddressRx = next.AddressRx;
                current.Info.AddressRy = next.AddressRy;
                current.Info.AddressRWr = next.AddressRWr;
                current.Info.AddressRWw = next.AddressRWw;

                next.AddressRx = next.AddressRx + current.Info.StationOccupies * _CC_REMOTE_IO_SIZE;
                next.AddressRy = next.AddressRy + current.Info.StationOccupies * _CC_REMOTE_IO_SIZE;
                next.AddressRWr = next.AddressRWr + current.Info.StationOccupies * _CC_REMOTE_REG_SIZE;
                next.AddressRWw = next.AddressRWw + current.Info.StationOccupies * _CC_REMOTE_REG_SIZE;

                int points = current.Info.Points;

                switch (current.Info.TerminalType)
                {
                    case CclinkTerminalType.DI:
                        current.Info.IndexDiBuf = m_DiCount;
                        break;
                    case CclinkTerminalType.DO:
                        current.Info.IndexDoBuf = m_DoCount;
                        break;
                    case CclinkTerminalType.DIO:
                        current.Info.IndexDiBuf = m_DiCount;
                        current.Info.IndexDoBuf = m_DoCount;
                        break;
                    case CclinkTerminalType.AI:
                        {
                            current.Info.IndexAiBuf = m_AiCount;
                            CclinkRangeType[] rangeTypes = tempDef.GetRangeTypeConfig();
                            CclinkMovingAvgType[] movingAvgTypes = tempDef.GetMovingAvgTypeConfig();
                            for (int i = 0; i < points; i++)
                            {
                                current.Info.Usage |= (ushort)(0x01 << i);
                                current.Info.RangeType |= (ushort)((ushort)rangeTypes[i] << (i * 4));
                                current.Info.MovingAvg |= (ushort)((ushort)movingAvgTypes[i] << (i * 4));
                            }
                        }
                        break;
                    case CclinkTerminalType.AO:
                        {
                            current.Info.IndexAoBuf = m_AoCount;
                            CclinkRangeType[] rangeTypes = tempDef.GetRangeTypeConfig();
                            CclinkOutputType[] outputTypes = tempDef.GetOuptTypeConfig();
                            for (int i = 0; i < points; i++)
                            {
                                current.Info.Usage |= (ushort)(0x01 << i);
                                current.Info.RangeType |= (ushort)((ushort)outputTypes[i] << (i * 4));
                                current.Info.RangeType |= (ushort)((ushort)rangeTypes[i] << ((i + 2) * 4));
                            }
                        }
                        break;
                    case CclinkTerminalType.BLDC:
                    case CclinkTerminalType.Inverter:
                        {
                            current.Info.IndexDiBuf = m_DiCount;
                            current.Info.IndexDoBuf = m_DoCount;
                            current.Info.IndexAiBuf = m_AiCount;
                            current.Info.IndexAoBuf = m_AoCount;
                        }
                        break;
                }
            }
        }

        //private void MakeTerminalInfo(ref IoTerminal terminal, ref int index)
        //{
        //    int channelCount = terminal.ChannelCount;
        //    for (int channelId = 0; channelId < channelCount; channelId++)
        //    {
        //        IoItem ioItem = terminal.Channels[channelId];

        //        // Master address, Node 별로 type 구분 없이 순차 증가한다.
        //        ioItem.Channel = index++;

        //        // Buffer address, Type별로 Node 구분없이 순차 증가한다.
        //        switch (ioItem.IoType)
        //        {
        //            case IoType.DI:
        //                ioItem.Id = m_DiCount++;
        //                break;
        //            case IoType.DO:
        //                ioItem.Id = m_DoCount++;
        //                break;
        //            case IoType.AI:
        //                ioItem.Id = m_AiCount++;
        //                break;
        //            case IoType.AO:
        //                ioItem.Id = m_AoCount++;
        //                break;
        //        }
        //    }		
        //}

        private void MakeTerminalInfo(ref IoTerminal terminal)
        {
            CClinkStation station = terminal as CClinkStation;

            int channelCount = terminal.ChannelCount;
            for (int channelId = 0; channelId < channelCount; channelId++)
            {
                IoItem ioItem = terminal.Channels[channelId];

                switch (ioItem.IoType)
                {
                    case IoType.DI:
                        ioItem.Channel = station.Info.AddressRx + channelId;
                        ioItem.Id = m_DiCount++;
                        break;
                    case IoType.DO:
                        ioItem.Channel = station.Info.AddressRy + channelId;
                        if (station.Info.TerminalType == CclinkTerminalType.DIO && station.Info.AddressAllocType == AddressAllocType.Inclusive)
                        {
                            ioItem.Channel -= station.Info.Points;
                        }
                        if (station.Info.TerminalType == CclinkTerminalType.BLDC ||
                            station.Info.TerminalType == CclinkTerminalType.Inverter)
                        {
                            ioItem.Channel -= 32;
                        }
                        ioItem.Id = m_DoCount++;
                        break;
                    case IoType.AI:
                        ioItem.Channel = station.Info.AddressRWr + channelId;
                        if (station.Info.TerminalType == CclinkTerminalType.BLDC ||
                            station.Info.TerminalType == CclinkTerminalType.Inverter)
                        {
                            ioItem.Channel -= 32 + 32;
                        }
                        ioItem.Id = m_AiCount++;
                        break;
                    case IoType.AO:
                        ioItem.Channel = station.Info.AddressRWw + channelId;
                        if (station.Info.TerminalType == CclinkTerminalType.BLDC ||
                            station.Info.TerminalType == CclinkTerminalType.Inverter)
                        {
                            ioItem.Channel -= 32 + 32 + 4;
                        }
                        ioItem.Id = m_AoCount++;
                        break;
                }
            }
        }

        private void CreateBuffer()
        {
            // 이 함수는 필히 CreateModuleInfo() 이후에 Call
            m_DiStatus = new bool[m_DiCount];
            m_DoStatus = new bool[m_DoCount];
            m_AiStatus = new int[m_AiCount];
            m_AoStatus = new int[m_AoCount];
            m_DoCmdBuf = new bool[m_DoCount];
            m_AoCmdBuf = new int[m_AoCount];
        }

        private void FormTimerTick(object sender, EventArgs e)
        {
            UpdateIoStatus();
        }

        private void SystemTimerTick(object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateIoStatus();
        }

        private void ThreadingTimerTick(Object stateInfo)
        {
            UpdateIoStatus();
        }

        //public void UpdateIoStatus()
        //{
        //    for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
        //    {
        //        MelsecMaster master = m_Masters[nodeId];

        //        IoNode node = m_ModuleConfig.Container[nodeId];

        //        int terminalCount = node.Terminals.Count;
        //        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
        //        {
        //            IoTerminal terminal = node.Terminals[terminalId];
        //            int channelCount = terminal.ChannelCount;

        //            short stationNo = (short)255;
        //            short networkNo = (short)0;

        //            switch (terminal.IoType)
        //            {
        //                case IoType.DI:
        //                    {
        //                        short byteSize = (short)((channelCount - 1) / 8 + 1);
        //                        short shortSize = (short)((byteSize - 1) / 2 + 1);
        //                        short[] readData = new short[shortSize];
        //                        short rv = master.MdReceive(stationNo, networkNo, devTYPE.devX, (short)terminal.Channels[0].Channel, ref byteSize, ref readData);
        //                        if (rv == 0)
        //                        {
        //                            for (int i = 0; i < channelCount; i++)
        //                            { 
        //                                int index = i / 16;
        //                                bool val = ((readData[index] >> (i % 16)) & 0x01) > 0 ;
        //                                m_DiStatus[terminal.Channels[i].Id] = val;
        //                            }
        //                        }
        //                    }
        //                    break;
        //                case IoType.DO:
        //                    {
        //                        short byteSize = (short)((channelCount - 1) / 8 + 1);
        //                        short shortSize = (short)((byteSize - 1) / 2 + 1);
        //                        short[] writeData = new short[shortSize];
        //                        for (int i = 0; i < channelCount; i++)
        //                        {
        //                            short val = m_DoCmdBuf[terminal.Channels[i].Id] ? (short)1 : (short)0;
        //                            int index = i / 16;
        //                            writeData[index] |= (short)(val << (i % 16));                                    
        //                        }

        //                        master.MdSend(stationNo, networkNo, devTYPE.devY, (short)terminal.Channels[0].Channel, ref byteSize, ref writeData);
        //                    }
        //                    break;
        //            }
        //        }
        //    }            
        //}

        public void UpdateIoStatus()
        {
            for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
            {
                MelsecMaster master = m_Masters[nodeId];

                IoNodeMelsec node = m_ModuleConfig.Container[nodeId] as IoNodeMelsec;

                int terminalCount = node.Terminals.Count;
                for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                {
                    CClinkStation terminal = node.Terminals[terminalId] as CClinkStation;

                    if (terminal.Info.StationType == CclinkStationType.Reserved) continue;

                    int points = terminal.Info.Points;

                    short stationNo = (short)255;
                    short networkNo = (short)0;

                    switch (terminal.Info.TerminalType)
                    {
                        case CclinkTerminalType.DI:
                            {
                                UpdateIoStatus_DI(node, terminal, master, points, stationNo, networkNo);
                            }
                            break;
                        case CclinkTerminalType.DO:
                            {
                                UpdateIoStatus_DO(node, terminal, master, points, stationNo, networkNo);
                            }
                            break;
                        case CclinkTerminalType.DIO:
                            {
                                UpdateIoStatus_DI(node, terminal, master, points, stationNo, networkNo);
                                UpdateIoStatus_DO(node, terminal, master, points, stationNo, networkNo);
                            }
                            break;
                        case CclinkTerminalType.AI:
                            {
                                if (!node.CcLinkMasterInfo.StationInitComp) break;
                                UpdateIoStatus_AI(node, terminal, master, points, stationNo, networkNo);
                            }
                            break;
                        case CclinkTerminalType.AO:
                            {
                                if (!node.CcLinkMasterInfo.StationInitComp) break;
                                UpdateIoStatus_AO(node, terminal, master, points, stationNo, networkNo);
                            }
                            break;
                        case CclinkTerminalType.BLDC:
                        case CclinkTerminalType.Inverter:
                            {
                                if (!node.CcLinkMasterInfo.StationInitComp) break;
                                UpdateIoStatus_DI(node, terminal, master, 32, stationNo, networkNo);
                                UpdateIoStatus_DO(node, terminal, master, 32, stationNo, networkNo);
                                UpdateIoStatus_AI(node, terminal, master, 4, stationNo, networkNo);
                                UpdateIoStatus_AO(node, terminal, master, 4, stationNo, networkNo);
                            }
                            break;
                    }
                }
            }
        }

        private void UpdateIoStatus_DI(IoNodeMelsec node, CClinkStation terminal, MelsecMaster master,
                                      int pointCnt, short stationNo, short networkNo)
        {
            devTYPE devType = node.DevTypeRx;
            int startAddress = terminal.Info.AddressRx;
            int bufferIndex = terminal.Info.IndexDiBuf;
            short byteSize = (short)((pointCnt - 1) / 8 + 1);
            short shortSize = (short)((byteSize - 1) / 2 + 1);
            short[] readData = new short[shortSize];
            short rv = master.MdReceive(stationNo, networkNo, devType, startAddress, ref byteSize, ref readData);
            if (rv == 0)
            {
                for (int i = 0; i < pointCnt; i++)
                {
                    int index = i / 16;
                    bool val = ((readData[index] >> (i % 16)) & 0x01) > 0;
                    m_DiStatus[bufferIndex + i] = val;
                }
            }
        }
        private void UpdateIoStatus_DO(IoNodeMelsec node, CClinkStation terminal, MelsecMaster master,
                                      int pointCnt, short stationNo, short networkNo)
        {
            devTYPE devType = node.DevTypeRy;
            int startAddress = terminal.Info.AddressRy;
            int bufferIndex = terminal.Info.IndexDoBuf;
            short byteSize = (short)((pointCnt - 1) / 8 + 1);
            short shortSize = (short)((byteSize - 1) / 2 + 1);
            short[] writeData = new short[shortSize];
            for (int i = 0; i < pointCnt; i++)
            {
                short val = m_DoCmdBuf[bufferIndex + i] ? (short)1 : (short)0;
                int index = i / 16;
                writeData[index] |= (short)(val << (i % 16));
            }

            //  for DIO
            if (terminal.Info.TerminalType == CclinkTerminalType.DIO && terminal.Info.AddressAllocType == AddressAllocType.Exclusive)
            {
                startAddress += pointCnt;
            }

            master.MdSend(stationNo, networkNo, devType, startAddress, ref byteSize, ref writeData);
        }
        private void UpdateIoStatus_AI(IoNodeMelsec node, CClinkStation terminal, MelsecMaster master,
                                      int pointCnt, short stationNo, short networkNo)
        {
            devTYPE devType = node.DevTypeRWr;
            int startAddress = terminal.Info.AddressRWr;
            int bufferIndex = terminal.Info.IndexAiBuf;
            short byteSize = (short)(pointCnt * 2);
            short shortSize = (short)pointCnt;
            short[] readData = new short[shortSize];
            short rv = master.MdReceive(stationNo, networkNo, devType, startAddress, ref byteSize, ref readData);
            if (rv == 0)
            {
                for (int i = 0; i < pointCnt; i++)
                {
                    int index = i;
                    short val = readData[index];
                    m_AiStatus[bufferIndex + i] = val;
                }
            }
        }
        private void UpdateIoStatus_AO(IoNodeMelsec node, CClinkStation terminal, MelsecMaster master,
                                       int pointCnt, short stationNo, short networkNo)
        {
            devTYPE devType = node.DevTypeRWw;
            int startAddress = terminal.Info.AddressRWw;
            int bufferIndex = terminal.Info.IndexAoBuf;
            short byteSize = (short)(pointCnt * 2);
            short shortSize = (short)pointCnt;
            short[] writeData = new short[shortSize];
            for (int i = 0; i < pointCnt; i++)
            {
                short val = (short)m_AoCmdBuf[bufferIndex + i];
                int index = i;
                writeData[index] = val;
            }

            master.MdSend(stationNo, networkNo, devType, startAddress, ref byteSize, ref writeData);
        }

        public void UpdateCrevisIoStatus()
        {
            for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
            {
                MelsecMaster master = m_Masters[nodeId];    //jemoon : 2011.03.21 recover

                IoNodeMelsec node = m_ModuleConfig.Container[nodeId] as IoNodeMelsec;

                int terminalCount = node.Terminals.Count;
                for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                {
                    CClinkStation terminal = node.Terminals[terminalId] as CClinkStation;

                    if (terminal.Info.StationType == CclinkStationType.Reserved) continue;

                    int points = terminal.Info.Points;

                    short stationNo = (short)255;
                    short networkNo = (short)0;

                    switch (terminal.Info.TerminalType)
                    {
                        case CclinkTerminalType.DI:
                            {
                                if (BaseGlobalVar.Manualcompulsion) break; // 11.02.23 minhan
                                                                           // run이 아닌 상황이라면 읽어 드리지 말고 , 대신 장비는 메뉴얼로 전환을 하고 출력은 주고, device 다운을 걸어 버린다.
                                                                           // 어디까지나 크래비스 상황의 컨셉이니 melsec cclink로 사용시는 이런 시나리오는 적용하지 않는다.

                                devTYPE devType = node.DevTypeRx;
                                int startAddress = terminal.Info.AddressRx;
                                int bufferIndex = terminal.Info.IndexDiBuf;
                                short byteSize = (short)((points - 1) / 8 + 1);
                                short shortSize = (short)((byteSize - 1) / 2 + 1);
                                short[] readData = new short[shortSize];
                                short rv = master.MdReceive(stationNo, networkNo, devType, startAddress, ref byteSize, ref readData);

                                if (rv == 0)
                                {
                                    for (int i = 0; i < points; i++)
                                    {
                                        int index = i / 16;
                                        bool val = ((readData[index] >> (i % 16)) & 0x01) > 0;
                                        m_DiStatus[bufferIndex + i] = val;
                                    }
                                }
                                else // 11.03.17 minhan
                                {
                                    BaseGlobalVar.Manualcompulsion = true;
                                    string msg = string.Format("Crevis DI Error Code : {0:d} : {1:d}", startAddress, rv);
                                    WriteLog(msg);
                                }
                            }
                            break;
                        case CclinkTerminalType.DO:
                            {
                                devTYPE devType = node.DevTypeRy;
                                int startAddress = terminal.Info.AddressRy;
                                int bufferIndex = terminal.Info.IndexDoBuf;
                                short byteSize = (short)((points - 1) / 8 + 1);
                                short shortSize = (short)((byteSize - 1) / 2 + 1);
                                short[] writeData = new short[shortSize];

                                if (!BaseGlobalVar.Manualcompulsion)
                                {
                                    for (int i = 0; i < points; i++)
                                    {
                                        short val = m_DoCmdBuf[bufferIndex + i] ? (short)1 : (short)0;
                                        int index = i / 16;
                                        writeData[index] |= (short)(val << (i % 16));
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < points; i++) // 무조건 0 그러면, 부저 안울리고, 그럴테지만. 메뉴얼 상태에서도 link 에러가 나면 어짜누.엔지니어 설득하삼.
                                    {
                                        //short val = m_DoCmdBuf[bufferIndex + i] ? (short)1 : (short)
                                        int index = i / 16;
                                        writeData[index] |= 0;
                                    }
                                }

                                master.MdSend(stationNo, networkNo, devType, startAddress, ref byteSize, ref writeData);
                            }
                            break;
                        case CclinkTerminalType.DIO:
                            {
                                devTYPE devType = node.DevTypeRx;
                                int startAddress = terminal.Info.AddressRx;
                                int bufferIndex = terminal.Info.IndexDiBuf;
                                short byteSize = (short)((points - 1) / 8 + 1);
                                short shortSize = (short)((byteSize - 1) / 2 + 1);
                                short[] readData = new short[shortSize];


                                if (!BaseGlobalVar.Manualcompulsion) // 11.02.23 minhan
                                {
                                    short rv = master.MdReceive(stationNo, networkNo, devType, startAddress, ref byteSize, ref readData);
                                    if (rv == 0)
                                    {
                                        for (int i = 0; i < points; i++)
                                        {
                                            int index = i / 16;
                                            bool val = ((readData[index] >> (i % 16)) & 0x01) > 0;
                                            m_DiStatus[bufferIndex + i] = val;
                                        }
                                    }
                                    else // 11.03.17 minhan
                                    {
                                        BaseGlobalVar.Manualcompulsion = true;
                                        string msg = string.Format("Crevis DIO Error Code : {0:d} : {1:d}", startAddress, rv);
                                        WriteLog(msg);
                                    }
                                }

                                devType = node.DevTypeRy;
                                startAddress = terminal.Info.AddressRy;
                                bufferIndex = terminal.Info.IndexDoBuf;
                                byteSize = (short)((points - 1) / 8 + 1);
                                shortSize = (short)((byteSize - 1) / 2 + 1);
                                short[] writeData = new short[shortSize];

                                if (!BaseGlobalVar.Manualcompulsion) // 11.02.23 minhan
                                {
                                    for (int i = 0; i < points; i++)
                                    {
                                        short val = m_DoCmdBuf[bufferIndex + i] ? (short)1 : (short)0;
                                        int index = i / 16;
                                        writeData[index] |= (short)(val << (i % 16));
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < points; i++)
                                    {
                                        //short val = m_DoCmdBuf[bufferIndex + i] ? (short)1 : (short)
                                        int index = i / 16;
                                        writeData[index] |= 0;
                                    }
                                }

                                if (terminal.Info.AddressAllocType == AddressAllocType.Exclusive)
                                {
                                    startAddress += points;
                                }

                                master.MdSend(stationNo, networkNo, devType, startAddress, ref byteSize, ref writeData);

                            }
                            break;
                        case CclinkTerminalType.AI:
                            {
                                if (BaseGlobalVar.Manualcompulsion) break; // 11.02.23 minhan

                                devTYPE devType = node.DevTypeRWr;
                                int startAddress = terminal.Info.AddressRWr;
                                int bufferIndex = terminal.Info.IndexAiBuf;
                                short byteSize = (short)(points * 2);
                                short shortSize = (short)points;
                                short[] readData = new short[shortSize];

                                short rv = master.MdReceive(stationNo, networkNo, devType, startAddress, ref byteSize, ref readData);
                                if (rv == 0)
                                {
                                    for (int i = 0; i < terminal.ChannelCount; i++)
                                    {
                                        int index = i;
                                        short val = readData[index];
                                        m_AiStatus[bufferIndex + i] = val;
                                    }
                                }
                                else // 11.03.17 minhan
                                {
                                    BaseGlobalVar.Manualcompulsion = true;
                                    string msg = string.Format("Crevis AI Error Code : {0:d} : {1:d}", startAddress, rv);
                                    WriteLog(msg);
                                }
                            }
                            break;
                        case CclinkTerminalType.AO:
                            {
                                devTYPE devType = node.DevTypeRWw;
                                int startAddress = terminal.Info.AddressRWw;
                                int bufferIndex = terminal.Info.IndexAoBuf;
                                short byteSize = (short)(points * 2);
                                short shortSize = (short)points;
                                short[] writeData = new short[shortSize];

                                if (!BaseGlobalVar.Manualcompulsion) // 11.02.23 minhan
                                {
                                    for (int i = 0; i < terminal.ChannelCount; i++)
                                    {
                                        short val = (short)m_AoCmdBuf[bufferIndex + i];
                                        int index = i;
                                        writeData[index] = val;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < terminal.ChannelCount; i++)
                                    {
                                        //short val = (short)m_AoCmdBuf[bufferIndex + i];
                                        int index = i;
                                        writeData[index] = 0;
                                    }
                                }

                                master.MdSend(stationNo, networkNo, devType, startAddress, ref byteSize, ref writeData);
                            }
                            break;

                        case CclinkTerminalType.BLDC:
                        case CclinkTerminalType.Inverter:
                            //  Crevis Station용 BLDC 제어는 아직 개발되지 않음
                            //  Crevis Station용 Inverter 제어는 아직 개발되지 않음
                            break;
                    }
                }
            }
        }



        public void CheckLinkState()
        {
            for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
            {
                MelsecMaster master = m_Masters[nodeId];    //jemoon : 2011.03.21 recover

                IoNodeMelsec node = m_ModuleConfig.Container[nodeId] as IoNodeMelsec;
                int addressBase = node.AddressBase;
                devTYPE devTypeRx = node.DevTypeRx;
                devTYPE devTypeSw = node.DevTypeSw;
                int addressSw = node.AddressSw;

                bool masterError = false;
                bool masterLinkOk = true;
                bool masterReady = true;

                short stationNo = (short)255;
                short networkNo = (short)0;




                //masterError = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + _CC_ADDRES_MASTER_ERROR));
                //masterLinkOk = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + _CC_ADDRES_MASTER_LINK_OK));
                //masterReady = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + _CC_ADDRES_MASTER_READY));

                node.CcLinkMasterInfo.HasTrouble = (masterError || !masterLinkOk || !masterReady);

                short[] temp = { 0, 0, 0, 0 };
                short size = 4 * 2;
                master.MdReceive(stationNo, networkNo, devTypeSw, (addressSw + _CC_ADDRES_STATION_LINK_STATE), ref size, ref temp);
                for (int i = 0; i < 64; i++)
                {
                    int index = i / 16;
                    node.CcLinkMasterInfo.SationLinkError[i] = ((temp[index] >> (i % 16)) & 0x01) > 0;
                }

                int terminalCount = node.Terminals.Count;
                for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                {
                    CClinkStation terminal = node.Terminals[terminalId] as CClinkStation;
                    terminal.Info.HasTrouble = node.CcLinkMasterInfo.SationLinkError[terminal.Info.StationId];
                }

            }
        }

        public void CheckCrevisLinkState()
        {
            for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
            {
                MelsecMaster master = m_Masters[nodeId];    //jemoon : 2011.03.21 recover

                if (!master.IsOpened) continue; // 11.02.20 minhan

                IoNodeMelsec node = m_ModuleConfig.Container[nodeId] as IoNodeMelsec;
                int addressBase = node.AddressBase;
                devTYPE devTypeRx = node.DevTypeRx;
                devTYPE devTypeSw = node.DevTypeSw;
                int addressSw = node.AddressSw;

                bool masterError = false;
                bool masterLinkOk = true;
                bool masterReady = true;
                bool ErrLink = false; // 11.02.23 minhan

                short stationNo = (short)255;
                short networkNo = (short)0;




                //masterError = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + _CC_ADDRES_MASTER_ERROR));
                //masterLinkOk = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + _CC_ADDRES_MASTER_LINK_OK));
                //masterReady = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + _CC_ADDRES_MASTER_READY));

                node.CcLinkMasterInfo.HasTrouble = (masterError || !masterLinkOk || !masterReady);

                short[] temp = { 0, 0, 0, 0 };
                short size = 4 * 2;
                master.MdReceive(stationNo, networkNo, devTypeSw, (addressSw + _CC_ADDRES_STATION_LINK_STATE), ref size, ref temp);
                for (int i = 0; i < 64; i++)
                {
                    int index = i / 16;
                    node.CcLinkMasterInfo.SationLinkError[i] = ((temp[index] >> (i % 16)) & 0x01) > 0;
                }

                int terminalCount = node.Terminals.Count;
                for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                {
                    CClinkStation terminal = node.Terminals[terminalId] as CClinkStation;
                    terminal.Info.HasTrouble = node.CcLinkMasterInfo.SationLinkError[terminal.Info.StationId];
                    if (terminal.Info.HasTrouble && !ErrLink) // 11.02.23 minhan
                    {
                        ErrLink = true;
                    }
                }

                if (!ErrLink) // 11.02.24 minhan 
                {
                    if (node.ReadyBitUse && (node.ReadyBit != null)) // 11.03.17 minhan
                    {
                        int m_ReadyCnt = node.ReadyBit.Length;
                        int m_ReadyBit = 0;

                        if (m_ReadyCnt != 0)
                        {
                            for (int i = 0; i < m_ReadyCnt; i++)
                            {
                                m_ReadyBit = node.ReadyBit[i];
                                masterLinkOk = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + m_ReadyBit));

                                if (!masterLinkOk)
                                {
                                    m_CrevisErrCode = m_ReadyBit; // Error
                                    return;
                                }
                                masterLinkOk = master.MdReceiveBit(stationNo, networkNo, devTypeRx, (addressBase + (m_ReadyBit - 1))); // Error bit 11.03.24 minhan

                                if (masterLinkOk)
                                {
                                    m_CrevisErrCode = m_ReadyBit - 1; // Error
                                    return;
                                }

                                if (m_CrevisErrCode != 0)
                                {
                                    m_CrevisErrCode = 0;
                                }
                            }
                        }
                        else if (m_CrevisErrCode != 0)
                        {
                            m_CrevisErrCode = 0;
                        }
                    }
                    else if (m_CrevisErrCode != 0)
                    {
                        m_CrevisErrCode = 0;
                    }
                }
            }
        }

        public void SeqInitStation()
        {
            for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
            {
                m_SeqInitStation[nodeId].Do();
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

        public void ActiveStateMonitor() // 11.02.20 minhan
        {
            for (int i = 0; i < m_MasterCount; i++)
            {
                ActiveStateMonitor(i);
            }
        }

        public void ActiveStateMonitor(int masterId) // 11.02.20 minhan
        {
            if (!m_Masters[masterId].IsOpened)
            {
                if (this.DeviceState != ActiveState.Stop)
                {
                    this.DeviceState = ActiveState.Stop;
                    string msg = string.Format("{0:d} : Connect fail", m_Masters[masterId].ChannelNo);
                    WriteLog(msg);
                }
            }
            else
            {
                for (int nodeId = 0; nodeId < m_MasterCount; nodeId++)
                {
                    MelsecMaster master = m_Masters[nodeId];    //jemoon : 2011.03.21 recover

                    IoNodeMelsec node = m_ModuleConfig.Container[nodeId] as IoNodeMelsec;


                    int terminalCount = node.Terminals.Count;
                    for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                    {
                        CClinkStation terminal = node.Terminals[terminalId] as CClinkStation;
                        if (terminal.Info.HasTrouble)
                        {
                            if (!BaseGlobalVar.Manualcompulsion)
                            {
                                BaseGlobalVar.Manualcompulsion = true;
                                string msg = string.Format("{0:d} : Link Error", m_Masters[masterId].ChannelNo);
                                WriteLog(msg);
                            }
                            return;
                        }
                    }

                    if (!BaseGlobalVar.Manualcompulsion && (m_CrevisErrCode != 0)) // 11.02.23 minhan Melsec CClink랑은 정상인데 Crevis랑 에러인 상황 뭐 이런 상황이 다 있는지
                    {
                        BaseGlobalVar.Manualcompulsion = true;
                        string msg = string.Format("{0:d} : Crevis Error Code : {1:d}", m_Masters[masterId].ChannelNo, m_CrevisErrCode);
                        WriteLog(msg);
                        return;
                    }
                }

                if (!BaseGlobalVar.Manualcompulsion && (this.DeviceState != ActiveState.Run)) // 11.02.23 minhan
                {
                    this.DeviceState = ActiveState.Run;
                    string msg = string.Format("{0:d} : Run", m_Masters[masterId].ChannelNo);
                    WriteLog(msg);
                }
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
            return m_DiStatus[index];
        }

        public bool ReadDoSync(int index)
        {
            return ReadDoAsync(index);
        }

        public bool ReadDoAsync(int index)
        {
            return m_DoCmdBuf[index];
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
            return (ushort)m_AoCmdBuf[index];
        }

        public void WriteDiSync(int index, bool val)
        {
            if (m_Simul.Device)
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
            m_DoCmdBuf[index] = val;
        }

        public void WriteAiSync(int index, short val)
        {
            if (m_Simul.Device)
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
            m_AoCmdBuf[index] = val;
        }

        public void Uninitialize()
        {
            m_Uninitializing = true;

            if (m_Simul.Device)
            {
                m_Initialized = false;
            }
            else
            {
                switch (m_WatchMode)
                {
                    case WatchMode.Thread:
                        {
                            if (m_ThreadWatch != null) m_ThreadWatch.Pause();
                        }
                        break;
                    case WatchMode.FormTimer:
                        {
                            if (m_FormTimer != null) m_FormTimer.Dispose();
                        }
                        break;
                    case WatchMode.SystemTimer:
                        {
                            if (m_SystemTimer != null) m_SystemTimer.Dispose();
                        }
                        break;
                    case WatchMode.ThreadingTimer:
                        {
                            if (m_ThreadingTimer != null) m_ThreadingTimer.Dispose();
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

    public class SeqInitStation : XSeqFunction
    {
        //private MelsecMaster m_Master;
        private MelsecMaster m_Master; // 11.02.19 minhan
        private IoNodeMelsec m_Node;

        //public SeqInitStation(MelsecMaster master, IoNodeMelsec node)
        public SeqInitStation(MelsecMaster master, IoNodeMelsec node) // 11.02.19 minhan
        {
            m_Master = master;
            m_Node = node;
        }

        public override int Do()
        {
            int seqNo = m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (!m_Node.CcLinkMasterInfo.StationInitComp)
                    {
                        seqNo = 10;
                    }
                    break;
                case 10:
                    {
                        int terminalCount = m_Node.Terminals.Count;
                        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                        {
                            CClinkStation station = m_Node.Terminals[terminalId] as CClinkStation;
                            CclinkTerminalType terminalType = station.Info.TerminalType;

                            if (terminalType != CclinkTerminalType.AI && terminalType != CclinkTerminalType.AO) continue;

                            int masterId = station.Info.MasterNo - 1;
                            int stationNo = station.Info.StationNo;
                            int addressRx = station.Info.AddressRx;
                            int addressRy = station.Info.AddressRy;
                            int addressRWr = station.Info.AddressRWr;
                            int points = station.Info.Points;

                            devTYPE devTypeRx = m_Node.CcLinkMasterInfo.DevTypeRx;
                            devTYPE devTypeRy = m_Node.CcLinkMasterInfo.DevTypeRy;
                            devTYPE devTypeRWw = m_Node.CcLinkMasterInfo.DevTypeRWw;

                            short stNo = 255;
                            short netNo = 0;
                            bool remoteReady = m_Master.MdReceiveBit(stNo, netNo, devTypeRx, addressRx + MelsecCCLink._CC_ADDRES_REMOTE_READY);
                            bool masterError = m_Node.CcLinkMasterInfo.HasTrouble;
                            bool stationLinkError = station.Info.HasTrouble;

                            if (masterError || stationLinkError)
                            {
                                m_Node.CcLinkMasterInfo.StationInitReq = false;
                                m_Node.CcLinkMasterInfo.StationInitComp = false;
                                m_Node.CcLinkMasterInfo.StationInitFail = true;
                            }
                            else
                            {
                                short[] data = { 0 };
                                short size = 2;
                                if (terminalType == CclinkTerminalType.AI)
                                {
                                    //Usage
                                    data[0] = (short)station.Info.Usage;
                                    m_Master.MdSend(stNo, netNo, devTypeRWw, addressRWr + MelsecCCLink._CC_ADDRES_AI_USAGE, ref size, ref data);
                                    //RangeType
                                    data[0] = (short)station.Info.RangeType;
                                    m_Master.MdSend(stNo, netNo, devTypeRWw, addressRWr + MelsecCCLink._CC_ADDRES_AI_RANGE_TYPE, ref size, ref data);
                                    //MovingAvg
                                    data[0] = (short)station.Info.MovingAvg;
                                    m_Master.MdSend(stNo, netNo, devTypeRWw, addressRWr + MelsecCCLink._CC_ADDRES_AI_MOV_AVG, ref size, ref data);
                                    //MovingAvgRun
                                    for (int i = 0; i < points; i++)
                                    {
                                        m_Master.MdSendBit(stNo, netNo, devTypeRy, addressRy + MelsecCCLink._CC_ADDRES_AI_MOV_AVG_START + i, true);
                                    }
                                    //Init Request
                                    m_Master.MdSendBit(stNo, netNo, devTypeRy, addressRy + MelsecCCLink._CC_ADDRES_STATION_INIT, true);
                                }
                                else
                                {
                                    ////Usage
                                    //data[0] = (short)station.Info.Usage;
                                    //m_Master.MdSend(stNo, netNo, devTypeRWw, addressRWr + MelsecCCLink._CC_ADDRES_AO_USAGE, ref size, ref data);
                                    ////RangeType & Hold or Clear
                                    //data[0] = (short)station.Info.RangeType;
                                    //m_Master.MdSend(stNo, netNo, devTypeRWw, addressRWr + MelsecCCLink._CC_ADDRES_AO_RANGE_TYPE, ref size, ref data);
                                    ////Output Enable
                                    //for (int i = 0; i < points; i++)
                                    //{
                                    //    m_Master.MdSendBit(stNo, netNo, devTypeRy, addressRy + MelsecCCLink._CC_ADDRES_AO_ENABLE + i, true);

                                    //}
                                    //Init Request
                                    //m_Master.MdSendBit(stNo, netNo, devTypeRy, addressRy + MelsecCCLink._CC_ADDRES_STATION_INIT, true);
                                }
                            }
                        }

                        if (m_Node.CcLinkMasterInfo.StationInitFail)
                        {
                            seqNo = 1000;
                        }
                        else
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        bool allComp = true;
                        int terminalCount = m_Node.Terminals.Count;
                        for (int terminalId = 0; terminalId < terminalCount; terminalId++)
                        {
                            CClinkStation station = m_Node.Terminals[terminalId] as CClinkStation;
                            CclinkTerminalType terminalType = station.Info.TerminalType;

                            if (terminalType != CclinkTerminalType.AI && terminalType != CclinkTerminalType.AO) continue;

                            int masterId = station.Info.MasterNo - 1;
                            int stationNo = station.Info.StationNo;
                            int addressRx = station.Info.AddressRx;
                            int addressRy = station.Info.AddressRy;
                            int addressRWr = station.Info.AddressRWr;
                            int points = station.Info.Points;

                            devTYPE devTypeRx = m_Node.CcLinkMasterInfo.DevTypeRx;
                            devTYPE devTypeRy = m_Node.CcLinkMasterInfo.DevTypeRy;
                            devTYPE devTypeRWw = m_Node.CcLinkMasterInfo.DevTypeRWw;

                            short stNo = 255;
                            short netNo = 0;
                            bool remoteReady = m_Master.MdReceiveBit(stNo, netNo, devTypeRx, addressRx + MelsecCCLink._CC_ADDRES_REMOTE_READY);
                            bool masterError = m_Node.CcLinkMasterInfo.HasTrouble;
                            bool stationLinkError = station.Info.HasTrouble;

                            if (masterError || stationLinkError)
                            {
                                m_Node.CcLinkMasterInfo.StationInitReq = false;
                                m_Node.CcLinkMasterInfo.StationInitComp = false;
                                m_Node.CcLinkMasterInfo.StationInitFail = true;
                            }
                            else
                            {
                                bool initComp = m_Master.MdReceiveBit(stNo, netNo, devTypeRx, addressRx + MelsecCCLink._CC_ADDRES_STATION_INIT);
                                //if (initComp && !station.Info.InitComp)
                                //{
                                //    station.Info.InitComp = true;
                                //    m_Master.MdSendBit(stNo, netNo, devTypeRy, addressRy + MelsecCCLink._CC_ADDRES_STATION_INIT, false);
                                //}



                                allComp &= true; // station.Info.InitComp;
                            }
                        }

                        if (allComp)
                        {
                            m_Node.CcLinkMasterInfo.StationInitComp = true;
                            seqNo = 0;
                        }
                        else if (this.GetElapsedTicks() > 1000)
                        {
                            m_Node.CcLinkMasterInfo.StationInitFail = true;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    {
                        if (m_Node.CcLinkMasterInfo.StationInitReq)
                        {
                            m_Node.CcLinkMasterInfo.StationInitReq = false;
                            m_Node.CcLinkMasterInfo.StationInitFail = false;
                            m_Node.CcLinkMasterInfo.StationInitComp = false;
                            seqNo = 0;
                        }
                    }
                    break;
            }

            m_SeqNo = seqNo;

            return -1;
        }
    }
}
