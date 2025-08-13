using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Dms.Common;
namespace Dms.Ctl
{
    unsafe public class MasterCrevis
    {
        // ================================================================================================
        // import Dll   
        //Device Type Define
        public const int MODBUS_TCP = 0;

        // Modbus Response Code ------------------------------------------------------------------------
        public const int NORMAL_RESPONSE = 0;
        public const int CONNECTION_FAIL = -1;
        public const int MAX_CONNECTION_EXCEEDED = -2;
        public const int ILLEGAL_DEVICE_TYPE = -3;

        public const int ILLEGAL_FUNCTION = 1;	                        //Error code : Illegal Function
        public const int ILLEGAL_DATA_ADDRESS = 2;	                    //Error code : Illegal Data Address
        public const int ILLEGAL_DATA_VALUE = 3;	                    //Error code : Illegal Data Value
        public const int SLAVE_DEVICE_FAILURE = 4;	                    //Error code : Slave Device Failure
        public const int ACKNOWLEDGE = 5;	                            //Error code : Acknowledge
        public const int SLAVE_DEVICE_BUSY = 6;	                        //Error code : Slave Device Busy
        public const int MEMORY_PARITY_ERROR = 8;	                    //Error code : Memory Parity Error
        public const int GATEWAY_PATH_UNAVAILABLE = 10;	                //Error code : Gateway Path Unavailable
        public const int GATEWAY_TARGET_DEVICE_FAILED_TO_RESPOND = 11;	//Error code : Gateway Target Device Failed to Respond

        public const int ILLEGAL_RESPONSE = 14;	                        //Error code : Illegal Response				//define by CREVIS
        public const int ILLEGAL_TRANSACTION_ID = 15;	                //Error code : Illegal Transaction ID		//define by CREVIS
        public const int TIMEOUT_RESPONSE = 16;	                        //Error code : Time out response			//define by CREVIS
        public const int NOT_CONNECT = 17;	                            //Error code : Not Connect Device			//define by CREVIS
        public const int ILLEGAL_HANDLE = 18;	                        //Error code : Illegal connection handle	//define by CREVIS
        public const int ILLEGAL_PROTOCOL_TYPE = 19;	                //Error code : Illegal protocol type		//define by CREVIS

        [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
        public struct DEVICEINFOMODBUSTCP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] IpAddress;
            public int PortNum;
            public uint InputSize;
            public uint OutputSize;
        };

        [DllImport("CrevisFnIO.dll")]
        public static extern int InitSystem();

        [DllImport("CrevisFnIO.dll")]
        public static extern int FreeSystem();

        [DllImport("CrevisFnIO.dll")]
        public static extern int OpenDevice(ref uint pDevice, ref DEVICEINFOMODBUSTCP pOpenDevInfo, int DeviceType);

        [DllImport("CrevisFnIO.dll")]
        public static extern int CloseDevice(uint hDevice);

        [DllImport("CrevisFnIO.dll")]
        public static extern int ReadInputByteData(uint hDevice, ushort addr, ref byte buffer, ushort len);

        [DllImport("CrevisFnIO.dll")]
        public static extern int ReadOutputByteData(uint hDevice, ushort addr, ref byte buffer, ushort len);

        [DllImport("CrevisFnIO.dll")]
        public static extern int WriteOutputByteData(uint hDevice, ushort addr, ref byte buffer, ushort len);
        // ================================================================================================

        private DEVICEINFOMODBUSTCP m_DevInfo;
        private uint m_hDevice = 0xFFFFFFFF;
        private bool m_IsConnected = false;

        public bool IsConnected
        {
            get { return m_IsConnected; }
            set { m_IsConnected = value; }
        }

        private DEVICEINFOMODBUSTCP DevInfo
        {
            get { return m_DevInfo; }
        }

        public int MasterCrevisInit()
        {
            m_DevInfo.IpAddress = new byte[4];
           
            m_DevInfo.PortNum = 502;
            return InitSystem();
        }

        public int MasterCrevisFree()
        {
            return FreeSystem();
        }

        public int Connect()
        {
            return OpenDevice(ref m_hDevice, ref m_DevInfo, MODBUS_TCP);
        }


