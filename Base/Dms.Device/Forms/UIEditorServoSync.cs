using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using Dms.Util.IODefine;
using Dms.Common;

namespace Dms.Device
{
	public class UIEditorServoSync : UITypeEditor
	{
        #region Contstructor
        public UIEditorServoSync()
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

            ServoUnit unit = (ServoUnit)context.Instance;

            FormServoSync ui = new FormServoSync();
            ui.Initialize(unit);                      
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
