using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using Dms.Util.IODefine;
using Dms.Common;
using System.Collections;

namespace Dms.Device
{
	public class UIEditorPropertyEdit : UITypeEditor
	{
        #region Contstructor
        public UIEditorPropertyEdit()
        {
        } 
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value) 
		{
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc == null)
            {   // uh oh.
                return value;
            }

            FormPropertyEdit ui = new FormPropertyEdit();
            ui.Initialize(value);
            edSvc.ShowDialog(ui);

            return value;
		}

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
        #endregion
	}
}
