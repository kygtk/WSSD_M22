using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtEStopMP2300 : XSeqFunction
    {
        protected int m_AxisCount;
        public SeqRbtEStopMP2300()
        {
        }
        public SeqRbtEStopMP2300(ServoUnitMp2300 unit)
        {
            Unit = unit;
            m_AxisCount = Unit.AxisCount;
        }
        private ServoUnitMp2300 Unit;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;
            uint nTimeover = 5000;

            switch (nSeqNo)
            {
                case 0:
                    {
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].ClearBit(true);
                            Unit.Axis[id].SetHomeStop(true);
                            Unit.Axis[id].EStop(true);
                        }
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!Unit.IsServoOn())
                    {
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].SetHomeStop(false);
                            Unit.Axis[id].SetPause(false);
                            Unit.Axis[id].EStop(false);
                        }
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    else if (GetElapsedTicks() > nTimeover)
                    {
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].SetHomeStop(false);
                            Unit.Axis[id].EStop(false);
                        }
                        result = (int)MP2300Error.errTimeOver;
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    if (Unit.IsMoveOk() || !Unit.IsServoOn())
                    {
                        //for (int id = 0; id < m_AxisCount; id++)
                        //{
                        //    Unit.Axis[id].ServoOn(false);
                        //    Unit.Axis[id].InitSeq();
                        //}
                        Unit.Ready = false;
                        Unit.InitSequenceParameterforEstop();
                        result = 0;
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > nTimeover)
                    {
                        result = (int)MP2300Error.errTimeOver;
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
