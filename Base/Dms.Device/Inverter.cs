using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using Dms.Util.IODefine;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Inverter : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorInverter tagDescriptor = new TagDescriptorInverter();
        #endregion

        #region Fields
        protected double m_CurFrequency = 0.0;
        protected int m_MaxFrequency = 60;
        protected int m_MinFrequency = 0;
        public Alarm ALM_InverterError = null;
        protected ushort m_MaxFrequencyAoValue = 0x7FFF;
        #endregion

        #region Properties
        [Category("Setting : Config")]
        public int MaxFrequency
        {
            get { return m_MaxFrequency; }
            set { m_MaxFrequency = value; }
        }
        [Category("Setting : Config")]
        public int MinFrequency
        {
            get { return m_MinFrequency; }
            set { m_MinFrequency = value; }
        }
        [Browsable(false)]
        public double CurFrequency
        {
            get { return m_CurFrequency; }
            set { m_CurFrequency = value; }
        }
        [Category("DMS : Setting")]
        public ushort MaxFrequencyAoValue
        {
            get { return m_MaxFrequencyAoValue; }
            set { m_MaxFrequencyAoValue = value; }
        }
        #endregion

        #region Constucture
        protected Inverter() { }
        #endregion

        #region Methods
        public virtual bool IsAlarm()
        {
            throw new NotImplementedException();
        }

        public virtual int GetAlarmId()
        {
            throw new NotImplementedException();
        }

        public virtual void Reset(bool reset)
        {
            throw new NotImplementedException();
        }

        public virtual void Run(bool run)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsRun()
        {
            throw new NotImplementedException();
        }

        public virtual void SetCurrentFrequency(double hertz)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentOutput(ushort adc)
        {
            double hertz;

            hertz = (double)adc * 60 / m_MaxFrequencyAoValue;

            SetCurrentFrequency(hertz);

            m_CurFrequency = GetCurrentFrequency();
        }

        public virtual double GetCurrentFrequency()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(Inverter); }
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
    public class Inverter_Io : Inverter
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiPowerCut = new IoDigitalInput();
        private IoDigitalOutput m_DoAlarmReset = new IoDigitalOutput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();
        private IoAnalogOutput m_AoFrequency = new IoAnalogOutput();
        private IoDigitalOutput m_DoInverterFreqSet = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("Setting : IO")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalInput DiPowerCut
        {//2009.09.21 kimgun
            get { return m_DiPowerCut; }
            set { m_DiPowerCut = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalOutput DoReset_Alarm
        {
            get { return m_DoAlarmReset; }
            set { m_DoAlarmReset = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalOutput DoInverterFreqSet
        {
            get { return m_DoInverterFreqSet; }
            set { m_DoInverterFreqSet = value; }
        }
        [Category("Setting : IO")]
        public IoAnalogOutput AoFrequency
        {
            get { return m_AoFrequency; }
            set { m_AoFrequency = value; }
        }
        #endregion

        #region Constucture
        public Inverter_Io()
        {
            this.Name = "__ Inverter";
        }
        #endregion

        #region Inverter Methods
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            return m_DiAlarm.GetState();
        }

        public override int GetAlarmId()
        {
            return m_DiAlarm.Id;
        }

        public override void Reset(bool reset)
        {
            m_DoAlarmReset.SetState(reset);
        }

        public override void Run(bool run)
        {
            m_DoRun.SetState(run);
        }

        public override bool IsRun()
        {
            return m_DoRun.GetState();
        }

        public override void SetCurrentFrequency(double hertz)
        {
            ushort adc;

            if (hertz > m_MaxFrequency)
            {
                hertz = m_MaxFrequency;
            }
            else if (hertz < m_MinFrequency)
            {
                hertz = m_MinFrequency;
            }

            adc = (ushort)(hertz * m_MaxFrequencyAoValue / 60);

            if (adc > m_MaxFrequencyAoValue) adc = m_MaxFrequencyAoValue;
            else if (adc < 0) adc = 0x0000;

            m_CurFrequency = hertz;

            m_AoFrequency.SetState(adc);
            m_DoInverterFreqSet.SetPulse(true, 1000);
        }

        public override double GetCurrentFrequency()
        {
            ushort adc = m_AoFrequency.GetState();
            double hertz;

            hertz = (double)adc * 60 / m_MaxFrequencyAoValue;

            return hertz;
        }
        #endregion

        #region Overrides
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
            ok &= (m_DiAlarm != null);
            ok &= (m_DoAlarmReset != null);
            ok &= (m_AoFrequency != null);


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
                if (null != this.m_DiAlarm)
                {
                    ALM_InverterError = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


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
                    m_DiAlarm.SetState(false);
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

            //if (Initialized == true) return DmsErrors.Success;

            //CreateTag(m_Server.TagContainer);

            //GenerateAssociatedDevices();

            //ALM_InverterError = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

            //bool ok = true;

            //ok &= (m_DiAlarm != null);
            //ok &= (m_DoAlarmReset != null);
            //ok &= (m_AoFrequency != null);

            //if (ok)
            //{
            //    SetSubscriber();

            //    this.Initialized = ok;

            //    return DmsErrors.Success;
            //}
            //else
            //{
            //    return DmsErrors.NotInitialized;
            //}
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, m_DiAlarm);
            m_Tag.SetValue(tagDescriptor.FREQUENCY, m_CurFrequency);
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class V1000 : Inverter
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalOutput m_DoAlarmReset = new IoDigitalOutput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();

        private IoAnalogInput m_AiCurrentFreq = new IoAnalogInput();
        private IoAnalogOutput m_AoTargetFreq = new IoAnalogOutput();
        private IoDigitalOutput m_DoFreqReq = new IoDigitalOutput();
        private IoDigitalInput m_DiFreqSet = new IoDigitalInput();
        #endregion

        #region Properties
        [Category("Setting : IO")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalOutput DoReset_Alarm
        {
            get { return m_DoAlarmReset; }
            set { m_DoAlarmReset = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }

        [Category("Setting : IO")]
        public IoAnalogInput AiCurrentFreq
        {
            get { return m_AiCurrentFreq; }
            set { m_AiCurrentFreq = value; }
        }
        [Category("Setting : IO")]
        public IoAnalogOutput AoTargetFreq
        {
            get { return m_AoTargetFreq; }
            set { m_AoTargetFreq = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalOutput DoFreqReq
        {
            get { return m_DoFreqReq; }
            set { m_DoFreqReq = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalInput DiFreqSet
        {
            get { return m_DiFreqSet; }
            set { m_DiFreqSet = value; }
        }
        [Category("DMS : Setting"), Browsable(false)]
        public new ushort MaxFrequencyAoValue
        {
            get { return m_MaxFrequencyAoValue; }
            set { m_MaxFrequencyAoValue = value; }
        }
        #endregion

        #region Constucture
        public V1000()
        {
            this.Name = "__ V1000 Inverter";
        }
        #endregion

        #region Methods
        private bool _IsAlarm()
        {
            if (!this.Initialized) return false;

            return m_DiAlarm.GetState();
        }

        private void _SetFreq(ushort freq)
        {
            if (!m_Initialized) return;

            m_AoTargetFreq.SetState(freq);

            Thread t = new Thread(() => _ReqFreq());
            t.IsBackground = true;
            t.Name = m_Name + "Req Freq";
            t.Start();
        }

        private void _ReqFreq()
        {
            if (!m_Initialized) return;

            m_DoFreqReq.SetState(true);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 3000)
            {
                bool ok = m_DiFreqSet.GetState();

                if (ok) break;
            }
            sw.Stop();

            m_DoFreqReq.SetState(false);
        }

        private ushort _GetFreq()
        {
            if (!m_Initialized) return 0;

            return (ushort)m_AiCurrentFreq.GetState();
        }
        #endregion

        #region Inverter Methods
        public override bool IsAlarm()
        {
            return _IsAlarm();
        }

        public override int GetAlarmId()
        {
            return m_DiAlarm.Id;
        }

        public override void Reset(bool reset)
        {
            m_DoAlarmReset.SetState(reset);
        }

        public override void Run(bool run)
        {
            m_DoRun.SetState(run);
        }

        public override bool IsRun()
        {
            return m_DoRun.GetState();
        }

        public override void SetCurrentFrequency(double hertz)
        {
            if (hertz > m_MaxFrequency)
            {
                hertz = m_MaxFrequency;
            }
            else if (hertz < m_MinFrequency)
            {
                hertz = m_MinFrequency;
            }

            m_CurFrequency = hertz;

            _SetFreq((ushort)hertz);
        }

        public override double GetCurrentFrequency()
        {
            return _GetFreq();
        }
        #endregion

        #region Overrides
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
            ok &= m_DiAlarm != null;
            ok &= m_DoAlarmReset != null;
            ok &= m_DoRun != null;

            ok &= m_AoTargetFreq != null;
            ok &= m_AiCurrentFreq != null;
            ok &= m_DoFreqReq != null;
            ok &= m_DiFreqSet != null;

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
                if (null != this.m_DiAlarm)
                {
                    ALM_InverterError = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


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
                    m_DiAlarm.SetState(false);
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

            //if (Initialized == true) return DmsErrors.Success;

            //CreateTag(m_Server.TagContainer);

            //GenerateAssociatedDevices();

            //ALM_InverterError = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

            //bool ok = true;

            //ok &= (m_DiAlarm != null);
            //ok &= (m_DoAlarmReset != null);
            //ok &= (m_AoFrequency != null);

            //if (ok)
            //{
            //    SetSubscriber();

            //    this.Initialized = ok;

            //    return DmsErrors.Success;
            //}
            //else
            //{
            //    return DmsErrors.NotInitialized;
            //}
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, m_DiAlarm);
            m_Tag.SetValue(tagDescriptor.FREQUENCY, m_CurFrequency);
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Inverter_Ec : Inverter
    {
        #region Fields
        private SlaveInverter m_SvInverter = new SlaveInverter();
        #endregion

        #region Properties
        [Category("Setting : Slave")]
        public SlaveInverter SlaveInverter
        {
            get { return m_SvInverter; }
            set { m_SvInverter = value; }
        }
        [Category("DMS : Setting"), Browsable(false)]
        public new ushort MaxFrequencyAoValue
        {
            get { return m_MaxFrequencyAoValue; }
            set { m_MaxFrequencyAoValue = value; }
        }
        #endregion

        #region Constucture
        public Inverter_Ec()
        {
            this.Name = "__ Inverter";
        }
        #endregion

        #region Inverter Methods
        public override bool IsAlarm()
        {
            if (!m_Initialized) return false;

            return m_SvInverter.IsAlarm();
        }

        public override int GetAlarmId()
        {
            return m_SvInverter.Id;
        }

        public override void Reset(bool reset)
        {
            if (!m_Initialized) return;

            m_SvInverter.SetAlarmReset(reset);
        }

        public override void Run(bool run)
        {
            if (!m_Initialized) return;

            m_SvInverter.SetRun(run);
        }

        public override bool IsRun()
        {
            return m_SvInverter.IsRun();
        }

        public override void SetCurrentFrequency(double hertz)
        {
            if (!m_Initialized) return;

            if (hertz > m_MaxFrequency)
            {
                hertz = m_MaxFrequency;
            }
            else if (hertz < m_MinFrequency)
            {
                hertz = m_MinFrequency;
            }

            m_SvInverter.SetTargetFrequency(hertz);
        }

        public override double GetCurrentFrequency()
        {
            if (!m_Initialized) return 0;

            return m_SvInverter.GetCurrentFrequency();
        }
        #endregion

        #region Overrides
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
            ok &= m_SvInverter != null;


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
                if (m_SvInverter != null)
                {
                    ALM_InverterError = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_SvInverter.SetAdvController();

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
                //if (m_Simul.Device)
                //{
                //    DiAlarm.SetState(false);
                //}


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

            //if (Initialized == true) return DmsErrors.Success;

            //CreateTag(m_Server.TagContainer);

            //GenerateAssociatedDevices();

            //ALM_InverterError = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

            //bool ok = true;

            //ok &= (m_DiAlarm != null);
            //ok &= (m_DoAlarmReset != null);
            //ok &= (m_AoFrequency != null);

            //if (ok)
            //{
            //    SetSubscriber();

            //    this.Initialized = ok;

            //    return DmsErrors.Success;
            //}
            //else
            //{
            //    return DmsErrors.NotInitialized;
            //}
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, IsAlarm());
            m_Tag.SetValue(tagDescriptor.FREQUENCY, m_CurFrequency);
        }
        #endregion
    }
}
