///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : SignalTower Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class SignalTower : _DeviceAsm
    {
        #region Consts
        public const int LampR = 0;
        public const int LampY = 1;
        public const int LampG = 2;
        public const int LampB = 3;
        #endregion

        #region Tag Descriptor
        protected static TagDescriptorSignalTower tagDescriptor = new TagDescriptorSignalTower();
        #endregion

        #region Fields
        [XmlIgnore()] public Lamp LampCommandR = Lamp.Off;
        [XmlIgnore()] public Lamp LampCommandY = Lamp.Off;
        [XmlIgnore()] public Lamp LampCommandG = Lamp.Off;
        [XmlIgnore()] public Lamp LampCommandB = Lamp.Off;
        #endregion

        #region Constructor
        protected SignalTower() { }
        #endregion

        #region Properties
        [XmlIgnore()] public virtual int LampCount { get; }
        #endregion

        #region Methods
        public virtual void SetState(int lampidx, bool state)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(SignalTower); }
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
    public class SignalTower_Io : SignalTower
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalOutput m_DoSignalLampR = new IoDigitalOutput();
        private IoDigitalOutput m_DoSignalLampY = new IoDigitalOutput();
        private IoDigitalOutput m_DoSignalLampG = new IoDigitalOutput();
        private IoDigitalOutput m_DoSignalLampB = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoSignalLampR
        {
            get { return m_DoSignalLampR; }
            set { m_DoSignalLampR = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoSignalLampY
        {
            get { return m_DoSignalLampY; }
            set { m_DoSignalLampY = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoSignalLampG
        {
            get { return m_DoSignalLampG; }
            set { m_DoSignalLampG = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoSignalLampB
        {
            get { return m_DoSignalLampB; }
            set { m_DoSignalLampB = value; }
        }

        public override int LampCount
        {
            get
            {
                int cnt = 0;
                if (m_DoSignalLampR != null) cnt++;
                if (m_DoSignalLampY != null) cnt++;
                if (m_DoSignalLampG != null) cnt++;
                if (m_DoSignalLampB != null) cnt++;
                return cnt;
            }
        }
        #endregion

        #region Constructor
        public SignalTower_Io()
        {
            this.Name = "__ Signal Lamp";
        }
        #endregion

        #region Methods
        public override void SetState(int lampidx, bool state)
        {
            switch (lampidx)
            {
                case LampR:
                    m_DoSignalLampR.SetState(state);
                    break;
                case LampY:
                    m_DoSignalLampY.SetState(state);
                    break;
                case LampG:
                    m_DoSignalLampG.SetState(state);
                    break;
                case LampB:
                    m_DoSignalLampB.SetState(state);
                    break;
            }
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
            ok &= (m_DoSignalLampR != null);
            ok &= (m_DoSignalLampY != null);
            ok &= (m_DoSignalLampG != null);
            ok &= (m_DoSignalLampB != null);


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
            m_Tag.SetValue(tagDescriptor.LAMP1, m_DoSignalLampR.GetState());
            m_Tag.SetValue(tagDescriptor.LAMP2, m_DoSignalLampY.GetState());
            m_Tag.SetValue(tagDescriptor.LAMP3, m_DoSignalLampG.GetState());
            m_Tag.SetValue(tagDescriptor.LAMP4, m_DoSignalLampB.GetState());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class SignalTower_Ec : SignalTower
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalOutput m_DoSignalLampR = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoSignalLampY = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoSignalLampG = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoSignalLampB = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoSignalLampR
        {
            get { return m_DoSignalLampR; }
            set { m_DoSignalLampR = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoSignalLampY
        {
            get { return m_DoSignalLampY; }
            set { m_DoSignalLampY = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoSignalLampG
        {
            get { return m_DoSignalLampG; }
            set { m_DoSignalLampG = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoSignalLampB
        {
            get { return m_DoSignalLampB; }
            set { m_DoSignalLampB = value; }
        }

        public override int LampCount
        {
            get
            {
                int cnt = 0;
                if (m_DoSignalLampR != null) cnt++;
                if (m_DoSignalLampY != null) cnt++;
                if (m_DoSignalLampG != null) cnt++;
                if (m_DoSignalLampB != null) cnt++;
                return cnt;
            }
        }
        #endregion

        #region Constructor
        public SignalTower_Ec()
        {
            this.Name = "__ Signal Lamp";
        }
        #endregion

        #region Methods
        public override void SetState(int lampidx, bool state)
        {
            switch (lampidx)
            {
                case LampR:
                    m_DoSignalLampR.SetState(state);
                    break;
                case LampY:
                    m_DoSignalLampY.SetState(state);
                    break;
                case LampG:
                    m_DoSignalLampG.SetState(state);
                    break;
                case LampB:
                    m_DoSignalLampB.SetState(state);
                    break;
            }
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
            ok &= (m_DoSignalLampR != null);
            ok &= (m_DoSignalLampY != null);
            ok &= (m_DoSignalLampG != null);
            ok &= (m_DoSignalLampB != null);


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
            m_Tag.SetValue(tagDescriptor.LAMP1, m_DoSignalLampR.GetState());
            m_Tag.SetValue(tagDescriptor.LAMP2, m_DoSignalLampY.GetState());
            m_Tag.SetValue(tagDescriptor.LAMP3, m_DoSignalLampG.GetState());
            m_Tag.SetValue(tagDescriptor.LAMP4, m_DoSignalLampB.GetState());
        }
        #endregion
    }
}
