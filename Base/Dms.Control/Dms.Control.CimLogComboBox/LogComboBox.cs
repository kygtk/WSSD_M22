using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Dms.Control
{
    public partial class LogComboBox : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private int m_MaxCount = 30;
        private bool m_CheckEnable = true;
        private Queue<string> m_LogQueue = null;
        private string OldMsg = "";
        #endregion

        #region properties
        [Category("DMS : UI"),
         DefaultValue(30),
         Description("Set max item count in ComboBox")]
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
        [Browsable(false)]
        public ComboBox LogCbBox
        {
            get { return cbLogList; }
        }
        #endregion

        #region Constructor
        public LogComboBox()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #endregion

        #region Override
        public bool Initialize(Queue<string> log)
        {
            m_LogQueue = log;

            tmrUpdate.Interval = 200;
            tmrUpdate.Enabled = true;

            m_Initialized = true;

            return m_Initialized;
        }
        #endregion

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (!m_Initialized) return;

            if (m_LogQueue.Count > 0)
            {
                bool checkCond = true;
                checkCond &= m_CheckEnable;

                if (checkCond)
                {
                    string msg = m_LogQueue.Dequeue();

                    if (msg != null)
                    {
                        if (msg != OldMsg)
                        {
                            if (cbLogList.Items.Count > m_MaxCount) cbLogList.Items.RemoveAt(0);
                            cbLogList.Items.Add(msg);
                            cbLogList.SelectedIndex = cbLogList.Items.Count - 1;
                            OldMsg = msg;
                        }
                    }
                }
            }
        }
    }
}
