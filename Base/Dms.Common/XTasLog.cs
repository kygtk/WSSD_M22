using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Globalization;
using System.Management;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Dms.Common
{
    public class XTasLog
    {
        #region enum
        public enum TasType : int
        {
            XML
        }
        #endregion

        #region Fields
        private static object m_LockKey = new object();

        private static int MAX_DAYS = 10;

        private string m_TasLogPath = "";
        private string m_TasDriveName = "";
        private string m_TasFileName = "";
        private string m_TasFullName = "";
        private string m_TasType = "";
        private string m_Date = "";
        private string m_SubMachineId;

        private bool m_Initialized;
        private bool m_Appended = true;
        private StreamWriter m_Writer = null;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        public static bool AvailableLogStorage = true;
        private static ThreadLogGarbageCollector m_ThreadGarbageCollector;
        private bool m_IsClosed = false;
        private static int m_CheckPath = -1;
        #endregion

        #region Properties
        public string TasFileName
        {
            get { return m_TasFileName; }
        }
        public string TasLogPath
        {
            get { return m_TasLogPath; }
        }
        public string TasFullName
        {
            get { return m_TasFullName; }
        }
        public string SubMachineId
        {
            get { return m_SubMachineId; }
            set { m_SubMachineId = value; }
        }
        #endregion

        #region Constructor
        public XTasLog(TasType TasType)
        {
            m_TasLogPath = m_AppConfig.TasLogPath.ToString();

            if (m_CheckPath == -1) CheckLogPath();

            if (m_CheckPath == 1)   //경로 지정이 성공한 경우에만
            {
                m_TasDriveName = m_TasLogPath.Substring(0, 2);
                m_TasType = TasType.ToString();
                //jemoon : 사양서 상에 TAS log는 Root:\TAS로 고정                    
                m_TasLogPath = m_TasDriveName + "\\TAS";

                MAX_DAYS = m_AppConfig.TasLogDays;

                //jemoon : 090916 - Log 삭제 기준에 문제가 있어 수정함
                if (m_ThreadGarbageCollector == null)
                {
                    m_ThreadGarbageCollector = ThreadLogGarbageCollector.Instance;
                    m_ThreadGarbageCollector.AddDirectory(m_TasLogPath, "", MAX_DAYS);
                }
            }
        }

        ~XTasLog()
        {
            if (m_ThreadGarbageCollector != null)
                m_ThreadGarbageCollector.Pause();
        }
        #endregion

        #region Methods
        private void CheckLogPath()
        {
            if (Directory.Exists(m_TasLogPath) == false)
            {
                MessageBox.Show("TAS Log Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "TAS Log Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_TasLogPath = dlg.SelectedPath;
                    m_AppConfig.TasLogPath.SelectedFolder = m_TasLogPath;
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

        public bool NewFile(string eqpName)
        {
            try
            {
                if (m_CheckPath != 1) return false;

                m_SubMachineId = eqpName;
                DateTime curTime = DateTime.Now;
                //m_TasLogPath = string.Format("{0}\\TAS\\{1}", m_TasDriveName, curTime.ToString("yyyyMMdd"));
                string path = string.Format("{0}\\{1}", m_TasLogPath, curTime.ToString("yyyyMMdd"));

                //string date = string.Format("{0:d04}{1:d02}{2:d02}{3:d02}{4:d02}{5:d02}{6:d02}",
                string date = string.Format("{0:d04}{1:d02}{2:d02}{3:d02}{4:d02}",
                                            curTime.Year,
                                            curTime.Month,
                                            curTime.Day,
                                            curTime.Hour,
                                            curTime.Minute);
                //curTime.Second,
                //curTime.Millisecond / 10);

                DriveInfo[] allDrive = DriveInfo.GetDrives();
                foreach (DriveInfo drive in allDrive)
                {
                    if (drive.Name.Substring(0, 2) == m_TasDriveName)
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

                    m_TasFullName = string.Format("{0}\\{1}_{2}.{3}", path, m_SubMachineId, date, m_TasType);
                    m_TasFileName = string.Format("{0}_{1}.{2}", m_SubMachineId, date, m_TasType);
                    m_Date = curTime.ToString("MMdd");

                    CreateStream();

                    Start();

                    m_IsClosed = false;
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

            m_Writer = new StreamWriter(m_TasFullName, m_Appended, Encoding.Default);
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

                    //if (m_Writer == null)
                    //{
                    //    if (NewFile(SubMachineId) != true)
                    //    {
                    //        // TOTO : error
                    //    }
                    //}

                    if (m_IsClosed) return;

                    if (!m_Initialized) return;

                    //date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    string buf;

                    if (message != null)
                    {
                        buf = string.Format("<DATA {0} />", message);
                        //message = date + "\t" + message;

                        m_Writer.WriteLine(buf);
                        m_Writer.Flush();
                        //m_Writer.Close();
                    }
                }
                catch
                {
                    //ToDo : 임시처리
                    m_Initialized = false;
                }
            }
        }

        private void Start()
        {
            if (m_Writer == null) return;

            string buf = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone='yes'?>";
            m_Writer.WriteLine(buf);
            buf = "<TAS>";
            m_Writer.WriteLine(buf);
            m_Writer.Flush();
        }

        public void Close()
        {
            if (m_Writer == null) return;

            m_IsClosed = true;

            string buf = "</TAS>";
            m_Writer.WriteLine(buf);
            m_Writer.Flush();
            m_Writer.Close();
        }

        public void UnInitialize()
        {
            Close();
        }
        #endregion
    }
}
