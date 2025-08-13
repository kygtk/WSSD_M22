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
    public partial class ViewBomAnalysis : UserControl
    {
        private bool m_ShowCost = true;
        private IoDefines m_IoDefines = null;
        private List<IoPart> m_ContainedTypes = null;
        
        public ViewBomAnalysis()
        {
            InitializeComponent();
        }

        public void Initialize(IoDefines iodefines)
        {
            m_IoDefines = iodefines;

            InitializeGridView();

            this.dataGridView1.ClearSelection();
        }

        public void Initialize(IoDefines iodefines, bool showCost)
        {
            m_ShowCost = showCost;
            
            Initialize(iodefines);
        }

        private void InitializeGridView()
        {
            long totalAmount = 0;
            m_ContainedTypes = m_IoDefines.GetContainedPartsTypes();
            int rows = m_ContainedTypes.Count;
            for (int i = 0; i < rows; i++)
            {
                IoPart ioPart = m_ContainedTypes[i];
                List<IoPart> ioParts = m_IoDefines.GetContainedParts(ioPart.GetType());

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dataGridView1);
                DataGridViewCellCollection cells = row.Cells;
                cells[ColDevice.DisplayIndex].Value = ioPart.GetType().Name;
                cells[ColPartCode.DisplayIndex].Value = ioPart.PartCode;
                cells[ColPartName.DisplayIndex].Value = ioPart.PartName;
                cells[ColPartSpec.DisplayIndex].Value = ioPart.PartSpec;
                cells[ColCount.DisplayIndex].Value = ioParts.Count;

                if (m_ShowCost)
                {
                    int partPrice = ioPart.PartPrice;
                    long amount = ioParts.Count * partPrice;
                    totalAmount += amount;

                    cells[ColPartPrice.DisplayIndex].Value = string.Format("{0:N0}", partPrice);
                    cells[ColAmount.DisplayIndex].Value = string.Format("{0:N0}", amount);
                }

                this.dataGridView1.Rows.Add(row);
            }

            ColPartPrice.Visible = m_ShowCost;
            ColAmount.Visible = m_ShowCost;

            if (m_ShowCost)
            {
                this.labelTotalAmountTitle.Visible = true;
                this.labelTotalAmount.Visible = true;
                this.labelTotalAmount.Text = string.Format("{0:N0}", totalAmount);
            }
        }
    }
}
