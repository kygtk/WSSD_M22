using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Dms.Common;

namespace Dms.Ctl
{
    public class ModbusTcpListener : XSequence
    {
        #region Fields
        private static object m_LockKey = new object();
        private TcpListener m_Listener;
        private List<ModbusTcpClient> m_Clients = new List<ModbusTcpClient>();
        private int m_SeqConnectNo = 0;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ModbusTcpListener(int sleepTime)
            : base(sleepTime)
        {
            m_ScanTime = sleepTime;

            m_Listener = new TcpListener(502);
            m_Listener.Start();
        }
        #endregion

        #region Override
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                SeqConnect();
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Sequence
        private void SeqConnect()
        {
            int seqNo = m_SeqConnectNo;

            switch (seqNo)
            {
                case 0:
                    if( m_Clients.Count > 0)
                    {
                        Socket socket = m_Listener.AcceptSocket();
                        int client = FindSlave(socket);

                        if (client >= 0)
                        {
                            m_Clients[client].SetConnection(socket);
                        }
                    }
                    break;
            }

            m_SeqConnectNo = seqNo;
        }
        #endregion

        #region Methods
        public void AddClient(ModbusTcpClient client)
        {
            lock(m_LockKey)
            {
                m_Clients.Add(client);
            }
        }

        private int FindSlave(Socket socket)
        {
            string address = socket.RemoteEndPoint.ToString();
            string[] connectedIP = address.Split(':');

            int count = m_Clients.Count;

            for (int i = 0; i < count; i++)
            {
                address = m_Clients[i].RemoteIP.ToString();

                if (connectedIP[0] == address)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion
    }
}
