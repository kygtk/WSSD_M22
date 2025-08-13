using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Dms.Common;
using Dms.Ctl;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.31
// Author       : Kim Youngsik
// Description  : Modbus Communication Device Server(Remote Machine)
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class ModbusCommServer : XSequence
    {
        #region Fields
        private ModbusTcpServer m_ModbusServer = null;
        private string m_UnitName;
        private byte m_UnitID;
        private XLog m_Log;
        private int m_SeqSendNo = 0;
        private int m_SeqConnectNo = 0;
        private AddressConfig m_AddressConfig = new AddressConfig();

        private bool[] m_InputBits;
        private bool[] m_OutputBits;
        private short[] m_InputWords;
        private short[] m_OutputWords;

        //ReadCoils/ReadMultipleRegisters의 경우 한번에 Request할 수 있는 Size에 제한이 있다.
        //따라서 설정되어 있는 Address 영역이 최대 Request Size를 초과할 경우 이를 나눠주어야 한다.
        private List<ushort> m_InputBitAddressInfo = new List<ushort>();
        private List<ushort> m_InputBitLengthInfo = new List<ushort>();
        private List<ushort> m_InputWordAddressInfo = new List<ushort>();
        private List<ushort> m_InputWordLengthInfo = new List<ushort>();
        private List<ushort> m_OutputBitAddressInfo = new List<ushort>();
        private List<ushort> m_OutputBitLengthInfo = new List<ushort>();
        private List<ushort> m_OutputWordAddressInfo = new List<ushort>();
        private List<ushort> m_OutputWordLengthInfo = new List<ushort>();

        private uint m_OldTick = 0;
        private int m_UpdateInterval;
        #endregion

        #region Properties
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
            get { return m_ModbusServer.Connected; }
        }
        public int UpdateInterval
        {
            get { return m_UpdateInterval; }
        }
        #endregion

        #region Constructor
        public ModbusCommServer(int sleepTime, XLog log, ModbusEqpInfo info)
            : base(sleepTime)
        {
            m_ScanTime = sleepTime;
            m_Log = log;

            m_ModbusServer = new ModbusTcpServer(info.UnitID, info.UnitName, info.IPAddress, m_Log);

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

            SplitAddressRange();
        }
        #endregion

        #region Overrride
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                SeqConnect();

                //연결이 되어 있으면 Polling 시작.
                if (m_ModbusServer.Connected)
                {
                    SeqPolling();
                }
            }

            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Sequence
        private void SeqConnect()
        {
            int SeqNo = m_SeqConnectNo;
            string logText = "";

            switch (SeqNo)
            {
                case 0:
                    //if (m_ModbusServer.Connected == false)
                    {
                        if (m_ModbusServer.Connect())
                        {
                            logText = string.Format("**********CONNECTED**********");
                            SetLog(logText);

                            //ClearListNQueue();
                            SeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    if (m_ModbusServer.Connected == false)
                    {
                        for (int i = 0; i < m_InputBits.Length; i++)
                        {
                            m_InputBits[i] = false;
                        }

                        for (int i = 0; i < m_InputWords.Length; i++)
                        {
                            m_InputWords[i] = 0;
                        }

                        SeqNo = 0;
                    }
                    break;
            }

            m_SeqConnectNo = SeqNo;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Method : SeqPolling()
        // Description : ReadCoils -> ReadMultipleRegisters -> WriteMultipleCoils -> WriteMultipleRegisters를
        //               주기적으로 수행함으로 Class 내부의 Array값(IB, OB, IW, OW)을 Remote Machine과
        //               동기화 시켜준다.
        //
        ///////////////////////////////////////////////////////////////////////////
        private void SeqPolling()
        {
            int SeqNo = m_SeqSendNo;
            //bool rv = false;

            switch (SeqNo)
            {
                case 0:
                    {
                        uint curTick = XFunc.GetTickCount();

                        m_UpdateInterval = (int)(curTick - m_OldTick);
                        m_OldTick = curTick;

                        for (int i = 0; i < m_InputBitAddressInfo.Count; i++)
                        {
                            byte[] result = m_ModbusServer.Request_ReadCoils(m_InputBitAddressInfo[i], m_InputBitLengthInfo[i]);

                            if (result != null)
                            {
                                GetValueFromResponse((byte)FunctionCodes.ReadCoils, m_InputBitAddressInfo[i], m_InputBitLengthInfo[i], result);
                            }
                        }

                        SeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        for (int i = 0; i < m_InputWordAddressInfo.Count; i++)
                        {
                            byte[] result = m_ModbusServer.Request_ReadMultipleRegisters(m_InputWordAddressInfo[i], m_InputWordLengthInfo[i]);

                            if (result != null)
                            {
                                GetValueFromResponse((byte)FunctionCodes.ReadMultipleRegisters, m_InputWordAddressInfo[i], m_InputWordLengthInfo[i], result);
                            }
                        }

                        SeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        {
                            for (int i = 0; i < m_OutputBitAddressInfo.Count; i++)
                            {
                                bool[] value = new bool[m_OutputBitLengthInfo[i]];

                                for (int j = 0; j < m_OutputBitLengthInfo[i]; j++)
                                {
                                    value[j] = m_OutputBits[m_OutputBitAddressInfo[i] - m_AddressConfig.OutputBitOffset + j];
                                }

                                byte[] result = m_ModbusServer.Request_WriteMultipleCoils(m_OutputBitAddressInfo[i], m_OutputBitLengthInfo[i], value);
                            }
                        }

                        SeqNo = 30;
                    }
                    break;
                case 30:
                    {
                        {
                            for (int i = 0; i < m_OutputWordAddressInfo.Count; i++)
                            {
                                short[] value = new short[m_OutputWordLengthInfo[i]];

                                for (int j = 0; j < m_OutputWordLengthInfo[i]; j++)
                                {
                                    value[j] = m_OutputWords[m_OutputWordAddressInfo[i] - m_AddressConfig.OutputWordOffset + j];
                                }

                                byte[] result = m_ModbusServer.Request_WriteMultipleRegisters(m_OutputWordAddressInfo[i], m_OutputWordLengthInfo[i], value);
                            }
                        }

                        SeqNo = 0;
                    }
                    break;
            }

            m_SeqSendNo = SeqNo;
        }
        #endregion

        #region Public Methods
        public void SetLog(string text)
        {
            string logText = "";

            logText = string.Format("[UNIT : {0} {1}]{2}{3}", m_UnitID, m_UnitName, '\t', text);
            m_Log.TextOut(logText);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// ReadCoils는 한번에 1000개 까지,
        /// WriteMultipleCoils는 한번에 800개 까지,
        /// ReadMultipleRegisters는 125개 까지,
        /// WriteMultipleRegister는 100개 까지 밖에 못하기 때문에
        /// 그 이상의 영역을 사용할 때에는 Data Area를 Split해서 Message를 보내야 한다.
        /// </summary>
        private void SplitAddressRange()
        {
            //1. Split Input Bit Area
            //ReadCoils는 한번에 2000개까지 하자...(원래 MAX는 2000개)
            ushort bitSplitCount = 2000;
            ushort bitRemainCount = m_AddressConfig.InputBitLength;
            ushort bitStartAddress = m_AddressConfig.InputBitOffset;

            while (bitRemainCount > 0)
            {
                m_InputBitAddressInfo.Add(bitStartAddress);

                if (bitSplitCount > bitRemainCount)
                {
                    m_InputBitLengthInfo.Add(bitRemainCount);
                    bitRemainCount = 0;
                }
                else
                {
                    m_InputBitLengthInfo.Add(bitSplitCount);
                    bitRemainCount -= bitSplitCount;
                    bitStartAddress += bitSplitCount;
                }
            }

            //2. Split Input Word Area
            //ReadMultipleRegisters는 한번에 124개 까지만 하자...(원래 MAX는 125개)
            ushort wordSplitCount = 124;
            ushort wordRemainCount = m_AddressConfig.InputWordLength;
            ushort wordStartAddress = m_AddressConfig.InputWordOffset;

            while (wordRemainCount > 0)
            {
                m_InputWordAddressInfo.Add(wordStartAddress);

                if (wordSplitCount > wordRemainCount)
                {
                    m_InputWordLengthInfo.Add(wordRemainCount);
                    wordRemainCount = 0;
                }
                else
                {
                    m_InputWordLengthInfo.Add(wordSplitCount);
                    wordRemainCount -= wordSplitCount;
                    wordStartAddress += wordSplitCount;
                }
            }

            //3. Split Output Bit Area
            //WriteMultipleCoils는 한번에 800개 까지 하자...(원래 MAX는 800개)
            bitSplitCount = 800;
            bitRemainCount = m_AddressConfig.OutputBitLength;
            bitStartAddress = m_AddressConfig.OutputBitOffset;

            while (bitRemainCount > 0)
            {
                m_OutputBitAddressInfo.Add(bitStartAddress);

                if (bitSplitCount > bitRemainCount)
                {
                    m_OutputBitLengthInfo.Add(bitRemainCount);
                    bitRemainCount = 0;
                }
                else
                {
                    m_OutputBitLengthInfo.Add(bitSplitCount);
                    bitRemainCount -= bitSplitCount;
                    bitStartAddress += bitSplitCount;
                }
            }

            //4. Split Output Word Area
            //WriteMultipleRegisters는 한번에 100개 까지만 하자...(원래 MAX는 100개)
            wordSplitCount = 100;
            wordRemainCount = m_AddressConfig.OutputWordLength;
            wordStartAddress = m_AddressConfig.OutputWordOffset;

            while (wordRemainCount > 0)
            {
                m_OutputWordAddressInfo.Add(wordStartAddress);

                if (wordSplitCount > wordRemainCount)
                {
                    m_OutputWordLengthInfo.Add(wordRemainCount);
                    wordRemainCount = 0;
                }
                else
                {
                    m_OutputWordLengthInfo.Add(wordSplitCount);
                    wordRemainCount -= wordSplitCount;
                    wordStartAddress += wordSplitCount;
                }
            }
        }

        private void GetValueFromResponse(byte functionCode, ushort refAddr, ushort refCount, byte[] value)
        {
            switch (functionCode)
            {
                case (byte)FunctionCodes.ReadCoils:
                    {
                        if (value == null) return;

                        for (int i = 0; i < value.Length; i++)
                        {
                            byte temp = value[i];

                            for (int j = 0; j < 8; j++)
                            {
                                int address = refAddr + (i * 8) + j;

                                if (address < (m_AddressConfig.InputBitOffset + m_AddressConfig.InputBitLength))
                                {
                                    if (((temp >> j) & 0x1) == 0x1)
                                    {
                                        m_InputBits[address - m_AddressConfig.InputBitOffset] = true;
                                    }
                                    else
                                    {
                                        m_InputBits[address - m_AddressConfig.InputBitOffset] = false;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case (byte)FunctionCodes.WriteCoils:
                    {
                        if (refAddr < (m_AddressConfig.OutputBitOffset + m_AddressConfig.OutputBitLength))
                        {
                            if (value[2] == 0x00) m_OutputBits[refAddr - m_AddressConfig.OutputBitOffset] = false;
                            else if (value[2] == 0xFF) m_OutputBits[refAddr - m_AddressConfig.OutputBitOffset] = true;
                        }
                    }
                    break;
                case (byte)FunctionCodes.ReadMultipleRegisters:
                    {
                        if (value == null) return;

                        int length = value.Length / 2;
                        for (int i = 0; i < length; i++)
                        {
                            int address = refAddr + i;

                            if (address < (m_AddressConfig.InputWordOffset + m_AddressConfig.InputWordLength))
                            {
                                m_InputWords[address - m_AddressConfig.InputWordOffset] = (short)((value[(i * 2) + 0] * 0x100) + value[(i * 2) + 1]);
                            }
                        }
                    }
                    break;
                case (byte)FunctionCodes.WriteMultipleRegisters:
                    break;
            }
        }
        #endregion
    }
}
