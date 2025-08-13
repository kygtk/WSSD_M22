using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dms.Common;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.28
// Author       : Kim Youngsik
// Description  : Modbus TCP Master(Remote Machine)
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class ModbusTcpServer
    {
        #region Fields
        private IPAddress m_RemoteIP;
        private IPEndPoint m_IPEndPoint;
        private Socket m_Socket;
        private string m_IpAddress;
        private string m_UnitName;
        private byte m_UnitID;
        private bool m_Connected;
        private ushort m_TransactionID;
        private List<byte> m_Buffer = new List<byte>();
        private XLog m_Log;
        //private uint m_Timeout = 1000;
        #endregion 

        #region Properties
        public bool Connected
        {
            get { return m_Connected; }
            set { m_Connected = value; }
        }
        public List<byte> ReceivedBuffer
        {
            get { return m_Buffer; }
            set { m_Buffer = value; }
        }
        #endregion

        #region Constructor
        public ModbusTcpServer(byte unitID, string unitName, string ipAddress, XLog log)
        {
            m_IpAddress = ipAddress;
            m_RemoteIP = IPAddress.Parse(m_IpAddress);
            m_IPEndPoint = new IPEndPoint(m_RemoteIP, 502);

            m_UnitName = unitName;
            m_UnitID = unitID;
            m_Log = log;

            m_Connected = false;
            m_TransactionID = 0;
        }

        ~ModbusTcpServer()
        {
            Dispose();
        }
        #endregion

        #region Methods
        public bool Connect()
        {
            try
            {
                m_Socket = new Socket(m_RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                m_Socket.Connect(m_IPEndPoint);
                m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, (int)1000);
                m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, (int)1000);
                m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Debug, 1);

                m_Connected = true;
                return true;
            }
            catch
            {
                m_Socket = null;
                m_Connected = false;
                return false;
            }
        }

        public void Dispose()
        {
            if (m_Socket != null)
            {
                m_Socket.Close();
                m_Socket = null;
            }

            string logText = "";
            logText = string.Format("##########DISCONNECTED##########");
            SetLog(logText);

            m_Connected = false;
        }

        public void SetLog(string text)
        {
            string logText = "";

            logText = string.Format("[UNIT : {0}-{1}]{2}{3}", m_UnitID, m_UnitName, '\t', text);
            m_Log.TextOut(logText);
        }

        private ushort GetTransactionID()

        {
            ushort retValue = m_TransactionID;
            m_TransactionID++;

            return retValue;
        }
        #endregion

        #region Methods - for Message Build
        ///////////////////////////////////////////////////////////////////////////
        // Method : CreateReadMessageHeader(ushort trID, ushort refAddress, ushort refCount, byte functionCode)
        // Description : Create message header of Read Messages
        //
        // 1. ReadCoils
        // 2. ReadInputDiscretes
        // 3. ReadMultipleRegisters
        // 4. ReadInputRegisters
        ///////////////////////////////////////////////////////////////////////////
        private byte[] CreateReadMessageHeader(ushort trID, ushort refAddress, ushort refCount, byte functionCode)
        {
            byte[] retValue = new byte[12];

            byte[] id = BitConverter.GetBytes(trID);
            byte[] address = BitConverter.GetBytes(refAddress);
            byte[] count = BitConverter.GetBytes(refCount);

            retValue[0] = id[1];
            retValue[1] = id[0];
            retValue[2] = 0;
            retValue[3] = 0;
            retValue[4] = 0;
            retValue[5] = 6;
            retValue[6] = m_UnitID;
            retValue[7] = functionCode;
            retValue[8] = address[1];
            retValue[9] = address[0];
            retValue[10] = count[1];
            retValue[11] = count[0];

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
        private byte[] CreateWriteMessageHeader(ushort trID, ushort refAddress, ushort refCount, byte byteCount, byte functionCode)
        {
            byte[] retValue = new byte[byteCount + 11];

            byte[] id = BitConverter.GetBytes(trID);
            byte[] address = BitConverter.GetBytes(refAddress);
            byte[] length = BitConverter.GetBytes((short)(byteCount + 5));
            byte[] count = BitConverter.GetBytes(refCount);

            retValue[0] = id[1];
            retValue[1] = id[0];
            retValue[2] = 0;
            retValue[3] = 0;
            retValue[4] = length[1];
            retValue[5] = length[0];
            retValue[6] = m_UnitID;
            retValue[7] = functionCode;
            retValue[8] = address[1];
            retValue[9] = address[0];

            if (functionCode >= (byte)FunctionCodes.ForceMultipleCoils)
            {
                retValue[10] = count[1];
                retValue[11] = count[0];
                retValue[12] = (byte)(byteCount - 2);
            }

            return retValue;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_ReadInputDiscretes(ushort refAddress, ushort refCount, ref byte[] refValue)
        // Description : Send request message for read multiple bits data
        //
        // [Message structure]
        // byte 0 ~ 6 : Message Header region
        // byte 7 ~ 11 : Message Data region
        //
        // [Byte Definition]
        // byte 0 : Upper byte of Transaction ID
        // byte 1 : Lower byte of Transaction ID
        // byte 2 : Upper byte of Protocol ID(fixed 0x00)
        // byte 3 : Lower byte of Protocol ID(fixed 0x00)
        // byte 4 : Upper byte of Length(0x00)
        // byte 5 : Lower byte of Length(0x06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x02)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public byte[] Request_ReadInputDiscretes(ushort refAddress, ushort refCount)
        {
            ushort transactionID = GetTransactionID();
            byte[] message = CreateReadMessageHeader(transactionID, refAddress, refCount, (byte)FunctionCodes.ReadInputDiscretes);

            try
            {
                byte[] buffer = new byte[256];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)    //case of timeout
                {
                    string logText = string.Format("ReadInputDiscretes({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)   //case of receiving exception
                {
                    string logText = string.Format("ReadInputDiscretes({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[buffer[8]];
                    Array.Copy(buffer, 9, retValue, 0, buffer[8]);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadInputDiscretes({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_ReadCoils(ushort refAddress, ushort refCount)
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
        public byte[] Request_ReadCoils(ushort refAddress, ushort refCount)
        {
            ushort transactionID = GetTransactionID();
            byte[] message = CreateReadMessageHeader(transactionID, refAddress, refCount, (byte)FunctionCodes.ReadCoils);

            try
            {
                byte[] buffer = new byte[512];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("ReadCoil({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("ReadCoil({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[buffer[8]];
                    Array.Copy(buffer, 9, retValue, 0, buffer[8]);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadCoil({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_WriteSingleCoil(ushort refAddress, bool value, ref byte[] result)
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
        public byte[] Request_WriteSingleCoil(ushort refAddress, bool value)
        {
            ushort transactionID = GetTransactionID();
            byte[] message = CreateWriteMessageHeader(transactionID, refAddress, 1, 1, (byte)FunctionCodes.WriteCoils);

            if (value)
                message[10] = 0xFF;
            else
                message[10] = 0x00;

            message[11] = 0x00;

            try
            {
                byte[] buffer = new byte[256];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("WriteSingleCoil({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("WriteSingleCoil({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[2];
                    Array.Copy(buffer, 10, retValue, 0, 2);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteCoil({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_WriteMultipleCoils(ushort refAddress, ushort refCount, bool[] value, ref byte[] result)
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
        public byte[] Request_WriteMultipleCoils(ushort refAddress, ushort refCount, bool[] value)
        {
            ushort transactionID = GetTransactionID();
            byte byteCount = (byte)((refCount + 7) / 8);

            byte[] message = CreateWriteMessageHeader(transactionID, refAddress, refCount, (byte)(byteCount + 2), (byte)FunctionCodes.ForceMultipleCoils);
            byte[] byteValue = new byte[byteCount];

            for (int i = 0; i < (int)refCount; i++)
            {
                byte val = 0;
                byte val2 = 0;

                if (value[i])
                {
                    val = 0x1;
                }
                else
                {
                    val = 0x0;
                }

                val2 = (byte)(val << (i % 8));

                byteValue[i / 8] += val2;
            }

            Array.Copy(byteValue, 0, message, 13, byteCount);

            try
            {
                byte[] buffer = new byte[256];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("WriteMultipleCoils({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("WriteMultipleCoils({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[4];
                    Array.Copy(buffer, 8, retValue, 0, 4);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteMultipleCoils({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_ReadInputRegisters(ushort refAddress, ushort refCount, ref byte[] refValue)
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
        // byte 7 : Function Code(0x04)
        // byte 8 : Upper byte of reference start address
        // byte 9 : Lower byte of reference start address
        // byte 10 : Upper byte of word count
        // byte 11 : Lower byte of word count
        ///////////////////////////////////////////////////////////////////////////
        public byte[] Request_ReadInputRegisters(ushort refAddress, ushort refCount)
        {
            ushort transactionID = GetTransactionID();
            byte[] message = CreateReadMessageHeader(transactionID, refAddress, refCount, (byte)FunctionCodes.ReadInputRegisters);

            try
            {
                byte[] buffer = new byte[512];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("ReadInputRegisters({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("ReadInputRegisters({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[buffer[8]];
                    Array.Copy(buffer, 8, retValue, 0, buffer[8]);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadInputRegisters({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_ReadMultipleRegisters(ushort refAddress, ushort refCount, ref byte[] refValue)
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
        public byte[] Request_ReadMultipleRegisters(ushort refAddress, ushort refCount)
        {
            ushort transactionID = GetTransactionID();
            byte[] message = CreateReadMessageHeader(transactionID, refAddress, refCount, (byte)FunctionCodes.ReadMultipleRegisters);

            try
            {
                byte[] buffer = new byte[512];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("ReadMultipleRegisters({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("ReadMultipleRegisters({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[buffer[8]];
                    Array.Copy(buffer, 9, retValue, 0, buffer[8]);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("ReadMultipleRegisters({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_WriteSingleRegister(ushort refAddress, short value, ref byte[] result)
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
        // byte 5 : Lower byte of Length(0X06)
        // byte 6 : Unit ID
        // byte 7 : Function Code(0x06)
        // byte 8 : Upper byte of reference address
        // byte 9 : Lower byte of reference address
        // byte 10 : Upper byte of value
        // byte 11 : Lower byte of value
        ///////////////////////////////////////////////////////////////////////////
        public byte[] Request_WriteSingleRegister(ushort refAddress, short value)
        {
            ushort transactionID = GetTransactionID();
            byte[] message = CreateWriteMessageHeader(transactionID, refAddress, 1, 1, (byte)FunctionCodes.WriteSingleRegister);
            byte[] val = BitConverter.GetBytes(value);

            message[10] = val[0];
            message[11] = val[1];

            try
            {
                byte[] buffer = new byte[512];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("WriteSingleRegister({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("WriteSingleRegister({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[4];
                    Array.Copy(buffer, 8, retValue, 0, 4);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteSingleRegister({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : Request_WriteMultipleRegisters(ushort refAddress, ushort refCount, short[] value, ref byte[] result)
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
        public byte[] Request_WriteMultipleRegisters(ushort refAddress, ushort refCount, short[] value)
        {
            ushort transactionID = GetTransactionID();
            byte byteCount = (byte)(refCount * 2);
            byte[] message = CreateWriteMessageHeader(transactionID, refAddress, refCount, (byte)(byteCount + 2), (byte)FunctionCodes.WriteMultipleRegisters);

            byte[] data = new byte[byteCount];

            for (int i = 0; i < refCount; i++)
            {
                byte[] val = BitConverter.GetBytes(value[i]);

                data[i * 2 + 0] = val[1];
                data[i * 2 + 1] = val[0];
            }

            Array.Copy(data, 0, message, 13, byteCount);

            try
            {
                byte[] buffer = new byte[512];
                int rv = 0;

                m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                rv = m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                if (rv == 0)
                {
                    string logText = string.Format("WriteMultipleRegisters({0:X4}) Receive Occur Timeout.", transactionID);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else if (rv == 9)
                {
                    string logText = string.Format("WriteMultipleRegisters({0:X4}) Receive Exception(Code : {1:X4}).", transactionID, buffer[8]);
                    SetLog(logText);
                    Dispose();
                    return null;
                }
                else
                {
                    byte[] retValue = new byte[4];
                    Array.Copy(buffer, 8, retValue, 0, 4);

                    return retValue;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string logText = string.Format("WriteMultipleRegister({0:X4}) Exception Error : {1}", transactionID, error);
                SetLog(logText);
                Dispose();
            }

            return null;
        }
        #endregion
    }
}
