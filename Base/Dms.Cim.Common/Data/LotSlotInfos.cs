using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Cim.Common
{
    [Serializable]
    public class LotSlotInfos
    {
        private const int MaxSlotNo = 50;

        private List<LotSlotInfo> m_Items = new List<LotSlotInfo>();

        public List<LotSlotInfo> Items
        {
            get { return m_Items; }
        }

        public LotSlotInfo this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public LotSlotInfos()
        {
//            Initialize(MaxSlotNo);
        }

        public void Add(LotSlotInfo info)
        {
            m_Items.Add(info);
        }

        public void SetApdData( int slotno, List<TagApdItem> apddata)
        {
            m_Items[slotno - 1].ApdDatas.SetItem(apddata);
        }

        public void Initialize(int SlotNo, List<string> dvname)
        {
            for (int i = 0; i < SlotNo; i++)
            {
                LotSlotInfo info = new LotSlotInfo();
                info.ApdDatas.Initialize(dvname);

                this.Add(info);
            }
        }

        public void Uninitialize()
        {
        }

    }
}
