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
    public partial class TempStatus : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorHeaterUnit tagDescriptor = new TagDescriptorHeaterUnit();
        #endregion

        #region Fields
        private int m_MonitorNo;
        private string[] m_TempList; 
        #endregion

        #region Properties
        [Category("DMS : Setting"), Description("Monitor No")]
        public int MonitorNo
        {
            get { return m_MonitorNo; }
            set { m_MonitorNo = value; }
        } 
        #endregion

        public TempStatus()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                if (m_Tag[tagDescriptor.TEMP_LIST].Value != null)
                {
                    m_TempList = m_Tag[tagDescriptor.TEMP_LIST].Value.Split('*');
                }

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.TEMP_LIST].Value != null)
            {
                string temps = m_Tag[tagDescriptor.TEMP_LIST].Value;
                m_TempList = temps.Split('*');
            }

            if (m_MonitorNo > m_TempList.Length) return;

            lblTemp.Text = m_TempList[m_MonitorNo - 1] + "℃";
        }
        #endregion

    }
}
