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
    public class UIEditorSlaveConfig : UITypeEditor
    {
        #region Contstructor
        public UIEditorSlaveConfig()
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
            FormSlaveConfig ui = new FormSlaveConfig();
            ui.Text = containerName + " : " + propertyName;
            ui.Initialize(value);

            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                ISlaveCollection originalCollection = value as ISlaveCollection;
                originalCollection.Clear();

                ISlaveCollection selectedCollection = ui.SelectedCollection;
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
