using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class ProcessUnit : _DeviceAsm
    {
        #region Fields
        private CvUnit m_OwnerUnit = null;
        private AutoValve m_AutoValve = null;
		private ProcessCondition m_RefProcessCondition = ProcessCondition.Stop;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public AutoValve AutoValve
        {
            get { return m_AutoValve; }
            set { m_AutoValve = value; }
        }
        [Category("DMS : Setting")]
        public CvUnit OwnerUnit
        {
            get { return m_OwnerUnit; }
            set { m_OwnerUnit = value; }
        }
		[XmlIgnore(), Browsable(false)]
		public ProcessCondition RefProcessCondition
		{
			get { return m_RefProcessCondition; }
			set { m_RefProcessCondition = value; }
		}
        #endregion

        #region Constructor
        public ProcessUnit()
        {
            this.Name = "__ Valve Unit";
        }
        #endregion

        #region Methods
        public void Run()
        {
            AutoValve.Open();
        }
        public void Stop()
        {
            AutoValve.Close();
        }
        public bool IsRun()
        {
            return AutoValve.IsOpen();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ProcessUnit); }
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

        public override void CreateTag(DeviceTags tagContainer)
        {
            
        }

        public override void UpdateTag()
        {
            
        }
        #endregion
    }
}
