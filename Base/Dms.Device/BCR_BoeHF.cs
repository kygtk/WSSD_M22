///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.02.18
// Author       : Kim Youngsik
// Description  : BCR Unit for BOE-HF Net-G Type
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
    public class BCR_BoeHF : _BCR
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        protected Mutex m_Mutex = new Mutex();
        private bool m_InProcess = false;
        private string m_BcrData = "";
        private SeqBcrRead_BoeHF[] m_SeqRead;
        #endregion

        #region Properties
        private int m_InterfaceTimeout = 10000;
        [Category("DMS : General Setting"), Description("Melsec Interface CST ID Reading Timeout Setting(msec)")]
        public int InterfaceTimeout
        {
            get { return m_InterfaceTimeout; }
            set { m_InterfaceTimeout = value; }
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
        public string BcrData
        {
            get { return m_BcrData; }
            set { m_BcrData = value; }
        }
        #endregion

        #region Melsec Device
        private IoDigitalInput m_mibLoaderOffline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLoaderOffline
        {
            get { return m_mibLoaderOffline; }
            set { m_mibLoaderOffline = value; }
        }

        private IoDigitalInput m_mibPortCommandReadComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortCommandReadComplete
        {
            get { return m_mibPortCommandReadComplete; }
            set { m_mibPortCommandReadComplete = value; }
        }
        private IoDigitalInput m_mibPortCommandEndReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortCommandEndReport
        {
            get { return m_mibPortCommandEndReport; }
            set { m_mibPortCommandEndReport = value; }
        }

        private IoDigitalOutput m_mobPortCommandReadRequest = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortCommandReadRequest
        {
            get { return m_mobPortCommandReadRequest; }
            set { m_mobPortCommandReadRequest = value; }
        }
        private IoDigitalOutput m_mobPortCommandEndConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortCommandEndConfirm
        {
            get { return m_mobPortCommandEndConfirm; }
            set { m_mobPortCommandEndConfirm = value; }
        }

        private IoAnalogInput m_miwEndCommandPortNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwEndCommandPortNo
        {
            get { return m_miwEndCommandPortNo; }
            set { m_miwEndCommandPortNo = value; }
        }
        private IoAnalogInput m_miwEndCommandCode = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwEndCommandCode
        {
            get { return m_miwEndCommandCode; }
            set { m_miwEndCommandCode = value; }
        }
        private IoAnalogInput m_miwEndCommandResult = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwEndCommandResult
        {
            get { return m_miwEndCommandResult; }
            set { m_miwEndCommandResult = value; }
        }
        private IoCollection<IoAnalogInput> m_miwPortCstIDs = new IoCollection<IoAnalogInput>();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoCollection<IoAnalogInput> miwPortCstIDs
        {
            get { return m_miwPortCstIDs; }
            set { m_miwPortCstIDs = value; }
        }

        private IoAnalogOutput m_mowCommandPortNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandPortNo
        {
            get { return m_mowCommandPortNo; }
            set { m_mowCommandPortNo = value; }
        }
        private IoAnalogOutput m_mowCommandCode = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandCode
        {
            get { return m_mowCommandCode; }
            set { m_mowCommandCode = value; }
        }
        #endregion

        #region Constructor
        public BCR_BoeHF()
        {
            this.Name = "__";
        }
        #endregion

        #region Methods
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
                //m_Comm = new XComm();
                //m_Comm.Initialize();
                //m_Comm.Simulate = m_Simul.Comport;
                //m_Comm.Open(ComPortname, 9600, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One);


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
                    m_SeqRead = new SeqBcrRead_BoeHF[m_MultiDropCount];
                    for (int i = 0; i < m_MultiDropCount; i++)
                    {
                        m_PortIds[i] = m_StartPortId + i;
                        m_SeqRead[i] = new SeqBcrRead_BoeHF(i, this);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override void SetSubscriber()
        {
            //base.SetSubscriber();

            //m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
        }

        public override int Reading(int portId)
        {
            int id = FindChannelId(portId);
            return m_SeqRead[id].Do();
        }

        public override void DirectReading(int portId)
        {
            short[] ndata = new short[DataLength];

            if (!Simul.Loader)
            {
                ndata = miwPortCstIDs[portId * 10].GetStates(DataLength);
            }

            string cstid = XFunc.ConvertToString(ndata, 0, DataLength, ByteOrder.BigEndian);
            MakeData(portId, cstid);
        }
        #endregion
    }

    public class SeqBcrRead_BoeHF : XSeqFunction
    {
        #region Fields
        private int m_ChannelId;
        private int m_PortId;
        private BCR_BoeHF m_Unit;
        private XTimer m_TimerRead = null;
        private int m_TimeoutRead = 5 * 1000;
        private int m_CommandResult = 0;
        #endregion

        #region Constructor
        public SeqBcrRead_BoeHF(int channelId, BCR_BoeHF unit)
        {
            m_ChannelId = channelId;
            m_Unit = unit;
            m_PortId = m_Unit.FindPortId(m_ChannelId);
            m_TimerRead = new XTimer(m_Unit.Name + "Read Timer");
            m_TimeoutRead = m_Unit.InterfaceTimeout;
            this.m_SeqFunName = "CST ID Read";
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
                        m_Unit.mowCommandPortNo.SetState((ushort)(m_PortId + 1));
                        m_Unit.mowCommandCode.SetState((ushort)PortCommandCode.CSTIDRead);
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Cst Id Read Request");

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 300)
                    {
                        m_Unit.mobPortCommandReadRequest.SetState(true);
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Request ON");

                        if (m_Unit.Simul.Loader)
                        {   //melsec simulation mode이면 바로 ok로 처리
                            m_Unit.mibPortCommandReadComplete.SetState(true);
                            m_TimerRead.Start(m_TimeoutRead);
                            seqNo = 20;
                        }
                        else
                        {
                            seqNo = 20;
                        }
                    }
                    break;

                case 20:
                    if (m_Unit.mibPortCommandReadComplete.GetState() == true)
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Complete ON");

                        m_Unit.mobPortCommandReadRequest.SetState(false);
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Request OFF");

                        m_Unit.mowCommandPortNo.SetState(0);
                        m_Unit.mowCommandCode.SetState(0);

                        if (m_Unit.Simul.Loader)
                        {
                            m_Unit.mibPortCommandReadComplete.SetState(false);
                        }

                        seqNo = 30;
                    }
                    else if (m_Unit.mibLoaderOffline.GetState())
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Loader Control Mode change Offline");

                        returnValue = 10;
                        m_Unit.InProcess = false;

                        seqNo = 0;
                    }
                    break;

                case 30:
                    if (m_Unit.mibPortCommandReadComplete.GetState() == false)
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Complete OFF");

                        if (m_Unit.Simul.Loader)
                        {
                            m_Unit.mibPortCommandEndReport.SetState(true);

                            m_Unit.miwEndCommandCode.SetState((short)PortCommandCode.CSTIDRead);
                            m_Unit.miwEndCommandPortNo.SetState((short)(m_PortId + 1));
                            m_Unit.miwEndCommandResult.SetState(1);
                        }

                        m_TimerRead.Start(m_TimeoutRead);
                        seqNo = 40;
                    }
                    else if (m_Unit.mibLoaderOffline.GetState())
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Loader Control Mode change Offline");

                        returnValue = 10;
                        m_Unit.InProcess = false;

                        seqNo = 0;
                    }
                    break;

                case 40:
                    if (m_Unit.mibPortCommandEndReport.GetState() == true)
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Report ON");

                        if ((m_Unit.miwEndCommandCode.GetState() == (short)PortCommandCode.CSTIDRead) &&
                           (m_Unit.miwEndCommandPortNo.GetState() == ((short)m_PortId + 1)))
                        {
                            m_CommandResult = m_Unit.miwEndCommandResult.GetState();

                            if (m_CommandResult == 1)
                            {
                                short[] ndata = new short[m_Unit.DataLength];

                                if (!m_Unit.Simul.Loader)
                                {
                                    ndata = m_Unit.miwPortCstIDs[m_PortId * 10].GetStates(m_Unit.DataLength);
                                }

                                string data = XFunc.ConvertToString(ndata, 0, m_Unit.DataLength, ByteOrder.BigEndian);
                                m_Unit.MakeData(m_PortId, data);
                            }
                            else
                            {
                                m_Unit.BcrData = "";
                            }

                            m_Unit.mobPortCommandEndConfirm.SetState(true);
                            m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Confirm ON");

                            if (m_Unit.Simul.Loader)
                            {
                                m_Unit.mibPortCommandEndReport.SetState(false);
                            }

                            seqNo = 50;
                        }
                    }
                    else if (m_Unit.mibLoaderOffline.GetState())
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Loader Control Mode change Offline");

                        returnValue = 10;
                        m_Unit.InProcess = false;

                        seqNo = 0;
                    }
                    else if (m_TimerRead.Over)
                    {
                        m_Unit.InProcess = false;
                        returnValue = 100;
                        seqNo = 0;
                    }
                    break;

                case 50:
                    if (m_Unit.mibPortCommandEndReport.GetState() == false)
                    {
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Report OFF");

                        m_Unit.mobPortCommandEndConfirm.SetState(false);
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Confirm OFF");

                        if (m_CommandResult != 2)
                        {
                            if (m_CommandResult == 1) returnValue = 0;
                            else returnValue = m_CommandResult;

                            m_Unit.InProcess = false;

                            seqNo = 0;
                        }
                        else
                        {
                            seqNo = 40;
                        }

                        string log;
                        log = string.Format("Port Command Result : {0}", m_CommandResult);
                        m_Unit.SetLog("CST ID", this.m_SeqFunName, m_PortId + 1, 0, log);

                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
    }
}
