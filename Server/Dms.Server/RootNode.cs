using System;
using System.Collections.Generic;
using System.Text;
using Dms.DeviceLibrary;
using Dms.Ctl;
using Dms.Common;
using System.IO;
using System.Windows.Forms;

namespace Dms.Server
{
    public class RootNode : DmsNode
    {
        #region Fields
        private Melsec m_MelsecBoardControl = new Melsec();
		//private IfSigFromUp m_IfFromUp1 = null;
        private static AppConfig m_AppConfig = new AppConfig();
        #endregion

        #region Properties
        public Melsec MelsecBoardControl
        {
            get { return m_MelsecBoardControl; }
        }
		//public IfSigFromUp IfFromUp1
		//{
		//    get { return m_IfFromUp1; }
		//}
        #endregion

        #region Constructor
        public RootNode()
        {
            m_GetDmsNodeDirectory = CheckPath;
        }
        #endregion

        #region Methods
        public void Uninitialize()
        {
            m_MelsecBoardControl.Uninitialize();
        }
        #endregion

        #region Override
        public override bool Initialize()
        {
            if(!m_CreateMode) ReadConfiguration(m_Config, this.GetPathName());

            int i = 0;

            m_MelsecBoardControl.AddNode(this, i++, "Melsec").Initialize();
            m_MelsecBoardControl.Open((short)channel.melsecnetG_Slot1, 0);

			//m_IfFromUp1 = new IfSigFromUp(m_MelsecBoardControl);
			//m_IfFromUp1.AddNode(this, i++, "IfFromUp1").Initialize();

            return base.Initialize();
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            RootNode gm;
            object obj = new object();

            if (dss.ReadXml(ref obj, typeof(RootNode), filename))
            {
                gm = (RootNode)obj;
            }
        }

        public override void WriteConfiguration()
        {
            m_Config.WriteXml(this, typeof(RootNode), GetPathName());
        }

        protected bool CheckPath(ref string path)
        {
            m_AppConfig.ReadXml();

            string filePath = m_AppConfig.MelsecConfigurationPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Melsec Configuration Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = Application.StartupPath;
                dlg.Description = "Melsec Configuraion Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.MelsecConfigurationPath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    path = filePath;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                path = filePath;
                return true;
            }
        }

        public override void GetDmsNodeNameList(TreeNode node, List<DmsNodeTag> list, Type type)
        {
            DmsNode tagNode = node.Tag as DmsNode;

            if (tagNode != null)
            {
                if (type == typeof(DmsNode))
                {
                    DmsNodeTag tag = new DmsNodeTag();
                    tag.Name = tagNode.GetOriginalName();
                    list.Add(tag);
                }
                else if (tagNode.GetType() == type)
                {
                    DmsNodeTag tag = new DmsNodeTag();
                    tag.Name = tagNode.GetOriginalName();
                    list.Add(tag);
                }
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                GetDmsNodeNameList(childNode, list, type);
            }
        }
        #endregion
    }
}
