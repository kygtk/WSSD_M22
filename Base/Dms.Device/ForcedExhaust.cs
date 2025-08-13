///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.06.13
// Author       : jemoon
// Description  : Forced Exhaust Class
///////////////////////////////////////////////////////////////////////////
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
    public class ForcedExhaust : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorForcedExhaust tagDescriptor = new TagDescriptorForcedExhaust();
        #endregion

        #region Fields
        private IoDigitalInput m_DiMcOn = new IoDigitalInput();
        private IoDigitalInput m_DiElbOn = new IoDigitalInput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();

        public Alarm ALM_McOff = null;
        public Alarm ALM_ElbOff = null;
        private TagSetupSenSorInterlock m_SetupIntr = null;
        private bool m_IsAlarm = false;
        private bool m_CycleStopInAlarmCondition = false;
        private bool m_AlwaysOn = true;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiMcOn
        {
            get { return m_DiMcOn; }
            set { m_DiMcOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiElbOn
        {
            get { return m_DiElbOn; }
            set { m_DiElbOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        [Category("DMS : Option"), Description("Set CycleStop when ionizer in alarm condition")]
        public bool CycleStopInAlarmCondition
        {
            get { return m_CycleStopInAlarmCondition; }
            set { m_CycleStopInAlarmCondition = value; }
        }
        [Category("DMS : Option"), Description("Always On when EQP has glass and run in auto mode")]
        public bool AlwaysOn
        {
            get { return m_AlwaysOn; }
            set { m_AlwaysOn = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupInterlock
        {
            get { return m_SetupIntr; }
            set { m_SetupIntr = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        #endregion

        #region Constructor
        public ForcedExhaust()
        {
            this.Name = "__ Unit Forced Exhaust";
        }
        #endregion

        #region Methods
        public bool IsMcOff()
        {
            bool alarm = false;
            alarm |= (m_DiMcOn != null) && m_DiMcOn.GetState();
            return alarm;
        }
        public bool IsElbOff()
        {
            bool alarm = false;
            alarm |= (m_DiElbOn != null) && m_DiElbOn.GetState();
            return alarm;
        }
        public bool IsRun()
        {
            bool run = false;
            run |= (m_DoRun != null) && m_DoRun.GetState();
            return run;
        }
        public void SetRun(bool run)
        {
            if (m_DoRun != null)
            {
                m_DoRun.SetState(run);
            }
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

            log = string.Format("FORCEDEXT\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ForcedExhaust); }
        }
        public override string ToString()
        {
            return this.Name;
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
            ok &= (m_DiMcOn != null);
            ok &= (m_DiElbOn != null);


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
                ALM_McOff = new Alarm(this.Name + " MC Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ElbOff = new Alarm(this.Name + " ELB Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.Etc);
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupIntr);


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
                    m_DiMcOn.SetState(true);
                    m_DiElbOn.SetState(true);
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
            m_Tag.SetValue(tagDescriptor.MC_ON, !IsMcOff());
            m_Tag.SetValue(tagDescriptor.ELB_ON, !IsElbOff());
            m_Tag.SetValue(tagDescriptor.RUN, IsRun());
        }
        #endregion
    }
}
