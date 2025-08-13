///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : eun
// Description  : PSM AP Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Reflection;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class TagApIfFlag
    {
        #region Fields
        private bool powerOn;
        private bool powerOff;
        private bool monitorAlarm;
        private PSMAp m_Parent;
        #endregion

        #region Properties
        public bool PowerOn
        {
            get { return powerOn; }
            set { powerOn = value; }
        }
        public bool PowerOff
        {
            get { return powerOff; }
            set { powerOff = value; }
        }
        public bool MonitorAlarm
        {
            get { return monitorAlarm; }
            set { monitorAlarm = value; }
        }
        #endregion

        #region Constructor
        public TagApIfFlag()
        {
        }

        public TagApIfFlag(PSMAp ap)
        {
            m_Parent = ap;
        }
        #endregion

        #region Methods
        public void Reset()
        {
            powerOn = false;
            powerOff = false;
            monitorAlarm = false;
        }
        #endregion
    }
    public delegate bool GetApUseConditionDelegate(PSMAp ap);
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PSMAp : _Plasma
    {
        #region Fields
        //Power Supply
        private IoDigitalInput m_DiPowerReady = new IoDigitalInput();
        private IoDigitalInput m_DiAlarmStatus0 = new IoDigitalInput();
        private IoDigitalInput m_DiAlarmStatus1 = new IoDigitalInput();
        private IoDigitalInput m_DiAlarmStatus2 = new IoDigitalInput();
        private IoDigitalOutput m_DoPowerOn = new IoDigitalOutput();

        //Flow
        private IoDigitalInput m_DiN2FlowLimitAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiCDAFlowLimitAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiUpExhaustAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiLowExhaustAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiPCWFlowLimitAlarm = new IoDigitalInput();
        private Gauge m_GaugePCW;

        //Color Board
        private ColorBoard m_ColorBoard = null; //Option

        //House
        private IoDigitalInput m_DiHouseClose = new IoDigitalInput();
        private IoDigitalInput m_DiHouseClose2 = new IoDigitalInput();

        private bool m_IsAlarm;
        private TagSetupInfo m_SetupApUse;
        private TagSetupInfo m_SetupApPowerSet;
        private TagSetupInfo m_SetupApN2FlowSet;
        private TagSetupInfo m_SetupApCDAFlowSet;
        private TagSetupInfo m_SetupAlarmIntrMargin;
        private TagSetupInfo m_SetupWarningIntrMargin;
        private TagSetupInfo m_SetupOnWaitTime;         //MFC Set하고 나서 Power On하기 전까지의 Margin Time
        public Alarm ALM_PowerNotReady = null;
        public Alarm ALM_HouseOpen = null;
        public Alarm ALM_InterlockOpen = null;
        public Alarm ALM_NotDefined = null;
        public Alarm ALM_LowVoltage = null;
        public Alarm ALM_SystemError = null;
        public Alarm ALM_OutputOpen = null;
        public Alarm ALM_LocalMode = null;
        public Alarm ALM_ArcTrip = null;
        private CvUnit m_Owner = null;
        private TagApIfFlag m_IfFlag = null;
        private TagSetupInfo m_SetupPCWIntrFlow;        //4L이상이어야 함.
                                                        //private bool m_CreateSetupParameter = false; //ControlBy Option으로 대체
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiPowerReady
        {
            get { return m_DiPowerReady; }
            set { m_DiPowerReady = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarmStatus0
        {
            get { return m_DiAlarmStatus0; }
            set { m_DiAlarmStatus0 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarmStatus1
        {
            get { return m_DiAlarmStatus1; }
            set { m_DiAlarmStatus1 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarmStatus2
        {
            get { return m_DiAlarmStatus2; }
            set { m_DiAlarmStatus2 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiN2FlowLimitAlarm
        {
            get { return m_DiN2FlowLimitAlarm; }
            set { m_DiN2FlowLimitAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiCDAFlowLimitAlarm
        {
            get { return m_DiCDAFlowLimitAlarm; }
            set { m_DiCDAFlowLimitAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiPCWFlowLimitAlarm
        {
            get { return m_DiPCWFlowLimitAlarm; }
            set { m_DiPCWFlowLimitAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiUpExhaustAlarm
        {
            get { return m_DiUpExhaustAlarm; }
            set { m_DiUpExhaustAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiLowExhaustAlarm
        {
            get { return m_DiLowExhaustAlarm; }
            set { m_DiLowExhaustAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiHouseClose2
        {
            get { return m_DiHouseClose2; }
            set { m_DiHouseClose2 = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiHouseClose
        {
            get { return m_DiHouseClose; }
            set { m_DiHouseClose = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoPowerSupplyOn
        {
            get { return m_DoPowerOn; }
            set { m_DoPowerOn = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugePCW
        {
            get { return m_GaugePCW; }
            set { m_GaugePCW = value; }
        }
        [Category("DMS : Setting")]
        public ColorBoard ColorBoard
        {
            get { return m_ColorBoard; }
            set { m_ColorBoard = value; }
        }
        [Category("DMS : Setting")]
        public CvUnit OwnerUnit
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        //ControlBy Option으로 대체
        //[Category("DMS : Setting")]
        //public bool CreateSetupParameter
        //{
        //    get { return m_CreateSetupParameter; }
        //    set { m_CreateSetupParameter = value; }
        //}
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupAlarmIntrMargin
        {
            get { return m_SetupAlarmIntrMargin; }
            set { m_SetupAlarmIntrMargin = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupWarningIntrMargin
        {
            get { return m_SetupWarningIntrMargin; }
            set { m_SetupWarningIntrMargin = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupPowerOnWaitTime
        {
            get { return m_SetupOnWaitTime; }
            set { m_SetupOnWaitTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagApIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupApUse
        {
            get { return m_SetupApUse; }
            set { m_SetupApUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupApPowerSet
        {
            get { return m_SetupApPowerSet; }
            set { m_SetupApPowerSet = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupApN2FlowSet
        {
            get { return m_SetupApN2FlowSet; }
            set { m_SetupApN2FlowSet = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupApCDAFlowSet
        {
            get { return m_SetupApCDAFlowSet; }
            set { m_SetupApCDAFlowSet = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupPCWIntrFlow
        {
            get { return m_SetupPCWIntrFlow; }
            set { m_SetupPCWIntrFlow = value; }
        }
        #endregion

        #region Constructor
        public PSMAp()
        {
            this.Name = "__ AP Plasma Unit";
        }
        #endregion

        #region Methods
        public bool IsStatusAlarm()
        {
            ApAlarmStatus status = 0;
            status |= m_DiAlarmStatus0.GetState() ? ApAlarmStatus.status0 : ApAlarmStatus.none;
            status |= m_DiAlarmStatus1.GetState() ? ApAlarmStatus.status1 : ApAlarmStatus.none;
            status |= m_DiAlarmStatus2.GetState() ? ApAlarmStatus.status2 : ApAlarmStatus.none;

            if (status < ApAlarmStatus.normal) return true;
            else return false;
        }

        public int GetStatusAlarm()
        {
            int value = 0;
            if (m_DiAlarmStatus0.GetState()) value |= (int)ApAlarmStatus.status0;
            if (m_DiAlarmStatus1.GetState()) value |= (int)ApAlarmStatus.status1;
            if (m_DiAlarmStatus2.GetState()) value |= (int)ApAlarmStatus.status2;
            return value;
        }

        public bool IsPowerReady()
        {
            return m_DiPowerReady.GetState();
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

            log = string.Format("ApPlasma\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override void PowerOn()
        {
            if (!this.Initialized) return;

            if (!IsPowerOn())
            {
                m_DoPowerOn.SetState(true);
            }
        }

        public override void PowerOff()
        {
            if (!this.Initialized) return;

            if (!IsPowerOff())
            {
                m_DoPowerOn.SetState(false);
            }
        }

        public override bool IsPowerOn()
        {
            if (!this.Initialized) return false;

            return m_DoPowerOn.GetState();
        }

        public override bool IsPowerOff()
        {
            if (!this.Initialized) return false;

            bool bOn = IsPowerOn();

            return !bOn;
        }

        public override void SetVoltage(double voltage)
        {
            //short diffAdc = (short)(m_AoAdcMax - m_AoAdcMin);
            //double diffReal = m_AoRealMax - m_AoRealMin;

            //ushort value = (ushort)(((voltage - m_AoRealMin) * ((double)diffAdc / diffReal)) + (double)m_AoAdcMin);

            //m_AoVoltageSet.SetState(value);
            m_MfcVoltage.SetFlow(voltage);
        }

        public override void SetN2Flow(double flow)
        {
            m_MfcN2.SetFlow(flow);
        }

        public override void SetCDAFlow(double flow)
        {
            m_MfcCDA.SetFlow(flow);
        }

        public override bool IsInterlockCondition()
        {
            if (m_IsPowerOnInterlockCondition == null)
            {
                return false;
            }
            else
            {
                return m_IsPowerOnInterlockCondition(this);
            }
        }

        public override bool IsUse()
        {
            if (m_ControlBy == ProcessControlBy.Setup)
            {
                return m_SetupApUse.GetValue<bool>();
            }
            else
            {
                return m_Server.JobCond.ApUse(this);
            }
        }

        public bool IsChamberClose()
        {
            bool ok = true;
            ok &= m_DiHouseClose.GetState();
            ok &= (m_DiHouseClose2 != null) ? m_DiHouseClose2.GetState() : true;
            return true;
        }

        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////


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
            ok &= (m_DiPowerReady != null);
            ok &= (m_DiN2FlowLimitAlarm != null);
            ok &= (m_DiCDAFlowLimitAlarm != null);
            ok &= (m_DiAlarmStatus0 != null);
            ok &= (m_DiAlarmStatus1 != null);
            ok &= (m_DiAlarmStatus2 != null);
            ok &= (m_DiHouseClose != null);
            ok &= (m_DoPowerOn != null);
            ok &= (m_GaugePCW != null);
            ok &= (m_MfcCDA != null);
            ok &= (m_MfcN2 != null);
            ok &= (m_MfcVoltage != null);
            ok &= (m_ActuatorUnit != null);


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
                ALM_PowerNotReady = new Alarm(this.Name + " Power Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HouseOpen = new Alarm(this.Name + " House Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_InterlockOpen = new Alarm(this.Name + " Interlock Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_NotDefined = new Alarm(this.Name + " Not Defined Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LowVoltage = new Alarm(this.Name + " Low Voltage Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_SystemError = new Alarm(this.Name + " System Error Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_OutputOpen = new Alarm(this.Name + " Output Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LocalMode = new Alarm(this.Name + " Local Mode Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ArcTrip = new Alarm(this.Name + " ARC Trip Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);



                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                if (m_ControlBy == ProcessControlBy.Setup)
                {
                    m_SetupApUse = new TagSetupInfo(this.Name + " Use", OptionType.None, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    setupGenInfoProvider.InitFromDB(m_SetupApUse);
                    m_SetupApPowerSet = new TagSetupInfo(this.Name + " Power Setting", OptionType.None, OptionFormat.Float, UnitType.kV, "10.0");
                    setupGenInfoProvider.InitFromDB(m_SetupApPowerSet);
                    m_SetupApN2FlowSet = new TagSetupInfo(this.Name + " N2 Flow Setting", OptionType.None, OptionFormat.Float, UnitType.lpm, "10.0");
                    setupGenInfoProvider.InitFromDB(m_SetupApN2FlowSet);
                    m_SetupApCDAFlowSet = new TagSetupInfo(this.Name + " CDA Flow Setting", OptionType.None, OptionFormat.Float, UnitType.lpm, "10.0");
                    setupGenInfoProvider.InitFromDB(m_SetupApCDAFlowSet);
                }
                m_SetupAlarmIntrMargin = new TagSetupInfo(this.Name + " Alarm Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "10.0");
                setupGenInfoProvider.InitFromDB(m_SetupAlarmIntrMargin);
                m_SetupWarningIntrMargin = new TagSetupInfo(this.Name + " Warning Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "5.0");
                setupGenInfoProvider.InitFromDB(m_SetupWarningIntrMargin);
                m_SetupOnWaitTime = new TagSetupInfo(this.Name + " Power On Wait Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "2");
                setupGenInfoProvider.InitFromDB(m_SetupOnWaitTime);
                m_SetupPCWIntrFlow = new TagSetupInfo(this.Name + " PCW Interlock Flow", OptionType.None, OptionFormat.Float, UnitType.L, "4.0");
                setupGenInfoProvider.InitFromDB(m_SetupPCWIntrFlow);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagApIfFlag(this);
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
                if (m_Simul.Device)
                {
                    m_DiAlarmStatus0.SetState(true);
                    m_DiAlarmStatus1.SetState(true);
                    m_DiAlarmStatus2.SetState(true);
                    m_DiPowerReady.SetState(true);
                    m_DiHouseClose.SetState(true);
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
            if (m_Tag != null)
            {
                m_Tag.SetValue(tagDescriptor.ON, IsPowerOn());
                m_Tag.SetValue(tagDescriptor.ALARM, m_IsAlarm);
                m_Tag.SetValue(tagDescriptor.USE, IsUse());
            }
        }
        #endregion
    }
}
