using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class EqpUnitInfo
    {
        #region Fields
        private List<TagEqpUnitInfo> m_Items = new List<TagEqpUnitInfo>();
        #endregion

        #region Properties
        public List<TagEqpUnitInfo> Items
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
        public EqpUnitInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion


        #region Method
        public void SaveToDB()
        {
            CimEqpUnitInfoAdapter adapter = new CimEqpUnitInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            CimEqpUnitInfoAdapter adapter = new CimEqpUnitInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public string GetName(int id)
        {
            string name = "";

            foreach (TagEqpUnitInfo tageqpunitinfo in m_Items)
            {
                if (tageqpunitinfo.Id == id)
                {
                    name = tageqpunitinfo.UnitName;
                    break;
                }
            }

            return name;
        }
        #endregion
    }
}
