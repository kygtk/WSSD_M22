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
using Dms.Server;

namespace Dms.Control
{
	public partial class ViewYaskawaNX100 : UserControl
	{
		#region Enum
        public enum RobotInteraction
        { 
            Get,
            Put,
        }
		#endregion
 
		#region Fields
		private YaskawaNX100 m_Robot = null;
        private _GenericCollection<PortUnit> m_PortUnit = new _GenericCollection<PortUnit>();
		private ClientManager m_Client = null;
		private bool m_Initialized = false;

        private DlgNX100InterLock m_DlgManualInterlock;

        int				m_PatternOfOperationNo = 0;
		int				m_PatternSlotNo;
		nxOP_PATTERN	m_PatternOperation;
		nxOP_HAND		m_PatternHand;
		nxOP_CUBE		m_PatternCube;

        nxOP_PATTERN    m_PatternOldGridDisplay;   // For Display Grid Control about DeviceIo and IfStep
        nxOP_CUBE       m_PatternOldCubeDisplay;

		nxOP_HAND[]		Hands =			{ nxOP_HAND.UPPER_HAND, nxOP_HAND.LOWER_HAND, };
		nxOP_PATTERN[]	Operations =	{ nxOP_PATTERN.GET_PREPARE, nxOP_PATTERN.PUT_PREPARE, nxOP_PATTERN.GET, nxOP_PATTERN.PUT, 
										  nxOP_PATTERN.EXCHANGE, nxOP_PATTERN.Y_ALIGN, nxOP_PATTERN.VCR_READ, };
		nxOP_CUBE[]		Cubes =			{ nxOP_CUBE.PORT1, nxOP_CUBE.PORT2, nxOP_CUBE.PORT3, nxOP_CUBE.PORT4, 
										  nxOP_CUBE.PORT5, nxOP_CUBE.PORT6, nxOP_CUBE.PORT7, nxOP_CUBE.PORT8, 
										  nxOP_CUBE.STAGE1, nxOP_CUBE.STAGE2, nxOP_CUBE.STAGE3, nxOP_CUBE.STAGE4, };
		#endregion

		#region Constructor
		public ViewYaskawaNX100()
		{
			InitializeComponent();
			
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);    //깜박임 방지
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.CacheText, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
		#endregion

        public void Initialize(Dms.Device.YaskawaNX100 robot)
		{
			m_Robot = robot;
			m_Client = ClientManager.Instance;
			m_Initialized = true;
			tmrUpdateState.Enabled = true;

            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;

            foreach(IGenericCollection collection in components.Items)
            {
                if(collection.ContainedItemType == m_PortUnit.ContainedItemType)
                {
                    for(int i = 0; i < collection.Count; i++)
                    {
                        m_PortUnit.Add(collection.Items[i]);
                    }
                }
            }

            InitControlTags();
            InitComboBoxItems();
            InitGridRobotStatusIo();
            InitGridRobotInterfaceIo();
		}

        private void InitControlTags()
        {
            // Set ComboBox Tag
            cboSelectSlot.Tag = "Slot Selection";
            cboSelectCube.Tag = "Cube Enterance Prohibition";
            cboSelectHand.Tag = "Hand Selection";
            cboOperation.Tag = "Pattern of Operation";

            // Set Grid Control Tag
            ifStepsExchange.Tag = nxOP_PATTERN.EXCHANGE.ToString();
            ifStepsGet.Tag = nxOP_PATTERN.GET.ToString();
            ifStepsPut.Tag = nxOP_PATTERN.PUT.ToString();
            //ifStepsHome.Tag = "null";

            // Set CheckBox Tag
            chkCubeInterfere.Tag = "Interference CUBE #";
            chkCubeProhibition.Tag = "Prohibition CUBE #";
            chkCubePassable.Tag = "Passable CUBE #";
        }
        
