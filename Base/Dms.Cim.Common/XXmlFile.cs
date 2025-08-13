using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace Dms.Cim.Common
{
    public class XXmlFile
    {
        private XmlDocument m_doc = new XmlDocument();
        private XmlNodeList m_NodeList;
        private string m_FilePath;

        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }

        public XXmlFile() {}

        public bool Open( string path )
        {
            bool bOk = false;

            // Check to see if the file exists.
            FileInfo fileinfo = new FileInfo(path);

            m_FilePath = path;

            try
            {
                if (fileinfo.Exists)
                {
                    m_doc.Load(path);
                    bOk = true;
                }
            }
            catch
            {
                return bOk;
            }

            return bOk;
        }

        public string ReadValue(string Section, string Key, string Default, int Depth)
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
                        return Default;
                    }
                    else
                    {
                        return m_NodeList[Depth - 1].InnerText;
                    }
                }
                else
                {
                    return Default;
                }
            }
            catch
            {
                return Default;
            }
        }

        public bool WriteValue(string Section, string Key, string Value, int Depth)
        {
            bool bOK = false;

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

    }
}
