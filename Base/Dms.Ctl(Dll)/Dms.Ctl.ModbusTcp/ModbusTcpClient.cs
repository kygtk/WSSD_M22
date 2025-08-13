using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dms.Common;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.31
// Author       : Kim Youngsik
// Description  : Modbus TCP Slave(Remote Machine)
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class ModbusTcpClient : XSequence
    {
        public delegate void ReceiveRequestMessage(byte[] reqMsg);
        public delegate void ConnectionClosed();  //2010.04.09 Youngsik... Connection이 종료 될 때...       
        public event ReceiveRequestMessage OnReceiveReqMsg;
        public event ConnectionClosed OnDisconnect;  //2010.04.09 Youngsik... Connection이 종료 될 때...

        #region Fields
        private static object m_LockKey = new object();
        private Socket m_ClientSocket;
        private NetworkStream m_Stream;
        private IPAddress m_RemoteIP;
        //private IPAddress m_LocalIP;
        private string m_UnitName;
        private byte m_UnitID;
        private bool m_Connected;
        private bool m_Received = false;
        //private ushort m_TransactionID;
        private int m_SeqWatchDogNo = 0;
        private uint m_TickStart;
        private XLog m_Log;
        private List<byte> m_Buffer = new List<byte>();
        //Received Request Message Queue
        //private Queue<ModbusTcpMessage> m_Request = new Queue<ModbusTcpMessage>();
        #endregion

        #region Properties
        public IPAddress RemoteIP
        {
            get { return m_RemoteIP; }
            set { m_RemoteIP = value; }
        }
        public byte UnitID
        {
            get { return m_UnitID; }
            set { m_UnitID = value; }
        }
        public bool Connected
        {
            get { return m_Connected; }
            set { m_Connected = value; }
        }
        #endregion

        #region #constructor
        public ModbusTcpClient(int scanTime, byte unitID, string unitName, string ipAddress, XLog log) : base(scanTime)
        {
            m_ScanTime = scanTime;
            m_Log = log;

            m_RemoteIP = IPAddress.Parse(ipAddress);
            m_UnitName = unitName;
            m_UnitID = unitID;
            m_ClientSocket = new Socket(m_RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            m_Connected = false;
            //m_TransactionID = 0;
        }
        #endregion

        #region Override
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                //Socket의 Stream Buffer에 있는 byte stream을 Read해서 m_Buffer에 저장한다.
                ReadBuffer();

                //m_Buffer에서 byte array를 가져와서 ModbusTcpMessage type으로 변환
                GetReceivedData();

                ////일정시간 Client로 부터 Request Message가 없으면 Disconnect 처리 한다.
                SeqWatchDog();

            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Sequence
        private void SeqWatchDog()
        {
            int seqNo = m_SeqWatchDogNo;

            switch (seqNo)
            {
                case 0:
                    if (m_Connected)
                    {
                        m_TickStart = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Received)
                    {
                        m_Received = false;
                        seqNo = 0;
                    }
                    else if (m_Connected == false)
                    {
                        ClearBuffer();
                        OnDisconnect();
                        seqNo = 0;
                    }
                    else if ((XFunc.GetTickCount() - m_TickStart) > 10000)
                    {
                        m_Connected = false;
                        m_ClientSocket.Disconnect(false);
                        m_ClientSocket.Close();
                        m_ClientSocket = null;
                        m_Stream = null;
                        ClearBuffer();
                        OnDisconnect();
                        seqNo = 0;
                    }
                    break;
            }

            m_SeqWatchDogNo = seqNo;
        }

        private void ReadBuffer()
        {
            if (m_Stream == null) return;

            if (m_Stream.DataAvailable)
            {
                byte[] buffer = new byte[512];

                try
                {
                    int length = m_Stream.Read(buffer, 0, buffer.Length);
                    m_Stream.Flush();

                    if (length > 0)
                    {
                        AddBuffer(length, buffer);
                        return;
                    }
                }
                catch (Exception err)
                {
                    string logText = string.Format("ReadBuffer Exception : {0}", err.ToString());
                    SetLog(logText);
                    return;
                }

            }

            return;
        }

        private void GetReceivedData()
        {
            int dataLength = 0;
            int msgLength = 0;

            if (m_Buffer.Count < 6) return;   //Message Length를 알 수 없는 길이이면 return...

            dataLength = (int)(m_Buffer[4] * 0x100) + (int)(m_Buffer[5]);
            msgLength = dataLength + 6;

            byte[] data = new byte[msgLength];

            if (m_Buffer.Count < msgLength) return;   //Message Length 보다 적게 받았으면 return;

            for (int i = 0; i < msgLength; i++)
            {
                data[i] = m_Buffer[i];
            }

            m_Received = true;
            OnReceiveReqMsg(data);

            RemoveBuffer(msgLength);
        }
        #endregion

        #region Methods
        public void SetConnection(Socket socket)
        {
            m_ClientSocket = socket;
            m_Stream = new NetworkStream(m_ClientSocket);

            string logText = string.Format("**********CONNECTED**********");
            SetLog(logText);

            m_Connected = true;
        }

        public void SetLog(string text)
        {
            string logText = "";

            logText = string.Format("[UNIT : {0}-{1}]{2}{3}", m_UnitID, m_UnitName, '\t', text);
            m_Log.TextOut(logText);
        }
        private void ClearBuffer()
        {
            m_Buffer.Clear();
        }

        private void AddBuffer(int size, byte[] data)
        {
            lock (m_LockKey)
            {
                for (int i = 0; i < size; i++)
                {
                    m_Buffer.Add(data[i]);
                }
            }
        }

        private void RemoveBuffer(int size)
        {
            lock (m_LockKey)
            {
                for (int i = 0; i < size; i++)
                {
                    if (m_Buffer.Count > 0)
                    {
                        m_Buffer.RemoveAt(0);
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : CreateReadMessageHeader(ushort trID, ushort refAddress, ushort refCount, byte functionCode)
        // Description : Create message header of Read Messages
        //
        // 1. ReadCoils
        // 2. ReadInputDiscretes
        // 3. ReadMultipleRegisters
        // 4. ReadInputRegisters
        ///////////////////////////////////////////////////////////////////////////
        private byte[] CreateReadMessageHeader(ushort trID, byte unitID, byte byteCount, byte functionCode)
        {
            byte[] retValue = new byte[byteCount + 9];

            byte[] id = BitConverter.GetBytes(trID);

            retValue[0] = id[1];
            retValue[1] = id[0];
            retValue[2] = 0;
            retValue[3] = 0;
            retValue[4] = 0;
            retValue[5] = (byte)(byteCount + 3);
            retValue[6] = unitID;
            retValue[7] = functionCode;
            retValue[8] = byteCount;

            return retValue;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : CreateWriteMessageHeader(ushort trID, ushort refAddress, ushort refCount, byte byteCount, byte functionCode)
        // Description : Create message header of Write Messages
        //
        // 1. WriteCoil
        // 2. WriteSingleRegister
        // 3. WriteMultipleRegister
        // 4. WriteMultipleCoils
        ///////////////////////////////////////////////////////////////////////////
        private byte[] CreateWriteMessageHeader(ushort trID, byte unitID, ushort refAddress, byte functionCode)
        {
            byte[] retValue = new byte[12];

            byte[] id = BitConverter.GetBytes(trID);
            byte[] address = BitConverter.GetBytes(refAddress);

            retValue[0] = id[1];
            retValue[1] = id[0];
            retValue[2] = 0;
            retValue[3] = 0;
            retValue[4] = 0;
            retValue[5] = 6;
            retValue[6] = unitID;
            retValue[7] = functionCode;
            retValue[8] = address[1];
            retValue[9] = address[0];

            return retValue;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_Exception(ModbusTcpMessage msg)
        // Description : Send request message for read multiple words data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ 11 : Exception Code
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x03)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x8n)
        // byte 8 : Exception Code
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_Exception(ushort trID, byte unitID, byte functionCode, byte exceptionCode)
        {
            byte[] message = new byte[9];

            byte[] id = BitConverter.GetBytes(trID);

            message[0] = id[1];
            message[1] = id[0];
            message[2] = 0;
            message[3] = 0;
            message[4] = 0;
            message[5] = 3;
            message[6] = unitID;
            message[7] = (byte)(functionCode + (byte)FunctionCodes.ExceptionOffset);
            message[8] = exceptionCode;

            try
            {
                string logText = string.Format("Function Code : {0:X2}{1}Exception Code : {2:X2}", functionCode, '\t', exceptionCode);
                SetLog(logText);

                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("Exception({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;

        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_ReadMultipleRegisters(ushort trID, byte unitID, ushort refCount, short[] value)
        // Description : Send request message for read multiple words data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ 11 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x03)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_ReadMultipleRegisters(ushort trID, byte unitID, ushort refCount, short[] value)
        {
            byte byteCount = (byte)(refCount * 2);
            byte[] byteValue = new byte[byteCount];

            byte[] message = CreateReadMessageHeader(trID, unitID, byteCount, (byte)FunctionCodes.ReadMultipleRegisters);

            for (int i = 0; i < refCount; i++)
            {
                byte[] temp = BitConverter.GetBytes(value[i]);
                byteValue[i * 2 + 0] = temp[1];
                byteValue[i * 2 + 1] = temp[0];
            }

            Array.Copy(byteValue, 0, message, 9, byteCount);

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadMultipleRegisters({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_ReadInputRegisters(ushort trID, byte unitID, ushort refCount, short[] value)
        // Description : Send request message for read multiple words data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ 11 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x03)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_ReadInputRegisters(ushort trID, byte unitID, ushort refCount, short[] value)
        {
            byte byteCount = (byte)(refCount * 2);
            byte[] byteValue = new byte[byteCount];

            byte[] message = CreateReadMessageHeader(trID, unitID, byteCount, (byte)FunctionCodes.ReadInputRegisters);

            for (int i = 0; i < refCount; i++)
            {
                byte[] temp = BitConverter.GetBytes(value[i]);
                byteValue[i * 2 + 0] = temp[1];
                byteValue[i * 2 + 1] = temp[0];
            }

            Array.Copy(byteValue, 0, message, 9, byteCount);

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadInputRegisters({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_ReadCoils(ushort trID, byte unitID, ushort refCount, bool[] value)
        // Description : Send request message for read multiple bits data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ 11 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x01)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_ReadCoils(ushort trID, byte unitID, ushort refCount, bool[] value)
        {
            byte byteCount = (byte)((refCount + 7) / 8);
            byte[] byteValue = new byte[byteCount];
            byte[] message = CreateReadMessageHeader(trID, unitID, byteCount, (byte)FunctionCodes.ReadCoils);

            for (int i = 0; i < byteCount; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int bitNo = (i * 8) + j;

                    if (bitNo < refCount)
                    {
                        byte temp = 0x0;

                        if (value[bitNo]) temp = 0x01;
                        else temp = 0x0;

                        temp = (byte)(temp << j);
                        byteValue[i] += temp;
                    }
                }
            }

            Array.Copy(byteValue, 0, message, 9, byteCount);

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadCoils({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_ReadInputDiscretes(ushort trID, byte unitID, ushort refCount, bool[] value)
        // Description : Send request message for read multiple bits data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ 11 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x01)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_ReadInputDiscretes(ushort trID, byte unitID, ushort refCount, bool[] value)
        {
            byte byteCount = (byte)((refCount + 7) / 8);
            byte[] byteValue = new byte[byteCount];
            byte[] message = CreateReadMessageHeader(trID, unitID, byteCount, (byte)FunctionCodes.ReadInputDiscretes);

            for (int i = 0; i < byteCount; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int bitNo = (i * 8) + j;

                    if (bitNo < refCount)
                    {
                        byte temp = 0x0;

                        if (value[bitNo]) temp = 0x01;
                        else temp = 0x0;

                        temp = (byte)(temp << j);
                        byteValue[i] += temp;
                    }
                }
            }

            Array.Copy(byteValue, 0, message, 9, byteCount);

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadInputDiscretes({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_WriteCoil(ushort trID, byte unitID, ushort refAddress, bool value)
        // Description : Send request message for change bit status
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ 11 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x05)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_WriteCoils(ushort trID, byte unitID, ushort refAddress, bool value)
        {
            byte[] message = CreateWriteMessageHeader(trID, unitID, refAddress, (byte)FunctionCodes.WriteCoils);

            if (value)
            {
                message[10] = 0xFF;
            }
            else
            {
                message[10] = 0x00;
            }

            message[11] = 0x00;

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteCoil({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_WriteMultipleCoils(ushort trID, byte unitID, ushort refAddress, ushort refCount)
        // Description : Send request message for change bit status
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ (8 + (count/8+1)) : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x0F)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of bit count
        // byte 11 : Lower byte of bit count
        // byte 12 : Byte count
        // byte 13 ~ : byte value of bits
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_WriteMultipleCoils(ushort trID, byte unitID, ushort refAddress, ushort refCount)
        {
            byte[] message = CreateWriteMessageHeader(trID, unitID, refAddress, (byte)FunctionCodes.ForceMultipleCoils);
            byte[] count = BitConverter.GetBytes(refCount);

            message[10] = count[1];
            message[11] = count[0];

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteMultipleCoils({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_WriteSingleRegisters(ushort trID, byte unitID, ushort refAddress, short value)
        // Description : Send request message for write multiple words data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ (count*2)+12 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(7+(count*2))
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x10)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        // byte 12 : Byte count(count*2)
        // byte 13 ~ (byte count * 2) : Word values
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_WriteSingleRegister(ushort trID, byte unitID, ushort refAddress, short value)
        {
            byte[] message = CreateWriteMessageHeader(trID, unitID, refAddress, (byte)FunctionCodes.WriteSingleRegister);
            byte[] val = BitConverter.GetBytes(value);

            message[10] = val[1];
            message[11] = val[0];

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteSingleRegister({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Response_WriteMultipleRegisters(ushort trID, byte unitID, ushort refAddress, ushort refCount)
        // Description : Send request message for write multiple words data
        //
        // [Message structure]
        // byte 0 ~ 7 : Message Header region
        // byte 8 ~ (count*2)+12 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(7+(count*2))
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x10)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        // byte 12 : Byte count(count*2)
        // byte 13 ~ (byte count * 2) : Word values
        ///////////////////////////////////////////////////////////////////////////
        public bool Response_WriteMultipleRegisters(ushort trID, byte unitID, ushort refAddress, ushort refCount)
        {
            byte[] message = CreateWriteMessageHeader(trID, unitID, refAddress, (byte)FunctionCodes.WriteMultipleRegisters);
            byte[] count = BitConverter.GetBytes(refCount);

            message[10] = count[1];
            message[11] = count[0];

            try
            {
                m_ClientSocket.Send(message, 0, message.Length, SocketFlags.None);
                return true;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteMultipleRegister({0:X4}) Exception Error : {1}", trID, error);
                SetLog(logText);

                logText = string.Format("##########DISCONNECTED##########");
                SetLog(logText);
                m_Connected = false;

                ClearBuffer();
            }

            return false;
        }
        #endregion
    }
}
