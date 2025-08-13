using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.DeviceLibrary;
using Dms.Common;
using Dms.Server;

namespace Dms.Control
{
    public partial class ModbusEqpUnit : UserControl
    {
        #region Fields
        private DmsNodeTag m_Tag = null;
        private ModbusRootNode m_RootNode;
        private ModbusEqp m_Eqp;

        private EqpState m_OldEqpState = EqpState.UnKnown;
        private ProcessState m_OldProcState = ProcessState.UnKnown;
        private bool m_OldAutoMode = false;
        private bool m_OldConnetion = false;
        private int m_OldAlarmCount = 0;

        private ushort m_OldWaferCount = 0;
        private string m_OldRecipeId = "";

        private Color m_EqpUnknown = Color.DarkGray;
        private Color m_EqpNormal = Color.Green;
        private Color m_EqpDown = Color.Red;
        private Color m_EqpPM = Color.Blue;
        private Color m_ProcUnknown = Color.WhiteSmoke;
        private Color m_ProcInit = Color.White;
        private Color m_ProcSetup = Color.PowderBlue;
        private Color m_ProcIdle = Color.Yellow;
        private Color m_ProcRun = Color.YellowGreen;
        private Color m_ProcPause = Color.LightGray;

        //for update tooltip
        string m_Connection = "";
        string m_AlarmCount = "";
        string m_EqpMode = "";
        string m_ProcStatus = "";
        string m_EqpStatus = "";
        #endregion

        #region Events
        [Category("DMS : EVENT"), Description("EQP Click Event")]
        public event EventHandler EqpClick;
        #endregion

        #region Properties
        [Category("DMS : Tag")]
        public DmsNodeTag DmsNodeTag
        {
            get { return m_Tag; }
            set { m_Tag = value; }
        }
        [Category("DMS : UI")]
        public Color EqpUnKnownColor
        {
            get { return m_EqpUnknown; }
            set { m_EqpUnknown = value; }
        }
        [Category("DMS : UI")]
        public Color NormalColor
        {
            get { return m_EqpNormal; }
            set { m_EqpNormal = value; }
        }
        [Category("DMS : UI")]
        public Color DownColor
        {
            get { return m_EqpDown; }
            set { m_EqpDown = value; }
        }
        [Category("DMS : UI")]
        public Color PMColor
        {
            get { return m_EqpPM; }
            set { m_EqpPM = value; }
        }
        [Category("DMS : UI")]
        public Color ProcessUnknownColor
        {
            get { return m_ProcUnknown; }
            set { m_ProcUnknown = value; }
        }
        [Category("DMS : UI")]
        public Color ProcessInitColor
        {
            get { return m_ProcInit; }
            set { m_ProcInit = value; }
        }
        [Category("DMS : UI")]
        public Color ProcessSetupColor
        {
            get { return m_ProcSetup; }
            set { m_ProcSetup = value; }
        }
        [Category("DMS : UI")]
        public Color ProcessIdleColor
        {
            get { return m_ProcIdle; }
            set { m_ProcIdle = value; }
        }
        [Category("DMS : UI")]
        public Color ProcessRunColor
        {
            get { return m_ProcRun; }
            set { m_ProcRun = value; }
        }
        [Category("DMS : UI")]
        public Color ProcessPauseColor
        {
            get { return m_ProcPause; }
            set { m_ProcPause = value; }
        }
        [Browsable(false)]
        public ModbusEqp Eqp
        {
            get { return m_Eqp; }
        }
        #endregion

        public ModbusEqpUnit()
        {
            InitializeComponent();
        }

