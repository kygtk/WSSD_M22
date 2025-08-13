using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using Dms.Common;
using Dms.Data.DataSetAlarmTableAdapters;

namespace Dms.Data
{
    public class AlarmHistoryAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetAlarm.AlarmHistoryDataTable m_Table;
        private AlarmHistoryTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public AlarmHistoryAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsAlarmsConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.AlarmDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsAlarmsConnectionString =
                    MakeDatabaseConnectionString(connectionString, path);

                this.InitAdapter();
            }
            else
            {
                Created = false;
                return;
            }
        }
        #endregion

        #region Methods
        private bool CheckFilePath(string fileName, ref string newFileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "default";   //if filename is null or empty, System.IO.FileInfo will throw exception
            }

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (fileInfo.Exists)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Alarm database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Alarm";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsAlarms.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.AlarmDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DataSetAlarm.AlarmHistoryRow Convert2Row(TagAlarmHistory item)
        {
            DataSetAlarm.AlarmHistoryRow row = m_Table.NewAlarmHistoryRow();
            row.AlarmId = item.Id;
            row.AlarmName = item.Name;
            row.AlarmLevel = item.Level.ToString();
            row.AlarmTime = item.Time;
            return row;
        }

        public TagAlarmHistory Convert2Tag(DataSetAlarm.AlarmHistoryRow row)
        {
            TagAlarm alarm = new TagAlarm();
            alarm.Id = row.AlarmId;
            alarm.Name = row.AlarmName;
            alarm.Level = (AlarmLevel)Enum.Parse(typeof(AlarmLevel), row.AlarmLevel, true);
            TagAlarmHistory history = new TagAlarmHistory(alarm, row.AlarmTime);

            return history;      
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetAlarm.AlarmHistoryDataTable();
            m_Adapter = new AlarmHistoryTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        public void ClearDB()
        {
            lock(m_LockKey)
            {
                //2009.05.14 eun
                //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
                //그래서 아래와 같이 역순으로 처리한다.
                int count = m_Table.Rows.Count;
                if (0 < count)
                {
                    for (int i = count - 1; i > -1; i--)
                    {
                        m_Table.Rows[i].Delete();
                    }

                    UpdateToDB(m_Table);
                }

                //int count = m_Table.Rows.Count;
                //if (0 < count)
                //{
                //    foreach (DataRow row in m_Table.Rows)
                //    {
                //        row.Delete();
                //    }

                //    UpdateToDB(m_Table);
                //}
            }
        }

        public void LoadFromDB()
        {
            lock(m_LockKey)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }        


        public void UpdateToDB(DataSetAlarm.AlarmHistoryDataTable table)
        {
            lock(m_LockKey)
            {
                try
                {
                    int rv = m_Adapter.Update(table);
                    if (rv > 0)
                    {
                        //m_Table.AcceptChanges();
                    }
                    else
                    {
                        // TODO : need recovery
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("UpdateToDB : failed - " + err.Message);
                }
            }        
        }

        public void Add(TagAlarmHistory history)
        {
            lock(m_LockKey)
            {
                m_Table.Rows.Add(Convert2Row(history));
                UpdateToDB(m_Table);
            }
        }

        private void Remove(TagAlarmHistory alarm)
        {
            lock(m_LockKey)
            {
                foreach (DataSetAlarm.AlarmHistoryRow row in m_Table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    if (string.Equals(alarm.Time, row.AlarmTime, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(alarm.Name, row.AlarmName, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Delete();
                        break;
                    }
                }
            }
        }

        public void Remove(List<TagAlarmHistory> alarms)
        {
            foreach (TagAlarmHistory history in alarms)
            {
                Remove(history);
            }

            UpdateToDB(m_Table);
        }


        #endregion
    }
}
