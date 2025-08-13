using System;
using System.Collections.Generic;
using System.Text;
using Dms.DeviceLibrary;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Ctl;
using Dms.Common;

namespace Dms.Mitsubishi
{
    public class MelsecDevice // : DmsNode
    {
        #region for SimulateAddress
        private static short m_SimulateBitAddress = 0;
        private static short m_SimulateWordAddress = 0;
        #endregion

        #region Fields
        protected MelsecDeviceInfo m_DevInfo;
        protected Melsec m_MelsecControlBoard = null;
        private short m_StartAddress = 0;
        protected short m_NetworkNo = 0;
        protected short m_StationNo = 255;
        #endregion

        #region Properties
        public short NetworkNo
        {
            get { return m_NetworkNo; }
            set { m_NetworkNo = value; }
        }

        public short StationNo
        {
            get { return m_StationNo; }
            set { m_StationNo = value; }
        }

        public devTYPE DevType
        {
            get { return m_DevInfo.m_DevType; }
            //set { m_DevInfo.m_DevType = value; }
        }

        public string StartAddress
        {
            get { return m_DevInfo.m_StartAddress; }
            set 
            { 
                try
                {
                    m_StartAddress = Convert.ToInt16(value, 16);
                    m_DevInfo.m_StartAddress = value;
                }
                catch //(Exception e)
                {
                    //System.Windows.Forms.MessageBox.Show(e.ToString());
                }
            }
        }

        public short Size
        {
            get { return m_DevInfo.m_Size; }
            set { m_DevInfo.m_Size = value; }
        }

        public string Name
        {
            get { return m_DevInfo.m_Name; }
            set { m_DevInfo.m_Name = value; }
        }

        #endregion

        #region Constructor
        public MelsecDevice()
        {
        }
        #endregion

        #region Methods
        public short GetStartAddress()
        {
            return m_StartAddress;
        }

        public void SetMelsecBoard(Melsec mel)
        {
            m_MelsecControlBoard = mel;

            if (mel.SimulateAddress)
            {
                SetSimulateAddress();
            }
        }

        public void SetSimulateAddress()
        {
            if (m_DevInfo.m_DevType == devTYPE.devB)
            {
                StartAddress = "0x" + string.Format("{0:X4}", m_SimulateBitAddress);
                m_SimulateBitAddress += Size;
                Name = StartAddress;
            }
            else if (m_DevInfo.m_DevType == devTYPE.devW)
            {
                StartAddress = "0x" + string.Format("{0:X4}", m_SimulateWordAddress);
                m_SimulateWordAddress += Size;
                Name = StartAddress;
            }            
        }
        #endregion

        #region Override methods
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return "Not Defined";
            }
            else
            {   
	            string val;
	            val = string.Format("{0}, {1}, {2}, {3}", 
	                m_DevInfo.m_DevType.ToString(), 
	                m_DevInfo.m_StartAddress, 
	                m_DevInfo.m_Size,
	                m_DevInfo.m_Name);
	            return val;
            }
        }
        #endregion
    }

    public class TagEqpNodeInfo
    {
        private short m_Address;
        private string m_Name;
        private string m_Type;

        public short Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public TagEqpNodeInfo()
        {

        }

        public TagEqpNodeInfo(short address, string name, string type)
        {
            this.m_Address = address;
            this.m_Name = name;
            this.m_Type = type;
        }
    }

    public class EqpNodeInfo
    {
        private List<TagEqpNodeInfo> m_NodeInfos = new List<TagEqpNodeInfo>();

        public List<TagEqpNodeInfo> Infos
        {
            get { return m_NodeInfos; }
        }

        public TagEqpNodeInfo this[int index]
        {
            get { return m_NodeInfos[index]; }
            set { m_NodeInfos[index] = value; }
        }

        public int Count
        {
            get { return m_NodeInfos.Count; }
        }

        public void Add(TagEqpNodeInfo Info)
        {
            m_NodeInfos.Add(Info);
        }
    }
}
