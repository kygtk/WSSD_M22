using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public class CimCurrentDataInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private CurrentDataInfo m_List = null;
        private CurrentDataInfoAdapter m_Adapter = null;
        private SortedList m_CurrentDataUnitList = new SortedList();
        #endregion

        #region Property
        public CurrentDataInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public CurrentDataInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        #endregion

        #region Singleton code...
        public static readonly CimCurrentDataInfoProvider Instance = new CimCurrentDataInfoProvider();
        #endregion

        #region Constructor
        public CimCurrentDataInfoProvider()
        {
            m_List = new CurrentDataInfo();
            m_Adapter = new CurrentDataInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetCurrentDataUnit();
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

            foreach (TagCurrentDataInfo info in m_List.Items)
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

            foreach (TagCurrentDataInfo info in m_List.Items)
            {
                if (info.Id == nId)
                {
                    value = info.Name;
                }
            }

            return value;
        }

        #endregion

        public bool AddItem(int nKey, CurrentDataInfo datainfo)
        {
            try
            {
                m_CurrentDataUnitList.Add(nKey, datainfo);

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

        public CurrentDataInfo GetItem(int nKey)
        {
            try
            {
                if (m_CurrentDataUnitList.Contains(nKey))
                {
                    return (CurrentDataInfo)m_CurrentDataUnitList[nKey];
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
                return m_CurrentDataUnitList.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int CurrentDataCount
        {
            get { return m_CurrentDataUnitList.Count; }
        }

        public void SetCurrentDataUnit()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;
            
           foreach( TagCurrentDataInfo info in m_List.Items)
           {
               if (info.UnitNo != OldUnitNo)
               {
                   UnitList.Add(info.UnitNo);
                   OldUnitNo = info.UnitNo;
               }
           }

           CurrentDataInfo[] currentdataunit = new CurrentDataInfo[UnitList.Count];

           for (int i = 0; i < currentdataunit.Length; i++)
           {
               currentdataunit[i] = new CurrentDataInfo();
           }

           foreach (TagCurrentDataInfo info in m_List.Items)
           {
               index = UnitList.IndexOf( info.UnitNo );
               if ( index != -1 )
               {
                   currentdataunit[index].Add(info);
               }
           }

           for (int i = 0; i < UnitList.Count; i++)
           {
               m_CurrentDataUnitList.Add(UnitList[i], currentdataunit[i]);
           }

        }

    }
}
