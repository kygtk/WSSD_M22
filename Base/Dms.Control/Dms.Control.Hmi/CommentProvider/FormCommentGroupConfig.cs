using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control.Hmi
{
	public partial class FormCommentGroupConfig : Form
	{
		#region Fields
		private HmiCommentProvider m_Provider;
		#endregion

		public FormCommentGroupConfig()
		{
			InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.CacheText, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		public void Initialize(HmiCommentProvider provider)
		{
			m_Provider = provider;
			this.viewCommentGroupConfig1.Initialize(m_Provider);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}