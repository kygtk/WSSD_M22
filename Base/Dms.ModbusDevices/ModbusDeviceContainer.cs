using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.ModbusDevices
{
    public class ModbusDeviceContainer
    {
        #region Fields
        private List<ModbusBitInput> m_BitInputs = new List<ModbusBitInput>();
        private List<ModbusBitOutput> m_BitOutputs = new List<ModbusBitOutput>();
        private List<ModbusWordInput> m_WordInputs = new List<ModbusWordInput>();
        private List<ModbusWordOutput> m_WordOutputs = new List<ModbusWordOutput>();
        private string m_FileName = "Devices.xml";
        private string m_DirName = @"..\ConfigFiles\Configuration\Modbus";
        #endregion
        
        #region Constructor
        public ModbusDeviceContainer()
        {
            AppConfig config = AppConfig.Instance;
            config.ReadXml();
            m_DirName = string.Format("{0}\\{1}", config.ModbusConfigurationPathName, "Modbus");
        }
        #endregion

        #region Properties
        public List<ModbusBitInput> BitInputs
        {
            get { return m_BitInputs; }
            set { m_BitInputs = value; }
        }
        public List<ModbusBitOutput> BitOutputs
        {
            get { return m_BitOutputs; }
            set { m_BitOutputs = value; }
        }
        public List<ModbusWordInput> WordInputs
        {
            get { return m_WordInputs; }
            set { m_WordInputs = value; }
        }
        public List<ModbusWordOutput> WordOutputs
        {
            get { return m_WordOutputs; }
            set { m_WordOutputs = value; }
        }
        #endregion

        #region Methods
        public void LoadDevicesFromDisk()
        {
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
            string filename = string.Format("{0}\\{1}", m_DirName, m_FileName);
            Directory.CreateDirectory(m_DirName);
            StreamWriter sw = new StreamWriter(filename);
            try
            {
                XmlSerializer serial = new XmlSerializer(typeof(ModbusDeviceContainer));
                serial.Serialize(sw, this);
                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                sw.Close();
            }
        }
        private void DeserializeFromXmlFile()
        {
            string filename = string.Format("{0}\\{1}", m_DirName, m_FileName);
            Directory.CreateDirectory(m_DirName);
            StreamReader sr = new StreamReader(filename);
            try
            {
                XmlSerializer serial = new XmlSerializer(typeof(ModbusDeviceContainer));
                object obj = new object();
                obj = serial.Deserialize(sr);

                this.BitInputs = ((ModbusDeviceContainer)obj).BitInputs;
                this.BitOutputs = ((ModbusDeviceContainer)obj).BitOutputs;
                this.WordInputs = ((ModbusDeviceContainer)obj).WordInputs;
                this.WordOutputs = ((ModbusDeviceContainer)obj).WordOutputs;

                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                sr.Close();
            }
        }
        #endregion
    }
}
