///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.11
// Author       : eun
// Description  : Cylinder UserControl
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
    [ToolboxBitmap(typeof(Cylinder), "CylinderAni.bmp")]
    public partial class Cylinder : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion

        #region Enum
        private enum State
        {
            FW, BW, NONE
        }

        public enum Types
        {
            Align_Upper, Align_Lower, Align_Left, Align_Right, Simple_Upper, Simple_Lower, Simple_Left, Simple_Right, Chuck_Left, Chuck_Right,Chuck_Up_Down, Lift_Up_Down,
            Align_Up_OpenClose, Align_Lo_OpenClose, Align_Left_OpenClose, Align_Right_OpenClose
        }

        public enum DirectionType
        {
            FW_BW, UP_DOWN, LEFT_RIGHT, OPEN_CLOSE
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Types m_Type = Types.Align_Upper;
        private DirectionType m_DirType = DirectionType.UP_DOWN;
        private bool m_DisplayOnly = false;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of Cylinder  - eun 20080211
        /// </summary>
        [Category("DMS : UI"),
         Description("Select type of Cylinder")]
        public Types CylinderType
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                SetDisplay();
            }
        }
        [Category("DMS : UI"),
        Description("Set true if you want to only display the state of actuator")]
        public bool DisplayOnly
        {
            get { return m_DisplayOnly; }
            set { m_DisplayOnly = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Cylinder contstructor - eun 20080111
        /// </summary>
        public Cylinder()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Set Images - eun 20080211
        /// </summary>
        private void SetDisplay()
        {
            m_Bitmap.Clear();
            if (m_Type == Types.Align_Upper)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_up_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_low_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Align_Lower)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_lo_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_up_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Align_Left)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_left_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_right_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Align_Right)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_right_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_left_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Simple_Upper)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_up_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_down_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_up_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Simple_Lower)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_down_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_up_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_down_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Simple_Left)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_left_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_right_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_left_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Simple_Right)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_right_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_left_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_simple_right_none);
                m_DirType = DirectionType.FW_BW;
            }
            else if (m_Type == Types.Chuck_Left)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_left_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_left_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_none);
                m_DirType = DirectionType.LEFT_RIGHT;
            }
            else if (m_Type == Types.Chuck_Right)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_right_fw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_right_bw);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_none);
                m_DirType = DirectionType.LEFT_RIGHT;
            }
            else if (m_Type == Types.Chuck_Up_Down)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_up);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_dn);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_chuck_none);
                m_DirType = DirectionType.UP_DOWN;
            }
            else if (m_Type == Types.Lift_Up_Down)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_lift_up);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_lift_down);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_lift_none);
                m_DirType = DirectionType.UP_DOWN;
            }
            else if (m_Type == Types.Align_Left_OpenClose)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_left_c);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_right_o);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.OPEN_CLOSE;
            }
            else if (m_Type == Types.Align_Lo_OpenClose)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_dn_c);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_up_o);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.OPEN_CLOSE;
            }
            else if (m_Type == Types.Align_Right_OpenClose)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_right_c);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_left_o);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.OPEN_CLOSE;
            }
            else if (m_Type == Types.Align_Up_OpenClose)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_up_c);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_dn_o);
                m_Bitmap.Add(Dms.Control.Properties.Resources.cyl_none);
                m_DirType = DirectionType.OPEN_CLOSE;
            }
            pbImage.Image = m_Bitmap[(int)State.FW];
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            if (m_Tag == null || m_Tag[tagDescriptor.ACT_STATUS] == null) return;

            if (m_Tag.Items.Count != 0)
            {
                if (m_DisplayOnly == false)
                {
                    CylinderOperateForm dlg = new CylinderOperateForm(m_Tag, m_DirType);
                    dlg.ShowDialog();
                    //jemoon : 110607
                    //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                    dlg.Dispose();
                }
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

            State state = State.NONE;

            ActuatorAct act = (ActuatorAct)Enum.Parse(typeof(ActuatorAct), m_Tag[tagDescriptor.ACT_STATUS].Value, true);

            switch (act)
            {
                case ActuatorAct.Pos:
                    state = State.FW;
                    break;
                case ActuatorAct.Neg:
                    state = State.BW;
                    break;
                default:
                    state = State.NONE;
                    break;

            }

            pbImage.Image = m_Bitmap[(int)state];
        }
        #endregion
    }
}
