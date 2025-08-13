using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class UnitInitialDataInfo
    {
        #region Fields
        private List<TagUnitInitialDataInfo> m_Items = new List<TagUnitInitialDataInfo>();
        #endregion

        #region Property
        public List<TagUnitInitialDataInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagUnitInitialDataInfo this[int index]
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
        public UnitInitialDataInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            UnitInitialDataInfoAdapter adapter = new UnitInitialDataInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            UnitInitialDataInfoAdapter adapter = new UnitInitialDataInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagUnitInitialDataInfo info)
        {
            m_Items.Add(info);
        }

        public int GetId(string dvname)
        {
            int id = 0;

            foreach (TagUnitInitialDataInfo info in m_Items)
            {
                if (info.DvName == dvname)
                {
                    id = info.Id;
                    break;
                }
            }

            return id;
        }

        public TagUnitInitialDataInfo GetTagInfo(string name)
        {
            foreach( TagUnitInitialDataInfo info in Items)
            {
                if( info.Name.ToUpper() == "DATA ID" )
                {
                    return info;
                }
            }

            return null;
        }
        #endregion
    }
}
