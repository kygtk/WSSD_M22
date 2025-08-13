using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Cim.Common
{
    public class CTTimeInfos
    {
        private List<CTTimeInfo> m_CTTimeInfos = new List<CTTimeInfo>();

        public List<CTTimeInfo> Infos
        {
            get { return m_CTTimeInfos; }
            set { m_CTTimeInfos = value; }
        }

        public CTTimeInfos()
        {

        }

        public CTTimeInfo this[int index]
        {
            get { return m_CTTimeInfos[index]; }
            set { m_CTTimeInfos[index] = value; }
        }

        public int Count
        {
            get { return m_CTTimeInfos.Count; }
        }

        public void Add(CTTimeInfo CTTimeInfo)
        {
            m_CTTimeInfos.Add(CTTimeInfo);
        }

        public void Initialize(int MaxNo)
        {
            for (int i = 0; i < MaxNo; i++)
            {
                CTTimeInfo CTTimeInfo = new CTTimeInfo(i);

                this.Add(CTTimeInfo);
            }
        }

        public void Uninitialize()
        {

        }
    }
}
