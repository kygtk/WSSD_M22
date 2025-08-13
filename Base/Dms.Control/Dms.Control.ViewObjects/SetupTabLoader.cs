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
    public partial class SetupTabLoader : UserControl
    {
        #region Fields
        private SetupLoaderInfoProvider m_LoaderInfoProvider;
        #endregion

        #region Constructor
        public SetupTabLoader()
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
        public void Initialize(SetupLoaderInfoProvider provider)
        {
            m_LoaderInfoProvider = provider;
            m_LoaderInfoProvider.Viewer.Add(this.viewSetupLoader);
            this.viewSetupLoader.InitDataView(provider.Adapter.Table, true);
        }
        #endregion
    }
}
