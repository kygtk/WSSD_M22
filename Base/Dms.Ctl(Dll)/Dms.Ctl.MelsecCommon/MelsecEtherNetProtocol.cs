using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Dms.Common;

namespace Dms.Ctl
{
    enum CommandType
    {
        Read,
        Write,
    }

    enum AccessType
    {
        BIT = 0x0001,
        WORD = 0x0000,
    }

    class MelsecEtherNetProtocol
    {
        #region Fields
        private static object m_LockKey = new object();
        private IPAddress m_RemoteIP;
        private IPEndPoint m_IPEndPoint;
        private EndPoint m_RemotePoint;
        private IPEndPoint m_IPLocalPoint;
        private EndPoint m_LocalPoint;
        private Socket m_Socket;
        private string m_IpAddress;
        private ushort m_PortNo;
        private ProtocolType m_ProtocolType = ProtocolType.Tcp;

        private bool m_IsOpened = false;
        private byte[] m_Buffer = new byte[2048];
        #endregion

        #region Properties
        public bool IsOpened
        {
            get { return m_IsOpened; }
        }
        #endregion

        #region Constructor
        public MelsecEtherNetProtocol(string ipAddress, ushort portNo, ProtocolType protocol)
        {
            m_IpAddress = ipAddress;
            m_PortNo = portNo;
            m_ProtocolType = protocol;
        }

        ~MelsecEtherNetProtocol()
        {
            Close();
        }
        #endregion

        #region Methods
        public bool Open()
        {
            //일단 연결 요청을 한다.
            Connect();

            //연결여부를 return
			//연결여부와 상관없이 일단 ok 해야 연결이 안되어있더라도 HMI를 실행할 수 있다..
			//단, 동작에 대한 interlock은 연결상태를 봐야 한다.
            //return m_IsOpened;
			return true;
        }

