using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class PipingA : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorValve tagDescriptor = new TagDescriptorValve();     
        #endregion

        #region Fields
        private Color m_OnColor;
        private Color m_OffColor;
        #endregion

        [Category("DMS : UI")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }
        [Category("DMS : UI")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        
        public PipingA()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo("AutoValve");
        }
        
        #region override
        public override bool Initialize(DeviceTags tags)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Initialized = ok;
                
                tmrUpdateState.Enabled = true;

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
            try
            {
                if (!m_Initialized) return;

                if (m_Tag[tagDescriptor.RUN].Value == Boolean.TrueString || m_Tag[tagDescriptor.RUN].Value == "1")
                {
                    this.BackColor = OnColor;
                }
                else
                {
                    this.BackColor = OffColor;
                }               
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion
    }
}