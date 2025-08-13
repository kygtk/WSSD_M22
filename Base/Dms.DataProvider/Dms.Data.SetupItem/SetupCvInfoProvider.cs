using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupCvInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private SetupCvInfoList m_List = null;
        private SetupCvInfoAdapter m_Adapter = null;
        private List<ViewSetupCvInfo> m_Viewer = new List<ViewSetupCvInfo>();
        #endregion

        #region Properties
        public SetupCvInfoList List
        {
            get { return m_List; }
        }
        public SetupCvInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public List<ViewSetupCvInfo> Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly SetupCvInfoProvider Instance = new SetupCvInfoProvider();
        #endregion

        #region Constructor
        private SetupCvInfoProvider()
        {
            m_Adapter = new SetupCvInfoAdapter();
            m_List = new SetupCvInfoList();
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
            lock (m_LockKey)
            {
                m_Adapter.DeleteGarbage(m_List);
            }
        }


        public bool InitFromDB(TagSetupCvInfo info)
        {
            lock (m_LockKey)
            {
                m_Adapter.InitFromDB(info);
                m_List.InitItem(info);
            }

            return true;
        }

        public bool LoadFromDB()
        {
            lock (m_LockKey)
            {
                return m_Adapter.LoadFromDB(m_List);
            }
        }

        public bool UpdateFromDB(string name)
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFromDB();

                TagSetupCvInfo info = new TagSetupCvInfo();
                if (m_Adapter.UpdateFromDB(name, info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public bool UpdateToDB(TagSetupCvInfo info)
        {
            lock (m_LockKey)
            {
                if (m_Adapter.UpdateToDB(info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public void UpdateToDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.SaveToDB();
                m_Adapter.UpdateFromDB(m_List);
            }
        }

        public void SetEditPermission(UserLevels curUserLevel)
        {
            if (m_Viewer != null)
            {
                foreach (ViewSetupCvInfo view in m_Viewer)
                {
                    view.SetEditable(curUserLevel);
                }
            }
        }

        public void RejectChanges()
        {
            lock (m_LockKey)
            {
                m_Adapter.RejectChanges();
            }
        }
        #endregion
    }
}
