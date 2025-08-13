using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Threading;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class EGiSInterface : _DeviceAsm
    {
        #region Tag Descriptor
        public static TagDesciptorCrackStatus tagDescriptor = new TagDesciptorCrackStatus();
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
        private IoDigitalInput m_InputSignal10 = new IoDigitalInput(); // 12.06.13 wang
        private IoDigitalInput m_InputSignal11 = new IoDigitalInput(); // 12.06.13 wang

        private IoDigitalOutput m_OutputSignal1 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal2 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal3 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal4 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal5 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal6 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal7 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal8 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal9 = new IoDigitalOutput(); // 12.06.13 wang
        private IoDigitalOutput m_OutputSignal10 = new IoDigitalOutput(); // 12.06.13 wang
        
        private IoAnalogOutput m_WOutPutSignal1 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal2 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal3 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal4 = new IoAnalogOutput();
       
        public Alarm ALM_Crack_NG;
        public Alarm ALM_Ready;

        private TagSetupInfo m_SetupCrackTRUse; // 11.03.02 minhan
        private TagSetupInfo m_SetupCrackLoaderUse; // 11.03.02 minhan
        private TagSetupSenSorInterlock m_SetupCrackInDetect = null;// 11.07.28 sungyong
        private TagSetupSenSorInterlock m_SetupCrackOutDetect = null;// 11.07.28 sungyong

        private string m_CrackStatus="Unknown";
        private System.Threading.Timer m_ThreadingTimer = null;
        #endregion
         #region Properties
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibReady
        {
            get { return m_InputSignal1; }
            set { m_InputSignal1 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibDown
        {
            get { return m_InputSignal2; }
            set { m_InputSignal2 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibResult_OK_IN
        {
            get { return m_InputSignal3; }
            set { m_InputSignal3 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibResult_OK_OUT
        {
            get { return m_InputSignal4; }
            set { m_InputSignal4 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibResult_NG_IN
        {
            get { return m_InputSignal5; }
            set { m_InputSignal5 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibResult_NG_OUT
        {
            get { return m_InputSignal6; }
            set { m_InputSignal6 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibPass_IN
        {
            get { return m_InputSignal7; }
            set { m_InputSignal7 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibPass_OUT
        {
            get { return m_InputSignal8; }
            set { m_InputSignal8 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibHeartBeat
        {
            get { return m_InputSignal9; }
            set { m_InputSignal9 = value; }
        }
        [Category("DMS : Bit Input")] // 12.06.13 wang
        public IoDigitalInput mibResult_Warning_IN 
        {
            get { return m_InputSignal10; }
            set { m_InputSignal10 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibResult_Warning_OUT // 12.06.13 wang
        {
            get { return m_InputSignal11; }
            set { m_InputSignal11 = value; }
        }
        
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobStart_IN
        {
            get { return m_OutputSignal1; }
            set { m_OutputSignal1 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobStart_OUT
        {
            get { return m_OutputSignal2; }
            set { m_OutputSignal2 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobOK_Accept_IN
        {
            get { return m_OutputSignal3; }
            set { m_OutputSignal3 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobOK_Accept_OUT
        {
            get { return m_OutputSignal4; }
            set { m_OutputSignal4 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobNG_Accept_IN
        {
            get { return m_OutputSignal5; }
            set { m_OutputSignal5 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobNG_Accept_OUT
        {
            get { return m_OutputSignal6; }
            set { m_OutputSignal6 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobVelocity_Check_IN
        {
            get { return m_OutputSignal7; }
            set { m_OutputSignal7 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobVelocity_Check_OUT
        {
            get { return m_OutputSignal8; }
            set { m_OutputSignal8 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobWarning_Accept_IN // 12.06.13 wang
        {
            get { return m_OutputSignal9; }
            set { m_OutputSignal9 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobWarning_Accept_OUT // 12.06.13 wang
        {
            get { return m_OutputSignal10; }
            set { m_OutputSignal10 = value; }
        }
        
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowGlass_ID_IN
        {
            get { return m_WOutPutSignal1; }
            set { m_WOutPutSignal1 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowGlass_ID_OUT
        {
            get { return m_WOutPutSignal2; }
            set { m_WOutPutSignal2 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowVelocity_IN
        {
            get { return m_WOutPutSignal3; }
            set { m_WOutPutSignal3 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowVelocity_OUT
        {
            get { return m_WOutPutSignal4; }
            set { m_WOutPutSignal4 = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupCrackTRUse // 11.03.02 minhan
        {
            get { return m_SetupCrackTRUse; }
            set { m_SetupCrackTRUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupCrackLoaderUse // 11.03.02 minhan
        {
            get { return m_SetupCrackLoaderUse; }
            set { m_SetupCrackLoaderUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupCrackInDetect // 11.07.28 sungyong
        {
            get { return m_SetupCrackInDetect; }
            set { m_SetupCrackInDetect = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupCrackOutDetect // 11.07.28 sungyong
        {
            get { return m_SetupCrackOutDetect; }
            set { m_SetupCrackOutDetect = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool IsTRUse // 11.03.02 minhan
        {
            get
            {
                if (m_SetupCrackTRUse == null) return false;
                return m_SetupCrackTRUse.GetValue<bool>();
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsLoaderUse // 11.03.02 minhan
        {
            get
            {
                if (m_SetupCrackLoaderUse == null) return false;
                return m_SetupCrackLoaderUse.GetValue<bool>();
            }
        }
        #endregion

        #region Constructor
        public EGiSInterface()
        {
            this.Name = "_EGiSInterface";
        }
        #endregion
        #region Methods
        public bool isReady()
        {
            bool m_isReady = false;
            m_isReady = mibReady.GetState();
            return m_isReady;
        }
        public bool isCheck()
        {
            bool m_isCheck = false;
            m_isCheck |= mobStart_IN.GetState();
            m_isCheck |= mobStart_OUT.GetState();
            return m_isCheck;
        }
        public bool isOkStatus()
        {
            bool m_isOkStatus = false;
            m_isOkStatus |= mibResult_OK_IN.GetState();
            m_isOkStatus |= mibResult_OK_OUT.GetState();
            return m_isOkStatus;
        }
        public bool isNGStatus()
        {
            bool m_isNGStatus = false;
            m_isNGStatus |= mibResult_NG_IN.GetState();
            m_isNGStatus |= mibResult_NG_OUT.GetState();
            return m_isNGStatus;
        }
        public bool isWarningStatus() // 12.06.13 wang
        {
            bool m_isWarningStatus = false;
            m_isWarningStatus |= mibResult_Warning_IN.GetState();
            m_isWarningStatus |= mibResult_Warning_OUT.GetState();
            return m_isWarningStatus;
        }
        public bool isSetupUse() // 11.03.02 minhan
        {
            bool m_isSetupUse = false;
            m_isSetupUse |= SetupCrackTRUse.GetValue<bool>();
            m_isSetupUse |= SetupCrackLoaderUse.GetValue<bool>();
            return m_isSetupUse;
        }
        public void SeqCheckStatus(Object stateInfo) // 11.03.02 minhan
        {
            if (!isSetupUse()) m_CrackStatus = "NoUse";
            else
            {
                
                if (isCheck()) m_CrackStatus = "Checking";
                else if (isOkStatus()) m_CrackStatus = "OK";
                else if (isNGStatus()) m_CrackStatus = "NG";
                else if (isWarningStatus()) m_CrackStatus = "Warning"; // 12.06.13 wang
                else if (isReady()) m_CrackStatus = "Ready";
                else m_CrackStatus = "UnKnown";

                //if(!isReady() && !isCheck() &&!isOkStatus() && !isNGStatus())
                //{
                //    m_CrackStatus = "UnKnown";
                //}
            }
            UpdateTag();
        }
        #endregion
        #region Override
        public override Type FamilyType
        {
            get { return typeof(EGiSInterface); }
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
                
                ALM_Crack_NG = new Alarm(this.Name + " Crack NG Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Ready = new Alarm(this.Name + " Ready Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion



                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                #endregion
                m_SetupCrackTRUse = new TagSetupInfo("TR Crack Machine Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                SetupGenInfoProvider.Instance.InitFromDB(m_SetupCrackTRUse); // 11.03.02 minhan
                m_SetupCrackLoaderUse = new TagSetupInfo("Loader Crack Machine Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                SetupGenInfoProvider.Instance.InitFromDB(m_SetupCrackLoaderUse); // 11.03.02 minhan
                m_SetupCrackInDetect = new TagSetupSenSorInterlock(this.Name + "Crack Detect (EQP Inside)", false, SensorInterlockType.Broken); // 11.07.28 sungyong
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupCrackInDetect);
                m_SetupCrackOutDetect = new TagSetupSenSorInterlock(this.Name + "Crack Detect (EQP Outside)", false, SensorInterlockType.Broken); // 11.07.28 sungyong
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupCrackOutDetect);
              


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();
                if (m_ThreadingTimer == null)
                {
                    m_ThreadingTimer = new System.Threading.Timer(new TimerCallback(SeqCheckStatus), null, 0, 100);
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                if (m_Simul.Device)
                {
                    mibReady.SetState(true);
                }
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
            //m_Tag.SetValue(tagDescriptor.INPUTSIGNAL1, diLdNormalStatus.GetState());
            m_Tag.SetValue(tagDescriptor.CrackStatus, m_CrackStatus);
        }
        public override void SetSubscriber()
        {
            // Tag 초기값 설정 및 UpdataeTag 설정
            m_Tag.SetValue(tagDescriptor.CrackStatus, m_CrackStatus);
        }
        #endregion
    }
}