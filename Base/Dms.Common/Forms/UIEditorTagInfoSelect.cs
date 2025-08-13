using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dms.Common
{
    public class UIEditorTagInfoSelect : UITypeEditor
    {
        #region Constructor
        public UIEditorTagInfoSelect()
        { 
        }
        #endregion

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null) return value;

            DeviceTagInfo tagInfo = (DeviceTagInfo)value;

            if (tagInfo == null) tagInfo = new DeviceTagInfo();

            DeviceTag tag = new DeviceTag();
            tag.DeviceType = tagInfo.DeviceType;
            tag.DeviceName = tagInfo.DeviceName;
            if (tagInfo.FamilyType != null)
            {
                tag.FamilyType = tagInfo.FamilyType;
            }
            else
            {
                tag.FamilyType = "";
            }

            FormTagSelect ui = new FormTagSelect();
            ui.Initialize(tag);    //Set current tag
            edSvc.ShowDialog(ui);

            if (ui.DialogResult == DialogResult.OK)
            {
                tagInfo = new DeviceTagInfo(tagInfo.FamilyType, tagInfo.DeviceType);

                if (ui.SelectedTag != null)
                {
                    //tag = ui.SelectedTag;
                    //value = tag;
                    tagInfo.FamilyType = ui.SelectedTag.FamilyType;
                    tagInfo.DeviceType = ui.SelectedTag.DeviceType;
                    tagInfo.DeviceName = ui.SelectedTag.DeviceName;
                    value = tagInfo;
                }
                else
                {
                    tagInfo.DeviceName = ""; // clear
                    value = tagInfo;
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
