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
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class EuvUnitComm : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region enum
        enum ControlCharacters
	    {
		    ccSTX	= 0x02,		// Start Byte
		    ccETX	= 0x03,		// End Byte
		    ccNOD1	= 0x30,		// Node No Byte
		    ccNOD2	= 0x30,		// Char '0'
		    ccSBA1	= 0x30,		// Char '0'
		    ccSBA2	= 0x30,		// Char '0'
		    ccSID	= 0x30,		// Char '0'
		    ccData	= 0001,
		    ccBCC_H	= 0x33,		// Char '3'
		    ccBCC_L	= 0x31,		// Char '1'
		    ccENQ	= 0x05,		// Request to Send
		    ccEOT	= 0x04,		// Request to Receive
		    ccACK	= 0x06,		// Correct Reception
		    ccNAK	= 0x15,		// Incorrect Reception
	    };
        #endregion

        #region Fields
        private XComm m_Comm;
        private PortNo m_PortNo = PortNo.COM1;
        private string m_CurData;
        private ArrayList m_ReceivedData = new ArrayList();
        //private static System.Threading.Timer m_SimulTimer = null;
        
        private const int m_DataSize = 12;
        //private int m_ChannelNo;
        public bool bReceived = false;
        public delegate void GetEuvReceivedData(object sender);
        public event GetEuvReceivedData DataReceived;
      //  OnEuvUnitComm += new GetEuvReceivedData(  );
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
        public string CurData
        {
            get { return m_CurData; }
            set { m_CurData = value; }
        }
        #endregion

        #region Constructor
        public EuvUnitComm()
        {
            this.Name = "__ Euv Comm";
        }
        #endregion

        #region Methods
        
        //--------------------------------------------------------------------
        // PC => MUX : "1\r"	: channel 1
        //			   "2\r"	: channel 2
        //			   ...
        //			   "A\r"	: Channel All
        //--------------------------------------------------------------------
        public bool SendCommand(char ch)
        {
            bool bRv = false;
            string sSenddata = string.Format( "%c\r", ch );
            bRv = m_Comm.Write(sSenddata);
	        //m_sSendData.ReleaseBuffer();
	        return bRv;
        }
        public bool SendCommand(string sCmd)
        {
            bool bRv = false;
               
            string sSenddata = Convert.ToString((char)ControlCharacters.ccSTX) + sCmd + Convert.ToString((char)ControlCharacters.ccETX); // 09.07.29 minhan
            bRv = m_Comm.Write(sSenddata);
	       // m_sSendData.ReleaseBuffer();
	        return bRv;
        }
//         public bool IsCommRcvd()
//         {
//             if ( bReceived )
//             {
//                 bReceived = false;
//                 return true;
//             }
//             else return false;
      
//             return true;
//         }
        private void ReceivedData(object sender)
        {
            try
            {
                Thread.Sleep(1000);
                //string recvData = m_Comm.ReadLine();//zhangliang 130924
                string recvData = m_Comm.ReadExisting();
                
                if (null != recvData)
                {
                    m_ReceivedData.Add(recvData);
                }

                foreach (string str in m_ReceivedData)
                {
                    if (str.Length < m_DataSize) m_CurData ="";
                    else
                    {
                        string recvValue = str;
                        m_CurData = recvValue;
                        if( DataReceived != null)
                         DataReceived(this);
                     //   UpdateTag();
                    }
                }
                m_ReceivedData.Clear();
              //  bReceived = true;
            }
            catch (Exception err)
            {
                m_Comm.SetLog(err.ToString());
             //   MessageBox.Show(err.ToString());
            }
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
//             m_Tag.SetValue(tagDescriptor.VALUE, m_CurData);
        }

        public override void UpdateTag()
        {
        //    m_Tag.SetValue(tagDescriptor.VALUE, m_CurData);
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
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                
                m_Comm = new XComm();
                m_Comm.Initialize();
                m_Comm.Simulate = m_Simul.Comport;
                m_Comm.Open(PortName, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.Two);
               // m_SimulTimer = new System.Threading.Timer(new TimerCallback(RequestCurValue), null, 300, 300);


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
