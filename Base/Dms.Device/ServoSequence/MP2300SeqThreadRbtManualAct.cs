using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;

namespace Dms.Device
{
    public class ThreadRbtManualActMP2300 : XSequence
    {
        private ServoUnitMp2300 m_Unit;
        private IServerManager m_Server;
        private SeqRbtManualActMP2300 m_SeqRbtManualAct;
        private SeqRbtEmoMP2300 m_SeqRbtEmo;
        //private SeqSetHomeCompMP2300 m_SeqSetHomeComp;
        //private SeqSetCurPositionMP2300 m_SeqSetCurPos;

        public ThreadRbtManualActMP2300(int scanTime, ServoUnitMp2300 unit)
            : base(scanTime)
        {
            m_Server = unit.ServerManager;
            m_Unit = unit;

            m_SeqRbtManualAct = new SeqRbtManualActMP2300(m_Unit);
            m_SeqRbtEmo = new SeqRbtEmoMP2300(m_Unit);
            //m_SeqSetHomeComp = new SeqSetHomeCompMP2300(m_Unit);
            //m_SeqSetCurPos = new SeqSetCurPositionMP2300(m_Unit);

            RegisterSequence(m_SeqRbtManualAct);
            RegisterSequence(m_SeqRbtEmo);
            //RegisterSequence(m_SeqSetHomeComp);
            //RegisterSequence(m_SeqSetCurPos);
        }

        public int ManualActionCmd
        {
            get { return m_SeqRbtManualAct.ManualCmd; }
            set { m_SeqRbtManualAct.ManualCmd = value; }
        }

        public short SelectedPointId
        {
            get { return m_SeqRbtManualAct.SelectedPointId; }
            set { m_SeqRbtManualAct.SelectedPointId = value; }
        }

        //public bool HomeCompRequest
        //{
        //    get { return m_SeqSetHomeComp.HomeCompRequest; }
        //    set 
        //    {
        //        if(value == true) m_SeqSetHomeComp.InitSeq();
        //        m_SeqSetHomeComp.HomeCompRequest = value; 
        //    }
        //}

        //public bool SetCurPositionRequest
        //{
        //    get { return m_SeqSetCurPos.SetCurPosRequest; }
        //    set
        //    {
        //        if (value == true) m_SeqSetCurPos.InitSeq();
        //        m_SeqSetCurPos.SetCurPosRequest = value;
        //    }
        //}

        //public double SetPositionData
        //{
        //    get { return m_SeqSetCurPos.Position; }
        //    set { m_SeqSetCurPos.Position = value; }
        //}

        //public int SeqPosAxisId
        //{
        //    get { return m_SeqSetCurPos.AxisId; }
        //    set { m_SeqSetCurPos.AxisId = value; }
        //}

        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
    }  
}
