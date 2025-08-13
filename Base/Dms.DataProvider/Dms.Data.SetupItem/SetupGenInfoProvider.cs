using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class SetupGenInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private SetupGenInfoList m_List = null;     
        private SetupGenInfoAdapter m_Adapter = null;
        private List<ViewSetupInfo> m_Viewer = new List<ViewSetupInfo>();
        #endregion

        #region Properties
        public SetupGenInfoList List
        {
            get { return m_List; }
        }
        public SetupGenInfoAdapter Adapter
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
        public static readonly SetupGenInfoProvider Instance = new SetupGenInfoProvider();
        #endregion

        #region Constructor
        private SetupGenInfoProvider()
        {
            m_Adapter = new SetupGenInfoAdapter();
            m_List = new SetupGenInfoList();
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


        public bool InitFromDB(TagSetupInfo info)
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
                foreach (ViewSetupInfo view in m_Viewer)
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

		//eun 20100402 Melsec의 Ao에 Write해주기 위해 db에 있는 setup parameter들을 ushort[] 형태로 반환받는다.
		public bool GetParameters(ref ushort[] buf)
		{
			return m_Adapter.GetParameters(ref buf);
		}

		//eun 20100402 db에 있는 setup parameter들을 List<TagSetupInfo> 형태로 반환받는다.
		public bool GetParameters(ref List<TagSetupInfo> buf)
		{
			return m_Adapter.GetParameters(ref buf);
		}
        #endregion
    }
}
