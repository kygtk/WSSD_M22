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
using Dms.Device;

namespace Dms.Control
{
    public partial class DlgShowerHead : Form
    {
        #region Tag Descriptor
        private TagDescriptorServoUnit tagDescriptor = new TagDescriptorServoUnit();
        #endregion

        #region Fields
        private ClientManager m_Client;
        protected DeviceTagInfo m_TagInfo = null;
        private DeviceTag m_TagServoUnit = null;
        private ServoUnit m_ServoUnit;
        private _GenericCollection<ServoMotor> m_Axes;
        private ServoMotor m_Axis;
        private int m_SelectedPoint = 0;
        #endregion

        public DlgShowerHead()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(DeviceTag tagServoUnit)
        {
            m_Client = ClientManager.Instance;
            m_TagServoUnit = tagServoUnit;
            m_TagInfo = new DeviceTagInfo("ServoUnit");

            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;
            m_ServoUnit = components[m_TagServoUnit.DeviceName] as ServoUnit;

            if (m_ServoUnit == null) 
            {
                tmrUpdateState.Enabled = false;
                return;
            }
            else
            {
                m_Axes = m_ServoUnit.Axis;
                m_Axis = m_Axes[0];

                InitTeachPointList();
                tmrUpdateState.Enabled = true;
            }

            lblCommand.Text = null;
            lblSetPos.Text = "0";
        }


        private void DlgShowerHead_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            
            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;

            m_ServoUnit = components[m_TagServoUnit.DeviceName] as ServoUnit;

            if (m_ServoUnit == null) return;

            tmrUpdateState.Enabled = true;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateEvent();
            UpdateSource();
            UpdateCurPos();
            UpdateSensor();
            UpdateStatusServoOperationButtons();
        }

