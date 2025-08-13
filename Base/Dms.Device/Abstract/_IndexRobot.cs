using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class RobotHandStage
    {
        private LoaderGlass m_Glass = new LoaderGlass();
        public LoaderGlass Glass
        {
            get { return m_Glass; }
            set { m_Glass = value; }
        }

        private bool m_GlassExist = false;
        public bool GlassExist
        {
            get { return m_GlassExist; }
            set { m_GlassExist = value; }
        }


        public RobotHandStage()
        {
        }
    }

    public enum RobotTask
    { 
        //Noop = -1,
        GetFromCst,         // Process 이전 Slot을 CST에서 꺼냄
        PutWaitToStage,     // 
        PutToStage,         // Process 이전 Slot을 EQP에 투입
        GetWaitFromStage,   //
        GetFromStage,       // Process 완료 Slot을 EQP에서 꺼냄
        PutToCst,           // Process 완료 Slot을 CST에 투입
        GetGlassAbort,      // Process 대기 Slot을 CST에 투입 - (Lot Abort 시 EQP에 투입대기중인 Slot을 CST로 복귀)
        //HomeReturn,       // External Command로 옮김
        //ThickKindSet,     // External Command로 옮김
        MaxTaskCount
    }

    public enum RobotAlarmCondition
    {
        NoAlarm = 0,
        HeavyAlarm = 1,
        LightAlarm = 2,
    }

    //public class RobotTaskPriority
    //{
    //    private List<RobotTask> m_TaskPriority = new List<RobotTask>();

    //    public List<RobotTask> TaskPriority
    //    {
    //        get { return m_TaskPriority; }
    //        set { m_TaskPriority = value; }
    //    }

    //    public RobotTaskPriority()
    //    {
    //        m_TaskPriority.Add(RobotTask.GetFromStage);
    //        m_TaskPriority.Add(RobotTask.PutToStage);
    //        m_TaskPriority.Add(RobotTask.PutToCst);
    //        m_TaskPriority.Add(RobotTask.GetFromCst);
    //    }
    //}
    
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    abstract public class _IndexRobot : _DeviceAsm
    {
        #region Fields
        protected RobotHandStage m_UpperHandStage = new RobotHandStage();
        protected RobotHandStage m_LowerHandStage = new RobotHandStage();
        protected int m_UpperHandCode;
        protected int m_LowerHandCode;
        protected LoaderRobotHand m_HandForBeforeProcess = LoaderRobotHand.lowerHand;
        protected LoaderRobotHand m_HandForAfterProcess = LoaderRobotHand.upperHand;
        private RobotTask[] m_TaskPriority = new RobotTask[] { RobotTask.GetFromStage, RobotTask.PutToStage, RobotTask.PutToCst, RobotTask.GetFromCst };
        //private bool m_TaskPriorityCycling = false;
        private int m_KindSettingNo;
        private RobotAlarmCondition m_AlarmCondition = RobotAlarmCondition.NoAlarm;
        private nxEX_CONTROL m_ExternalControl = nxEX_CONTROL.EX_NONE;

        protected SetupLoaderInfoProvider m_SetupLoaderInfoProvider = null;
        protected TagSetupInfo m_SetupRobotPinUpDnUse;
        protected TagSetupInfo m_SetupRobotEffectAlignY;
        protected TagSetupInfo m_SetupRobotEffectAlignX;
        protected TagSetupInfo m_SetupRobotVirtualMode;
        protected TagSetupInfo m_SetupRobotSpeedLimitMode;
        protected TagSetupInfo m_SetupRobotSpeedSelect;
        protected TagSetupInfo m_SetupRobotSpeedManual;
        protected TagSetupInfo m_SetupRobotAdsorption;

        private int m_ScrapPortNo = 0;
        private int m_ScrapSlotNo = 0;
        private bool m_ScrapRequest = false;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public RobotHandStage UpperHandStage
        {
            get { return m_UpperHandStage; }
            set { m_UpperHandStage = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RobotHandStage LowerHandStage
        {
            get { return m_LowerHandStage; }
            set { m_LowerHandStage = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RobotAlarmCondition AlarmCondition
        {
            get { return m_AlarmCondition; }
            set { m_AlarmCondition = value; }
        }
        [Category("DMS : Basic Setting - Hand Use case")]
        public LoaderRobotHand HandForBeforeProcess
        {
            get { return m_HandForBeforeProcess; }
            set { m_HandForBeforeProcess = value; }
        }
        [Category("DMS : Basic Setting - Hand Use case")]
        public LoaderRobotHand HandForAfterProcess
        {
            get { return m_HandForAfterProcess; }
            set { m_HandForAfterProcess = value; }
        }
        [Category("DMS : Basic Setting - Task Priority")]
        public RobotTask[] TaskPriority
        {
            get { return m_TaskPriority; }
            set { m_TaskPriority = value; }
        }
        //[Category("DMS : Basic Setting - Task Priority")]
        //public bool TaskPriorityCycling
        //{
        //    get { return m_TaskPriorityCycling; }
        //    set { m_TaskPriorityCycling = value; }
        //}
        [Browsable(false), XmlIgnore()]
        public int KindSettingNo
        {
            get { return m_KindSettingNo; }
            set { m_KindSettingNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public nxEX_CONTROL ExternalControl
        {
            get { return m_ExternalControl; }
            set { m_ExternalControl = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotPinUpDnUse
        {
            get { return m_SetupRobotPinUpDnUse; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotEffectAlignY
        {
            get { return m_SetupRobotEffectAlignY; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotEffectAlignX
        {
            get { return m_SetupRobotEffectAlignX; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotVirtualMode
        {
            get { return m_SetupRobotVirtualMode; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotSpeedLimitMode
        {
            get { return m_SetupRobotSpeedLimitMode; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotSpeedSelect
        {
            get { return m_SetupRobotSpeedSelect; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotSpeedManual
        {
            get { return m_SetupRobotSpeedManual; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRobotAdsorption
        {
            get { return m_SetupRobotAdsorption; }
        }

        [Browsable(false), XmlIgnore()]
        public int ScrapPortNo
        {
            get { return m_ScrapPortNo; }
            set { m_ScrapPortNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int ScrapSlotNo
        {
            get { return m_ScrapSlotNo; }
            set { m_ScrapSlotNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool ScrapRequest
        {
            get { return m_ScrapRequest; }
            set { m_ScrapRequest = value; }
        }
        #endregion

        #region Abstract methods
        abstract public bool IsAlarm();
        abstract public int GetSlotNo();
        abstract public int GetOperationHand();
        abstract public RobotSelectedMode GetModeSelected();
        abstract public bool IsUpperHandGlassDetected();
        abstract public bool IsLowerHandGlassDetected();
        abstract public bool IsHandGlassDetected(LoaderRobotHand hand);
        abstract public bool IsHandGlassEmpty(LoaderRobotHand hand);
        abstract public bool IsRobotReady();
        abstract public bool IsRobotRunning();
        abstract public bool IsRobotServoOn();
        abstract public bool IsRobotHandHome();
        abstract public bool IsRobotEmoAlarm();
        abstract public bool IsRobotHold();
        abstract public bool IsOperationBegin();
        abstract public bool IsRobotBusy();
        abstract public void SetExternalStart(bool on);
        abstract public void SetRobotPause(bool pause);
        abstract public void SetGlassKind(int kindNo);
        abstract public void SetSlotSelection(int slotNo);
        abstract public int GetSelectedSlotNo();
        abstract public void SetExternalSpeedRatio();
        abstract public void SetGlassThickness();
        abstract public void SetAdsorptionTime();
        abstract public RobotCommandResult SetRobotAction(RobotActionType act, int portNo, int slotNo);
        abstract public bool GetGlassOriginalPosition(LoaderRobotHand hand, ref int portId, ref int slotId);
        abstract public bool GetGlassTargetPosition(LoaderRobotHand hand, ref int portId, ref int slotId);
        abstract public bool SetGlassOriginalPosition(LoaderRobotHand hand, int portId, int slotId);
        abstract public bool SetGlassTargetPosition(LoaderRobotHand hand, int portId, int slotId);
        abstract public bool GetGlass(LoaderRobotHand hand, ref LoaderGlass gls);
        abstract public bool SetGlass(LoaderRobotHand hand, LoaderGlass gls);
        abstract public bool ResetGlass(LoaderRobotHand hand);
        abstract public bool GetRobotTask(ref RobotActionType action, ref int portId, ref int slotId);
        abstract public bool GetRobotTask(ref RobotActionType action, ref LoaderRobotHand hand, ref int portId, ref int slotId);

        abstract public void InitJobSeq();
        abstract public void InitScrapSeq();

        abstract public int SeqExternalControl();
        abstract public int SeqRobotHome();
        abstract public int SeqRobotAlarm();
        abstract public int SeqGetGlass(LoaderRobotHand hand, int portId, int slotId);
        abstract public int SeqPutGlass(LoaderRobotHand hand, int portId, int slotId);
        abstract public int SeqGetStandby(LoaderRobotHand hand, int portId, int slotId);
        abstract public int SeqPutStandby(LoaderRobotHand hand, int portId, int slotId);
        abstract public int SeqYAlign(LoaderRobotHand hand, int portId);
        abstract public int SeqProcessCheck();
        abstract public int SeqWait(int portId, int slotId);
        abstract public int SeqExchange(LoaderRobotHand handPut, int portId, int slotId);
        abstract public int SeqGetGlassWithPinUpDn(LoaderRobotHand hand, int portId, int slotId);
        abstract public int SeqPutGlassWithPinUpDn(LoaderRobotHand hand, int portId, int slotId);
		abstract public bool IsRobotInterferingWith(int portId);

        public RobotTask GetTaskPriority(int i)
        {
            //if(i < (int)RobotTask.GetFromCst || i > (int)RobotTask.PutToCst)
            //{
            //    return RobotTask.Noop;
            //}
            //else
            {
                return m_TaskPriority[i];
            }
        }

        public void SetTaskSchedule(RobotTask curTask)
        {
            //if(TaskPriorityCycling)
            //{
            //    RobotTask[] taskBuffer = new RobotTask[TaskPriority.Length];
                
            //    for(int i = 0; i < TaskPriority.Length; i++)
            //    {
            //        if(curTask == TaskPriority[i])
            //        {
            //            taskBuffer[TaskPriority.Length - 1] = TaskPriority[i];
            //            for(int j = i; j < TaskPriority.Length - 1; j++)
            //            {
            //                taskBuffer[j] = TaskPriority[j + 1];
            //            }
            //            TaskPriority = taskBuffer;
            //        }
            //        else
            //        {
            //            taskBuffer[i] = TaskPriority[i];
            //        }
            //    }
            //}
        }
        #endregion
    }
}
