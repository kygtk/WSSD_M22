///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : XLog
///////////////////////////////////////////////////////////////////////////
// * Revision history
// * Append or overwrite : jemoon, 2009.03.13
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Globalization;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace Dms.Common
{
    public static class HmiLog
    {
        private static XLog m_Logger = new XLog("HmiLog", XLog.LogStampType.UseStamp);
        public static void WriteLog(string log)
        {
            string msg = string.Format("HMI  \t{0}", log);
            m_Logger.TextOut(msg);
        }
    }

    public static class ExceptionLog
    {
        private static XLog m_Logger = new XLog("ExceptionLog", XLog.LogStampType.UseStamp);

        public static void WriteLog(string log)
        {
            string msg = string.Format("EXCEPTION\t{0}", log);
            m_Logger.TextOut(msg);
        }
    }

    public static class XSimLog
    {
        private const int m_MaxLines = 100;

        public static ListBox LogList = new ListBox();

        public static void WriteLog(string log)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

            if (!string.IsNullOrEmpty(log))
            {
                string message = date + "\t" + log;

                if (LogList.Items.Count > m_MaxLines) LogList.Items.RemoveAt(0);

                LogList.Items.Add(message);
            }
        }
    }

    public class XLog
    {
        public enum LogStampType : int
        {
            NoUseStamp,
            UseStamp
        }

        private static object m_LockKey = new object();

        private static int MAX_DAYS = 30;
        private static string SUB_DIR_KEY = "DmsLog";

        private string m_LogPath = "";
        private string m_DriveName = "";
        private string m_FileName = "";
        private string m_FullName = "";
        private string m_Date = "";

        private bool m_Initialized;
        private bool m_Appended = true;
        private StreamWriter m_Writer = null;
        private LogStampType m_StampType;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        public static bool AvailableLogStorage = true;
        private static ThreadLogGarbageCollector m_ThreadGarbageCollector;
        private static int m_CheckPath = -1;
        private bool m_ValidFileName = false;   //2009.08.07 Youngsik

        public XLog(string fileName, LogStampType stampType)
        {
            //jemoon : 090929 - Default file path option
            m_LogPath = m_AppConfig.EqpLogPathName;

            if (m_CheckPath == -1) CheckLogPath();

            if (m_CheckPath == 1)   //경로 지정이 성공한 경우에만
            {
                m_ValidFileName = CheckFileName(fileName);  //2009.08.07 Youngsik

                m_DriveName = m_LogPath.Substring(0, 2);

                m_StampType = stampType;
                m_FileName = fileName;

                MAX_DAYS = m_AppConfig.EqpLogDays;

                //jemoon : 090916 - Log 삭제 기준에 문제가 있어 수정함
                if (m_ThreadGarbageCollector == null)
                {
                    m_ThreadGarbageCollector = ThreadLogGarbageCollector.Instance;
                    m_ThreadGarbageCollector.AddDirectory(m_LogPath, SUB_DIR_KEY, MAX_DAYS);
                }
            }
        }

        private void CheckLogPath()
        {
            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(m_LogPath);
            }

            if (Directory.Exists(m_LogPath) == false)
            {
                MessageBox.Show("EQP Log Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "EQP Log Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_LogPath = dlg.SelectedPath;
                    m_AppConfig.EqpLogPath.SelectedFolder = m_LogPath;
                    if (m_AppConfig.WriteXml())
                        m_CheckPath = 1;
                    else
                        m_CheckPath = 0;
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

        //2009.08.07 Youngsik for validation check of file name.
        private bool CheckFileName(string fileName)
        {
            char[] ch = Path.GetInvalidFileNameChars();

            for (int i = 0; i < ch.Length; i++)
            {
                int find = fileName.IndexOf(ch[i]);
                if (find >= 0)
                {
                    string message = string.Format("{0} is invalid file name. Check this name.", fileName);
                    MessageBox.Show(message);

                    return false;
                }
            }

            return true;
        }

        public XLog(string fileName, LogStampType stampType, bool appended)
            : this(fileName, stampType)
        {
            m_Appended = appended;
        }

        ~XLog()
        {
            if (m_ThreadGarbageCollector != null)
                m_ThreadGarbageCollector.Pause();
        }

        public bool NewFile()
        {
            try
            {
                if (m_CheckPath != 1) return false;

                //2009.08.07 Youngsik
                if (m_ValidFileName == false) return false;

                string path;
                DateTime curTime = DateTime.Now;
                path = string.Format(m_LogPath + "\\" + SUB_DIR_KEY + "[{0}]", curTime.ToString("yyyyMMdd"));

                #region Test
                // 아래의 방법으론 약간의 문제가 있다.
                // Query를 정확히 해주지 않으면 매번 모든 Derive를 검색해야 하는데,
                // Query문 작성법 아직 모르겠슴.
                //string filter = "WHERE DriveType="+((int)DriveType.Fixed).ToString(); 
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_logicaldisk " + filter);
                //foreach (ManagementObject disk in searcher.Get())
                //{
                //    string deviceId = Convert.ToString(disk["DeviceID"]);
                //    int driveType = Convert.ToInt32(disk["DriveType"]);
                //    if (deviceId == m_DriveName)
                //    {
                //        Int64 totalSize = Convert.ToInt64(disk["Size"]);
                //        Int64 freeSize = Convert.ToInt64(disk["FreeSpace"]);
                //        double freeSizeRatio = (double)freeSize / (double)totalSize * 100;
                //        if (freeSizeRatio > 30)
                //        {
                //            find = true;
                //            //break;
                //        }
                //        else
                //        {
                //            //string temp;
                //            //temp = string.Format("{0:F1}% Free", freeRatio);
                //            //MessageBox.Show(temp);
                //        }
                //    }
                //} 
                #endregion

                DriveInfo[] allDrive = DriveInfo.GetDrives();
                foreach (DriveInfo drive in allDrive)
                {
                    if (drive.Name.Substring(0, 2) == m_DriveName)
                    {
                        if (!drive.IsReady) break;

                        long totalSize = drive.TotalSize;
                        //long freeSize = drive.TotalFreeSpace;
                        long availableSize = drive.AvailableFreeSpace;
                        double freeRatio = (double)availableSize / (double)totalSize * 100;

                        // freesize가 설정치 이상이면 OK
                        if (freeRatio > m_AppConfig.StorageAvailableRatio)
                        {
                            AvailableLogStorage = true;
                            //break;
                        }
                        else
                        {
                            AvailableLogStorage = false;
                            //string temp;
                            //temp = string.Format("{0:F1}% Free", freeRatio);
                            //MessageBox.Show(temp);
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

                    m_FullName = string.Format("{0}\\{1}{2}.log", path, m_FileName, curTime.ToString("yyyyMMdd"));
                    m_Date = curTime.ToString("MMdd");

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

        private void CloseStream()
        {
            if (m_Writer != null && m_Writer.BaseStream != null)
            {
                m_Writer.Close();
            }
        }

        #region No Use
        //private void RemoveExpiredFile()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(m_LogPath + "\\Log");   //find the Log folder

        //    if (dir.Exists) //jemoon : directory가 있을때만 remove하자
        //    {
        //        DirectoryInfo[] directories = dir.GetDirectories("Log*");   //find the Log[DATE] folders under the Log folder

        //        DeleteDirectories(directories);
        //    }
        //}

        //private void DeleteDirectories(DirectoryInfo[] directories)
        //{
        //    DateTime curtime = DateTime.Now;
        //    int count = directories.Length;
        //    for (int i = count - 1; i >= 0; i--)
        //    {
        //        TimeSpan diff = curtime - directories[i].LastWriteTime;
        //        if (diff.Days > MAX_DAYS)
        //        {
        //            //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        //            Trace.AutoFlush = true;
        //            Trace.Indent();
        //            Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " : Entering - " + m_FileName);

        //            DirectoryInfo[] subDirectories = directories[i].GetDirectories();
        //            DeleteDirectories(subDirectories);

        //            FileInfo[] files = directories[i].GetFiles();
        //            int fileCount = files.Length;
        //            for (int j = (fileCount - 1); j >= 0; j--)
        //            {
        //                try
        //                {
        //                    files[j].Delete();
        //                }
        //                catch
        //                { 
        //                    // If file is not closed : can not delete
        //                }
        //            }

        //            subDirectories = directories[i].GetDirectories();
        //            int subDirectoriesCount = subDirectories.Length;
        //            files = directories[i].GetFiles();
        //            fileCount = files.Length;
        //            if (subDirectoriesCount == 0 && fileCount == 0)
        //            {
        //                directories[i].Delete(true);  //Delete directory and files
        //            }

        //            Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " : Exiting - " + m_FileName);
        //            Trace.Unindent();
        //        }
        //    }
        //} 
        #endregion

        public void TextOut(string message)
        {
            lock (m_LockKey)
            {
                try
                {
                    if (m_CheckPath != 1) return;

                    //2009.08.07 Youngsik
                    if (m_ValidFileName == false) return;

                    DateTime curtime = DateTime.Now;
                    string date = curtime.ToString("MMdd");
                    if (string.Compare(m_Date, date) != 0 || m_Initialized == false)
                    {
                        if (NewFile() != true)
                        {
                            // TOTO : error
                        }
                    }
                    //else
                    //{
                    //    CreateStream();
                    //}

                    if (!m_Initialized) return;

                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                    if (message != null)
                    {
                        message = date + "\t" + message;

                        m_Writer.WriteLine(message);
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

        public void TextOut(string message, LogStampType type)
        {
            lock (m_LockKey)
            {
                if (m_CheckPath != 1) return;

                //2009.08.07 Youngsik
                if (m_ValidFileName == false) return;

                if (type == LogStampType.UseStamp)
                {
                    TextOut(message);
                }
                else
                {
                    try
                    {
                        DateTime curtime = DateTime.Now;
                        string date = curtime.ToString("MMdd");
                        if (string.Compare(m_Date, date) != 0 || m_Initialized == false)
                        {
                            if (NewFile() != true)
                            {
                                // TOTO : error
                            }
                        }
                        //else
                        //{
                        //    CreateStream();
                        //}

                        if (!m_Initialized) return;

                        if (message != null)
                        {
                            m_Writer.WriteLine(message);
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
        }

        public void HexOut(byte[] szBuf, int nLength)
        {
            int i, j;
            string sTmp, sOut;

            for (i = 0; i * 16 <= nLength; i++)
            {
                sOut = "\t";
                // Hexa Code Area
                for (j = 0; j < 16; j++)
                {
                    if (i * 16 + j >= nLength) break;
                    sTmp = string.Format("{0:X02} {1}", szBuf[i * 16 + j] & 0xff, (j == 7) ? " " : "");
                    sOut += sTmp;
                }
                // Empty Area
                if (j < 16)
                {
                    for (; j < 16; j++)
                    {
                        sOut += "    ";
                    }
                }
                // Character Area
                for (j = 0; j < 16; j++)
                {
                    if (i * 16 + j >= nLength) break;
                    byte b = szBuf[i * 16 + j];
                    if (b < 0x20 || b > 0x80)
                    {
                        sTmp = ".";
                    }
                    else
                    {
                        sTmp = string.Format("{0:C}", Convert.ToChar(b));
                    }
                    sOut += sTmp;
                }
                TextOut(sOut, LogStampType.NoUseStamp);
            }
        }
    }
}



