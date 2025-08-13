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
using Dms.Cim.Common;

namespace Dms.Control
{
    public partial class CimLogList : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private int m_MaxCount = 30;
        private bool m_CheckEnable = true;
        private Queue<string> m_LogListQueue = null;
        private string OldMsg = "";
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
        #endregion

        #region Constructor
        public CimLogList()
        {
            InitializeComponent();
        } 
        #endregion

        #region Methods

        #endregion

        #region Override
        public bool Initialize(Queue<string> queue)
        {
            m_LogListQueue = queue;

            tmrUpdateState.Interval = 50;
            tmrUpdateState.Enabled = true;

            m_Initialized = true;

            return m_Initialized;
        }
        #endregion

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (!m_Initialized) return;

            if (m_LogListQueue.Count > 0)
            {
                bool checkCond = true;
                checkCond &= m_CheckEnable;

                if (checkCond)
                {
                    string msg = m_LogListQueue.Dequeue();

                    if (msg != null)
                    {
                        if (msg != OldMsg)
                        {
                            if (lstLog.Items.Count > m_MaxCount) lstLog.Items.RemoveAt(0);
                            lstLog.Items.Add(msg);
                            lstLog.SetSelected(lstLog.Items.Count - 1, true);
                            OldMsg = msg;
                        }
                    }
                }
            }
        }
    }
}
