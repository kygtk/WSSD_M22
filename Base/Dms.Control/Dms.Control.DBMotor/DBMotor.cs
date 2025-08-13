///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.24
// Author       : Hoon
// Description  : DBMotor UserControl
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
    [ToolboxBitmap(typeof(DBMotor), "DBMotorAni.bmp")]
    public partial class DBMotor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Enum
        public enum AnimFrames
        {
            DB1, DB2, DB3, DB4, DB5, DB6
        }
        public enum Types
        {
            UP, DOWN
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private ClientManager m_Client = null;
        private Types m_Type = Types.UP;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of DBMotor  - Hoon 20080824
        /// </summary>
        [Category("DMS : UI"),
         Description("Select type of DBMotor")]
        public Types DBMotorType
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                SetDiplay();
            }
        }
        [Category("DMS : UI"),
        Description("DBMotor animation update time interval")]
        public int AnimationUpdateInterval
        {
            get { return tmrUpdateAnimation.Interval; }
            set { tmrUpdateAnimation.Interval = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter DBMotor contstructor - Hoon 20080824
        /// </summary>
        public DBMotor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// make the current DBMotor type's bitmap list - Hoon 20080824
        /// </summary>
        private void SetDiplay()
        {
            m_Bitmap.Clear();
            if (m_Type == Types.UP)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB3);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB4);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB5);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB6);
                axDiskBrush1.DiskType = 0;
            }
            else
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB1_D);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB2_D);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB3_D);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB4_D);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB5_D);
                m_Bitmap.Add(Dms.Control.Properties.Resources.DB6_D);
                axDiskBrush1.DiskType = 1;
            }
        }
        /// <summary>
        /// Update DBMotor animaion - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Show RbMotor dialog - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBMotor_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;

                DlgDBMotor dlg = new DlgDBMotor();
                dlg.Initialize(m_Tag);
                dlg.ShowDialog();
            }
            else MessageBox.Show("Tag is not selected.");
        }
        #endregion

        #region Override
        /// <summary>
        /// It should be called by HMI - Hoon 20080824
        /// </summary>
        /// <param name="tags"></param>
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
                axDiskBrush1.SetStop();
            }
            else if (m_Tag[tagDescriptor.CPON].Value == bool.FalseString || m_Tag[tagDescriptor.CPON].Value == "0")
            {
                axDiskBrush1.SetStop();
            }
            else if (m_Tag[tagDescriptor.STOP].Value == bool.TrueString || m_Tag[tagDescriptor.STOP].Value == "1")
            {
                axDiskBrush1.SetStop();
            }
            else if ((m_Tag[tagDescriptor.CW].Value == bool.TrueString || m_Tag[tagDescriptor.CW].Value == "1"))// ||
            {
                axDiskBrush1.SetCW();
                axDiskBrush1.SetRun();
            }
            else if ((m_Tag[tagDescriptor.CCW].Value == bool.TrueString || m_Tag[tagDescriptor.CCW].Value == "1"))
            {
                axDiskBrush1.SetCCW();
                axDiskBrush1.SetRun();
            }
        }
        #endregion
    }

//    public partial class DBMotor : UserControl
//    {
//        public DBMotor()
//        {
//            InitializeComponent();
//        }
//    }
}
