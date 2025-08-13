using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Globalization;
using System.Management;
using System.Diagnostics;

namespace Dms.Common
{
    public class XApdLog
    {
        #region enum
        public enum ApdType : int
        {
            GPD,
            LPD,
            TPD
        }
        #endregion

        #region Fields
        private static object m_LockKey = new object();

        private static int MAX_DAYS = 30;

        private string m_LogPath = "";
        private string m_DriveName = "";
        private string m_FileName = "";
        private string m_FullName = "";
        //private string m_Date = "";
        private string m_ApdType = "";
        //private DateTime m_CreateTime; //jemoon : 사용하지 않음

        private bool m_Initialized;
        private bool m_Appended = true;
        private StreamWriter m_Writer = null;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        public static bool AvailableLogStorage = true;
        private ThreadLogGarbageCollector m_ThreadGarbageCollector;
        private static int m_CheckPath = -1;
        #endregion

        #region Properties
        public string FileName
        {
            get { return m_FileName; }
        }
        public string LogPath
        {
            get { return m_LogPath; }
        }
        public string FullName
        {
            get { return m_FullName; }
        }
        #endregion

        #region Constructor
        public XApdLog(ApdType apdType)
        {
            m_LogPath = m_AppConfig.ApdLogPath.ToString();

            if (m_CheckPath == -1) CheckLogPath();

            if (m_CheckPath == 1)   //경로 지정이 성공한 경우에만
            {
                m_DriveName = m_LogPath.Substring(0, 2); //HDD 용량을 Check하기 위해
                m_ApdType = apdType.ToString();
                m_LogPath = m_DriveName + "\\" + m_ApdType + "S";

                MAX_DAYS = m_AppConfig.ApdLogDays;

                //jemoon : 090916 - Log 삭제 기준에 문제가 있어 수정함
                if (m_ThreadGarbageCollector == null)
                {
                    m_ThreadGarbageCollector = ThreadLogGarbageCollector.Instance;
                    m_ThreadGarbageCollector.AddDirectory(m_LogPath, "", MAX_DAYS);
                }
            }
        }

        ~XApdLog()
        {
            if (m_ThreadGarbageCollector != null)
                m_ThreadGarbageCollector.Pause();
        }
        #endregion

        #region Methods
        private void CheckLogPath()
        {
            if (Directory.Exists(m_LogPath) == false)
            {
                MessageBox.Show("APD Log Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "APD Log Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_LogPath = dlg.SelectedPath;
                    m_AppConfig.ApdLogPath.SelectedFolder = m_LogPath;
                    m_AppConfig.WriteXml();
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_CheckPath = 1;
            }
        }

        public bool NewFile(string fileName)
        {
            try
            {
                if (m_CheckPath != 1) return false;

                DateTime curTime = DateTime.Now;
                string path = string.Format("{0}\\{1}\\{2}", m_LogPath, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"));

                string date = string.Format("{0:d04}{1:d02}{2:d02}{3:d02}{4:d02}{5:d02}{6:d02}",
                                            curTime.Year,
                                            curTime.Month,
                                            curTime.Day,
                                            curTime.Hour,
                                            curTime.Minute,
                                            curTime.Second,
                                            curTime.Millisecond / 10);

                DriveInfo[] allDrive = DriveInfo.GetDrives();
                foreach (DriveInfo drive in allDrive)
                {
                    if (drive.Name.Substring(0, 2) == m_DriveName)
                    {
                        if (!drive.IsReady) break;

                        long totalSize = drive.TotalSize;
                        long availableSize = drive.AvailableFreeSpace;
                        double freeRatio = (double)availableSize / (double)totalSize * 100;

                        // freesize가 설정치 이상이면 OK
                        if (freeRatio > m_AppConfig.StorageAvailableRatio)
                        {
                            AvailableLogStorage = true;
                        }
                        else
                        {
                            AvailableLogStorage = false;
                        }
                    }
                }

                if (!AvailableLogStorage)
                {
                    CloseStream();
                }
                else
                {
                    Directory.CreateDirectory(path);

                    m_FullName = string.Format("{0}\\{1}_{2}.{3}", path, fileName, date, m_ApdType);
                    m_FileName = string.Format("{0}_{1}.{2}", fileName, date, m_ApdType);

                    CreateStream();
                }

                //RemoveExpiredFile();

                m_Initialized = AvailableLogStorage;
                return AvailableLogStorage;
            }
            catch  //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                AvailableLogStorage = false;
                m_Initialized = AvailableLogStorage;
                return false;
            }
        }

        private void CreateStream()
        {
            CloseStream();

            m_Writer = new StreamWriter(m_FullName, m_Appended, Encoding.Default);
        }

        public void CloseStream()
        {
            if (m_Writer != null && m_Writer.BaseStream != null)
            {
                m_Writer.Close();
            }
        }

        public void TextOut(string message)
        {
            lock (m_LockKey)
            {
                try
                {
                    if (m_CheckPath != 1) return;

                    if (!m_Initialized) return;

                    if (message != null)
                    {
                        m_Writer.WriteLine(message);
                        m_Writer.Flush();
                    }
                }
                catch
                {
                    //ToDo : 임시처리
                    m_Initialized = false;
                }
            }
        }
        #endregion
    }
}
