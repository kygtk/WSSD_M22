///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Gauge Class
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.07 : jemoon - Debug 모드시 초기값 셋팅
///////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.Data;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Gauge : _Gauge
    {
        #region Enums
        public enum interlockMethod : int
        {//2011.01.18 kimgun
            PC, //기존의 상항 하한 설정방식
            PLC, //plc에서 사용하는 마진방식(taget+margin,target-margin)
        }
        #endregion

        #region Fields
        protected static CalibrationProvider m_CalibrationProvider = null;
        protected static SetupGaugeInterlockProvider m_InterlockProvider = null;
        protected GaugeType m_GaugeType;
        protected short m_OldAdc = 0;
        protected bool m_InterlockEnable = true;
        protected interlockMethod m_InterlockMethod = interlockMethod.PC;//2011.01.18 kimgun 이게 PLC가 되면 margin방식이고 PC면 기존 방식이다. setup view의 내용이 달라진다.
        protected ushort m_PointCount = 1;
        protected TagCalibrationInfo m_Info = null;
        protected TagGaugeInterlock m_SetupInterlock = null;
        protected bool m_IsAlarm = false;
        protected CvUnit m_Owner = null;

        public Alarm ALM_LowerAlarm = null;
        public Alarm ALM_LowerWarning = null;
        public Alarm ALM_UpperWarning = null;
        public Alarm ALM_UpperAlarm = null;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public GaugeType GaugeType
        {
            get { return m_GaugeType; }
            set { m_GaugeType = value; }
        }
        [Category("DMS : Setting")]
        public bool InterlockEnable
        {
            get { return m_InterlockEnable; }
            set { m_InterlockEnable = value; }
        }
        [Category("DMS : Setting"), Description("If PLC, 'TargetValue +/- Margin' is setting value.")]
        public interlockMethod InterlockMethod
        {//2011.01.18 kimgun
            get { return m_InterlockMethod; }
            set { m_InterlockMethod = value; }
        }
        [Category("DMS : Setting"), Description("Number of Decimal Places")]
        public ushort PointCount
        {
            get { return m_PointCount; }
            set { m_PointCount = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagCalibrationInfo Info
        {
            get { return m_Info; }
            set { m_Info = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagGaugeInterlock SetupInterlock
        {
            get { return m_SetupInterlock; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        [Category("DMS : Setting")]
        public CvUnit OwnerUnit
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        #endregion

        #region Constructor
        protected Gauge() { }
        #endregion

        #region Methods
        public double UpdateValue()
        {
            if (this.Initialized == false) return 0.0;

            short adc = GetValue();
            double gain = m_Info.FilterGain;

            m_CurAdc = (short)(adc * gain + m_OldAdc * (1.0 - gain));
            m_OldAdc = m_CurAdc;

            double realMax = m_Info.RealMax;
            double realMin = m_Info.RealMin;
            short adcMax = m_Info.AdcMax;
            short adcMin = m_Info.AdcMin;

            switch (m_Info.Scale)
            {
                case ScaleType.Common:
                    realMax = m_Info.RealMax;
                    realMin = m_Info.RealMin;
                    break;
                case ScaleType.Log:
                    realMax = Math.Log(m_Info.RealMax);
                    realMin = Math.Log(m_Info.RealMin);
                    break;
                case ScaleType.Log10:
                    realMax = Math.Log10(m_Info.RealMax);
                    realMin = Math.Log10(m_Info.RealMin);
                    break;
            }


            short diffAdc = (short)(adcMax - adcMin);
            double diffReal = realMax - realMin;

            if (diffAdc == 0)
            {
                m_CurValue = m_CurAdc;
            }
            else
            {
                double val = (diffReal / (double)diffAdc) * (m_CurAdc - adcMin) + realMin;

                switch (m_Info.Scale)
                {
                    case ScaleType.Common:
                        m_CurValue = val;
                        break;
                    case ScaleType.Log:
                        m_CurValue = Math.Exp(val);
                        break;
                    case ScaleType.Log10:
                        m_CurValue = Math.Pow(10.0, val);
                        break;
                }
            }

            if (m_OldValue != m_CurValue)
            {
                //m_OldValue = (m_CurValue = Math.Round(m_CurValue, 1));
                m_OldValue = (m_CurValue = Math.Round(m_CurValue, m_PointCount));
                UpdateTag();

                //string msg = string.Format("Gauge : {0} state change", this.Name);
                //m_Server.FireEvent(m_Tag, msg);
            }

            return m_CurValue;
        }
        protected virtual short GetValue()
        {
            return 0;
        }

        public bool UpdateFromStorage()
        {
            if (this.Initialized == false) return false;

            return m_CalibrationProvider.UpdateFromDB(m_Info.Name);
        }

        private bool WriteToStorage(TagCalibrationInfo info)
        {
            if (this.Initialized == false) return false;

            return m_CalibrationProvider.UpdateToDB(info);
        }

        public void SetGaugeInfo(TagCalibrationInfo info)
        {
            if (this.Initialized == false) return;

            WriteToStorage(info);
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

            log = string.Format("Gauge   \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(Gauge); }
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

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }

        public override void UpdateTag()
        {
            if (m_Simul.Device && m_UseSimulator)
            {
                m_Tag.SetValue(tagDescriptor.CURVAL, m_SimulCurValue);
            }
            else
            {
                m_Tag.SetValue(tagDescriptor.CURVAL, m_CurValue);
            }
            m_Tag.SetValue(tagDescriptor.CURADC, m_CurAdc);
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Gauge_Io : Gauge
    {
        #region Fields
        private IoAnalogInput m_Ai = new IoAnalogInput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoAnalogInput AnalogInput
        {
            get { return m_Ai; }
            set { m_Ai = value; }
        }
        #endregion

        #region Constructor
        public Gauge_Io()
        {
            this.Name = "__ Gauge";
        }
        #endregion

        #region Methods
        protected override short GetValue()
        {
            return m_Ai.GetState();
        }

        /// <summary>
        /// Reading GaugeInfo from Storage
        /// if items is exist
        /// then get from storage;
        /// else get default value by gauge type,
        /// </summary>
        /// <returns></returns>
        //public bool InitFromStorage(TagCalibrationInfo info)
        //{
        //    return m_CalibrationProvider.InitFromDB(info);
        //}
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
            ok &= (m_Ai != null);


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
                if (m_InterlockEnable)
                {
                    ALM_LowerAlarm = new Alarm(this.Name + " Lower Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_LowerWarning = new Alarm(this.Name + " Lower Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                    ALM_UpperWarning = new Alarm(this.Name + " Upper Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                    ALM_UpperAlarm = new Alarm(this.Name + " Upper Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_CalibrationProvider = CalibrationProvider.Instance;
                m_Info = new TagCalibrationInfo(m_GaugeType, this.Name);
                m_CalibrationProvider.InitFromDB(m_Info);

                if (m_InterlockEnable) // 11.02.01 minhan
                {
                    m_InterlockProvider = SetupGaugeInterlockProvider.Instance;

                    if (InterlockMethod == interlockMethod.PC)
                    {

                        m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 20.0, 40.0, 100.0, 150.0, 2, false);
                        m_InterlockProvider.InitFromDB(m_SetupInterlock);
                    }
                    else
                    {
                        m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 5.0, 40.0, 5.0, 10.0, 2, false);
                        m_InterlockProvider.InitFromDB(m_SetupInterlock);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_OldAdc = m_Ai.GetState();


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
                    short initValue = (short)(m_Info.AdcMin + 2000);
                    m_OldAdc = initValue;
                    m_Ai.SetState(initValue);
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
    public class Gauge_Ec : Gauge
    {
        #region Fields
        private SlaveAnalogInput m_Ai = new SlaveAnalogInput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public SlaveAnalogInput AnalogInput
        {
            get { return m_Ai; }
            set { m_Ai = value; }
        }
        #endregion

        #region Constructor
        public Gauge_Ec()
        {
            this.Name = "__ Gauge";
        }
        #endregion

        #region Methods
        protected override short GetValue()
        {
            return m_Ai.GetState();
        }

        /// <summary>
        /// Reading GaugeInfo from Storage
        /// if items is exist
        /// then get from storage;
        /// else get default value by gauge type,
        /// </summary>
        /// <returns></returns>
        //public bool InitFromStorage(TagCalibrationInfo info)
        //{
        //    return m_CalibrationProvider.InitFromDB(info);
        //}
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
            ok &= (m_Ai != null);


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
                if (m_InterlockEnable)
                {
                    ALM_LowerAlarm = new Alarm(this.Name + " Lower Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_LowerWarning = new Alarm(this.Name + " Lower Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                    ALM_UpperWarning = new Alarm(this.Name + " Upper Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                    ALM_UpperAlarm = new Alarm(this.Name + " Upper Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_CalibrationProvider = CalibrationProvider.Instance;
                m_Info = new TagCalibrationInfo(m_GaugeType, this.Name);
                m_CalibrationProvider.InitFromDB(m_Info);

                if (m_InterlockEnable) // 11.02.01 minhan
                {
                    m_InterlockProvider = SetupGaugeInterlockProvider.Instance;

                    if (InterlockMethod == interlockMethod.PC)
                    {

                        m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 20.0, 40.0, 100.0, 150.0, 2, false);
                        m_InterlockProvider.InitFromDB(m_SetupInterlock);
                    }
                    else
                    {
                        m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 5.0, 40.0, 5.0, 10.0, 2, false);
                        m_InterlockProvider.InitFromDB(m_SetupInterlock);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_OldAdc = m_Ai.GetState();


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
                    short initValue = (short)(m_Info.AdcMin + 2000);
                    m_OldAdc = initValue;
                    m_Ai.SetState(initValue);
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
    public class Gauge_Ap : Gauge
    {
        #region Fields
        private _DevicePeer m_Peer;
        #endregion

        #region Properties
        [Category("DMS : Setting"), ReadOnly(true)]
        [XmlIgnore()]
        public _DevicePeer Peer
        {
            get { return m_Peer; }
            set { m_Peer = value; }
        }
        #endregion

        #region Constructor
        public Gauge_Ap()
        {
            m_Name = "__ Gauge";
        }
        #endregion

        #region Methods
        protected override short GetValue()
        {
            switch (m_Info.Type)
            {
                case GaugeType.AP_DIW_Flow:
                    if (m_Peer.PeerType != PeerType.DIW) return 0;
                    return ((FlowmeterDIW)m_Peer).GetFlowValue();
                case GaugeType.AP_DIW_Press:
                    if (m_Peer.PeerType != PeerType.DIW) return 0;
                    return ((FlowmeterDIW)m_Peer).GetPressValue();

                case GaugeType.AP_CDA_Flow:
                    if (m_Peer.PeerType != PeerType.CDA) return 0;
                    return ((FlowmeterCDA)m_Peer).GetFlowValue();
                case GaugeType.AP_CDA_Press:
                    if (m_Peer.PeerType != PeerType.CDA) return 0;
                    return ((FlowmeterCDA)m_Peer).GetPressValue();

                case GaugeType.AP_LFC_Flow:
                    if (m_Peer.PeerType != PeerType.LFC) return 0;
                    return (short)((LFC)m_Peer).GetFlowValue();
                case GaugeType.AP_LFC_Press:
                    if (m_Peer.PeerType != PeerType.LFC) return 0;
                    return (short)((LFC)m_Peer).GetPressValue();
                case GaugeType.AP_LFC_OpenRate:
                    if (m_Peer.PeerType != PeerType.LFC) return 0;
                    return (short)((LFC)m_Peer).GetCurrentOpenRate();

                case GaugeType.AP_Manometer_Exhaust:
                    if (m_Peer.PeerType != PeerType.Manometer) return 0;
                    return ((Manometer)m_Peer).GetExhaustValue();

                case GaugeType.AP_LCT_Level1:
                    if (m_Peer.PeerType != PeerType.LCT) return 0;
                    return (short)((LCT)m_Peer).GetLevel1Value();
                case GaugeType.AP_LCT_Level2:
                    if (m_Peer.PeerType != PeerType.LCT) return 0;
                    return (short)((LCT)m_Peer).GetLevel2Value();
                case GaugeType.AP_LCT_Consistence:
                    if (m_Peer.PeerType != PeerType.LCT) return 0;
                    return (short)((LCT)m_Peer).GetConsistenceValue();

                default:
                    return 0;
            }
        }
        #endregion

        #region Overrides
        public override DmsErrors Initialize()
        {////////////////////////////////////////////////////////////////////////////////////////
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
            ok &= (m_Peer != null);
            if (ok) ok &= (m_Peer as Manometer) != null;


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
                if (m_InterlockEnable)
                {
                    ALM_LowerAlarm = new Alarm(this.Name + " Lower Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_LowerWarning = new Alarm(this.Name + " Lower Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                    ALM_UpperWarning = new Alarm(this.Name + " Upper Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                    ALM_UpperAlarm = new Alarm(this.Name + " Upper Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_CalibrationProvider = CalibrationProvider.Instance;
                m_Info = new TagCalibrationInfo(m_GaugeType, this.Name);
                m_CalibrationProvider.InitFromDB(m_Info);

                if (m_InterlockEnable) // 11.02.01 minhan
                {
                    m_InterlockProvider = SetupGaugeInterlockProvider.Instance;

                    if (InterlockMethod == interlockMethod.PC)
                    {

                        m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 20.0, 40.0, 100.0, 150.0, 2, false);
                        m_InterlockProvider.InitFromDB(m_SetupInterlock);
                    }
                    else
                    {
                        m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 5.0, 40.0, 5.0, 10.0, 2, false);
                        m_InterlockProvider.InitFromDB(m_SetupInterlock);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_OldAdc = GetValue();


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
                    short initValue = (short)(m_Info.AdcMin + 2000);
                    m_OldAdc = initValue;
                    //m_Ai.SetState(initValue);
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
}
