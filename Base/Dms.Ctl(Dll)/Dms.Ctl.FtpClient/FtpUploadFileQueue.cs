using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dms.Ctl
{
    public class FtpUploadInfo
    {
        #region Fields
        private string m_Server;
        private string m_Port;
        private string m_DirName;
        private string m_FullFileName;
        private string m_FileName;
        private string m_Id;
        private string m_Password;
        #endregion

        #region Properties
        public string Server
        {
            get { return m_Server; }
            set { m_Server = value; }
        }

        public string Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        public string DirName
        {
            get { return m_DirName; }
            set { m_DirName = value; }
        }

        public string FullFileName
        {
            get { return m_FullFileName; }
            set { m_FullFileName = value; }
        }

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        #endregion

        #region Constructor
        public FtpUploadInfo(string server, string port,
            string dirName, string fullFileName,
            string fileName, string id, string password)
        {
            m_Server = server;
            m_Port = port;
            m_DirName = dirName;
            m_FullFileName = fullFileName;
            m_FileName = fileName;
            m_Id = id;
            m_Password = password;
        }
        #endregion
    }

    public class FtpUploadFileQueue
    {
        #region Fields
        private static object m_LockKey = new object();
        private string m_LastErrorMsg = "No Error";
        private static List<FtpUploadInfo> m_UploadFileList = new List<FtpUploadInfo>();
        #endregion

        #region Singletone
        public static readonly FtpUploadFileQueue Instance = new FtpUploadFileQueue();
        #endregion

        #region Properties
        public string LastErrorMsg
        {
            get { return m_LastErrorMsg; }
        }
        #endregion

        #region Constructor
        private FtpUploadFileQueue()
        {
        }
        #endregion

        #region Methods
        public void Add(FtpUploadInfo info)
        {
            lock (m_LockKey)
            {
                while (true)
                {
                    if (m_UploadFileList.Count > 100)
                    {
                        m_UploadFileList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                m_UploadFileList.Add(info);
            }
        }

        public void Clear()
        {
            lock (m_LockKey)
            {
                while (m_UploadFileList.Count > 0)
                {
                    m_UploadFileList.RemoveAt(0);
                }
            }
        }
        public FtpUploadInfo TrimLast()
        {
            lock (m_LockKey)
            {
                if (m_UploadFileList.Count > 0)
                {
                    FtpUploadInfo info;
                    info = m_UploadFileList[0];
                    m_UploadFileList.RemoveAt(0);
                    return info;
                }
                else return null;
            }
        }

        public bool IsEmpty()
        {
            return m_UploadFileList.Count <= 0;
        }
        #endregion

    }
}
