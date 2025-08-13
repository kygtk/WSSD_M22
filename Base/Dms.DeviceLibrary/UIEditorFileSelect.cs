using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace Dms.DeviceLibrary
{
    public class UIEditorFileSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorFileSelect()
        {
        }
        #endregion

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc == null)
            {   // uh oh.
                return value;
            }

            FileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Device";
            dlg.Filter = "(*.xml)|*.xml|All files(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                NodeInfoOpen ni = new NodeInfoOpen();
                ni.FileName = dlg.FileName;

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(ni.FileName);
                System.Xml.XmlElement el = doc.DocumentElement;
                System.Xml.XmlNodeList elList = doc.GetElementsByTagName("Genealogy");

                foreach (System.Xml.XmlElement e in elList)
                {
                    ni.Genealogy = e.InnerText;
                }
                value = ni;
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

    }

    [Editor(typeof(UIEditorFileSelect), typeof(UITypeEditor))]
    public class NodeInfoOpen
    {
        #region Fields
        private string m_FileName;
        private string m_Genealogy;
        #endregion
        #region Properties
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        public string Genealogy
        {
            get { return m_Genealogy; }
            set { m_Genealogy = value; }
        }
        #endregion
        #region Constructor
        public NodeInfoOpen()
        {
        }
        #endregion

        public override string ToString()
        {
            return m_FileName;
        }
    }

}
