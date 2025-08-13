using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Cim.Common
{
    [Serializable()]
    public class TagSubUnitInfo
    {
        private int m_Id;
        private int m_SubUnitNo;
        private string m_UnitName;
        private string m_Desc;
        private string m_Address;

        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        public int SubUnitNo
        {
            get { return m_SubUnitNo; }
            set { m_SubUnitNo = value; }
        }
        public string UnitName
        {
            get { return m_UnitName; }
            set { m_UnitName = value; }
        }
        public string Desc
        {
            get { return m_Desc; }
            set { m_Desc = value; }
        }
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        public TagSubUnitInfo()
        {
        }

        public TagSubUnitInfo(int id, int subunitno, string unitname, string desc, string address)
        {
            this.m_Id = id;
            this.m_SubUnitNo = subunitno;
            this.m_UnitName = unitname;
            this.m_Desc = desc;
            this.m_Address = address;
        }

        public void Clone(TagSubUnitInfo info)
        {
            this.m_Id = info.m_Id;
            this.m_SubUnitNo = info.m_SubUnitNo;
            this.m_UnitName = info.m_UnitName;
            this.m_Desc = info.m_Desc;
            this.m_Address = info.m_Address;
        }
    }
}
