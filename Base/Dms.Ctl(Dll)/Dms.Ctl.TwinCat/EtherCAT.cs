////////////////////////////////////////////////////////////////////////////////////////
// * fileName : EtherCAT.cs
// * description : 
// * written by : Jemoon
// * date : 2007.10.19
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.26 - jemoon : read, writer buffering concept 적용
// * 2009.02.13 - jemoon : code tuning
// * 2009.09.11 - jemoon : Multi Thread에 대한 안정성 개선,
//                         AdsStream에 대한 안정성 개선,
//                         사용하지 않는 변수 제거, 
//                         불합리 Code 개선 : Polling 함수에서 Event Fire 조건제거   
// * 2009.10.10 - jemoon : WriteDo Buffering bug fix        
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TwinCAT.Ads;
using Dms.Common;
using Microsoft.Win32;

namespace Dms.Ctl
{
    public class EtherCAT : ICtlDevice
    {
        /// <summary>
        /// This class is wrapper class for handling TwinCAT
        /// Digital I/O 와 Analog I/O는 구분되어 각각의 Task로 구성되어야 한다.
        /// ex)
        /// Task1(port:301) - DI/DO
        /// Task2(port:302) - AI/AO
        /// </summary>

        #region Singleton
        public static readonly EtherCAT Instance = new EtherCAT();
        #endregion

        #region Fields
        private bool m_Initialized = false;
        private bool m_Uninitializing = false;
        private IoUpdateMode m_UpdateMode = IoUpdateMode.Polling;
        private ActiveState m_DeviceState;
        private TcAdsClient m_AdsRouter;
        private TcAdsClient[] m_AdsClient;
        private int[] m_PortNo;
        private List<int> m_HandleNotification = new List<int>();
        private List<int> m_HandlePortId = new List<int>();
        private TcSystemServerClass m_TcSvr = null;
        private TcAdsSymbolInfoLoader m_SymbolLoader = null;
        private AdsState m_AdsState;
        private XLog m_Log = new XLog("TwinCatLog", XLog.LogStampType.UseStamp);

        private int m_DioPortId = 0;
        private int m_AioPortId = 0;
        private int m_PortCount;
        private int m_ClientCount;

        /////////////////////////////////////////////////////////////////////////////////
        //각 접점의 총수량
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

        /////////////////////////////////////////////////////////////////////////////////
        //Sequence에서 Write Command Buffer size
        private int m_DoCmdBufSize;
        private int m_DoCmdBufByteSize;
        private int m_AoCmdBufSize;

        /////////////////////////////////////////////////////////////////////////////////
        //Sequence에서 Write Command Buffer size
        //jemoon : 090831 - Write가 실패할수 있기에, m_DoCmdBufOld 대신에 m_DoStatusByte 사용
        //private byte[] m_DoCmdBufOld = null;
        private byte[] m_DoStatusByte = null;
        private byte[] m_DoCmdBufByte = null;   //jemoon : 090911 - Multi thread에 대한 안정성 강화
        private bool[] m_DoCmdBuf = null;       //jemoon : 090911 - Multi thread에 대한 안정성 강화    
        private ushort[] m_AoCmdBuf = null;
        private ushort[] m_AoCmdBufOld = null;

        /////////////////////////////////////////////////////////////////////////////////
        //Sequence에서 Read 하는 current 상태
        private bool[] m_DiStatus = null;
        private bool[] m_DoStatus = null;
        private short[] m_AiStatus = null;
        private ushort[] m_AoStatus = null;

