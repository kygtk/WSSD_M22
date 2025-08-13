using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.ModbusDevices
{
    public partial class FormModbusDevices : Form
    {
        #region Fields
        private Type m_CurrentType;
        private ModbusDeviceContainer m_Container = null;
        private ModbusDevice m_SelectedDevice = new ModbusDevice();
        private ModbusDeviceContainer m_FilterContainer = new ModbusDeviceContainer();
        private bool m_FilterOn = false;
        #endregion

        #region Constructor
        public FormModbusDevices()
        {
            InitializeComponent();
        }

        public FormModbusDevices(Type type)
        {
            m_CurrentType = type;
        }

        public FormModbusDevices(Type type, ModbusDeviceContainer container)
        {
            InitializeComponent();

            m_CurrentType = type;
            m_Container = container;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (type == typeof(ModbusBitInput))
            {
                UpdateDataGridView(dataGridView1, m_Container, "BitInputs");
            }
            else if (type == typeof(ModbusBitOutput))
            {
                UpdateDataGridView(dataGridView1, m_Container, "BitOutputs");
            }
            else if (type == typeof(ModbusWordInput))
            {
                UpdateDataGridView(dataGridView1, m_Container, "WordInputs");
            }
            else if (type == typeof(ModbusWordOutput))
            {
                UpdateDataGridView(dataGridView1, m_Container, "WordOutputs");
            }
        }
        #endregion

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            if (m_CurrentType == typeof(ModbusBitInput))
            {
                ModbusBitInput mi = new ModbusBitInput();
                m_Container.BitInputs.Add(mi);
                UpdateDataGridView(dataGridView1, m_Container, "BitInputs");
            }
            else if (m_CurrentType == typeof(ModbusBitOutput))
            {
                ModbusBitOutput mo = new ModbusBitOutput();
                m_Container.BitOutputs.Add(mo);
                UpdateDataGridView(dataGridView1, m_Container, "BitOutputs");
            }
            else if (m_CurrentType == typeof(ModbusWordInput))
            {
                ModbusWordInput wi = new ModbusWordInput();
                m_Container.WordInputs.Add(wi);
                UpdateDataGridView(dataGridView1, m_Container, "WordInputs");
            }
            else if (m_CurrentType == typeof(ModbusWordOutput))
            {
                ModbusWordOutput wo = new ModbusWordOutput();
                m_Container.WordOutputs.Add(wo);
                UpdateDataGridView(dataGridView1, m_Container, "WordOutputs");
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_Container.SaveDevicesToDisk();
        }

        private void UpdateDataGridView(DataGridView view, object obj, string dataMember)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = obj;
            bs.DataMember = dataMember;

            view.DataSource = bs;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //e.RowIndex;
            //dataGridView1[0, e.RowIndex].Value;
            if (e.RowIndex < 0) return;
            m_SelectedDevice = (ModbusDevice)dataGridView1.Rows[e.RowIndex].DataBoundItem;
        }

        public ModbusDevice GetSelectedItem()
        {
            return m_SelectedDevice;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.V)
                {
                    if (MessageBox.Show("Do you want to paste contents from clipboard?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

					Cursor.Current = Cursors.WaitCursor;

                    string s = Clipboard.GetText();
                    char[] splitter = { '\r', '\n' };
                    string[] lines = s.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    DataGridView view = sender as DataGridView;
                    int row = view.CurrentCell.RowIndex;
                    int col = view.CurrentCell.ColumnIndex;
                    foreach (string line in lines)
                    {
                        if (row < view.RowCount && line.Length > 0)
                        {
                            string[] cells = line.Split('\t');
                            for (int i = 0; i < cells.Length; i++)
                            {
                                if (col + i < view.ColumnCount)
                                {
                                    if (view[col + i, row].ValueType == typeof(Int32))
                                    {
                                        if (cells[i] != "")
                                            view[col + i, row].Value = Convert.ToInt32(cells[i]);
                                        else view[col + i, row].Value = 0;
                                    }
                                    else view[col + i, row].Value = cells[i];

                                }
                                else break;
                            }
                            row++;
                        }
                        else break;
                    }

					Cursor.Current = Cursors.Default;
					
					MessageBox.Show("Paste complete!");
                }
            }
            catch (Exception ex)
            {
				Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (!m_FilterOn)
            {
                this.chkFilter.Checked = true;
            }
            else
            {
                Filtering();
            }
        }

        private void Filtering()
        {
            if (m_CurrentType == typeof(ModbusBitInput))
            {
                m_FilterContainer.BitInputs.Clear();

                foreach (ModbusBitInput input in m_Container.BitInputs)
                {
                    if (input.Name.ToUpper().Contains(txtFilter.Text.ToUpper()))
                    {
                        m_FilterContainer.BitInputs.Add(input);
                    }
                }

                UpdateDataGridView(dataGridView1, m_FilterContainer, "BitInputs");
            }
            else if (m_CurrentType == typeof(ModbusBitOutput))
            {
                m_FilterContainer.BitOutputs.Clear();

                foreach (ModbusBitOutput Output in m_Container.BitOutputs)
                {
                    if (Output.Name.ToUpper().Contains(txtFilter.Text.ToUpper()))
                    {
                        m_FilterContainer.BitOutputs.Add(Output);
                    }
                }

                UpdateDataGridView(dataGridView1, m_FilterContainer, "BitOutputs");
            }
            else if (m_CurrentType == typeof(ModbusWordInput))
            {
                m_FilterContainer.WordInputs.Clear();

                foreach (ModbusWordInput input in m_Container.WordInputs)
                {
                    if (input.Name.ToUpper().Contains(txtFilter.Text.ToUpper()))
                    {
                        m_FilterContainer.WordInputs.Add(input);
                    }
                }

                UpdateDataGridView(dataGridView1, m_FilterContainer, "WordInputs");

            }
            else if (m_CurrentType == typeof(ModbusWordOutput))
            {
                m_FilterContainer.WordOutputs.Clear();

                foreach (ModbusWordOutput Output in m_Container.WordOutputs)
                {
                    if (Output.Name.ToUpper().Contains(txtFilter.Text.ToUpper()))
                    {
                        m_FilterContainer.WordOutputs.Add(Output);
                    }
                }

                UpdateDataGridView(dataGridView1, m_FilterContainer, "WordOutputs");
            }
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            m_FilterOn = (sender as CheckBox).Checked;

            if (m_FilterOn)
            {
                Filtering();
            }
            else
            {
                this.txtFilter.Text = "";
                Filtering();
            }

        }
    }
}