using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class CurrentDataInfo
    {
        #region Fields
        private List<TagCurrentDataInfo> m_Items = new List<TagCurrentDataInfo>();
        #endregion

        #region Property
        public List<TagCurrentDataInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagCurrentDataInfo this[int index]
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
        public CurrentDataInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            CurrentDataInfoAdapter adapter = new CurrentDataInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            CurrentDataInfoAdapter adapter = new CurrentDataInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagCurrentDataInfo info)
        {
            m_Items.Add(info);
        }
        #endregion
    }
}
