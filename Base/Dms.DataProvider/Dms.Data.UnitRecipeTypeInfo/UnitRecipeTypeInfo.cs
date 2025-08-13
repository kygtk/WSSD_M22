using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class UnitRecipeTypeInfo
    {
        #region Fields
        private List<TagUnitRecipeTypeInfo> m_Items = new List<TagUnitRecipeTypeInfo>();
        #endregion

        #region Property
        public List<TagUnitRecipeTypeInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagUnitRecipeTypeInfo this[int index]
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
        public UnitRecipeTypeInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            UnitRecipeTypeInfoAdapter adapter = new UnitRecipeTypeInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            UnitRecipeTypeInfoAdapter adapter = new UnitRecipeTypeInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagUnitRecipeTypeInfo info)
        {
            m_Items.Add(info);
        }
        #endregion
    }
}
