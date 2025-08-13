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
    public partial class RefTagDescriptor : DmsUserControl
    {
        #region Fields
        private TagDescriptor m_TagDescriptor;
        private TagUnit m_Unit;
        #endregion

        #region Properties
        [Category("DMS : Tag"),
        Description("Select TagDescriptor")]
        public TagDescriptor ReferenceTagDescriptor
        {
            get { return m_TagDescriptor; }
            set { m_TagDescriptor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title Text")]
        public string TitleText
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title BackColor")]
        public Color TitleBackColor
        {
            get { return this.lblTitle.BackColor; }
            set { this.lblTitle.BackColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title ForeColor")]
        public Color TitleForeColor
        {
            get { return this.lblTitle.ForeColor; }
            set { this.lblTitle.ForeColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title Text Font")]
        public Font TitleTextFont
        {
            get { return this.lblTitle.Font; }
            set { this.lblTitle.Font = value; }
        }
        [Category("DMS : UI"),
        Description("Adjust Title panel size")]
        public int TitlePanelSize
        {
            get { return this.splitContainer1.SplitterDistance; }
            set { this.splitContainer1.SplitterDistance = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title panel Border Style")]
        public BorderStyle TitlePanelBorderStyle
        {
            get { return lblTitle.BorderStyle; }
            set { lblTitle.BorderStyle = value; }
        }
        [Category("DMS : UI"),
        Description("Select alignment of Title panel's text")]
        public ContentAlignment TitlePanelTextAlign
        {
            get { return lblTitle.TextAlign; }
            set { lblTitle.TextAlign = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value BackColor")]
        public Color ValueBackColor
        {
            get { return this.lblValue.BackColor; }
            set { this.lblValue.BackColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value ForeColor")]
        public Color ValueForeColor
        {
            get { return this.lblValue.ForeColor; }
            set { this.lblValue.ForeColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value Text Font")]
        public Font ValueTextFont
        {
            get { return this.lblValue.Font; }
            set { this.lblValue.Font = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value Text Unit")]
        public UnitType ValueTextUnit
        {
            get { return m_Unit.Unit; }
            set { m_Unit.Unit = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value Border Style")]
        public BorderStyle ValueBorderStyle
        {
            get { return lblValue.BorderStyle; }
            set { lblValue.BorderStyle = value; }
        }
        [Category("DMS : UI"),
        Description("Select alignment of value panel's text")]
        public ContentAlignment ValueTextAlign
        {
            get { return lblValue.TextAlign; }
            set { lblValue.TextAlign = value; }
        }
        [Category("DMS : UI"),
        Description("Adjust distance between Title panel and Value")]
        public int Distance
        {
            get { return splitContainer1.SplitterWidth; }
            set { splitContainer1.SplitterWidth = value; }
        }
        #endregion

        #region Events
        [Category("DMS : EVENT"), Description("Click Event")]
        public event EventHandler ValueClick;
        #endregion

        #region Constructor
        public RefTagDescriptor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void lblValue_Click(object sender, EventArgs e)
        {
            if (ValueClick != null)
            {
                ValueClick(sender, e);
            }
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                if (m_TagDescriptor == null)
                {
                    string msg = string.Format("TagDescriptor of {0} is not created.", this.Name);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag.Items.Count <= m_TagDescriptor.Id)
                {
                    string msg = string.Format("{0} is not member of {1}'s Tag(Id)", m_TagDescriptor.Key, this.Name);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[m_TagDescriptor.Id] == null)
                {
                    string msg = string.Format("{0} is not member of {1}'s Tag", m_TagDescriptor.Key, this.Name);
                    MessageBox.Show(msg);
                    ok = false;
                }

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            lblValue.Text = m_Tag[m_TagDescriptor.Id].Value + " " + m_Unit.Name;
        }
        #endregion
    }
}
