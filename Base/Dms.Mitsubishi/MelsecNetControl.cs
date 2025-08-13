using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Dms.DeviceLibrary;
using Dms.Ctl;
using Dms.Common;

namespace Dms.Mitsubishi
{
    public struct MelsecDeviceInfo
    {
        public devTYPE m_DevType;
        public string m_StartAddress;
        public short m_Size;
        public string m_Name;

        public MelsecDeviceInfo(devTYPE devType, string startAddress, short size, string name)
        {
            m_DevType = devType;
            m_StartAddress = startAddress;
            m_Size = size;
            m_Name = name;
        }
    }
}
