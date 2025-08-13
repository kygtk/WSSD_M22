using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace Dms.DeviceLibrary
{
    public class DmsSerializingService
    {
        #region Fields
        private static bool m_IgnoreAllMissingFilesOpen = false;
        private static bool m_OpenAllMissingFiles = false;
        //private string m_DirName = @"..\ConfigFiles\Configuration";
        #endregion

        #region Constructor
        public DmsSerializingService()
        {
        }
        #endregion
        
        #region Methods
        public void SetPathWithDirDialog()
        {
        }

        public bool ReadXml(ref object obj, Type type, string filename)
        {
            //string path = GetPathName(filename);
            FileInfo f = new FileInfo(filename);
            if (f.Exists)
            {
                StreamReader sr = new StreamReader(filename);
                try
                {
                    XmlSerializer serial = new XmlSerializer(type);
                    obj = serial.Deserialize(sr);
                    sr.Close();
                    return true;
                }
                catch (Exception e) //Don't Use XFunc.ExceptionHandler.Add(err);
                {
                    MessageBox.Show(e.ToString());
                    sr.Close();
                    return false;
                }
            }
            else if (!m_IgnoreAllMissingFilesOpen)
            {
                if (!m_OpenAllMissingFiles)
                {
                    FormFileSelectOption dlg = new FormFileSelectOption(filename);
                    DialogResult dr;
                    dr = dlg.ShowDialog();
                    if (dr == DialogResult.Retry)
                    {
                        m_OpenAllMissingFiles = true;
                    }
                    else if (dr == DialogResult.Ignore)
                    {
                        m_IgnoreAllMissingFilesOpen = true;
                        return false;
                    }
                }

                //string path = m_DirName;
                string path = filename.Substring(0, filename.LastIndexOf("\\"));
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] xmlFiles = dir.GetFiles("*.xml");
                string[] sTokens;
                sTokens = filename.Split(new char[] { '\\', ']'});                

                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = sTokens[sTokens.Length-2] + "]" +  sTokens[sTokens.Length-1];
                ofd.Filter = "XML file (*.xml)|*.xml";
                ofd.InitialDirectory = path;

                foreach (FileInfo info in xmlFiles)
                {
                    if (info.Name.IndexOf(sTokens[6]) > 0) 
                    {
                        ofd.FileName = info.Name;
                    }
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(ofd.FileName);

                    try
                    {
                        XmlSerializer serial = new XmlSerializer(type);
                        obj = serial.Deserialize(sr);
                        sr.Close();
                        return true;
                    }
                    catch (Exception e) //Don't Use XFunc.ExceptionHandler.Add(err);
                    {
                        MessageBox.Show(e.ToString());
                        sr.Close();
                        return false;
                    }
                    finally
                    {
                        Directory.SetCurrentDirectory(Application.StartupPath);
                    }
                }
                else return false;
            }
            return false;
        }

        public void WriteXml(Object obj, Type type, string filename)
        {
            //string dirName = m_DirName;
            string dirName = filename.Substring(0, filename.LastIndexOf("\\"));

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            StreamWriter sw = new StreamWriter(filename);
            try
            {
                XmlSerializer serial = new XmlSerializer(type);
                serial.Serialize(sw, obj);
                sw.Close();
            }
            catch (Exception e) //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(e.ToString());
                sw.Close();
            }
        }
        #endregion
    }
}
