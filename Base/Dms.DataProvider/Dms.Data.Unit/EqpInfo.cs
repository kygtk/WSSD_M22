using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Cim.Common;

namespace Dms.Data
{
    [Serializable()]
    public class EqpInfo
    {
        #region Fields
        private int m_UnitNo;
        private string m_UnitName;
        private eqpMODE m_Mode;
        private eqpProcessMode m_ProcessMode = eqpProcessMode.eqpManual;
        private eqpSTATUS m_Status = eqpSTATUS.eqpIdle;
        private eqpDETOUR_MODE m_DetourMode;

        private int m_MaxGlassCount;
        private int m_CurrentGlassCount;
        private int m_PossibleGlassCount;
        private int m_BufferedGlassCount;
        private eqpTACT_TIME_CONTROL_MODE m_TactTimeMode;
        private Recovery m_Recovery = new Recovery();
        private eqpSETUP_STATUS m_SetupStatus;

        private List<EqpGlassData> m_SubUnitGlassData = new List<EqpGlassData>();
        private EqpScrap m_ScrapData;

        private EqpGlassData m_LoadGlassData = new EqpGlassData();
        private EqpGlassData m_UnloadGlassData = new EqpGlassData();

        private List<int> m_CurrentAlarmList = new List<int>();

        private int m_CurrentRecipeId;

        private bool m_AlarmExist;
        private bool m_WarningExist;
        private bool m_InProcessError;
        private bool m_Pause;

        private bool m_IsInStage = false;
        private bool m_IsOutStage = false;

        private List<bool> m_RecipeUse = new List<bool>();

        private EqpCommand m_OriginalPoint = new EqpCommand();
        private EqpCommand m_Prepare = new EqpCommand();
        private EqpCommand m_InitializeGlassData = new EqpCommand();
        private EqpCommand m_BuzzerStop = new EqpCommand();
        private EqpCommand m_AlarmReset = new EqpCommand();
        private EqpCommand m_CommandMessage = new EqpCommand();
        private EqpCommand m_OperatorCallOnReq = new EqpCommand();
        private EqpCommand m_OperatorCallOffReq = new EqpCommand();
        private EqpCommand m_PauseOnReq = new EqpCommand();
        private EqpCommand m_PauseOffReq = new EqpCommand();

        private string m_CimMessage = "";

        private bool m_RecipeAvailableChekcReq = false;
        private bool m_InitialDataAvailableCheckReq = false;
        private bool m_InitialModeAvailableCheckReq = false;

        private string m_HostRecipeId = "";
        private bool m_HostRecipeCheckReq = false;
        private bool m_HostRecipeListReq = false;

        private bool m_SupplyEnable = true;


        // Solar Equipment
        private List<eqpSolarStatus> m_SolarUnitState = new List<eqpSolarStatus>();
        #endregion

        #region Properties

        public int UnitNo
        {
            get { return m_UnitNo; }
            set { m_UnitNo = value; }
        }

        public string UnitName
        {
            get { return m_UnitName; }
            set { m_UnitName = value; }
        }

        public eqpMODE UnitMode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        public eqpProcessMode ProcessMode
        {
            get { return m_ProcessMode; }
            set { m_ProcessMode = value; }
        }

        public eqpSTATUS UnitStatus
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        public eqpDETOUR_MODE UnitDetourMode
        {
            get { return m_DetourMode; }
            set { m_DetourMode = value; }
        }

        public int MaxGlassCount
        {
            get { return m_MaxGlassCount; }
            set { m_MaxGlassCount = value; }
        }

        public int CurrentGlassCount
        {
            get { return m_CurrentGlassCount; }
            set { m_CurrentGlassCount = value; }
        }

        public int PossibleGlassCount
        {
            get { return m_PossibleGlassCount; }
            set { m_PossibleGlassCount = value; }
        }

        public int BufferedGlassCount
        {
            get { return m_BufferedGlassCount; }
            set { m_BufferedGlassCount = value; }
        }

        public eqpTACT_TIME_CONTROL_MODE TactTimeMode
        {
            get { return m_TactTimeMode; }
            set { m_TactTimeMode = value; }
        }

