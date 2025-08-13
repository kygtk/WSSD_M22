using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using Dms.Sequence;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class JobCondition : _JobCondition
    {
        #region Fields
        private bool m_Initialized = false;
        private ServerManager m_Server;
        private TagInterlock m_Interlock;
        private TagRecipe m_CurrentRecipe;
        private GenInfoHandler m_GenInfos;
        #endregion

        #region Singleton code...
        public static readonly JobCondition Instance = new JobCondition();
        #endregion

        #region Constructor
        private JobCondition()
        {
        }

        public void Initialize(ServerManager server)
        {
            if (m_Initialized == false)
            {
                m_Server = server;
                m_Interlock = new TagInterlock(m_Server);
                m_CurrentRecipe = new TagRecipe();
                m_Server.DataProvider.RecipeProvider.GetCurrentRecipe(ref m_CurrentRecipe);
                m_ComponentContainer = DmsComponents.Instance.ComponentContainer;
                m_GenInfos = GenInfoHandler.Instance;

                m_Initialized = true;
            }
        }
        #endregion

        #region Properties
        #endregion

        #region Methods
        public int GlassSize()//2009.07.30 kimgun
        {
            return this.m_Server.SetupGlassSize.GetValue<int>();
        }
        #endregion

        #region Override
        public override TagInterlock Interlock
        {
            get { return m_Interlock; }
            set { m_Interlock = value; }
        }

        public override HeavyInterlock HeavyInterlock
        {
            get { return m_Interlock.Heavy; }
        }

        public override TagRecipe CurrentRecipe
        {
            get { return m_CurrentRecipe; }
            set
            {
                m_CurrentRecipe.Clone(value);
            }
        }

        public override int TactTime
        {
            get { return m_CurrentRecipe.TactTime; }
        }

        public override bool ProcessMode
        {
            get { return m_Server.SetupJobMode.GetValue<bool>(); }
        }

        public override bool SetupIdleUse { get { return m_Server.SetupIdleUse.GetValue<bool>(); } }

        public override int SetupIdleRunTime { get { return m_Server.SetupIdleRunTime.GetValue<int>(); } }

        public override int SetupIdleStopTime { get { return m_Server.SetupIdleStopTime.GetValue<int>(); } }

        public override int SetupHpmjRunTime { get { return m_Server.SetupHpmjRunTime.GetValue<int>(); } }//lkl 150929

        public override int SetupHpmjStopTime { get { return m_Server.SetupHpmjStopTime.GetValue<int>(); } } //lkl 150929

        public override int CvProcessSpeed(int id)
        {
            return m_CurrentRecipe.CvSpeed;
        }
        public override void SetTactTime(TactTimeAct act)
        {
            if (act == TactTimeAct.ttCOUNT_START)
            {
                GlobalVar.StartCount = true;// m_Server.SeqFlag.StartCount = true;
                m_GenInfos.TactTime = 0;
            }
            else if (act == TactTimeAct.ttCOUNT_END)
            {
                GlobalVar.StartCount = false;// m_Server.SeqFlag.StartCount = false;
                m_GenInfos.TactTime = 0;
            }
        }

        public override void SetRecipe2JobCond(string recipeId)
        {
            if (recipeId == null || recipeId == "") return;

            TagRecipe item = new TagRecipe();
            if (true == m_Server.DataProvider.RecipeProvider.GetRecipe(recipeId, ref item))
            {
                this.CurrentRecipe = item;
                m_GenInfos.CurRecipeId = this.CurrentRecipe.Id;
                m_Server.DataProvider.RecipeProvider.SetCurrentRecipe(recipeId);
            }
            else
                GlobalVar.RecipeNG = true;//2009.09.14 kimgun
        }

        public override bool RbDirection(RbUnit unit)
        {
            if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1UpDir;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1LoDir;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2UpDir;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2LoDir;
            }
            else
            {
                return false;
            }
        }

        public override double RbProcessSpeed(RbUnit unit)
        {
            if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1UpSpeed;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1LoSpeed;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2UpSpeed;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2LoSpeed;
            }
            else
            {
                return 0.0;
            }
        }

        public override bool RbUse(RbUnit unit)
        {
            if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1UpUse;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1LoUse;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2UpUse;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2LoUse;
            }
            else
            {
                return false;
            }
        }

        public override double RbGap(RbUnit unit)
        {
            if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1UpGap;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor1_Name)
            {
                return m_CurrentRecipe.RB1LoGap;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Up_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2UpGap;
            }
            else if (unit.Motor.Name == eqpRbMotors._RB_Unit_Lo_RbMotor2_Name)
            {
                return m_CurrentRecipe.RB2LoGap;
            }
            else
            {
                return 0.0;
            }
        }

        public override bool EuvUse(_EuvUnit unit)
        {
            return ((UshioEuvUnit)unit).SetupEuvUse.GetValue<bool>();
        }



        public override bool HpmjUse(Hpmj unit)
        {
            return this.CurrentRecipe.HpmjUse;
        }

        public override int HpmjPressure(Hpmj unit)
        {
            return this.CurrentRecipe.HpmjPressure;
        }

        public override bool EuvLampUse(EuvLamp unit)
        {
            //RECIPE에 따라 변경해 주어야 함.
            if ((unit.Name == eqpEuvLamps._EuvUnit_Lamp1_Name) && this.CurrentRecipe.EUVLAMP1_USE) return true;
            if ((unit.Name == eqpEuvLamps._EuvUnit_Lamp2_Name) && this.CurrentRecipe.EUVLAMP2_USE) return true;
            if ((unit.Name == eqpEuvLamps._EuvUnit_Lamp3_Name) && this.CurrentRecipe.EUVLAMP3_USE) return true;
            return false;
        }


        //public override double ApVoltage(_Plasma unit)
        //{
        //    // Recipe로 변경해주어야 함
        //    //return ((SeAp)unit).SetupApPowerSet.GetValue<double>();
        //    //return 0.0;
        //    return this.CurrentRecipe.ApVoltage; // 09.09.09 minhan
        //}

        //public override double ApN2Flow(_Plasma unit)
        //{
        //    // Recipe로 변경해주어야 함
        //    //return ((SeAp)unit).SetupApN2FlowSet.GetValue<double>();
        //    //return 0.0;
        //    return this.CurrentRecipe.ApN2Flow; // 09.09.09 minhan
        //}

        //public override double ApCDAFlow(_Plasma unit)
        //{
        //    // Recipe로 변경해주어야 함
        //    //return ((SeAp)unit).SetupApCDAFlowSet.GetValue<double>();
        //    //return 0.0;
        //    return this.CurrentRecipe.ApCDAFlow; // 09.09.09 minhan
        //}

        public override int GetGlassCountMaxCapability()
        {
            int count = m_Server.SetupMaxGlassNo.GetValue<int>();
            return count;
        }

        public override int GetGlassCountPutIntoPossible()
        {
            int count = GetGlassCountMaxCapability() - GetGlassCountInEqp();
            return count;
        }

        public override List<int> GetCurrentAlarmIds()
        {
            List<int> ids = m_Server.DataProvider.CurrentAlarms.GetCurrentAlarmIds();
            List<int> alarmIds = new List<int>();

            AlarmListProvider alarmList = m_Server.DataProvider.AlarmList;
            for (int i = 0; i < ids.Count; i++)
            {
                TagAlarm alarm = new TagAlarm();
                bool existInAlarmList = alarmList.GetAlarm(ids[i], alarm);
                if (existInAlarmList)
                {
                    if (alarm.Level == AlarmLevel.S)
                    {
                        alarmIds.Add(alarm.Id);
                    }
                }
            }

            return alarmIds;
        }

        public override List<int> GetCurrnetWarningIds()
        {
            List<int> ids = m_Server.DataProvider.CurrentAlarms.GetCurrentAlarmIds();
            List<int> waringIds = new List<int>();

            AlarmListProvider alarmList = m_Server.DataProvider.AlarmList;
            for (int i = 0; i < ids.Count; i++)
            {
                TagAlarm alarm = new TagAlarm();
                bool existInAlarmList = alarmList.GetAlarm(ids[i], alarm);
                if (existInAlarmList)
                {
                    if (alarm.Level == AlarmLevel.L)
                    {
                        waringIds.Add(alarm.Id);
                    }
                }
            }

            return waringIds;
        }
        #endregion
    }
}
