using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        public static class IoCtl
        {
            #region Fields
            private static bool m_Initialized = false;

            private static Io Io;
            #endregion

            #region Properties
            public static bool Initialized
            {
                get { return m_Initialized; }
            }
            #endregion

            #region Methods
            internal static void Initialize()
            {
                Io = new Io(m_Wmx);

                m_Initialized = true;
            }

            internal static void Uninitialize()
            {
                Io.Dispose();

                m_Initialized = false;
            }

            public static int SetOutBit(bool op, int bitAddr, int byteAddr)
            {
                int eCode = Io.SetOutBit(byteAddr, bitAddr, op ? (byte)1 : (byte)0);
                return eCode;
            }

            public static int SetOutBytes(int bytesize, byte[] data, int byteAddr)
            {
                int eCode = Io.SetOutBytes(byteAddr, bytesize, data);
                return eCode;
            }

            public static int SetOutBytes(uint bytesize, byte[] data, uint byteAddr)
            {
                return SetOutBytes((int)bytesize, data, (int)byteAddr);
            }

            public static int SetOutBytes(int bytesize, bool[] data, int byteAddr)
            {
                byte[] bytedata = new byte[bytesize];

                for (int i = 0; i < data.Length; i++)
                {
                    int byteidx = i / 8;
                    int bitidx = i % 8;
                    bytedata[byteidx] += data[i] ? (byte)(1 << bitidx)
                                                 : (byte)0;
                }

                return Io.SetOutBytes(byteAddr, bytesize, bytedata);
            }

            public static int SetOutBytes(uint bytesize, bool[] data, uint byteAddr)
            {
                return SetOutBytes((int)byteAddr, data, (int)bytesize);
            }

            public static int GetOutBit(ref bool op, int bitAddr, int byteAddr)
            {
                byte pdata = 0;
                int eCode = Io.GetOutBit(byteAddr, bitAddr, ref pdata);
                op = pdata == 1;

                return eCode;
            }

            public static int GetOutBytes(int bytesize, ref bool[] op, int byteAddr)
            {
                byte[] pdata = new byte[0];
                int eCode = Io.GetOutBytes(byteAddr, bytesize, ref pdata);

                if (eCode != 0) return eCode;

                op = new bool[pdata.Length * 8];

                for (int byteidx = 0; byteidx < bytesize; byteidx++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        op[byteidx * 8 + i] = (pdata[byteidx] & 0x01 << i) != 0;
                    }
                }

                return eCode;
            }

            public static int GetOutBytes(int bytesize, ref byte[] data, int byteAddr)
            {
                int eCode = Io.GetOutBytes(byteAddr, bytesize, ref data);

                return eCode;
            }

            public static int GetInBit(ref bool op, int bitAddr, int byteAddr = 0)
            {
                byte pdata = 0;
                int eCode = Io.GetInBit(byteAddr, bitAddr, ref pdata);
                op = pdata == 1;

                return eCode;
            }

            public static int GetInBytes(int bytesize, ref bool[] op, int byteAddr)
            {
                byte[] pdata = new byte[0];
                int eCode = Io.GetInBytes(byteAddr, bytesize, ref pdata);

                if (eCode != 0) return eCode;

                op = new bool[pdata.Length * 8];

                for (int byteidx = 0; byteidx < bytesize; byteidx++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        op[byteidx * 8 + i] = (pdata[byteidx] & 0x01 << i) != 0;
                    }
                }

                return eCode;
            }

            public static int GetInBytes(uint bytesize, ref bool[] op, uint byteAddr)
            {
                return GetInBytes((int)bytesize, ref op, (int)byteAddr);
            }

            public static int GetInBytes(int bytesize, ref byte[] data, int byteAddr)
            {
                int eCode = Io.GetInBytes(byteAddr, bytesize, ref data);

                return eCode;
            }

            public static int GetInBytes(uint bytesize, ref byte[] data, uint byteAddr)
            {
                return GetInBytes((int)bytesize, ref data, (int)byteAddr);
            }
            #endregion
        }
    }
}
