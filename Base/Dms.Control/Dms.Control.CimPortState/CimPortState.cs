using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Cim.Common;

namespace Dms.Control
{
    public partial class CimPortState : UserControl
    {
        #region Fields
        private PortInfo m_PortInfo = null;
        private Color m_LRColor = Color.White;
        private Color m_LCColor = Color.LightCyan;
        private Color m_URColor = Color.LightPink;
        private Color m_UCColor = Color.Brown;
        private Color m_DisableColor = Color.LightGray;

        private bool m_Initialized = false;
        private bool m_TimerUpdateEnable = false;
        private int m_Id;
        private int m_DisplayId;
        private string OldPortStatus = " ";
        private string OldCstId = "";
        private string OldPortGlassCount = "";
        private string OldTrsMode = "";
        private string OldDisplay = "";
        private string OldPortEnable = "";
//        private int OldLotNo = -1;

        //private LotColorInfo m_LotColorInfo = null;
        #endregion

        #region Properties
        [Category("DMS : UI"),
        Description("PORT NO")]
        public int PortId
        {
            get { return m_Id; }
            set 
            { 
                m_Id = value;
            } 
        }

        [Category("DMS : UI"),
        Description("PORT NO(DISPLAY)")]
        public int DisplyPortId
        {
            get { return m_DisplayId; }
            set
            {
                m_DisplayId = value;
                lblPortName.Text = "PORT " + Convert.ToString(m_DisplayId);
            }
        }

        [Category("DMS : UI"),
        Description("LR COLOR")]
        public Color LRCOLOR
        {
            get { return m_LRColor; }
            set { m_LRColor = value; }
        }

        [Category("DMS : UI"),
        Description("LC COLOR")]
        public Color LCCOLOR
        {
            get { return m_LCColor; }
            set { m_LCColor = value; }
        }

        [Category("DMS : UI"),
        Description("UR COLOR")]
        public Color URCOLOR
        {
            get { return m_URColor; }
            set { m_URColor = value; }
        }

        [Category("DMS : UI"),
        Description("UC COLOR")]
        public Color UCCOLOR
        {
            get { return m_UCColor; }
            set { m_UCColor = value; }
        }

        [Category("DMS : UI"),
        Description("UC COLOR")]
        public Color DISABLECOLOR
        {
            get { return m_DisableColor; }
            set { m_DisableColor = value; }
        }


        [Browsable(false)]
        public bool TimerUpdateEnable
        {
            get
            {
                return m_TimerUpdateEnable;
            }
            set
            {
                m_TimerUpdateEnable = value;
                this.tmrUpdateState.Enabled = value;
            }
        }

        public PortInfo PortInfo
        {
            get { return m_PortInfo; }
            set { m_PortInfo = value; }
        }
        #endregion

        public CimPortState()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.CacheText, true);

            InitializeComponent();                        
        }

        public void Initialize(PortInfos ports)
        {
            m_Initialized = true;
            tmrUpdateState.Enabled = true;

            if (m_Id > 0 && m_Id <= ports.Count) m_PortInfo = ports[m_Id-1];
        }

        private void UpdateState()
        {
            if (false == m_Initialized || m_PortInfo == null) return;

            string portstatus = m_PortInfo.PortStatus;

            if (OldPortStatus != portstatus)
            {
                if (portstatus == PortStatus.None.ToString() ||
                    portstatus == PortStatus.LoadRequest.ToString() )
                {
                    lblPortStatus.Text = "LR";
                    OldPortStatus = lblPortStatus.Text;
                }
                else if (portstatus == PortStatus.UnloadRequest.ToString())
                {
                    lblPortStatus.Text = "UR";
                    OldPortStatus = lblPortStatus.Text;
                }
                else if( portstatus == PortStatus.UnloadComplete.ToString())
                {
                    lblPortStatus.Text = "UC";
                    OldPortStatus = lblPortStatus.Text;
                }
                else
                {
                    lblPortStatus.Text = "LC";
                    OldPortStatus = lblPortStatus.Text;
                }
            }
            else if( portstatus == "" )
            {
                lblPortStatus.Text = "";
            }

            string cstid = m_PortInfo.CstId;

            if (OldCstId != cstid)
            {
//                lblCstID.BackColor = Color.Yellow;
                lblCstID.Text = cstid;
                OldCstId = lblCstID.Text;
            }
            else if( cstid == "" )
            {
                lblCstID.Text = "";
//                lblCstID.BackColor = Color.White;
            }

            string portglasscount = string.Format("{0:00}/{1:00}/{2:00}", PortInfo.InNum, PortInfo.OutNum, PortInfo.TotalNum);

            if (OldPortGlassCount != portglasscount)
            {
                lblPortCount.Text = portglasscount;
                OldPortGlassCount = lblPortCount.Text;
            }
            else if (portglasscount == "")
            {
                lblPortCount.Text = "";
            }

            string trsmode = m_PortInfo.TrsMode;
            if (OldTrsMode != trsmode)
            {
                lblTrsMode.Text = trsmode;
                OldTrsMode = lblTrsMode.Text;
            }
            else if (OldTrsMode == "")
            {
                lblTrsMode.Text = "";
            }
/*
            int LotNo = m_CassettePort.LotNo;

            if (LotNo == 0 && OldLotNo != LotNo)
            {
                lblPortName.BackColor = Color.White;
                lblPortStatus.BackColor = Color.White;
                lblCstID.BackColor = Color.White;
                lblLotID.BackColor = Color.White;
                lblRecipeID.BackColor = Color.White;

                OldLotNo = 0;
            }
            else if( LotNo > 0 && OldLotNo != LotNo)
            {
                Color color = LotColorInfo.GetColor(m_CassettePort.LotNo);

                lblPortName.BackColor = color;
                lblPortStatus.BackColor = color;
                lblCstID.BackColor = color;
                lblLotID.BackColor = color;
                lblRecipeID.BackColor = color;

                OldLotNo = LotNo;
            }
*/
            if (OldPortEnable != m_PortInfo.Enable.ToUpper())
            {
                if (m_PortInfo.Enable.ToUpper() == "ENABLE")
                {
                    OldDisplay = "";
                }
                else if( m_PortInfo.Enable.ToUpper() == "DISABLE" )
                {
                    lblPortName.BackColor = m_DisableColor;
                }

                OldPortEnable = m_PortInfo.Enable.ToUpper();
            }

            if (OldDisplay != OldPortStatus)
            {
                if (m_PortInfo.Enable.ToUpper() == "ENABLE")
                {
                    if (OldPortStatus == "LR")
                    {
                        lblPortName.BackColor = m_LRColor;
                    }
                    else if (OldPortStatus == "LC")
                    {
                        lblPortName.BackColor = m_LCColor;
                    }
                    else if (OldPortStatus == "UR")
                    {
                        lblPortName.BackColor = m_URColor;
                    }
                    else if (OldPortStatus == "UC")
                    {
                        lblPortName.BackColor = m_UCColor;
                    }

                    OldDisplay = OldPortStatus;
                }
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }
    }
}
