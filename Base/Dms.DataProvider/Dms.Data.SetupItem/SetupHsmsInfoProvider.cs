using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;

using Dms.Common;

namespace Dms.Data
{
    public class SetupHsmsInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private SetupHsmsInfoList m_List = null;
        private SetupHsmsInfoAdapter m_Adapter = null;
        private SetupHsmsInfoXml m_SecomXml = null;
        private List<ViewSetupInfo> m_Viewer = new List<ViewSetupInfo>();
        private string m_EqpName;
        private bool m_FileOpened = false;
        #endregion

        #region Properties
        public SetupHsmsInfoList List
        {
            get { return m_List; }
        }
        public SetupHsmsInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public SetupHsmsInfoXml SecomXml
        {
            get { return m_SecomXml; }
            set { m_SecomXml = value; }
        }
        public List<ViewSetupInfo> Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly SetupHsmsInfoProvider Instance = new SetupHsmsInfoProvider();
        #endregion

        #region Constructor
        private SetupHsmsInfoProvider()
        {
            m_Adapter = new SetupHsmsInfoAdapter();
            m_List = new SetupHsmsInfoList();
            m_SecomXml = new SetupHsmsInfoXml();
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

        public bool LoadFromXml(string EqpName)
        {
            m_EqpName = EqpName;
            m_FileOpened = m_SecomXml.GetParameters(EqpName);

            return m_FileOpened;
        }

        //2009.07.01 Youngsik... Copy from XML to DB
        public bool SyncFromXml()
        {
            if (m_FileOpened == false) return false;

            foreach (TagSetupInfo info in m_List.Items)
            {
                foreach (Parameter param in m_SecomXml.Parameters)
                {
                    if ((param.Name != "T9") && (info.Name == param.Description))
                    {
                        info.Val = param.Value;
                        UpdateToDB(info);
                    }
                }
            }

            return true;
        }

        //2009.07.01 Youngsik... Copy from DB to XML
        public bool SyncFromDB()
        {
            if (m_FileOpened == false) return false;

            foreach (TagSetupInfo info in m_List.Items)
            {
                foreach (Parameter param in m_SecomXml.Parameters)
                {
                    if ((param.Name != "T9") && (info.Name == param.Description))
                    {
                        param.Value = info.Val;
                        m_SecomXml.WriteValue(m_EqpName, param.Name, param.Value, 1);
                    }
                }
            }

            return true;
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
                SyncFromDB();   
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
        #endregion
    }
}
