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
    public enum SalienceState
    { 
        None,                   //Salience 상태없음
        Alarm,                  //Alarm
        Caution,                //Warning
        Processing,             //작업중
        UserAttentionRequired   //주의요망
    }

    public partial class ViewSalience : DmsUserControl
    {
        #region Fields
        private Color m_ColorAlarm = Color.LightCoral;
        private Color m_ColorCaution = Color.Yellow;
        private Color m_ColorProcess = Color.SkyBlue;
        private Color m_ColorAttention = Color.Chartreuse;
        private List<Color> m_StateColors = new List<Color>();
        private SalienceState m_CurState = SalienceState.None;
        //private SalienceState m_OldState = SalienceState.None;             
        #endregion

        #region Properties
        [Category("DMS : UI"), Description("Select State BackColor")]
        public Color ColorAlarm
        {
            get { return m_ColorAlarm; }
            set { m_ColorAlarm = value; }
        }
        [Category("DMS : UI"), Description("Select State BackColor")]
        public Color ColorCaution
        {
            get { return m_ColorCaution; }
            set { m_ColorCaution = value; }
        }
        [Category("DMS : UI"), Description("Select State BackColor")]
        public Color ColorProcess
        {
            get { return m_ColorProcess; }
            set { m_ColorProcess = value; }
        }
        [Category("DMS : UI"), Description("Select State BackColor")]
        public Color ColorAttention
        {
            get { return m_ColorAttention; }
            set { m_ColorAttention = value; }
        }
        #endregion

        #region Constructor
        public ViewSalience()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        } 
        #endregion

        #region Methods
        public void SetState(SalienceState state)
        {
            m_CurState = state;
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tagContainer)
        {
            // ViewSalience Tag 관련 암것도 할필요가 없어요

            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            m_StateColors.Add(Color.Transparent);
            m_StateColors.Add(m_ColorAlarm);
            m_StateColors.Add(m_ColorCaution);
            m_StateColors.Add(m_ColorProcess);
            m_StateColors.Add(m_ColorAttention);
            
            // Timer를 설정한다.
            tmrUpdateState = new System.Windows.Forms.Timer();
            tmrUpdateState.Interval = 500;
            tmrUpdateState.Tick += new System.EventHandler(tmrUpdateState_Tick);
            tmrUpdateState.Enabled = true;

            this.Visible = false;

            m_Initialized = true;

            return true;
        }

        private bool m_UpdateFlag = false;

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        protected override void UpdateState()
        {
            if (m_CurState == SalienceState.None)
            {
                this.Visible = false;
            }
            else
            {
                m_UpdateFlag = !m_UpdateFlag;
                this.Visible = m_UpdateFlag;
                this.labelState.BackColor = m_StateColors[(int)m_CurState];
            }
        }
        #endregion
    }
}
