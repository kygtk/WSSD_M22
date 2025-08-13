using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Common;

namespace Dms.Server
{
    public partial class FormSetup : Form
    {
        private ServerManager m_Server = ServerManager.Instance;
        private SetupGenInfoProvider m_SetupGenInfo;
        private SetupIdleInfoProvider m_SetupIdleInfo;
        private SetupGaugeInterlockProvider m_SetupGaugeInterlock;
        private SetupSensorInterlockProvider m_SetupSensorIntr;
        private SetupCvInfoProvider m_SetupCvInfo;
        private SetupCvDistanceProvider m_SetupCvDistance;
        private SetupSensorTimeoutProvider m_SetupSensorTimeout; 
        private CalibrationProvider m_CalProvider;
        private SetupTankLevelProvider m_SetupTankLevel;



        public FormSetup()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void InitObject()
        {
            try
            {
                m_SetupGenInfo = m_Server.DataProvider.SetupGenInfo;
                m_SetupGenInfo.Viewer.Clear();
                m_SetupGenInfo.Viewer.Add(this.viewSetupGenInfo);
                this.viewSetupGenInfo.InitDataView(m_SetupGenInfo.Adapter.Table, true);

                m_SetupIdleInfo = m_Server.DataProvider.SetupIdleInfo;
                m_SetupIdleInfo.Viewer.Clear();
                m_SetupIdleInfo.Viewer.Add(this.viewSetupIdleInfo);
                this.viewSetupIdleInfo.InitDataView(m_SetupIdleInfo.Adapter.Table, true);

                m_SetupGaugeInterlock = m_Server.DataProvider.SetupGaugeInterlock;
                m_SetupGaugeInterlock.Viewer.Clear();
                m_SetupGaugeInterlock.Viewer.Add(this.viewSetupGaugeInterlock);
                this.viewSetupGaugeInterlock.InitDataView(m_SetupGaugeInterlock.Adapter.Table);

                m_SetupCvInfo = m_Server.DataProvider.SetupCvInfo;
                m_SetupCvInfo.Viewer.Clear();
                m_SetupCvInfo.Viewer.Add(this.viewSetupCvInfo);
                this.viewSetupCvInfo.InitDataView(m_SetupCvInfo.Adapter.Table);

                m_SetupSensorIntr = m_Server.DataProvider.SetupSensorIntr;
                m_SetupSensorIntr.Viewer.Clear();
                m_SetupSensorIntr.Viewer.Add(this.viewSetupSensorInterlock);
                this.viewSetupSensorInterlock.InitDataView(m_SetupSensorIntr.Adapter.Table);

                m_SetupCvDistance = m_Server.DataProvider.SetupCvDistance;
                m_SetupCvDistance.Viewer.Clear();
                m_SetupCvDistance.Viewer.Add(this.viewSetupCvDistance);
                this.viewSetupCvDistance.InitDataView(m_SetupCvDistance.Adapter.Table, true);

                m_SetupSensorTimeout = m_Server.DataProvider.SetupSensorTimeout;
                m_SetupSensorTimeout.Viewer.Clear();
                m_SetupSensorTimeout.Viewer.Add(this.viewSetupSensorTimeout);
                this.viewSetupSensorTimeout.InitDataView(m_SetupSensorTimeout.Adapter.Table, false);

                m_CalProvider = m_Server.DataProvider.CalibrationProvider;
                this.viewCalibration.InitDataView(m_CalProvider.Adapter.Table);

                m_SetupTankLevel = m_Server.DataProvider.SetupTankLevel;
                m_SetupTankLevel.Viewer.Clear();
                m_SetupTankLevel.Viewer.Add(this.viewSetupTankLevel);
                this.viewSetupTankLevel.InitDataView(m_SetupTankLevel.Adapter.Table, true);

                m_Server.DataProvider.SetEditPermission(UserLevels.Administrator);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }            
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSaveGen_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupGenInfo.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSaveIdle_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupIdleInfo.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSaveCv_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupCvInfo.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSaveCal_Click(object sender, EventArgs e)
        {
            try
            {
                m_CalProvider.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSaveSenIntr_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupSensorIntr.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSaveDistance_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupCvDistance.UpdateToDB();
                m_SetupSensorTimeout.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSaveGaugeInterlock_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupGaugeInterlock.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonTankLevel_Click(object sender, EventArgs e)
        {
            try
            {
                m_SetupTankLevel.UpdateToDB();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }
    }
}