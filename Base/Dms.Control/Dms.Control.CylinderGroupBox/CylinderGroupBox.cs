using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class CylinderGroupBox : GroupBox
    {
        #region Enum
        public enum DirectionType
        {
            FW_BW, UP_DOWN, LEFT_RIGHT, OPEN_CLOSE
        }
        #endregion

        #region Fields
        private DeviceTags m_Tags = null;
        private ClientManager m_Client = null;
        private DirectionType m_Type = DirectionType.FW_BW;
        #endregion

        #region Properties
        [Category("DMS : UI"),
         Description("Select type of Cylinder")]
        public DirectionType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region Constructor
        public CylinderGroupBox()
        {
            InitializeComponent();
            this.Click += new EventHandler(CylinderGroupBox_Click);
        }
        #endregion

        #region Methods
        void CylinderGroupBox_Click(object sender, EventArgs e)
        {
            if (m_Client == null) m_Client = ClientManager.Instance;
            if (m_Client.GenInfos.AutoMode) return;
            if (m_Tags == null) GetCurrentTags();

            if (m_Tags.Count != 0)
            {
                CylinderAllOperateForm dlg = new CylinderAllOperateForm(m_Tags, m_Type);
                dlg.Caption = this.Name;
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog甫 荤侩窍咯 汽阑 钎矫茄 版快, Dispose甫 龋免窍咯 汽狼 葛电 牧飘费阑 啊厚瘤 荐笼贸府.	 
                dlg.Dispose();
            }
        }

        private void GetCurrentTags()
        {
            m_Tags = new DeviceTags();
            DeviceTags tagContainer = m_Client.TagContainer;
            DmsUserControl userControl;
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                userControl = control as DmsUserControl;
                if (userControl != null)
                {
                    DeviceTag tag = tagContainer.GetTag(userControl.DeviceTagInfo.DeviceName);
                    if (tag == null)
                    {
                        string msg = string.Format("Tag of {0} is not defined", userControl.DeviceTagInfo.DeviceName);
                        MessageBox.Show(msg);
                        return;
                    }
                    m_Tags.Add(tag);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 사용자 지정 그리기 코드를 여기에 추가합니다.
            if (this.Text != "") this.Text = "";
            // 기본 클래스 OnPaint를 호출하고 있습니다.
            base.OnPaint(pe);
        }
        #endregion
    }
}
