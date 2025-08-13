using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using Dms.Common;
using Dms.Ctl;
using System.Xml.Serialization;

namespace Dms.Device
{
    public delegate double GetCurrentRbGapGuageValue(int id, double recvData);
 
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class MitutoyoRbGapGauge : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private XComm m_Comm;
        private PortNo m_PortNo = PortNo.COM1;
        private double m_CurValue = 0.0;
        private ArrayList m_ReceivedData = new ArrayList();
        private System.Threading.Timer m_PollingTimer = null;

        private const int m_DataSize = 12;
        private int m_ChannelNo;

        public event GetCurrentRbGapGuageValue OnGetCurrentGapGaugeValue;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Category("DMS : Setting")]
        public int ChannelNo
        {
            get { return m_ChannelNo; }
            set { m_ChannelNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string PortName
        {
            get { return m_PortNo.ToString(); }
        }
        [Browsable(false), XmlIgnore()]
        public double CurValue
        {
            get { return m_CurValue; }
            set { m_CurValue = value; }
        }
        #endregion

        #region Constructor
        public MitutoyoRbGapGauge()
        {
            this.Name = "__ Gap Gauge";
        }
        #endregion

        #region Methods
        //--------------------------------------------------------------------
        // PC => MUX : "1\r"	: channel 1
        //			   "2\r"	: channel 2
        //			   ...
        //			   "A\r"	: Channel All
        //--------------------------------------------------------------------
        public void SendCommand(int ch)
        {
            string sendData = Convert.ToChar(ch) + "\r";
            m_Comm.Write(sendData);
        }

        private void ReceivedData(object sender)
        {
            try
            {
                string recvData = m_Comm.ReadLine();
                if (null != recvData)
                {
                    m_ReceivedData.Add(recvData);
                }

                foreach (string str in m_ReceivedData)
                {
                    if (str.Length < m_DataSize) m_CurValue = 0.00;
                    else
                    {
                        double recvValue = Convert.ToDouble(str.Substring(3, 9));
                        if (OnGetCurrentGapGaugeValue != null)
                        {
                            m_CurValue = OnGetCurrentGapGaugeValue(this.Id, recvValue);
                        }
                        else
                        {
                            m_CurValue = recvValue;
                        }
                        UpdateTag();
                    }
                }
                m_ReceivedData.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void RequestCurValue(Object stateInfo)
        {
            SendCommand(m_ChannelNo);
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            m_Tag.SetValue(tagDescriptor.VALUE, m_CurValue);
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.VALUE, m_CurValue);
        }

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
                m_Comm.Open(PortName, 2400, System.IO.Ports.Parity.Even, 8, System.IO.Ports.StopBits.One);
                m_PollingTimer = new System.Threading.Timer(new TimerCallback(RequestCurValue), null, 300, 300);


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
        #endregion
    }
}
