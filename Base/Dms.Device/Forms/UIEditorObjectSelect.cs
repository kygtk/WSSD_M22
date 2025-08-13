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
	public class UIEditorObjectSelect : UITypeEditor
	{
        #region Contstructor
        public UIEditorObjectSelect()
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

            Type containerType = context.Instance.GetType();
            string containerName = ((_Device)context.Instance).Name;
            Type propertyType = context.PropertyDescriptor.PropertyType;
            string propertyName = context.PropertyDescriptor.DisplayName;
            IComponentContainer iComponents = (context.Instance as _DeviceAsm).ComponentContainer;

            string key1 = containerType.Name;
            string key2 = containerName;
            string key3 = propertyName;
            
            key1 = key1.Substring(0, 2);
            key2 = key2.Substring(0, 2);
            key3 = key3.Substring(key3.Length - 2, 2);
            
            FormObjectSelect ui = new FormObjectSelect();
            ui.Initialize(iComponents, propertyType, containerName + " : " + propertyName);
                      
            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                value = ui.SelectedObject;
            }

            return value;
		}

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
        #endregion
	}
}
