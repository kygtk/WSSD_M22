using System;
using System.Collections.Generic;
using System.Text;
using Dms.Cim.Common;

namespace Dms.Data
{
    public class SubUnitInfo
    {
        #region Fields
        private List<TagSubUnitInfo> m_Items = new List<TagSubUnitInfo>();
        #endregion

        #region Properties
        public List<TagSubUnitInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion

        #region Constructor
        public SubUnitInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            SubUnitInfoAdapter adapter = new SubUnitInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            SubUnitInfoAdapter adapter = new SubUnitInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public string GetName(int id)
        {
            string name = "";

            foreach (TagSubUnitInfo tagsubunitinfo in m_Items)
            {
                if (tagsubunitinfo.Id == id)
                {
                    name = tagsubunitinfo.UnitName;
                    break;
                }
            }

            return name;
        }
        #endregion
    }
}
