using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupRobotInfoProvider
    {
        #region Fields
        private SetupRobotInfoList m_List = null;     
        private SetupRobotInfoAdapter m_Adapter = null;
        private List<ViewSetupInfo> m_Viewer = new List<ViewSetupInfo>();
        #endregion

        #region Properties
        public SetupRobotInfoList List
        {
            get { return m_List; }
        }
        public SetupRobotInfoAdapter Adapter
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
        public static readonly SetupRobotInfoProvider Instance = new SetupRobotInfoProvider();
        #endregion

        #region Constructor
        private SetupRobotInfoProvider()
        {
            m_Adapter = new SetupRobotInfoAdapter();
            m_List = new SetupRobotInfoList();
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
            lock (this)
            {
                m_Adapter.DeleteGarbage(m_List);
            }
        }


        public bool InitFromDB(TagSetupInfo info)
        {
            lock (this)
            {
                m_Adapter.InitFromDB(info);
                m_List.InitItem(info);
            }

            return true;
        }

        public bool LoadFromDB()
        {
            lock (this)
            {
                return m_Adapter.LoadFromDB(m_List);
            }
        }

        public bool UpdateFromDB(string name)
        {
            lock (this)
            {
                m_Adapter.LoadFromDB();

                TagSetupInfo info = new TagSetupInfo();
                if (true == m_Adapter.UpdateFromDB(name, info))
                {
                    return m_List.UpdateToList(info);
                }
            }

            return false;
        }

        public bool UpdateToDB(TagSetupInfo info)
        {
            lock (this)
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
            lock (this)
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
            lock (this)
            {
                m_Adapter.RejectChanges();
            }
        }
        #endregion
    }
}