        public bool Initialize(ModbusRootNode rootNode)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            if (m_Tag == null || string.IsNullOrEmpty(m_Tag.Name))
            {
                string msg = string.Format("Tag of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            m_RootNode = rootNode;

            ModbusEqp eqp = m_RootNode.GetModbusEqp(m_Tag.Name);

            if (eqp == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_Eqp = eqp;
            }

            SetDisplay();

            this.lblName.ForeColor = Color.Red;
            this.tmrUpdateState.Enabled = true;

            return true;
        }

        private void SetDisplay()
        {
            if (m_Eqp == null) return;

            this.lblName.Text = m_Eqp.EqpUnitName;
            this.lblWafer.Text = m_Eqp.WaferCount.ToString();
            this.lblRecipe.Text = m_Eqp.RecipeId;

            m_Connection = m_Eqp.GetConnection() ? "Communication : Connect" : "Communication : Disconnect";
            m_AlarmCount = string.Format("Alarm Count : {0}", m_Eqp.Variable.AlarmId.Count);
            m_EqpMode = m_Eqp.Variable.AutoMode ? "Equipment Mode : Auto" : "Equipment Mode : Manual";
            m_ProcStatus = "Process Status : " + m_Eqp.Variable.ProcessStatus.ToString();
            m_EqpStatus = "Equipment Status : " + m_Eqp.Variable.EquipmentStatus.ToString();

            string toolTip = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", m_Connection, m_AlarmCount, m_EqpMode, m_ProcStatus, m_EqpStatus);
            this.toolTip1.SetToolTip(this.lblName, toolTip);
        }

        // 화면 깜빡임 최소화
        protected void SetDoubleBuffer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            bool changed = false;

            if (m_Eqp == null) return;

            if (m_OldAutoMode != m_Eqp.Variable.AutoMode)
            {
                m_OldAutoMode = m_Eqp.Variable.AutoMode;
                m_EqpMode = m_Eqp.Variable.AutoMode ? "Equipment Mode : Auto" : "Equipment Mode : Manual";
                changed = true;
            }

            if (m_OldAlarmCount != m_Eqp.Variable.AlarmId.Count)
            {
                m_OldAlarmCount = m_Eqp.Variable.AlarmId.Count;
                m_AlarmCount = string.Format("Alarm Count : {0}", m_Eqp.Variable.AlarmId.Count);
                changed = true;
            }

            if (m_OldConnetion != m_Eqp.GetConnection())
            {
                m_OldConnetion = m_Eqp.GetConnection();
                m_Connection = m_OldConnetion ? "Communication : Connect" : "Communication : Disconnect";
                changed = true;

                if (m_OldConnetion == true)
                {
                    this.lblName.ForeColor = Color.Blue;
                }
                else
                {
                    this.lblName.ForeColor = Color.Red;
                }
            }

            if (m_OldEqpState != m_Eqp.EqpState)
            {
                m_OldEqpState = m_Eqp.EqpState;

                switch (m_OldEqpState)
                {
                    case EqpState.UnKnown:
                        this.BackColor = m_EqpUnknown;
                        break;
                    case EqpState.Normal:
                        this.BackColor = m_EqpNormal;
                        break;
                    case EqpState.Fault:
                        this.BackColor = m_EqpDown;
                        break;
                    case EqpState.PM:
                        this.BackColor = m_EqpPM;
                        break;
                }

                m_EqpStatus = "Equipment Status : " + m_OldEqpState.ToString();
                changed = true;
            }

            if (m_OldProcState != m_Eqp.ProcessState)
            {
                m_OldProcState = m_Eqp.ProcessState;

                switch (m_OldProcState)
                {
                    case ProcessState.UnKnown:
                        this.lblName.BackColor = m_ProcUnknown;
                        break;
                    case ProcessState.Excute:
                        this.lblName.BackColor = m_ProcRun;
                        break;
                    case ProcessState.Idle:
                        this.lblName.BackColor = m_ProcIdle;
                        break;
                    case ProcessState.Init:
                        this.lblName.BackColor = m_ProcInit;
                        break;
                    case ProcessState.Setup:
                        this.lblName.BackColor = m_ProcSetup;
                        break;
                    case ProcessState.Pause:
                        this.lblName.BackColor = m_ProcPause;
                        break;
                }

                m_ProcStatus = "Process Status : " + m_OldProcState.ToString();
                changed = true;
            }

            if (m_OldWaferCount != m_Eqp.WaferCount)
            {
                m_OldWaferCount = m_Eqp.WaferCount;
                lblWafer.Text = m_OldWaferCount.ToString();
            }

            if (m_OldRecipeId != m_Eqp.RecipeId)
            {
                m_OldRecipeId = m_Eqp.RecipeId;
                lblRecipe.Text = m_OldRecipeId;
            }

            if (changed == true)
            {
                string toolTip = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", m_Connection, m_AlarmCount, m_EqpMode, m_ProcStatus, m_EqpStatus);
                this.toolTip1.SetToolTip(this.lblName, toolTip);
            }
        }

        private void ModbusEqpUnit_Click(object sender, EventArgs e)
        {
            Eqp_Click(this, e);
        }

        private void Eqp_Click(object sender, EventArgs e)
        {
            if (EqpClick != null)
            {
                EqpClick(sender, e);
            }
        }
    }
}
