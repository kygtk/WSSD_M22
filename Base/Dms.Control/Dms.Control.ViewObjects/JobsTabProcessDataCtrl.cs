using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;

namespace Dms.Control
{
    public partial class JobsTabProcessDataCtrl : UserControl
    {
        public JobsTabProcessDataCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
        }

        #region Methods
        public void Initialize()
        {
            ClientManager clientManager = ClientManager.Instance;
            this.viewApdItem1.Initialize(clientManager.ApdItemsHandler);
            this.viewPartsLifeTime1.Initialize(clientManager.PartsItemsHandler);
        }
        #endregion
    }
}
