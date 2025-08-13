///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Collection of DeviceTag
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Common
{
    [Serializable()]
    public class DeviceTags
    {
        #region Implement IEnumerator
        // IEnumerable Interface Implementation:
        // Declaration of the GetEnumerator() method 
        // required by IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new InnerEnumerator(this);
        }

        // Inner class implements IEnumerator interface:
        private class InnerEnumerator : IEnumerator
        {
            private int m_Index = -1;
            private DeviceTags m_Collection;

            public InnerEnumerator(DeviceTags collection)
            {
                m_Collection = collection;
            }

            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (m_Index < m_Collection.Count - 1)
                {
                    m_Index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                m_Index = -1;
            }

            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return m_Collection.Items[m_Index];
                }
            }
        }
        #endregion

        #region Fields
        private List<DeviceTag> m_Items = new List<DeviceTag>();
        private static string m_FileName;
        private static int m_CheckPath = -1;
        #endregion

        #region Properties
        public List<DeviceTag> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public DeviceTag this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }
        public DeviceTag this[string key]
        {
            get
            {
                foreach (DeviceTag item in m_Items)
                {
                    if (key == item.DeviceName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }
        public DeviceTags this[Type type]
        {
            get
            {
                return GetTags(type.Name);
            }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }
        public string FileName
        {
            get { return m_FileName; }
        }
        #endregion

        #region Constructor
        #endregion

        #region Methods
        public void Add(DeviceTag tag)
        {
            m_Items.Add(tag);
        }

        public DeviceTag GetTag(string key)
        {
            return this[key];
        }

        public DeviceTags GetTags(string key)
        {
            DeviceTags tags = new DeviceTags();

            foreach (DeviceTag tag in m_Items)
            {
                if (tag.DeviceType.Contains(key))
                {
                    tags.Add(tag);
                }
            }

            return tags;
        }

        public DeviceTags GetTags(params string[] keys)
        {
            DeviceTags tags = new DeviceTags();

            foreach (DeviceTag tag in m_Items)
                foreach (string key in keys)
                    if (tag.DeviceType.Contains(key))
                        tags.Add(tag);

            return tags;
        }

        public DeviceTags GetFamilyTags(string key)
        {
            DeviceTags tags = new DeviceTags();

            foreach (DeviceTag tag in m_Items)
            {
                if (tag.FamilyType.Contains(key) || tag.DeviceType.Contains(key))
                {
                    tags.Add(tag);
                }
            }

            return tags;

        }


        public void WriteXml(string fileName)
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

                return;
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
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                System.Windows.Forms.MessageBox.Show(err.ToString());
            }
        }
        public void WriteXml()
        {
            //if (m_FileName == null)
            {
                m_FileName = this.GetType().Name + ".xml";
            }

            WriteXml(m_FileName);
        }

        public bool ReadXml(string fileName)
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
                    //MessageBox.Show("File not found");
                    //OpenFileDialog dlg = new OpenFileDialog();
                    ////dlg.InitialDirectory = Application.StartupPath;
                    //dlg.Title = "Select XML file : " + this.GetType().Name;
                    //dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    //if (DialogResult.OK == dlg.ShowDialog())
                    //{
                    //    m_FileName = dlg.FileName;
                    //}
                    //else
                    //{
                    //    return;
                    //}
                    string name = $"{this.GetType().Name}.xml";
                    MessageBox.Show($"{name} file does not exist in the specified location. Check the file and try again.");
                    return false;
                }

                StreamReader sr = new StreamReader(m_FileName);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());

                m_Items.Clear();
                m_Items = (xmlSer.Deserialize(sr) as DeviceTags).Items;

                sr.Close();

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
                return false;
            }
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

        private void CheckPath()
        {
            AppConfig AppConfig = AppConfig.Instance;
            string filePath = AppConfig.ComponentContainerPathName;
            string fileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);

            //if (m_AppConfig.UseDefaultFilePath)
            //{
            //    Directory.CreateDirectory(filePath);
            //}

            // jemoon : file의 존재 여부를 확인해야 한다.
            //if (Directory.Exists(filePath) == false)
            if (File.Exists(fileName) == false)
            {
                MessageBox.Show("DeviceTags Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "DeviceTags File Folder";
                //dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    AppConfig.ComponentContainerPath.SelectedFolder = filePath;
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
        #endregion
    }
}