        public int Connect(string masterIp, int masterPortNo)
        {
            int rv = -1;
            string[] Ips = masterIp.Split('.');

            SetIpAddress(Convert.ToByte(Ips[0]),Convert.ToByte(Ips[1]),Convert.ToByte(Ips[2]),Convert.ToByte(Ips[3]));

            m_DevInfo.PortNum = masterPortNo;

            if ( 0 == (rv =Connect())) m_IsConnected = true;

            return rv;
        }

        public int SetIpAddress(byte Ip0, byte Ip1, byte Ip2, byte Ip3)
        {
            m_DevInfo.IpAddress[0] = Ip0;
            m_DevInfo.IpAddress[1] = Ip1;
            m_DevInfo.IpAddress[2] = Ip2;
            m_DevInfo.IpAddress[3] = Ip3;
            return NORMAL_RESPONSE;
        }

        public int FnIoCloseDevice()
        {
            int ret = CloseDevice(m_hDevice);
            m_hDevice = 0xFFFFFFFF;
            return ret;
        }

        public int FnIoReadInputByteData(ushort addr, ref byte buffer, ushort len)
        {
            return ReadInputByteData(m_hDevice, addr, ref buffer, len);
        }

        public int FnIoReadOutputByteData(ushort addr, ref byte buffer, ushort len)
        {
            return ReadOutputByteData(m_hDevice, addr, ref buffer, len);
        }

        public int FnIoWriteOutputByteData(ushort addr, ref byte buffer, ushort len)
        {
            return WriteOutputByteData(m_hDevice, addr, ref buffer, len);
        }
        
        public int[] ReadAnalogInputs(byte module_nr, byte size)
        {
            int ret = -1;
            int[] rv = new int[size];
            byte data = 0;
            
            for(int i = 0; i<size;i++)
            {
                rv[i] = 0;

                ret = ReadInputByteData(m_hDevice, (ushort)(module_nr + i * 2), ref data, (ushort)(1));

                rv[i] |= data & 0x00ff;

                ret = ReadInputByteData(m_hDevice, (ushort)(module_nr + i * 2 + 1), ref data, (ushort)(1));

                rv[i] |= (data << 8) & 0xff00;
            }

            return rv;
        }

        public int[] ReadAnalogInputs(byte module_nr, byte offset, byte size)
        {
            int[] rv = new int[size];

            return rv;
        }
        public bool[] ReadDigitalInputs(byte module_nr, byte size)
        {
            bool[] rv = new bool[size];

            byte data = 0;

            int ret = ReadInputByteData(m_hDevice, module_nr, ref data,(ushort)(size/8));

            for (int i = 0; i < size; i++)
            {

                if ((data >> i & 0x01) == 1) rv[i] = true;
                else rv[i] = false;                
            }

            return rv;
        }
        public bool[] ReadDigitalInputs(byte module_nr, ushort offset, ushort size)
        {
            bool[] rv = new bool[size];




            return rv;
        }
        //public int[] ReadRegister(byte module_nr, int register)
        //{
        //    int[] rv = new int[size];

        //    return rv;
        //}
        public bool WriteAnalogOutputs(byte module_nr, int[] values)
        {
            bool rv = false;

            byte data = 0;
            int ret = -1;
            
            for(int i=0; i<values.Length;i++)
            {
                data = (byte)(values[i] & 0x00FF);

                ret = WriteOutputByteData(m_hDevice, (ushort)(module_nr + i * 2), ref data, (ushort)1);

                data = (byte)((values[i] & 0xFF00) >> 8);

                ret = WriteOutputByteData(m_hDevice, (ushort)(module_nr + i * 2 + 1), ref data, (ushort)1);
            }                                             

            return rv;
        }
        public bool WriteAnalogOutputs(byte module_nr, byte offset, int[] values)
        {
            bool rv = false;

            return rv;
        }
        public bool WriteDigitalOutputs(byte module_nr, bool[] values)
        {
            bool rv = false;


            byte data = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == true)
                {
                    data |= (byte)(1 << i);
                }
            }            

            int ret = WriteOutputByteData(m_hDevice, module_nr, ref data, (ushort)(values.Length / 8));


            return rv;
        }
        public bool WriteDigitalOutputs(byte module_nr, byte offset, bool[] values)
        {
            bool rv = false;

            return rv;
        }
        //public bool WriteRegister(byte module_nr, int register, int[] values)
        //{
        //    bool rv = false;

        //    return rv;
        //}
    }
}
