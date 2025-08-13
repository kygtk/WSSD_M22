using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Data;

namespace Dms.Control
{
    public partial class SetupTabGeneral : UserControl
    {
        #region Fields
        private SetupGenInfoProvider m_GenInfoProvider;
        private SetupIdleInfoProvider m_IdleInfoProvider;
        private SetupTankLevelProvider m_TankLevelProvider;
        private bool m_IsIdleInfoVisible = true;
        private bool m_IsTankLevelVisible = true;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public bool IsIdleInfoVisible
        {
            get { return m_IsIdleInfoVisible; }
            set { m_IsIdleInfoVisible = value; }
        }
        [Category("DMS : UI")]
        public bool IsTankLevelVisible
        {
            get { return m_IsTankLevelVisible; }
            set { m_IsTankLevelVisible = value; }
        }
        #endregion

        #region Constructor
        public SetupTabGeneral()
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
        public void Initialize(SetupGenInfoProvider provider)
        {
            m_GenInfoProvider = provider;
            m_GenInfoProvider.Viewer.Add(this.viewSetupGeneral);
            this.viewSetupGeneral.InitDataView(provider.Adapter.Table, true);
        }

        public void Initialize(SetupIdleInfoProvider provider)
        {
            m_IdleInfoProvider = provider;
            m_IdleInfoProvider.Viewer.Add(this.viewSetupIdleRun);
            this.viewSetupIdleRun.InitDataView(provider.Adapter.Table, true);
        }

        public void Initialize(SetupTankLevelProvider provider)
        {
            m_TankLevelProvider = provider;
            m_TankLevelProvider.Viewer.Add(this.viewSetupTankLevel);
            this.viewSetupTankLevel.InitDataView(provider.Adapter.Table, true);
        }

        private void SetupTabGeneral_Load(object sender, EventArgs e)
        {
            this.viewSetupIdleRun.Visible = m_IsIdleInfoVisible;
            this.viewSetupTankLevel.Visible = m_IsTankLevelVisible;
        }
        #endregion
    }
}
