using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    #region Enum
    public enum HeaterStatus
    {
        NoUse,
        NotReady,
        Ready,
        Heating,
        Alarm,
        Off
    }
    #endregion

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class HeaterUnit : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorHeaterUnit tagDescriptor = new TagDescriptorHeaterUnit();
        #endregion

        #region Fields
        private ST540 m_Tic = null;
        private Heater m_Heater = null;
        private TankLevel m_ILevel = null;
        private AutoValve m_ChillerValve = null;

        private HeaterStatus m_HeaterStatus = HeaterStatus.NotReady;
        private HeaterAct m_RefAct = HeaterAct.Off;
        private HeaterAct m_ManualAct = HeaterAct.Off;
        private int m_ManualTemp = 0;
        private int[] m_CurTemp = null; 

        public Alarm ALM_OVER_TEMP = null;
        private TagSetupInfo m_SetupHeaterUse = null;
        #endregion

        #region Properties
        [Category("Setting")]
        public Heater Heater
        {
            get { return m_Heater; }
            set { m_Heater = value; }
        }
        [Category("Setting")]
        public ST540 Tic
        {
            get { return m_Tic; }
            set { m_Tic = value; }
        }
        [Category("Setting")]
        public TankLevel TankLevel
        {
            get { return m_ILevel; }
            set { m_ILevel = value; }
        }
        [Category("Setting")]
        public AutoValve ChillerValve
        {
            get { return m_ChillerValve; }
            set { m_ChillerValve = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHeaterUse
        {
            get { return m_SetupHeaterUse; }
            set { m_SetupHeaterUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public HeaterAct ManualAct
        {
            get { return m_ManualAct; }
            set { m_ManualAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public HeaterAct RefAct
        {
            get { return m_RefAct; }
            set { m_RefAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int ManualTemp
        {
            get { return m_ManualTemp; }
            set { m_ManualTemp = value; }
        }
        public HeaterStatus HeaterStatus
        {
            get { return m_HeaterStatus; }
            set { m_HeaterStatus = value; }
        }
        #endregion

        #region Constructor
        public HeaterUnit()
        {
            this.Name = "__ Heater Unit";
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

            log = string.Format("HeaterUnit\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;

        }

        public void SetTemperature(int address, int temp)
        {
            m_Tic.SetTemperature(address, temp);
        }
        #endregion 

        #region Override
        public override Type FamilyType
        {
            get { return typeof(HeaterUnit); }
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
                ALM_OVER_TEMP = new Alarm(this.Name + "Over Temp Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_CurTemp = new Int32[m_Tic.MonitorNo];


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
            m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            m_Tag.SetValue(tagDescriptor.STATUS, m_HeaterStatus);

            if (m_Tic != null)
            {
                string tempList = "";
                for (int i = 0; i < m_Tic.MonitorNo; i++)
                {
                    tempList += "0" + "*";
                }

                m_Tag.SetValue(tagDescriptor.TEMP_LIST, tempList);
            }
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.STATUS, HeaterStatus);

            if (m_Tic != null)
            {
                string tempList = "";
                for (int i = 0; i < m_Tic.MonitorNo; i++)
                {
                    tempList += m_Tic.GetPvValue(i).ToString() + "*";
                }

                m_Tag.SetValue(tagDescriptor.TEMP_LIST, tempList);
            }
        }
        #endregion       
    }
}
