using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Data
{
    public class CimUnitInitialDataTypeInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private UnitInitialDataTypeInfo m_List = null;
        private UnitInitialDataTypeInfoAdapter m_Adapter = null;
        private SortedList m_UnitInitialDataTypeInfoList = new SortedList();
        #endregion

        #region Property
        public UnitInitialDataTypeInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public UnitInitialDataTypeInfoAdapter Adapter
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
        public CimUnitInitialDataTypeInfoProvider()
        {
            m_List = new UnitInitialDataTypeInfo();
            m_Adapter = new UnitInitialDataTypeInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
            SetUnitInitialDataTypeInfo();
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

        public bool AddInfo(int nKey, UnitInitialDataTypeInfo datainfo)
        {
            try
            {
                m_UnitInitialDataTypeInfoList.Add(nKey, datainfo);

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

        public UnitInitialDataTypeInfo GetInfo(int nKey)
        {
            try
            {
                if (m_UnitInitialDataTypeInfoList.Contains(nKey))
                {
                    return (UnitInitialDataTypeInfo)m_UnitInitialDataTypeInfoList[nKey];
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
                return m_UnitInitialDataTypeInfoList.Keys;
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
            get { return m_UnitInitialDataTypeInfoList.Count; }
        }

        public void SetUnitInitialDataTypeInfo()
        {
            List<int> UnitList = new List<int>();
            int OldUnitNo = -1;
            int index = -1;
            
           foreach( TagUnitInitialDataTypeInfo info in m_List.Items)
           {
               if (info.UnitNo != OldUnitNo)
               {
                   UnitList.Add(info.UnitNo);
                   OldUnitNo = info.UnitNo;
               }
           }

           UnitInitialDataTypeInfo[] UnitInitialDataType = new UnitInitialDataTypeInfo[UnitList.Count];

           for (int i = 0; i < UnitInitialDataType.Length; i++)
           {
               UnitInitialDataType[i] = new UnitInitialDataTypeInfo();
           }

           foreach (TagUnitInitialDataTypeInfo info in m_List.Items)
           {
               index = UnitList.IndexOf( info.UnitNo );
               if ( index != -1 )
               {
                   UnitInitialDataType[index].Add(info);
               }
           }

           for (int i = 0; i < UnitList.Count; i++)
           {
               m_UnitInitialDataTypeInfoList.Add(UnitList[i], UnitInitialDataType[i]);
           }

        }
    }
}
