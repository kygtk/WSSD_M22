///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Pump Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Pump : _Pump
    {
        #region Fields
        public Alarm ALM_PumpAlarm;
        protected TagSetupInfo m_SetupPumpUse;
        protected TagSetupInfo m_SetupInverterHz;
        protected _GenericCollection<Gauge> m_InterlockGauges = new _GenericCollection<Gauge>();
        protected Inverter m_Inverter;
        protected bool m_InverterControlBySetup = false;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public _GenericCollection<Gauge> InterlockGauges
        {
            get { return m_InterlockGauges; }
            set { m_InterlockGauges = value; }
        }
        [Category("DMS : Setting")]
        public Inverter Inverter
        {
            get { return m_Inverter; }
            set { m_Inverter = value; }
        }
        [Category("DMS : Option")]
        public bool InverterControlBySetup
        {
            get { return m_InverterControlBySetup; }
            set { m_InverterControlBySetup = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupPumpUse
        {
            get { return m_SetupPumpUse; }
            set { m_SetupPumpUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupInverterHz
        {
            get { return m_SetupInverterHz; }
            set { m_SetupInverterHz = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsUse
        {
            get
            {
                if (m_SetupPumpUse == null) return false;
                return m_SetupPumpUse.GetValue<bool>();
            }
        }
        #endregion

        #region Constructor
        protected Pump() { }
        #endregion

        #region Methods
        public double GetInterlockGaugeValue()
        {
            if (!this.Initialized) return 0.0;

            double minValue = -999.99;
            foreach (_Gauge gauge in m_InterlockGauges)
            {
                double value = gauge.CurValue;
                if (minValue == -999.99)
                {
                    minValue = value;
                }
                if (minValue > value)
                {
                    minValue = value;
                }
            }
            return minValue;
        }

        public double GetInterlockGaugeMaxValue()
        {
            if (!this.Initialized) return 0.0;

            double maxValue = 999.99;
            foreach (_Gauge gauge in m_InterlockGauges)
            {
                double value = gauge.CurValue;
                if (maxValue == 999.99)
                {
                    maxValue = value;
                }
                if (maxValue < value)
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }

        public double GetInterlockGaugeValue(int id)
        {
            if (!this.Initialized) return 0.0;

            _Gauge gauge = m_InterlockGauges[id] as _Gauge;
            return gauge.CurValue;
        }

        public bool SetPumpAct(PumpAct act)
        {
            if (!this.Initialized) return false;

            switch (act)
            {
                case PumpAct.Noop:
                    break;
                case PumpAct.Stop:
                    Stop();
                    break;
                case PumpAct.Run:
                    Run();
                    break;
            }

            return true;
        }
        #endregion

        #region Virtuals
        #endregion

        #region _Pump Overrides
        public override bool IsAlarm()
        {
            throw new NotImplementedException();
        }

        public override bool IsRun()
        {
            throw new NotImplementedException();
        }

        public override bool IsStop()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override bool IsProcess()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(Pump); }
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
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
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Pump_Io : Pump
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        #endregion

        #region Constructor
        public Pump_Io()
        {
            this.Name = "__ Pump";
        }
        #endregion

        #region Methods
        #endregion

        #region _Pump Overrides
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            return DiAlarm.GetState();
        }

        public override bool IsRun()
        {
            if (!this.Initialized) return false;

            if (!m_InverterControlBySetup && m_DoRun != null)
                return m_DoRun.GetState();
            else
                return m_Inverter.IsRun();
        }

        public override bool IsStop()
        {
            if (!this.Initialized) return false;

            bool bRun = false;

            bRun |= IsRun();

            return !bRun;
        }

        public override void Stop()
        {
            if (!this.Initialized) return;

            if (true == IsRun())
            {
                if (!m_InverterControlBySetup && m_DoRun != null)
                    m_DoRun.SetState(false);
                else
                    m_Inverter.Run(false);
            }
        }

        public override void Run()
        {
            if (!this.Initialized) return;

            if (m_Inverter != null && m_InverterControlBySetup)
            {
                m_Inverter.SetCurrentFrequency(m_SetupInverterHz.GetValue<double>());
            }

            if (!IsRun())
            {
                if (!m_InverterControlBySetup && m_DoRun != null)
                    m_DoRun.SetState(true);
                else
                    m_Inverter.Run(true);
            }
        }

        public override Boolean IsProcess()
        {
            return IsRun();
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
            ok &= (DiAlarm != null);
            ok &= (m_DoRun != null) || (m_Inverter != null);


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
                if (null != this.DiAlarm)
                {
                    ALM_PumpAlarm = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                if (this.Name != "HPMJ Pump")
                {
                    m_SetupPumpUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupPumpUse);
                    if (m_Inverter != null && m_InverterControlBySetup)
                    {
                        m_SetupInverterHz = new TagSetupInfo(this.Name + " Invert Hz", OptionType.None, OptionFormat.None, UnitType.Hz, "60");
                        SetupGenInfoProvider.Instance.InitFromDB(m_SetupInverterHz);
                    }
                }
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
                    DiAlarm.SetState(false);
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
            m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            m_Tag.SetValue(tagDescriptor.RUN, IsRun());
            m_Tag.SetValue(tagDescriptor.STOP, !IsRun());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Pump_Ec : Pump
    {
        #region Fields
        private SlaveDigitalInput m_DiAlarm = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoRun = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public SlaveDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : Setting")]
        public SlaveDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        #endregion

        #region Constructor
        public Pump_Ec()
        {
            this.Name = "__ Pump";
        }
        #endregion

        #region Methods
        #endregion

        #region _Pump Overrides
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            return DiAlarm.GetState();
        }

        public override bool IsRun()
        {
            if (!this.Initialized) return false;

            if (!m_InverterControlBySetup && m_DoRun != null)
                return m_DoRun.GetState();
            else
                return m_Inverter.IsRun();
        }

        public override bool IsStop()
        {
            if (!this.Initialized) return false;

            bool bRun = false;

            bRun |= IsRun();

            return !bRun;
        }

        public override void Stop()
        {
            if (!this.Initialized) return;

            if (true == IsRun())
            {
                if (!m_InverterControlBySetup && m_DoRun != null)
                    m_DoRun.SetState(false);
                else
                    m_Inverter.Run(false);
            }
        }

        public override void Run()
        {
            if (!this.Initialized) return;

            if (m_Inverter != null && m_InverterControlBySetup)
            {
                m_Inverter.SetCurrentFrequency(m_SetupInverterHz.GetValue<double>());
            }

            if (!IsRun())
            {
                if (!m_InverterControlBySetup && m_DoRun != null)
                    m_DoRun.SetState(true);
                else
                    m_Inverter.Run(true);
            }
        }

        public override Boolean IsProcess()
        {
            return IsRun();
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
            ok &= (DiAlarm != null);
            ok &= (m_DoRun != null) || (m_Inverter != null);


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
                if (null != this.DiAlarm)
                {
                    ALM_PumpAlarm = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                if (this.Name != "HPMJ Pump")
                {
                    m_SetupPumpUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupPumpUse);
                    if (m_Inverter != null && m_InverterControlBySetup)
                    {
                        m_SetupInverterHz = new TagSetupInfo(this.Name + " Invert Hz", OptionType.None, OptionFormat.None, UnitType.Hz, "60");
                        SetupGenInfoProvider.Instance.InitFromDB(m_SetupInverterHz);
                    }
                }
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
                    DiAlarm.SetState(false);
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
            m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            m_Tag.SetValue(tagDescriptor.RUN, IsRun());
            m_Tag.SetValue(tagDescriptor.STOP, !IsRun());
        }
        #endregion
    }
}
