using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public class CimUnitRecipeTypeInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private UnitRecipeTypeInfo m_List = null;
        private UnitRecipeTypeInfoAdapter m_Adapter = null;
        private SortedList m_UnitRecipeTypeInfoList = new SortedList();
        #endregion

        #region Property
        public UnitRecipeTypeInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public UnitRecipeTypeInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        #endregion

        #region
        public static readonly CimUnitRecipeTypeInfoProvider Instance = new CimUnitRecipeTypeInfoProvider();
        #endregion

        #region Constructor
        public CimUnitRecipeTypeInfoProvider()
        {
            m_List = new UnitRecipeTypeInfo();
            m_Adapter = new UnitRecipeTypeInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetUnitRecipeTypeInfo();
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
        #endregion

        public bool AddInfo(int nKey, UnitRecipeTypeInfo datainfo)
        {
            try
            {
                m_UnitRecipeTypeInfoList.Add(nKey, datainfo);

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

        public UnitRecipeTypeInfo GetInfo(int nKey)
        {
            try
            {
                if (m_UnitRecipeTypeInfoList.Contains(nKey))
                {
                    return (UnitRecipeTypeInfo)m_UnitRecipeTypeInfoList[nKey];
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
                return m_UnitRecipeTypeInfoList.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int GetInfoCount
        {
            get { return m_UnitRecipeTypeInfoList.Count; }
        }

        public void SetUnitRecipeTypeInfo()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;
            
           foreach( TagUnitRecipeTypeInfo info in m_List.Items)
           {
               if (info.UnitNo != OldUnitNo)
               {
                   UnitList.Add(info.UnitNo);
                   OldUnitNo = info.UnitNo;
               }
           }

           UnitRecipeTypeInfo[] UnitRecipeType = new UnitRecipeTypeInfo[UnitList.Count];

           for (int i = 0; i < UnitRecipeType.Length; i++)
           {
               UnitRecipeType[i] = new UnitRecipeTypeInfo();
           }

           foreach (TagUnitRecipeTypeInfo info in m_List.Items)
           {
               index = UnitList.IndexOf( info.UnitNo );
               if ( index != -1 )
               {
                   UnitRecipeType[index].Add(info);
               }
           }

           for (int i = 0; i < UnitList.Count; i++)
           {
               m_UnitRecipeTypeInfoList.Add(UnitList[i], UnitRecipeType[i]);
           }

        }

    }
}
