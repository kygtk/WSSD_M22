using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class EqpInfos
    {
        private List<EqpInfo> m_EqpInfos = new List<EqpInfo>();

        public List<EqpInfo> Infos
        {
            get { return m_EqpInfos; }
            set { m_EqpInfos = value; }
        }

        public EqpInfos()
        {

        }

        public EqpInfo this[int index]
        {
            get { return m_EqpInfos[index]; }
            set { m_EqpInfos[index] = value; }
        }

        public int Count
        {
            get { return m_EqpInfos.Count; }
        }

        public void Add(EqpInfo PortInfo)
        {
            m_EqpInfos.Add(PortInfo);
        }

        public EqpInfo GetEqpInfo(int UnitNo )
        {
            foreach( EqpInfo info in Infos )
            {
                if (info.UnitNo == UnitNo)
                {
                    return info;
                }
            }

            return null;
        }

        public void Initialize(UnitInfo unitinfo, int recipeusecount)
        {
            foreach (TagUnitInfo info in unitinfo.Items)
            {
                EqpInfo EqpInfo = new EqpInfo();

                EqpInfo.UnitName = info.Name;
                EqpInfo.UnitNo = info.Id;

                if( info.SubUnitInfos.Count > 0 ) EqpInfo.Initialize(info.SubUnitInfos.Count, recipeusecount);

                this.Add(EqpInfo);
            }
        }

        public void Uninitialize()
        {

        }

    }
}
