using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;
using System.Data;

namespace Dms.Data
{
    public class AlarmHistoryProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        //private AlarmHistory m_List = null;       
        private AlarmHistoryAdapter m_Adapter = null;
        private ViewAlarmHistory m_Viewer = null;
        #endregion

        #region Properties
        public AlarmHistoryAdapter Adapter
        {
            get { return m_Adapter; }
        }

        public ViewAlarmHistory Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly AlarmHistoryProvider Instance = new AlarmHistoryProvider();
        #endregion

        #region Constructor
        private AlarmHistoryProvider()
        {
            m_Adapter = new AlarmHistoryAdapter();
        }

        #endregion

        #region Methods
        public void Create()
        {

        }

        // Call by sequence
        public void Add(TagAlarmHistory history)
        {
            lock (m_LockKey)
            {
                if (m_Viewer != null)
                {
                    m_Viewer.AddAlarm(history);
                }
                else
                {
                    InvokeAdd(history);
                }
            }
        }

        // Call by viewer
        public void InvokeAdd(TagAlarmHistory history)
        {
            m_Adapter.Add(history);
        }

        // Call by sequence
        public void Remove(List<TagAlarmHistory> alarms)
        {
            lock (m_LockKey)
            {
                m_Adapter.Remove(alarms);
            }
        }

        // Call by viewer
        public void Remove(DataGridViewSelectedRowCollection selectedItems)
        {
            try
            {
                List<TagAlarmHistory> alarms = new List<TagAlarmHistory>();
                foreach (DataGridViewRow viewRow in selectedItems)
                {
                    DataSetAlarm.AlarmHistoryRow row;
                    row = (DataSetAlarm.AlarmHistoryRow)(((DataRowView)(viewRow.DataBoundItem)).Row);
                    alarms.Add(m_Adapter.Convert2Tag(row));
                }

                Remove(alarms);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void LoadFromDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFromDB();
            }
        }

        //public void UpdateFromDB()
        //{
        //    lock(m_LockKey)
        //    {
        //        m_Adapter.UpdateFromDB();
        //    }
        //}

        public void UpdateToDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.UpdateToDB();
            }
        }

        public void WriteText(string fileName)
        {
            try
            {
                StreamWriter sw = File.CreateText(fileName);
                sw.AutoFlush = true;

                string txt = "No.\tTime\t\t\tId\tLevel\tDescription";
                sw.WriteLine(txt);
                sw.WriteLine("================================================================");


                int i = 0;
                foreach (DataSetAlarm.AlarmHistoryRow row in m_Adapter.Table.Rows)
                {
                    TagAlarmHistory history = m_Adapter.Convert2Tag(row);
                    txt = string.Format("{0}\t{1}\t{2:d4}\t{3}\t{4}", ++i, history.Time, history.Id, history.Level, history.Name);
                    sw.WriteLine(txt);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void WriteText()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save As...";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "AlarmHistory.txt";
            dlg.DefaultExt = "txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;//2009.10.14 okt
            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    this.WriteText(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion
    }
}
