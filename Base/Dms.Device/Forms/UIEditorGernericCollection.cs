///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.09.06
// Author       : eunsung
// Description  : CollectionEditor for _GenericCollection
//-------------------------------------------------------------------------
// Revison History
// * 2008.09.08 : jemoon - using DmsCollectionEditor : CollectionEditor
//                override CreateInstance(Type) - Create instance by m_ContainedObjectType
///////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using Dms.Util.IODefine;
using Dms.Common;
using System.Collections;
using System.ComponentModel.Design;

namespace Dms.Device
{

    public class GenericCollectionEditor : CollectionEditor
    {
        private Type m_ContainedObjectType;

        #region Contstructor
        public GenericCollectionEditor(Type itemType) : base(itemType)
        {
        }
        #endregion

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            m_ContainedObjectType = ((IGenericCollection)context.Instance).ContainedItemType;
            
            return base.EditValue(context, provider, value);
        }

        protected override object CreateInstance(Type itemType)
        {
            return Activator.CreateInstance(m_ContainedObjectType);
        }  
    }


	public class UIEditorGenericCollection : UITypeEditor
	{
        #region Contstructor
        public UIEditorGenericCollection()
        {
        } 
        #endregion

        private GenericCollectionEditor _editor = new GenericCollectionEditor(typeof(ArrayList)); //change ArrayList to your type

        #region Override
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value) 
		{
            if (value != null)
            {
                value = this._editor.EditValue(context, sp, value);
                ArrayList list = (ArrayList)value;  //change ArrayList to your type

               return list.Clone();
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
