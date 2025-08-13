///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.13
// Author       : jemoon
// Description  : Actuator UserControl
//-------------------------------------------------------------------------
// Revison History
// 

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
    public partial class Actuator : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion
        
        #region Enum
        private enum State
        {
            Positive, 
            Negative, 
            Unkown
        }

        public enum ViewrTypes
        {
            Align_Up,
            Align_Lo,
            Align_L,
            Align_R, 
            Simple_Up,
            Simple_Lo,
            Simple_L,
            Simple_R,
            Chuck_L,
            Chuck_R,
            Lift1,
            Lift2,
            Align_Up_OpenClose,
            Align_Lo_OpenClose,
            Align_L_OpenClose,
            Align_R_OpenClose,
            UserDefinedText
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private ViewrTypes m_Type = ViewrTypes.Align_L;
        private ActuatorType m_ActuatorType = ActuatorType.Align;
        private bool m_DisplayOnly = false;
        private string m_PositiveText = "";
        private string m_NegativeText = "";
        private string m_UnknownText = "Unknown";
        private Color m_PositiveBackColor = Color.White;
        private Color m_PositiveForeColor = Color.Blue;
        private Color m_NegativeBackColor = Color.White;
        private Color m_NegativeForeColor = Color.Brown;
        private Color m_UnknownBackColor = Color.White;
        private Color m_UnknownForeColor = Color.Red;
        private Font m_TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of Actuator - eun 20080111
        /// </summary>
        [Category("DMS : UI"),
         Description("Select type of Actuator")]
        public ViewrTypes ViewerType
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                SetDisplay();
            }
        }
        [Category("DMS : UI")]
        public ActuatorType ActuatorType
        {
            get { return m_ActuatorType; }
        }
        [Category("DMS : UI"),
        Description("Set true if you want to only display the state of actuator")]
        public bool DisplayOnly
        {
            get { return m_DisplayOnly; }
            set { m_DisplayOnly = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public string TextPositive
        {
            get { return m_PositiveText; }
            set { m_PositiveText = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public string TextNegative
        {
            get { return m_NegativeText; }
            set { m_NegativeText = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public string TextUnknown
        {
            get { return m_UnknownText; }
            set { m_UnknownText = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Color PositiveBackColor
        {
            get { return m_PositiveBackColor; }
            set { m_PositiveBackColor = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Color PositiveForeColor
        {
            get { return m_PositiveForeColor; }
            set { m_PositiveForeColor = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Color NegativeBackColor
        {
            get { return m_NegativeBackColor; }
            set { m_NegativeBackColor = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Color NegativeForeColor
        {
            get { return m_NegativeForeColor; }
            set { m_NegativeForeColor = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Color UnknownBackColor
        {
            get { return m_UnknownBackColor; }
            set { m_UnknownBackColor = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Color UnknownForeColor
        {
            get { return m_UnknownForeColor; }
            set { m_UnknownForeColor = value; }
        }
        [Category("DMS : UI - UserDefinedText Type Only")]
        public Font TextFont
        {
            get { return m_TextFont; }
            set { m_TextFont = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Actuator contstructor - eun 20080111
        /// </summary>
        public Actuator()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);

            SetDisplay();
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Set Images - eun 20080211
        /// </summary>
        private void SetDisplay()
        {
            m_Bitmap.Clear();

            if (m_Type != ViewrTypes.UserDefinedText)
            {
                this.pbImage.Visible = true;
                this.labelText.Visible = false;
            }

            switch (m_Type)
            { 
                case ViewrTypes.Align_L:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_left_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_left_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.Align;              
                    break;
                case ViewrTypes.Align_R:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_right_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_right_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Align_Up:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_up_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_up_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Align_Lo:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_lo_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_lo_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Chuck_L:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.chuck_left_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.chuck_left_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.chuck_none);
                    m_ActuatorType = ActuatorType.Chuck;
                    break;
                case ViewrTypes.Chuck_R:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.chuck_right_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.chuck_right_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.chuck_none);
                    m_ActuatorType = ActuatorType.Chuck;
                    break;
                case ViewrTypes.Simple_L:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_left_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_left_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_left_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Simple_R:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_right_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_right_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_right_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Simple_Up:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_up_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_up_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_up_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Simple_Lo:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_lo_fw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_lo_bw);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.simple_lo_none);
                    m_ActuatorType = ActuatorType.Align;
                    break;
                case ViewrTypes.Lift1:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.lift_up1);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.lift_down1);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.lift_none);
                    m_ActuatorType = ActuatorType.Lift;
                    break;
                case ViewrTypes.Lift2:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.lift_up2);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.lift_down2);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.lift_none);
                    m_ActuatorType = ActuatorType.Lift;
                    break;   
                case ViewrTypes.Align_L_OpenClose:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_left_c);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_right_o);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.OpenClose;
                    break;
                case ViewrTypes.Align_Lo_OpenClose:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_dn_c);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_up_o);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.OpenClose;
                    break;
                case ViewrTypes.Align_R_OpenClose:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_right_c);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_left_o);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.OpenClose;
                    break;
                case ViewrTypes.Align_Up_OpenClose:
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_up_c);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_dn_o);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.align_none);
                    m_ActuatorType = ActuatorType.OpenClose;
                    break;
                case ViewrTypes.UserDefinedText:
                    this.pbImage.Visible = false;
                    this.labelText.Visible = true;
                    this.labelText.Text = m_UnknownText;
                    this.labelText.BackColor = m_UnknownBackColor;
                    this.labelText.ForeColor = m_UnknownForeColor;
                    this.labelText.Font = m_TextFont;
                    m_ActuatorType = ActuatorType.UserDefinedText;
                    break;
            }

            if (m_Type != ViewrTypes.UserDefinedText)
            {
                pbImage.Image = m_Bitmap[(int)State.Positive];
            }
        }

        /// <summary>
        /// Update Actuator status - eun 20080211
        /// </summary>
        private void pbImage_Click(object sender, EventArgs e)
        {
            if (m_Tag == null || m_Tag[tagDescriptor.ACT_STATUS] == null) return;

            if (m_Tag.Items.Count != 0)
            {
                if (m_DisplayOnly == false)
                {
                    DlgActuatorOperate dlg = new DlgActuatorOperate(m_Tag, m_ActuatorType);
                    dlg.ShowDialog();
                    //jemoon : 110607
                    //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                    dlg.Dispose();
                }
            }
        }
        private void labelText_Click(object sender, EventArgs e)
        {
            if (m_Tag == null || m_Tag[tagDescriptor.ACT_STATUS] == null) return;

            if (m_Tag.Items.Count != 0)
            {
                if (m_DisplayOnly == false)
                {
                    DlgActuatorOperate dlg = new DlgActuatorOperate(m_Tag, m_ActuatorType, this);
                    dlg.ShowDialog();
                    //jemoon : 110607
                    //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                    dlg.Dispose();
                }
            }
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if(ok)
            {
                if (m_Tag[tagDescriptor.ACT_STATUS] == null)
                {
                    string msg = string.Format("Tag of {0}'s key does not exist.", this.Name);
                    MessageBox.Show(msg);
                    ok = false;
                }

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                SetDisplay();
                
                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            State state = State.Unkown;

            ActuatorAct act = (ActuatorAct)Enum.Parse(typeof(ActuatorAct), m_Tag[tagDescriptor.ACT_STATUS].Value, true);

            if (m_Type != ViewrTypes.UserDefinedText)
            {
                switch (act)
                {
                    case ActuatorAct.Pos:
                        state = State.Positive;
                        break;
                    case ActuatorAct.Neg:
                        state = State.Negative;
                        break;
                    default:
                        state = State.Unkown;
                        break;

                }

                pbImage.Image = m_Bitmap[(int)state];
            }
            else
            { 
                switch (act)
                {
                    case ActuatorAct.Pos:
                        this.labelText.Text = m_PositiveText;
                        this.labelText.BackColor = m_PositiveBackColor;
                        this.labelText.ForeColor = m_PositiveForeColor;
                        break;
                    case ActuatorAct.Neg:
                        this.labelText.Text = m_NegativeText;
                        this.labelText.BackColor = m_NegativeBackColor;
                        this.labelText.ForeColor = m_NegativeForeColor;
                        break;
                    default:
                        this.labelText.Text = m_UnknownText;
                        this.labelText.BackColor = m_UnknownBackColor;
                        this.labelText.ForeColor = m_UnknownForeColor;
                        break;
                }
            }

        }
        #endregion
    }
}
