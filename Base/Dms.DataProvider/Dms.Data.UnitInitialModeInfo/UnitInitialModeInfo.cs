using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class UnitInitialModeInfo
    {
        #region Fields
        private List<TagUnitInitialModeInfo> m_Items = new List<TagUnitInitialModeInfo>();
        #endregion

        #region Property
        public List<TagUnitInitialModeInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagUnitInitialModeInfo this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion

        #region Constructor
        public UnitInitialModeInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            UnitInitialModeInfoAdapter adapter = new UnitInitialModeInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            UnitInitialModeInfoAdapter adapter = new UnitInitialModeInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagUnitInitialModeInfo info)
        {
            m_Items.Add(info);
        }

        public int GetId(string dvname)
        {
            int id = 0;

            foreach (TagUnitInitialModeInfo info in m_Items)
            {
                if (info.DvName == dvname)
                {
                    id = info.Id;
                    break;
                }
            }

            return id;
        }
        #endregion
    }
}
