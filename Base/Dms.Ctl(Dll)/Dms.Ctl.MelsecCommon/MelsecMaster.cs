///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.18
// Author       : jemoon
// Description  : Melsec Common Master
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Microsoft.VisualBasic;
using System.IO;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Dms.Util.IODefine;
using System.Diagnostics;
using System.Net.Sockets;


namespace Dms.Ctl
{
    public class MelsecMaster
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
        private bool m_Simualate = false;
        private bool m_IsOpened = false;
        private int m_nPath = 0;
        private short m_MelsecType = 0;
        private short m_nAlarmCode = 0;
        private string m_ChannelName = "";
        private short m_ChannelNo = 0;
        //by Youngsik... for test
        private static bool m_ProcessWorkingSetActivated = false;
        private MelsecEtherNetProtocol m_MelsecEtherNet = null;
        #endregion

        #region Properties
        public bool IsOpened
        {
            get
            {
                if (m_ChannelNo == (short)channel.melsecEthernet)
                {
                    m_IsOpened = m_MelsecEtherNet.IsOpened;
                }

                return m_IsOpened;
            }
        }
        public bool Simulate
        {
            get { return m_Simualate; }
            set { m_Simualate = value; }
        }
        public int Path
        {
            get { return m_nPath; }
        }
        public short AlarmCode
        {
            get { return m_nAlarmCode; }
        }
        public short ChannelNo
        {
            get { return m_ChannelNo; }
            set { m_ChannelNo = value; }
        }
        #endregion

        #region Constructor
        private MelsecMaster()
        {
            m_Simualate = AppConfig.Instance.Simul.Melsec;
        }

        private MelsecMaster(short channelNo, string ipAddress, ushort portNo, ProtocolType type)
        {
            m_Simualate = AppConfig.Instance.Simul.Melsec;
            m_ChannelNo = channelNo;

            if (m_ChannelNo == (short)channel.melsecEthernet)
            {
                m_MelsecEtherNet = new MelsecEtherNetProtocol(ipAddress, portNo, type);
            }
        }

