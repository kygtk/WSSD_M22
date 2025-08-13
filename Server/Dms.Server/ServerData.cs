using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Management;
using Dms.Common;
using Dms.Data;
using Dms.Util.IODefine;
using System.Windows.Forms;
using System.Collections;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Server
{
    //[Synchronization]
    partial class ServerManager
    {
        #region Fields
        private DmsComponents m_DmsComponents = DmsComponents.Instance;
        private DataProvider m_DataProvider;// = new DataProvider();
        private GlassDataHandler m_GlassData;
        private readonly JobCondition m_JobCondition = JobCondition.Instance;
        private TagSetupInfo m_SetupJobMode;
        private TagSetupInfo m_SetupSingleMode;
        private TagSetupInfo m_SetupGlassSize;
        private TagSetupInfo m_SetupMaxGlassNo;
        // private TagSetupInfo m_SetupCvInSpeed;
        // private TagSetupInfo m_SetupCvOutSpeed;
        private TagSetupInfo m_SetupCvStopSpeed;
        private TagSetupInfo m_SetupIdleWaitTime;
        //private TagSetupInfo m_GaugeRecheckTime; // 09.11.02 minhan 
        private TagSetupInfo m_GaugeAlarmCheckTime; // 09.12.01 minhan;//lkl 150929
        private TagSetupInfo m_DualSkipDelayTime;//2010.06.16 kimgun
        private TagSetupInfo m_SetupMemoryLImit;//2010.07.19 kimgun
        private TagSetupInfo m_SetupHpmjRunTime;//lkl 150929
        private TagSetupInfo m_SetupHpmjStopTime;//lkl 150929

        private TagSetupInfo m_SetupAkReverseDiff;//2010.09.09 kimgun
                                                  //11.02.01 상위 베이스 수정
                                                  //private TagSetupInfo m_SetupApVolAlarmIntrMargin;
                                                  //private TagSetupInfo m_SetupApVolWarningIntrMargin;
                                                  //private TagSetupInfo m_SetupApN2AlarmIntrMargin;
                                                  //private TagSetupInfo m_SetupApN2WarningIntrMargin;
                                                  //private TagSetupInfo m_SetupApCDAAlarmIntrMargin;
                                                  //private TagSetupInfo m_SetupApCDAWarningIntrMargin;
                                                  //private TagSetupInfo m_ApAlarmcheckTime;
                                                  //private TagSetupInfo m_ApStatusCheckTime; // 11.04.27 minhan

        private TagSetupInfo m_SetupIdleUse;
        private TagSetupInfo m_SetupIdleRunTime;
        private TagSetupInfo m_SetupIdleStopTime;
        private TagSetupInfo m_SetupHpmjIdleUse;
        private TagSetupInfo m_SetupIdleHpmjPress;
        private TagSetupInfo m_SetupPumpIntPrePress; // 11.02.01 minhan;//lkl 150929
        private TagSetupInfo m_SetupPumpIntPostPress;// lkl 150929
        private TagSetupInfo m_SetupPumpIntFlow;//lkl 150929
        private TagSetupInfo m_SetupPumpIntTime;//lkl 150929
        private TagSetupInfo m_SetupFlowWarningMargin;
        private TagSetupInfo m_SetupFlowAlarmMargin;
        private TagSetupInfo m_SetupPressWarningMargin;
        private TagSetupInfo m_SetupPressAlarmMargin;
        private TagSetupInfo m_SetupLdTiltingDWTimeoutAlarm;
        private TagSetupInfo m_SetupLdTiltingUPTimeoutAlarm;
        private TagSetupInfo m_SetupLdAlignFwTimeoutAlarm;
        private TagSetupInfo m_SetupLdAlignBwTimeoutAlarm;
        private TagSetupInfo m_SetupLdIdleRollerFwTimeoutAlarm;//2013.11.12 fan
        private TagSetupInfo m_SetupLdIdleRollerBwTimeoutAlarm;//2013.11.12 fan
        private TagSetupInfo m_RbIdleUse; // 10.11.08 minhan
        private TagSetupInfo m_ULOutSensorOnTimeMargin; // 11.02.01 minhan
        private TagSetupInfo m_EgisInterfaceTiemout; // 11.02.25 minhan
        private TagSetupInfo m_EgisLoaderInterfaceTiemout; // 11.04.27 minhan
        private TagSetupInfo m_IonizerStopAlarmCheckTime; // 11.03.25 minhan
        private TagSetupInfo m_FfuMotorSpeed; // 11.05.06 minhan
        private TagSetupInfo m_SetupTpdEqpName;
        private TagSetupInfo m_SetupTpdSamplingTime;
        private TagSetupInfo m_SetupTpdFileSaveTime;
        private TagSetupInfo m_SetupTraceServerIp;
        private TagSetupInfo m_SetupTraceServerPort;

        private GenInfoHandler m_GenInfos = GenInfoHandler.Instance;
        private EqpManager m_EqpStateManager;
        private ApdItemsHandler m_ApdItemsHandler = ApdItemsHandler.Instance;
        private PartsItemsHandler m_PartsItemsHandler = PartsItemsHandler.Instance;
        private HpmjItemsHandler m_HpmjItemsHandler = HpmjItemsHandler.Instance;
        #endregion

        #region Properties
        public IEqpManager EqpStateManager
        {
            get { return m_EqpStateManager; }
        }
        public DataProvider DataProvider
        {
            get { return m_DataProvider; }
        }
        public IoDefines IoDefines
        {
            get { return m_DataProvider.IoDefines; }
        }
        public DeviceTags TagContainer
        {
            get { return m_DataProvider.TagContainer; }
        }
        public AlarmListProvider AlarmList
        {
            get { return m_DataProvider.AlarmList; }
        }
        public SetupGenInfoProvider SetupGenInfo
        {
            get { return m_DataProvider.SetupGenInfo; }
        }
        public SetupIdleInfoProvider SetupIdleInfo
        {
            get { return m_DataProvider.SetupIdleInfo; }
        }
        public SetupCvDistanceProvider SetupCvDistance
        {
            get { return m_DataProvider.SetupCvDistance; }
        }
        public SetupCvInfoProvider SetupCvInfo
        {
            get { return m_DataProvider.SetupCvInfo; }
        }
        public SetupSensorInterlockProvider SetupSensorIntr
        {
            get { return m_DataProvider.SetupSensorIntr; }
        }
        public SetupTankLevelProvider SetupTankLevel
        {
            get { return m_DataProvider.SetupTankLevel; }
        }
        public SetupDevTankLevelProvider SetupDevTankLevel
        {
            get { return m_DataProvider.SetupDevTankLevel; }
        }
        public SetupGaugeInterlockProvider SetupGaugeInterlock
        {
            get { return m_DataProvider.SetupGaugeInterlock; }
        }
        public SetupSensorTimeoutProvider SetupSensorTimeout
        {
            get { return m_DataProvider.SetupSensorTimeout; }
        }
        public CalibrationProvider CalibrationProvider
        {
            get { return m_DataProvider.CalibrationProvider; }
        }
        public IComponentContainer ComponentContainer
        {
            get { return m_DmsComponents.ComponentContainer; }
        }
        public IGlassDataHandler GlassData
        {
            get { return m_GlassData; }
        }
        public IJobCondition JobCond
        {
            get { return m_JobCondition; }
        }
        public TagSetupInfo SetupJobMode
        {
            get { return m_SetupJobMode; }
            set { m_SetupJobMode = value; }
        }
        public TagSetupInfo SetupGlassSize
        {
            get { return m_SetupGlassSize; }
            set { m_SetupGlassSize = value; }
        }
        public TagSetupInfo SetupMaxGlassNo
        {
            get { return m_SetupMaxGlassNo; }
            set { m_SetupMaxGlassNo = value; }
        }
        public int GetSetupMaxGlassNo   //  BM
        {
            get { return m_SetupMaxGlassNo.GetValue<int>(); }
        }
        public TagSetupInfo SetupHpmjIdleUse
        {//2009.09.17 kimgun
            get { return m_SetupHpmjIdleUse; }
            set { m_SetupHpmjIdleUse = value; }
        }
        public TagSetupInfo SetupIdleHpmjPress
        {//2009.09.17 kimgun
            get { return m_SetupIdleHpmjPress; }
            set { m_SetupIdleHpmjPress = value; }
        }
        public TagSetupInfo SetupPumpIntPrePress // 11.02.01 minhan;//lkl 150929
        {//2009.09.21 kimgun
            get { return m_SetupPumpIntPrePress; }
            set { m_SetupPumpIntPrePress = value; }
        }
        public TagSetupInfo SetupPumpIntPostPress
        {//2009.09.21 kimgun;//lkl 150929
            get { return m_SetupPumpIntPostPress; }
            set { m_SetupPumpIntPostPress = value; }
        }
        public TagSetupInfo SetupPumpIntFlow
        {//2009.09.21 kimgun;//lkl 150929
            get { return m_SetupPumpIntFlow; }
            set { m_SetupPumpIntFlow = value; }
        }
        public TagSetupInfo SetupPumpIntTime
        {//2009.09.21 kimgun;//lkl 150929
            get { return m_SetupPumpIntTime; }
            set { m_SetupPumpIntTime = value; }
        }
        public TagSetupInfo SetupFlowWarningMargin
        {//090923 LeeChungWon
            get { return m_SetupFlowWarningMargin; }
            set { m_SetupFlowWarningMargin = value; }
        }
        public TagSetupInfo SetupFlowAlarmMargin
        {//090923 LeeChungWon
            get { return m_SetupFlowAlarmMargin; }
            set { m_SetupFlowAlarmMargin = value; }
        }
        public TagSetupInfo SetupPressWarningMargin
        {//090923 LeeChungWon
            get { return m_SetupPressWarningMargin; }
            set { m_SetupPressWarningMargin = value; }
        }
        public TagSetupInfo SetupPressAlarmMargin
        {//090923 LeeChungWon
            get { return m_SetupPressAlarmMargin; }
            set { m_SetupPressAlarmMargin = value; }
        }
        //public TagSetupInfo GaugeRecheckTime // 09.11.02 minhan
        //{
        //    get { return m_GaugeRecheckTime; }
        //    set { m_GaugeRecheckTime = value; }
        //}
        public TagSetupInfo GaugeAlarmCheckTime // 09.12.01 minhan//lkl 150929
        {
            get { return m_GaugeAlarmCheckTime; }
            set { m_GaugeAlarmCheckTime = value; }
        }

        public TagSetupInfo SetupHpmjRunTime
        {//lkl 150929
            get { return m_SetupHpmjRunTime; }
            set { m_SetupHpmjRunTime = value; }
        }

        public TagSetupInfo SetupHpmjStopTime
        {//lkl 150929
            get { return m_SetupHpmjStopTime; }
            set { m_SetupHpmjStopTime = value; }
        }

        public TagSetupInfo SetupLdTiltingUPTimeoutAlarm
        {
            get { return m_SetupLdTiltingUPTimeoutAlarm; }
            set { m_SetupLdTiltingUPTimeoutAlarm = value; }
        }
        public TagSetupInfo SetupLdTiltingDWTimeoutAlarm
        {
            get { return m_SetupLdTiltingDWTimeoutAlarm; }
            set { m_SetupLdTiltingDWTimeoutAlarm = value; }
        }
        public TagSetupInfo SetupLdAlignFwTimeoutAlarm
        {
            get { return m_SetupLdAlignFwTimeoutAlarm; }
            set { m_SetupLdAlignFwTimeoutAlarm = value; }
        }
        public TagSetupInfo SetupLdAlignBwTimeoutAlarm
        {
            get { return m_SetupLdAlignBwTimeoutAlarm; }
            set { m_SetupLdAlignBwTimeoutAlarm = value; }
        }
        public TagSetupInfo SetupLdIdleRollerFwTimeoutAlarm  //2013.11.12 fan
        {
            get { return m_SetupLdIdleRollerFwTimeoutAlarm; }
            set { m_SetupLdIdleRollerFwTimeoutAlarm = value; }
        }
        public TagSetupInfo SetupLdIdleRollerBwTimeoutAlarm  //2013.11.12 fan
        {
            get { return m_SetupLdIdleRollerBwTimeoutAlarm; }
            set { m_SetupLdIdleRollerBwTimeoutAlarm = value; }
        }
        public TagSetupInfo SetupAkReverseDiff
        {//2010.09.09 kimgun
            get { return m_SetupAkReverseDiff; }
            set { m_SetupAkReverseDiff = value; }
        }
        //public TagSetupInfo SetupCvInSpeed
        //{
        //    get { return m_SetupCvInSpeed; }
        //    set { m_SetupCvInSpeed = value; }
        //}
        //public TagSetupInfo SetupCvOutSpeed
        //{
        //    get { return m_SetupCvOutSpeed; }
        //    set { m_SetupCvOutSpeed = value; }
        //}
        public TagSetupInfo SetupCvStopSpeed
        {
            get { return m_SetupCvStopSpeed; }
            set { m_SetupCvStopSpeed = value; }
        }
        public int GetSetupCvStopSpeed
        {
            get { return m_SetupCvStopSpeed.GetValue<int>(); }
        }
        public TagSetupInfo DualSkipDelayTime
        {
            get { return m_DualSkipDelayTime; }
            set { m_DualSkipDelayTime = value; }
        }
        public TagSetupInfo SetupIdleUse
        {
            get { return m_SetupIdleUse; }
            set { m_SetupIdleUse = value; }
        }
        public TagSetupInfo SetupIdleRunTime
        {
            get { return m_SetupIdleRunTime; }
            set { m_SetupIdleRunTime = value; }
        }
        public TagSetupInfo SetupIdleStopTime
        {
            get { return m_SetupIdleStopTime; }
            set { m_SetupIdleStopTime = value; }
        }
        public TagSetupInfo SetupSingleMode
        {
            get { return m_SetupSingleMode; }
            set { m_SetupSingleMode = value; }
        }
        public bool IsSingleMode        //  BM
        {
            get { return m_SetupSingleMode.GetValue<bool>(); }
        }
        public TagSetupInfo SetupIdleWaitTime
        {
            get { return m_SetupIdleWaitTime; }
            set { m_SetupIdleWaitTime = value; }
        }
        //public TagSetupInfo HpmjSetupMainDiIntTime 10.12.21 minhan
        //{//2010.09.09 kimgun
        //    get { return m_HpmjSetupMainDiIntTime; }
        //    set { m_HpmjSetupMainDiIntTime = value; }
        //}
        public TagSetupInfo RbIdleUse // 10.11.08 minhan
        {
            get { return m_RbIdleUse; }
            set { m_RbIdleUse = value; }
        }
        public TagSetupInfo ULOutSensorOnTimeMargin // 11.02.01 minhan
        {
            get { return m_ULOutSensorOnTimeMargin; }
            set { m_ULOutSensorOnTimeMargin = value; }
        }
        public TagSetupInfo EgisInterfaceTiemout // 11.02.25 minhan
        {
            get { return m_EgisInterfaceTiemout; }
            set { m_EgisInterfaceTiemout = value; }
        }
        public TagSetupInfo EgisLoaderInterfaceTiemout // 11.04.27 minhan
        {
            get { return m_EgisLoaderInterfaceTiemout; }
            set { m_EgisLoaderInterfaceTiemout = value; }
        }
        public TagSetupInfo SetupMemoryLImit
        {
            get { return m_SetupMemoryLImit; }
            set { m_SetupMemoryLImit = value; }
        }
        public ApdItemsHandler ApdItemsHandler
        {
            get { return m_ApdItemsHandler; }
        }
        public PartsItemsHandler PartsItemsHandler
        {
            get { return m_PartsItemsHandler; }
        }

        public HpmjItemsHandler HpmjItemsHandler
        {
            get { return m_HpmjItemsHandler; }
        }
        //Mr.Kang
        //public TagSetupInfo SetupN2LimitTime // 10.12.21 minhan 일단 사용하지 않기에 삭제
        //{
        //    get { return m_SetupN2LimitTime; }
        //    set { m_SetupN2LimitTime = value; }
        //}
        //public TagSetupInfo SetupN2NGCount
        //{
        //    get { return m_SetupN2NGCount; }
        //    set { m_SetupN2NGCount = value; }
        //}
        //public TagSetupInfo SetupCDALimitTime
        //{
        //    get { return m_SetupCDALimitTime; }
        //    set { m_SetupCDALimitTime = value; }
        //}
        //public TagSetupInfo SetupCDANGCount
        //{
        //    get { return m_SetupCDANGCount; }
        //    set { m_SetupCDANGCount = value; }
        //}
        //public TagSetupInfo SetupVolLimitTime
        //{
        //    get { return m_SetupVolLimitTime; }
        //    set { m_SetupVolLimitTime = value; }
        //}
        //public TagSetupInfo SetupVolNGCount
        //{
        //    get { return m_SetupVolNGCount; }
        //    set { m_SetupVolNGCount = value; }
        //}
        //11.02.01 minhan
        //public TagSetupInfo SetupApVolAlarmIntrMargin
        //{
        //    get { return m_SetupApVolAlarmIntrMargin; }
        //    set { m_SetupApVolAlarmIntrMargin = value; }
        //}
        //public TagSetupInfo SetupApVolWarningIntrMargin
        //{
        //    get { return m_SetupApVolWarningIntrMargin; }
        //    set { m_SetupApVolWarningIntrMargin = value; }
        //}
        //public TagSetupInfo SetupApN2AlarmIntrMargin
        //{
        //    get { return m_SetupApN2AlarmIntrMargin; }
        //    set { m_SetupApN2AlarmIntrMargin = value; }
        //}
        //public TagSetupInfo SetupApN2WarningIntrMargin
        //{
        //    get { return m_SetupApN2WarningIntrMargin; }
        //    set { m_SetupApN2WarningIntrMargin = value; }
        //}
        //public TagSetupInfo SetupApCDAAlarmIntrMargin
        //{
        //    get { return m_SetupApCDAAlarmIntrMargin; }
        //    set { m_SetupApCDAAlarmIntrMargin = value; }
        //}
        //public TagSetupInfo SetupApCDAWarningIntrMargin
        //{
        //    get { return m_SetupApCDAWarningIntrMargin; }
        //    set { m_SetupApCDAWarningIntrMargin = value; }
        //}
        //public TagSetupInfo ApAlarmcheckTime // 11.01.27 minhan
        //{
        //    get { return m_ApAlarmcheckTime; }
        //    set { m_ApAlarmcheckTime = value; }
        //}
        //public TagSetupInfo ApStatusCheckTime // 11.04.27 minhan
        //{
        //    get { return m_ApStatusCheckTime; }
        //    set { m_ApStatusCheckTime = value; }
        //}
        public TagSetupInfo IonizerStopAlarmCheckTime // 11.03.25 minhan
        {
            get { return m_IonizerStopAlarmCheckTime; }
            set { m_IonizerStopAlarmCheckTime = value; }
        }
        public TagSetupInfo FfuMotorSpeed // 11.05.06 minhan
        {
            get { return m_FfuMotorSpeed; }
            set { m_FfuMotorSpeed = value; }
        }
        public TagSetupInfo SetupTpdEqpName
        {
            get { return m_SetupTpdEqpName; }
            set { m_SetupTpdEqpName = value; }
        }
        public TagSetupInfo SetupTpdSamplingTime
        {
            get { return m_SetupTpdSamplingTime; }
            set { m_SetupTpdSamplingTime = value; }
        }
        public TagSetupInfo SetupTpdFileSaveTime
        {
            get { return m_SetupTpdFileSaveTime; }
            set { m_SetupTpdFileSaveTime = value; }
        }
        public TagSetupInfo SetupTraceServerIp
        {
            get { return m_SetupTraceServerIp; }
            set { m_SetupTraceServerIp = value; }
        }
        public TagSetupInfo SetupTraceServerPort
        {
            get { return m_SetupTraceServerPort; }
            set { m_SetupTraceServerPort = value; }
        }
        #endregion

        #region Methods
        private DmsErrors InitializeServerData()
        {
            try
            {
                bool ok = true;

                m_DataProvider = new DataProvider();

                if (!m_DataProvider.Created)
                {
                    return DmsErrors.DBNotInitialized;
                }

                if (!m_DmsComponents.ReadXml())
                {
                    return DmsErrors.ConfigFileNotFound;
                }

                m_ServerMode = AppConfig.Instance.ServerMode;

                m_GenInfos.CreateTags(m_DataProvider.TagContainer);

                m_GlassData = new GlassDataHandler(m_DataProvider.GlassDataProvider);
                m_GlassData.CreateHandler(this);

                m_JobCondition.Initialize(this);

                //m_SetupJobMode = new TagSetupInfo("Job Mode", OptionType.JobMode, OptionFormat.Boolean, UnitType.None, "1");
                m_SetupJobMode = new TagSetupInfo("Job Mode", OptionType.Alternative, OptionFormat.JobMode, UnitType.None, JobMode.Process.ToString());
                m_SetupGlassSize = new TagSetupInfo("Glass Size", OptionType.None, OptionFormat.Digit, UnitType.mm, "1500");
                m_SetupMaxGlassNo = new TagSetupInfo("Max Glass No", OptionType.None, OptionFormat.Digit, UnitType.EA, "4", "1", "5");
                m_SetupSingleMode = new TagSetupInfo("Single Mode", OptionType.Alternative, OptionFormat.OnOff, UnitType.None, OptionFormat.OnOff.ToString()); // 09.11.04 minhan
                m_SetupIdleWaitTime = new TagSetupInfo("Idle Wait Time", OptionType.None, OptionFormat.Digit, UnitType.min, "1");
                m_SetupCvStopSpeed = new TagSetupInfo("DecelSpeed", OptionType.None, OptionFormat.Digit, UnitType.mmpm, "1600");//2010.06.16 kimgun
                m_DualSkipDelayTime = new TagSetupInfo("Dual Sensor Skip Delay Time", OptionType.None, OptionFormat.Float, UnitType.sec, "1");//2010.06.16 kimgun
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupJobMode);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupSingleMode);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupGlassSize);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupMaxGlassNo);

                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupCvStopSpeed);//2010.06.16 kimgun
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_DualSkipDelayTime);//2010.06.16 kimgun

                //m_SetupApVolAlarmIntrMargin = new TagSetupInfo(" Voltage Alarm Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "10.0", "1.0", "100");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_SetupApVolAlarmIntrMargin); // 11.02.01 minhan
                //m_SetupApVolWarningIntrMargin = new TagSetupInfo(" Voltage Warning Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "5.0", "1.0", "100");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_SetupApVolWarningIntrMargin);
                //m_SetupApN2AlarmIntrMargin = new TagSetupInfo(" N2 Alarm Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "10.0", "1.0", "100");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_SetupApN2AlarmIntrMargin);
                //m_SetupApN2WarningIntrMargin = new TagSetupInfo(" N2 Warning Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "5.0", "1.0", "100");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_SetupApN2WarningIntrMargin);
                //m_SetupApCDAAlarmIntrMargin = new TagSetupInfo(" CDA Alarm Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "10.0", "1.0", "100");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_SetupApCDAAlarmIntrMargin);
                //m_SetupApCDAWarningIntrMargin = new TagSetupInfo(" CDA Warning Interlock Margin", OptionType.None, OptionFormat.Float, UnitType.Percent, "5.0", "1.0", "100");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_SetupApCDAWarningIntrMargin);
                //m_ApAlarmcheckTime = new TagSetupInfo("AP Alarm Check Time", OptionType.None, OptionFormat.Float, UnitType.sec, "2.0", "0.0", "5.0"); // 10.12.21 minhan
                //m_DataProvider.SetupGenInfo.InitFromDB(m_ApAlarmcheckTime); // 11.01.27 minhan
                //m_ApStatusCheckTime = new TagSetupInfo("AP Status Check Time", OptionType.None, OptionFormat.Float, UnitType.sec, "2.0", "0.0", "5.0");
                //m_DataProvider.SetupGenInfo.InitFromDB(m_ApStatusCheckTime); // 11.04.27 minhan
                m_EgisInterfaceTiemout = new TagSetupInfo("Egis Check Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "5", "0", "20"); // 11.04.27 minhan
                m_DataProvider.SetupGenInfo.InitFromDB(m_EgisInterfaceTiemout); // 11.03.20 minhan
                m_EgisLoaderInterfaceTiemout = new TagSetupInfo("Egis Loader Check Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "5", "0", "60");
                m_DataProvider.SetupGenInfo.InitFromDB(m_EgisLoaderInterfaceTiemout); // 11.04.27 minhan

                m_SetupLdTiltingUPTimeoutAlarm = new TagSetupInfo("LD Tilting UP Timeout value", OptionType.None, OptionFormat.Digit, UnitType.sec, "10", "1", "20"); // 09.11.04 minhan
                m_SetupLdTiltingDWTimeoutAlarm = new TagSetupInfo("LD Tilting DW Timeout value", OptionType.None, OptionFormat.Digit, UnitType.sec, "10", "1", "20"); // 09.11.04 minhan
                m_SetupLdAlignFwTimeoutAlarm = new TagSetupInfo("LD Align Fw Timeout value", OptionType.None, OptionFormat.Digit, UnitType.sec, "10", "1", "20"); // 09.11.04 minhan
                m_SetupLdAlignBwTimeoutAlarm = new TagSetupInfo("LD Align Bw Timeout value", OptionType.None, OptionFormat.Digit, UnitType.sec, "10", "1", "20"); // 09.11.04 minhan
                m_SetupLdIdleRollerFwTimeoutAlarm = new TagSetupInfo("LD IdleRoller Fw Timeout value", OptionType.None, OptionFormat.Digit, UnitType.sec, "10", "1", "20"); // 2013.11.13 fan
                m_SetupLdIdleRollerBwTimeoutAlarm = new TagSetupInfo("LD IdleRoller Bw Timeout value", OptionType.None, OptionFormat.Digit, UnitType.sec, "10", "1", "20"); // 2013.11.13 fan
                //m_GaugeRecheckTime = new TagSetupInfo("GaugeAlarm ReCheck Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "2", "0", "10"); // 09.12.28 minhan
                m_RbIdleUse = new TagSetupInfo("RB Idle Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.NoUse.ToString()); // 10.11.08 minhan
                m_IonizerStopAlarmCheckTime = new TagSetupInfo("Ionizer Stop Alarm Check TimeOut", OptionType.None, OptionFormat.Digit, UnitType.sec, "2", "0", "5"); // 11.03.25 minhan
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupLdTiltingUPTimeoutAlarm);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupLdTiltingDWTimeoutAlarm);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupLdAlignBwTimeoutAlarm);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupLdAlignFwTimeoutAlarm);
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupLdIdleRollerFwTimeoutAlarm);//2013.11.13 fan
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_SetupLdIdleRollerBwTimeoutAlarm);//2013.11.13 fan
                                                                                               //m_DataProvider.SetupGenInfo.InitFromDB(this.m_GaugeRecheckTime);//09.11.02 minhan 11.02.01 minhan

                m_DataProvider.SetupGenInfo.InitFromDB(this.m_IonizerStopAlarmCheckTime); // 11.03.25 minhan
                m_FfuMotorSpeed = new TagSetupInfo("FFU Motor Speed", OptionType.None, OptionFormat.Digit, UnitType.mmpm, "800", "0", "1400"); // 11.05.06 minhan
                m_DataProvider.SetupGenInfo.InitFromDB(this.m_FfuMotorSpeed); // 11.05.06 minhan
                m_SetupTpdEqpName = new TagSetupInfo("TPD File EQP Name", OptionType.None, OptionFormat.String, UnitType.None, "HDC");
                m_DataProvider.SetupGenInfo.InitFromDB(m_SetupTpdEqpName);
                m_SetupTpdSamplingTime = new TagSetupInfo("TPD Sampling Interval", OptionType.None, OptionFormat.Digit, UnitType.sec, "2");
                m_DataProvider.SetupGenInfo.InitFromDB(m_SetupTpdSamplingTime);
                m_SetupTpdFileSaveTime = new TagSetupInfo("TPD File Save Interval", OptionType.None, OptionFormat.Digit, UnitType.min, "1");
                m_DataProvider.SetupGenInfo.InitFromDB(m_SetupTpdFileSaveTime);
                m_SetupTraceServerIp = new TagSetupInfo("Trace Data Server IP", OptionType.None, OptionFormat.IpAddress, UnitType.None, "127.0.0.1");
                m_DataProvider.SetupGenInfo.InitFromDB(m_SetupTraceServerIp);
                m_SetupTraceServerPort = new TagSetupInfo("Trace Data Server PORT", OptionType.None, OptionFormat.Digit, UnitType.None, "7000");
                m_DataProvider.SetupGenInfo.InitFromDB(m_SetupTraceServerPort);

                // 2010.09.09 kimgun
                SetupAkReverseDiff = new TagSetupInfo("Ak Reverse Diff", OptionType.None, OptionFormat.Digit, UnitType.lpm, "100", "1", "1000"); // 2010.09.09 kimgun
                m_DataProvider.SetupGenInfo.InitFromDB(this.SetupAkReverseDiff);

                m_SetupIdleUse = new TagSetupInfo("Idle Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                m_SetupIdleRunTime = new TagSetupInfo("Run Time", OptionType.None, OptionFormat.Digit, UnitType.min, "60"); // 11.02.01 minhan
                m_SetupIdleStopTime = new TagSetupInfo("Stop Time", OptionType.None, OptionFormat.Digit, UnitType.min, "10");
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_SetupIdleUse);
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_SetupIdleRunTime);
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_SetupIdleStopTime);
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_SetupIdleWaitTime);
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_RbIdleUse); // 10.11.08 minhan

                m_SetupHpmjIdleUse = new TagSetupInfo("HPMJ Idle Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());//2009.09.17 kimgun
                m_SetupIdleHpmjPress = new TagSetupInfo("HPMJ Pressure", OptionType.None, OptionFormat.Digit, UnitType.Bar, "80", "80", "130");// 11.04.20 minhan
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_SetupHpmjIdleUse);//2009.09.17 kimgun
                m_DataProvider.SetupIdleInfo.InitFromDB(this.m_SetupIdleHpmjPress);//2009.09.17 kimgun

                m_SetupPumpIntPrePress = new TagSetupInfo("HPMJ PUMP INT PRE PRESS", OptionType.None, OptionFormat.Digit, UnitType.Bar, "10", "1", "100");//2009.09.21 KIMGUN 11.02.01 minhan;//lkl 150929
                m_SetupPumpIntPostPress = new TagSetupInfo("HPMJ PUMP INT POST PRESS", OptionType.None, OptionFormat.Digit, UnitType.Bar, "10", "1", "100");//2009.09.21 KIMGUN;//lkl 150929
                m_SetupPumpIntFlow = new TagSetupInfo("HPMJ PUMP INT FLOW", OptionType.None, OptionFormat.Digit, UnitType.lpm, "5", "1", "10");//2009.09.21 KIMGUN;//lkl 150929
                m_SetupPumpIntTime = new TagSetupInfo("HPMJ PUMP INT TIME", OptionType.None, OptionFormat.Digit, UnitType.sec, "5", "1", "15");//09.12.28 minhan;//lkl 150929
                // 11.02.01 minhan plc에서 펌프 컨트롤을 한다고 하면 위 파라미터는 필요가 없겠지.정말?
                m_SetupFlowWarningMargin = new TagSetupInfo("HPMJ Flowrate Warning Margin", OptionType.None, OptionFormat.Float, UnitType.lpm, "1.0");//090923 LeeChungWon
                m_SetupFlowAlarmMargin = new TagSetupInfo("HPMJ Flowrate Alarm Margin", OptionType.None, OptionFormat.Float, UnitType.lpm, "2.0");//090923 LeeChungWon
                m_SetupPressWarningMargin = new TagSetupInfo("HPMJ Pressure Warning Margin", OptionType.None, OptionFormat.Float, UnitType.Bar, "1.0");//090923 LeeChungWon
                m_SetupPressAlarmMargin = new TagSetupInfo("HPMJ Pressure Alarm Margin", OptionType.None, OptionFormat.Float, UnitType.Bar, "2.0");//090923 LeeChungWon
                m_GaugeAlarmCheckTime = new TagSetupInfo("HPMJ Gauge Alarm Check Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "2", "0", "10"); // 09.12.28 minhan//lkl 150929
                //m_HpmjSetupMainDiIntTime = new TagSetupInfo("HPMJ Main DI Interlock Check Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "1", "0", "3"); // 09.12.28 minhan) 10.12.21 minhan

                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupPumpIntPrePress);//2009.09.21 kimgun 11.02.01 minhan;//lkl 150929
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupPumpIntPostPress);//2009.09.21 kimgun;//lkl 150929
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupPumpIntFlow);//2009.09.21 kimgun;//lkl 150929
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupPumpIntTime);//2009.09.21 kimgun;//lkl 150929             
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupFlowWarningMargin);//090923 LeeChungWon
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupFlowAlarmMargin);//090923 LeeChungWon
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupPressWarningMargin);//090923 LeeChungWon
                m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_SetupPressAlarmMargin);//090923 LeeChungWon
                //m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_GaugeAlarmCheckTime); // 09.12.01 minhan
                //m_DataProvider.SetupHpmjInfo.InitFromDB(this.m_HpmjSetupMainDiIntTime); // 09.12.01 minhan 10.12.21 minhan

                m_ULOutSensorOnTimeMargin = new TagSetupInfo("UL Unit Out Sensor On Time Margin", OptionType.None, OptionFormat.Digit, UnitType.sec, "0", "0", "6"); // 11.02.01 minhan
                m_DataProvider.SetupCvDistance.InitFromDB(this.m_ULOutSensorOnTimeMargin);

                SetInitData();

                //return DmsErrors.Success;
                return ok ? DmsErrors.Success : DmsErrors.InternalError;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                WriteExceptionLog(msg);
                MessageBox.Show(msg);
                //Application.Exit();            

                return DmsErrors.InternalError;
            }
        }

        private void SetInitData()
        {
            //Set Current Recipe
            m_GenInfos.AutoMode = true;   //jemoon : 프로그램 시작시 AutoMode로 시작
            m_GenInfos.CurRecipeId = m_JobCondition.CurrentRecipe.Id;
            m_GenInfos.TactTime = 0;
            m_GenInfos.IdleRunningProgress = "0";
            m_GenInfos.EuvLampOnProgress = "0";
            //GenInfos.EqpState = EqpState.UnKnown;
            //GenInfos.ProcessState = ProcessState.UnKnown;

            //m_GenInfos.EqpCtlMode = '2';
            m_GenInfos.LoopBackTest = false;
            m_GenInfos.OfflineMode = true;
            m_GenInfos.OnlineMode = false;
            m_GenInfos.NormalMode = false;
            m_GenInfos.ParticleMode = false;
            m_GenInfos.RecoveryMode = false;

            GlobalVar.TimeOver = true;
            GlobalVar.StartCount = false;
            GlobalVar.GlassOutComp1 = true;

            GlobalVar.InitParameter();
        }

        #endregion
    }
}
