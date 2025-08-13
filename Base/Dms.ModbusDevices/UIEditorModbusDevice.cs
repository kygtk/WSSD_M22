using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using Dms.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Dms.ModbusDevices
{
    public class UIEditorModbusDevice : UITypeEditor
    {
        #region constructor
        public UIEditorModbusDevice()
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

            ModbusDevice dev = new ModbusDevice();


            ModbusDevice modbus = new ModbusDevice();

            FormModbusDevices form = new FormModbusDevices(typeOfSelectedItem, ModbusDeviceManager.Instance.ModbusContainer);

            if (form.ShowDialog() == DialogResult.OK)
            {
                modbus = form.GetSelectedItem();
                return modbus;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
