using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Dms.DeviceLibrary;
using Dms.Ctl;
using Dms.Server;

namespace Dms.Util
{
    public partial class Form1 : Form
    {
        private TreeNode m_SelectedNode = null;
        private ModbusRootNode m_ModbusRootNode = null;

        private BindingSource m_BindSource = new BindingSource();
        
        public Form1()
        {
            InitializeComponent();

            m_ModbusRootNode = new ModbusRootNode();
            m_ModbusRootNode.SetConfigMode(true);
            this.formDeviceAssy1.MenuEnabled = true;
        }

        private void WriteNode(TreeNode ChildNode)
        {
            foreach (TreeNode node in ChildNode.Nodes)
            {
                ((DmsNode)node.Tag).WriteConfiguration();

                WriteNode(node);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.btnLoad.Enabled = false;
            this.btnCreate.Enabled = false;
            this.btnSimulateAddress.Enabled = false;

            if (!m_ModbusRootNode.NodeCreated)
            {
                m_ModbusRootNode.AddNode(null, 0, "ModbusNode").Initialize();
            }

            formDeviceAssy1.UpdateTreeViewWith(m_ModbusRootNode);
        }

        private void Initialize(DataGridView gridview, BindingSource bindSoruce, string member)
        {
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            /*
                if (null != m_SelectedNode)
                {
                    ((DmsNode)m_SelectedNode.Tag).WriteConfiguration();
                }
            */
            if (null != m_SelectedNode)
            {
                foreach (TreeNode node in formDeviceAssy1.DeviceTreeView.Nodes)
                {
                    ((DmsNode)node.Tag).WriteConfiguration();

                    WriteNode(node);
                }

                m_Container.Items.Clear();
                m_ModbusRootNode.GetDmsNodeNameList(m_ModbusRootNode.GetNode(), m_Container.Items, typeof(ModbusEqp));
                m_Container.WriteXml();

                MessageBox.Show("Xml Writing is OK", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private Dms.Common.DmsNodeTags m_Container = new Dms.Common.DmsNodeTags();
        

        private void dataGridViewEventAddress_KeyDown(object sender, KeyEventArgs e)
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
                            int count = cells.Length;
                            for (int i = 0; i < count; i++)
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
            catch (Exception ex)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
				Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void formDeviceAssy1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

        }

        private void formDeviceAssy1_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_SelectedNode = e.Node;
        }

        private void formDeviceAssy1_NodeAdd(object sender, EventArgs e)
        {
            Form2 frmEqpName = new Form2();

            frmEqpName.ShowDialog();

            string eqpName = frmEqpName.EqpName;

            if (eqpName != "")
            {
                m_ModbusRootNode.AddModbusEqp(eqpName);
            }
        }

        private void formDeviceAssy1_NodeDel(object sender, EventArgs e, TreeNode node)
        {
            if (m_ModbusRootNode.DelModbusEqp(node.Index) == true)
            {
                //node.Remove();
                node.Remove();

                //m_ModbusRootNode.GetNode().Nodes.Clear();

                for (int i = 0; i < m_ModbusRootNode.ModbusEqp.Count; i++)
                {
                    ModbusEqp eqp = m_ModbusRootNode.ModbusEqp[i];
                    TreeNode eqpTreeNode = eqp.GetNode();
                    eqpTreeNode.Text = string.Format("{0}[0.{1}]EQP {2} NODE", eqp.GetType().Name, i, eqp.EqpNodeName);
                    

                    for (int j = 0; j < eqpTreeNode.Nodes.Count; j++)
                    {
                        eqpTreeNode.Nodes[j].Text = string.Format("{0}[0.{1}.{2}]{3} {4}", eqp.GetType().Name, i, j, eqp.EqpNodeName, eqp.NodeNames[j]);
                    }
                }
                
                //formDeviceAssy1.UpdateTreeViewWith(m_ModbusRootNode);

            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.btnLoad.Enabled = false;
            this.btnCreate.Enabled = false;
            this.btnSimulateAddress.Enabled = false;

            if (!m_ModbusRootNode.NodeCreated)
            {
                m_ModbusRootNode.SetCreateMode(true);
                m_ModbusRootNode.AddNode(null, 0, "ModbusNode").Initialize();
                m_ModbusRootNode.SetCreateMode(false);

                formDeviceAssy1.UpdateTreeViewWith(m_ModbusRootNode);
            }
        }

        private void buttonSimulateAddress_Click(object sender, EventArgs e)
        {
            this.btnLoad.Enabled = false;
            this.btnCreate.Enabled = false;
            this.btnSimulateAddress.Enabled = false;
            this.btnWrite.Enabled = false;

            if (!m_ModbusRootNode.NodeCreated)
            {
                m_ModbusRootNode.SetCreateMode(true);
                m_ModbusRootNode.AddNode(null, 0, "ModbusNode").Initialize();
                m_ModbusRootNode.SetCreateMode(false);

                formDeviceAssy1.UpdateTreeViewWith(m_ModbusRootNode);
            }
        }
    }
}