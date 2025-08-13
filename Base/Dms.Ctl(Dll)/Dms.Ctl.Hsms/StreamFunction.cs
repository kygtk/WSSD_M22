using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SEComEnabler.SEComPlugIn;
using SEComEnabler.SEComStructure;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05
// Author       : Kim Youngsik
// Description  : Base class of SmFn classes
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class StreamFunction
    {
        #region Fields
        private SEComData m_SecomData;
        private bool m_PrimaryRcvd;
        private bool m_SecondaryRcvd;
        private bool m_T3Timeout;   //2009.08.25 Youngsik... T3 Timeout flag 추가
        private long m_SystemByte;
        private int m_Stream;
        private int m_Function;
        private int m_Ack;
        private string m_sAck;
        private string m_PrimaryMsgName;
        private string m_SecondaryMsgName;
        private bool m_UseAbort;
        #endregion

        #region Properties
        public SEComData SecomData
        {
            get { return m_SecomData; }
            set { m_SecomData = value; }
        }
        public bool PrimaryRcvd
        {
            get { return m_PrimaryRcvd; }
            set { m_PrimaryRcvd = value; }
        }
        public bool SecondaryRcvd
        {
            get { return m_SecondaryRcvd; }
            set { m_SecondaryRcvd = value; }
        }
        //2009.08.25 Youngsik... T3 Timeout flag 추가
        public bool T3Timeout
        {
            get { return m_T3Timeout; }
            set { m_T3Timeout = value; }
        }
        public long SystemByte
        {
            get { return m_SystemByte; }
            set { m_SystemByte = value; }
        }
        public int Stream
        {
            get { return m_Stream; }
            set { m_Stream = value; }
        }
        public int Function
        {
            get { return m_Function; }
            set { m_Function = value; }
        }
        public int Ack
        {
            get { return m_Ack; }
            set { m_Ack = value; }
        }
        public string sAck
        {
            get { return m_sAck; }
            set { m_sAck = value; }
        }
        public string PrimaryMsgName
        {
            get { return m_PrimaryMsgName; }
            set { m_PrimaryMsgName = value; }
        }
        public string SecondaryMsgName
        {
            get { return m_SecondaryMsgName; }
            set { m_SecondaryMsgName = value; }
        }
        public bool UseAbort
        {
            get { return m_UseAbort; }
            set { m_UseAbort = value; }
        }
        #endregion

        #region Constructor
        public StreamFunction()
        {
            m_SecomData = new SEComData();

            m_PrimaryRcvd = false;
            m_SecondaryRcvd = false;

            m_UseAbort = true;

            m_Ack = -1;
        }

        public SXTransaction OnAbortSend(SEComData secomData)
        {
            SXTransaction transaction = new SXTransaction();

            m_SystemByte = secomData.HeaderItems.SystemBytes;

            transaction.Stream = m_Stream;
            transaction.Function = 0;
            transaction.MessageName = string.Format("S{0}F0", transaction.Stream);
            transaction.SystemBytes = m_SystemByte;

            return transaction;
        }
        #endregion
    }
}
