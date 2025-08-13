///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : AirKnife UserControl
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
    [ToolboxBitmap(typeof(AirKnife), "AirKnifeAni.bmp")]
    public partial class AirKnife : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPiping tagDescriptor = new TagDescriptorPiping();
        #endregion

        #region Enum
        private enum AnimFrames
        {
            ak1, ak2, ak3, ak4
        }

        public enum Types
        {
            LOW_LEFT, LOW_RIGHT, UP_LEFT, UP_RIGHT
        }
        #endregion

        #region Fields
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Types m_Type = Types.LOW_LEFT;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of AirKnife  - eun 20080115
        /// </summary>
        [Category("DMS : UI"),
         Description("Select type of AirKnife")]
        public Types AirKnifeType
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
        /// No parameter AirKnife contstructor - eun 20080111
        /// </summary>
        public AirKnife()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// make the current AirKnife type's bitmap list - eun 20080115 
        /// </summary>
        private void SetDiplay()
        {
            m_Bitmap.Clear();
            if (m_Type == Types.LOW_LEFT)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_L_0);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_L_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_L_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_L_3);
                this.axAirKnife1.AirKnifeType = 2;
            }
            else if (m_Type == Types.LOW_RIGHT)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_R_0);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_R_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_R_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_l_R_3);
                this.axAirKnife1.AirKnifeType = 3;
            }
            else if (m_Type == Types.UP_LEFT)
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_L_0);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_L_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_L_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_L_3);
                this.axAirKnife1.AirKnifeType = 0;
            }
            else
            {
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_R_0);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_R_1);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_R_2);
                m_Bitmap.Add(Dms.Control.Properties.Resources.ak_u_R_3);
                this.axAirKnife1.AirKnifeType = 1;
            }
        }

        /// <summary>
        /// Update AirKnife animaion - eun 20080115
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
                if (m_Tag[tagDescriptor.RUN] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", tagDescriptor[tagDescriptor.RUN].Key);
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
                axAirKnife1.SetRun();
            }
            else
            {
                axAirKnife1.SetStop();
            }
        }

        #endregion
    }
}