        private void InitComboBoxItems()
		{
			// ComboBox Item Setting - Slot List
			for(int i = 1; i <= m_Robot.MaxSlotNo; i++)
			{
				cboSelectSlot.Items.Add(i.ToString());
			}

			// ComboBox Item Setting - Port & Stage List
			foreach(string portName in m_Robot.UsedPortName)
			{
                cboSelectCube.Items.Add(portName);
			}
			foreach(string stageName in m_Robot.UsedStageName)
			{
				cboSelectCube.Items.Add(stageName);
			}
		
			// ComboBox Item Setting - Hand List
			foreach(nxOP_HAND hand in Hands)
			{
				cboSelectHand.Items.Add(hand.ToString());
			}

            // ComboBox Item Setting - Operation List
            ////
            foreach(nxOP_PATTERN op in m_Robot.UsedManualPattern)
            {
                cboOperation.Items.Add(op.ToString());
            }
            ////
            //foreach(nxOP_PATTERN op in Operations)
            //{
            //    cboOperation.Items.Add(op.ToString());
            //}
		}

        private void InitGridRobotStatusIo()
        {
            // Data Binding - viewDeviceIoStatus1
            viewDeviceIoStatus1.AddDeviceIo(m_Robot.DiRemoteModeSelect, "Remote Mode Select");
            viewDeviceIoStatus1.AddDeviceIo(m_Robot.DiPlayModeSelect, "Play Mode Select");
            viewDeviceIoStatus1.AddDeviceIo(m_Robot.DiTeachModeSelect, "Teach Mode Select");
            viewDeviceIoStatus1.AddDeviceIo(m_Robot.DiTeachLockSet, "TEACH-LOCKSET");
            // Data Binding - viewDeviceIoStatus2
            viewDeviceIoStatus2.AddDeviceIo(m_Robot.DiRunning, "Runnning");
            viewDeviceIoStatus2.AddDeviceIo(m_Robot.DiServoOn, "Servo is On");
            viewDeviceIoStatus2.AddDeviceIo(m_Robot.DiOperationOriginPoint, "Origin Point");
            viewDeviceIoStatus2.AddDeviceIo(m_Robot.DiHomePositionReturn, "Home Pos Return");
            // Data Binding - viewDeviceIoStatus3
            viewDeviceIoStatus3.AddDeviceIo(m_Robot.DiRobotHold, "ROBOT HOLD");
            viewDeviceIoStatus3.AddDeviceIo(m_Robot.DiPowerOn, "Control Power On");
            viewDeviceIoStatus3.AddDeviceIo(m_Robot.DiSafetyPlug, "Safety Plug");
            viewDeviceIoStatus3.AddDeviceIo(m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation, "Prepare Motion Strobe");

            // Data Binding - viewDeviceIoAlarm1
            viewDeviceIoAlarm1.AddDeviceIo(m_Robot.DiAlarmErrorOccurred, "Alarm / Error");
            viewDeviceIoAlarm1.AddDeviceIo(m_Robot.DiAlarmBattery, "Battery Alarm");
            viewDeviceIoAlarm1.AddDeviceIo(m_Robot.DiErrorUpperVacuum, "Upper Vac Error");
            viewDeviceIoAlarm1.AddDeviceIo(m_Robot.DiErrorLowerVacuum, "Lower Vac Error");
            // Data Binding - viewDeviceIoAlarm1
            viewDeviceIoAlarm2.AddDeviceIo(m_Robot.DiErrorUpperAlignment, "Upper Align Error");
            viewDeviceIoAlarm2.AddDeviceIo(m_Robot.DiErrorLowerAlignment, "Lower Align Error");
            viewDeviceIoAlarm2.AddDeviceIo(m_Robot.DiErrorUpperAlign_Y, "Upper Y-Align Error");
            viewDeviceIoAlarm2.AddDeviceIo(m_Robot.DiErrorLowerAlign_Y, "Lower Y-Align Error");
            // Data Binding - viewDeviceIoAlarm1
            viewDeviceIoAlarm3.AddDeviceIo(m_Robot.DiAbnormalityInGlassFallUpper, "Upper Abnormal Fall");
            viewDeviceIoAlarm3.AddDeviceIo(m_Robot.DiAbnormalityInGlassFallLower, "Lower Abnormal Fall");
        }

