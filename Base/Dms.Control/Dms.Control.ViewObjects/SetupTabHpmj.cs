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
    public partial class SetupTabHpmj : UserControl
    {
        #region Fields
        private SetupHpmjInfoProvider m_HpmjProvider;
        #endregion

        #region Constructor
        public SetupTabHpmj()
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
        public void Initialize(SetupHpmjInfoProvider provider)
        {
            m_HpmjProvider = provider;
            m_HpmjProvider.Viewer.Add(this.viewSetupHpmj);
            this.viewSetupHpmj.InitDataView(provider.Adapter.Table, true);
        }
        #endregion
    }
}
