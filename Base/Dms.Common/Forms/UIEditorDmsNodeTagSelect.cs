using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Dms.Common
{
    class UIEditorDmsNodeTagSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorDmsNodeTagSelect()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null) return value;

            FormDmsNodeTagSelect ui = new FormDmsNodeTagSelect();
            ui.Initialize((DmsNodeTag)value);    //Set current tag
            edSvc.ShowDialog(ui);

            DmsNodeTag tag = new DmsNodeTag();
            if (ui.DialogResult == DialogResult.OK)
            {
                if (ui.SelectedTag != null)
                {
                    tag = ui.SelectedTag;
                    value = tag;
                }
                else
                {
                    value = new DmsNodeTag();
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        #endregion
    }
}
