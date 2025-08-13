///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.21
// Author       : jemoon
// Description  : Dlg form for BCR Unit
//-------------------------------------------------------------------------
// Revison History
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Device;
using Dms.Common;

namespace Dms.Control
{
    public partial class DlgBCRUnit : Form
    {
        private bool m_Inprocess = false;
        private int m_PortId;
        private PortUnit m_PortUnit;

        public DlgBCRUnit()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public DlgBCRUnit(PortUnit port) : this()
        {
            m_PortUnit = port;
            m_PortId = m_PortUnit.Id;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (!m_Inprocess)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Can not close! BCR unit is processing!");
            }
        }

        private void DlgMappingUnit_Load(object sender, EventArgs e)
        {
            this.labelBcrName.Text = m_PortUnit.Name;
            this.timerUpdateState.Enabled = true;
        }

        private void UpdataCstId()
        {
            string oldData = this.labelBcrData.Text;
            string curData = m_PortUnit.BCR.GetData(m_PortId);
            if (oldData != curData && curData != null)
            {
//                this.labelCstId.Text = curData;
                this.labelBcrData.Text = m_PortUnit.BCR.GetData(m_PortId);
            }
        }

        private void BcrReading()
        {
            if (m_Inprocess)
            {
                if (m_PortUnit.BCR.Reading(m_PortId) >= 0) m_Inprocess = false;
            }
        }

        private void timerUpdateState_Tick(object sender, EventArgs e)
        {
            UpdataCstId();
            BcrReading();
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            m_Inprocess = true;
        }

        private void labelCstId_Click(object sender, EventArgs e)
        {
            if(!m_Inprocess)
            {
                KeyPadTextBox keyPad = new KeyPadTextBox(this.labelBcrData.Text);
                KeyInValidation validation = new KeyInValidation();
                validation.Format = OptionFormat.String;
                keyPad.Validation = validation;
                if(keyPad.ShowDialog() == DialogResult.OK)
                {
                    this.labelBcrData.Text = keyPad.NewValue;
                    m_PortUnit.BCR.SetData(m_PortId, this.labelBcrData.Text);
                }
                //jemoon : 110607
                //ShowDialog甫 荤侩窍咯 汽阑 钎矫茄 版快, Dispose甫 龋免窍咯 汽狼 葛电 牧飘费阑 啊厚瘤 荐笼贸府.
                keyPad.Dispose();
            }
        }
    }
}