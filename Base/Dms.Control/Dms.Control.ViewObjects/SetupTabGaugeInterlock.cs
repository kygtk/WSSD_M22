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
    public partial class SetupTabGaugeInterlock : UserControl
    {
        #region Fields
        private SetupGaugeInterlockProvider m_Provider;
        #endregion

        #region Constructor
        public SetupTabGaugeInterlock()
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
        public void Initialize(SetupGaugeInterlockProvider provider)
        {
            m_Provider = provider;
            m_Provider.Viewer.Add(this.viewSetupGaugeInterlock);
            this.viewSetupGaugeInterlock.InitDataView(provider.Adapter.Table);
        }
        #endregion
    }
}
