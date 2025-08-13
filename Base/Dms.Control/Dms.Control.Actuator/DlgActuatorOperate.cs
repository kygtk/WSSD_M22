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
    public partial class DlgActuatorOperate : Form
    {
        #region Tag Descriptor
        public static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private ClientManager m_Client = ClientManager.Instance;
        private ActuatorType m_ActuatorType;
        private Actuator m_ActuatorControl;
        #endregion

        #region Constructor
        public DlgActuatorOperate(DeviceTag tag, ActuatorType actuatorType)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            
            m_Tag = tag;
            m_ActuatorType = actuatorType;
        }
        public DlgActuatorOperate(DeviceTag tag, ActuatorType actuatorType, Actuator control) :
            this(tag, actuatorType)
        {
            m_ActuatorControl = control;
        }
        #endregion

        #region Methods
        private void btnFW_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ActuatorManual, m_Tag.DeviceName, ActuatorAct.Pos);
        }

        private void btnBW_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ActuatorManual, m_Tag.DeviceName, ActuatorAct.Neg);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            this.Close();
        }

        private void timerUpdateState_Tick(object sender, EventArgs e)
        {
            string pos = m_Tag[tagDescriptor.POS_SENSOR].Value;
            string neg = m_Tag[tagDescriptor.NEG_SENSOR].Value;

            if (pos == bool.TrueString || pos == "1") 
                this.checkFW.Checked = true;
            else 
                this.checkFW.Checked = false;

            if (neg == bool.TrueString || neg == "1") 
                this.checkBW.Checked = true;
            else 
                this.checkBW.Checked = false;
        }

        private void ActuatorOperateForm_Load(object sender, EventArgs e)
        {
            if (m_Tag != null)
            {
                this.Text = m_Tag.DeviceName;
            }
            if (m_Client.GenInfos.AutoMode)
            {
                this.btnBW.Enabled = false;
                this.btnFW.Enabled = false;
            }

            if (m_ActuatorType == ActuatorType.Lift)
            {
                checkFW.Text = "Up Sensor";
                checkBW.Text = "Down Sensor";
                btnFW.Text = "UP";
                btnBW.Text = "DOWN";
            }
            else if (m_ActuatorType == ActuatorType.Chuck)
            {
                checkFW.Text = "Lock Sensor";
                checkBW.Text = "Unlock Sensor";
                btnFW.Text = "LOCK";
                btnBW.Text = "UNLOCK";
            }
            else if (m_ActuatorType == ActuatorType.OpenClose)
            {
                checkFW.Text = "Close Sensor";
                checkBW.Text = "Open Sensor";
                btnFW.Text = "CLOSE";
                btnBW.Text = "OPEN";
            }
            else if (m_ActuatorType == ActuatorType.UserDefinedText)
            {
                checkFW.Text = m_ActuatorControl.TextPositive;
                checkBW.Text = m_ActuatorControl.TextNegative;
                btnFW.Text = m_ActuatorControl.TextPositive;
                btnBW.Text = m_ActuatorControl.TextNegative;
            }
        }
        #endregion
    }
}