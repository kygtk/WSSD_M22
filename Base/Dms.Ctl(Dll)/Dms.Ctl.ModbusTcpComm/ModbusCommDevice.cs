using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.ComponentModel;
using Dms.Common;
using Dms.Ctl;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.07.09
// Author       : Kim Youngsik
// Description  : MODBUS TCP for Modbus communication
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class ModbusCommDevice
    {
        #region Fields
        //List of Remote Server Machine
        private List<ModbusCommServer> m_ModbusServer = new List<ModbusCommServer>();
        //List of Remote Client Machine
        private List<ModbusCommClient> m_ModbusClient = new List<ModbusCommClient>();
        private List<ModbusTcpClient> m_Clients = new List<ModbusTcpClient>();
        private List<ModbusEqpInfo> m_Machines = new List<ModbusEqpInfo>();
        private ModbusTcpListener m_ModbusListener = null;
        private List<string> m_Errors = new List<string>();
        private int m_Count = 0;
        private int m_ClientNo = 0;
        private XLog m_ServerLog = null;
        private XLog m_ClientLog = null;
        private XLog m_ModbusCommLog = null;
        #endregion

        #region Properties
        [Browsable(false)]
        public List<ModbusCommServer> Server
        {
            get { return m_ModbusServer; }
            set { m_ModbusServer = value; }
        }
        [Browsable(false)]
        public List<ModbusCommClient> Client
        {
            get { return m_ModbusClient; }
            set { m_ModbusClient = value; }
        }
        [Browsable(false)]
        public int Count
        {
            get { return m_Count; }
        }
        #endregion

        #region Constructor
        public static readonly ModbusCommDevice Instance = new ModbusCommDevice();

        public ModbusCommDevice()
        {

        }
        #endregion

        #region Communication Setting Method
        public bool AddModbusEqp(ModbusEqpInfo eqp)
        {
            string logName = "";

            if (m_ModbusCommLog == null)
            {
                m_ModbusCommLog = new XLog("ModbusCommLog", XLog.LogStampType.UseStamp);
            }

            if( FindDuplicatedUnit(eqp) == false) return false;

            if (eqp.ModbusType == ModbusType.Server)
            {
                //Remote Machine이 Server인 경우...
                if (m_ServerLog == null)
                {
                    logName = string.Format("ModbusTcpComm_Server");
                    m_ServerLog = new XLog(logName, XLog.LogStampType.UseStamp);
                }

                string logText = "";
                logText = string.Format("====================================================================");
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("ModbusEqp is created as ModbusServer type.");
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Unit ID : {0}", eqp.UnitID);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Unit Name : {0}", eqp.UnitName);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     IP Address : {0}", eqp.IPAddress);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Bit Input Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.InputBitOffset, eqp.InputBitLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Bit Output Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.OutputBitOffset, eqp.OutputBitLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Word Input Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.InputWordOffset, eqp.InputWordLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Word Output Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.OutputWordOffset, eqp.OutputWordLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("====================================================================");
                m_ModbusCommLog.TextOut(logText);

                m_ModbusServer.Add(new ModbusCommServer(20, m_ServerLog, eqp));

                int no = m_ModbusServer.Count - 1;
                m_ModbusServer[no].Start();

                return true;
            }
            else if (eqp.ModbusType == ModbusType.Client)
            {
                //Remote Machine이 Client인 경우...
                if (m_ClientLog == null)
                {
                    logName = string.Format("ModbusTcpComm_Client");
                    m_ClientLog = new XLog(logName, XLog.LogStampType.UseStamp);
                }

                string logText = "";
                logText = string.Format("====================================================================");
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("ModbusEqp is created as ModbusClient type.");
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Unit ID : {0}", eqp.UnitID);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Unit Name : {0}", eqp.UnitName);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     IP Address : {0}", eqp.IPAddress);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Bit Input Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.InputBitOffset, eqp.InputBitLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Bit Output Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.OutputBitOffset, eqp.OutputBitLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Word Input Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.InputWordOffset, eqp.InputWordLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("     Word Output Configuration : Offset(0X{0:X4}), Length(0X{1:X4})", eqp.OutputWordOffset, eqp.OutputWordLength);
                m_ModbusCommLog.TextOut(logText);
                logText = string.Format("====================================================================");
                m_ModbusCommLog.TextOut(logText);

                m_ModbusClient.Add(new ModbusCommClient(m_ClientLog, eqp));

                if (m_ModbusListener == null)
                {
                    m_ModbusListener = new ModbusTcpListener(20);
                    m_ModbusListener.Start();
                }

                m_ModbusListener.AddClient(m_ModbusClient[m_ClientNo].Client);
                m_ClientNo++;

                return true;
            }

            return false;
        }
        #endregion

        #region Methods
        private bool FindDuplicatedUnit(ModbusEqpInfo eqp)
        {
            int count = m_Machines.Count;

            for( int i = 0 ; i < count ; i++)
            {
                if (m_Machines[i].UnitID == eqp.UnitID)
                {
                    string logText = string.Format("{0} and {1} have same Unit ID {2}. Check Configuration.", m_Machines[i].UnitName, eqp.UnitName, eqp.UnitID);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
            }

            m_Machines.Add(eqp);

            return true;
        }

        public ModbusCommServer FindServer(int unitID)
        {
            int count = m_ModbusServer.Count;

            if (count <= 0) return null;

            for (int i = 0; i < count; i++)
            {
                if (unitID == m_ModbusServer[i].UnitID)
                {
                    return m_ModbusServer[i];
                }
            }

            return null;
        }

        public ModbusCommClient FindClient(int unitID)
        {
            int count = m_ModbusClient.Count;

            if (count <= 0) return null;

            for (int i = 0; i < count; i++)
            {
                if (unitID == m_ModbusClient[i].UnitID)
                {
                    return m_ModbusClient[i];
                }
            }

            return null;
        }

        public bool GetConnection(int unitID)
        {
            ModbusCommServer server = FindServer(unitID);

            if (server != null)
            {
                return server.Connected;
            }

            ModbusCommClient client = FindClient(unitID);

            if (client != null)
            {
                return client.Connected;
            }

            return false;
        }

        public int GetScanTime(int unitID)
        {
            ModbusCommServer server = FindServer(unitID);

            if (server != null)
            {
                return server.UpdateInterval;
            }

            ModbusCommClient client = FindClient(unitID);

            if (client != null)
            {
                return 0;
            }

            return 0;
        }

        private void FindError(params object[] data)
        {
            string type = (string)data[0];
            int unitID = (int)data[1];
            ushort address = (ushort)data[2];

            string err = string.Format("{0} {1} {2}", type, unitID, address);

            int count = m_Errors.Count;

            for (int i = 0; i < count; i++)
            {
                if (err == m_Errors[i]) return;
            }

            string logText = string.Format("{0} - Unit ID({1}), Address(0X{2:X4})", type, unitID, address);
            m_ModbusCommLog.TextOut(logText);
            m_Errors.Add(err);
        }

        public bool GetInputBit(int unitID, ushort refAddress)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.InputBitOffset + server.AddressConfig.InputBitLength))
                {
                    logText = string.Format("GetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputBitOffset
                                            , server.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < server.AddressConfig.InputBitOffset)
                {
                    logText = string.Format("GetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputBitOffset
                                            , server.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                return server.IB[refAddress - server.AddressConfig.InputBitOffset];
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.InputBitOffset + client.AddressConfig.InputBitLength))
                {
                    logText = string.Format("GetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputBitOffset
                                            , client.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < client.AddressConfig.InputBitOffset)
                {
                    logText = string.Format("GetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputBitOffset
                                            , client.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (client.Connected == false)
                {
                    return false;
                }

                return client.IB[refAddress - client.AddressConfig.InputBitOffset];
            }

            FindError("GIB", unitID, refAddress);

            return false;
        }

        public bool SetInputBit(int unitID, ushort refAddress, bool state)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.InputBitOffset + server.AddressConfig.InputBitLength))
                {
                    logText = string.Format("SetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputBitOffset
                                            , server.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < server.AddressConfig.InputBitOffset)
                {
                    logText = string.Format("SetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputBitOffset
                                            , server.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                //server.WriteBit(refAddress, state);
                server.IB[refAddress - server.AddressConfig.InputBitOffset] = state;
                return true;
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.InputBitOffset + client.AddressConfig.InputBitLength))
                {
                    logText = string.Format("SetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputBitOffset
                                            , client.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < client.AddressConfig.InputBitOffset)
                {
                    logText = string.Format("SetInputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputBitOffset
                                            , client.AddressConfig.InputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                client.IB[refAddress - client.AddressConfig.InputBitOffset] = state;

                return true;
            }

            FindError("SIB", unitID, refAddress);

            return false;
        }

        public bool GetOutputBit(int unitID, ushort refAddress)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.OutputBitOffset + server.AddressConfig.OutputBitLength))
                {
                    logText = string.Format("GetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputBitOffset
                                            , server.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < server.AddressConfig.OutputBitOffset)
                {
                    logText = string.Format("GetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputBitOffset
                                            , server.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                return server.OB[refAddress - server.AddressConfig.OutputBitOffset];
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.OutputBitOffset + client.AddressConfig.OutputBitLength))
                {
                    logText = string.Format("GetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputBitOffset
                                            , client.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < client.AddressConfig.OutputBitOffset)
                {
                    logText = string.Format("GetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputBitOffset
                                            , client.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                return client.OB[refAddress - client.AddressConfig.OutputBitOffset];
            }

            FindError("GOB", unitID, refAddress);

            return false;
        }

        public bool SetOutputBit(int unitID, ushort refAddress, bool state)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.OutputBitOffset + server.AddressConfig.OutputBitLength))
                {
                    logText = string.Format("SetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputBitOffset
                                            , server.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < server.AddressConfig.OutputBitOffset)
                {
                    logText = string.Format("SetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputBitOffset
                                            , server.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                server.OB[refAddress - server.AddressConfig.OutputBitOffset] = state;
                return true;
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.OutputBitOffset + client.AddressConfig.OutputBitLength))
                {
                    logText = string.Format("SetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputBitOffset
                                            , client.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < client.AddressConfig.OutputBitOffset)
                {
                    logText = string.Format("SetOutputBit - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputBitOffset
                                            , client.AddressConfig.OutputBitLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                client.OB[refAddress - client.AddressConfig.OutputBitOffset] = state;

                return true;
            }

            FindError("SOB", unitID, refAddress);

            return false;
        }

        public short GetInputWord(int unitID, ushort refAddress)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.InputWordOffset + server.AddressConfig.InputWordLength))
                {
                    logText = string.Format("GetInputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputWordOffset
                                            , server.AddressConfig.InputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }
                else if (refAddress < server.AddressConfig.InputWordOffset)
                {
                    logText = string.Format("GetInputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputWordOffset
                                            , server.AddressConfig.InputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }

                return server.IW[refAddress - server.AddressConfig.InputWordOffset];
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.InputWordOffset + client.AddressConfig.InputWordLength))
                {
                    logText = string.Format("GetInputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputWordOffset
                                            , client.AddressConfig.InputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }
                else if (refAddress < client.AddressConfig.InputWordOffset)
                {
                    logText = string.Format("GetInputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputWordOffset
                                            , client.AddressConfig.InputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }
                else if (client.Connected == false)
                {
                    return 0;
                }

                return client.IW[refAddress - client.AddressConfig.InputWordOffset];
            
            }

            FindError("GIW", unitID, refAddress);

            return 0;
        }

        public short[] GetInputWords(int unitID, ushort refAddress, ushort refCount)
        {
            string logText = "";
            ModbusCommServer server = FindServer(unitID);

            short[] rv = new short[refCount];

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.InputWordOffset + server.AddressConfig.InputWordLength))
                {
                    logText = string.Format("GetInputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputWordOffset
                                            , server.AddressConfig.InputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }
                else if (refAddress < server.AddressConfig.InputWordOffset)
                {
                    logText = string.Format("GetInputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , server.AddressConfig.InputWordOffset
                                            , server.AddressConfig.InputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }

                for (int i = 0; i < refCount; i++)
                {
                    rv[i] = server.IW[refAddress + i - server.AddressConfig.InputWordOffset];
                }

                return rv;
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.InputWordOffset + client.AddressConfig.InputWordLength))
                {
                    logText = string.Format("GetInputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputWordOffset
                                            , client.AddressConfig.InputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }
                else if (refAddress < client.AddressConfig.InputWordOffset)
                {
                    logText = string.Format("GetInputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , client.AddressConfig.InputWordOffset
                                            , client.AddressConfig.InputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }
                else if (client.Connected == false)
                {
                    return rv;
                }

                for (int i = 0; i < refCount; i++)
                {
                    rv[i] = client.IW[refAddress + i - client.AddressConfig.InputWordOffset];
                }

                return rv;
            }

            FindError("GIWs", unitID, refAddress);

            return rv;
        }

        public string GetInputString(int unitID, ushort refAddress, ushort refCount)
        {
            string rv = "";
            short[] data = new short[refCount];
            data = GetInputWords(unitID, refAddress, refCount);

            for (int i = 0; i < refCount; i++)
            {
                short wData = data[i];

                char ch;
                ch = (char)(wData & 0x00FF);
                if (ch == 0) ch = ' ';
                rv += ch;
                ch = (char)((wData >> 8) & 0x00FF);
                if (ch == 0) ch = ' ';
                rv += ch;
            }

            return rv;
        }

        public short GetOutputWord(int unitID, ushort refAddress)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.OutputWordOffset + server.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("GetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }
                else if (refAddress < server.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("GetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }

                return server.OW[refAddress - server.AddressConfig.OutputWordOffset];
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.OutputWordOffset + client.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("GetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }
                else if (refAddress < client.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("GetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return 0;
                }

                return client.OW[refAddress - client.AddressConfig.OutputWordOffset];
            }

            FindError("GOW", unitID, refAddress);

            return 0;
        }

        public short[] GetOutputWords(int unitID, ushort refAddress, ushort refCount)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            short[] rv = new short[refCount];

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.OutputWordOffset + server.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("GetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }
                else if (refAddress < server.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("GetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }

                for (int i = 0; i < refCount; i++)
                {
                    rv[i] = server.OW[refAddress + i - server.AddressConfig.OutputWordOffset];
                }

                return rv;
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.OutputWordOffset + client.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("GetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }
                else if (refAddress < client.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("GetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return rv;
                }

                for (int i = 0; i < refCount; i++)
                {
                    rv[i] = client.OW[refAddress + i - client.AddressConfig.OutputWordOffset];
                }

                return rv;
            }

            FindError("GOWs", unitID, refAddress);

            return rv;
        }

        public string GetOutputString(int unitID, ushort refAddress, ushort refCount)
        {
            string rv = "";
            short[] data = new short[refCount];
            data = GetOutputWords(unitID, refAddress, refCount);

            for (int i = 0; i < refCount; i++)
            {
                short wData = data[i];

                char ch;
                ch = (char)(wData & 0x00FF);
                if (ch == 0) ch = ' ';
                rv += ch;
                ch = (char)((wData >> 8) & 0x00FF);
                if (ch == 0) ch = ' ';
                rv += ch;
            }

            return rv;
        }

        public bool SetOutputWord(int unitID, ushort refAddress, short value)
        {
            string logText = "";

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.OutputWordOffset + server.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("SetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < server.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("SetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                server.OW[refAddress - server.AddressConfig.OutputWordOffset] = value;
                return true;
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.OutputWordOffset + client.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("SetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < client.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("SetOutputWord - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                client.OW[refAddress - client.AddressConfig.OutputWordOffset] = value;
                return true;
            }

            FindError("SOW", unitID, refAddress);

            return false;
        }

        public bool SetOutputWords(int unitID, ushort refAddress, ushort refCount, short[] value)
        {
            string logText = "";
            if (refCount > 124) return false;

            ModbusCommServer server = FindServer(unitID);

            //unitID에 해당하는 Remote Machine이 Server일 경우
            if (server != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (server.AddressConfig.OutputWordOffset + server.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("SetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , server.AddressConfig.OutputWordOffset
                                            , server.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < server.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("SetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                               , unitID
                                               , server.AddressConfig.OutputWordOffset
                                               , server.AddressConfig.OutputWordLength
                                               , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                for (int i = 0; i < refCount; i++)
                {
                    server.OW[refAddress + i - server.AddressConfig.OutputWordOffset] = value[i];
                }

                return true;
            }

            ModbusCommClient client = FindClient(unitID);

            //unitID에 해당하는 Remote Machine이 Client일 경우
            if (client != null)
            {
                //refAddress가 Out of range일 경우
                if (refAddress > (client.AddressConfig.OutputWordOffset + client.AddressConfig.OutputWordLength))
                {
                    logText = string.Format("SetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }
                else if (refAddress < client.AddressConfig.OutputWordOffset)
                {
                    logText = string.Format("SetOutputWords - UNIT ID({0}) - [Start Address({1:X4})   Length({2:X4})   Reference Address({3:X4})   Reference Length({4:X4})]"
                                            , unitID
                                            , client.AddressConfig.OutputWordOffset
                                            , client.AddressConfig.OutputWordLength
                                            , refAddress, refCount);
                    m_ModbusCommLog.TextOut(logText);
                    return false;
                }

                for (int i = 0; i < refCount; i++)
                {
                    client.OW[refAddress + i - client.AddressConfig.OutputWordOffset] = value[i];
                }

                return true;
            }

            FindError("SOWs", unitID, refAddress);

            return false;
        }

        public bool SetOutputString(int unitID, ushort refAddress, ushort refCount, string value)
        {
            short[] wData = new short[refCount];

            for (int i = 0; i < refCount; i++)
            {
                char ch1, ch2;

                if (value.Length <= i * 2) ch1 = (char)0x20;
                else ch1 = Convert.ToChar(value.Substring(i * 2, 1));

                if (value.Length <= i * 2 + 1) ch2 = (char)0x20;
                else ch2 = Convert.ToChar(value.Substring(i * 2 + 1, 1));

                if (ch1 == ' ') ch1 = (char)0x20;
                if (ch2 == ' ') ch2 = (char)0x20;
                if (ch1 == '\r') ch1 = (char)0x00;
                if (ch2 == '\r') ch2 = (char)0x00;

                wData[i] = (short)((ch2 << 8) | ch1);
            }

            return SetOutputWords(unitID, refAddress, refCount, wData);
        }
        #endregion
    }
}

