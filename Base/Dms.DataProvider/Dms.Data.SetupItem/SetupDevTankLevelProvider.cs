using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupDevTankLevelProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private SetupDevTankLevelList m_List = null;
        private SetupDevTankLevelAdapter m_Adapter = null;
        private List<ViewSetupInfo> m_Viewer = new List<ViewSetupInfo>();
        #endregion

        #region Properties
        public SetupDevTankLevelList List
        {
            get { return m_List; }
        }
        public SetupDevTankLevelAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public List<ViewSetupInfo> Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly SetupDevTankLevelProvider Instance = new SetupDevTankLevelProvider();
        #endregion

        #region Constructor
        private SetupDevTankLevelProvider()
        {
            m_Adapter = new SetupDevTankLevelAdapter();
            m_List = new SetupDevTankLevelList();
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


        public bool InitFromDB(TagSetupInfo info)
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

        public void UpdateFromDB(String name, TagSetupInfo info)  //for Tank Usercontrol
        {
            m_Adapter.UpdateFromDB(name, info);
        }

        public bool UpdateFromDB(string name)
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFromDB();

                TagSetupInfo info = new TagSetupInfo();
                if (m_Adapter.UpdateFromDB(name, info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public bool UpdateToDB(TagSetupInfo info)
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
                foreach (ViewSetupInfo view in m_Viewer)
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
