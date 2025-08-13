using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Microsoft.VisualBasic;
using System.IO;
using Dms.DeviceLibrary;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;


namespace Dms.Ctl
{
    #region Define Struct
    public struct tagADDR_INFO
    {
        private devTYPE m_nDevType;
        private string m_sSetAddress;
        private short m_nStartAddr;
        private short m_nSize;

        public devTYPE Type
        {
            get { return m_nDevType; }
            set { m_nDevType = value; }
        }

        public string SetAddress
        {
            get { return m_sSetAddress; }
            set
            {
                m_sSetAddress = value;
                SetStartAddress(value);
            }
        }

        [ReadOnly(true)]
        public short StartAddress
        {
            get { return m_nStartAddr; }
            set { m_nStartAddr = value; }
        }

        public short Size
        {
            get { return m_nSize; }
            set { m_nSize = value; }
        }

        //Constructor..
        public tagADDR_INFO(devTYPE nDevType, string sSetAddress, short nStartAddr, short nSize)
        {
            m_nDevType = nDevType;
            m_sSetAddress = sSetAddress;
            m_nStartAddr = nStartAddr;
            m_nSize = nSize;
        }

        public void SetStartAddress(string address)
        {
            this.m_nStartAddr = Convert.ToInt16(address, 16);

        }

        public override string ToString()
        {
            string info = "";
            info += m_nDevType.ToString() + " : ";
            info += m_sSetAddress + " - ";
            info += m_nSize.ToString();

            return info;
        }
    }
    #endregion

    public class Melsec : DmsNode
    {
        #region Melsec DLL Import
        [DllImport("MdFunc32.dll", EntryPoint = "mdOpen")]
        static extern short mdopen(short Chan, short Mode, ref int Path);
        [DllImport("MdFunc32.dll", EntryPoint = "mdClose")]
        static extern short mdclose(int Path);
        [DllImport("MdFunc32.dll", EntryPoint = "mdSend")]
        static extern short mdsend(int Path, short Stno, short Devtyp, short devno, ref short size_Renamed, ref short buf);
        [DllImport("MdFunc32.dll", EntryPoint = "mdReceive")]
        static extern short mdreceive(int Path, short Stno, short Devtyp, short devno, ref short size_Renamed, ref short buf);
        [DllImport("MdFunc32.dll", EntryPoint = "mdDevSet")]
        static extern short mddevset(int Path, short Stno, short Devtyp, short devno);
        [DllImport("MdFunc32.dll", EntryPoint = "mdDevRst")]
        static extern short mddevrst(int Path, short Stno, short Devtyp, short devno);
        [DllImport("MdFunc32.dll", EntryPoint = "mdRandW")]
        static extern short mdrandw(int Path, short Stno, ref short dev, ref short buf, short bufsiz);
        [DllImport("MdFunc32.dll", EntryPoint = "mdRandR")]
        static extern short mdrandr(int Path, short Stno, ref short dev, ref short buf, short bufsiz);
        [DllImport("MdFunc32.dll", EntryPoint = "mdControl")]
        static extern short mdcontrol(int Path, short Stno, short buf);
        [DllImport("MdFunc32.dll", EntryPoint = "mdTypeRead")]
        static extern short mdtyperead(int Path, short Stno, ref short format_name);
        [DllImport("MdFunc32.dll")]
        static extern short mdBdLedRead(int Path, ref short buf);
        [DllImport("MdFunc32.dll")]
        static extern short mdBdModRead(int Path, ref short Mode);
        [DllImport("MdFunc32.dll")]
        static extern short mdBdModSet(int Path, short Mode);
        [DllImport("MdFunc32.dll")]
        static extern short mdBdRst(int Path);
        [DllImport("MdFunc32.dll")]
        static extern short mdBdSwRead(int Path, ref short buf);
        [DllImport("MdFunc32.dll")]
        static extern short mdBdVerRead(int Path, ref short buf);
        [DllImport("MdFunc32.dll", EntryPoint = "mdInit")]
        static extern short mdinit(int Path);
        [DllImport("MdFunc32.dll")]
        static extern short mdWaitBdEvent(int Path, ref short eventno, int timeout, ref short signaledno, ref short details);
        [DllImport("MdFunc32.dll", EntryPoint = "mdSendEx")]
        static extern int mdsendex(int Path, int Netno, int Stno, int Devtyp, int devno, ref int size, ref short buf);
        [DllImport("MdFunc32.dll", EntryPoint = "mdReceiveEx")]
        static extern int mdreceiveex(int Path, int Netno, int Stno, int Devtyp, int devno, ref int size, ref short buf);
        [DllImport("MdFunc32.dll", EntryPoint = "mdDevSetEx")]
        static extern int mddevsetex(int Path, int Netno, int Stno, int Devtyp, int devno);
        [DllImport("MdFunc32.dll", EntryPoint = "mdDevRstEx")]
        static extern int mddevrstex(int Path, int Netno, int Stno, int Devtyp, int devno);
        [DllImport("MdFunc32.dll", EntryPoint = "mdRandWEx")]
        static extern int mdrandwex(int Path, int Netno, int Stno, ref int dev, ref short buf, int bufsize);
        [DllImport("MdFunc32.dll", EntryPoint = "mdRandREx")]
        static extern int mdrandrex(int Path, int Netno, int Stno, ref int dev, ref short buf, int bufsize);
        #endregion

        #region Fields
        private static object m_LockKey = new object();
        private static bool m_IsConfigurator = false;
        private bool m_Simulate = true;
        private bool m_SimulateAddress = false;
        private bool m_Monitor = false;
        private bool m_Initialized = false;

