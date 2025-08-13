using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;

namespace Dms.Control
{
    public partial class ServoTabMp2300Ctrl : UserControl
    {
        #region Enum
        private enum ObjLevel
        {
            ServoUnit,
            ServoMotor
        }

        //private enum AxesListHeader
        //{ No, Name, Set_Pos, Cur_Pos, Cur_Vel, Cur_Acc, Neg, Home, Pos, Event, Source }
        #endregion

        #region Fields
        private ServoTabMp2300MapDataCtrl m_MapControl = null;
        private ServoUnitMp2300s m_ServoUnits;
        private ServoUnitMp2300 m_CurrentServoUnit;
        private _GenericCollection<ServoMotorMp2300> m_Axes;
        private ServoMotorMp2300 m_CurrentAxis;
        //private int m_OldRowIndex = 0;
        private int m_SelectedPoint = 0;
        private System.Threading.Mutex m_Mutex = new System.Threading.Mutex();

        //private double oldPosition = 0;
        //private string oldEvent = null;
        //private string oldSource = null;
        //private LimitSensor oldLimit;
        private ClientManager m_Client;
        private bool m_Initialized = false;
        private bool m_UseCompareHomeInfo;
        private bool m_UseComparePointInfo;
        private int m_TeachPointListHeight = 0;
        #endregion

        #region Properties
        public ServoUnitMp2300s ServoUnits
        {
            get { return m_ServoUnits; }
            set { m_ServoUnits = value; }
        }

        public ServoUnitMp2300 CurrentServoUnit
        {
            get { return m_CurrentServoUnit; }
            set { m_CurrentServoUnit = value; }
        }

        public _GenericCollection<ServoMotorMp2300> Axes
        {
            get { return m_Axes; }
            set { m_Axes = value; }
        }

        public ServoMotorMp2300 CurrentAxis
        {
            get { return m_CurrentAxis; }
            set { m_CurrentAxis = value; }
        }

        public int SelectedPoint
        {
            get { return m_SelectedPoint; }
        }

        public bool UpdateTimerEnabled
        {
            get { return tmrUpdateState.Enabled; }
            set
            {
                if (value == false)
                {
                    tmrUpdateState.Enabled = value;
                }
                else
                {
                    if (m_ServoUnits.Count > 0)
                    {
                        tmrUpdateState.Enabled = value;
                    }
                }
            }
        }
        public bool UseCompareHomeInfo
        {
            get { return m_UseCompareHomeInfo; }
            set { m_UseCompareHomeInfo = value; }
        }
        public bool UseComparePointInfo
        {
            get { return m_UseComparePointInfo; }
            set { m_UseComparePointInfo = value; }
        }
        #endregion

        #region Contructor
        public ServoTabMp2300Ctrl()
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
        public void Initialize(ServoUnitMp2300s servoUnits)
        {
            m_Client = ClientManager.Instance;

            m_ServoUnits = servoUnits;

            if (m_ServoUnits.Count == 0)
            {
                splitContainer1.Panel2.Enabled = false;
                tmrUpdateState.Enabled = false;
            }
            else
            {
                m_CurrentServoUnit = ServoUnits[0];
                m_Axes = CurrentServoUnit.Axis;
                m_CurrentAxis = Axes[0];
                lblSelectedAxis.Text = m_CurrentAxis.Name;

                CompareData();

                InitServoUnitTree();
                InitTeachPointList();
                InitAxesList();

                tmrUpdateState.Enabled = true;
            }

            m_TeachPointListHeight = this.listTeachPoint.Height;
            InitViewObject();
            m_MapControl = new ServoTabMp2300MapDataCtrl();

            m_Initialized = true;
        }

