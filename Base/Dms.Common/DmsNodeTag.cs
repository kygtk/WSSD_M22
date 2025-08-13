using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dms.Common
{
    [Serializable()]
    [Editor(typeof(UIEditorDmsNodeTagSelect), typeof(UITypeEditor))]
    public class DmsNodeTag
    {
        private string m_Name = "";

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public DmsNodeTag()
        { 
        }

        #region Override
        public override string ToString()
        {
            if (string.IsNullOrEmpty(m_Name))
            {
                return "Select Node...";
            }
            else
            {
                return m_Name;
            }
        }
        #endregion
    }

    [Serializable()]
    public class DmsNodeTags
    {
        private List<DmsNodeTag> m_Items = new List<DmsNodeTag>();
        private static string m_FileName;
        private static int m_CheckPath = -1;

        public List<DmsNodeTag> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public DmsNodeTags()
        { 
        }

        public void Add(DmsNodeTag item)
        {
            m_Items.Add(item);
        }

        public bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                return ReadXml(m_FileName);
            }
            else return false;
        }

        private bool ReadXml(string fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    m_FileName = fileName;
                }
                else
                {
                    string name = string.Format("{0}.xml", this.GetType().Name);
                    MessageBox.Show(name + " file does not exist in the specified location. Check the file and  try again.");
                    return false;
                }

                StreamReader sr = new StreamReader(m_FileName);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());
                m_Items.Clear();
                m_Items = (xmlSer.Deserialize(sr) as DmsNodeTags).Items;
                sr.Close();

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }

        public bool WriteXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                return WriteXml(m_FileName);
            }
            return false;
        }

        private bool WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());

            try // jemoon : 오류가 있는지 먼저 try
            {
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();

                return false;
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

                m_FileName = fileName;

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                System.Windows.Forms.MessageBox.Show(err.ToString());
                return false;
            }
        }

        private void CheckPath()
        {
            AppConfig AppConfig = AppConfig.Instance;
            string filePath = AppConfig.DevicePathName;

            if (AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Device Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = Application.StartupPath;
                dlg.Description = "Device(DmsNodeTag) Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    AppConfig.DevicePath.SelectedFolder = filePath;
                    AppConfig.WriteXml();

                    m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                m_CheckPath = 1;
            }
        }
    }
}
