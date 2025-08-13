using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Cim.Common;

namespace Dms.Data
{
    [Serializable]
    public class GlassApdDataHistory
    {
        private List<ApdData> m_Items = new List<ApdData>();

        public List<ApdData> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public ApdData Last
        {
            get
            {
                if (Count == 0)
                {
                    return null;
                }
                else
                    return m_Items[Count - 1];
            }
        }

        public ApdData this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public GlassApdDataHistory()
        {
        }
    }
}
