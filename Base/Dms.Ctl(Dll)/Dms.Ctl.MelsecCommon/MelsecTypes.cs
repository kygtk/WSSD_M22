///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.18
// Author       : jemoon
// Description  : Melsec Common Types
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
using System.IO;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;


namespace Dms.Ctl
{
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
           this.m_nStartAddr = Convert.ToInt16( address, 16);

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
}
