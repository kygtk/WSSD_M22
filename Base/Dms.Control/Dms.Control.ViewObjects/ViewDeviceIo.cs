///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.26
// Author       : eun
// Description  : ViewDeviceIo UserControl
//-------------------------------------------------------------------------
// Revison History
// * 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Dms.Device;
using Dms.Util.IODefine;
using Dms.ServerCommon;

namespace Dms.Control
{
    public partial class ViewDeviceIo : DmsUserControl
    {
        public enum IoBindingMode
        {
            AutoGenerate,   //지정된 Device의 I/O를 자동으로 등록
            UserDefine      //Device를 지정하지않고 수동으로 등록
        }

        #region Fields
        private IoType m_IoType;
        private ArrayList m_IoList = new ArrayList();
        private _DeviceAsm m_Device;
        private int m_ListCount;
        private Color m_OnColor = Color.GreenYellow;
        private Color m_OffColor = Color.White;
        private bool m_IdVisible = true;
        private BorderStyle m_GridBorderStyle = BorderStyle.FixedSingle;
        private IoBindingMode m_IoBindMode = IoBindingMode.AutoGenerate;
        #endregion

        #region Properties
        [Category("DMS : Option")]
        public IoType IoType
        {
            get { return m_IoType; }
            set { m_IoType = value; }
        }
        [Category("DMS : Option")]
        public IoBindingMode IoBind
        {
            get { return m_IoBindMode; }
            set { m_IoBindMode = value; }
        }
        [Category("DMS : UI")]
        public bool IdVisible
        {
            get { return m_IdVisible; }
            set { m_IdVisible = value; }
        }
        [Category("DMS : UI")]
        public BorderStyle GridBorderStyle
        {
            get { return m_GridBorderStyle; }
            set
            {
                m_GridBorderStyle = value;
                this.labelTitle.BorderStyle = m_GridBorderStyle;
                this.dataGridView.BorderStyle = m_GridBorderStyle;
            }
        }
        [Category("DMS : UI")]
        public string Title
        {
            get { return labelTitle.Text; }
            set { labelTitle.Text = value; }
        }
        [Category("DMS : UI")]
        public bool TitleVisible
        {
            get { return this.labelTitle.Visible; }
            set
            {
                this.labelTitle.Visible = value;
                int tileHeight = 0;
                if (value)
                {
                    tileHeight = 25;
                }
                splitContainer1.SplitterDistance = tileHeight;
            }
        }
        [Category("DMS : UI")]
        public ContentAlignment TitleTextAlign
        {
            get { return this.labelTitle.TextAlign; }
            set { this.labelTitle.TextAlign = value; }
        }
        [Category("DMS : UI")]
        public Font TitleTextFont
        {
            get { return this.labelTitle.Font; }
            set { this.labelTitle.Font = value; }
        }
        [Category("DMS : UI")]
        public Font ContentsFont
        {
            get { return this.dataGridView.DefaultCellStyle.Font; }
            set { this.dataGridView.DefaultCellStyle.Font = value; }
        }
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
        #endregion

        #region Constructor
        public ViewDeviceIo()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        public override bool Initialize(DeviceTags tagContainer)
        {
            bool ok = true;
            if (m_IoBindMode == IoBindingMode.AutoGenerate)
            {
                ok &= base.Initialize(tagContainer);
            }
            else
            {
                SetDoubleBuffer();
                this.tmrUpdateState = new Timer();
                this.tmrUpdateState.Tick += new EventHandler(tmrUpdateState_Tick);
            }

            if (ok)
            {
                if (!Initialize()) return false;

                this.tmrUpdateState.Interval = 100;
                this.tmrUpdateState.Enabled = true;

                m_Initialized = true;
            }

            return m_Initialized;
        }

        private bool Initialize()
        {
            if (m_IoBindMode == IoBindingMode.AutoGenerate)
            {
                m_Device = DmsComponents.Instance.ComponentContainer[m_Tag.DeviceName] as _DeviceAsm;
                if (m_Device == null)
                {
                    string msg = string.Format("Tag of {0} does not exist", this.Name);
                    MessageBox.Show(msg);
                    return false;
                }
            }

            InitData();

            return true;
        }

        private void InitData()
        {
            if (m_IoBindMode == IoBindingMode.AutoGenerate)
            {
                m_IoList = m_Device.GetAssociatedIoDevices(m_IoType);
            }

            if (m_IoList == null) return;

            m_ListCount = m_IoList.Count;

            if (m_IoType == IoType.DI || m_IoType == IoType.DO)
            {
                dataGridView.ColumnCount = 2;
            }
            else
            {
                dataGridView.ColumnCount = 3;
            }

            dataGridView.ColumnHeadersVisible = false;

            dataGridView.RowCount = m_ListCount;
            IoItem item = null;
            for (int i = 0; i < m_ListCount; i++)
            {
                item = (m_IoList[i] as _DeviceIo).GetIoInfo();
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    dataGridView[0, i].Value = item.Id;
                }
                else
                {
                    dataGridView[0, i].Value = item.WiringNo;
                }
                dataGridView[1, i].Value = item.Name;
            }

            UpdateState();

            int count = dataGridView.ColumnCount;
            for (int i = 0; i < count; i++)
            {
                if (i == 1) //IoName
                {
                    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else    // Address, Value
                {
                    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (i == 0) //id
                    {
                        dataGridView.Columns[i].Visible = m_IdVisible;
                    }
                }
            }

            dataGridView.ClearSelection();
        }

        protected override void UpdateState()
        {
            string value = "";

            if (m_IoType == IoType.DI || m_IoType == IoType.DO)
            {
                for (int i = 0; i < m_ListCount; i++)
                {
                    _DeviceIo io = (m_IoList[i] as _DeviceIo);
                    if (io != null)
                    {
                        value = io.GetIoStateString();
                        if (value == bool.TrueString) dataGridView[1, i].Style.BackColor = m_OnColor;
                        else dataGridView[1, i].Style.BackColor = m_OffColor;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_ListCount; i++)
                {
                    _DeviceIo io = (m_IoList[i] as _DeviceIo);
                    if (io != null)
                    {
                        dataGridView[2, i].Value = io.GetIoStateString();
                    }
                }
            }
        }

        public void SetMonitorTimer(bool enable)
        {
            if (tmrUpdateState == null) return;
            this.tmrUpdateState.Enabled = enable;
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }

        public void AddDeviceIo(_DeviceIo io, string name)
        {
            if (m_IoBindMode != IoBindingMode.UserDefine)
                return;

            _DeviceIo ioTemp = io.Clone();
            ioTemp.GetIoInfo().Name = name;
            ioTemp.Name = name;
            m_IoList.Add(ioTemp);

        }

        public void AddDeviceIo(_DeviceIo io)
        {
            if (m_IoBindMode != IoBindingMode.UserDefine)
                return;

            m_IoList.Add(io);
        }
        #endregion
    }
}
