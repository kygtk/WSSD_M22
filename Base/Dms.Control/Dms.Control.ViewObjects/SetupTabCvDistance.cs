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
    public partial class SetupTabCvDistance : UserControl
    {
        #region Fields
        private SetupCvDistanceProvider m_CvDistanceProvider;
        private SetupSensorTimeoutProvider m_SensorTimeoutProvider; 
        #endregion

        #region Constructor
        public SetupTabCvDistance()
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
        public void Initialize(SetupCvDistanceProvider provider)
        {
            m_CvDistanceProvider = provider;
            m_CvDistanceProvider.Viewer.Add(this.viewSetupCvDistance);
            this.viewSetupCvDistance.InitDataView(provider.Adapter.Table, true);
        }

        public void Initialize(SetupSensorTimeoutProvider provider)
        {
            m_SensorTimeoutProvider = provider;
            m_SensorTimeoutProvider.Viewer.Add(this.viewSetupCvTimeout);
            this.viewSetupCvTimeout.InitDataView(provider.Adapter.Table, false);
        }
        #endregion
    }
}
