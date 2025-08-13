using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public enum JobEndCode
    {
        NormalEnd,
        RobotAlarm,
        JobCancel,
        OperatorCall,
    }

    public class ThreadRbtMng : XSequence
    {
        #region Fields
        protected static _GenericCollection<LoaderUnit> m_Loaders = new _GenericCollection<LoaderUnit>();
        protected static int m_UnitCount;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public ThreadRbtMng(int scanTime, IServerManager server)
            : base(scanTime)
        {
        }
        #endregion

        #region Override
        protected override void RegisterSequences()
        {
            base.RegisterSequences();
        }

        public override void Sequence()
        {
            base.Sequence();
        }
        #endregion

        #region Methods
        #endregion
    }

    public class SeqRobotMng : XSeqFunction
    {
        #region Fields
        protected _IndexRobot m_Robot = null;
        protected ILoaderSlave m_Loader = null;

        protected int m_TaskPortId;
        protected int m_TaskSlotId;
        protected RobotActionType m_TaskRbtAction = RobotActionType.None;
        protected LoaderRobotHand m_TaskRbtHand = LoaderRobotHand.lowerHand;  // For Manual Action

        protected Alarm ALM_JobAbnormalEnd = null;
        #endregion

        #region Constructor
        public SeqRobotMng(_IndexRobot robot, ILoaderSlave loader)
        {
            m_Robot = robot;
            m_Loader = loader;

            ALM_JobAbnormalEnd = new Alarm(robot.Name + " Job Abnormal End Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion 

        #region Override

        public override int Do()
        {
            RobotAlarmManage();
            InterlockSignals();
            SetExternalParameter();     // Always Set Params

            RobotExternalControl();

            int returnValue = -1;
            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    if (GetNextTask(ref m_TaskRbtAction, ref m_TaskPortId, ref m_TaskSlotId))
                    {
                        seqNo = 100;
                    }
                    else if (GetManualTask(ref m_TaskRbtAction, ref m_TaskRbtHand, ref m_TaskPortId, ref m_TaskSlotId))
                    {
                        seqNo = 200;
                    }
                    break;

                case 100:
                    if (CheckRobotProcess() != 0)
                    {
                        int rv = DoRobotAction(m_TaskRbtAction, m_TaskPortId, m_TaskSlotId);

                        if (rv == (int)JobEndCode.NormalEnd)
                        {
                            // Sequence Normal End
                            seqNo = 0;
                        }
                        else if (rv == (int)JobEndCode.RobotAlarm)
                        {
                            GenInfoHandler.Instance.CycleStop = true;
                            seqNo = 1000;
                        }
                        else if (rv == (int)JobEndCode.JobCancel)
                        {
                            m_AlarmId = ALM_JobAbnormalEnd.Id;
                            m_Robot.ServerManager.EqpStateManager.SetAlarm(m_AlarmId);
                            seqNo = 2000;
                        }
                        else if (rv == (int)JobEndCode.OperatorCall)
                        {
                        }
                    }
                    break;

                case 200:
                    if (CheckRobotProcess() != 0)
                    {
                        int rv = DoRobotManualAction(m_TaskRbtAction, m_TaskRbtHand, m_TaskPortId, m_TaskSlotId);

                        if (rv == (int)JobEndCode.NormalEnd)
                        {
                            // Sequence Normal End
                            seqNo = 0;
                        }
                        else if (rv == (int)JobEndCode.RobotAlarm)
                        {
                            seqNo = 1000;
                        }
                        else if (rv == (int)JobEndCode.JobCancel)
                        {
                            m_AlarmId = ALM_JobAbnormalEnd.Id;
                            m_Robot.ServerManager.EqpStateManager.SetAlarm(m_AlarmId);
                            seqNo = 2000;
                        }
                        else if (rv == (int)JobEndCode.OperatorCall)
                        {
                        }
                    }
                    break;

                case 1000:  // CASE: Robot Action Sequence Abnormal End Recovery
                    if (m_Robot.IsRobotReady())
                    {
                        if (m_Robot.AlarmCondition == RobotAlarmCondition.NoAlarm)
                        {
                            seqNo = 0;
                        }
                    }
                    break;

                case 2000:
                    if (m_Robot.ServerManager.EqpStateManager.AlarmResetSwitchPushed)
                    {
                        if (m_Robot.IsRobotReady())
                        {
                            m_Robot.ServerManager.EqpStateManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            seqNo = 0;
                        }
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return returnValue;
        }

        #endregion

        #region Methods
        protected virtual int DoRobotHome()
        {
            return -1;
        }
        protected virtual bool GetNextTask(ref RobotActionType action, ref int portId, ref int slotId)
        {
            return false;
        }

        protected virtual bool GetManualTask(ref RobotActionType action, ref LoaderRobotHand hand, ref int portId, ref int slotId)
        {
            return false;
        }

        protected virtual int CheckRobotProcess()
        {
            return -1;
        }

        protected virtual int DoRobotAction(RobotActionType action, int portId, int slotId)
        {
            return -1;
        }

        protected virtual int DoRobotManualAction(RobotActionType action, LoaderRobotHand hand, int portId, int slotId)
        {
            return -1;
        }

        protected virtual int RobotExternalControl()
        {
            return -1;
        }

        protected virtual void RobotAlarmManage()
        {
        }

        protected virtual void InterlockSignals()
        {
        }

        protected virtual void SetExternalParameter()
        {
        }
        #endregion
    }

}
