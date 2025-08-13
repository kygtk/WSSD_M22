///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.03.18
// Author       : eun
// Description  : SPG Motor 부하율 측정을 위한 Device(XOP-L20)
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;
using Dms.Common;
using Dms.Ctl;

namespace Dms.Device
{
	public class MotorLoadFactor : _DeviceAsm
	{
		#region Tag Descriptor
		protected static TagDescriptorMotorLoadFactor tagDescriptor = new TagDescriptorMotorLoadFactor();
		#endregion

		#region Fields
		private XComm m_Comm;
		private PortNo m_PortNo = PortNo.COM1;
		private int m_IdNo = 0;					//모듈의 ID
		private const int Max_Channel_No = 20;	//하나의 모듈에 연결할 수 있는 최대 모터의 개수. 만약 이 값을 20이 아닌 다른 값으로 수정하려면, TagDescriptorMotorLoadFactor도 수정해 주어야 함.
		private float[] m_CurValues = new float[Max_Channel_No];
		private System.Threading.Timer m_PollingTimer = null;
		#endregion

		#region Properties
		[Category("DMS : Setting")]
		public PortNo PortNo
		{
			get { return m_PortNo; }
			set { m_PortNo = value; }
		}
		[Category("DMS : Setting")]
		public int IdNo
		{
			get { return m_IdNo; }
			set { m_IdNo = value; }
		}
		[Browsable(false), XmlIgnore()]
		public string PortName
		{
			get { return m_PortNo.ToString(); }
		}
		#endregion

		#region Constructor
		public MotorLoadFactor()
		{
			this.Name = "__ Motor Load Factor";
		}
		#endregion

		#region Methods
		private void RequestCurValue(Object stateInfo)
        {
            SendCommand();
        }

		//buf[0] = byte(0x01); //SOH
		//buf[1] = byte(0x02); //LENGTH
		//buf[2] = byte(0x00); //ID NO. (Slave의 고유 번호)
		//buf[3] = byte(0x80); //COMMAND
		//buf[4] = byte(0x80); //CHECKSUM (ID NO. ~ COMMAND까지의 8-bit 논리합)
		
		private const byte m_Length = 0x02;	//ID NO ~ COMMAND까지의 길이
		private const int m_Command = 0x80;	//Ch0~Ch19 request
        private byte[] m_SendStream = new byte[5];
		private void SendCommand()
		{
			//jemoon : string으로 조합하면 '0'일때 null로 되면서 이후가 짤린다.
			//char checksum = (char)(m_IdNo | m_Command);
			//string command = string.Format("{0}{1}{2}{3}{4}", AsciiChar.SOH, m_Length, (char)m_IdNo, (char)m_Command, checksum);

			//m_Comm.Write(command);
			//jemoon : char 절대 안됨 필히 byte type으로 송신 할것!!!!!!!		
			byte checksum = (byte)(m_IdNo | m_Command);
			m_SendStream[0] = (byte)Asciis.SOH;
			m_SendStream[1] = m_Length;
			m_SendStream[2] = (byte)m_IdNo;
			m_SendStream[3] = (byte)m_Command;
			m_SendStream[4] = checksum;
			bool ok = m_Comm.Write(m_SendStream, 0, 5);
		}

