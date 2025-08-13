using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class UnitRecipeInfo
    {
        #region Fields
        private List<TagUnitRecipeInfo> m_Items = new List<TagUnitRecipeInfo>();
        #endregion

        #region Property
        public List<TagUnitRecipeInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagUnitRecipeInfo this[int index]
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
        public UnitRecipeInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            UnitRecipeInfoAdapter adapter = new UnitRecipeInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            UnitRecipeInfoAdapter adapter = new UnitRecipeInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagUnitRecipeInfo info)
        {
            m_Items.Add(info);
        }

        public int GetId(string dvname)
        {
            int id = 0;

            foreach (TagUnitRecipeInfo info in m_Items)
            {
                if (info.DvName == dvname)
                {
                    id = info.Id;
                    break;
                }
            }

            return id;
        }

        public TagUnitRecipeInfo GetTagInfo(string name)
        {
            foreach (TagUnitRecipeInfo info in Items)
            {
                if (info.Name.ToUpper() == "PROCESS ID")
                {
                    return info;
                }
            }

            return null;
        }

        #endregion

    }
}
