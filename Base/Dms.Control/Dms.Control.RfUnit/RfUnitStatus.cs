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
    public partial class RfUnitStatus : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorRfUnit tagDescriptor = new TagDescriptorRfUnit();
        #endregion

        private ClientManager m_Client = null;

        #region Properties
        [Category("DMS : UI")]
        public Color RfUnitStateBackColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfUnitStateTitleBackColor
        {
            get { return lblTunePos.BackColor; }
            set 
            {
                lblTunePosTitle.BackColor = value;
                lblLoadPosTitle.BackColor = value;
                lblForwardPowerTitle.BackColor = value;
                lblReflectedPowerTitle.BackColor = value;              
            }
        }
        //[Category("DMS : UI")]
        //public Color RfTunerStateTitleForeColor
        //{
        //    get { return lblTitle.ForeColor; }
        //    set { lblTitle.ForeColor = value; }
        //}
        //[Category("DMS : UI")]
        //public Color RfTunerStateItemForeColor
        //{
        //    get { return lblItem1.ForeColor; }
        //    set { lblItem1.ForeColor = lblItem2.ForeColor = lblItem3.ForeColor = value; }
        //}
        //[Category("DMS : UI")]
        //public Color RfTunerStateItemBackColor
        //{
        //    get { return lblItem1.BackColor; }
        //    set { lblItem1.BackColor = lblItem2.BackColor = lblItem3.BackColor = value; }
        //}
        #endregion
        
        public RfUnitStatus()
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
            lblTunePos.Text = m_Tag[tagDescriptor.TUNEPOS].Value + " %";
            lblLoadPos.Text = m_Tag[tagDescriptor.LOADPOS].Value + " %";

            lblForwardPower.Text = m_Tag[tagDescriptor.FORWARDPOWER].Value + " W";
            lblReflectedPower.Text = m_Tag[tagDescriptor.REFLECTEDPOWER].Value + " W";

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
