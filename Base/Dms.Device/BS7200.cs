///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.12.04
// Author       : eun
// Description  : BS7200 Loadcell Class
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
    public class BS7200 : _DeviceAsm
    {
        #region Tag Descriptor
        //protected static TagDescriptorBS7200 tagDescriptor = new TagDescriptorBS7200();
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private XComm m_Comm;
        private PortNo m_PortNo = PortNo.COM1;
        private double m_CurValue = 0.0;
        private ArrayList m_ReceivedData = new ArrayList();
        private System.Threading.Timer m_SimulTimer = null;  //for simulation

        private const int m_DataSize = 11;
        private const int m_PacketSize = 14;
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
        [Browsable(false), XmlIgnore()]
        public double CurValue
        {
            get { return m_CurValue; }
            set { m_CurValue = value; }
        }
        #endregion

        #region Constructor
        public BS7200()
        {
            this.Name = "__";
        }
        #endregion

        #region Methods
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
                    if (str.Contains("\0\0")) break;

                    if (str.Length == m_DataSize)
                    {
                        int sign = (str.Substring(1, 1) == "+") ? 1 : -1;
                        string value = str.Substring(2, str.Length - 2);
                        m_CurValue = sign * double.Parse(value);
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

        private void GenerateRandomValue(Object stateInfo)
        {
            Random randGen = new Random();
            int genValue = randGen.Next(300, 600);

            m_CurValue = Convert.ToDouble(genValue) / 10.0;
            UpdateTag();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(BS7200); }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            //m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            //m_Tag.SetValue(tagDescriptor.VALUE, m_CurValue);
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
            m_Tag.SetValue(tagDescriptor.VALUE, m_CurValue);
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
                if (m_Simul.Comport)
                {
                    m_SimulTimer = new System.Threading.Timer(new TimerCallback(GenerateRandomValue), null, 100, 100);
                }


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
