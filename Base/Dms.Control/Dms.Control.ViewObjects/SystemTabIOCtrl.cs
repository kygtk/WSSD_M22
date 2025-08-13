using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Util.IODefine;
using System.Xml.Serialization;

namespace Dms.Control
{
    public partial class SystemTabIOCtrl : UserControl
    {
        #region Fields
        private IoDefines m_IoDefines;
        #endregion

        #region Constructor
        public SystemTabIOCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        } 
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public ViewIOEdit ViewIOEdit
        {
            get { return this.viewIOEdit1; }
        }
        #endregion

        #region Methods
        public void Initialize(IoDefines ioDefines)
        {
            m_IoDefines = ioDefines;

            ClientManager client = ClientManager.Instance;
            ICtlDevice ctlDevice = client.EventSubscriber.IServerManager.IoController;
            this.viewIOEdit1.Initialize(m_IoDefines, ViewIOEdit.OpMode.System, ctlDevice);
        }

        public void Initialize(IoDefines ioDefines, ICtlDevice controller)
        {
            Initialize(ioDefines, controller, ViewIOEdit.OpMode.System);
        }

        public void Initialize(IoDefines ioDefines, ICtlDevice controller, ViewIOEdit.OpMode mode)
        {
            m_IoDefines = ioDefines;
            this.viewIOEdit1.Initialize(m_IoDefines, mode, controller);        
        }

        //jemoon : Controller를 직접 이용하도록 수정
        //private void viewIOEdit1_OnSetIoState(object sender, IoStateEventArgs e)
        //{
        //    ClientManager client = ClientManager.Instance;
        //    client.SendCommand(Command.SetIoState, e);
        //}
        #endregion
    }
}
