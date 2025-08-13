using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BufferUnit : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorBufferUnit tagDescriptor = new TagDescriptorBufferUnit();
        #endregion

        #region Fields
        private Sensor m_TrayExistSensor;
        private ServoUnit m_HandServoUnit;

        private TagTransferIfFlag m_IfFlag;
        #endregion

        #region Properties
        [Category("Setting")]
        public ServoUnit HandServoUnit
        {
            get { return m_HandServoUnit; }
            set { m_HandServoUnit = value; }
        }
        [Category("Setting")]
        public Sensor TrayExistSensor
        {
            get { return m_TrayExistSensor; }
            set { m_TrayExistSensor = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagTransferIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        #endregion

        public BufferUnit()
        {

        }

        public bool IsTrayExist()
        {
            return m_TrayExistSensor.IsDetected();
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
            //ok &= (m_Ai != null);


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
                //if (m_InterlockEnable)
                //{
                //    ALM_LowerAlarm = new Alarm(this.Name + " Lower Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //    ALM_LowerWarning = new Alarm(this.Name + " Lower Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                //    ALM_UpperWarning = new Alarm(this.Name + " Upper Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                //    ALM_UpperAlarm = new Alarm(this.Name + " Upper Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //}

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                //m_CalibrationProvider = CalibrationProvider.Instance;
                //m_Info = new TagCalibrationInfo(m_GaugeType, this.Name);
                //m_CalibrationProvider.InitFromDB(m_Info);

                //if (m_InterlockEnable)
                //{
                //    m_InterlockProvider = SetupGaugeInterlockProvider.Instance;
                //    m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 20.0, 100.0, 150.0, false);
                //    m_InterlockProvider.InitFromDB(m_SetupInterlock);
                //}

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                //m_OldAdc = m_Ai.GetState();
                m_IfFlag = new TagTransferIfFlag();
                m_IfFlag.Reset();

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
            m_Tag.SetValue(tagDescriptor.TRAYEXIST, TrayExistSensor.IsDetected());
        }
    }
}
