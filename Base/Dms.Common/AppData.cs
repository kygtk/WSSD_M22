///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.04.10
// Author       : jemoon
// Description  : AppData
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Threading;

namespace Dms.Common
{
    public class AppData
    {
        #region Fields
        private static bool m_Initialized = false;
        private static string m_File;
        private static long m_TotalGlassCount = 0;
        private static long m_CurGlassCount = 0;        
        private static short m_ApdReportIndex = 0;
        private static int m_CheckPath = -1;
        private static Mutex m_Mutex = new Mutex();
        #endregion

        #region Properties
        public long TotalGlassCount
        {
            get { return m_TotalGlassCount; }
            set { m_TotalGlassCount = value; }
        }
        public long CurGlassCount
        {
            get { return m_CurGlassCount; }
            set { m_CurGlassCount = value; }
        }
        public short ApdReportIndex
        {
            get { return m_ApdReportIndex; }
            set { m_ApdReportIndex = value; }
        } 
        #endregion

        #region Singleton code...
        public static readonly AppData Instance = new AppData();
        #endregion

        #region Constructor
        //jemoon : Singleton 으로 구현되어야 하는데... 
        public AppData()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;
                ReadXml();
            }
        } 
        #endregion

        #region Methods
        public void ReadXml(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                m_File = fileName;
            }
            else
            {
                AppData app = new AppData();
                //app.WriteXml();
                app.WriteXml(fileName);
            }

            StreamReader sr = new StreamReader(m_File);
            XmlSerializer xmlSer = new XmlSerializer(typeof(AppData));
            AppData config = xmlSer.Deserialize(sr) as AppData;
            sr.Close();
        }

        public bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                ReadXml(m_File);
                return true;
            }
            else return false;
        }

        Mutex mux = new Mutex();
        public void WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());
 
            try
            {
                mux.WaitOne();

                // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                if (sw != null) sw.Close();

                XFunc.ExceptionHandler.Add(err);

                mux.ReleaseMutex();

                return;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.CopyTo(fileName + ".old", true);
                }

                sw = new StreamWriter(fileName);
                xmlSer.Serialize(sw, this);
                sw.Close();
                m_File = fileName;
                mux.ReleaseMutex();
            }
            catch
            {
                //ToDo:임시처리
                //System.Windows.Forms.MessageBox.Show(err.ToString());
                mux.ReleaseMutex();
            }
        }

        public void WriteXml()
        {
            m_Mutex.WaitOne();

            if (m_CheckPath == 1 && !string.IsNullOrEmpty(m_File))
            {
                FileInfo fileInfo = new FileInfo(m_File);
                if (fileInfo.Exists)
                {
                    WriteXml(m_File);
                }
            }

            m_Mutex.ReleaseMutex();
        }

        private void CheckPath()
        {
            AppConfig appConfig = AppConfig.Instance;
            appConfig.ReadXml();

            //jemoon : 090929 - use default path option
            string filePath = appConfig.AppDataPathName;

            if (appConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("AppData Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "AppData File Folder";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    appConfig.AppDataPath.SelectedFolder = filePath;
                    appConfig.WriteXml();

                    m_File = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_File = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                m_CheckPath = 1;
            }
        }
        #endregion
    }
}
