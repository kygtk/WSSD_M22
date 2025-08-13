using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public partial class ViewSlaveSelect2 : UserControl
    {
        private List<string> m_Pool = null;
        private static string m_FindName = "";
        private string m_SelectedName = "";
        private static bool m_AutoFind = false;

        [Browsable(false), XmlIgnore()]
        public string SelectedName
        {
            get { return m_SelectedName; }
        }

        public ViewSlaveSelect2()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(List<string> sourcePool)
        {
            m_Pool = sourcePool;

            this.checkBoxAutoFind.Checked = m_AutoFind;
            this.txtName.Text = m_FindName;

            if (!m_AutoFind)
            {
                Find();
            }
        }

        private static readonly char[] m_FindSplitter = new char[] { ' ' };

        private static bool FindName(string source)
        {
            string[] keys = m_FindName.Split(m_FindSplitter);

            bool matched = true;
            for (int i = 0; i < keys.Length; i++)
            {
                matched &= (source.IndexOf(keys[i], StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return matched;

            ////Ignore case but slow than "source.Contains".
            //return (source.IndexOf(m_FindName, StringComparison.OrdinalIgnoreCase) >= 0) ;
            ////return (source.Contains(m_FindName)) ;
        }

        private void Find()
        {
            m_FindName = this.txtName.Text;
            if (string.IsNullOrEmpty(m_FindName))
            {
                this.listBox.DataSource = m_Pool;
            }
            else
            {
                this.listBox.DataSource = m_Pool.FindAll(FindName);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Find();
        }

        private void listBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.listBox.SelectedIndex != -1)
            {
                m_SelectedName = this.listBox.SelectedValue.ToString();
            }
        }

        private void checkBoxAutoFind_CheckedChanged(object sender, EventArgs e)
        {
            m_AutoFind = ((CheckBox)sender).Checked;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            m_FindName = ((TextBox)sender).Text;
            if (m_AutoFind)
            {
                Find();
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Find();
            }
        }
    }
}
