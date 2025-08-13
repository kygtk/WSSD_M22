using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.DeviceLibrary
{
    public abstract class DmsModbusNode : DmsNode
    {
        private static AppConfig m_AppConfig = AppConfig.Instance;

        public DmsModbusNode()
        {
            m_GetDmsNodeDirectory = CheckPath;
        }

        protected bool CheckPath(ref string path)
        {
            string filePath = m_AppConfig.ModbusConfigurationPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Modbus Configuration Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = Application.StartupPath;
                dlg.Description = "Modbus Configuraion Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.ModbusConfigurationPath.SelectedFolder = filePath;
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
    }
}
