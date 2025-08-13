///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.11
// Author       : eun
// Description  : AutoValve Open/Close Dialog
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
    public partial class DlgGasControl : Form
    {
        #region Tag Descriptor
        public static TagDescriptorGasControl tagDescriptor = new TagDescriptorGasControl();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private ClientManager m_Client = ClientManager.Instance;
        #endregion

        #region Delegates
        /// <summary>
        /// AutoValve manual action changed delegate - eun 20080118
        /// </summary>
        /// <param name="act"></param>
        //public delegate void AutoValveManualActEventHandler(AutoValveAct act);
        /// <summary>
        /// Autovalve manual action changed event - eun 20080118
        /// </summary>
        //public event AutoValveManualActEventHandler AutoValveActChanged;
        #endregion

        #region Properties
        public DeviceTag GasControlTag
        {
            set
            { 
                m_Tag = value;
                m_TagOld.Clone(m_Tag);
                this.Text = m_Tag.DeviceName;
                UpdateState();
                tmrUpdateState.Enabled = true;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No Parameter Constructor - eun 200870111
        /// </summary>
        public DlgGasControl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
    
        /// <summary>
        /// Having name parameter constructor - eun 20080111
        /// </summary>
        /// <param name="name"></param>
        public DlgGasControl(DeviceTag tag)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_Tag = tag;
            m_TagOld.Clone(m_Tag);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Close this dialog - eun 20080111
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            tmrUpdateState.Enabled = false;
            this.Close();
            //this.Dispose();
        }

        /// <summary>
        /// Set dialog title and current status - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgAutoValve_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;
            btnOk.Focus();
            UpdateState();
            tmrUpdateState.Enabled = true;
        } 

        /// <summary>
        /// Send open state message - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.GasControl, m_Tag.DeviceName, AutoValveAct.Open);
        }

        /// <summary>
        /// Send close state message - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.GasControl, m_Tag.DeviceName, AutoValveAct.Close);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }

        private void UpdateState()
        {
            //if (m_Tag[tagDescriptor.OPEN].Value == bool.TrueString || m_Tag[tagDescriptor.OPEN].Value == "1")
            //{
            //    this.btnOpen.Enabled = false;
            //    this.btnClose.Enabled = true;
            //}
            //else //if (m_Tag[_RUN].Value == bool.FalseString || m_Tag[_RUN].Value == "0")
            //{
            //    this.btnOpen.Enabled = true;
            //    this.btnClose.Enabled = false;
            //}
        }
        #endregion
    }
}