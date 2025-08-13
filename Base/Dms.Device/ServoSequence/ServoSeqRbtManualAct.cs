using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtManualAct : XSeqFunction
    {
        public SeqRbtManualAct()
        { 
        }
        public SeqRbtManualAct(ServoUnit unit)
        {
            Unit = unit;
            StartPos = new RbtPos(Unit.AxisCount);
            TargetPos = new RbtPos(Unit.AxisCount);
            TempPos = new RbtPos(Unit.AxisCount);
        }
        private ServoUnit Unit;

        private short m_SelectedPointId;
        private RbtPos m_startPos;
        private RbtPos m_tempPos;
        private RbtPos m_targetPos;
        private Mutex m_Mutex = new Mutex();
           

        public short SelectedPointId
        {
            get { return m_SelectedPointId; }
            set { m_SelectedPointId = value; }
        }

        public RbtPos StartPos
        {
            get { return m_startPos; }
            set { m_startPos = value; }
        }
        public RbtPos TempPos
        {
            get { return m_tempPos; }
            set { m_tempPos = value; }
        }
        public RbtPos TargetPos
        {
            get { return m_targetPos; }
            set { m_targetPos = value; }
        }

        public int ManualCmd
        {
            get { return m_SeqNo; }
            set 
            {
                m_Mutex.WaitOne();
                m_SeqNo = value;
                m_Mutex.ReleaseMutex();
            }
        }

        public override int Do()
        {
            int result = -1;

            m_Mutex.WaitOne();
            {
                if (Unit.ServerManager.JobCond.Interlock.EmoCondition.IsAlarm)
                {
                    this.m_SeqNo = 0;
                }
                else
                {

                    int nSeqNo = this.m_SeqNo;
                    switch (nSeqNo)
                    {
                        case (short)RbtAction.None:
                            break;
                        case (short)RbtAction.Estop:
                            if (Unit.RbtEStop())
                            {
                                Unit.Message = "Command : Servo E-Stop - Ok";
                                nSeqNo = (short)RbtAction.None;
                            }
                            break;
                        case (short)RbtAction.ServoOn:
                            if (Unit.RbtReset())
                            {
                                Unit.Message = "Command : Servo ON - Ok";
                                nSeqNo = (short)RbtAction.None;
                            }
                            break;
                        case (short)RbtAction.Home:
                            if (Unit.IsInterlockCondition())
                            {
                                Unit.Message = "Servo E-Stop : Interlock Condition";
                                nSeqNo = (short)RbtAction.Estop;
                            }
                            else if (0 == (result = Unit.RbtMoveHome()))
                            {
                                Unit.Message = "Command : Servo Home - Ok";
                                nSeqNo = (short)RbtAction.None;
                            }
                            else if (0 < result)
                            {
                                Unit.Message = "Command : Servo Home - NG";
                                nSeqNo = (short)RbtAction.None;
                            }
                            break;
                        case (short)RbtAction.Move:
                            if (Unit.IsInterlockCondition(this.SelectedPointId))
                            {
                                Unit.Message = "Servo E-Stop : Interlock Condition";
                                nSeqNo = (short)RbtAction.Estop;
                            }
                            else if (0 == (result = Unit.RbtMovePos(this.SelectedPointId)))
                            { // OK
                                Unit.Message = "Command : Servo Move - Ok";
                                nSeqNo = (short)RbtAction.None;
                            }
                            else if (0 < result)
                            { // NG
                                Unit.Message = "Command : Servo Move - NG";
                                nSeqNo = (short)RbtAction.None;
                            }
                            break;
                        case (short)RbtAction.MoveRepeat:
                            {
                                Unit.GetCurPosition(ref m_startPos);
                                TargetPos = Unit.GetTeachPointPos(this.SelectedPointId);

                                Unit.Message = "Command : Servo Move Repeat";
                                nSeqNo = 110;
                            }
                            break;
                        case 110:
                            if (Unit.IsInterlockCondition())
                            {
                                Unit.Message = "Servo E-Stop : Interlock Condition";
                                nSeqNo = (short)RbtAction.Estop;
                            }
                            else if (0 == (result = Unit.RbtMovePos(TargetPos)))
                            {
                                TempPos = TargetPos.Clone();
                                TargetPos = StartPos.Clone();
                                StartPos = TempPos.Clone();

                                m_StartTicks = XFunc.GetTickCount();

                                Unit.Message = "Command : Servo Move Repeat";

                                nSeqNo = 120;
                            }
                            else if (0 < result)
                            {
                                Unit.Message = "Command : Servo Move Repeat - NG";
                                nSeqNo = (short)RbtAction.None;
                            }
                            break;
                        case 120:
                            if (GetElapsedTicks() > Unit.RepeatWaitTime)
                            {
                                Unit.Message = "Command : Servo Move Repeat - Wait";

                                nSeqNo = 110;
                            }
                            break;
                    }

                    this.m_SeqNo = nSeqNo;
                }
            }
            m_Mutex.ReleaseMutex();

            return result;
        }
    }          
        
}