        private short m_nNetworkNo = 0;
        private short m_nStationNo = 255;
        private short m_nUpNetworkNo = 0;
        private short m_nUpStationNo = 255;
        private short m_nDnNetworkNo = 0;
        private short m_nDnStationNo = 255;

        private int m_nPath = 0;		// 
        private short m_nMode = 0;		// mode data by own card form registry
        //private short m_nStNo = 0;		// the station number to set in MELSEC data library
        private short m_nAlarmCode = 0;
        private short m_nPlcForm = 0;
        private short m_nMonitorSize = 0;

        private bool m_bOpen = false;

        private List<tagADDR_INFO> m_MonitorAddress = null;

        private short[] m_nReadData = null;
        private List<short[]> m_nReadDataBuf = new List<short[]>();
        private clsThreadMelsec m_ThreadMelsec;
        private XLog m_MelsecLog = new XLog("MelsecLog", XLog.LogStampType.UseStamp);
        private static AppConfig m_AppConfig = AppConfig.Instance;
        //field for process working set
        private bool m_ProcessWorkingSetActivate = false;
        private static bool m_ProcessWorkingSetActivated = false;
        private ushort m_ProcessMinWorkingSetSize = 1;  //1MB
        private ushort m_ProcessMaxWorkingSetSize = 3;	//3MB
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public bool Simulate  // TODO: make sure this is called from somewhere.
        {
            get { return m_Simulate; }
            set { m_Simulate = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool SimulateAddress
        {
            get { return m_SimulateAddress; }
        }
        [Category("DMS : Setting")]
        public bool Monitoring
        {
            get { return m_Monitor; }
            set { m_Monitor = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool Initialized
        {
            get { return m_Initialized; }
        }

        [Browsable(false), XmlIgnore()]
        public static bool IsConfigurator
        {
            get { return m_IsConfigurator; }
            set { m_IsConfigurator = value; }
        }

        [Category("Network : Setting")]
        public short NetworkNo
        {
            get { return m_nNetworkNo; }
            set { m_nNetworkNo = value; }
        }

        [Category("Network : Setting")]
        public short StationNo
        {
            get { return m_nStationNo; }
            set { m_nStationNo = value; }
        }

        [Category("Network : Setting")]
        public short UpNetworkNo
        {
            get { return m_nUpNetworkNo; }
            set { m_nUpNetworkNo = value; }
        }

        [Category("Network : Setting")]
        public short UpStationNo
        {
            get { return m_nUpStationNo; }
            set { m_nUpStationNo = value; }
        }

        [Category("Network : Setting")]
        public short DnNetworkNo
        {
            get { return m_nDnNetworkNo; }
            set { m_nDnNetworkNo = value; }
        }

        [Category("Network : Setting")]
        public short DnStationNo
        {
            get { return m_nDnStationNo; }
            set { m_nDnStationNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public short Mode
        {
            get { return m_nMode; }
            set { m_nMode = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsOpen
        {
            get { return m_bOpen; }
        }

        [Category("DMS : Address Range")]
        public List<tagADDR_INFO> MonitorAddress
        {
            get { return m_MonitorAddress; }
            set { m_MonitorAddress = value; }
        }
        [Category("!Working Set"), Description("Set true when an error(code 77) occurs due to MD function execution")]
        public bool ProcessWorkingSetActivate
        {
            get { return m_ProcessWorkingSetActivate; }
            set { m_ProcessWorkingSetActivate = value; }
        }
        [Category("!Working Set"), Description("Minimum Size (MB)")]
        public ushort ProcessMinWorkingSetSize
        {
            get { return m_ProcessMinWorkingSetSize; }
            set { m_ProcessMinWorkingSetSize = value; }
        }
        [Category("!Working Set"), Description("Maximum Size (MB)")]
        public ushort ProcessMaxWorkingSetSize
        {
            get { return m_ProcessMaxWorkingSetSize; }
            set { m_ProcessMaxWorkingSetSize = value; }
        }
        #endregion

        #region Constructor
        public Melsec()
        {
            m_MonitorAddress = new List<tagADDR_INFO>();
            m_GetDmsNodeDirectory = CheckPath;
        }
        #endregion

        #region Methods
        /// <summary>
        /// INI화일에서 Melsec관련 설정을 읽어 변수에 저장
        /// </summary>
        private void Init()
        {
            try
            {

                foreach (tagADDR_INFO ai in m_MonitorAddress)
                {
                    ai.SetStartAddress(ai.SetAddress);

                    // Total Monitor size
                    if (ai.Type == devTYPE.devB) m_nMonitorSize += (short)((ai.Size - 1) / 16 + 1);
                    else if (ai.Type == devTYPE.devW) m_nMonitorSize += ai.Size;

                    // Unit Monitor size
                    short size = 0;
                    if (ai.Type == devTYPE.devB) size = (short)((ai.Size - 1) / 16 + 1);
                    else if (ai.Type == devTYPE.devW) size = ai.Size;
                    // Make buffer for unit
                    m_nReadDataBuf.Add(new short[size]);
                }

                // Make memory for total
                m_nReadData = new short[m_nMonitorSize];
            }
            catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString() + "\nMelsec::Init()함수를 체크하세요");
            }
        }

        public short GetReadData(int i)
        {
            return m_nReadData[i];
        }

        public bool Open(short nChannel, short nMode)
        {
            short nRv;

            if (Initialized == false)
            {
                Init();
            }

            m_Initialized = true;

            if (m_Simulate != true)
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                //Procedures and sample program for increasing the minimum working set area of the PC
                //The following provides measures for increasing the minimum working set area of the PC when an
                //error of error code 77 occurs due to MD function execution, and its sample program.
                //The PC board driver runs using the minimum working set area in the memory area reserved in the
                //application program. Some application program may use a large area of the minimum working set
                //area. In such a case, when the minimum working set area for the PC board driver cannot be
                //reserved, an error code 77 is returned.
                //If this situation occurs, increase the minimum working set area in the application program before
                //executing the MD function. (See the following sample program.)
                //The minimum working set area of 200KB is reserved at startup of the personal computer.
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                if (!m_Simulate && !m_ProcessWorkingSetActivated)
                {
                    if (m_ProcessWorkingSetActivate)
                    {
                        m_ProcessWorkingSetActivated = true;
                        XFunc.SetProcessWorkingSet(m_ProcessMinWorkingSetSize, m_ProcessMaxWorkingSetSize);
                    }
                }

                // short	ret;		return value				OUT
                // long		*path;		opened loop path pointer
                // short	channel;	path of channel				IN
                // short	mode;		dummy(select -1)			IN
                if ((nRv = mdopen(nChannel, nMode, ref m_nPath)) != 0)
                {
                    string sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("MELSEC-NET : mdOpen error {0}", m_nAlarmCode);

                    m_bOpen = false;

                    MessageBox.Show(sError);

                    return false;
                }

                m_bOpen = true;
            }

            if (m_IsConfigurator == false)
            {
                if (m_Simulate)
                {
                    MessageBox.Show("MELSEC-NET Simulate Mode");
                }
            }

            m_ThreadMelsec = new clsThreadMelsec(this);
            m_ThreadMelsec.Start();

            return true;
        }

        public bool Close()
        {
            m_bOpen = false;

            short nRv;

            if (!m_Simulate)
            {// short	ret;	return value	OUT
                if ((nRv = mdclose(m_nPath)) != 0)
                {
                    string sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("MELSEC-NET : mdClose error {0}", m_nAlarmCode);
                    MessageBox.Show(sError);

                    return false;
                }
            }
            return true;
        }

        public bool DevSet(short nNetNo, short nStNo, devTYPE dev, short nDevNo)
        {
            short nRv;

            // short	ret;	return value			OUT
            // long		path;	path of channel			IN
            // short	stno;	station number			IN
            // short	devtyp;	device type				IN
            // short	devno;	selected device number	IN
            short nStationNo = (short)(nStNo + (nNetNo << 8));

            if ((nRv = mddevset(m_nPath, nStationNo, (short)dev, nDevNo)) != 0)
            {
                string sError;

                m_nAlarmCode = nRv;
                sError = string.Format("mdDevSet error {%d}", m_nAlarmCode);
                MessageBox.Show(sError);

                return false;
            }
            return true;
        }

        public bool DevRst(short nNetNo, short nStNo, devTYPE dev, short nDevNo)
        {
            short nRv;

            // short	ret;	return value			OUT
            // long		path;	path of channel			IN
            // short	stno;	station number			IN
            // short	devtyp;	device type				IN
            // short	devno;	selected device number	IN
            short nStationNo = (short)(nStNo + (nNetNo << 8));

            nRv = mddevrst(m_nPath, nStationNo, (short)dev, nDevNo);

            if (nRv != 0)
            {
                string sError;
                m_nAlarmCode = nRv;
                sError = string.Format("mdDevRst error {0}", m_nAlarmCode);
                MessageBox.Show(sError);

                return false;
            }
            return true;
        }

        public bool Control(short nNetNo, short nStNo, short nControlMode)
        {
            short nRv;

            // short	ret;	return value	OUT
            // long		path;	path of channel	IN
            // short	stno;	station number	IN
            // short	buf;	selected code	IN

            //Remotes RUN/STOP/PAUSE to PLC
            //Indicates the selected code
            //Remote RUN	0
            //Remote STOP	1
            //Remote PAUSE	2

            short nStationNo = (short)(nStNo + (nNetNo << 8));

            nRv = mdcontrol(m_nPath, nStationNo, nControlMode);

            if (nRv != 0)
            {
                string sError;
                m_nAlarmCode = nRv;
                sError = string.Format("mdControl error {0}", nRv);
                MessageBox.Show(sError);

                return false;
            }
            return true;
        }

        public bool TypeRead(short nNetNo, short nStNo)
        {
            short nRv;
            string sError;

            short nStationNo = (short)(nStNo + (nNetNo << 8));

            // short	ret;	return value		OUT
            // long		path;	path of channel		IN
            // short	stno;	station number		IN
            // short	*buf;	format name code	OUT
            nRv = mdtyperead(m_nPath, nStationNo, ref m_nPlcForm);

            if (nRv != 0)
            {
                m_nAlarmCode = nRv;
                sError = string.Format("mdTypeRead error {0}", m_nAlarmCode);
                MessageBox.Show(sError);

                return false;
            }
            return true;
        }

        public bool ReceiveBit(short nDevNo)
        {
            if (m_IsConfigurator) return false;

            // Find the address
            int nLen = m_MonitorAddress.Count;
            int nValue;
            bool bReturn;
            int nIndex = 0;

            for (int i = 0; i < nLen; i++)
            {
                tagADDR_INFO temp = m_MonitorAddress[i];

                if (temp.Type == devTYPE.devB &&
                    (nDevNo >= temp.StartAddress && nDevNo < temp.StartAddress + temp.Size))
                {
                    if (nDevNo - temp.StartAddress >= 16)
                    {
                        nIndex += (nDevNo - temp.StartAddress) / 16;
                    }

                    // Send the bit data
                    nValue = (short)((m_nReadData[nIndex] >> ((nDevNo - temp.StartAddress) % 16)) & 0x0001);
                    bReturn = (bool)Interaction.IIf(nValue > 0, true, false);
                    return bReturn;
                }

                if (temp.Type == devTYPE.devB) nIndex += (temp.Size - 1) / 16 + 1;
                else if (temp.Type == devTYPE.devW) nIndex += temp.Size;
            }

            string sError;
            sError = string.Format("ReceiveBit Address {0} is not registered", nDevNo);
            MessageBox.Show(sError);

            return false;
        }

        public string ReceiveBit(string address)
        {
            if (ReceiveBit(GetAddressDec(address)))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public short ReceiveWord(short nDevNo)
        {
            if (m_IsConfigurator) return 0;

            // Find the address
            int nLen = m_MonitorAddress.Count;
            int nIndex = 0;

            for (int i = 0; i < nLen; i++)
            {
                tagADDR_INFO temp = m_MonitorAddress[i];

                if (temp.Type == devTYPE.devW &&
                    (nDevNo >= temp.StartAddress && nDevNo < temp.StartAddress + temp.Size))
                {
                    // Send Data
                    short nReturn = m_nReadData[nIndex + nDevNo - temp.StartAddress];
                    return nReturn;
                }

                if (temp.Type == devTYPE.devB) nIndex += (temp.Size - 1) / 16 + 1;
                else if (temp.Type == devTYPE.devW) nIndex += temp.Size;
            }

            string sError;
            sError = string.Format("ReceiveWord Address {0} is not registered", nDevNo);
            MessageBox.Show(sError);

            return 0;
        }

        public string ReceiveWord(string address)
        {
            int value;

            value = ReceiveWord(GetAddressDec(address));

            return value.ToString();
        }

        /////////////////////////////////////////////////////////////
        // ReadString(D007000, lpszBuf, 8)
        // lpszBuf = "1234abcd"
        // sStr   = 32 31 34 33 62 61 64 63
        public string ReadString(short nDevNo, int nBufLen)
        {
            if (m_IsConfigurator) return "";

            // Find the address
            int nLen = m_MonitorAddress.Count;
            int nIndex = 0;
            int i = 0;

            for (i = 0; i < nLen; i++)
            {
                tagADDR_INFO temp = m_MonitorAddress[i];

                if (temp.Type == devTYPE.devW &&
                    (nDevNo >= temp.StartAddress && nDevNo < temp.StartAddress + temp.Size))
                {
                    nIndex = nIndex + nDevNo - temp.StartAddress;
                    break;
                }

                if (temp.Type == devTYPE.devB) nIndex += (temp.Size - 1) / 16 + 1;
                else if (temp.Type == devTYPE.devW) nIndex += temp.Size;
            }

            // Check the address
            if (i == nLen)
            {
                string sError;
                sError = string.Format("ReceiveWord Address {0} is not registered", nDevNo);
                MessageBox.Show(sError);
                return "";
            }

            // Convert to string	        
            short nWord = (short)(nBufLen / 2 + nBufLen % 2);
            string sStr = "";

            for (i = 0; i < nWord; i++)
            {
                short nData = m_nReadData[nIndex + i];

                char ch;
                ch = (char)(nData & 0x00FF);
                if (ch == 0) ch = ' ';
                sStr += ch;
                ch = (char)((nData >> 8) & 0x00FF);
                if (ch == 0) ch = ' ';
                sStr += ch;
            }

            return sStr;
        }

        public bool SendBit(short nNetNo, short nStNo, short nDevNo, short nData)
        {
            lock (m_LockKey)
            {
                if (Simulate)
                {

                    return DebugSendBit(nDevNo, nData);
                }

                short nRv;

                // short	ret;	return value						OUT
                // long		path;	path of channel						IN
                // short	stno;	station number						IN
                //			2 Byte : Upper = Network No.
                //					 Lower = Station No. or Group No.
                // short	devtyp;	device type							IN
                // short	devno;	front device No.					IN
                // short	data;	written data (single precision)		IN
                short nStationNo = (short)(nStNo + (nNetNo << 8));

                if (nData > 0)
                {
                    if ((nRv = mddevset(m_nPath, nStationNo, (short)devTYPE.devB, nDevNo)) != 0)
                    {
                        String sError;
                        m_nAlarmCode = nRv;
                        sError = string.Format("mdDevSet error {0}", m_nAlarmCode);
                        MessageBox.Show(sError);

                        return false;
                    }
                }
                else
                {
                    if ((nRv = mddevrst(m_nPath, nStationNo, (short)devTYPE.devB, nDevNo)) != 0)
                    {
                        String sError;
                        m_nAlarmCode = nRv;
                        sError = string.Format("mdDevRst error {0}", m_nAlarmCode);
                        MessageBox.Show(sError);

                        return false;
                    }
                }
                return true;
            }
        }

        public bool SendBit(short nNetNo, short nStNo, short nDevNo, string sData)
        {
            lock (m_LockKey)
            {
                if (Simulate)
                {
                    return DebugSendBit(GetAddress(devTYPE.devB, nDevNo), sData, false);
                }

                short nRv;

                short nLen = (short)sData.Length;
                short nWord = (short)((nLen - 1) / 16 + 1);
                short nSize = (short)(nWord * 2);
                short[] nData = new short[nWord];
                short[] nSendData = new short[nWord];

                string nRecvBin = "";
                string nSendBin = "";
                string TempBin = "";

                // short	ret;	return value						OUT
                // long		path;	path of channel						IN
                // short	stno;	station number						IN
                // short	devtyp;	device type							IN
                // short	devno;	front device No.					IN
                // short	data;	written data (single precision)		IN
                short nStationNo = (short)(nStNo + (nNetNo << 8));

                if ((nRv = mdreceive(m_nPath, nStationNo, (short)devTYPE.devB, nDevNo, ref nSize, ref nData[0])) != 0)
                {
                    String sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("mdRecevie(Bit) error {0}", nRv);
                    MessageBox.Show(sError);

                    return false;
                }

                for (int i = 0; i < nWord; i++)
                {
                    nRecvBin += CompareBin(nData[i]);
                    nSendBin = sData.PadRight(nWord * 16, '0');
                }

                for (int n = 0; n < nWord * 16; n++)
                {
                    if (n < nLen)
                        TempBin += nSendBin.Substring(n, 1);
                    else
                        TempBin += nRecvBin.Substring(n, 1);
                }

                for (int i = 0; i < nWord; i++)
                {
                    nSendData[i] = CompareDec(TempBin.Substring(i * 16, 16));
                }

                if ((nRv = mdsend(m_nPath, nStationNo, (short)devTYPE.devB, nDevNo, ref nSize, ref nSendData[0])) != 0)
                {
                    String sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("mdSend(Bit) error {0}", nRv);
                    MessageBox.Show(sError);

                    return false;
                }
                return true;
            }
        }


        public bool SendWord(short nNetNo, short nStNo, short nDevNo, short nData)
        {
            lock (m_LockKey)
            {
                if (Simulate)
                {
                    return DebugSendWord(nDevNo, nData);
                }

                short nRv;
                short nSize = 2;	// 2 bytes

                // short	ret;	return value						OUT
                // long		path;	path of channel						IN
                // short	stno;	station number						IN
                // short	devtyp;	device type							IN
                // short	devno;	front device No.					IN
                // short	data;	written data (single precision)		IN
                short nStationNo = (short)(nStNo + (nNetNo << 8));

                if ((nRv = mdsend(m_nPath, nStationNo, (short)devTYPE.devW, nDevNo, ref nSize, ref nData)) != 0)
                {
                    String sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("mdSend error {0}", nRv);
                    MessageBox.Show(sError);

                    return false;
                }
                return true;
            }
        }

        //////////////////////////////////////////////////////////////
        // ReadString(D007000, lpszBuf, 8)
        // lpszBuf = "1234abcd"
        // sStr   = 32 31 34 33 62 61 64 63
        public bool WriteString(short nNetNo, short nStNo, short nDevNo, string sStr, short nLen)
        {
            lock (m_LockKey)
            {
                if (Simulate)
                {
                    return DebugSendString(nDevNo, sStr, nLen);
                }

                short nStationNo = (short)(nStNo + (nNetNo << 8));
                short nWord = (short)(nLen / 2 + nLen % 2);
                short[] nData = new short[nWord];
                short nSize;
                short nRv;

                for (int i = 0; i < nWord; i++)
                {
                    char ch1, ch2;

                    if (sStr.Length <= i * 2) ch1 = (char)0x20;
                    else ch1 = Convert.ToChar(sStr.Substring(i * 2, 1));

                    if (sStr.Length <= i * 2 + 1) ch2 = (char)0x20;
                    else ch2 = Convert.ToChar(sStr.Substring(i * 2 + 1, 1));

                    if (ch1 == ' ') ch1 = (char)0x20;	// Space는 그대로 ...........
                    if (ch2 == ' ') ch2 = (char)0x20;
                    if (ch1 == '\r') ch1 = (char)0x00;	// Space는 그대로 ...........
                    if (ch2 == '\r') ch2 = (char)0x00;

                    nData[i] = (short)((ch2 << 8) | ch1);
                }

                nSize = (short)(nWord * 2);

                if ((nRv = mdsend(m_nPath, nStationNo, (short)devTYPE.devW, nDevNo, ref nSize, ref nData[0])) != 0)
                {
                    String sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("mdSend error {0}", m_nAlarmCode);
                    MessageBox.Show(sError);

                    return false;
                }
                return true;
            }
        }

        public short[] ReceiveWords(short nDevNo, int size)
        {
            if (m_IsConfigurator) return null;

            // Find the address
            int nLen = m_MonitorAddress.Count;
            int nIndex = 0;

            for (int i = 0; i < nLen; i++)
            {
                tagADDR_INFO temp = m_MonitorAddress[i];

                if (temp.Type == devTYPE.devW &&
                    (nDevNo >= temp.StartAddress && nDevNo < temp.StartAddress + temp.Size))
                {
                    // Send Data
                    short[] nReturn = new short[size];
                    for (int k = 0; k < size; k++)
                    {
                        nReturn[k] = m_nReadData[nIndex + nDevNo + k - temp.StartAddress];
                    }
                    return nReturn;
                }

                if (temp.Type == devTYPE.devB) nIndex += (temp.Size - 1) / 16 + 1;
                else if (temp.Type == devTYPE.devW) nIndex += temp.Size;
            }

            string sError;
            sError = string.Format("ReceiveWord Address {0} is not registered", nDevNo);
            MessageBox.Show(sError);

            return null;
        }

        public bool SendWords(short nNetNo, short nStNo, short nDevNo, short[] nData)
        {
            lock (m_LockKey)
            {
                if (Simulate)
                {
                    return DebugSendWords(nDevNo, nData);
                }

                short nRv;
                short nSize = (short)(nData.Length * 2);

                // short	ret;	return value						OUT
                // long		path;	path of channel						IN
                // short	stno;	station number						IN
                // short	devtyp;	device type							IN
                // short	devno;	front device No.					IN
                // short	data;	written data (single precision)		IN
                short nStationNo = (short)(nStNo + (nNetNo << 8));

                if ((nRv = mdsend(m_nPath, nStationNo, (short)devTYPE.devW, nDevNo, ref nSize, ref nData[0])) != 0)
                {
                    String sError;
                    m_nAlarmCode = nRv;
                    sError = string.Format("mdSend error {0}", nRv);
                    MessageBox.Show(sError);

                    return false;
                }
                return true;
            }
        }


        public string CompareBin(short nValue)
        {
            string DectoBin;
            string Result = "";

            DectoBin = Convert.ToString(nValue, 2);
            DectoBin = DectoBin.PadLeft(16, '0');

            for (int i = DectoBin.Length - 1; i >= 0; i--)
            {
                Result = Result + DectoBin.Substring(i, 1);
            }

            return Result;
        }

        public short CompareDec(string sValue)
        {
            string sResult = "";

            int count = sValue.Length;
            for (int i = 0; i < count; i++)
            {
                sResult = sValue.Substring(i, 1) + sResult;
            }

            return Convert.ToInt16(sResult, 2);
        }

        public string Reverse(string sValue)
        {
            string sResult = "";

            for (int i = sValue.Length - 1; i >= 0; i--)
            {
                sResult = sResult + sValue.Substring(i, 1);
            }

            return sResult;
        }

        public String DecToHex(short nValue)
        {
            string sHex = "";

            try
            {
                sHex = Convert.ToString(nValue, 16).PadLeft(4, '0');
            }
            catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                sHex = err.Message.ToString();
            }
            return sHex.ToUpper();
        }

        public String DecToHex(int nValue)
        {
            string sHex = "";

            try
            {
                sHex = Convert.ToString(nValue, 16).PadLeft(4, '0');
            }
            catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                sHex = err.Message.ToString();
            }
            return sHex.ToUpper();
        }


        public String DecToHex(string sValue)
        {
            string sHex = "";


            try
            {
                sHex = Convert.ToString(Convert.ToInt32(sValue), 16).PadLeft(4, '0');
            }
            catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                sHex = err.Message.ToString();
            }
            return sHex.ToUpper();
        }

        public string DecToAsc(string sValue)
        {
            string sTemp = "";

            sTemp = DecToHex(Convert.ToInt32(sValue));

            return HexToAsc(sTemp);
        }

        public String DecToBin(short nValue)
        {
            string sBin = "";

            try
            {
                sBin = Convert.ToString(nValue, 2).PadLeft(16, '0');
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                sBin = err.Message.ToString();
            }
            return sBin.ToUpper();
        }

        public String DecToBin(string sValue)
        {
            string sBin = "";

            try
            {
                sBin = Convert.ToString(Convert.ToInt32(sValue), 2).PadLeft(16, '0');
            }
            catch (Exception ex)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                sBin = ex.Message.ToString();
            }
            return sBin.ToUpper();
        }

        public int HexToDec(string sValue)
        {
            try
            {
                int nValue;

                nValue = Convert.ToInt32(sValue, 16);

                return nValue;
            }
            catch
            {
                return -1;
            }
        }

        public string HexToAsc(string sValue)
        {
            try
            {
                string sTemp = "";
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < sValue.Length / 2; i++)
                {
                    int nDec = 0;

                    nDec = Convert.ToInt32(sValue.Substring(i * 2, 2), 16);

                    sTemp = Convert.ToChar(nDec).ToString();
                    sb.Insert(0, sTemp);
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string HexToBin(string sValue)
        {
            try
            {
                string sTemp = "";
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < sValue.Length / 2; i++)
                {
                    int nDec = 0;

                    nDec = Convert.ToInt32(sValue.Substring(i * 2, 2), 16);

                    sTemp = Convert.ToString(nDec, 2);
                    sb.Append(Strings.UCase(sTemp.PadLeft(8, '0')));
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public int AscToDec(string sValue)
        {
            int nDec = 0;
            string sTemp = AscToHex(sValue);

            nDec = Convert.ToInt32(sTemp, 16);

            return nDec;
        }

        public string AscToHex(string sValue)
        {
            string sTemp = "";
            StringBuilder sb = new StringBuilder();

            int count = sValue.Length;
            for (int i = 0; i < count; i++)
            {
                int nDec = Strings.Asc(sValue.Substring(i, 1));

                sTemp = Convert.ToString(nDec, 16);
                sb.Insert(0, Strings.UCase(sTemp.PadLeft(2, '0')));
            }

            if (sValue.Length == 1)
                sb.Insert(0, "00");

            return sb.ToString();
        }

        public string AscToBin(string sValue)
        {
            string sTemp = "";
            StringBuilder sb = new StringBuilder();

            int count = sValue.Length;
            for (int i = 0; i < count; i++)
            {
                int nDec = Strings.Asc(sValue.Substring(i, 1));

                sTemp = Convert.ToString(nDec, 2); // 10진수를 2진수로
                sb.Insert(0, Strings.UCase(sTemp.PadLeft(8, '0')));
            }

            if (sValue.Length == 1)
                sb.Insert(0, "00000000");

            return sb.ToString();

        }

        public int BinToDec(string sValue)
        {
            try
            {
                int nDec = 0;
                nDec = Convert.ToInt32(sValue, 2);

                return nDec;
            }
            catch
            {
                return -1;
            }
        }

        public string BinToAsc(string sValue)
        {
            try
            {
                int nLength = sValue.Length / 8;
                string sTemp = "";
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < nLength; i++)
                {
                    int nDec = 0;

                    nDec = Convert.ToInt32(sValue.Substring(i * 8, 8), 2);

                    sTemp = Convert.ToChar(nDec).ToString();
                    sb.Insert(0, sTemp);
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string BinToHex(string sValue)
        {
            try
            {
                string sTemp = "";
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < sValue.Length / 8; i++)
                {
                    int nDec = 0;


                    nDec = Convert.ToInt32(sValue.Substring(i * 8, 8), 2);

                    sTemp = Convert.ToString(nDec, 16);
                    sb.Append(Strings.UCase(sTemp.PadLeft(2, '0')));
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }

        }

        public string GetAddress(devTYPE type, int value)
        {
            try
            {
                string sAddress = "";
                StringBuilder sbuild = new StringBuilder();

                sAddress = Convert.ToString(Convert.ToInt32(value), 16);

                if (type == devTYPE.devB) sbuild.Append("B");
                else if (type == devTYPE.devW) sbuild.Append("W");

                sbuild.Append(Strings.UCase(sAddress.PadLeft(4, '0')));

                return sbuild.ToString();
            }
            catch
            {
                return "Invalid Address";
            }
        }

        public short GetAddressDec(string sAddress)
        {
            string sHex = sAddress.Substring(1);
            return (short)HexToDec(sHex);
        }

        public void SetLog(string sLog)
        {
            m_MelsecLog.TextOut(sLog);
        }

        public bool Monitor(short nNetNo, short nStNo)
        {
            if (!m_bOpen || m_Simulate || m_IsConfigurator) return false;

            short nRv = -1;

            // short	ret;	return value								OUT
            // long		path;	path of channel								IN
            // short	stno;	station number								IN
            // short	dev[ ];	random selected device						IN
            // short	buf[ ];	reading data(single precision)				OUT
            // short	bufsize;	bit count of reading data store area	IN
            short nStationNo = (short)(nStNo + (nNetNo << 8));

            int nLen = m_MonitorAddress.Count;

            int m_StartSize = 0;

            if (m_Simulate)
            {
                nRv = 0;
            }
            else
            {
                for (int i = 0; i < nLen; i++)
                {
                    // get address
                    tagADDR_INFO addressInfo = m_MonitorAddress[i];
                    // get word size
                    short nSize = (short)(m_nReadDataBuf[i].Length);
                    short nByteSize = (short)(nSize * 2);

                    nRv = mdreceive(m_nPath, nStationNo, (short)addressInfo.Type, addressInfo.StartAddress, ref nByteSize, ref m_nReadDataBuf[i][0]);

                    if (nRv == 0)
                    {
                        for (short j = 0; j < nSize; j++)
                        {
                            m_nReadData[m_StartSize + j] = m_nReadDataBuf[i][j];
                        }
                    }
                    else
                    {
                        m_MelsecLog.TextOut("Monitor Address : " + addressInfo.SetAddress + " Size : " + addressInfo.Size);
                    }

                    m_StartSize += nSize;

                    //if (addressInfo.Type == devTYPE.devB) m_StartSize += (short)((addressInfo.Size - 1) / 16 + 1);
                    //else if (addressInfo.Type == devTYPE.devW) m_StartSize += addressInfo.Size;
                }
            }

            return false;
        }

        // jemoon : 090312 추가함
        public void Uninitialize()
        {
            m_ThreadMelsec.Pause();

            Close();
        }


        #region Debug Function

        public string DebugReceiveBit(string address)
        {
            if (ReceiveBit(GetAddressDec(address)))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public bool DebugSendBit(short nDevNo, short nData)
        {
            // Find the address
            int nLen = m_MonitorAddress.Count;
            bool bReturn = false;
            short nValue = 0;
            int nIndex = 0;

            for (int i = 0; i < nLen; i++)
            {
                tagADDR_INFO temp = m_MonitorAddress[i];

                if (temp.Type == devTYPE.devB &&
                    (nDevNo >= temp.StartAddress && nDevNo < temp.StartAddress + temp.Size))
                {
                    if (nDevNo - temp.StartAddress >= 16)
                    {
                        nIndex += (nDevNo - temp.StartAddress) / 16;
                    }

                    // Send the bit data
                    nValue = (short)(1 << ((nDevNo - temp.StartAddress) % 16));

                    if (nData == 1)
                        m_nReadData[nIndex] = (short)(m_nReadData[nIndex] | nValue);
                    else
                        m_nReadData[nIndex] = (short)(m_nReadData[nIndex] & ~nValue);

                    bReturn = true;

                    return bReturn;
                }

                if (temp.Type == devTYPE.devB) nIndex += (temp.Size - 1) / 16 + 1;
                else if (temp.Type == devTYPE.devW) nIndex += temp.Size;
            }

            return bReturn;
        }

        public bool DebugSendBit(string address, string value, bool EventAccept)
        {
            int nValue;
            int nAddress;
            string sAddress = "";

            int count = value.Length;
            for (int i = 0; i < count; i++)
            {
                nAddress = 0;
                nAddress = GetAddressDec(address);
                nAddress = nAddress + i;

                sAddress = GetAddress(devTYPE.devB, nAddress);

                if (value.Substring(i, 1) == "1") nValue = 1;
                else nValue = 0;


                DebugSendBit((short)nAddress, (short)nValue);
            }
            return true;
        }


        public bool DebugSendWord(int nDevNo, int nData)
        {
            bool nRv = false;

            int nStartCount = 0;

            int monitorCount = m_MonitorAddress.Count;
            tagADDR_INFO addrInfo;
            for (int i = 0; i < monitorCount; i++)
            {
                addrInfo = m_MonitorAddress[i];
                if (addrInfo.Type == devTYPE.devW)
                {
                    if (nDevNo >= addrInfo.StartAddress &&
                        nDevNo < (addrInfo.StartAddress + addrInfo.Size))
                    {
                        int nRest = nDevNo - addrInfo.StartAddress;
                        m_nReadData[nStartCount + nRest] = (short)nData;

                        nRv = true;
                        break;
                    }

                }

                if (addrInfo.Type == devTYPE.devB) nStartCount += (addrInfo.Size - 1) / 16 + 1;
                else if (addrInfo.Type == devTYPE.devW) nStartCount += addrInfo.Size;

            }

            return nRv;

        }

        public bool DebugSendString(short nDevNo, string sStr, short nLen)
        {
            short nWord = (short)(nLen / 2 + nLen % 2);
            short[] nData = new short[nWord];
            bool nRv = false;

            for (int i = 0; i < nWord; i++)
            {
                char ch1, ch2;

                if (sStr.Length <= i * 2) ch1 = (char)0x20;
                else ch1 = Convert.ToChar(sStr.Substring(i * 2, 1));

                if (sStr.Length <= i * 2 + 1) ch2 = (char)0x20;
                else ch2 = Convert.ToChar(sStr.Substring(i * 2 + 1, 1));

                if (ch1 == ' ') ch1 = (char)0x20;	// Space는 그대로 ...........
                if (ch2 == ' ') ch2 = (char)0x20;
                if (ch1 == '\r') ch1 = (char)0x00;	// Space는 그대로 ...........
                if (ch2 == '\r') ch2 = (char)0x00;

                nData[i] = (short)((ch2 << 8) | ch1);
            }

            int nStartCount = 0;
            int monitorCount = m_MonitorAddress.Count;
            tagADDR_INFO addrInfo;
            for (int i = 0; i < monitorCount; i++)
            {
                addrInfo = m_MonitorAddress[i];
                if (addrInfo.Type == devTYPE.devW)
                {
                    if (nDevNo >= addrInfo.StartAddress &&
                        nDevNo < (addrInfo.StartAddress + addrInfo.Size))
                    {
                        int nRest = nDevNo - addrInfo.StartAddress;

                        for (int n = 0; n < nWord; n++)
                        {
                            m_nReadData[nStartCount + nRest + n] = (short)nData[n];
                        }

                        nRv = true;
                        break;
                    }

                }

                if (addrInfo.Type == devTYPE.devB) nStartCount += (addrInfo.Size - 1) / 16 + 1;
                else if (addrInfo.Type == devTYPE.devW) nStartCount += addrInfo.Size;

            }

            return nRv;
        }

        public bool DebugSendWords(int nDevNo, short[] nData)
        {
            bool nRv = false;

            int nStartCount = 0;
            int monitorCount = m_MonitorAddress.Count;
            tagADDR_INFO addrInfo;
            for (int i = 0; i < monitorCount; i++)
            {
                addrInfo = m_MonitorAddress[i];
                if (addrInfo.Type == devTYPE.devW)
                {
                    if (nDevNo >= addrInfo.StartAddress &&
                        nDevNo < (addrInfo.StartAddress + addrInfo.Size))
                    {
                        int nRest = nDevNo - addrInfo.StartAddress;

                        int dataLength = nData.Length;
                        for (int k = 0; k < dataLength; k++)
                        {
                            m_nReadData[nStartCount + nRest + k] = nData[k];
                        }

                        nRv = true;
                        break;
                    }

                }

                if (addrInfo.Type == devTYPE.devB) nStartCount += (addrInfo.Size - 1) / 16 + 1;
                else if (addrInfo.Type == devTYPE.devW) nStartCount += addrInfo.Size;
            }

            return nRv;

        }

        #endregion // for Debug Functions

        #endregion // for Methods

        #region DmsNode override
        public override bool Initialize()
        {
            if (!m_CreateMode) ReadConfiguration(m_Config, GetPathName());

            if (m_SimulateAddress)
            {
                this.MonitorAddress.Clear();

                // 512 * 32 = 3FFF;
                short size = 512;
                string startAddress;
                for (short i = 0; i < 32; i++)
                {
                    startAddress = string.Format("{0:X4}", size * i);
                    tagADDR_INFO addrInfo = new tagADDR_INFO(devTYPE.devB, startAddress, (short)(size * i), size);
                    this.MonitorAddress.Add(addrInfo);
                    addrInfo = new tagADDR_INFO(devTYPE.devW, startAddress, (short)(size * i), size);
                    this.MonitorAddress.Add(addrInfo);
                }
            }

            return base.Initialize();
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            Melsec mel;
            object obj = new object();
            if (dss.ReadXml(ref obj, typeof(Melsec), filename))
            {
                mel = (Melsec)obj;
                this.Simulate = mel.Simulate;
                this.Monitoring = mel.Monitoring;
                this.NetworkNo = mel.NetworkNo;
                this.StationNo = mel.StationNo;
                this.UpNetworkNo = mel.UpNetworkNo;
                this.UpStationNo = mel.UpStationNo;
                this.DnNetworkNo = mel.DnNetworkNo;
                this.DnStationNo = mel.DnStationNo;
                //              this.Mode = mel.Mode;
                this.MonitorAddress = mel.MonitorAddress;
                this.ProcessWorkingSetActivate = mel.ProcessWorkingSetActivate;
                this.ProcessMinWorkingSetSize = mel.ProcessMinWorkingSetSize;
                this.ProcessMaxWorkingSetSize = mel.ProcessMaxWorkingSetSize;
            }
        }

        public override void WriteConfiguration()
        {
            DmsSerializingService dss = new DmsSerializingService();
            dss.WriteXml(this, typeof(Melsec), GetPathName());
        }

        public void SetSimuateAddressMode(bool on)
        {
            m_SimulateAddress = on;
        }

        protected bool CheckPath(ref string path)
        {
            m_AppConfig.ReadXml();

            string filePath = m_AppConfig.MelsecConfigurationPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Melsec Configuration Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = Application.StartupPath;
                dlg.Description = "Melsec Configuraion Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.MelsecConfigurationPath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    path = filePath;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                path = filePath;
                return true;
            }
        }
        #endregion
    }
}
