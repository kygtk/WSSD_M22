using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Device
{
    public partial class FormPropertyEdit : Form
    {
        private object m_SelectedObject;

        public FormPropertyEdit()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(object selectedObject)
        {
            m_SelectedObject = selectedObject;
            this.propertyGrid1.SelectedObject = m_SelectedObject;
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            IGenericCollection collection = m_SelectedObject as IGenericCollection;
            if (collection != null)
            {
                int count = collection.Count;
                for (int i = 0; i < count; i++)
                {
                    _Device device = collection.GetItem(i) as _Device;
                    if (device != null)
                    {
                        device.Id = i;
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}