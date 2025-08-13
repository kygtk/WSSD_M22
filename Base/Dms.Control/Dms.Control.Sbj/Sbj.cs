///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Sbj UserControl
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
    [ToolboxBitmap(typeof(Sbj), "SbjAni.bmp")]
    public partial class Sbj : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPiping tagDescriptor = new TagDescriptorPiping();
        #endregion

        #region Enum
        private enum AnimFrames
        {
            run1, run2
        }

        public enum Types
        {
            UPPER_LEFT_DI, UPPER_RIGHT_DI, LOWER_LEFT_DI, LOWER_RIGHT_DI
        }
        #endregion

        #region Fields
        private Types m_Type = Types.UPPER_LEFT_DI;
        protected DeviceTag m_TagDi = null;
        protected DeviceTag m_TagDiOld = new DeviceTag();
        private DeviceTagInfo m_TagInfoDi;
        private int m_CdaRun = -1;
        private int m_DiRun = -1;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of Sbj  - eun 20080115
        /// </summary>
        [Category("DMS : UI")]
        public Types SbjType
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                SetDiplay();
            }
        }
        [Category("DMS : Tag")]
        public DeviceTagInfo DIDeviceTagInfo
        {
            get { return m_TagInfoDi; }
            set { m_TagInfoDi = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Sbj contstructor - eun 20080111
        /// </summary>
        public Sbj()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagInfoDi = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// make the current Sbj type's bitmap list - eun 20080115 
        /// </summary>
        private void SetDiplay()
        {
            switch (m_Type)
            {
                case Types.UPPER_LEFT_DI:
                    axSbjUnit1.SbjType = 0;
                    break;
                case Types.UPPER_RIGHT_DI:
                    axSbjUnit1.SbjType = 1;
                    break;
                case Types.LOWER_LEFT_DI:
                    axSbjUnit1.SbjType = 2;
                    break;
                case Types.LOWER_RIGHT_DI:
                    axSbjUnit1.SbjType = 3;
                    break;
            }

        }
        /// <summary>
        /// Update Sbj animation - eun 20080115
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
                DeviceTag tagDi = m_Tags[m_TagInfoDi.DeviceName];
                if (tagDi == null)
                {
                    string msg = string.Format("Tag of {0} does not exist.", this.Name);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else if (m_Tag[tagDescriptor.RUN] == null || tagDi[tagDescriptor.RUN] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.RUN].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                else
                {
                    m_TagDi = tagDi;
                    m_TagDiOld.Clone(m_TagDi);
                }

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
                m_CdaRun = 1;
            }
            else
            {
                m_CdaRun = 0;
            }

            if (m_TagDi[tagDescriptor.RUN].Value == bool.TrueString || m_TagDi[tagDescriptor.RUN].Value == "1")
            {
                m_DiRun = 1;
            }
            else
            {
                m_DiRun = 0;
            }


            axSbjUnit1.SetSbjState(m_DiRun, m_CdaRun);
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
            if (m_TagDiOld.IsChanged(m_TagDi))
            {
                m_TagDiOld.Clone(m_TagDi);
                UpdateState();
            }
        }
        #endregion
    }
}
