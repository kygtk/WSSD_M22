using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMovePoint : XSeqFunction
    {
        public SeqRbtMovePoint()
        {       
        }
        public SeqRbtMovePoint(ServoUnit unit)
        {
            Unit = unit;
        }
        private ServoUnit Unit;
        private short RetryCnt = 0;

        public int Do(short posId)
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (posId < 0 || posId > (Unit.TeachPoints - 1))
                    {
                        result = 100;
                        break;
                    }
                    else
                    {
                        MoveType moveType = Unit.MoveType;

                        switch (moveType)
                        {
                            case MoveType.S1Move:
                                {
                                    int rv = Unit.RbtMoveS1(Unit.GetTeachPointPos(posId), Unit.GetVel());
                                    if (0 == rv)
                                    {
                                        nSeqNo = 10;
                                    }
                                    else if (0 < rv)
                                    {
                                        result = rv;
                                        nSeqNo = 0;
                                    }
                                }
                                break;
                            case MoveType.S2Move:
                                throw new Exception("The method or operation is not implemented.");
                            case MoveType.T1Move:
                                throw new Exception("The method or operation is not implemented.");
                            case MoveType.T2Move:
                                throw new Exception("The method or operation is not implemented.");
                            case MoveType.CMove:
                                throw new Exception("The method or operation is not implemented.");
                            case MoveType.AT1Move:
                                {
                                    int rv = Unit.RbtMoveAT1(Unit.GetTeachPointPos(posId), Unit.GetVel());
                                    if (0 == rv)
                                    {
                                        nSeqNo = 10;
                                    }
                                    else if (0 < rv)
                                    {
                                        result = rv;
                                        nSeqNo = 0;
                                    }
                                }
                                break;
                        }

                    }
                    break;
                case 10:
                    if (Unit.IsRbtStatusOk())
                    {
                        nSeqNo = 20;
                    }
                    else
                    {
                        RetryCnt = 0;
                        result = 200;
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    {
                        bool inpos = true;
                        int axisCount = Unit.AxisCount;
                        for (int i = 0; i < axisCount; i++)
                        {
                            double teachPos = Unit.TeachPoint[posId].Pos[i];
                            //double curPos = Unit.CurPos.Pos[i];
                            //double curPos = Unit.CurPos.Pos[i];
                            double curPos = 0.0;
                            Unit.GetCurPosition((short)i, ref curPos);
                            inpos &= (Math.Abs(teachPos - curPos) < (double)InPosition.Margin);
                        }

                        if (true == inpos)
                        {
                            RetryCnt = 0;
                            result = 0;
                            nSeqNo = 0;
                        }
                        else
                        {
                            if (RetryCnt < (int)MaxRetry.Cnt)
                            {
                                RetryCnt++;
                                nSeqNo = 0;
                            }
                            else
                            {
                                RetryCnt = 0;
                                result = 300;
                                nSeqNo = 0;
                            }
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}
