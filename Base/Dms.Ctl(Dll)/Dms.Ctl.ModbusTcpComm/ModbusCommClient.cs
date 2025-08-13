using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dms.Common;
using Dms.Ctl;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.31
// Author       : Kim Youngsik
// Description  : Modbus Communication Device Client(Remote Machine)
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public enum DataType
    {
        InputBit,
        OutputBit,
        InputWord,
        OutputWord,
    }

    public class ModbusCommClient
    {
        #region Fields
        private static object m_LockKey = new object();
        private ModbusTcpClient m_ModbusClient = null;
        private string m_UnitName;
        private byte m_UnitID;
        private XLog m_Log;
        private AddressConfig m_AddressConfig = new AddressConfig();

        private bool[] m_InputBits;
        private bool[] m_OutputBits;
        private short[] m_InputWords;
        private short[] m_OutputWords;
        #endregion

        #region Properties
        public ModbusTcpClient Client
        {
            get { return m_ModbusClient; }
            set { m_ModbusClient = value; }
        }
        public bool[] IB
        {
            get { return m_InputBits; }
            set { m_InputBits = value; }
        }
        public bool[] OB
        {
            get { return m_OutputBits; }
            set { m_OutputBits = value; }
        }
        public short[] IW
        {
            get { return m_InputWords; }
            set { m_InputWords = value; }
        }
        public short[] OW
        {
            get { return m_OutputWords; }
            set { m_OutputWords = value; }
        }
        public AddressConfig AddressConfig
        {
            get { return m_AddressConfig; }
            set { m_AddressConfig = value; }
        }
        public byte UnitID
        {
            get { return m_UnitID; }
            set { m_UnitID = value; }
        }
        public bool Connected
        {
            get { return m_ModbusClient.Connected; }
        }
        #endregion

        #region Constructor
        public ModbusCommClient(XLog log, ModbusEqpInfo info)
        {
            m_Log = log;

            m_ModbusClient = new ModbusTcpClient(10, info.UnitID, info.UnitName, info.IPAddress, m_Log);
            m_ModbusClient.Start();

            m_UnitName = info.UnitName;
            m_UnitID = info.UnitID;

            m_AddressConfig.InputBitOffset = info.InputBitOffset;
            m_AddressConfig.OutputBitOffset = info.OutputBitOffset;
            m_AddressConfig.InputWordOffset = info.InputWordOffset;
            m_AddressConfig.OutputWordOffset = info.OutputWordOffset;

            m_AddressConfig.InputBitLength = info.InputBitLength;
            m_AddressConfig.OutputBitLength = info.OutputBitLength;
            m_AddressConfig.InputWordLength = info.InputWordLength;
            m_AddressConfig.OutputWordLength = info.OutputWordLength;

            m_InputBits = new bool[m_AddressConfig.InputBitLength];
            m_OutputBits = new bool[m_AddressConfig.OutputBitLength];
            m_InputWords = new short[m_AddressConfig.InputWordLength];
            m_OutputWords = new short[m_AddressConfig.OutputWordLength];

            m_ModbusClient.OnReceiveReqMsg += new ModbusTcpClient.ReceiveRequestMessage(OnMessageReceived);
            m_ModbusClient.OnDisconnect += new ModbusTcpClient.ConnectionClosed(OnDisconnect);
        }
        #endregion

        #region Methods
        public void SetLog(string text)
        {
            string logText = "";

            logText = string.Format("[UNIT : {0}-{1}]{2}{3}", m_UnitID, m_UnitName, '\t', text);
            m_Log.TextOut(logText);
        }

        //2010.04.09 Youngsik... Connection이 종료 될 때... Input Bit/Word를 초기화 한다.
        private void ClearInputData()
        {
            for (int i = 0; i < m_AddressConfig.InputBitLength; i++)
            {
                m_InputBits[i] = false;
            }

            for (int i = 0; i < m_AddressConfig.InputWordLength; i++)
            {
                m_InputWords[i] = 0;
            }
        }

        //2010.04.09 Youngsik... Connection이 종료 될 때...
        private void OnDisconnect()
        {
            ClearInputData();
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : CheckMesssageValidation(DataType type, ushort address, ushort count)
        // Description : Client로 부터 받은 Request Message에서 요구하는 Data의 Address와 Length의 적합성 판단.
        //               요청한 Data의 Address가 설정 범위를 벗어나거나 요청한 Count가 최대 Count를 넘기면
        //               Response Message 대신 Exception Message를 보낸다.
        //
        // 1. Input Bit는 최대 길이 800 까지    ==> ForceMultipleCoils
        // 2. Output Bit는 최대 길이 2000 까지  ==> ReadMultipleCoils
        // 3. Input Word는 최대 길이 100 까지   ==> WriteMultipleRegisters
        // 4. Output Word는 최대 길이 125까지   ==> ReadMultipleRegisters
        ///////////////////////////////////////////////////////////////////////////
        private byte CheckMessageValidation(DataType type, ushort address, ushort count)
        {
            switch (type)
            {
                case DataType.InputBit:
                    {
                        ushort start = m_AddressConfig.InputBitOffset;
                        ushort end = (ushort)(m_AddressConfig.InputBitOffset + m_AddressConfig.InputBitLength);

                        if (((address < start) || (address > end)) || (count > 800))
                        {
                            return (byte)ExceptionCodes.IllegalDataAddress;
                        }
                    }
                    break;
                case DataType.OutputBit:
                    {
                        ushort start = m_AddressConfig.OutputBitOffset;
                        ushort end = (ushort)(m_AddressConfig.OutputBitOffset + m_AddressConfig.OutputBitLength);

                        if ((address < start) || (address > end) || (count > 2000))
                        {
                            return (byte)ExceptionCodes.IllegalDataAddress;
                        }
                    }
                    break;
                case DataType.InputWord:
                    {
                        ushort start = m_AddressConfig.InputWordOffset;
                        ushort end = (ushort)(m_AddressConfig.InputWordOffset + m_AddressConfig.InputWordOffset);

                        if (((address < start) || (address > end)) || (count > 100))
                        {
                            return (byte)ExceptionCodes.IllegalDataAddress;
                        }
                    }
                    break;
                case DataType.OutputWord:
                    {
                        ushort start = m_AddressConfig.OutputWordOffset;
                        ushort end = (ushort)(m_AddressConfig.OutputWordOffset + m_AddressConfig.OutputWordOffset);

                        if (((address < start) || (address > end)) || (count > 125))
                        {
                            return (byte)ExceptionCodes.IllegalDataAddress;
                        }
                    }
                    break;
            }

            return 0;
        }

        public void OnMessageReceived(byte[] reqMsg)
        {
            lock(m_LockKey)
            {
                ushort transactionID = (ushort)(reqMsg[0] * 0x100 + reqMsg[1]);
                byte functionCode = reqMsg[7];
                ushort length = (ushort)(reqMsg[4] * 0x100 + reqMsg[5]);
                byte unitID = reqMsg[6];

                byte[] data = new byte[length-2];

                for(int i = 0; i<data.Length ; i++)
                {
                    data[i] = reqMsg[i+8];
                }

                ProcessMessage(transactionID, unitID, functionCode, data);
            }
        }

        public void ProcessMessage(ushort transactionID, byte unitID, byte functionCode, byte[] data)
        {
            switch (functionCode)
            {
                case (byte)FunctionCodes.ReadCoils:
                    {
                        Process_ReadCoils(transactionID, unitID, data);
                    }
                    break;
                case (byte)FunctionCodes.ReadInputDiscretes:
                    {
                        m_ModbusClient.Response_Exception(transactionID, unitID, functionCode, (byte)ExceptionCodes.IllegalFunction);
                    }
                    break;
                case (byte)FunctionCodes.ReadMultipleRegisters:
                    {
                        Process_ReadMultipleRegisters(transactionID, unitID, data);
                    }
                    break;
                case (byte)FunctionCodes.ReadInputRegisters:
                    {
                        m_ModbusClient.Response_Exception(transactionID, unitID, functionCode, (byte)ExceptionCodes.IllegalFunction);
                    }
                    break;
                case (byte)FunctionCodes.WriteCoils:
                    {
                        Process_WriteCoils(transactionID, unitID, data);
                    }
                    break;
                case (byte)FunctionCodes.WriteSingleRegister:
                    {
                        Process_WriteSingleRegister(transactionID, unitID, data);
                    }
                    break;
                case (byte)FunctionCodes.WriteMultipleRegisters:
                    {
                        Process_WriteMultipleRegisters(transactionID, unitID, data);
                    }
                    break;
                case (byte)FunctionCodes.ForceMultipleCoils:
                    {
                        Process_WriteMultipleCoils(transactionID, unitID, data);
                    }
                    break;
                default:
                    {
                        m_ModbusClient.Response_Exception(transactionID, unitID, functionCode, (byte)ExceptionCodes.IllegalFunction);
                    }
                    break;
            }
        }

        private void Process_ReadCoils(ushort transactionID, byte unitID, byte[] data)
        {
            if (data.Length == 4)
            {
                ushort refAddress = (ushort)(data[0] * 0x100 + data[1]);
                ushort refCount = (ushort)(data[2] * 0x100 + data[3]);

                byte exceptionCode = CheckMessageValidation(DataType.OutputBit, refAddress, refCount);

                if (exceptionCode == 0)
                {
                    bool[] value = new bool[refCount];

                    for (int i = 0; i < refCount; i++)
                    {
                        value[i] = m_OutputBits[refAddress - m_AddressConfig.OutputBitOffset + i];
                    }

                    m_ModbusClient.Response_ReadCoils(transactionID, unitID, refCount, value);
                }
                else
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.ReadCoils, exceptionCode);
                }
            }
            else
            {
                m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.ReadCoils, (byte)ExceptionCodes.IllegalResponseLength);
            }
        }

        private void Process_ReadMultipleRegisters(ushort transactionID, byte unitID, byte[] data)
        {
            if (data.Length == 4)
            {
                ushort refAddress = (ushort)(data[0] * 0x100 + data[1]);
                ushort refCount = (ushort)(data[2] * 0x100 + data[3]);

                byte exceptionCode = CheckMessageValidation(DataType.OutputWord, refAddress, refCount);

                if (exceptionCode == 0)
                {
                    short[] value = new short[refCount];

                    for (int i = 0; i < refCount; i++)
                    {
                        value[i] = m_OutputWords[refAddress - m_AddressConfig.OutputWordOffset + i];
                    }

                    m_ModbusClient.Response_ReadMultipleRegisters(transactionID, unitID, refCount, value);
                }
                else
                {
                    m_ModbusClient.Response_Exception(transactionID, UnitID, (byte)FunctionCodes.ReadMultipleRegisters, exceptionCode);
                }
            }
            else
            {
                m_ModbusClient.Response_Exception(transactionID, UnitID, (byte)FunctionCodes.ReadMultipleRegisters, (byte)ExceptionCodes.IllegalResponseLength);
            }
        }

        private void Process_WriteCoils(ushort transactionID, byte unitID, byte[] data)
        {
            if (data.Length == 4)
            {
                ushort refAddress = (ushort)(data[0] * 0x100 + data[1]);

                byte exceptionCode = CheckMessageValidation(DataType.InputBit, refAddress, 1);

                if (exceptionCode == 0)
                {
                    bool value = false;

                    if (data[2] == 0xFF) value = true;

                    m_InputBits[refAddress - m_AddressConfig.InputBitOffset] = value;

                    m_ModbusClient.Response_WriteCoils(transactionID, unitID, refAddress, value);
                }
                else
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteCoils, exceptionCode);
                }
            }
            else
            {
                m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteCoils, (byte)ExceptionCodes.IllegalResponseLength);
            }
        }

        private void Process_WriteSingleRegister(ushort transactionID, byte unitID, byte[] data)
        {
            if (data.Length == 4)
            {
                ushort refAddress = (ushort)(data[0] * 0x100 + data[1]);

                byte exceptionCode = CheckMessageValidation(DataType.InputWord, refAddress, 1);

                if (exceptionCode == 0)
                {
                    short value = (short)(data[2] * 0x100 + data[3]);

                    m_InputWords[refAddress - m_AddressConfig.InputWordOffset] = value;

                    m_ModbusClient.Response_WriteSingleRegister(transactionID, unitID, refAddress, value);
                }
                else
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteSingleRegister, exceptionCode);
                }
            }
            else
            {
                m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteSingleRegister, (byte)ExceptionCodes.IllegalResponseLength);
            }
        }

        private void Process_WriteMultipleCoils(ushort transactionID, byte unitID, byte[] data)
        {
            if (data.Length > 4)
            {
                ushort refAddress = (ushort)(data[0] * 0x100 + data[1]);
                ushort refCount = (ushort)(data[2] * 0x100 + data[3]);
                byte byteCount = data[4];

                if (data.Length != (int)(byteCount + 5))
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.ForceMultipleCoils, (byte)ExceptionCodes.IllegalResponseLength);
                    return;
                }

                byte exceptionCode = CheckMessageValidation(DataType.InputBit, refAddress, refCount);

                if (exceptionCode == 0)
                {
                    for (int i = 0; i < refCount; i++)
                    {
                        byte temp = data[i / 8 + 5];

                        if (((temp >> (i % 8)) & 0x1) == 0x1)
                        {
                            m_InputBits[refAddress - AddressConfig.InputBitOffset + i] = true;
                        }
                        else
                        {
                            m_InputBits[refAddress - AddressConfig.InputBitOffset + i] = false;
                        }
                    }

                    m_ModbusClient.Response_WriteMultipleCoils(transactionID, unitID, refAddress, refCount);
                }
                else
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.ForceMultipleCoils, exceptionCode);
                }
            }
            else
            {
                m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.ForceMultipleCoils, (byte)ExceptionCodes.IllegalResponseLength);
            }
        }

        private void Process_WriteMultipleRegisters(ushort transactionID, byte unitID, byte[] data)
        {
            if (data.Length > 4)
            {
                ushort refAddress = (ushort)(data[0] * 0x100 + data[1]);
                ushort refCount = (ushort)(data[2] * 0x100 + data[3]);
                byte byteCount = data[4];

                if (data.Length != (int)(byteCount + 5))
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteMultipleRegisters, (byte)ExceptionCodes.IllegalResponseLength);
                    return;
                }

                byte exceptionCode = CheckMessageValidation(DataType.InputWord, refAddress, refCount);

                if (exceptionCode == 0)
                {
                    for (int i = 0; i < refCount; i++)
                    {
                        m_InputWords[refAddress - m_AddressConfig.InputWordOffset + i] = (short)(data[i * 2 + 5] * 0x100 + data[i * 2 + 6]);
                    }

                    m_ModbusClient.Response_WriteMultipleRegisters(transactionID, unitID, refAddress, refCount);
                }
                else
                {
                    m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteMultipleRegisters, exceptionCode);
                }
            }
            else
            {
                m_ModbusClient.Response_Exception(transactionID, unitID, (byte)FunctionCodes.WriteMultipleRegisters, (byte)ExceptionCodes.IllegalResponseLength);
            }
        }
        #endregion
    }
}
