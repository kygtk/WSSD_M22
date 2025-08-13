using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using System.Xml.Serialization;

namespace Dms.Control
{
    public partial class ActuatorTurn : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorActuatorTurn tagDescriptor = new TagDescriptorActuatorTurn();
        #endregion

        #region Fields
        protected string[] m_PositionList;
        private ClientManager m_Client = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public string[] PositionList
        {
            get { return m_PositionList; }
        }
        #endregion

        #region Constructor
        public ActuatorTurn()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        } 
        #endregion

        private void lblTitle_Click(object sender, EventArgs e)
        {
            ControlClick();
        }

        private void lblValue_Click(object sender, EventArgs e)
        {
            ControlClick();
        }

        private void ControlClick()
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                DlgActuatorTurnOperate dlg = new DlgActuatorTurnOperate();
                dlg.Initialize(this, m_Client.GenInfos.AutoMode);
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.
                dlg.Dispose();
            }
            else MessageBox.Show("Tag is not selected.");
        }

        #region Override
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            double pos = Convert.ToDouble(m_Tag[tagDescriptor.CUR_POS].Value);
            int index = (int)pos;

            if (index >= 0 && index < m_PositionList.Length)
            {
                this.lblValue.Text = m_PositionList[index];
            }
            else
            {
                this.lblValue.Text = "UnKnown";
            }

            bool alarm = Convert.ToBoolean(m_Tag[tagDescriptor.ALARM].Value) || (m_Tag[tagDescriptor.ACT_STATUS].Value == ActuatorAct.EStop.ToString());
            bool moving = (m_Tag[tagDescriptor.ACT_STATUS].Value != m_Tag[tagDescriptor.ACT_COMMAND].Value);
            if (alarm)
            {
                this.lblTitle.BackColor = Color.Red;
            }
            else
            {
                this.lblTitle.BackColor = Color.GreenYellow;
            }

            if (moving)
            {
                this.lblValue.BackColor = Color.Gold;
            }
            else
            {
                this.lblValue.BackColor = Color.White;
            }
        }

        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if(ok)
            {
                if (m_Tag[tagDescriptor.POSITION_LIST].Value != null)
                {
                    m_PositionList = m_Tag[tagDescriptor.POSITION_LIST].Value.Split('*');
                }

                this.lblTitle.Text = m_Tag.DeviceName;

                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }
        #endregion
    }
}
