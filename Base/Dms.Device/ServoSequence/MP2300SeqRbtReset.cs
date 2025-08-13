using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtResetMP2300 : XSeqFunction
    {
        public SeqRbtResetMP2300()
        {
        }
        public SeqRbtResetMP2300(ServoUnitMp2300 unit)
        {
            Unit = unit;
        }
        private ServoUnitMp2300 Unit;

        private int m_nAxisCnt = 0;
        public int AxisCnt
        {
            get { return m_nAxisCnt; }
            set { m_nAxisCnt = value; }
        }

        public override int Do()
        {
            int nRv = -1;
            int result = -1;
            int nSeqNo = this.m_SeqNo;
            uint nTimeover = 5000;

            switch (nSeqNo)
            {
                case 0:
                    {
                        AxisCnt = 0;
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if ((nRv = Unit.Axis[AxisCnt].ClearAxisErr()) == 0)
                    {
                        Unit.InitSequenceParameterforEstop();
                        Unit.Axis[AxisCnt].ServoOn(true);
                        AxisCnt++;
                        nSeqNo = 20;
                    }
                    else if (nRv > 0)
                    {
                        for (AxisCnt = 0; AxisCnt < Unit.AxisCount; AxisCnt++)
                        {
                            Unit.Axis[AxisCnt].ServoOn(false);
                        }
                        AxisCnt = 0;
                        Unit.Ready = false;
                        result = nRv;
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    if (AxisCnt < Unit.AxisCount)
                    {
                        nSeqNo = 10;
                    }
                    else
                    {
                        //if (true == Unit.Sync)
                        //{
                        //    Unit.SetSyncControl(true);
                        //}
                        AxisCnt = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    {
                        bool bServoOn = true;
                        for (AxisCnt = 0; AxisCnt < Unit.AxisCount; AxisCnt++)
                        {
                            bServoOn &= Unit.Axis[AxisCnt].GetServoOnState();
                        }
                        AxisCnt = 0;

                        if (bServoOn)
                        {
                            Unit.Ready = true;
                            result = 0;
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > nTimeover)
                        {
                            for (AxisCnt = 0; AxisCnt < Unit.AxisCount; AxisCnt++)
                            {
                                Unit.Axis[AxisCnt].ServoOn(false);
                            }
                            AxisCnt = 0;
                            Unit.Ready = false;
                            result = (int)MP2300Error.errTimeOver;
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
