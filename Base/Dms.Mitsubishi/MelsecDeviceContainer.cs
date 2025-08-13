using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Mitsubishi
{
    public class MelsecDeviceContainer
    {
        #region Fields
        private List<MelsecBitInput> m_BitInputs = new List<MelsecBitInput>();
        private List<MelsecBitOutput> m_BitOutputs = new List<MelsecBitOutput>();
        private List<MelsecWordInput> m_WordInputs = new List<MelsecWordInput>();
        private List<MelsecWordOutput> m_WordOutputs = new List<MelsecWordOutput>();
        private string m_FileName = null;
        private string m_DirName = null;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        private static int m_CheckPath = -1;
        #endregion
        
        #region Constructor
        public MelsecDeviceContainer()
        {
            //m_DirName =  @"..\ConfigFiles\Configuration";
            m_FileName = this.GetType().Name + ".xml";
        }
        #endregion

        #region Properties
        public List<MelsecBitInput> BitInputs
        {
            get { return m_BitInputs; }
            set { m_BitInputs = value; }
        }
        public List<MelsecBitOutput> BitOutputs
        {
            get { return m_BitOutputs; }
            set { m_BitOutputs = value; }
        }
        public List<MelsecWordInput> WordInputs
        {
            get { return m_WordInputs; }
            set { m_WordInputs = value; }
        }
        public List<MelsecWordOutput> WordOutputs
        {
            get { return m_WordOutputs; }
            set { m_WordOutputs = value; }
        }
        #endregion

        #region Methods
        public void LoadDevicesFromDisk()
        {
            if (m_CheckPath == -1) CheckPath(ref m_DirName);
            if (m_CheckPath != 1) return;

            string filename = string.Format("{0}\\{1}", m_DirName, m_FileName);
            FileInfo fi = new FileInfo(filename);
            if (fi.Exists)
            {
                DeserializeFromXmlFile();
            }
        }

        public void SaveDevicesToDisk()
        {
            SerializeToXmlFile();
        }

        private void SerializeToXmlFile()
        {
            if (m_CheckPath == -1) CheckPath(ref m_DirName);
            if (m_CheckPath != 1) return;

            string filename = string.Format("{0}\\{1}", m_DirName, m_FileName);
            //Directory.CreateDirectory(m_DirName);
            StreamWriter sw = new StreamWriter(filename);
            try
            {
                XmlSerializer serial = new XmlSerializer(typeof(MelsecDeviceContainer));
                serial.Serialize(sw, this);
                sw.Close();
            }
            catch (Exception e) //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(e.ToString());
                sw.Close();
            }
        }
        private void DeserializeFromXmlFile()
        {
            if (m_CheckPath == -1) CheckPath(ref m_DirName);
            if (m_CheckPath != 1) return;

            string filename = string.Format("{0}\\{1}", m_DirName, m_FileName);
            //Directory.CreateDirectory(m_DirName);
            StreamReader sr = new StreamReader(filename);
            try
            {
                XmlSerializer serial = new XmlSerializer(typeof(MelsecDeviceContainer));
                object obj = new object();
                obj = serial.Deserialize(sr);

                this.BitInputs = ((MelsecDeviceContainer)obj).BitInputs;
                this.BitOutputs = ((MelsecDeviceContainer)obj).BitOutputs;
                this.WordInputs = ((MelsecDeviceContainer)obj).WordInputs;
                this.WordOutputs = ((MelsecDeviceContainer)obj).WordOutputs;

                sr.Close();
            }
            catch (Exception e) //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(e.ToString());
                sr.Close();
            }
        }

        protected void CheckPath(ref string path)
        {
            m_AppConfig.ReadXml();

            string filePath = m_AppConfig.MelsecConfigurationPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Melsec Configuration Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = Application.StartupPath;
                dlg.Description = "Melsec Configuraion Folder";
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.MelsecConfigurationPath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    path = filePath;
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                path = filePath;
                m_CheckPath = 1;
            }
        }
        #endregion
    }
}
