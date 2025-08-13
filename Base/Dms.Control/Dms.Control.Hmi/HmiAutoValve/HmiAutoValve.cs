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

namespace Dms.Control.Hmi
{
    public partial class HmiAutoValve : DmsHmiComponent
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

		private ushort m_ValveNo = 1;
		private IoDigitalInput m_DiValveOpenStatus = null;
		private IoDigitalOutput m_DoValveOpenSwitch = null;
		private IoDigitalOutput m_DoValveCloseSwitch = null;
		private IoAnalogOutput m_AoSelectedValveNo = null;
        #endregion

        #region Properties
        [Category("DMS : !Option"), Description("Select type of AutoValve")]
        public DeviceType ValveType
        {
            get { return m_Type; }
            set 
            { 
                m_Type = value;
                SetValveType();
            }
        }
		[Category("DMS : !Option"), Description("Select type of AutoValve")]
        public ImageType ValveImageType
        {
            get { return m_ImageType; }
            set
            {
                m_ImageType = value;
                SetValveType();
            }
        }
		[Category("DMS : Data"), Description("Set Valve No")]
		public ushort ValveNo
		{
			get { return m_ValveNo; }
			set { m_ValveNo = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoDigitalInput DiValveOpenStatus
		{
			get { return m_DiValveOpenStatus; }
			set { m_DiValveOpenStatus = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoDigitalOutput DoValveOpenSwitch
		{
			get { return m_DoValveOpenSwitch; }
			set { m_DoValveOpenSwitch = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoDigitalOutput DoValveCloseSwitch
		{
			get { return m_DoValveCloseSwitch; }
			set { m_DoValveCloseSwitch = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogOutput AoSelectedValveNo
		{
			get { return m_AoSelectedValveNo; }
			set { m_AoSelectedValveNo = value; }
		}
        #endregion

        #region Constructor
		public HmiAutoValve()
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
			if (m_Initialized)
			{
				pbImage.Image = m_OldImage;
				DlgAutoValve dlg = new DlgAutoValve(this);
				dlg.ShowDialog();
			}
        }
        #endregion

        #region Override
        public override bool Initialize()
        {
            bool ok = base.Initialize();

            if (ok)
            { 
                m_Initialized = ok;

				tmrUpdateState.Enabled = m_Initialized;
            }

            return m_Initialized;
        }

		bool test = false;
		int ct = 0;
        protected override void UpdateState()
        {
            if (!m_Initialized) return;
			//test = !test;
			//bool valveOpen = test;
			bool valveOpen = m_DiValveOpenStatus.GetState();
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

			if (test != valveOpen)
			{
				test = valveOpen;

				HmiLog.WriteLog((ct++).ToString());
			}
        }
        #endregion
    }
}
