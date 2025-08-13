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
	public class UIEditorIOSelect : UITypeEditor
	{
        #region Contstructor
        public UIEditorIOSelect()
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

			//string key1 = containerType.Name;
			//string key2 = containerName;
			//string key3 = propertyName;

			//key1 = key1.Substring(0, 2);
			//key2 = key2.Substring(0, 2);
			//key3 = key3.Substring(key3.Length - 2, 2);

            //IoItem io = null;
            //type = value.GetType();
            //if(type == typeof(IoDigitalInput))
            //{
            //    io = (value as IoDigitalInput).IoInfo;
            //}
            //else if(type == typeof(IoDigitalOutput))
            //{
            //    io = (value as IoDigitalOutput).IoInfo;
            //}
            //else if (type == typeof(IoAnalogInput))
            //{
            //    io = (value as IoAnalogInput).IoInfo;
            //}
            //else if (type == typeof(IoAnalogOutput))
            //{
            //    io = (value as IoAnalogOutput).IoInfo;
            //}

            IoItem io = null;
            if (propertyType == typeof(IoDigitalInput))
            {
                io = new IoItem(IoType.DI);
            }
            else if (propertyType == typeof(IoDigitalOutput))
            {
                io = new IoItem(IoType.DO);
            }
            else if (propertyType == typeof(IoAnalogInput))
            {
                io = new IoItem(IoType.AI);
            }
            else if (propertyType == typeof(IoAnalogOutput))
            {
                io = new IoItem(IoType.AO);
            }

			//FormIOSelect ui = new FormIOSelect();
			//ui.OwnerName = containerName + " : " + propertyName;
			//ui.SelectedIO = io;
			//ui.Initialize();

			FormIOSelect2 ui = new FormIOSelect2();
			ui.OwnerName = containerName + " : " + propertyName;
			ui.SelectedIO = io;
			ui.Initialize();
            
            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                if (ui.SelectedIO.Name == null) // clear config.
                {
                    value = null;
                }
                else
                {
					_DeviceIo selectedIo = Activator.CreateInstance(propertyType) as _DeviceIo;
					selectedIo.Name = ui.SelectedIO.Name;

					value = selectedIo;
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
