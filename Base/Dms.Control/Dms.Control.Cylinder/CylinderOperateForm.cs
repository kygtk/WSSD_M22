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
    public partial class CylinderOperateForm : Form
    {
        #region Tag Descriptor
        public static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private ClientManager m_Client = ClientManager.Instance;
        private Cylinder.DirectionType m_Type;
        #endregion

        #region Constructor
        public CylinderOperateForm(DeviceTag tag, Cylinder.DirectionType type)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_Tag = tag;
            m_Type = type;
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
            if (m_Tag[tagDescriptor.POS_SENSOR].Value == bool.TrueString || m_Tag[tagDescriptor.POS_SENSOR].Value == "1") 
                this.checkFW.Checked = true;
            else 
                this.checkFW.Checked = false;

            if (m_Tag[tagDescriptor.NEG_SENSOR].Value == bool.TrueString || m_Tag[tagDescriptor.NEG_SENSOR].Value == "1") 
                this.checkBW.Checked = true;
            else 
                this.checkBW.Checked = false;
        }

        private void CylinderOperateForm_Load(object sender, EventArgs e)
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

            if (m_Type == Cylinder.DirectionType.UP_DOWN)
            {
                checkFW.Text = "Up Sensor";
                checkBW.Text = "Down Sensor";
                btnFW.Text = "UP";
                btnBW.Text = "DOWN";
            }
            else if (m_Type == Cylinder.DirectionType.LEFT_RIGHT)
            {
                checkFW.Text = "Left Sensor";
                checkBW.Text = "Right Sensor";
                btnFW.Text = "LEFT";
                btnBW.Text = "RIGHT";
            }
            else if (m_Type == Cylinder.DirectionType.OPEN_CLOSE)
            {
                checkFW.Text = "Close Sensor";
                checkBW.Text = "Open Sensor";
                btnFW.Text = "Close";
                btnBW.Text = "Open";
            }
        }
        #endregion
    }
}