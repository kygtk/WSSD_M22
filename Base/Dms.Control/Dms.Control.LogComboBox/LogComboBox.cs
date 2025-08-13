///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.17
// Author       : eun
// Description  : Log List UserControl
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(LogComboBox), "LogListIcon")]
    public partial class LogComboBox : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private int m_MaxCount = 30;
        private bool m_CheckEnable = true;
        private bool m_ClearSelection = false;
        #endregion

        #region properties
        [Category("DMS : UI"),
         DefaultValue(30),
         Description("Set max item count in ListBox")]
        public int MaxItemCount
        {
            get { return m_MaxCount; }
            set { m_MaxCount = value; }
        }
        [Category("DMS : UI")]
        public bool CheckEnable
        {
            get { return m_CheckEnable; }
            set { m_CheckEnable = value; }
        }
        [Category("DMS : UI")]
        public bool ClearSelection
        {
            get { return m_ClearSelection; }
            set { m_ClearSelection = value; }
        }
        #endregion

        #region Constructor
        public LogComboBox()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        } 
        #endregion

        #region Methods

        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Initialized = ok;

                tmrUpdateState.Interval = 200;
                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.VALUE] != null)
            {
                bool checkCond = true;
                checkCond &= (m_Tag[tagDescriptor.VALUE].Value != null);
                checkCond &= m_CheckEnable;

                if (checkCond)
                {
                    string msg = m_Tag[tagDescriptor.VALUE].Value;
                    if (msg == bool.TrueString || msg == bool.FalseString || msg == "0") return;
                    if (lstCombo.Items.Count > m_MaxCount) lstCombo.Items.RemoveAt(0);
                    lstCombo.Items.Add(msg);
                    lstCombo.SelectedIndex = lstCombo.Items.Count - 1;
                }
            }
        }
        #endregion

        private void lstLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            //for change selected color, but it doesn't work
            //int i = lstLog.SelectedIndex;
            //Rectangle rc = lstLog.GetItemRectangle(i);
            //SolidBrush myBrush = new SolidBrush(Color.LightYellow);
            //Graphics g = Graphics.FromHwnd(lstLog.Handle);
            //g.FillRectangle(myBrush, rc);
            if (m_ClearSelection == true)
            {
                lstCombo.Items.Clear();
            }
        }
    }
}
