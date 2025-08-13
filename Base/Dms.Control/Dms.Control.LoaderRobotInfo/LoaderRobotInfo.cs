using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Dms.Client;
using Dms.Device;

namespace LoaderRobotInfo
{
    public partial class LoaderRobotInfo : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorLoaderRobot tagDescriptor = new TagDescriptorLoaderRobot();
        #endregion

        #region Fields
        private string m_UpperHandGlsData;
        private string m_LowerHandGlsData;
        #endregion
        
        public LoaderRobotInfo()
        {
            InitializeComponent();
            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
   
        #region Methods
        private void Initialize()
        {
        }

        private void UpdateHandGlsData()
        {
            string glsData = "";

            if(System.Convert.ToInt32(m_Tag[tagDescriptor.UpperHandPortNo].Value) < (int)nxOP_CUBE.PORT1)
                glsData = "";
            else
                glsData = string.Format("{0:0} / {1:00}", System.Convert.ToInt32(m_Tag[tagDescriptor.UpperHandPortNo].Value),
                                            System.Convert.ToInt32(m_Tag[tagDescriptor.UpperHandSlotNo].Value));

            if(m_UpperHandGlsData != glsData)
            {
                m_UpperHandGlsData = glsData;
                lblValue1.Text = m_UpperHandGlsData;
            }

            if(System.Convert.ToInt32(m_Tag[tagDescriptor.LowerHandPortNo].Value) < (int)nxOP_CUBE.PORT1)
                glsData = "";
            else
                glsData = string.Format("{0:0} / {1:00}", System.Convert.ToInt32(m_Tag[tagDescriptor.LowerHandPortNo].Value),
                                            System.Convert.ToInt32(m_Tag[tagDescriptor.LowerHandSlotNo].Value));

            if(m_LowerHandGlsData != glsData)
            {
                m_LowerHandGlsData = glsData;
                lblValue2.Text = m_LowerHandGlsData;
            }
        }
        #endregion

        #region override
        public override bool Initialize(DeviceTags tagContainer)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            bool ok = base.Initialize(tagContainer);
            if(ok)
            {
                Initialize();

                // Timer를 설정한다.
                this.tmrUpdateState = new System.Windows.Forms.Timer();
                this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
                this.tmrUpdateState.Enabled = true;

                m_Initialized = true;
                UpdateState();
            }

            return m_Initialized;
        }
    
        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }
   
        protected override void UpdateState()
        {
            if(!m_Initialized) return;
            try
            {
                UpdateHandGlsData();
            }
            catch(Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion
    }
}
