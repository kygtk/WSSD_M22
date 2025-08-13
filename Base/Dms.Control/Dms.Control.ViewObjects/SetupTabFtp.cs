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
    public partial class SetupTabFtp : UserControl
    {
        #region Fields
        private SetupFtpInfoProvider m_FtpProvider;
        #endregion

        #region Constructor
        public SetupTabFtp()
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
        public void Initialize(SetupFtpInfoProvider provider)
        {
            m_FtpProvider = provider;
            m_FtpProvider.Viewer.Add(this.viewSetupFtp);
            this.viewSetupFtp.InitDataView(provider.Adapter.Table, true);
        }
        #endregion
    }
}
