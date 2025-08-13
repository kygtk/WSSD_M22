using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dms.DeviceLibrary;
using Dms.Ctl;
using Dms.Common;
using System.Windows.Forms;
using System.IO;

namespace Dms.Mitsubishi
{
    public class MelsecNode : DmsNode
    {
        #region Fields
        private Melsec m_Melsec = null;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Constructor
        public MelsecNode()
        {
            m_GetDmsNodeDirectory = CheckPath;
        }

        public MelsecNode(Melsec melsecBoard)
        {
            m_Melsec = melsecBoard;
            m_GetDmsNodeDirectory = CheckPath;
        }
        #endregion

        #region Methods
        private void SyncInstance(object configration)
        {
            // Property의 Type이 MelsecDevice 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(MelsecDevice), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get, set 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();
                MethodInfo setMethodInfo = info.GetSetMethod();

                // Configration file에서 현재 class로 set
                object obj = getMethodInfo.Invoke(configration, null);
                object[] parameters = new object[] { obj };
                setMethodInfo.Invoke(this, parameters);
            }
        }

        private void SetMelsecBorad(Melsec melsec)
        {
            // Property의 Type이 MelsecDevice 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(MelsecDevice), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();

                // 현재 Class의 object를 가져온다.
                MelsecDevice mel = getMethodInfo.Invoke(this, null) as MelsecDevice;
                mel.SetMelsecBoard(melsec);
            }
        }

        public override bool Initialize()
        {
            if (!m_CreateMode) ReadConfiguration(m_Config, this.GetPathName());

            SetMelsecBorad(m_Melsec);

            return base.Initialize();
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            object obj = new object();
            if (dss.ReadXml(ref obj, this.GetType(), filename))
            {
                SyncInstance(obj);
            }
        }

        public override void WriteConfiguration()
        {
            m_Config.WriteXml(this, this.GetType(), GetPathName());
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
        #endregion
    }
}
