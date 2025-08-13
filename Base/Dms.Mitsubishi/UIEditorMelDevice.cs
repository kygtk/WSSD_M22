using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Dms.Common;

namespace Dms.Mitsubishi
{
    public class UIEditorMelDevice : UITypeEditor
    {
        #region constructor
        public UIEditorMelDevice()
        {
        }
        #endregion

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc == null)
            {
                return value;
            }

            Type typeOfSelectedItem = context.PropertyDescriptor.PropertyType;

            MelsecDevice mel = new MelsecDevice();
            FormMelsecDevices form = new FormMelsecDevices(typeOfSelectedItem, MelsecDeviceManager.Instance.MelContainer);
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                mel = form.GetSelectedItem();
                return mel;
            }
            return value;
            //if( typeOfSelectedItem == typeof(MelsecBitInput) )
            //{
            //    FormMelsecDevices form = new FormMelsecDevices(typeOfSelectedItem, MelsecDeviceContainer.Instance);
            //    form.Show();
            //}
            //else if( typeOfSelectedItem == typeof(MelsecBitOutput) )
            //{
            //    FormMelsecDevices form = new FormMelsecDevices(typeOfSelectedItem, MelsecDeviceContainer.Instance);
            //    form.Show();
            //}
            //else if( typeOfSelectedItem == typeof(MelsecWordInput) )
            //{
            //    FormMelsecDevices form = new FormMelsecDevices(typeOfSelectedItem, MelsecDeviceContainer.Instance);
            //    form.Show();
            //}
            //else if( typeOfSelectedItem == typeof(MelsecWordOutput) )
            //{
            //    FormMelsecDevices form = new FormMelsecDevices(typeOfSelectedItem, MelsecDeviceContainer.Instance);
            //    form.Show();
            //}



            //return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
