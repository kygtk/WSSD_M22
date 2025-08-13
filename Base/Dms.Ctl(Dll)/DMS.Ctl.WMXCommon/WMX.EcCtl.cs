using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using WMX3ApiCLR.EcApiCLR;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        public static class EcCtl
        {
            #region Fields
            private static bool m_Initialized = false;

            private static Thread m_ThreadUpdateSlaveData = null;
            private static int m_NetworkScanIntervalMilliSeconds = 500;
            private static DateTime m_NetworkScanTime = DateTime.Now;
            private static DateTime m_HotConnectTime = DateTime.Now;

            private static Ecat m_Ec;
            private static EcMasterInfo m_EcMasterInfo;

            private static List<int> m_VendorIds;
            private static List<int> m_ProductCodes;
            private static List<string> m_VendorNames;
            private static List<string> m_ProductNames;

            private static List<Slave> m_Slaves;
            #endregion

            #region Flags
            public static bool flag_PauseUpdatingSlave = false;
            public static bool flag_DoUpdatingSlaveOnce = false;

            public static int flag_EngineOperation = 0;
            #endregion

            #region Properties
            public static bool Initialized
            {
                get { return m_Initialized; }
            }

            public static int UpdateSlaveDataIntervalMilliSeconds
            {
                get { return m_NetworkScanIntervalMilliSeconds; }
                set { m_NetworkScanIntervalMilliSeconds = value; }
            }

            public static List<Slave> Slaves
            {
                get { return m_Slaves; }
                set { m_Slaves = value; }
            }
            #endregion

            #region Methods
            internal static void Initialize()
            {
                m_Ec = new Ecat(m_Wmx);
                m_EcMasterInfo = new EcMasterInfo();

                ReadAllESIFiles();
                InitializeSlaves();
                InitializeThread();

                m_Initialized = true;
            }

            private static void ReadAllESIFiles()
            {
                m_VendorIds = new List<int>();
                m_ProductCodes = new List<int>();
                m_VendorNames = new List<string>();
                m_ProductNames = new List<string>();

                string folderdir = @"C:\Program Files\SoftServo\WMX3\ESI";
                DirectoryInfo DI = new DirectoryInfo(folderdir);

                FileInfo[] files = DI.GetFiles();

                foreach (FileInfo file in files)
                {
                    if (file.Extension != ".xml")
                        continue;

                    string filedir = file.FullName;

                    XmlDocument xml = new XmlDocument();
                    xml.Load(filedir);

                    XmlNodeList nodeVendorList = xml.SelectNodes("/EtherCATInfo/Vendor");

                    if (nodeVendorList.Count <= 0) continue;
                    string sVendorID = nodeVendorList[0].SelectSingleNode("Id").InnerText;
                    int nVendorID = Convert.ToInt32(sVendorID.Substring(2), 16);

                    XmlNodeList nodeProductList = xml.SelectNodes("/EtherCATInfo/Descriptions/Devices/Device");

                    for (int i = 0; i < nodeProductList.Count; i++)
                    {
                        string sProductData = nodeProductList[i].SelectSingleNode("Type").OuterXml;

                        string[] splitData = sProductData.Split(' ');
                        string sProductCode = "";
                        foreach (string st in splitData)
                        {
                            if (!st.StartsWith("ProductCode")) continue;

                            sProductCode = st.Split('\"')[1];
                            break;
                        }
                        int nProductCode = sProductCode.StartsWith("#x") ? Convert.ToInt32(sProductCode.Substring(2), 16)
                                                                         : Convert.ToInt32(sProductCode);

                        string sVendorName = nodeVendorList[0].SelectSingleNode("Name").InnerText;
                        string sProductName = nodeProductList[i].SelectSingleNode("Type").InnerText;

                        //  Save
                        {
                            m_VendorIds.Add(nVendorID);
                            m_ProductCodes.Add(nProductCode);
                            m_VendorNames.Add(sVendorName);
                            m_ProductNames.Add(sProductName);
                        }
                    }
                }
            }

            private static void InitializeSlaves()
            {
                if (m_Slaves == null)
                    m_Slaves = new List<Slave>();
                else
                    m_Slaves.Clear();

                ScanNetwork();
                UpdateMasterInfo();

                uint SlaveCount = m_EcMasterInfo.NumOfSlaves;
                for (int i = 0; i < SlaveCount; i++)
                {
                    EcSlaveInfo SlaveInfo = m_EcMasterInfo.Slaves[i];

                    Slave Slave = MakeSlave(i, SlaveInfo);
                    m_Slaves.Add(Slave);
                }
            }

            private static Slave MakeSlave(int SlaveNo, EcSlaveInfo SlaveInfo)
            {
                int AliasNo = (int)SlaveInfo.Alias;
                int VendorId = (int)SlaveInfo.VendorId;
                int ProductCode = (int)SlaveInfo.ProductCode;

                int InSize = SlaveInfo.InputSize;
                int OutSize = SlaveInfo.OutputSize;
                int InAddr = SlaveInfo.InputAddr;
                int OutAddr = SlaveInfo.OutputAddr;
                int AxesNum = (int)SlaveInfo.NumOfAxes;

                string VendorName;
                string ProductName;

                GetName(VendorId, ProductCode, out VendorName, out ProductName);

                //  Servo
                if /**/ (AxesNum != 0)
                    return new Slave_Servo(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, SlaveInfo.AxisInfo[0].AxisIndex);
                //  Fastech BLDC
                else if (VendorId == 0x0FA00000 && ProductCode == 0x00000FA4)
                    return new Slave_BLDC(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr, Slave_BLDC.BldcType.Fastech);
                //  YDIIT BLDC
                else if (VendorId == 0x00000080 && ProductCode == 0x00000003)
                    return new Slave_BLDC(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr, Slave_BLDC.BldcType.YDIIT);
                //  LS Inverter
                else if (VendorId == 0x000005E1 && ProductCode == 0x64150034)
                    return new Slave_Inverter(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr);
                //  DMS AP
                else if (VendorId == 0x00000F1C && ProductCode == 0x10000100)
                    return new Slave_AP(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr);
                else if (VendorId == 0x0000079a && ProductCode == 0x00abcdef)
                    return new Slave_AP(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr);
                //  YDIIT Serial to EtherCAT
                else if (VendorId == 0x00000080 && ProductCode == 0x00000016) { }
                //  I/O Slaves
                else
                {
                    bool isAnalog = ProductName.Contains("AI")
                                 || ProductName.Contains("AO")
                                 || ProductName.Contains("AD")
                                 || ProductName.Contains("DA");
                    if (isAnalog)
                        return new Slave_AIO(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr);
                    else
                        return new Slave_DIO(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName, InSize, OutSize, InAddr, OutAddr);
                }

                //  아무것도 해당되지 않으면 빈 슬레이브 반환
                return new Slave(SlaveNo, AliasNo, VendorId, VendorName, ProductCode, ProductName);
            }

            private static void GetName(int VendorId, int ProductCode, out string VendorName, out string ProductName)
            {
                VendorName = "";
                ProductName = "";

                int nNumOfESIData = m_VendorIds.Count;

                for (int i = 0; i < nNumOfESIData; i++)
                {
                    if (VendorId == m_VendorIds[i] && ProductCode == m_ProductCodes[i])
                    {
                        VendorName = m_VendorNames[i];
                        ProductName = m_ProductNames[i];
                        break;
                    }
                }
            }

            private static void InitializeThread()
            {
                if (m_ThreadUpdateSlaveData != null)
                {
                    m_ThreadUpdateSlaveData.Abort();
                    while (m_ThreadUpdateSlaveData.IsAlive) { }
                }

                m_ThreadUpdateSlaveData = new Thread(() => UpdateSlaveData_InSubThread())
                {
                    Name = "UpdateSlaveData",
                    IsBackground = true
                };
                m_ThreadUpdateSlaveData.Start();
            }

            private static void UpdateSlaveData_InSubThread()
            {
                while (true)
                {
                    Thread.Sleep(20);

                    //  Engine Operation 중 Update 중단
                    if (flag_EngineOperation != 0) continue;

                    //  Master Info Scan
                    if ((DateTime.Now - m_NetworkScanTime).TotalMilliseconds > m_NetworkScanIntervalMilliSeconds)
                    {
                        if (ScanNetwork() == 0)
                            m_NetworkScanTime = DateTime.Now;
                    }
                    UpdateMasterInfo();

                    //  Hot Connect
                    if (SlaveCount(EcStateMachine.None) > 0 && (DateTime.Now - m_HotConnectTime).TotalSeconds >= 3)
                    {
                        if (HotConnect() == 0)
                            m_HotConnectTime = DateTime.Now;
                    }

                    if (flag_PauseUpdatingSlave)
                    {
                        if (flag_DoUpdatingSlaveOnce)
                            flag_DoUpdatingSlaveOnce = false;
                        else
                            continue;
                    }

                    //  Update
                    foreach (Slave slave in m_Slaves)
                    {
                        slave.UpdateState(m_EcMasterInfo);
                        slave.UpdateESCReg();
                        slave.Update();
                    }
                }
            }

            internal static void Uninitialize()
            {
                UninitializeThread();

                m_Ec.Dispose();
                m_EcMasterInfo = null;

                m_Initialized = false;
            }

            private static void UninitializeThread()
            {
                if (m_ThreadUpdateSlaveData == null) return;
                else
                    m_ThreadUpdateSlaveData.Abort();

                while (m_ThreadUpdateSlaveData.IsAlive)
                    Thread.Sleep(1);
            }

            public static int ScanNetwork()
            {
                return m_Ec.ScanNetwork();
            }

            public static int UpdateMasterInfo()
            {
                return m_Ec.GetMasterInfo(m_EcMasterInfo);
            }

            private static int HotConnect()
            {
                int eCode = m_EcMasterInfo.StartHotconnect();
                return eCode;
            }

            public static Slave GetSlave(int AliasNo, uint VendorId, int ProductCode)
            {
                foreach (Slave slave in m_Slaves)
                {
                    if (slave.AliasNo != AliasNo) continue;
                    if (slave.VendorId != VendorId) continue;
                    if (slave.ProductCode != ProductCode) continue;

                    return slave;
                }

                return null;
            }

            public static int SlaveCount()
            {
                if (!m_Initialized) return -1;

                return m_Slaves.Count;
            }

            public static int SlaveCount(SlaveType Type)
            {
                if (!m_Initialized) return -1;

                int cnt = 0;
                foreach (Slave slave in m_Slaves)
                    if (slave.SlaveType == Type) cnt++;

                return cnt;
            }

            public static int SlaveCount(EcStateMachine State)
            {
                if (!m_Initialized) return -1;

                int cnt = 0;
                foreach (Slave slave in m_Slaves)
                    if (slave.SlaveState == State) cnt++;

                return cnt;
            }

            #region SDO/PDO Control
            public static int SetSDO(int SlaveNo, int idx, int idx_sub, int size, int value)
            {
                byte[] sdo = new byte[size];
                uint errCode = 0;

                for (int i = 0; i < size; i++)
                {
                    sdo[i] = (byte)((value >> (8 * i)) & (0x00FF));
                }

                int eCode = m_Ec.SdoDownload(SlaveNo, idx, idx_sub, sdo, ref errCode);

                return eCode;
            }

            public static int GetSDO(int SlaveNo, int idx, int idx_sub, out byte[] value, out uint actualsize)
            {
                value = new byte[32];
                actualsize = 0;
                uint errCode = 0;

                int eCode = m_Ec.SdoUpload(SlaveNo, idx, idx_sub, value, ref actualsize, ref errCode);

                return eCode;
            }

            public static int TxPDO(int SlaveNo, int idx, int idx_sub, int size, int value)
            {
                byte[] pdo = new byte[size];

                for (int i = 0; i < size; i++)
                {
                    pdo[i] = (byte)((value >> (8 * i)) & (0x00FF));
                }

                int eCode = m_Ec.TxPdoWrite(0, SlaveNo, idx, idx_sub, pdo, 0xFFFF);

                return eCode;
            }
            public static int TxPDO(int SlaveNo, int idx, int idx_sub, int size, uint value)
            {
                byte[] pdo = new byte[size];

                for (int i = 0; i < size; i++)
                {
                    pdo[i] = (byte)((value >> (8 * i)) & (0x00FF));
                }

                int eCode = m_Ec.TxPdoWrite(0, SlaveNo, idx, idx_sub, pdo, 0xFFFF);

                return eCode;
            }

            public static int RxPDO(int SlaveNo, int idx, int idx_sub, int size, out byte[] value)
            {
                value = new byte[size];
                uint realsize = 0;

                int eCode = m_Ec.PdoRead(SlaveNo, idx, idx_sub, value, ref realsize);

                return eCode;
            }
            #endregion

            #region Register Control
            public static byte[] ReadRegister(int SlaveNo, int RegAddr, int RegLen)
            {
                byte[] data = new byte[RegLen];
                m_Ec.RegisterRead(SlaveNo, RegAddr, data);
                return data;
            }

            public static void WriteRegister(int SlaveNo, int RegAddr, params byte[] RegData)
            {
                m_Ec.RegisterWrite(SlaveNo, RegAddr, RegData);
            }
            #endregion
            #endregion
        }
    }
}
