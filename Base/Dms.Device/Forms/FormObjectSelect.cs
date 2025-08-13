using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Device
{
    public partial class FormObjectSelect : Form
    {
        private object m_SelctedObject = null;

        public object SelectedObject
        {
            get { return m_SelctedObject; }
            set { m_SelctedObject = value; }
        }

        public FormObjectSelect()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(IComponentContainer components, Type type, string name, params string[] keys)
        {
            this.viewObjectSelect1.ObjectName = name;
            this.viewObjectSelect1.InitializeByFilter(components, type, keys);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.SelectedObject = this.viewObjectSelect1.SelectedObject;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.SelectedObject = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}