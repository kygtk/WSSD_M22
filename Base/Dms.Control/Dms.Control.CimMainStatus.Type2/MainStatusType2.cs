using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class MainStatusType2 : UserControl
    {
        public enum ControlMode
        {
            Auto,
            Manual
        };

        public enum EqpStatus
        {
            Normal,
            Fault,
            PM
        };

        public enum ProcessStatus
        {
            Init,
            Idle,
            Setup,
            Ready,
            Executing,
            Pause
        }

        public enum RemoteStatus
        {
            Offline,
            Control,
            Monitor,
        }

        #region Fields

        //private ControlMode m_Auto = ControlMode.Auto;
        //private EqpStatus m_EqpStatus = EqpStatus.Normal;
        //private ProcessStatus m_ProcStatus = ProcessStatus.Init;
        //private RemoteStatus m_RemoteStatus = RemoteStatus.Offline;

        //fore color
        private Color m_ControlAutoForeColor = Color.MidnightBlue;
        private Color m_ControlManualForeColor = Color.MidnightBlue;
        private Color m_EqpNormalForeColor = Color.MidnightBlue;
        private Color m_EqpFaultForeColor = Color.MidnightBlue;
        private Color m_EqpPMForeColor = Color.MidnightBlue;
        private Color m_ProcInitForeColor = Color.MidnightBlue;
        private Color m_ProcSetupForeColor = Color.MidnightBlue;
        private Color m_ProcIdleForeColor = Color.MidnightBlue;
        private Color m_ProcReadyForeColor = Color.MidnightBlue;
        private Color m_ProcExeForeColor = Color.MidnightBlue;
        private Color m_ProcPauseForeColor = Color.MidnightBlue;
        private Color m_RemoteOfflineForeColor = Color.Red;
        private Color m_RemoteMonitorForeColor = Color.Red;
        private Color m_RemoteControlForeColor = Color.Blue;

        //back color
        private Color m_ControlAutoBackColor = Color.White;
        private Color m_ControlManualBackColor = Color.White;
        private Color m_EqpNormalBackColor = Color.White;
        private Color m_EqpFaultBackColor = Color.White;
        private Color m_EqpPMBackColor = Color.White;
        private Color m_ProcInitBackColor = Color.White;
        private Color m_ProcSetupBackColor = Color.White;
        private Color m_ProcIdleBackColor = Color.White;
        private Color m_ProcReadyBackColor = Color.White;
        private Color m_ProcExeBackColor = Color.White;
        private Color m_ProcPauseBackColor = Color.White;
        private Color m_RemoteOfflineBackColor = Color.White;
        private Color m_RemoteMonitorBackColor = Color.Yellow;
        private Color m_RemoteControlBackColor = Color.LightGreen;

        #endregion

        #region Properties

        [Category("Color:Control status"),
         Description("Text color Setting")]
        public Color ControlAutoForeColor
        {
            get { return m_ControlAutoForeColor; }
            set { m_ControlAutoForeColor = value; }
        }

        [Category("Color:Control status"),
         Description("Text color Setting")]
        public Color ControlAutoBackColor
        {
            get { return m_ControlAutoBackColor; }
            set { m_ControlAutoBackColor = value; }
        }

        [Category("Color:Control status"),
         Description("Text color Setting")]
        public Color ControlManualForeColor
        {
            get { return m_ControlManualForeColor; }
            set { m_ControlManualForeColor = value; }
        }

        [Category("Color:Control status"),
         Description("Text color Setting")]
        public Color ControlManualBackColor
        {
            get { return m_ControlManualBackColor; }
            set { m_ControlManualBackColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color EqpNormalForeColor
        {
            get { return m_EqpNormalForeColor; }
            set { m_EqpNormalForeColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color EqpNormalBackColor
        {
            get { return m_EqpNormalBackColor; }
            set { m_EqpNormalBackColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color EqpFaultForeColor
        {
            get { return m_EqpFaultForeColor; }
            set { m_EqpFaultForeColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color EqpFaultBackColor
        {
            get { return m_EqpFaultBackColor; }
            set { m_EqpFaultBackColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color EqpPMForeColor
        {
            get { return m_EqpPMForeColor; }
            set { m_EqpPMForeColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color EqpPMBackColor
        {
            get { return m_EqpPMBackColor; }
            set { m_EqpPMBackColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color ProcInitForeColor
        {
            get { return m_ProcInitForeColor; }
            set { m_ProcInitForeColor = value; }
        }

        [Category("Color:Eqp status"),
         Description("Text color Setting")]
        public Color ProcInitBackColor
        {
            get { return m_ProcInitBackColor; }
            set { m_ProcInitBackColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcSetupForeColor
        {
            get { return m_ProcSetupForeColor; }
            set { m_ProcSetupForeColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcSetupBackColor
        {
            get { return m_ProcSetupBackColor; }
            set { m_ProcSetupBackColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcIdleForeColor
        {
            get { return m_ProcIdleForeColor; }
            set { m_ProcIdleForeColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcIdleBackColor
        {
            get { return m_ProcIdleBackColor; }
            set { m_ProcIdleBackColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcReadyForeColor
        {
            get { return m_ProcReadyForeColor; }
            set { m_ProcReadyForeColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcReadyBackColor
        {
            get { return m_ProcReadyBackColor; }
            set { m_ProcReadyBackColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcExeForeColor
        {
            get { return m_ProcExeForeColor; }
            set { m_ProcExeForeColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcExeBackColor
        {
            get { return m_ProcExeBackColor; }
            set { m_ProcExeBackColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcPauseForeColor
        {
            get { return m_ProcPauseForeColor; }
            set { m_ProcPauseForeColor = value; }
        }

        [Category("Color:Process status"),
         Description("Text color Setting")]
        public Color ProcPauseBackColor
        {
            get { return m_ProcPauseBackColor; }
            set { m_ProcPauseBackColor = value; }
        }

        // Remote Mode Color Setting
        [Category("Color:Remote mode"),
         Description("Text color Setting")]
        public Color RemoteOfflineForeColor
        {
            get { return m_RemoteOfflineForeColor; }
            set { m_RemoteOfflineForeColor = value; }
        }

        [Category("Color:Remote mode"),
         Description("Text color Setting")]
        public Color RemoteOfflineBackColor
        {
            get { return m_RemoteOfflineBackColor; }
            set { m_RemoteOfflineBackColor = value; }
        }

        [Category("Color:Remote mode"),
         Description("Text color Setting")]
        public Color RemoteMonitorForeColor
        {
            get { return m_RemoteMonitorForeColor; }
            set { m_RemoteMonitorForeColor = value; }
        }

        [Category("Color:Remote mode"),
         Description("Text color Setting")]
        public Color RemoteMonitorBackColor
        {
            get { return m_RemoteMonitorBackColor; }
            set { m_RemoteMonitorBackColor = value; }
        }

        [Category("Color:Remote mode"),
         Description("Text color Setting")]
        public Color RemoteControlForeColor
        {
            get { return m_RemoteControlForeColor; }
            set { m_RemoteControlForeColor = value; }
        }

        [Category("Color:Remote mode"),
         Description("Text color Setting")]
        public Color RemoteControlBackColor
        {
            get { return m_RemoteControlBackColor; }
            set { m_RemoteControlBackColor = value; }
        }
        #endregion

        #region Methods
        public void SetControlMode(ControlMode mode)
        {
            lblControlMode.Text = mode.ToString().ToUpper();
            switch (mode)
            {
                case ControlMode.Auto:
                    lblControlMode.ForeColor = m_ControlAutoForeColor;
                    lblControlMode.BackColor = m_ControlAutoBackColor;
                    break;
                case ControlMode.Manual:
                    lblControlMode.ForeColor = m_ControlManualForeColor;
                    lblControlMode.BackColor = m_ControlManualBackColor;
                    break;
            }
        }

        public void SetEqpStatus(EqpStatus status)
        {
            lblEqpStatus.Text = status.ToString().ToUpper();
            switch (status)
            {
                case EqpStatus.Fault:
                    lblEqpStatus.ForeColor = m_EqpFaultForeColor;
                    lblEqpStatus.BackColor = m_EqpFaultBackColor;
                    break;
                case EqpStatus.Normal:
                    lblEqpStatus.ForeColor = m_EqpNormalForeColor;
                    lblEqpStatus.BackColor = m_EqpNormalBackColor;
                    break;
                case EqpStatus.PM:
                    lblEqpStatus.ForeColor = m_EqpPMForeColor;
                    lblEqpStatus.BackColor = m_EqpPMBackColor;
                    break;
            }
        }

        public void SetProcessStatus(ProcessStatus status)
        {
            lblProcessStatus.Text = status.ToString().ToUpper();
            switch (status)
            {
                case ProcessStatus.Init:
                    lblProcessStatus.ForeColor = m_ProcInitForeColor;
                    lblProcessStatus.BackColor = m_ProcInitBackColor;
                    break;
                case ProcessStatus.Idle:
                    lblProcessStatus.ForeColor = m_ProcIdleForeColor;
                    lblProcessStatus.BackColor = m_ProcIdleBackColor;
                    break;
                case ProcessStatus.Ready:
                    lblProcessStatus.ForeColor = m_ProcReadyForeColor;
                    lblProcessStatus.BackColor = m_ProcReadyBackColor;
                    break;
                case ProcessStatus.Setup:
                    lblProcessStatus.ForeColor = m_ProcSetupForeColor;
                    lblProcessStatus.BackColor = m_ProcSetupBackColor;
                    break;
                case ProcessStatus.Executing:
                    lblProcessStatus.ForeColor = m_ProcExeForeColor;
                    lblProcessStatus.BackColor = m_ProcExeBackColor;
                    break;
                case ProcessStatus.Pause:
                    lblProcessStatus.ForeColor = m_ProcPauseForeColor;
                    lblProcessStatus.BackColor = m_ProcPauseBackColor;
                    break;
            }
        }

        public void SetRemoteStatus(RemoteStatus status)
        {
            lblHostConnection.Text = status.ToString().ToUpper();
            switch (status)
            {
                case RemoteStatus.Offline:
                    lblHostConnection.ForeColor = m_RemoteOfflineForeColor;
                    lblHostConnection.BackColor = m_RemoteOfflineBackColor;
                    break;
                case RemoteStatus.Monitor:
                    lblHostConnection.ForeColor = m_RemoteMonitorForeColor;
                    lblHostConnection.BackColor = m_RemoteMonitorBackColor;
                    break;
                case RemoteStatus.Control:
                    lblHostConnection.ForeColor = m_RemoteControlForeColor;
                    lblHostConnection.BackColor = m_RemoteControlBackColor;
                    break;
            }
        }
        #endregion

        #region Constructor
        public MainStatusType2()
        {
            InitializeComponent();

            SetControlMode(ControlMode.Auto);
            SetEqpStatus(EqpStatus.Normal);
            SetProcessStatus(ProcessStatus.Init);
            SetRemoteStatus(RemoteStatus.Offline);
        }
        #endregion
    }
}