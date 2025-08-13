///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.21
// Author       : jemoon
// Description  : Dlg form for Mapping Unit
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
    public partial class DlgMappingUnit : Form
    {
        private bool m_Inprocess = false;
        private int m_MaxSlotCount;
        private int m_PortId;
        private PortUnit m_PortUnit;
        private CstSlot1Position m_Slot1Position;
        private List<ViewCstSlot> m_Slots = new List<ViewCstSlot>();
        //private MappingStatus[] m_MappingStatus = null;

        private bool m_AutoMapping = false;

        public DlgMappingUnit()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public DlgMappingUnit(PortUnit port)
            : this()
        {
            m_PortUnit = port;
            m_PortId = m_PortUnit.Id;
            m_MaxSlotCount = m_PortUnit.MappingUnit.MaxSlotCount;
            m_Slot1Position = m_PortUnit.Cst.Slot1Position;
        }

        public DlgMappingUnit(PortUnit port, bool autoMapping)
            : this()
        {
            m_PortUnit = port;
            m_PortId = m_PortUnit.Id;
            m_MaxSlotCount = m_PortUnit.MappingUnit.MaxSlotCount;
            m_Slot1Position = m_PortUnit.Cst.Slot1Position;

            m_AutoMapping = autoMapping;
            m_Inprocess = autoMapping;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (!m_Inprocess)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Can not close! Mapping unit is processing!");
            }
        }

        private void DlgMappingUnit_Load(object sender, EventArgs e)
        {
            this.labelPortName.Text = m_PortUnit.Name;

            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                ViewCstSlot slot = new ViewCstSlot();
                slot.SlotId = i;
                slot.Exist = false;
                m_Slots.Add(slot);
            }

            DockStyle dockStyle;
            if (m_Slot1Position == CstSlot1Position.Bottom)
            {
                dockStyle = DockStyle.Bottom;
            }
            else
            {
                dockStyle = DockStyle.Top;
            }

            //Slot order에 따라 Bottom to Top, Top to Bottom은 동적으로 되어야 하겠지
            int height = 0;
            for (int i = (m_MaxSlotCount - 1); i >= 0; i--)
            {
                int slotNo = i + 1;
                if (slotNo / 5 > 0 && slotNo % 5 == 0)
                {
                    ViewCstSlotSeparator separator = new ViewCstSlotSeparator();
                    separator.No = slotNo.ToString();
                    separator.Dock = dockStyle;
                    this.panelCst.Controls.Add(separator);
                    height += separator.Size.Height;
                }

                ViewCstSlot slot = m_Slots[i];
                slot.Dock = dockStyle;
                this.panelCst.Controls.Add(slot);
                height += slot.Size.Height;
            }

            // Cst panel size 계산
            Size cstSizeOrg = this.panelCstFrame.Size;
            Size cstSizeNew = cstSizeOrg;
            cstSizeNew.Height = height + 15;

            // Dlg size 계산
            int margin = cstSizeNew.Height - cstSizeOrg.Height;
            Size dlgSize = this.Size;
            dlgSize.Height = dlgSize.Height + margin;
            this.Size = dlgSize;


            buttonMapping.Enabled = !m_AutoMapping;
            buttonClose.Enabled = !m_AutoMapping;

            this.timerUpdateState.Enabled = true;
        }

        private void UpdataSlotState()
        {
            MappingStatus[] mappingStatus = m_PortUnit.MappingUnit.GetGlassMappingStatus(m_PortId);

            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                m_Slots[i].Exist = ((mappingStatus[i] == MappingStatus.On));
            }
        }

        private void DoMapping()
        {
            if (m_Inprocess)
            {
                if (m_PortUnit.MappingUnit.Mapping(m_PortId) >= 0) m_Inprocess = false;
            }
        }

        private void UpdateButtonStatus()
        {
            bool enable = true;
            enable &= !m_Inprocess;
            buttonClose.Enabled = enable;

            enable &= !m_AutoMapping;
            buttonMapping.Enabled = enable;
        }

        private void timerUpdateState_Tick(object sender, EventArgs e)
        {
            UpdataSlotState();
            DoMapping();
            UpdateButtonStatus();
        }

        private void buttonMapping_Click(object sender, EventArgs e)
        {
            m_Inprocess = true;
            //while (m_PortUnit.MappingUnit.Mapping(m_PortId) == -1)
            //{ 
            //    // Noop : wait mapping complete
            //}
            //m_Inprocess = false;
        }
    }
}