///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : RbMotor UserControl
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
using Dms.Client;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(RbMotor), "RbMotorAni.bmp")]
    public partial class RbMotor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Enum
        public enum AnimFrames
        {
            rb1, rb2, rb3, rb4, rb5, rb6, rb7, rb8, rb9, rbAlarm, rbOff
        }
        #endregion

        #region Fields
        private Bitmap[] m_Bitmap = new Bitmap[11];
        private ClientManager m_Client = null;
        #endregion

        //#region Delegates
        //public delegate void RbMotorEventHandler();

        //[Category("DMS : Event"),
        //Description("Handling RbMotor click event")]
        //public event RbMotorEventHandler RollBrushClick;
        //#endregion

        #region Properties
        /// <summary>
        /// RbMotor animation update time interval - eun 20080110
        /// </summary>
        [Category("DMS : UI"),
        Description("RbMotor animation update time interval")]
        public int AnimationUpdateInterval
        {
            get { return tmrUpdateAnimation.Interval; }
            set { tmrUpdateAnimation.Interval = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter RbMotor constructor
        /// </summary>
        public RbMotor()
        {
            InitializeComponent();

            m_Bitmap[0] = Dms.Control.Properties.Resources.RB1;
            m_Bitmap[1] = Dms.Control.Properties.Resources.RB2;
            m_Bitmap[2] = Dms.Control.Properties.Resources.RB3;
            m_Bitmap[3] = Dms.Control.Properties.Resources.RB4;
            m_Bitmap[4] = Dms.Control.Properties.Resources.RB5;
            m_Bitmap[5] = Dms.Control.Properties.Resources.RB6;
            m_Bitmap[6] = Dms.Control.Properties.Resources.RB7;
            m_Bitmap[7] = Dms.Control.Properties.Resources.RB8;
            m_Bitmap[8] = Dms.Control.Properties.Resources.RB9;
            m_Bitmap[9] = Dms.Control.Properties.Resources.RBALARM;
            m_Bitmap[10] = Dms.Control.Properties.Resources.RBOFF;

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update Rbmotor animaion - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Show RbMotor dialog - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbMotor_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;

                DlgRbMotor dlg = new DlgRbMotor();
                dlg.Initialize(m_Tag);
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog甫 荤侩窍咯 汽阑 钎矫茄 版快, Dispose甫 龋免窍咯 汽狼 葛电 牧飘费阑 啊厚瘤 荐笼贸府.	 
                dlg.Dispose();
            }
            else MessageBox.Show("Tag is not selected.");
        }
        #endregion

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
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.ALARM].Value == "1")
            {
                axRollBrush1.SetStop();
            }
            else if (m_Tag[tagDescriptor.CPON].Value == bool.FalseString || m_Tag[tagDescriptor.CPON].Value == "0")
            {
                axRollBrush1.SetStop();
            }
            else if (m_Tag[tagDescriptor.STOP].Value == bool.TrueString || m_Tag[tagDescriptor.STOP].Value == "1")
            {
                axRollBrush1.SetStop();
            }
            else if ((m_Tag[tagDescriptor.CW].Value == bool.TrueString || m_Tag[tagDescriptor.CW].Value == "1"))// ||
            {
                axRollBrush1.SetCW();
                axRollBrush1.SetRun();
            }
            else if ((m_Tag[tagDescriptor.CCW].Value == bool.TrueString || m_Tag[tagDescriptor.CCW].Value == "1"))
            {
                axRollBrush1.SetCCW();
                axRollBrush1.SetRun();
            }
        }
        #endregion
    }
}