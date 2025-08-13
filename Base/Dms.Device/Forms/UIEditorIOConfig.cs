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
	public class UIEditorIOConfig : UITypeEditor
	{
        #region Contstructor
        public UIEditorIOConfig()
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
            //string containerName = ((_Device)context.Instance).Name;
            _Device device = context.Instance as _Device;
            string containerName = (device != null) ? device.Name : "";
            Type propertyType = context.PropertyDescriptor.PropertyType;
            string propertyName = context.PropertyDescriptor.DisplayName;

            //Type containedItemType = (value as GenericCollection).ContainedItemType;
            //FormObjectConfig ui = new FormObjectConfig();
            //ui.Initialize(containedItemType, key2);
            FormIOConfig ui = new FormIOConfig();
            ui.Text = containerName + " : " + propertyName;
            ui.Initialize(value);
                      
            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                iIoCollection originalCollection = value as iIoCollection;
                originalCollection.Clear();

                iIoCollection selectedCollection = ui.SelectedCollection;
                foreach (object item in selectedCollection)
                {
                    originalCollection.Add(item);
                }
                originalCollection.MaxSimulateCount = selectedCollection.MaxSimulateCount;
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
