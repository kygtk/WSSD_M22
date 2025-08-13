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
    public class YaskawaNX100 : _IndexRobot, IRobotSlave
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
        private nxOP_PATTERN[] m_UsedManualPattern = new nxOP_PATTERN[] { nxOP_PATTERN.GET, };

        private float m_Adsorption = 0.0F;
        private int m_PatternNo = 0;
        private int[] m_UpperHandGlsData = new int[] { 0, 0 };
        private int[] m_LowerHandGlsData = new int[] { 0, 0 };

        private nxOP_CUBE m_Cube;                   // Robot Auto 동작시 Cube
        private nxOP_HAND m_Hand;                   // Robot Auto 동작시 Hand
        private nxOP_PATTERN m_Pattern;             // Robot Auto 동작시 Pattern
        private nxOP_CUBE m_ManualCube;             // Robot Manual 조작시 Cube
        private nxOP_HAND m_ManualHand;             // Robot Manual 조작시 Hand
        private nxOP_PATTERN m_ManualPattern;       // Robot Manual 조작시 Pattern
                                                    //private nxEX_CONTROL m_ExternalControl;     // Robot External Control

        private int m_ManualSlotNo = 0;
        private bool m_ManualMotionStrobeReq = false;
        private bool m_ManualHomeReturnReq = false;

        private bool m_InProcess = false;

        // I/O Setting - Robot Status
        private IoDigitalInput m_DiRunning = new IoDigitalInput();
        private IoDigitalInput m_DiServoOn = new IoDigitalInput();
        private IoDigitalInput m_DiTopOfMasterJob = new IoDigitalInput();
        private IoDigitalInput m_DiRobotHold = new IoDigitalInput();
        private IoDigitalInput m_DiIntermediateStart = new IoDigitalInput();
        private IoDigitalInput m_DiSafetyPlug = new IoDigitalInput();
        private IoDigitalInput m_DiPowerOn = new IoDigitalInput();

        // I/O Setting - Robot Alarm
        private IoDigitalInput m_DiAlarmErrorOccurred = new IoDigitalInput();
        private IoDigitalInput m_DiAlarmBattery = new IoDigitalInput();
        private IoDigitalInput m_DiErrorUpperVacuum = new IoDigitalInput();
        private IoDigitalInput m_DiErrorLowerVacuum = new IoDigitalInput();
        private IoDigitalInput m_DiErrorUpperAlignment = new IoDigitalInput();
        private IoDigitalInput m_DiErrorLowerAlignment = new IoDigitalInput();
        private IoDigitalInput m_DiErrorUpperAlign_Y = new IoDigitalInput();
        private IoDigitalInput m_DiErrorLowerAlign_Y = new IoDigitalInput();
        private IoDigitalInput m_DiAbnormalityInGlassFallUpper = new IoDigitalInput();
        private IoDigitalInput m_DiAbnormalityInGlassFallLower = new IoDigitalInput();
        private IoDigitalInput m_DiVacuumLowLevel = new IoDigitalInput();

        // I/O Setting - Mode   
        private IoDigitalInput m_DiRemoteModeSelect = new IoDigitalInput();
        private IoDigitalInput m_DiPlayModeSelect = new IoDigitalInput();
        private IoDigitalInput m_DiTeachModeSelect = new IoDigitalInput();
        private IoDigitalInput m_DiTeachLockSet = new IoDigitalInput();

        // I/O Setting - Motion Pattern Strobe
        private IoDigitalInput m_DiCompletionOfReceptionOfKindData = new IoDigitalInput();
        private IoDigitalInput m_DiCompletionOfReceptionOfMotionPatternData = new IoDigitalInput();
        private IoDigitalInput m_DiCompletionOfReceptionOfSlotData = new IoDigitalInput();
        private IoDigitalInput m_DiCompletionOfPatternStrobeReadingPreparationOfOperation = new IoDigitalInput();
        private IoDigitalInput m_DiPatternOfOperationIsPerformed = new IoDigitalInput();

        // I/O Setting - External Opeartion Command 
        private IoDigitalInput m_DiOperationOriginPoint = new IoDigitalInput();
        private IoDigitalInput m_DiHomePositionReturn = new IoDigitalInput();
        private IoDigitalInput m_DiVcrReadingCompletion = new IoDigitalInput();
        private IoDigitalInput m_DiCompletionOfPreparationToReadsVCR = new IoDigitalInput();
        private IoDigitalInput m_DiMovingToVcrPosition = new IoDigitalInput();
        private IoDigitalInput m_DiOperationPut = new IoDigitalInput();
        private IoDigitalInput m_DiOperationGet = new IoDigitalInput();
        private IoDigitalInput m_DiOperationPreapare = new IoDigitalInput();
        private IoDigitalInput m_DiOperationAlign_Y = new IoDigitalInput();
        private IoDigitalInput m_DiOperationExchange = new IoDigitalInput();
        private IoDigitalInput m_DiCompletePut = new IoDigitalInput();
        private IoDigitalInput m_DiCompleteGet = new IoDigitalInput();
        private IoDigitalInput m_DiCompletePrepare = new IoDigitalInput();
        private IoDigitalInput m_DiCompleteAlign_Y = new IoDigitalInput();
        private IoDigitalInput m_DiCompleteExchange = new IoDigitalInput();

        // I/O Setting - Delivery Completion & Preparation
        private IoDigitalInput m_DiAdsorptionConveyenceUpper = new IoDigitalInput();
        private IoDigitalInput m_DiAdsorptionConveyenceLower = new IoDigitalInput();
        private IoDigitalInput m_DiVirtualModeConveyance = new IoDigitalInput();

        // I/O Setting - Glass Detect & Vacuum Check
        private IoDigitalInput m_DiUpperAdsorptionConfirm = new IoDigitalInput();
        private IoDigitalInput m_DiLowerAdsorptionConfirm = new IoDigitalInput();
        private IoDigitalInput m_DiUpperGlassDetectConfirm = new IoDigitalInput();
        private IoDigitalInput m_DiLowerGlassDetectConfirm = new IoDigitalInput();

        // I/O Setting - External Command
        private IoDigitalOutput m_DoExternalStart = new IoDigitalOutput();
        private IoDigitalOutput m_DoExternalHold = new IoDigitalOutput();
        private IoDigitalOutput m_DoExternalServoOn = new IoDigitalOutput();
        private IoDigitalOutput m_DoExternalServoOff = new IoDigitalOutput();
        private IoDigitalOutput m_DoCallMasterJob = new IoDigitalOutput();
        private IoDigitalOutput m_DoUpperVacuumOnRequest = new IoDigitalOutput();
        private IoDigitalOutput m_DoLowerVacuumOnRequest = new IoDigitalOutput();
        private IoDigitalOutput m_DoUpperVacuumOffRequest = new IoDigitalOutput();
        private IoDigitalOutput m_DoLowerVacuumOffRequest = new IoDigitalOutput();

        // I/O Setting - Entrance Prohibition & Interference

        // I/O Setting - Motion Pattern Strobe
        private IoDigitalOutput m_DoMotionPatternReadingStrobe = new IoDigitalOutput();
        private IoDigitalOutput m_DoCompletionOfPatternCheckOfOperation = new IoDigitalOutput();

        // I/O Setting - Delivery Completion & Preparation
        private IoDigitalOutput m_DoVirtualConveyanceMode = new IoDigitalOutput();

        // I/O Setting - External Delivery Setting
        private IoDigitalOutput m_DoSpeedLimitMode = new IoDigitalOutput();

        // I/O Setting - External Opeartion Command 
        private IoDigitalOutput m_DoHomePositionReturnRequest = new IoDigitalOutput();
        private IoDigitalOutput m_DoCompletionOfPutOperationReceptionist = new IoDigitalOutput();
        private IoDigitalOutput m_DoCompletionOfGetOperationReceptionist = new IoDigitalOutput();
        private IoDigitalOutput m_DoCompletionOfPreparationReception = new IoDigitalOutput(); //////
        private IoDigitalOutput m_DoCompletionCheckOfVcrOperation = new IoDigitalOutput();
        private IoDigitalOutput m_DoCompletionCheckOfY_AlignOperation = new IoDigitalOutput();
        private IoDigitalOutput m_DoCompletionOfExchangeOperationReceptionst = new IoDigitalOutput();
        private IoDigitalOutput m_DoExchangeOperationCommand = new IoDigitalOutput();
        private IoDigitalOutput m_DoVcrReadingCommand = new IoDigitalOutput();
        private IoDigitalOutput m_DoAlignEffective_T_X = new IoDigitalOutput();
        private IoDigitalOutput m_DoAlignEffective_Y = new IoDigitalOutput();

        // I/O Setting - Robot Alarm
        private IoDigitalOutput m_DoAlarmErrorReset = new IoDigitalOutput();

        // I/O Setting - Normal Status
        private IoDigitalOutput m_DoPlcNormal = new IoDigitalOutput();

        // I/O Collection - DigitalInput
        private IoCollection<IoDigitalInput> m_DiInterfereCubeBits = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalInput> m_DiCompleteDeliveryBits = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalInput> m_DiSlotNoBits = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalInput> m_DiPatternOfOperationBits = new IoCollection<IoDigitalInput>();

        // I/O Collection - DigitalOutput
        private IoCollection<IoDigitalOutput> m_DoEnterProhibitCubeBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoEnterPassableCubeBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoKindOfGlassSettingBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoCompletionOfDeliveryBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoSlotSelectionBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoPatternOfOperationBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoExternalSpeedCommandBits = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoDigitalOutput> m_DoAdsorptionTimeBits = new IoCollection<IoDigitalOutput>();

        // Alarm List
        public Alarm ALM_BatteryAlarm = null;
        public Alarm ALM_ErrorOccuredAlarm = null;
        public Alarm ALM_UpperHandVacuumError = null;
        public Alarm ALM_LowerHandVacuumError = null;
        public Alarm ALM_UpperHandAlignmentError = null;
        public Alarm ALM_LowerHandAlignmentError = null;
        public Alarm ALM_UpperHandYAlignError = null;
        public Alarm ALM_LowerHandYAlignError = null;
        public Alarm ALM_UpperAbnormalGlassFall = null;
        public Alarm ALM_LowerAbnormalGlassFall = null;

        // For Robot Sequence
        private YaskawaNx100ExternalControlSeq m_ExternalControlSeq = null;
        private YaskawaNx100HomeSeq m_HomeSeq = null;
        private YaskawaNx100GetPutGlassSeq m_GetPutSeq = null;
        private YaskawaNx100YAlignSeq m_YAlignSeq = null;
        private YaskawaNx100ProcessCheckSeq m_ProcessCheckSeq = null;
        private YaskawaNx100AlarmSeq m_AlarmSeq = null;
        #endregion

        #region Properties
        //[Category("DMS : Relaion"), Description("Select Port List for Indexer")]
        //public _GenericCollection<PortUnit> PortUnit
        //{
        //    get { return m_PortUnit; }
        //    set { m_PortUnit = value; }
        //}
        //[Category("DMS : Relaion"), Description("Select Stage List for Indexer")]
        //public _GenericCollection<PortUnit> StageUnit
        //{
        //    get { return m_StageUnit; }
        //    set { m_StageUnit = value; }
        //}
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
        public nxOP_PATTERN[] UsedManualPattern
        {
            get { return m_UsedManualPattern; }
            set { m_UsedManualPattern = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiRunning
        {
            get { return m_DiRunning; }
            set { m_DiRunning = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiServoOn
        {
            get { return m_DiServoOn; }
            set { m_DiServoOn = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiTopOfMasterJob
        {
            get { return m_DiTopOfMasterJob; }
            set { m_DiTopOfMasterJob = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiRobotHold
        {
            get { return m_DiRobotHold; }
            set { m_DiRobotHold = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiIntermediateStart
        {
            get { return m_DiIntermediateStart; }
            set { m_DiIntermediateStart = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiSafetyPlug
        {
            get { return m_DiSafetyPlug; }
            set { m_DiSafetyPlug = value; }
        }
        [Category("DMS : I/O Setting - Robot Status")]
        public IoDigitalInput DiPowerOn
        {
            get { return m_DiPowerOn; }
            set { m_DiPowerOn = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiAlarmErrorOccurred
        {
            get { return m_DiAlarmErrorOccurred; }
            set { m_DiAlarmErrorOccurred = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiAlarmBattery
        {
            get { return m_DiAlarmBattery; }
            set { m_DiAlarmBattery = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiErrorUpperVacuum
        {
            get { return m_DiErrorUpperVacuum; }
            set { m_DiErrorUpperVacuum = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiErrorLowerVacuum
        {
            get { return m_DiErrorLowerVacuum; }
            set { m_DiErrorLowerVacuum = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiErrorUpperAlignment
        {
            get { return m_DiErrorUpperAlignment; }
            set { m_DiErrorUpperAlignment = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiErrorLowerAlignment
        {
            get { return m_DiErrorLowerAlignment; }
            set { m_DiErrorLowerAlignment = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiErrorUpperAlign_Y
        {
            get { return m_DiErrorUpperAlign_Y; }
            set { m_DiErrorUpperAlign_Y = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiErrorLowerAlign_Y
        {
            get { return m_DiErrorLowerAlign_Y; }
            set { m_DiErrorLowerAlign_Y = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiAbnormalityInGlassFallUpper
        {
            get { return m_DiAbnormalityInGlassFallUpper; }
            set { m_DiAbnormalityInGlassFallUpper = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalInput DiAbnormalityInGlassFallLower
        {
            get { return m_DiAbnormalityInGlassFallLower; }
            set { m_DiAbnormalityInGlassFallLower = value; }
        }
        [Category("DMS : I/O Setting - Gauge Alarm"), Description("Vacuum Low Level")]
        public IoDigitalInput DiVacuumLowLevel
        {
            get { return m_DiVacuumLowLevel; }
            set { m_DiVacuumLowLevel = value; }
        }
        [Category("DMS : I/O Setting - Mode")]
        public IoDigitalInput DiRemoteModeSelect
        {
            get { return m_DiRemoteModeSelect; }
            set { m_DiRemoteModeSelect = value; }
        }
        [Category("DMS : I/O Setting - Mode")]
        public IoDigitalInput DiPlayModeSelect
        {
            get { return m_DiPlayModeSelect; }
            set { m_DiPlayModeSelect = value; }
        }
        [Category("DMS : I/O Setting - Mode")]
        public IoDigitalInput DiTeachModeSelect
        {
            get { return m_DiTeachModeSelect; }
            set { m_DiTeachModeSelect = value; }
        }
        [Category("DMS : I/O Setting - Mode")]
        public IoDigitalInput DiTeachLockSet
        {
            get { return m_DiTeachLockSet; }
            set { m_DiTeachLockSet = value; }
        }
        [Category("DMS : I/O Setting - Entrance Prohibition & Interference")]
        public IoCollection<IoDigitalInput> DiInterfereCubeBits
        {
            get { return m_DiInterfereCubeBits; }
            set { m_DiInterfereCubeBits = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoCollection<IoDigitalInput> DiSlotNoBits
        {
            get { return m_DiSlotNoBits; }
            set { m_DiSlotNoBits = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoCollection<IoDigitalInput> DiPatternOfOperationBits
        {
            get { return m_DiPatternOfOperationBits; }
            set { m_DiPatternOfOperationBits = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalInput DiCompletionOfReceptionOfKindData
        {
            get { return m_DiCompletionOfReceptionOfKindData; }
            set { m_DiCompletionOfReceptionOfKindData = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalInput DiCompletionOfReceptionOfMotionPatternData
        {
            get { return m_DiCompletionOfReceptionOfMotionPatternData; }
            set { m_DiCompletionOfReceptionOfMotionPatternData = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalInput DiCompletionOfReceptionOfSlotData
        {
            get { return m_DiCompletionOfReceptionOfSlotData; }
            set { m_DiCompletionOfReceptionOfSlotData = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalInput DiCompletionOfPatternStrobeReadingPreparationOfOperation
        {
            get { return m_DiCompletionOfPatternStrobeReadingPreparationOfOperation; }
            set { m_DiCompletionOfPatternStrobeReadingPreparationOfOperation = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalInput DiPatternOfOperationIsPerformed
        {
            get { return m_DiPatternOfOperationIsPerformed; }
            set { m_DiPatternOfOperationIsPerformed = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiOperationOriginPoint
        {
            get { return m_DiOperationOriginPoint; }
            set { m_DiOperationOriginPoint = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiHomePositionReturn
        {
            get { return m_DiHomePositionReturn; }
            set { m_DiHomePositionReturn = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiVcrReadingCompletion
        {
            get { return m_DiVcrReadingCompletion; }
            set { m_DiVcrReadingCompletion = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiCompletionOfPreparationToReadsVCR
        {
            get { return m_DiCompletionOfPreparationToReadsVCR; }
            set { m_DiCompletionOfPreparationToReadsVCR = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiMovingToVcrPosition
        {
            get { return m_DiMovingToVcrPosition; }
            set { m_DiMovingToVcrPosition = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiOperationPut
        {
            get { return m_DiOperationPut; }
            set { m_DiOperationPut = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiOperationGet
        {
            get { return m_DiOperationGet; }
            set { m_DiOperationGet = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiOperationPreapare
        {
            get { return m_DiOperationPreapare; }
            set { m_DiOperationPreapare = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiOperationAlign_Y
        {
            get { return m_DiOperationAlign_Y; }
            set { m_DiOperationAlign_Y = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiOperationExchange
        {
            get { return m_DiOperationExchange; }
            set { m_DiOperationExchange = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiCompletePut
        {
            get { return m_DiCompletePut; }
            set { m_DiCompletePut = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiCompleteGet
        {
            get { return m_DiCompleteGet; }
            set { m_DiCompleteGet = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiCompletePrepare
        {
            get { return m_DiCompletePrepare; }
            set { m_DiCompletePrepare = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiCompleteAlign_Y
        {
            get { return m_DiCompleteAlign_Y; }
            set { m_DiCompleteAlign_Y = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalInput DiCompleteExchange
        {
            get { return m_DiCompleteExchange; }
            set { m_DiCompleteExchange = value; }
        }
        [Category("DMS : I/O Setting - Delivery Completion & Preparation")]
        public IoCollection<IoDigitalInput> DiCompleteDeliveryBits
        {
            get { return m_DiCompleteDeliveryBits; }
            set { m_DiCompleteDeliveryBits = value; }
        }
        [Category("DMS : I/O Setting - Delivery Completion & Preparation")]
        public IoDigitalInput DiAdsorptionConveyenceUpper
        {
            get { return m_DiAdsorptionConveyenceUpper; }
            set { m_DiAdsorptionConveyenceUpper = value; }
        }
        [Category("DMS : I/O Setting - Delivery Completion & Preparation")]
        public IoDigitalInput DiAdsorptionConveyenceLower
        {
            get { return m_DiAdsorptionConveyenceLower; }
            set { m_DiAdsorptionConveyenceLower = value; }
        }
        [Category("DMS : I/O Setting - Delivery Completion & Preparation")]
        public IoDigitalInput DiVirtualModeConveyance
        {
            get { return m_DiVirtualModeConveyance; }
            set { m_DiVirtualModeConveyance = value; }
        }
        [Category("DMS : I/O Setting - Glass Detect & Vacuum Check")]
        public IoDigitalInput DiUpperAdsorptionConfirm
        {
            get { return m_DiUpperAdsorptionConfirm; }
            set { m_DiUpperAdsorptionConfirm = value; }
        }
        [Category("DMS : I/O Setting - Glass Detect & Vacuum Check")]
        public IoDigitalInput DiLowerAdsorptionConfirm
        {
            get { return m_DiLowerAdsorptionConfirm; }
            set { m_DiLowerAdsorptionConfirm = value; }
        }
        [Category("DMS : I/O Setting - Glass Detect & Vacuum Check")]
        public IoDigitalInput DiUpperGlassDetectConfirm
        {
            get { return m_DiUpperGlassDetectConfirm; }
            set { m_DiUpperGlassDetectConfirm = value; }
        }
        [Category("DMS : I/O Setting - Glass Detect & Vacuum Check")]
        public IoDigitalInput DiLowerGlassDetectConfirm
        {
            get { return m_DiLowerGlassDetectConfirm; }
            set { m_DiLowerGlassDetectConfirm = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoExternalStart
        {
            get { return m_DoExternalStart; }
            set { m_DoExternalStart = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoExternalHold
        {
            get { return m_DoExternalHold; }
            set { m_DoExternalHold = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoExternalServoOn
        {
            get { return m_DoExternalServoOn; }
            set { m_DoExternalServoOn = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoExternalServoOff
        {
            get { return m_DoExternalServoOff; }
            set { m_DoExternalServoOff = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoCallMasterJob
        {
            get { return m_DoCallMasterJob; }
            set { m_DoCallMasterJob = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoUpperVacuumOnRequest
        {
            get { return m_DoUpperVacuumOnRequest; }
            set { m_DoUpperVacuumOnRequest = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoLowerVacuumOnRequest
        {
            get { return m_DoLowerVacuumOnRequest; }
            set { m_DoLowerVacuumOnRequest = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoUpperVacuumOffRequest
        {
            get { return m_DoUpperVacuumOffRequest; }
            set { m_DoUpperVacuumOffRequest = value; }
        }
        [Category("DMS : I/O Setting - External Command")]
        public IoDigitalOutput DoLowerVacuumOffRequest
        {
            get { return m_DoLowerVacuumOffRequest; }
            set { m_DoLowerVacuumOffRequest = value; }
        }
        [Category("DMS : I/O Setting - Entrance Prohibition & Interference")]
        public IoCollection<IoDigitalOutput> DoEnterProhibitCubeBits
        {
            get { return m_DoEnterProhibitCubeBits; }
            set { m_DoEnterProhibitCubeBits = value; }
        }
        [Category("DMS : I/O Setting - Entrance Prohibition & Interference")]
        public IoCollection<IoDigitalOutput> DoEnterPassableCubeBits
        {
            get { return m_DoEnterPassableCubeBits; }
            set { m_DoEnterPassableCubeBits = value; }
        }
        [Category("DMS : I/O Setting - Kind of glass, Motion Pattern Strobe")]
        public IoCollection<IoDigitalOutput> DoKindOfGlassSettingBits
        {
            get { return m_DoKindOfGlassSettingBits; }
            set { m_DoKindOfGlassSettingBits = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoCollection<IoDigitalOutput> DoSlotSelectionBits
        {
            get { return m_DoSlotSelectionBits; }
            set { m_DoSlotSelectionBits = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoCollection<IoDigitalOutput> DoPatternOfOperationBits
        {
            get { return m_DoPatternOfOperationBits; }
            set { m_DoPatternOfOperationBits = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalOutput DoMotionPatternReadingStrobe
        {
            get { return m_DoMotionPatternReadingStrobe; }
            set { m_DoMotionPatternReadingStrobe = value; }
        }
        [Category("DMS : I/O Setting - Motion Pattern Strobe")]
        public IoDigitalOutput DoCompletionOfPatternCheckOfOperation
        {
            get { return m_DoCompletionOfPatternCheckOfOperation; }
            set { m_DoCompletionOfPatternCheckOfOperation = value; }
        }
        [Category("DMS : I/O Setting - Delivery Completion & Preparation")]
        public IoCollection<IoDigitalOutput> DoCompletionOfDeliveryBits
        {
            get { return m_DoCompletionOfDeliveryBits; }
            set { m_DoCompletionOfDeliveryBits = value; }
        }
        [Category("DMS : I/O Setting - Delivery Completion & Preparation")]
        public IoDigitalOutput DoVirtualConveyanceMode
        {
            get { return m_DoVirtualConveyanceMode; }
            set { m_DoVirtualConveyanceMode = value; }
        }
        [Category("DMS : I/O Setting - External Delivery Setting")]
        public IoDigitalOutput DoSpeedLimitMode
        {
            get { return m_DoSpeedLimitMode; }
            set { m_DoSpeedLimitMode = value; }
        }
        [Category("DMS : I/O Setting - External Delivery Setting")]
        public IoCollection<IoDigitalOutput> DoExternalSpeedCommandBits
        {
            get { return m_DoExternalSpeedCommandBits; }
            set { m_DoExternalSpeedCommandBits = value; }
        }
        [Category("DMS : I/O Setting - External Delivery Setting")]
        public IoCollection<IoDigitalOutput> DoAdsorptionTimeBits
        {
            get { return m_DoAdsorptionTimeBits; }
            set { m_DoAdsorptionTimeBits = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoHomePositionReturnRequest
        {
            get { return m_DoHomePositionReturnRequest; }
            set { m_DoHomePositionReturnRequest = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoCompletionOfPutOperationReceptionist
        {
            get { return m_DoCompletionOfPutOperationReceptionist; }
            set { m_DoCompletionOfPutOperationReceptionist = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoCompletionOfGetOperationReceptionist
        {
            get { return m_DoCompletionOfGetOperationReceptionist; }
            set { m_DoCompletionOfGetOperationReceptionist = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoCompletionOfPreparationReception
        {
            get { return m_DoCompletionOfPreparationReception; }
            set { m_DoCompletionOfPreparationReception = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoCompletionCheckOfVcrOperation
        {
            get { return m_DoCompletionCheckOfVcrOperation; }
            set { m_DoCompletionCheckOfVcrOperation = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoCompletionCheckOfY_AlignOperation
        {
            get { return m_DoCompletionCheckOfY_AlignOperation; }
            set { m_DoCompletionCheckOfY_AlignOperation = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoCompletionOfExchangeOperationReceptionst
        {
            get { return m_DoCompletionOfExchangeOperationReceptionst; }
            set { m_DoCompletionOfExchangeOperationReceptionst = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoExchangeOperationCommand
        {
            get { return m_DoExchangeOperationCommand; }
            set { m_DoExchangeOperationCommand = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoVcrReadingCommand
        {
            get { return m_DoVcrReadingCommand; }
            set { m_DoVcrReadingCommand = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoAlignEffective_T_X
        {
            get { return m_DoAlignEffective_T_X; }
            set { m_DoAlignEffective_T_X = value; }
        }
        [Category("DMS : I/O Setting - External Opeartion Command")]
        public IoDigitalOutput DoAlignEffective_Y
        {
            get { return m_DoAlignEffective_Y; }
            set { m_DoAlignEffective_Y = value; }
        }
        [Category("DMS : I/O Setting - Robot Alarm")]
        public IoDigitalOutput DoAlarmErrorReset
        {
            get { return m_DoAlarmErrorReset; }
            set { m_DoAlarmErrorReset = value; }
        }
        [Category("DMS : I/O Setting - Normal Status")]
        public IoDigitalOutput DoPlcNormal
        {
            get { return m_DoPlcNormal; }
            set { m_DoPlcNormal = value; }
        }
        [Browsable(false)]
        public int PatternNo
        {
            get { return m_PatternNo; }
        }
        [Browsable(false)]
        public nxOP_CUBE Cube
        {
            get { return m_Cube; }
        }
        [Browsable(false), XmlIgnore()]
        public nxOP_HAND Hand
        {
            get { return m_Hand; }
        }
        [Browsable(false), XmlIgnore()]
        public nxOP_PATTERN Pattern
        {
            get { return m_Pattern; }
        }
        [Browsable(false), XmlIgnore()]
        public nxOP_CUBE ManualCube
        {
            get { return m_ManualCube; }
            set { m_ManualCube = value; }
        }
        [Browsable(false), XmlIgnore()]
        public nxOP_HAND ManualHand
        {
            get { return m_ManualHand; }
            set { m_ManualHand = value; }
        }
        [Browsable(false), XmlIgnore()]
        public nxOP_PATTERN ManualPattern
        {
            get { return m_ManualPattern; }
            set { m_ManualPattern = value; }
        }
        //[Browsable(false), XmlIgnore()]
        //public nxEX_CONTROL ExternalControl
        //{
        //    get { return m_ExternalControl; }
        //    set { m_ExternalControl = value; }
        //}
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
        public bool InProcess
        {
            get { return m_InProcess; }
            set { m_InProcess = value; }
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

        #region Constructure
        public YaskawaNX100()
        {
            this.Name = "__ Yaskawa Robot";
            m_UpperHandCode = 0;
            m_LowerHandCode = 64;
        }
        #endregion

        #region _IndexRobot Abstract override
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            bool alarm = false;
            alarm |= m_DiAlarmErrorOccurred.GetState();
            alarm |= m_DiAlarmBattery.GetState();
            alarm |= m_DiErrorUpperVacuum.GetState();
            alarm |= m_DiErrorLowerVacuum.GetState();
            alarm |= m_DiErrorUpperAlignment.GetState();
            alarm |= m_DiErrorLowerAlignment.GetState();
            alarm |= m_DiErrorUpperAlign_Y.GetState();
            alarm |= m_DiErrorLowerAlign_Y.GetState();
            alarm |= m_DiAbnormalityInGlassFallUpper.GetState();
            alarm |= m_DiAbnormalityInGlassFallLower.GetState();

            return alarm;
        }

        public override int GetSlotNo()
        {
            int slotNo = 0;
            int itemsCount = m_DiSlotNoBits.Count;

            for (int i = 0; i < itemsCount; i++)
            {
                slotNo |= ((m_DiSlotNoBits[i].GetState() ? 1 : 0) << i);
            }

            return slotNo;
        }

        public override int GetOperationHand()
        {
            // Upper Hand : 1~64, 129~192
            // Lower Hand : 65~128, 193~255
            int patternNo = GetPatternOfOperation();
            int handNo = patternNo / ((int)nxOP_HAND.LOWER_HAND + 1);

            if (handNo % 2 == 0)
            {
                return (int)nxOP_HAND.UPPER_HAND;
            }
            else
            {
                return (int)nxOP_HAND.LOWER_HAND;
            }
        }

        public override RobotSelectedMode GetModeSelected()
        {
            if (m_DiRemoteModeSelect.GetState()) return RobotSelectedMode.RemoteMode;
            else if (m_DiPlayModeSelect.GetState()) return RobotSelectedMode.PlayMode;
            else if (m_DiTeachModeSelect.GetState()) return RobotSelectedMode.TeachMode;
            else
            {
                return RobotSelectedMode.Unknown;
            }
        }

        public override bool IsUpperHandGlassDetected()
        {
            if (!this.Initialized) return false;

            bool bRv = true;
            bRv &= m_DiUpperGlassDetectConfirm.GetState();
            bRv &= m_DiUpperAdsorptionConfirm.GetState();

            return bRv;
        }

        // Only for simulation
        public void SetUpperHandGlassDetected(bool on)
        {
            if (m_Simul.Device)
            {
                m_DiUpperGlassDetectConfirm.SetState(on);
                m_DiUpperAdsorptionConfirm.SetState(on);
            }
        }

        public override bool IsLowerHandGlassDetected()
        {
            if (!this.Initialized) return false;

            bool bRv = true;
            bRv &= m_DiLowerGlassDetectConfirm.GetState();
            bRv &= m_DiLowerAdsorptionConfirm.GetState();

            return bRv;
        }

        // Only for simulation
        public void SetLowerHandGlassDetected(bool on)
        {
            if (m_Simul.Device)
            {
                m_DiLowerGlassDetectConfirm.SetState(on);
                m_DiLowerAdsorptionConfirm.SetState(on);
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
                rv &= !m_DiUpperGlassDetectConfirm.GetState();
                rv &= !m_DiUpperAdsorptionConfirm.GetState();
            }
            else
            {
                rv &= !m_DiLowerGlassDetectConfirm.GetState();
                rv &= !m_DiLowerAdsorptionConfirm.GetState();
            }
            return rv;
        }

        // Only for simulation
        public void SetHandGlassDetected(LoaderRobotHand hand, bool on)
        {
            if (m_Simul.Device)
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
            if (Simul.Device == false)
            {
                //ready &= m_DiHomePositionReturn.GetState();
                ready &= m_DiServoOn.GetState();
                ready &= m_DiRunning.GetState();
                ready &= m_DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState();
            }
            return ready;
        }

        public override bool IsRobotRunning()
        {
            bool ready = true;
            if (Simul.Device == false)
            {
                ready &= m_DiServoOn.GetState();
                ready &= m_DiRunning.GetState();
            }
            return ready;
        }

        public override bool IsRobotServoOn()
        {
            bool servoOn = true;
            servoOn &= m_DiServoOn.GetState();

            return servoOn;
        }

        public override bool IsRobotHandHome()
        {
            bool handHome = true;
            if (Simul.Device == false)
            {
                //handHome &= m_DiHomePositionReturn.GetState();
                handHome &= m_DiOperationOriginPoint.GetState();
            }
            return handHome;
        }

        public override bool IsRobotEmoAlarm()
        {
            bool emoAlarm = false;
            if (Simul.Device == false)
            {
                emoAlarm |= (m_DiSafetyPlug.GetState() == false);
                //emoAlarm |= m_Di
            }
            return emoAlarm;
        }

        public override bool IsRobotHold()
        {
            bool robotHold = false;
            if (Simul.Device == false)
            {
                robotHold |= m_DiRobotHold.GetState();
            }
            return robotHold;
        }

        public override bool IsOperationBegin()
        {
            bool begin = true;
            begin &= m_DiRunning.GetState();
            begin &= m_DiPatternOfOperationIsPerformed.GetState();
            begin &= !m_DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState();

            return begin;
        }

        public override bool IsRobotBusy()
        {
            bool busy = false;

            busy |= m_DiOperationPut.GetState();
            busy |= m_DiOperationGet.GetState();
            busy |= m_DiOperationPreapare.GetState();
            busy |= m_DiOperationExchange.GetState();

            busy |= m_DiOperationAlign_Y.GetState();
            busy |= m_DiMovingToVcrPosition.GetState();

            //busy |= m_InProcess;
            return busy;
        }

        public override void SetExternalStart(bool on)
        {
            m_DoExternalStart.SetState(on);
        }

        public override void SetRobotPause(bool pause)
        {
            m_DoExternalHold.SetState(pause);
        }

        public override void SetGlassKind(int kindNo)                   // Index[0~15]를 첨자로 하는 Thickness Array의 해당하는 Output Bit를 Set
        {
            int itemsCount = m_DoKindOfGlassSettingBits.Count;

            for (int i = 0; i < itemsCount; i++)
            {
                bool flag = ((kindNo >> i) & 0x01) > 0;
                m_DoKindOfGlassSettingBits[i].SetState(flag);
            }
        }

        public override void SetSlotSelection(int slotNo)
        {
            int itemsCount = m_DoSlotSelectionBits.Count;
            if (itemsCount == 0) return;

            for (int i = 0; i < itemsCount; i++)
            {
                bool flag = ((slotNo >> i) & 0x01) > 0;
                m_DoSlotSelectionBits[i].SetState(flag);
            }
        }

        public override int GetSelectedSlotNo()
        {
            int itemsCount = m_DoSlotSelectionBits.Count;
            if (itemsCount == 0) return 0;

            int slotNo = 0;
            for (int i = 0; i < itemsCount; i++)
            {
                slotNo |= ((m_DoSlotSelectionBits[i].GetState() ? 1 : 0) << i);
            }

            return slotNo;
        }

        public override void SetExternalSpeedRatio()
        {
            // Speed Selection
            string ratio;
            if (m_GenInfos.AutoMode)
                ratio = m_SetupRobotSpeedSelect.GetValue<string>();
            else
                ratio = m_SetupRobotSpeedManual.GetValue<string>();

            for (int i = 0; i < m_DoExternalSpeedCommandBits.Count; i++)
            {
                if (ratio == (RobotSpeed.RATIO_10 + i).ToString())
                    DoExternalSpeedCommandBits[i].SetState(true);
                else
                    DoExternalSpeedCommandBits[i].SetState(false);
            }

            // Speed Limit Mode
            bool speedLimitMode = (m_SetupRobotSpeedLimitMode.GetValue<string>() == bool.TrueString);
            bool curMode = m_DoSpeedLimitMode.GetState();
            if (curMode != speedLimitMode)
                DoSpeedLimitMode.SetState(speedLimitMode);
        }

        public override void SetGlassThickness()
        {
            int kindness = 1;   // dspcrassus - 일단 Tianma4.5에서는 기본값 '1'으로 설정... Robot Setup 항목으로 변경해야 함
            int count = m_DoKindOfGlassSettingBits.Count;
            for (int i = 0; i < count; i++)
            {
                bool flag = ((kindness >> i) & 0x01) > 0;
                m_DoKindOfGlassSettingBits[i].SetState(flag);
            }
        }

        public override void SetAdsorptionTime()
        {
            int itemsCount = m_DoAdsorptionTimeBits.Count;
            int adsorptionTime = (int)(m_SetupRobotAdsorption.GetValue<float>() * 10);

            if ((int)(m_Adsorption * 10) == adsorptionTime) return;

            m_Adsorption = (float)(adsorptionTime / 10);

            for (int i = 0; i < itemsCount; i++)
            {
                bool flag = ((adsorptionTime >> i) & 0x01) > 0;
                m_DoAdsorptionTimeBits[i].SetState(flag);
            }
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
            if (hand == LoaderRobotHand.upperHand && m_DiUpperGlassDetectConfirm.GetState() == true &&
                UpperHandStage.GlassExist)
            {
                UpperHandStage.Glass.OrgPortNo = portId + 1;
                UpperHandStage.Glass.OrgSlotNo = slotId + 1;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && m_DiLowerGlassDetectConfirm.GetState() == true &&
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
            if (hand == LoaderRobotHand.upperHand && m_DiUpperGlassDetectConfirm.GetState() == true &&
                UpperHandStage.GlassExist)
            {
                UpperHandStage.Glass.TargetPortNo = portId + 1;
                UpperHandStage.Glass.TargetSlotNo = slotId + 1;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && m_DiLowerGlassDetectConfirm.GetState() == true &&
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
            if (hand == LoaderRobotHand.upperHand && m_DiUpperGlassDetectConfirm.GetState() == true &&
                UpperHandStage.GlassExist == true)
            {
                gls = UpperHandStage.Glass;
                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && m_DiLowerGlassDetectConfirm.GetState() == true &&
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
            if (hand == LoaderRobotHand.upperHand && m_DiUpperGlassDetectConfirm.GetState() == true)
            {
                UpperHandStage.Glass = gls;
                UpperHandStage.GlassExist = true;

                m_UpperHandGlsData[0] = gls.OrgPortNo;
                m_UpperHandGlsData[1] = gls.OrgSlotNo;

                return true;
            }
            else if (hand == LoaderRobotHand.lowerHand && m_DiLowerGlassDetectConfirm.GetState() == true)
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
            return m_ExternalControlSeq.Do();
        }

        public override int SeqRobotHome()
        {
            return m_HomeSeq.Do();
        }

        public override int SeqRobotAlarm()
        {
            return m_AlarmSeq.Do();
        }

        public void InitExternalSeq()
        {
            ExternalControl = nxEX_CONTROL.EX_NONE;
            m_ExternalControlSeq.InitSeq();
        }

        public override void InitJobSeq()
        {
            m_ManualMotionStrobeReq = false;
            m_GetPutSeq.InitSeq();
        }

        public override void InitScrapSeq()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqGetGlass(LoaderRobotHand hand, int portId, int slotId)
        {
            nxOP_HAND opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = nxOP_HAND.UPPER_HAND;
            else opHand = nxOP_HAND.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, nxOP_PATTERN.GET, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqPutGlass(LoaderRobotHand hand, int portId, int slotId)
        {
            nxOP_HAND opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = nxOP_HAND.UPPER_HAND;
            else opHand = nxOP_HAND.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, nxOP_PATTERN.PUT, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqGetStandby(LoaderRobotHand hand, int portId, int slotId)
        {
            nxOP_HAND opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = nxOP_HAND.UPPER_HAND;
            else opHand = nxOP_HAND.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, nxOP_PATTERN.GET_PREPARE, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqPutStandby(LoaderRobotHand hand, int portId, int slotId)
        {
            nxOP_HAND opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = nxOP_HAND.UPPER_HAND;
            else opHand = nxOP_HAND.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHand, nxOP_PATTERN.PUT_PREPARE, portId, slotId);
            return m_GetPutSeq.Do();
        }

        public override int SeqYAlign(LoaderRobotHand hand, int portId)
        {
            nxOP_HAND opHand;
            if (hand == LoaderRobotHand.upperHand) opHand = nxOP_HAND.UPPER_HAND;
            else opHand = nxOP_HAND.LOWER_HAND;
            m_YAlignSeq.SetCondition(opHand, portId);

            return m_YAlignSeq.Do();
        }

        public override int SeqProcessCheck()
        {
            return m_ProcessCheckSeq.Do();
        }

        public override int SeqWait(int portId, int slotId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqExchange(LoaderRobotHand handPut, int portId, int slotId)
        {
            nxOP_HAND opHandPut;
            if (handPut == LoaderRobotHand.upperHand) opHandPut = nxOP_HAND.UPPER_HAND;
            else opHandPut = nxOP_HAND.LOWER_HAND;
            m_GetPutSeq.SetCondition(opHandPut, nxOP_PATTERN.EXCHANGE, portId, slotId);
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
            bool interfering = true;
            interfering &= ((int)m_Cube - 1 == portId);
            interfering &= (m_Pattern == nxOP_PATTERN.EXCHANGE || m_Pattern == nxOP_PATTERN.GET || m_Pattern == nxOP_PATTERN.PUT);
            interfering &= DiPatternOfOperationIsPerformed.GetState();

            return interfering;
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

                if (m_DiRobotHold.GetState()) return false;

                if (m_ManualHomeReturnReq)
                {
                    m_ManualHomeReturnReq = false;
                    action = RobotActionType.ReturnHome;
                    rv = true;
                }
                else
                {
                    if (m_ManualHand == nxOP_HAND.UPPER_HAND)
                        handManual = LoaderRobotHand.upperHand;
                    else
                        handManual = LoaderRobotHand.lowerHand;

                    portIdManual = (int)m_ManualCube - 1;
                    slotIdManual = m_ManualSlotNo - 1;

                    switch (m_ManualPattern)
                    {
                        case nxOP_PATTERN.GET_PREPARE:
                            actionManual = RobotActionType.GetStandBy;
                            break;
                        case nxOP_PATTERN.PUT_PREPARE:
                            actionManual = RobotActionType.PutStandBy;
                            break;
                        case nxOP_PATTERN.Y_ALIGN:
                            actionManual = RobotActionType.Y_Align;
                            break;
                        case nxOP_PATTERN.VCR_READ:
                            actionManual = RobotActionType.VcrRead;
                            break;
                        case nxOP_PATTERN.EXCHANGE:
                            {
                                // No Operation Tianma45
                            }
                            break;
                        case nxOP_PATTERN.GET:
                            actionManual = RobotActionType.Get;
                            break;
                        case nxOP_PATTERN.PUT:
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
            }
            return rv;
        }
        #endregion

        #region Methods
        public bool IsDeliveryPreparationComp(int eqpNo)
        {
            if (m_DiCompleteDeliveryBits.Count < eqpNo)
            {
                return false;
            }
            else
            {
                return m_DiCompleteDeliveryBits[eqpNo - 1].GetState();
            }
        }

        protected void CreateSimulatedIoCollection()
        {
            //if(m_Simul.IoMapping)
            //{
            //    m_MapResultInputs.Clear();

            //    for(int i = 0; i < m_MaxSlotCount; i++)
            //    {
            //        IoDigitalInput io = new IoDigitalInput();
            //        io.Name = "di" + XFunc.FilterigName(this.Name) + "MapResult" + string.Format("{0}", (i + 1));
            //        m_MapResultInputs.Add(io);
            //    }
            //}
        }

        private bool IsReceptionComp(string ioName)
        {
            bool complete = false;
            if (ioName == m_DiCompletionOfReceptionOfKindData.Name)
            {
                complete = m_DiCompletionOfReceptionOfKindData.GetState();
            }
            else if (ioName == m_DiCompletionOfReceptionOfMotionPatternData.Name)
            {
                complete = m_DiCompletionOfReceptionOfMotionPatternData.GetState();
            }
            else if (ioName == m_DiCompletionOfReceptionOfSlotData.Name)
            {
                complete = m_DiCompletionOfReceptionOfSlotData.GetState();
            }

            return complete;
        }

        private bool IsCubeInterference(nxOP_CUBE cube)
        {
            int index = (int)cube - 1;

            return m_DiInterfereCubeBits[index].GetState();
        }

        private int GetInterfereTarget()	// Parsing Cube No from Operation Pattern 
        {
            int cubeNo = 0;
            for (int i = 0; i < (int)nxOP_CUBE.MAX_CUBE; i++)
            {
                if (IsCubeInterference(nxOP_CUBE.PORT1 + i))
                {
                    cubeNo = i + (int)nxOP_CUBE.PORT1;
                    break;
                }
            }
            return cubeNo;
        }

        public int GetPatternOfOperation()
        {
            int patternNo = 0;
            int itemsCount = m_DiPatternOfOperationBits.Count;

            for (int i = 0; i < itemsCount; i++)
            {
                patternNo |= ((m_DiPatternOfOperationBits[i].GetState() ? 1 : 0) << i);
            }

            return patternNo;
        }

        public void SetEnteranceCubeBits(nxOP_CUBE cube, bool enter)	// 해당 Cube에 Robot의 진입 조건 Interlock 상태를 Set
        {
            int cubeId = (int)cube - (int)nxOP_CUBE.PORT1;
            int stageId = (int)cube - (int)nxOP_CUBE.STAGE1;

            m_DoEnterProhibitCubeBits[cubeId].SetState(!enter);     // Prohibition : OFF - Enter

            if (stageId >= 0)
            {
                m_DoEnterPassableCubeBits[stageId].SetState(enter);	// Passable : ON - Enter (Only Stage)
            }
        }

        private int GetPatternNo(nxOP_PATTERN motion, nxOP_HAND hand, nxOP_CUBE cube)
        {
            int patternNo = 0;
            if (motion == nxOP_PATTERN.Y_ALIGN || motion == nxOP_PATTERN.VCR_READ)
            {
                patternNo = (int)hand + (int)motion;
            }
            else if (motion == nxOP_PATTERN.EXCHANGE)
            {
                patternNo = (int)hand + (int)motion + (int)cube - (int)nxOP_CUBE.STAGE1;
            }
            else
            {
                patternNo = (int)hand + (int)motion + (int)cube;
            }

            return patternNo;
        }

        public int SetPatternOfOperation(nxOP_PATTERN motion, nxOP_HAND hand, nxOP_CUBE cube)
        {
            int itemsCount = m_DoPatternOfOperationBits.Count;
            if (itemsCount == 0)
            {
                return 0;
            }

            m_Pattern = motion;
            m_Hand = hand;
            m_Cube = cube;

            //nxOP_HAND opHand;

            int patternNo = GetPatternNo(motion, hand, cube);
            m_PatternNo = patternNo;

            for (int i = 0; i < itemsCount; i++)
            {
                bool flag = ((patternNo >> i) & 0x01) > 0;
                m_DoPatternOfOperationBits[i].SetState(flag);
            }

            return patternNo;
        }

        public void SetPatternOfOperationNxSimul(nxOP_PATTERN motion, nxOP_HAND hand, nxOP_CUBE cube)
        {
            if (!Simul.Device) return;

            int itemsCount = m_DiPatternOfOperationBits.Count;
            if (itemsCount <= 0) return;

            int patternNo = GetPatternNo(motion, hand, cube);
            for (int i = 0; i < itemsCount; i++)
            {
                bool flag = ((patternNo >> i) & 0x01) > 0;
                m_DiPatternOfOperationBits[i].SetState(flag);
            }
        }

        public void ResetPatternOfOperation()
        {
            int itemsCount = m_DoPatternOfOperationBits.Count;
            if (itemsCount == 0)
                return;

            for (int i = 0; i < itemsCount; i++)
            {
                m_DoPatternOfOperationBits[i].SetState(false);
            }
        }

        public void SetEffectiveParam(nxEFFECT_PARAM effect, bool set)
        {
            bool enable = set;

            switch (effect)
            {
                case nxEFFECT_PARAM.EFFECT_X:
                    {
                        enable &= (m_SetupRobotEffectAlignX.GetValue<string>() == bool.TrueString);
                        m_DoAlignEffective_T_X.SetState(enable);
                    }
                    break;
                case nxEFFECT_PARAM.EFFECT_Y:
                    {
                        enable &= (m_SetupRobotEffectAlignY.GetValue<string>() == bool.TrueString);
                        m_DoAlignEffective_Y.SetState(enable);
                    }
                    break;
            }
        }

        //public void SetDeliveryCompletion(int eqpNo)
        public void SetDeliveryCompletion(int eqpNo, bool flag)
        {
            int itemsCount = m_DoCompletionOfDeliveryBits.Count;
            if (itemsCount == 0)
            {
                return;
            }

            m_DoCompletionOfDeliveryBits[eqpNo - 1].SetState(flag);
            // dsptemp - DoCompletionOfDeliveryBits : Binary는 아닐텐데... 밑에 처럼 하면 안되겠지
            //for (int i = 0; i < itemsCount; i++)
            //{
            //    bool flag = ((eqpNo >> i) & 0x01) > 0;
            //    m_DoCompletionOfDeliveryBits[i].SetState(flag);
            //}
        }

        public void ResetRobotIfSignal()
        {
            ResetPatternOfOperation();
            SetSlotSelection(0);
            DoMotionPatternReadingStrobe.SetState(false);
            DoCompletionOfPatternCheckOfOperation.SetState(false);
            DoCompletionOfGetOperationReceptionist.SetState(false);
            DoCompletionOfPutOperationReceptionist.SetState(false);
            DoCompletionOfExchangeOperationReceptionst.SetState(false);

            for (int i = 0; i < DoCompletionOfDeliveryBits.Count; i++)
                DoCompletionOfDeliveryBits[i].SetState(false);

            #region Reset Robot Side Signal for Simulation 
            if (Simul.Instance.Device)
            {
                DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(true);
                DiCompletionOfReceptionOfMotionPatternData.SetState(false);
                DiPatternOfOperationIsPerformed.SetState(false);
                DiOperationGet.SetState(false);
                DiOperationPut.SetState(false);
                DiOperationExchange.SetState(false);
                DiCompleteGet.SetState(false);
                DiCompletePut.SetState(false);
                DiCompleteExchange.SetState(false);
            }
            #endregion
        }

        // For UserControl - LoaderRobot
        private bool IsUpperGlassDetectSensorOn()
        {
            if (!this.Initialized) return false;

            bool bRv = true;
            bRv &= m_DiUpperGlassDetectConfirm.GetState();

            return bRv;
        }
        // For UserControl - LoaderRobot
        private bool IsLowerGlassDetectSensorOn()
        {
            if (!this.Initialized) return false;

            bool bRv = true;
            bRv &= m_DiLowerGlassDetectConfirm.GetState();

            return bRv;
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
            m_DiInterfereCubeBits.MaxSimulateCount = m_MaxCubeNo;
            m_DiCompleteDeliveryBits.MaxSimulateCount = m_MaxStageNo;
            m_DiSlotNoBits.MaxSimulateCount = 8; //8 Bits
            m_DiPatternOfOperationBits.MaxSimulateCount = 8; //8 Bits
            m_DoEnterProhibitCubeBits.MaxSimulateCount = m_MaxCubeNo;
            m_DoEnterPassableCubeBits.MaxSimulateCount = m_MaxStageNo;
            m_DoKindOfGlassSettingBits.MaxSimulateCount = 4; //4 Bits
            m_DoCompletionOfDeliveryBits.MaxSimulateCount = m_MaxStageNo;
            m_DoSlotSelectionBits.MaxSimulateCount = 8; //8 Bits
            m_DoPatternOfOperationBits.MaxSimulateCount = 8; //8 Bits
            m_DoExternalSpeedCommandBits.MaxSimulateCount = 8; //8 Bits
            m_DoAdsorptionTimeBits.MaxSimulateCount = 8; //8 Bits


            bool ok = true;
            ok &= GenerateAssociatedDevices();

            // ////////////////////////////////////////////////////////////////////////////////////////
            // // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion
            ok &= (m_DiRunning != null);
            ok &= (m_DiServoOn != null);
            ok &= (m_DiTopOfMasterJob != null);
            ok &= (m_DiRobotHold != null);
            ok &= (m_DiIntermediateStart != null);
            ok &= (m_DiSafetyPlug != null);
            ok &= (m_DiPowerOn != null);

            ok &= (m_DiAlarmErrorOccurred != null);
            ok &= (m_DiAlarmBattery != null);
            ok &= (m_DiErrorUpperVacuum != null);
            ok &= (m_DiErrorLowerVacuum != null);
            ok &= (m_DiErrorUpperAlignment != null);
            ok &= (m_DiErrorLowerAlignment != null);
            ok &= (m_DiErrorUpperAlign_Y != null);
            ok &= (m_DiErrorLowerAlign_Y != null);
            ok &= (m_DiAbnormalityInGlassFallUpper != null);
            ok &= (m_DiAbnormalityInGlassFallLower != null);

            ok &= (m_DiRemoteModeSelect != null);
            ok &= (m_DiPlayModeSelect != null);
            ok &= (m_DiTeachModeSelect != null);
            ok &= (m_DiTeachLockSet != null);

            ok &= (m_DiCompletionOfReceptionOfKindData != null);
            ok &= (m_DiCompletionOfReceptionOfMotionPatternData != null);
            ok &= (m_DiCompletionOfReceptionOfSlotData != null);
            ok &= (m_DiCompletionOfPatternStrobeReadingPreparationOfOperation != null);
            ok &= (m_DiPatternOfOperationIsPerformed != null);

            ok &= (m_DiOperationOriginPoint != null);
            ok &= (m_DiHomePositionReturn != null);
            ok &= (m_DiVcrReadingCompletion != null);
            ok &= (m_DiCompletionOfPreparationToReadsVCR != null);
            ok &= (m_DiMovingToVcrPosition != null);
            ok &= (m_DiOperationPut != null);
            ok &= (m_DiOperationGet != null);
            ok &= (m_DiOperationPreapare != null);
            ok &= (m_DiOperationAlign_Y != null);
            ok &= (m_DiOperationExchange != null);
            ok &= (m_DiCompletePut != null);
            ok &= (m_DiCompleteGet != null);
            ok &= (m_DiCompletePrepare != null);
            ok &= (m_DiCompleteAlign_Y != null);
            ok &= (m_DiCompleteExchange != null);

            ok &= (m_DiUpperAdsorptionConfirm != null);
            ok &= (m_DiLowerAdsorptionConfirm != null);
            ok &= (m_DiUpperGlassDetectConfirm != null);
            ok &= (m_DiLowerGlassDetectConfirm != null);

            ok &= (m_DoExternalStart != null);
            ok &= (m_DoExternalHold != null);
            ok &= (m_DoExternalServoOn != null);
            ok &= (m_DoExternalServoOff != null);
            ok &= (m_DoCallMasterJob != null);
            ok &= (m_DoUpperVacuumOnRequest != null);
            ok &= (m_DoLowerVacuumOnRequest != null);
            ok &= (m_DoUpperVacuumOffRequest != null);
            ok &= (m_DoLowerVacuumOffRequest != null);

            ok &= (m_DoMotionPatternReadingStrobe != null);
            ok &= (m_DoCompletionOfPatternCheckOfOperation != null);

            ok &= (m_DoSpeedLimitMode != null);

            ok &= (m_DoHomePositionReturnRequest != null);
            ok &= (m_DoCompletionOfPutOperationReceptionist != null);
            ok &= (m_DoCompletionOfGetOperationReceptionist != null);
            ok &= (m_DoCompletionOfPreparationReception != null);
            ok &= (m_DoCompletionCheckOfVcrOperation != null);
            ok &= (m_DoCompletionCheckOfY_AlignOperation != null);
            ok &= (m_DoCompletionOfExchangeOperationReceptionst != null);
            ok &= (m_DoExchangeOperationCommand != null);
            ok &= (m_DoVcrReadingCommand != null);
            ok &= (m_DoAlignEffective_T_X != null);
            ok &= (m_DoAlignEffective_Y != null);

            ok &= (m_DoAlarmErrorReset != null);
            ok &= (m_DoPlcNormal != null);

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
                ALM_ErrorOccuredAlarm = new Alarm(this.Name + " Alarm / Error Occurred", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_BatteryAlarm = new Alarm(this.Name + " Battery Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_UpperHandVacuumError = new Alarm(this.Name + " Vacuum Error (Upper Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LowerHandVacuumError = new Alarm(this.Name + " Vacuum Error (Lower Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_UpperHandAlignmentError = new Alarm(this.Name + " Alignment Error (Upper Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LowerHandAlignmentError = new Alarm(this.Name + " Alignment Error (Lower Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_UpperHandYAlignError = new Alarm(this.Name + " Y-Align Error (Upper Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LowerHandYAlignError = new Alarm(this.Name + " Y-Align Error (Lower Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_UpperAbnormalGlassFall = new Alarm(this.Name + " Abnormalities in glass fall (Upper Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_LowerAbnormalGlassFall = new Alarm(this.Name + " Abnormalities in glass fall (Lower Hand)", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupLoaderInfoProvider = SetupLoaderInfoProvider.Instance;
                m_SetupRobotPinUpDnUse = new TagSetupInfo(this.Name + " Pin Up/Down Case Use", OptionType.None, OptionFormat.Use, UnitType.None, "null");
                m_SetupRobotEffectAlignY = new TagSetupInfo(this.Name + " Y-Alignment Effective", OptionType.None, OptionFormat.Enable, UnitType.None, "null");
                m_SetupRobotEffectAlignX = new TagSetupInfo(this.Name + " Θ,X Alignment Effective", OptionType.None, OptionFormat.Enable, UnitType.None, "null");
                m_SetupRobotVirtualMode = new TagSetupInfo(this.Name + " Virtual Conveyance Mode", OptionType.None, OptionFormat.Boolean, UnitType.None, bool.FalseString);
                m_SetupRobotSpeedLimitMode = new TagSetupInfo(this.Name + " Speed Limit Mode", OptionType.None, OptionFormat.Boolean, UnitType.None, bool.FalseString);
                m_SetupRobotSpeedSelect = new TagSetupInfo(this.Name + " External Speed Select", OptionType.Alternative, OptionFormat.RobotSpeed, UnitType.None, RobotSpeed.RATIO_10.ToString());
                m_SetupRobotSpeedManual = new TagSetupInfo(this.Name + " Manual Mode Speed Select", OptionType.Alternative, OptionFormat.RobotSpeed, UnitType.None, RobotSpeed.RATIO_10.ToString());
                m_SetupRobotAdsorption = new TagSetupInfo(this.Name + " Adsorption Time", OptionType.None, OptionFormat.Float, UnitType.sec, "0.1", "0.1", "25.5");
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotPinUpDnUse);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotEffectAlignY);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotEffectAlignX);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotVirtualMode);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotSpeedLimitMode);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotSpeedSelect);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotSpeedManual);
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupRobotAdsorption);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건		
                m_ExternalControlSeq = new YaskawaNx100ExternalControlSeq(this);
                m_HomeSeq = new YaskawaNx100HomeSeq(this);
                m_GetPutSeq = new YaskawaNx100GetPutGlassSeq(this);
                m_YAlignSeq = new YaskawaNx100YAlignSeq(this);
                m_ProcessCheckSeq = new YaskawaNx100ProcessCheckSeq(this);
                m_AlarmSeq = new YaskawaNx100AlarmSeq(this);

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
                    DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(true);
                    DiRemoteModeSelect.SetState(true);
                    DiSafetyPlug.SetState(true);
                    DiPowerOn.SetState(true);
                    foreach (IoDigitalInput io in DiInterfereCubeBits)
                    {
                        io.SetState(false);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                    foreach (IoDigitalOutput io in DoEnterProhibitCubeBits)
                    {
                        io.SetState(true);
                    }
                    foreach (IoDigitalOutput io in DoEnterPassableCubeBits)
                    {
                        io.SetState(false);
                    }
                    foreach (IoDigitalOutput io in DoKindOfGlassSettingBits)
                    {
                        io.SetState(false);
                    }
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
                m_Tag.SetValue(tagDescriptor.PatternOperationPerform, m_DiPatternOfOperationIsPerformed.GetState());
                m_Tag.SetValue(tagDescriptor.InterfereTarget, GetInterfereTarget());
                m_Tag.SetValue(tagDescriptor.OperationHand, GetOperationHand());
                m_Tag.SetValue(tagDescriptor.GetOperation, m_DiOperationGet.GetState());
                m_Tag.SetValue(tagDescriptor.PutOperation, m_DiOperationPut.GetState());
                m_Tag.SetValue(tagDescriptor.PrepareOperation, m_DiOperationPreapare.GetState());
                m_Tag.SetValue(tagDescriptor.UpHandGlassDetect, IsUpperGlassDetectSensorOn());
                m_Tag.SetValue(tagDescriptor.LoHandGlassDetect, IsLowerGlassDetectSensorOn());
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

        //public bool IsHandGlassExist(LoaderRobotHand hand)
        //{
        //    if (hand == LoaderRobotHand.upperHand)
        //    {
        //        return IsUpperHandGlassDetected() && m_UpperHandStage.GlassExist;
        //    }
        //    else
        //    {
        //        return IsLowerHandGlassDetected() && m_LowerHandStage.GlassExist;
        //    }
        //}

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

    public class YaskawaNx100ExternalControlSeq : XSeqFunction
    {
        #region Fields
        private YaskawaNX100 m_Robot = null;
        private nxEX_CONTROL m_ExCtrl = nxEX_CONTROL.EX_NONE;
        #endregion

        #region Constructor
        public YaskawaNx100ExternalControlSeq(YaskawaNX100 robot)
        {
            m_Robot = robot;
        }
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;

            if (m_ExCtrl == nxEX_CONTROL.EX_NONE)
                m_ExCtrl = m_Robot.ExternalControl;

            string seqName = "EX Control";

            switch (m_ExCtrl)
            {
                case nxEX_CONTROL.EX_SERVO_ON:
                    {
                        switch (seqNo)
                        {
                            case 0:
                                if (!m_Robot.DiServoOn.GetState() || !m_Robot.DiRunning.GetState())
                                {
                                    m_Robot.DoExternalServoOn.SetState(true);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Servo On [ON]");
                                    seqNo = 10;
                                }
                                else
                                {
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                }
                                break;
                            case 10:
                                if (m_Robot.DiServoOn.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot is On");

                                    m_Robot.DoExternalServoOn.SetState(false);
                                    m_Robot.DoExternalStart.SetState(true);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Servo On [OFF]");
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Start [ON]");
                                    seqNo = 20;
                                }
                                break;
                            case 20:
                                if (m_Robot.DiRunning.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot is Running");

                                    m_Robot.DoExternalStart.SetState(false);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Start [OFF]");
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                    returnValue = 0;
                                    seqNo = 0;
                                }
                                break;
                        }
                    }
                    break;
                case nxEX_CONTROL.EX_SERVO_OFF:
                    {
                        switch (seqNo)
                        {
                            case 0:
                                if (m_Robot.DiServoOn.GetState())
                                {
                                    m_Robot.DoExternalServoOff.SetState(true);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Servo Off [ON]");
                                    seqNo = 10;
                                }
                                else
                                {
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                }
                                break;
                            case 10:
                                if (!m_Robot.DiServoOn.GetState())
                                {
                                    m_Robot.DoExternalServoOff.SetState(false);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Servo Off [OFF]");
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                    returnValue = 0;
                                    seqNo = 0;
                                }
                                break;
                        }
                    }
                    break;
                case nxEX_CONTROL.EX_HOLD_ON:
                    {
                        switch (seqNo)
                        {
                            case 0:
                                if (!m_Robot.DiRobotHold.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Robot Hold Start");

                                    m_Robot.DoExternalHold.SetState(true);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Robot Hold [ON]");
                                    seqNo = 10;
                                }
                                else
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot is in Hold Status");
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                    returnValue = 0;
                                }
                                break;
                            case 10:
                                if (m_Robot.DiRobotHold.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot Hold [ON]");
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                    returnValue = 0;
                                    seqNo = 0;
                                }
                                break;
                        }
                    }
                    break;
                case nxEX_CONTROL.EX_HOLD_OFF:
                    {
                        switch (seqNo)
                        {
                            case 0:
                                if (m_Robot.DiRobotHold.GetState() || m_Robot.DoExternalHold.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Robot Hold Release Start");

                                    m_Robot.DoExternalHold.SetState(false);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Robot Hold [OFF]");
                                    seqNo = 10;
                                }
                                else if (!m_Robot.DiRobotHold.GetState() && !m_Robot.DoExternalHold.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Robot is not in Hold Status");
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                    returnValue = 0;
                                }
                                break;
                            case 10:
                                if (!m_Robot.DiRobotHold.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot Hold [OFF]");

                                    if (m_Robot.DiServoOn.GetState())
                                    {
                                        m_Robot.DoExternalStart.SetState(true);
                                        m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Start [ON]");
                                        seqNo = 20;
                                    }
                                    else
                                    {
                                        m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot Servo is Off already");
                                        m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                        m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                        seqNo = 0;
                                    }
                                }
                                break;
                            case 20:
                                if (m_Robot.DiRunning.GetState())
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot is Running");

                                    m_Robot.DoExternalStart.SetState(false);
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "External Start [OFF]");
                                    m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                    m_ExCtrl = nxEX_CONTROL.EX_NONE;
                                    seqNo = 0;
                                    returnValue = 0;
                                }
                                break;
                        }
                    }
                    break;
                case nxEX_CONTROL.EX_HOME_RETURN:
                    {
                        switch (seqNo)
                        {
                            case 0:
                                {
                                    m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot Home Return Start");
                                    seqNo = 10;
                                }
                                break;
                            case 10:
                                {
                                    int rv = m_Robot.SeqRobotHome();
                                    if (rv == 0)
                                    {
                                        m_Robot.SetLog(m_Robot.Name, seqName, 0, 0, "Robot Home Return Complete");
                                        m_Robot.ExternalControl = nxEX_CONTROL.EX_NONE;
                                        m_ExCtrl = nxEX_CONTROL.EX_NONE;

                                        seqNo = 0;
                                        returnValue = 0;
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }

    public class YaskawaNx100HomeSeq : XSeqFunction
    {
        #region Fields
        private YaskawaNX100 m_Robot = null;
        protected IServerManager m_Server;
        protected _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public YaskawaNx100HomeSeq(YaskawaNX100 robot)
        {
            m_Robot = robot;
            m_Server = m_Robot.ServerManager;
            m_GenInfos = m_Robot.GenInfos;
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
                    if (m_Robot.Simul.Device || m_Robot.DiRemoteModeSelect.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Remote Mode Selected : ON");

                        m_Robot.DoExternalServoOff.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] SERVO OFF : ON");

                        seqNo = 10;

                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiServoOn.SetState(false);
                            m_Robot.DiRunning.SetState(false);
                        }
                    }
                    break;

                case 10:
                    if (!m_Robot.DiServoOn.GetState() && !m_Robot.DiRunning.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Servo Power & Running : OFF");
                        m_Robot.DoExternalServoOff.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] SERVO OFF : OFF");

                        m_Robot.DoCallMasterJob.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] CALL MASTER JOB : ON");

                        seqNo = 20;

                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiTopOfMasterJob.SetState(true);
                        }
                    }
                    break;

                case 20:
                    if (m_Robot.DiTopOfMasterJob.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] TOP Of MASTER JOB : ON");

                        m_Robot.DoHomePositionReturnRequest.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] HOME Position Return Request : ON");

                        m_Robot.DoCallMasterJob.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] CALL MASTER JOB : OFF");

                        m_Robot.DoExternalServoOn.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] External Servo On : ON");

                        seqNo = 30;

                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiTopOfMasterJob.SetState(false);
                            m_Robot.DiServoOn.SetState(true);
                        }
                    }
                    break;

                case 30:
                    if (m_Robot.DiServoOn.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Robot Servo Power : ON");

                        m_Robot.DoExternalServoOn.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] External Servo On : OFF");

                        m_Robot.DoExternalStart.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] External Start : ON");
                        seqNo = 40;

                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiRunning.SetState(true);
                            m_Robot.DiHomePositionReturn.SetState(true);
                        }
                    }
                    break;

                case 40:
                    if (m_Robot.DiHomePositionReturn.GetState() && m_Robot.DiRunning.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Robot is Running : ON");
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Robot is Moving to HOME Position");

                        m_Robot.DoHomePositionReturnRequest.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] HOME Position Return Request : OFF");
                        m_Robot.DoExternalStart.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] External Start : OFF");

                        seqNo = 50;

                        if (m_Robot.Simul.Device)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                    }
                    else if (m_Robot.DiOperationOriginPoint.GetState() && m_Robot.DiRunning.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Robot is Running : ON");
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Robot Moving Home Complete");

                        m_Robot.DoHomePositionReturnRequest.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] HOME Position Return Request : OFF");
                        m_Robot.DoExternalStart.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[PLC] External Start : OFF");

                        m_GenInfos.CycleStop = true;
                        m_Robot.InitJobSeq();
                        returnValue = 0;
                        seqNo = 0;
                    }
                    break;

                case 50:
                    if (m_Robot.DiOperationOriginPoint.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Home", 0, 0, "[NX] Robot Moving Home Complete");

                        m_GenInfos.CycleStop = true;
                        m_Robot.InitJobSeq();
                        returnValue = 0;
                        seqNo = 0;
                    }
                    else if (m_Robot.Simul.Device)
                    {
                        if (XFunc.GetTickCount() - m_StartTicks > 3000)
                        {
                            m_Robot.DiHomePositionReturn.SetState(false);
                            m_Robot.DiOperationOriginPoint.SetState(true);
                        }
                    }
                    break;
            }
            m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }

    public class YaskawaNx100ProcessCheckSeq : XSeqFunction
    {
        #region Fields
        private YaskawaNX100 m_Robot = null;
        protected IServerManager m_Server;
        protected _GenInfoHandler m_GenInfos;

        private bool m_InTaskJob = false;
        private bool m_InManualJob = false;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public YaskawaNx100ProcessCheckSeq(YaskawaNX100 robot)
        {
            m_Robot = robot;
            m_Server = m_Robot.ServerManager;
            m_GenInfos = m_Robot.GenInfos;
        }
        #endregion

        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_GenInfos.AutoMode && m_Robot.IsRobotBusy())
                    {
                        m_InTaskJob = true;
                        m_InManualJob = false;
                        seqNo = 10;
                    }
                    else if (!m_GenInfos.AutoMode && m_Robot.IsRobotBusy())
                    {
                        m_InTaskJob = false;
                        m_InManualJob = true;
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_GenInfos.AutoMode && m_InTaskJob)
                    {
                        m_Robot.ExternalControl = nxEX_CONTROL.EX_SERVO_OFF;
                        m_Robot.InProcess = false;
                        returnValue = 0;
                        seqNo = 100;
                    }
                    else if (m_GenInfos.AutoMode && m_InManualJob)
                    {
                        m_Robot.ExternalControl = nxEX_CONTROL.EX_SERVO_OFF;
                        m_Robot.InProcess = false;
                        returnValue = 0;
                        seqNo = 100;
                    }
                    else if (!m_Robot.IsRobotBusy())
                    {
                        m_InTaskJob = false;
                        m_InManualJob = false;
                        seqNo = 0;
                    }
                    break;
                case 100:
                    if (m_Robot.IsRobotReady())
                    {
                        m_InTaskJob = false;
                        m_InManualJob = false;
                        seqNo = 0;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return returnValue;
        }
    }

    public class YaskawaNx100GetPutGlassSeq : XSeqFunction
    {
        #region Fields
        private YaskawaNX100 m_Robot = null;
        protected IServerManager m_Server;
        private int m_PortId = -1;
        private int m_SlotId = -1;
        private int m_SlotNo = -1;
        private nxOP_HAND m_Hand;
        private nxOP_PATTERN m_OpPattern;

        #endregion

        #region Constructor
        public YaskawaNx100GetPutGlassSeq(YaskawaNX100 robot)
        {
            m_Robot = robot;
            m_Server = m_Robot.ServerManager;
            m_SeqFunName = "GetPutGlassSeq";
        }
        #endregion

        #region Methods
        public void SetCondition(nxOP_HAND hand, nxOP_PATTERN pattern, int portId, int slotId)
        {
            m_Hand = hand;
            m_OpPattern = pattern;
            m_PortId = portId;
            m_SlotId = slotId;

            if (m_PortId < (int)nxOP_CUBE.STAGE1 - 1)
                m_SlotNo = slotId + 1;
            else
                m_SlotNo = 1;   // EQP에 대한 JOB 진행 시 SlotNo 1로 Set
        }
        #endregion

        #region Override
        public override int Do()
        {
            //int rv = -1;
            int returnValue = -1;
            int seqNo = m_SeqNo;
            if (m_Robot.AlarmCondition != RobotAlarmCondition.NoAlarm)
            {
                m_Robot.SetEffectiveParam(nxEFFECT_PARAM.EFFECT_X, false);
                m_Robot.SetEffectiveParam(nxEFFECT_PARAM.EFFECT_Y, false);
                m_Robot.InProcess = false;
                m_SeqNo = 0;
                return (int)m_Robot.AlarmCondition;
            }

            string seqName = "GET/PUT";

            switch (seqNo)
            {
                case 0:
                    if (m_Robot.DiRunning.GetState() == false)
                    {
                        m_Robot.DoExternalStart.SetState(true); // External Start는 최초에 Robot Running 할 때만 필요
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "External Start ON");
                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiRunning.SetState(true);
                        }

                        seqNo = 10;
                    }
                    else
                        seqNo = 15;
                    break;

                case 10:
                    if (m_Robot.DiRunning.GetState() == true)
                    {
                        m_Robot.DoExternalStart.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "External Start OFF");

                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(true);
                        }

                        seqNo = 15;
                    }
                    break;

                case 15:
                    if (m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState() == true)
                    {
                        m_Robot.SetLog(m_Robot.Name, m_SeqFunName, m_PortId + 1, m_SlotId + 1, "Start : " + m_OpPattern.ToString());

                        m_Robot.SetPatternOfOperation(m_OpPattern, m_Hand, (nxOP_CUBE)(m_PortId + 1));
                        m_Robot.SetSlotSelection(m_SlotNo);
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Pattern Data Set");

                        bool virtualMode = (m_Robot.SetupRobotVirtualMode.GetValue<string>() == bool.TrueString);
                        m_Robot.DoVirtualConveyanceMode.SetState(virtualMode);

                        if (m_OpPattern == nxOP_PATTERN.PUT && (nxOP_CUBE)(m_PortId + 1) == nxOP_CUBE.STAGE1)
                            m_Robot.SetEffectiveParam(nxEFFECT_PARAM.EFFECT_Y, true);    // Put Into In Stage
                        else if (m_OpPattern == nxOP_PATTERN.GET && (nxOP_CUBE)(m_PortId + 1) < nxOP_CUBE.STAGE1)
                            m_Robot.SetEffectiveParam(nxEFFECT_PARAM.EFFECT_X, true);   // Get From Cst

                        m_Robot.InProcess = true;
                        seqNo = 20;
                    }
                    break;

                case 20:
                    if (m_SlotNo == m_Robot.GetSelectedSlotNo())
                    {
                        m_Robot.DoMotionPatternReadingStrobe.SetState(true);
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Motion Pattern Reading Strobe ON");

                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(false);
                            m_Robot.SetPatternOfOperationNxSimul(m_OpPattern, m_Hand, (nxOP_CUBE)(m_PortId + 1));
                            m_Robot.DiCompletionOfReceptionOfMotionPatternData.SetState(true);
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 30;
                    }
                    break;

                case 30:
                    if (m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState() == false &&
                        m_Robot.DiCompletionOfReceptionOfMotionPatternData.GetState() == true)
                    {
                        int patternNoRobot = m_Robot.GetPatternOfOperation();
                        bool patternCheckOk = m_Robot.PatternNo == patternNoRobot;
                        patternCheckOk &= (m_PortId >= (int)nxOP_CUBE.STAGE1 - 1 || m_SlotId == m_Robot.GetSlotNo() - 1);

                        if (m_Robot.Simul.Device || patternCheckOk)  // m_PatternNo 의 Tag와 GetPatternOfOperation()의 Return Value를 비교해야함
                        {
                            m_Robot.DoMotionPatternReadingStrobe.SetState(false);
                            m_Robot.DoCompletionOfPatternCheckOfOperation.SetState(true);
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Motion Pattern Reading Strobe OFF");
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Pattern Check Of Operation ON");

                            if (m_Robot.Simul.Device)
                            {
                                m_Robot.DiCompletionOfReceptionOfMotionPatternData.SetState(false);
                            }

                            seqNo = 40;
                        }
                        else if (XFunc.GetTickCount() - m_StartTicks > 10000)
                        {
                            returnValue = 0;
                            seqNo = 0;
                        }
                    }
                    break;

                case 40:
                    if (m_Robot.DiCompletionOfReceptionOfMotionPatternData.GetState() == false)
                    {
                        m_Robot.SetSlotSelection(0);
                        m_Robot.ResetPatternOfOperation();
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Pattern Data Reset");

                        m_Robot.DoCompletionOfPatternCheckOfOperation.SetState(false);
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Pattern Check Of Operation OFF");

                        if (m_Robot.Simul.Device) m_Robot.DiPatternOfOperationIsPerformed.SetState(true);

                        seqNo = 50;
                    }
                    break;

                case 50:
                    if (m_Robot.DiPatternOfOperationIsPerformed.GetState() == true)
                    {
                        bool interfereAction = false;
                        interfereAction |= (m_OpPattern == nxOP_PATTERN.PUT || m_OpPattern == nxOP_PATTERN.GET || m_OpPattern == nxOP_PATTERN.EXCHANGE);
                        if (interfereAction)
                        {
                            m_Robot.SetEnteranceCubeBits((nxOP_CUBE)(m_PortId + 1), true);
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Enterance Cube Bits SET");
                            seqNo = 60;
                        }
                        else if ((m_OpPattern == nxOP_PATTERN.GET_PREPARE || m_OpPattern == nxOP_PATTERN.PUT_PREPARE) &&
                                m_Robot.DiOperationPreapare.GetState())
                        {
                            seqNo = 70;
                        }

                        if (m_Robot.Simul.Device && interfereAction)
                        {
                            m_Robot.DiInterfereCubeBits[m_PortId].SetState(true);
                            if (m_OpPattern == nxOP_PATTERN.EXCHANGE)
                            {
                                m_Robot.DiOperationGet.SetState(true);
                                m_Robot.DiOperationExchange.SetState(true);
                            }
                            else if (m_OpPattern == nxOP_PATTERN.GET)
                                m_Robot.DiOperationGet.SetState(true);
                            else if (m_OpPattern == nxOP_PATTERN.PUT)
                                m_Robot.DiOperationPut.SetState(true);
                            else if (m_OpPattern == nxOP_PATTERN.GET_PREPARE || m_OpPattern == nxOP_PATTERN.PUT_PREPARE)
                                m_Robot.DiOperationPreapare.SetState(true);

                            m_StartTicks = XFunc.GetTickCount();
                        }
                    }
                    break;

                case 60:
                    if (m_Robot.DiInterfereCubeBits[m_PortId].GetState() == true || m_Robot.SetupRobotVirtualMode.GetValue<string>() == bool.TrueString)
                    {
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, string.Format("CUBE[{0}] is in Interfere", m_PortId + 1));
                        if (m_Robot.Simul.Device) // Simulate Glass Detection
                        {
                            if (GetElapsedTicks() < 2000) break;
                            else
                            {
                                if (m_OpPattern == nxOP_PATTERN.EXCHANGE)
                                {
                                    if (m_Hand == nxOP_HAND.UPPER_HAND && !m_Robot.IsLowerHandGlassDetected())
                                        m_Robot.SetLowerHandGlassDetected(true);    // Lower Hand GET Case
                                    else if (m_Hand == nxOP_HAND.LOWER_HAND && !m_Robot.IsUpperHandGlassDetected())
                                        m_Robot.SetUpperHandGlassDetected(true);    // Upper Hand GET Case
                                }
                                else if (m_OpPattern == nxOP_PATTERN.GET)
                                {
                                    if (m_Hand == nxOP_HAND.UPPER_HAND && !m_Robot.IsUpperHandGlassDetected())
                                        m_Robot.SetUpperHandGlassDetected(true);    // Upper Hand GET Case
                                    else if (m_Hand == nxOP_HAND.LOWER_HAND && !m_Robot.IsLowerHandGlassDetected())
                                        m_Robot.SetLowerHandGlassDetected(true);    // Lower Hand GET Case
                                }
                                else if (m_OpPattern == nxOP_PATTERN.PUT)
                                {
                                    if (m_Hand == nxOP_HAND.UPPER_HAND && m_Robot.IsUpperHandGlassDetected())
                                        m_Robot.SetUpperHandGlassDetected(false);   // Upper Hand PUT Case
                                    else if (m_Hand == nxOP_HAND.LOWER_HAND && m_Robot.IsLowerHandGlassDetected())
                                        m_Robot.SetLowerHandGlassDetected(false);   // Lower Hand PUT Case
                                }
                                m_StartTicks = XFunc.GetTickCount();
                            }
                        }

                        if (m_OpPattern == nxOP_PATTERN.EXCHANGE && m_Robot.DiOperationGet.GetState() && m_Robot.DiOperationExchange.GetState())
                        {
                            // Exchange Operation is Started (GET)
                            seqNo = 70;
                        }
                        else if ((m_OpPattern == nxOP_PATTERN.GET && m_Robot.DiOperationGet.GetState()) ||
                                (m_OpPattern == nxOP_PATTERN.PUT && m_Robot.DiOperationPut.GetState()))
                        {
                            // Get Operation is Started
                            // dspcrassus - Robot Hand Up/Dn 동작 없이 EQP의 Pin Up/Dn 으로 Glass 인계시 사용
                            if (m_Robot.SetupRobotPinUpDnUse.GetValue<string>() == bool.TrueString && m_PortId >= (int)nxOP_CUBE.STAGE1 - 1)
                            {
                                if (m_Robot.IsDeliveryPreparationComp(m_PortId + 1 - (int)nxOP_CUBE.PORT8))   // dsptemp - STAGE1 부터 1을 넘겨주자...
                                {
                                    m_Robot.SetDeliveryCompletion(m_PortId + 1 - (int)nxOP_CUBE.PORT8, true);
                                    seqNo = 65;
                                }
                                else if (m_Robot.Simul.Device)
                                {
                                    m_Robot.DiCompleteDeliveryBits[m_PortId - (int)nxOP_CUBE.PORT8].SetState(true); // dsptemp - IoCollection에서는 index 0 부터 시작해야함
                                }
                            }
                            else
                            {
                                seqNo = 70;
                            }
                        }
                    }
                    else if ((m_OpPattern == nxOP_PATTERN.GET && m_Robot.DiCompleteGet.GetState()) ||
                             (m_OpPattern == nxOP_PATTERN.PUT && m_Robot.DiCompletePut.GetState()))
                    {
                        m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, string.Format("CUBE[{0}] Interference bit check skip", m_PortId + 1));
                        if (m_Robot.DiCompleteGet.GetState())
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion of Get Operation is ON");
                        else if (m_Robot.DiCompletePut.GetState())
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion of Put Operation is ON");
                        seqNo = 70;
                    }
                    break;

                case 65:
                    if (!m_Robot.IsDeliveryPreparationComp(m_PortId + 1 - (int)nxOP_CUBE.PORT8))
                    {
                        m_Robot.SetDeliveryCompletion(m_PortId + 1 - (int)nxOP_CUBE.PORT8, false);
                        seqNo = 70;
                    }
                    else if (m_Robot.Simul.Device)
                    {
                        m_Robot.DiCompleteDeliveryBits[m_PortId - (int)nxOP_CUBE.PORT8].SetState(false);
                    }
                    break;

                case 70:
                    {
                        if (m_Robot.Simul.Device) // Simulate Operation Complete Signal & Interference Status except EXCHANGE Case
                        {
                            if (GetElapsedTicks() < 2000) break;
                            else if (m_OpPattern == nxOP_PATTERN.EXCHANGE)
                            {
                                m_Robot.DiCompleteGet.SetState(true);
                            }
                            else if (m_OpPattern == nxOP_PATTERN.GET)
                            {
                                m_Robot.DiInterfereCubeBits[m_PortId].SetState(false);
                                m_Robot.DiCompleteGet.SetState(true);
                            }
                            else if (m_OpPattern == nxOP_PATTERN.PUT)
                            {
                                m_Robot.DiInterfereCubeBits[m_PortId].SetState(false);
                                m_Robot.DiCompletePut.SetState(true);
                            }
                            else if (m_OpPattern == nxOP_PATTERN.GET_PREPARE || m_OpPattern == nxOP_PATTERN.PUT_PREPARE)
                            {
                                m_Robot.DiCompletePrepare.SetState(true);
                            }
                        }

                        if (m_OpPattern == nxOP_PATTERN.EXCHANGE || m_OpPattern == nxOP_PATTERN.GET)
                        {
                            if (m_Robot.DiCompleteGet.GetState())
                            {
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion of Get Operation is ON");
                                m_Robot.DoCompletionOfGetOperationReceptionist.SetState(true);
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Get Operation Receptionist ON");
                                seqNo = 80;
                            }
                        }
                        else if (m_OpPattern == nxOP_PATTERN.PUT)
                        {
                            if (m_Robot.DiCompletePut.GetState())
                            {
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion of Put Operation is ON");
                                m_Robot.DoCompletionOfPutOperationReceptionist.SetState(true);
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Put Operation Receptionist ON");
                                seqNo = 80;
                            }
                        }
                        else if (m_OpPattern == nxOP_PATTERN.GET_PREPARE || m_OpPattern == nxOP_PATTERN.PUT_PREPARE)
                        {
                            if (m_Robot.DiCompletePrepare.GetState())
                            {
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion of Preparation Operation is ON");
                                m_Robot.DoCompletionOfPreparationReception.SetState(true);
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Put Operation Receptionist ON");
                                seqNo = 80;
                            }
                        }
                    }
                    break;

                case 80:
                    {
                        if (m_Robot.Simul.Device)
                        {
                            if (m_Robot.DoCompletionOfGetOperationReceptionist.GetState())
                            {
                                m_Robot.DiCompleteGet.SetState(false);
                                if (m_OpPattern == nxOP_PATTERN.EXCHANGE)
                                {
                                    m_Robot.DiOperationGet.SetState(false);
                                    m_Robot.DiOperationPut.SetState(true);
                                }
                            }
                            else if (m_Robot.DoCompletionOfPutOperationReceptionist.GetState())
                            {
                                m_Robot.DiCompletePut.SetState(false);
                            }
                            else if (m_Robot.DoCompletionOfPreparationReception.GetState())
                            {
                                m_Robot.DiCompletePrepare.SetState(false);
                            }
                            m_StartTicks = XFunc.GetTickCount();
                        }

                        if (m_OpPattern == nxOP_PATTERN.EXCHANGE && m_Robot.DiOperationGet.GetState() == false && m_Robot.DiCompleteGet.GetState() == false)
                        {
                            m_Robot.DoCompletionOfGetOperationReceptionist.SetState(false);
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Get Operation Receptionist OFF");

                            seqNo = 90;
                        }
                        else if ((m_OpPattern == nxOP_PATTERN.GET && m_Robot.DiCompleteGet.GetState() == false) ||
                                (m_OpPattern == nxOP_PATTERN.PUT && m_Robot.DiCompletePut.GetState() == false))
                        {
                            if (m_OpPattern == nxOP_PATTERN.GET)
                            {
                                m_Robot.DoCompletionOfGetOperationReceptionist.SetState(false);
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Get Operation Receptionist OFF");
                            }
                            else if (m_OpPattern == nxOP_PATTERN.PUT)
                            {
                                m_Robot.DoCompletionOfPutOperationReceptionist.SetState(false);
                                m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Put Operation Receptionist OFF");
                            }

                            m_Robot.SetEnteranceCubeBits((nxOP_CUBE)(m_PortId + 1), false);

                            seqNo = 200;
                        }
                        else if ((m_OpPattern == nxOP_PATTERN.GET_PREPARE || m_OpPattern == nxOP_PATTERN.PUT_PREPARE) && !m_Robot.DiCompletePrepare.GetState())
                        {
                            m_Robot.DoCompletionOfPreparationReception.SetState(false);
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Prepare Operation Receptionist OFF");
                            seqNo = 200;
                        }
                    }
                    break;

                case 90:
                    {
                        if (m_Robot.Simul.Device)
                        {
                            if (GetElapsedTicks() < 2000) break;
                            else if (m_Hand == nxOP_HAND.UPPER_HAND && m_Robot.IsUpperHandGlassDetected())
                                m_Robot.SetUpperHandGlassDetected(false);   // Upper Hand PUT Case
                            else if (m_Hand == nxOP_HAND.LOWER_HAND && m_Robot.IsLowerHandGlassDetected())
                                m_Robot.SetLowerHandGlassDetected(false);   // Lower Hand PUT Case

                            m_StartTicks = XFunc.GetTickCount();
                        }

                        if (m_OpPattern == nxOP_PATTERN.EXCHANGE && m_Robot.DiOperationPut.GetState() == true)
                        {
                            // Exchange Operation is Keeping (PUT)
                            seqNo = 95;
                        }
                    }
                    break;

                case 95:
                    {
                        if (m_Robot.Simul.Device)
                        {
                            if (GetElapsedTicks() < 2000) break;
                            else
                            {
                                m_Robot.DiInterfereCubeBits[m_PortId].SetState(false);
                                m_Robot.DiCompletePut.SetState(true);
                                m_Robot.DiCompleteExchange.SetState(true);
                            }
                        }

                        if (m_Robot.DiCompletePut.GetState() == true && m_Robot.DiCompleteExchange.GetState() == true)
                        {
                            m_Robot.DoCompletionOfPutOperationReceptionist.SetState(true);
                            m_Robot.DoCompletionOfExchangeOperationReceptionst.SetState(true);
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Put Operation Receptionist ON");
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Exchange Operation Receptionist ON");

                            seqNo = 100;
                        }
                    }
                    break;

                case 100:
                    {
                        if (m_Robot.Simul.Device)
                        {
                            m_Robot.DiCompletePut.SetState(false);
                            m_Robot.DiCompleteExchange.SetState(false);
                        }

                        if (!m_Robot.DiCompletePut.GetState() == false && !m_Robot.DiCompleteExchange.GetState() == false)
                        {
                            m_Robot.DoCompletionOfPutOperationReceptionist.SetState(false);
                            m_Robot.DoCompletionOfExchangeOperationReceptionst.SetState(false);
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Put Operation Receptionist OFF");
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Completion Of Exchange Operation Receptionist OFF");

                            m_Robot.SetEnteranceCubeBits((nxOP_CUBE)(m_PortId + 1), false);

                            seqNo = 200;
                        }
                    }
                    break;

                case 200:
                    {
                        bool end = false;

                        if (m_Robot.Simul.Device)
                        {
                            if (m_Robot.DiPatternOfOperationIsPerformed.GetState() == true ||
                               m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState() == false)
                            {
                                m_Robot.DiOperationPut.SetState(false);
                                m_Robot.DiOperationGet.SetState(false);
                                m_Robot.DiOperationExchange.SetState(false);
                                m_Robot.DiOperationPreapare.SetState(false);
                                m_Robot.DiPatternOfOperationIsPerformed.SetState(false);
                                m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.SetState(true);
                            }
                        }

                        if (m_OpPattern == nxOP_PATTERN.EXCHANGE && m_Robot.DiOperationExchange.GetState() == false &&
                           m_Robot.DiOperationPut.GetState() == false && m_Robot.DiOperationGet.GetState() == false)
                        {
                            end = true;
                        }
                        else if ((m_OpPattern == nxOP_PATTERN.GET && m_Robot.DiOperationGet.GetState() == false) ||
                                (m_OpPattern == nxOP_PATTERN.PUT && m_Robot.DiOperationPut.GetState() == false))
                        {
                            end = true;
                        }
                        else if ((m_OpPattern == nxOP_PATTERN.GET_PREPARE || m_OpPattern == nxOP_PATTERN.PUT_PREPARE) && !m_Robot.DiOperationPreapare.GetState())
                        {
                            end = true;
                        }

                        end &= (m_Robot.DiPatternOfOperationIsPerformed.GetState() == false);
                        end &= (m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState() == true);

                        if (end)
                        {
                            m_Robot.SetLog(m_Robot.Name, seqName, m_PortId + 1, m_SlotId + 1, "Operation End Condition is TRUE");

                            if (m_OpPattern == nxOP_PATTERN.GET)
                            {
                                if (m_Robot.GenInfos.AutoMode) m_Robot.FireRobotTransferEvent(RobotActionType.Get, m_PortId, m_SlotId);
                                if ((nxOP_CUBE)(m_PortId + 1) < nxOP_CUBE.STAGE1) m_Robot.SetEffectiveParam(nxEFFECT_PARAM.EFFECT_X, false);
                            }
                            else if (m_OpPattern == nxOP_PATTERN.PUT)
                            {
                                if (m_Robot.GenInfos.AutoMode) m_Robot.FireRobotTransferEvent(RobotActionType.Put, m_PortId, m_SlotId);
                                if ((nxOP_CUBE)(m_PortId + 1) == nxOP_CUBE.STAGE1) m_Robot.SetEffectiveParam(nxEFFECT_PARAM.EFFECT_Y, false);
                            }

                            m_Robot.DoVirtualConveyanceMode.SetState(false);

                            m_Robot.InProcess = false;
                            returnValue = 0;
                            seqNo = 0;
                        }
                    }
                    break;
            }
            m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }

    public class YaskawaNx100YAlignSeq : XSeqFunction
    {
        #region Fields
        private YaskawaNX100 m_Robot = null;
        private int m_PortId = -1;
        private nxOP_HAND m_Hand;
        #endregion

        #region Constructor
        public YaskawaNx100YAlignSeq(YaskawaNX100 robot)
        {
            m_Robot = robot;
        }
        #endregion

        #region Methods
        public void SetCondition(nxOP_HAND hand, int portId)
        {
            m_Hand = hand;
            m_PortId = portId;
        }
        #endregion

        public override int Do()
        {
            //int rv = -1;
            int returnValue = -1;
            int seqNo = m_SeqNo;
            //if(m_Robot.AlarmCondition != RobotAlarmCondition.NoAlarm)
            //{
            //    SeqNo = 0;
            //    return (int)m_Robot.AlarmCondition;
            //}

            switch (seqNo)
            {
                case 0:
                    if (!m_Robot.DiRunning.GetState())
                    {
                        m_Robot.SetLog(m_Robot.Name, "Y-Align", m_PortId + 1, 0, "External Start : ON");
                        m_Robot.DoExternalStart.SetState(true);
                        seqNo = 10;
                    }
                    else
                        seqNo = 15;
                    break;

                case 10:
                    if (m_Robot.Simul.Device == true || m_Robot.DiRunning.GetState() == true)
                    {
                        m_Robot.SetLog(m_Robot.Name, "Y-Align", m_PortId + 1, 0, "NX Running : Confirm");
                        m_Robot.DoExternalStart.SetState(false);

                        seqNo = 15;
                    }
                    break;

                case 15:
                    if (m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState() == true)
                    {
                        m_Robot.SetPatternOfOperation(nxOP_PATTERN.Y_ALIGN, m_Hand, (nxOP_CUBE)(m_PortId + 1));
                        m_Robot.SetSlotSelection(1); // dspcrassus - Y Align 시에는 SlotNo를 1로 한다

                        m_Robot.DoMotionPatternReadingStrobe.SetState(true);
                        seqNo = 20;
                    }
                    break;

                case 20:
                    if (m_Robot.Simul.Device == true ||
                        m_Robot.DiCompletionOfReceptionOfMotionPatternData.GetState() == true)
                    {
                        // Let's check robot side command copy of motion and slot setting.
                        int patternNoRobot = m_Robot.GetPatternOfOperation();
                        if (m_Robot.PatternNo == patternNoRobot)
                        {
                            m_Robot.DoMotionPatternReadingStrobe.SetState(false);
                            m_Robot.DoCompletionOfPatternCheckOfOperation.SetState(true);
                            seqNo = 30;
                        }
                        //else
                        //{
                        //    m_Robot.DoExternalStart.SetState(false);
                        //    m_Robot.DoMotionPatternReadingStrobe.SetState(false);
                        //    seqNo = 0;
                        //}
                    }
                    break;

                case 30:
                    if (m_Robot.Simul.Device == true ||
                        m_Robot.DiCompletionOfReceptionOfMotionPatternData.GetState() == false)
                    {
                        m_Robot.SetSlotSelection(0);
                        m_Robot.SetPatternOfOperation(0, m_Hand, 0);

                        m_Robot.DoCompletionOfPatternCheckOfOperation.SetState(false);
                        seqNo = 40;
                    }
                    break;

                case 40:
                    {
                        //m_Robot.SetEnteranceCubeBits((nxOP_CUBE)(m_PortId + 1), true);
                        seqNo = 50;
                    }
                    break;

                case 50:
                    if (m_Robot.Simul.Device == true || m_Robot.DiOperationAlign_Y.GetState() == true)
                    {
                        seqNo = 60;
                    }
                    break;

                case 60:
                    if (m_Robot.Simul.Device == true || m_Robot.DiCompleteAlign_Y.GetState() == true)
                    {
                        m_Robot.DoCompletionCheckOfY_AlignOperation.SetState(true);
                        seqNo = 70;
                    }
                    break;

                case 70:
                    if (m_Robot.DiCompleteAlign_Y.GetState() == false)
                    {
                        m_Robot.DoCompletionCheckOfY_AlignOperation.SetState(false);
                        seqNo = 80;
                    }
                    break;

                case 80:
                    {
                        bool endOk = !m_Robot.DiOperationAlign_Y.GetState();
                        endOk &= !m_Robot.DiPatternOfOperationIsPerformed.GetState();
                        endOk &= m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState();

                        if (endOk)
                        {
                            returnValue = 0;
                            seqNo = 0;
                        }
                    }
                    break;
            }
            m_SeqNo = seqNo;
            return returnValue;
        }
    }

    public class YaskawaNx100AlarmSeq : XSeqFunction
    {
        private enum CheckStatus
        {
            ModeSelect,
            ServoOff,
            SafetyPlug,
            MaxStatusNo
        }

        #region Fields
        private YaskawaNX100 m_Robot = null;
        protected IServerManager m_Server;
        protected _GenInfoHandler m_GenInfos;
        private IoCollection<IoDigitalInput> m_DiRobotAlarmBits = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalInput> m_DiAppAlarmBits = new IoCollection<IoDigitalInput>();
        private List<Alarm> m_RobotAlarms = new List<Alarm>();
        private List<Alarm> m_AppAlarms = new List<Alarm>();
        private List<bool> m_RobotAlarmStatuses = new List<bool>();
        private List<bool> m_AppAlarmStatuses = new List<bool>();

        private List<Alarm> ALM_AbnormalStatuses = new List<Alarm>();
        private List<bool> m_AbnormalStatuses = new List<bool>();
        #endregion

        #region Constructor
        public YaskawaNx100AlarmSeq(YaskawaNX100 robot)
        {
            m_Robot = robot;
            m_Server = m_Robot.ServerManager;

            m_DiRobotAlarmBits.Add(m_Robot.DiAlarmErrorOccurred);
            m_DiRobotAlarmBits.Add(m_Robot.DiAlarmBattery);
            m_DiAppAlarmBits.Add(m_Robot.DiErrorUpperVacuum);
            m_DiAppAlarmBits.Add(m_Robot.DiErrorLowerVacuum);
            m_DiAppAlarmBits.Add(m_Robot.DiErrorUpperAlignment);
            m_DiAppAlarmBits.Add(m_Robot.DiErrorLowerAlignment);
            m_DiAppAlarmBits.Add(m_Robot.DiErrorUpperAlign_Y);
            m_DiAppAlarmBits.Add(m_Robot.DiErrorLowerAlign_Y);
            m_DiAppAlarmBits.Add(m_Robot.DiAbnormalityInGlassFallUpper);
            m_DiAppAlarmBits.Add(m_Robot.DiAbnormalityInGlassFallLower);

            m_RobotAlarms.Add(m_Robot.ALM_ErrorOccuredAlarm);
            m_RobotAlarms.Add(m_Robot.ALM_BatteryAlarm);
            m_AppAlarms.Add(m_Robot.ALM_UpperHandVacuumError);
            m_AppAlarms.Add(m_Robot.ALM_LowerHandVacuumError);
            m_AppAlarms.Add(m_Robot.ALM_UpperHandAlignmentError);
            m_AppAlarms.Add(m_Robot.ALM_LowerHandAlignmentError);
            m_AppAlarms.Add(m_Robot.ALM_UpperHandYAlignError);
            m_AppAlarms.Add(m_Robot.ALM_LowerHandYAlignError);
            m_AppAlarms.Add(m_Robot.ALM_UpperAbnormalGlassFall);
            m_AppAlarms.Add(m_Robot.ALM_LowerAbnormalGlassFall);

            for (int i = 0; i < m_RobotAlarms.Count; i++)
            {
                m_RobotAlarmStatuses.Add(false);
            }
            for (int i = 0; i < m_AppAlarms.Count; i++)
            {
                m_AppAlarmStatuses.Add(false);
            }

            ALM_AbnormalStatuses.Add(new Alarm(robot.Name + " Mode Select Error", AlarmLevel.S, AlarmCode.EquipmentStatusWarning));
            ALM_AbnormalStatuses.Add(new Alarm(robot.Name + " Abnormal Servo Off Error", AlarmLevel.S, AlarmCode.EquipmentSafety));
            ALM_AbnormalStatuses.Add(new Alarm(robot.Name + " Safety Plug Off Error", AlarmLevel.S, AlarmCode.EquipmentSafety));
            for (int i = 0; i < ALM_AbnormalStatuses.Count; i++)
            {
                m_AbnormalStatuses.Add(false);
            }

        }
        #endregion

        #region Methods
        #endregion

        public override int Do()
        {
            int rv = -1;
            int seqNo = m_SeqNo;

            CheckAbnormalCondition();

            switch (seqNo)
            {
                case 0:
                    {
                        int count = m_DiRobotAlarmBits.Count;
                        #region Set Robot Alarm
                        for (int index = 0; index < count; index++)
                        {
                            if (m_DiRobotAlarmBits[index].GetState() && !m_RobotAlarmStatuses[index])
                            {
                                m_RobotAlarmStatuses[index] = true;
                                m_Server.EqpStateManager.SetAlarm(m_RobotAlarms[index].Id);
                                m_Robot.SetLog(m_Robot.Name, "SET_ALARM", 0, 0, m_RobotAlarms[index].Name);

                                if (m_Robot.AlarmCondition != RobotAlarmCondition.HeavyAlarm)
                                    m_Robot.AlarmCondition = RobotAlarmCondition.HeavyAlarm;
                            }
                        }
                        #endregion

                        count = m_DiAppAlarmBits.Count;
                        #region Set Application Alarm
                        for (int index = 0; index < count; index++)
                        {
                            if (m_DiAppAlarmBits[index].GetState() && !m_AppAlarmStatuses[index])
                            {
                                m_AppAlarmStatuses[index] = true;
                                m_Server.EqpStateManager.SetAlarm(m_AppAlarms[index].Id);
                                m_Robot.SetLog(m_Robot.Name, "SET_ALARM", 0, 0, m_AppAlarms[index].Name);

                                if (m_Robot.AlarmCondition == RobotAlarmCondition.NoAlarm)
                                    m_Robot.AlarmCondition = RobotAlarmCondition.LightAlarm;
                            }
                        }
                        #endregion

                        if (m_Robot.AlarmCondition != RobotAlarmCondition.NoAlarm && m_Server.EqpStateManager.AlarmResetSwitchPushed)
                        {
                            m_Robot.DoAlarmErrorReset.SetState(true);
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 10;
                        }
                    }
                    break;
                case 10:
                    if ((XFunc.GetTickCount() - m_StartTicks > 1000 && !m_Server.EqpStateManager.AlarmResetSwitchPushed))
                    {
                        m_Robot.ResetRobotIfSignal();

                        m_Robot.DoAlarmErrorReset.SetState(false);
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (XFunc.GetTickCount() - m_StartTicks > 1000)
                    {
                        bool heavyAlarm = false;
                        bool lightAlarm = false;

                        int count = m_DiRobotAlarmBits.Count;
                        #region Reset Robot Alarm
                        for (int index = 0; index < count; index++)
                        {
                            if (!m_DiRobotAlarmBits[index].GetState() && m_RobotAlarmStatuses[index])
                            {
                                m_RobotAlarmStatuses[index] = false;
                                m_Server.EqpStateManager.ResetAlarm(m_RobotAlarms[index].Id);
                                m_Robot.SetLog(m_Robot.Name, "RESET_ALARM", 0, 0, m_RobotAlarms[index].Name);
                            }
                            else if (m_DiRobotAlarmBits[index].GetState())
                                heavyAlarm = true;
                        }
                        #endregion

                        count = m_DiAppAlarmBits.Count;
                        #region Reset Application Alarm
                        for (int index = 0; index < count; index++)
                        {
                            if (!m_DiAppAlarmBits[index].GetState() && m_AppAlarmStatuses[index])
                            {
                                m_AppAlarmStatuses[index] = false;
                                m_Server.EqpStateManager.ResetAlarm(m_AppAlarms[index].Id);
                                m_Robot.SetLog(m_Robot.Name, "RESET_ALARM", 0, 0, m_AppAlarms[index].Name);
                            }
                            else if (m_DiAppAlarmBits[index].GetState())
                                lightAlarm = true;
                        }
                        #endregion

                        if (heavyAlarm)
                            m_Robot.AlarmCondition = RobotAlarmCondition.HeavyAlarm;
                        else if (lightAlarm)
                            m_Robot.AlarmCondition = RobotAlarmCondition.LightAlarm;
                        else
                            m_Robot.AlarmCondition = RobotAlarmCondition.NoAlarm;

                        seqNo = 0;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return rv;
        }

        private void CheckAbnormalCondition()
        {
            if (!m_GenInfos.EqpInitComp) return;

            bool[] abnormalStatus = new bool[(int)CheckStatus.MaxStatusNo];

            abnormalStatus[(int)CheckStatus.ModeSelect] = !m_Robot.DiRemoteModeSelect.GetState();
            abnormalStatus[(int)CheckStatus.ModeSelect] &= m_Robot.DiSafetyPlug.GetState();

            abnormalStatus[(int)CheckStatus.ServoOff] = m_Robot.DiRemoteModeSelect.GetState();
            abnormalStatus[(int)CheckStatus.ServoOff] &= !m_Robot.DiRunning.GetState();
            abnormalStatus[(int)CheckStatus.ServoOff] &= !m_Robot.DiServoOn.GetState();
            abnormalStatus[(int)CheckStatus.ServoOff] &= m_Robot.ExternalControl == nxEX_CONTROL.EX_NONE;

            abnormalStatus[(int)CheckStatus.SafetyPlug] = !m_Robot.DiSafetyPlug.GetState();

            int count = ALM_AbnormalStatuses.Count;
            for (int i = 0; i < count; i++)
            {
                if (abnormalStatus[i] && !m_AbnormalStatuses[i])
                {
                    m_AbnormalStatuses[i] = true;
                    m_Server.EqpStateManager.SetAlarm(ALM_AbnormalStatuses[i].Id);
                }
                else if (!abnormalStatus[i] && m_AbnormalStatuses[i])
                {
                    m_AbnormalStatuses[i] = false;
                    m_Server.EqpStateManager.ResetAlarm(ALM_AbnormalStatuses[i].Id);
                }
            }
        }
    }
}
