///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Pump Manual Operation Dialog(no Inverter)
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
    public partial class DlgPump : Form
    {
        #region Tag Descriptor
        public static TagDescriptorPump tagDescriptor = new TagDescriptorPump();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private ClientManager m_Client = ClientManager.Instance;
        private Dms.Control.Pump.DeviceType m_Type = Dms.Control.Pump.DeviceType.Normal;

        private const int _NormalHeight = 134;
        private const int _InverterHeight = 271;
        #endregion

        #region Properties
        public Dms.Control.Pump.DeviceType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region Delegates
        /// <summary>
        /// Pump manual action changed delegate - eun 20080211
        /// </summary>
        /// <param name="act"></param>
        //public delegate void PumpManualActEventHandler(int id, PumpAct act);
        /// <summary>
        /// Pump manual action changed event - eun 20080211
        /// </summary>
        //public event PumpManualActEventHandler PumpActChanged;
        #endregion

        /// <summary>
        /// DlgPump Constructor having tag - eun 20080118
        /// </summary>
        /// <param name="pump"></param>
        #region Constructor
        public DlgPump(DeviceTag tag)
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
        /// Update pump state - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (m_Tag[tagDescriptor.ALARM].Value == "1" || m_Tag[tagDescriptor.ALARM].Value == bool.TrueString)
            {
                optAlarm.Checked = true;
                optNoAlarm.Checked = false;
                btnStart.Enabled = false;
            }
            else if (m_Tag[tagDescriptor.RUN].Value == "1" || m_Tag[tagDescriptor.RUN].Value == bool.TrueString)
            {
                optNoAlarm.Checked = true;
                optAlarm.Checked = false;
                btnOff.Enabled = true;
                btnStart.Enabled = false;
            }
            else
            {
                optNoAlarm.Checked = true;
                optAlarm.Checked = false;
                btnOff.Enabled = false;
                btnStart.Enabled = true;
            }

            if (m_Type == Dms.Control.Pump.DeviceType.Normal)
            {

            }

        }

        /// <summary>
        /// start the timer - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgPump_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;
            if (m_Type == Dms.Control.Pump.DeviceType.Normal)
            {
                this.Size = new Size(this.Width, _NormalHeight);
            }
            else if (m_Type == Dms.Control.Pump.DeviceType.Inverter)
            {
                this.Size = new Size(this.Width, _InverterHeight);
            }

            UpdateState();
            tmrUpdateState.Enabled = true;
        }

        /// <summary>
        /// - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.PumpManual, m_Tag.DeviceName, PumpAct.Run);
        }

        /// <summary>
        /// - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.PumpManual, m_Tag.DeviceName, PumpAct.Stop);
        }

        /// <summary>
        /// close the dialog - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}