using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using Dms.Ctl;
using System.Windows.Forms;

namespace Dms.Sequence
{
    public class ThreadCimHsmsControl : XSequence
    {
        #region Fields
        private static object m_LockKey = new object();
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
        public ThreadCimHsmsControl()
        {
            m_HsmsCmdQueue = HsmsMessageQueue.Instance;
        }

        public ThreadCimHsmsControl(int scanTime)
            : base(scanTime)
        {
            m_HsmsCmdQueue = HsmsMessageQueue.Instance;

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
        public void SendRequest(string msg, params object[] objParameter)
        {
            string sParameter = "";
            string sTemp = "";

            try
            {
                for (int i = 0; i <= objParameter.Length - 1; i++)
                {
                    sTemp = sTemp + objParameter[i] + ",";
                }

                sParameter = msg + "," + sTemp;

                lock(m_LockKey)
                {
                    m_HsmsCmdQueue.EnqueueSendReq(sParameter);
                }
            }
            catch //(Exception err)
            {

            }
        }
        #endregion

    }
}
