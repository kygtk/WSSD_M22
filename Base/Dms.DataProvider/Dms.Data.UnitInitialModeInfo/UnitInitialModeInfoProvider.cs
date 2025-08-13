using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public class CimUnitInitialModeInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private UnitInitialModeInfo m_List = null;
        private UnitInitialModeInfoAdapter m_Adapter = null;
        private SortedList m_UnitInitialModeInfoList = new SortedList();
        #endregion

        #region Property
        public UnitInitialModeInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public UnitInitialModeInfoAdapter Adapter
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
        public CimUnitInitialModeInfoProvider()
        {
            m_List = new UnitInitialModeInfo();
            m_Adapter = new UnitInitialModeInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetUnitInitialModeInfo();
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

            foreach (TagUnitInitialModeInfo info in m_List.Items)
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

            foreach (TagUnitInitialModeInfo info in m_List.Items)
            {
                if (info.Id == nId)
                {
                    value = info.Name;
                }
            }

            return value;
        }

        #endregion

        public bool AddInfo(int nKey, UnitInitialModeInfo datainfo)
        {
            try
            {
                m_UnitInitialModeInfoList.Add(nKey, datainfo);

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

        public UnitInitialModeInfo GetInfo(int nKey)
        {
            try
            {
                if (m_UnitInitialModeInfoList.Contains(nKey))
                {
                    return (UnitInitialModeInfo)m_UnitInitialModeInfoList[nKey];
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
                return m_UnitInitialModeInfoList.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int CurrentUnitInitialModeInfoCount
        {
            get { return m_UnitInitialModeInfoList.Count; }
        }

        public void SetUnitInitialModeInfo()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;
            
           foreach( TagUnitInitialModeInfo info in m_List.Items)
           {
               if (info.UnitNo != OldUnitNo)
               {
                   UnitList.Add(info.UnitNo);
                   OldUnitNo = info.UnitNo;
               }
           }

           UnitInitialModeInfo[] UnitInitialMode = new UnitInitialModeInfo[UnitList.Count];

           for (int i = 0; i < UnitInitialMode.Length; i++)
           {
               UnitInitialMode[i] = new UnitInitialModeInfo();
           }

           foreach (TagUnitInitialModeInfo info in m_List.Items)
           {
               index = UnitList.IndexOf( info.UnitNo );
               if ( index != -1 )
               {
                   UnitInitialMode[index].Add(info);
               }
           }

           for (int i = 0; i < UnitList.Count; i++)
           {
               m_UnitInitialModeInfoList.Add(UnitList[i], UnitInitialMode[i]);
           }

        }
    }
}
