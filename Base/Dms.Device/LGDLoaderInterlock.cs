using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class LGDLoaderInterlock : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorLoaderInterlock tagDescriptor = new TagDescriptorLoaderInterlock();
        #endregion

        #region Fields
        private IoDigitalInput m_InputSignal1 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal2 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal3 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal4 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal5 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal6 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal7 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal8 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal9 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal10 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal11 = new IoDigitalInput();
        private IoDigitalOutput m_OutputSignal1 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal2 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal3 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal4 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal5 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal6 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal7 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal8 = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput diLdNormalStatus
        {
            get { return m_InputSignal1; }
            set { m_InputSignal1 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diUnloadReady
        {
            get { return m_InputSignal2; }
            set { m_InputSignal2 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diLoadReady
        {
            get { return m_InputSignal3; }
            set { m_InputSignal3 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diExchangeReady
        {
            get { return m_InputSignal4; }
            set { m_InputSignal4 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diRobotBusy
        {
            get { return m_InputSignal5; }
            set { m_InputSignal5 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diRobotBusy2
        {
            get { return m_InputSignal6; }
            set { m_InputSignal6 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diRobotEnd
        {
            get { return m_InputSignal7; }
            set { m_InputSignal7 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diRobotEnd2
        {
            get { return m_InputSignal8; }
            set { m_InputSignal8 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diNextGlassExist
        {
            get { return m_InputSignal9; }
            set { m_InputSignal9 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diUnloadComplete
        {
            get { return m_InputSignal10; }
            set { m_InputSignal10 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput diLoadComplete
        {
            get { return m_InputSignal11; }
            set { m_InputSignal11 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doEqpNormalStatus
        {
            get { return m_OutputSignal1; }
            set { m_OutputSignal1 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doUnloadEnable
        {
            get { return m_OutputSignal2; }
            set { m_OutputSignal2 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doLoadEnable
        {
            get { return m_OutputSignal3; }
            set { m_OutputSignal3 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doExchangeEnable
        {
            get { return m_OutputSignal4; }
            set { m_OutputSignal4 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doPinActionBusy
        {
            get { return m_OutputSignal5; }
            set { m_OutputSignal5 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doUnloadConfirm
        {
            get { return m_OutputSignal6; }
            set { m_OutputSignal6 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doRobotActionEnable
        {
            get { return m_OutputSignal7; }
            set { m_OutputSignal7 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput doRobotActionEnable2
        {
            get { return m_OutputSignal8; }
            set { m_OutputSignal8 = value; }
        }
        #endregion
 
        #region Constructor
        public LGDLoaderInterlock()
        {
            this.Name = "__ LoaderInterlock";
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
			#endregion
			//ok &= (m_DiAlarm != null);
			//ok &= (m_DiCpOn != null);


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
				//ALM_FfuAlarm = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
				//ALM_CpOff = new Alarm(this.Name + " C/P Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


				////////////////////////////////////////////////////////////////////////////////////////
				// 6. SetupItem 생성
				#region Example
				//m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
				//SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
				#endregion
				//m_SetupFfuIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.FFU);
				//SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupFfuIntr);


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
				//if (m_Simul.Device)
				//{
				//    m_DiCpOn.SetState(true);
				//}


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
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL1, diLdNormalStatus.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL2, diUnloadReady.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL3, diLoadReady.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL4, diExchangeReady.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL5, diRobotBusy.GetState());
			if(diRobotBusy2 != null) m_Tag.SetValue(tagDescriptor.INPUTSIGNAL6, diRobotBusy2.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL7, diRobotEnd.GetState());
			if (diRobotEnd2 != null) m_Tag.SetValue(tagDescriptor.INPUTSIGNAL8, diRobotEnd2.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL9, diNextGlassExist.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL10, diUnloadComplete.GetState());
			m_Tag.SetValue(tagDescriptor.INPUTSIGNAL11, diLoadComplete.GetState());

			m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL1, doEqpNormalStatus.GetState());
			m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL2, doUnloadEnable.GetState());
			m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL3, doLoadEnable.GetState());
			m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL4, doExchangeEnable.GetState());
			m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL5, doPinActionBusy.GetState());
			if (doUnloadConfirm != null) m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL6, doUnloadConfirm.GetState());
			if (doRobotActionEnable != null) m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL7, doRobotActionEnable.GetState());
			if (doRobotActionEnable2 != null) m_Tag.SetValue(tagDescriptor.OUTPUTSIGNAL8, doRobotActionEnable2.GetState());
		} 
		#endregion
    }
}
