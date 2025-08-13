using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data.DataSetUnitInfoTableAdapters;
using System.Windows.Forms;

namespace Dms.Data
{
    public class CimEqpUnitInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private EqpUnitInfo m_List = null;
        private CimEqpUnitInfoAdapter m_Adapter = null;
        #endregion

        #region Properties
        public EqpUnitInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public CimEqpUnitInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        #endregion

        #region Constructor
        public CimEqpUnitInfoProvider()
        {
            m_List = new EqpUnitInfo();
            m_Adapter = new CimEqpUnitInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();
        }

        public void LoadFromDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFormDB(m_List);
            }
        }

        public void SaveToDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.SaveToDB(m_List);
            }
        }
        #endregion
    }
}
