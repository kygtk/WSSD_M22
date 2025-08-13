using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dms.Common
{
    public class UIEditorSimluationConfigHelper : UITypeEditor
    {
        #region Constructor
        public UIEditorSimluationConfigHelper()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            //IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc == null) return value;

            FormSimulationConfigHelper form = new FormSimulationConfigHelper();
            form.ShowDialog();

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        #endregion
    }
}