        /////////////////////////////////////////////////////////////////////////////////
        //Event Driven 방식에서 변화를 Check하기 위한 변수
        //사용하지 않음
        //private bool[] m_DiStatusOld = null;
        //private bool[] m_DoStatusOld = null;
        //private short[] m_AiStatusOld = null;
        //private ushort[] m_AoStatusOld = null;
        //private bool m_DiStatusInit = false;
        //private bool m_DoStatusInit = false;
        //private bool m_AiStatusInit = false;
        //private bool m_AoStatusInit = false;

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // Ads Stream for Polling Read / Write
        private AdsStream m_DiStreamPollRead = null;
        private AdsStream m_DoStreamPollRead = null;
        private AdsStream m_AiStreamPollRead = null;
        private AdsStream m_AoStreamPollRead = null;
        private AdsStream m_DoStreamPollWrite = null;
        private AdsStream m_AoStreamPollWrite = null;
        private BinaryReader m_DiBinReader = null;
        private BinaryReader m_DoBinReader = null;
        private BinaryReader m_AiBinReader = null;
        private BinaryReader m_AoBinReader = null;
        private BinaryWriter m_DoBinWriter = null;
        private BinaryWriter m_AoBinWriter = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////
        //Ads Stream for Event
        private AdsStream m_DiStreamEvent = null;
        private AdsStream m_DoStreamEvent = null;
        private AdsStream m_AiStreamEvent = null;
        private AdsStream m_AoStreamEvent = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////
        //Watch Thread
        private ThreadTcWatch m_ThreadTcWatch;
        private Simul m_Simul;
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
        #endregion

        #region Event
        public event IoStateChangeEventHandler OnIoStateChange;
        #endregion

        #region Constructor
        private EtherCAT()
        {
            m_Simul = AppConfig.Instance.Simul;

            WriteTwinCatMasterKey();
        }
        #endregion

