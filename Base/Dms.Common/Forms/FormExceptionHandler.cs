using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
	public partial class FormExceptionHandler : Form
	{
		private XExceptionHandler m_ExceptionHandler = XExceptionHandler.Instance;
		private const int m_MaxCount = 100;

		public FormExceptionHandler()
		{
			CheckForIllegalCrossThreadCalls = false;

			InitializeComponent();
		}

		private void FormExceptionHandler_Load(object sender, EventArgs e)
		{
			XExceptionHandler.LogList = this.listBoxException;
		}
	}
}