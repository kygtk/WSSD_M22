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
    class UIEditorSlaveSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorSlaveSelect()
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

            string containerName = "";
            _Device device = context.Instance as _Device;
            if (device != null) containerName = device.Name;

            Type propertyType = context.PropertyDescriptor.PropertyType;
            string propertyName = context.PropertyDescriptor.DisplayName;

            EcSlaveItem slave = null;
            if /**/ (propertyType == typeof(SlaveServo))
                slave = new EcSlaveItem(EcSlaveItemType.Servo);
            else if (propertyType == typeof(SlaveBLDC))
                slave = new EcSlaveItem(EcSlaveItemType.BLDC);
            else if (propertyType == typeof(SlaveInverter))
                slave = new EcSlaveItem(EcSlaveItemType.Inverter);
            else if (propertyType == typeof(SlaveDigitalInput))
                slave = new EcSlaveItem(EcSlaveItemType.DI);
            else if (propertyType == typeof(SlaveDigitalOutput))
                slave = new EcSlaveItem(EcSlaveItemType.DO);
            else if (propertyType == typeof(SlaveAnalogInput))
                slave = new EcSlaveItem(EcSlaveItemType.AI);
            else if (propertyType == typeof(SlaveAnalogOutput))
                slave = new EcSlaveItem(EcSlaveItemType.AO);
            else if (propertyType == typeof(SlaveAP))
                slave = new EcSlaveItem(EcSlaveItemType.AP);

            FormSlaveSelect2 ui = new FormSlaveSelect2();
            ui.OwnerName = containerName + " : " + propertyName;
            ui.SelectedSlave = slave;
            ui.Initialize();

            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                if (ui.SelectedSlave.Name == null) // clear config.
                {
                    value = null;
                }
                else
                {
                    _DeviceSlave selectedslave = Activator.CreateInstance(propertyType) as _DeviceSlave;
                    selectedslave.Name = ui.SelectedSlave.Name;

                    value = selectedslave;
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