        private void btnEstop_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoManual, ServoManualAct.Estop, m_ServoUnit);
            lblCommand.Text = "E-STOP";
        }

        private void btnServoOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoManual, ServoManualAct.ServoOn, m_ServoUnit);
            lblCommand.Text = "Servo On";
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoManual, ServoManualAct.Home, m_ServoUnit);

            lblCommand.Text = "Homming";
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            short selectedId = GetSelctedPointId();
            if (selectedId < 0)
            {
                MessageBox.Show("Please select target point!");
                return;
            }

            if (m_ServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoManual, ServoManualAct.Move, m_ServoUnit, m_SelectedPoint);
            }

            lblCommand.Text = "Move";
        }

        private void trackBarVelRatio_Scroll(object sender, EventArgs e)
        {
            txtVelRatio.Text = ((TrackBar)sender).Value.ToString();
        }

        private void UpdateCurPos()
        {
            string curPos = (m_Axis.GetPosition()).ToString();
            if (lblCurPos.Text != curPos) lblCurPos.Text = curPos;
        }

        private void UpdateEvent()
        {
            string curEvent = null;

            switch (m_Axis.GetAxisState())
            {
                case AxisEvent.NoEvent:
                    curEvent = "NoEvent";
                    break;
                case AxisEvent.StopEvent:
                    curEvent = "StopEvent";
                    break;
                case AxisEvent.EStopEvent:
                    curEvent = "EStopEvent";
                    break;
                case AxisEvent.AbortEvent:
                    curEvent = "AbortEvent";
                    break;
            }

            if (lblEvent.Text != curEvent) lblEvent.Text = curEvent;
        }

        private void UpdateSource()
        {
            string curSource = null;

            switch (m_Axis.GetAxisSource())
            {
                case AxisSource.StNone:
                    curSource = "StNone";
                    break;
                case AxisSource.StHomeSwitch:
                    curSource = "StHomeSwitch";
                    break;
                case AxisSource.StPosLimit:
                    curSource = "StPosLimit";
                  break;
                case AxisSource.StNegLimit:
                    curSource = "StNegLimit";
                   break;
                case AxisSource.StAmpFault:
                    curSource = "StAmpFault";
                    break;
                case AxisSource.StALimit:
                    curSource = "StALimit";
                    break;
                case AxisSource.StVLimit:
                    curSource = "StVLimit";
                 break;
                case AxisSource.StXNegLimit:
                    curSource = "StXNegLimit";
                    break;
                case AxisSource.StXPosLimit:
                    curSource = "StXPosLimit";
                   break;
                case AxisSource.StErrorLimit:
                    curSource = "StErrorLimit";
                    break;
                case AxisSource.StPcCommand:
                    curSource = "StPcCommand";
                    break;
                case AxisSource.StOutofFrames:
                    curSource = "StOutofFrames";
                  break;
                case AxisSource.StAmpPowerOnOff:
                    curSource = "StAmpPowerOnOff";
                 break;
                case AxisSource.StAbsCommError:
                    curSource = "StAbsCommError";
                  break;
                case AxisSource.StInpositonStatus:
                    curSource = "StInpositonStatus";
                  break;
                case AxisSource.StRunStopCommand:
                    curSource = "StRunStopCommand";
                  break;
                case AxisSource.StCollisionState:
                    curSource = "StCollisionState";
                    break;
                case AxisSource.StPaustateState:
                    curSource = "StPaustateState";
                    break;
            }
            if (lblSource.Text != curSource) lblSource.Text = curSource;
        }

        private void UpdateStatusServoOperationButtons()
        {
            // Servo Status
            bool servoOn = m_ServoUnit.Ready;
            bool servoHome = m_ServoUnit.HomeComp;
            bool servoMoving = m_ServoUnit.ManualMoving;

            // Button Servo Estop
            checkBoxServoEstop.Checked = !servoOn;

            // Button Servo On
            checkBoxServoOn.Checked = servoOn;
            btnServoOn.Enabled = !servoOn;

            // Button Servo Home
            checkBoxServoHome.Checked = servoHome;

            if (servoHome)
            {   // Home complete
                if (btnHome.BackColor != Color.Lime) btnHome.BackColor = Color.Lime;
            }
            else if (GetManualCommand() == RbtAction.Home)
            {   // Homing
                if (btnHome.BackColor != Color.Red) btnHome.BackColor = Color.Red;
            }
            else
            {   // Noop
                if (btnHome.BackColor != Color.LemonChiffon) btnHome.BackColor = Color.LemonChiffon;
            }

            btnHome.Enabled = servoOn && !servoMoving;

            // Servo Operation Buttons
            btnMove.Enabled = servoOn && servoHome && !servoMoving;
        }

        private void UpdateSensor()
        {
            bool negSwitch = m_Axis.GetNegSwitch();
            bool posSwitch = m_Axis.GetPosSwitch();
            bool homeSwitch = m_Axis.GetHomeSwitch();

            lblNegSwitch.BackColor = negSwitch ? Color.Red : Color.White;
            lblHomeSwitch.BackColor = homeSwitch ? Color.Lime : Color.White;
            lblPosSwitch.BackColor = posSwitch ? Color.Red : Color.White;
        }

        private RbtAction GetManualCommand()
        {
            RbtAction act = (RbtAction)m_ServoUnit.ManualActionCmd;
            if (act >= RbtAction.MoveRepeat && act < RbtAction.EndOfActionCode)
            {
                act = RbtAction.MoveRepeat;
            }
            return act;
        }

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            short selectedId = GetSelctedPointId();

            if (btnRepeat.Text == "REPEAT")
            {
                if (selectedId < 0)
                {
                    MessageBox.Show("Please select target point!");
                    return;
                }

                btnRepeat.Text = "STOP";

                lblCommand.Text = "Repeat Start"; 

                if (m_ServoUnit.IsInterlockCondition())
                {
                    MessageBox.Show("Can not move : Interlock Condition");
                }
                else
                {
                    m_Client.SendCommand(Command.ServoManual, ServoManualAct.RepeatStart, m_ServoUnit, m_SelectedPoint);
                }
            }
            else
            {
                btnRepeat.Text = "REPEAT";

                lblCommand.Text = "Repeat Stop";

                m_Client.SendCommand(Command.ServoManual, ServoManualAct.RepeatStop, m_ServoUnit);
            }
        }

        private void InitTeachPointList()
        {
            listTeachPoint.Columns.Clear();
            ColumnHeader[] colHeaders = new ColumnHeader[m_Axes.Count + 2];
            int colHeaderCount = colHeaders.Length;
            for (int i = 0; i < colHeaderCount; i++)
            {
                colHeaders[i] = new ColumnHeader();
            }

            ColumnHeader colHeader = colHeaders[0];
            colHeader.Text = "No";
            colHeader.TextAlign = HorizontalAlignment.Center;
            colHeader.Width = 35;

            int axisCount = m_Axes.Count;
            for (int i = 0; i < axisCount; i++)
            {
                colHeader = colHeaders[i + 1];
                colHeader.Text = "Position";
                colHeader.TextAlign = HorizontalAlignment.Right;
                colHeader.Width = 70;
            }

            colHeader = colHeaders[colHeaderCount - 1];
            colHeader.Text = "Description";
            colHeader.TextAlign = HorizontalAlignment.Left;
            colHeader.Width = 160;

            for (int i = 0; i < colHeaderCount; i++)
            {
                listTeachPoint.Columns.Add(colHeaders[i]);
            }

            SetTeachPointListData();
        }

        private void SetTeachPointListData()
        {
            listTeachPoint.Items.Clear();
            string tmp = "";
            int pointCount = m_ServoUnit.TeachPoints;
            int axisCount = m_ServoUnit.AxisCount;

            for (short id = 0; id < pointCount; id++)
            {
                ListViewItem lvItem = new ListViewItem((id + 1).ToString());
                for (short axisId = 0; axisId < axisCount; axisId++)
                {
                    tmp = string.Format("{0:F2}", m_ServoUnit.GetTeachPointPos(id, axisId));
                    lvItem.SubItems.Add(tmp);
                }
                lvItem.SubItems.Add(m_ServoUnit.GetPointName(id));
                listTeachPoint.Items.Add(lvItem);
            }
        }

        private void listTeachPoint_Click(object sender, EventArgs e)
        {
            short pointId = GetSelctedPointId();
            RbtPos pos = m_ServoUnit.GetTeachPointPos(pointId);

            m_SelectedPoint = pointId;

            lblSetPos.Text = m_ServoUnit.GetTeachPointPos(Convert.ToInt16(m_SelectedPoint), 0).ToString();

        }

        private short GetSelctedPointId()
        {
            // -1 : not selected
            short pointId = -1;
            if (listTeachPoint.FocusedItem != null)
            {
                string item = listTeachPoint.FocusedItem.Text;
                pointId = (short)(Convert.ToInt16(item) - 1);
            }

            return pointId;
        }

        private void trackBarJogVel_Scroll(object sender, EventArgs e)
        {
            txtJogVel.Text = ((TrackBar)sender).Value.ToString();
            
        }

        private void btnJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_ServoUnit == null)
            {
                MessageBox.Show("Please select axis!");
                return;
            }

            // Jog도 인터락을 체크 하는지 ? 안한다면 아래 조건 Skip하도록
            if (m_ServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoManual, ServoManualAct.JogPlusStart, m_Axis, txtJogVel.Text);
            }

            lblCommand.Text = "Start Jog Move Plus ";
        }

        private void btnJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_ServoUnit == null)
            {
                MessageBox.Show("Please select axis!");
                return;
            }

            // Jog도 인터락을 체크 하는지 ? 안한다면 아래 조건 Skip하도록
            if (m_ServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoManual, ServoManualAct.JogMinusStart, m_Axis, txtJogVel.Text);
            }

            lblCommand.Text = "Start Jog Move Minus.";
        }

        private void btnJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_Client.SendCommand(Command.ServoManual, ServoManualAct.JogMinusStop, m_Axis);

            lblCommand.Text = "Stop Jog Move";
        }

        private void btnJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            m_Client.SendCommand(Command.ServoManual, ServoManualAct.JogPlusStop, m_Axis);
            lblCommand.Text = "Stop Jog Move";
        }
    }
}