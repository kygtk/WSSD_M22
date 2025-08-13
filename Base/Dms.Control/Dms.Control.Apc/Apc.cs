using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class Apc : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorApc tagDescriptor = new TagDescriptorApc();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        #endregion Fields

        #region Properties
        [Category("DMS : UI")]
        public Color ApcBackColor
        {
            get { return lblBackColor.BackColor; }
            set { lblBackColor.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color ApcForeColor
        {
            get { return lblState.BackColor; }
            set { lblState.BackColor = value; }
        }
        #endregion
        
        #region Constructor
        public Apc()
        {
            InitializeComponent();
        }
        #endregion
        
        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                if (m_Tag[tagDescriptor.VALUE] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.VALUE].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }

                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.VALUE].Value == "OPEN")
            {
                //pbImage.Image = Properties.Resources.APC_Open;
                lblState.BackColor = Color.Yellow;
                lblState.Text = "OPEN";
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "CLOSE")
            {
                //pbImage.Image = Properties.Resources.APC_Close;
                lblState.BackColor = Color.DimGray;
                lblState.Text = "CLOSE";
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_A")
            {
                //pbImage.Image = Properties.Resources.APC_A;
                lblState.BackColor = Color.Gold;
                lblState.Text = "Point A";
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_B")
            {
                //pbImage.Image = Properties.Resources.APC_B;
                lblState.BackColor = Color.Gold;
                lblState.Text = "Point B";
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_C")
            {
                //pbImage.Image = Properties.Resources.APC_C;
                lblState.BackColor = Color.Gold;
                lblState.Text = "Point C";
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_D")
            {
                // pbImage.Image = Properties.Resources.APC_D;
                lblState.BackColor = Color.Gold;
                lblState.Text = "Point D";
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_E")
            {
                // pbImage.Image = Properties.Resources.APC_E;
                lblState.BackColor = Color.Gold;
                lblState.Text = "Point E";
            }
            //if (m_Tag[tagDescriptor.RUN].Value == bool.TrueString || m_Tag[tagDescriptor.RUN].Value == "1")
            //{
            //    axAirKnife1.SetRun();
            //}
            //else
            //{
            //    axAirKnife1.SetStop();
            //}
        }
        #endregion

        private void ApcClick(object sender, EventArgs e)
        {
            
            if (m_Tag == null) return;           
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;
                DlgApc dlg = new DlgApc(m_Tag);
                dlg.ShowDialog();
            }
            else MessageBox.Show("Tag is not selected.");
        }
    }
}
