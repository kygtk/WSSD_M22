using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using Dms.Common;
using System.Windows.Forms;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.07.01
// Author       : Kim Youngsik
// Description  : Read/Write SEComINI.xml
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Data
{
    #region Definition of Parameter
    public class Parameter
    {
        #region Fields
        private string m_ParameterName = "";
        private string m_ParameterValue = "";
        private string m_ParameterDescription = "";
        #endregion

        #region Properties
        public string Name
        {
            get { return m_ParameterName; }
            set { m_ParameterName = value; }
        }

        public string Value
        {
            get { return m_ParameterValue; }
            set { m_ParameterValue = value; }
        }

        public string Description
        {
            get { return m_ParameterDescription; }
            set { m_ParameterDescription = value; }
        }
        #endregion

        #region Constuctors
        public Parameter()
        {
            m_ParameterName = "";
            m_ParameterValue = "";
            m_ParameterDescription = "";
        }

        public Parameter(string name, string value, string description)
        {
            m_ParameterName = name;
            m_ParameterValue = value;
            m_ParameterDescription = description;
        }
        #endregion

        #region Methods
        #endregion
    }
    #endregion

    public class SetupHsmsInfoXml
    {
        #region Fields
        private XmlDocument m_doc = new XmlDocument();
        private XmlNodeList m_NodeList;
        private string m_FilePath;

        private Parameter m_EqpID = new Parameter();
        private Parameter m_SoftVer = new Parameter();
        private Parameter m_SecomEqpId = new Parameter();
        private Parameter m_RemoteIP = new Parameter();
        private Parameter m_RemotePort = new Parameter();
        private Parameter m_LocalPort = new Parameter();
        private Parameter m_ConnectMode = new Parameter();
        private Parameter m_DeviceID = new Parameter();

        private Parameter m_T3Timeout = new Parameter();
        private Parameter m_T5Timeout = new Parameter();
        private Parameter m_T6Timeout = new Parameter();
        private Parameter m_T7Timeout = new Parameter();
        private Parameter m_T8Timeout = new Parameter();
        private Parameter m_T9Timeout = new Parameter();

        private Parameter m_LinkTest = new Parameter();
        private Parameter m_RetryCount = new Parameter();
        private Parameter m_T3RetryCount = new Parameter();

        private List<Parameter> m_Parameters = new List<Parameter>();
        private const string _DefaultSecomXmlFileName = "SEComINI.XML";
        #endregion

        #region Properties
        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }
        public Parameter EqpID
        {
            get { return m_EqpID; }
            set { m_EqpID = value; }
        }

        public Parameter SoftVer
        {
            get { return m_SoftVer; }
            set { m_SoftVer = value; }
        }

        public Parameter SecomEqpId
        {
            get { return m_SecomEqpId; }
            set { m_SecomEqpId = value; }
        }

        public Parameter RemoteIP
        {
            get { return m_RemoteIP; }
            set { m_RemoteIP = value; }
        }

        public Parameter RemotePort
        {
            get { return m_RemotePort; }
            set { m_RemotePort = value; }
        }

        public Parameter LocalPort
        {
            get { return m_LocalPort; }
            set { m_LocalPort = value; }
        }

        public Parameter ConnectMode
        {
            get { return m_ConnectMode; }
            set { m_ConnectMode = value; }
        }

        public Parameter DeviceID
        {
            get { return m_DeviceID; }
            set { m_DeviceID = value; }
        }

        public Parameter T3
        {
            get { return m_T3Timeout; }
            set { m_T3Timeout = value; }
        }

        public Parameter T5
        {
            get { return m_T5Timeout; }
            set { m_T5Timeout = value; }
        }

        public Parameter T6
        {
            get { return m_T6Timeout; }
            set { m_T6Timeout = value; }
        }

        public Parameter T7
        {
            get { return m_T7Timeout; }
            set { m_T7Timeout = value; }
        }

        public Parameter T8
        {
            get { return m_T8Timeout; }
            set { m_T8Timeout = value; }
        }

        public Parameter T9
        {
            get { return m_T9Timeout; }
            set { m_T9Timeout = value; }
        }

        public Parameter LinkTest
        {
            get { return m_LinkTest; }
            set { m_LinkTest = value; }
        }

        public Parameter RetryCount
        {
            get { return m_RetryCount; }
            set { m_RetryCount = value; }
        }

        public Parameter T3RetryCount
        {
            get { return m_T3RetryCount; }
            set { m_T3RetryCount = value; }
        }

        public List<Parameter> Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }
        #endregion

        #region Singleton
        public static readonly SetupHsmsInfoXml Instance = new SetupHsmsInfoXml();
        #endregion

        #region Constructor
        public SetupHsmsInfoXml()
        {
            m_FilePath = "D:\\HOST01";
            m_Parameters.Clear();

            AddParameter(ref m_RemoteIP, "REMOTEIP", "127.0.0.1", "Host Remote IP Address");
            AddParameter(ref m_RemotePort, "REMOTEPORT", "5000", "Host Remote Port Number");
            AddParameter(ref m_LocalPort, "LOCALPORT", "5000", "Host Local Port Number");
            AddParameter(ref m_ConnectMode, "HSMSMODE", "Active", "Connection Mode");
            AddParameter(ref m_DeviceID, "DEVICEID", "0", "DeviceId");

            AddParameter(ref m_T3Timeout, "T3", "60", "T3 Timeout (Reply Timeout)");
            AddParameter(ref m_T5Timeout, "T5", "60", "T5 Timeout (Separation Timeout)");
            AddParameter(ref m_T6Timeout, "T6", "60", "T6 Timeout (Transaction Timeout)");
            AddParameter(ref m_T7Timeout, "T7", "60", "T7 Timeout (Not-Selected Timeout)");
            AddParameter(ref m_T8Timeout, "T8", "60", "T8 Timeout (Intercharacter Timeout)");
            AddParameter(ref m_T9Timeout, "T9", "60", "T9 Timeout (Conversation Timeout)");

            AddParameter(ref m_LinkTest, "LINKTEST", "60", "Link Test Timer");
        }

        public void AddParameter(ref Parameter param, string name, string value, string description)
        {
            param.Name = name;
            param.Value = value;
            param.Description = description;

            m_Parameters.Add(param);
        }

        public bool GetParameters(string eqpName)
        {
            if (OpenFile() == false) return false;

            //bool ok = true;

            foreach (Parameter param in m_Parameters)
            {
                if (param.Name != "T9")
                {
                    string value = param.Value;
                    if (ReadValue(eqpName, param.Name, 1, ref value) == false) return false;
                    param.Value = value;
                }
            }

            return true;
        }

        public bool OpenFile()
        {
            bool bOk = false;

            //jemoon : 090929 - default path option
            AppConfig config = AppConfig.Instance;
            string path = "";
            if (config.UseDefaultFilePath)
            {
                path = AppConfig.DefaultConfigFilePath + "\\" + _DefaultSecomXmlFileName;
            }
            else
            {
                path = config.SecomXmlFile.SelectedFile;
                if (string.IsNullOrEmpty(path))
                {
                    path = "default";   //if filename is null or empty, System.IO.FileInfo will throw exception
                }
            }

            // Check to see if the file exists.
            FileInfo fileinfo = new FileInfo(path);

            try
            {
                if (fileinfo.Exists)
                {
                    m_FilePath = path;
                    m_doc.Load(m_FilePath);
                    bOk = true;
                }
                else
                {
                    MessageBox.Show("Secom xml File not found");
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = Application.StartupPath;
                    dlg.Title = "Select xml file : Secom";
                    dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    dlg.FileName = "SEComINI.XML";
                    if (DialogResult.OK == dlg.ShowDialog())
                    {
                        m_FilePath = dlg.FileName;
                        config.SecomXmlFile.SelectedFile = m_FilePath;
                        config.WriteXml();
                        m_doc.Load(m_FilePath);
                        bOk = true;
                    }
                }
            }
            catch
            {
                return bOk;
            }

            return bOk;
        }

        public bool ReadValue(string Section, string Key, int Depth, ref string Value)
        {
            StringBuilder xmlPath = new StringBuilder();

            try
            {
                xmlPath.Append("//" + Section);
                xmlPath.Append("/descendant::" + Key);

                m_NodeList = m_doc.DocumentElement.SelectNodes(xmlPath.ToString());

                if (m_NodeList.Count > 0)
                {
                    if ((Depth - 1) >= m_NodeList.Count)
                    {
                        return false;
                    }
                    else
                    {
                        Value = m_NodeList[Depth - 1].InnerText;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool WriteValue(string Section, string Key, string Value, int Depth)
        {
            bool bOK = false;

            if (Key == "T9") return true;

            StringBuilder xmlPath = new StringBuilder();

            xmlPath.Append("//" + Section);
            xmlPath.Append("/descendant::" + Key);

            m_NodeList = m_doc.DocumentElement.SelectNodes(xmlPath.ToString());

            if (m_NodeList.Count > 0)
            {
                if ((Depth - 1) >= m_NodeList.Count)
                {
                    return bOK;
                }
                else
                {
                    m_NodeList[Depth - 1].InnerText = Value;
                    m_doc.Save(m_FilePath);

                    bOK = true;
                }
            }

            return bOK;
        }
        #endregion
    }
}
