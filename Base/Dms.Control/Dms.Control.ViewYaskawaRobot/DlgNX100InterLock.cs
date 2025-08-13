using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;
using Dms.Server;
using Dms.Data;

namespace Dms.Control
{
    public partial class DlgNX100InterLock : Form
    {
        private enum Conditions
        {
            Condition1,
            Condition2,
            Condition3,
            Condition4,
            Condition5,
            Condition6,
            Condition7,
            Condition8,
            MaxItemNo,
        }

        private enum Status
        {
            Wait,
            Ok,
            Ng,
            Skip,
        }

        private enum Column
        {
            Description,
            Status,
            MaxItemNo,
        }

        #region Filed
        private YaskawaNX100 m_Robot = null;
        private PortUnit m_PortUnit = null;

        private bool m_ConditionOk = true;
        private bool m_CheckComp = false;
        private bool m_RobotJobBegin = false;
        private bool m_RobotJobComp = false;
        private bool m_ForceMove = false;

        private bool m_MappingOk = false;

        private string m_Command = null;
        #endregion

        #region Constructor
        public DlgNX100InterLock()
        {
            InitializeComponent();
        }

        public DlgNX100InterLock(PortUnit portUnit, YaskawaNX100 robot)              //port interlock Check Constructor
        {
            InitializeComponent();

            m_PortUnit = portUnit;
            m_Robot = robot;

            InitControl();

        }
        #endregion

        #region methods
        private void InitControl()
        {
            int col = (int)Column.Description;
            int row = (int)Conditions.Condition1;

            dataGridView.ColumnCount = (int)Column.MaxItemNo;
            dataGridView.RowCount = (int)Conditions.MaxItemNo;
            dataGridView.Columns[col].Width = dataGridView.Width - 60;
            
            dataGridView[col, row++].Value = "Robot Ready";
            dataGridView[col, row++].Value = "CST Detection Sensor";
            dataGridView[col, row++].Value = "CST Opposite Sensor";
            dataGridView[col, row++].Value = "CST Clamp Forward Sensor";
            dataGridView[col, row++].Value = "CST Floating Lock Status";
            dataGridView[col, row++].Value = "CST Glass Mapping";
            dataGridView[col, row++].Value = "CST Selected Slot Check";
            dataGridView[col, row++].Value = "Robot Hand Glass Check";

            col = (int)Column.Status;
            row = (int)Conditions.Condition1;
            dataGridView.Columns[col].Width = 57;

            for(int i = 0; i < dataGridView.RowCount; i++)
            {
                dataGridView[col, row].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dataGridView[col, row].Style.BackColor = Color.White;
                dataGridView[col, row++].Value = Status.Wait;
            }

            lblStatus.BackColor = Color.Blue;
            lblStatus.Text = "Init Condition Check";

            string hand = m_Robot.ManualHand.ToString();
            string cube = m_Robot.ManualCube.ToString();
            string pattern = m_Robot.ManualPattern.ToString();
            int slotNo = m_Robot.ManualSlotNo;

            if(m_PortUnit != null)
                m_Command = string.Format("CUBE : {0}\nJOB  : {1}\nHAND : {2}\nSLOT : {3}\n", 
                                          cube, pattern, hand, slotNo);
            else
                m_Command = string.Format("CUBE : {0}\nJOB  : {1}\nHAND : {2}\n", 
                                          cube, pattern, hand);
            lblPattern.Text = m_Command;

            btnClose.Enabled = false;
            btnForceMove.Enabled = false;
            timer1.Start();
        }

        private void SetCheckStatus(Conditions condition, Status status)
        {
            int col = (int)Column.Status;
            int row = (int)condition;

            Color color = Color.White;

            if(status == Status.Wait)
                color = Color.White;
            else if(status == Status.Ok)
                color = Color.Green;
            else if(status == Status.Ng)
                color = Color.Red;
            else if(status == Status.Skip)
                color = Color.DarkGray;
            
            dataGridView[col, row].Value = status;
            dataGridView[col, row].Style.BackColor = color;
        }

        private void UpdataButtonStatus()
        {
            bool enable = true;
            enable &= !m_ConditionOk;
            enable &= m_CheckComp;
            enable &= m_Robot.IsRobotReady();
            enable &= !m_Robot.IsRobotHold();
            btnForceMove.Enabled = enable;

            enable = m_CheckComp;
            enable |= !m_ConditionOk;
            btnClose.Enabled = enable;
        }

        private void UpdateCheckStatus()
        {
            bool checkComp = true;
            bool conditionOk = true;
            bool status = false;
            int col = (int)Column.Status;
            for (int row = 0; row < (int)Conditions.MaxItemNo; row++)
			{
                status = (Status)dataGridView[col, row].Value != Status.Wait;
                checkComp &= status;

                status = (Status)dataGridView[col, row].Value != Status.Ng;
                conditionOk &= status;
                
                if(!conditionOk)
                    break;
			}
            m_CheckComp = checkComp;
            m_ConditionOk = conditionOk;

            if (!m_ConditionOk)
            {
                lblStatus.BackColor = Color.Red;
                lblStatus.Text = "Condition Check Fail";
            }
            else if (!m_CheckComp && m_ConditionOk)
            {
                lblStatus.BackColor = Color.LightBlue;
                lblStatus.Text = "In Condition Check";
            }
            else if (m_CheckComp && m_ConditionOk)
            {
                lblStatus.BackColor = Color.Green;
                lblStatus.Text = "Condition Check OK";
            }
        }

