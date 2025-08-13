///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.20
// Author       : jemoon
// Description  : UserControl for PortUnit
//-------------------------------------------------------------------------
// Revison History
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using Dms.Client;
using Dms.ServerCommon;

namespace Dms.Control
{
    public partial class ViewPortUnit : DmsUserControl
    {
        #region Fields
        private ClientManager m_Client = null;
        private PortUnit m_PortUnit;
        private CstSlot1Position m_Slot1Position;
        private List<ViewCstSlot> m_Slots = new List<ViewCstSlot>();
        private int m_MaxSlotCount;
        #endregion

        #region Constructor
        public ViewPortUnit()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        public bool Initialize()
        {
            m_Client = ClientManager.Instance;
            IComponentContainer components = DmsComponents.Instance.ComponentContainer;

            m_PortUnit = components[m_TagInfo.DeviceName] as PortUnit;

            if (m_PortUnit == null)
            {
                string msg = string.Format("Device of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_Slot1Position = m_PortUnit.Cst.Slot1Position;
                m_MaxSlotCount = m_PortUnit.MappingUnit.MaxSlotCount;

                MakeCstSlotView();
            }

            return true;
        }

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (!Initialize()) return false;

            if (ok)
            {
                m_Initialized = ok;

                this.labelPortName.Text = m_PortUnit.Name;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                m_Slots[i].SetSlotStatus(m_PortUnit.Cst.GetGlassStatus(i));
                //m_Slots[i].SetSlotStatus((GlassStatus)(i%10)); // test
            }

            this.labelCstId.Text = m_PortUnit.Cst.CstID;

            this.labelPortStatus.Text = m_PortUnit.PortStatus.ToString();
        }
        #endregion

        private void labelCstId_Click(object sender, EventArgs e)
        {
            if (m_Client.GenInfos.AutoMode) return;
            if (m_Client.CurrentUserLevel < UserLevels.Technician) return;

            if (m_PortUnit.MappingUnit != null)
            {
                DlgBCRUnit dlg = new DlgBCRUnit(m_PortUnit);
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
                dlg.Dispose();
            }
        }

        private void MakeCstSlotView()
        {
            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                ViewCstSlot slot = new ViewCstSlot();
                slot.SlotId = i;
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
            for (int i = (m_MaxSlotCount - 1); i >= 0; i--)
            {
                int slotNo = i + 1;
                if (slotNo / 5 > 0 && slotNo % 5 == 0)
                {
                    ViewCstSlotSeparator separator = new ViewCstSlotSeparator();
                    separator.No = slotNo.ToString();
                    separator.Dock = dockStyle;
                    this.panelCst.Controls.Add(separator);
                }

                ViewCstSlot slot = m_Slots[i];
                slot.Dock = dockStyle;
                this.panelCst.Controls.Add(slot);
            }
        }

        private void ViewPortUnit_Load(object sender, EventArgs e)
        {
        }

        private void labelPortName_Click(object sender, EventArgs e)
        {
            if (m_Client.GenInfos.AutoMode) return;
            if (m_Client.CurrentUserLevel < UserLevels.Technician) return;

            if (m_PortUnit.MappingUnit != null)
            {
                DlgMappingUnit dlg = new DlgMappingUnit(m_PortUnit);
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
                dlg.Dispose();
            }
        }
    }
}
