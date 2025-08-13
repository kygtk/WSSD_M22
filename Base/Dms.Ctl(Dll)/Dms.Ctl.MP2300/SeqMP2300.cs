using System;
using System.Collections.Generic;
using System.Text;
using Dms.Ctl;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;

namespace Dms.Ctl
{
    public class ThreadMP2300Monitor : XSequence
    {
        private MP2300Ctl m_Mp2300;

        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqMp2300Monitor(m_Mp2300));
            RegisterSequence(new SeqMonitorLink(m_Mp2300));
        }

        public ThreadMP2300Monitor(int scanTime, MP2300Ctl mp2300)
            : base(scanTime)
        {
            m_Mp2300 = mp2300;
            RegisterSequences();
        }

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                //if (m_Server.State != ActiveState.Run) return;
                //if (!m_Server.ControllerIsRun) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                //m_Server.WriteExceptionLog(msg);

                //if (m_Server.AppConfig.Simul.Device)
                //{
                MessageBox.Show(msg);
                //}
            }
        }
        #endregion
    }

    public class SeqMp2300Monitor : XSeqFunction
    {
        private MP2300Ctl m_Mp2300;

        public SeqMp2300Monitor(MP2300Ctl mp2300)
        {
            m_Mp2300 = mp2300;
        }

        #region Sequence
        public override int Do()
        {
            int nRv = -1;
            int err = -1;
            int nSeqNo = this.m_SeqNo;

            if (m_Mp2300.IsConnected)
            {
                nSeqNo = 0;
                return nRv;
            }

            switch (nSeqNo)
            {
                case 0:
                    if ((err = m_Mp2300.InitSocket()) > -1)
                    {
                        if (err == -1)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 500)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return nRv;
        }
        #endregion
    }

    public class SeqMonitorLink : XSeqFunction
    {
        private MP2300Ctl m_Mp2300;
        private uint m_nCnt;

        public SeqMonitorLink(MP2300Ctl mp2300)
        {
            m_Mp2300 = mp2300;
        }

        #region Sequence
        public override int Do()
        {
            if (m_Mp2300.IsConnected == false) return -1;

            int nSeqNo = this.m_SeqNo;
            uint monitorTime = 500;
            uint nTimeover = 1000;
            uint nRetry = 10;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Mp2300.IsConnected)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        m_Mp2300.IsTryLink = true;
                        nSeqNo = 10;
                        m_Mp2300.MP2300Log.TextOut("SeqMonitorLink : start");
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > monitorTime)
                    {
                        if (m_Mp2300.GetHeartBit())
                        {
                            m_Mp2300.SetHeartBit(false);
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_Mp2300.SetHeartBit(true);
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 20:
                    if (!m_Mp2300.GetHeartBit())
                    {
                        if (!m_Mp2300.IsLinked)
                        {
                            m_Mp2300.IsLinked = true;
                            m_Mp2300.IsTryLink = false;
                        }
                        m_StartTicks = XFunc.GetTickCount();

                        m_Mp2300.IsTryLink = false;
                        m_nCnt = 0;
                        nSeqNo = 10;
                    }
                    else if (GetElapsedTicks() > nTimeover)
                    {
                        if (m_nCnt > nRetry)
                        {
                            m_nCnt = 0;
                            m_Mp2300.IsLinked = false;
                        }
                        else m_nCnt++;

                        m_Mp2300.IsTryLink = true;
                        string sMsg = string.Format("SeqMonitorLink <OFF> : close socket (Cnt:{0}, Pos:{1})", m_nCnt, m_Mp2300.nThreadPos);
                        m_Mp2300.MP2300Log.TextOut(sMsg);

                        m_Mp2300.CloseSocket();
                        nSeqNo = 0;
                    }
                    break;
                case 30:
                    if (m_Mp2300.GetHeartBit())
                    {
                        if (!m_Mp2300.IsLinked)
                        {
                            m_Mp2300.IsTryLink = false;
                            m_Mp2300.IsLinked = true;
                        }
                        m_StartTicks = XFunc.GetTickCount();

                        m_Mp2300.IsTryLink = false;
                        m_nCnt = 0;
                        nSeqNo = 10;
                    }
                    else if (GetElapsedTicks() > nTimeover)
                    {
                        if (m_nCnt > nRetry)
                        {
                            m_nCnt = 0;
                            m_Mp2300.IsLinked = false;
                        }
                        else m_nCnt++;
                        m_Mp2300.IsTryLink = true;
                        string sMsg = string.Format("SeqMonitorLink <ON> : close socket (Cnt:{0}, Pos:{1})", m_nCnt, m_Mp2300.nThreadPos);
                        m_Mp2300.MP2300Log.TextOut(sMsg);

                        m_Mp2300.CloseSocket();
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class ThreadMP2300 : XSequence
    {
        private MP2300Ctl m_Mp2300;

        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqRead(m_Mp2300));
            RegisterSequence(new SeqWrite(m_Mp2300));
        }

        public ThreadMP2300(int scanTime, MP2300Ctl mp2300)
            : base(scanTime)
        {
            m_Mp2300 = mp2300;
            RegisterSequences();
        }

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                //if (m_Server.State != ActiveState.Run) return;
                //if (!m_Server.ControllerIsRun) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                //m_Server.WriteExceptionLog(msg);

                //if (m_Server.AppConfig.Simul.Device)
                //{
                MessageBox.Show(msg);
                //}
            }
        }
        #endregion
    }

    public class SeqRead : XSeqFunction
    {
        private MP2300Ctl m_Mp2300;

        public SeqRead(MP2300Ctl mp2300)
        {
            m_Mp2300 = mp2300;
        }

        #region Sequence
        public override int Do()
        {
            if (!m_Mp2300.IsConnected) return -1;

            m_Mp2300.Read((uint)10000, (uint)MP2300Ctl.IF_NUM, m_Mp2300.InBuffer);
            Thread.Sleep(1);

            return -1;
        }
        #endregion
    }

    public class SeqWrite : XSeqFunction
    {
        private MP2300Ctl m_Mp2300;

        public SeqWrite(MP2300Ctl mp2300)
        {
            m_Mp2300 = mp2300;
        }

        #region Sequence
        public override int Do()
        {
            if (!m_Mp2300.IsConnected) return -1;

            for (int i = 0; i < MP2300Ctl.CMD_BIT_NUM; i++)
            {
                int val = 0;
                for (int j = 0; j < m_Mp2300.MaxAxisNo; j++)
                {
                    val |= ((m_Mp2300.CommandBit[i, j] ? 1 : 0) << j);
                }
                if (i < 14) m_Mp2300.OutBuffer[i] = (ushort)val;
                else m_Mp2300.OutBuffer[i - 14 + 248] = (ushort)val;
            }

            m_Mp2300.Write((uint)12000, (uint)MP2300Ctl.IF_NUM, m_Mp2300.OutBuffer);
            Thread.Sleep(1);

            return -1;
        }
        #endregion
    }


}
