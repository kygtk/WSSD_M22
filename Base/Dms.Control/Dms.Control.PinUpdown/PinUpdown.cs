using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using Dms.Client;
using Dms.Server;

namespace Dms.Control
{
    public partial class PinUpdown : DmsUserControl
    {
        #region Tag Descriptor
        private TagDescriptorServoUnit tagDescriptor = new TagDescriptorServoUnit();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        private ServoUnit m_ServoUnit;
        #endregion

        public PinUpdown()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo("ServoUnit");
        }

        #region Methods
        private bool Initialize()
        {
            m_Client = ClientManager.Instance;
            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;

            m_ServoUnit = components[m_TagInfo.DeviceName] as ServoUnit;

            if (m_ServoUnit == null) return false;

            return true;
        }
        #endregion

        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            if (!Initialize()) return false;

            m_Initialized = true;
            tmrUpdateState.Enabled = true;
            return m_Initialized;
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            DlgPinUpdown dlgPinUpdown = new DlgPinUpdown();
            dlgPinUpdown.Initialize(m_Tag);
            dlgPinUpdown.ShowDialog();
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            //int curPointId = m_ServoUnit.GetCurPointId();

            //if (curPointId >= 0 && curPointId < m_ServoUnit.TeachPointName.Length)
            //{
            //    string curPointName = m_ServoUnit.TeachPointName[curPointId];

            //    if (curPointName == "UP") lblUp.BackColor = Color.LawnGreen;
            //    else lblUp.BackColor = Color.White;

            //    if (curPointName == "DOWN") lblDn.BackColor = Color.LawnGreen;
            //    else lblDn.BackColor = Color.White;
            //}
            //else
            //{
            //    lblUp.BackColor = Color.White;
            //    lblDn.BackColor = Color.White;
            //}
        }
    }
}
