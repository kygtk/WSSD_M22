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
    #region Enum
    public enum heaterStatus
    {
        NoUse,
        NotReady,
        Ready,
        Heating,
        Alarm,
        Off
    }
    #endregion

    public partial class HeaterStatus : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorHeaterUnit tagDescriptor = new TagDescriptorHeaterUnit();
        #endregion

        [Category("DMS : UI"),
        Description("Select Data BackColor")]
        public Color DataBackColor
        {
            get { return this.lblStatus.BackColor; }
            set { this.lblStatus.BackColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Status ForeColor")]
        public Color DataForeColor
        {
            get { return this.lblStatus.ForeColor; }
            set { this.lblStatus.ForeColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Data Text Font")]
        public Font DataTextFont
        {
            get { return this.lblStatus.Font; }
            set { this.lblStatus.Font = value; }
        }
        
        [Category("DMS : UI"),
        Description("Select Data Border Style")]
        public BorderStyle DataBorderStyle
        {
            get { return lblStatus.BorderStyle; }
            set { lblStatus.BorderStyle = value; }
        }
        [Category("DMS : UI")]
        public bool StatusVisible
        {
            get { return lblStatus.Visible;}
            set 
            {
                lblStatus.Visible = value;
            }
        }

        public HeaterStatus()
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
                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            lblStatus.Text = m_Tag[tagDescriptor.STATUS].Value;

            if (lblStatus.Text == heaterStatus.Off.ToString()) lblStatus.BackColor = Color.DarkGray;
            if (lblStatus.Text == heaterStatus.NoUse.ToString()) lblStatus.BackColor = Color.DarkGray;
            if (lblStatus.Text == heaterStatus.Alarm.ToString()) lblStatus.BackColor = Color.Red;
            if (lblStatus.Text == heaterStatus.NotReady.ToString()) lblStatus.BackColor = Color.Gray;
            if (lblStatus.Text == heaterStatus.Heating.ToString()) lblStatus.BackColor = Color.Yellow;
            if (lblStatus.Text == heaterStatus.Ready.ToString()) lblStatus.BackColor = Color.GreenYellow;
        }
        #endregion

        private void lblStatus_Click(object sender, EventArgs e)
        {
            DlgHeater dlgHeater = new DlgHeater(m_Tag);

            dlgHeater.ShowDialog();

            //jemoon : 110607
            //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
            dlgHeater.Dispose();

        }
    }
}
