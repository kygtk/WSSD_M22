using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dms.Common
{
    public class UIEditorTagSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorTagSelect()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null) return value;

            FormTagSelect ui = new FormTagSelect();
            ui.Initialize((DeviceTag)value);    //Set current tag
            edSvc.ShowDialog(ui);

            DeviceTag tag = new DeviceTag();
            if (ui.DialogResult == DialogResult.OK)
            {
                if (ui.SelectedTag != null)
                {
                    tag = ui.SelectedTag;
                    value = tag;
                }
                else
                {
                    value = new DeviceTag(((DeviceTag)value).FamilyType, ((DeviceTag)value).DeviceType);
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        #endregion
    }
}
