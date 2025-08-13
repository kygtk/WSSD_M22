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
    public class EuvLamp : _Lamp
    {
        #region Struct
        protected struct TagLampIfFlag
        {
            public bool m_bLampIntensityCheckReq;
            public bool m_bLampIntensityCheckComp;
            public bool m_bLampIntensityCheckFail;
            public double m_nLampIntensity;
            public uint m_nLampOnTime;
        }
        #endregion

        #region Fields
        protected TagLampIfFlag m_IfFlag;
        protected bool m_IsOn = false;
        protected TagSetupInfo m_SetupUsedTime = null;
        public Alarm ALM_UsedTimeOver = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public bool LampIntensityCheckReq
        {
            get { return m_IfFlag.m_bLampIntensityCheckReq; }
            set { m_IfFlag.m_bLampIntensityCheckReq = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool LampIntensityCheckComp
        {
            get { return m_IfFlag.m_bLampIntensityCheckComp; }
            set { m_IfFlag.m_bLampIntensityCheckComp = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool LampIntensityCheckFail
        {
            get { return m_IfFlag.m_bLampIntensityCheckFail; }
            set { m_IfFlag.m_bLampIntensityCheckFail = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double LampIntensity
        {
            get { return m_IfFlag.m_nLampIntensity; }
            set { m_IfFlag.m_nLampIntensity = value; }
        }
        [Browsable(false), XmlIgnore()]
        public uint LampOnTime
        {
            get { return m_IfFlag.m_nLampOnTime; }
            set { m_IfFlag.m_nLampOnTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupUsedTime
        {
            get { return m_SetupUsedTime; }
            set { m_SetupUsedTime = value; }
        }
        #endregion

        #region Constructor
        protected EuvLamp() { }
        #endregion

        #region Methods
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

            log = string.Format("EuvLamp  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        public uint GetLampUsedTime()
        {
            return m_IfFlag.m_nLampOnTime;
        }
        public bool IsUse()
        {
            return m_Server.JobCond.EuvLampUse(this);
        }

        public virtual bool GetLampOnSelected()
        {
            throw new NotImplementedException();
        }
        public virtual void LampOnSelect(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool GetUsedTimeOver()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override void Off()
        {
            if (!this.Initialized) return;

            m_IsOn = false;
        }
        public override bool IsOn()
        {
            if (!this.Initialized) return false;
            return m_IsOn;
        }
        public override bool IsOff()
        {
            if (!this.Initialized) return false;
            return !m_IsOn;
        }
        public override void On()
        {
            if (!this.Initialized) return;
            if (GetLampOnSelected())
                m_IsOn = true;
        }

        public override Type FamilyType
        {
            get { return typeof(EuvLamp); }
        }
        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);

                if (m_Simul.Device)
                {
                    MessageBox.Show(msg);
                }
            }
        }
        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        public override void UpdateTag()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class EuvLamp_Io : EuvLamp
    {
        #region Fields
        private IoDigitalOutput m_DoSelect = new IoDigitalOutput();     //Ushio : ULS, GS : Lamp Select
        private IoDigitalInput m_DiUsedTimeOver = new IoDigitalInput(); //Ushio : ULA
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalOutput DoSelect
        {
            get { return m_DoSelect; }
            set { m_DoSelect = value; }
        }
        [Category("DMS : Setting"),
        Description("Set this when use Ushio EUV Unit")]
        public IoDigitalInput DiUsedTimeOver
        {
            get { return m_DiUsedTimeOver; }
            set { m_DiUsedTimeOver = value; }
        }
        #endregion

        #region Constructor
        public EuvLamp_Io()
        {
            this.Name = "Euv Lamp__";
        }
        #endregion

        #region Override

        #region Method
        public override bool GetLampOnSelected()
        {
            return m_DoSelect.GetState();
        }
        public override void LampOnSelect(bool bOn)
        {
            if (bOn ^ GetLampOnSelected())
            {
                m_DoSelect.SetState(bOn);
            }
        }
        public override bool GetUsedTimeOver()
        {
            return m_DiUsedTimeOver.GetState();
        }
        #endregion
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
            //ok &= (m_DoON != null);
            ok &= (m_DoSelect != null);


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
                ALM_UsedTimeOver = new Alarm(this.Name + " Used Time Over", AlarmLevel.L, AlarmCode.EquipmentSafety); // 09.12.01 minhan


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupUsedTime = new TagSetupInfo(this.Name + " Life Time", OptionType.None, OptionFormat.Digit, UnitType.hour, "1000");
                setupGenInfoProvider.InitFromDB(this.m_SetupUsedTime);


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
            m_Tag.SetValue(tagDescriptor.NOUSE, !IsUse());
            m_Tag.SetValue(tagDescriptor.ON, IsOn());
            m_Tag.SetValue(tagDescriptor.OFF, IsOff());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class EuvLamp_Ec : EuvLamp
    {
        #region Fields
        private SlaveDigitalOutput m_DoSelect = new SlaveDigitalOutput();     //Ushio : ULS, GS : Lamp Select
        private SlaveDigitalInput m_DiUsedTimeOver = new SlaveDigitalInput(); //Ushio : ULA
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public SlaveDigitalOutput DoSelect
        {
            get { return m_DoSelect; }
            set { m_DoSelect = value; }
        }
        [Category("DMS : Setting"),
        Description("Set this when use Ushio EUV Unit")]
        public SlaveDigitalInput DiUsedTimeOver
        {
            get { return m_DiUsedTimeOver; }
            set { m_DiUsedTimeOver = value; }
        }
        #endregion

        #region Constructor
        public EuvLamp_Ec()
        {
            this.Name = "Euv Lamp__";
        }
        #endregion

        #region Override

        #region Method
        public override bool GetLampOnSelected()
        {
            return m_DoSelect.GetState();
        }
        public override void LampOnSelect(bool bOn)
        {
            if (bOn ^ GetLampOnSelected())
            {
                m_DoSelect.SetState(bOn);
            }
        }
        public override bool GetUsedTimeOver()
        {
            return m_DiUsedTimeOver.GetState();
        }
        #endregion
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
            //ok &= (m_DoON != null);
            ok &= (m_DoSelect != null);


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
                ALM_UsedTimeOver = new Alarm(this.Name + " Used Time Over", AlarmLevel.L, AlarmCode.EquipmentSafety); // 09.12.01 minhan


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupUsedTime = new TagSetupInfo(this.Name + " Life Time", OptionType.None, OptionFormat.Digit, UnitType.hour, "1000");
                setupGenInfoProvider.InitFromDB(this.m_SetupUsedTime);


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
            m_Tag.SetValue(tagDescriptor.NOUSE, !IsUse());
            m_Tag.SetValue(tagDescriptor.ON, IsOn());
            m_Tag.SetValue(tagDescriptor.OFF, IsOff());
        }
        #endregion
    }
}
