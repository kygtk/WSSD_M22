using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtEStop : XSeqFunction
    {
        protected int m_AxisCount;
        public SeqRbtEStop()
        { 
        }
        public SeqRbtEStop(ServoUnit unit)
        {
            Unit = unit;
            m_AxisCount = Unit.AxisCount;
        }
        private ServoUnit Unit;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        Unit.Ready = false;

                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].EStop();
                        }

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (Unit.IsMoveOk())
                    {
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].ServoOn(false);
                        }

                        if (true == Unit.Sync)
                        {
                            Unit.SetSyncControl(false);
                        }

                        Unit.InitSequenceParameter();

                        result = 0;
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
