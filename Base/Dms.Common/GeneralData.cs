using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace Dms.Common
{
    public class GeneralData
    {
        private static AppConfig m_AppConfig = AppConfig.Instance;
        private static string m_FileName = null;
        private static int m_CheckPath = -1;
        private static Mutex m_staticMutex = new Mutex();
        private static string m_Name = typeof(GeneralData).Name;

        #region Singleton
        public static readonly GeneralData Instance = new GeneralData();
        #endregion

        #region Constructor
        private GeneralData()
        {
        }
        #endregion

        #region Methods
        private static void CheckFilePath()
        {
            //jemoon : 090929 - use default path option
            string filePath = m_AppConfig.DevicePathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("General Data Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Device Folder (for GeneralData)";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.DevicePath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    m_FileName = string.Format("{0}\\{1}.ini", filePath, m_Name);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.ini", filePath, m_Name);
                m_CheckPath = 1;
            }
        }

        
        public static string GetStringValue(string section, string entry, string defaultValue)
        {
            if (m_CheckPath == -1) CheckFilePath();

            if (m_CheckPath != 1) return defaultValue;

            m_staticMutex.WaitOne();
            string rv = defaultValue;

            try
            {
                rv = XINIFile.ReadValue(section, entry, defaultValue, m_FileName);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            m_staticMutex.ReleaseMutex();
            return rv;
        }

        public static int GetIntValue(string section, string entry, int defaultValue)
        {
            if (m_CheckPath == -1) CheckFilePath();

            if (m_CheckPath != 1) return defaultValue;

            m_staticMutex.WaitOne();
            int rv = defaultValue;

            try
            {
                rv = XINIFile.ReadValue(section, entry, defaultValue, m_FileName);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            m_staticMutex.ReleaseMutex();
            return rv;
        }

        public static bool SetStringValue(string section, string entry, string value)
        {
            if (m_CheckPath == -1) CheckFilePath();

            if (m_CheckPath != 1) return false;

            m_staticMutex.WaitOne();
            bool ok = false;

            try
            {
                ok = XINIFile.WriteValue(section, entry, value, m_FileName);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            m_staticMutex.ReleaseMutex();
            return ok;
        }

        public static bool SetIntValue(string section, string entry, int value)
        {
            if (m_CheckPath == -1) CheckFilePath();

            if (m_CheckPath != 1) return false;

            m_staticMutex.WaitOne();
            bool ok = false;

            try
            {
                ok = XINIFile.WriteValue(section, entry, value, m_FileName);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            m_staticMutex.ReleaseMutex();
            return ok;
        }
        #endregion
    }

    public class XINIFile
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        public static string ReadValue(String strSection, String strKey, String strDefault, String strINIPath)
        {
            StringBuilder dstrResult = new StringBuilder(255);

            try
            {
                GetPrivateProfileString(strSection, strKey, strDefault, dstrResult, 255, strINIPath);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            return dstrResult.ToString();
        }

        public static int ReadValue(String strSection, String strKey, int nDefault, String strINIPath)
        {
            int rv = nDefault;

            try
            {
                rv = GetPrivateProfileInt(strSection, strKey, nDefault, strINIPath);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            return rv;
        }

        public static bool WriteValue(String strSection, String strKey, String strValue, String strINIPath)
        {
            try
            {
                return WritePrivateProfileString(strSection, strKey, strValue, strINIPath);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }

        public static bool WriteValue(String strSection, String strKey, int nValue, String strINIPath)
        {
            try
            {
                return WritePrivateProfileString(strSection, strKey, nValue.ToString(), strINIPath);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }
    }
}
