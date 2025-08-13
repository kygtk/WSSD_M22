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
    public partial class DlgDryPump : Form
    {
        #region Tag Descriptor
        public static TagDescriptorDryPump tagDescriptor = new TagDescriptorDryPump();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private ClientManager m_Client = ClientManager.Instance;
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
        public DlgDryPump(DeviceTag tag)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            tmrUpdateState.Enabled = true;

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
                lblALARM.BackColor = Color.Red;
                btnStart.Enabled = false;
            }
            else 
            {
                lblALARM.BackColor = Color.White;
            }

            if (m_Tag[tagDescriptor.WARNING].Value == "1" || m_Tag[tagDescriptor.WARNING].Value == bool.TrueString)
            {
                lblWARNING.BackColor = Color.Red;
            }
            else
            {
                lblWARNING.BackColor = Color.White;
            }

            if (m_Tag[tagDescriptor.RUNNING].Value == "1" || m_Tag[tagDescriptor.RUNNING].Value == bool.TrueString)
            {
                lblRUNNING.BackColor = Color.GreenYellow;
            }
            else
            {
                lblRUNNING.BackColor = Color.White;
            }

            //if (m_Tag[tagDescriptor.EXTWARNING].Value == "1" || m_Tag[tagDescriptor.EXTWARNING].Value == bool.TrueString)
            //{
            //    lblEXTWARNING.BackColor = Color.Red;
            //}
            //else
            //{
            //    lblEXTWARNING.BackColor = Color.White;
            //}

            //if (m_Tag[tagDescriptor.PCWFLOWOK].Value == "1" || m_Tag[tagDescriptor.PCWFLOWOK].Value == bool.TrueString)
            //{
            //    lblPCWFLOWOK.BackColor = Color.Red;
            //}
            //else
            //{
            //    lblPCWFLOWOK.BackColor = Color.White;
            //}

            //if (m_Tag[tagDescriptor.N2FLOWWARNING].Value == "1" || m_Tag[tagDescriptor.N2FLOWWARNING].Value == bool.TrueString)
            //{
            //    lblN2FLOWWARNING.BackColor = Color.Red;
            //}
            //else
            //{
            //    lblN2FLOWWARNING.BackColor = Color.White;
            //}


            
        }

        /// <summary>
        /// start the timer - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgPump_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;

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



    }
}