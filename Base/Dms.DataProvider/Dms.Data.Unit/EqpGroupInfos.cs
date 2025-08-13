using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class EqpGroupInfos
    {
        private List<EqpGroupInfo> m_EqpGroupInfos = new List<EqpGroupInfo>();

        public List<EqpGroupInfo> Infos
        {
            get { return m_EqpGroupInfos; }
            set { m_EqpGroupInfos = value; }
        }

        public EqpGroupInfos()
        {

        }

        public EqpGroupInfo this[int index]
        {
            get { return m_EqpGroupInfos[index]; }
            set { m_EqpGroupInfos[index] = value; }
        }

        public int Count
        {
            get { return m_EqpGroupInfos.Count; }
        }

        public void Add(EqpGroupInfo Info)
        {
            m_EqpGroupInfos.Add(Info);
        }

        public EqpGroupInfo GetEqpInfo(int groupno)
        {
            foreach (EqpGroupInfo info in Infos)
            {
                if (info.EqpGroupNo == groupno)
                {
                    return info;
                }
            }

            return null;
        }

        public void Initialize(List<UnitInfo> grouplist, EqpInfos eqpinfos)
        {
            foreach (UnitInfo info in grouplist)
            {
                EqpGroupInfo GroupInfo = new EqpGroupInfo();

                GroupInfo.EqpGroupNo = info.Items[0].GroupNo;

                foreach( TagUnitInfo tagunitinfo in info.Items )
                {
                    foreach( EqpInfo eqpinfo in eqpinfos.Infos )
                    {
                        if( tagunitinfo.Id == eqpinfo.UnitNo )
                        {
                            GroupInfo.GroupInfo.Add(eqpinfos.Infos[tagunitinfo.Id]);
                        }
                    }
                }

                this.Add(GroupInfo);
            }
        }

        public void Uninitialize()
        {

        }
    }
}
