///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : AquaKnife UserControl
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
    [ToolboxBitmap(typeof(AquaKnife), "AquaKnifeAni.bmp")]
    public partial class AquaKnife : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPiping tagDescriptor = new TagDescriptorPiping();
        #endregion

        #region Enum
        private enum AnimFrames
        {
            aqk1, aqk2, aqk3, aqk4, aqk5, aqk6
        }

        public enum Types
        {
            LEFT, RIGHT
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Types m_Type = Types.LEFT;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of AquaKnife  - eun 20080115
        /// </summary>
        [Category("DMS : UI"),
         Description("Select type of AquaKnife")]
        public Types AquaKnifeType
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
        /// No parameter AquaKnife contstructor - eun 20080111
        /// </summary>
        public AquaKnife()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// make the current AquaKnife type's bitmap list - eun 20080115 
        /// </summary>
        private void SetDiplay()
        {
            m_Bitmap.Clear();
            if (m_Type == Types.LEFT)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_L_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_L_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_L_3);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_L_4);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_L_5);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_L_6);
                axAqkUnit1.AqkType = 0;
            }
            else
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_R_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_R_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_R_3);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_R_4);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_R_5);
                m_Bitmap.Add(Dms.Control.Properties.Resources.AQK_R_6);
                axAqkUnit1.AqkType = 1;
            }
        }

        /// <summary>
        /// Update AquaKnife animaion - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
        }
        #endregion

        #region Override
        /// <summary>
        /// It should be called by HMI - eun 20080110
        /// </summary>
        /// <param name="tags"></param>
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                if (m_Tag[tagDescriptor.RUN] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.RUN].Key);
                    MessageBox.Show(msg);
                    ok = false;
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
                axAqkUnit1.SetRun();
            }
            else
            {
                axAqkUnit1.SetStop();
            }
        }
        #endregion
    }
}
