using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using Dms.Common;

namespace Dms.Ctl
{
    #region Data Format
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RegFormat
    {
        public byte byLow;
        public byte byHigh;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EIFHeader
    {
        public byte byCmdType;
        public byte bySerialNum;
        public byte byRecvChannel;
        public byte bySendChannel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] byDummy;
        public byte byDataLenLow;
        public byte byDataLenHigh;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] byDummy2;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Ex090BCommand
    {
        public byte byLenLow;
        public byte byLenHigh;
        public byte byMfc;
        public byte bySfc;
        public byte byCpu;
        public byte bySpare;
        public byte byRefAddLow;
        public byte byRefAddHigh;
        public byte byRegNumLow;
        public byte byRegNumHigh;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Ex09Response
    {
        public byte byLenLow;
        public byte byLenHigh;
        public byte byMfc;
        public byte bySfc;
        public byte byCpu;
        public byte bySpare;
        public byte byNumLow;
        public byte byNumHigh;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Ex0BResponse
    {
        public byte byLenLow;
        public byte byLenHigh;
        public byte byMfc;
        public byte bySfc;
        public byte byCpu;
        public byte bySpare;
        public byte byRefAddLow;
        public byte byRefAddHigh;
        public byte byRefNumLow;
        public byte byRefNumHigh;
    }
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct SF09Command
    //{
    //    public EIFHeader EifHeader;
    //    public Ex09Command Command;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
    //    public RegFormat[] RegData;

    //    //public SF09Command(int anything)
    //    //{
    //    //    this.RegData = new RegFormat[MP2300.REGNUM];
    //    //    this.EifHeader = new EIFHeader(1);
    //    //}
    //}
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct SF09Response
    //{
    //    public EIFHeader EifHeader;
    //    public Ex09Response Response;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
    //    public RegFormat[] RegData;

    //    //public SF09Response(int anything)
    //    //{
    //    //    this.RegData = new RegFormat[MP2300.REGNUM];
    //    //    this.EifHeader = new EIFHeader(1);
    //    //}
    //}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SF09WriteCommand
    {
        public EIFHeader EifHeader;
        public Ex090BCommand Command;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
        public RegFormat[] RegData;

        public SF09WriteCommand()
        {
            EifHeader = new EIFHeader();
            EifHeader.byDummy = new byte[2];
            EifHeader.byDummy2 = new byte[4];
            Command = new Ex090BCommand();
            RegData = new RegFormat[MP2300.REGNUM];
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SF09ReadCommand
    {
        public EIFHeader EifHeader;
        public Ex090BCommand Command;

        public SF09ReadCommand()
        {
            EifHeader = new EIFHeader();
            EifHeader.byDummy = new byte[2];
            EifHeader.byDummy2 = new byte[4];
            Command = new Ex090BCommand();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SF09ReadResponse
    {
        public EIFHeader EifHeader;
        public Ex09Response Response;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
        public RegFormat[] RegData;

        public SF09ReadResponse()
        {
            EifHeader = new EIFHeader();
            EifHeader.byDummy = new byte[2];
            EifHeader.byDummy2 = new byte[4];
            Response = new Ex09Response();
            RegData = new RegFormat[MP2300.REGNUM];
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SF09WriteResponse
    {
        public EIFHeader EifHeader;
        public Ex0BResponse Response;

        public SF09WriteResponse()
        {
            EifHeader = new EIFHeader();
            EifHeader.byDummy = new byte[2];
            EifHeader.byDummy2 = new byte[4];
            Response = new Ex0BResponse();
        }
    }
    #endregion

    public class MP2300
    {
        #region Fields
        private static int EIF_HEADER_LEN = 12;
        private static int SFC = 0x09;
        //private static int READ_DATA_NUM = 10;
        //private static int M_REG_ADDR = 0;
        private static int SEND_CMD_SIZE = 22;
        private static int RECV_CMD_SIZE = 20;

        public static int REGNUM = 2000;    //Max Read/Write
        public static int IF_NUM = 500;
        public static int ORG_NUM = 320;
        public static int POINT_NUM = 1280;
        public static int CMD_BIT_NUM = 16;     //20090911 eun 각 축에 대한 비트를 가지고 있는 Command의 수(12000~12013, 12248, 12249)

        public int nThreadPos = -1;

        private const string HOST_IP_ADDR = "192.168.1.5";
        private const string LOCAL_IP_ADDR = "192.168.1.1";

        protected ushort[] m_InBuf;
        private ushort[] m_OutBuf;
        private bool[,] m_CmdBit;            //20090911 eun 각 축에 대한 비트 Command를 처리하기 위한 버퍼 추가(쓰레드에 대한 안정성확보)
        protected ushort[] m_OrgBuf;
        private ushort[] m_OrgMem;
        protected ushort[][] m_PointBuf;
        private ushort[,] m_PointMem;
        private IPAddress m_HostIp = IPAddress.Parse(HOST_IP_ADDR);
        private IPAddress m_LocalIp = IPAddress.Parse(LOCAL_IP_ADDR);
        private int m_HostPort = 10060;
        private int m_LocalPort = 10050;
        protected bool m_Connected;
        //private int m_Socket;
        private int m_SerialNo;
        private SF09ReadCommand m_ReadCmd;      //only command
        private SF09ReadResponse m_ReadResp;    //command + data
        private SF09WriteCommand m_WriteCmd;    //command + data
        private SF09WriteResponse m_WriteResp;  //only command
        private UdpClient m_UdpClient = null;
        private Mutex m_Mutex = new Mutex();
        private Socket m_Socket = null;
        public XLog MP2300Log = new XLog("MP2300Log", XLog.LogStampType.UseStamp);
        protected bool m_Simulate = true;
        protected bool m_IsLinked = false;
        private bool m_IsTryLink = true;
        protected short m_MaxAxisNo = 16;
        #endregion

        #region Properties
        public IPAddress HostIp
        {
            get { return m_HostIp; }
            set { m_HostIp = value; }
        }
        public IPAddress LocalIp
        {
            get { return m_LocalIp; }
            set { m_LocalIp = value; }
        }
        public int HostPort
        {
            get { return m_HostPort; }
            set { m_HostPort = value; }
        }
        public int LocalPort
        {
            get { return m_LocalPort; }
            set { m_LocalPort = value; }
        }
        public bool IsConnected
        {
            get { return m_Connected; }
            set { m_Connected = value; }
        }
        public ushort[] InBuffer
        {
            get { return m_InBuf; }
            set { m_InBuf = value; }
        }
        public ushort[] OutBuffer
        {
            get { return m_OutBuf; }
            set { m_OutBuf = value; }
        }
        public bool Simulation
        {
            get { return m_Simulate; }
            set { m_Simulate = value; }
        }
        public bool IsLinked
        {
            get { return m_IsLinked; }
            set { m_IsLinked = value; }
        }
        public bool IsTryLink
        {
            get { return m_IsTryLink; }
            set { m_IsTryLink = value; }
        }
        public ushort[] HomeBuf
        {
            get { return m_OrgBuf; }
            set { m_OrgBuf = value; }
        }
        public ushort[][] PointBuf
        {
            get { return m_PointBuf; }
            set { m_PointBuf = value; }
        }
        public bool[,] CommandBit
        {
            get { return m_CmdBit; }
            set { m_CmdBit = value; }
        }
        public short MaxAxisNo
        {
            get { return m_MaxAxisNo; }
            set { m_MaxAxisNo = value; }
        }
        #endregion

        #region Constructor
        public MP2300()
        {
        }
        #endregion

        #region Methods
        public void InitParameter()
        {
            m_IsLinked = false;
            m_Connected = false;
            m_SerialNo = 0;

            m_InBuf = new ushort[IF_NUM];
            m_OutBuf = new ushort[IF_NUM];
            m_CmdBit = new bool[CMD_BIT_NUM, m_MaxAxisNo];
            m_OrgBuf = new ushort[ORG_NUM];
            m_OrgMem = new ushort[ORG_NUM];
            m_PointBuf = new ushort[2][];
            m_PointBuf[0] = new ushort[POINT_NUM];
            m_PointBuf[1] = new ushort[POINT_NUM];
            m_PointBuf.Initialize();
            m_PointMem = new ushort[2, POINT_NUM];
            m_ReadCmd = new SF09ReadCommand();
            m_ReadResp = new SF09ReadResponse();
            m_WriteCmd = new SF09WriteCommand();
            m_WriteResp = new SF09WriteResponse();

            //Test for StructureToByte / ByteToStructure
            //SetCmdReadData(10000, 2);
            //byte[] test = XFunc.StructureToByte(m_SendCmd);
            //SF09Command data = (SF09Command)XFunc.ByteToStructure(test, typeof(SF09Command));
        }

        public void SetIpAddress(IPAddress hostIP, int hostPort, IPAddress localIP, int localPort)
        {
            m_HostIp = hostIP;
            m_LocalIp = localIP;
            m_HostPort = hostPort;
            m_LocalPort = localPort;
        }

        public string GetHostIpAddress()
        {
            if (m_HostIp == null) return HOST_IP_ADDR;
            return m_HostIp.ToString();
        }

        public string GetLocalIpAddress()
        {
            if (m_LocalIp == null) return LOCAL_IP_ADDR;
            return m_LocalIp.ToString();
        }

        public int InitSocket()
        {
            int nRv = -1;

            try
            {
                if (m_Connected) nRv = 1;
                MP2300Log.TextOut("InitSocket : Start");
                IPEndPoint hostIpep = new IPEndPoint(m_HostIp, m_HostPort);
                IPEndPoint localIpep = new IPEndPoint(m_LocalIp, m_LocalPort);
                //m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //m_Socket.Bind(ipep);
                m_UdpClient = new UdpClient(hostIpep);
                m_UdpClient.Connect(localIpep);
                m_Connected = true;
                MP2300Log.TextOut("InitSocket : Connection OK");

                nRv = 1;
            }
            catch (SocketException er)
            {
                int code = er.ErrorCode;
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
                err.Message.ToString();
                CloseSocket();
                MP2300Log.TextOut("InitSocket : close socket");
                nRv = 0;
            }

            return nRv;
        }

        public int CheckReadRespData(byte[] rBuff, byte[] sBuff, int nLen)
        {
            int rc = 0;
            int nSize = MP2300Ctl.RECV_CMD_SIZE + MP2300Ctl.IF_NUM * 2;
            if (nLen < nSize)
            {
                rc = -1;    // Checks the total length
                return rc;
            }
            if (rBuff[0] != 0x19)
            {
                rc = -2;    // Checks the packet type
                return rc;
            }
            if (sBuff[1] != rBuff[1])
            {
                rc = -3;    // Checks the serial number
                return rc;
            }
            if ((rBuff[6] != 0xFC) && (rBuff[7] != 0x03))
            {
                rc = -4;    // Checks the length of the total data in the message
                return rc;
            }
            if ((rBuff[12] != 0xEE) || (rBuff[13] != 0x03))
            {
                rc = -5;    // Checks the MEMOBUS data length
                return rc;
            }
            if (rBuff[14] != 0x20)
            {
                rc = -6;    // Checks the MFC
                return rc;
            }
            if (rBuff[15] != 0x09)
            {
                rc = -7;    // Checks the SFC
                return rc;
            }
            if ((rBuff[18] != 0xF4) || (rBuff[19] != 0x01))
            {
                rc = -8;    // Checks the number of registers
                return rc;
            }

            return rc;
        }

        public int CheckReadRespData(int nLen)
        {
            byte[] rBuff = XFunc.StructureToByte(m_ReadResp);
            byte[] sBuff = XFunc.StructureToByte(m_ReadCmd);

            return CheckReadRespData(rBuff, sBuff, nLen);
        }

        public bool Read(uint startAddr, uint size, ushort[] buf)
        {
            if (m_Simulate || !m_Connected) return false;

            int rc;

            try
            {
                m_Mutex.WaitOne();

                ushort uSize = SetCmdReadData(startAddr, size);

                byte[] sendData = XFunc.StructureToByte(m_ReadCmd);
                if (sendData == null)
                {
                    if (m_Connected)
                    {
                        //CloseSocket();
                        MP2300Log.TextOut("Read : Error sendData");
                    }
                    m_Mutex.ReleaseMutex();
                    return false;
                }
                IPEndPoint ipep = new IPEndPoint(m_LocalIp, m_LocalPort);

                nThreadPos = 0;
                int bufSize = SEND_CMD_SIZE;
                rc = m_UdpClient.Send(sendData, bufSize);

                nThreadPos = 1;
                if (rc <= 0)
                {
                    if (m_Connected)
                    {
                        //CloseSocket();
                        MP2300Log.TextOut("Read : Error socket (send : " + rc.ToString() + ")");
                    }
                    m_Mutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(15);
                byte[] recvData = m_UdpClient.Receive(ref ipep);
                nThreadPos = 2;

                bufSize = RECV_CMD_SIZE + 2 * REGNUM;
                byte[] recvBuf = new byte[bufSize];
                recvBuf.Initialize();
                if (recvData.Length <= recvBuf.Length)
                {
                    recvData.CopyTo(recvBuf, 0);
                }

                m_ReadResp = (SF09ReadResponse)XFunc.ByteToStructure(recvBuf, typeof(SF09ReadResponse));
                if (m_ReadResp == null)
                {
                    if (m_Connected)
                    {
                        //CloseSocket();
                        MP2300Log.TextOut("Read : Error socket (m_ReadResp)");
                    }
                    m_Mutex.ReleaseMutex();
                    return false;
                }

                if (startAddr == 10000)
                {
                    //rc = CheckReadRespData((int)recvData.Length);
                    rc = CheckReadRespData(recvData, sendData, (int)recvData.Length);

                    if (rc < 0)
                    {
                        if (m_Connected)
                        {
                            //CloseSocket();
                            MP2300Log.TextOut("Read : Error socket - Check Code(" + rc.ToString() + ")");
                        }
                        m_Mutex.ReleaseMutex();
                        return false;
                    }
                }

                for (int i = 0; i < (int)size; i++)
                {
                    buf[i] = (ushort)((m_ReadResp.RegData[i].byHigh << 8) | m_ReadResp.RegData[i].byLow);
                }

                m_Mutex.ReleaseMutex();
                return true;
            }
            catch (Exception err)
            {
                err.Message.ToString();
                if (m_Connected)
                {
                    CloseSocket();
                    MP2300Log.TextOut("Read : close socket (Exception)");
                }
                m_Mutex.ReleaseMutex();
                return false;
            }
        }

        public int CheckWriteRespData(byte[] rBuff, byte[] sBuff, int nLen)
        {
            int rc = 0;
            int nSize = MP2300Ctl.SEND_CMD_SIZE;
            if (nLen < nSize)
            {
                rc = -1;	// Checks the total length
                return rc;
            }
            if (rBuff[0] != 0x19)
            {
                rc = -2;	// Checks the packet type
                return rc;
            }
            if (sBuff[1] != rBuff[1])
            {
                rc = -3;	// Checks the serial number
                return rc;
            }
            if ((rBuff[6] != 0x16) && (rBuff[7] != 0x00))
            {
                rc = -4;	// Checks the length of the total data in the message
                return rc;
            }
            if ((rBuff[12] != 0x08) || (rBuff[13] != 0x00))
            {
                rc = -5;	// Checks the MEMOBUS data length
                return rc;
            }
            if (rBuff[14] != 0x20)
            {
                rc = -6;	// Checks the MFC
                return rc;
            }
            if (rBuff[15] != 0x0B)
            {
                rc = -7;	// Checks the SFC
                return rc;
            }
            if ((rBuff[18] != 0xE0) || (rBuff[19] != 0x2E))
            {
                rc = -8;	// Checks the number of registers
                return rc;
            }

            return rc;
        }

        public int CheckWriteRespData(int nLen)
        {
            byte[] rBuff = XFunc.StructureToByte(m_WriteResp);
            byte[] sBuff = XFunc.StructureToByte(m_WriteCmd);

            return CheckWriteRespData(rBuff, sBuff, nLen);
        }

        public bool Write(uint startAddr, uint size, ushort[] buf)
        {
            if (m_Simulate || !m_Connected) return false;

            int rc;

            try
            {
                m_Mutex.WaitOne();

                ushort uSize = SetCmdWriteData(startAddr, size);

                for (int i = 0; i < (int)size; i++)
                {
                    m_WriteCmd.RegData[i].byHigh = (byte)(buf[i] >> 8 & 0x00FF);
                    m_WriteCmd.RegData[i].byLow = (byte)(buf[i] & 0x00FF);
                }

                byte[] sendData = XFunc.StructureToByte(m_WriteCmd);
                if (sendData == null)
                {
                    if (m_Connected)
                    {
                        //CloseSocket();
                        MP2300Log.TextOut("Write : Error sendData");
                    }
                    m_Mutex.ReleaseMutex();
                    return false;
                }
                IPEndPoint ipep = new IPEndPoint(m_LocalIp, m_LocalPort);

                nThreadPos = 3;
                int bufSize = MP2300Ctl.SEND_CMD_SIZE + (int)size * 2;
                rc = m_UdpClient.Send(sendData, bufSize);

                nThreadPos = 4;
                if (rc <= 0)
                {
                    if (m_Connected)
                    {
                        //CloseSocket();
                        MP2300Log.TextOut("Write : Error socket (send : " + rc.ToString() + ")");
                    }
                    m_Mutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(10);
                byte[] recvData = m_UdpClient.Receive(ref ipep);
                nThreadPos = 5;

                m_WriteResp = (SF09WriteResponse)XFunc.ByteToStructure(recvData, typeof(SF09WriteResponse));
                if (m_WriteResp == null)
                {
                    if (m_Connected)
                    {
                        //CloseSocket();
                        MP2300Log.TextOut("Write : Error socket (m_WriteResp)");
                    }
                    m_Mutex.ReleaseMutex();
                    return false;
                }

                if (startAddr == 12000)
                {
                    //rc = CheckWriteRespData((int)recvData.Length);
                    rc = CheckWriteRespData(recvData, sendData, (int)recvData.Length);

                    if (rc < 0)
                    {
                        if (m_Connected)
                        {
                            //CloseSocket();
                            MP2300Log.TextOut("Write : Error socket - Check Code(" + rc.ToString() + ")");
                        }
                        m_Mutex.ReleaseMutex();
                        return false;
                    }
                }

                m_Mutex.ReleaseMutex();
            }
            catch (Exception err)
            {
                err.Message.ToString();
                if (m_Connected)
                {
                    CloseSocket();
                    MP2300Log.TextOut("Write : close socket (Exception)");
                }
                m_Mutex.ReleaseMutex();
                return false;
            }

            return true;
        }

        //private void Monitor()
        //{
        //    if (m_Initialized)
        //    {
        //        if (m_Connected)
        //        {
        //            m_SeqNo = 0;
        //            return;
        //        }

        //        int err = -1;

        //        switch (m_SeqNo)
        //        {
        //            case 0:
        //                {
        //                    if((err = InitSocket()) > -1)
        //                    {
        //                        if (err == 1)
        //                        {
        //                            m_StartTicks = XFunc.GetTickCount();
        //                            m_SeqNo = 10;
        //                        }
        //                    }
        //                }
        //                break;
        //            case 10:
        //                if (XFunc.GetTickCount() - m_StartTicks > 500)
        //                {
        //                    m_SeqNo = 0;
        //                }
        //                break;
        //        }
        //    }
        //}

        // (SendTo) ///////////////////////
        // 00		코맨트 타입		 0x11
        // 01		식별번호         serial
        // 02		송신선 채널      0x00
        // 03		송신원 채널      0x00
        // 04		미사용           0x00
        // 05		미사용           0x00
        // 06		데이터 길이(L)   0x16
        // 07		데이터 길이(H)   0x00
        // 08		미사용           0x00 
        // 09		미사용           0x00
        // 0A		미사용           0x00
        // 0B		미사용           0x00 
        // 0C		Length(L)        0x08
        // 0D		Length(H)        0x00
        // 0E		MFC              0x20
        // 0F		SFC              0x09
        // 10		CPU 번호         0x00
        // 11		미사용           0x00
        // 12		레지스트 번호(H) 0x10
        // 13		레지스트 번호(L) 0x00
        // 14		레지스트 수(H)   0x1A
        // 15		레지스트 수(L)   0x10
        /////////////////////////////////////
        private ushort SetCmdReadData(uint startAddr, uint size)
        {
            ushort len = 0;
            len = (ushort)(EIF_HEADER_LEN + 10);

            m_ReadCmd.EifHeader.byCmdType = 0x11;
            m_ReadCmd.EifHeader.bySerialNum = (byte)m_SerialNo;
            m_ReadCmd.EifHeader.byRecvChannel = 0x00;
            m_ReadCmd.EifHeader.bySendChannel = 0x00;
            m_ReadCmd.EifHeader.byDummy[0] = 0x00;
            m_ReadCmd.EifHeader.byDummy[1] = 0x00;
            m_ReadCmd.EifHeader.byDataLenLow = (byte)(len & 0x00FF);
            m_ReadCmd.EifHeader.byDataLenHigh = (byte)((len >> 8) & 0x00FF);
            m_ReadCmd.EifHeader.byDummy2[0] = 0x20;
            m_ReadCmd.EifHeader.byDummy2[1] = 0x00;
            m_ReadCmd.EifHeader.byDummy2[2] = 0x00;
            m_ReadCmd.EifHeader.byDummy2[3] = 0x00;

            m_ReadCmd.Command.byLenLow = 0x0A;
            m_ReadCmd.Command.byLenHigh = 0x00;
            m_ReadCmd.Command.byMfc = 0x20;
            m_ReadCmd.Command.bySfc = (byte)SFC;
            m_ReadCmd.Command.byCpu = 0x10;
            m_ReadCmd.Command.bySpare = 0x00;
            m_ReadCmd.Command.byRefAddLow = (byte)(startAddr & 0x00FF);
            m_ReadCmd.Command.byRefAddHigh = (byte)((startAddr >> 8) & 0x00FF);
            m_ReadCmd.Command.byRegNumLow = (byte)(size & 0x00FF);
            m_ReadCmd.Command.byRegNumHigh = (byte)((size >> 8) & 0x00FF);

            m_SerialNo++;

            return len;
        }

        // (Recv From) ///////////////////////
        // 00		코맨트 타입		 0x11
        // 01		식별번호         serial
        // 02		송신선 채널      0x00
        // 03		송신원 채널      0x00
        // 04		미사용           0x00
        // 05		미사용           0x00
        // 06		데이터 길이(L)   0x16
        // 07		데이터 길이(H)   0x00
        // 08		미사용           0x00 
        // 09		미사용           0x00
        // 0A		미사용           0x00
        // 0B		미사용           0x00 
        // 0C		Length(L)        0x08
        // 0D		Length(H)        0x00
        // 0E		MFC              0x20
        // 0F		SFC              0x09
        // 10		CPU 번호         0x00
        // 11		미사용           0x00
        // 12		레지스트 수(H)   0x1A
        // 13		레지스트 수(L)   0x10
        /////////////////////////////////////
        private ushort SetCmdWriteData(uint startAddr, uint size)
        {
            ushort len = 0;
            len = (ushort)(EIF_HEADER_LEN + 10);

            m_WriteCmd.EifHeader.byCmdType = 0x11;
            m_WriteCmd.EifHeader.bySerialNum = (byte)m_SerialNo;
            m_WriteCmd.EifHeader.byRecvChannel = 0x00;
            m_WriteCmd.EifHeader.bySendChannel = 0x00;
            m_WriteCmd.EifHeader.byDummy[0] = 0x00;
            m_WriteCmd.EifHeader.byDummy[1] = 0x00;
            m_WriteCmd.EifHeader.byDataLenLow = (byte)(len & 0x00FF);
            m_WriteCmd.EifHeader.byDataLenHigh = (byte)((len >> 8) & 0x00FF);
            m_WriteCmd.EifHeader.byDummy2[0] = 0x00;
            m_WriteCmd.EifHeader.byDummy2[1] = 0x00;
            m_WriteCmd.EifHeader.byDummy2[2] = 0x00;
            m_WriteCmd.EifHeader.byDummy2[3] = 0x00;

            int dataSize = 8 + (int)size * 2;
            m_WriteCmd.Command.byLenLow = (byte)(dataSize & 0x00FF);
            m_WriteCmd.Command.byLenHigh = (byte)((dataSize >> 8) & 0x00FF);
            m_WriteCmd.Command.byMfc = 0x20;
            m_WriteCmd.Command.bySfc = 0x0B;
            m_WriteCmd.Command.byCpu = 0x10;
            m_WriteCmd.Command.bySpare = 0x00;
            m_WriteCmd.Command.byRefAddLow = (byte)(startAddr & 0x00FF);
            m_WriteCmd.Command.byRefAddHigh = (byte)((startAddr >> 8) & 0x00FF);
            m_WriteCmd.Command.byRegNumLow = (byte)(size & 0x00FF);
            m_WriteCmd.Command.byRegNumHigh = (byte)((size >> 8) & 0x00FF);

            m_SerialNo++;

            return len;
        }

        public void CloseSocket()
        {
            //m_IsLinked = false;
            m_Connected = false;
            if (m_UdpClient != null)
            {
                m_UdpClient.Close();
            }
            if (m_Socket != null)
            {
                m_Socket.Close();
            }
        }

        public bool GetRecvBit(int nAxis, int nAddress)
        {
            ushort val = m_InBuf[nAddress];
            bool bRv = (((val >> nAxis) & 0x01) != 0);
            return bRv;
        }

        public bool GetSendBit(int nAxis, int nAddress)
        {
            ushort val = m_OutBuf[nAddress];
            bool bRv = (((val >> nAxis) & 0x01) != 0);
            return bRv;
        }

        public int SetSendBit(int nAxis, int nAddress, bool bState)
        {
            m_CmdBit[nAddress, nAxis] = bState;
            return 0;
        }

        public ushort GetRecvWord(int nAxis, int nAddress)
        {
            ushort data = 0;
            int index = nAddress + (nAxis * 6);
            data = m_InBuf[index];
            return data;
        }

        public ushort GetSendWord(int nAxis, int nAddress)
        {
            ushort data = 0;
            int index = nAddress + (nAxis * 13);
            data = m_OutBuf[index];
            return data;
        }

        public int SetSendWord(int nAxis, int nAddress, int nData)
        {
            int index = nAddress + (nAxis * 13);
            m_OutBuf[index] = (ushort)nData;
            return 0;
        }

        public int GetRecvDWord(int nAxis, int nAddress)
        {
            //int data = 0;
            //int index = nAddress + (nAxis * 6);
            //data += m_InBuf[index];
            //data += (m_InBuf[index + 1] << 16);
            //return data;
            //Mr.Kang Test C++ 과 동일하게 적용
            int nIndex;
            int nData = 0;
            if (nAddress < 250)
                nIndex = nAddress + (nAxis * 6);
            else
                nIndex = nAddress + (nAxis * 2);
            nData += (m_InBuf[nIndex]);
            nData += (m_InBuf[nIndex + 1] << 16);
            return nData;
        }

        public int GetSendDWord(int nAxis, int nAddress)
        {
            int data = 0;
            int index = nAddress + (nAxis * 13);
            data += m_OutBuf[index];
            data += (m_OutBuf[index + 1] << 16);
            return data;
        }

        public int SetSendDWord(int nAxis, int nAddress, int nData)
        {
            int index;
            if (nAddress < 250)
                index = nAddress + (nAxis * 13);
            else
                index = nAddress + (nAxis * 2);
            m_OutBuf[index] = (ushort)(nData & 0xFFFF);
            m_OutBuf[index + 1] = (ushort)((nData >> 16) & 0xFFFF);
            return 0;
        }
        #endregion
    }
}
