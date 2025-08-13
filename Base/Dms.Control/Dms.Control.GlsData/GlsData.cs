///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.14
// Author       : eun
// Description  : GlsData UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using Dms.Client;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(GlsData), "GlsDataAni.bmp")]
    public partial class GlsData : DmsUserControl
    {
        #region Fields
        private int m_PositionId = 0;
        private GlassDataProvider m_GlassDataProvider = null;
        private LostGlassDataProvider m_LostGlassDataProvider = null;
        private ClientManager m_Client;
        private bool m_CreateEnable = false;
        private bool m_RecoveryEnable = false;
        private bool m_RequestEnable = false;
        TagGlassData m_Data = new TagGlassData();
        private ulong oldPositionFlag = 0;
        //private static bool m_Editable = false;
        private bool m_Editable = false; // 11.02.09 minhan
        #endregion

        #region Delegates
        //public delegate void GlsDataEventHandler();
        public delegate void ParentOnPaint();

        //[Category("DMS : Event"),
        //Description("Handling GlsData click event")]
        //public event GlsDataEventHandler GlsDataClick;
        //public event ParentOnPaint ParentPaint;
        #endregion

        #region Properties
        [Category("DMS : Basic Info")]
        public int PositionId
        {
            get { return m_PositionId; }
            set { m_PositionId = value; }
        }
        [Category("DMS : Basic Info")]
        public bool CreateEnable
        {
            get { return m_CreateEnable; }
            set { m_CreateEnable = value; }
        }
        [Category("DMS : Basic Info")]
        public bool RecoveryEnable
        {
            get { return m_RecoveryEnable; }
            set { m_RecoveryEnable = value; }
        }
        [Category("DMS : Basic Info")]
        public bool RequestEnable
        {
            get { return m_RequestEnable; }
            set { m_RequestEnable = value; }
        }
        [Category("DMS : Basic Info")]
        public bool Editable
        {
            get { return m_Editable; }
            set { m_Editable = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter GlsData contstructor - eun 20080110
        /// </summary>
        public GlsData()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        public override bool Initialize(DeviceTags tagContainer)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            m_Client = ClientManager.Instance;
            m_GlassDataProvider = m_Client.GlassDataProvider;
            m_LostGlassDataProvider = m_Client.LostGlassDataProvider;

            this.Initialize(m_GlassDataProvider);

            return true;
        }

        /// <summary>
        /// It should be called by HMI - eun 20080110
        /// </summary>
        /// <param name="tags"></param>
        public void Initialize(GlassDataProvider dataProvider)
        {
            m_GlassDataProvider = dataProvider;

            //UpdateState(false);
            UpdateState();

            // Timer를 설정한다.
            tmrUpdateState = new System.Windows.Forms.Timer();
            tmrUpdateState.Tick += new System.EventHandler(tmrUpdateState_Tick);
            tmrUpdateState.Enabled = true;

            m_Initialized = true;
        }


        /// <summary>
        /// Update text and color of GlsData - eun 20080110
        /// </summary>
        //private void UpdateState(bool exist)
        //{
        //    if (exist == false)
        //    {
        //        if (lblData.Text != "")
        //        {
        //            lblData.Text = "";
        //            lblData.BackColor = Color.White;
        //        }
        //    }
        //    else
        //    {
        //        m_GlassDataProvider.GetData(m_PositionId, ref m_Data);

        //        if (lblData.Text != m_Data.Item.GlassNumberCode.SlotNo.ToString())
        //        {
        //            lblData.Text = m_Data.Item.GlassNumberCode.SlotNo.ToString();// m_Data.Item.GlassId;
        //            lblData.BackColor = Color.Yellow;
        //        }
        //    }
        //}

        /// <summary>
        /// Eventhandler for GlsData dialog - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblData_Click(object sender, System.EventArgs e)
        {
            if (m_GlassDataProvider == null) return;

            if (!IsExist())
            {
                if (m_Client.GenInfos.AutoMode) return;

                if (m_CreateEnable || m_RecoveryEnable || m_RequestEnable)
                {
                    if (m_RecoveryEnable &&
                        (MessageBox.Show("Do you want to request glass data from CIM?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)) // 11.02.01 minhan
                    {
                        m_Client.SendCommand(Command.GlassDataCreate, m_PositionId);
                        //TODO : dialog로 바꾸기
                        DlgGlassInfo dlg = new DlgGlassInfo(m_PositionId);
                        dlg.CimEnable = true;
                        dlg.ModifyEnable = true;
                        dlg.Initialize();
                        dlg.ShowDialog();
                        //jemoon : 110607
                        //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                        dlg.Dispose();
                    }
                    else if (m_CreateEnable &&
                        (MessageBox.Show("Do you want to create glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        m_Client.SendCommand(Command.GlassDataCreate, m_PositionId);
                        //TODO : dialog로 바꾸기
                    }
                    else if (m_RecoveryEnable &&
                            (MessageBox.Show("Do you want to recovery glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        //m_Client.SendCommand(Command.GlassDataRecovery, m_PositionId);
                        //TODO : dialog로 바꾸기
                        DlgLostGlassList dlg = new DlgLostGlassList();
                        dlg.Initialize(m_LostGlassDataProvider);
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            m_Client.SendCommand(Command.GlassDataRecovery, dlg.SelectedNo, m_PositionId);
                        }
                    }
                }
                //if (!m_CreateEnable) return;
                //if (MessageBox.Show("Do you want to create glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
                //m_Client.SendCommand(Command.GlassDataCreate, m_PositionId);
            }
            else
            {
                //DlgGlassData보여주기
                DlgGlassInfo dlg = new DlgGlassInfo(m_PositionId);
                dlg.Editable = m_Editable;
                dlg.Initialize();
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                dlg.Dispose();
            }

            //if (GlsDataClick != null)
            //    GlsDataClick();
        }

        //public bool IsExist()
        //{
        //    if (m_GlassDataProvider == null) return false;
        //    return m_GlassDataProvider.IsExist(m_PositionId);
        //}

        public bool IsExist()
        {
            int[] ids;
            m_GlassDataProvider.GetAllPositionId(out ids);
            return IsExist(m_PositionId, ids);
        }

        private bool IsExist(int key, int[] container)
        {
            foreach (int i in container)
            {
                if (key == i) return true;
            }

            return false;
        }
        #endregion

        #region Override
        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            ulong positionFlag = m_GlassDataProvider.GetPositionFlag();
            bool changed = (oldPositionFlag ^ positionFlag) > 0;
            if (changed)
            {
                oldPositionFlag = positionFlag;
                UpdateState();
            }
            else
            {
                UpdateDisplayData();    //GlassData의 Position은 바뀌지 않았지만 화면에 보여지는 내용이 바뀌었을 경우
            }

            //UpdateState();
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (IsExist() == false)
            {
                if (lblData.Text != "")
                {
                    lblData.Text = "";
                    lblData.BackColor = Color.White;
                }
            }
            else
            {
                m_GlassDataProvider.GetData(m_PositionId, ref m_Data);

                if (lblData.Text != m_Data.Item.GlassNumberCode.SlotNo.ToString())
                {
                    lblData.Text = m_Data.Item.GlassNumberCode.SlotNo.ToString();// m_Data.Item.GlassId;
                    lblData.BackColor = Color.Yellow;
                }
            }

            //UpdateState(IsExist());
        }

        private void UpdateDisplayData()
        {
            if (!m_Initialized) return;
            if (IsExist() == false) return;

            m_GlassDataProvider.GetData(m_PositionId, ref m_Data);

            if (lblData.Text != m_Data.Item.GlassNumberCode.SlotNo.ToString())
            {
                lblData.Text = m_Data.Item.GlassNumberCode.SlotNo.ToString();
            }
        }
        #endregion
    }
}
