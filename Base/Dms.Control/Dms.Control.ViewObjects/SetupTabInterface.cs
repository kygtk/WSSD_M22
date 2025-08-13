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
    public partial class SetupTabInterface : UserControl
    {
        #region Fields
        private SetupInterfaceProvider m_InterfaceProvider;
        #endregion

        #region Constructor
        public SetupTabInterface()
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
        public void Initialize(SetupInterfaceProvider provider)
        {
            m_InterfaceProvider = provider;
            m_InterfaceProvider.Viewer.Add(this.viewSetupInterface);
            this.viewSetupInterface.InitDataView(provider.Adapter.Table, true);
        }
        #endregion
    }
}
