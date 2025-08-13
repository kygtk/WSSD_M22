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
	public class UIEditorObjectConfig : UITypeEditor
	{
        #region Contstructor
        public UIEditorObjectConfig()
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

            //Type containedItemType = (value as GenericCollection).ContainedItemType;
            //FormObjectConfig ui = new FormObjectConfig();
            //ui.Initialize(containedItemType, key2);
            FormObjectConfig ui = new FormObjectConfig();
            ui.Text = containerName + " : " + propertyName;
            ui.Initialize(iComponents, value);
                      
            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                IGenericCollection collection = value as IGenericCollection;
                collection.Clear();
                
                ArrayList collection2 = ui.SelectedObject as ArrayList;
                foreach (object item in collection2)
                {
                    collection.Add(item);
                }
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
