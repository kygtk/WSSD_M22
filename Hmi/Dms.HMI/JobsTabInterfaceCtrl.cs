using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Server;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class JobsTabInterfaceCtrl : UserControl // 10.12.21 minhan 아마 나중에 메뉴얼 모드에서 enable 시그널 살려 달라고 할텐데 지금은 Auto만 보자.
    {
        #region Fields
        private ServerManager m_Server;
        private BOELoaderInterface m_BoeInterface;
        private GantryUnit m_GantryUnit;
        private short m_GantrySendPosition;
        private int CurGtPosition;
        private bool CheckGtPosOk;
        private bool RecvCond;
        private bool SendCond;
        private CvUnit m_CvUnit;
        #endregion

        public JobsTabInterfaceCtrl()
        {

            InitializeComponent();
        }

        public void Initialize() // 11.02.09 minhan
        {
            tmrUpdateState.Enabled = true;
            m_Server = ServerManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_GantrySendPosition = eqpPos_TR_Servo_Unit.Send;
            CurGtPosition = 0;
            CheckGtPosOk = false;
            RecvCond = false;
            SendCond = false;
            m_CvUnit = eqpTransferUnits._LD_CvUnit;
        }


        private bool IsRecvCond() // 11.02.09 minhan
        {
            bool cond = true;
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;

            //CurGtPosition = m_GantryUnit.Servo.GetCurPointId();

            //if ((CurGtPosition == (int)m_GantryWaitPosition) &&
            //    m_GantryUnit.diWait_Pos_Sensor.IsDetected() &&
            //    !m_GantryUnit.diSend_Pos_Sensor.IsDetected() &&
            //    !m_GantryUnit.diRecv_Pos_Sensor.IsDetected())
            //{
            //    CheckGtPosOk = true;
            //}
            //else CheckGtPosOk = false;

            cond &= eqpActuatorUnits._LD_Tilting_Unit.IsNegative();
            cond &= eqpActuatorUnits._LD_Idle_Roller.IsNegative();
            //cond &= !eqpSensor_Ios._LD_Robot_Hand_Interlock_Sensor.IsDetected(); // 11.03.25 minhan loader에서 대응안됨.
            //cond &= m_BoeInterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();
            cond &= (heavy & HeavyInterlock.Emo) <= 0;
            cond &= (heavy & HeavyInterlock.Door) <= 0;
            cond &= !eqpGlsSensors._LD_Unit_Gls_In_Sensor.IsDetected();
            cond &= !eqpGlsSensors._LD_Unit_Gls_Out_Sensor.IsDetected();
            cond &= !m_Server.GlassData.IsExist(m_CvUnit.DataMatchingKey(0));
            //cond &= CheckGtPosOk;
            cond &= !GenInfoHandler.Instance.AutoMode;

            return cond;
        }

        private bool IsSendCond() // 11.02.09 minhan
        {
            bool cond = true;
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;

            CurGtPosition = m_GantryUnit.Servo.GetCurPointId();

            if ((CurGtPosition == (int)m_GantrySendPosition) &&
                m_GantryUnit.diSend_Pos_Sensor.IsDetected() &&
                !m_GantryUnit.diWait_Pos_Sensor.IsDetected() &&
                !m_GantryUnit.diRecv_Pos_Sensor.IsDetected())
            {
                CheckGtPosOk = true;
            }
            else CheckGtPosOk = false;

            cond &= eqpActuatorUnits._TR_Align.IsNegative();
            //cond &= !eqpSensor_Ios._Robot_Hand_Interlock_Sensor.IsDetected(); // 11.03.25 minhan Loader에서 대응이 안됨.
            //cond &= m_BoeInterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState();
            cond &= (heavy & HeavyInterlock.Emo) <= 0;
            cond &= (heavy & HeavyInterlock.Door) <= 0;
            cond &= eqpGlsSensors._TR_Unit_Gls_Exist_Sensor.IsDetected();
            //cond &= !m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
            cond &= CheckGtPosOk;
            cond &= !GenInfoHandler.Instance.AutoMode;

            return cond;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            RecvCond = IsRecvCond(); // 11.02.09 minhan
            SendCond = IsSendCond();

            // cleaner
            this.ckbHDCAvaliable.Checked = m_BoeInterface.mobCleaner_is_Available.GetState();
            this.ckbUnloadInterlock.Checked = m_BoeInterface.mobUnload_Stage_Inter_lock_U.GetState();
            this.ckbUnloadWait.Checked = m_BoeInterface.mobUnload_Wait_U.GetState();

            //this.ckbLoadReq.Checked = m_BoeInterface.mobCleaner_Load_Request.GetState();
            //this.ckbLDAccessPossible.Checked = m_BoeInterface.mobCleaner_Access_Possible_Load_Stage.GetState();
            //this.chkLdSubstratePresent.Checked = m_BoeInterface.mobSubstrate_Present_at_Cleaner_Load_Stage.GetState();
            //this.chkLdDataReadcomp.Checked = m_BoeInterface.mobCleaner_Received_Data_Read_Completed.GetState();

            this.ckbUnloadReq.Checked = m_BoeInterface.mobCleaner_Unload_Request_U.GetState();
            this.ckbULAccessPossible.Checked = m_BoeInterface.mobCleaner_Access_Possible_Unload_Stage_U.GetState();
            this.chkUlSubstratePresent.Checked = m_BoeInterface.mobSubstrate_Present_at_Cleaner_Unload_Stage_U.GetState();
            this.chkUlDataReadReq.Checked = m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.GetState();

            this.chkCleanOut.Checked = m_BoeInterface.mobCleanOut_Mode.GetState(); // 11.06.11 minhan

            // loader
            this.ckbLoaderPower.Checked = m_BoeInterface.mibLoader_Power_ON.GetState();

            this.ckbLDReady.Checked = m_BoeInterface.mibTransfer_Readyto_Cleaner_Load_Stage.GetState();
            this.ckbLDRobotAceess.Checked = m_BoeInterface.mibRobot_Accessing_at_Cleaner_Load_Stage.GetState();
            this.chkLdDataReadReq.Checked = m_BoeInterface.mibCleaner_Recived_Data_Read_Request.GetState();

            //this.ckbULReady.Checked = m_BoeInterface.mibTransfer_Ready_to_Cleaner_Unload_Stage_L.GetState();
            //this.ckbULRobotAceess.Checked = m_BoeInterface.mibRobot_Accessing_at_Cleaner_Unload_Stage.GetState();
            //this.chkUlDataReadcomp.Checked = m_BoeInterface.mibCleaner_reported_Data_Read_Complete_L.GetState();

            this.chkNosub.Checked = m_BoeInterface.mibNoSubstarate_to_Cleaner.GetState(); // 11.05.02 minhan
            //this.chkExrequest.Checked = m_BoeInterface.mibExchange_Req.GetState(); // 11.06.10 minhan

            if (GenInfoHandler.Instance.AutoMode == false) // 11.03.25 minhan 일단 진행하자. 그리고 통보해야함.
            {
                btnloadOff.Enabled = true;
                btnunloadOff.Enabled = true;

                if (!RecvCond)
                {
                    btnloadOn.Enabled = false;
                    m_BoeInterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
                }
                else
                {
                    btnloadOn.Enabled = true;
                }

                if (!SendCond)
                {
                    btnunloadOn.Enabled = false;
                    m_BoeInterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
                }
                else
                {
                    btnunloadOn.Enabled = true;
                }

                if (m_BoeInterface.mobCleaner_Access_Possible_Load_Stage.GetState() == true)
                {
                    btnloadOn.BackColor = System.Drawing.Color.Red;
                    btnloadOff.BackColor = System.Drawing.Color.Blue;
                }
                else
                {
                    btnloadOn.BackColor = System.Drawing.Color.Blue;
                    btnloadOff.BackColor = System.Drawing.Color.Red;
                }

                if (m_BoeInterface.mobCleaner_Access_Possible_Unload_Stage_U.GetState() == true)
                {
                    btnunloadOn.BackColor = System.Drawing.Color.Red;
                    btnunloadOff.BackColor = System.Drawing.Color.Blue;
                }
                else
                {
                    btnunloadOn.BackColor = System.Drawing.Color.Blue;
                    btnunloadOff.BackColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                btnloadOn.Enabled = false;
                btnunloadOn.Enabled = false;
                btnloadOff.Enabled = false;
                btnunloadOff.Enabled = false;
                btnloadOn.BackColor = System.Drawing.Color.Blue;
                btnunloadOn.BackColor = System.Drawing.Color.Blue;
            }

        }

        private void LoadEnableOn_Click(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else if (!RecvCond) return;
            else
            {
                if (DialogResult.No == MessageBox.Show("Please Interlock Check.. Do you want Running?", "WSSD", MessageBoxButtons.YesNo)) // 11.03.25 minhan
                {
                    return;
                }
                else
                {
                    m_BoeInterface.mobCleaner_Access_Possible_Load_Stage.SetState(true);
                }
            }

        }

        private void UnloadEnableOn_Click(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else if (!SendCond) return;
            else
            {
                if (DialogResult.No == MessageBox.Show("Please Interlock Check.. Do you want Running?", "WSSD", MessageBoxButtons.YesNo)) // 11.03.25 minhan
                {
                    return;
                }
                else
                {
                    m_BoeInterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(true);
                }
            }

        }

        private void LoadEnableOff_Click(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else
            {
                m_BoeInterface.mobCleaner_Access_Possible_Load_Stage.SetState(false);
            }

        }

        private void UnloadEnableOff_Click(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else
            {
                m_BoeInterface.mobCleaner_Access_Possible_Unload_Stage_U.SetState(false);
            }

        }
    }
}
