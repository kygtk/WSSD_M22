///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Shower UserControl
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
    [ToolboxBitmap(typeof(Shower), "ShowerAni.bmp")]
    public partial class Shower : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPiping tagDescriptor = new TagDescriptorPiping();
        #endregion

        #region Enum
        private enum AnimFrames
        {
            shower1, shower2, shower3, shower4, shower5, shower6
        }

        public enum Types
        {
            LOW, UP
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Types m_Type = Types.LOW;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of Shower  - eun 20080115
        /// </summary>
        [Category("DMS : UI")]
        public Types ShowerType
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                SetDiplay();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Shower contstructor - eun 20080111
        /// </summary>
        public Shower()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// make the current Shower type's bitmap list - eun 20080115 
        /// </summary>
        private void SetDiplay()
        {
            m_Bitmap.Clear();
            if (m_Type == Types.LOW)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_LO_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_LO_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_LO_3);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_LO_4);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_LO_5);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_LO_6);
                this.axShwUnit1.ShwType = 1;
            }
            else
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_UP_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_UP_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_UP_3);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_UP_4);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_UP_5);
                m_Bitmap.Add(Dms.Control.Properties.Resources.Shower_UP_6);
                this.axShwUnit1.ShwType = 0;
            }
        }

        /// <summary>
        /// Update Shower animation - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
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

            if (m_Tag[tagDescriptor.RUN].Value == bool.TrueString || m_Tag[tagDescriptor.RUN].Value == "1")
            {
                axShwUnit1.SetRun();
            }
            else
            {
                axShwUnit1.SetStop();
            }
        }
        #endregion
    }
}
