using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;

namespace Dms.Server
{
    public partial class FormAlarmList : Form
    {
        private ServerManager m_Server = ServerManager.Instance;
        private AlarmListProvider m_AlarmListProvider;
        private CurrentAlarmsProvider m_CurrentAlarmsProvider;
        
        public FormAlarmList()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void InitObject()
        {
            try
            {
                m_AlarmListProvider = m_Server.DataProvider.AlarmList;
                m_CurrentAlarmsProvider = m_Server.DataProvider.CurrentAlarms;

                viewAlarmList.InitGridView(m_AlarmListProvider);
                viewCurrentAlarms.InitGridView(m_CurrentAlarmsProvider);
                //viewAlarmHistory.InitAlarmHistoryView(m_AlarmProvider);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonAlarmSet_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textAlarmId.Text == "")
                {
                    return;
                }

                int alarmId = Convert.ToInt32(this.textAlarmId.Text);

                m_Server.EqpStateManager.SetAlarm(alarmId);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonAlarmReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textAlarmId.Text == "")
                {
                    return;
                }

                int alarmId = Convert.ToInt32(this.textAlarmId.Text);

                m_Server.EqpStateManager.ResetAlarm(alarmId);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                m_AlarmListProvider.SaveToDB();

                MessageBox.Show("Update Complete : AlarmList Database!");
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}