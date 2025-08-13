using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Dms.Common;

namespace Dms.Util.IODefine
{
	public partial class ViewMelsecDeviceEdit : UserControl
	{
		enum ColumnId
		{ 
			InOutType,
			DeviceType,
			AddressType,
			Address,
			Name,
			IoType
		}

		private MelsecDeviceInfos m_DataSource;
		public ViewMelsecDeviceEdit()
		{
			InitializeComponent();
		}

		public void Initialize(MelsecDeviceInfos deviceInfos)
		{
			m_DataSource = deviceInfos;
			MakeGridView(this.dataGridViewDevices, m_DataSource);
		}

		private void MakeGridView(DataGridView view, MelsecDeviceInfos bindSource)
		{
			BindGridView(view, bindSource);

			DataGridViewComboBoxColumn colInoutType = new DataGridViewComboBoxColumn();
			colInoutType.DataSource = Enum.GetValues(typeof(IoInOutType));
			colInoutType.DataPropertyName = "InOutType";
			colInoutType.HeaderText = "In/Out";
			view.Columns.Add(colInoutType);
			
			DataGridViewComboBoxColumn colDeviceType = new DataGridViewComboBoxColumn();
			colDeviceType.DataSource = Enum.GetValues(typeof(devTYPE));
			colDeviceType.DataPropertyName = "DeviceType";
			colDeviceType.HeaderText = "DevType";
			view.Columns.Add(colDeviceType);

			DataGridViewTextBoxColumn colAddressType = new DataGridViewTextBoxColumn();
			colAddressType.DataPropertyName = "AddressType";
			colAddressType.HeaderText = "Index";
			view.Columns.Add(colAddressType);

			DataGridViewTextBoxColumn colAddress = new DataGridViewTextBoxColumn();
			colAddress.DataPropertyName = "AddressStr";
			colAddress.HeaderText = "Address";
			view.Columns.Add(colAddress);

			DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
			colName.DataPropertyName = "Name";
			colName.HeaderText = "Name";
			view.Columns.Add(colName);

			DataGridViewTextBoxColumn colIoType = new DataGridViewTextBoxColumn();
			colIoType.DataPropertyName = "IoType";
			colIoType.HeaderText = "I/O";
			view.Columns.Add(colIoType);

			//DataGridViewTextBoxColumn colIdByIoType = new DataGridViewTextBoxColumn();
			//colIdByIoType.DataPropertyName = "IdByIoType";
			//colIdByIoType.HeaderText = "Id";
			//view.Columns.Add(colIdByIoType);
		}

		private void BindGridView(DataGridView view, MelsecDeviceInfos bindSource)
		{
			BindingSource bs = new BindingSource();
			bs.DataSource = bindSource;
			bs.DataMember = "Items";

			view.AutoGenerateColumns = false;
			view.DataSource = bs;
		}

		public void UpdateGridView()
		{
			BindGridView(dataGridViewDevices, m_DataSource);
		}


		private void dataGridViewDevices_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			DataGridView view = (DataGridView)sender;
			MessageBox.Show("Data Error : " + e.Exception.ToString());
			e.ThrowException = false;
		}

		private void AddRows()
		{
			FormNodeAdd form = new FormNodeAdd();
			form.Text = " * Add object *";
			int curRowId = dataGridViewDevices.Rows.Count - 1;
			if (form.ShowDialog() == DialogResult.OK)
			{
				int count = form.Count;
				for (int i = 0; i < count; i++)
				{
					MelsecDeviceInfo info = new MelsecDeviceInfo();
					m_DataSource.Items.Add(info);
				}

				BindGridView(dataGridViewDevices, m_DataSource);

				dataGridViewDevices.ClearSelection();
				//dataGridViewDevices.Rows[curRowId + 1].Selected = true; //Add된 Row의 첫번째로 선택
				dataGridViewDevices.CurrentCell = dataGridViewDevices[0, curRowId + 1]; 
				dataGridViewDevices.Refresh();
			}
		}

		private void InsertRows(int index)
		{
			FormNodeAdd form = new FormNodeAdd();
			form.Text = " * Insert object *";
			int curRowId = index;
			if (form.ShowDialog() == DialogResult.OK)
			{
				int count = form.Count;
				for (int i = 0; i < count; i++)
				{
					MelsecDeviceInfo info = new MelsecDeviceInfo();
					m_DataSource.Items.Insert(index + i, info);
				}

				BindGridView(dataGridViewDevices, m_DataSource);
				
				dataGridViewDevices.ClearSelection();
				//dataGridViewDevices.Rows[curRowId].Selected = true; //Insert된 Row의 첫번째로 선택
				dataGridViewDevices.CurrentCell = dataGridViewDevices[0, curRowId]; 
				dataGridViewDevices.Refresh();
			}		
		}

		private void dataGridViewDevices_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				InsertRows(e.RowIndex);
			}
		}

		private void dataGridViewDevices_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				AddRows();
			}
			else
			{
				int columnIndex = e.ColumnIndex;
				if (columnIndex == (int)ColumnId.InOutType)
				{
					MelsecDeviceInfos.SortByInout(m_DataSource);
					dataGridViewDevices.Refresh();
				}
				else if (columnIndex == (int)ColumnId.DeviceType)
				{
					MelsecDeviceInfos.SortByDevType(m_DataSource);
					dataGridViewDevices.Refresh();
				}
				else if (columnIndex == (int)ColumnId.IoType)
				{
					MelsecDeviceInfos.SortByIoType(m_DataSource);
					dataGridViewDevices.Refresh();				
				}
			}
		}

		private void dataGridViewDevices_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex != -1 && e.ColumnIndex != -1)
			{
				AddRows();
			}
		}

		private void dataGridViewDevices_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyCode == Keys.V)
				{
					DataGridView view = sender as DataGridView;
					DataGridViewCell cell;
					int row = view.CurrentCell.RowIndex;
					int col = view.CurrentCell.ColumnIndex;
					int colCount = view.ColumnCount;

					// 시작 column idexe가 Name이 아니면 return
					//if (col != (int)ColumnId.Name)
					//{
					//    MessageBox.Show("Please select Name");
					//}

					if (MessageBox.Show("Do you want to paste contents from clipboard?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

					Cursor.Current = Cursors.WaitCursor;
					view.Enabled = false;

					string s = Clipboard.GetText();
					char[] splitter = { '\r', '\n' };
					string[] lines = s.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
					int lineCount = lines.Length;
					
					string line;
					for (int i = 0; i < lineCount; i++)
					{
						line = lines[i];
						if (row < view.RowCount && line.Length > 0)
						{
							string[] cells = line.Split('\t');
							int cellCount = cells.Length;
							for (int j = 0; j < cellCount; j++)
							{
								if (col + j < colCount)
								{
									cell = view[col + j, row];
									cell.Value = cells[j];
								}
								else break;
							}
							row++;
						}
						else break;
					}

					view.Enabled = true;
					Cursor.Current = Cursors.Default;

					MessageBox.Show("Paste complete!");
				}
			}
			catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				Cursor.Current = Cursors.Default;
				MessageBox.Show(err.Message.ToString());
			}
		}
	}
}
