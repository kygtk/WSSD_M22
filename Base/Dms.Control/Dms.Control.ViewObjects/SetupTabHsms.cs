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
    public partial class SetupTabHsms : UserControl
    {
        #region Fields
        private SetupHsmsInfoProvider m_HsmsProvider;
        private SetupHsmsEqpInfoProvider m_HsmsEqpProvider;
        #endregion

        #region Constructor
        public SetupTabHsms()
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
        public void Initialize(SetupHsmsInfoProvider provider)
        {
            m_HsmsProvider = provider;
            m_HsmsProvider.Viewer.Add(this.viewSetupHsms);
            this.viewSetupHsms.InitDataView(provider.Adapter.Table, true);
        }

        public void Initialize(SetupHsmsEqpInfoProvider provider)
        {
            m_HsmsEqpProvider = provider;
            m_HsmsEqpProvider.Viewer.Add(this.viewSetupHsmsEqp);
            this.viewSetupHsmsEqp.InitDataView(provider.Adapter.Table, true);
        }
        #endregion
    }
}
