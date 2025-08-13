///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : CvMotor UserControl
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
using Dms.Device;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(CvMotor), "CvMotorIcon")]
    public partial class CvMotor : DmsUserControl, IGlassAni
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Enum
        public enum AnimFrames
        {
            cv1, cv2, cv3, cv4, cv5, cv6, cvAlarm, cvOff
        }
        public enum MotorDirection
        {
            CwIsFw = 1,
            CcwIsFw = -1
        }
        #endregion

        #region Fields
        private MotorDirection m_CurMotorDirection = MotorDirection.CwIsFw;
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private ClientManager m_Client = null;
        private int m_SpeedInterval = 300;
        private bool m_Fw = false;
        private bool m_Bw = false;
        #endregion

        #region Properties
        /// <summary>
        /// CvMotor animation update time interval - eun 20080110
        /// </summary>
        [Category("DMS : UI"),
         Description("CvMotor animation update time interval")]
        public int AnimationUpdateInterval
        {
            get { return tmrUpdateAnimation.Interval; }
            set { tmrUpdateAnimation.Interval = value; }
        }

        /// <summary>
        /// Forward direction - eun 20080110
        /// </summary>
        [Category("DMS : UI"),
         Description("Select forward direction")]
        public MotorDirection CurMotorDirection
        {
            get { return m_CurMotorDirection; }
            set { m_CurMotorDirection = value; }
        }

        ///// <summary>
        ///// Set Maxspeed of CvMotor for dialog - eun 20080116
        ///// </summary>
        //[Category("DMS : UI"),
        // DefaultValue(18000),
        // Description("Set maximum speed of cvmotor for manual operation dialog")]
        //public int MaxSpeed
        //{
        //    get { return m_MaxSpeed; }
        //    set  { m_MaxSpeed = value; }
        //}

        ///// <summary>
        ///// Set Minspeed of CvMotor for dialog - eun 20080116
        ///// </summary>
        //[Category("DMS : UI"),
        // DefaultValue(900),
        // Description("Set minimum speed of cvmotor for manual operation dialog")]
        //public int MinSpeed
        //{
        //    get { return m_MinSpeed; }
        //    set { m_MinSpeed = value; }
        //}

        /// <summary>
        /// Set interval of speed for dialog - eun 20080116
        /// </summary>
        [Category("DMS : UI"),
         DefaultValue(300),
         Description("Set speed interval for manual operation dialog")]
        public int SpeedInterval
        {
            get { return m_SpeedInterval; }
            set { m_SpeedInterval = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter CvMotor contstructor - eun 20080110
        /// </summary>
        public CvMotor()
        {

            InitializeComponent();

            m_Bitmap.Add(Dms.Control.Properties.Resources.cv1);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv2);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv3);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv4);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv5);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv6);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv_alarm);
            m_Bitmap.Add(Dms.Control.Properties.Resources.cv_off);

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update cvmotor animaion - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Eventhandler for cvmotor dialog - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CvMotor_Click(object sender, EventArgs e)
        {
            if (m_Client.GenInfos.AutoMode) return;
            if (m_Client.UserLevel < UserLevels.Technician) return;

            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                DlgCvMotor dlg = new DlgCvMotor();
                //TagRecipe recipe = new TagRecipe();
                //m_Client.DataProvider.RecipeProvider.GetCurrentRecipe(ref recipe);
                //int speed = recipe.ProcessSpeed;

                IJobCondition jobCondition = m_Client.EventSubscriber.IServerManager.JobCond;
                int speed = jobCondition.CvProcessSpeed(0);


                if ((dlg.RecipeSpeed == 0) || (speed != dlg.RecipeSpeed)) dlg.RecipeSpeed = speed;
                if ((dlg.SpeedInterval == 0) || (m_SpeedInterval != dlg.SpeedInterval)) dlg.SpeedInterval = m_SpeedInterval;

                dlg.Initialize(m_Tag);

                if (dlg.DialogResult == DialogResult.Cancel) return;
                else dlg.ShowDialog();
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

        private bool setOff = false;
        private bool setAlarm = false;
        protected override void UpdateState()
        {
            if (!m_Initialized) return;
            string alarm = m_Tag[tagDescriptor.ALARM].Value;
            string cpon = m_Tag[tagDescriptor.CPON].Value;
            string stop = m_Tag[tagDescriptor.STOP].Value;
            string fw = m_Tag[tagDescriptor.FW].Value;
            string bw = m_Tag[tagDescriptor.BW].Value;

            if (alarm == bool.TrueString || alarm == "1")
            {
                axCVSingle1.SetAlarm();
                setAlarm = true;
                m_Fw = m_Bw = false;
            }
            else if (cpon == bool.FalseString || cpon == "0")
            {
                axCVSingle1.SetOff();
                setOff = true;
                m_Fw = m_Bw = false;
            }
            else if (stop == bool.TrueString || stop == "1")
            {
                if (setAlarm)
                {
                    axCVSingle1.ResetAlarm();
                    setAlarm = false;
                }
                if (setOff)
                {
                    axCVSingle1.ResetOff();
                    setOff = false;
                }

                axCVSingle1.SetStop();

                m_Fw = m_Bw = false;
            }
            else if (fw == bool.TrueString || fw == "1")
            {
                if (m_CurMotorDirection == MotorDirection.CwIsFw)
                    axCVSingle1.SetStartCw();
                else
                    axCVSingle1.SetStartCcw();
                m_Fw = true;
            }
            else if (bw == bool.TrueString || bw == "1")
            {
                if (m_CurMotorDirection == MotorDirection.CwIsFw)
                    axCVSingle1.SetStartCcw();
                else
                    axCVSingle1.SetStartCw();
                m_Bw = true;
            }
        }
        #endregion

        #region IGlassAni Members
        public Boolean IsMove()
        {
            return m_Fw || m_Bw;
        }

        public Boolean IsFw()
        {
            return m_Fw;
        }

        public Boolean IsBw()
        {
            return m_Bw;
        }

        #endregion
    }
}