///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Tank UserControl (VerticalProgressBar + Label)
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

namespace Dms.Control
{
    public partial class Tank : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorTankUnit tagDescriptor = new TagDescriptorTankUnit();
        #endregion

        #region Fields
        private SetupTankLevelProvider m_TankLevelProvider = null;
        private TagSetupInfo m_SetupInfo = null;
        private int[] m_SetupValues;
        private int m_LevelCount = 0;
        private string[] m_Levels = { " LL", " L", " M", " H", " HH" };
        #endregion

        #region Properties
        /// <summary>
        /// Setup Values from server - eun 20080115
        /// </summary>
        //[Browsable(false)]
        //public int[] SetupValues    //0 : LL, 1 : L, 2 : MIDDLE, 3 : TOP    //이 연결은 HMI에서?
        //{
        //    get { return m_SetupValues; }
        //    set { m_SetupValues = value; }
        //}
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Tank contstructor - eun 20080115
        /// </summary>
        public Tank()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        public bool Initialize(DeviceTags tags, SetupTankLevelProvider tankLevelProvider)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_LevelCount = Convert.ToInt32(m_Tag[tagDescriptor.LEVELS].Value);
                m_SetupValues = new int[m_LevelCount];

                m_TankLevelProvider = tankLevelProvider;

                //for get setupvalues from db - eun 20080116
                for (int i = 0; i < m_LevelCount; i++)
                {
                    m_SetupInfo = new TagSetupInfo(m_Tag[tagDescriptor.TANKLEVELNAME].Value + m_Levels[i], OptionType.None, OptionFormat.Digit, UnitType.L, (i * 20).ToString());
                    //m_TankLevelProvider.InitFromDB(m_SetupInfo);
                    m_TankLevelProvider.UpdateFromDB(m_SetupInfo.Name, m_SetupInfo);
                    m_SetupValues[i] = m_SetupInfo.GetValue<int>();
                }

                pbTank.Maximum = m_SetupValues[m_LevelCount - 1];

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        private void Tank_Load(object sender, EventArgs e)
        {
            pbTank.Controls.Add(lblStatus);
            lblStatus.BackColor = Color.FromArgb(0, lblStatus.BackColor);   //for transparent label
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tagContainer)
        {
            ClientManager manager = ClientManager.Instance;
            m_TankLevelProvider = manager.SetupTankLevelProvider;

            bool ok = this.Initialize(tagContainer, m_TankLevelProvider);
            
            return ok;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_LevelCount >= m_Levels.Length)
            {
                pbTank.Value = m_SetupValues[4];
                pbTank.FillColor = Color.Red;
                lblStatus.Text = "TOP";
            }

            if (m_Tag[tagDescriptor.SUPPLYSTOP].Value == bool.TrueString || m_Tag[tagDescriptor.SUPPLYSTOP].Value == "1")
            {
                pbTank.Value = m_SetupValues[3];
                pbTank.FillColor = Color.Orange;
                lblStatus.Text = "HIGH";
            }
            else if (m_Tag[tagDescriptor.SUPPLYREQUEST].Value == bool.TrueString || m_Tag[tagDescriptor.SUPPLYREQUEST].Value == "1")
            {
                pbTank.Value = m_SetupValues[2];
                pbTank.FillColor = Color.LightSteelBlue;
                lblStatus.Text = "MIDDLE2";
            }   
            else if (m_Tag[tagDescriptor.RUNENABLE].Value == bool.TrueString || m_Tag[tagDescriptor.RUNENABLE].Value == "1")
            {
                pbTank.Value = m_SetupValues[1];
                pbTank.FillColor = Color.Coral;
                lblStatus.Text = "MIDDLE1";
            }
            else if (m_Tag[tagDescriptor.BOTTOMLEVEL].Value == bool.TrueString || m_Tag[tagDescriptor.BOTTOMLEVEL].Value == "1")
            {
                pbTank.Value = m_SetupValues[0];
                pbTank.FillColor = Color.Orange;
                lblStatus.Text = "LOW";
            }
            else
            {
                pbTank.Value = 0;
                pbTank.FillColor = Color.Red;
                lblStatus.Text = "BOTTOM";
            }
        }
        #endregion
    }
}