        public eqpSETUP_STATUS SetupStatus
        {
            get { return m_SetupStatus; }
            set { m_SetupStatus = value; }
        }

        public int CurRecipeID
        {
            get { return m_CurrentRecipeId; }
            set { m_CurrentRecipeId = value; }
        }

        public int AlarmCount
        {
            get { return m_CurrentAlarmList.Count; }
        }

        public List<int> AlarmList
        {
            get { return m_CurrentAlarmList; }
        }

        public List<EqpGlassData> SubUnitGlassData
        {
            get { return m_SubUnitGlassData; }
            set { m_SubUnitGlassData = value; }
        }

        public List<bool> RecipeUse
        {
            get { return m_RecipeUse; }
            set { m_RecipeUse = value; }
        }

        public EqpScrap Scrap
        {
            get { return m_ScrapData; }
            set { m_ScrapData = value; }
        }

        public EqpGlassData LoadingGlassData
        {
            get { return m_LoadGlassData; }
            set { m_LoadGlassData = value; }
        }

        public EqpGlassData UnloadingGlassData
        {
            get { return m_UnloadGlassData; }
            set { m_UnloadGlassData = value; }
        }

        public bool Pause
        {
            get { return m_Pause; }
            set { m_Pause = value; }
        }

        public bool AlarmExist
        {
            get { return m_AlarmExist; }
            set { m_AlarmExist = value; }
        }

        public bool WarningExist
        {
            get { return m_WarningExist; }
            set { m_WarningExist = value; }
        }

        public bool InProcessError
        {
            get { return m_InProcessError; }
            set { m_InProcessError = value; }
        }

        public EqpScrap ScrapData
        {
            get { return m_ScrapData; }
            set { m_ScrapData = value; }
        }

        public EqpCommand OriginalPoint
        {
            get { return m_OriginalPoint; }
            set { m_OriginalPoint = value; }
        }
        public EqpCommand Prepare
        {
            get { return m_Prepare; }
            set { m_Prepare = value; }
        }
        public EqpCommand InitializeGlassData
        {
            get { return m_InitializeGlassData; }
            set { m_InitializeGlassData = value; }
        }
        public EqpCommand BuzzerStop
        {
            get { return m_BuzzerStop; }
            set { m_BuzzerStop = value; }
        }
        public EqpCommand AlarmReset
        {
            get { return m_AlarmReset; }
            set { m_AlarmReset = value; }
        }
        public EqpCommand CommandMessage
        {
            get { return m_CommandMessage; }
            set { m_CommandMessage = value; }
        }
        public EqpCommand OperatorCallOnReq
        {
            get { return m_OperatorCallOnReq; }
            set { m_OperatorCallOnReq = value; }
        }
        public EqpCommand OperatorCallOffReq
        {
            get { return m_OperatorCallOffReq; }
            set { m_OperatorCallOffReq = value; }
        }
        public EqpCommand PauseOnReq
        {
            get { return m_PauseOnReq; }
            set { m_PauseOnReq = value; }
        }
        public EqpCommand PauseOffReq
        {
            get { return m_PauseOffReq; }
            set { m_PauseOffReq = value; }
        }
        public string CimMessage
        {
            get { return m_CimMessage; }
            set { m_CimMessage = value; }
        }
        public bool RecipeAvailableCheckReq
        {
            get { return m_RecipeAvailableChekcReq; }
            set { m_RecipeAvailableChekcReq = value; }
        }

        public bool InitialDataAvailableCheckReq
        {
            get { return m_InitialDataAvailableCheckReq; }
            set { m_InitialDataAvailableCheckReq = value; }
        }
        public bool InitialModeAvailableCheckReq
        {
            get { return m_InitialModeAvailableCheckReq; }
            set { m_InitialModeAvailableCheckReq = value; }
        }

