using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Dms.Cim.Common
{
    public class DataQueue
    {
        private Queue<InfoDlgMessage> m_InfoDlgQueue = new Queue<InfoDlgMessage>(); // MainForm에서 사용하는 Queue
        private Queue<string> m_LogListQueue = new Queue<string>(); // JobForm에서 나타나는 Log List에서 사용하는 Queue
        private Queue<string> m_HostMessageQueue = new Queue<string>(); // 호스트 로그를 표시하는 Queue
        private Queue<string> m_EqpSetupLogQueue = new Queue<string>();// EqpSetupForm에서 나타나는 Log를 사용하는 Queue
        private List<short> m_UnloadLotNo = new List<short>();

        #region Property
        public Queue<InfoDlgMessage> InfoDlgQueue
        {
            get { return m_InfoDlgQueue; }
            set { m_InfoDlgQueue = value; }
        }

        public Queue<string> LogListQueue
        {
            get { return m_LogListQueue; }
            set { m_LogListQueue = value; }
        }

        public Queue<string> HostMessageQueue
        {
            get { return m_HostMessageQueue; }
            set { m_HostMessageQueue = value; }
        }

        public Queue<string> EqpSetupLogListQueue
        {
            get { return m_EqpSetupLogQueue; }
            set { m_EqpSetupLogQueue = value; }
        }

        public List<short> UnloadLotNo
        {
            get { return m_UnloadLotNo; }
            set { m_UnloadLotNo = value; }
        }

        public int GetInfoDlgQueueCount
        {
            get{ return m_InfoDlgQueue.Count; }
        }

        public int GetLogListQueueCount
        {
            get { return m_LogListQueue.Count; }
        }

        public int GetHostMessageQueueCount
        {
            get { return m_HostMessageQueue.Count; }
        }

        public int GetEqpLogQueueCount
        {
            get { return m_EqpSetupLogQueue.Count; }
        }
        #endregion

        public void SetInfoDlg(InfoDlgMessage message)
        {
            m_InfoDlgQueue.Enqueue(message);
        }


        public InfoDlgMessage GetInfoDlg()
        {
            InfoDlgMessage message = null;

            try
            {
                message = m_InfoDlgQueue.Dequeue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", GetInfoDlg()");
            }

            return (InfoDlgMessage)message;
        }

        public InfoDlgMessage GetCurrentInfoDlg()
        {
            InfoDlgMessage message = null;

            try
            {
                message = m_InfoDlgQueue.Peek();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", GetInfoDlg()");
            }

            return (InfoDlgMessage)message;
        }

        public void SetLogListQueue(string log)
        {
            try
            {
                m_LogListQueue.Enqueue(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", Log List Queue: " + log.ToString());
            }
        }


        public string GetLogListQueue()
        {
            string message = "";

            try
            {
                message = m_LogListQueue.Dequeue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", GetLogListQueue()");
            }

            return message;
        }

        public void SetHostMessageQueue(string msg)
        {
            try
            {
                m_HostMessageQueue.Enqueue(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", Host Message Queue: " + msg.ToString());
            }
        }

        public string GetHostMessageQueue()
        {
            string message = "";

            try
            {
                message = m_HostMessageQueue.Dequeue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", GetHostMessageQueue()");
            }

            return message;
        }


        public void SetEqpLogListQueue(string log)
        {
            try
            {
                m_EqpSetupLogQueue.Enqueue(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", Eqp Log List Queue: " + log.ToString());
            }
        }


        public string GetEqpLogListQueue()
        {
            string message = "";

            try
            {
                message = m_EqpSetupLogQueue.Dequeue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ", Eqp GetLogListQueue()");
            }

            return message;
        }


        public void AddUnloadLotNo(short LotNo)
        {
            m_UnloadLotNo.Add(LotNo);
        }

        public short GetUnloadLotNo()
        {
            short LotNo = 0;

            if( m_UnloadLotNo.Count >= 1)
            {
                LotNo = m_UnloadLotNo[0];
//                m_UnloadLotNo.RemoveAt(0);
            }

            return LotNo;
        }

        public bool DeleteUnloadLotNo(short LotNo)
        {
            bool bRv = false;

            for( int i = 0; i < m_UnloadLotNo.Count; i++)
            {
                if (m_UnloadLotNo[i] == LotNo)
                {
                    m_UnloadLotNo.RemoveAt(i);
                    bRv = true;
                }
            }

            return bRv;
        }
    }

    public enum InfoType
    {
        enumInfoDisplay,
        enumHostMessage,
        enumHostRecvMessage,
        enumOfflineStart,
        enumLotCancel,
        enumTrsModeChange,
        enumLotConfirm,
        enumFormClose,
        enumOperatorCall,
        enumLotInfoConfirm,
        enumLoopBack,
        enumModeChange,
        enumRechuck,
        enumKeyInCstId,
        enumEqpCommand,
        enumLoaderScrap,
        enumClose,
    }

    public class InfoDlgMessage
    {
        private InfoType m_InfoType;
        private int m_PortNo;
        private string m_StreamFunction;
        private string m_Message;

        public InfoType Type
        {
            get { return m_InfoType; }
            set { m_InfoType = value; }
        }

        public int PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }

        public string SnFm
        {
            get { return m_StreamFunction; }
            set { m_StreamFunction = value; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public InfoDlgMessage() { }

        public InfoDlgMessage(InfoType type, int portno, string snfm, string message)
        {
            this.m_InfoType = type;
            this.m_PortNo = portno;
            this.m_StreamFunction = snfm;
            this.m_Message = message;
        }
    }
}
