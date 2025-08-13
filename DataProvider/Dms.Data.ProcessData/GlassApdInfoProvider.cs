using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

namespace Dms.Data
{
    public class GlassApdInfoProvider
    {
        #region Fields
        private GlassApdInfo m_List = null;
        private GlassApdInfoAdapter m_Adapter = null;
        private SortedList m_GlassApdDataUnitList = new SortedList();
        private List<string> m_NameList = new List<string>();
        private List<string> m_ApdNameList = new List<string>();
        #endregion

        #region Property
        public GlassApdInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public GlassApdInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }

        public List<string> NameList
        {
            get { return m_NameList; }
        }

        public List<string> ApdNameList
        {
            get { return m_ApdNameList; }
        }
        #endregion

        #region Singleton code...
        public static readonly GlassApdInfoProvider Instance = new GlassApdInfoProvider();
        #endregion

        #region Constructor
        public GlassApdInfoProvider()
        {
            m_List = new GlassApdInfo();
            m_Adapter = new GlassApdInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetApdDataInfoUnitList();
            SetNameList();
        }

        public void LoadFromDB()
        {
            lock (this)
            {
                m_Adapter.LoadFormDB(m_List);
            }
        }

        public void SaveToDB()
        {
            lock (this)
            {
                m_Adapter.SaveToDB(m_List);
            }
        }

        public int GetId(string Name)
        {
            int id = 0;
            foreach (TagGlassApdInfo info in m_List.Items)
            {
                if (info.UseData)
                {
                    id += 1;

                    if (info.DvName == Name)
                    {
                        break;
                    }
                }
            }

            return id;
        }

        public bool AddItem(int unitno, GlassApdInfo apdinfo)
        {
            try
            {
                m_GlassApdDataUnitList.Add(unitno, apdinfo);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public GlassApdInfo GetItem(int nKey)
        {
            try
            {
                if (m_GlassApdDataUnitList.Contains(nKey))
                {
                    return (GlassApdInfo)m_GlassApdDataUnitList[nKey];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection GetKeys()
        {
            try
            {
                return m_GlassApdDataUnitList.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }
/*
        public GlassApdInfo GetItem(string sKey)
        {
            GlassApdInfo apdinfo = null;

            try
            {
                foreach (GlassApdInfo info in m_GlassApdDataUnitList)
                {
                    if (info.Items[0].UnitName == sKey)
                    {
                        apdinfo = info;
                        break;
                    }
                }
            }
            catch
            {
            }

            return apdinfo;
        }
*/
        public int ApdDataInfoCount
        {
            get { return m_GlassApdDataUnitList.Count; }
        }

        public void SetApdDataInfoUnitList()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;

            foreach (TagGlassApdInfo info in m_List.Items)
            {
                if (info.UnitNo != OldUnitNo)
                {
                    UnitList.Add(info.UnitNo);
                    OldUnitNo = info.UnitNo;
                }
            }

            GlassApdInfo[] glassapddatainfounit = new GlassApdInfo[UnitList.Count];

            for (int i = 0; i < glassapddatainfounit.Length; i++)
            {
                glassapddatainfounit[i] = new GlassApdInfo();
            }

            foreach (TagGlassApdInfo info in m_List.Items)
            {
                index = UnitList.IndexOf(info.UnitNo);
                if (index != -1)
                {
                    glassapddatainfounit[index].Add(info);
                }
            }

            m_GlassApdDataUnitList.Clear();

            for (int i = 0; i < UnitList.Count; i++)
            {
                m_GlassApdDataUnitList.Add(UnitList[i], glassapddatainfounit[i]);
            } 
        }

        public void SetNameList()
        {
            int count = m_List.Count;

            m_NameList.Clear();
            m_ApdNameList.Clear();

            for (int i = 0; i < count; i++)
            {
                TagGlassApdInfo info = m_List.Items[i];

                if (info.UseData)
                {
                    m_NameList.Add(info.DvName);
                }

                if (info.HostReport)
                {
                    m_ApdNameList.Add(info.DvName);
                }
            }
        }
        #endregion
    }
}
