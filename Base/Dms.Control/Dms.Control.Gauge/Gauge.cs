///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.14
// Author       : eun
// Description  : Gauge UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using Dms.Client;
using System.Xml.Serialization;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(Gauge), "GaugeAni.bmp")]
    public partial class Gauge : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorGauge tagDescriptor = new TagDescriptorGauge();
        #endregion

        public enum ImageType
        {
            Default, New
        }

        #region Fields
        private ClientManager m_Client = null;
        private TagCalibrationInfo m_CalibraionInfo = null;
        private CalibrationProvider m_CalibrationProvider = null;
        private ImageType m_GaugeImageType = ImageType.Default;
        #endregion

        #region Properties
        /// <summary>
        /// DlgCalibration display this value - eun 20080115
        /// </summary>
        /// 
        [Category("DMS : UI")]
        public Color GaugeBackColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("DMS : UI")]
        public ImageType GaugeImageType
        {
            get { return m_GaugeImageType; }
            set
            {
                m_GaugeImageType = value;
                SetGaugeType();
            }
        }
        [Browsable(false), XmlIgnore()]
        public string CurValue
        {
            get { return m_Tag[tagDescriptor.CURVAL].Value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagCalibrationInfo CalibraionInfo
        {
            get { return m_CalibraionInfo; }
        }

        [Browsable(false), XmlIgnore()]
        public CalibrationProvider CalibrationProvider
        {
            get { return m_CalibrationProvider; }
        }

        /// <summary>
        /// DlgCalibration display this value - eun 20080115
        /// </summary>
        [Browsable(false), XmlIgnore()]
        public string CurAdc
        {
            get { return m_Tag[tagDescriptor.CURADC].Value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Gauge contstructor - eun 20080114
        /// </summary>
        public Gauge()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);

            //SetGaugeType();
        }
        #endregion

        #region Methods


        /// <summary>
        /// It should be called by HMI - eun 20080114
        /// </summary>
        /// <param name="tags"></param>
        public void Initialize(DeviceTags tags, CalibrationProvider calibrationProvider)
        {
            SetGaugeType();
            m_CalibrationProvider = calibrationProvider;
            m_CalibraionInfo = calibrationProvider.GetInfo(m_Tag.DeviceName);
        }

        public void SetGaugeType()
        {
            if (m_GaugeImageType == ImageType.Default)
            {
                this.Padding = new Padding(0);
                this.BorderStyle = BorderStyle.FixedSingle;
                this.lblValue.Dock = DockStyle.Fill;
                this.lblValue.BackColor = Color.White;
                this.lblValue.BorderStyle = BorderStyle.None;
            }
            else if (m_GaugeImageType == ImageType.New)
            {
                this.Padding = new Padding(3);
                this.BorderStyle = BorderStyle.None;
                this.lblValue.Dock = DockStyle.Fill;
                this.lblValue.BackColor = Color.White;
                this.lblValue.BorderStyle = BorderStyle.None;
            }
        }

        /// <summary>
        /// Set color of gauge - eun 20080115
        /// </summary>
        private void SetDisplay()
        {
            switch (m_CalibraionInfo.Type)
            {
                case GaugeType.PG35:
                    lblValue.BackColor = Color.LightCyan;
                    break;
                case GaugeType.CKDFlow:
                case GaugeType.ULK25:
                case GaugeType.JLK25:
                    lblValue.BackColor = Color.Pink;
                    break;
                case GaugeType.Sensys:
                    lblValue.BackColor = Color.LightYellow;
                    break;
                case GaugeType.GsEuv:
                    lblValue.BackColor = Color.MistyRose;
                    break;

                //  BM : AP Type Gauge 일괄 MediumPurple 설정
                case GaugeType.AP_DIW_Flow:
                case GaugeType.AP_DIW_Press:
                case GaugeType.AP_CDA_Flow:
                case GaugeType.AP_CDA_Press:
                case GaugeType.AP_SmartDamper_Pressure:
                case GaugeType.AP_SmartDamper_ValveAngle:
                case GaugeType.AP_LFC_Flow:
                case GaugeType.AP_LFC_Press:
                case GaugeType.AP_LFC_OpenRate:
                case GaugeType.AP_Manometer_Exhaust:
                case GaugeType.AP_LCT_Level1:
                case GaugeType.AP_LCT_Level2:
                case GaugeType.AP_LCT_Consistence:
                    lblValue.BackColor = Color.MediumPurple;
                    break;

                default:
                    lblValue.BackColor = Color.White;
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Show DlgCalibration dialog for calibration - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblValue_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                DlgCalibration dlg = new DlgCalibration(this);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK &&
                    m_CalibraionInfo != dlg.CalibrationInfo)
                {
                    m_CalibraionInfo.Clone(dlg.CalibrationInfo);
                    SetDisplay();
                    m_Client.SendCommand(Command.CalibrationSave, m_CalibraionInfo);
                }
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                dlg.Dispose();
            }
            else MessageBox.Show("Tag is not selected.");
        }

        #region Override
        public override bool Initialize(DeviceTags tagContainer)
        {
            bool ok = base.Initialize(tagContainer);

            if (ok)
            {
                m_Client = ClientManager.Instance;
                m_CalibrationProvider = m_Client.CalibrationProvider;
                this.Initialize(tagContainer, m_CalibrationProvider);

                m_Initialized = ok;

                tmrUpdateState.Interval = 500;
                tmrUpdateState.Enabled = ok;

                SetDisplay();

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_CalibraionInfo.Type == GaugeType.PCG550)
            {
                double u = Convert.ToDouble(m_Tag[tagDescriptor.CURVAL].Value);

                double value = Math.Pow(10, (0.778 * (u - 6.304)));

                lblValue.Text = string.Format("{0:F2} Torr", value);
            }
            else
            {
                lblValue.Text = m_Tag[tagDescriptor.CURVAL].Value + " " + m_CalibraionInfo.Unit.Name;
            }
        }
        #endregion
    }
}