        #region Method
        public void WriteTwinCatMasterKey()
        {
            if (m_Simul.IoController || m_Simul.Device) return;

            string path = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Beckhoff\\TwinCAT\\";
            string version = "2.10.000";
            string valueName = "Serial";
            string masterKey = "CE50-BF54-4A18-DC40";

            Registry.SetValue(path + version, valueName, masterKey);
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
                m_AdsClient[i].Connect(m_PortNo[i]);

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

        private void DisoseClient()
        {
            if (m_Simul.IoController) return;

            for (int i = 0; i < m_PortCount; i++)
            {
                m_AdsClient[i].Dispose();
            }

            m_HandleNotification.Clear();
            m_HandlePortId.Clear();
        }

        private void CreateSymbol()
        {
            if (m_Simul.IoController) return;

            for (int i = 0; i < m_PortCount; i++)
            {
                m_SymbolLoader = m_AdsClient[i].CreateSymbolInfoLoader();
                int SymbolCount = m_SymbolLoader.GetSymbolCount(true);

                foreach (TcAdsSymbolInfo symbol in m_SymbolLoader)
                {
                    switch (symbol.IndexGroup)
                    {
                        case (int)AdsReservedIndexGroups.IOImageRWIX:
                            m_DiCount++;
                            if (m_DioPortId == 0) m_DioPortId = i;
                            break;
                        case (int)AdsReservedIndexGroups.IOImageRWOX:
                            m_DoCount++;
                            if (m_DioPortId == 0) m_DioPortId = i;
                            break;
                        case (int)AdsReservedIndexGroups.IOImageRWIB:
                            m_AiCount++;
                            if (m_AioPortId == 0) m_AioPortId = i;
                            break;
                        case (int)AdsReservedIndexGroups.IOImageRWOB:
                            m_AoCount++;
                            if (m_AioPortId == 0) m_AioPortId = i;
                            break;
                    }
                }
            }
        }

        private void CreateBuffer()
        {
            if (m_Simul.IoController)
            {   // jemoon : TwinCAT 미설치시 Buffer Size는 넉넉히 잡아준다.
                m_DiCount = 0x7FFF;
                m_DoCount = 0x7FFF;
                m_AiCount = 0x7FFF;
                m_AoCount = 0x7FFF;
            }

            //Data size 계산
            //DI/DO는 byte size, bool size 전환 필요함
            m_DiMemorySize = (m_DiCount - 1) / 8 + 1;
            m_DoMemorySize = (m_DoCount - 1) / 8 + 1;
            m_AiMemorySize = m_AiCount;
            m_AoMemorySize = m_AoCount;

            //Ads에서 용되는 Stream 초기화
            if (m_UpdateMode == IoUpdateMode.EventDriven)
            {
                m_DiStreamEvent = new AdsStream(m_DiCount);
                m_DoStreamEvent = new AdsStream(m_DoCount);
                m_AiStreamEvent = new AdsStream(m_AiCount * 2);
                m_AoStreamEvent = new AdsStream(m_AoCount * 2);
            }
            else
            {
                m_DiStreamPollRead = new AdsStream(m_DiMemorySize);
                m_DoStreamPollRead = new AdsStream(m_DoMemorySize);
                m_AiStreamPollRead = new AdsStream(m_AiMemorySize * 2);
                m_AoStreamPollRead = new AdsStream(m_AoMemorySize * 2);
                m_DoStreamPollWrite = new AdsStream(m_DoMemorySize);
                m_AoStreamPollWrite = new AdsStream(m_AoMemorySize * 2);

                m_DiBinReader = new BinaryReader(m_DiStreamPollRead);
                m_DoBinReader = new BinaryReader(m_DoStreamPollRead);
                m_AiBinReader = new BinaryReader(m_AiStreamPollRead);
                m_AoBinReader = new BinaryReader(m_AoStreamPollRead);
                m_DoBinWriter = new BinaryWriter(m_DoStreamPollWrite);
                m_AoBinWriter = new BinaryWriter(m_AoStreamPollWrite);
            }

            // Current상태 Update Buffer
            m_DiStatus = new bool[m_DiCount];
            m_DoStatus = new bool[m_DoCount];
            m_AiStatus = new short[m_AiCount];
            m_AoStatus = new ushort[m_AoCount];
            //jemoon : 090911 - 사용하지 않음
            //m_DiStatusOld = new bool[m_DiCount];
            //m_DoStatusOld = new bool[m_DoCount];
            //m_AiStatusOld = new short[m_AiCount];
            //m_AoStatusOld = new ushort[m_AoCount];

            // Sequence command 처리를 위한 Buffer
            m_DoCmdBuf = new bool[m_DoCount];
            m_DoCmdBufSize = m_DoCount;
            m_DoCmdBufByte = new byte[m_DoMemorySize];
            m_DoCmdBufByteSize = m_DoMemorySize;
            //jemoon : 090831 - Write가 실패할수 있기에, m_DoCmdBufOld 대신에 m_DoStatusByte 사용
            //m_DoCmdBufOld = new byte[m_DoCmdBufSize];
            m_DoStatusByte = new byte[m_DoCmdBufByteSize];
            m_AoCmdBuf = new ushort[m_AoMemorySize];
            m_AoCmdBufSize = m_AoCmdBuf.Length;
            m_AoCmdBufOld = new ushort[m_AoCmdBufSize];
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

                    if (m_TcSvr.SystemState != (int)AdsState.Run)
                    {
                        string msg = "TwinCAT Start failed";
                        WriteLog(msg);
                        MessageBox.Show(msg);
                        //Application.Exit();
                        return DmsErrors.InternalError;
                    }
                    else
                    {
                        m_AdsRouter = new TcAdsClient();
                        m_AdsRouter.Connect(m_PortNo[0]);
                        AddRouterNotification();

                        m_AdsClient = new TcAdsClient[m_PortCount];
                        m_ClientCount = m_AdsClient.Length;
                        if (CreateClient() == DmsErrors.Success)
                        {
                            CreateSymbol();

                            CreateBuffer();

                            if (m_UpdateMode == IoUpdateMode.EventDriven)
                            {
                                AddDeviceNotification();
                            }

                            m_DeviceState = (m_TcSvr.SystemState == (int)AdsState.Run) ? ActiveState.Run : ActiveState.UnKnown;

                            m_ThreadTcWatch = new ThreadTcWatch(this);
                            m_ThreadTcWatch.Start();

                            m_Initialized = true;
                        }

                        return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
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
            if (m_Simul.IoController) return;

            for (int i = 0; i < m_DiCount; i++)
            {
                AddDeviceNotification(IoType.DI, i);
            }

            for (int i = 0; i < m_DoCount; i++)
            {
                AddDeviceNotification(IoType.DO, i);
            }

            for (int i = 0; i < m_AiCount; i++)
            {
                AddDeviceNotification(IoType.AI, i);
            }

            for (int i = 0; i < m_AoCount; i++)
            {
                AddDeviceNotification(IoType.AO, i);
            }

            for (int i = 0; i < m_ClientCount; i++)
            {
                m_AdsClient[i].AdsNotification += new AdsNotificationEventHandler(OnDeviceNotification);
            }
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
            if (m_TcSvr.SystemState != (int)AdsState.Run)
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

                        ClearAllNode();

                        //DeleteNotification();

                        for (int i = 0; i < m_ClientCount; i++)
                        {
                            m_AdsClient[i].Dispose();
                        }

                        TcStop();
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


        public void ClearAllNode()
        {
            for (int i = 0; i < m_DoCount; i++)
            {
                WriteDoSync(i, false);
            }
        }

        int _IOImageRWIB = (int)AdsReservedIndexGroups.IOImageRWIB;
        public void ReadDi()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_DiCount <= 0) return;

            m_AdsClient[m_DioPortId].Read(_IOImageRWIB, 0, m_DiStreamPollRead, 0, m_DiMemorySize);
            m_DiStreamPollRead.Position = 0;

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

                    // jemoon : 090911 - Event Driven case 제거
                    // Polling 이면 Event Fire 하지 않으므로 필요아래 부분 필요없다
                    //// Fire event...
                    //if (m_UpdateMode == IoUpdateMode.EventDriven)
                    //{
                    //    if (!m_DiStatusInit || m_DiStatusOld[bitIndex] != m_DiStatus[bitIndex])
                    //    {
                    //        m_DiStatusInit = true;
                    //        m_DiStatusOld[bitIndex] = m_DiStatus[bitIndex];

                    //        IoStateChangeEventHandler eHandle = OnIoStateChange;
                    //        if (eHandle != null)
                    //        {
                    //            OnIoStateChange(this, new IoStateEventArgs(IoType.DI, bitIndex));
                    //        }
                    //    }
                    //}
                }
            }
        }

