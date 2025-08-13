///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.12.09
// Author       : jemoon
// Description  : AutoValve UserControl for PLC HMI
//-------------------------------------------------------------------------
// Revison History
// * 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;
using Dms.Server;

namespace Dms.Control.Hmi
{
    public partial class HmiAutoValve2 : DmsUserControl
    {
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
            Default, Type1
        }
        #endregion

        #region Fields
        private DeviceType m_Type = DeviceType.Vertical;
        private ImageType m_ImageType = ImageType.Default;
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Image m_OldImage = null;

		private IServerManager m_Server = null;
		private Dms.Device.HmiAutoValve m_Valve = null;
        #endregion

        #region Properties
        [Category("DMS : UI"), Description("Select type of AutoValve")]
        public DeviceType ValveType
        {
            get { return m_Type; }
            set 
            { 
                m_Type = value;
                SetValveType();
            }
        }
        [Category("DMS : UI"), Description("Select type of AutoValve")]
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
		public HmiAutoValve2()
        {
            InitializeComponent();
            
            m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_off);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_off_h);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_on);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_on_h);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_push);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_push_h);

			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_v_off);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_h_off);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_v_on);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_h_on);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_v_push);
			m_Bitmap.Add(Dms.Control.Hmi.Properties.Resources.autovalve_h_push);

			m_TagInfo = new DeviceTagInfo(this.GetType().Name);

            SetValveType();
        }
        #endregion

        #region Methods
        private void SetValveType()
        {
            if (m_Type == DeviceType.Vertical)
            {
				if (m_ImageType == ImageType.Default)
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_v];
				}
				else
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVoff];
				}
                
            }
            else
            {
				if (m_ImageType == ImageType.Default)
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_h];
				}
				else
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHoff];
				}
            }
        }

        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
			if (m_Tag == null) return;
			if (m_Tag.Items.Count == 0) return;

            if (m_Type == DeviceType.Vertical)
            {
                m_OldImage = pbImage.Image;
				if (m_ImageType == ImageType.Default)
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_v];
				}
				else
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVpush];
				}
            }
            else
            {
                m_OldImage = pbImage.Image;
				if (m_ImageType == ImageType.Default)
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_h];
				}
				else
				{
					pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHpush];
				}
            }
        }
        
        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
			if (m_Tag == null) return;
			if (m_Tag.Items.Count == 0) return;
			if (!m_Initialized) return;

			{
				pbImage.Image = m_OldImage;
				DlgAutoValve2 dlg = new DlgAutoValve2(m_Valve);
				dlg.ShowDialog();
			}
        }
        #endregion

        #region Override
		public override bool Initialize(DeviceTags tags)
        {
			bool ok = base.Initialize(tags);

            if (ok)
            {
				m_Server = ServerManager.Instance;
				m_Valve = m_Server.ComponentContainer[m_Tag.DeviceName] as Dms.Device.HmiAutoValve;

				if (m_Valve == null)
				{
					string msg = string.Format("Device of {0} does not exist.", this.Name);
					MessageBox.Show(msg);
					ok = false;
				}
				else
				{
					tmrUpdateState.Enabled = ok;

					UpdateState();
				}
            }

			m_Initialized = ok;

            return m_Initialized;
        }

		protected override void tmrUpdateState_Tick(object sender, EventArgs e)
		{
			UpdateState();
		}

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

			bool valveOpen = m_Valve.IsOpen();
			if (valveOpen)
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
