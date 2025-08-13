///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.10
// Author       : Hoon
// Description  : Measure Tank
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Dms.Device
{
    public interface ICtlDetTank
    {
        bool IsTankReady();
        void SetOwnerPump(PumpUnit unit);
        bool IsLevelFault();
        bool IsRunEnableLevel();
    }

    #region TagDetIfFlag
    public class TagDetTankIfFlag
    {
        public bool DetTankReady;
        public bool TankLevelFault;

        public void Reset()
        {
            DetTankReady = false;
            TankLevelFault = false;
        }
    }

    #endregion

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DetTankUnit : _DeviceAsm, ICtlDetTank
    {
        #region Tag Descriptor
        protected TagDescriptorTankUnit tagDescriptor = new TagDescriptorTankUnit();
        #endregion

        #region Fields
        private AutoValve m_AutoValve = null;
        private TankLevel m_ILevel = null;
        //private ITankLevel m_ILevel = null;
        private Pump m_Pump = null;
        public Alarm ALM_DetTankLowLevel = null;
        public Alarm ALM_DetTankLowAlarmLevel = null;
        public Alarm ALM_LevelFault = null;
        protected TagDetTankIfFlag m_DetIfFlag = null;
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public AutoValve AutoValve
        {
            get { return m_AutoValve; }
            set { m_AutoValve = value; }
        }
        [Category("DMS : Relation")]
        public TankLevel TankLevel
        {
            get { return m_ILevel; }
            set { m_ILevel = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagDetTankIfFlag DetIfFlag
        {
            get { return m_DetIfFlag; }
            set { m_DetIfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool LevelFault
        {
            get { return DetIfFlag.TankLevelFault; }
        }
        [Browsable(false), XmlIgnore()]
        public bool TopLevel
        {
            get { return m_ILevel.TopLevelConfirm == LevelConfirm.Confirm; }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyRequest
        {
            get { return m_ILevel.SupplyRequestConfirm == LevelConfirm.Confirm; }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyStop
        {
            get { return m_ILevel.SupplyStopConfirm == LevelConfirm.Confirm; }
        }
        [Browsable(false), XmlIgnore()]
        public bool BottomLevel
        {
            get { return m_ILevel.BottomLevelConfirm == LevelConfirm.Confirm; }
        }
        [Browsable(false), XmlIgnore()]
        public bool RunEnable
        {
            get { return m_ILevel.RunEnableConfirm == LevelConfirm.Confirm; }
        }
        [Browsable(false), XmlIgnore()]
        public bool TopLevelDetect
        {
            get { return m_ILevel.TopLevelDetect == LevelDetect.Detect; }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyRequestDetect
        {
            get { return m_ILevel.SupplyRequestDetect == LevelDetect.Detect; }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyStopDetect
        {
            get { return m_ILevel.SupplyStopDetect == LevelDetect.Detect; }
        }
        [Browsable(false), XmlIgnore()]
        public bool BottomLevelDetect
        {
            get { return m_ILevel.BottomLevelDetect == LevelDetect.Detect; }
        }
        [Browsable(false), XmlIgnore()]
        public bool RunEnableDetect
        {
            get { return m_ILevel.RunEnableDetect == LevelDetect.Detect; }
        }
        [Browsable(false), XmlIgnore()]
        public Pump OwnerPump
        {
            get { return m_Pump; }
            set { m_Pump = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool PumpUse
        {
            get
            {
                if (m_Pump == null)
                {
                    return false;
                }
                else
                {
                    return m_Pump.IsUse;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool PumpRun
        {
            get
            {
                if (m_Pump == null)
                {
                    return false;
                }
                else
                {
                    return m_Pump.IsRun();
                }
            }
        }
        #endregion

        #region Constructor
        public DetTankUnit()
        {
            this.Name = "__ Tank";
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

            log = string.Format("DetTankUnit\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

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
                ALM_DetTankLowLevel = new Alarm(this.Name + " Low Level Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_DetTankLowAlarmLevel = new Alarm(this.Name + " Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LevelFault = new Alarm(this.Name + " Level Fault Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_DetIfFlag = new TagDetTankIfFlag();
                m_DetIfFlag.Reset();


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

        public override void HandleEvent(object sender, IoStateEventArgs e)
        {
            if (!this.Initialized) return;

            UpdateTag();

            //string msg = string.Format("{0} : Tank state change", this.Name);
            //m_Server.FireEvent(m_Tag, msg);
        }

        public override void SetSubscriber()
        {
            base.SetSubscriber();

            this.TankLevel.OnLevelSatateChanged += new IoStateChangeEventHandler(HandleEvent);
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
                int levelCount = (this.TankLevel == null) ? 0 : this.TankLevel.Count;
                m_Tag[tagDescriptor.LEVELS].Value = levelCount.ToString();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.TOPLEVEL, this.TopLevel);
            m_Tag.SetValue(tagDescriptor.SUPPLYSTOP, this.SupplyStop);
            m_Tag.SetValue(tagDescriptor.SUPPLYREQUEST, this.SupplyRequest);
            m_Tag.SetValue(tagDescriptor.RUNENABLE, this.RunEnable);
            m_Tag.SetValue(tagDescriptor.BOTTOMLEVEL, this.BottomLevel);
        }
        #endregion

        #region ICtlTank Member
        public bool IsTankReady()
        {
            return DetIfFlag.DetTankReady;
        }

        public void SetOwnerPump(PumpUnit unit)
        {
            m_Pump = unit.Pump;
        }

        public bool IsLevelFault()
        {
            return DetIfFlag.TankLevelFault;
        }

        public bool IsRunEnableLevel()
        {
            return (BottomLevel & RunEnable);
        }
        #endregion
    }
}
