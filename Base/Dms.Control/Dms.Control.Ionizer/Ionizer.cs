///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Ionizer UserControl
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

namespace Dms.Control
{
    [ToolboxBitmap(typeof(Ionizer), "IonizerAni.bmp")]
    public partial class Ionizer : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorIonizer tagDescriptor = new TagDescriptorIonizer();
        #endregion

        #region Enum
        private enum AnimFrames
        {
            run1, run2, run3, run4, run5, run6, run7, run8, run9, run10, run11, run12, run13, alarm1, alarm2
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Ionizer contstructor - eun 20080111
        /// </summary>
        public Ionizer()
        {
            InitializeComponent();

            m_Bitmap.Add(Dms.Control.Properties.Resources.Run1);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run2);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run3);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run4);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run5);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run6);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run7);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run8);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run9);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run10);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run11);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run12);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Run13);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Alarm1);
            m_Bitmap.Add(Dms.Control.Properties.Resources.Alarm2);

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update Ionizer animation - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
        }

        private void Ionizer_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region Override
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

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if ((m_Tag[tagDescriptor.LEVEL_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.LEVEL_ALARM].Value == "1") ||
                (m_Tag[tagDescriptor.COND_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.COND_ALARM].Value == "1") ||
                (m_Tag[tagDescriptor.CONTROL_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.CONTROL_ALARM].Value == "1") ||
                (m_Tag[tagDescriptor.RUN_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.RUN_ALARM].Value == "1")) // 09.11.26 minhan
            {
                axIonizer1.SetAlarm();
            }
            else
            {
                axIonizer1.SetStart();
            }
        }
        #endregion
    }
}
