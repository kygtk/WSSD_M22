using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Dms.Data;
using Dms.Common;
using Dms.Mitsubishi;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Ctl;
using System.Reflection;
using Dms.Device;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Dms.Server
{
	// define interface data

	public class IfDevice : _DeviceAsm
	{
		#region Tag Descriptor
		protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
		#endregion

		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public IfDevice()
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
			//ok &= (m_DiAlive != null);
			//ok &= (m_AiCurrentRecipeId != null);


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
		#endregion
	}

	[Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
	public class IfSignalFromCim : IfDevice
	{
		#region Fields
		private IoDigitalInput m_DiTimeDataSend;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiTimeDataSend
		{
			get { return m_DiTimeDataSend; }
			set { m_DiTimeDataSend = value; }
		}

		private IoDigitalInput m_DiAlarmReleaseRequest;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiAlarmReleaseRequest
		{
			get { return m_DiAlarmReleaseRequest; }
			set { m_DiAlarmReleaseRequest = value; }
		}

		private IoDigitalInput m_DiEcsStart;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiEcsStart
		{
			get { return m_DiEcsStart; }
			set { m_DiEcsStart = value; }
		}

		private IoDigitalInput m_DiLotEndSend;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiLotEndSend
		{
			get { return m_DiLotEndSend; }
			set { m_DiLotEndSend = value; }
		}

		private IoDigitalInput m_DiLostGlassRequestAckOk;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiLostGlassRequestAckOk
		{
			get { return m_DiLostGlassRequestAckOk; }
			set { m_DiLostGlassRequestAckOk = value; }
		}

		private IoDigitalInput m_DiLostGlassRequestAckNg;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiLostGlassRequestAckNg
		{
			get { return m_DiLostGlassRequestAckNg; }
			set { m_DiLostGlassRequestAckNg = value; }
		}

		private IoDigitalInput m_DiRecipeBodyDataRequest;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiRecipeBodyDataRequest
		{
			get { return m_DiRecipeBodyDataRequest; }
			set { m_DiRecipeBodyDataRequest = value; }
		}

		private IoDigitalInput m_DiRecipeVariousDataRequest;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiRecipeVariousDataRequest
		{
			get { return m_DiRecipeVariousDataRequest; }
			set { m_DiRecipeVariousDataRequest = value; }
		}

		private IoDigitalInput m_DiRecipeVariousDataConfirm;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiRecipeVariousDataConfirm
		{
			get { return m_DiRecipeVariousDataConfirm; }
			set { m_DiRecipeVariousDataConfirm = value; }
		}

		private IoDigitalInput m_DiLostGlassDataReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiLostGlassDataReport
		{
			get { return m_DiLostGlassDataReport; }
			set { m_DiLostGlassDataReport = value; }
		}

		private IoDigitalInput m_DiUnloadGlassDataRequest;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiUnloadGlassDataRequest
		{
			get { return m_DiUnloadGlassDataRequest; }
			set { m_DiUnloadGlassDataRequest = value; }
		}

		private IoDigitalInput m_DiGlassDataSend;
		[Category("DMS : I/O Setting")]
		public IoDigitalInput DiGlassDataSend
		{
			get { return m_DiGlassDataSend; }
			set { m_DiGlassDataSend = value; }
		}

		private IoAnalogInput m_AiTimeYear;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiTimeYear
		{
			get { return m_AiTimeYear; }
			set { m_AiTimeYear = value; }
		}

		private IoAnalogInput m_AiTimeMonth;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiTimeMonth
		{
			get { return m_AiTimeMonth; }
			set { m_AiTimeMonth = value; }
		}

		private IoAnalogInput m_AiTimeDay;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiTimeDay
		{
			get { return m_AiTimeDay; }
			set { m_AiTimeDay = value; }
		}

		private IoAnalogInput m_AiTimeHour;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiTimeHour
		{
			get { return m_AiTimeHour; }
			set { m_AiTimeHour = value; }
		}

		private IoAnalogInput m_AiTimeMinute;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiTimeMinute
		{
			get { return m_AiTimeMinute; }
			set { m_AiTimeMinute = value; }
		}

		private IoAnalogInput m_AiTimeSecond;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiTimeSecond
		{
			get { return m_AiTimeSecond; }
			set { m_AiTimeSecond = value; }
		}

		private IoCollection<IoAnalogInput> m_AiLotEndLotIds = new IoCollection<IoAnalogInput>();	//length : 8 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogInput> AiLotEndLotIds
		{
			get { return m_AiLotEndLotIds; }
			set { m_AiLotEndLotIds = value; }
		}

		private IoAnalogInput m_AiLotEndLotNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiLotEndLotNumber
		{
			get { return m_AiLotEndLotNumber; }
			set { m_AiLotEndLotNumber = value; }
		}

		private IoAnalogInput m_AiRecipeBodyRequestRecipeNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeBodyRequestRecipeNumber
		{
			get { return m_AiRecipeBodyRequestRecipeNumber; }
			set { m_AiRecipeBodyRequestRecipeNumber = value; }
		}

		private IoCollection<IoAnalogInput> m_AiLostGlassDatas = new IoCollection<IoAnalogInput>(); //length : 48 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogInput> AiLostGlassDatas
		{
			get { return m_AiLostGlassDatas; }
			set { m_AiLostGlassDatas = value; }
		}

		private IoAnalogInput m_AiGlassDataSendFlag;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiGlassDataSendFlag
		{
			get { return m_AiGlassDataSendFlag; }
			set { m_AiGlassDataSendFlag = value; }
		}

		private IoCollection<IoAnalogInput> m_AiGlassDatas = new IoCollection<IoAnalogInput>(); //length : 48 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogInput> AiGlassDatas
		{
			get { return m_AiGlassDatas; }
			set { m_AiGlassDatas = value; }
		}

		private IoAnalogInput m_AiRequestCeid;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRequestCeid
		{
			get { return m_AiRequestCeid; }
			set { m_AiRequestCeid = value; }
		}

		private IoAnalogInput m_AiRecipeVariousRequestRecipeCommand;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeVariousRequestRecipeCommand
		{
			get { return m_AiRecipeVariousRequestRecipeCommand; }
			set { m_AiRecipeVariousRequestRecipeCommand = value; }
		}

		private IoAnalogInput m_AiRecipeVariousRequestRecipeNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeVariousRequestRecipeNumber
		{
			get { return m_AiRecipeVariousRequestRecipeNumber; }
			set { m_AiRecipeVariousRequestRecipeNumber = value; }
		}

		private IoAnalogInput m_AiRecipeVariousRequestRecipeLevel;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeVariousRequestRecipeLevel
		{
			get { return m_AiRecipeVariousRequestRecipeLevel; }
			set { m_AiRecipeVariousRequestRecipeLevel = value; }
		}

		private IoAnalogInput m_AiRecipeVariousRequestRecipeType;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeVariousRequestRecipeType
		{
			get { return m_AiRecipeVariousRequestRecipeType; }
			set { m_AiRecipeVariousRequestRecipeType = value; }
		}

		private IoCollection<IoAnalogInput> m_AiRecipeVariousRequestRecipeVersions = new IoCollection<IoAnalogInput>(); //length : 3 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogInput> AiRecipeVariousRequestRecipeVersions
		{
			get { return m_AiRecipeVariousRequestRecipeVersions; }
			set { m_AiRecipeVariousRequestRecipeVersions = value; }
		}

		private IoAnalogInput m_AiRequestUnitNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRequestUnitNumber
		{
			get { return m_AiRequestUnitNumber; }
			set { m_AiRequestUnitNumber = value; }
		}

		private IoAnalogInput m_AiRecipeVariousReportConfirmCeid;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeVariousReportConfirmCeid
		{
			get { return m_AiRecipeVariousReportConfirmCeid; }
			set { m_AiRecipeVariousReportConfirmCeid = value; }
		}

		private IoAnalogInput m_AiRecipeVariousReportConfirmAck;
		[Category("DMS : I/O Setting")]
		public IoAnalogInput AiRecipeVariousReportConfirmAck
		{
			get { return m_AiRecipeVariousReportConfirmAck; }
			set { m_AiRecipeVariousReportConfirmAck = value; }
		}

		private IoCollection<IoAnalogInput> m_AiEqpSpecificDatas = new IoCollection<IoAnalogInput>(); //length : 16 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogInput> AiEqpSpecificDatas
		{
			get { return m_AiEqpSpecificDatas; }
			set { m_AiEqpSpecificDatas = value; }
		}

		private IoCollection<IoAnalogInput> m_AiRecipeParameters = new IoCollection<IoAnalogInput>(); //length : 48 word ?
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogInput> AiRecipeParameters
		{
			get { return m_AiRecipeParameters; }
			set { m_AiRecipeParameters = value; }
		}
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public IfSignalFromCim()
		{
			this.Name = "__";
		}
		#endregion

		#region Methods
		#endregion

		#region Override
		#endregion
	}

	[Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
	public class IfSignalToCim : IfDevice
	{
		#region Fields
		private IoDigitalOutput m_DoOnlineState;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoOnlineState
		{
			get { return m_DoOnlineState; }
			set { m_DoOnlineState = value; }
		}

		private IoDigitalOutput m_DoUnitAutoMode;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoUnitAutoMode
		{
			get { return m_DoUnitAutoMode; }
			set { m_DoUnitAutoMode = value; }
		}

		private IoDigitalOutput m_DoOperationCycleStop;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoOperationCycleStop
		{
			get { return m_DoOperationCycleStop; }
			set { m_DoOperationCycleStop = value; }
		}

		private IoCollection<IoDigitalOutput> m_DoUnitStatus = new IoCollection<IoDigitalOutput>();//Length : 3 bit
		[Category("DMS : I/O Setting")]
		public IoCollection<IoDigitalOutput> DoUnitStatus
		{
			get { return m_DoUnitStatus; }
			set { m_DoUnitStatus = value; }
		}

		//private IoDigitalOutput m_DoUnitStatus;
		//[Category("DMS : I/O Setting")]
		//public IoDigitalOutput DoUnitStatusRun
		//{
		//    get { return m_DoUnitStatusRun; }
		//    set { m_DoUnitStatusRun = value; }
		//}

		//private IoDigitalOutput m_DoUnitStatusIdle;
		//[Category("DMS : I/O Setting")]
		//public IoDigitalOutput DoUnitStatusIdle
		//{
		//    get { return m_DoUnitStatusIdle; }
		//    set { m_DoUnitStatusIdle = value; }
		//}

		//private IoDigitalOutput m_DoUnitStatusDown;
		//[Category("DMS : I/O Setting")]
		//public IoDigitalOutput DoUnitStatusDown
		//{
		//    get { return m_DoUnitStatusDown; }
		//    set { m_DoUnitStatusDown = value; }
		//}

		private IoDigitalOutput m_DoGlassInProcessing;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoGlassInProcessing
		{
			get { return m_DoGlassInProcessing; }
			set { m_DoGlassInProcessing = value; }
		}

		private IoDigitalOutput m_DoGlassExistInUnit;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoGlassExistInUnit
		{
			get { return m_DoGlassExistInUnit; }
			set { m_DoGlassExistInUnit = value; }
		}

		private IoDigitalOutput m_DoLightAlarmReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoLightAlarmReport
		{
			get { return m_DoLightAlarmReport; }
			set { m_DoLightAlarmReport = value; }
		}

		private IoDigitalOutput m_DoHeavyAlarmReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoHeavyAlarmReport
		{
			get { return m_DoHeavyAlarmReport; }
			set { m_DoHeavyAlarmReport = value; }
		}

		private IoCollection<IoDigitalOutput> m_DoUnitOperationModes = new IoCollection<IoDigitalOutput>(); //Length : 16 bit
		[Category("DMS : I/O Setting")]
		public IoCollection<IoDigitalOutput> DoUnitOperationModes
		{
			get { return m_DoUnitOperationModes; }
			set { m_DoUnitOperationModes = value; }
		}

		private IoDigitalOutput m_DoGlassApdReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoGlassApdReport
		{
			get { return m_DoGlassApdReport; }
			set { m_DoGlassApdReport = value; }
		}

		private IoDigitalOutput m_DoScrapGlassDataReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoScrapGlassDataReport
		{
			get { return m_DoScrapGlassDataReport; }
			set { m_DoScrapGlassDataReport = value; }
		}

		private IoDigitalOutput m_DoGlassDataRequest;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoGlassDataRequest
		{
			get { return m_DoGlassDataRequest; }
			set { m_DoGlassDataRequest = value; }
		}

		private IoDigitalOutput m_DoGlassDataChangeReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoGlassDataChangeReport
		{
			get { return m_DoGlassDataChangeReport; }
			set { m_DoGlassDataChangeReport = value; }
		}

		private IoDigitalOutput m_DoRecipeBodyDataReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoRecipeBodyDataReport
		{
			get { return m_DoRecipeBodyDataReport; }
			set { m_DoRecipeBodyDataReport = value; }
		}

		private IoDigitalOutput m_DoEqpRecipeListChange;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoEqpRecipeListChange
		{
			get { return m_DoEqpRecipeListChange; }
			set { m_DoEqpRecipeListChange = value; }
		}

		private IoDigitalOutput m_DoLotApdReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoLotApdReport
		{
			get { return m_DoLotApdReport; }
			set { m_DoLotApdReport = value; }
		}

		private IoDigitalOutput m_DoRecipeVariousRequestConfirm;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoRecipeVariousRequestConfirm
		{
			get { return m_DoRecipeVariousRequestConfirm; }
			set { m_DoRecipeVariousRequestConfirm = value; }
		}

		private IoDigitalOutput m_DoRecipeVariousDataReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoRecipeVariousDataReport
		{
			get { return m_DoRecipeVariousDataReport; }
			set { m_DoRecipeVariousDataReport = value; }
		}

		private IoDigitalOutput m_DoUnloadGlassDataReport;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoUnloadGlassDataReport
		{
			get { return m_DoUnloadGlassDataReport; }
			set { m_DoUnloadGlassDataReport = value; }
		}

		private IoDigitalOutput m_DoLoadGlassDataRequest;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoLoadGlassDataRequest
		{
			get { return m_DoLoadGlassDataRequest; }
			set { m_DoLoadGlassDataRequest = value; }
		}

		private IoDigitalOutput m_DoUnloadDataRequestConfirm;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoUnloadDataRequestConfirm
		{
			get { return m_DoUnloadDataRequestConfirm; }
			set { m_DoUnloadDataRequestConfirm = value; }
		}

		private IoDigitalOutput m_DoLoadGlassDataReceive;
		[Category("DMS : I/O Setting")]
		public IoDigitalOutput DoLoadGlassDataReceive
		{
			get { return m_DoLoadGlassDataReceive; }
			set { m_DoLoadGlassDataReceive = value; }
		}

		private IoCollection<IoDigitalOutput> m_DoUnitStatusProductTypes = new IoCollection<IoDigitalOutput>();//3bit
		[Category("DMS : I/O Setting")]
		public IoCollection<IoDigitalOutput> DoUnitStatusProductTypes
		{
			get { return m_DoUnitStatusProductTypes; }
			set { m_DoUnitStatusProductTypes = value; }
		}

		//private IoDigitalOutput m_DoUnitStatusExperiment;
		//[Category("DMS : I/O Setting")]
		//public IoDigitalOutput DoUnitStatusExperiment
		//{
		//    get { return m_DoUnitStatusExperiment; }
		//    set { m_DoUnitStatusExperiment = value; }
		//}

		//private IoDigitalOutput m_DoUnitStatusMonitoring;
		//[Category("DMS : I/O Setting")]
		//public IoDigitalOutput DoUnitStatusMonitoring
		//{
		//    get { return m_DoUnitStatusMonitoring; }
		//    set { m_DoUnitStatusMonitoring = value; }
		//}

		//private IoDigitalOutput m_DoUnitStatusDevelopment;
		//[Category("DMS : I/O Setting")]
		//public IoDigitalOutput DoUnitStatusDevelopment
		//{
		//    get { return m_DoUnitStatusDevelopment; }
		//    set { m_DoUnitStatusDevelopment = value; }
		//}

		private IoCollection<IoDigitalOutput> m_DoPositionGlassExists = new IoCollection<IoDigitalOutput>(); //Length : 32 bit
		[Category("DMS : I/O Setting")]
		public IoCollection<IoDigitalOutput> DoPositionGlassExists
		{
			get { return m_DoPositionGlassExists; }
			set { m_DoPositionGlassExists = value; }
		}

		private IoAnalogOutput m_AoTpdUpdateTime;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoTpdUpdateTime
		{
			get { return m_AoTpdUpdateTime; }
			set { m_AoTpdUpdateTime = value; }
		}

		private IoAnalogOutput m_AoTpdReportNo1;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoTpdReportNo1
		{
			get { return m_AoTpdReportNo1; }
			set { m_AoTpdReportNo1 = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoTpdReports = new IoCollection<IoAnalogOutput>(); //Length : 64 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoTpdReports
		{
			get { return m_AoTpdReports; }
			set { m_AoTpdReports = value; }
		}

		private IoAnalogOutput m_AoTpdReportNo2;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoTpdReportNo2
		{
			get { return m_AoTpdReportNo2; }
			set { m_AoTpdReportNo2 = value; }
		}

		private IoAnalogOutput m_AoIonizerStatus;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoIonizerStatus
		{
			get { return m_AoIonizerStatus; }
			set { m_AoIonizerStatus = value; }
		}

		private IoAnalogOutput m_AoUnitDownAlarmCode;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoUnitDownAlarmCode
		{
			get { return m_AoUnitDownAlarmCode; }
			set { m_AoUnitDownAlarmCode = value; }
		}

		private IoAnalogOutput m_AoCurrentEqpRecipeNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoCurrentEqpRecipeNumber
		{
			get { return m_AoCurrentEqpRecipeNumber; }
			set { m_AoCurrentEqpRecipeNumber = value; }
		}

		private IoAnalogOutput m_AoGlassCountInUnit;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoGlassCountInUnit
		{
			get { return m_AoGlassCountInUnit; }
			set { m_AoGlassCountInUnit = value; }
		}

		private IoAnalogOutput m_AoPutIntoPossibleCount;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoPutIntoPossibleCount
		{
			get { return m_AoPutIntoPossibleCount; }
			set { m_AoPutIntoPossibleCount = value; }
		}

		private IoAnalogOutput m_AoLightAlarmCode;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoLightAlarmCode
		{
			get { return m_AoLightAlarmCode; }
			set { m_AoLightAlarmCode = value; }
		}

		private IoAnalogOutput m_AoHeavyAlarmCode;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoHeavyAlarmCode
		{
			get { return m_AoHeavyAlarmCode; }
			set { m_AoHeavyAlarmCode = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoReportGlassDatas = new IoCollection<IoAnalogOutput>(); //Legnth : 48 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoReportGlassDatas
		{
			get { return m_AoReportGlassDatas; }
			set { m_AoReportGlassDatas = value; }
		}

		//private IoAnalogOutput m_AoDataReportCeid;
		//[Category("DMS : I/O Setting")]
		//public IoAnalogOutput AoDataReportCeid
		//{
		//    get { return m_AoDataReportCeid; }
		//    set { m_AoDataReportCeid = value; }
		//}

		//private IoAnalogOutput m_AoDataReportRecipeNumber;
		//[Category("DMS : I/O Setting")]
		//public IoAnalogOutput AoDataReportRecipeNumber
		//{
		//    get { return m_AoDataReportRecipeNumber; }
		//    set { m_AoDataReportRecipeNumber = value; }
		//}

		//private IoAnalogOutput m_AoDataReportAckData;
		//[Category("DMS : I/O Setting")]
		//public IoAnalogOutput AoDataReportAckData
		//{
		//    get { return m_AoDataReportAckData; }
		//    set { m_AoDataReportAckData = value; }
		//}

		private IoCollection<IoAnalogOutput> m_AoDataReports = new IoCollection<IoAnalogOutput>(); //Length : 48 word	 
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoDataReports
		{
			get { return m_AoDataReports; }
			set { m_AoDataReports = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoGlassDataRequestGlassId = new IoCollection<IoAnalogOutput>(); //Length : 8 word	 
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoGlassDataRequestGlassId
		{
			get { return m_AoGlassDataRequestGlassId; }
			set { m_AoGlassDataRequestGlassId = value; }
		}

		private IoAnalogOutput m_AoGlassDataRequestGlassCode;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoGlassDataRequestGlassCode
		{
			get { return m_AoGlassDataRequestGlassCode; }
			set { m_AoGlassDataRequestGlassCode = value; }
		}

		private IoAnalogOutput m_AoGlassDataRequestOption;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoGlassDataRequestOption
		{
			get { return m_AoGlassDataRequestOption; }
			set { m_AoGlassDataRequestOption = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoPositionGlassCodes = new IoCollection<IoAnalogOutput>(); //Length : 32 word	 
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoPositionGlassCodes
		{
			get { return m_AoPositionGlassCodes; }
			set { m_AoPositionGlassCodes = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoEqpRecipeNumberLists = new IoCollection<IoAnalogOutput>(); //Length : 8 word	 
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoEqpRecipeNumberLists
		{
			get { return m_AoEqpRecipeNumberLists; }
			set { m_AoEqpRecipeNumberLists = value; }
		}

		private IoAnalogOutput m_AoGlassDataReceiveAck;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoGlassDataReceiveAck
		{
			get { return m_AoGlassDataReceiveAck; }
			set { m_AoGlassDataReceiveAck = value; }
		}

		private IoAnalogOutput m_AoGlassMoveStatus;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoGlassMoveStatus
		{
			get { return m_AoGlassMoveStatus; }
			set { m_AoGlassMoveStatus = value; }
		}

		private IoAnalogOutput m_AoLoadRequestGlassCode;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoLoadRequestGlassCode
		{
			get { return m_AoLoadRequestGlassCode; }
			set { m_AoLoadRequestGlassCode = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoUnloadGlassDatas = new IoCollection<IoAnalogOutput>(); //Length : 48 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoUnloadGlassDatas
		{
			get { return m_AoUnloadGlassDatas; }
			set { m_AoUnloadGlassDatas = value; }
		}

		private IoAnalogOutput m_AoLotApdLotNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoLotApdLotNumber
		{
			get { return m_AoLotApdLotNumber; }
			set { m_AoLotApdLotNumber = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoLotApdDataReports = new IoCollection<IoAnalogOutput>(); //Length : 31 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoLotApdDataReports
		{
			get { return m_AoLotApdDataReports; }
			set { m_AoLotApdDataReports = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoGlassApdDataReports = new IoCollection<IoAnalogOutput>(); //Length : 100 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoGlassApdDataReports
		{
			get { return m_AoGlassApdDataReports; }
			set { m_AoGlassApdDataReports = value; }
		}

		private IoAnalogOutput m_AoRecipeBodyReportCeid;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeBodyReportCeid
		{
			get { return m_AoRecipeBodyReportCeid; }
			set { m_AoRecipeBodyReportCeid = value; }
		}

		private IoAnalogOutput m_AoRecipeBodyReportRecipeNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeBodyReportRecipeNumber
		{
			get { return m_AoRecipeBodyReportRecipeNumber; }
			set { m_AoRecipeBodyReportRecipeNumber = value; }
		}

		private IoAnalogOutput m_AoRecipeBodyRequestConfirmAck;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeBodyRequestConfirmAck
		{
			get { return m_AoRecipeBodyRequestConfirmAck; }
			set { m_AoRecipeBodyRequestConfirmAck = value; }
		}

		private IoAnalogOutput m_AoRecipeBodyReportRecipeLevel;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeBodyReportRecipeLevel
		{
			get { return m_AoRecipeBodyReportRecipeLevel; }
			set { m_AoRecipeBodyReportRecipeLevel = value; }
		}

		private IoAnalogOutput m_AoRecipeBodyReportRecipeType;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeBodyReportRecipeType
		{
			get { return m_AoRecipeBodyReportRecipeType; }
			set { m_AoRecipeBodyReportRecipeType = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoRecipeBodyReportRecipeVersions = new IoCollection<IoAnalogOutput>(); //Length : 3 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoRecipeBodyReportRecipeVersions
		{
			get { return m_AoRecipeBodyReportRecipeVersions; }
			set { m_AoRecipeBodyReportRecipeVersions = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoRecipeBodyEqpSpecificDatas = new IoCollection<IoAnalogOutput>(); //Length : 16 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoRecipeBodyEqpSpecificDatas
		{
			get { return m_AoRecipeBodyEqpSpecificDatas; }
			set { m_AoRecipeBodyEqpSpecificDatas = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoRecipeBodyDatas = new IoCollection<IoAnalogOutput>(); //Length : 48 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoRecipeBodyDatas
		{
			get { return m_AoRecipeBodyDatas; }
			set { m_AoRecipeBodyDatas = value; }
		}

		private IoAnalogOutput m_AoRecipeRequestConfirmCeid;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeRequestConfirmCeid
		{
			get { return m_AoRecipeRequestConfirmCeid; }
			set { m_AoRecipeRequestConfirmCeid = value; }
		}

		private IoAnalogOutput m_AoRecipeRequestConfirmAck;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeRequestConfirmAck
		{
			get { return m_AoRecipeRequestConfirmAck; }
			set { m_AoRecipeRequestConfirmAck = value; }
		}

		private IoAnalogOutput m_AoRecipeVariousReportCeid;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeVariousReportCeid
		{
			get { return m_AoRecipeVariousReportCeid; }
			set { m_AoRecipeVariousReportCeid = value; }
		}

		private IoAnalogOutput m_AoRecipeVariousReportCeidOrder;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeVariousReportCeidOrder
		{
			get { return m_AoRecipeVariousReportCeidOrder; }
			set { m_AoRecipeVariousReportCeidOrder = value; }
		}

		private IoAnalogOutput m_AoRecipeTotalCount;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeTotalCount
		{
			get { return m_AoRecipeTotalCount; }
			set { m_AoRecipeTotalCount = value; }
		}

		private IoAnalogOutput m_AoRecipeReportCount;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeReportCount
		{
			get { return m_AoRecipeReportCount; }
			set { m_AoRecipeReportCount = value; }
		}

		private IoAnalogOutput m_AoRecipeVariousCommand;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeVariousCommand
		{
			get { return m_AoRecipeVariousCommand; }
			set { m_AoRecipeVariousCommand = value; }
		}

		private IoAnalogOutput m_AoRecipeVariousRecipeNumber;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeVariousRecipeNumber
		{
			get { return m_AoRecipeVariousRecipeNumber; }
			set { m_AoRecipeVariousRecipeNumber = value; }
		}

		private IoAnalogOutput m_AoRecipeVariousRecipeLevel;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeVariousRecipeLevel
		{
			get { return m_AoRecipeVariousRecipeLevel; }
			set { m_AoRecipeVariousRecipeLevel = value; }
		}

		private IoAnalogOutput m_AoRecipeVariousRecipeType;
		[Category("DMS : I/O Setting")]
		public IoAnalogOutput AoRecipeVariousRecipeType
		{
			get { return m_AoRecipeVariousRecipeType; }
			set { m_AoRecipeVariousRecipeType = value; }
		}
		
		private IoCollection<IoAnalogOutput> m_AoRecipeVariousRecipeVersions = new IoCollection<IoAnalogOutput>(); //Length : 3 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoRecipeVariousRecipeVersions
		{
			get { return m_AoRecipeVariousRecipeVersions; }
			set { m_AoRecipeVariousRecipeVersions = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoRecipeVariousEqpSpecificDatas = new IoCollection<IoAnalogOutput>(); //Length : 16 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoRecipeVariousEqpSpecificDatas
		{
			get { return m_AoRecipeVariousEqpSpecificDatas; }
			set { m_AoRecipeVariousEqpSpecificDatas = value; }
		}

		private IoCollection<IoAnalogOutput> m_AoRecipePrameters = new IoCollection<IoAnalogOutput>(); //Length : 48 word
		[Category("DMS : I/O Setting")]
		public IoCollection<IoAnalogOutput> AoRecipePrameters
		{
			get { return m_AoRecipePrameters; }
			set { m_AoRecipePrameters = value; }
		}

		#endregion

		#region Properties
		#endregion

		#region Constructor
		public IfSignalToCim()
		{
			this.Name = "__";
		}
		#endregion

		#region Methods
		#endregion

		#region Override
		#endregion
	}
}

