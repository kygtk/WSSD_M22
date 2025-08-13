using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public class CimUnitInitialDataInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private UnitInitialDataInfo m_List = null;
        private UnitInitialDataInfoAdapter m_Adapter = null;
        private SortedList m_UnitInitialDataInfoList = new SortedList();
        #endregion

        #region Property
        public UnitInitialDataInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public UnitInitialDataInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }

/*
        public static String CurrentDBPath
        {
            get
            {
                // use reflection to find the location of the config file
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                return DBPath.GetCurrentPath(asm, "Dms.Data.Properties.Settings.DmsProcessDataConnectionString");
            }
            set
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                DBPath.SetCurrentPath(asm, "Dms.Data.Properties.Settings.DmsProcessDataConnectionString", value);
            }
        }
 */ 

        #endregion

        #region Constructor
        public CimUnitInitialDataInfoProvider()
        {
            m_List = new UnitInitialDataInfo();
            m_Adapter = new UnitInitialDataInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetUnitInitialDataInfo();
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

            foreach (TagUnitInitialDataInfo info in m_List.Items)
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

            foreach (TagUnitInitialDataInfo info in m_List.Items)
            {
                if (info.Id == nId)
                {
                    value = info.Name;
                }
            }

            return value;
        }

        #endregion

        public bool AddUnitInitialDataInfo(int nKey, UnitInitialDataInfo datainfo)
        {
            try
            {
                m_UnitInitialDataInfoList.Add(nKey, datainfo);

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

        public UnitInitialDataInfo GetUnitInitialDataInfo(int nKey)
        {
            try
            {
                if (m_UnitInitialDataInfoList.Contains(nKey))
                {
                    return (UnitInitialDataInfo)m_UnitInitialDataInfoList[nKey];
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
                return m_UnitInitialDataInfoList.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int CurrentUnitInitialDataInfoCount
        {
            get { return m_UnitInitialDataInfoList.Count; }
        }

        public void SetUnitInitialDataInfo()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;
            
           foreach( TagUnitInitialDataInfo info in m_List.Items)
           {
               if (info.UnitNo != OldUnitNo)
               {
                   UnitList.Add(info.UnitNo);
                   OldUnitNo = info.UnitNo;
               }
           }

           UnitInitialDataInfo[] UnitInitialData = new UnitInitialDataInfo[UnitList.Count];

           for (int i = 0; i < UnitInitialData.Length; i++)
           {
               UnitInitialData[i] = new UnitInitialDataInfo();
           }

           foreach (TagUnitInitialDataInfo info in m_List.Items)
           {
               index = UnitList.IndexOf( info.UnitNo );
               if ( index != -1 )
               {
                   UnitInitialData[index].Add(info);
               }
           }

           for (int i = 0; i < UnitList.Count; i++)
           {
               m_UnitInitialDataInfoList.Add(UnitList[i], UnitInitialData[i]);
           }

        }
    }
}