        private void InitGridRobotInterfaceIo()
        {
            // Data Binding - viewDeviceIoNx Side
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiTopOfMasterJob, "Tob of Master Job");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiCompletionOfReceptionOfMotionPatternData, "Pattern Recept Complete");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiPatternOfOperationIsPerformed, "Pattern is Performed");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiOperationExchange, "EXCHANGE Operation");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiCompleteExchange, "EXCHANGE Complete");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiOperationGet, "GET Operation");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiCompleteGet, "GET Complete");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiOperationPut, "PUT Operation");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiCompletePut, "PUT Complete");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiOperationAlign_Y, "Y-Align Operation");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiCompleteAlign_Y, "Y-Align Complete");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiMovingToVcrPosition, "VCR Moving Pos");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiVcrReadingCompletion, "VCR Read Complete");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiUpperAdsorptionConfirm, "Upper Arm Adsorption");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiUpperGlassDetectConfirm, "Upper Arm Glass Detect");
            //viewDeviceIoNx.AddDeviceIo(m_Robot.DiAdsorptionConveyenceUpper, "Upper Arm Conveyance");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiLowerAdsorptionConfirm, "Lower Arm Adsorption");
            viewDeviceIoNx.AddDeviceIo(m_Robot.DiLowerGlassDetectConfirm, "Lower Arm Glass Detect");
            //viewDeviceIoNx.AddDeviceIo(m_Robot.DiAdsorptionConveyenceLower, "Lower Arm Conveyance");
            // Data Binding - viewDeviceIoPlc Side
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoExternalServoOn, "SERVO ON");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCallMasterJob, "Call Master Job");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoExternalStart, "EXTERNAL START");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoExternalHold, "EXTERNAL HOLD");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoHomePositionReturnRequest, "Home Pos Return Req");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoMotionPatternReadingStrobe, "Pattern Read Strobe");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCompletionOfPatternCheckOfOperation, "Pattern Check Complete");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCompletionOfExchangeOperationReceptionst, "EXCHANGE Recept Complete");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCompletionOfGetOperationReceptionist, "GET Recept Complete");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCompletionOfPutOperationReceptionist, "PUT Recept Complete");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCompletionCheckOfY_AlignOperation, "Y-Align Check Complete");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoVcrReadingCommand, "VCR Reading Command");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoCompletionCheckOfVcrOperation, "VCR Check Complete");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoAlignEffective_Y, "Y-Alignment Effective");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoAlignEffective_T_X, "Theta, X Alignment Effect");
            viewDeviceIoPlc.AddDeviceIo(m_Robot.DoVirtualConveyanceMode, "Virtual Conveyance Mode");
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
		{
			if(m_Initialized)
			{
				MonitorRobotStatus();
				UpdateBtnStatus();
                UpdateComboStatus();
                UpdateInterfaceStatus();
                UpdatePatternOfOperation();
			}
		}

		private void UpdatePatternOfOperation()
		{
			m_PatternOfOperationNo = m_Robot.GetPatternOfOperation();

			string msg = "[NO: " + m_PatternOfOperationNo.ToString() + "] ";

			if(m_Robot.Pattern == nxOP_PATTERN.Y_ALIGN || m_Robot.Pattern == nxOP_PATTERN.VCR_READ)
				msg += m_Robot.Hand.ToString() + " " + m_Robot.Pattern.ToString();
			else
				msg += m_Robot.Hand.ToString() + " " + m_Robot.Cube.ToString() + " " + m_Robot.Pattern.ToString();
		}

        private void UpdateInterfaceStatus()
        {
            if(m_Robot == null) return;

            if(m_PatternOldGridDisplay != m_Robot.Pattern)
            {
                m_PatternOldGridDisplay = m_Robot.Pattern;

                foreach (System.Windows.Forms.Control grid in panelIfSteps.Controls)
                {
                    if (grid.Tag == null)
                    {
                        grid.Visible = false;
                        continue;
                    }
                    else if (m_Robot.Pattern.ToString() == grid.Tag.ToString())
                        grid.Visible = true;
                }
                foreach (System.Windows.Forms.Control grid in panelIfSteps.Controls)
                {
                    if (grid.Tag == null)
                        continue;
                    else if (m_Robot.Pattern.ToString() != grid.Tag.ToString())
                        grid.Visible = false;
                }
            }

            if (m_PatternOldCubeDisplay != m_Robot.Cube)
            {
                m_PatternOldCubeDisplay = m_Robot.Cube;

                string msg;
                msg = string.Format("{0}{1}", chkCubeInterfere.Tag.ToString(), (int)m_Robot.Cube);
                if(chkCubeInterfere.Text != msg) chkCubeInterfere.Text = msg;

                msg = string.Format("{0}{1}", chkCubeProhibition.Tag.ToString(), (int)m_Robot.Cube);
                if(chkCubeProhibition.Text != msg) chkCubeProhibition.Text = msg;

                msg = string.Format("{0}{1}", chkCubePassable.Tag.ToString(), (int)m_Robot.Cube);
                if(chkCubePassable.Text != msg) chkCubePassable.Text = msg;
                if(m_Robot.Cube < nxOP_CUBE.STAGE1) chkCubePassable.Text = "";
            }
        }

		private void UpdateBtnStatus()
        {
            #region Update Button Pushed Status
            btnServoOn.Checked = (m_Robot.ExternalControl == nxEX_CONTROL.EX_SERVO_ON);
            btnServoOff.Checked = (m_Robot.ExternalControl == nxEX_CONTROL.EX_SERVO_OFF);
            //btnRobotHome.Checked = m_Robot.ManualHomeReturnReq;
            btnRobotHome.Checked = (m_Robot.ExternalControl == nxEX_CONTROL.EX_HOME_RETURN);
            btnRobotHold.Checked = m_Robot.DoExternalHold.GetState();
            btnAlarmReset.Checked = m_Robot.DoAlarmErrorReset.GetState();
            btnInitCommand.Checked = false;
            btnMotionStrobe.Checked = m_Robot.ManualMotionStrobeReq;//m_Robot.DoMotionPatternReadingStrobe.GetState();
            #endregion

            #region Update Button Enalbe Status
            bool enable = !m_Robot.ServerManager.GenInfos.AutoMode;
            enable &= m_Robot.ExternalControl == nxEX_CONTROL.EX_NONE;
            btnServoOff.Enabled = enable;                                   // BUTTON SERVO OFF
            enable &= !m_Robot.DiPatternOfOperationIsPerformed.GetState();
            btnServoOn.Enabled = enable;                                    // BUTTON SERVO ON

            //enable = !m_Robot.DiPatternOfOperationIsPerformed.GetState();
            enable = m_Robot.DiRemoteModeSelect.GetState();
            //enable &= m_Robot.DiServoOn.GetState();
            enable &= !m_Robot.ServerManager.GenInfos.AutoMode;
            enable &= m_Robot.ExternalControl == nxEX_CONTROL.EX_NONE;
            btnRobotHome.Enabled = enable;                                  // BUTTON ROBOT HOME

            enable = m_Robot.DiAlarmErrorOccurred.GetState();
            enable |= m_Robot.DiAlarmBattery.GetState();
            enable |= m_Robot.DiErrorUpperAlign_Y.GetState();
            enable |= m_Robot.DiErrorUpperAlignment.GetState();
            enable |= m_Robot.DiErrorUpperVacuum.GetState();
            enable |= m_Robot.DiErrorLowerAlign_Y.GetState();
            enable |= m_Robot.DiErrorLowerAlignment.GetState();
            enable |= m_Robot.DiErrorLowerVacuum.GetState();
            btnAlarmReset.Enabled = enable;                                 // BUTTON ALARM RESET

            enable = m_Robot.ExternalControl != nxEX_CONTROL.EX_NONE;
            btnInitCommand.Enabled = enable;                                // BUTTON COMMAND INIT

            enable = m_Robot.DiCompletionOfPatternStrobeReadingPreparationOfOperation.GetState();
            enable &= !m_Robot.DiPatternOfOperationIsPerformed.GetState();
            enable &= !m_Robot.ServerManager.GenInfos.AutoMode;
            if(!m_Robot.ManualHomeReturnReq)
            {
                enable &= (m_PatternCube >= nxOP_CUBE.STAGE1 || (m_PatternCube >= nxOP_CUBE.PORT1 && cboSelectSlot.SelectedItem != null)) ? true : false;
                enable &= (cboOperation.SelectedItem != null) ? true : false;
                enable &= (cboSelectHand.SelectedItem != null) ? true : false;
            }
            btnMotionStrobe.Enabled = enable;                               // BUTTON MOTION PATTERN READING STRBOE

            enable = m_Robot.ExternalControl == nxEX_CONTROL.EX_NONE;
            btnRobotHold.Enabled = enable;                                  // BUTTON ROBOT HOLD
            //btnPatternReset.Enable = true;
            #endregion
        }

        private void UpdateComboStatus()
        {
            bool enable = !m_Robot.ServerManager.GenInfos.AutoMode;
            enable &= !m_Robot.DiPatternOfOperationIsPerformed.GetState();
            foreach(System.Windows.Forms.Control control in groupBoxMotionCommand.Controls)
            {
                if(control.Enabled != enable)
                    control.Enabled = enable;
            }
        }

		private void MonitorRobotStatus()
		{
            string operationStatus = GetRobotStatusMessage();
            if(viewDeviceIoStatus2.Title.ToString() != operationStatus)
                viewDeviceIoStatus2.Title = operationStatus;
		}

		private string GetRobotStatusMessage()
		{
			string msg;
			if(m_Robot.IsAlarm())
				msg = "[ALARM]";
			else if(m_Robot.DiRobotHold.GetState())
				msg = "[HOLDING]";
            else if(m_Robot.DiOperationOriginPoint.GetState())
                msg = "[MOVING to HOME]";
            else if(m_Robot.IsOperationBegin())
                msg = "[IN OPERATION]";
            else if(m_Robot.IsRobotReady())
				msg = "[READY]";
			else if(m_Robot.DiHomePositionReturn.GetState())
				msg = "[HOME]";
			else
				msg = "[UNKNOWN]";

			return msg;
		}

		private void SelectionChangeCommitted(object sender, EventArgs e)
		{
			if(((ComboBox)sender).SelectedItem != null)
			{
				if(((ComboBox)sender).Tag == cboSelectSlot.Tag)
				{
					m_PatternSlotNo = System.Convert.ToInt32(cboSelectSlot.SelectedItem);
				}
				else if(((ComboBox)sender).Tag == cboSelectHand.Tag)
				{
					m_PatternHand = Hands[cboSelectHand.SelectedIndex];
				}
				else if(((ComboBox)sender).Tag == cboSelectCube.Tag)
				{
                    int usedPortCount = m_Robot.UsedPortName.Length;

                    int cubeId = 0;
                    if (cboSelectCube.SelectedIndex < usedPortCount)
                    {
                        cubeId = cboSelectCube.SelectedIndex;
                    }
                    else
                    {
                        int selectedStageId = cboSelectCube.SelectedIndex - usedPortCount;
                        cubeId = m_Robot.MaxPortNo + selectedStageId;
                    }
                    
					m_PatternCube = Cubes[cubeId];
				}
				else if(((ComboBox)sender).Tag == cboOperation.Tag)
				{
					//m_PatternOperation = Operations[cboOperation.SelectedIndex];
                    m_PatternOperation = m_Robot.UsedManualPattern[cboOperation.SelectedIndex];
				}
			}
		}

        private void viewDeviceIoMode_Load(object sender, EventArgs e)
        {

        }

        private void btnMotionStrobe_Click(object sender, EventArgs e)
        {
            m_Robot.ManualCube = m_PatternCube;
            m_Robot.ManualPattern = m_PatternOperation;
            m_Robot.ManualHand = m_PatternHand;
            m_Robot.ManualSlotNo = m_PatternSlotNo;
            
            if (m_PatternCube >= nxOP_CUBE.PORT1 && m_PatternCube < nxOP_CUBE.STAGE1)                   //Port  Case
            {
                m_DlgManualInterlock = new DlgNX100InterLock(m_PortUnit[(int)m_PatternCube - 1], m_Robot);
            }
            else                                                                                        //Stage Case
            {
                m_DlgManualInterlock = new DlgNX100InterLock(null, m_Robot);
            }

            m_DlgManualInterlock.Show();

            cboSelectCube.SelectedItem = null;
            cboOperation.SelectedItem = null;
            cboSelectHand.SelectedItem = null;
            cboSelectSlot.SelectedItem = null;
/*            if(!m_Robot.ServerManager.GenInfos.AutoMode)
            {
                string text = "";
                string caption = "";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icons = MessageBoxIcon.Information;
                MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
                MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

                bool mappingOk = false;
                bool mappingSkip = false;

                if(m_PatternCube >= nxOP_CUBE.PORT1 && m_PatternCube < nxOP_CUBE.STAGE1)
                {
                    text = "Do you want to check manual Mapping, first?";
                    caption = "Manual Mapping";
                    icons = MessageBoxIcon.Question;
                    if(MessageBox.Show(text, caption, buttons, icons, defaultButtons, options) == DialogResult.Yes)
                    {
                        int portId = (int)(m_PatternCube - 1);
                        int slotId = m_PatternSlotNo - 1;
                        DlgMappingUnit dlg = new DlgMappingUnit(m_PortUnit[portId], true);
                        dlg.ShowDialog();
                        
                        bool cstGlassExist = m_PortUnit[portId].MappingUnit.GetGlassMappingStatus(portId, slotId) == MappingStatus.On;
                        if((m_PatternOperation == nxOP_PATTERN.GET && cstGlassExist) ||
                           (m_PatternOperation == nxOP_PATTERN.PUT && !cstGlassExist))
                        {
                            mappingOk = true;
                        }
                        else
                        {
                            text = "Operation is denied.\nGlass status and JOB is not matched";
                            caption = "Job Command Error";
                            buttons = MessageBoxButtons.OK;
                            icons = MessageBoxIcon.Error;
                            MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                        }
                    }
                    else
                    {
                        text = string.Format("Please confirm following motion selection is correct?\n\nCUBE:\t{0}\nSLOT:\t{1}\nARM:\t{2}\nJOB:\t{3}\n",
                                             cboSelectCube.SelectedItem.ToString(), m_PatternSlotNo.ToString(), m_PatternHand.ToString(), m_PatternOperation.ToString());
                        caption = "Manual Job Confirm";
                        buttons = MessageBoxButtons.OKCancel;
                        icons = MessageBoxIcon.Warning;
                        if(MessageBox.Show(text, caption, buttons, icons, defaultButtons, options) == DialogResult.OK)
                        {
                            mappingSkip = true;
                        }
                    }
                }
                else
                {
                    text = string.Format("Please confirm following motion selection is correct?\n\nCUBE:\t{0}\nARM:\t{1}\nJOB:\t{2}\n",
                                         cboSelectCube.SelectedItem.ToString(), m_PatternHand.ToString(), m_PatternOperation.ToString());
                    caption = "Manual Job Confirm";
                    buttons = MessageBoxButtons.OKCancel;
                    icons = MessageBoxIcon.Warning;
                    if(MessageBox.Show(text, caption, buttons, icons, defaultButtons, options) == DialogResult.OK)
                    {
                        mappingSkip = true;
                    }
                }

                if(mappingOk || mappingSkip)
                {
                    m_Robot.ManualCube = m_PatternCube;
                    m_Robot.ManualPattern = m_PatternOperation;
                    m_Robot.ManualHand = m_PatternHand;
                    m_Robot.ManualSlotNo = m_PatternSlotNo;
                    m_Robot.ManualMotionStrobeReq = true;
                }
                cboSelectSlot.SelectedItem = null;
                cboSelectHand.SelectedItem = null;
                cboSelectCube.SelectedItem = null;
                cboOperation.SelectedItem = null;
            }*/
        }

        private void btnRobotHome_Click(object sender, EventArgs e)
        {
            if(!m_Robot.ServerManager.GenInfos.AutoMode)
            {
                m_Robot.ExternalControl = nxEX_CONTROL.EX_HOME_RETURN;
            }
        }

        private void btnRobotHold_Click(object sender, EventArgs e)
        {
            string text;
            string caption = "Robot Hold";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icons = MessageBoxIcon.Question;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;
            
            if(!m_Robot.DoExternalHold.GetState())
                text = "Do you want to Hold Robot Action";
            else
                text = "Do you want to Release Robot Holding";

            if(MessageBox.Show(text, caption, buttons, icons, defaultButtons, options) == DialogResult.Yes)
            {
                if(!m_Robot.DoExternalHold.GetState())
                    m_Robot.ExternalControl = nxEX_CONTROL.EX_HOLD_ON;
                else
                    m_Robot.ExternalControl = nxEX_CONTROL.EX_HOLD_OFF;
            }
        }

        private void btnServoOn_Click(object sender, EventArgs e)
        {
            m_Robot.ExternalControl = nxEX_CONTROL.EX_SERVO_ON;
        }

        private void btnServoOff_Click(object sender, EventArgs e)
        {
            m_Robot.ExternalControl = nxEX_CONTROL.EX_SERVO_OFF;
        }

        private void btnInitCommand_CheckedChanged(object sender, EventArgs e)
        {
            m_Robot.InitExternalSeq();
        }
	}
}