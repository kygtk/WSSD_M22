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
    public class FlowAccuItem
    {
        #region Fields
        private DateTime m_SavedTime;
        private double m_TotalFlow;
        private double m_OldTotalFlow;
        private string m_DeviceName = null;
        private string m_FileName = null;
        private int m_HoldTime = 1;
        private Mutex m_Mutex = new Mutex();
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        public DateTime SavedTime
        {
            get { return m_SavedTime; }
            set { m_SavedTime = value; }
        }
        public double TotalFlow
        {
            get { return m_TotalFlow; }
            set { m_TotalFlow = value; }
        }
        public double OldTotalFlow
        {
            get { return m_OldTotalFlow; }
            set { m_OldTotalFlow = value; }
        }
        public int HoldTime
        {
            get { return m_HoldTime; }
            set { m_HoldTime = value; }
        }
        #endregion

        #region Constructor
        private FlowAccuItem()
        {

        }
        public FlowAccuItem(string name, int HoldTime)
        {
            m_DeviceName = name;
            m_HoldTime = HoldTime;
            // TotalFlow = new double[cnt];
        }
        #endregion

        #region Destruction
        ~FlowAccuItem()
        {
            UpdateData();
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
        //분당 한번씩 체킹한다.프로그램 재실행때도 체킹한다.
        public bool HoldTimeCheck(int HoldTime, bool DailyCheck)
        {
            bool bRv = false;
            long delta;
            DateTime curtime = DateTime.Now;
            if (!DailyCheck)
                delta = (long)((TimeSpan)(curtime - SavedTime)).TotalHours;
            else
                delta = (long)((TimeSpan)(curtime - SavedTime)).TotalDays;

            if (delta >= HoldTime)
            {
                SavedTime = curtime;
                UpdateData();
                bRv = true;
            }
            return bRv;
        }
        //public double AccumulateUpdate(double flow, int group)
        //{
        //    double dVal = TotalFlow[group] += flow;
        //    return dVal;
        //}
        public void SetData(FlowAccuItem item)
        {
            this.SavedTime = item.SavedTime;
            this.TotalFlow = item.TotalFlow;
            this.OldTotalFlow = item.OldTotalFlow;
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

                    m_FileName = string.Format("{0}\\{1}.xml", folderPath, m_DeviceName);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.xml", folderPath, m_DeviceName);
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
                m_FileName = string.Format("{0}\\{1}.xml", m_AppConfig.DevicePathName, m_DeviceName);

                try
                {
                    FileInfo fileInfo = new FileInfo(m_FileName);
                    if (fileInfo.Exists)
                    {
                        sr = new StreamReader(m_FileName);
                        FlowAccuItem storedHandler;
                        storedHandler = xmlSer.Deserialize(sr) as FlowAccuItem;

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
                    //XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);

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

                //XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);
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

                //XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);
            }
        }

        #endregion
    }
}
