///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.28
// Author       : jemoon
// Description  : AreaSensor Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AreaSensor : _InterlockSensor
    {
        #region Tag Descriptor
        protected static TagInterlockCondition m_InterlockCondition = new TagInterlockCondition();
        #endregion

        #region Fields
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public static TagInterlockCondition InterlockCondition
        {
            get { return m_InterlockCondition; }
            set { m_InterlockCondition = value; }
        }
        #endregion

        #region Constructor
        protected AreaSensor() { }
        #endregion

        #region Methods
        public void SetInterlockCondtion(bool set)
        {
            if (m_UseHeavyInterlock)
            {
                if (set)
                {
                    m_InterlockCondition.SetAlarm(m_Id);
                }
                else
                {
                    m_InterlockCondition.ResetAlarm(m_Id);
                }
            }
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(AreaSensor); }
        }

        public override DmsErrors SubInitialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AreaSensor_Io : AreaSensor
    {
        #region Fields
        protected IoDigitalInput m_DiSensor = new IoDigitalInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiSensor
        {
            get { return m_DiSensor; }
            set { m_DiSensor = value; }
        }
        #endregion

        #region Constructor
        public AreaSensor_Io()
        {
            this.Name = "__ Unit Area Sensor";
            m_InterlockType = SensorInterlockType.Area;
        }
        #endregion

        #region Methods
        public override void SetState(bool state)
        {
            m_DiSensor.SetState(state);
        }

        public override bool IsDetected()
        {
            if (!this.Initialized) return false;

            return m_DiSensor.GetState();
        }
        #endregion

        #region Override
        public override DmsErrors SubInitialize()
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
            ok &= (m_DiSensor != null);


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
                if (m_Simul.Device)
                {
                    m_DiSensor.SetState(false);
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
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AreaSensor_Ec : AreaSensor
    {
        #region Fields
        protected SlaveDigitalInput m_DiSensor = new SlaveDigitalInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiSensor
        {
            get { return m_DiSensor; }
            set { m_DiSensor = value; }
        }
        #endregion

        #region Constructor
        public AreaSensor_Ec()
        {
            this.Name = "__ Unit Area Sensor";
            m_InterlockType = SensorInterlockType.Area;
        }
        #endregion

        #region Methods
        public override void SetState(bool state)
        {
            m_DiSensor.SetState(state);
        }

        public override bool IsDetected()
        {
            if (!this.Initialized) return false;

            return m_DiSensor.GetState();
        }
        #endregion

        #region Override
        public override DmsErrors SubInitialize()
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
            ok &= (m_DiSensor != null);


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
                if (m_Simul.Device)
                {
                    m_DiSensor.SetState(false);
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
        #endregion
    }
}
