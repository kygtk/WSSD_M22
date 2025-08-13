using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class TagButton : Button, IDmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Enum
        public enum Type
        {
            A_TrueCheck, B_FalseCheck
        } 
        #endregion

        #region Fields
        private DeviceTags m_Tags = null;
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private DeviceTagInfo m_TagInfo;
        private Type m_Type = Type.A_TrueCheck;
        private Type m_OldType = Type.A_TrueCheck;
        private Color m_OnColor = Color.LightSteelBlue;
        private Color m_OffColor = Color.Transparent;
        private Command m_Command = Command.Noop;
        private ClientManager m_Client = null;
        private ContentAlignment m_TextAlign = ContentAlignment.BottomCenter;
        private bool m_Initialized = false;
        #endregion

        #region Properties
        [Category("DMS : Tag")]
        public DeviceTagInfo DeviceTagInfo
        {
            get { return m_TagInfo; }
            set { m_TagInfo = value; }
        }
        [Category("DMS : UI")]
        public Type ButtonCheckedType
        {
            get { return m_Type; }
            set 
            { 
                m_Type = value;
                SetDisplay();
            }
        }
        [Category("DMS : UI")]
        public Color ButtonCheckedColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }
        [Category("DMS : UI")]
        public Color ButtonUnCheckedColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        [Category("DMS : UI")]
        public ContentAlignment ButtonTextAlign
        {
            get
            {
                return m_TextAlign;
            }
            set
            {
                base.TextAlign = value;
                m_TextAlign = value;
            }
        }
        [Category("DMS : Basic Info")]
        public Command Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        [Browsable(false)]
        public override ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
                base.TextAlign = value;
            }
        }
        #endregion

        public TagButton()
        {
            this.BackColor = m_OffColor;
            this.Size = new Size(72, 66);
            this.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.Text = "Button";

            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 사용자 지정 그리기 코드를 여기에 추가합니다.

            // 기본 클래스 OnPaint를 호출하고 있습니다.
            base.OnPaint(pe);
        }

        public bool Initialize(DeviceTags tags)
        {
            this.TextAlign = m_TextAlign;

            bool ok = true;

            ok &= (m_TagInfo != null);
            if (m_TagInfo != null)
            {
                ok &= !string.IsNullOrEmpty(m_TagInfo.DeviceName);
            }

            if (!ok)
            { 
                //jemoon : m_TagInfo가 지정되어 있지 않으면 그냥 Command Button으로만 사용한다.
                m_Initialized = true;
            }
            else if (!m_Initialized)
            {
                m_Tags = tags;
                DeviceTag tag = m_Tags[m_TagInfo.DeviceName];

                if (tag == null)
                {
                    string msg = string.Format("Tag of {0} does not exist.", this.Name);
                    MessageBox.Show(msg);
                    ok = false;                    
                }
                else
                {
                    m_Tag = tag;
                    m_TagOld.Clone(m_Tag);
                }

                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        private void SetDisplay()
        {
            if (m_OldType != m_Type)
            {
                Color color = m_OnColor;
                m_OnColor = m_OffColor;
                m_OffColor = color;
                m_OldType = m_Type;
                this.BackColor = m_OffColor;
            }
        }

        private void UpdateState()
        {
            if (m_Initialized)
            {
                if (m_Tag[tagDescriptor.VALUE].Value == bool.TrueString || m_Tag[tagDescriptor.VALUE].Value == "1")
                    this.BackColor = m_OnColor;
                else this.BackColor = m_OffColor;
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }

        private void TagButton_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count == 0 || !m_Initialized) return;
            
            m_Client.SendCommand(m_Command);
        }

        public DeviceTag GetDeviceTag()
        {
            return m_Tag;
        }
    }
}
