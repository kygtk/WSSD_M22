///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.02
// Author       : L.Y.S
// Description  : Mfc Manual Operation Dialog
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Server;

namespace Dms.Control
{
    public partial class DlgMfc : Form
    {
        #region Tag Descriptor
        public static TagDescriptorMfc tagDescriptor = new TagDescriptorMfc();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTags m_Tags = null;
        //private int m_GasFlowrate = 0;
        private ClientManager m_Client = ClientManager.Instance;
        private int m_TagsCount;

        private const int _Gap = 10;
        private const int _StartPointY = 20;
        #endregion

        #region Constructor
        public DlgMfc()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// It should be called by MFC
        /// </summary>
        /// <param name="MfcControltag"></param>
        public void Initialize(DeviceTag MfcControltag)
        {
            //Initialize Mfc's tags
            m_Tag = MfcControltag;
            m_Tags = m_Client.DataProvider.TagContainer.GetTags(m_Tag.DeviceType);
            m_TagsCount = m_Tags.Count;
            
            if (m_Tags.GetTag(m_Tag.DeviceName) == null)
            {
                MessageBox.Show("Undefined Mfc");
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            Point point = new Point(_Gap, _StartPointY);

            MfcButtonOp[] controls = new MfcButtonOp[m_Tags.Count];

            for (int i = 0; i < m_TagsCount; i++)
            {
                //MfcButtonOp(Horizontal)
                MfcButtonOp MfcOp = new MfcButtonOp(m_Tags, m_Tags[i]);
                MfcOp.MfcClick += new MfcButtonOp.MfcClickEventHandler(MfcOp_MfcClick);
                controls[i] = MfcOp;
            }

            tbPanel.Controls.AddRange(controls);

            if (tbPanel.Controls.Count == 0) tbPanel.Visible = false;
        }

        void MfcOp_MfcClick(DeviceTag tag, MFCAct act, double Flowrate)
        {
            m_Client.SendCommand(Command.MfcManual, tag.DeviceName, act, Flowrate);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}