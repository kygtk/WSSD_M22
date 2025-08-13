using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Device;
using System.IO;
using System.Net;


namespace Dms.Ctl
{
    public class FtpClient : XSequence
    {
        #region Fields
        private static object m_LockKey = new object();
        private string m_LastErrorMsg;
        private IServerManager m_Server;
        #endregion

        #region Constructor
        public FtpClient(int sleepTime, IServerManager server) : base(sleepTime)
        {
            m_Server = server;
        }
        #endregion

        #region Override
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (!FtpUploadFileQueue.Instance.IsEmpty())
                {                    
                    FtpUpload(FtpUploadFileQueue.Instance.TrimLast());
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// File Upload
        /// </summary>
        /// <param name="serverAddress">FTP서버주소</param>
        /// <param name="port">포트번호</param>
        /// <param name="targetDir">FTP서버 디렉토리</param>
        /// <param name="id">아이디</param>
        /// <param name="pass">비밀번호</param>
        /// <param name="uploadFile">경로를 포함한 업로드 Filename</param>
        /// <param name="uploadFileShort">경로를 제외한 업로드 Filename</param>
        /// <returns>true: 성공, false: 실패</returns>
        public bool FtpUpload(string serverAddress,
        string port,
        string targetDir,
        string id,
        string pass,
        string uploadFile,
        string uploadFileShort)
        {
            lock(m_LockKey)
            {
                bool ret = false;
                Stream reqStream = null;
                FileStream fileStream = null;
                //FtpWebRequest upResponse = null;
                try
                {
                    if (File.Exists(uploadFile))
                    {
                        FtpWebRequest fwReq = (FtpWebRequest)WebRequest.Create("ftp://"
                                                            + serverAddress + ":"
                                                            + port + "/"
                                                            + targetDir + "/"
                                                            + uploadFileShort);
                        fwReq.Proxy = null;
                        fwReq.Method = WebRequestMethods.Ftp.UploadFile;
                        fwReq.Credentials = new NetworkCredential(id, pass);
                        reqStream = fwReq.GetRequestStream();

                        fileStream = File.Open(uploadFile, FileMode.Open);
                        byte[] buf = new byte[1024];
                        int bytesRead = 0;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buf, 0, buf.Length);
                            if (bytesRead == 0) break;
                            reqStream.Write(buf, 0, bytesRead);
                        }
                        reqStream.Close();
                        //upResponse = (FtpWebResponse)fwReq.GetResponse();
                        ret = true;
                    }
                    else
                    {
                        m_LastErrorMsg = "File not exist.";
                        ret = false;
                    }

                }
                catch (Exception ex)
                {
                    m_LastErrorMsg = ex.ToString();
                    ret = false;
                }
                finally
                {
                    //if (upResponse !=null) upResponse.;
                    if (fileStream != null) fileStream.Close();
                    if (reqStream != null) reqStream.Close();
                }
                return ret;
            }
        }

        public bool FtpUpload(FtpUploadInfo info)
        {
            string log;

            lock(m_LockKey)
            {
                bool ret = false;
                Stream reqStream = null;
                FileStream fileStream = null;
                //FtpWebRequest upResponse = null;
                try
                {
                    if (File.Exists(info.FullFileName))
                    {
                        FtpWebRequest fwReq = (FtpWebRequest)WebRequest.Create("ftp://"
                                                            + info.Server+ ":"
                                                            + info.Port + "/"
                                                            + info.DirName + "/"
                                                            + info.FileName);
                        fwReq.Proxy = null;
                        fwReq.Method = WebRequestMethods.Ftp.UploadFile;
                        fwReq.Credentials = new NetworkCredential(info.Id, info.Password);
                        reqStream = fwReq.GetRequestStream();

                        fileStream = File.Open(info.FullFileName, FileMode.Open);
                        byte[] buf = new byte[1024];
                        int bytesRead = 0;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buf, 0, buf.Length);
                            if (bytesRead == 0) break;
                            reqStream.Write(buf, 0, bytesRead);
                        }
                        reqStream.Close();

                        m_Server.Log("FTP Client\tFile Upload Success.");
                        log = string.Format("FTP Client\tftp://{0}@{1}:{2}/{3}/{4}",
                                                            info.Id,
                                                            info.Server,
                                                            info.Port,
                                                            info.DirName,
                                                            info.FileName);
                        m_Server.Log(log);
                        //upResponse = (FtpWebResponse)fwReq.GetResponse();
                        ret = true;
                    }
                    else
                    {
                        m_Server.Log("FTP Client\tFile Upload Fail. File not exist.");
                        m_LastErrorMsg = "File not exist.";
                        ret = false;
                    }

                }
                catch (Exception ex)
                {
                    char[] separator = new char[] { '\n' };
                    string[] err;
                    string exErr;
                    exErr = ex.ToString();

                    err = exErr.Split(separator);

                    log = string.Format("FTP Client\tFile Upload Fail. {0}", err[0]);
                    m_Server.Log(log);
                    log = string.Format("FTP Client\tftp://{0}@{1}:{2}/{3}/{4}",
                                                            info.Id,
                                                            info.Server,
                                                            info.Port,
                                                            info.DirName,
                                                            info.FileName);
                    m_Server.Log(log);

                    m_LastErrorMsg = ex.ToString();
                    ret = false;
                }
                finally
                {
                    //if (upResponse !=null) upResponse.;
                    if (fileStream != null) fileStream.Close();
                    if (reqStream != null) reqStream.Close();
                }
                return ret;
            }
        }
        #endregion
    }
}
