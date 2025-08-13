using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    public class MixTankItem
    {
        #region Fields
        private MixTankMode m_CurShowerMode;
        private DetSupplyMode m_DetSupplyMode;
        private int m_MixTankUsedTime = 0;
        private int m_MixTankGlassCount = 0;
        private int m_MixTankRecycleCount = 0;
        private int m_MixTankDrainCount = 0;
        private int m_MixTankStandbyTime = 0;
        private int m_MixTankPrevUsedTime = 0;
        private string m_FileName = null;
        private string m_MixTankName = null;
        private Mutex m_Mutex = new Mutex();
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        public MixTankMode CurShowerMode
        {
            get { return m_CurShowerMode; }
            set { m_CurShowerMode = value; }
        }
        public DetSupplyMode DetSupplyMode
        {
            get { return m_DetSupplyMode; }
            set { m_DetSupplyMode = value; }
        }
        public int MixTankUsedTime
        {
            get { return m_MixTankUsedTime; }
            set { m_MixTankUsedTime = value; }
        }
        public int MixTankGlassCount
        {
            get { return m_MixTankGlassCount; }
            set { m_MixTankGlassCount = value; }
        }
        public int MixTankRecycleCount
        {
            get { return m_MixTankRecycleCount; }
            set { m_MixTankRecycleCount = value; }
        }
        public int MixTankDrainCount
        {
            get { return m_MixTankDrainCount; }
            set { m_MixTankDrainCount = value; }
        }
        public int MixTankStandbyTime
        {
            get { return m_MixTankStandbyTime; }
            set { m_MixTankStandbyTime = value; }
        }
        public int MixTankPrevUsedTime
        {
            get { return m_MixTankPrevUsedTime; }
            set { m_MixTankPrevUsedTime = value; }
        }
        #endregion

        #region Constructor
        private MixTankItem()
        {
            
        }
        public MixTankItem(string name)
        {
            m_MixTankName = name;
        }
        #endregion

        #region Methods
        public bool InitParameter()
        {
            return ReadXml();
        }
        public void UpdateData()
        {
            WriteXml();
        }
        private void SetData(MixTankItem item)
        {
            this.CurShowerMode = item.CurShowerMode;
            this.MixTankUsedTime = item.MixTankUsedTime;
            this.MixTankGlassCount = item.MixTankGlassCount;
            this.MixTankRecycleCount = item.MixTankRecycleCount;
            this.MixTankDrainCount = item.MixTankDrainCount;
            this.MixTankStandbyTime = item.MixTankStandbyTime;
            this.MixTankPrevUsedTime = item.MixTankPrevUsedTime;
        }
        private void CheckPath()
        {
            m_AppConfig.ReadXml();

            //jemoon : 090929 - use default path option
            string folderPath = m_AppConfig.DevicePathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(folderPath);
            }

            if (Directory.Exists(folderPath) == false)
            {
                MessageBox.Show("Device Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Device Folder (for MixTank)";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    folderPath = dlg.SelectedPath;
                    m_AppConfig.DevicePath.SelectedFolder = folderPath;
                    m_AppConfig.WriteXml();

                    m_FileName = string.Format("{0}\\{1}.xml", folderPath, m_MixTankName);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.xml", folderPath, m_MixTankName);
                m_CheckPath = 1;
            }
        }
        private bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                StreamReader sr = null;
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());

                //jemoon : 090929 - m_MixTankName에 따라 m_FileName이 달라져야 한다.
                m_FileName = string.Format("{0}\\{1}.xml", m_AppConfig.DevicePathName, m_MixTankName);

                try
                {
                    FileInfo fileInfo = new FileInfo(m_FileName);
                    if (fileInfo.Exists)
                    {
                        sr = new StreamReader(m_FileName);
                        MixTankItem storedHandler;
                        storedHandler = xmlSer.Deserialize(sr) as MixTankItem;

                        SetData(storedHandler);

                        sr.Close();
                    }
                    else
                    {
                        WriteXml();
                    }

                    return true;
                }
                catch (Exception err)
                {
                    string msg = err.ToString();
                    System.Windows.Forms.MessageBox.Show(err.ToString());

                    if (sr != null) sr.Close();

                    return false;
                }
            }
            else return false;
        }

        private void WriteXml()
        {
            if (m_CheckPath != 1 || string.IsNullOrEmpty(m_FileName)) return;

            m_Mutex.WaitOne();

            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());

            try
            {   // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(m_FileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(m_FileName + ".try");
                file.Delete();
            }
            catch (Exception err)
            {
                if (sw != null) sw.Close();
                m_Mutex.ReleaseMutex();
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                return;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(m_FileName);
                if (file.Exists)
                {
                    file.CopyTo(m_FileName + ".old", true);
                }

                sw = new StreamWriter(m_FileName);
                xmlSer.Serialize(sw, this);
                sw.Close();

                m_Mutex.ReleaseMutex();
            }
            catch (Exception err)
            {
                m_Mutex.ReleaseMutex();
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());
            }
        }
        #endregion
    }
}
