using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace Dms.Common
{
    public class UIEditorTagDescriptorSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorTagDescriptorSelect()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null) return value;

            object parent = context.Instance;

            PropertyInfo[] propertyInfos = XFunc.GetProperties(parent, typeof(DeviceTagInfo), Compatibility.Match);
            if (propertyInfos.Length > 0)
            {
                MethodInfo getMethodInfo = propertyInfos[0].GetGetMethod();
                DeviceTagInfo deviceTagInfo = getMethodInfo.Invoke(parent, null) as DeviceTagInfo;
                if (deviceTagInfo.DeviceName == "")
                {
                    MessageBox.Show("Reference DeviceTag is not selected!");
                }
                else
                {
                    FormTagDescriptorSelect ui = new FormTagDescriptorSelect();
                    ui.DeviceTagInfo = deviceTagInfo;
                    edSvc.ShowDialog(ui);

                    if (ui.DialogResult == DialogResult.OK)
                    {
                        value = ui.SelectedTagDescriptor;
                    }
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        #endregion
    }
}
