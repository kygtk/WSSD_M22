using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Collections;

namespace Dms.Ctl
{
    #region clsAddressMapType
    [Serializable]
    public class clsAdressMapType
    {
        private string m_ReqBit = "";              //'EventBit
        private string m_CompBit = "";             //'Event에 대한 Confirm Bit
        private string m_AbortBit = "";
        private string m_ReplyBit = "";            // Status Bit에 대한 Reply Bit
        private string m_Commander = "";           //'Event/Status를 요청하는 주체(PLC/CIM) ((Eventer))
        private string m_EventDescription = "";    //'EventBiT 설명
        private string m_CompDescription = "";     //'CompBit 설명
        private string m_Status = "";              //'ReqBit의 종류를 설명한다(Req,Comp,Status)

        private string m_ActName = "";             //'Event를 처리할 ACT Name
        private string m_ActDesc = "";             //'Event에 대한 설명
        private int m_ActFrom = 0;                 //'Event 발생 Port or Unit번호
        private int m_ActFromSub = 0;              //'Event 발생 SubUnit 번호
        private string m_ActVal = "";              //'Event ACT에 대한 값

        public string ReqBit
        {
            get { return m_ReqBit; }
            set { m_ReqBit = value; }
        }

        public string CompBit
        {
            get { return m_CompBit; }
            set { m_CompBit = value; }
        }

        public string AbortBit
        {
            get { return m_AbortBit; }
            set { m_AbortBit = value; }
        }

        public string ReplyBit
        {
            get { return m_ReplyBit; }
            set { m_ReplyBit = value; }
        }

        public string Commander
        {
            get { return m_Commander; }
            set { m_Commander = value; }
        }

        public string EventDescription
        {
            get { return m_EventDescription; }
            set { m_EventDescription = value; }
        }

        public string CompDescription
        {
            get { return m_CompDescription; }
            set { m_CompDescription = value; }
        }

        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        public string ActName
        {
            get { return m_ActName; }
            set { m_ActName = value; }
        }

        public string ActDesc
        {
            get { return m_ActDesc; }
            set { m_ActDesc = value; }
        }

        public int ActFrom
        {
            get { return m_ActFrom; }
            set { m_ActFrom = value; }
        }

        public int ActFromSub
        {
            get { return m_ActFromSub; }
            set { m_ActFromSub = value; }
        }

        public string ActVal
        {
            get { return m_ActVal; }
            set { m_ActVal = value; }
        }

        public void Reset()
        {
            m_ReqBit = "";
            m_CompBit = "";
            m_AbortBit = "";
            m_ReplyBit = "";
            m_Commander = "";
            m_EventDescription = "";
            m_CompDescription = "";
            m_Status = "";

            m_ActName = "";
            m_ActDesc = "";
            m_ActFrom = 0;
            m_ActFromSub = 0;
            m_ActVal = "";
        }
    }
    #endregion
}
