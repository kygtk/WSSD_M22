using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Dms.Ctl
{
	public class UIEditorTagAddressMapCollection : UITypeEditor
	{
        #region Contstructor
        public UIEditorTagAddressMapCollection()
        {
        } 
        #endregion

        private CollectionEditor _editor = new CollectionEditor(typeof(List<TagAddressMap>)); //change ArrayList to your type

        #region Override
        public override object  EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
    	{
            if (value != null)
            {
                value = this._editor.EditValue(context, sp, value);
                List<TagAddressMap> list = (List<TagAddressMap>)value;  //change ArrayList to your type

                List<TagAddressMap> Clone = new List<TagAddressMap>();

                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    Clone.Add(list[i]);
                }

                return Clone;
            }

            return base.EditValue(context, sp, value);
		}

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
            return this._editor.GetEditStyle(context);
        }
        #endregion
	}
}
