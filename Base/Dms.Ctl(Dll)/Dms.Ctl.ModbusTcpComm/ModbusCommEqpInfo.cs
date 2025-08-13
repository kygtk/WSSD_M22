using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Dms.DeviceLibrary;

namespace Dms.Ctl
{
    public enum ModbusType
    {
        Server,
        Client,
    }

    public struct AddressConfig
    {
        public ushort InputBitOffset;
        public ushort OutputBitOffset;
        public ushort InputWordOffset;
        public ushort OutputWordOffset;

        public ushort InputBitLength;
        public ushort OutputBitLength;
        public ushort InputWordLength;
        public ushort OutputWordLength;
    }

    public class ModbusEqpInfo : DmsModbusNode
    {
        #region Fields
        private byte m_UnitID;
        private string m_UnitName;
        private string m_IPAddress;
        private ModbusType m_Type;

        private ushort m_InputBitOffset;
        private ushort m_OutputBitOffset;
        private ushort m_InputWordOffset;
        private ushort m_OutputWordOffset;

        private ushort m_InputBitLength;
        private ushort m_OutputBitLength;
        private ushort m_InputWordLength;
        private ushort m_OutputWordLength;
        #endregion

        #region Properties
        [Category("1. EQP Info")]
        public byte UnitID
        {
            get { return m_UnitID; }
            set { m_UnitID = value; }
        }
        [Category("1. EQP Info")]
        public string UnitName
        {
            get { return m_UnitName; }
            set { m_UnitName = value; }
        }
        [Category("1. EQP Info")]
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }
        [Category("1. EQP Info")]
        public ModbusType ModbusType
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        [Category("2. Address Range Info")]
        public ushort InputBitOffset
        {
            get { return m_InputBitOffset; }
            set { m_InputBitOffset = value; }
        }
        [Category("2. Address Range Info")]
        public ushort OutputBitOffset
        {
            get { return m_OutputBitOffset; }
            set { m_OutputBitOffset = value; }
        }
        [Category("2. Address Range Info")]
        public ushort InputWordOffset
        {
            get { return m_InputWordOffset; }
            set { m_InputWordOffset = value; }
        }
        [Category("2. Address Range Info")]
        public ushort OutputWordOffset
        {
            get { return m_OutputWordOffset; }
            set { m_OutputWordOffset = value; }
        }
        [Category("2. Address Range Info")]
        public ushort InputBitLength
        {
            get { return m_InputBitLength; }
            set { m_InputBitLength = value; }
        }
        [Category("2. Address Range Info")]
        public ushort OutputBitLength
        {
            get { return m_OutputBitLength; }
            set { m_OutputBitLength = value; }
        }
        [Category("2. Address Range Info")]
        public ushort InputWordLength
        {
            get { return m_InputWordLength; }
            set { m_InputWordLength = value; }
        }
        [Category("2. Address Range Info")]
        public ushort OutputWordLength
        {
            get { return m_OutputWordLength; }
            set { m_OutputWordLength = value; }
        }
        #endregion

        #region Constructor
        public ModbusEqpInfo()
        {
            m_UnitID = 0;
            m_UnitName = "UNIT____";
            m_IPAddress = "127.0.0.1";
            m_Type = ModbusType.Server;

            m_InputBitOffset = 0;
            m_OutputBitOffset = 0;
            m_InputWordOffset = 0;
            m_OutputWordOffset = 0;

            m_InputBitLength = 100;
            m_OutputBitLength = 100;
            m_InputWordLength = 100;
            m_OutputWordLength = 100;
        }
        #endregion

        #region override Methods
        public override bool Initialize()
        {
            if (!m_CreateMode) ReadConfiguration(m_Config, GetPathName());

            return base.Initialize();
        }

        public override void WriteConfiguration()
        {
            DmsSerializingService dss = new DmsSerializingService();
            dss.WriteXml(this, typeof(ModbusEqpInfo), GetPathName());
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            ModbusEqpInfo remoteMachine;
            object obj = new object();

            if (dss.ReadXml(ref obj, typeof(ModbusEqpInfo), filename))
            {
                remoteMachine = (ModbusEqpInfo)obj;
            }
        }
        #endregion
    }
}
