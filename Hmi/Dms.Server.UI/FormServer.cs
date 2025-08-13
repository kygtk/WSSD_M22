using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using System.Collections;
using Dms.ServerCommon;

namespace Dms.Server
{
    public partial class FormServer : Form
    {
        private ServerManager m_Server = ServerManager.Instance;
        private bool m_Control = false;

        public FormServer()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(bool control)
        {
            try
            {
                m_Control = control;

                if (m_Control)
                {
                    if (AppConfig.Instance.AutoStart)
                    {
                        m_Server.Initialize();

                        this.buttonInit.Enabled = !m_Server.Initialized;
                    }
                }

                UpdateServerCommandButton();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                m_Server.Uninitialize();
                MessageBox.Show(msg);
                this.Dispose();
            }
        }

        private void RemServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "DMS Server" + " - Run in " + AppConfig.Instance.ServerMode.ToString() + " Mode";
                this.labelServerState.Text = m_Server.State.ToString();

                m_Server.ServerStateChanged += new StateChanged(UpdateServerState);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                m_Server.Uninitialize();
                MessageBox.Show(msg);
                this.Dispose();
            }
        }

        private void UpdateServerState()
        {
            this.labelServerState.Text = m_Server.State.ToString();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            if (!m_Control)
            {
                this.Close();
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Exit?", "WSSD Sever",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    m_Server.Uninitialize();
                    this.Close();
                }
            }
        }

        private void FireEvent_Click(object sender, EventArgs e)
        {
            /****
            ////IBroadcaster bcast = (IBroadcaster)RemotingHelper.GetObject(typeof(IBroadcaster));
            BroadcasterImpl bcast = (BroadcasterImpl)RemoteSingletonObjectsList.Instance.GetRemoteSingletonObject(typeof(BroadcasterImpl));
            bcast.BroadcastMessage("Hello World! Events work fine now ... ");
            ***/

            ////DmsServerManager m_Server = DmsServerManager.Instance;
            ////m_Server.Initialize();
            ////m_Server.FireEventToClient();

            //TagRecipe recipe = new TagRecipe();
            //m_Server.GetCurrentRecipe(ref recipe);

            //DisplayData.BeginUpdate();

            //DisplayData.Items.Add(recipe.Id);
            //DisplayData.Items.Add(recipe.Comment);
            //DisplayData.Items.Add(recipe.CvSpeed.ToString());

            //TagGlassData data = new TagGlassData();
            //data.PosId = 0;
            //m_Server.GlassInfoList.CreateGlassData(data);

            //DisplayData.EndUpdate();

        }

        private void buttonAlarm_Click(object sender, EventArgs e)
        {
            if (AppConfig.Instance.Simul.Device)
            {
                m_Server.EqpStateManager.AlarmResetSwitchPushed = true;
            }
        }

        private void buttonLdIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (AppConfig.Instance.Simul.Device)
                {
                    ArrayList cvUnits = DmsComponents.Instance[typeof(CvUnit)];
                    ((CvUnit)cvUnits[0]).GlsInSensor.SetState(true);
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonAlarmList_Click(object sender, EventArgs e)
        {
            try
            {
                FormAlarmList form = new FormAlarmList();

                form.InitObject();

                form.ShowDialog();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            try
            {
                FormSetup form = new FormSetup();

                form.InitObject();

                form.ShowDialog();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                FormRecipe form = new FormRecipe();

                form.InitObject();

                form.ShowDialog();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void HHLevel_Click(object sender, EventArgs e)
        {
            //if (m_Server.Simulation.Device)
            //{
            //    if (m_Server.LevelSensors.Count == 5)
            //    {
            //        checkBox1.AutoCheck = true;
            //        bool chk = m_Server.LevelSensors[4].DiSensor.GetState();
            //        m_Server.LevelSensors[4].DiSensor.SetState(!chk);
            //    }
            //    else
            //    {
            //        checkBox1.AutoCheck = false;
            //        return;
            //    }
            //}
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Server.Simulation.Device)
            //{
            //    bool chk = m_Server.LevelSensors[3].DiSensor.GetState();
            //    m_Server.LevelSensors[3].DiSensor.SetState(!chk);
            //}
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Server.Simulation.Device)
            //{
            //    bool chk = m_Server.LevelSensors[2].DiSensor.GetState();
            //    m_Server.LevelSensors[2].DiSensor.SetState(!chk);
            //}
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Server.Simulation.Device)
            //{
            //    bool chk = m_Server.LevelSensors[1].DiSensor.GetState();
            //    m_Server.LevelSensors[1].DiSensor.SetState(!chk);
            //}
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Server.Simulation.Device)
            //{
            //    bool chk = m_Server.LevelSensors[0].DiSensor.GetState();
            //    m_Server.LevelSensors[0].DiSensor.SetState(!chk);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FormTagContainer form;
                form = new FormTagContainer();

                form.Initialize(m_Server.DataProvider.TagContainer);
                form.ShowDialog();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            try
            {
                m_Server.Initialize();
                UpdateServerCommandButton();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                m_Server.Start();
                UpdateServerCommandButton();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            try
            {
                m_Server.Stop();
                UpdateServerCommandButton();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void UpdateServerCommandButton()
        {
            try
            {
                this.buttonInit.Enabled = !m_Server.Initialized;
                this.buttonStart.Enabled = !((m_Server.State == ActiveState.Run) || !m_Server.Initialized);
                this.buttonPause.Enabled = ((m_Server.State == ActiveState.Run) && m_Server.Initialized);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonFTP_Click(object sender, EventArgs e)
        {
            try
            {
                FormFtp form = new FormFtp();

                //form.InitObject();

                form.ShowDialog();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }
    }
}