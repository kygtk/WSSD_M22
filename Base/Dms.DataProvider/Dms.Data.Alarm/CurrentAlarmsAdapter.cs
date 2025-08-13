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
    public class CurrentAlarmsAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private bool m_Initialized = false;
        private DataSetAlarm.CurrentAlarmsDataTable m_Table;
        private CurrentAlarmsTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public CurrentAlarmsAdapter()
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

        private DataSetAlarm.CurrentAlarmsRow Convert2Row(TagCurrentAlarm item)
        {
            DataSetAlarm.CurrentAlarmsRow row = m_Table.NewCurrentAlarmsRow();
            row.AlarmId = item.Id;
            row.AlarmName = item.Name;
            return row;
        }

        private TagCurrentAlarm Convert2Tag(DataRow row)
        {
            DataSetAlarm.CurrentAlarmsRow alarmRow = m_Table.NewCurrentAlarmsRow();
            alarmRow.ItemArray = row.ItemArray;

            return Convert2Tag(alarmRow);
        }

        private TagCurrentAlarm Convert2Tag(DataSetAlarm.CurrentAlarmsRow row)
        {
            TagCurrentAlarm item = new TagCurrentAlarm();
            item.Id = row.AlarmId;
            item.Name = row.AlarmName;
            return item;
        }

        public bool InitAdapter()
        {
            if (m_Initialized) return true;

            m_Table = new DataSetAlarm.CurrentAlarmsDataTable();
            m_Adapter = new CurrentAlarmsTableAdapter();
            m_Adapter.Fill(m_Table);

            m_Initialized = true;

            return true;
        }


        public bool IsAlarm(TagAlarm alarm)
        {
            bool exist = false;

            lock (m_LockKey)
            {
                foreach (DataSetAlarm.CurrentAlarmsRow row in m_Table.Rows)
                {
                    if (alarm.Id == row.AlarmId)
                    {
                        exist = true;
                        break;
                    }
                }
            }

            return exist;
        }

        public void ClearDB()
        {
            lock (m_LockKey)
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
            lock (m_LockKey)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void LoadFromDB(CurrentAlarms list)
        {
            lock (m_LockKey)
            {
                LoadFromDB();

                list.Items.Clear();

                foreach (DataSetAlarm.CurrentAlarmsRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }


        public void SaveToDB(CurrentAlarms list)
        {
            lock (m_LockKey)
            {
                m_Table.Clear();

                foreach (TagCurrentAlarm item in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(item));
                }

                UpdateToDB(m_Table);
            }
        }

        public void UpdateFromDB()
        {
            lock (m_LockKey)
            {
                LoadFromDB();
            }
        }


        public void UpdateFromDB(CurrentAlarms list)
        {
            lock (m_LockKey)
            {
                UpdateFromDB();

                int listCount = list.Count;
                int tableCount = m_Table.Rows.Count;

                if (listCount > tableCount)
                {   // case : deleted
                    for (int i = listCount; i > tableCount; i--)
                    {
                        list.Items.RemoveAt(i - 1);
                    }
                }

                int rowCount = m_Table.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    if (i >= listCount)
                    {
                        list.Add(Convert2Tag(m_Table.Rows[i]));
                    }
                    else if (IsChanged(m_Table.Rows[i].ItemArray, Convert2Row(list.Items[i]).ItemArray))
                    {
                        list.Items[i].Clone(Convert2Tag(m_Table.Rows[i]));
                    }
                }
            }
        }



        public void UpdateToDB(DataSetAlarm.CurrentAlarmsDataTable table)
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
                MessageBox.Show(err.Message);
            }
        }

        public void UpdateToDB()
        {
            lock (m_LockKey)
            {
                UpdateToDB(m_Table);
            }
        }

        public void Add(TagCurrentAlarm alarm)
        {
            lock (m_LockKey)
            {
                m_Table.Rows.Add(Convert2Row(alarm));
                UpdateToDB(m_Table);
            }
        }

        public void Add(TagAlarm alarm)
        {
            lock (m_LockKey)
            {
                TagCurrentAlarm currentAlarm = new TagCurrentAlarm(alarm.Id, alarm.Name);
                Add(currentAlarm);
            }
        }

        public void Remove(int alarmId)
        {
            lock (m_LockKey)
            {
                int count = m_Table.Rows.Count;
                foreach (DataSetAlarm.CurrentAlarmsRow row in m_Table.Rows)
                {
                    if (alarmId == row.AlarmId)
                    {
                        row.Delete();

                        UpdateToDB(m_Table);
                        break;
                    }
                }
            }
        }

        //Alarm/Warning 구분없이 모두 반환
        public List<int> GetCurrentAlarmIds()
        {
            lock (m_LockKey)
            {
                List<int> ids = new List<int>();
                foreach (DataSetAlarm.CurrentAlarmsRow row in m_Table.Rows)
                {
                    ids.Add(row.AlarmId);
                }
                return ids;
            }
        }
        #endregion
    }
}
