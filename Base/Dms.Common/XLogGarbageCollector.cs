using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Dms.Common
{
    public class ThreadLogGarbageCollector : XSequence
    {
        #region Fields
        private bool m_Initialized = false;
        private SeqLogGarbageCollector m_SeqLogGarbageCollector;
        #endregion

        //jemoon : 090916 - 여러로그 폴더를 각각의 Thread로 돌릴경우 같은 경로가 중복되면 서로 지우기 때문에
        //Sigleton으로 개선
        #region Singleton code...
        public static readonly ThreadLogGarbageCollector Instance = new ThreadLogGarbageCollector();
        #endregion

        #region Constructor
        private ThreadLogGarbageCollector()
        {
            if (!m_Initialized)
            {
                // 파일, 폴더 삭제하는 Thread는 기존 Thread 에 절대로 영향을 주면 안되므로 Lowest
                this.m_SeqThread.Priority = ThreadPriority.Lowest;

                m_SeqLogGarbageCollector = new SeqLogGarbageCollector();
                RegisterSequence(m_SeqLogGarbageCollector);

                // 하루동안 Sleep 했다가 하루에 한번씩만 기간 만료 삭제 시도
                m_ScanTime = 24 * 60 * 60 * 1000; //24h

                this.Start();

                m_Initialized = true;
            }
        }
        #endregion

        #region Methods
        public void AddDirectory(string dir, string subDirKeword, int maxDays)
        {
            m_SeqLogGarbageCollector.AddDirectory(dir, subDirKeword, maxDays);
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            Thread.Sleep(m_ScanTime);

            foreach (XSeqFunction seq in m_SeqFunctions)
            {
                seq.Do();
            }
        }
        #endregion
    }

    public class SeqLogGarbageCollector : XSeqFunction
    {
        #region Fields
        private List<int> m_MaxDays = new List<int>();  //기간만료기준
        private List<string> m_Directories = new List<string>();    //지워야 할 Log 폴더 List
        private List<string> m_SubDirKeywords = new List<string>(); //아무거나 지우면 안되고 Dms Log 폴더만 확인할 필요가 있음
        private Mutex m_Mutex = new Mutex();
        #endregion

        #region Constructor
        public SeqLogGarbageCollector()
        {
        }
        #endregion

        #region Sequence Function
        public override int Do()
        {
            RemoveExpiredFile();

            return -1;
        }
        #endregion

        #region Methods
        public void AddDirectory(string dir, string subDirKeyword, int maxDays)
        {
            m_Mutex.WaitOne();
            if (!m_Directories.Contains(dir) || !m_SubDirKeywords.Contains(subDirKeyword))
            {
                m_Directories.Add(dir);
                m_SubDirKeywords.Add(subDirKeyword);
                m_MaxDays.Add(maxDays);
            }
            m_Mutex.ReleaseMutex();
        }

        private void RemoveExpiredFile()
        {
            m_Mutex.WaitOne();
            List<string> logs = new List<string>();
            logs = m_Directories.GetRange(0, m_Directories.Count);
            m_Mutex.ReleaseMutex();

            if (logs != null && logs.Count > 0)
            {
                int count = logs.Count;
                for (int i = 0; i < count; i++)
                {
                    DirectoryInfo dir = new DirectoryInfo(logs[i]);   //find the Log folder
                    if (dir.Exists) //jemoon : directory가 있을때만 remove하자
                    {
                        //지정된 Log 폴더에 하부 폴더가 있을경우 ex) Log\\DmsLog[090916]
                        DirectoryInfo[] subDirectories = dir.GetDirectories();
                        List<DirectoryInfo> filter = new List<DirectoryInfo>();
                        int dirCount = subDirectories.Length;
                        string subKey = m_SubDirKeywords[i];

                        //로그지정경로의 하부 Dir중에 sub dir keyword가 포함된 폴더만 다시 추리기
                        //sub dir keyword가 지정되지 않은경우에는 pass
                        for (int k = (dirCount - 1); k >= 0; k--)
                        {
                            if (string.IsNullOrEmpty(subKey))
                            {
                                filter.Add(subDirectories[k]);
                            }
                            else if (subDirectories[k].Name.Contains(subKey))
                            {
                                filter.Add(subDirectories[k]);
                            }
                        }

                        if (filter.Count > 0)
                        {
                            DeleteDirectories(m_MaxDays[i], filter.ToArray());
                        }
                    }
                }
            }
        }

        // jemoon : 재귀호출 - 폴더 내부에 포함된 하부폴더, 파일을 순차적으로 지우기 위함
        private void DeleteDirectories(int maxDay, DirectoryInfo[] directories)
        {
            if (directories == null || directories.Length == 0) return;

            try
            {
                DateTime curtime = DateTime.Now;
                int count = directories.Length;
                //int deletedFiles = 0;
                //long deletedSize = 0;
                for (int i = (count - 1); i >= 0; i--)
                {
                    TimeSpan diff = curtime - directories[i].CreationTime;
                    if (diff.Days > maxDay)
                    {
                        //Trace.AutoFlush = true;
                        //Trace.Indent();
                        //Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " : Entering");

                        if (!directories[i].Exists) continue;
                        DirectoryInfo[] subDirectories = directories[i].GetDirectories();

                        if (directories != null || directories.Length > 0)
                        {
                            DeleteDirectories(maxDay, subDirectories);
                        }

                        // 더이상 하부 폴더가 없다면 해당 폴더의 파일 전부 삭제
                        FileInfo[] files = directories[i].GetFiles();
                        int fileCount = files.Length;
                        for (int j = (fileCount - 1); j >= 0; j--)
                        {
                            try
                            {
                                //long size = files[j].Length;
                                //deletedFiles++;
                                //deletedSize += size;

                                if (files[j].Exists)
                                {
                                    files[j].Delete();
                                }

                                //여러파일을 동시에 지우게 되면 CPU점유율이 상승 되므로
                                //적당한 Sleep을 주고 천천히 하나씩 지워준다.
                                Thread.Sleep(100);
                            }
                            catch
                            {
                                // 만약 지울려는 파일이 open되어 있는경우라면 exception발생
                                // catch로 빠지고 다음 진행
                                // 현재,file이 open 되어 있는지 확인 할수 있는 방법을 찾지 못해
                                // 예외 처리로 대체
                                // 향후, 아래 코드로 대체 할 수 있으면 좋겠다
                                // if( !file.IsOpen() )
                                // { 
                                //     file.Delete();
                                // }
                            }
                        }

                        int subDirectoriesCount = subDirectories.Length;
                        fileCount = directories[i].GetFiles().Length;
                        if (subDirectoriesCount == 0 && fileCount == 0)
                        {
                            directories[i].Delete();  //Delete directory
                        }

                        //Trace.WriteLine(string.Format("deleted : {0} files, total {1} bytes", deletedFiles.ToString(), deletedSize.ToString()));
                        //Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " : Exiting");
                        //Trace.Unindent();
                    }
                }
            }
            catch
            {
                // Error
            }
        }
        #endregion
    }
}
