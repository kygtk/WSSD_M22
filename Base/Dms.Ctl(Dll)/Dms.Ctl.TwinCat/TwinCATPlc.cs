////////////////////////////////////////////////////////////////////////////////////////
// * fileName : TwinCATPlc.cs
// * description : 
// * written by : eun
// * date : 2010.08.25
//-------------------------------------------------------------------------
// Revison History
//    
///////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Windows.Forms;
using TwinCAT.Ads;
using Dms.Common;
using Dms.Util.IODefine;

namespace Dms.Ctl
{
    public class TwinCATPlc : ICtlDevice
    {
        #region Singleton
        public static readonly TwinCATPlc Instance = new TwinCATPlc();
        #endregion

        #region Fields
        private bool m_Initialized = false;
        private bool m_Uninitializing = false;
        private IoUpdateMode m_UpdateMode = IoUpdateMode.Polling;
        private ActiveState m_DeviceState;
        private TcAdsClient m_AdsRouter;
        private TcAdsClient[] m_AdsClient;
        private int[] m_PortNo;
        private TcSystemServerClass m_TcSvr = null;
        //private TcAdsSymbolInfoLoader m_SymbolLoader = null;
        private AdsState m_AdsState = AdsState.Stop;
        private XLog m_Log = new XLog("TwinCatPlcLog", XLog.LogStampType.UseStamp);

        private string m_AmsNetId;
        private int m_PlcPortId = 0;
        //private int m_IoPortId = 1;
        //private int m_NcPortId = 0;
        private int m_PortCount;
        private int m_ClientCount;

        private ThreadTcPlcWatch m_ThreadTcWatch;
        private Simul m_Simul;

        //////////////////////////////////////////////////////////////////////////////////////////////////
        //TwinCATPLC Variables
        private byte[] m_InStatus;
        private byte[] m_OutStatus;
        private byte[] m_OutCmdBuf;

        private AdsStream m_InReadStream;
        private AdsStream m_OutReadStream;
        private AdsStream m_OutWriteStream;

        private BinaryReader m_InReader;
        private BinaryReader m_OutReader;
        private BinaryWriter m_OutWriter;

        private int m_InStartOffset = 0;
        private int m_OutStartOffset = 2000;
        private int m_InSize = 2000;
        private int m_OutSize = 2000;
        private int _PlcRWMB = (int)AdsReservedIndexGroups.PlcRWMB;

        //////////////////////////////////////////////////////////////////////////////////////////////////
        //TwinCATPLC DI/DO/AI/AO
        private bool[] m_DiStatus = null;
        private bool[] m_DoStatus = null;
        private short[] m_AiStatus = null;
        private ushort[] m_AoStatus = null;

        private AdsStream m_DiStream = null;
        private AdsStream m_DoStream = null;
        private AdsStream m_AiStream = null;
        private AdsStream m_AoStream = null;
        private AdsStream m_DoStreamPollWrite = null;
        private AdsStream m_AoStreamPollWrite = null;
        private BinaryReader m_DiBinReader = null;
        private BinaryReader m_DoBinReader = null;
        private BinaryReader m_AiBinReader = null;
        private BinaryReader m_AoBinReader = null;
        private BinaryWriter m_DoBinWriter = null;
        private BinaryWriter m_AoBinWriter = null;

        private int m_DiStartOffset = 0;
        private int m_DoStartOffset = 0;
        private int m_AiStartOffset = 0;
        private int m_AoStartOffset = 0;
        private int m_DiSize = 0;
        private int m_DoSize = 0;
        private int m_AiSize = 0;
        private int m_AoSize = 0;

        private int m_DiCount;
        private int m_DoCount;
        private int m_AiCount;
        private int m_AoCount;

        /////////////////////////////////////////////////////////////////////////////////
        //각 Ads Stream 의 Size
        private int m_DiMemorySize;
        private int m_DoMemorySize;
        private int m_AiMemorySize;
        private int m_AoMemorySize;
        #endregion

