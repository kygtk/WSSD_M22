///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05.25
// Author       : eun
// Description  : Mixture Chemical Supply Pump Unit
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    public class MixSupplyPumpUnit : _DeviceAsm
    {
        #region Fields
        private TagPumpIfFlag m_IfFlag = null;
        private Pump m_Pump = null;
        //private MixTankUnit m_TankUnit = null;
        private _GenericCollection<MixTankUnit> m_TankUnits = new _GenericCollection<MixTankUnit>();
        private PumpAct m_RefAct = PumpAct.Stop;
        private PumpAct m_ManualAct;
        public Alarm ALM_PumpInterlockAlarm = null;
        private static TagSetupInfo m_SetupPumpInterlockTime = null;
        private static TagSetupInfo m_SetupPumpInterlockFlowRate = null;
        private DetergentUnit m_DetUnit = null;
        #endregion

        #region Properties
        [Category("Setting")]
        public Pump Pump
        {
            get { return m_Pump; }
            set { m_Pump = value; }
        }
        [Category("Setting")]
        public _GenericCollection<MixTankUnit> MixTankUnits
        {
            get { return m_TankUnits; }
            set
            {
                m_TankUnits = value;
                int count = m_TankUnits.Count;
                for (int i = 0; i < count; i++)
                {
                    m_TankUnits[i].OwnerPump = m_Pump;
                }
            }
        }
        //[Category("Setting")]
        [Browsable(false), XmlIgnore()]
        public MixTankUnit MixTank
        {
            get
            {
                if (m_DetUnit == null) return null;
                else return m_DetUnit.CurTankUnit;
            }
        }
        [Browsable(false), XmlIgnore()]
        public PumpAct RefAct
        {
            get { return m_RefAct; }
            set { m_RefAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public PumpAct ManualAct
        {
            get { return m_ManualAct; }
            set { m_ManualAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupInfoPumpInterlockTime
        {
            get { return m_SetupPumpInterlockTime; }
            set { m_SetupPumpInterlockTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupInfoPumpInterlockFlowRate
        {
            get { return m_SetupPumpInterlockFlowRate; }
            set { m_SetupPumpInterlockFlowRate = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagPumpIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        //[Browsable(false), XmlIgnore()]
        [Category("Setting")]
        public DetergentUnit DetUnit
        {
            get { return m_DetUnit; }
            set { m_DetUnit = value; }
        }
        #endregion

        #region Constructor
        public MixSupplyPumpUnit()
        {
            this.Name = "__ Mix Supply Pump Unit";
        }
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

            log = string.Format("MixSupplyPumpUnit\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;

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
                if (null != this.Pump)
                {
                    ALM_PumpInterlockAlarm = new Alarm(this.Name + " Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                if (null != this.Pump)
                {
                    if (null == m_SetupPumpInterlockTime)
                    {
                        m_SetupPumpInterlockTime = new TagSetupInfo("Pump Interlock Check Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "3");
                        SetupGenInfoProvider.Instance.InitFromDB(m_SetupPumpInterlockTime);
                    }
                    if (null == m_SetupPumpInterlockFlowRate)
                    {
                        m_SetupPumpInterlockFlowRate = new TagSetupInfo("Pump Interlock Flow Rate", OptionType.None, OptionFormat.Digit, UnitType.lpm, "5");
                        SetupGenInfoProvider.Instance.InitFromDB(m_SetupPumpInterlockFlowRate);
                    }
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagPumpIfFlag();
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

        }

        public override void UpdateTag()
        {

        }
        #endregion
    }
}
