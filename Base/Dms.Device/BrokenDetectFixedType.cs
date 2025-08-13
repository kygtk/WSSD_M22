///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.13
// Author       : eun
// Description  : Glass Broken Detect Fixed Type(4 Sensors) Class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BrokenDetectFixedType : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorBrokenFixed tagDescriptor = new TagDescriptorBrokenFixed();
        #endregion

        #region Fields
        protected bool m_IsAlarm = false;
        public Alarm ALM_BrokenDetectFrontSensor1 = null;
        public Alarm ALM_BrokenDetectFrontSensor2 = null;
        public Alarm ALM_BrokenDetectRearSensor1 = null;
        public Alarm ALM_BrokenDetectRearSensor2 = null;
        protected TagSetupSenSorInterlock m_SetupBrokenIntrFront1 = null;
        protected TagSetupSenSorInterlock m_SetupBrokenIntrFront2 = null;
        protected TagSetupSenSorInterlock m_SetupBrokenIntrRear1 = null;
        protected TagSetupSenSorInterlock m_SetupBrokenIntrRear2 = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupBrokenInterlockFront1
        {
            get { return m_SetupBrokenIntrFront1; }
            set { m_SetupBrokenIntrFront1 = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupBrokenInterlockFront2
        {
            get { return m_SetupBrokenIntrFront2; }
            set { m_SetupBrokenIntrFront2 = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupBrokenInterlockRear1
        {
            get { return m_SetupBrokenIntrRear1; }
            set { m_SetupBrokenIntrRear1 = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupBrokenInterlockRear2
        {
            get { return m_SetupBrokenIntrRear2; }
            set { m_SetupBrokenIntrRear2 = value; }
        }
        #endregion

        #region Constructor
        protected BrokenDetectFixedType() { }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(BrokenDetectFixedType); }
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
            throw new NotImplementedException();
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BrokenDetectFixedType_Io : BrokenDetectFixedType
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalInput m_DiFrontSensor1 = new IoDigitalInput();
        private IoDigitalInput m_DiFrontSensor2 = new IoDigitalInput();
        private IoDigitalInput m_DiRearSensor1 = new IoDigitalInput();
        private IoDigitalInput m_DiRearSensor2 = new IoDigitalInput();
        #endregion

        #region Properties
        [Category("Setting")]
        public IoDigitalInput BrokenSensorFront1
        {
            get { return m_DiFrontSensor1; }
            set { m_DiFrontSensor1 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput BrokenSensorFront2
        {
            get { return m_DiFrontSensor2; }
            set { m_DiFrontSensor2 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput BrokenSensorRear1
        {
            get { return m_DiRearSensor1; }
            set { m_DiRearSensor1 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput BrokenSensorRear2
        {
            get { return m_DiRearSensor2; }
            set { m_DiRearSensor2 = value; }
        }
        #endregion

        #region Constructor
        public BrokenDetectFixedType_Io()
        {
            this.Name = "__ Unit Broken Detect FixedType";
        }
        #endregion

        #region Methods
        #endregion

        #region Override

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
            ok &= (m_DiFrontSensor1 != null);
            ok &= (m_DiFrontSensor2 != null);
            ok &= (m_DiRearSensor1 != null);
            ok &= (m_DiRearSensor2 != null);


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
                ALM_BrokenDetectFrontSensor1 = new Alarm(this.Name + " Front1" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BrokenDetectFrontSensor2 = new Alarm(this.Name + " Front2" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BrokenDetectRearSensor1 = new Alarm(this.Name + " Rear1" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BrokenDetectRearSensor2 = new Alarm(this.Name + " Rear2" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupSensorInterlockProvider setupSensorIntrProvider = SetupSensorInterlockProvider.Instance;
                m_SetupBrokenIntrFront1 = new TagSetupSenSorInterlock(this.Name + " Front1", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrFront1);
                m_SetupBrokenIntrFront2 = new TagSetupSenSorInterlock(this.Name + " Front2", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrFront2);
                m_SetupBrokenIntrRear1 = new TagSetupSenSorInterlock(this.Name + " Rear1", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrRear1);
                m_SetupBrokenIntrRear2 = new TagSetupSenSorInterlock(this.Name + " Rear2", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrRear2);


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.FRONT_1_DETECT, BrokenSensorFront1.GetState());
            m_Tag.SetValue(tagDescriptor.FRONT_2_DETECT, BrokenSensorFront2.GetState());
            m_Tag.SetValue(tagDescriptor.REAR_1_DETECT, BrokenSensorRear1.GetState());
            m_Tag.SetValue(tagDescriptor.REAR_2_DETECT, BrokenSensorRear2.GetState());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BrokenDetectFixedType_Ec : BrokenDetectFixedType
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalInput m_DiFrontSensor1 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiFrontSensor2 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiRearSensor1 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiRearSensor2 = new SlaveDigitalInput();
        #endregion

        #region Properties
        [Category("Setting")]
        public SlaveDigitalInput BrokenSensorFront1
        {
            get { return m_DiFrontSensor1; }
            set { m_DiFrontSensor1 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput BrokenSensorFront2
        {
            get { return m_DiFrontSensor2; }
            set { m_DiFrontSensor2 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput BrokenSensorRear1
        {
            get { return m_DiRearSensor1; }
            set { m_DiRearSensor1 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput BrokenSensorRear2
        {
            get { return m_DiRearSensor2; }
            set { m_DiRearSensor2 = value; }
        }
        #endregion

        #region Constructor
        public BrokenDetectFixedType_Ec()
        {
            this.Name = "__ Unit Broken Detect FixedType";
        }
        #endregion

        #region Methods
        #endregion

        #region Override

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
            ok &= (m_DiFrontSensor1 != null);
            ok &= (m_DiFrontSensor2 != null);
            ok &= (m_DiRearSensor1 != null);
            ok &= (m_DiRearSensor2 != null);


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
                ALM_BrokenDetectFrontSensor1 = new Alarm(this.Name + " Front1" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BrokenDetectFrontSensor2 = new Alarm(this.Name + " Front2" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BrokenDetectRearSensor1 = new Alarm(this.Name + " Rear1" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BrokenDetectRearSensor2 = new Alarm(this.Name + " Rear2" + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupSensorInterlockProvider setupSensorIntrProvider = SetupSensorInterlockProvider.Instance;
                m_SetupBrokenIntrFront1 = new TagSetupSenSorInterlock(this.Name + " Front1", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrFront1);
                m_SetupBrokenIntrFront2 = new TagSetupSenSorInterlock(this.Name + " Front2", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrFront2);
                m_SetupBrokenIntrRear1 = new TagSetupSenSorInterlock(this.Name + " Rear1", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrRear1);
                m_SetupBrokenIntrRear2 = new TagSetupSenSorInterlock(this.Name + " Rear2", false, SensorInterlockType.Broken);
                setupSensorIntrProvider.InitFromDB(this.m_SetupBrokenIntrRear2);


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.FRONT_1_DETECT, BrokenSensorFront1.GetState());
            m_Tag.SetValue(tagDescriptor.FRONT_2_DETECT, BrokenSensorFront2.GetState());
            m_Tag.SetValue(tagDescriptor.REAR_1_DETECT, BrokenSensorRear1.GetState());
            m_Tag.SetValue(tagDescriptor.REAR_2_DETECT, BrokenSensorRear2.GetState());
        }
        #endregion
    }
}
