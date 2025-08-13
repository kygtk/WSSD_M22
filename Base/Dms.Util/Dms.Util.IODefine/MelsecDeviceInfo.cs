///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.05
// Author       : jemoon
// Description  : MelsecDeviceInfo, MelsecDeviceInfos, MelsecDeviceRandomInfos
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public class MelsecDeviceInfo
    {
        private IoInOutType m_InOutType = IoInOutType.In;
        private devTYPE m_DeviceType = devTYPE.devL;
        private int m_NodeId = 0;
        private int m_TerminalId = 0;
        private int m_ChannelId = 0;
        private string m_AddressStr = "";
        private int m_AddressDec = 0;
        private string m_AddressHex = "0x0000";
        //private int m_IdByIoType = 0;
        private string m_Name = "";

        /// <summary>
        /// NodeId
        /// </summary>
        public int NodeId
        {
            get { return m_NodeId; }
            set { m_NodeId = value; }
        }

        /// <summary>
        /// ChannelId
        /// </summary>
        public int ChannelId
        {
            get { return m_ChannelId; }
            set { m_ChannelId = value; }
        }

        /// <summary>
        /// TerminalId
        /// </summary>
        public int TerminalId
        {
            get { return m_TerminalId; }
            set { m_TerminalId = value; }
        }

        /// <summary>
        /// Input / Output 
        /// </summary>
        public IoInOutType InOutType
        {
            get { return m_InOutType; }
            set { m_InOutType = value; }
        }

        /// <summary>
        /// Melsec device Type 
        /// </summary>
        public devTYPE DeviceType
        {
            get { return m_DeviceType; }
            set { m_DeviceType = value; }
        }

        /// <summary>
        /// DI/DO/AI/AO 
        /// </summary>
        public IoType IoType
        {
            get
            {
                return MelsecTerminal.GetIoType(m_InOutType, m_DeviceType);
            }
        }

        /// <summary>
        /// Digital / Analog
        /// </summary>
        public IoDataType IoDataType
        {
            get
            {
                return MelsecTerminal.GetIoDataType(m_DeviceType);
            }
        }

        /// <summary>
        /// Address가 10진수 or 16진수
        /// </summary>
        public MelsecTerminal.AddressingType AddressType
        {
            get { return MelsecTerminal.GetAddressingType(m_DeviceType); }
        }

        /// <summary>
        /// get : 10진수로 반환, set : 10진수로 설정
        /// </summary>
        public int AddressDec
        {
            get { return m_AddressDec; }
            set
            {
                m_AddressDec = value;
                m_AddressHex = string.Format("{0:X4}", m_AddressDec);
            }
        }

        /// <summary>
        /// get : 16진수 string으로 반환, set : 16진수로 설정
        /// </summary>
        public string AddressHex
        {
            get { return m_AddressHex; }
            set
            {
                m_AddressHex = value;
                m_AddressDec = Convert.ToInt32(m_AddressHex, 16);
            }
        }

        /// <summary>
        /// 10진수, 16진수 구분없는 Address 표기
        /// </summary>
        public string AddressStr
        {
            get { return m_AddressStr; }
            set
            {
                m_AddressStr = value;

                if (AddressType == MelsecTerminal.AddressingType.DEC)
                {
                    this.AddressDec = Convert.ToInt32(value);
                }
                else
                {
                    this.AddressDec = Convert.ToInt32(value, 16);
                }
            }
        }

        ///// <summary>
        ///// Id by iotype
        ///// </summary>
        //public int IdByIoType
        //{
        //    get { return m_IdByIoType; }
        //    set { m_IdByIoType = value; }
        //}

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public MelsecDeviceInfo()
        {
        }

        public bool IsValid()
        {
            bool valid = true;
            valid &= !string.IsNullOrEmpty(m_Name);
            valid &= !string.IsNullOrEmpty(m_AddressStr);
            return valid;
        }

        /// <summary>
        /// address는 10진수로
        /// </summary>
        /// <param name="address"></param>
        public void SetAddress(int address)
        {
            this.AddressDec = address;

            if (this.AddressType == MelsecTerminal.AddressingType.DEC)
            {
                m_AddressStr = string.Format("{0:D4}", m_AddressDec);
            }
            else
            {
                m_AddressStr = this.AddressHex;
            }
        }

        public void SetAddress(string address)
        {
            this.AddressStr = address;
        }

        public override string ToString()
        {
            string info = "";
            info += m_InOutType.ToString() + " : ";
            info += MelsecTerminal.GetDevTypeCategoryName(m_DeviceType);
            if (this.AddressType == MelsecTerminal.AddressingType.HEX)
            {
                info += string.Format("{0:X4}", m_AddressDec);
            }
            else
            {
                info += string.Format("{0:d4}", m_AddressDec);

            }
            return info;
        }
    }

    public class MelsecDeviceInfos
    {
        private List<MelsecDeviceInfo> m_Items = new List<MelsecDeviceInfo>();
        public List<MelsecDeviceInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public MelsecDeviceInfos()
        {
        }

        public static MelsecDeviceInfos GetMelsecDeviceInfos(IoNode ioNode)
        {
            MelsecDeviceInfos infos = new MelsecDeviceInfos();

            foreach (IoTerminal terminal in ioNode.Terminals)
            {
                MelsecTerminal melsecDev = (terminal as MelsecTerminal);

                if (melsecDev != null)
                {
                    int count = melsecDev.Channels.Count;
                    for (int i = 0; i < count; i++)
                    {
                        MelsecDeviceInfo info = new MelsecDeviceInfo();
                        info.NodeId = ioNode.Id;
                        info.InOutType = XFunc.GetInOutType(melsecDev.IoType);
                        info.DeviceType = melsecDev.DeviceType;
                        info.SetAddress(melsecDev.OffsetDec + i);
                        info.Name = melsecDev.Channels[i].Name;
                        infos.Items.Add(info);
                    }
                }
            }

            SortByIoType(infos);

            return infos;
        }

        public static List<IoTerminal> MakeTerminals(MelsecDeviceInfos infos)
        {
            List<IoTerminal> terminals = new List<IoTerminal>();
            foreach (MelsecDeviceInfo info in infos.Items)
            {
                MelDevs terminal = new MelDevs();
                terminal.DeviceType = info.DeviceType;
                terminal.IoType = info.IoType;
                terminal.OffsetDec = info.AddressDec;
                terminal.Size = 1;  // CreateChannel은 Size property 내부에서 call 됨
                terminal.Channels[0].Name = info.Name;

                terminals.Add((IoTerminal)terminal);
            }

            return terminals;
        }

        //0 보다 작음 : target의 값보다 작습니다. 
        //0 : target의 값과 같습니다. 
        //0 보다 큼 : target의 값보다 큽니다.
        private static int CompareByInoutType(MelsecDeviceInfo source, MelsecDeviceInfo target)
        {
            if (source.InOutType == target.InOutType)
            {
                return CompareByDevType(source, target);
            }
            else if ((int)source.InOutType < (int)target.InOutType)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //0 보다 작음 : target의 값보다 작습니다. 
        //0 : target의 값과 같습니다. 
        //0 보다 큼 : target의 값보다 큽니다.
        private static int CompareByDevType(MelsecDeviceInfo source, MelsecDeviceInfo target)
        {
            if (source.DeviceType == target.DeviceType)
            {
                return CompareByAddress(source, target);
            }
            else if ((int)source.DeviceType < (int)target.DeviceType)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //0 보다 작음 : target의 값보다 작습니다. 
        //0 : target의 값과 같습니다. 
        //0 보다 큼 : target의 값보다 큽니다.
        private static int CompareByAddress(MelsecDeviceInfo source, MelsecDeviceInfo target)
        {
            if (source.AddressDec == target.AddressDec)
            {
                return 0;
            }
            else if (source.AddressDec < target.AddressDec)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //0 보다 작음 : target의 값보다 작습니다. 
        //0 : target의 값과 같습니다. 
        //0 보다 큼 : target의 값보다 큽니다.
        private static int CompareByIoType(MelsecDeviceInfo source, MelsecDeviceInfo target)
        {
            if (source.IoType == target.IoType)
            {
                return CompareByDevType(source, target);
            }
            else if ((int)source.IoType < (int)target.IoType)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public static void SortByInout(MelsecDeviceInfos source)
        {
            source.Items.Sort(CompareByInoutType);
        }
        public static void SortByDevType(MelsecDeviceInfos source)
        {
            source.Items.Sort(CompareByDevType);
        }
        public static void SortByAddress(MelsecDeviceInfos source)
        {
            source.Items.Sort(CompareByAddress);
        }
        public static void SortByIoType(MelsecDeviceInfos source)
        {
            source.Items.Sort(CompareByIoType);
            //UpdateIdByIoType(source);
        }

        private static int m_FindAddress = 0;
        private static devTYPE m_FindAddressDevType = devTYPE.devB;
        private static IoInOutType m_FindAddressInoutType = IoInOutType.In;
        private static bool FindAddress(MelsecDeviceInfo source)
        {
            if (m_FindAddressDevType == source.DeviceType &&
                m_FindAddressInoutType == source.InOutType)
            {
                return source.AddressDec == m_FindAddress;
            }
            else
            {
                return false;
            }
        }

        private static IoInOutType m_FindInOut = IoInOutType.In;
        private static bool FindInOut(MelsecDeviceInfo source)
        {
            return source.InOutType == m_FindInOut;
        }

        private static IoType m_FindIoType = IoType.DI;
        private static bool FindIoType(MelsecDeviceInfo source)
        {
            return source.IoType == m_FindIoType;
        }

        private static devTYPE m_FindDeviceType = devTYPE.devB;
        private static bool FindDevice(MelsecDeviceInfo source)
        {
            return source.DeviceType == m_FindDeviceType;
        }

        public static MelsecDeviceInfos GetItemsBy(MelsecDeviceInfos source, IoInOutType type)
        {
            MelsecDeviceInfos infos = new MelsecDeviceInfos();
            m_FindInOut = type;
            infos.Items = source.Items.FindAll(FindInOut);

            return infos;
        }

        public static MelsecDeviceInfos GetItemsBy(MelsecDeviceInfos source, IoType type)
        {
            MelsecDeviceInfos infos = new MelsecDeviceInfos();

            m_FindIoType = type;
            infos.Items = source.Items.FindAll(FindIoType);

            return infos;
        }

        public static MelsecDeviceInfos GetItemsBy(MelsecDeviceInfos source, devTYPE type)
        {
            MelsecDeviceInfos infos = new MelsecDeviceInfos();

            m_FindDeviceType = type;
            infos.Items = source.Items.FindAll(FindDevice);

            return infos;
        }

        public static MelsecDeviceInfo GetItemsBy(MelsecDeviceInfos source, devTYPE dev, int address, IoInOutType inoutType)
        {
            m_FindAddressDevType = dev;
            m_FindAddress = address;
            m_FindAddressInoutType = inoutType;

            MelsecDeviceInfo info = source.Items.Find(FindAddress);
            return info;

            //int count = source.Items.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    MelDevInfo temp = source.Items[i];
            //    if (temp.DeviceType == dev)
            //    {
            //        if (temp.AddressDec == address)
            //        {
            //            return temp;
            //        }
            //    }
            //}
            //return null;
        }

        //public static void UpdateIdByIoType(MelDevInfos source)
        //{ 
        //    Array ioTypes = Enum.GetValues(typeof(IoType));
        //    foreach (object obj in ioTypes)
        //    {
        //        MelDevInfos temp = GetItemsBy(source, (IoType)obj);

        //        int count = temp.Items.Count;
        //        for (int i = 0; i < count; i++)
        //        {
        //            temp.Items[i].IdByIoType = i;
        //        }
        //    }
        //}

        public void UpdateChannelId()
        {
            int id = 0;
            int count = this.Items.Count;
            for (int i = 0; i < count; i++)
            {
                this.Items[i].ChannelId = id++;
            }
        }
    }

    public class MelsecDeviceRandomInfos
    {
        public devTYPE[] DeviceTypes;
        public int[] Indexes;
        public short[] Size;

        public int Count
        {
            get
            {
                if (DeviceTypes != null)
                {
                    return DeviceTypes.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
        public MelsecDeviceRandomInfos(short size)
        {
            DeviceTypes = new devTYPE[size];
            Indexes = new int[size];
            Size = new short[size];
        }
    }
}