        private bool CompareData()
        {
            Dms.Ctl.MP2300Ctl mp2300 = Dms.Ctl.MP2300Ctl.Instance;
            bool bEqual = true;
            DialogResult result;

            if (m_UseCompareHomeInfo)
            {
                ushort[] mpbuf = mp2300.GetHomeInfo();
                ushort[] localBuf = ServoMotorMp2300.HomeInfoBuf;

                for (int i = 0; i < Dms.Ctl.MP2300Ctl.ORG_NUM; i++)
                {
                    if (mpbuf[i] != localBuf[i])
                    {
                        bEqual = false;
                        break;
                    }
                }

                if (!bEqual)
                {
                    if ((result = MessageBox.Show("Compare Home Information : Not Equal!!  \n Yes : MP2300 --> DICS \n No : DICS --> MP2300", "Compare Home Info", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Yes)
                    {
                        //mpbuf의 내용으로 ComponentContainer.xml의 내용을 바꾸어 주어야 함.
                        //mp2300.SetHomeInfo(mpbuf);
                    }
                    else if (result == DialogResult.No)
                    {
                        mp2300.SetHomeInfo(localBuf);
                    }
                }
            }
            else
            {
                mp2300.SetHomeInfo(ServoMotorMp2300.HomeInfoBuf);
            }

            bEqual = true;
            if (m_UseComparePointInfo)
            {
                ushort[][] mpbuf = mp2300.GetPointInfo();
                ushort[][] localBuf = ServoUnitMp2300.PointInfoBuf;

                for (int i = 0; i < Dms.Ctl.MP2300Ctl.POINT_NUM; i++)
                {
                    if ((mpbuf[0][i] != localBuf[0][i]) || (mpbuf[1][i] != localBuf[1][i]))
                    {
                        bEqual = false;
                        break;
                    }
                }

                if (!bEqual)
                {
                    if ((result = MessageBox.Show("Compare Teaching Point Information : Not Equal!!  \n Yes : MP2300 --> DICS \n No : DICS --> MP2300", "Compare Home Info", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Yes)
                    {
                        //mpbuf의 내용으로 .pos파일과 ComponentContainer.xml의 내용을 바꾸어 주어야 함.
                        //mp2300.SetPointInfo(mpbuf);
                    }
                    else if (result == DialogResult.No)
                    {
                        mp2300.SetPointInfo(localBuf);
                    }
                }
            }
            else
            {
                mp2300.SetPointInfo(ServoUnitMp2300.PointInfoBuf);
            }

            return true;
        }

        private void InitServoUnitTree()
        {
            treeViewServo.BeginUpdate();

            int unitCount = m_ServoUnits.Count;
            for (int i = 0; i < unitCount; i++)
            {
                TreeNode nodeUnit = new TreeNode();
                ServoUnitMp2300 unit = m_ServoUnits[i];
                int axisCount = unit.AxisCount;
                for (int j = 0; j < axisCount; j++)
                {
                    TreeNode nodeAxis = new TreeNode();
                    ServoMotorMp2300 axis = unit.Axis[j];
                    nodeAxis.Tag = axis;
                    nodeAxis.Text = axis.Name;
                    nodeUnit.Nodes.Add(nodeAxis);
                }

                nodeUnit.Tag = unit;
                nodeUnit.Text = unit.Name;

                treeViewServo.Nodes.Add(nodeUnit);
            }
            treeViewServo.ExpandAll();
            treeViewServo.EndUpdate();
        }

        public void InitViewObject()
        {
            // Operation Buttons
            btnEstop.Enabled = true;
            btnServoOn.Enabled = false;
            btnHome.Enabled = false;
            btnMove.Enabled = false;
            btnRepeat.Enabled = false;
            btnJogMinus.Enabled = false;
            btnJogPlus.Enabled = false;
            // Teaching point buttons
            btnSave.Enabled = true;
            btnSend.Enabled = true;
            btnRead.Enabled = true;

            txtJogVel.ReadOnly = true;
            txtVelRatio.ReadOnly = true;

            if (m_ServoUnits.Count > 0)
            {
                lblCurrentUnitName.Text = CurrentServoUnit.Name;

                int velRatio = (int)(m_CurrentServoUnit.VelRatio * 100);
                txtVelRatio.Text = string.Format("{0:d}", velRatio);
                trackBarVelRatio.Value = velRatio;

                double jogVel = m_CurrentAxis.MinJogVelocity;
                txtJogVel.Text = string.Format("{0:f1}", jogVel);
                trackBarJogVel.Value = CalcJogVelRatio(jogVel);

                txtJogVel.LimitHigh = m_CurrentAxis.MaxJogVelocity.ToString();
                txtJogVel.LimitLow = m_CurrentAxis.MinJogVelocity.ToString();

                double waitTime = ((double)m_CurrentServoUnit.RepeatWaitTime / 1000.0);
                txtRepeatWaitTime.Text = waitTime.ToString();

                this.groupBoxFindHome.Visible = (CurrentAxis.AxisType == AxisType.RbGap);
                if (this.groupBoxFindHome.Visible)
                {
                    this.listTeachPoint.Height = m_TeachPointListHeight / 2;
                }
                else
                {
                    this.listTeachPoint.Height = m_TeachPointListHeight;
                }
            }
        }

        private int CalcJogVelRatio(double velocity)
        {
            double min = m_CurrentAxis.MinJogVelocity;
            double max = m_CurrentAxis.MaxJogVelocity;

            if (velocity > max) velocity = max;
            if (velocity < min) velocity = min;

            int ratio = (int)((velocity / (double)max) * 100);

            return ratio;
        }

        private double CalcJogVel(int ratio)
        {
            double min = m_CurrentAxis.MinJogVelocity;
            double max = m_CurrentAxis.MaxJogVelocity;
            double velocity = max * ((double)ratio / 100.0);

            if (velocity > max) velocity = max;
            if (velocity < min) velocity = min;

            return velocity;
        }

        private void InitTeachPointList()
        {
            listTeachPoint.Columns.Clear();
            ColumnHeader[] colHeaders = new ColumnHeader[Axes.Count + 2];
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
                colHeader.Text = Axes[i].Name;
                colHeader.TextAlign = HorizontalAlignment.Right;
                colHeader.Width = 120;
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
            int pointCount = CurrentServoUnit.TeachPoints;
            int axisCount = CurrentServoUnit.AxisCount;
            for (short id = 0; id < pointCount; id++)
            {
                ListViewItem lvItem = new ListViewItem((id + 1).ToString());
                for (short axisId = 0; axisId < axisCount; axisId++)
                {
                    tmp = string.Format("{0:F2}", CurrentServoUnit.GetTeachPointPos(id, axisId));
                    lvItem.SubItems.Add(tmp);
                }
                lvItem.SubItems.Add(CurrentServoUnit.GetPointName(id));
                listTeachPoint.Items.Add(lvItem);
            }
        }

        private void InitAxesList()
        {
            //DataGridViewColumnCollection columns = gridViewAxes.Columns;
            //columns[colSetPos.DisplayIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //columns[colCurPos.DisplayIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //columns[colCurAcc.DisplayIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //columns[colCurVel.DisplayIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //columns[colName.DisplayIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            int columnCount = gridViewAxes.Columns.Count;
            for (int i = 0; i < columnCount; i++)
            {
                if ((i <= colCurAcc.DisplayIndex) && (i >= colName.DisplayIndex))
                {
                    gridViewAxes.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    gridViewAxes.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

            SetAxesListData();
        }

        private void SetAxesListData()
        {
            gridViewAxes.Rows.Clear();
            try
            {
                int axisCount = Axes.Count;
                for (short i = 0; i < axisCount; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(gridViewAxes);
                    DataGridViewCellCollection cells = row.Cells;
                    cells[colNo.DisplayIndex].Value = i + 1;
                    cells[colName.DisplayIndex].Value = Axes[i].Name;
                    cells[colCurVel.DisplayIndex].Style.BackColor = Color.LightYellow;
                    cells[colCurAcc.DisplayIndex].Style.BackColor = Color.LightYellow;
                    gridViewAxes.Rows.Add(row);
                }

                UpdateAxesListData();

                gridViewAxes.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                gridViewAxes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void UpdateAxesListData()
        {
            try
            {
                int axisCount = Axes.Count;
                for (short i = 0; i < axisCount; i++)
                {
                    DataGridViewCellCollection cells = gridViewAxes.Rows[i].Cells;
                    //   cells[colSetPos.DisplayIndex].Value = string.Format("{0:F2}", CurrentServoUnit.GetTeachPointPos(Convert.ToInt16(m_SelectedPoint), 0));
                    cells[colCurPos.DisplayIndex].Value = string.Format("{0:F2}", Axes[i].GetPosition());
                    cells[colCurVel.DisplayIndex].Value = Axes[i].AxisVel;
                    cells[colCurAcc.DisplayIndex].Value = Axes[i].AxisAcc;
                    cells[colNeg.DisplayIndex].Style.BackColor = Axes[i].GetNegSwitch() ? Color.Red : Color.White;
                    cells[colHome.DisplayIndex].Style.BackColor = Axes[i].GetHomeSwitch() ? Color.Lime : Color.White;
                    cells[colPos.DisplayIndex].Style.BackColor = Axes[i].GetPosSwitch() ? Color.Red : Color.White;
                    cells[colEvent.DisplayIndex].Value = (Axes[i].GetAxisState()).ToString();
                    cells[colSource.DisplayIndex].Value = (Axes[i].GetAxisSource()).ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void trackBarVelRatio_Scroll(object sender, EventArgs e)
        {
            txtVelRatio.Text = ((TrackBar)sender).Value.ToString();
        }

        private void trackBarJogVel_Scroll(object sender, EventArgs e)
        {
            //double newVel = (m_CurrentAxis.MaxJogVelocity - m_CurrentAxis.MinJogVelocity ) * (double)(((TrackBar)sender).Value) / 100.0 + m_CurrentAxis.MinJogVelocity;
            //txtJogVel.Text = string.Format("{0:f1}", newVel);

            txtJogVel.Text = string.Format("{0:f1}", CalcJogVel(((TrackBar)sender).Value));
        }

        private void txtVelRatio_TextChanged(object sender, EventArgs e)
        {
            trackBarVelRatio.Value = Convert.ToInt32(txtVelRatio.Text);
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.ChangeVelRatio, m_CurrentServoUnit, txtVelRatio.Text);
        }

        private void txtJogVel_TextChanged(object sender, EventArgs e)
        {
            double dValue = Convert.ToDouble(txtJogVel.Text);
            trackBarJogVel.Value = CalcJogVelRatio(dValue);
        }

        /// <summary>
        /// Change current Servo Unit - eun 20080122
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewServo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                m_Mutex.WaitOne();
                {
                    TreeNode curNode = e.Node;
                    if (curNode.Level == (int)ObjLevel.ServoUnit)
                    {
                        CurrentServoUnit = ServoUnits[curNode.Index];
                        Axes = CurrentServoUnit.Axis;
                        CurrentAxis = Axes[0];
                        lblCurrentUnitName.Text = CurrentServoUnit.Name;
                        lblSelectedAxis.Text = CurrentAxis.Name;
                        InitTeachPointList();
                        SetAxesListData();
                    }
                    else if (curNode.Level == (int)ObjLevel.ServoMotor)
                    {
                        CurrentServoUnit = ServoUnits[curNode.Parent.Index];
                        Axes = CurrentServoUnit.Axis;
                        CurrentAxis = Axes[curNode.Index];
                        lblCurrentUnitName.Text = CurrentServoUnit.Name;
                        lblSelectedAxis.Text = CurrentAxis.Name;
                        InitTeachPointList();
                        SetAxesListData();
                    }

                    InitViewObject();
                }
                m_Mutex.ReleaseMutex();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            short selectedId = GetSelctedPointId();
            if (selectedId < 0)
            {
                MessageBox.Show("Please select target point!");
                return;
            }


            if (DialogResult.Yes == MessageBox.Show("Set current position to Point ?", "Teaching", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.PosSend, m_CurrentServoUnit, m_SelectedPoint);
                SetTeachPointListData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Save to storage ?", "Teaching", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.PosSave, m_CurrentServoUnit);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Read from storage ?", "Teaching", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.PosRead, m_CurrentServoUnit);

                SetTeachPointListData();
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            short selectedId = GetSelctedPointId();
            if (selectedId < 0)
            {
                MessageBox.Show("Please select target point!");
                return;
            }

            if (m_CurrentServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.Move, m_CurrentServoUnit, m_SelectedPoint);
            }
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

                if (m_CurrentServoUnit.IsInterlockCondition())
                {
                    MessageBox.Show("Can not move : Interlock Condition");
                }
                else
                {
                    m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.RepeatStart, m_CurrentServoUnit, m_SelectedPoint);
                }
            }
            else
            {
                btnRepeat.Text = "REPEAT";

                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.RepeatStop, m_CurrentServoUnit);
            }
        }

        private void btnServoOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.ServoOn, m_CurrentServoUnit);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (m_CurrentServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.Home, m_CurrentServoUnit);
            }
        }

        private void btnEstop_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.Estop, m_CurrentServoUnit);
        }

