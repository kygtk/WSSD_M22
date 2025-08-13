using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Server
{
	public partial class FormScrapCode : Form
	{
		public ScrapCode SelcetedCode = ScrapCode.EqpTrouble;

		public FormScrapCode()
		{
			InitializeComponent();
		}

		private void FormScrapCode_Load(object sender, EventArgs e)
		{
			this.comboBoxCodeList.DataSource = Enum.GetValues(typeof(ScrapCode));
		}

		private void buttonSelect_Click(object sender, EventArgs e)
		{
			SelcetedCode = (ScrapCode)comboBoxCodeList.SelectedItem;
			this.DialogResult = DialogResult.OK;
		}												

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}