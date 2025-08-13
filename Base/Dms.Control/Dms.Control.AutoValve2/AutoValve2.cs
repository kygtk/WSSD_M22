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
    [ToolboxBitmap(typeof(AutoValve2), "AutoValveIcon.bmp")]
    public partial class AutoValve2 : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion

        #region Enum

        public enum AnimFrames
        {
            autovalveHoff, autovalveHon, autovalveVoff, autovalveVon, autovalveHpush, autovalveVpush
            //autovalveOff_v, autovalveOff_h, autovalveOn_v, autovalveOn_h, autovalvePush_v, autovalvePush_h,
            //gatevalveOff_v, gatevalveOff_h, gatevalveOn_v, gatevalveOn_h, gatevalvePush_v, gatevalvePush_h,
            //autovalve_r_Off, autovalve_r_On, autovalve_r_Push,
        }

        public enum DeviceType
        {
            Horizon, Vertical, HGate, VGate
        }
        #endregion

        #region Fields
        private DeviceType m_Type = DeviceType.Vertical;
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        private Image m_OldImage = null;
        private ClientManager m_Client = null;
        //private static DlgAutoValve dlg = null;
        #endregion

        #region Properties
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
        #endregion

        #region Constructor
        public AutoValve2()
        {
            InitializeComponent();

            // autovalveHoff, autovalveHon, autovalveVoff, autovalveVon
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_h_off);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_h_on);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_v_off);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_v_on);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_h_push);
            m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_v_push);

            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_off);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_off_h);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_on);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_on_h);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_push);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_push_h);

            //m_Bitmap.Add(Dms.Control.Properties.Resources.VGate_Close);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.HGate_Close);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.VGate_Open);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.HGate_Open);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.VGate_Focus);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.HGate_Focus);

            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_r_off);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_r_on);
            //m_Bitmap.Add(Dms.Control.Properties.Resources.autovalve_r_push);

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void SetValveType()
        {
            //if (m_Type == DeviceType.Vertical)
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_v];
            //else if (m_Type == DeviceType.Horizon)
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveOff_h];
            //else if (m_Type == DeviceType.VGate)
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.gatevalveOff_v];
            //else if (m_Type == DeviceType.HGate)
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.gatevalveOff_h];
            //pbImage.Image = m_Bitmap[(int)AnimFrames.autovalve_r_Off];

            if (m_Type == DeviceType.Horizon)
                pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHoff];
            else
                pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVoff];
        }

        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count == 0) return;
            if (m_Client.GenInfos.AutoMode) return;

            if (m_Type == DeviceType.Vertical)
            {
                m_OldImage = pbImage.Image;
                pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVpush];
            }
            else
            {
                m_OldImage = pbImage.Image;
                pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHpush];
            }

            //if (m_Type == DeviceType.Vertical)
            //{
            //    m_OldImage = pbImage.Image;
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalvePush_v];
            //}
            //else if (m_Type == DeviceType.Horizon)
            //{
            //    m_OldImage = pbImage.Image;
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalvePush_h];
            //}
            //else if (m_Type == DeviceType.VGate)
            //{
            //    m_OldImage = pbImage.Image;
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.gatevalvePush_v];
            //}
            //else if (m_Type == DeviceType.HGate)
            //{
            //    m_OldImage = pbImage.Image;
            //    pbImage.Image = m_Bitmap[(int)AnimFrames.gatevalvePush_h];
            //}

            //m_OldImage = pbImage.Image;
            //pbImage.Image = m_Bitmap[(int)AnimFrames.autovalve_r_Push];
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;
                //UpdateState();
                pbImage.Image = m_OldImage;
                DlgAutoValve2 dlg = new DlgAutoValve2(m_Tag);
                dlg.ShowDialog();
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
                if (m_Tag[tagDescriptor.ACT_STATUS] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.ACT_STATUS].Key);
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

            if (m_Tag[tagDescriptor.ACT_STATUS].Value == ActuatorAct.Pos.ToString())
            {
                if (m_Type == DeviceType.Horizon)
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHon];
                else
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVon];
            }
            else
            {
                if (m_Type == DeviceType.Horizon)
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveHoff];
                else
                    pbImage.Image = m_Bitmap[(int)AnimFrames.autovalveVoff];
            }
        }
        #endregion
    }
}
