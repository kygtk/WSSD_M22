using System;
using System.Collections.Generic;
using System.Text;
using Dms.Cim.Common;

namespace Dms.Data
{
    public class UnitInfo
    {
        #region Fields
        private List<TagUnitInfo> m_Items = new List<TagUnitInfo>();
        #endregion

        #region Properties
        public List<TagUnitInfo> Items
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
        public UnitInfo()
        {
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        public void SaveToDB()
        {
            UnitInfoAdapter adapter = new UnitInfoAdapter();
            adapter.SaveToDB(this);
        }

        public void LoadToDB()
        {
            UnitInfoAdapter adapter = new UnitInfoAdapter();
            adapter.LoadFormDB(this);
        }

        public void Add(TagUnitInfo info)
        {
            m_Items.Add(info);
        }

        public int GetId(string name)
        {
            int id = 0;

            foreach (TagUnitInfo tagunitinfo in m_Items)
            {
                if (tagunitinfo.Name == name)
                {
                    id = tagunitinfo.Id;
                    break;
                }
            }

            return id;
        }

        public string GetName(int id)
        {
            string name = "";

            foreach (TagUnitInfo tagunitinfo in m_Items)
            {
                if (tagunitinfo.Id == id)
                {
                    name = tagunitinfo.Name;
                    break;
                }
            }

            return name;
        }

        public string GetSubUnitName(int unitno, int subunitid)
        {
            string name = "";

            foreach (TagUnitInfo tagunitinfo in m_Items)
            {
                if (tagunitinfo.Id == unitno)
                {
                    if (subunitid == 0)
                    {
                        name = tagunitinfo.Name;
                        break;
                    }
                    else
                    {
                        foreach (TagSubUnitInfo tagsubunitinfo in tagunitinfo.SubUnitInfos.Items)
                        {
                            if (subunitid == tagsubunitinfo.SubUnitNo)
                            {
                                name = tagsubunitinfo.UnitName;
                                break;
                            }
                        }
                    }
                }
            }

            return name;
        }

        public TagUnitInfo GetUnitInfo(int id)
        {
            TagUnitInfo info = null;

            foreach (TagUnitInfo tagunitinfo in m_Items)
            {
                if (tagunitinfo.Id == id)
                {
                    info = tagunitinfo;
                    break;
                }
            }

            return info;
        }

        public bool IsExistUnitId(string unitid)
        {
            bool brv = false;

            foreach (TagUnitInfo tagunitinfo in m_Items)
            {
                if (tagunitinfo.UnitId == unitid)
                {
                    brv = true;
                    break;
                }
            }

            return brv;
        }

        public TagUnitInfo GetUnitInfo(string unitid)
        {
            TagUnitInfo info = null;

            foreach (TagUnitInfo tagunitinfo in m_Items)
            {
                if (tagunitinfo.UnitId == unitid)
                {
                    info = tagunitinfo;
                    break;
                }
            }

            return info;
        }
        #endregion
    }
}