        int _IOImageRWOB = (int)AdsReservedIndexGroups.IOImageRWOB;
        public void ReadDo()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_DoCount <= 0) return;

            m_AdsClient[m_DioPortId].Read(_IOImageRWOB, 0, m_DoStreamPollRead, 0, m_DoMemorySize);
            m_DoStreamPollRead.Position = 0;

            byte data;
            int bitIndex;
            for (int i = 0; i < m_DoMemorySize; i++)
            {
                data = m_DoBinReader.ReadByte();
                //jemoon : 090831 - WriteDo() 에서 CurrentStatus를 Byte 형태로 비교하기 위해서
                m_DoStatusByte[i] = data;

                for (int bit = 0; bit < 8; bit++)
                {
                    bitIndex = i * 8 + bit;
                    if (bitIndex == m_DoCount) return;

                    m_DoStatus[bitIndex] = ((data >> bit) & 0x01) > 0;

                    // jemoon : 090911 - Event Driven case 제거
                    // Polling 이면 Event Fire 하지 않으므로 필요아래 부분 필요없다
                    //// Fire event...
                    //if (m_UpdateMode == IoUpdateMode.EventDriven)
                    //{
                    //    if (!m_DoStatusInit || m_DoStatusOld[bitIndex] != m_DoStatus[bitIndex])
                    //    {
                    //        m_DoStatusInit = true;
                    //        m_DoStatusOld[bitIndex] = m_DoStatus[bitIndex];

                    //        IoStateChangeEventHandler eHandle = OnIoStateChange;
                    //        if (eHandle != null)
                    //        {
                    //            OnIoStateChange(this, new IoStateEventArgs(IoType.DO, bitIndex));
                    //        }
                    //    }
                    //}
                }
            }
        }

        public void ReadAi()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_AiCount <= 0) return;

            m_AdsClient[m_AioPortId].Read(_IOImageRWIB, 0, m_AiStreamPollRead);
            m_AiStreamPollRead.Position = 0;
            for (int i = 0; i < m_AiCount; i++)
            {
                m_AiStatus[i] = m_AiBinReader.ReadInt16();

                // jemoon : 090911 - Event Driven case 제거
                // Polling 이면 Event Fire 하지 않으므로 필요아래 부분 필요없다
                //// Fire event...
                //if (m_UpdateMode == IoUpdateMode.EventDriven)
                //{
                //    if (!m_AiStatusInit || m_AiStatusOld[i] != m_AiStatus[i])
                //    {
                //        m_AiStatusInit = true;
                //        m_AiStatusOld[i] = m_AiStatus[i];

                //        IoStateChangeEventHandler eHandle = OnIoStateChange;
                //        if (eHandle != null)
                //        {
                //            OnIoStateChange(this, new IoStateEventArgs(IoType.AI, i));
                //        }
                //    }
                //}
            }
        }

        public void ReadAo()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if ((AdsState)m_TcSvr.SystemState != AdsState.Run) return;
            if (m_AoCount <= 0) return;

            m_AdsClient[m_AioPortId].Read(_IOImageRWOB, 0, m_AoStreamPollRead);
            m_AoStreamPollRead.Position = 0;
            for (int i = 0; i < m_AoCount; i++)
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

        /// <summary>
        /// Write all of digital output cache via TwinCAT API at one time
        /// This function should be called by ThreadTcWriteState.
        /// </summary>
        public void WriteDo()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if (m_TcSvr.SystemState != (int)AdsState.Run) return;

            bool changed = false;
            int bitIndex = 0;
            for (int i = 0; i < m_DoCmdBufByteSize; i++)
            {
                ////////////////////////////////////////////////////////////////
                //jemon : 090911 - Multi Thread 안정성 고려
                byte val = 0;
                for (int bit = 0; bit < 8; bit++)
                {
                    bitIndex = i * 8 + bit;
                    if (bitIndex == m_DoCount) break;   //jemoon : 091010 Bug fix

                    val |= (byte)((m_DoCmdBuf[bitIndex] ? 1 : 0) << bit);
                }
                m_DoCmdBufByte[i] = val;
                ////////////////////////////////////////////////////////////////

                //jemoon : 090831 - Write가 실패할수 있기에, m_DoCmdBufOld 대신에 m_DoStatusByte 사용
                //if (m_DoCmdBufOld[i] != m_DoCmdBuf[i])
                if (m_DoStatusByte[i] != m_DoCmdBufByte[i])
                {
                    changed = true;
                    break;
                }
            }

            if (changed)
            {
                m_DoStreamPollWrite.Position = 0;

                for (int i = 0; i < m_DoCmdBufByteSize; i++)
                {
                    m_DoBinWriter.Write(m_DoCmdBufByte[i]);
                }

                m_AdsClient[m_DioPortId].Write(_IOImageRWOB, 0, m_DoStreamPollWrite, 0, m_DoCmdBufByteSize);

                //jemoon : 090831 - Write가 실패할수 있기에, m_DoCmdBufOld 대신에 m_DoStatusByte 사용
                //for (int i = 0; i < m_DoCmdBufSize; i++)
                //{
                //    if (m_DoCmdBufOld[i] != m_DoCmdBuf[i])
                //    {
                //        m_DoCmdBufOld[i] = m_DoCmdBuf[i];
                //    }
                //}
            }
        }

        /// <summary>
        /// Write all of analog output cache via TwinCAT API at one time
        /// This function should be called by ThreadTcWriteState.
        /// </summary>
        public void WriteAo()
        {
            if (m_Simul.IoController) return;

            if (m_AdsState != AdsState.Run) return;
            //if (m_TcSvr.SystemState != (int)AdsState.Run) return;

            bool changed = false;
            for (int i = 0; i < m_AoCmdBufSize; i++)
            {
                //jemoon : 090831 - Write가 실패할수 있기에, m_AoCmdBufOld 대신에 m_AoStatus 사용
                //if (m_AoCmdBufOld[i] != m_AoCmdBuf[i])
                if (m_AoStatus[i] != m_AoCmdBuf[i])
                {
                    changed = true;
                    break;
                }
            }

            if (changed)
            {
                m_AoStreamPollWrite.Position = 0;

                for (int i = 0; i < m_AoCmdBufSize; i++)
                {
                    m_AoBinWriter.Write(m_AoCmdBuf[i]);
                }

                m_AdsClient[m_AioPortId].Write(_IOImageRWOB, 0, m_AoStreamPollWrite);

                //jemoon : 090831 - Write가 실패할수 있기에, m_AoCmdBufOld 대신에 m_AoStatus 사용
                //for (int i = 0; i < m_AoCmdBufSize; i++)
                //{
                //    if (m_AoCmdBufOld[i] != m_AoCmdBuf[i])
                //    {
                //        m_AoCmdBufOld[i] = m_AoCmdBuf[i];
                //    }
                //}
            }
        }


        private DmsErrors AddDeviceNotification(IoType type, int offset)
        {
            if (m_Simul.IoController) return DmsErrors.Success;

            int handle = 0;
            switch (type)
            {
                case IoType.DI:
                    {
                        handle = m_AdsClient[m_DioPortId].AddDeviceNotification((int)AdsReservedIndexGroups.IOImageRWIX, offset,
                                                            m_DiStreamEvent, offset, 1, AdsTransMode.OnChange,
                                                            10, 0, IoType.DI);
                        m_HandlePortId.Add(m_DioPortId);
                        m_HandleNotification.Add(handle);
                    }
                    break;
                case IoType.DO:
                    {
                        handle = m_AdsClient[m_DioPortId].AddDeviceNotification((int)AdsReservedIndexGroups.IOImageRWOX, offset,
                                                            m_DoStreamEvent, offset, 1, AdsTransMode.OnChange,
                                                            10, 0, IoType.DO);
                        m_HandlePortId.Add(m_DioPortId);
                        m_HandleNotification.Add(handle);
                    }
                    break;
                case IoType.AI:
                    {
                        handle = m_AdsClient[m_AioPortId].AddDeviceNotification((int)AdsReservedIndexGroups.IOImageRWIB, offset * 2,
                                                            m_AiStreamEvent, offset * 2, 2, AdsTransMode.OnChange,
                                                            10, 0, IoType.AI);
                        m_HandlePortId.Add(m_AioPortId);
                        m_HandleNotification.Add(handle);
                    }
                    break;
                case IoType.AO:
                    {
                        handle = m_AdsClient[m_AioPortId].AddDeviceNotification((int)AdsReservedIndexGroups.IOImageRWOB, offset * 2,
                                                            m_AoStreamEvent, offset * 2, 2, AdsTransMode.OnChange,
                                                            10, 0, IoType.AO);
                        m_HandlePortId.Add(m_AioPortId);
                        m_HandleNotification.Add(handle);
                    }
                    break;
            }

            return DmsErrors.Success;
        }

        private void OnDeviceNotification(object sender, AdsNotificationEventArgs e)
        {
            if (m_Simul.IoController) return;

            try
            {
                BinaryReader binReader = new BinaryReader(e.DataStream);
                e.DataStream.Position = e.Offset;

                IoType type = (IoType)e.UserData;
                int id = e.Offset;

                switch (type)
                {
                    case IoType.DI:
                        {
                            bool state = binReader.ReadBoolean();
                            m_DiStatus[id] = state;
                        }
                        break;
                    case IoType.DO:
                        {
                            bool state = binReader.ReadBoolean();
                            m_DoStatus[id] = state;
                        }
                        break;
                    case IoType.AI:
                        {
                            short state = binReader.ReadInt16();
                            id /= 2;
                            m_AiStatus[id] = state;
                        }
                        break;
                    case IoType.AO:
                        {
                            ushort state = binReader.ReadUInt16();
                            id /= 2;
                            m_AoStatus[id] = state;
                        }
                        break;
                }

                // Fire event...
                IoStateChangeEventHandler eHandle = OnIoStateChange;
                if (eHandle != null)
                {
                    OnIoStateChange(this, new IoStateEventArgs(type, id));
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }

        public void MonitorState()
        {
            if (m_Simul.IoController) return;

            m_AdsState = (AdsState)m_TcSvr.SystemState;

            if (m_AdsState != AdsState.Run)
            {
                TcStart();
            }
        }

        /* callback function called on state changes of the local AMS router */
        private void AmsRouterNotificationCallback(object sender, AmsRouterNotificationEventArgs e)
        {
            if (m_Simul.IoController) return;

            if (e.State != AmsRouterState.Start)
            {
                m_DeviceState = ActiveState.Stop;
                DisoseClient();
            }
            else
            {
                CreateClient();

                if (m_UpdateMode == IoUpdateMode.EventDriven)
                {
                    AddDeviceNotification();
                }

                m_DeviceState = ActiveState.Run;
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
        /// <summary>
        /// Read digtal input state from TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        int _IOImageRWIX = (int)AdsReservedIndexGroups.IOImageRWIX;
        public bool ReadDiSync(int offset)
        {
            bool val = false;
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return false;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadDiAsync(offset);
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_DioPortId].Read(_IOImageRWIX, offset, ds);
                    ds.Position = 0;
                    val = br.ReadBoolean();
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);   
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }

            return val;

        }


        /// <summary>
        /// Read digital input state from local cache
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool ReadDiAsync(int offset)
        {
            return m_DiStatus[offset];
        }


        /// <summary>
        /// Read digital output state from TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        int _IOImageRWOX = (int)AdsReservedIndexGroups.IOImageRWOX;
        public bool ReadDoSync(int offset)
        {
            bool val = false;
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return false;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadDoAsync(offset);
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_DioPortId].Read(_IOImageRWOX, offset, ds);
                    ds.Position = 0;
                    val = br.ReadBoolean();
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }

            return val;
        }

        /// <summary>
        /// Read digital output state from local cache
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool ReadDoAsync(int offset)
        {
            return m_DoStatus[offset];
        }


        /// <summary>
        /// Write digital input state via TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="val"></param>
        public void WriteDiSync(int offset, bool val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    m_DiStatus[offset] = val;
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_DioPortId].Write(_IOImageRWIX, offset, ds);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }

        /// <summary>
        /// Write one digital output via TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="val"></param>
        public void WriteDoSync(int offset, bool val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    WriteDoAsync(offset, val);
                }
                else
                {
                    AdsStream ds = new AdsStream(1);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_DioPortId].Write(_IOImageRWOX, offset, ds);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }


        /// <summary>
        /// Write one digital output to cache
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="val"></param>
        public void WriteDoAsync(int offset, bool val)
        {
            if (m_Simul.IoController)
            {
                m_DoStatus[offset] = val;
            }
            else
            {
                m_DoCmdBuf[offset] = val;

                //jemoon : 090911 - Multi Thead 안정성 고려
                //if (val)
                //{
                //    m_DoCmdBufByte[offset / 8] |= (byte)(0x01 << offset % 8);
                //}
                //else
                //{
                //    m_DoCmdBufByte[offset / 8] &= (byte)(~(0x01 << offset % 8));
                //}
            }
        }


        /// <summary>
        /// Read analog input state from TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public short ReadAiSync(int offset)
        {
            short val = 0;
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return 0;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadAiAsync(offset);
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_AioPortId].Read(_IOImageRWIB, offset * 2, ds);
                    ds.Position = 0;
                    val = br.ReadInt16();
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }

            return val;
        }


        /// <summary>
        /// Read analog input state from cache
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public short ReadAiAsync(int offset)
        {
            return m_AiStatus[offset];
        }


        /// <summary>
        /// Read analog output state from TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ushort ReadAoSync(int offset)
        {
            ushort val = 0;
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return 0;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    return ReadAoAsync(offset);
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryReader br = new BinaryReader(ds);

                    m_AdsClient[m_AioPortId].Read(_IOImageRWOB, offset * 2, ds);
                    ds.Position = 0;
                    val = br.ReadUInt16();
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }

            return val;
        }


        /// <summary>
        /// Read analog output state from cache
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ushort ReadAoAsync(int offset)
        {
            return m_AoStatus[offset];
        }


        /// <summary>
        /// Write analog input state via TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="val"></param>
        public void WriteAiSync(int offset, short val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    m_AiStatus[offset] = val;
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_AioPortId].Write(_IOImageRWIB, offset * 2, ds);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }


        /// <summary>
        /// Write one analog output state via TwinCAT API
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="val"></param>
        public void WriteAoSync(int offset, ushort val)
        {
            if (!m_Initialized)
            {
                MessageBox.Show("EtherCAT not initialized!");
                return;
            }

            try
            {
                if (m_Simul.IoController)
                {
                    WriteAoAsync(offset, val);
                }
                else
                {
                    AdsStream ds = new AdsStream(2);
                    BinaryWriter bw = new BinaryWriter(ds);

                    ds.Position = 0;
                    bw.Write(val);

                    m_AdsClient[m_AioPortId].Write(_IOImageRWOB, offset * 2, ds);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                WriteLog(err.ToString());
            }
        }


        /// <summary>
        /// Write one analog output state to cache
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="val"></param>
        public void WriteAoAsync(int offset, ushort val)
        {
            if (m_Simul.IoController)
            {
                m_AoStatus[offset] = val;
            }
            else
            {
                m_AoCmdBuf[offset] = val;
            }
        }
        #endregion

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

        #endregion

        #region ICtlDevice 멤버


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
