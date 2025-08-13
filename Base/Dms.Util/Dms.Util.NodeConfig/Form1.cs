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
        //private ServerManager m_ServerManager = ServerManager.Instance;
        private RootNode m_RootNode = null;

        private BindingSource m_BindSource = new BindingSource();

        public Form1()
        {
            InitializeComponent();

            Melsec.IsConfigurator = true;
            m_RootNode = new RootNode();
			m_RootNode.SetConfigMode(true);
            m_BindSource.DataSource = m_RootNode.MelsecBoardControl;
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

            if (!m_RootNode.NodeCreated)
            {
                m_RootNode.AddNode(null, 0, "RootNode").Initialize();
            }

            formDeviceAssy1.UpdateTreeViewWith(m_RootNode);
            viewMelsecNetMonitor1.Initialize(m_RootNode);
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
                m_RootNode.GetDmsNodeNameList(m_RootNode.GetNode(), m_Container.Items, typeof(DmsNode));
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
                    char[] splitter = { '\r', '\n'};
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.btnLoad.Enabled = false;
            this.btnCreate.Enabled = false;
            this.btnSimulateAddress.Enabled = false;

            if (!m_RootNode.NodeCreated)
            {
                m_RootNode.SetCreateMode(true);
                m_RootNode.AddNode(null, 0, "RootNode").Initialize();
                m_RootNode.SetCreateMode(false);
               
                formDeviceAssy1.UpdateTreeViewWith(m_RootNode);
            }
        }

        private void buttonSimulateAddress_Click(object sender, EventArgs e)
        {
            this.btnLoad.Enabled = false;
            this.btnCreate.Enabled = false;
            this.btnSimulateAddress.Enabled = false;
            this.btnWrite.Enabled = false;

            if (!m_RootNode.NodeCreated)
            {
                m_RootNode.SetCreateMode(true);
                m_RootNode.MelsecBoardControl.SetSimuateAddressMode(true);
                m_RootNode.AddNode(null, 0, "RootNode").Initialize();
                m_RootNode.SetCreateMode(false);

                formDeviceAssy1.UpdateTreeViewWith(m_RootNode);
                this.viewMelsecNetMonitor1.Initialize(m_RootNode);
            }
        }
    }
}