        private void gridViewAxes_CurrentCellChanged(object sender, EventArgs e)
        {
            //jemoon : 090610 tree view click 했을때와 동기화 하기가 쉽지 않음
            //try
            //{
            //    if (gridViewAxes.CurrentCellAddress.X < 0 || gridViewAxes.CurrentCellAddress.Y < 0) return;

            //    if (m_OldRowIndex != -1)
            //    {
            //        gridViewAxes.InvalidateRow(m_OldRowIndex);
            //    }
            //    m_OldRowIndex = gridViewAxes.CurrentCellAddress.Y;
            //    lblSelectedAxis.Text = gridViewAxes[colName.DisplayIndex, m_OldRowIndex].Value.ToString();
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.ToString());
            //}
        }
        #endregion

        private void btnJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentAxis == null)
            {
                MessageBox.Show("Please select axis!");
                return;
            }

            // Jog도 인터락을 체크 하는지 ? 안한다면 아래 조건 Skip하도록
            if (m_CurrentServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.JogPlusStart, m_CurrentAxis, txtJogVel.Text);
            }
        }

        private void btnJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.JogPlusStop, m_CurrentAxis);
        }

        private void btnJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentAxis == null)
            {
                MessageBox.Show("Please select axis!");
                return;
            }

            // Jog도 인터락을 체크 하는지 ? 안한다면 아래 조건 Skip하도록
            if (m_CurrentServoUnit.IsInterlockCondition())
            {
                MessageBox.Show("Can not move : Interlock Condition");
            }
            else
            {
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.JogMinusStart, m_CurrentAxis, txtJogVel.Text);
            }
        }

        private void btnJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.JogMinusStop, m_CurrentAxis);
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

        private void listTeachPoint_Click(object sender, EventArgs e)
        {
            short pointId = GetSelctedPointId();
            RbtPos pos = CurrentServoUnit.GetTeachPointPos(pointId);

            m_SelectedPoint = pointId;
        }

        private void gridViewAxes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //int axisId = e.RowIndex;
            //int item = e.ColumnIndex;

            //if(item == (int)AxesListHeader.Cur_Vel) Axes[axisId].AxisVel = Convert.ToDouble(gridViewAxes.Rows[axisId].Cells[item].Value.ToString());
            //if(item == (int)AxesListHeader.Cur_Acc) Axes[axisId].AxisAcc = Convert.ToInt16(gridViewAxes.Rows[axisId].Cells[item].Value.ToString());

            //CurrentServoUnit.SaveVelToFile();
        }

        private void gridViewAxes_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }


        //private void gridViewAxes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}

        private void gridViewAxes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // index == -1 : Header
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            // column index == 0 : Axis Number
            if (e.ColumnIndex == 0) return;

            {
                bool changed = false;
                int axisId = e.RowIndex;
                int item = e.ColumnIndex;

                if (item == colCurVel.DisplayIndex)
                {
                    KeyInValidation validation = new KeyInValidation();
                    KeyPadTextBox keyPad = new KeyPadTextBox(Axes[axisId].AxisVel.ToString());
                    validation.Format = OptionFormat.Float;
                    validation.Low = "0.0";
                    validation.High = (Axes[axisId].AxisType == AxisType.RbGap) ? "1.5" : "";
                    keyPad.Validation = validation;

                    if (keyPad.ShowDialog() == DialogResult.OK)
                    {
                        gridViewAxes.Rows[axisId].Cells[item].Value = keyPad.NewValue;
                        Axes[axisId].AxisVel = Convert.ToDouble(keyPad.NewValue);
                        changed = true;
                    }
                    //jemoon : 110607
                    //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
                    keyPad.Dispose();
                }
                else if (item == colCurAcc.DisplayIndex && m_Client.CurrentUserLevel == UserLevels.Administrator)
                {


                    KeyInValidation validation = new KeyInValidation();
                    KeyPadTextBox keyPad = new KeyPadTextBox(Axes[axisId].AxisAcc.ToString());
                    validation.Format = OptionFormat.Digit;
                    validation.Low = "50";
                    validation.High = Int16.MaxValue.ToString();
                    keyPad.Validation = validation;

                    if (keyPad.ShowDialog() == DialogResult.OK)
                    {
                        gridViewAxes.Rows[axisId].Cells[item].Value = keyPad.NewValue;
                        Axes[axisId].AxisAcc = Convert.ToInt16(keyPad.NewValue);
                        changed = true;
                    }
                    //jemoon : 110607
                    //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
                    keyPad.Dispose();
                }

                if (changed)
                {
                    CurrentServoUnit.SaveVelToFile();
                }
            }
        }

        private void gridViewAxes_SelectionChanged(object sender, EventArgs e)
        {
            this.gridViewAxes.ClearSelection();
        }

        private void buttonAllEstop_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.EstopAll);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (!m_Initialized) return;

            bool permission = true;
            permission &= !m_Client.GenInfos.AutoMode;
            permission &= (m_Client.CurrentUserLevel >= UserLevels.Technician);
            this.Enabled = permission;

            if (m_CurrentServoUnit.ServerManager.Initialized &&
                m_CurrentServoUnit.Initialized)
            {
                m_Mutex.WaitOne();
                {
                    UpdateCurrentServoStatus();
                    UpdateCurrentPoint();
                    UpdateStatusServoOperationButtons();
                    UpdateManualCommand();
                    UpdateMassage();
                }
                m_Mutex.ReleaseMutex();
            }
        }

        private void UpdateCurrentPoint()
        {
            string curPoint = CurrentServoUnit.GetPointName((short)(CurrentServoUnit.GetCurPointId()));
            if (lblCurrentPos.Text != curPoint) lblCurrentPos.Text = "  " + curPoint;
        }

        private void UpdateCurrentServoStatus()
        {
            bool change = true;

            //jemoon :Tab하나에 여러개를 처리 하다 보니 아래 코드는 개선하기 전까지 의미가 없음
            //foreach (ServoMotor axis in CurrentServoUnit.Axis)
            //{
            //    double curPosition = axis.GetPosition();
            //    if (curPosition != oldPosition)
            //    {
            //        oldPosition = curPosition;
            //        change = true;
            //    }

            //    string curEvent = axis.GetAxisState().ToString();
            //    if (curEvent != oldEvent)
            //    {
            //        oldEvent = curEvent;
            //        change = true;
            //    }

            //    string curSource = axis.GetAxisSource().ToString();
            //    if (curSource != oldSource)
            //    {
            //        oldSource = curSource;
            //        change = true;
            //    }

            //}

            if (change) UpdateAxesListData();
        }

        private void UpdateStatusServoOperationButtons()
        {
            // Servo Status
            bool servoOn = m_CurrentServoUnit.Ready;
            bool servoHome = m_CurrentServoUnit.HomeComp;
            bool servoMoving = m_CurrentServoUnit.ManualMoving;

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
            btnRepeat.Enabled = servoOn && servoHome && (!servoMoving || GetManualCommand() == RbtAction.MoveRepeat);
            btnJogPlus.Enabled = servoOn && !servoMoving;
            btnJogMinus.Enabled = servoOn && !servoMoving;
            if (btnFindHome.Visible)
            {
                btnFindHome.Enabled = servoOn && !servoMoving;
            }
            if (btnSaveFindHome.Visible)
            {
                btnSaveFindHome.Enabled = servoOn && !servoMoving;
            }

            // Teaching
            bool ok = (GetSelctedPointId() >= 0) && !servoMoving;
            btnSend.Enabled = ok;
            btnSave.Enabled = ok;
            btnRead.Enabled = ok;
            listTeachPoint.Enabled = !servoMoving;
        }

        private RbtAction GetManualCommand()
        {
            RbtAction act = (RbtAction)m_CurrentServoUnit.ManualActionCmd;
            if (act >= RbtAction.MoveRepeat && act < RbtAction.EndOfActionCode)
            {
                act = RbtAction.MoveRepeat;
            }

            return act;
        }

        private void UpdateManualCommand()
        {
            RbtAction act = GetManualCommand();
            if (act == RbtAction.MoveRepeat)
            {
                btnRepeat.Text = "STOP";
            }
            else
            {
                btnRepeat.Text = "REPEAT";
            }

            lblCommand.Text = "  " + act.ToString();
        }

        private void UpdateMassage()
        {
            lblMessage.Text = "  " + m_CurrentServoUnit.Message;
            lblComStatus.Text = (m_CurrentAxis.IsCommConnected ? "Connected" : "Not Connected");

            if (txtFindHome.Visible)
            {
                txtFindHome.Text = string.Format("{0:F4}", m_CurrentServoUnit.Theta);
            }
        }

        private void txtRepeatWaitTime_TextChanged(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                int waitTime = (int)(Convert.ToDouble(txtRepeatWaitTime.Text) * 1000);
                m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.ChangeRepeatWaitTime, m_CurrentServoUnit, waitTime);
            }
        }

        private void btnMapData_Click(object sender, EventArgs e)
        {
            Form mapForm = new Form();
            mapForm.Size = new Size(m_MapControl.Width, m_MapControl.Height + 20);
            if (m_MapControl.IsDisposed)
            {
                m_MapControl = new ServoTabMp2300MapDataCtrl();
            }
            mapForm.Controls.Add(m_MapControl);
            mapForm.Load += new EventHandler(mapForm_Load);
            mapForm.Show();
        }

        void mapForm_Load(object sender, EventArgs e)
        {
            m_MapControl.Initialize();
            m_MapControl.SetMonitorTimer(true);
        }

        private void btnFindHome_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.FindHome, m_CurrentServoUnit);
        }

        private void btnSaveFindHome_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ServoMP2300Manaul, ServoManualAct.SaveFindHome, m_CurrentServoUnit);
        }
    }
}
