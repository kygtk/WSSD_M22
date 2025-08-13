///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.19
// Author       : jemoon
// Description  : N400K BCR Controller
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing.Design;
using Dms.Ctl;
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BCRn400k : _BCR
    {
        public enum Command
        {
            LON,
            LOFF,
            Polling,
        }

        public enum Response
        {
            None = -1,
            OK, //Read 성공
            SendFail, //Command Send fail
            TimeOver, //Response timeover
            PacketError, //Response packet error
            //아래부터는 N400k 고유의 error 값
            e,//아무것도 판독하지 않았을때
            OVER,//버퍼 오버가 발생했을 때
            NC,//원하는 ID 번호의 BL이 존재하지 않거나 통신이 확립되어 있지 않은 경우
            SERR, //통신 에러
        }

        // [STX][%][P][mm(id)][ETX]
        public enum SendProtocol
        {
            Header,
            Per,
            Command,
            Id1,
            Id2,
            Tail
        }

        // [STX][%][P][mm(id)][Data][ETX]
        public enum ResponseProtocol
        {
            Header,
            Per,
            Command,
            Id1,
            Id2,
            Separator,
            DataStart
        }

        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        protected Mutex m_Mutex = new Mutex();
        private XComm m_Comm;
        private PortNo m_ComPortNo = PortNo.COM1;
        private SeqN400kRead[] m_SeqRead;
        private bool m_InProcess = false;
        private Response m_BcrResponse = Response.None;
        private string m_BcrData = "";
        private static readonly char _Header = AsciiChar.STX;
        private static readonly char _Tail = AsciiChar.ETX;
        private static readonly string _CommandHeader = string.Format("{0}{1}{2}", _Header, AsciiChar.PERCENT, "P"); //[STX][%][P]
        private static readonly string _ResponsHeader = string.Format("{0}{1}{2}", _Header, AsciiChar.PERCENT, "P"); //[STX][%][P]
        private static readonly string[] _ErrorCodes = { Response.e.ToString(), Response.OVER.ToString(), Response.NC.ToString(), Response.SERR.ToString() };
        private Queue<string> m_QueueResponse = new Queue<string>();
        #endregion

        #region Properties
        [Category("DMS : Option")]
        public PortNo ComPortNo
        {
            get { return m_ComPortNo; }
            set { m_ComPortNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string ComPortname
        {
            get { return m_ComPortNo.ToString(); }
        }
        [Browsable(false), XmlIgnore()]
        public bool InProcess
        {
            get { return m_InProcess; }
            set
            {
                m_Mutex.WaitOne();
                m_InProcess = value;
                m_Mutex.ReleaseMutex();
            }
        }
        [Browsable(false), XmlIgnore()]
        public Response BcrResponse
        {
            get { return m_BcrResponse; }
            set { m_BcrResponse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string BcrData
        {
            get { return m_BcrData; }
            set { m_BcrData = value; }
        }
        [Browsable(false), XmlIgnore()]
        public Queue<string> QueueResponse
        {
            get { return m_QueueResponse; }
            set { m_QueueResponse = value; }
        }
        #endregion

        #region Constructor
        public BCRn400k()
        {
            this.Name = "__";
        }
        #endregion

        #region Methods
        private void ReceivedData(object sender)
        {
            string receivedStream = m_Comm.ReadExisting();
            m_QueueResponse.Enqueue(receivedStream);

            //if (!receivedStream.Contains(_ResponsHeader) || !receivedStream.Contains(_Tail.ToString()))
            //{   //이렇다면 packet error
            //    m_BcrResponse = Response.PacketError;
            //}
            //else
            //{   //packet을 분석해 보자
            //    int streamLength = receivedStream.Length;
            //    int dataLength = streamLength - ((int)ResponseProtocol.DataStart + 1) - 1;  //-1:Tail
            //    string id = receivedStream.Substring((int)ResponseProtocol.Id1, 2);
            //    string data = receivedStream.Substring((int)ResponseProtocol.DataStart, dataLength);

            //    int errorCodeCount = _ErrorCodes.Length;
            //    for (int i = 0; i < errorCodeCount; i++)
            //    {
            //        if (string.Equals(data, _ErrorCodes[i]))
            //        { 
            //            m_BcrResponse = (Response)Enum.Parse(typeof(Response), data,true);
            //            return;
            //        }
            //    }

            //    m_BcrData = data;
            //    m_BcrResponse = Response.OK;
            //}
        }

        private string MakeCommand(int portId, Command command)
        {
            string strCommand = "";
            int channelId = FindChannelId(portId);
            switch (command)
            {
                case Command.LON:
                    {
                        strCommand = string.Format("{0}%T{1:00}-LON{2}", _Header, (channelId + 1), _Tail);
                    }
                    break;
                case Command.LOFF:
                    {
                        strCommand = string.Format("{0}%T{1:00}-LOFF{2}", _Header, (channelId + 1), _Tail);
                    }
                    break;
                case Command.Polling:
                    {
                        // [STX][%][P][mm(id)][ETX]
                        //strCommand = string.Format("{0}{1:00}{2}", _CommandHeader, (channelId + 1), _Tail);
                        strCommand = string.Format("{0}%P{1:00}{2}", _Header, (channelId + 1), _Tail);
                    }
                    break;

            }

            return strCommand;
        }

        public bool SendCommand(int portId, Command command)
        {
            return m_Comm.Write(MakeCommand(portId, command));
        }
        #endregion

        #region Override
        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
        }

        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_Comm = new XComm();
                m_Comm.Initialize();
                m_Comm.Simulate = m_Simul.Comport;
                m_Comm.Open(ComPortname, 9600, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                    if (m_ControlMode == MuxMode.Single) m_MultiDropCount = 1;

                    m_Data = new string[m_MultiDropCount];
                    m_UnitStatus = new int[m_MultiDropCount];
                    m_PortIds = new int[m_MultiDropCount];
                    m_SeqRead = new SeqN400kRead[m_MultiDropCount];
                    for (int i = 0; i < m_MultiDropCount; i++)
                    {
                        m_PortIds[i] = m_StartPortId + i;
                        m_SeqRead[i] = new SeqN400kRead(i, this);
                    }
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override void SetSubscriber()
        {
            base.SetSubscriber();

            m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
        }

        public override int Reading(int portId)
        {
            int id = FindChannelId(portId);
            return m_SeqRead[id].Do();
        }

        public override void DirectReading(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }

    public class SeqN400kRead : XSeqFunction
    {
        #region Fields
        private int m_ChannelId;
        private int m_PortId;
        private BCRn400k m_Unit;
        private XTimer m_TimerRead = null;
        private int m_TimeoutRead = 2 * 1000;
        #endregion

        #region Constructor
        public SeqN400kRead(int channelId, BCRn400k unit)
        {
            m_ChannelId = channelId;
            m_Unit = unit;
            m_PortId = m_Unit.FindPortId(m_ChannelId);
            m_TimerRead = new XTimer(m_Unit.Name + "Read Timer");
            m_TimeoutRead = m_Unit.TimeoutRead;
        }
        #endregion

        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    if (!m_Unit.InProcess)
                    {
                        //작업중임을 알리는 flag를 set하고
                        m_Unit.InProcess = true;
                        //BCR의 Read 결과를 clear한다.
                        m_Unit.BcrResponse = BCRn400k.Response.None;
                        seqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_Unit.Simul.Comport)
                        {   //com port simulation mode이면 바로 ok로 처리
                            //                        m_Unit.BcrResponse = BCRn400k.Response.OK;
                            //                        seqNo = 20;

                            m_Unit.MakeData(m_PortId, m_Unit.BcrData);
                            m_Unit.InProcess = false;
                            returnValue = 0;
                            seqNo = 0;
                        }
                        else
                        {   //Send command
                            bool sendOk = m_Unit.SendCommand(m_PortId, BCRn400k.Command.LON);
                            if (!sendOk)
                            {   //command 송신 실패
                                m_Unit.BcrResponse = BCRn400k.Response.SendFail;
                                m_Unit.InProcess = false;
                                returnValue = 3;
                                seqNo = 0;
                            }
                            else
                            {   //command 송신 성공, read timeout 설정하고 수신대기
                                m_TimerRead.Start(m_TimeoutRead);
                                seqNo = 20;
                            }
                        }
                    }
                    break;
                case 20:
                    if (m_Unit.QueueResponse.Count > 0)
                    {
                        string checkStream = m_Unit.QueueResponse.Peek();
                        bool curBlPolling = checkStream.Contains(string.Format("{0}%P{1:00}", AsciiChar.STX, (m_PortId + 1)));
                        bool otherBlPolling = checkStream.Contains(string.Format("{0}%P", AsciiChar.STX));
                        if (curBlPolling)
                        {
                            string queueStream = null;
                            do
                            {
                                queueStream += m_Unit.QueueResponse.Dequeue();
                            } while (!queueStream.Contains(string.Format("{0}", AsciiChar.ETX)) && m_Unit.QueueResponse.Count > 0);

                            int streamLength = queueStream.Length;
                            int dataLength = streamLength - ((int)BCRn400k.ResponseProtocol.DataStart + 1);
                            string id = queueStream.Substring((int)BCRn400k.ResponseProtocol.Id1, 2);
                            string data = queueStream.Substring((int)BCRn400k.ResponseProtocol.DataStart, dataLength);

                            if (data == BCRn400k.Response.e.ToString() || data == BCRn400k.Response.OVER.ToString() ||
                               data == BCRn400k.Response.NC.ToString() || data == BCRn400k.Response.SERR.ToString() ||
                               data == "ERROR")
                            {
                                m_Unit.InProcess = false;

                                if (data == BCRn400k.Response.e.ToString())
                                    m_Unit.BcrResponse = BCRn400k.Response.e;
                                else if (data == BCRn400k.Response.OVER.ToString())
                                    m_Unit.BcrResponse = BCRn400k.Response.OVER;
                                else if (data == BCRn400k.Response.NC.ToString())
                                    m_Unit.BcrResponse = BCRn400k.Response.NC;
                                else if (data == BCRn400k.Response.SERR.ToString())
                                    m_Unit.BcrResponse = BCRn400k.Response.SERR;

                                returnValue = 1;
                            }
                            else
                            {
                                //m_Unit.QueueBcr.Clear();
                                m_Unit.MakeData(m_PortId, data);
                                m_Unit.InProcess = false;
                                m_Unit.BcrResponse = BCRn400k.Response.OK;
                                returnValue = 0;
                            }
                            seqNo = 0;
                        }
                        else if (!otherBlPolling)
                        {
                            m_Unit.QueueResponse.Dequeue();
                        }
                    }
                    else if (m_TimerRead.Over)
                    {
                        m_Unit.BcrResponse = BCRn400k.Response.TimeOver;

                        m_Unit.SendCommand(m_PortId, BCRn400k.Command.LOFF);

                        m_Unit.InProcess = false;
                        returnValue = 2;
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
    }
}
