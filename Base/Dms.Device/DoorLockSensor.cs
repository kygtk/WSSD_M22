using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DoorLockSensor : DoorSensor
    {
        #region Tag Descriptor
        protected static new TagDescriptorDoorLock tagDescriptor = new TagDescriptorDoorLock();
        #endregion

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        protected DoorLockSensor() { }
        #endregion

        #region Methods
        public virtual bool IsLocked()
        {
            throw new NotImplementedException();
        }
        public virtual void SetLock(bool op)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(DoorLockSensor); }
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
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DoorLockSensor_Io : DoorLockSensor
    {
        #region Fields
        protected IoDigitalInput m_DiSensor = new IoDigitalInput();
        protected IoDigitalOutput m_DoLock = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiSensor
        {
            get { return m_DiSensor; }
            set { m_DiSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoLock
        {
            get { return m_DoLock; }
            set { m_DoLock = value; }
        }
        #endregion

        #region Constructor
        public DoorLockSensor_Io()
        {
            this.Name = "__ Unit Door Lock Sensor";
            m_InterlockType = SensorInterlockType.Door;
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

        public override bool IsLocked()
        {
            if (!m_Initialized) return false;

            return m_DoLock.GetState();
        }

        public override void SetLock(bool op)
        {
            if (!m_Initialized) return;

            m_DoLock.SetState(op);
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
            ok &= m_DoLock != null;


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
    public class DoorLockSensor_Ap : DoorLockSensor
    {
        #region Fields
        protected D4SL m_D4SL;
        protected int m_Channel;
        #endregion

        #region Properties
        [Category("DMS : I/O Setting"), ReadOnly(true)]
        [Description("Connected D4SL board.\r\nCreate D4SL first, then connect door lock sensor in D40A setting.")]
        [XmlIgnore()]
        public D4SL D4SL
        {
            get { return m_D4SL; }
            set { m_D4SL = value; }
        }
        [Category("DMS : I/O Setting"), ReadOnly(true), DisplayName("D4SL Channel")]
        [Description("Channel in D4SL board.")]
        [XmlIgnore()]
        public int Channel
        {
            get { return m_Channel; }
            set
            {
                m_Channel = value;
                if (m_Channel < 0) m_Channel = 0;
                if (m_Channel > 7) m_Channel = 7;
            }
        }
        #endregion


        #region Constructor
        public DoorLockSensor_Ap()
        {
            this.Name = "__ Unit Door Lock Sensor";
            m_InterlockType = SensorInterlockType.Door;
        }
        #endregion

        #region Methods
        public override void SetState(bool state)
        {
        }

        public override bool IsDetected()
        {
            if (!m_Initialized) return false;

            return m_D4SL.IsDetected(m_Channel);
        }

        public override bool IsLocked()
        {
            if (!m_Initialized) return false;

            return m_D4SL.IsLocked(m_Channel);
        }

        public override void SetLock(bool op)
        {
            if (!m_Initialized) return;

            m_D4SL.SetLock(m_Channel, op);
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
            ok &= m_D4SL != null;



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
                    //m_DiSensor.SetState(false);
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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.DETECT, IsDetected());
            m_Tag.SetValue(tagDescriptor.LOCKED, IsLocked());
        }
        #endregion
    }
}
