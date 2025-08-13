using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class CimSubUnitInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private SubUnitInfo m_List = null;
        private SubUnitInfoAdapter m_Adapter = null;

        #endregion

        #region Properties
        public SubUnitInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public SubUnitInfoAdapter Adapter
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
        public CimSubUnitInfoProvider()
        {
            m_List = new SubUnitInfo();
            m_Adapter = new SubUnitInfoAdapter();

        }

        public void Create()
        {
            LoadFromDB();
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
    }
}
