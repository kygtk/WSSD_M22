using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using Dms.Util.IODefine;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Yaskawa_BoeHF_Theragen : _IndexRobot, IRobotSlave
    {
        #region Fields
        protected TagDescriptorLoaderRobot tagDescriptor = new TagDescriptorLoaderRobot();
        //protected _GenericCollection<PortUnit> m_PortUnit = new _GenericCollection<PortUnit>();
        //protected _GenericCollection<PortUnit> m_StageUnit = new _GenericCollection<PortUnit>();	// StageUnit Device 추가/수정 필요함
        protected int m_MaxSlotNo = 255;
        protected int m_MaxPortNo = 8;
        protected int m_MaxStageNo = 4;
        protected int m_MaxCubeNo;
        private string[] m_ThickKind = new string[] { "0.0T" };
        private string[] m_UsedPortName = new string[] { "Port1" };
        private string[] m_UsedStageName = new string[] { "Stage1" };
        private boeRbtCommandCode[] m_UsedManualPattern = new boeRbtCommandCode[] { boeRbtCommandCode.GET, };

        private int m_PatternNo = 0;
        private int[] m_UpperHandGlsData = new int[] { 0, 0 };
        private int[] m_LowerHandGlsData = new int[] { 0, 0 };
        //private nxOP_CUBE m_Cube;
        //private nxOP_HAND m_Hand;
        //private nxOP_PATTERN m_Pattern;

        private boeRbtOpDirection m_MaunalPrimaryPos;
        private boeRbtCommandCode m_ManualOperation;
        private boeRbtCommandArm m_ManualHand;
        private int m_ManualSlotNo = 0;
        private bool m_ManualMotionStrobeReq = false;
        private bool m_ManualHomeReturnReq = false;

        private bool m_IsRobotCommand = false;
        private bool m_bInitGetPutScrapSeq = false;

        // For Robot Sequence
        private Yaskawa_BoeHF_HomeSeq m_HomeSeq = null;
        private Yaskawa_BoeHF_GetPutGlassSeq m_GetPutSeq = null;
        #endregion

        #region Properties
        private int m_InterfaceTimeout = 60000;
        [Category("DMS : General Setting"), Description("Robot Command Interface Timeout Setting(msec)")]
        public int InterfaceTimeout
        {
            get { return m_InterfaceTimeout; }
            set { m_InterfaceTimeout = value; }
        }
        [Category("DMS : Setting"), Description("Maximum Slot No of Cassette for Indexer\nA difference setting is approved for each Fab")]
        public int MaxSlotNo
        {
            get { return m_MaxSlotNo; }
            set { m_MaxSlotNo = value; }
        }
        [Category("DMS : Setting"), Description("I/O Map Defined Maximum Port No\nThis must set by following Specification")]
        public int MaxPortNo
        {
            get { return m_MaxPortNo; }
            set { m_MaxPortNo = value; }
        }
        [Category("DMS : Setting"), Description("I/O Map Defined Maximum Stage No\nThis must set by following Specification")]
        public int MaxStageNo
        {
            get { return m_MaxStageNo; }
            set { m_MaxStageNo = value; }
        }
        [Category("DMS : Setting"), Description("I/O Map Defined Kind of Thickness\nSet Value [00.00T]")]
        public string[] ThickKind
        {
            get { return m_ThickKind; }
            set { m_ThickKind = value; }
        }
        [Category("DMS : Setting"), Description("Define Used Port name")]
        public string[] UsedPortName
        {
            get { return m_UsedPortName; }
            set { m_UsedPortName = value; }
        }
        [Category("DMS : Setting"), Description("Define Used Stage name")]
        public string[] UsedStageName
        {
            get { return m_UsedStageName; }
            set { m_UsedStageName = value; }
        }
        [Category("DMS : Setting"), Description("Define Used Manual Operation")]
        public boeRbtCommandCode[] UsedManualPattern
        {
            get { return m_UsedManualPattern; }
            set { m_UsedManualPattern = value; }
        }
        //[Category("DMS : I/O Setting - Robot Status")]


        [Browsable(false)]
        public int PatternNo
        {
            get { return m_PatternNo; }
        }

        [Browsable(false), XmlIgnore()]
        public bool IsRobotCommand
        {
            get { return m_IsRobotCommand; }
            set { m_IsRobotCommand = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool InitGetPutScrapSeq
        {
            get { return m_bInitGetPutScrapSeq; }
            set { m_bInitGetPutScrapSeq = value; }
        }

        [Browsable(false), XmlIgnore()]
        public boeRbtOpDirection MaunalPrimaryPos
        {
            get { return m_MaunalPrimaryPos; }
            set { m_MaunalPrimaryPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public boeRbtCommandCode ManualOperation
        {
            get { return m_ManualOperation; }
            set { m_ManualOperation = value; }
        }
        [Browsable(false), XmlIgnore()]
        public boeRbtCommandArm ManualHand
        {
            get { return m_ManualHand; }
            set { m_ManualHand = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int ManualSlotNo
        {
            get { return m_ManualSlotNo; }
            set { m_ManualSlotNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool ManualMotionStrobeReq
        {
            get { return m_ManualMotionStrobeReq; }
            set { m_ManualMotionStrobeReq = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool ManualHomeReturnReq
        {
            get { return m_ManualHomeReturnReq; }
            set { m_ManualHomeReturnReq = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int[] UpperHandGlsData
        {
            get { return m_UpperHandGlsData; }
            set { m_UpperHandGlsData = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int[] LowerHandGlsData
        {
            get { return m_LowerHandGlsData; }
            set { m_LowerHandGlsData = value; }
        }
        #endregion

        #region Robot I/O
        // Online, Offline Bit Add
        private IoDigitalInput m_mibLoaderOnline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLoaderOnline
        {
            get { return m_mibLoaderOnline; }
            set { m_mibLoaderOnline = value; }
        }
        private IoDigitalInput m_mibLoaderOffline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLoaderOffline
        {
            get { return m_mibLoaderOffline; }
            set { m_mibLoaderOffline = value; }
        }

        // I/O Setting - Bit Input
        private IoDigitalInput m_mibRobotCommandReadConfirm = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotCommandReadConfirm
        {
            get { return m_mibRobotCommandReadConfirm; }
            set { m_mibRobotCommandReadConfirm = value; }
        }
        private IoDigitalInput m_mibRobotCommandEndReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotCommandEndReport
        {
            get { return m_mibRobotCommandEndReport; }
            set { m_mibRobotCommandEndReport = value; }
        }
        private IoDigitalInput m_mibVcrReadRequest = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibVcrReadRequest
        {
            get { return m_mibVcrReadRequest; }
            set { m_mibVcrReadRequest = value; }
        }
        private IoDigitalInput m_mibRobotEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotEnable
        {
            get { return m_mibRobotEnable; }
            set { m_mibRobotEnable = value; }
        }
        private IoDigitalInput m_mibRobotOriginPosition = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotOriginPosition
        {
            get { return m_mibRobotOriginPosition; }
            set { m_mibRobotOriginPosition = value; }
        }
        private IoDigitalInput m_mibRobotBusy = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotBusy
        {
            get { return m_mibRobotBusy; }
            set { m_mibRobotBusy = value; }
        }
        private IoDigitalInput m_mibRobotPause = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotPause
        {
            get { return m_mibRobotPause; }
            set { m_mibRobotPause = value; }
        }
        private IoDigitalInput m_mibUpperHandGlassExist = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibUpperHandGlassExist
        {
            get { return m_mibUpperHandGlassExist; }
            set { m_mibUpperHandGlassExist = value; }
        }
        private IoDigitalInput m_mibLowerHandGlassExist = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLowerHandGlassExist
        {
            get { return m_mibLowerHandGlassExist; }
            set { m_mibLowerHandGlassExist = value; }
        }
        private IoDigitalInput m_mibRobotAlarm = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibRobotAlarm
        {
            get { return m_mibRobotAlarm; }
            set { m_mibRobotAlarm = value; }
        }
        private IoDigitalInput m_mibIFNormalStatus = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFNormalStatus
        {
            get { return m_mibIFNormalStatus; }
            set { m_mibIFNormalStatus = value; }
        }
        private IoDigitalInput m_mibIFGetReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFGetReady
        {
            get { return m_mibIFGetReady; }
            set { m_mibIFGetReady = value; }
        }
        private IoDigitalInput m_mibIFPutReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFPutReady
        {
            get { return m_mibIFPutReady; }
            set { m_mibIFPutReady = value; }
        }
        private IoDigitalInput m_mibIFExchangeReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFExchangeReady
        {
            get { return m_mibIFExchangeReady; }
            set { m_mibIFExchangeReady = value; }
        }
        private IoDigitalInput m_mibIFRobotBusy = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFRobotBusy
        {
            get { return m_mibIFRobotBusy; }
            set { m_mibIFRobotBusy = value; }
        }
        private IoDigitalInput m_mibIFRobotComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFRobotComplete
        {
            get { return m_mibIFRobotComplete; }
            set { m_mibIFRobotComplete = value; }
        }
        private IoDigitalInput m_mibIFEqpNormalStatus = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpNormalStatus
        {
            get { return m_mibIFEqpNormalStatus; }
            set { m_mibIFEqpNormalStatus = value; }
        }
        private IoDigitalInput m_mibIFEqpGetEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpGetEnable
        {
            get { return m_mibIFEqpGetEnable; }
            set { m_mibIFEqpGetEnable = value; }
        }
        private IoDigitalInput m_mibIFEqpPutEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpPutEnable
        {
            get { return m_mibIFEqpPutEnable; }
            set { m_mibIFEqpPutEnable = value; }
        }
        private IoDigitalInput m_mibIFEqpExchangeEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpExchangeEnable
        {
            get { return m_mibIFEqpExchangeEnable; }
            set { m_mibIFEqpExchangeEnable = value; }
        }
        private IoDigitalInput m_mibIFEqpGlassExist = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpGlassExist
        {
            get { return m_mibIFEqpGlassExist; }
            set { m_mibIFEqpGlassExist = value; }
        }
        // I/O Setting - Bit Output
        private IoDigitalOutput m_mobRobotCommandReadRequest = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobRobotCommandReadRequest
        {
            get { return m_mobRobotCommandReadRequest; }
            set { m_mobRobotCommandReadRequest = value; }
        }
        private IoDigitalOutput m_mobRobotCommandEndConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobRobotCommandEndConfirm
        {
            get { return m_mobRobotCommandEndConfirm; }
            set { m_mobRobotCommandEndConfirm = value; }
        }
        private IoDigitalOutput m_mobVcrReadConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobVcrReadConfirm
        {
            get { return m_mobVcrReadConfirm; }
            set { m_mobVcrReadConfirm = value; }
        }
        // I/O Setting - Word Input   
        private IoAnalogInput m_miwCommandCodeNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandCodeNo
        {
            get { return m_miwCommandCodeNo; }
            set { m_miwCommandCodeNo = value; }
        }
        private IoAnalogInput m_miwCommandArmNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandArmNo
        {
            get { return m_miwCommandArmNo; }
            set { m_miwCommandArmNo = value; }
        }
        private IoAnalogInput m_miwCommandTargetNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandTargetNo
        {
            get { return m_miwCommandTargetNo; }
            set { m_miwCommandTargetNo = value; }
        }
        private IoAnalogInput m_miwCommandPrimaryPos = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandPrimaryPos
        {
            get { return m_miwCommandPrimaryPos; }
            set { m_miwCommandPrimaryPos = value; }
        }
        private IoAnalogInput m_miwCommandSecondaryPos = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandSecondaryPos
        {
            get { return m_miwCommandSecondaryPos; }
            set { m_miwCommandSecondaryPos = value; }
        }
        private IoAnalogInput m_miwCommandResult = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandResult
        {
            get { return m_miwCommandResult; }
            set { m_miwCommandResult = value; }
        }
        // I/O Setting - Word Output
        private IoAnalogOutput m_mowCommandCodeNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandCodeNo
        {
            get { return m_mowCommandCodeNo; }
            set { m_mowCommandCodeNo = value; }
        }
        private IoAnalogOutput m_mowCommandArmNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandArmNo
        {
            get { return m_mowCommandArmNo; }
            set { m_mowCommandArmNo = value; }
        }
        private IoAnalogOutput m_mowCommandTargetNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandTargetNo
        {
            get { return m_mowCommandTargetNo; }
            set { m_mowCommandTargetNo = value; }
        }
        private IoAnalogOutput m_mowCommandPrimaryPos = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandPrimaryPos
        {
            get { return m_mowCommandPrimaryPos; }
            set { m_mowCommandPrimaryPos = value; }
        }
        private IoAnalogOutput m_mowCommandSecondaryPos = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandSecondaryPos
        {
            get { return m_mowCommandSecondaryPos; }
            set { m_mowCommandSecondaryPos = value; }
        }
        #endregion

        #region Constructure
        public Yaskawa_BoeHF_Theragen()
        {
            this.Name = "__ Yaskawa Robot";
            m_UpperHandCode = (int)boeRbtCommandArm.UPPER_HAND;
            m_LowerHandCode = (int)boeRbtCommandArm.LOWER_HAND;
        }
        #endregion

        #region _IndexRobot Abstract override
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            bool alarm = false;
            alarm |= m_mibRobotAlarm.GetState();

            return alarm;
        }

        public override int GetSlotNo()
        {
            int slotNo = 0;
            //int itemsCount = m_DiSlotNoBits.Count;

            //for (int i = 0; i < itemsCount; i++)
            //{
            //    slotNo |= ((m_DiSlotNoBits[i].GetState() ? 1 : 0) << i);
            //}

            return slotNo;
        }

        public override int GetOperationHand()
        {
            //            int armNo = (int)m_miwCommandArmNo.GetState();
            int armNo = (int)m_mowCommandArmNo.GetState();

            return (armNo == 1) ? (int)nxOP_HAND.UPPER_HAND : (int)nxOP_HAND.LOWER_HAND;
        }

        public override RobotSelectedMode GetModeSelected()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsUpperHandGlassDetected()
        {
            if (!this.Initialized) return false;

            bool bRv = true;
            bRv &= m_mibUpperHandGlassExist.GetState();

            return bRv;
        }

        // Only for simulation
        public void SetUpperHandGlassDetected(bool on)
        {
            if (m_Simul.Loader)
            {
                m_mibUpperHandGlassExist.SetState(on);
            }
        }

        public override bool IsLowerHandGlassDetected()
        {
            if (!this.Initialized) return false;

            bool bRv = true;
            bRv &= m_mibLowerHandGlassExist.GetState();

            return bRv;
        }

        // Only for simulation
        public void SetLowerHandGlassDetected(bool on)
        {
            if (m_Simul.Loader)
            {
                m_mibLowerHandGlassExist.SetState(on);
            }
        }

        public override bool IsHandGlassDetected(LoaderRobotHand hand)
        {
            if (hand == LoaderRobotHand.upperHand)
            {
                return IsUpperHandGlassDetected();
            }
            else
            {
                return IsLowerHandGlassDetected();
            }
        }

        public override bool IsHandGlassEmpty(LoaderRobotHand hand)
        {
            bool rv = true;
            if (hand == LoaderRobotHand.upperHand)
            {
                rv &= !m_mibUpperHandGlassExist.GetState();
            }
            else
            {
                rv &= !m_mibLowerHandGlassExist.GetState();
            }
            return rv;
        }

        // Only for simulation
        public void SetHandGlassDetected(LoaderRobotHand hand, bool on)
        {
            if (m_Simul.Loader)
            {
                if (hand == LoaderRobotHand.upperHand)
                {
                    SetUpperHandGlassDetected(on);
                }
                else
                {
                    SetLowerHandGlassDetected(on);
                }
            }
        }

        public override bool IsRobotReady()
        {
            bool ready = true;

            if (Simul.Loader == false)
            {
                ready &= m_mibRobotEnable.GetState();
                ready &= !m_mibRobotBusy.GetState();
                ready &= !m_mibRobotPause.GetState();
                ready &= !m_mibRobotAlarm.GetState();
            }

            return ready;
        }

        public override bool IsRobotRunning()
        {
            bool ready = true;

            if (Simul.Device == false)
            {
                ready &= m_mibRobotEnable.GetState();
                ready &= m_mibRobotBusy.GetState();
            }

            return ready;
        }

        public override bool IsRobotServoOn()
        {
            bool ready = true;

            if (Simul.Device == false)
            {
                ready &= m_mibRobotEnable.GetState();
            }

            return ready;
        }

        public override bool IsRobotHandHome()
        {
            bool handHome = true;
            if (Simul.Loader == false)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            return handHome;
        }

        public override bool IsRobotEmoAlarm()
        {
            bool emoAlarm = false;
            if (Simul.Loader == false)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            return emoAlarm;
        }

        public override bool IsRobotHold()
        {
            bool robotHold = false;
            if (Simul.Loader == false)
            {
                robotHold |= m_mibRobotPause.GetState();
            }
            return robotHold;
        }

        public override bool IsOperationBegin()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsRobotBusy()
        {
            bool busy = false;

            busy |= m_mibRobotBusy.GetState();

            return busy;
        }

        public override void SetExternalStart(bool on)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetRobotPause(bool pause)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetGlassKind(int kindNo)					// Index[0~15]를 첨자로 하는 Thickness Array의 해당하는 Output Bit를 Set
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetSlotSelection(int slotNo)
        {
            m_mowCommandSecondaryPos.SetState((ushort)slotNo);
        }

        public override int GetSelectedSlotNo()
        {
            ushort slotno;

            slotno = m_mowCommandSecondaryPos.GetState();

            return (int)slotno;
        }

        public override void SetExternalSpeedRatio()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetGlassThickness()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetAdsorptionTime()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override RobotCommandResult SetRobotAction(RobotActionType act, int portNo, int slotNo)
        {
            return RobotCommandResult.accept;
        }

        public override bool GetGlassOriginalPosition(LoaderRobotHand hand, ref int portId, ref int slotId)
        {
            RobotHandStage robotHandStage = null;
            if (hand == LoaderRobotHand.upperHand)
            {
                robotHandStage = m_UpperHandStage;
            }
            else if (hand == LoaderRobotHand.lowerHand)
            {
                robotHandStage = m_LowerHandStage;
            }

            if (robotHandStage == null || !robotHandStage.GlassExist) return false;

            portId = robotHandStage.Glass.OrgPortNo - 1;
            slotId = robotHandStage.Glass.OrgSlotNo - 1;

            if (portId < 0 || slotId < 0)
                return false;
            return true;
        }

        public override bool GetGlassTargetPosition(LoaderRobotHand hand, ref int portId, ref int slotId)
        {
            RobotHandStage robotHandStage = null;
            if (hand == LoaderRobotHand.upperHand)
            {
                robotHandStage = m_UpperHandStage;
            }
            else if (hand == LoaderRobotHand.lowerHand)
            {
                robotHandStage = m_LowerHandStage;
            }

            if (robotHandStage == null || !robotHandStage.GlassExist) return false;

            portId = robotHandStage.Glass.TargetPortNo - 1;
            slotId = robotHandStage.Glass.TargetSlotNo - 1;

            if (portId < 0 || slotId < 0)
                return false;
            return true;
        }

        public override bool SetGlassOriginalPosition(LoaderRobotHand hand, int portId, int slotId)
        {
            // before set the glass data you should check glass sensor status 
            if (hand == LoaderRobotHand.upperHand && /*m_DiUpperGlassDetectConfirm.GetState() == true &&*/
                UpperHandStage.GlassExist)
            {
                UpperHandStage.Glass.OrgPortNo = portId + 1;
                UpperHandStage.Glass.OrgSlotNo = slotId + 1;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && /*m_DiLowerGlassDetectConfirm.GetState() == true &&*/
                LowerHandStage.GlassExist)
            {
                LowerHandStage.Glass.OrgPortNo = portId + 1;
                LowerHandStage.Glass.OrgSlotNo = portId + 1;

                return true;
            }
            return false;
        }

        public override bool SetGlassTargetPosition(LoaderRobotHand hand, int portId, int slotId)
        {
            // before set the glass data you should check glass sensor status 
            if (hand == LoaderRobotHand.upperHand && /*m_DiUpperGlassDetectConfirm.GetState() == true &&*/
                UpperHandStage.GlassExist)
            {
                UpperHandStage.Glass.TargetPortNo = portId + 1;
                UpperHandStage.Glass.TargetSlotNo = slotId + 1;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && /*m_DiLowerGlassDetectConfirm.GetState() == true &&*/
                LowerHandStage.GlassExist)
            {
                LowerHandStage.Glass.TargetPortNo = portId + 1;
                LowerHandStage.Glass.TargetSlotNo = portId + 1;

                return true;
            }
            return false;
        }

        public override bool GetGlass(LoaderRobotHand hand, ref LoaderGlass gls)
        {
            // before set the glass data you should check glass sensor status 
            if (hand == LoaderRobotHand.upperHand && /*m_DiUpperGlassDetectConfirm.GetState() == true &&*/
                UpperHandStage.GlassExist == true)
            {
                gls = UpperHandStage.Glass;
                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && /*m_DiLowerGlassDetectConfirm.GetState() == true &&*/
                LowerHandStage.GlassExist == true)
            {
                gls = LowerHandStage.Glass;
                return true;
            }
            return false;
        }

        public override bool SetGlass(LoaderRobotHand hand, LoaderGlass gls)
        {
            // before set the glass data you should check glass sensor status 
            if (hand == LoaderRobotHand.upperHand /*&& m_DiUpperGlassDetectConfirm.GetState() == true*/)
            {
                UpperHandStage.Glass = gls;
                UpperHandStage.GlassExist = true;

                m_UpperHandGlsData[0] = gls.OrgPortNo;
                m_UpperHandGlsData[1] = gls.OrgSlotNo;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand /*&& m_DiLowerGlassDetectConfirm.GetState() == true*/)
            {
                LowerHandStage.Glass = gls;
                LowerHandStage.GlassExist = true;

                m_LowerHandGlsData[0] = gls.OrgPortNo;
                m_LowerHandGlsData[1] = gls.OrgSlotNo;

                return true;
            }
            return false;
        }

        public override bool ResetGlass(LoaderRobotHand hand)
        {
            if (hand == LoaderRobotHand.upperHand)
            {
                UpperHandStage.GlassExist = false;

                m_UpperHandGlsData[0] = m_UpperHandGlsData[1] = 0;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand)
            {
                LowerHandStage.GlassExist = false;

                m_LowerHandGlsData[0] = m_LowerHandGlsData[1] = 0;

                return true;
            }
            return false;
        }

        public override int SeqExternalControl()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqRobotHome()
        {
            return m_HomeSeq.Do();
        }

        public override int SeqRobotAlarm()
        {
            //return m_AlarmSeq.Do();
            return -1;
        }

        public override void InitJobSeq()
        {
            m_GetPutSeq.InitSeq();

            SetLog(Name, "InitJobSeq", 0, 0, "Init Job Seq");
        }

        public override void InitScrapSeq()
        {
            m_bInitGetPutScrapSeq = true;

            SetLog(Name, "InitScrapSeq", 0, 0, "Init Scrap Seq");
        }

        public override int SeqGetGlass(LoaderRobotHand hand, int portId, int slotId)
        {
            boeRbtCommandArm opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = boeRbtCommandArm.UPPER_HAND;
            else opHand = boeRbtCommandArm.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, boeRbtCommandCode.GET, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqPutGlass(LoaderRobotHand hand, int portId, int slotId)
        {
            boeRbtCommandArm opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = boeRbtCommandArm.UPPER_HAND;
            else opHand = boeRbtCommandArm.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, boeRbtCommandCode.PUT, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqGetStandby(LoaderRobotHand hand, int portId, int slotId)
        {
            boeRbtCommandArm opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = boeRbtCommandArm.UPPER_HAND;
            else opHand = boeRbtCommandArm.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, boeRbtCommandCode.GET_WAIT, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqPutStandby(LoaderRobotHand hand, int portId, int slotId)
        {
            boeRbtCommandArm opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = boeRbtCommandArm.UPPER_HAND;
            else opHand = boeRbtCommandArm.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, boeRbtCommandCode.PUT_WAIT, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqYAlign(LoaderRobotHand hand, int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqProcessCheck()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqWait(int portId, int slotId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqExchange(LoaderRobotHand handPut, int portId, int slotId)
        {
            boeRbtCommandArm opHandPut;
            if (handPut == LoaderRobotHand.upperHand) opHandPut = boeRbtCommandArm.UPPER_HAND;
            else opHandPut = boeRbtCommandArm.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHandPut, boeRbtCommandCode.EXCHANGE, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqGetGlassWithPinUpDn(LoaderRobotHand hand, int portId, int slotId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqPutGlassWithPinUpDn(LoaderRobotHand hand, int portId, int slotId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsRobotInterferingWith(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool GetRobotTask(ref RobotActionType action, ref int portId, ref int slotId)
        {
            return false;
        }

        public override bool GetRobotTask(ref RobotActionType action, ref LoaderRobotHand hand, ref int portId, ref int slotId)
        {
            bool rv = false;
            LoaderRobotHand handManual;
            RobotActionType actionManual = RobotActionType.None;

            int portIdManual = -1;
            int slotIdManual = -1;

            if (m_ManualMotionStrobeReq)     // Robot Manual Run 할 때, 설정한 Pattern을 조합...
            {
                m_ManualMotionStrobeReq = false;

                if (mibRobotPause.GetState()) return false;

                if (m_ManualHand == boeRbtCommandArm.UPPER_HAND)
                    handManual = LoaderRobotHand.upperHand;
                else
                    handManual = LoaderRobotHand.lowerHand;

                portIdManual = (int)m_MaunalPrimaryPos - 1;
                slotIdManual = m_ManualSlotNo - 1;

                switch (m_ManualOperation)
                {
                    case boeRbtCommandCode.GET_WAIT:
                        actionManual = RobotActionType.GetStandBy;
                        break;
                    case boeRbtCommandCode.PUT_WAIT:
                        actionManual = RobotActionType.PutStandBy;
                        break;
                    case boeRbtCommandCode.EXCHANGE:
                        {
                            // No Operation Tianma45
                        }
                        break;
                    case boeRbtCommandCode.GET:
                        actionManual = RobotActionType.Get;
                        break;
                    case boeRbtCommandCode.PUT:
                        actionManual = RobotActionType.Put;
                        break;
                }

                if (actionManual != RobotActionType.None)
                {
                    hand = handManual;
                    action = actionManual;
                    portId = portIdManual;
                    slotId = slotIdManual;
                    rv = true;
                }
            }
            return rv;
        }
        #endregion

        #region Methods
        public void ResetRobotIfSignal()
        {
            //ResetPatternOfOperation();
            //SetSlotSelection(0);
            //DoMotionPatternReadingStrobe.SetState(false);
            //DoCompletionOfPatternCheckOfOperation.SetState(false);
            //DoCompletionOfGetOperationReceptionist.SetState(false);
            //DoCompletionOfPutOperationReceptionist.SetState(false);
            //DoCompletionOfExchangeOperationReceptionst.SetState(false);

            //for (int i = 0; i < DoCompletionOfDeliveryBits.Count; i++)
            //    DoCompletionOfDeliveryBits[i].SetState(false);

            //#region Reset Robot Side Signal for Simulation
            //if (m_Simul.Device)
            //{
            //    DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(true);
            //    DiCompletionOfReceptionOfMotionPatternData.SetState(false);
            //    DiPatternOfOperationIsPerformed.SetState(false);
            //    DiOperationGet.SetState(false);
            //    DiOperationPut.SetState(false);
            //    DiOperationExchange.SetState(false);
            //    DiCompleteGet.SetState(false);
            //    DiCompletePut.SetState(false);
            //    DiCompleteExchange.SetState(false);
            //}
            //#endregion
        }


        private int GetInterfereTarget()	// Parsing Cube No from Operation Pattern 
        {
            int cubeNo = 0;
            //            int targetNo = (int)m_miwCommandTargetNo.GetState();
            //            int primaryPosition = (int)m_miwCommandPrimaryPos.GetState();
            int targetNo = (int)m_mowCommandTargetNo.GetState();
            int primaryPosition = (int)m_mowCommandPrimaryPos.GetState();

            if (targetNo == 1)
            {
                if (primaryPosition == 1)
                {
                    cubeNo = (int)nxOP_CUBE.PORT1;
                }
                else if (primaryPosition == 2)
                {
                    cubeNo = (int)nxOP_CUBE.PORT2;
                }
            }
            else if (targetNo == 2)
            {
                if (primaryPosition == 2)
                {
                    cubeNo = (int)nxOP_CUBE.STAGE1;
                }
                else if (primaryPosition == 1)
                {
                    cubeNo = (int)nxOP_CUBE.STAGE2;
                }
            }

            return cubeNo;
        }

        private bool IsGetOperation()
        {
            bool getOperation = false;
            //            short commandCode = m_miwCommandCodeNo.GetState();
            short commandCode = (short)m_mowCommandCodeNo.GetState();

            if (commandCode == 4)
            {
                getOperation = true;
            }

            return getOperation;
        }

        private bool IsPutOperation()
        {
            bool putOperation = false;
            //            short commandCode = m_miwCommandCodeNo.GetState();
            short commandCode = (short)m_mowCommandCodeNo.GetState();

            if (commandCode == 5)
            {
                putOperation = true;
            }

            return putOperation;
        }


        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.

            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true)
                return DmsErrors.Success;

            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            m_MaxCubeNo = m_MaxPortNo + m_MaxStageNo;


            bool ok = true;
            ok &= GenerateAssociatedDevices();

            // ////////////////////////////////////////////////////////////////////////////////////////
            // // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion
            ok &= (m_mibLowerHandGlassExist != null);
            ok &= (m_mibRobotAlarm != null);
            ok &= (m_mibRobotBusy != null);
            ok &= (m_mibRobotCommandEndReport != null);
            ok &= (m_mibRobotCommandReadConfirm != null);
            ok &= (m_mibRobotEnable != null);
            ok &= (m_mibRobotOriginPosition != null);
            ok &= (m_mibRobotPause != null);
            ok &= (m_mibUpperHandGlassExist != null);
            ok &= (m_mibVcrReadRequest != null);

            ok &= (m_mobRobotCommandEndConfirm != null);
            ok &= (m_mobRobotCommandReadRequest != null);
            ok &= (m_mobVcrReadConfirm != null);

            ok &= (m_miwCommandArmNo != null);
            ok &= (m_miwCommandCodeNo != null);
            ok &= (m_miwCommandPrimaryPos != null);
            ok &= (m_miwCommandResult != null);
            ok &= (m_miwCommandSecondaryPos != null);
            ok &= (m_miwCommandTargetNo != null);

            ok &= (m_mowCommandArmNo != null);
            ok &= (m_mowCommandCodeNo != null);
            ok &= (m_mowCommandPrimaryPos != null);
            ok &= (m_mowCommandSecondaryPos != null);
            ok &= (m_mowCommandTargetNo != null);

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
                m_HomeSeq = new Yaskawa_BoeHF_HomeSeq(this);
                m_GetPutSeq = new Yaskawa_BoeHF_GetPutGlassSeq(this);
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
                    //DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(true);
                    //DiRemoteModeSelect.SetState(true);
                    //foreach (IoDigitalInput io in DiInterfereCubeBits)
                    //{
                    //    io.SetState(false);
                    //}
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
                m_Tag.SetValue(tagDescriptor.PatternOperationPerform, m_mibRobotBusy.GetState());
                //                m_Tag.SetValue(tagDescriptor.PatternOperationPerform, IsRobotCommand);
                m_Tag.SetValue(tagDescriptor.InterfereTarget, GetInterfereTarget());
                m_Tag.SetValue(tagDescriptor.OperationHand, GetOperationHand());
                m_Tag.SetValue(tagDescriptor.GetOperation, IsGetOperation());
                m_Tag.SetValue(tagDescriptor.PutOperation, IsPutOperation());
                //m_Tag.SetValue(tagDescriptor.PrepareOperation, m_DiOperationPreapare.GetState());
                m_Tag.SetValue(tagDescriptor.UpHandGlassDetect, IsUpperHandGlassDetected());
                m_Tag.SetValue(tagDescriptor.LoHandGlassDetect, IsLowerHandGlassDetected());
                m_Tag.SetValue(tagDescriptor.MaxPortNo, m_MaxPortNo);
                m_Tag.SetValue(tagDescriptor.MaxStageNo, m_MaxStageNo);
                m_Tag.SetValue(tagDescriptor.PatternNo, m_PatternNo);
                m_Tag.SetValue(tagDescriptor.UpperHandPortNo, UpperHandGlsData[0]); // LoaderRobotInfo 에서 사용하는 Tag
                m_Tag.SetValue(tagDescriptor.UpperHandSlotNo, UpperHandGlsData[1]); // LoaderRobotInfo 에서 사용하는 Tag
                m_Tag.SetValue(tagDescriptor.LowerHandPortNo, LowerHandGlsData[0]); // LoaderRobotInfo 에서 사용하는 Tag
                m_Tag.SetValue(tagDescriptor.LowerHandSlotNo, LowerHandGlsData[1]); // LoaderRobotInfo 에서 사용하는 Tag
            }
        }
        #endregion

        #region IRobotSlave 멤버

        public bool IsHandGlassExist(LoaderRobotHand hand)
        {
            if (hand == LoaderRobotHand.upperHand)
            {
                return IsUpperHandGlassDetected() && m_UpperHandStage.GlassExist;
            }
            else
            {
                return IsLowerHandGlassDetected() && m_LowerHandStage.GlassExist;
            }
        }

        public event RobotTransferEventHandler OnRobotTransferEvent;
        public void FireRobotTransferEvent(RobotActionType type, int portId, int slotId)
        {
            if (OnRobotTransferEvent != null)
            {
                OnRobotTransferEvent(type, portId, slotId);
            }
        }
        #endregion
    }

    public class Yaskawa_BoeHF_HomeSeq : XSeqFunction
    {
        #region Fields
        private Yaskawa_BoeHF_Theragen m_Robot = null;
        private int m_CommandEndCode = 0;
        private int m_CommandResult = 0;
        #endregion

        #region Constructor
        public Yaskawa_BoeHF_HomeSeq(Yaskawa_BoeHF_Theragen robot)
        {
            m_Robot = robot;
        }
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;
            if (m_Robot.AlarmCondition != RobotAlarmCondition.NoAlarm)
            {
                m_SeqNo = 0;
                return (int)m_Robot.AlarmCondition;
            }

            switch (seqNo)
            {
                case 0:
                    if (m_Robot.mibRobotBusy.GetState() == false)
                    {
                        m_Robot.mowCommandCodeNo.SetState((ushort)boeRbtCommandCode.RESET);
                        m_Robot.mowCommandArmNo.SetState((ushort)boeRbtCommandArm.UPPER_HAND);
                        m_Robot.mowCommandTargetNo.SetState((ushort)boeRbtCommandTarget.PORT);
                        m_Robot.mowCommandPrimaryPos.SetState(0);
                        m_Robot.mowCommandSecondaryPos.SetState(0);

                        m_Robot.mobRobotCommandReadRequest.SetState(true);

                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mobRobotCommandReadRequest ON");
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Robot.mibRobotCommandReadConfirm.GetState() == true)
                    {
                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mibRobotCommandReadConfirm ON");
                        m_Robot.mobRobotCommandReadRequest.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mobRobotCommandReadRequest OFF");

                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (m_Robot.mibRobotCommandReadConfirm.GetState() == false)
                    {
                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mibRobotCommandReadConfirm OFF");
                        seqNo = 30;
                    }
                    break;
                case 30:
                    if (m_Robot.mibRobotCommandEndReport.GetState() == true)
                    {
                        m_CommandEndCode = (int)m_Robot.miwCommandCodeNo.GetState();
                        m_CommandResult = (int)m_Robot.miwCommandResult.GetState();

                        if (m_CommandEndCode == (int)boeRbtCommandCode.RESET)
                        {
                            m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mibRobotCommandEndReport ON");

                            m_Robot.mobRobotCommandEndConfirm.SetState(true);
                            m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mobRobotCommandEndConfirm ON");

                            seqNo = 40;
                        }
                    }
                    break;
                case 40:
                    if (m_Robot.mibRobotCommandEndReport.GetState() == false)
                    {
                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mibRobotCommandEndReport OFF");
                        m_Robot.mobRobotCommandEndConfirm.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, "mobRobotCommandEndConfirm OFF");

                        string log = "";
                        if (m_CommandResult == (int)boeRbtCommandResult.NORMAL_END)
                        {
                            log = string.Format("Reset Command Result : {0}", boeRbtCommandResult.NORMAL_END.ToString());
                            returnValue = 0;
                        }
                        else if (m_CommandResult == (int)boeRbtCommandResult.ABNORMAL_END)
                        {
                            log = string.Format("Reset Command Reset : {0}", boeRbtCommandResult.ABNORMAL_END.ToString());
                            returnValue = m_CommandResult;
                        }
                        else if (m_CommandResult == (int)boeRbtCommandResult.ILLEGAL_COMMAND)
                        {
                            log = string.Format("Reset Command Reset : {0}", boeRbtCommandResult.ILLEGAL_COMMAND.ToString());
                            returnValue = m_CommandResult;
                        }

                        m_Robot.SetLog(m_Robot.Name, "RESET", 0, 0, log);
                        seqNo = 0;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }

    public class Yaskawa_BoeHF_GetPutGlassSeq : XSeqFunction
    {
        #region Fields
        private Yaskawa_BoeHF_Theragen m_Robot = null;
        private int m_PortId = -1;
        private int m_SlotId = -1;
        private int m_PortNo = -1;
        private int m_SlotNo = -1;
        private boeRbtCommandArm m_Hand;
        private boeRbtCommandCode m_OpPattern;
        private int m_CommandEndCode = 0;
        private int m_CommandResult = 0;
        private string m_Log;
        #endregion

        #region Constructor
        public Yaskawa_BoeHF_GetPutGlassSeq(Yaskawa_BoeHF_Theragen robot)
        {
            m_Robot = robot;
            m_SeqFunName = "GetPutGlassSeq";
        }
        #endregion

        #region Methods
        public void SetCondition(boeRbtCommandArm hand, boeRbtCommandCode pattern, int portId, int slotId)
        {
            m_Hand = hand;
            m_OpPattern = pattern;
            m_PortId = portId;
            m_SlotId = slotId;

            m_PortNo = m_PortId + 1;
            m_SlotNo = m_SlotId + 1;
        }
        #endregion

        #region Override
        public override void InitSeq()
        {
            m_SeqNo = 0;

            if (m_Robot == null) return;

            // Robot Command Read Request
            m_Robot.mowCommandCodeNo.SetState(0);
            m_Robot.mowCommandArmNo.SetState(0);
            m_Robot.mowCommandTargetNo.SetState(0);
            m_Robot.mowCommandPrimaryPos.SetState(0);
            m_Robot.mowCommandSecondaryPos.SetState(0);

            m_Robot.mobRobotCommandReadRequest.SetState(false);

            // Robot Command End Report
            m_Robot.mobRobotCommandEndConfirm.SetState(false);

            m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Init SeqNo and Signal Reset");
        }
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;

            if (m_Robot.InitGetPutScrapSeq)
            {
                m_Robot.InitGetPutScrapSeq = false;

                m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Scrap Request");

                returnValue = (int)boeRbtCommandResult.SCRAP;

                return returnValue;
            }

            switch (seqNo)
            {
                case 0:
                    if (m_Robot.IsRobotReady())
                    {
                        m_Robot.mowCommandCodeNo.SetState((ushort)m_OpPattern);
                        m_Robot.mowCommandArmNo.SetState((ushort)m_Hand);
                        if ((m_PortNo >= (int)boeRbtOpDirection.PORT1) && (m_PortNo <= (int)boeRbtOpDirection.PORT8))
                        {
                            m_Robot.mowCommandTargetNo.SetState((ushort)boeRbtCommandTarget.PORT);
                            m_Robot.mowCommandPrimaryPos.SetState((ushort)m_PortNo);
                            m_Robot.mowCommandSecondaryPos.SetState((ushort)m_SlotNo);

                            m_Log = string.Format("ACT({0}), HAND({1}), TARGET ({2}), PORTNO({3}), SLOTNO({4})", m_OpPattern.ToString(),
                                                                                                  m_Hand.ToString(),
                                                                                                  "PORT",
                                                                                                  m_PortNo,
                                                                                                  m_SlotNo);

                        }
                        else if ((m_PortNo >= (int)boeRbtOpDirection.STAGE1) && (m_PortNo <= (int)boeRbtOpDirection.STAGE4))
                        {
                            m_Robot.mowCommandTargetNo.SetState((ushort)boeRbtCommandTarget.EQP);
                            int stageNo = m_PortNo - (int)boeRbtOpDirection.PORT8;

                            // UNIT ID = Loading : 2, Unloading : 1
                            // Stage No = 1

                            if (stageNo == 1) stageNo = 2;
                            else if (stageNo == 2) stageNo = 1;

                            m_Robot.mowCommandPrimaryPos.SetState((ushort)stageNo);
                            m_Robot.mowCommandSecondaryPos.SetState(1);

                            m_Log = string.Format("ACT({0}), HAND({1}), TARGET ({2}), UNITID({3}), STAGENO({4})", m_OpPattern.ToString(),
                                                                                                 m_Hand.ToString(),
                                                                                                 "EQP",
                                                                                                 stageNo,
                                                                                                 "1");
                        }

                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, m_Log);

                        m_Robot.IsRobotCommand = true;

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 5;

                        if (m_Robot.Simul.Loader == true)
                        {
                            m_Robot.mibRobotBusy.SetState(true);
                        }
                    }
                    break;

                case 5:
                    if (GetElapsedTicks() > 250)
                    {
                        m_Robot.mobRobotCommandReadRequest.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mobRobotCommandReadRequest ON");

                        if (m_Robot.Simul.Loader)
                        {
                            m_Robot.mibRobotCommandReadConfirm.SetState(true);
                        }

                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (m_Robot.mibRobotCommandReadConfirm.GetState() == true)
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mibRobotCommandReadConfirm ON");
                        m_Robot.mobRobotCommandReadRequest.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mobRobotCommandReadRequest OFF");

                        if (m_Robot.Simul.Loader)
                        {
                            m_Robot.mibRobotCommandReadConfirm.SetState(false);
                        }

                        seqNo = 20;
                    }
                    else if (m_Robot.mibLoaderOffline.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Loader change Offline");
                        returnValue = (int)boeRbtCommandResult.ABNORMAL_END;

                        InitSeq();

                        seqNo = 0;
                    }
                    break;

                case 20:
                    if (m_Robot.mibRobotCommandReadConfirm.GetState() == false)
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mibRobotCommandReadConfirm OFF");

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 30;

                        if (m_Robot.Simul.Loader)
                        {
                            m_Robot.miwCommandTargetNo.SetState((short)m_Robot.mowCommandTargetNo.GetState());
                            m_Robot.miwCommandCodeNo.SetState((short)m_Robot.mowCommandCodeNo.GetState());
                            m_Robot.miwCommandPrimaryPos.SetState((short)m_Robot.mowCommandPrimaryPos.GetState());
                            m_Robot.miwCommandSecondaryPos.SetState((short)m_Robot.mowCommandSecondaryPos.GetState());
                            m_Robot.miwCommandArmNo.SetState((short)m_Robot.mowCommandArmNo.GetState());
                            m_Robot.miwCommandResult.SetState(1);

                            m_Robot.mibRobotCommandEndReport.SetState(true);

                            m_StartTicks = XFunc.GetTickCount();

                            seqNo = 25;
                        }
                    }
                    else if (m_Robot.mibLoaderOffline.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Loader change Offline");
                        returnValue = (int)boeRbtCommandResult.ABNORMAL_END;

                        InitSeq();

                        seqNo = 0;
                    }
                    break;

                case 25: // Simulation용으로
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Simulation Delay 1sec");

                        seqNo = 30;
                    }
                    break;

                case 30:
                    if (m_Robot.mibRobotCommandEndReport.GetState() == true)
                    {
                        m_CommandEndCode = (int)m_Robot.miwCommandCodeNo.GetState();
                        m_CommandResult = (int)m_Robot.miwCommandResult.GetState();

                        short commandArm = m_Robot.miwCommandArmNo.GetState();
                        short commandTarget = m_Robot.miwCommandTargetNo.GetState();
                        short commandPrimary = m_Robot.miwCommandPrimaryPos.GetState();
                        short commandSecondary = m_Robot.miwCommandSecondaryPos.GetState();

                        string commandcode = "";
                        string commandhand = "";
                        string commandtarget = "";

                        if (m_CommandEndCode == 2) commandcode = "GET WAIT";
                        else if (m_CommandEndCode == 3) commandcode = "PUT WAIT";
                        else if (m_CommandEndCode == 4) commandcode = "GET";
                        else if (m_CommandEndCode == 5) commandcode = "PUT";
                        else if (m_CommandEndCode == 6) commandcode = "EXCHANGE";

                        if (commandArm == 1) commandhand = "UPPER_HAND";
                        else commandhand = "LOWER_HAND";

                        if (commandTarget == 1) commandtarget = "PORT";
                        else if (commandTarget == 2) commandtarget = "EQP";
                        else if (commandTarget == 3) commandtarget = "VCR POSITION";

                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "-------------------------------------------------------------------------");

                        if (commandtarget == "PORT")
                        {
                            m_Log = string.Format("ACT ({0}), HAND({1}), TARGET ({2}), PORTNO({3}), SLOTNO({4})", commandcode,
                                                                                                 commandhand,
                                                                                                 commandtarget,
                                                                                                 commandPrimary.ToString(),
                                                                                                 commandSecondary.ToString());
                        }
                        else if (commandtarget == "EQP")
                        {
                            m_Log = string.Format("ACT ({0}), HAND({1}), TARGET ({2}), UNITID({3}), SLOTNO({4})", commandcode,
                                                                                                 commandhand,
                                                                                                 commandtarget,
                                                                                                 commandPrimary.ToString(),
                                                                                                 commandSecondary.ToString());
                        }

                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, m_Log);


                        if (m_CommandEndCode != (int)boeRbtCommandCode.RESET)
                        {
                            m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mibRobotCommandEndReport ON");

                            m_Robot.mobRobotCommandEndConfirm.SetState(true);
                            m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mobRobotCommandEndConfirm ON");

                            seqNo = 40;
                        }

                        if (m_Robot.Simul.Loader)
                        {
                            m_Robot.mibRobotCommandEndReport.SetState(false);

                            if (m_OpPattern == boeRbtCommandCode.GET)
                            {
                                if (m_Hand == boeRbtCommandArm.UPPER_HAND && !m_Robot.IsUpperHandGlassDetected())
                                {
                                    m_Robot.SetUpperHandGlassDetected(true);    // Upper Hand GET Case
                                }
                                else if (m_Hand == boeRbtCommandArm.LOWER_HAND && !m_Robot.IsLowerHandGlassDetected())
                                {
                                    m_Robot.SetLowerHandGlassDetected(true);    // Lower Hand GET Case
                                }
                            }
                            else if (m_OpPattern == boeRbtCommandCode.PUT)
                            {
                                if (m_Hand == boeRbtCommandArm.UPPER_HAND && m_Robot.IsUpperHandGlassDetected())
                                {
                                    m_Robot.SetUpperHandGlassDetected(false);   // Upper Hand PUT Case
                                }
                                else if (m_Hand == boeRbtCommandArm.LOWER_HAND && m_Robot.IsLowerHandGlassDetected())
                                {
                                    m_Robot.SetLowerHandGlassDetected(false);   // Lower Hand PUT Case
                                }
                            }

                            m_StartTicks = XFunc.GetTickCount();

                            seqNo = 35;
                        }
                    }
                    else if (m_Robot.mibLoaderOffline.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Loader change Offline");
                        returnValue = (int)boeRbtCommandResult.ABNORMAL_END;

                        InitSeq();

                        seqNo = 0;
                    }
                    else if (GetElapsedTicks() > m_Robot.InterfaceTimeout)
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Robot Command End Report TimeOut Error");

                        returnValue = (int)boeRbtCommandResult.TIMEOUT_ERROR;
                        InitSeq();
                        seqNo = 0;
                    }
                    break;

                case 35:
                    if (GetElapsedTicks() > 500)
                    {
                        m_Robot.mibRobotBusy.SetState(false);

                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "Simulation Robot Busy OFF");

                        seqNo = 40;
                    }
                    break;

                case 40:
                    if (m_Robot.mibRobotCommandEndReport.GetState() == false)
                    {
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mibRobotCommandEndReport OFF");

                        m_Robot.mobRobotCommandEndConfirm.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, "mobRobotCommandEndConfirm OFF");

                        if (m_CommandResult == (int)boeRbtCommandResult.NORMAL_END)
                        {
                            m_Log = string.Format("Robot Command Result : {0}", boeRbtCommandResult.NORMAL_END.ToString());

                            if (m_OpPattern == boeRbtCommandCode.GET)
                            {
                                if (m_Robot.GenInfos.AutoMode) m_Robot.FireRobotTransferEvent(RobotActionType.Get, m_PortId, m_SlotId);
                            }
                            else if (m_OpPattern == boeRbtCommandCode.PUT)
                            {
                                if (m_Robot.GenInfos.AutoMode) m_Robot.FireRobotTransferEvent(RobotActionType.Put, m_PortId, m_SlotId);
                            }

                            returnValue = 0;
                        }
                        else if (m_CommandResult == (int)boeRbtCommandResult.ABNORMAL_END)
                        {
                            m_Log = string.Format("Robot Command Reset : {0}", boeRbtCommandResult.ABNORMAL_END.ToString());
                            returnValue = m_CommandResult;
                        }
                        else if (m_CommandResult == (int)boeRbtCommandResult.ILLEGAL_COMMAND)
                        {
                            m_Log = string.Format("Robot Command Reset : {0}", boeRbtCommandResult.ILLEGAL_COMMAND.ToString());
                            returnValue = m_CommandResult;
                        }

                        m_Robot.IsRobotCommand = false;

                        m_Robot.SetLog(m_Robot.Name, "PUT/GET", 0, 0, m_Log);
                        seqNo = 0;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }
}
