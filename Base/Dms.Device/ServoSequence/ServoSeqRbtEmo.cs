using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtEmo : XSeqFunction
    {
        protected int m_AxisCount;
        protected IJobCondition m_JobCond;
        protected bool m_OldSate = false;

        public SeqRbtEmo()
        { 
        }
        public SeqRbtEmo(ServoUnit unit)
        {
            Unit = unit;
            m_AxisCount = Unit.AxisCount;
            m_JobCond = Unit.ServerManager.JobCond;
            this.m_SeqFunName = this.GetType().Name;
        }
        private ServoUnit Unit;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_JobCond.Interlock.EmoCondition.IsAlarm)
                    {
                        Unit.SetLog(Unit.Name, this.m_SeqFunName, 0, 0, "EMO - Pressed");
                        m_OldSate = Unit.Ready;
                        nSeqNo = 10;                    
                    }
                    break;
                case 10:
                    if(Unit.RbtEStop())
                    {
                        Unit.SetLog(Unit.Name, this.m_SeqFunName, 0, 0, "EMO - Estop");
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (!m_JobCond.Interlock.EmoCondition.IsAlarm)
                    {
                        Unit.SetLog(Unit.Name, this.m_SeqFunName, 0, 0, "EMO - Released");
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (!m_OldSate || Unit.RbtReset())
                    {
                        Unit.SetLog(Unit.Name, this.m_SeqFunName, 0, 0, "EMO - Servo recovery ok");
                        nSeqNo = 0;                     
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
