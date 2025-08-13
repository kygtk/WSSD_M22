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
    public partial class MatchBox : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorRfg tagDescriptorRfg = new TagDescriptorRfg();
        TagDescriptorRfTuner tagDescriptorRfTuner = new TagDescriptorRfTuner();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        protected DeviceTag m_TagRfTuner = null;
        protected DeviceTag m_TagRfTunerOld = new DeviceTag();
        protected DeviceTagInfo m_TagRfTunerInfo = null;

        protected Color m_RfOnColor;
        protected Color m_RfOffColor;
        #endregion

        #region Properties
        [Category("DMS : Tag")]
        public DeviceTagInfo DeviceTagRfTunerInfo
        {
            get { return m_TagRfTunerInfo; }
            set { m_TagRfTunerInfo = value; }
        }
        [Category("DMS : UI")]
        public Color MatchBoxBackColor
        {
            get { return lblBackColor.BackColor; }
            set { lblBackColor.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color MatchBoxColor
        {
            get { return lblMatchBox.BackColor; }
            set { lblMatchBox.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfOnColor
        {
            get { return m_RfOnColor; }
            set { m_RfOnColor = value; }
        }
        [Category("DMS : UI")]
        public Color RfOffColor
        {
            get { return m_RfOffColor; }
            set { m_RfOffColor = value; }
        }
        #endregion

        #region Constructor
        public MatchBox()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagRfTunerInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        private void lblMatchBox_Click(object sender, EventArgs e)
        {
            DlgMatchBox dlg = new DlgMatchBox(m_Tag, m_TagRfTuner);

            dlg.ShowDialog();
        }


        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            DeviceTag tag = m_Tags[m_TagRfTunerInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else // 나의 Tag가 존재한다면 초기화
            {
                m_TagRfTuner = tag;
                m_TagRfTunerOld.Clone(m_TagRfTuner);
            }

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
            if (m_Tag[tagDescriptorRfg.ONSTATE].Value == bool.TrueString || m_Tag[tagDescriptorRfg.ONSTATE].Value == "1")
            {
                lblMatchBox.BackColor = m_RfOnColor;
            }
            else
            {
                lblMatchBox.BackColor = m_RfOffColor;
            }
        }
        
        #endregion
               

       

        
    }
}
