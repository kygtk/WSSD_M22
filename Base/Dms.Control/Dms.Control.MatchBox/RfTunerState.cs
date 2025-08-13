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
    public partial class RfTunerState : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorRfTuner tagDescriptor = new TagDescriptorRfTuner();
        #endregion

        private ClientManager m_Client = null;

        #region Properties
        #region Properties
        [Category("DMS : UI")]
        public Color RfTunerStateBackColor
        {
            get { return lblBackColor.BackColor; }
            set { lblBackColor.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfTunerStateTitleBackColor
        {
            get { return lblTitle.BackColor; }
            set { lblTitle.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfTunerStateTitleForeColor
        {
            get { return lblTitle.ForeColor; }
            set { lblTitle.ForeColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfTunerStateItemForeColor
        {
            get { return lblItem1.ForeColor; }
            set { lblItem1.ForeColor = lblItem2.ForeColor = lblItem3.ForeColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfTunerStateItemBackColor
        {
            get { return lblItem1.BackColor; }
            set { lblItem1.BackColor = lblItem2.BackColor = lblItem3.BackColor = value; }
        }
        #endregion
        #endregion

        public RfTunerState()
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
            //if (m_Tag[tagDescriptor.POWERSENSED].Value == true.ToString() || (m_Tag[tagDescriptor.POWERSENSED].Value == "1"))
            //{
            //    pbPowerSensed.BackColor = Color.Green;
            //}
            //else
            //{
            //    pbPowerSensed.BackColor = Color.White;
            //}

            //if (m_Tag[tagDescriptor.POWERTUNED].Value == true.ToString() || (m_Tag[tagDescriptor.POWERTUNED].Value == "1"))
            //{
            //    pbPowerTuned.BackColor = Color.Green;
            //}
            //else
            //{
            //    pbPowerTuned.BackColor = Color.White;
            //}

            //if (m_Tag[tagDescriptor.TUNERFAULT].Value == true.ToString() || (m_Tag[tagDescriptor.TUNERFAULT].Value == "1"))
            //{
            //    pbTunerFault.BackColor = Color.Red;
            //}
            //else
            //{
            //    pbTunerFault.BackColor = Color.White;
            //}
        }
        #endregion
    }
}
