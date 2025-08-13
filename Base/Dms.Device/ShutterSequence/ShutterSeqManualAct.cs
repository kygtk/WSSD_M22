using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;

namespace Dms.Device
{
    public class SeqShutterManualAct : XSeqFunction
    {
        public SeqShutterManualAct()
        {
        }

        public SeqShutterManualAct(ShutterUnit unit)
        {
            Unit = unit;
            this.m_SeqFunName = this.GetType().Name;
        }
        private ShutterUnit Unit;
        private Mutex m_Mutex = new Mutex();

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
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case (short)ShutterAct.Noop:
                    break;
                case (short)ShutterAct.Open:
                    if (0 == (result = Unit.Open()))
                    {
                        nSeqNo = (short)ShutterAct.Noop;
                    }
                    else if (0 < result)
                    {
                        nSeqNo = (short)ShutterAct.Noop;
                    }
                    break;
                case (short)ShutterAct.Close:
                    if (0 == (result = Unit.Close()))
                    {
                        nSeqNo = (short)ShutterAct.Noop;
                    }
                    else if (0 < result)
                    {
                        nSeqNo = (short)ShutterAct.Noop;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
