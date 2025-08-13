using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public class CimUnitRecipeInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private UnitRecipeInfo m_List = null;
        private UnitRecipeInfoAdapter m_Adapter = null;
        private SortedList m_UnitRecipeInfoList = new SortedList();
        #endregion

        #region Property
        public UnitRecipeInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public UnitRecipeInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        #endregion

        #region Singleton code...
        public static readonly CimUnitRecipeInfoProvider Instance = new CimUnitRecipeInfoProvider();
        #endregion

        #region Constructor
        public CimUnitRecipeInfoProvider()
        {
            m_List = new UnitRecipeInfo();
            m_Adapter = new UnitRecipeInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetUnitRecipeInfo();
        }

        public void LoadFromDB()
        {
            lock(m_LockKey)
            {
                m_Adapter.LoadFormDB(m_List);
            }
        }

        public void SaveToDB()
        {
            lock(m_LockKey)
            {
                m_Adapter.SaveToDB(m_List);
            }
        }

        public int GetId(string Name)
        {
            int value = 0;

            foreach (TagUnitRecipeInfo info in m_List.Items)
            {
                if (info.Name == Name)
                {
                    value = info.Id;
                }
            }

            return value;
        }

        public string GetUnitIdToName(string sUnitId)
        {
            int nId = 0;
            string value = "";

            nId = Convert.ToInt32(sUnitId.Substring(4));

            foreach (TagUnitRecipeInfo info in m_List.Items)
            {
                if (info.Id == nId)
                {
                    value = info.Name;
                }
            }

            return value;
        }

        #endregion

        public bool AddUnitRecipeInfo(int nKey, UnitRecipeInfo datainfo)
        {
            try
            {
                m_UnitRecipeInfoList.Add(nKey, datainfo);

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

        public UnitRecipeInfo GetUnitRecipeInfo(int nKey)
        {
            try
            {
                if (m_UnitRecipeInfoList.Contains(nKey))
                {
                    return (UnitRecipeInfo)m_UnitRecipeInfoList[nKey];
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
                return m_UnitRecipeInfoList.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int CurrentUnitRecipeInfoCount
        {
            get { return m_UnitRecipeInfoList.Count; }
        }

        public void SetUnitRecipeInfo()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;
            
           foreach( TagUnitRecipeInfo info in m_List.Items)
           {
               if (info.UnitNo != OldUnitNo)
               {
                   UnitList.Add(info.UnitNo);
                   OldUnitNo = info.UnitNo;
               }
           }

           UnitRecipeInfo[] UnitRecipe = new UnitRecipeInfo[UnitList.Count];

           for (int i = 0; i < UnitRecipe.Length; i++)
           {
               UnitRecipe[i] = new UnitRecipeInfo();
           }

           foreach (TagUnitRecipeInfo info in m_List.Items)
           {
               index = UnitList.IndexOf( info.UnitNo );
               if ( index != -1 )
               {
                   UnitRecipe[index].Add(info);
               }
           }

           for (int i = 0; i < UnitList.Count; i++)
           {
               m_UnitRecipeInfoList.Add(UnitList[i], UnitRecipe[i]);
           }

        }
    }
}
