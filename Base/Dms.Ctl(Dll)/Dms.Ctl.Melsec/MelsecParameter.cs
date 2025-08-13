using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Ctl
{
    [Serializable]
    public class AddressMapList
    {
        private List<TagAddressMap> m_Items = new List<TagAddressMap>();

        public List<TagAddressMap> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public AddressMapList()
        {

        }

        public TagAddressMap this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Add(TagAddressMap item)
        {
            m_Items.Add(item);
        }

        public AddressMapList Clone()
        {
            AddressMapList list = new AddressMapList();
            int count = this.Items.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(this.Items[i]);
            }

            return list;
        }

    }


    #region TagAddressMap
    [Serializable]
    public class TagAddressMap
    {
        private string m_ReqBit = "";              //'EventBit
        private string m_CompBit = "";             //'Event에 대한 Confirm Bit
        private string m_AbortBit = "";
        private string m_ReplyBit = "";            // Status Bit에 대한 Reply Bit
        private string m_ActDesc = "";             //'Event에 대한 설명                
        private string m_Commander = "";           //'Event/Status를 요청하는 주체(PLC/DCM) ((Eventer))
        private string m_ActName = "";             //'Event를 처리할 ACT Name
        private int m_ActFrom = 0;                 //'Event 발생 Port or Unit번호
        private int m_ActFromSub = 0;              //'Event 발생 SubUnit 번호
        private string m_ActVal = "";              //'Event ACT에 대한 값
        private bool m_TimeOut = false;
        private string m_TimeOutDesc = "";

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

        public bool TimeOut
        {
            get { return m_TimeOut; }
            set { m_TimeOut = value; }
        }

        public string TimeOutDesc
        {
            get { return m_TimeOutDesc; }
            set { m_TimeOutDesc = value; }
        }

        public void Reset()
        {
            m_ReqBit = "";
            m_CompBit = "";
            m_AbortBit = "";
            m_ReplyBit = "";
            m_Commander = "";
            m_ActName = "";
            m_ActDesc = "";
            m_ActFrom = 0;
            m_ActFromSub = 0;
            m_ActVal = "";
            m_TimeOut = false;
            m_TimeOutDesc = "";
        }

        public void Clone(TagAddressMap data)
        {
            this.ReqBit = data.ReqBit;
            this.CompBit = data.CompBit;
            this.AbortBit = data.AbortBit;
            this.ReplyBit = data.ReplyBit;
            this.Commander = data.Commander;
            this.ActName = data.ActName;
            this.ActDesc = data.ActDesc;
            this.ActFrom = data.ActFrom;
            this.ActFromSub = data.ActFromSub;
            this.ActVal = data.ActVal;
            this.TimeOut = data.TimeOut;
            this.TimeOutDesc = data.TimeOutDesc;
        }

        public override string ToString()
        {
            string info = "";
            info += m_ReqBit.ToString();

            if (m_CompBit.ToString() != "")
            {
                info += " - ";
                info += m_CompBit.ToString();
            }

            info += " : ";

            if (m_Commander == "P") info += "PLC";
            else info += "CIM";

            return info;
        }

    }
    #endregion

    public class MelsecParameter
    {
        #region variables
        private string m_Simuate;
        private string m_Monitoring;
        private string m_MonitorCount;
        private List<string> m_MoniotrStartAddress;
        private List<string> m_MonitorSize;

        private string m_ScanCount;
        private List<string> m_ScanStartAddress;
        private List<string> m_ScanSize;
        #endregion

        #region Properties
        public string Simuate
        {
            get { return m_Simuate; }
            set { m_Simuate = value; }
        }

        public string Monitoring
        {
            get { return m_Monitoring; }
            set { m_Monitoring = value; }
        }

        public string MonitorCount
        {
            get { return m_MonitorCount; }
            set { m_MonitorCount = value; }
        }

        public string ScanCount
        {
            get { return m_ScanCount; }
            set { m_ScanCount = value; }
        }

        public List<string> MoniotrStartAddress
        {
            get { return m_MoniotrStartAddress; }
            set { m_MoniotrStartAddress = value; }
        }

        public List<string> MonitorSize
        {
            get { return m_MonitorSize; }
            set { m_MonitorSize = value; }
        }

        public List<string> ScanStartAddress
        {
            get { return m_ScanStartAddress; }
            set { m_ScanStartAddress = value; }
        }

        public List<string> ScanSize
        {
            get { return m_ScanSize; }
            set { m_ScanSize = value; }
        }

        public MelsecParameter()
        {
            m_MoniotrStartAddress = new List<string>();
            m_MonitorSize = new List<string>();


            m_ScanStartAddress = new List<string>();
            m_ScanSize = new List<string>();
        }

        #endregion
    }
}
