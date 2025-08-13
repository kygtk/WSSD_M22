using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Dms.Server
{
    public class EventMessageQueue // 11.05.17 minhan
    {
        #region Fields
        private Queue<string> m_RecvMsgQueue = new Queue<string>();
        #endregion

        #region Properties
        public int RecvMsgQueueCount
        {
            get { return m_RecvMsgQueue.Count; }
        }
        #endregion

        #region Singleton
        public static readonly EventMessageQueue Instance = new EventMessageQueue();
        #endregion

        #region Constructor
        public EventMessageQueue()
        {
            m_RecvMsgQueue.Clear();
        }
        #endregion

        #region Methods
      
        public void EnqueueRecvMsg(string msg)
        {
            m_RecvMsgQueue.Enqueue(msg);
        }
        public string DequeueRecvMsg()
        {
            return m_RecvMsgQueue.Dequeue();
        }
        #endregion
    }
}
