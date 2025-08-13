using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqClearAxisErr : XSeqFunction
    {
        public SeqClearAxisErr()
        {
        }

        public SeqClearAxisErr(ServoMotor servoMotor)
        {
            Servo = servoMotor;
        }
        private ServoMotor Servo;

        private short RetryCnt = 0;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (Servo.ClearStatus() == false)
                    {
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (RetryCnt == (short)MaxRetry.Cnt)
                        {
                            nSeqNo = 20;
                        }
                        else
                        {
                            RetryCnt++;
                        }
                    }
                    break;
                case 10:
                    if (Servo.IsCmdDone())
                    {
                        Servo.ClearFrames();
                        Servo.ClearStatus();

                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        RetryCnt = 0;
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
