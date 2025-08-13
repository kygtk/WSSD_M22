using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SEComEnabler.SEComPlugIn;
using SEComEnabler.SEComStructure;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05
// Author       : Kim Youngsik
// Description  : HSMS Message Queues
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class HsmsMessageQueue
    {
        #region Fields
        private Queue<SXTransaction> m_SendMsgQueue = new Queue<SXTransaction>();
        private Queue<SEComData> m_RecvMsgQueue = new Queue<SEComData>();
        private Queue<string> m_SendReqMsgQueue = new Queue<string>();
        private Queue<string> m_TimeoutQueue = new Queue<string>();
        private Queue<string> m_ErrorQueue = new Queue<string>();
        #endregion

        #region Properties
        public int SendMsgQueueCount
        {
            get { return m_SendMsgQueue.Count; }
        }
        public int RecvMsgQueueCount
        {
            get { return m_RecvMsgQueue.Count; }
        }
        public int TimeoutQueueCount
        {
            get { return m_TimeoutQueue.Count; }
        }
        public int ErrorQueueCount
        {
            get { return m_ErrorQueue.Count; }
        }
        public int SendReqQueueCount
        {
            get { return m_SendReqMsgQueue.Count; }
        }
        #endregion

        #region Singleton
        public static readonly HsmsMessageQueue Instance = new HsmsMessageQueue();
        #endregion

        #region Constructor
        public HsmsMessageQueue()
        {
            m_SendMsgQueue.Clear();
            m_RecvMsgQueue.Clear();
            m_SendReqMsgQueue.Clear();
            m_TimeoutQueue.Clear();
            m_ErrorQueue.Clear();
        }
        #endregion

        #region Methods
        public void EnqueueSendMsg(SXTransaction sxTrx)
        {
            m_SendMsgQueue.Enqueue(sxTrx);
        }
        
        public SXTransaction DequeueSendMsg()
        {
            return m_SendMsgQueue.Dequeue();
        }

        public void EnqueueRecvMsg(SEComData secomData)
        {
            m_RecvMsgQueue.Enqueue(secomData);
        }

        public SEComData DequeueRecvMsg()
        {
            return m_RecvMsgQueue.Dequeue();
        }

        public void EnqueueSendReq(string msg)
        {
            m_SendReqMsgQueue.Enqueue(msg);
        }

        public string DequeueSendReq()
        {
            return m_SendReqMsgQueue.Dequeue();
        }

        public void EnqueueTimeout(string timeout)
        {
            m_TimeoutQueue.Enqueue(timeout);
        }

        public string DequeueTimeout()
        {
            return m_TimeoutQueue.Dequeue();
        }

        public void EnqueueError(string error)
        {
            m_ErrorQueue.Enqueue(error);
        }

        public string DequeueError()
        {
            return m_ErrorQueue.Dequeue();
        }
        #endregion
    }
}
