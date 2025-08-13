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
    public partial class PmChamberProcessState : DmsUserControl
    {
        TagDescriptorPmChamber tagDescriptor = new TagDescriptorPmChamber();
        
        #region Fields
        
        private string[] m_StepNames = 
        {
            "Noop",
            "Initialize",
            "ChamberVent",            
            "WaferRecv",
            "SlowPumping",
            "FastPumping",
            "ProcessStep1",
            "ProcessStep2",
            "ProcessStep3",
            "ProcessStep4",
            "ProcessStep5",
            "ChamberPurge",   
            "WaferSend",
            "CyclePurge"
        };

        private const int _StartPointX = 3;
        private const int _StartPointY = 3;
        private const int _Heignt = 20;
        private const int _Width = 113;
        private const int _Gap = 3;
        private Description[] m_Controls;
        #endregion

        [Category("DMS : UI")]
        public Color PanelBackColor
        {
            get { return pnBack.BackColor; }
            set { pnBack.BackColor = value; }
        }
        
        public PmChamberProcessState()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo("PmChamber");

            Initialize();
        }
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }
        public void Initialize()
        {
            int stepCount = m_StepNames.Length;
            
            // Set Size
            if (stepCount != 0)
            {
                this.Size = new Size(_Gap * 2 + _Width, _Gap + ((_Heignt + _Gap) * stepCount));
            }

            m_Controls = new Description[stepCount];
            
            for (int i = 0; i < stepCount; i++)
            {
                Description description = new Description();

                description.Location = new Point(_StartPointX, _StartPointY + (i * (_Heignt + _Gap)));
                description.Size = new Size(_Width, _Heignt);
                description.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                description.DescriptionText = m_StepNames[i];
                m_Controls[i] = description;
            }

            this.pnBack.Controls.AddRange(m_Controls);            
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

             int no = Convert.ToInt32( m_Tag[tagDescriptor.PROCESSSTEP].Value);

             for (int i = 0; i < m_StepNames.Length; i++)
             {
                 if (no == i) m_Controls[i].DescriptionBackColor = Color.GreenYellow;
                 else m_Controls[i].DescriptionBackColor = Color.White;
             }
        }       
    }
}
