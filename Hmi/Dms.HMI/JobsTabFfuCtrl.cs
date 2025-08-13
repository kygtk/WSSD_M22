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
using Dms.Data;

namespace Dms.HMI
{
    public partial class JobsTabFfuCtr : UserControl // 11.03.07 minhan
    {
        #region Fields
        private ServerManager m_Server;
        private FanFilterControl m_fanCon;
        private string m_Msg;
        private string m_oldMsg;
        private bool m_Use;
        #endregion

        public JobsTabFfuCtr()
        {

            InitializeComponent();

            m_Server = ServerManager.Instance;
            m_fanCon = eqpFanFilterControls._LD_Unit_Fan_Filter_Control;
        }

        public void Initialize()
        {
            tmrUpdateState.Enabled = true;
            m_Msg = "";
            m_oldMsg = "";
            m_Use = false;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
            {
                m_Use = m_fanCon.SetupFfuControlIntr.GetValue<bool>();

                if (m_Use && !GlobalVar.FfuControlComErr)
                {
                    this.BtnCur1.Checked = m_fanCon.IsCurrentAlarm(0);
                    this.BtnCur2.Checked = m_fanCon.IsCurrentAlarm(1);
                    this.BtnCur3.Checked = m_fanCon.IsCurrentAlarm(2);
                    this.BtnCur4.Checked = m_fanCon.IsCurrentAlarm(3);
                    this.BtnCur5.Checked = m_fanCon.IsCurrentAlarm(4);
                    this.BtnCur6.Checked = m_fanCon.IsCurrentAlarm(5);
                    this.BtnCur7.Checked = m_fanCon.IsCurrentAlarm(6);
                    this.BtnCur8.Checked = m_fanCon.IsCurrentAlarm(7);
                    this.BtnCur9.Checked = m_fanCon.IsCurrentAlarm(8);
                    this.BtnCur10.Checked = m_fanCon.IsCurrentAlarm(9);
                    this.BtnCur11.Checked = m_fanCon.IsCurrentAlarm(10);
                    this.BtnCur12.Checked = m_fanCon.IsCurrentAlarm(11);
                    this.BtnCur13.Checked = m_fanCon.IsCurrentAlarm(12);
                    this.BtnCur14.Checked = m_fanCon.IsCurrentAlarm(13);
                    this.BtnCur15.Checked = m_fanCon.IsCurrentAlarm(14);
                    this.BtnCur16.Checked = m_fanCon.IsCurrentAlarm(15);
                    this.BtnCur17.Checked = m_fanCon.IsCurrentAlarm(16);
                    this.BtnCur18.Checked = m_fanCon.IsCurrentAlarm(17);
                    this.BtnCur19.Checked = m_fanCon.IsCurrentAlarm(18);
                    this.BtnCur20.Checked = m_fanCon.IsCurrentAlarm(19);
                    this.BtnCur21.Checked = m_fanCon.IsCurrentAlarm(20);
                    this.BtnCur22.Checked = m_fanCon.IsCurrentAlarm(21);
                    this.BtnCur23.Checked = m_fanCon.IsCurrentAlarm(22);
                    this.BtnCur24.Checked = m_fanCon.IsCurrentAlarm(23);
                    this.BtnCur25.Checked = m_fanCon.IsCurrentAlarm(24);
                    this.BtnCur26.Checked = m_fanCon.IsCurrentAlarm(25);
                    this.BtnCur27.Checked = m_fanCon.IsCurrentAlarm(26);
                    this.BtnCur28.Checked = m_fanCon.IsCurrentAlarm(27);
                    this.BtnCur29.Checked = m_fanCon.IsCurrentAlarm(28);
                    this.BtnCur30.Checked = m_fanCon.IsCurrentAlarm(29);
                    this.BtnCur31.Checked = m_fanCon.IsCurrentAlarm(30);
                    this.BtnCur32.Checked = m_fanCon.IsCurrentAlarm(31);

                    this.BtnMotor1.Checked = m_fanCon.IsMotorAlarm(0);
                    this.BtnMotor2.Checked = m_fanCon.IsMotorAlarm(1);
                    this.BtnMotor3.Checked = m_fanCon.IsMotorAlarm(2);
                    this.BtnMotor4.Checked = m_fanCon.IsMotorAlarm(3);
                    this.BtnMotor5.Checked = m_fanCon.IsMotorAlarm(4);
                    this.BtnMotor6.Checked = m_fanCon.IsMotorAlarm(5);
                    this.BtnMotor7.Checked = m_fanCon.IsMotorAlarm(6);
                    this.BtnMotor8.Checked = m_fanCon.IsMotorAlarm(7);
                    this.BtnMotor9.Checked = m_fanCon.IsMotorAlarm(8);
                    this.BtnMotor10.Checked = m_fanCon.IsMotorAlarm(9);
                    this.BtnMotor11.Checked = m_fanCon.IsMotorAlarm(10);
                    this.BtnMotor12.Checked = m_fanCon.IsMotorAlarm(11);
                    this.BtnMotor13.Checked = m_fanCon.IsMotorAlarm(12);
                    this.BtnMotor14.Checked = m_fanCon.IsMotorAlarm(13);
                    this.BtnMotor15.Checked = m_fanCon.IsMotorAlarm(14);
                    this.BtnMotor16.Checked = m_fanCon.IsMotorAlarm(15);
                    this.BtnMotor17.Checked = m_fanCon.IsMotorAlarm(16);
                    this.BtnMotor18.Checked = m_fanCon.IsMotorAlarm(17);
                    this.BtnMotor19.Checked = m_fanCon.IsMotorAlarm(18);
                    this.BtnMotor10.Checked = m_fanCon.IsMotorAlarm(19);
                    this.BtnMotor21.Checked = m_fanCon.IsMotorAlarm(20);
                    this.BtnMotor22.Checked = m_fanCon.IsMotorAlarm(21);
                    this.BtnMotor23.Checked = m_fanCon.IsMotorAlarm(22);
                    this.BtnMotor24.Checked = m_fanCon.IsMotorAlarm(23);
                    this.BtnMotor25.Checked = m_fanCon.IsMotorAlarm(24);
                    this.BtnMotor26.Checked = m_fanCon.IsMotorAlarm(25);
                    this.BtnMotor27.Checked = m_fanCon.IsMotorAlarm(26);
                    this.BtnMotor28.Checked = m_fanCon.IsMotorAlarm(27);
                    this.BtnMotor29.Checked = m_fanCon.IsMotorAlarm(28);
                    this.BtnMotor30.Checked = m_fanCon.IsMotorAlarm(29);
                    this.BtnMotor31.Checked = m_fanCon.IsMotorAlarm(30);
                    this.BtnMotor32.Checked = m_fanCon.IsMotorAlarm(31);

                    this.BtnConn1.Checked = m_fanCon.IsNoConnect(0);
                    this.BtnConn2.Checked = m_fanCon.IsNoConnect(1);
                    this.BtnConn3.Checked = m_fanCon.IsNoConnect(2);
                    this.BtnConn4.Checked = m_fanCon.IsNoConnect(3);
                    this.BtnConn5.Checked = m_fanCon.IsNoConnect(4);
                    this.BtnConn6.Checked = m_fanCon.IsNoConnect(5);
                    this.BtnConn7.Checked = m_fanCon.IsNoConnect(6);
                    this.BtnConn8.Checked = m_fanCon.IsNoConnect(7);
                    this.BtnConn9.Checked = m_fanCon.IsNoConnect(8);
                    this.BtnConn10.Checked = m_fanCon.IsNoConnect(9);
                    this.BtnConn11.Checked = m_fanCon.IsNoConnect(10);
                    this.BtnConn12.Checked = m_fanCon.IsNoConnect(11);
                    this.BtnConn13.Checked = m_fanCon.IsNoConnect(12);
                    this.BtnConn14.Checked = m_fanCon.IsNoConnect(13);
                    this.BtnConn15.Checked = m_fanCon.IsNoConnect(14);
                    this.BtnConn16.Checked = m_fanCon.IsNoConnect(15);
                    this.BtnConn17.Checked = m_fanCon.IsNoConnect(16);
                    this.BtnConn18.Checked = m_fanCon.IsNoConnect(17);
                    this.BtnConn19.Checked = m_fanCon.IsNoConnect(18);
                    this.BtnConn10.Checked = m_fanCon.IsNoConnect(19);
                    this.BtnConn21.Checked = m_fanCon.IsNoConnect(20);
                    this.BtnConn22.Checked = m_fanCon.IsNoConnect(21);
                    this.BtnConn23.Checked = m_fanCon.IsNoConnect(22);
                    this.BtnConn24.Checked = m_fanCon.IsNoConnect(23);
                    this.BtnConn25.Checked = m_fanCon.IsNoConnect(24);
                    this.BtnConn26.Checked = m_fanCon.IsNoConnect(25);
                    this.BtnConn27.Checked = m_fanCon.IsNoConnect(26);
                    this.BtnConn28.Checked = m_fanCon.IsNoConnect(27);
                    this.BtnConn29.Checked = m_fanCon.IsNoConnect(28);
                    this.BtnConn30.Checked = m_fanCon.IsNoConnect(29);
                    this.BtnConn31.Checked = m_fanCon.IsNoConnect(30);
                    this.BtnConn32.Checked = m_fanCon.IsNoConnect(31);

                    this.textBoxSpeed1.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(0));
                    this.textBoxSpeed2.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(1));
                    this.textBoxSpeed3.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(2));
                    this.textBoxSpeed4.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(3));
                    this.textBoxSpeed5.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(4));
                    this.textBoxSpeed6.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(5));
                    this.textBoxSpeed7.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(6));
                    this.textBoxSpeed8.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(7));
                    this.textBoxSpeed9.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(8));
                    this.textBoxSpeed10.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(9));
                    this.textBoxSpeed11.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(10));
                    this.textBoxSpeed12.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(11));
                    this.textBoxSpeed13.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(12));
                    this.textBoxSpeed14.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(13));
                    this.textBoxSpeed15.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(14));
                    this.textBoxSpeed16.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(15));
                    this.textBoxSpeed17.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(16));
                    this.textBoxSpeed18.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(17));
                    this.textBoxSpeed19.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(18));
                    this.textBoxSpeed20.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(19));
                    this.textBoxSpeed21.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(20));
                    this.textBoxSpeed22.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(21));
                    this.textBoxSpeed23.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(22));
                    this.textBoxSpeed24.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(23));
                    this.textBoxSpeed25.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(24));
                    this.textBoxSpeed26.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(25));
                    this.textBoxSpeed27.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(26));
                    this.textBoxSpeed28.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(27));
                    this.textBoxSpeed29.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(28));
                    this.textBoxSpeed30.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(29));
                    this.textBoxSpeed31.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(30));
                    this.textBoxSpeed32.Text = string.Format("{0}", m_fanCon.IsCurMotorSpeed(31));

                    m_Msg = "";
                    m_oldMsg = "";
                    this.FfuDisplay.Text = "FFU Status Comport Normal";
                }
                else
                {
                    if (!m_Use)
                    {
                        this.FfuDisplay.Text = "FFU Unit No Use";
                    }
                    else
                    {
                        this.FfuDisplay.Text = "FFU Status Comport Error";
                    }
                }
            }
            catch (Exception err)
            {
                m_Msg = err.ToString();

                if (m_Msg != m_oldMsg)
                {
                    m_oldMsg = m_Msg;
                    m_Server.WriteExceptionLog(m_Msg);
                    this.FfuDisplay.Text = "FFU Status Format Error";
                }
            }
        }
    }
}
