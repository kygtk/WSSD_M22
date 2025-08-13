///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.18
// Author       : eun
// Description  : RbMotor Manual Operation Dialog
///////////////////////////////////////////////////////////////////////////
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
    public partial class DlgRbMotor : Form
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTags m_Tags = null;
        private ClientManager m_Client = ClientManager.Instance;
        private Size m_OriginSize;
        private int m_TagsCount;

        private int m_MinSpeed = 100;
        private int m_MaxSpeed = 600;
        private int m_Interval = 20;
        private int m_CurrentSpeed = 0;
        private const int _DefaultSpeed = 400;

        private const int _RbMotorOpWidth = 80;
        private const int _Gap = 10;
        private const int _StartPointY = 30;
        #endregion

        #region Constructor
        /// <summary>
        /// No Parameter RbMotor Constructor - eun 20080118
        /// </summary>
        public DlgRbMotor()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// It should be called by RbMotor - eun 20080118
        /// </summary>
        /// <param name="rbMotortag"></param>
        public void Initialize(DeviceTag rbMotortag)
        {
            //Initialize RbMotor's tags
            DeviceTags tagContainer = m_Client.TagContainer;
            m_Tag = rbMotortag;
            m_Tags = tagContainer.GetTags(m_Tag.DeviceType);
            m_TagsCount = m_Tags.Count;
            if (m_Tags.GetTag(m_Tag.DeviceName) == null)
            {
                MessageBox.Show("Undefined RbMotor");
                return;
            }

            //Initialize RbMotor speed data in combo box
            int itemCount = (int)((m_MaxSpeed - m_MinSpeed) / m_Interval);
            for (int i = 0; i <= itemCount; i++)
            {
                int value = m_MinSpeed + (int)(m_Interval * i);
                cboSpeed.Items.Add(value);
            }
            //cboSpeed.SelectedItem = _DefaultSpeed;
            //m_CurrentSpeed = _DefaultSpeed;
            m_CurrentSpeed = Convert.ToInt32(m_Tag[tagDescriptor.SPEED].Value);
            cboSpeed.SelectedItem = m_CurrentSpeed;

            //Calculate and set dialog size
            /*int totalWidth = m_Tags.Items.Count * (_Gap + _RbMotorOpWidth) + _Gap;
            if (this.gbIndividualOp.Width < totalWidth)
            {
                int diff = totalWidth - this.gbIndividualOp.Width;
                if (diff > _Gap * 2)
                {
                    this.Size = new Size(this.Width + diff, this.Height);
                    gbIndividualOp.Size = new Size(gbIndividualOp.Width + diff, gbIndividualOp.Height);
                }
            }*/
            m_OriginSize = this.Size;

            Point point = new Point(_Gap, _StartPointY);
            RbMotorOperation[] controls = new RbMotorOperation[m_TagsCount];
            for (int i = 0; i < m_TagsCount; i++)
            {
                RbMotorOperation rbMotorOp = new RbMotorOperation(m_Tags[i]);
                rbMotorOp.RbMotorClick += new RbMotorOperation.RbMotorClickEventHandler(rbMotorOp_RbMotorClick);
                rbMotorOp.Location = point;
                point.X += _Gap + _RbMotorOpWidth;
                controls[i] = rbMotorOp;
            }
            tbPanel.Controls.AddRange(controls);
            //foreach (DeviceTag tag in m_Tags.Items)
            //{
            //    if (m_Tag.DeviceName == tag.DeviceName)
            //    {
            //        RbMotorOperation rbMotorOp = new RbMotorOperation(tag);
            //        rbMotorOp.Location = point;
            //        gbIndividualOp.Controls.Add(rbMotorOp);
            //        rbMotorOp.RbMotorClick += new RbMotorOperation.RbMotorClickEventHandler(rbMotorOp_RbMotorClick);
            //        point.X += _Gap + _RbMotorOpWidth;
            //    }
            //}
        }

        /// <summary>
        /// RbMotor Click EventHandler - eun 20080118
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="act"></param>
        void rbMotorOp_RbMotorClick(DeviceTag tag, RbMotorAct act)
        {
            int speed = m_CurrentSpeed;

            if (act == RbMotorAct.Stop)
            {
                speed = 0;
            }
            else if (act == RbMotorAct.Ccw)
            {
                speed = m_CurrentSpeed * -1;
            }

            m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, act, speed);
        }

        /// <summary>
        /// AllCw button click event - eun 20080118 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCw_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, RbMotorAct.Cw, m_CurrentSpeed);
            }
        }

        /// <summary>
        /// AllCcw button click event - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCcw_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, RbMotorAct.Ccw, m_CurrentSpeed * -1);
            }
        }

        /// <summary>
        /// AllStop button click event - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllStop_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, RbMotorAct.Stop, 0);
            }
        }

        /// <summary>
        /// Update control's status - eun 20080118 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            bool cvCw = false;
            bool cvCcw = false;
            bool cvStop = true;
            foreach (DeviceTag tag in m_Tags.Items)
            {
                cvCw |= (tag[tagDescriptor.CW].Value == "1" || tag[tagDescriptor.CW].Value == bool.TrueString);
                cvCcw |= (tag[tagDescriptor.CCW].Value == "1" || tag[tagDescriptor.CCW].Value == bool.TrueString);
                cvStop &= (tag[tagDescriptor.STOP].Value == "1" || tag[tagDescriptor.STOP].Value == bool.TrueString);
            }
            if (cvStop)
            {
                btnAllCw.Enabled = true;
                btnAllCcw.Enabled = true;
            }
            if (cvCw)
            {
                btnAllCcw.Enabled = false;
            }
            if (cvCcw)
            {
                btnAllCw.Enabled = false;
            }
        }

        /// <summary>
        /// timer enable - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgRbMotor_Load(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
        }

        /// <summary>
        /// Close this dialog - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Set current speed by selectedItem - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            //m_CurrentSpeed = (int)(((ComboBox)sender).SelectedItem);

            //m_Client.SendCommand(Command.RbMotorManual, m_Tag.DeviceName, RbMotorAct.Noop, m_CurrentSpeed);

        }


        private void cboSpeed_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                m_CurrentSpeed = (int)(((ComboBox)sender).SelectedItem);

                foreach (DeviceTag tag in m_Tags.Items)
                {
                    bool cvCw = (tag[tagDescriptor.CW].Value == "1" || tag[tagDescriptor.CW].Value == bool.TrueString);
                    bool cvCcw = (tag[tagDescriptor.CCW].Value == "1" || tag[tagDescriptor.CCW].Value == bool.TrueString);
                    bool cvStop = (tag[tagDescriptor.STOP].Value == "1" || tag[tagDescriptor.STOP].Value == bool.TrueString);

                    if (cvCw) m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, RbMotorAct.Cw, m_CurrentSpeed);
                    else if (cvCcw) m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, RbMotorAct.Ccw, m_CurrentSpeed * -1);
                    else m_Client.SendCommand(Command.RbMotorManual, tag.DeviceName, RbMotorAct.Stop, 0);
                }

                //m_Client.SendCommand(Command.RbMotorManual, m_Tag.DeviceName, RbMotorAct.Noop, m_CurrentSpeed);
            }
        }
        #endregion
    }
}