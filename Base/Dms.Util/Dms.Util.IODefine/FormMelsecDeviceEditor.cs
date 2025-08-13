using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Util.IODefine
{
	public partial class FormMelsecDeviceEditor : Form
	{
		private MelsecDeviceInfos m_MelDevInfos = null;
		private MelsecDeviceInfos m_MelDevInfosTemp = new MelsecDeviceInfos();

		public FormMelsecDeviceEditor()
		{
			InitializeComponent();
		}

		public void Initialize(MelsecDeviceInfos infos)
		{
			m_MelDevInfos = infos;

			m_MelDevInfosTemp.Items.Clear();
			foreach(MelsecDeviceInfo info in m_MelDevInfos.Items)
			{
				m_MelDevInfosTemp.Items.Add(info);
			}

			this.viewMelsecDevices1.Initialize(m_MelDevInfosTemp);
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = m_Updated ? DialogResult.OK : DialogResult.Cancel;
			this.Close();
		}

		private bool m_Updated = false;
		private void UpdateItem()
		{
			// Trim not valid object
			int count = m_MelDevInfosTemp.Items.Count;
			for (int i = count - 1; i >= 0; i-- )
			{
				if (!m_MelDevInfosTemp.Items[i].IsValid())
				{
					m_MelDevInfosTemp.Items.RemoveAt(i);
				}
			}

			//Sort
			MelsecDeviceInfos.SortByIoType(m_MelDevInfosTemp);

			// commit
			m_MelDevInfos.Items.Clear();
			foreach (MelsecDeviceInfo info in m_MelDevInfosTemp.Items)
			{
				m_MelDevInfos.Items.Add(info);
			}

			//Refresh gridView
			this.viewMelsecDevices1.UpdateGridView();

			m_Updated = true;
		}

		private void buttonCommit_Click(object sender, EventArgs e)
		{
			UpdateItem();
		}
	}
}