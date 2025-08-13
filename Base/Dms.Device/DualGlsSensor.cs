///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05.12
// Author       : eun
// Description  : DualGlsSensor
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DualGlsSensor : GlsSensor
    {
        #region Tag Descriptor
        protected static new TagDescriptorDualGlsSensor tagDescriptor = new TagDescriptorDualGlsSensor();
        #endregion

        #region Fields
        protected bool m_UseOop = true;
        protected bool m_UseOp = true;
        protected bool m_SickOp;

        protected Logic m_DetectLogic = Logic.AND;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public bool UseOop
        {
            get { return m_UseOop; }
            set
            {
                m_UseOop = value;
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool UseOp
        {
            get { return m_UseOp; }
            set
            {
                m_UseOp = value;
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool SickOp
        {
            get { return m_SickOp; }
            set { m_SickOp = value; }
        }

        [Category("DMS : Dual Glass Sensor Option")]
        public Logic DetectLogic { get { return m_DetectLogic; } set { m_DetectLogic = value; } }
        #endregion

        #region Constructor
        protected DualGlsSensor() { }
        #endregion

        #region Methods
        public virtual void SetUseNoUse(DualGlsSensorType type, bool value)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsOpDetected()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsOopDetected()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(GlsSensor); }
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DualGlsSensor_Io : DualGlsSensor
    {
        #region Fields
        protected IoDigitalInput m_DiSensor = new IoDigitalInput();
        private IoDigitalInput m_DiSensorOp = new IoDigitalInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiSensor
        {
            get { return m_DiSensor; }
            set { m_DiSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiSensorOp
        {
            get { return m_DiSensorOp; }
            set { m_DiSensorOp = value; }
        }
        #endregion

        #region Constructor
        public DualGlsSensor_Io()
        {
            this.Name = "__ Unit Dual Gls __ Sensor";
        }
        #endregion

        #region Methods
        public override void SetUseNoUse(DualGlsSensorType type, bool value)
        {
            if (type == DualGlsSensorType.Op)
            {
                m_UseOp = value;
            }
            else
            {
                m_UseOop = value;
            }

            UpdateTag();
        }

        public override bool IsOpDetected()
        {
            if (m_UseOp) return m_DiSensorOp.GetState();
            return false;
        }

        public override bool IsOopDetected()
        {
            if (m_UseOop) return m_DiSensor.GetState();
            return false;
        }
        #endregion

        #region Override
        public override bool IsDetected()   //  20250807 bm : Dual Glass Sensor Option에 Logic 추가하여 해당 로직을 기본으로 사용하도록 변경
        {
            return IsDetected(m_DetectLogic);
        }

        public override bool IsDetected(Logic logic)
        {
            if (!this.Initialized) return false;

            //Use/NoUse에 따라 세부사항 작성
            if (logic == Logic.OR)
            {
                if (m_UseOop == false)
                {
                    return m_DiSensorOp.GetState();
                }
                else if (m_UseOp == false)
                {
                    return m_DiSensor.GetState();
                }
                else
                {
                    return (m_DiSensor.GetState() || m_DiSensorOp.GetState());
                }
            }
            else
            {
                if (m_UseOop == false) return m_DiSensorOp.GetState();
                else if (m_UseOp == false) return m_DiSensor.GetState();
                else return (m_DiSensor.GetState() && m_DiSensorOp.GetState());
            }
        }

        public override void SetState(bool state, Logic logic)
        {
            m_DiSensorOp.SetState(state);
            if (logic == Logic.AND)
            {
                m_DiSensor.SetState(state);
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
            m_Tag.SetValue(tagDescriptor.DETECT, m_DiSensor.GetState());
            m_Tag.SetValue(tagDescriptor.DETECTOP, m_DiSensorOp.GetState());
            m_Tag.SetValue(tagDescriptor.SICK, Sick);
            m_Tag.SetValue(tagDescriptor.SICKOP, m_SickOp);
            m_Tag.SetValue(tagDescriptor.USE, m_UseOop);
            m_Tag.SetValue(tagDescriptor.USEOP, m_UseOp);
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
            ok &= (m_DiSensor != null);
            ok &= (m_DiSensorOp != null);


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
                    m_DiSensorOp.SetState(false);
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
    public class DualGlsSensor_Ec : DualGlsSensor
    {
        #region Fields
        protected SlaveDigitalInput m_DiSensor = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiSensorOp = new SlaveDigitalInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiSensor
        {
            get { return m_DiSensor; }
            set { m_DiSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiSensorOp
        {
            get { return m_DiSensorOp; }
            set { m_DiSensorOp = value; }
        }
        #endregion

        #region Constructor
        public DualGlsSensor_Ec()
        {
            this.Name = "__ Unit Dual Gls __ Sensor";
        }
        #endregion

        #region Methods
        public override void SetUseNoUse(DualGlsSensorType type, bool value)
        {
            if (type == DualGlsSensorType.Op)
            {
                m_UseOp = value;
            }
            else
            {
                m_UseOop = value;
            }

            UpdateTag();
        }

        public override bool IsOpDetected()
        {
            if (m_UseOp) return m_DiSensorOp.GetState();
            return false;
        }

        public override bool IsOopDetected()
        {
            if (m_UseOop) return m_DiSensor.GetState();
            return false;
        }
        #endregion

        #region Override
        public override bool IsDetected()       //  20250807 bm : Dual Glass Sensor Option에 Logic 추가하여 해당 로직을 기본으로 사용하도록 변경
        {
            return IsDetected(m_DetectLogic);
        }

        public override bool IsDetected(Logic logic)
        {
            if (!this.Initialized) return false;

            //Use/NoUse에 따라 세부사항 작성
            if (logic == Logic.OR)
            {
                if (m_UseOop == false)
                {
                    return m_DiSensorOp.GetState();
                }
                else if (m_UseOp == false)
                {
                    return m_DiSensor.GetState();
                }
                else
                {
                    return (m_DiSensor.GetState() || m_DiSensorOp.GetState());
                }
            }
            else
            {
                if (m_UseOop == false) return m_DiSensorOp.GetState();
                else if (m_UseOp == false) return m_DiSensor.GetState();
                else return (m_DiSensor.GetState() && m_DiSensorOp.GetState());
            }
        }

        public override void SetState(bool state, Logic logic)
        {
            m_DiSensorOp.SetState(state);
            if (logic == Logic.AND)
            {
                m_DiSensor.SetState(state);
            }
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.DETECT, m_DiSensor.GetState());
            m_Tag.SetValue(tagDescriptor.DETECTOP, m_DiSensorOp.GetState());
            m_Tag.SetValue(tagDescriptor.SICK, Sick);
            m_Tag.SetValue(tagDescriptor.SICKOP, m_SickOp);
            m_Tag.SetValue(tagDescriptor.USE, m_UseOop);
            m_Tag.SetValue(tagDescriptor.USEOP, m_UseOp);
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
            ok &= (m_DiSensor != null);
            ok &= (m_DiSensorOp != null);


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
                    m_DiSensorOp.SetState(false);
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
