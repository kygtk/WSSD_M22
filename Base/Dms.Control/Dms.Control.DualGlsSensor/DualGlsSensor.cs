///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05.12
// Author       : eun
// Description  : DualGlsSensor UserControl
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
using Dms.Client;

namespace Dms.Control
{
    public partial class DualGlsSensor : DmsUserControl
    {
        #region Tag Descriptor
        protected static TagDescriptorDualGlsSensor tagDescriptor = new TagDescriptorDualGlsSensor();
        #endregion

        #region Fields
        private Color m_OnColor = Color.Lime;
        private Color m_SickOnColor = Color.IndianRed;//2010.07.26 kimgun
        private Color m_SickColor = Color.HotPink;
        private Color m_OffColor = Color.White;
        private Color m_NoUseColor = Color.Red;
        private ClientManager m_Client;
        #endregion

        #region Properties
        [Category("DMS : UI"),
         Description("Select sensor on color")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }
        [Category("DMS : UI"),
         Description("Select sensor off color")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        [Category("DMS : UI"),
         Description("Select sensor sick color")]
        public Color SickColor
        {
            get { return m_SickColor; }
            set { m_SickColor = value; }
        }
        [Category("DMS : UI"),
         Description("Select sensor sick On color")]
        public Color SickOnColor
        {//2010.07.26 kimgun
            get { return m_SickOnColor; }
            set { m_SickOnColor = value; }
        }
        [Category("DMS : UI"),
         Description("Select sensor no use color")]
        public Color NoUseColor
        {
            get { return m_NoUseColor; }
            set { m_NoUseColor = value; }
        }
        #endregion

        #region Constructor
        public DualGlsSensor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void DualGlsSensor_Click(object sender, EventArgs e)
        {
            if (m_Initialized == false) return;
            if (m_Client.GenInfos.AutoMode) return;

            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                DlgDualGlsSensor dlg = new DlgDualGlsSensor();
                dlg.Initialize(m_Tag);
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
                dlg.Dispose();
            }
            else
            {
                MessageBox.Show("Tag is not selected");
            }
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tagContainer)
        {
            this.splitContainer1.SplitterDistance = this.Height / 2;
            this.splitContainer1.IsSplitterFixed = true;

            bool ok = base.Initialize(tagContainer);

            if (ok)
            {
                DeviceTag tag = m_Tags[m_TagInfo.DeviceName];
                if (tag == null)
                {
                    string msg = string.Format("Tag of {0} does not exist.", this.Name);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.DETECT] == null || tag[tagDescriptor.DETECT] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.DETECT].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.SICK] == null || tag[tagDescriptor.SICK] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.SICK].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.DETECTOP] == null || tag[tagDescriptor.DETECTOP] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.DETECTOP].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.USE] == null || tag[tagDescriptor.USE] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.USE].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.USEOP] == null || tag[tagDescriptor.USEOP] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.USEOP].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.SICKOP] == null || tag[tagDescriptor.SICKOP] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.SICKOP].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else
                {
                    m_Tag = tag;
                    m_TagOld.Clone(m_Tag);
                }

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();

                if (m_Client == null) m_Client = ClientManager.Instance;
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.SICK].Value == bool.TrueString || m_Tag[tagDescriptor.SICK].Value == "1")
            {
                if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
                {//2010.07.26 kimgun
                    splitContainer1.Panel1.BackColor = m_SickOnColor;
                }
                else
                {
                    splitContainer1.Panel1.BackColor = m_SickColor;
                }
            }
            else if (m_Tag[tagDescriptor.USE].Value == bool.FalseString || m_Tag[tagDescriptor.USE].Value == "0")
            {
                if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
                {//2010.07.26 kimgun
                    splitContainer1.Panel1.BackColor = m_SickOnColor;
                }
                else
                {
                    splitContainer1.Panel1.BackColor = m_NoUseColor;
                }
            }
            else if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
            {
                splitContainer1.Panel1.BackColor = m_OnColor;
            }
            else
            {
                splitContainer1.Panel1.BackColor = m_OffColor;
            }

            if (m_Tag[tagDescriptor.SICKOP].Value == bool.TrueString || m_Tag[tagDescriptor.SICKOP].Value == "1")
            {
                if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
                {//2010.07.26 kimgun
                    splitContainer1.Panel2.BackColor = m_SickOnColor;
                }
                else
                {
                    splitContainer1.Panel2.BackColor = m_SickColor;
                }
            }
            else if (m_Tag[tagDescriptor.USEOP].Value == bool.FalseString || m_Tag[tagDescriptor.USEOP].Value == "0")
            {
                if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
                {//2010.07.26 kimgun
                    splitContainer1.Panel2.BackColor = m_SickOnColor;
                }
                else
                {
                    splitContainer1.Panel2.BackColor = m_NoUseColor;
                }
            }
            else if (m_Tag[tagDescriptor.DETECTOP].Value == bool.TrueString || m_Tag[tagDescriptor.DETECTOP].Value == "1")
            {
                splitContainer1.Panel2.BackColor = m_OnColor;
            }
            else
            {
                splitContainer1.Panel2.BackColor = m_OffColor;
            }
        }
        #endregion

        
    }
}
