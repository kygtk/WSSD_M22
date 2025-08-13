using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using Dms.Ctl;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadHsmsControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private HsmsMessageQueue m_HsmsCmdQueue;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {

        }
        #endregion

        #region Constructor
        public ThreadHsmsControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_HsmsCmdQueue = HsmsMessageQueue.Instance;
            m_GenInfos = GenInfoHandler.Instance;

            RegisterSequences();
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

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
        #endregion

        #region Mothods
        public void SendRequest(string sfName)
        {
            m_HsmsCmdQueue.EnqueueSendReq(sfName);
        }
        #endregion

    }
}
