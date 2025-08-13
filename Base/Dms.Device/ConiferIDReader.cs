using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using Dms.Ctl;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ConiferIDReader : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorConiferIDReader tagDescriptor = new TagDescriptorConiferIDReader();
        #endregion

        #region Fields
        private XComm m_Comm = null;
        private PortNo m_PortNo = PortNo.COM1;
        private BaudRate m_BaudRate = BaudRate.Low;
        private ArrayList m_ReceivedData = new ArrayList();

        private const int m_DataSize = 8;
        private readonly string m_SendFrameFormat;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string PortName
        {
            get { return m_PortNo.ToString(); }
        }
        [Category("DMS : Setting")]
        public BaudRate BaudRate
        {
            get { return m_BaudRate; }
            set { m_BaudRate = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string BaudRateName
        {
            get { return m_BaudRate.ToString(); }
        }
        #endregion

        #region Constructor
        public ConiferIDReader()
        {
            this.Name = "__";
            m_SendFrameFormat = Convert.ToChar(0x3C) + Convert.ToChar(0x20) + Convert.ToChar(0x3E) + "\r\n";

        }
        #endregion

        #region Methods
        private void SendData()
        {
            m_Comm.Write(m_SendFrameFormat);
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

                foreach (char str in m_ReceivedData)
                {

                }
            }
            catch (Exception err)
            {
                if (m_Simul.Comport)
                {
                    MessageBox.Show(err.ToString());
                }
            }
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ConiferIDReader); }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            m_Tag.SetValue(tagDescriptor.VALUE, "");
        }

        public override void UpdateTag()
        {
            //m_Tag.SetValue(tagDescriptor.VALUE, m_CurValue);
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
                m_Comm.Open(PortName, 9600/*int.Parse(BaudRateName)*/, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


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