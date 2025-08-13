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
    public partial class ViewUshioEuv : UserControl
    {
        #region Enum

        private enum EuvUsedTimeHeader
        {
            Lamp, UsedTime, WarningSet
        }

        private enum LampIntensityHeader
        {
            Lamp, Intensity
        }
        private enum EuvDelay
        {
            DeStart,
            DeEnd
        }
        private enum EuvBtn
        {
            BtnUERT,
            btnUICR,
            btnULST
        }
        #endregion

        #region Fields
        private Dms.Device.UshioEuvUnit m_Euv = null;
        private ClientManager m_Client;
        private bool m_Initialized = false;
        private EuvDelay[] Euvreset = { EuvDelay.DeEnd, EuvDelay.DeEnd, EuvDelay.DeEnd };
        private uint[] m_EuvTicks = new uint[3];
        private CheckBox[] btnCheck = new CheckBox[3];
        #endregion

        #region Constructor
        public ViewUshioEuv()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);    //깜박임 방지
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }
        #endregion

        public void Initialize(Dms.Device.UshioEuvUnit euv)
        {

            m_Euv = euv;
            m_Client = ClientManager.Instance;
            InitGrid();
            m_Initialized = true;
            tmrUpdateState.Enabled = true;
            InitView();
        }

        private void InitView()
        {
            SetTags();
            UpdateState();

        }

        private void InitGrid()
        {

            DataGridViewTextBoxColumn colLampUsedTime = new DataGridViewTextBoxColumn();
            colLampUsedTime.HeaderText = EuvUsedTimeHeader.Lamp.ToString();
            colLampUsedTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataEuvUsedTimeGridView.Columns.Add(colLampUsedTime);

            DataGridViewTextBoxColumn colUsedTimeUsedTime = new DataGridViewTextBoxColumn();
            colUsedTimeUsedTime.HeaderText = EuvUsedTimeHeader.UsedTime.ToString();
            colUsedTimeUsedTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataEuvUsedTimeGridView.Columns.Add(colUsedTimeUsedTime);

            //DataGridViewTextBoxColumn colWarningSetUsedTime = new DataGridViewTextBoxColumn();
            //colWarningSetUsedTime.HeaderText = EuvUsedTimeHeader.WarningSet.ToString();
            //colWarningSetUsedTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dataEuvUsedTimeGridView.Columns.Add(colWarningSetUsedTime);

            foreach (DataGridViewColumn column in dataEuvUsedTimeGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dataEuvUsedTimeGridView.Rows.Add(m_Euv.Lamps.Count - 1);

            DataGridViewTextBoxColumn colLamp = new DataGridViewTextBoxColumn();
            colLamp.HeaderText = LampIntensityHeader.Lamp.ToString();
            colLamp.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataLampIntensityGridView.Columns.Add(colLamp);

            DataGridViewTextBoxColumn colLampIntensity = new DataGridViewTextBoxColumn();
            colLampIntensity.HeaderText = LampIntensityHeader.Intensity.ToString();
            colLampIntensity.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataLampIntensityGridView.Columns.Add(colLampIntensity);

            foreach (DataGridViewColumn column in dataLampIntensityGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dataLampIntensityGridView.Rows.Add(m_Euv.Lamps.Count - 1);

            Setdata();

            this.dataEuvUsedTimeGridView.AutoGenerateColumns = false;
            this.dataLampIntensityGridView.AutoGenerateColumns = false;

        }

        private void Setdata()
        {

            foreach (Dms.Device.EuvLamp item in m_Euv.Lamps)
            {
                this.dataEuvUsedTimeGridView.Rows[item.Id].SetValues(item.Name, item.GetLampUsedTime());

                this.dataLampIntensityGridView.Rows[item.Id].SetValues(item.Name, item.LampIntensity);
            }


        }

        private void SetTags()
        {
            chkLampTime.Tag = (int)euvalarmIndex.euvAlmLampUseTimeExcess;
            chkLampFail.Tag = (int)euvalarmIndex.euvAlmLampOnFail;
            chkProtective.Tag = (int)euvalarmIndex.euvAlmProtectiveFunction;
            chkCircuitAnomaly.Tag = (int)euvalarmIndex.euvAlmElectricCircuit;
            chkN2PressSurplus.Tag = (int)euvalarmIndex.euvAlmN2PressSurplus;
            chkLampTemp.Tag = (int)euvalarmIndex.euvAlmLampHouseTemp;
            chkLampOpen.Tag = (int)euvalarmIndex.euvAlmLampHouseOpen;
            chkTransTemp.Tag = (int)euvalarmIndex.euvAlmTransformerTemp;
            chkN2PressDrop.Tag = (int)euvalarmIndex.euvAlmN2PressDrop;
            chkElecCoverOpen.Tag = (int)euvalarmIndex.euvAlmElectricCompCover;
            chkControlAnomaly.Tag = (int)euvalarmIndex.euvAlmControlSystem;
            chkEMO.Tag = (int)euvalarmIndex.euvAlmEMO;

            if (m_Euv.GetType() == typeof(UshioEuvUnit_Io))
            {
                chkUNRE.Tag = ((UshioEuvUnit_Io)m_Euv).DiUNRE;
                chkUCRE.Tag = ((UshioEuvUnit_Io)m_Euv).DiUCRE;
                chkULAC.Tag = ((UshioEuvUnit_Io)m_Euv).DiULAC;
                chkURDY.Tag = ((UshioEuvUnit_Io)m_Euv).DiURDY;
            }
            if (m_Euv.GetType() == typeof(UshioEuvUnit_Ec))
            {
                chkUNRE.Tag = ((UshioEuvUnit_Ec)m_Euv).DiUNRE;
                chkUCRE.Tag = ((UshioEuvUnit_Ec)m_Euv).DiUCRE;
                chkULAC.Tag = ((UshioEuvUnit_Ec)m_Euv).DiULAC;
                chkURDY.Tag = ((UshioEuvUnit_Ec)m_Euv).DiURDY;
            }

            checkULON.Tag = Common.UshioEuvAct.ULON;
            checkUDLC.Tag = Common.UshioEuvAct.UDLC;
            checkUEMG.Tag = Common.UshioEuvAct.UEMG;
            checkUERT.Tag = Common.UshioEuvAct.UERT;
            checkUICR.Tag = Common.UshioEuvAct.UICR;
            checkULST.Tag = Common.UshioEuvAct.ULST;
            checkUTRE.Tag = Common.UshioEuvAct.UTRE;
            chkN2InOpen.Tag = Common.UshioEuvAct.N2InValve;
            chkCDAInOpen.Tag = Common.UshioEuvAct.CdaInValve;
            chkPCWInOpen.Tag = Common.UshioEuvAct.PcwInValve;
            chkPCWOutOpen.Tag = Common.UshioEuvAct.PcwOutValve;
            btnCylUp.Tag = Common.UshioEuvAct.EuvUpdnUp;
            btnCylDown.Tag = Common.UshioEuvAct.EuvUpdnDn;

        }

        private void UpdateState()
        {
            chkUp.Checked = m_Euv.ActuatorUnit.IsPositive();
            chkDown.Checked = m_Euv.ActuatorUnit.IsNegative();

            if (m_Euv.Lamps[0].IsOff()) pbLamp1Status.BackColor = Color.DarkGray;
            else if (m_Euv.Lamps[0].IsOn()) pbLamp1Status.BackColor = Color.Green;


            if (m_Euv.Lamps[1].IsOff()) pbLamp2Status.BackColor = Color.DarkGray;
            else if (m_Euv.Lamps[1].IsOn()) pbLamp2Status.BackColor = Color.Green;

            if (m_Euv.Lamps[2].IsOff()) pbLamp3Status.BackColor = Color.DarkGray;
            else if (m_Euv.Lamps[2].IsOn()) pbLamp3Status.BackColor = Color.Green;

            int value = m_Euv.GetEuvAlarm();
            foreach (CheckBox box in gbAlmStatus.Controls)
            {
                //Alarm이 여러개 동시 발생한 경우 대응 불가
                //box.Checked = (int)box.Tag == value ;
                int key = (int)box.Tag;
                box.Checked = ((key & value) > 0);	// 수정된 코드
            }

            foreach (CheckBox box in gbEuvStatus.Controls)
            {
                box.Checked = ((Dms.Device.IoDigitalInput)box.Tag).GetState();
            }

            if (m_Euv.N2Valve.IsOpen())
            {
                chkN2InOpen.Checked = true;
                chkN2InOpen.Text = "N2 IN Close";
            }
            else if (m_Euv.N2Valve.IsClose())
            {
                chkN2InOpen.Checked = false;
                chkN2InOpen.Text = "N2 IN Open";
            }

            if (m_Euv.PCWInValve.IsOpen())
            {
                chkPCWInOpen.Checked = true;
                chkPCWInOpen.Text = "PCW IN Close";
            }
            else if (m_Euv.PCWInValve.IsClose())
            {
                chkPCWInOpen.Checked = false;
                chkPCWInOpen.Text = "PCW IN Open";
            }

            if (m_Euv.PCWOutValve.IsOpen())
            {
                chkPCWOutOpen.Checked = true;
                chkPCWOutOpen.Text = "PCW OUT Close";
            }
            else if (m_Euv.PCWOutValve.IsClose())
            {
                chkPCWOutOpen.Checked = false;
                chkPCWOutOpen.Text = "PCW OUT Open";
            }

            if (m_Euv.CdaValve.IsOpen())
            {
                chkCDAInOpen.Checked = true;
                chkCDAInOpen.Text = "CDA IN Close";
            }
            else if (m_Euv.CdaValve.IsClose())
            {
                chkCDAInOpen.Checked = false;
                chkCDAInOpen.Text = "CDA IN Open";
            }

            if (m_Euv.GetLampOnState())
            //if (m_Euv.DoULON.GetState())
            {
                checkULON.Checked = true;
                checkULON.Text = "LAMP OFF";
            }
            else
            {
                checkULON.Checked = false;
                checkULON.Text = "LAMP ON";
            }
            checkUDLC.Checked = m_Euv.IsLampLocalLock();
            checkUEMG.Checked = m_Euv.IsLampEmergency();
            checkUERT.Checked = m_Euv.IsEmoReset();
            checkUICR.Checked = m_Euv.IsSetInitialCount();
            checkULST.Checked = m_Euv.IsLampUseTimeReset();
            checkUTRE.Checked = m_Euv.IsLampRemote();
        }
        private void button_clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string value = "";
            m_Client.SendCommand(Dms.Common.Command.UshioEuvManual, m_Euv, button.Tag, value);

        }
        private void SetControls(bool enable)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                control.Enabled = enable;
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                SetControls(!m_Client.GenInfos.AutoMode);   //Auto Mode일 경우 모든 Control을 Disable만듬.
                UpdateState();
                Setdata(); // 09.07.30 minhan
            }
            for (int i = 0; i < 3; i++)
            {
                if ((XFunc.GetTickCount() - m_EuvTicks[i] > 1000) &&
               Euvreset[i] == EuvDelay.DeStart)
                {
                    if (btnCheck[i] != null)
                        m_Client.SendCommand(Dms.Common.Command.UshioEuvManual, m_Euv, btnCheck[i].Tag, false);
                    Euvreset[i] = EuvDelay.DeEnd;
                }
            }


        }

        private void btnCylUp_Click(object sender, EventArgs e)
        {
            m_Euv.ActuatorUnit.SetPositiveAct();
        }

        private void btnCylDown_Click(object sender, EventArgs e)
        {
            m_Euv.ActuatorUnit.SetNegativeAct();
        }

        private void chkLamp1Select_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLamp1Select.Checked)
            {
                m_Euv.Lamps[0].LampOnSelect(true);
                if (m_Euv.GetLampOnState()) m_Euv.Lamps[0].On();
                //if (m_Euv.DoULON.GetState()) m_Euv.Lamps[0].On();
            }
            else if (!chkLamp1Select.Checked)
            {
                m_Euv.Lamps[0].LampOnSelect(false);
                m_Euv.Lamps[0].Off();
            }
        }

        private void chkLamp2Select_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLamp2Select.Checked)
            {
                m_Euv.Lamps[1].LampOnSelect(true);
                if (m_Euv.GetLampOnState()) m_Euv.Lamps[1].On();
                //if (m_Euv.DoULON.GetState()) m_Euv.Lamps[1].On();
            }
            else if (!chkLamp2Select.Checked)
            {
                m_Euv.Lamps[1].LampOnSelect(false);
                m_Euv.Lamps[1].Off();
            }
        }

        private void chkLamp3Select_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLamp3Select.Checked)
            {
                m_Euv.Lamps[2].LampOnSelect(true);
                if (m_Euv.GetLampOnState()) m_Euv.Lamps[2].On();
                //if (m_Euv.DoULON.GetState()) m_Euv.Lamps[2].On();
            }
            else if (!chkLamp3Select.Checked)
            {
                m_Euv.Lamps[2].LampOnSelect(false);
                m_Euv.Lamps[2].Off();
            }
        }
        private void check_click(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            bool bOn = false;
            if (check.Name == checkUDLC.Name)
            {
                bOn = m_Euv.IsLampLocalLock();
                //bOn = m_Euv.DoUDLC.GetState();
            }
            else if (check.Name == checkUEMG.Name)
            {
                bOn = m_Euv.IsLampEmergency();
                //bOn = m_Euv.DoUEMG.GetState();
            }
            else if (check.Name == checkUERT.Name)
            {
                bOn = m_Euv.IsEmoReset();
                btnCheck[0] = checkUERT;
                m_EuvTicks[0] = XFunc.GetTickCount();
                Euvreset[0] = EuvDelay.DeStart;
            }
            else if (check.Name == checkUICR.Name)
            {
                bOn = m_Euv.IsSetInitialCount();
                //bOn = m_Euv.DoUICR.GetState();
                btnCheck[1] = checkUICR;
                m_EuvTicks[1] = XFunc.GetTickCount();
                Euvreset[1] = EuvDelay.DeStart;
            }
            else if (check.Name == checkULST.Name)
            {
                bOn = m_Euv.IsLampUseTimeReset();
                btnCheck[2] = checkULST;
                m_EuvTicks[2] = XFunc.GetTickCount();
                Euvreset[2] = EuvDelay.DeStart;
            }
            else if (check.Name == checkUTRE.Name)
            {
                bOn = m_Euv.IsLampRemote();
                //bOn = m_Euv.DoUTRE.GetState();
            }
            else if (check.Name == checkULON.Name)
            {
                bOn = m_Euv.GetLampOnState();
                //bOn = m_Euv.DoULON.GetState();
            }
            else if (check.Name == chkN2InOpen.Name)
            {
                bOn = m_Euv.N2Valve.IsOpen();
            }
            else if (check.Name == chkPCWInOpen.Name)
            {
                bOn = m_Euv.PCWInValve.IsOpen();
            }
            else if (check.Name == chkPCWOutOpen.Name)
            {
                bOn = m_Euv.PCWOutValve.IsOpen();
            }
            else if (check.Name == chkCDAInOpen.Name)
            {
                bOn = m_Euv.CdaValve.IsOpen();
            }
            m_Client.SendCommand(Dms.Common.Command.UshioEuvManual, m_Euv, check.Tag, !bOn);
        }

        private void HouseUpdnClick(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            m_Client.SendCommand(Dms.Common.Command.UshioEuvManual, m_Euv, button.Tag, true);
        }

        private void HouseUpdnRelease(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            m_Client.SendCommand(Dms.Common.Command.UshioEuvManual, m_Euv, button.Tag, false);
        }
    }
}
