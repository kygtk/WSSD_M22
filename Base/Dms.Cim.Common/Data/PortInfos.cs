using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Cim.Common
{
    public class PortInfos
    {
        private List<PortInfo> m_Items = new List<PortInfo>();

        public List<PortInfo> Items
        {
            get { return m_Items; }
        }

        public PortInfo this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public PortInfos()
        {
        }

        public void Add(PortInfo info)
        {
            m_Items.Add(info);
        }

        public void Initialize(int portcount)
        {
            for (int i = 0; i < portcount; i++)
            {
                PortInfo info = new PortInfo();
                this.Add(info);
            }
        }

        public void Uninitialize()
        {
        }
    }
}
