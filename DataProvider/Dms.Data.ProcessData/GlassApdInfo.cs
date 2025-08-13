using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class GlassApdInfo
    {
        #region Fields
        private List<TagGlassApdInfo> m_Items = new List<TagGlassApdInfo>();
        #endregion

        #region Property
        public List<TagGlassApdInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagGlassApdInfo this[int index]
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
        public GlassApdInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        public int GetWordSize()
        {
            int count = 0;

            foreach( TagGlassApdInfo info in m_Items )
            {
                count += info.WordSize;
            }

            return count;
        }

        public void SaveToDB()
        {
            GlassApdInfoAdapter adapter = new GlassApdInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            GlassApdInfoAdapter adapter = new GlassApdInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagGlassApdInfo info)
        {
            m_Items.Add(info);
        }

        #endregion

    }
}
