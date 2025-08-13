///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.02
// Author       : L.Y.S
// Description  : MFC UserControl
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;
using Dms.Server;


namespace Dms.Control
{
    [ToolboxBitmap(typeof(Mfc), "MfcIcon")]
    public partial class Mfc : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMfc tagDescriptor = new TagDescriptorMfc();
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private ClientManager m_Client = null;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public string MfcName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }
        [Category("DMS : UI")]
        public Color MfcBackColor
        {
            get { return lblBackColor.BackColor; }
            set { lblBackColor.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color CurFlowBackColor
        {
            get { return lblCurFlow.BackColor; }
            set { lblCurFlow.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color CurFlowForeColor
        {
            get { return lblCurFlow.ForeColor; }
            set { lblCurFlow.ForeColor = value; }
        }
        [Category("DMS : UI")]
        public Font CurFlowFont
        {
            get { return lblCurFlow.Font; }
            set { lblCurFlow.Font = value; }
        }
        [Category("DMS : UI")]
        public Color NameColor
        {
            get { return lblName.BackColor; }
            set { lblName.BackColor = lblMfc.BackColor = value; }
        }
        #endregion

        #region Constructor
        public Mfc()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void Mfc_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                DlgMfc dlg = new DlgMfc();

                dlg.Initialize(m_Tag);

                if (dlg.DialogResult == DialogResult.Cancel) return;
                else dlg.ShowDialog(); 
            }
            else MessageBox.Show("Tag is not Selected.");
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (m_Tag[tagDescriptor.ON].Value == true.ToString() || (m_Tag[tagDescriptor.ON].Value == "1") )
            {
                this.BackColor = Color.GreenYellow;

                lblName.BackColor = Color.GreenYellow;
                lblMfc.BackColor = Color.GreenYellow;

            }
            else if (m_Tag[tagDescriptor.ON].Value == false.ToString() || (m_Tag[tagDescriptor.ON].Value == "0"))
            {
                this.BackColor = Color.FromArgb(192, 192, 255);

                lblName.BackColor = Color.FromArgb(192, 192, 255);
                lblMfc.BackColor = Color.FromArgb(192, 192, 255);
            }

            lblCurFlow.Text = m_Tag[tagDescriptor.CURVAL].Value;
        }
        #endregion
    }
}