        private void DoCheckCondition()
        {
            bool conditionOk = true;
            Status status = Status.Wait;
            int col = (int)Column.Status;
            int row = 0;

            bool handInterrupt = false;
            handInterrupt |= m_Robot.ManualPattern == nxOP_PATTERN.GET;
            handInterrupt |= m_Robot.ManualPattern == nxOP_PATTERN.PUT;
            handInterrupt |= m_Robot.ManualPattern == nxOP_PATTERN.EXCHANGE;

            #region Condition : Robot Ready
            row = (int)Conditions.Condition1;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                conditionOk = m_Robot.IsRobotReady();
                conditionOk &= !m_Robot.IsRobotHold();
                status = conditionOk ? Status.Ok : Status.Ng;

                //SetCheckStatus(Conditions.Condition1, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : CST Detection Sensor
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                if(m_PortUnit != null && handInterrupt)
                {
                    conditionOk = m_PortUnit.IsCstExist();
                    status = conditionOk ? Status.Ok : Status.Ng;
                }
                else
                    status = Status.Skip;
                //SetCheckStatus(Conditions.Condition2, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : CST Opposite Sensor
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                if(m_PortUnit != null && handInterrupt)
                {
                    conditionOk = !m_PortUnit.IsCstOpposite();
                    status = conditionOk ? Status.Ok : Status.Ng;
                }
                else 
                    status = Status.Skip;
                //SetCheckStatus(Conditions.Condition3, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : CST Clamp Forward Sensor
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                if(m_PortUnit != null && handInterrupt)
                {
                    conditionOk = m_PortUnit.IsCstClampPos();
                    status = conditionOk ? Status.Ok : Status.Ng;
                }
                else
                    status = Status.Skip;
                //SetCheckStatus(Conditions.Condition4, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : CST Floating Lock Status
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                if(m_PortUnit != null && handInterrupt)
                {
                    conditionOk = m_PortUnit.IsCstFloatPos();
                    status = conditionOk ? Status.Ok : Status.Ng;
                }
                else
                    status = Status.Skip;
                //SetCheckStatus(Conditions.Condition5, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : CST Glass Mapping
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                if(m_PortUnit != null && handInterrupt)
                {
                    int mapping = m_PortUnit.MappingUnit.Mapping(m_PortUnit.Id);
                    if(mapping == 0)
                    {
                        status = Status.Ok;
                        m_MappingOk = true;
                    }
                    else if(mapping > 0)
                    {
                        status = Status.Ng;
                        m_MappingOk = false;
                    }
                    else
                        status = Status.Wait;
                }
                else
                    status = Status.Skip;
                //SetCheckStatus(Conditions.Condition6, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : CST Selected Slot Check
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                if(m_PortUnit != null && handInterrupt)
                {
                    if(m_MappingOk)
                    {
                        MappingStatus mappingStatus = m_PortUnit.MappingUnit.GetGlassMappingStatus(m_PortUnit.Id, m_Robot.ManualSlotNo - 1);
                        conditionOk = (mappingStatus == MappingStatus.On && m_Robot.ManualPattern == nxOP_PATTERN.GET);
                        conditionOk |= (mappingStatus == MappingStatus.Off && m_Robot.ManualPattern == nxOP_PATTERN.PUT);

                        status = conditionOk ? Status.Ok : Status.Ng;
                    }
                    else
                        status = Status.Wait;
                }
                else
                    status = Status.Skip;
                //SetCheckStatus(Conditions.Condition7, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion

            #region Condition : Robot Hand Glass Check
            row++;
            if((Status)dataGridView[col, row].Value == Status.Wait)
            {
                bool handGlsExist = false;
                handGlsExist |= (m_Robot.ManualHand == nxOP_HAND.LOWER_HAND && m_Robot.IsLowerHandGlassDetected());
                handGlsExist |= (m_Robot.ManualHand == nxOP_HAND.UPPER_HAND && m_Robot.IsUpperHandGlassDetected());

                conditionOk = (handGlsExist && m_Robot.ManualPattern == nxOP_PATTERN.PUT);
                conditionOk |= (handGlsExist && m_Robot.ManualPattern == nxOP_PATTERN.PUT_PREPARE);
                conditionOk |= (!handGlsExist && m_Robot.ManualPattern == nxOP_PATTERN.GET);
                conditionOk |= (!handGlsExist && m_Robot.ManualPattern == nxOP_PATTERN.GET_PREPARE);
                status = conditionOk ? Status.Ok : Status.Ng;

                //SetCheckStatus(Conditions.Condition8, status);
                SetCheckStatus((Conditions)row, status);
            }
            #endregion
        }
        #endregion

        #region Sequence & Event
        private void timerUpdateState_Tick(object sender, EventArgs e)
        {
            UpdataButtonStatus();

            if(!m_RobotJobBegin)
            {
                UpdateCheckStatus();
                DoCheckCondition();
            }

            bool beginCondition = m_CheckComp & m_ConditionOk;
            beginCondition |= m_CheckComp & m_ForceMove;
            if(beginCondition && !m_RobotJobBegin)
            {
                m_Robot.ManualMotionStrobeReq = true;
            }

            if(!m_RobotJobBegin && m_Robot.IsRobotBusy())
            {
                string pattern = lblPattern.Text;
                pattern += "Manual Job is Begun\n";
                lblPattern.Text = pattern;

                m_RobotJobBegin = true;
            }

            if(m_RobotJobBegin && !m_RobotJobComp)
            {
                if(m_Robot.IsRobotReady())
                {
                    m_RobotJobComp = true;
                    lblPattern.Text += "Manual Job is Finished\n";
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnForce_Click(object sender, EventArgs e)
        {
            if(!m_ForceMove)
                m_ForceMove = true;
        }
        #endregion
    }
}