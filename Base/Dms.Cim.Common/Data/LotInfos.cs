using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Cim.Common
{
    [Synchronization]
    public class LotInfos
    {
        string m_sDir = @"..\..\..\SettingFile\Data\LotInfo";

        public LotInfos()
        {
//            Initialize();
        }

        private List<LotInfo> m_LotInfos = new List<LotInfo>();

        public List<LotInfo> Infos
        {
            get { return m_LotInfos; }
            set { m_LotInfos = value; }
        }

        public LotInfo this[int index]
        {
            get { return m_LotInfos[index]; }
            set { m_LotInfos[index] = value; }
        }

        public int Count
        {
            get { return m_LotInfos.Count; }
        }

        public void Initialize()
        {
            DirectoryInfo dir = new DirectoryInfo(m_sDir);
            FileInfo[] fis = dir.GetFiles("*.xml");

            foreach (FileInfo f in fis)
            {
                LotInfo lot = null;

                if (ReadLotInfo(f.FullName, ref lot) == true)
                {
                    m_LotInfos.Add(lot);
                }
            }
        }

        public short GetMaxLotNo()
        {
            short LotNo = 0;

            foreach (LotInfo info in m_LotInfos)
            {
                if( info.LotNo > LotNo ) LotNo = info.LotNo;
            }

            if (LotNo >= 255) LotNo = 1;
            else LotNo += 1;

            return LotNo;
        }

/*
        public int SearchNotDefineUnloaderPortNo()
        {
            short lotno = 0;

            foreach (LotInfo info in m_LotInfos)
            {
                if (info.UnloaderPortNo == 0 && 
                    !info.UnloaderPortMatched &&
                    ( info.LotStatus == enumHostLotStatus.enumInProcess || 
                      info.LotStatus == enumHostLotStatus.enumWaitingProcess ) 
                    ) lotno = info.LotNo;
            }

            return lotno;
        }
*/
        public LotInfo GetLotInfo( int lotno)
        {
            LotInfo lotinfo = null;

            foreach(LotInfo info in m_LotInfos )
            {
                if( info.LotNo == lotno )
                {
                    lotinfo = info;
                    break;
                }
            }

            return lotinfo;
        }

        public LotInfo GetLotInfo(string LotId)
        {
            LotInfo lotinfo = null;

            foreach (LotInfo info in m_LotInfos)
            {
                if (info.LOTID == LotId)
                {
                    lotinfo = info;
                    break;
                }
            }

            return lotinfo;
        }

        public void Add(LotInfo Info)
        {
            m_LotInfos.Add(Info);
        }

        public void DeleteLotNo(int lotno)
        {
            int count = m_LotInfos.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                LotInfo lotinfo = m_LotInfos[i];

                if (lotno == lotinfo.LotNo)
                {
                    FileInfo fi = new FileInfo(lotinfo.GetPathName());

                    try
                    {
                        fi.Delete();
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        m_LotInfos.Remove(lotinfo);
                    }
                }
            }
        }

        public void Delete(int index)
        {
            FileInfo fi = new FileInfo(m_LotInfos[index].GetPathName());

            try
            {
                fi.Delete();
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                m_LotInfos.RemoveAt(index);
            }
        }

        public void Delete(string sLotID)
        {
            int count = m_LotInfos.Count;

            for (int i = count-1; i >= 0; i--)
            {
                LotInfo lotinfo = m_LotInfos[i];

                if (sLotID == lotinfo.LOTID)
                {
                    FileInfo fi = new FileInfo(lotinfo.GetPathName());

                    try
                    {
                        fi.Delete();
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        m_LotInfos.Remove(lotinfo);
                    }
                }
            }

        }

        public bool ReadLotInfo(string path, ref LotInfo lot)
        {
            bool rv = true;
            StreamReader sr = new StreamReader(path);

            object obj = new object();

            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(LotInfo));

                obj = xmlSer.Deserialize(sr) as LotInfo;
                lot = (LotInfo)obj;
            }
            catch //(InvalidOperationException e)
            {
//                MessageBox.Show(e.Message);
                rv = false;
            }
            finally
            {
                sr.Close();
            }

            return rv;
        }
    }
}

