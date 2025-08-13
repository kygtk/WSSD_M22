using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Device
{
    public partial class FormObjectConfig : Form
    {
        private object m_SelctedObject = null;

        public object SelectedObject
        {
            get { return m_SelctedObject; }
            set { m_SelctedObject = value; }
        }

        public FormObjectConfig()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(IComponentContainer components, object selectedItem, params string[] keys)
        {

            if (selectedItem != null)
            {
                this.viewObjectConfig1.InitializeCurrentConfigTree(selectedItem);
            }
            else
            {
                MessageBox.Show("Wrong data!");
            }

            IGenericCollection collection = selectedItem as IGenericCollection;
            if (collection != null)
            {
                this.viewObjectConfig1.InitializeComponentsTreeByFilter(components, collection.ContainedItemType, keys);
            }
            else
            {
                MessageBox.Show("Wrong data!");
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.SelectedObject = this.viewObjectConfig1.CurConfig;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}