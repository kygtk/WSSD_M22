///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.07.16
// Author       : EUN
// Description  : RPS(Remote Plasma System) - for cleaning chamber. (mks)
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class RPS : DmsUserControl
    {
        #region Tag Descriptor
        protected static TagDescriptorRPS tagDescriptor = new TagDescriptorRPS();
        #endregion

        #region Fields
        protected Color m_OnColor = Color.GreenYellow;
        protected Color m_OffColor = Color.Gray;
        protected Color m_ReadyColor = Color.WhiteSmoke;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }
        [Category("DMS : UI")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        [Category("DMS : UI")]
        public Color ReadyColor
        {
            get { return m_ReadyColor; }
            set { m_ReadyColor = value; }
        }
        #endregion

        #region Constructor
        public RPS()
        {
            InitializeComponent();
            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void RPS_Click(object sender, EventArgs e)
        {
            DlgRPS dlg = new DlgRPS(m_Tag);
            dlg.ShowDialog();
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tagContainer)
        {
            bool ok = base.Initialize(tagContainer);
            if (ok)
            {
                m_Initialized = ok;
                tmrUpdateState.Enabled = ok;
                UpdateState();
            }
            return m_Initialized;
        }
        protected override void UpdateState()
        {
            if (m_Tag[tagDescriptor.PLASMAON].Value == bool.TrueString || m_Tag[tagDescriptor.PLASMAON].Value == "1")
            {
                lblState.BackColor = m_OnColor;
            }
            else if (m_Tag[tagDescriptor.READY].Value == bool.TrueString || m_Tag[tagDescriptor.READY].Value == "1")
            {
                lblState.BackColor = m_ReadyColor;
            }
            else
            {
                lblState.BackColor = m_OffColor;
            }
        }
        #endregion
    }
}
