using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;
using Dms.Device;

namespace Dms.Control
{
    public partial class ViewEyeEuv : UserControl
    {
        #region enum
        private enum EuvDelay
        {
            DeStart,
            DeEnd
        }
        private enum EuvBtn
        {
            BtnReset,
        }
        #endregion
        #region Fields
        private Dms.Device.EyeEuvUnit m_Euv = null;
        private ClientManager m_Client;
        private bool m_Initialized = false;
        private EuvDelay[] Euvreset = { EuvDelay.DeEnd, EuvDelay.DeEnd, EuvDelay.DeEnd };
        private UInt32[] m_EuvTicks = new UInt32[1];
        private CheckBox[] btnCheck = new CheckBox[1]; 
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ViewEyeEuv()
        {
           InitializeComponent();

           this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
           this.SetStyle(ControlStyles.UserPaint, true);
           this.SetStyle(ControlStyles.CacheText, true);
           this.SetStyle(ControlStyles.DoubleBuffer, true);
           this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        public void Initialize(Dms.Device.EyeEuvUnit euv)
        {
            m_Euv = euv;
            m_Client = ClientManager.Instance;
        //    InitGrid();
            m_Initialized = true;
            tmrUpdateState.Enabled = true;
            InitView();
        }

        private void InitView()
        {
            SetTags();
            UpdateState();
            tmrUpdateState.Enabled = true;
        }

        private void SetTags()
        {
            checkRemoteIn.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiRemote;
            checkReadyIn.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiReady;
            checkLamp1OkIn.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiLamp1Ok;
            checkLamp2OkIn.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiLamp2Ok;
            checkLamp1TimeOverIn.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiLamp1TimeOver;
            checkLamp2TimeOverIn.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiLamp2TimeOver;
            checkTrouble1.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiTroubleInside;
            checkTrouble2.Tag = (Dms.Device.IoDigitalInput)m_Euv.DiTroubleOutside;

            checkRemoteOut.Tag = Common.EyeEuvAct.REMOTE;
            checkStanby.Tag = Common.EyeEuvAct.STANBY;
            checkLamp1On.Tag =Common.EyeEuvAct.LAMP1ON;
            checkLamp2On.Tag = Common.EyeEuvAct.LAMP2ON;
            checkWaterFlowOk.Tag = Common.EyeEuvAct.WATERFLOWOK;
            checkWaterLeakOk.Tag = Common.EyeEuvAct.WATERLEAKOK;
            checkLampCoolingCdaOk.Tag = Common.EyeEuvAct.COLLINGCDAOK;
            checkReset.Tag = Common.EyeEuvAct.RESET;
            checkEmergencyStop.Tag = Common.EyeEuvAct.EMOGENCYSTOP;
            checkCDAValve.Tag = Common.EyeEuvAct.CdaInValve;
            checkN2Valve.Tag = Common.EyeEuvAct.N2InValve;
            checkPcwInValve.Tag = Common.EyeEuvAct.PcwInValve;
            checkPcwOutValve.Tag = Common.EyeEuvAct.PcwOutValve;
            btnCylUp.Tag = Common.EyeEuvAct.EuvUpdnUp;
            btnCylDown.Tag = Common.EyeEuvAct.EuvUpdnDn;
        }

        private void UpdateState()
        {
            chkUp.Checked = m_Euv.ActuatorUnit.IsPositive();
            chkDown.Checked = m_Euv.ActuatorUnit.IsNegative();

            if (m_Euv.Lamps[0].IsOff()) pbLamp1Status.BackColor = Color.DarkGray;
            else if (m_Euv.Lamps[0].IsOn()) pbLamp1Status.BackColor = Color.Green;

            if (m_Euv.Lamps[1].IsOff()) pbLamp2Status.BackColor = Color.DarkGray;
            else if (m_Euv.Lamps[1].IsOn()) pbLamp2Status.BackColor = Color.Green;

            foreach (CheckBox box in gbEuvStatus.Controls)
            {
                box.Checked = ((Dms.Device.IoDigitalInput)box.Tag).GetState();
            }

            foreach (CheckBox box in gbAlarmStatus.Controls)
            {
                box.Checked = ((Dms.Device.IoDigitalInput)box.Tag).GetState();
            }


            if (m_Euv.N2Valve != null &&
                m_Euv.N2Valve.IsOpen())
            {
                checkN2Valve.Checked = true;
                checkN2Valve.Text = "N2 In Close";
            }
            else if (m_Euv.N2Valve != null &&
                     m_Euv.N2Valve.IsClose())
            {
                checkN2Valve.Checked = false;
                checkN2Valve.Text = "N2 In Open";
            }
            if (m_Euv.CDAValve != null &&
                m_Euv.CDAValve.IsOpen())
            {
                checkCDAValve.Checked = true;
                checkCDAValve.Text = "CDA In Close";
            }
            else if (m_Euv.CDAValve != null &&
                     m_Euv.CDAValve.IsClose())
            {
                checkCDAValve.Checked = false;
                checkCDAValve.Text = "CDA In Open";
            }
            if (m_Euv.PCWInValve != null &&
                m_Euv.PCWInValve.IsOpen())
            {
                checkPcwInValve.Checked = true;
                checkPcwInValve.Text = "PCW In Close";
            }
            else if (m_Euv.PCWInValve != null &&
                     m_Euv.PCWInValve.IsClose())
            {
                checkPcwInValve.Checked = false;
                checkPcwInValve.Text = "PCW In Open";
            }
            if (m_Euv.PCWOutValve != null &&
                m_Euv.PCWOutValve.IsOpen())
            {
                checkPcwOutValve.Checked = true;
                checkPcwOutValve.Text = "PCW Out Close";
            }
            else if (m_Euv.PCWOutValve != null &&
                    m_Euv.PCWOutValve.IsClose())
            {
                checkPcwOutValve.Checked = false;
                checkPcwOutValve.Text = "PCW Out Open";
            }
            if (m_Euv.DoLamp1ON.GetState())
            {
                checkLamp1On.Checked = true;
                checkLamp1On.Text = "LAMP OFF";
            }
            else
            {
                checkLamp1On.Checked = false;
                checkLamp1On.Text = "LAMP ON";
            }
            if (m_Euv.DoLamp2ON.GetState())
            {
                checkLamp2On.Checked = true;
                checkLamp2On.Text = "LAMP OFF";
            }
            else
            {
                checkLamp2On.Checked = false;
                checkLamp2On.Text = "LAMP ON";
            }
            if (m_Euv.DoRemote.GetState())
            {
                checkRemoteOut.Checked = true;
            }
            else
            {
                checkRemoteOut.Checked = false;
            }
            if (m_Euv.DoStandby.GetState())
            {
                checkStanby.Checked = true;
            }
            else
            {
                checkStanby.Checked = false;
            }
            if (m_Euv.DoWaterFlowOK.GetState())
            {
                checkWaterFlowOk.Checked = true;
            }
            else
            {
                checkWaterFlowOk.Checked = false;
            }
            if (m_Euv.DoWaterLeakOK.GetState())
            {
                checkWaterLeakOk.Checked = true;
            }
            else
            {
                checkWaterLeakOk.Checked = false;
            }
            if (m_Euv.DoLampCoolCDAOK.GetState())
            {
                checkLampCoolingCdaOk.Checked = true;
            }
            else
            {
                checkLampCoolingCdaOk.Checked = false;
            }
            if (m_Euv.DoEmergencyStop.GetState())
            {
                checkEmergencyStop.Checked = true;
            }
            else
            {
                checkEmergencyStop.Checked = false;
            }
            if (m_Euv.DoReset.GetState())
            {
                checkReset.Checked = true;
            }
            else
            {
                checkReset.Checked = false;
            }
        }

        private void HouseUpdnClick(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            m_Client.SendCommand(Dms.Common.Command.EyeEuvManual, m_Euv, button.Tag, true);
        }

        private void HouseUpdnRelease(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            m_Client.SendCommand(Dms.Common.Command.EyeEuvManual, m_Euv, button.Tag, false);
        }

        private void Check_Click(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            bool bOn = false;
            if (check.Name == checkRemoteOut.Name)
            {
                bOn = m_Euv.DoRemote.GetState();
            }
            else if (check.Name == checkStanby.Name)
            {
                bOn = m_Euv.DoStandby.GetState();
            }
            else if (check.Name == checkWaterFlowOk.Name)
            {
                bOn = m_Euv.DoWaterFlowOK.GetState();
            }
            else if (check.Name == checkWaterLeakOk.Name)
            {
                bOn = m_Euv.DoWaterLeakOK.GetState();
            }
            else if (check.Name == checkLampCoolingCdaOk.Name)
            {
                bOn = m_Euv.DoLampCoolCDAOK.GetState();
            }
            else if (check.Name == checkEmergencyStop.Name)
            {
                bOn = m_Euv.DoEmergencyStop.GetState();
            }
            else if (check.Name == checkReset.Name)
            {
                bOn = m_Euv.DoReset.GetState();
                btnCheck[0] = checkReset;
                m_EuvTicks[0] = XFunc.GetTickCount();
                Euvreset[0] = EuvDelay.DeStart;
            }
            else if (check.Name == checkN2Valve.Name)
            {
                if (m_Euv.N2Valve != null)//2009.12.18 kimgun
                    bOn = m_Euv.N2Valve.IsOpen();
            }
            else if (check.Name == checkPcwInValve.Name)
            {
                if (m_Euv.PCWInValve != null)//2009.12.18 kimgun
                    bOn = m_Euv.PCWInValve.IsOpen();
            }
            else if (check.Name == checkPcwOutValve.Name)
            {
                if (m_Euv.PCWOutValve != null)//2009.12.18 kimgun
                    bOn = m_Euv.PCWOutValve.IsOpen();
            }
            else if (check.Name == checkCDAValve.Name)
            {
                if (m_Euv.CDAValve != null)//2009.12.18 kimgun
                    bOn = m_Euv.CDAValve.IsOpen();
            }
            else if (check.Name == checkLamp1On.Name)
            {
                bOn = m_Euv.DoLamp1ON.GetState();
            }
            else if (check.Name == checkLamp2On.Name)
            {
                bOn = m_Euv.DoLamp2ON.GetState();
            }
            m_Client.SendCommand(Dms.Common.Command.EyeEuvManual, m_Euv, check.Tag, !bOn);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                SetControls(!m_Client.GenInfos.AutoMode);   //Auto Mode일 경우 모든 Control을 Disable만듬.
                UpdateState();
            }

            for (int i = 0; i < 1; i++)
            {
                if ((XFunc.GetTickCount() - m_EuvTicks[i] > 1000) &&
               Euvreset[i] == EuvDelay.DeStart)
                {
                    if (btnCheck[i] != null)
                        m_Client.SendCommand(Dms.Common.Command.EyeEuvManual, m_Euv, btnCheck[i].Tag, false);
                    Euvreset[i] = EuvDelay.DeEnd;
                }
            }

        }

        private void SetControls(bool enable)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                control.Enabled = enable;
            }
        }
    }
}
