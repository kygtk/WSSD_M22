using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dms.Common
{
    public class UIEditorFileSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorFileSelect()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            //IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc == null) return value;

            //FormSimulationConfigHelper form = new FormSimulationConfigHelper();
            //FolderBrowserDialog dlg = new FolderBrowserDialog();
            FileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Application.StartupPath;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FileSelect file = new FileSelect();
                file.SelectedFile = dlg.FileName;
                //path.SelectedPath = dlg.SelectedPath;

                value = file;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        #endregion
    }

    [Editor(typeof(UIEditorFileSelect), typeof(UITypeEditor))]
    public class FileSelect
    {
        #region Fields
        private string m_File = "Select File";
        #endregion

        #region Properties
        public string SelectedFile
        {
            get { return m_File; }
            set { m_File = value; }
        }
        #endregion

        #region Constructor
        public FileSelect()
        {

        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return m_File;
        }
        #endregion
    }

    public class UIEditorFolderSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorFolderSelect()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.SelectedPath = Application.StartupPath;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FolderSelect folder = new FolderSelect();
                folder.SelectedFolder = dlg.SelectedPath;
                value = folder;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        #endregion
    }

    [Editor(typeof(UIEditorFolderSelect), typeof(UITypeEditor))]
    public class FolderSelect
    {
        #region Fields
        private string m_Folder = "Select Folder";
        #endregion

        #region Properties
        public string SelectedFolder
        {
            get { return m_Folder; }
            set { m_Folder = value; }
        }
        #endregion

        #region Constructor
        public FolderSelect()
        { 
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return m_Folder;
        }
        #endregion
    }
}
