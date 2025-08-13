///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : jemoon
// Description  : General Server Data UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

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
    public partial class GenInfo : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private TagUnit m_Unit;
        private bool m_AutoHide = false;
        private bool m_AutoHideLogic = true;
        private Timer m_AutoHideTimer = null;
        private TagDescriptor m_RefTagDescriptor = null;
        #endregion

        #region Properties
        [Category("DMS : Tag"),
        Description("Select TagDescriptor")]
        public TagDescriptor ReferenceTagDescriptor
        {
            get { return m_RefTagDescriptor; }
            set { m_RefTagDescriptor = value; }
        }
        [Category("DMS : Basic Info"),
        Description("Select Title Text")]
        public string TitleText
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }
        [Category("DMS : Basic Info"),
        Description("Select using Auto Hide Function or not")]
        public bool AutoHide
        {
            get { return m_AutoHide; }
            set { m_AutoHide = value; }
        }
        [Category("DMS : Basic Info"),
        Description("If Geninfo's value same this value, Auto Hide Function will be executed ")]
        public bool AutoHideLogic
        {
            get { return m_AutoHideLogic; }
            set { m_AutoHideLogic = value; }
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
        Description("Select Title panel text align")]
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
        Description("Select Value text align")]
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
        public GenInfo()
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
        /// <summary>
        /// It should be called by HMI - eun 20080110
        /// </summary>
        /// <param name="tags"></param>
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                //jemoon : 세부 TagDescriptor를 지정한경우
                if (m_RefTagDescriptor != null)
                {
                    if (m_Tag.Items.Count <= m_RefTagDescriptor.Id)
                    {
                        string msg = string.Format("{0} is not member of {1}'s Tag(Id)", m_RefTagDescriptor.Key, this.Name);
                        MessageBox.Show(msg);
                        ok = false;
                    }
                    else if (m_Tag[m_RefTagDescriptor.Id] == null)
                    {
                        string msg = string.Format("{0} is not member of {1}'s Tag", m_RefTagDescriptor.Key, this.Name);
                        MessageBox.Show(msg);
                        ok = false;
                    }
                }

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                if (m_Initialized)
                {
                    if (m_AutoHide)
                    {
                        m_AutoHideTimer = new Timer();
                        m_AutoHideTimer.Interval = 500;
                        m_AutoHideTimer.Tick += new EventHandler(m_AutoHideTimer_Tick);
                        m_AutoHideTimer.Enabled = true;
                    }

                    UpdateState();
                }
            }

            return m_Initialized;
        }

        private void m_AutoHideTimer_Tick(object sender, EventArgs e)
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.VALUE].Value == m_AutoHideLogic.ToString())
            {
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
            }
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;
            
            if (m_RefTagDescriptor != null)
            {   //jemoon : 세부 Degdescriptor가 지정된 경우
                lblValue.Text = m_Tag[m_RefTagDescriptor.Id].Value + " " + m_Unit.Name;
            }
            else
            {
                lblValue.Text = m_Tag[tagDescriptor.VALUE].Value + " " + m_Unit.Name;
            }
        }
        #endregion
    }
}
