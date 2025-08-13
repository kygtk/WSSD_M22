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
    public partial class SystemTabSlaveCtrl : UserControl
    {
        #region Fields
        private IoDefines m_IoDefines;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public ViewSlaveEdit ViewSlaveEdit
        {
            get { return this.viewSlaveEdit1; }
        }
        #endregion

        #region Constructor
        public SystemTabSlaveCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        public void Initialize(IoDefines ioDefines)
        {
            m_IoDefines = ioDefines;

            ClientManager client = ClientManager.Instance;
            ICtlDevice ctlDevice = client.EventSubscriber.IServerManager.EcController;
            this.viewSlaveEdit1.Initialize(m_IoDefines, ViewSlaveEdit.OpMode.System, ctlDevice);
        }

        public void Initialize(IoDefines ioDefines, ICtlDevice controller)
        {
            Initialize(ioDefines, controller, ViewSlaveEdit.OpMode.System);
        }

        public void Initialize(IoDefines ioDefines, ICtlDevice controller, ViewSlaveEdit.OpMode mode)
        {
            m_IoDefines = ioDefines;
            this.viewSlaveEdit1.Initialize(m_IoDefines, mode, controller);
        }
        #endregion
    }
}
