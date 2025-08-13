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
    public partial class RfUnit : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorRfUnit tagDescriptorRfUnit = new TagDescriptorRfUnit();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
       
        protected Color m_RfOnColor;
        protected Color m_RfOffColor;
        #endregion

        #region Properties
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
        public RfUnit()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);           
        }
        #endregion

        private void lblMatchBox_Click(object sender, EventArgs e)
        {
            DlgRfUnit dlg = new DlgRfUnit(m_Tag);

            dlg.ShowDialog();
        }


        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            //DeviceTag tag = m_Tags[m_TagRfTunerInfo.DeviceName];

            //if (tag == null)
            //{
            //    string msg = string.Format("Tag of {0} does not exist.", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //else // 나의 Tag가 존재한다면 초기화
            //{
            //    m_TagRfTuner = tag;
            //    m_TagRfTunerOld.Clone(m_TagRfTuner);
            //}

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
            if (m_Tag[tagDescriptorRfUnit.ONSTATE].Value == bool.TrueString || m_Tag[tagDescriptorRfUnit.ONSTATE].Value == "1")
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
