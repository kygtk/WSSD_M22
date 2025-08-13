using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;

namespace Dms.Control
{
    public partial class RfState : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorRfg tagDescriptor = new TagDescriptorRfg();
        #endregion

        private ClientManager m_Client = null;
        
        #region Properties
        [Category("DMS : UI")]
        public Color RfStateBackColor
        {
            get { return lblBackColor.BackColor; }
            set { lblBackColor.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfStateTitleBackColor
        {
            get { return lblTitle.BackColor; }
            set { lblTitle.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfStateTitleForeColor
        {
            get { return lblTitle.ForeColor; }
            set { lblTitle.ForeColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfStateItemForeColor
        {
            get { return lblItem1.ForeColor; }
            set { lblItem1.ForeColor = lblItem2.ForeColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfStateItemBackColor
        {
            get { return lblItem1.BackColor; }
            set { lblItem1.BackColor = lblItem2.BackColor = value; }
        }
        #endregion

        public RfState()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }

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
            if (m_Tag[tagDescriptor.ONSTATE].Value == true.ToString() || (m_Tag[tagDescriptor.ONSTATE].Value == "1"))
            {
                pbOnState.BackColor = Color.Lime;
            }
            else
            {
                pbOnState.BackColor = Color.White;
            }

            if (m_Tag[tagDescriptor.OVERTEMP].Value == true.ToString() || (m_Tag[tagDescriptor.OVERTEMP].Value == "1"))
            {
                pbOverTemp.BackColor = Color.Red;
            }
            else
            {
                pbOverTemp.BackColor = Color.White;
            }
        }
        #endregion
}
}
