using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Ctl;

namespace Dms.Control
{
    public partial class ServoTabMp2300MapDataCtrl : UserControl
    {
        #region Fields
        private MP2300Ctl m_Mp2300;
        private const int MAXROW = 20;
        private const int MAXCOL = 18;
        private const int INADDRSTART = 10000;
        private const int OUTADDRSTART = 12000;
        private int m_StartAddrIn = INADDRSTART;
        private int m_SetPageIn = 0;
        private int m_StartAddrOut = OUTADDRSTART;
        private int m_SetPageOut = 0;
        private ushort[] m_OldInBuf;
        private ushort[] m_OldOutBuf;
        private bool m_FirstIn = false;
        private bool m_FirstOut = false;
        #endregion

        public ServoTabMp2300MapDataCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #region Methods
        public void Initialize()
        {
            m_Mp2300 = MP2300Ctl.Instance;
            m_OldInBuf = new ushort[20];
            m_OldOutBuf = new ushort[20];

            InitGridView();
            SetInputIndex();
            SetOutputIndex();
            SetAddressList();
            SetInputData();
            SetOutputData();
        }

        public void SetMonitorTimer(bool enable)
        {
            tmrUpdateState.Enabled = enable;
        }

        private void InitGridView()
        {
            DataGridViewTextBoxColumn col;

            //Input GridView
            int count = 0;
            for (int i = 0; i < MAXCOL; i++)
            {
                col = new DataGridViewTextBoxColumn();
                if (i == 0)
                {
                    col.HeaderText = "Address";
                    col.Width = 65;
                }
                else if (i == 1)
                {
                    col.HeaderText = "Data";
                    col.Width = 45;
                }
                else
                {
                    col.HeaderText = string.Format("{0:X}", count);
                    count++;
                    col.Width = 18;
                }
                this.gridViewInput.Columns.Add(col);
            }

            this.gridViewInput.Rows.Add(MAXROW);

            //Output GridView
            count = 0;
            for (int i = 0; i < MAXCOL; i++)
            {
                col = new DataGridViewTextBoxColumn();
                if (i == 0)
                {
                    col.HeaderText = "Address";
                    col.Width = 65;
                }
                else if (i == 1)
                {
                    col.HeaderText = "Data";
                    col.Width = 45;
                }
                else
                {
                    col.HeaderText = string.Format("{0:X}", count);
                    count++;
                    col.Width = 18;
                }
                this.gridViewOutput.Columns.Add(col);
            }

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "SET";
            col.Width = 45;
            this.gridViewOutput.Columns.Add(col);

            this.gridViewOutput.Rows.Add(MAXROW);

            m_FirstIn = true;
            m_FirstOut = true;
        }

        private void SetInputIndex()
        {
            for (int i = 0; i < MAXROW; i++)
            {
                this.gridViewInput.Rows[i].Height = 20;
                this.gridViewInput[0, i].Value = string.Format("MW{0:d05}", (m_StartAddrIn + m_SetPageIn * 20) + i);
            }
        }

        private void SetOutputIndex()
        {
            for (int i = 0; i < MAXROW; i++)
            {
                this.gridViewOutput.Rows[i].Height = 20;
                this.gridViewOutput[0, i].Value = string.Format("MW{0:d05}", (m_StartAddrOut + m_SetPageOut * 20) + i);
            }
        }

        private void SetAddressList()
        {
            int count = MP2300Ctl.IF_NUM / 20;
            string addr;
            for (int i = 0; i < count; i++)
            {
                addr = string.Format("MW{0:d05}", INADDRSTART + 20 * i);
                cboInput.Items.Add(addr);
                addr = string.Format("MW{0:d05}", OUTADDRSTART + 20 * i);
                cboOutput.Items.Add(addr);
            }
        }

        private void SetInputData()
        {
            for (int row = 0; row < MAXROW; row++)
            {
                int index = m_StartAddrIn - INADDRSTART + row;
                ushort buf = m_Mp2300.InBuffer[index];

                if (m_FirstIn || (m_OldInBuf[row] != buf))
                {
                    m_OldInBuf[row] = buf;
                    for (int col = 1; col < MAXCOL; col++)
                    {
                        if (col == 1)
                        {
                            this.gridViewInput[col, row].Value = string.Format("{0:X04}", buf);
                        }
                        else
                        {
                            int val = ((buf >> (col-2) & 0x01) != 0 ? 1 : 0);
                            this.gridViewInput[col, row].Value = string.Format("{0:d01}", val);
                        }
                    }

                    this.gridViewInput.InvalidateRow(row);
                }
            }

            m_FirstIn = false;
        }

        private void SetOutputData()
        {
            for (int row = 0; row < MAXROW; row++)
            {
                int index = m_StartAddrOut - OUTADDRSTART + row;
                ushort buf = m_Mp2300.OutBuffer[index];

                if (m_FirstOut || (m_OldOutBuf[row] != buf))
                {
                    m_OldOutBuf[row] = buf;
                    for (int col = 1; col < MAXCOL+1; col++)
                    {
                        if (col == 1 || col == MAXCOL)
                        {
                            this.gridViewOutput[col, row].Value = string.Format("{0:X04}", buf);
                        }
                        else
                        {
                            int val = ((buf >> (col - 2) & 0x01) != 0 ? 1 : 0);
                            this.gridViewOutput[col, row].Value = string.Format("{0:d01}", val);
                        }
                    }

                    this.gridViewOutput.InvalidateRow(row);
                }
            }

            m_FirstOut = false;
        }
        
        private void cboInput_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox box = (sender as ComboBox);
            int index = box.SelectedIndex;
            if (index >= 0)
            {
                m_StartAddrIn = INADDRSTART + index * 20;
                m_SetPageIn = 0;
                SetInputIndex();
                SetInputData();
            }
        }

        private void cboOutput_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox box = (sender as ComboBox);
            int index = box.SelectedIndex;
            if (index >= 0)
            {
                m_StartAddrOut = OUTADDRSTART + index * 20;
                m_SetPageOut = 0;
                SetOutputIndex();
                SetOutputData();
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            SetInputData();
            SetOutputData();
        }
        #endregion
    }
}