        public string HostRecipeId
        {
            get { return m_HostRecipeId; }
            set { m_HostRecipeId = value; }
        }
        public bool HostRecipeCheckReq
        {
            get { return m_HostRecipeCheckReq; }
            set { m_HostRecipeCheckReq = value; }
        }
        public bool HostRecipeListReq
        {
            get { return m_HostRecipeListReq; }
            set { m_HostRecipeListReq = value; }
        }
        public bool IsInStage
        {
            get { return m_IsInStage; }
            set { m_IsInStage = value; }
        }
        public bool IsOutStage
        {
            get { return m_IsOutStage; }
            set { m_IsOutStage = value; }
        }

        public Recovery Recovery
        {
            get { return m_Recovery; }
            set { m_Recovery = value; }
        }

        public bool SupplyEnable
        {
            get { return m_SupplyEnable; }
            set { m_SupplyEnable = value; }
        }

        public List<eqpSolarStatus> SolarUnitState
        {
            get { return m_SolarUnitState; }
            set { m_SolarUnitState = value; }
        }
        #endregion

        public void Initialize(int MaxSubUnitNo, int RecipeUseCount)
        {
            for (int i = 0; i < MaxSubUnitNo; i++)
            {
                EqpGlassData GlasssData = new EqpGlassData();
                m_SubUnitGlassData.Add(GlasssData);
            }

            for (int i = 0; i < RecipeUseCount; i++)
            {
                m_RecipeUse.Add(false);
            }
        }

        public void InitializeSolarState(int MaxSubUnitState)
        {
            for (int i = 0; i < MaxSubUnitState; i++)
            {
                eqpSolarStatus status = new eqpSolarStatus();
                SolarUnitState.Add(status);
            }
        }

        public bool SetAlarmId(int nId)
        {
            if (!m_CurrentAlarmList.Contains(nId))
            {
                m_CurrentAlarmList.Add(nId);

                return true;
            }

            return false;
        }

        public bool ResetAlarmId(int nId)
        {
            if (m_CurrentAlarmList.Contains(nId))
            {
                m_CurrentAlarmList.Remove(nId);

                return true;
            }

            return false;
        }
    }

    public class EqpScrap
    {
        public int PortNo;
        public int SlotNo;
        public int ScrapCode;
    }

    public class EqpCommand
    {
        private bool m_bCommandReq;
        private bool m_bCommandProcessing;
        private bool m_bCommandComplete;

        public EqpCommand()
        {
            m_bCommandReq = false;
            m_bCommandProcessing = false;
            m_bCommandComplete = false;
        }

        public bool CommandReq
        {
            get { return m_bCommandReq; }
            set { m_bCommandReq = value; }
        }

        public bool CommandProcessing
        {
            get { return m_bCommandProcessing; }
            set { m_bCommandProcessing = value; }
        }

        public bool CommandComplete
        {
            get { return m_bCommandComplete; }
            set { m_bCommandComplete = value; }
        }

        public void Reset()
        {
            m_bCommandReq = false;
            m_bCommandProcessing = false;
            m_bCommandComplete = false;
        }
    }


    public class Recovery
    {
        private eqpRecoveryType m_Type;
        private bool m_RecoveryReq;
        private bool m_RecoveryStarting;
        private bool m_RecoveryComplete;
        private eqpRECOVERY_STATUS m_RecoveryStatus;

        public Recovery()
        {
            m_Type = eqpRecoveryType.eqpRECOVERY;
            m_RecoveryReq = false;
            m_RecoveryStarting = false;
            m_RecoveryComplete = false;

            m_RecoveryStatus = eqpRECOVERY_STATUS.eqpNone;
        }

        public eqpRecoveryType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public bool RecoveryReq
        {
            get { return m_RecoveryReq; }
            set { m_RecoveryReq = value; }
        }

        public bool RecoveryStarting
        {
            get { return m_RecoveryStarting; }
            set { m_RecoveryStarting = value; }
        }

        public bool RecoveryComplete
        {
            get { return m_RecoveryComplete; }
            set { m_RecoveryComplete = value; }
        }

        public eqpRECOVERY_STATUS RecoveryStatus
        {
            get { return m_RecoveryStatus; }
            set { m_RecoveryStatus = value; }
        }
    }
}