        public MelsecMaster(IoNodeMelsec node)
            : this((short)node.ChannelNo, node.EtherNetIpAddress, node.EtherNetPortNo, node.EthenetProtocol)
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
            m_Simualate = AppConfig.Instance.Simul.Melsec;
            if (!m_Simualate && !m_ProcessWorkingSetActivated)
            {
                if (node.ProcessWorkingSetActivate)
                {
                    m_ProcessWorkingSetActivated = true;
                    XFunc.SetProcessWorkingSet(node.ProcessMinWorkingSetSize, node.ProcessMaxWorkingSetSize);
                }
            }
        }
        #endregion

        #region Event
        #endregion

        #region MdFunctions
        public bool Open()
        {
            return Open(m_ChannelNo, 0);
        }

        /// <summary>
        /// Melsec Master Open
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private bool Open(short ch, short mode)
        {
            short nRv;
            m_ChannelName = string.Format("MELSEC[{0}]", ((channel)ch).ToString());

            if (m_Simualate)
            {
                m_IsOpened = true;
                string msg = string.Format("{0} : run in simulation mode !", m_ChannelName);
                MessageBox.Show(msg);
            }
            else
            {
                // short	ret;		return value				OUT
                // long		*path;		opened loop path pointer
                // short	channel;	path of channel				IN
                // short	mode;		dummy(select -1)			IN
                if (ch == (short)channel.melsecEthernet)
                {
                    if (m_MelsecEtherNet.Open())
                    {
                        m_IsOpened = true;
                    }
                    else
                    {
                        string sError = string.Format("MelsecEtherNet Connection Error");
                        MessageBox.Show(sError);

                        m_IsOpened = false;
                    }
                }
                else
                {
                    if ((nRv = mdopen(ch, mode, ref m_nPath)) == 0)
                    {
                        // nRv == 0 : 성공
                        m_IsOpened = true;
                    }
                    else
                    {   // nRv != 0 : 실패
                        m_nAlarmCode = nRv;
                        string sError = string.Format("{0} : mdOpen error {1}", m_ChannelName, m_nAlarmCode.ToString());
                        MessageBox.Show(sError);

                        m_IsOpened = false;
                    }
                }
            }

            return m_IsOpened;
        }

        /// <summary>
        /// Melsec Master Colse
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            m_IsOpened = false;

            if (m_Simualate)
            {
                return true;
            }
            else
            {
                short nRv;

                if (m_ChannelNo == (short)channel.melsecEthernet)
                {
                    if (m_MelsecEtherNet.Close())
                    {
                        return true;
                    }
                    else
                    {
                        string sError = string.Format("MelsecEtherNet Close Error");
                        MessageBox.Show(sError);

                        return false;
                    }
                }
                else
                {
                    if ((nRv = mdclose(m_nPath)) == 0)
                    {
                        // nRv == 0 : 성공
                        return true;
                    }
                    else
                    {   // nRv != 0 : 실패
                        m_nAlarmCode = nRv;
                        string sError = string.Format("{0} : mdclose error {1}", m_ChannelName, m_nAlarmCode.ToString());
                        MessageBox.Show(sError);

                        return false;
                    }
                }

            }
        }

        public void Connect()
        {
            if (m_ChannelNo == (short)channel.melsecEthernet)
            {
                m_MelsecEtherNet.Connect();
            }
        }

        public bool DevSet(short networkNo, short stationNo, devTYPE type, int index)
        {
            // short	ret;	return value			OUT
            // long		path;	path of channel			IN
            // short	stno;	station number			IN
            // short	devtyp;	device type				IN
            // short	devno;	selected device number	IN
            short nRv = 0;
            if (index < 16 * 1024)  //MelsecH는 16k address까지 지원
            {
                short nStationNo = (short)(stationNo + (networkNo << 8));
                nRv = mddevset(m_nPath, nStationNo, (short)type, (short)index);
            }
            else
            {
                nRv = (short)mddevsetex(m_nPath, networkNo, stationNo, (int)type, index);
            }

            if (nRv == 0)
            {   //성공
                return true;
            }
            else
            {   //실패
                m_nAlarmCode = nRv;
                string sError = string.Format("{0} : DevSet error {1}", m_ChannelName, m_nAlarmCode.ToString());
                MessageBox.Show(sError);

                return false;
            }
        }

        public bool DevRst(short networkNo, short stationNo, devTYPE type, int index)
        {
            // short	ret;	return value			OUT
            // long		path;	path of channel			IN
            // short	stno;	station number			IN
            // short	devtyp;	device type				IN
            // short	devno;	selected device number	IN
            short nRv = 0;
            if (index < 16 * 1024)  //MelsecH는 16k address까지 지원
            {
                short nStationNo = (short)(stationNo + (networkNo << 8));
                nRv = mddevrst(m_nPath, nStationNo, (short)type, (short)index);
            }
            else
            {
                nRv = (short)mddevrstex(m_nPath, networkNo, stationNo, (int)type, index);
            }

            if (nRv == 0)
            {   //성공
                return true;
            }
            else
            {   //실패
                m_nAlarmCode = nRv;
                string sError = string.Format("{0} : DevRst error {1}", m_ChannelName, m_nAlarmCode.ToString());
                MessageBox.Show(sError);

                return false;
            }
        }

        public short MdReceive(short stationNo, short networkNo, devTYPE type, int index, ref short byteSize, ref short[] readData)
        {
            if (m_ChannelNo == (short)channel.melsecEthernet)
            {
                try
                {
                    if (m_MelsecEtherNet.IsOpened)
                    {
                        return (short)m_MelsecEtherNet.BlockReadDevice(type, index, ref byteSize, ref readData);
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception err)
                {
                    string sError = err.ToString();
                    return 1;
                }
            }
            else
            {
                if (index < 16 * 1024)  //MelsecH는 16k address까지 지원
                {
                    short stationNoNew = (short)(stationNo + (networkNo << 8));
                    return mdreceive(m_nPath, stationNoNew, (short)type, (short)index, ref byteSize, ref readData[0]);
                }
                else
                {
                    int size = byteSize;
                    return (short)mdreceiveex(m_nPath, networkNo, stationNo, (int)type, index, ref size, ref readData[0]);
                }
            }
        }

        public bool MdReceiveBit(short stationNo, short networkNo, devTYPE type, int index)
        {
            // address must be muliple of 8
            short[] readData = new short[1];
            short byteSize = 1;
            bool val = false;

            int address = (index / 8) * 8;
            int offset = (index % 8);
            short rv = MdReceive(stationNo, networkNo, type, address, ref byteSize, ref readData);
            if (rv == 0)
            {
                val = ((readData[0] >> offset) & (short)0x01) > 0;
            }

            return val;
        }

        public short MdSend(short stationNo, short networkNo, devTYPE type, int index, ref short byteSize, ref short[] writeData)
        {
            if (m_ChannelNo == (short)channel.melsecEthernet)
            {
                try
                {
                    if (m_MelsecEtherNet.IsOpened)
                    {
                        return (short)m_MelsecEtherNet.BlockWriteDevice(type, index, ref byteSize, ref writeData);
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception err)
                {
                    string sError = err.ToString();
                    return 1;
                }
            }
            else
            {
                if (index < 16 * 1024)  //MelsecH는 16k address까지 지원
                {
                    short stationNoNew = (short)(stationNo + (networkNo << 8));
                    return mdsend(m_nPath, stationNoNew, (short)type, (short)index, ref byteSize, ref writeData[0]);
                }
                else
                {
                    int size = byteSize;
                    return (short)mdsendex(m_nPath, networkNo, stationNo, (int)type, index, ref size, ref writeData[0]);
                }
            }
        }

        public bool MdSendBit(short stationNo, short networkNo, devTYPE type, int index, bool state)
        {
            bool rv = false;
            if (state)
            {
                rv = DevSet(networkNo, stationNo, type, index);
            }
            else
            {
                rv = DevRst(networkNo, stationNo, type, index);
            }
            return rv;
        }

        //public short MdRandReceive(short stationNo, short networkNo, ref devTYPE[] devTypes, ref int[] indexes, ref short[] byteSize, ref short[] readData)
        //{
        //    if (m_ChannelName == (short)channel.melsecEthernet)
        //    {
        //        try
        //        {
        //            if (m_MelsecEtherNet.IsOpened)
        //            {
        //                return m_MelsecEtherNet.RandomReadDevice(devTypes, indexes, byteSize, ref readData);
        //            }
        //            else
        //            {
        //                return 1;
        //            }
        //        }
        //        catch (Exception err)
        //        {
        //            string error = err.ToString();
        //            return 1;
        //        }
        //    }
        //    else
        //    {

        //    }

        //    return 0;
        //}

        /// <summary>
        /// 현재는 size는 의미 없음
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="networkNo"></param>
        /// <param name="devTypes"></param>
        /// <param name="indexes"></param>
        /// <param name="size"></param>
        /// <param name="writeData"></param>
        /// <returns></returns>

        public short MdBitRandSend(short stationNo, short networkNo, ref devTYPE[] devTypes, ref int[] indexes, ref short[] size, ref short[] writeData)
        {
            if (m_ChannelNo == (short)channel.melsecEthernet)
            {
                try
                {
                    if (m_MelsecEtherNet.IsOpened)
                    {
                        return m_MelsecEtherNet.RandomWriteBitDevice(devTypes, indexes, size, writeData);
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception err)
                {
                    string error = err.ToString();
                    return 1;
                }
            }
            else
            {

            }

            return 0;
        }

        /// <summary>
        /// 현재는 size는 의미 없음
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="networkNo"></param>
        /// <param name="devTypes"></param>
        /// <param name="indexes"></param>
        /// <param name="size"></param>
        /// <param name="writeData"></param>
        /// <returns></returns>
        public short MdWordRandSend(short stationNo, short networkNo, ref devTYPE[] devTypes, ref int[] indexes, ref short[] size, ref short[] writeData)
        {
            if (m_ChannelNo == (short)channel.melsecEthernet)
            {
                try
                {
                    if (m_MelsecEtherNet.IsOpened)
                    {
                        return m_MelsecEtherNet.RandomWriteWordDevice(devTypes, indexes, size, writeData);
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception err)
                {
                    string error = err.ToString();
                    return 1;
                }
            }
            else
            {

            }

            return 0;
        }

        public bool SetControlMode(short nNetNo, short nStNo, short nControlMode)
        {
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
            short nRv = mdcontrol(m_nPath, nStationNo, nControlMode);
            if (nRv == 0)
            {   //성공
                return true;
            }
            else
            {   //실패
                m_nAlarmCode = nRv;
                string sError = string.Format("{0} : mdcontrol error {1}", m_ChannelName, m_nAlarmCode.ToString());
                MessageBox.Show(sError);

                return false;
            }
        }

        public bool TypeRead(short nNetNo, short nStNo)
        {

            // short	ret;	return value		OUT
            // long		path;	path of channel		IN
            // short	stno;	station number		IN
            // short	*buf;	format name code	OUT
            short nStationNo = (short)(nStNo + (nNetNo << 8));
            short nRv = mdtyperead(m_nPath, nStationNo, ref m_MelsecType);

            if (nRv == 0)
            {   //성공
                return true;
            }
            else
            {   //실패
                m_nAlarmCode = nRv;
                string sError = string.Format("{0} : mdtyperead error {1}", m_ChannelName, m_nAlarmCode.ToString());
                MessageBox.Show(sError);

                return false;
            }
        }
        #endregion

        #region Methods

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
        #endregion
    }
}