        public bool Close()
        {
            try
            {
                m_Socket.Close();
                m_Socket = null;
                m_IPEndPoint = null;
                m_IsOpened = false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Connect()
        {
            lock(m_LockKey)
            {
                try
                {
                    if (m_ProtocolType == ProtocolType.Tcp)
                    {
                        m_RemoteIP = IPAddress.Parse(m_IpAddress);
                        m_IPEndPoint = new IPEndPoint(m_RemoteIP, (int)m_PortNo);

                        m_Socket = new Socket(m_RemoteIP.AddressFamily, SocketType.Stream, m_ProtocolType);
                        m_Socket.Connect(m_IPEndPoint);
                        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, (int)1000);
                        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, (int)1000);
                        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Debug, 1);
                    }
                    else if (m_ProtocolType == ProtocolType.Udp)
                    {
                        m_RemoteIP = IPAddress.Parse(m_IpAddress);
                        m_IPEndPoint = new IPEndPoint(m_RemoteIP, (int)m_PortNo);
                        m_RemotePoint = (EndPoint)m_IPEndPoint;

                        m_IPLocalPoint = new IPEndPoint(IPAddress.Any, (int)m_PortNo);
                        m_LocalPoint = (EndPoint)m_IPLocalPoint;

                        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, m_ProtocolType);
                        //m_Socket.Connect(m_IPEndPoint);
                        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, (int)1000);
                        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, (int)1000);
                        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Debug, 1);
                    }

                    m_IsOpened = true;
                }
                catch//(Exception error)
                {
					//System.Windows.Forms.MessageBox.Show(error.ToString());
                    m_Socket = null;
                    m_IsOpened = false;
                }
            }
        }

        private byte GetDeviceCode(devTYPE type)
        {
            if (type == devTYPE.devX) return 0x9C;
            else if (type == devTYPE.devY) return 0x9D;
            else if (type == devTYPE.devM) return 0x90;
            else if (type == devTYPE.devL) return 0x92;
            else if (type == devTYPE.devB) return 0xA0;
            else if (type == devTYPE.devF) return 0x93;
            else if (type == devTYPE.devD) return 0xA8;
            else if (type == devTYPE.devW) return 0xB4;
            else if (type == devTYPE.devR) return 0xAF;
			else if (type == devTYPE.devZR) return 0xB0;
            else return 0x00;
        }

        //*************************************************************************************************
        // Method : int BuildMessageHeader_BlockAccess(CommandType cmdType, devTYPE devType, short index, short byteSize, ref byte[] message)
        // Description : Create EtherNet TCP Message Header
        //
        // message[0]~message[1] = 0x50, 0x00 : Command (Response의 경우 0xD0, 0x00)
        // message[2] = 0x00 : 네트워크 번호 (Q시리즈 E71 장착국(자국)의 경우 0x00)
        // message[3] = 0xFF : PLC 번호 (Q시리즈 E71 장착국(자국)의 경우 0xFF : 0xFF는 네트워크 번호가 0x00일때만 유효)
        // message[4]~message[5] = 0xFF, 0x03 : 요구상대 모듈 I/O 번호 (멀티 CPU 시스템의 PLC CPU가 아닌 경우 고정)
        // message[6] = 0x00 : 요구상대 모듈 국번호 (멀티 CPU 시스템의 PLC CPU가 아닌 경우 고정)
        // message[7] = variable : Message 길이의 Lower byte (header[9] 부터 Message 끝까지의 byte 수)
        // message[8] = variable : Message 길이의 Upper byte (header[9] 부터 Message 끝까지의 byte 수)
        // message[9] = variable : CPU 감시 타이머의 Lower byte
        //                        (Q시리즈E71이 PLC CPU로 읽기/쓰기요구를 출력 후 응답이 올때 까지의 대기 시간, 0이면 무한대기)
        // message[10] = variable : CPU 감시 타이머의 Upper byte
        //                        (Q시리즈E71이 PLC CPU로 읽기/쓰기요구를 출력 후 응답이 올때 까지의 대기 시간, 0이면 무한대기)
        // message[11]~message[12] = variable : Command (0x01, 0x04 : Read Command), (0x01, 0x14 : Write Command)
        // message[13]~message[14] = variable : Sub-Command (0x00, 0x00 : Word 단위), (0x01, 0x00 : Bit 단위)
        // message[15]~message[17] = variable : Access할 Device의 시작 주소
        // message[18] = variable : Access할 Device의 Type ==> GetDeviceCode() Method 참조
        // message[19]~message[20] = variable : Access할 Device 수의 Word Size
        //*************************************************************************************************
        private int BuildMessageHeader_BlockAccess(CommandType cmdType, devTYPE devType, int index, short byteSize, ref byte[] message)
        {
            int wordSize = (int)(byteSize / 2) + (int)((byteSize % 2) > 0 ? 1 : 0);

            message[0] = 0x50;
            message[1] = 0x00;
            message[2] = 0x00;
            message[3] = 0xFF;
            message[4] = 0xFF;
            message[5] = 0x03;
            message[6] = 0x00;

            if (cmdType == CommandType.Read)
            {
                message[7] = 0x0C;
                message[8] = 0x00;
                message[11] = 0x01;
                message[12] = 0x04;
            }
            else
            {
				int length = byteSize + 12;
                message[7] = (byte)(length % 0x100);
                message[8] = (byte)(length / 0x100);
                message[11] = 0x01;
                message[12] = 0x14;
            }

            message[9] = 0x10;
            message[10] = 0x00;
            message[13] = (byte)((ushort)AccessType.WORD % 0x100);
            message[14] = (byte)((ushort)AccessType.WORD / 0x100); ;
            
			//message[15] = (byte)(index % 0x100);
			//message[16] = (byte)(index / 0x100);
			//message[17] = 0x00;

			//message[15] = (byte)(index % 0x100);
			//int a = (int)(index / 0x100); 
			//message[16] = (byte)(a % 0x100);
			//message[17] = (byte)(a / 0x100);

			message[15] = (byte)(index & 0xFF);
			message[16] = (byte)((index >> 8) & 0xFF);
			message[17] = (byte)((index >> 16) & 0xFF);

			message[18] = GetDeviceCode(devType);
            message[19] = (byte)(wordSize % 0x100);
            message[20] = (byte)(wordSize / 0x100);

            return 0;
        }

        //*************************************************************************************************
        // Method : int BuildMessageHeader_RandomWrite(AccessType accessType, byte deviceCount, ref byte[] message)
        // Description : Create EtherNet TCP Message Header
        //
        // message[0]~message[1] = 0x50, 0x00 : Command (Response의 경우 0xD0, 0x00)
        // message[2] = 0x00 : 네트워크 번호 (Q시리즈 E71 장착국(자국)의 경우 0x00)
        // message[3] = 0xFF : PLC 번호 (Q시리즈 E71 장착국(자국)의 경우 0xFF : 0xFF는 네트워크 번호가 0x00일때만 유효)
        // message[4]~message[5] = 0xFF, 0x03 : 요구상대 모듈 I/O 번호 (멀티 CPU 시스템의 PLC CPU가 아닌 경우 고정)
        // message[6] = 0x00 : 요구상대 모듈 국번호 (멀티 CPU 시스템의 PLC CPU가 아닌 경우 고정)
        // message[7] = variable : Message 길이의 Lower byte (header[9] 부터 Message 끝까지의 byte 수)
        // message[8] = variable : Message 길이의 Upper byte (header[9] 부터 Message 끝까지의 byte 수)
        // message[9] = variable : CPU 감시 타이머의 Lower byte
        //                        (Q시리즈E71이 PLC CPU로 읽기/쓰기요구를 출력 후 응답이 올때 까지의 대기 시간, 0이면 무한대기)
        // message[10] = variable : CPU 감시 타이머의 Upper byte
        //                        (Q시리즈E71이 PLC CPU로 읽기/쓰기요구를 출력 후 응답이 올때 까지의 대기 시간, 0이면 무한대기)
        // message[11]~message[12] = variable : Command (0x03, 0x04 : Random Read), (0x02, 0x14 : Random Write)
        // message[13]~message[14] = variable : Sub-Command (0x00, 0x00 : Word 단위), (0x01, 0x00 : Bit 단위)
        //
        // 1. case of Random Bit Write
        //   message[15] = variable : Access Bit Count
        //
        // 2. case of Random Word Write
        //   message[15] = variable : Access Word Count
        //   message[16] = variable : Access Double-Word Count
        //*************************************************************************************************
        private int BuildMessageHeader_RandomWrite(AccessType accessType, byte deviceCount, ref byte[] message)
        {
            ushort msgLength = 0;

            if (accessType == AccessType.BIT)
            {
                msgLength = (ushort)(deviceCount * 5 + 7);
            }
            else
            {
                msgLength = (ushort)(deviceCount * 6 + 8);
            }

            message[0] = 0x50;
            message[1] = 0x00;
            message[2] = 0x00;
            message[3] = 0xFF;
            message[4] = 0xFF;
            message[5] = 0x03;
            message[6] = 0x00;
            message[7] = (byte)(msgLength % 0x100);
            message[8] = (byte)(msgLength / 0x100);
            message[9] = 0x10;
            message[10] = 0x00;
            message[11] = 0x02;
            message[12] = 0x14;
            message[13] = (byte)((ushort)accessType % 0x100);
            message[14] = (byte)((ushort)accessType / 0x100);

            if (accessType == AccessType.BIT)
            {
                message[15] = deviceCount;
            }
            else
            {
                message[15] = deviceCount;
                message[16] = 0x00;  //일단 double-word는 write하지 않는 것으로 사용...
            }

            return 0;
        }

        //*************************************************************************************************
        // Method : short BlockReadDevice(devTYPE type, short index, ref short byteSize, ref short[] readData)
        // Description : Mitsubishi PLC로 부터 원하는 Device를 byteSize 만큼 읽어온다.
        //               최대 960 word 만큼 일괄로 처리 가능.
        //*************************************************************************************************
        public short BlockReadDevice(devTYPE type, int index, ref short byteSize, ref short[] readData)
        {
            try
            {
				short size = (short)(byteSize + byteSize % 2);
                byte[] message = new byte[21];
				int rv = BuildMessageHeader_BlockAccess(CommandType.Read, type, index, size, ref message);

                if (m_ProtocolType == ProtocolType.Tcp)
                {
                    m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                    rv = m_Socket.Receive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None);
                }
                else if (m_ProtocolType == ProtocolType.Udp)
                {
                    m_Socket.SendTo(message, message.Length, SocketFlags.None, m_RemotePoint);
                    rv = m_Socket.ReceiveFrom(m_Buffer, m_Buffer.Length, SocketFlags.None, ref m_LocalPoint);
                }

                if (rv != 0)
                {
                    short error = (short)(m_Buffer[9] + m_Buffer[10] * 0x100);

					if ((error == 0) && ((rv - 11) == size))
                    {
						int count = (size / 2);
                        for (int i = 0; i < count; i++)
                        {
                            readData[i] = (short)(m_Buffer[i * 2 + 11] + (m_Buffer[i * 2 + 12] * 0x100));
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }

                return 0;
            }
            catch
            {
                Close();
                return 1;
            }
        }

        //*************************************************************************************************
        // Method : short BlockWriteDevice(devTYPE type, short index, ref short byteSize, ref short[] writeData)
        // Description : Mitsubishi PLC의 Device에 byteSize 만큼 값을 쓴다.
        //               최대 960 word 만큼 일괄로 처리 가능.
        //*************************************************************************************************
        public short BlockWriteDevice(devTYPE type, int index, ref short byteSize, ref short[] writeData)
        {
            try
            {
				short size = (short)(byteSize + (byteSize % 2));
				byte[] message = new byte[21 + size];
				int rv = BuildMessageHeader_BlockAccess(CommandType.Write, type, index, size, ref message);

				int count = (size / 2);
                for (int i = 0; i < count; i++)
                {
                    message[i * 2 + 21] = (byte)((ushort)writeData[i] % 0x100);
                    message[i * 2 + 22] = (byte)((ushort)writeData[i] / 0x100);
                }

                if (m_ProtocolType == ProtocolType.Tcp)
                {
                    m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                    rv = m_Socket.Receive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None);
                }
                else if (m_ProtocolType == ProtocolType.Udp)
                {
                    m_Socket.SendTo(message, message.Length, SocketFlags.None, m_RemotePoint);
                    rv = m_Socket.ReceiveFrom(m_Buffer, m_Buffer.Length, SocketFlags.None, ref m_LocalPoint);
                }

                if (rv != 0)
                {
                    short error = (short)(m_Buffer[9] + m_Buffer[10] * 0xFF);
                    return error;
                }

                return 0;
            }
            catch
            {
                Close();
                return 1;
            }
        }

        //*************************************************************************************************
        // Method : short RandomWriteBitDevice(devTYPE[] type, short[] indexes, short[] byteSize, short[] writeData)
        // Description : Mitsubishi PLC의 Bit Device들에 값을 쓴다.
        //               Max 188 bits.
        //*************************************************************************************************
        public short RandomWriteBitDevice(devTYPE[] type, int[] indexes, short[] size, short[] writeData)
        {
            try
            {
                if ((type.Length != indexes.Length) || (type.Length != writeData.Length) ||
                    (indexes.Length != writeData.Length))
                {
                    return 100; //array의 길이가 다를 때...
                }

                int msgLength = 16 + (int)(type.Length * 5);
                byte[] message = new byte[msgLength];

                int rv = BuildMessageHeader_RandomWrite(AccessType.BIT, (byte)type.Length, ref message);

                int count = type.Length;

                for (int i = 0; i < count; i++)
                {
                    int startIndex = (i * 5) + 16;

                    message[startIndex + 0] = (byte)((ushort)(indexes[i] % 0x100));
                    message[startIndex + 1] = (byte)((ushort)(indexes[i] / 0x100));
                    message[startIndex + 2] = 0x00;
                    message[startIndex + 3] = GetDeviceCode(type[i]);
                    message[startIndex + 4] = (byte)((ushort)(writeData[i] % 0x100));
                }

                if (m_ProtocolType == ProtocolType.Tcp)
                {
                    m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                    rv = m_Socket.Receive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None);
                }
                else if (m_ProtocolType == ProtocolType.Udp)
                {
                    m_Socket.SendTo(message, message.Length, SocketFlags.None, m_RemotePoint);
                    rv = m_Socket.ReceiveFrom(m_Buffer, m_Buffer.Length, SocketFlags.None, ref m_LocalPoint);
                }

                if (rv != 0)
                {
                    ushort error = (ushort)(m_Buffer[9] + m_Buffer[10] * 0xFF);
                    return (short)error;
                }

                return 0;
            }
            catch//(Exception err)
            {
                Close();
                return 1;
            }
        }

        //*************************************************************************************************
        // Method : short RandomWriteWordDevice(devTYPE[] type, short[] indexes, short[] byteSize, short[] writeData)
        // Description : Mitsubishi PLC의 Word Device들에 값을 쓴다.
        //               Max 120 words.
        //*************************************************************************************************
        public short RandomWriteWordDevice(devTYPE[] type, int[] indexes, short[] size, short[] writeData)
        {
            try
            {
                if ((type.Length != indexes.Length) || (type.Length != writeData.Length) ||
                    (indexes.Length != writeData.Length))
                {
                    return 100; //array의 길이가 다를 때...
                }

                int msgLength = 17 + (int)(type.Length * 6);
                byte[] message = new byte[msgLength];

                int rv = BuildMessageHeader_RandomWrite(AccessType.WORD, (byte)type.Length, ref message);

                int count = type.Length;

                for (int i = 0; i < count; i++)
                {
                    int startIndex = 17 + (i * 6);

                    message[startIndex + 0] = (byte)((ushort)(indexes[i] % 0x100));
                    message[startIndex + 1] = (byte)((ushort)(indexes[i] / 0x100));
                    message[startIndex + 2] = 0x00;
                    message[startIndex + 3] = GetDeviceCode(type[i]);
                    message[startIndex + 4] = (byte)((ushort)(writeData[i] % 0x100));
                    message[startIndex + 5] = (byte)((ushort)(writeData[i] / 0x100));
                }

                if (m_ProtocolType == ProtocolType.Tcp)
                {
                    m_Socket.Send(message, 0, message.Length, SocketFlags.None);
                    rv = m_Socket.Receive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None);
                }
                else if (m_ProtocolType == ProtocolType.Udp)
                {
                    m_Socket.SendTo(message, message.Length, SocketFlags.None, m_RemotePoint);
                    rv = m_Socket.ReceiveFrom(m_Buffer, m_Buffer.Length, SocketFlags.None, ref m_LocalPoint);
                }

                if (rv != 0)
                {
                    short error = (short)(m_Buffer[9] + m_Buffer[10] * 0xFF);
                    return error;
                }

                return 0;
            }
            catch
            {
                Close();
                return 1;
            }
        }
        #endregion
    }
}
