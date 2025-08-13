///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.25
// Author       : Hoon
// Description  : DBMotor Manual Operation Dialog
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
using Dms.Server;

namespace Dms.Control
{
    public partial class DlgDBMotor : Form
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

        private const int _DBMotorOpWidth = 80;
        private const int _Gap = 10;
        private const int _StartPointY = 30;
        #endregion

        #region Constructor
        /// <summary>
        /// No Parameter DBMotor Constructor - Hoon 20080824
        /// </summary>
        public DlgDBMotor()
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
        /// It should be called by DBMotor - Hoon 20080824
        /// </summary>
        /// <param name="DBMotortag"></param>
        public void Initialize(DeviceTag DBMotortag)
        {
            //Initialize DBMotor's tags
            DeviceTags tagContainer = m_Client.DataProvider.TagContainer;
            m_Tag = DBMotortag;
            m_Tags = tagContainer.GetTags(m_Tag.DeviceType);
            m_TagsCount = m_Tags.Count;
            if (m_Tags.GetTag(m_Tag.DeviceName) == null)
            {
                MessageBox.Show("Undefined DBMotor");
                return;
            }

            //Initialize DBMotor speed data in combo box
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
            /*int totalWidth = m_Tags.Items.Count * (_Gap + _DBMotorOpWidth) + _Gap;
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
            DBMotorOperation[] controls = new DBMotorOperation[m_TagsCount];
            for (int i = 0; i < m_TagsCount; i++)
            {
                DBMotorOperation DBMotorOp = new DBMotorOperation(m_Tags[i]);
                DBMotorOp.DBMotorClick += new DBMotorOperation.DBMotorClickEventHandler(DBMotorOp_DBMotorClick);
                DBMotorOp.Location = point;
                point.X += _Gap + _DBMotorOpWidth;
                controls[i] = DBMotorOp;
            }
            gbIndividualOp.Controls.AddRange(controls);
            //foreach (DeviceTag tag in m_Tags.Items)
            //{
            //    if (m_Tag.DeviceName == tag.DeviceName)
            //    {
            //        DBMotorOperation DBMotorOp = new DBMotorOperation(tag);
            //        DBMotorOp.Location = point;
            //        gbIndividualOp.Controls.Add(DBMotorOp);
            //        DBMotorOp.DBMotorClick += new DBMotorOperation.DBMotorClickEventHandler(DBMotorOp_DBMotorClick);
            //        point.X += _Gap + _DBMotorOpWidth;
            //    }
            //}
        }

        /// <summary>
        /// DBMotor Click EventHandler - Hoon 20080824
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="act"></param>
        void DBMotorOp_DBMotorClick(DeviceTag tag, DBMotorAct act)
        {
            int speed = m_CurrentSpeed;

            if (act == DBMotorAct.Stop)
            {
                speed = 0;
            }
            else if (act == DBMotorAct.Ccw)
            {
                speed = m_CurrentSpeed * -1;
            }

            m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, act, speed);
        }

        /// <summary>
        /// AllCw button click event - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCw_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, DBMotorAct.Cw, m_CurrentSpeed);
            }
        }

        /// <summary>
        /// AllCcw button click event - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCcw_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, DBMotorAct.Ccw, m_CurrentSpeed * -1);
            }
        }

        /// <summary>
        /// AllStop button click event - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllStop_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, DBMotorAct.Stop, 0);
            }
        }

        /// <summary>
        /// Update control's status - Hoon 20080824
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
                cvCw |= (tag[tagDescriptor.CW].Value == "1" || tag[tagDescriptor.CW].Value == bool.TrueString) ? true : false;
                cvCcw |= (tag[tagDescriptor.CCW].Value == "1" || tag[tagDescriptor.CCW].Value == bool.TrueString) ? true : false;
                cvStop &= (tag[tagDescriptor.STOP].Value == "1" || tag[tagDescriptor.STOP].Value == bool.TrueString) ? true : false;
            }
            if (cvStop == true)
            {
                btnAllCw.Enabled = true;
                btnAllCcw.Enabled = true;
            }
            if (cvCw == true)
            {
                btnAllCcw.Enabled = false;
            }
            if (cvCcw == true)
            {
                btnAllCw.Enabled = false;
            }
        }

        /// <summary>
        /// timer enable - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgDBMotor_Load(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
        }

        /// <summary>
        /// Close this dialog - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Set current speed by selectedItem - Hoon 20080824
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            //m_CurrentSpeed = (int)(((ComboBox)sender).SelectedItem);

            //m_Client.SendCommand(Command.DBMotorManual, m_Tag.DeviceName, DBMotorAct.Noop, m_CurrentSpeed);

        }
        

        private void cboSpeed_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                m_CurrentSpeed = (int)(((ComboBox)sender).SelectedItem);

                foreach (DeviceTag tag in m_Tags.Items)
                {
                    bool cvCw = (tag[tagDescriptor.CW].Value == "1" || tag[tagDescriptor.CW].Value == bool.TrueString) ? true : false;
                    bool cvCcw = (tag[tagDescriptor.CCW].Value == "1" || tag[tagDescriptor.CCW].Value == bool.TrueString) ? true : false;
                    bool cvStop = (tag[tagDescriptor.STOP].Value == "1" || tag[tagDescriptor.STOP].Value == bool.TrueString) ? true : false;

                    if (cvCw) m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, DBMotorAct.Cw, m_CurrentSpeed);
                    else if (cvCcw) m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, DBMotorAct.Ccw, m_CurrentSpeed * -1);
                    else m_Client.SendCommand(Command.DBMotorManual, tag.DeviceName, DBMotorAct.Stop, 0);
                }

                //m_Client.SendCommand(Command.DBMotorManual, m_Tag.DeviceName, DBMotorAct.Noop, m_CurrentSpeed);
            }
        }
        #endregion
    }
}