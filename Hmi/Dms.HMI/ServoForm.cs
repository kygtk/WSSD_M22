using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Device;
using Dms.Control;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class ServoForm : Form
    {
        #region Fields
        private ClientManager m_Client = ClientManager.Instance;
        //MMC
        private Dms.HMI.ServoTabMainCtrl servoTabMain; // 10.12.25 minhan
        private ServoUnits m_ServoUnits = null;
        //MP2300
        // private ServoTabMp2300Ctrl servoTabMp2300;
        // private ServoUnitMp2300s m_Mp2300s = null;
        #endregion

        #region Constructor
        public ServoForm()
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
        private void ServoForm_Load(object sender, EventArgs e)
        {
            IComponentContainer components = DmsComponents.Instance.ComponentContainer;
            m_ServoUnits = components.GetCollection<ServoUnit>() as ServoUnits;
            //m_Mp2300s = components.GetCollection<ServoUnitMp2300>() as ServoUnitMp2300s; 

            //if ((m_ServoUnits == null || m_ServoUnits.Count == 0) &&
            //    (m_Mp2300s == null || m_Mp2300s.Count == 0)) return;
            if ((m_ServoUnits == null) || (m_ServoUnits.Count == 0)) return;

            if (m_ServoUnits != null && m_ServoUnits.Count > 0)
            {
                servoTabMain = new Dms.HMI.ServoTabMainCtrl(); // 10.12.25 minhan
                servoTabMain.Initialize(m_ServoUnits);

                TabPage tabPageServo = new TabPage("SERVO");
                tabPageServo.UseVisualStyleBackColor = true;
                tabPageServo.Controls.Add(servoTabMain);
                this.tabControl1.Controls.Add(tabPageServo);
            }
            //else if (m_Mp2300s != null && m_Mp2300s.Count > 0)
            //{
            //    servoTabMp2300 = new ServoTabMp2300Ctrl();
            //    Dms.Common.AppConfig app = Dms.Common.AppConfig.Instance;
            //    if (!app.Simul.Motion)
            //    {
            //        servoTabMp2300.UseCompareHomeInfo = false;
            //        servoTabMp2300.UseComparePointInfo = false;
            //    }

            //    servoTabMp2300.Initialize(m_Mp2300s);
            //    TabPage tabPageServo = new TabPage("SERVO");
            //    tabPageServo.UseVisualStyleBackColor = true;
            //    tabPageServo.Controls.Add(servoTabMp2300);
            //    this.tabControl1.Controls.Add(tabPageServo);
            //}
        }

        private void ServoForm_Activated(object sender, EventArgs e)
        {
            if (servoTabMain != null)
            {
                servoTabMain.InitViewObject();
                servoTabMain.UpdateTimerEnabled = true;
            }
            //else if (servoTabMp2300 != null)
            //{
            //    servoTabMp2300.InitViewObject();
            //    servoTabMp2300.UpdateTimerEnabled = true;
            //}
        }

        private void ServoForm_Deactivate(object sender, EventArgs e)
        {
            if (servoTabMain != null)
            {
                servoTabMain.UpdateTimerEnabled = false;
                m_Client.SendCommand(Dms.Common.Command.ServoManual, Dms.Common.ServoManualAct.ResestManualAct);
            }
            //else if (servoTabMp2300 != null)
            //{
            //    servoTabMp2300.UpdateTimerEnabled = false;
            //    m_Client.SendCommand(Dms.Common.Command.ServoMP2300Manaul, Dms.Common.ServoManualAct.ResestManualAct);
            //}
        }
        #endregion
    }
}