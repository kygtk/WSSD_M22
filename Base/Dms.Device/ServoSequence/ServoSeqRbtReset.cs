using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtReset : XSeqFunction
    {
        public SeqRbtReset()
        {
        }
        public SeqRbtReset(ServoUnit unit)
        {
            Unit = unit;
        }
        private ServoUnit Unit;

        private int m_nAxisCnt = 0;
        public int AxisCnt
        {
            get { return m_nAxisCnt; }
            set { m_nAxisCnt = value; }
        }

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (!Unit.Sync) nSeqNo = 10;
                    else
                    {
                        Unit.SetSyncControl(false);

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (Unit.Axis[AxisCnt].ClearAxisErr())
                    {
                        Unit.Axis[AxisCnt].ServoOn(true);
                        AxisCnt++;

                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > 100)
                    {
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (AxisCnt < Unit.AxisCount)
                    {
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (true == Unit.Sync)
                        {
                            Unit.SetSyncControl(true);
                        }

                        AxisCnt = 0;

                        Unit.Ready = true;

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