        #region Properties
        public IoUpdateMode UpdateMode
        {
            get { return m_UpdateMode; }
        }
        public ActiveState DeviceState
        {
            get { return m_DeviceState; }
            set { m_DeviceState = value; }
        }
        public string ControllerState
        {
            get { return m_AdsState.ToString(); }
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

        public int InStartOffset
        {
            get { return m_InStartOffset; }
            set { m_InStartOffset = value; }
        }
        public int OutStartOffset
        {
            get { return m_OutStartOffset; }
            set { m_OutStartOffset = value; }
        }
        public int InSize
        {
            get { return m_InSize; }
            set { m_InSize = value; }
        }
        public int OutSize
        {
            get { return m_OutSize; }
            set { m_OutSize = value; }
        }
        public string AmsNetId
        {
            get { return m_AmsNetId; }
            set { m_AmsNetId = value; }
        }
        public byte[] InStatus
        {
            get { return m_InStatus; }
            set { m_InStatus = value; }
        }
        public byte[] OutStatus
        {
            get { return m_OutStatus; }
            set { m_OutStatus = value; }
        }
        #endregion

        #region Event
        public event IoStateChangeEventHandler OnIoStateChange;
        #endregion

        #region Constructor
        private TwinCATPlc()
        {
            m_Simul = AppConfig.Instance.Simul;

            WriteTwinCatMasterKey();
        }
        #endregion

        #region Method
        public void WriteTwinCatMasterKey()
        {
            //string path = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Beckhoff\\TwinCAT\\";
            //string version = "2.10.000";
            //string valueName = "Serial";
            //string masterKey = "CE50-BF54-4A18-DC40";

            //Registry.SetValue(path + version, valueName, masterKey);
        }

        public void WriteLog(string msg)
        {
            m_Log.TextOut(msg);
        }

        private DmsErrors CreateClient()
        {
            if (m_Simul.IoController) return DmsErrors.Success;

            for (int i = 0; i < m_PortCount; i++)
            {
                m_AdsClient[i] = new TcAdsClient();
                m_AdsClient[i].Connect(m_AmsNetId, m_PortNo[i]);

                if (m_AdsClient[i].IsConnected == false)
                {
                    string msg = "TwinCAT Connection failed!";
                    WriteLog(msg);
                    MessageBox.Show(msg);

                    return DmsErrors.InternalError;
                }
            }

            return DmsErrors.Success;
        }

        private void DisposeClient()
        {
            if (m_Simul.IoController) return;

            for (int i = 0; i < m_PortCount; i++)
            {
                m_AdsClient[i].Dispose();
            }

            //m_HandleNotification.Clear();
            //m_HandlePortId.Clear();
        }

        private VarType GetVarType(TcAdsSymbolInfo info)
        {
            int offset = (int)info.IndexOffset;
            long group = info.IndexGroup;
            {
                //if (offset >= m_InStartOffset && offset < (m_InStartOffset + m_InSize)) return TwinCATPlcType.In;
                //else if (offset >= m_OutStartOffset && offset < (m_OutStartOffset + m_OutSize)) return TwinCATPlcType.Out;
                //else return TwinCATPlcType.None;

                if (group == (long)AdsReservedIndexGroups.PlcRWMB && offset >= m_InStartOffset && offset < (m_InStartOffset + m_InSize)) return VarType.In;
                else if (group == (long)AdsReservedIndexGroups.PlcRWMB && offset >= m_OutStartOffset && offset < (m_OutStartOffset + m_OutSize)) return VarType.Out;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWIX && offset >= (m_DiStartOffset * 8) && offset < ((m_DiStartOffset * 8) + m_DiSize)) return VarType.Di;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWOX && offset >= (m_DoStartOffset * 8) && offset < ((m_DoStartOffset * 8) + m_DoSize)) return VarType.Do;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWIB && offset >= m_AiStartOffset && offset < (m_AiStartOffset + m_AiSize)) return VarType.Ai;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWOB && offset >= m_AoStartOffset && offset < (m_AoStartOffset + m_AoSize)) return VarType.Ao;
                else return VarType.None;
            }
        }


        private void CreateSymbol()
        {
            /*if (m_Simul.IoController) return;
            m_InCollection.Clear();
            m_OutCollection.Clear();
            int i = 0;
            //for (int i = 0; i < m_PortCount; i++)
            {
                m_SymbolLoader = m_AdsClient[i].CreateSymbolInfoLoader();
                //int SymbolCount = m_SymbolLoader.GetSymbolCount(true);

                foreach (TcAdsSymbolInfo symbol in m_SymbolLoader)
                {
                    switch (GetVarType(symbol))
                    {
                        case VarType.Di: m_DiCount++; break;
                        case VarType.Do: m_DoCount++; break;
                        case VarType.Ai: m_AiCount++; break;
                        case VarType.Ao: m_AoCount++; break;
                    }

                    switch (symbol.IndexGroup)
                    {
                        case (long)AdsReservedIndexGroups.PlcRWMB:
                            {
                                VarType type = GetVarType(symbol);
                                if (type == VarType.None) continue;

                                if (symbol.Parent == null)
                                {
                                    if (symbol.SubSymbolCount == 0)
                                    {
                                        //parent x, subsymbol x => terminal 1ea, io 1ea
                                        VariableInfo info = new VariableInfo();
                                        info.Name = symbol.Name;
                                        info.Size = symbol.Size;
                                        info.Offset = (int)symbol.IndexOffset;
                                        info.DataType = (int)symbol.Datatype;
                                        info.VarType = type;
                                        if (type == VarType.In)
                                        {
                                            if (symbol.Type == "BOOL") m_DiCount++;
                                            else m_AiCount++;
                                            info.Id = info.Offset - m_InStartOffset;
                                            m_InCollection.Items.Add(info);
                                        }
                                        else
                                        {
                                            if (symbol.Type == "BOOL") m_DoCount++;
                                            else m_AoCount++;
                                            info.Id = info.Offset - m_OutStartOffset;
                                            m_OutCollection.Items.Add(info);
                                        }
                                    }
                                }
                                else
                                {
                                    if (symbol.SubSymbolCount == 0)
                                    {
                                        //parent o, subsymbol x => item 1ea
                                        VariableInfo info = new VariableInfo();
                                        info.Name = symbol.Name;
                                        info.Size = symbol.Size;
                                        info.Offset = (int)symbol.IndexOffset;
                                        info.DataType = (int)symbol.Datatype;
                                        info.VarType = type;
                                        if (type == VarType.In)
                                        {
                                            if (symbol.Type == "BOOL") m_DiCount++;
                                            else m_AiCount++;
                                            info.Id = info.Offset - m_InStartOffset;
                                            m_InCollection.Items.Add(info);
                                        }
                                        else
                                        {
                                            if (symbol.Type == "BOOL") m_DoCount++;
                                            else m_AoCount++;
                                            info.Id = info.Offset - m_OutStartOffset;
                                            m_OutCollection.Items.Add(info);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }*/
        }

        private void CreateBuffer()
        {
            m_InStatus = new byte[m_InSize];
            m_OutStatus = new byte[m_OutSize];
            m_OutCmdBuf = new byte[m_OutSize];

            m_InReadStream = new AdsStream(m_InStatus);
            m_OutReadStream = new AdsStream(m_OutStatus);
            m_OutWriteStream = new AdsStream(m_OutCmdBuf);

            m_InReader = new BinaryReader(m_InReadStream);
            m_OutReader = new BinaryReader(m_OutReadStream);
            m_OutWriter = new BinaryWriter(m_OutWriteStream);

            m_DiStatus = new bool[m_DiSize];
            m_DoStatus = new bool[m_DoSize];
            m_AiStatus = new short[m_AiSize];
            m_AoStatus = new ushort[m_AoSize];

            //Data size 계산
            //DI/DO는 byte size, bool size 전환 필요함
            m_DiMemorySize = /*m_DiSize;//*/ (m_DiSize - 1) / 8 + 1;
            m_DoMemorySize = /*m_DoSize;//*/ (m_DoSize - 1) / 8 + 1;
            m_AiMemorySize = m_AiSize * 2;
            m_AoMemorySize = m_AoSize * 2;

            m_DiStream = new AdsStream(m_DiMemorySize);
            m_DoStream = new AdsStream(m_DoMemorySize);
            m_AiStream = new AdsStream(m_AiMemorySize);// new AdsStream(m_AiMemorySize * 2);
            m_AoStream = new AdsStream(m_AoMemorySize);// new AdsStream(m_AoMemorySize * 2);
            m_DoStreamPollWrite = new AdsStream(m_DoMemorySize);
            m_AoStreamPollWrite = new AdsStream(m_AoMemorySize);// new AdsStream(m_AoMemorySize * 2);

            m_DiBinReader = new BinaryReader(m_DiStream);
            m_DoBinReader = new BinaryReader(m_DoStream);
            m_AiBinReader = new BinaryReader(m_AiStream);
            m_AoBinReader = new BinaryReader(m_AoStream);
            m_DoBinWriter = new BinaryWriter(m_DoStreamPollWrite);
            m_AoBinWriter = new BinaryWriter(m_AoStreamPollWrite);
        }

        public DmsErrors Initialize(IoUpdateMode updateMode, params int[] portNo)
        {
            if (m_Initialized)
            {
                return DmsErrors.Success;
            }

            try
            {
                //jemoon : TwinCAT 미설치시
                if (m_Simul.IoController)
                {
                    m_UpdateMode = updateMode;

                    CreateBuffer();

                    m_DeviceState = ActiveState.Run;

                    m_Initialized = true;

                    MessageBox.Show(string.Format("System run in {0} Simulation mode!!", this.GetType().Name));

                    return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
                }
                else
                {
                    m_UpdateMode = updateMode;
                    m_PortNo = new int[portNo.Length];
                    m_PortCount = m_PortNo.Length;
                    for (int i = 0; i < m_PortCount; i++)
                    {
                        m_PortNo[i] = portNo[i];
                    }

                    m_TcSvr = new TcSystemServerClass();

                    TcStart();

                    if (m_TcSvr.SystemState == (int)AdsState.Stop)
                    {
                        string msg = "TwinCAT Start failed";
                        WriteLog(msg);
                        MessageBox.Show(msg);
                        //Application.Exit();
                        return DmsErrors.InternalError;
                    }
                    else
                    {
                        if (m_Simul.Device)
                        {
                            m_AmsNetId = m_TcSvr.AmsNetId;
                        }
                        m_AdsRouter = new TcAdsClient();
                        m_AdsRouter.Connect(m_AmsNetId, m_PortNo[0]);
                        AddRouterNotification();

                        m_AdsClient = new TcAdsClient[m_PortCount];
                        m_ClientCount = m_AdsClient.Length;
                        if (CreateClient() == DmsErrors.Success)
                        {
                            //CreateSymbol();    //해줄필요없다
                            CreateBuffer();

                            //m_DeviceState = (m_TcSvr.SystemState == (int)AdsState.Run) ? ActiveState.Run : ActiveState.UnKnown;
                            m_DeviceState = (m_AdsRouter.RouterState == AmsRouterState.Start) ? ActiveState.Run : ActiveState.UnKnown;
                            //m_AdsState = m_AdsRouter.ReadState().AdsState;   //연결안됐을때 에러난다.

                            m_ThreadTcWatch = new ThreadTcPlcWatch(this);
                            m_ThreadTcWatch.Start();
                            m_Initialized = true;
                        }

                        return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
                    }
                }
            }
            catch (Exception err)
            {
                string msg = "TwinCAT Start failed";
                WriteLog(msg);
                MessageBox.Show(msg);

                msg = err.ToString();
                WriteLog(msg);
                //Application.Exit();
                return DmsErrors.InternalError;
            }
        }

        private void AddDeviceNotification()
        {
            m_AdsStream = new AdsStream(2);
            m_AdsBinReader = new BinaryReader(m_AdsStream);
            m_NotificationHandle = m_AdsRouter.AddDeviceNotification((int)AdsReservedIndexGroups.DeviceData,
                                                                                    (int)AdsReservedIndexOffsets.DeviceDataAdsState,
                                                                                    m_AdsStream,
                                                                                    AdsTransMode.OnChange,
                                                                                    0,
                                                                                    0,
                                                                                    null);

            m_AdsRouter.AdsNotification += new AdsNotificationEventHandler(OnAdsNotification);
        }

        private void AddRouterNotification()
        {
            if (m_Simul.IoController) return;

            m_AdsRouter.AmsRouterNotification += new AmsRouterNotificationEventHandler(AmsRouterNotificationCallback);
        }

        public void TcStart()
        {
            if (m_Simul.IoController) return;

            Object param = null;
            if (m_TcSvr.SystemState == (int)AdsState.Stop)
            {
                TcStop();

                m_TcSvr.StartSystem(param);
            }
        }

        public void TcStop()
        {
            if (m_Simul.IoController) return;

            Object param = null;
            if (m_TcSvr.SystemState != (int)AdsState.Stop)
            {
                m_TcSvr.StopSystem(param);
            }
        }

        public void Uninitialize()
        {
            try
            {
                if (m_Initialized)
                {
                    m_Uninitializing = true;

                    if (!m_Simul.IoController)
                    {
                        //Watch thread가 중지할 수 있는 여유를 줘야 한다.
                        m_ThreadTcWatch.Pause();
                        System.Threading.Thread.Sleep(500);

                        //ClearAllNode();

                        DeleteNotification();

                        for (int i = 0; i < m_ClientCount; i++)
                        {
                            m_AdsClient[i].Dispose();
                        }

                        //TcStop();
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = "TwinCAT Uninitialize failed";
                WriteLog(msg);

                msg = err.ToString();
                WriteLog(msg);
            }

            m_Initialized = false;
        }

        private void DeleteNotification()
        {
            m_AdsRouter.DeleteDeviceNotification(m_NotificationHandle);
        }

        public void ClearAllNode()
        {
            for (int i = 0; i < m_OutSize; i++)
            {
                WriteDoSync(i, false);
            }
        }

        public bool CreateMasterInfo(IoDefines iodefines)
        {
            if (iodefines.BusType != FieldBusType.TwinCATPlc) return false;
            else
            {
                if (iodefines.Container.Count > 0)
                {
                    foreach (IoNode node in iodefines.Container)
                    {
                        IoNodeTwinCATPLC plcNode = (IoNodeTwinCATPLC)node;
                        if (plcNode.Id == (int)VarType.In)
                        {
                            m_InStartOffset = plcNode.Offset;
                            m_InSize = plcNode.Size;
                        }
                        else if (plcNode.Id == (int)VarType.Out)
                        {
                            m_OutStartOffset = plcNode.Offset;
                            m_OutSize = plcNode.Size;
                        }
                        else if (plcNode.Id == (int)VarType.Di)
                        {
                            m_DiStartOffset = plcNode.Offset;
                            m_DiSize = plcNode.Size;
                        }
                        else if (plcNode.Id == (int)VarType.Do)
                        {
                            m_DoStartOffset = plcNode.Offset;
                            m_DoSize = plcNode.Size;
                        }
                        else if (plcNode.Id == (int)VarType.Ai)
                        {
                            m_AiStartOffset = plcNode.Offset;
                            m_AiSize = plcNode.Size;
                        }
                        else if (plcNode.Id == (int)VarType.Ao)
                        {
                            m_AoStartOffset = plcNode.Offset;
                            m_AoSize = plcNode.Size;
                        }
                        m_AmsNetId = plcNode.AmsNetId;
                    }
                }

                m_DiCount = iodefines.DigitalInputs.Count;
                m_DoCount = iodefines.DigitalOutputs.Count;
                m_AiCount = iodefines.AnalogInputs.Count;
                m_AoCount = iodefines.AnalogOutputs.Count;
            }
            return true;
        }

        public void ReadInput()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;

            if (m_InSize <= 0) return;

            m_AdsClient[m_PlcPortId].Read(_PlcRWMB, m_InStartOffset, m_InReadStream, 0, m_InSize);
            m_InReadStream.Position = 0;
            for (int i = 0; i < m_InSize; i++)
            {
                m_InStatus[i] = m_InReader.ReadByte();
            }
        }

        public void ReadOutput()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;

            if (m_OutSize <= 0) return;

            m_AdsClient[m_PlcPortId].Read(_PlcRWMB, m_OutStartOffset, m_OutReadStream, 0, m_OutSize);
            m_OutReadStream.Position = 0;
            for (int i = 0; i < m_OutSize; i++)
            {
                m_OutStatus[i] = m_OutReader.ReadByte();
            }
        }

        public void WriteOutput()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;

            if (m_OutSize <= 0) return;

            bool changed = false;
            for (int i = 0; i < m_OutSize; i++)
            {
                if (m_OutCmdBuf[i] != m_OutStatus[i])
                {
                    changed = true;
                    break;
                }
            }

            if (changed)
            {
                m_OutWriteStream.Position = 0;

                for (int i = 0; i < m_OutSize; i++)
                {
                    m_OutWriter.Write(m_OutCmdBuf[i]);
                }

                m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset, m_OutWriteStream);
            }
        }

        private bool m_IsConnected = false;
        public void MonitorState()
        {
            if (m_Simul.IoController) return;

            //m_AdsState = (AdsState)m_TcSvr.SystemState;

            //if (m_AdsState != AdsState.Run)
            if ((AdsState)m_TcSvr.SystemState == AdsState.Stop)
            {
                TcStart();
            }

            try
            {
                m_AdsState = m_AdsRouter.ReadState().AdsState; //AdsState가 Stop이지만, m_AdsRouter.RouterState는 Start일수 있다.
                m_DeviceState = (m_AdsState != AdsState.Run) ? ActiveState.Stop : ActiveState.Run; //(m_AdsRouter.RouterState != AmsRouterState.Start) ? ActiveState.Stop : ActiveState.Run;
                if (m_IsConnected == false)
                {
                    AddDeviceNotification();
                    m_IsConnected = true;
                }
            }
            catch (Exception err)
            {
                m_AdsState = AdsState.Stop;
                m_DeviceState = ActiveState.Error;
            }
        }

        /* callback function called on state changes of the local AMS router */
        private void AmsRouterNotificationCallback(object sender, AmsRouterNotificationEventArgs e)
        {
            if (m_Simul.IoController) return;

            if (e.State != AmsRouterState.Start)
            {
                m_DeviceState = ActiveState.Stop;
                DisposeClient();
            }
            else
            {
                CreateClient();

                //if (m_UpdateMode == IoUpdateMode.EventDriven)
                //{
                //    AddDeviceNotification();
                //}

                m_DeviceState = ActiveState.Run;
            }
        }

        private int m_NotificationHandle = 0;
        private AdsStream m_AdsStream = null;
        private BinaryReader m_AdsBinReader = null;
        private void OnAdsNotification(object sender, AdsNotificationEventArgs e)
        {
            if (e.NotificationHandle == m_NotificationHandle)
            {
                m_AdsState = (AdsState)m_AdsBinReader.ReadInt16();
                m_DeviceState = (m_AdsRouter.RouterState == AmsRouterState.Start) ? ActiveState.Run : ActiveState.UnKnown;
                m_Log.TextOut(string.Format("AdsState : {0}, DeviceState : {1}", m_AdsState.ToString(), m_DeviceState.ToString()));
            }
        }

        int _IOImangeRWIX = (int)AdsReservedIndexGroups.IOImageRWIX;
        int _IOImageRWIB = (int)AdsReservedIndexGroups.IOImageRWIB;
        public void ReadDi()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_DiCount <= 0) return;

            //m_AdsClient[m_PlcPortId].Read(_IOImangeRWIX, 100/*m_DiStartOffset*/, m_DiStream, 0, m_DiMemorySize);
            m_AdsClient[m_PlcPortId].Read(_IOImageRWIB, m_DiStartOffset, m_DiStream, 0, m_DiMemorySize);
            m_DiStream.Position = 0;

            byte data;
            int bitIndex;
            for (int i = 0; i < m_DiMemorySize; i++)
            {
                data = m_DiBinReader.ReadByte();
                for (int bit = 0; bit < 8; bit++)
                {
                    bitIndex = i * 8 + bit;
                    if (bitIndex == m_DiCount) return;

                    m_DiStatus[bitIndex] = ((data >> bit) & 0x01) > 0;

                }
            }
            //for (int i = 0; i < m_DiSize; i++)
            //{
            //    m_DiStatus[i] = m_DiBinReader.ReadBoolean();
            //}
        }

        int _IOImageRWOX = (int)AdsReservedIndexGroups.IOImageRWOX;
        int _IOImageRWOB = (int)AdsReservedIndexGroups.IOImageRWOB;
        public void ReadDo()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_DoCount <= 0) return;

            //m_AdsClient[m_PlcPortId].Read(_IOImageRWOX, m_DoStartOffset, m_DoStream, 0, m_DoSize);
            m_AdsClient[m_PlcPortId].Read(_IOImageRWOB, m_DoStartOffset, m_DoStream, 0, m_DoMemorySize);
            m_DoStream.Position = 0;

            byte data;
            int bitIndex;
            for (int i = 0; i < m_DoMemorySize; i++)
            {
                data = m_DoBinReader.ReadByte();
                //jemoon : 090831 - WriteDo() 에서 CurrentStatus를 Byte 형태로 비교하기 위해서
                //m_DoStatusByte[i] = data;

                for (int bit = 0; bit < 8; bit++)
                {
                    bitIndex = i * 8 + bit;
                    if (bitIndex == m_DoCount) return;

                    m_DoStatus[bitIndex] = ((data >> bit) & 0x01) > 0;
                }
            }

            //for (int i = 0; i < m_DoSize; i++)
            //{
            //    m_DoStatus[i] = m_DoBinReader.ReadBoolean();
            //}
        }

        public void ReadAi()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_AiCount <= 0) return;

            m_AdsClient[m_PlcPortId].Read(_IOImageRWIB, m_AiStartOffset, m_AiStream);
            m_AiStream.Position = 0;
            for (int i = 0; i < m_AiSize; i++)
            {
                m_AiStatus[i] = m_AiBinReader.ReadInt16();
            }
        }

        public void ReadAo()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_AoCount <= 0) return;

            m_AdsClient[m_PlcPortId].Read(_IOImageRWOB, m_AoStartOffset, m_AoStream);
            m_AoStream.Position = 0;
            for (int i = 0; i < m_AoSize; i++)
            {
                m_AoStatus[i] = m_AoBinReader.ReadUInt16();

                // jemoon : 090911 - Event Driven case 제거
                // Polling 이면 Event Fire 하지 않으므로 필요아래 부분 필요없다
                //// Fire event...
                //if (m_UpdateMode == IoUpdateMode.EventDriven)
                //{
                //    if (!m_AoStatusInit || m_AoStatusOld[i] != m_AoStatus[i])
                //    {
                //        m_AoStatusInit = true;
                //        m_AoStatusOld[i] = m_AoStatus[i];

                //        IoStateChangeEventHandler eHandle = OnIoStateChange;
                //        if (eHandle != null)
                //        {
                //            OnIoStateChange(this, new IoStateEventArgs(IoType.AO, i));
                //        }
                //    }
                //}
            }
        }

        // ViewIOEdit에서 호출됨.
        // VarType.In, Out의 경우 IoType.DI, DO일때는 bool형식으로 반환. IoType.AI, AO의 경우에는 string형식으로 반환.
        public object Read(int index, IoType type, int group, int dataType, int node)
        {
            if (m_Initialized == false || m_AdsState != AdsState.Run) return null;

            switch (group)
            {
                case (int)VarType.In:
                    {
                        if (m_InSize <= 0 || m_InSize < index) return null;
                        if (type == IoType.DI) return BitConverter.ToBoolean(m_InStatus, index);
                        else return Read(index, group, dataType, node).ToString();
                    }
                case (int)VarType.Out:
                    {
                        if (m_OutSize <= 0 || m_OutSize < index) return null;
                        if (type == IoType.DO) return BitConverter.ToBoolean(m_OutStatus, index);
                        else return Read(index, group, dataType, node).ToString();
                    }
                case (int)VarType.Di:
                    {
                        if (m_DiSize <= 0 || m_DiSize <= index) return false;
                        return m_DiStatus[index];
                    }
                case (int)VarType.Do:
                    {
                        if (m_DoSize <= 0 || m_DoSize <= index) return false;
                        return m_DoStatus[index];
                    }
                case (int)VarType.Ai:
                    {
                        if (m_AiSize <= 0 || m_AiSize <= index) return 0;
                        return m_AiStatus[index];
                    }
                case (int)VarType.Ao:
                    {
                        if (m_AoSize <= 0 || m_AoSize <= index) return 0;
                        return m_AoStatus[index];
                    }
            }

            return null;
        }

        // IoAnalogInput, IoDigitalInput의 GetStateAs()에서 호출됨. 
        // VarType.In, Out의 경우, 자기의 type에 맞는 형식으로 반환됨
        public object Read(int index, int group, int dataType, int node)
        {
            if (m_Initialized == false || m_AdsState != AdsState.Run) return null;

            switch (group)
            {
                case (int)VarType.In:
                case (int)VarType.Out:
                    {
                        byte[] buf;
                        //VariableInfo info;
                        if (group == (int)VarType.In)
                        {
                            if (m_InSize <= 0 || m_InSize < index) return null;
                            buf = m_InStatus;
                            //info = m_InCollection.GetItem(index);
                        }
                        else
                        {
                            if (m_OutSize <= 0 || m_OutSize < index) return null;
                            buf = m_OutStatus;
                            //info = m_OutCollection.GetItem(index);
                        }

                        //switch (info.DataType)
                        switch (dataType)
                        {
                            case (int)AdsDatatypeId.ADST_BIT:
                                return BitConverter.ToBoolean(buf, index);
                            case (int)AdsDatatypeId.ADST_UINT8:
                                return buf[index];
                            case (int)AdsDatatypeId.ADST_INT16:
                                return BitConverter.ToInt16(buf, index);
                            case (int)AdsDatatypeId.ADST_INT32:
                                return BitConverter.ToInt32(buf, index);
                            case (int)AdsDatatypeId.ADST_UINT16:
                                return BitConverter.ToUInt16(buf, index);
                            case (int)AdsDatatypeId.ADST_STRING:
                                return XFunc.ConvertToString(buf, index, node/* info.Size*/, ByteOrder.BigEndian).Trim();// BitConverter.ToString(buf, offset, 81).ToCharArray();
                            case (int)AdsDatatypeId.ADST_REAL32:
                                return BitConverter.ToSingle(buf, index);
                            case (int)AdsDatatypeId.ADST_UINT32:
                                return BitConverter.ToUInt32(buf, index);
                        }
                        return null;
                    }
                case (int)VarType.Di:
                    {
                        if (m_DiSize <= 0 || m_DiSize <= index) return false;
                        return m_DiStatus[index];
                    }
                case (int)VarType.Do:
                    {
                        if (m_DoSize <= 0 || m_DoSize <= index) return false;
                        return m_DoStatus[index];
                    }
                case (int)VarType.Ai:
                    {
                        if (m_AiSize <= 0 || m_AiSize <= index) return 0;
                        return m_AiStatus[index];
                    }
                case (int)VarType.Ao:
                    {
                        if (m_AoSize <= 0 || m_AoSize <= index) return 0;
                        return m_AoStatus[index];
                    }
            }

            return null;
        }

        // IoAnalogOutput, IoDigitalOutput의 SetStateSync에서 호출됨
        public void Write(int index, object val, int group, int dataType, int node)
        {
            if (m_Initialized == false || m_AdsState != AdsState.Run) return;

            switch (group)
            {
                case (int)VarType.In:
                case (int)VarType.Out:
                    {
                        #region VarType.In, Out
                        //VariableInfo info = null;
                        AdsStream ds;
                        BinaryWriter bw;
                        int offset = 0;

                        if (group == (int)VarType.In)
                        {
                            //info = m_InCollection.GetItem(index);
                            offset = m_InStartOffset;
                        }
                        else
                        {
                            //info = m_OutCollection.GetItem(index);
                            offset = m_OutStartOffset;
                        }

                        //switch (info.DataType)
                        switch (dataType)
                        {
                            case (int)AdsDatatypeId.ADST_BIT:
                                {
                                    bool newVal = Convert.ToBoolean(val);

                                    ds = new AdsStream(1);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_UINT8:
                                {
                                    byte newVal = Convert.ToByte(val);

                                    ds = new AdsStream(1);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_INT16:
                                {
                                    short newVal = Convert.ToInt16(val);
                                    byte[] bytes = BitConverter.GetBytes(newVal);

                                    ds = new AdsStream(2);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_INT32:
                                {
                                    int newVal = Convert.ToInt32(val);
                                    byte[] bytes = BitConverter.GetBytes(newVal);

                                    ds = new AdsStream(4);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_UINT16:
                                {
                                    ushort newVal = Convert.ToUInt16(val);
                                    byte[] bytes = BitConverter.GetBytes(newVal);

                                    ds = new AdsStream(2);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_STRING:
                                {
                                    string newVal = Convert.ToString(val);

                                    ds = new AdsStream(node/*info.Size*/);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_REAL32:
                                {
                                    float newVal = Convert.ToSingle(val);

                                    ds = new AdsStream(4);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                            case (int)AdsDatatypeId.ADST_UINT32:
                                {
                                    uint newVal = Convert.ToUInt32(val);
                                    byte[] bytes = BitConverter.GetBytes(newVal);

                                    ds = new AdsStream(4);
                                    bw = new BinaryWriter(ds);

                                    ds.Position = 0;
                                    bw.Write(newVal);

                                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, offset + index, ds);
                                }
                                break;
                        }
                        #endregion
                    }
                    break;
                case (int)VarType.Di:
                    {
                        bool newValue = Convert.ToBoolean(val);

                        AdsStream ds = new AdsStream(1);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImangeRWIX, (m_DiStartOffset * 8) + index, ds);
                    }
                    break;
                case (int)VarType.Do:
                    {
                        bool newValue = Convert.ToBoolean(val);

                        AdsStream ds = new AdsStream(1);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImageRWOX, (m_DoStartOffset * 8) + index, ds);
                    }
                    break;
                case (int)VarType.Ai:
                    {
                        short newValue = Convert.ToInt16(val);

                        AdsStream ds = new AdsStream(2);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImageRWIB, m_AiStartOffset + (index * 2), ds);
                    }
                    break;
                case (int)VarType.Ao:
                    {
                        ushort newValue = Convert.ToUInt16(val);

                        AdsStream ds = new AdsStream(2);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImageRWOB, m_AoStartOffset + (index * 2), ds);
                    }
                    break;
            }
        }

        // ViewIOEdit에서 호출됨
        public void Write(int index, object val, IoType type, int group, int dataType, int node)
        {
            if (m_Initialized == false || m_AdsState != AdsState.Run) return;

            switch (group)
            {
                case (int)VarType.In:
                case (int)VarType.Out:
                    {
                        Write(index, val, group, dataType, node);
                    }
                    break;
                case (int)VarType.Di:
                    {
                        bool newValue = Convert.ToBoolean(val);

                        AdsStream ds = new AdsStream(1);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImangeRWIX, (m_DiStartOffset * 8) + index, ds);
                    }
                    break;
                case (int)VarType.Do:
                    {
                        bool newValue = Convert.ToBoolean(val);

                        AdsStream ds = new AdsStream(1);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImageRWOX, (m_DoStartOffset * 8) + index, ds);
                    }
                    break;
                case (int)VarType.Ai:
                    {
                        short newValue = Convert.ToInt16(val);

                        AdsStream ds = new AdsStream(2);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImageRWIB, m_AiStartOffset + (index * 2), ds);
                    }
                    break;
                case (int)VarType.Ao:
                    {
                        ushort newValue = Convert.ToUInt16(val);

                        AdsStream ds = new AdsStream(2);
                        BinaryWriter bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newValue);

                        m_AdsClient[m_PlcPortId].Write(_IOImageRWOB, m_AoStartOffset + (index * 2), ds);
                    }
                    break;
            }
        }

        #endregion

        #region ICtlDevice
        public bool Initialized
        {
            get { return m_Initialized; }
            set { m_Initialized = value; }
        }
        public bool Uninitializing
        {
            get { return m_Uninitializing; }
            set { m_Uninitializing = value; }
        }

        public short ReadAiAsync(int index)
        {
            //if (m_InSize <= 0 || m_InSize < index) return 0;
            //return BitConverter.ToInt16(m_InStatus, index);
            if (m_AiSize <= 0 || m_AiSize <= index) return 0;
            return m_AiStatus[index];
        }

        public short ReadAiSync(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ushort ReadAoAsync(int index)
        {
            //if (m_OutSize <= 0 || m_OutSize < index) return 0;
            //return BitConverter.ToUInt16(m_OutStatus, index);

            if (m_AoSize <= 0 || m_AoSize <= index) return 0;
            return m_AoStatus[index];
        }

        public ushort ReadAoSync(int index)
        {
            ushort val = 0;
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return val;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadAoAsync(index);
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_PlcPortId].Read(_PlcRWMB, m_OutStartOffset + index, ds);
                    ds.Position = 0;
                    val = br.ReadUInt16();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
            return val;
        }

        public bool ReadDiAsync(int index)
        {
            //if (m_OutSize <= 0 || m_OutSize < index) return false;
            //return BitConverter.ToBoolean(m_InStatus, index);

            if (m_DiSize <= 0 || m_DiSize <= index) return false;
            return m_DiStatus[index];
        }

        public bool ReadDiSync(int index)
        {
            bool val = false;
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return false;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadDiAsync(index);
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_PlcPortId].Read(_PlcRWMB, m_InStartOffset + index, ds);
                    ds.Position = 0;
                    val = br.ReadBoolean();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
            return val;
        }

        public bool ReadDoAsync(int index)
        {
            //if (m_OutSize <= 0 || m_OutSize < index) return false;
            //return BitConverter.ToBoolean(m_OutStatus, index);

            if (m_DoSize <= 0 || m_DoSize <= index) return false;
            return m_DoStatus[index];
        }

        public bool ReadDoSync(int index)
        {
            bool val = false;
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return false;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadDoAsync(index);
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_PlcPortId].Read(_PlcRWMB, m_OutStartOffset + index, ds);
                    ds.Position = 0;
                    val = br.ReadBoolean();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
            return val;
        }

        public void WriteAiSync(int index, short val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    byte[] buf = BitConverter.GetBytes(val);
                    for (int i = 0; i < buf.Length; i++)
                    {
                        m_InStatus[index + i] = buf[i];
                    }
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_InStartOffset + index, ds);
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }

        public void WriteAoAsync(int index, ushort val)
        {
            byte[] buf = BitConverter.GetBytes(val);
            for (int i = 0; i < buf.Length; i++)
            {
                if (m_Simul.IoController)
                {
                    m_OutStatus[index + i] = buf[i];
                }
                else
                {
                    m_OutCmdBuf[index + i] = buf[i];
                }
            }
        }

        public void WriteAoSync(int index, ushort val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    WriteAoAsync(index, val);
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }

        public void WriteDiSync(int index, bool val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    m_InStatus[index] = val ? (byte)1 : (byte)0;
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_InStartOffset + index, ds);
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }

        public void WriteDoAsync(int index, bool val)
        {
            if (m_Simul.IoController)
            {
                m_OutStatus[index] = val ? (byte)1 : (byte)0;
            }
            else
            {
                m_OutCmdBuf[index] = val ? (byte)1 : (byte)0;
            }
        }

        public void WriteDoSync(int index, bool val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("TwinCATPlc is not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    WriteDoAsync(index, val);
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }

        public void WriteSync(int index, object val, IoType type)
        {
            /*if (m_Initialized == false || m_AdsState != AdsState.Run) return;

            byte[] buf;
            VariableInfo info;
            AdsStream ds;
            BinaryWriter bw;
            if (type == IoType.DO || type == IoType.AO)
            {
                buf = m_OutCmdBuf;
                info = m_OutCollection.GetItem(index);
            }
            else
            {
                //buf = m_InStatus;
                //info = m_InCollection.GetItem(offset);
                return;
            }

            switch (info.DataType)
            {
                case (int)AdsDatatypeId.ADST_BIT:
                    {
                        bool newVal = Convert.ToBoolean(val);

                        ds = new AdsStream(1);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
                case (int)AdsDatatypeId.ADST_UINT8:
                    {
                        byte newVal = Convert.ToByte(val);

                        ds = new AdsStream(1);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
                case (int)AdsDatatypeId.ADST_INT16:
                    {
                        short newVal = Convert.ToInt16(val);
                        byte[] bytes = BitConverter.GetBytes(newVal);

                        ds = new AdsStream(2);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
                case (int)AdsDatatypeId.ADST_UINT16:
                    {
                        ushort newVal = Convert.ToUInt16(val);
                        byte[] bytes = BitConverter.GetBytes(newVal);

                        ds = new AdsStream(2);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
                case (int)AdsDatatypeId.ADST_STRING:
                    {
                        string newVal = Convert.ToString(val);

                        ds = new AdsStream(info.Size);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
                case (int)AdsDatatypeId.ADST_REAL32:
                    {
                        float newVal = Convert.ToSingle(val);

                        ds = new AdsStream(4);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
                case (int)AdsDatatypeId.ADST_UINT32:
                    {
                        uint newVal = Convert.ToUInt32(val);
                        byte[] bytes = BitConverter.GetBytes(newVal);

                        ds = new AdsStream(4);
                        bw = new BinaryWriter(ds);

                        ds.Position = 0;
                        bw.Write(newVal);

                        m_AdsClient[m_PlcPortId].Write(_PlcRWMB, m_OutStartOffset + index, ds);
                    }
                    break;
            }*/
        }
        #endregion
    }
}
