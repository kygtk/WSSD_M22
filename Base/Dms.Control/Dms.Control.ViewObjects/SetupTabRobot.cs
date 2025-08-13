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
    public partial class SetupTabRobot : UserControl
    {
        #region Fields
        private SetupRobotInfoProvider m_RobotInfoProvider;
        #endregion

        #region Constructor
        public SetupTabRobot()
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
        public void Initialize(SetupRobotInfoProvider provider)
        {
            m_RobotInfoProvider = provider;
            m_RobotInfoProvider.Viewer.Add(this.viewSetupRobot);
            this.viewSetupRobot.InitDataView(provider.Adapter.Table, true);
        }
        #endregion
    }
}
