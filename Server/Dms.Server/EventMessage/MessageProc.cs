using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Sequence;
using Dms.Device;
using Dms.Data;
//using Dms.Client; // 10.40.29 minhan
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Dms.Server
{
    public class EventmessageProc : XSequence // 11.05.17 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        #endregion

        #region Constructor
        public EventmessageProc(int scanTime, ServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            RegisterSequences();
        }
        #endregion

        #region Register Sequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqEventMessageProcess(this));
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

                //if (!m_Server.GenInfos.AutoMode) return; // 10.04.29 minhan

                foreach (XSeqFunction seq in m_SeqFunctions)
                    seq.Do();
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion
    }

    public class SeqEventMessageProcess : XSeqFunction
    {
        #region Fields
        protected static EventmessageProc m_Control;
        private EventMessageQueue m_MsgQueue;
        #endregion

        #region Constructor
        public SeqEventMessageProcess(EventmessageProc control)
        {
            m_Control = control;
            m_MsgQueue = EventMessageQueue.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_MsgQueue.RecvMsgQueueCount > 0)
            {
                if (GlobalVar.MsgRequest) // 허락한 메세지를 처리
                {
                    OnMsgReceived(m_MsgQueue.DequeueRecvMsg());
                }
                else // 잔당은 지우자.
                {
                    m_MsgQueue.DequeueRecvMsg();
                }
            }

            return -1;
        }
        #endregion

        #region Methods

        private void OnMsgReceived(string data)
        {
            string Command_data = data;

            switch (Command_data)
            {
                case "Broken":
                    {
                        if (GlobalVar.MsgBrokenComp) return;

                        if (DialogResult.Yes == MessageBox.Show("Glass Broken Scan Alarm. Yes: Skip, No: Again ", "Broken Scan Alarm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                        {
                            GlobalVar.MsgSkip = true;
                        }
                        else
                        {
                            GlobalVar.MsgSkip = false;
                        }

                        GlobalVar.MsgBrokenComp = true;
                    }
                    break;
                case "ULBroken": // 11.06.08 minhan
                    {
                        if (GlobalVar.MsgULBrokenComp) return;

                        if (DialogResult.Yes == MessageBox.Show("UL C/V Glass Broken Scan Alarm. Yes: Skip, No: Again ", "Broken Scan Alarm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification))
                        {
                            GlobalVar.MsgULSkip = true;
                        }
                        else
                        {
                            GlobalVar.MsgULSkip = false;
                        }

                        GlobalVar.MsgULBrokenComp = true;
                    }
                    break;
            }
        }
        #endregion
    }
}
