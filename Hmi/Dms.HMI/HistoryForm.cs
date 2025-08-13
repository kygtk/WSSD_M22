using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Device; 
using Dms.Server; 
using Dms.Data;
using Dms.Client;
using Dms.Common;
using Dms.Control;

namespace Dms.HMI
{
    public partial class HistoryForm : Form
    {
        private ServerManager m_Server = null;
        private GlassApdHistoryProvider m_GlassApdHistoryProvider = null;
        private HistoryTabGlassApdLog m_HistoryTabGlassApdLog;

        public HistoryForm()
        {
            m_Server = ServerManager.Instance;
            m_GlassApdHistoryProvider = m_Server.DataProvider.GlassApdDataHistoryProvider;

            InitializeComponent();
            

            m_HistoryTabGlassApdLog = new HistoryTabGlassApdLog();
            this.tabPageGlassApdHistoryLog.Controls.Add(m_HistoryTabGlassApdLog);
            m_HistoryTabGlassApdLog.Initialize(m_GlassApdHistoryProvider);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
        }

        private void HistoryForm_Activated(object sender, EventArgs e)
        {
        }
    }
}