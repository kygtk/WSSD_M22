using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Common
{
    [Serializable()]
    [Editor(typeof(UIEditorKeyPadSelect), typeof(UITypeEditor))]
    public class KeyPadInfo
    {
        private Type m_Type = null;
        public Type KeyPadType
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public KeyPadInfo()
        { 
        }

        public KeyPad CreateInstance()
        {
            return Activator.CreateInstance(m_Type) as KeyPad;
        }
        
        public override string ToString()
        {
            return (m_Type != null) ? m_Type.Name : "Not Seleted";
        }
    }

    [Serializable()]
    public class KeyPad : Form
    {
        #region Fields
        protected KeyInValidation m_Validation = null;
        protected string m_OldValue = "";
        protected string m_NewValue = "";
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public KeyInValidation Validation
        {
            get { return m_Validation; }
            set { m_Validation = value; }
        }
        [Category("DMS : Setting")]
        public string OldValue
        {
            get { return m_OldValue; }
            set { m_OldValue = value; }
        }
        [Category("DMS : Setting")]
        public string NewValue
        {
            get { return m_NewValue; }
            set { m_NewValue = value; }
        }
        [Category("DMS : Setting")]
        public string Caption
        {
            get { return this.Text; }
            set { this.Text = value; }
        }
        #endregion

        #region Constructor
        public KeyPad()
        { 
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.GetType().Name;
        }
        #endregion
    }
}
