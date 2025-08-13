using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class UnitInitialDataTypeInfo
    {
        #region Fields
        private List<TagUnitInitialDataTypeInfo> m_Items = new List<TagUnitInitialDataTypeInfo>();
        #endregion

        #region Property
        public List<TagUnitInitialDataTypeInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagUnitInitialDataTypeInfo this[int index]
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
        public UnitInitialDataTypeInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            UnitInitialDataTypeInfoAdapter adapter = new UnitInitialDataTypeInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            UnitInitialDataTypeInfoAdapter adapter = new UnitInitialDataTypeInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagUnitInitialDataTypeInfo info)
        {
            m_Items.Add(info);
        }
        #endregion
    }
}
