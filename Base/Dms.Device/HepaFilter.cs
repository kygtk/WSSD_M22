///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.21
// Author       : eun
// Description  : Hepa filter Class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;
using Dms.Util.IODefine;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class HepaFilter : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorHepaFilter tagDescriptor = new TagDescriptorHepaFilter();
        #endregion

        #region Fields
        public Alarm ALM_HepaAlarm = null;
        public Alarm ALM_CpOff = null;
        protected TagSetupSenSorInterlock m_SetupHepaIntr = null;
        protected bool m_IsAlarm = false;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupHepaInterlock
        {
            get { return m_SetupHepaIntr; }
            set { m_SetupHepaIntr = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        #endregion

        #region Constructor
        protected HepaFilter() { }
        #endregion

        #region Methods
        public virtual bool IsAlarmDetect()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsCpOn()
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

            log = string.Format("HEPA    \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(HepaFilter); }
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
    public class HepaFilter_Io : HepaFilter
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiCpOn = new IoDigitalInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        #endregion

        #region Constructor
        public HepaFilter_Io()
        {
            this.Name = "__ Unit Hepa Filter";
        }
        #endregion

        #region Methods
        public override bool IsAlarmDetect()
        {
            if (m_DiAlarm != null)
            {
                return m_DiAlarm.GetState();
            }
            else return false;
        }
        public override bool IsCpOn()
        {
            return m_DiCpOn.GetState();
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
            ok &= (DiCpOn != null);


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
                if (DiAlarm != null)
                {
                    ALM_HepaAlarm = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }
                ALM_CpOff = new Alarm(this.Name + " C/P Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupHepaIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.FFU);
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupHepaIntr);


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
                    DiCpOn.SetState(true);
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
            if (DiAlarm != null)
            {
                m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            }
            m_Tag.SetValue(tagDescriptor.CPON, DiCpOn.GetState());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class HepaFilter_Ec : HepaFilter
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalInput m_DiAlarm = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiCpOn = new SlaveDigitalInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        #endregion

        #region Constructor
        public HepaFilter_Ec()
        {
            this.Name = "__ Unit Hepa Filter";
        }
        #endregion

        #region Methods
        public override bool IsAlarmDetect()
        {
            if (m_DiAlarm != null)
            {
                return m_DiAlarm.GetState();
            }
            else return false;
        }
        public override bool IsCpOn()
        {
            return m_DiCpOn.GetState();
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
            ok &= (DiCpOn != null);


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
                if (DiAlarm != null)
                {
                    ALM_HepaAlarm = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }
                ALM_CpOff = new Alarm(this.Name + " C/P Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupHepaIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.FFU);
                SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupHepaIntr);


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
                    DiCpOn.SetState(true);
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
            if (DiAlarm != null)
            {
                m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            }
            m_Tag.SetValue(tagDescriptor.CPON, DiCpOn.GetState());
        }
        #endregion
    }
}
