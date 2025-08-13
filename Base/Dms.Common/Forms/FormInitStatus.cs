using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class FormInitStatus : Form
    {
        private bool m_Initialized = false;
        private GenericTags m_InitItems = new GenericTags();
        private Size m_OrgSize;
        public bool Initialized
        {
            get { return m_Initialized; }
        }

        public FormInitStatus()
        {
            InitializeComponent();
        }

        public void Initialize(List<XSeqInitFunction> initSeqfuncs)
        {
            // init items
            foreach (XSeqInitFunction func in initSeqfuncs)
            { 
                foreach (GenericTag item in func.InitCheckItems )
                {
					if (item != null)
					{
						m_InitItems.Add(item);
					}
                }
            }
            
            // init gridview
            InitGridView();

            m_Initialized = true;
        }

        private void InitGridView()
        {
            int rows = m_InitItems.Count;
            for (int i = 0; i < rows; i++)
            {
                GenericTag item = m_InitItems[i];
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dataGridViewInitItem);
                DataGridViewCellCollection cells = row.Cells;
                cells[ColumnItem.DisplayIndex].Value = item.Key;
                cells[ColumnStatus.DisplayIndex].Value = item.Value;

                this.dataGridViewInitItem.Rows.Add(row);
            }
        }

        private void UpdataeGridView()
        {
            try
            {
                bool complete = true;
                int rows = this.dataGridViewInitItem.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    GenericTag item = m_InitItems[i];
                    complete &= (item.Value == InitCheckState.OK || item.Value == InitCheckState.NoUse);

                    Color setColor = Color.White;
                    switch (item.Value)
                    {
                        case InitCheckState.NotReady:
                            setColor = Color.WhiteSmoke;
                            break;
                        case InitCheckState.Checking:
                            setColor = Color.Yellow;
                            break;
                        case InitCheckState.NG:
                            setColor = Color.Red;
                            break;
                        case InitCheckState.NoUse:
                            setColor = Color.LightBlue;
                            break;
                        case InitCheckState.OK:
                            setColor = Color.Lime;
                            break;
                        case InitCheckState.ServoEStop:
                            setColor = Color.LightPink;
                            break;
                        case InitCheckState.ServoHoming:
                            setColor = Color.GreenYellow;
                            break;
                        case InitCheckState.ServoReset:
                            setColor = Color.Orange;
                            break;
                    }

                    DataGridViewRow row = this.dataGridViewInitItem.Rows[i];
                    DataGridViewCellCollection cells = row.Cells;
                    cells[ColumnStatus.DisplayIndex].Value = item.Value;
                    cells[ColumnStatus.DisplayIndex].Style.BackColor = setColor;                    
                }

                if (complete) this.dataGridViewInitItem.Enabled = false; // 11.03.02 minhan
                this.timerUpdateViewer.Enabled = !complete;
                this.timerFormCloser.Enabled = complete;
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                UpdataeGridView();
            }
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Hide")
            {
                ((Button)sender).Text = "Show";
                this.ClientSize = new Size(m_OrgSize.Width, 35);
            }
            else
            {
                ((Button)sender).Text = "Hide";
                this.ClientSize = m_OrgSize;
            }
        }

        private void FormInitStatus_Load(object sender, EventArgs e)
        {
            m_OrgSize = this.ClientSize;
        }

        private void dataGridViewInitItem_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridViewInitItem.ClearSelection();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}