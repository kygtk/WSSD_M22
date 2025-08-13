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
    public partial class ViewCstSlot : UserControl
    {
        private int m_SlotId;
        public int SlotId
        {
            get { return m_SlotId; }
            set { m_SlotId = value; }
        }
        
        private Color m_ColorOn = Color.Chartreuse;
        private Color m_ColorOff = Color.Silver;

        private static Color m_ColorEmpty = Color.Silver;
        private static Color m_ColorSelected = Color.YellowGreen;
        private static Color m_ColorWait = Color.GreenYellow;
        private static Color m_ColorStarted = Color.LimeGreen;
        private static Color m_ColorProc = Color.Green;
        private static Color m_ColorAborted = Color.Gray;
        private static Color m_ColorCanceled = Color.Gray;
        private static Color m_ColorOk = Color.DeepSkyBlue;
        private static Color m_ColorNg = Color.Orange;
        private static Color m_ColorScraped = Color.Red;
        private static Color m_ColorExist = Color.Yellow;

        public bool Exist
        {
            set 
            {
               SetSlotStatus(value ? m_ColorOn : m_ColorOff);
            }
        }

        public ViewCstSlot()
        {
            InitializeComponent();

            this.Exist = false;
        }

        private void SetSlotStatus(Color color)
        {
            this.labelSlotState.BackColor = color;
        }
             

        public void SetSlotStatus(GlassStatus status)
        {
            switch (status)
            { 
                case GlassStatus.Aborted:
                    SetSlotStatus(m_ColorAborted);
                    break;
                case GlassStatus.Canceled:
                    SetSlotStatus(m_ColorCanceled);
                    break;
                case GlassStatus.Empty:
                    SetSlotStatus(m_ColorEmpty);
                    break;
                case GlassStatus.Ng:
                    SetSlotStatus(m_ColorNg);
                    break;
                case GlassStatus.Ok:
                    SetSlotStatus(m_ColorOk);
                    break;
                case GlassStatus.Proc:
                    SetSlotStatus(m_ColorProc);
                    break;
                case GlassStatus.Scraped:
                    SetSlotStatus(m_ColorScraped);
                    break;
                case GlassStatus.Selected:
                    SetSlotStatus(m_ColorSelected);
                    break;
                case GlassStatus.Started:
                    SetSlotStatus(m_ColorStarted);
                    break;
                case GlassStatus.Wait:
                    SetSlotStatus(m_ColorWait);
                    break;
                case GlassStatus.Exist:
                    SetSlotStatus(m_ColorExist);
                    break;
                default:
                    SetSlotStatus(m_ColorOff);
                    break;
            }
        }
    }
}
