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
    public class DryPump : _Pump
    {
        #region Tag Descriptor
        protected static new TagDescriptorDryPump tagDescriptor = new TagDescriptorDryPump();
        #endregion

        #region Fields
        private IoDigitalInput m_DiRunning = new IoDigitalInput();
        private IoDigitalInput m_DiWarning = new IoDigitalInput();
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();

        //private IoDigitalInput m_DiN2FlowWarning = new IoDigitalInput();
        //private IoDigitalInput m_DiExtWarning = new IoDigitalInput();
        //private IoDigitalInput m_DiPcwFlow = new IoDigitalInput();

        //private IoDigitalOutput m_DoRemoteMode = new IoDigitalOutput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();

        public Alarm ALM_PumpAlarm;
        public Alarm ALM_PumpWarning;

        private TagSetupInfo m_SetupPumpUse;
        private _GenericCollection<Gauge> m_InterlockGauges = new _GenericCollection<Gauge>();
        private Inverter m_Inverter;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiWarning
        {
            get { return m_DiWarning; }
            set { m_DiWarning = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiRunning
        {
            get { return m_DiRunning; }
            set { m_DiRunning = value; }
        }
        //[Category("DMS : Setting")]
        //public IoDigitalInput DiN2FlowWarning
        //{
        //    get { return m_DiN2FlowWarning; }
        //    set { m_DiN2FlowWarning = value; }
        //}
        //[Category("DMS : Setting")]
        //public IoDigitalInput DiExtWarning
        //{
        //    get { return m_DiExtWarning; }
        //    set { m_DiExtWarning = value; }
        //}
        //[Category("DMS : Setting")]
        //public IoDigitalInput DiPcwFlow
        //{
        //    get { return m_DiPcwFlow; }
        //    set { m_DiPcwFlow = value; }
        //}
        [Category("DMS : Setting")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        //[Category("DMS : Setting")]
        //public IoDigitalOutput DoRemoteMode
        //{
        //    get { return m_DoRemoteMode; }
        //    set { m_DoRemoteMode = value; }
        //}    
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
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupPumpUse
        {
            get { return m_SetupPumpUse; }
            set { m_SetupPumpUse = value; }
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
        public DryPump()
        {
            this.Name = "__ Pump";
        }
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

        public bool IsWarning()
        {
            if (!this.Initialized) return false;

            return DiWarning.GetState();
        }
        public bool IsRuuning()
        {
            if (!this.Initialized) return false;

            return DiRunning.GetState();
        }
        //public bool IsPcwFlowOk()
        //{
        //    if (!this.Initialized) return false;

        //    return DiPcwFlow.GetState();
        //}
        //public bool IsExtWarning()
        //{
        //    if (!this.Initialized) return false;

        //    return DiExtWarning.GetState();
        //}
        #endregion

        #region Override
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            bool rv = false;

            rv |= DiAlarm.GetState();
            //rv |= DiWarning.GetState();
            //rv |= DiPcwFlow.GetState();

            return rv;
        }

        public override bool IsRun()
        {
            if (!this.Initialized) return false;

            return DoRun.GetState();
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
                DoRun.SetState(false);
                if (Simul.Device) DiRunning.SetState(false);
            }
        }

        public override void Run()
        {
            if (!this.Initialized) return;

            if (!IsRun())
            {
                DoRun.SetState(true);

                if (Simul.Device) DiRunning.SetState(true);
            }
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
            ok &= (DiAlarm != null);
            ok &= (DoRun != null);


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

                if (null != this.DiWarning)
                {
                    ALM_PumpWarning = new Alarm(this.Name + " Warning", AlarmLevel.L, AlarmCode.EquipmentSafety);
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupPumpUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                SetupGenInfoProvider.Instance.InitFromDB(m_SetupPumpUse);


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
            m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            m_Tag.SetValue(tagDescriptor.RUN, DoRun.GetState());
            m_Tag.SetValue(tagDescriptor.RUNNING, DiRunning.GetState());
            m_Tag.SetValue(tagDescriptor.STOP, !DoRun.GetState());

            //m_Tag.SetValue(tagDescriptor.PCWFLOWOK, DiPcwFlow.GetState());
            //m_Tag.SetValue(tagDescriptor.EXTWARNING, DiExtWarning.GetState());
            //m_Tag.SetValue(tagDescriptor.N2FLOWWARNING, DiN2FlowWarning.GetState()); 
            m_Tag.SetValue(tagDescriptor.WARNING, DiWarning.GetState());
        }

        public override Boolean IsProcess()
        {
            return IsRun();
        }
        #endregion
    }
}
