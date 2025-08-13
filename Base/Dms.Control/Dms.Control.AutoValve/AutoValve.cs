///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.11
// Author       : eun
// Description  : AutoValve UserControl
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

namespace Dms.Control
{
    [ToolboxBitmap(typeof(AutoValve), "AutoValveIcon.bmp")]
    public partial class AutoValve : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPiping tagDescriptor = new TagDescriptorPiping();
        #endregion

        #region Enum

        public enum AnimFrames
        {
            autovalveOff_v, autovalveOff_h, autovalveOn_v, autovalveOn_h, autovalvePush_v, autovalvePush_h,
            autovalveVoff, autovalveHoff, autovalveVon, autovalveHon, autovalveVpush, autovalveHpush
        }

        public enum DeviceType
        {
            Horizon, Vertical
        }

        public enum ImageType
        {
            Default, New
        }
        #endregion

        #region Fields
        private DeviceType m_Type = DeviceType.Vertical;
        private ImageType m_ImageType = ImageType.Default;
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Image m_OldImage = null;
        private ClientManager m_Client = null;
        //private static DlgAutoValve dlg = null;
        #endregion

        #region Properties
        /// <summary>
        /// Set current type of AutoValve  - eun 20080111
        /// </summary>
        [Category("DMS : UI"),
         Description("Select type of AutoValve")]
        public DeviceType ValveType
        {
            get { return m_Type; }
            set 
            { 
                m_Type = value;
                SetValveType();
            }
        }
        [Category("DMS : UI"),
         Description("Select type of AutoValve")]
        public ImageType ValveImageType
        {
            get { return m_ImageType; }
            set
            {
                m_ImageType = value;
                SetValveType();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter AutoValve contstructor - eun 20080111
        /// </summary>
        public AutoValve()
        {
            InitializeComponent();
            
            m_TagInfo = new DeviceTagInfo(this.GetType().Name);

            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_off);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_off_h);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_on);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_on_h);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_push);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_push_h);

            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_v_off);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_h_off);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_v_on);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_h_on);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_v_push);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_h_push);

            SetValveType();
        }
        #endregion

        #region Methods
        /// <summary>
        /// set image of current valve type - eun 20080111
        /// </summary>
        private void SetValveType()
        {
            if (m_Type == DeviceType.Vertical)
            {
                if (m_ImageType == ImageType.Default)
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_v];
                else
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVoff];
                
            }
            else
            {
                if (m_ImageType == ImageType.Default) 
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_h];
                else
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHoff];
            }
        }

        /// <summary>
        /// set pushed image - eun 20080111
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count == 0 ) return;
            if (m_Client.GenInfos.AutoMode) return;

            if (m_Type == DeviceType.Vertical)
            {
                m_OldImage = pbImage.Image;
                if (m_ImageType == ImageType.Default)
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_v];
                else
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVpush];
            }
            else
            {
                m_OldImage = pbImage.Image;
                if (m_ImageType == ImageType.Default)
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_h];
                else
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHpush];
            }
        }
        
        /// <summary>
        /// show valve open/close operation dialog - eun 20080111
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;
                //UpdateState();
                pbImage.Image = m_OldImage;
                DlgAutoValve dlg = new DlgAutoValve(m_Tag);
                dlg.ShowDialog();
                //jemoon : 110607
                //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
                dlg.Dispose();

                //if (dlg == null || dlg.IsDisposed)
                //{
                //    dlg = new DlgAutoValve(m_Tag);
                //    dlg.Show();
                //    dlg.Activate();
                //}
                //else
                //{
                //    dlg.ValveTag = m_Tag;
                //    dlg.Activate();
                //}
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
                if (m_Tag[tagDescriptor.RUN] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.RUN].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }

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

            if (m_Tag[tagDescriptor.RUN].Value == bool.TrueString || m_Tag[tagDescriptor.RUN].Value == "1")
            {
                if (m_Type == DeviceType.Vertical)
                {
                    if (m_ImageType == ImageType.Default)
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOn_v];
                    else
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVon];
                }
                else
                {
                    if (m_ImageType == ImageType.Default)
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOn_h];
                    else
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHon];
                }
            }
            else
            {
                if (m_Type == DeviceType.Vertical)
                {
                    if (m_ImageType == ImageType.Default)
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_v];
                    else
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVoff];
                }
                else
                {
                    if (m_ImageType == ImageType.Default)
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_h];
                    else
                        pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHoff];
                }
            }
        }
        #endregion
    }
}