		//SOH / LEN(10) / LEN(1) / COMMAND
		//DATA0(100) / DATA0(10) / DATA0(1) / DATA0(0.1)
		//DATA1(100) / DATA1(10) / DATA1(1) / DATA1(0.1)
		//...
		//DATA19(100) / DATA19(10) / DATA19(1) / DATA19(0.1)
		//CR/LF
		private const int m_MaxLength = 5 + (4 * Max_Channel_No);
		private void ReceivedData(object sender)
		{
            try
            {   //Data parsing 오류로 인한 crash 방지
                string recvData = m_Comm.ReadLine();

                if (recvData.Length < m_MaxLength) return;	//최대길이확인

                byte soh = (byte)Convert.ToChar(recvData.Substring(0, 1));
                byte command = (byte)Convert.ToChar(recvData.Substring(3, 1));

                if (soh != (byte)Asciis.SOH) return;		//SOH확인
                //jemoon : command를 80h(all)로 보냈을때 회신이 80h로 돌아 오지 않는다
                //byte -> string -> byte로 변환하면서 이상해 지는건지...
                //이후 data는 정상이므로 command check는 일단 skip
                //if (command != (byte)m_Command) return;	//COMMAND확인

                //필터링이 안됨. 정규 표현식 오류. 80자리 체크해야함
                //string dataPattern = "[0-9]";
                string dataPattern = "[0-9]{80}";
                System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(dataPattern);
                if (!rex.IsMatch(recvData.Substring(4, 4 * Max_Channel_No))) return;	//DATA가 모두 숫자인지 확인

                ParseData(recvData);
            }
            catch
            { 
            }
		}

		private void ParseData(string data)
		{
			for (int i = 1; i < Max_Channel_No + 1; i++)
			{
				m_CurValues[i - 1] = Convert.ToSingle(data.Substring(i * 4, 4)) / 10;
			}
		}
		#endregion

		#region Override
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
			//ok &= (m_AoGaugeFlowSet != null);
			//ok &= (m_Gauge != null);

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
				//jemoon
				//usb-232 converter <-> 485 연결시 dtr enable 하지 않으면 송신이 안됨
				//485 : realsys cnv485ns
				m_Comm.SerialPort.DtrEnable = true;
				
				m_PollingTimer = new System.Threading.Timer(new TimerCallback(RequestCurValue), null, 300, 1000);


				if (m_Simul.Device)
				{
					//m_GetPressure = m_Server.JobCond.CurrentRecipe.BaseVacuumPress;
				}


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

		public override void UpdateTag()
		{
			m_Tag.SetValue(tagDescriptor.MOTOR1, m_CurValues[0]);
			m_Tag.SetValue(tagDescriptor.MOTOR2, m_CurValues[1]);
			m_Tag.SetValue(tagDescriptor.MOTOR3, m_CurValues[2]);
			m_Tag.SetValue(tagDescriptor.MOTOR4, m_CurValues[3]);
			m_Tag.SetValue(tagDescriptor.MOTOR5, m_CurValues[4]);
			m_Tag.SetValue(tagDescriptor.MOTOR6, m_CurValues[5]);
			m_Tag.SetValue(tagDescriptor.MOTOR7, m_CurValues[6]);
			m_Tag.SetValue(tagDescriptor.MOTOR8, m_CurValues[7]);
			m_Tag.SetValue(tagDescriptor.MOTOR9, m_CurValues[8]);
			m_Tag.SetValue(tagDescriptor.MOTOR10, m_CurValues[9]);
			m_Tag.SetValue(tagDescriptor.MOTOR11, m_CurValues[10]);
			m_Tag.SetValue(tagDescriptor.MOTOR12, m_CurValues[11]);
			m_Tag.SetValue(tagDescriptor.MOTOR13, m_CurValues[12]);
			m_Tag.SetValue(tagDescriptor.MOTOR14, m_CurValues[13]);
			m_Tag.SetValue(tagDescriptor.MOTOR15, m_CurValues[14]);
			m_Tag.SetValue(tagDescriptor.MOTOR16, m_CurValues[15]);
			m_Tag.SetValue(tagDescriptor.MOTOR17, m_CurValues[16]);
			m_Tag.SetValue(tagDescriptor.MOTOR18, m_CurValues[17]);
			m_Tag.SetValue(tagDescriptor.MOTOR19, m_CurValues[18]);
			m_Tag.SetValue(tagDescriptor.MOTOR20, m_CurValues[19]);
		}

		public override void SetSubscriber()
        {
            base.SetSubscriber();

            m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
        }

		public override DmsErrors Uninitialize()
		{
			if (m_Comm.IsOpen())
			{
				m_Comm.Close();
			}
			return base.Uninitialize();
		}
		#endregion
	}
}
