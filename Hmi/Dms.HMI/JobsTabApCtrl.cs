using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Server;
using Dms.Device;
using Dms.Control;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class JobsTabApCtrl : UserControl
    {
        private ViewPSMAp viewPSM;
        private ViewSeAp viewSE;
        private _GenericCollection<PSMAp> psmApUnits;// 10.12.21 minhan
        private _GenericCollection<SeAp> seApUnits;

        //private string ApSetvalue; // 09.10.08 minhan

        public JobsTabApCtrl()
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
            psmApUnits = components.ComponentContainer.GetCollection<PSMAp>();
            seApUnits = components.ComponentContainer.GetCollection<SeAp>();
            viewPSM = new ViewPSMAp();
            viewSE = new ViewSeAp();

            if (psmApUnits != null && psmApUnits.Count != 0)
            {
                viewPSM.Initialize(psmApUnits[0]);
                this.Controls.Add(viewPSM);
                viewPSM.txtSetCDA.LimitHigh = "10";
                viewPSM.txtSetCDA.LimitLow = "1";
                viewPSM.txtSetN2.LimitHigh = "1500";
                viewPSM.txtSetN2.LimitLow = "100";
                viewPSM.txtSetVol.LimitHigh = "15";
                viewPSM.txtSetVol.LimitLow = "8";
            }
            else if (seApUnits != null && seApUnits.Count != 0) // 11.03.24 minhan
            {
                viewSE.Initialize(seApUnits[0]);
                this.Controls.Add(viewSE);
                viewSE.txtSetCDA.LimitHigh = "10";
                viewSE.txtSetCDA.LimitLow = "0";
                viewSE.txtSetN2.LimitHigh = "1500";
                viewSE.txtSetN2.LimitLow = "700";
                viewSE.txtSetVol.LimitHigh = "14";
                viewSE.txtSetVol.LimitLow = "7";
            }
        }
        private void tmrUpdateApSet(object sender, EventArgs e) // 09.10.08 minhan
        {
            //Mr.kang GaugeInterlock 관련 수정
            if (psmApUnits != null && psmApUnits.Count != 0) // 10.12.21 minhan
            {
                if ((viewPSM.txtSetCDA.Text != "") && (viewPSM.txtSetCDA.Text != null))
                {
                    GlobalVar.ApManualCDASet = Convert.ToDouble(viewPSM.txtSetCDA.Text);
                }
                else GlobalVar.ApManualCDASet = 0;

                if ((viewPSM.txtSetN2.Text != "") && (viewPSM.txtSetN2.Text != null))
                {
                    GlobalVar.ApManualN2Set = Convert.ToDouble(viewPSM.txtSetN2.Text);
                }
                else GlobalVar.ApManualN2Set = 0;

                if ((viewPSM.txtSetVol.Text != "") && (viewPSM.txtSetVol.Text != null))
                {
                    GlobalVar.ApManualVolSet = Convert.ToDouble(viewPSM.txtSetVol.Text);
                }
                else GlobalVar.ApManualVolSet = 0;
            }
            else if (seApUnits != null && seApUnits.Count != 0)
            {
                if ((viewSE.txtSetN2.Text != "") && (viewSE.txtSetN2.Text != null))
                {
                    GlobalVar.ApManualN2Set = Convert.ToDouble(viewSE.txtSetN2.Text);
                }
                else GlobalVar.ApManualN2Set = 0;

                if ((viewSE.txtSetCDA.Text != "") && (viewSE.txtSetCDA.Text != null))
                {
                    GlobalVar.ApManualCDASet = Convert.ToDouble(viewSE.txtSetCDA.Text);
                }
                else GlobalVar.ApManualCDASet = 0;

                if ((viewSE.txtSetVol.Text != "") && (viewSE.txtSetVol.Text != null))
                {
                    GlobalVar.ApManualVolSet = Convert.ToDouble(viewSE.txtSetVol.Text);
                }
                else GlobalVar.ApManualVolSet = 0;
            }
        }
    }
}
