///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.21
// Author       : eun
// Description  : Ionizer Class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using Dms.Util.IODefine;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Ionizer : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorIonizer tagDescriptor = new TagDescriptorIonizer();
        #endregion

        #region Fields
        public Alarm ALM_RunAlarm = null; // 09.11.26 minhan
        public Alarm ALM_LevelAlarm = null;
        public Alarm ALM_ConditionAlarm = null;
        public Alarm ALM_ControlAlarm = null;
        protected TagSetupSenSorInterlock m_SetupIonizerIntr = null;
        protected bool m_IsAlarm = false;
        protected bool m_Run = false;
        protected bool m_CycleStopInAlarmCondition = true;
        protected bool m_AlwaysOn = true; //jemoon
        #endregion

        #region Properties
        [Category("DMS : Option"), Description("Set CycleStop when ionizer in alarm condition")]
        public bool CycleStopInAlarmCondition
        {
            get { return m_CycleStopInAlarmCondition; }
            set { m_CycleStopInAlarmCondition = value; }
        }
        [Category("DMS : Option"), Description("Keep ionizer ON condition always")]
        public bool AlwaysOn
        {
            get { return m_AlwaysOn; }
            set { m_AlwaysOn = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupIonizerInterlock
        {
            get { return m_SetupIonizerIntr; }
            set { m_SetupIonizerIntr = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        #endregion

        #region Constructor
        protected Ionizer() { }
        #endregion

        #region Methods
        public virtual bool IsRunAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsLevelAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsConditionAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsControlAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual void SetRun(bool on)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsRun()
        {
            throw new NotImplementedException();
        }

        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("IONIZER \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get
            {
                return typeof(Ionizer);
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
            m_Tag.SetValue(tagDescriptor.RUN_ALARM, IsRunAlarm()); // 09.11.27 minhan
            m_Tag.SetValue(tagDescriptor.LEVEL_ALARM, IsLevelAlarm());
            m_Tag.SetValue(tagDescriptor.COND_ALARM, IsConditionAlarm());
            m_Tag.SetValue(tagDescriptor.CONTROL_ALARM, IsControlAlarm());
            m_Tag.SetValue(tagDescriptor.RUN, IsRun());
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Ionizer_Io : Ionizer
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalInput m_DiRunAlarm = new IoDigitalInput(); // 09.11.26 minhan
        private IoDigitalInput m_DiLevelAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiConditionAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiControlAlarm = new IoDigitalInput();
        private IoDigitalOutput m_DoIonizerRun = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiRunAlarm // 09.11.26 minhan
        {
            get { return m_DiRunAlarm; }
            set { m_DiRunAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiLevelAlarm
        {
            get { return m_DiLevelAlarm; }
            set { m_DiLevelAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiConditionAlarm
        {
            get { return m_DiConditionAlarm; }
            set { m_DiConditionAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiControlAlarm
        {
            get { return m_DiControlAlarm; }
            set { m_DiControlAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoIonizerRun
        {
            get { return m_DoIonizerRun; }
            set { m_DoIonizerRun = value; }
        }
        #endregion

        #region Constructor
        public Ionizer_Io()
        {
            this.Name = "__ Unit Ionizer";
        }
        #endregion

        #region Methods
        public override bool IsRunAlarm() // 09.11.26 minhan
        {
            if (m_DiRunAlarm != null)
            {
                return m_DiRunAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool IsLevelAlarm()
        {
            if (m_DiLevelAlarm != null)
            {
                return m_DiLevelAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool IsConditionAlarm()
        {
            if (m_DiConditionAlarm != null)
            {
                return m_DiConditionAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool IsControlAlarm()
        {
            if (m_DiControlAlarm != null)
            {
                return m_DiControlAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override void SetRun(bool on)
        {
            if (m_DoIonizerRun != null)
            {
                m_DoIonizerRun.SetState(on);
            }
            else
            {
                m_Run = on;
            }
        }
        public override bool IsRun()
        {
            bool run = true;
            if (m_DoIonizerRun != null)
            {
                run = m_DoIonizerRun.GetState();
            }
            else
            {
                run = m_Run;
            }

            return run;
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
            ok &= (DiLevelAlarm != null);
            ok &= (DiConditionAlarm != null);
            ok &= (DiControlAlarm != null);
            //ok &= (DiRunAlarm != null); // 09.11.26 minhan //jemoon : 안쓰는 경우도 check 해야겠지

            if (m_DoIonizerRun == null)
            {
                m_Run = true;
            }

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
                ALM_LevelAlarm = new Alarm(this.Name + " Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ConditionAlarm = new Alarm(this.Name + " Condition Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ControlAlarm = new Alarm(this.Name + " Control Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                if (m_DiRunAlarm != null)
                {
                    ALM_RunAlarm = new Alarm(this.Name + " Run Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.11.27 minhan
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupIonizerIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.Etc);
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupIonizerIntr);


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
    public class Ionizer_Ec : Ionizer
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalInput m_DiRunAlarm = new SlaveDigitalInput(); // 09.11.26 minhan
        private SlaveDigitalInput m_DiLevelAlarm = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiConditionAlarm = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiControlAlarm = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoIonizerRun = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiRunAlarm // 09.11.26 minhan
        {
            get { return m_DiRunAlarm; }
            set { m_DiRunAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiLevelAlarm
        {
            get { return m_DiLevelAlarm; }
            set { m_DiLevelAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiConditionAlarm
        {
            get { return m_DiConditionAlarm; }
            set { m_DiConditionAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiControlAlarm
        {
            get { return m_DiControlAlarm; }
            set { m_DiControlAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoIonizerRun
        {
            get { return m_DoIonizerRun; }
            set { m_DoIonizerRun = value; }
        }
        #endregion

        #region Constructor
        public Ionizer_Ec()
        {
            this.Name = "__ Unit Ionizer";
        }
        #endregion

        #region Methods
        public override bool IsRunAlarm() // 09.11.26 minhan
        {
            if (m_DiRunAlarm != null)
            {
                return m_DiRunAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool IsLevelAlarm()
        {
            if (m_DiLevelAlarm != null)
            {
                return m_DiLevelAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool IsConditionAlarm()
        {
            if (m_DiConditionAlarm != null)
            {
                return m_DiConditionAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool IsControlAlarm()
        {
            if (m_DiControlAlarm != null)
            {
                return m_DiControlAlarm.GetState();
            }
            else
            {
                return false;
            }
        }
        public override void SetRun(bool on)
        {
            if (m_DoIonizerRun != null)
            {
                m_DoIonizerRun.SetState(on);
            }
            else
            {
                m_Run = on;
            }
        }
        public override bool IsRun()
        {
            bool run = true;
            if (m_DoIonizerRun != null)
            {
                run = m_DoIonizerRun.GetState();
            }
            else
            {
                run = m_Run;
            }

            return run;
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
            ok &= (DiLevelAlarm != null);
            ok &= (DiConditionAlarm != null);
            ok &= (DiControlAlarm != null);
            //ok &= (DiRunAlarm != null); // 09.11.26 minhan //jemoon : 안쓰는 경우도 check 해야겠지

            if (m_DoIonizerRun == null)
            {
                m_Run = true;
            }

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
                ALM_LevelAlarm = new Alarm(this.Name + " Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ConditionAlarm = new Alarm(this.Name + " Condition Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ControlAlarm = new Alarm(this.Name + " Control Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                if (m_DiRunAlarm != null)
                {
                    ALM_RunAlarm = new Alarm(this.Name + " Run Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.11.27 minhan
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupIonizerIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.Etc);
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupIonizerIntr);


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
}
