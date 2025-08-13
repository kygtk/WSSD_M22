///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.11
// Author       : eun
// Description  : Pump UserControl (No Inverter)
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
using Dms.Client;
using System.Xml.Serialization;

namespace Dms.Control
{
    public partial class Pump : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPump tagDescriptor = new TagDescriptorPump();
        #endregion

        #region Enum
        public enum AnimFrames
        {
            pump1, pump2, pump3, pump4, pump5, pump6, pump7, pumpAlarm, pumpDisable
        }

        public enum DeviceType
        {
            Normal, Inverter
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private ClientManager m_Client = null;
        private DeviceType m_Type = DeviceType.Normal;
        #endregion

        #region Properties
        /// <summary>
        /// Pump animation update time interval - eun 20080111
        /// </summary>
        //[Category("DMS : UI"),
        //Description("Pump animation update time interval")]
        //public int AnimationUpdateInterval
        //{
        //    get { return tmrUpdateAnimation.Interval; }
        //    set { tmrUpdateAnimation.Interval = value; }
        [Category("DMS : UI"), Description("Pump Type")]
        public DeviceType PumpType
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        //}
        [Browsable(false), XmlIgnore()]
        public bool AlarmState
        {
            get { return m_Tag[tagDescriptor.ALARM].Value == bool.TrueString; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Pump contstructor - eun 20080111
        /// </summary>
        public Pump()
        {
            InitializeComponent();

            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump1);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump2);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump3);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump4);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump5);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump6);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Pump7);
            m_Bitmap.Add(Dms.Control.Properties.Resources.PumpAlarm);
            m_Bitmap.Add(Dms.Control.Properties.Resources.PumpDisable);

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update Pump animaion - eun 20080111
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Eventhandler for Pump operation dialog - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pump_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;
                DlgPump dlg = new DlgPump(m_Tag);
                dlg.Type = m_Type;
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                dlg.Dispose();
            }
            else MessageBox.Show("Tag is not selected.");
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.ALARM].Value == "1")
            {
                axPump1.SetAlarm();
            }
            else if (m_Tag[tagDescriptor.STOP].Value == bool.TrueString || m_Tag[tagDescriptor.STOP].Value == "1")
            {
                axPump1.SetStop();
            }
            else if (m_Tag[tagDescriptor.RUN].Value == bool.TrueString || m_Tag[tagDescriptor.RUN].Value == "1")
            {
                axPump1.SetStart();
            }
        }
        #endregion
    }
}
