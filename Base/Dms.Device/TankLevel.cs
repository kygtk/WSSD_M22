using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class TankLevel : _DeviceAsm, ITankLevel
    {
        #region Fields
        public delegate bool IsConfirmed();
        protected IsConfirmed IsTopLevel;
        protected IsConfirmed IsSupplyStop;
        protected IsConfirmed IsSupplyRequest;
        protected IsConfirmed IsRunEnable;
        protected IsConfirmed IsBottomLevel;

        public delegate bool IsDetected();
        protected IsDetected IsTopLevelDetect;
        protected IsDetected IsSupplyStopDetect;
        protected IsDetected IsSupplyRequestDetect;
        protected IsDetected IsRunEnableDetect;
        protected IsDetected IsBottomLevelDetect;

        protected List<TagSetupInfo> m_SetupTankLevels = new List<TagSetupInfo>();
        protected TagSetupInfo m_SetupTankLevel = null;
        protected _GenericCollection<LevelSensor> m_Sensors = new _GenericCollection<LevelSensor>();
        #endregion

        #region Properties
        [Description("Index of bottom level sensor is 0")]
        public _GenericCollection<LevelSensor> Sensors
        {
            get { return m_Sensors; }
            set { m_Sensors = value; }
        }
        public virtual int Count
        {
            get { return m_Sensors.Count; }
        }
        [Browsable(false), XmlIgnore()]
        public List<TagSetupInfo> SetupTankLevels
        {
            get { return m_SetupTankLevels; }
            set { m_SetupTankLevels = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupTankLevel
        {
            get { return m_SetupTankLevel; }
            set { m_SetupTankLevel = value; }
        }
        #endregion

        #region Constructor
        public TankLevel()
        {
            this.Name = "__ Tank Level";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Server 내부로 전달
        /// </summary>
        public void FireEvent()
        {
            IoStateChangeEventHandler eHandle = this.OnLevelSatateChanged;
            if (eHandle != null)
            {
                OnLevelSatateChanged(this, new IoStateEventArgs());
            }
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(TankLevel); }
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
            ok &= (m_Sensors.Count >= 4);


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
                SetupTankLevelProvider setupTankLevelProvider = SetupTankLevelProvider.Instance;
                m_SetupTankLevel = new TagSetupInfo(this.Name + " LL", OptionType.None, OptionFormat.Digit, UnitType.L, "30");
                setupTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " L", OptionType.None, OptionFormat.Digit, UnitType.L, "50");
                setupTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " M", OptionType.None, OptionFormat.Digit, UnitType.L, "100");
                setupTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " H", OptionType.None, OptionFormat.Digit, UnitType.L, "130");
                setupTankLevelProvider.InitFromDB(m_SetupTankLevel);
                if (m_Sensors.Count == 5)
                {
                    m_SetupTankLevel = new TagSetupInfo(this.Name + " HH", OptionType.None, OptionFormat.Digit, UnitType.L, "150");
                    setupTankLevelProvider.InitFromDB(m_SetupTankLevel);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                IsSupplyStop = new IsConfirmed(m_Sensors[3].IsConfirmed);
                IsSupplyRequest = new IsConfirmed(m_Sensors[2].IsConfirmed);
                IsRunEnable = new IsConfirmed(m_Sensors[1].IsConfirmed);
                IsBottomLevel = new IsConfirmed(m_Sensors[0].IsConfirmed);
                IsSupplyStopDetect = new IsDetected(m_Sensors[3].IsDetected);
                IsSupplyRequestDetect = new IsDetected(m_Sensors[2].IsDetected);
                IsRunEnableDetect = new IsDetected(m_Sensors[1].IsDetected);
                IsBottomLevelDetect = new IsDetected(m_Sensors[0].IsDetected);
                if (m_Sensors.Count == 5)
                {
                    IsTopLevel = new IsConfirmed(m_Sensors[4].IsConfirmed);
                    IsTopLevelDetect = new IsDetected(m_Sensors[4].IsDetected);
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

        public override void HandleEvent(object sender, IoStateEventArgs e)
        {
            FireEvent();
        }

        public override void SetSubscriber()
        {
            base.SetSubscriber();

            foreach (LevelSensor sensor in Sensors)
            {
                sensor.OnSensorStateChanged += new IoStateChangeEventHandler(HandleEvent);
            }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {

        }

        public override void UpdateTag()
        {

        }
        #endregion

        #region ITankLevel Member
        [Browsable(false), XmlIgnore()]
        public LevelConfirm TopLevelConfirm
        {
            get
            {
                if (IsTopLevel == null)
                {
                    return LevelConfirm.NotUsed;
                }
                else
                {
                    return IsTopLevel() == true ? LevelConfirm.Confirm : LevelConfirm.NotConfirm;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelConfirm SupplyRequestConfirm
        {
            get
            {
                if (IsSupplyRequest == null)
                {
                    return LevelConfirm.NotUsed;
                }
                else
                {
                    return IsSupplyRequest() == true ? LevelConfirm.Confirm : LevelConfirm.NotConfirm;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelConfirm SupplyStopConfirm
        {
            get
            {
                if (IsSupplyStop == null)
                {
                    return LevelConfirm.NotUsed;
                }
                else
                {
                    return IsSupplyStop() == true ? LevelConfirm.Confirm : LevelConfirm.NotConfirm;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelConfirm RunEnableConfirm
        {
            get
            {
                if (IsRunEnable == null)
                {
                    return LevelConfirm.NotUsed;
                }
                else
                {
                    return IsRunEnable() == true ? LevelConfirm.Confirm : LevelConfirm.NotConfirm;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelConfirm BottomLevelConfirm
        {
            get
            {
                if (IsBottomLevel == null)
                {
                    return LevelConfirm.NotUsed;
                }
                else
                {
                    return IsBottomLevel() == true ? LevelConfirm.Confirm : LevelConfirm.NotConfirm;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelDetect TopLevelDetect
        {
            get 
            {
                if (IsTopLevelDetect == null)
                {
                    return LevelDetect.NotUsed;
                }
                else
                {
                    return IsTopLevelDetect() == true ? LevelDetect.Detect : LevelDetect.NotDetect;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelDetect SupplyRequestDetect
        {
            get
            {
                if (IsSupplyRequestDetect == null)
                {
                    return LevelDetect.NotUsed;
                }
                else
                {
                    return IsSupplyRequestDetect() == true ? LevelDetect.Detect : LevelDetect.NotDetect;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelDetect SupplyStopDetect
        {
            get
            {
                if (IsSupplyStopDetect == null)
                {
                    return LevelDetect.NotUsed;
                }
                else
                {
                    return IsSupplyStopDetect() == true ? LevelDetect.Detect : LevelDetect.NotDetect;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelDetect RunEnableDetect
        {
            get
            {
                if (IsRunEnableDetect == null)
                {
                    return LevelDetect.NotUsed;
                }
                else
                {
                    return IsRunEnableDetect() == true ? LevelDetect.Detect : LevelDetect.NotDetect;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public LevelDetect BottomLevelDetect
        {
            get
            {
                if (IsBottomLevelDetect == null)
                {
                    return LevelDetect.NotUsed;
                }
                else
                {
                    return IsBottomLevelDetect() == true ? LevelDetect.Detect : LevelDetect.NotDetect;
                }
            }
        }

        public event IoStateChangeEventHandler OnLevelSatateChanged;
        #endregion
    }
}
