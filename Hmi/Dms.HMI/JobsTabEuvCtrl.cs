using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Device;
using Dms.Control;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class JobsTabEuvCtrl : UserControl
    {
        private ViewUshioEuv viewUshioEuv;

        public JobsTabEuvCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
        }

        public void Initialize()
        {
            DmsComponents components = DmsComponents.Instance;
            _GenericCollection<UshioEuvUnit> ushioEuvUnits = components.ComponentContainer.GetCollection<UshioEuvUnit>();
            viewUshioEuv = new ViewUshioEuv();

            if (ushioEuvUnits != null || ushioEuvUnits.Count != 0)
            {
                viewUshioEuv.Initialize(ushioEuvUnits[0]);
                this.Controls.Add(viewUshioEuv);
            }
        }
    }
}
