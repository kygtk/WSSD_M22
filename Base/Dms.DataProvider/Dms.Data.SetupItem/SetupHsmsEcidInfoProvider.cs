using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dms.Common;

namespace Dms.Data
{
    public class SetupHsmsEcidInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private SetupHsmsEcidInfoList m_List = null;
        private SetupHsmsEcidInfoAdapter m_Adapter = null;
        private List<ViewHsmsSetupInfo> m_Viewer = new List<ViewHsmsSetupInfo>();
        #endregion

        #region Properties
        public SetupHsmsEcidInfoList List
        {
            get { return m_List; }
        }
        public SetupHsmsEcidInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public List<ViewHsmsSetupInfo> Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly SetupHsmsEcidInfoProvider Instance = new SetupHsmsEcidInfoProvider();
        #endregion

        #region Constructor
        private SetupHsmsEcidInfoProvider()
        {
            m_Adapter = new SetupHsmsEcidInfoAdapter();
            m_List = new SetupHsmsEcidInfoList();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void Create()
        {

        }

        public void DeleteDbGarbage()
        {
            lock(m_LockKey)
            {
                m_Adapter.DeleteGarbage(m_List);
            }
        }

        public bool InitFromDB(TagHsmsSetupInfo info)
        {
            lock(m_LockKey)
            {
                m_Adapter.InitFromDB(info);
                m_List.InitItem(info);
            }

            return true;
        }

        public bool LoadFromDB()
        {
            lock(m_LockKey)
            {
                return m_Adapter.LoadFromDB(m_List);
            }
        }

        public bool UpdateFromDB(string name)
        {
            lock(m_LockKey)
            {
                m_Adapter.LoadFromDB();

                TagHsmsSetupInfo info = new TagHsmsSetupInfo();
                if (true == m_Adapter.UpdateFromDB(name, info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public bool UpdateToDB(TagHsmsSetupInfo info)
        {
            lock(m_LockKey)
            {
                if (true == m_Adapter.UpdateToDB(info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public void UpdateToDB()
        {
            lock(m_LockKey)
            {
                m_Adapter.SaveToDB();
                m_Adapter.UpdateFromDB(m_List);
            }
        }

        public void SetEditPermission(UserLevels curUserLevel)
        {
            if (m_Viewer != null)
            {
                foreach (ViewHsmsSetupInfo view in m_Viewer)
                {
                    view.SetEditable(curUserLevel);
                }
            }
        }

        public void RejectChanges()
        {
            lock(m_LockKey)
            {
                m_Adapter.RejectChanges();
            }
        }

        public bool Save(List<string> list, List<string> value)
        {
            bool bRv = true;

            m_Adapter.Save(list, value);

            return bRv;
        }
        #endregion
    }
}
