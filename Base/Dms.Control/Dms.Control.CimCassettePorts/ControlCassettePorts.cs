using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Loader;
using Dms.Cim.Common;
using Dms.Common;
using Dms.Ctl;

namespace Dms.Control
{
    public partial class ControlCassettePorts : UserControl
    {
        #region Fields
        private bool m_ComponentsInitComplete = false;
        private ILoader m_Loader = null;
        private HostInfo m_HostInfo;
        private BindingSource m_BindSrc = new BindingSource();
        private List<TabPage> m_TabsPages = new List<TabPage>();
        private List<ControlCassettePort> m_PortControls = new List<ControlCassettePort>();
        #endregion

        #region Constructor
        public ControlCassettePorts()
        {
            InitializeComponent();
        }
        #endregion

        #region
        public int PortIndex
        {
            get { return this.tabControl1.SelectedIndex; }
        }
        #endregion

        #region Methods
        public void Initialize(ILoader loader, HostInfo info)
        {
            m_Loader = loader;
            m_HostInfo = info;
            InitTabPages();
        }

        private void InitTabPages()
        {
            if (m_Loader == null || m_ComponentsInitComplete) return;

            #region DataGridView data binding
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            m_BindSrc.DataSource = m_Loader.GetPortsInfo();
            dataGridView1.DataSource = m_BindSrc;
            #endregion

            #region TabPage Initailize & data binding
            int portCount = m_Loader.PortCount;
            for (int i = 0; i < portCount; i++)
            {
                ControlCassettePort cstPort = new ControlCassettePort(m_Loader, i, m_HostInfo);
                m_PortControls.Add(cstPort);
                m_TabsPages.Add(new TabPage());

                this.tabControl1.Controls.Add(m_TabsPages[i]);
                m_TabsPages[i].Name = string.Format("Port{0}", i + 1);
                m_TabsPages[i].Padding = new System.Windows.Forms.Padding(3);
                m_TabsPages[i].Text = string.Format("Port{0}", i + 1);
                m_TabsPages[i].TabIndex = i;
                m_TabsPages[i].UseVisualStyleBackColor = true;

                m_TabsPages[i].Controls.Add(m_PortControls[i]);
                m_PortControls[i].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                m_PortControls[i].BackColor = System.Drawing.Color.Transparent;
                m_PortControls[i].Location = new System.Drawing.Point(0, 0);
                m_PortControls[i].Name = string.Format("controlCassettePort{0}", i + 1);
                m_PortControls[i].Size = m_TabsPages[i].Size - new System.Drawing.Size(0, 0);
                m_PortControls[i].TabIndex = 0;
            }
            m_ComponentsInitComplete = true;
            #endregion
        }

        /// <summary>
        /// Tabpage내의 Glass Information을 표시하는 Gridview의 data를 Update합니다.
        /// </summary>
        /// <param name="portNo">Update하고자 하는 port number</param>
        public void UpdatePortInfoAndGrid(int portNo)
        {
            if (portNo >= m_PortControls.Count || portNo < 0) return;

            CassettePort portinfo = m_Loader.GetPortInfo(portNo);

            m_PortControls[portNo].InitGridView(portinfo);
            m_PortControls[portNo].InitData(portinfo);

            // DataGridView data binding
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            m_BindSrc.DataSource = m_Loader.GetPortsInfo();
            dataGridView1.DataSource = m_BindSrc;
        }

        public void SelectPortAndMode(int portno)
        {
            if (portno > m_Loader.PortCount) return;

            int nPortNo = portno - 1;

            this.tabControl1.SelectedIndex = nPortNo;

            if (m_Loader.LoaderType == LoaderType.Loader)
            {
                m_PortControls[nPortNo].SetModify(true);
            }
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}