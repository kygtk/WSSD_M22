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
    public class AlarmListAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetAlarm.AlarmListDataTable m_Table;
        private AlarmListTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public AlarmListAdapter()
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

        private DataSetAlarm.AlarmListRow Convert2Row(TagAlarm item)
        {
            DataSetAlarm.AlarmListRow row = m_Table.NewAlarmListRow();
            row.AlarmId = item.Id;
            row.AlarmName = item.Name;
            row.AlarmLevel = item.Level.ToString();
            row.AlarmCode = item.Code.ToString();
            return row;
        }

        private TagAlarm Convert2Tag(DataSetAlarm.AlarmListRow row)
        {
            TagAlarm item = new TagAlarm();
            item.Id = row.AlarmId;
            item.Name = row.AlarmName;
            item.Level = (AlarmLevel)Enum.Parse(typeof(AlarmLevel), row.AlarmLevel, true);
            item.Code = (AlarmCode)Enum.Parse(typeof(AlarmCode), row.AlarmCode, true);
            return item;        
        }
        
        
        public bool InitAdapter()
        {
            m_Table = new DataSetAlarm.AlarmListDataTable();
            m_Adapter = new AlarmListTableAdapter();
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

        public void LoadFromDB(AlarmList list)
        {
            lock(m_LockKey)
            {
                LoadFromDB();

                list.Items.Clear();

                foreach (DataSetAlarm.AlarmListRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }


        public void UpdateToDB(DataSetAlarm.AlarmListDataTable table)
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

        public void SaveToDB(AlarmList list)
        {
            lock(m_LockKey)
            {
                ClearDB();

                foreach (TagAlarm alarm in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(alarm));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
