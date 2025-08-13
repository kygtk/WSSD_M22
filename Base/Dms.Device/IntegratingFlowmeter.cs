using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Dms.Ctl;
using System.Threading;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class IntegratingFlowmeter : _DeviceAsm
    {
        #region Enum
        enum ControlCharacters
        {
            ccSTX = 0x02,		// Start Byte
            ccETX = 0x03,		// End Byte
            ccENQ = 0x05,		// Request to Send
            ccEOT = 0x04,		// Request to Receive
            ccACK = 0x06,		// Correct Reception
            ccNAK = 0x15,		// Incorrect Reception
        };

        enum _TERMINATE_CODE
        {
            ccOK = 0x11,
            ccNG = 0x0F,
        };
        #endregion

        #region Tag Descriptor
        protected static TagDescriptorIntegratingFlowMeter tagDescriptor = new TagDescriptorIntegratingFlowMeter();
        #endregion

        #region Fields
        private IoDigitalOutput m_FlowMeterReset = new IoDigitalOutput();
        #endregion

        #region Fields
        private PortNo m_PortNo = PortNo.COM4;
        private XComm m_Comm;
        private float m_CurFlowrate = 0.0f;
        private float m_IntegralFlow = 0.0f;
        private int m_UnitNo = 1;
        private System.Threading.Timer m_SendCommandTimer = null;
        private int m_MaxLength = 19;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalOutput FlowMeterReset
        {
            get { return m_FlowMeterReset; }
            set { m_FlowMeterReset = value; }
        }
        [Category("DMS : Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Category("DMS : Setting")]
        public int MaxLength
        {
            get { return m_MaxLength; }
            set { m_MaxLength = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string IntegralFlow // 09.11.03 minhan 
        {
            get { return m_IntegralFlow.ToString(); }
        }
        [Browsable(false), XmlIgnore()]
        public string PortName
        {
            get { return m_PortNo.ToString(); }
        }
        #endregion

        #region Constructor
        public IntegratingFlowmeter()
        {
            this.Name = "__";
        }
        #endregion

        #region Methods
        private void ReceivedData(object sender)
        {
            try
            {
                Thread.Sleep(100); 
                string recvData = "";
                char[] buf = new char[MaxLength];
                int len = m_Comm.Read(ref buf, MaxLength);
                
                if (len > 0)
                {
                    if (buf[0] == (char)ControlCharacters.ccSTX)
                    {
                        if (len == MaxLength && buf[18] == (char)ControlCharacters.ccETX)
                        {
                          if(buf[1] != 'C')
                            {
                                foreach (char cbuf in buf)
                                    recvData += string.Format("{0}", cbuf);
                                char chr = buf[3];
                                if (chr == (char)_TERMINATE_CODE.ccOK)
                                {
                                    string str;
                                    int point;
                                    //순시 Data
                                    str = recvData.Substring(4, 6);
                                    point = Convert.ToInt32(recvData.Substring(10, 1));
                                    str = str.Insert(6 - point, ".");
                                    m_CurFlowrate = Convert.ToSingle(str);
                                    //적산 Data
                                    str = recvData.Substring(11, 6);
                                    point = Convert.ToInt32(recvData.Substring(17, 1));
                                    str = str.Insert(6 - point, ".");
                                    float val = Convert.ToSingle(str);
                                    m_IntegralFlow = val;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                m_Comm.SetLog(err.ToString());
             //   MessageBox.Show(err.ToString());
            }
        }

        public void SendCommand(Object stateInfo)
        {
            string sendData;
            sendData = Convert.ToChar((char)ControlCharacters.ccSTX) + string.Format("{0:d02}DATA?", m_UnitNo) + Convert.ToChar((char)ControlCharacters.ccETX);
            m_Comm.Write(sendData);
        }

        public void SetUnitNo(int unitNo)
        {
            m_UnitNo = unitNo;
        }

        public void Reset()
        {
            m_CurFlowrate = 0.0f;
            m_IntegralFlow = 0.0f;
        }
        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.


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
            //ok &= (FlowMeterReset != null);
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
                m_Comm.Open(PortName, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


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
                    m_SendCommandTimer = new System.Threading.Timer(new System.Threading.TimerCallback(SendCommand), null, 10, 500);
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

        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
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
            m_Tag.SetValue(tagDescriptor.CURFLOW, m_CurFlowrate);
            m_Tag.SetValue(tagDescriptor.INTEGRALFLOW, m_IntegralFlow);
            m_Tag.SetValue(tagDescriptor.FLOWMETERRESET, FlowMeterReset.GetState());
        }

        public override Type FamilyType
        {
            get
            {
                return typeof(IntegratingFlowmeter);
            }
        }
        #endregion
    }
}
