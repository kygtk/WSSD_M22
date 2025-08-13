using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using Dms.Common;
using Dms.Data.DataSetSetupItemTableAdapters;

namespace Dms.Data
{
    public class SetupGaugeInterlockAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private bool m_Initialized = false;
        private DataSetSetupItem.SetupGaugeInterlockDataTable m_Table;
        private SetupGaugeInterlockTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public SetupGaugeInterlockAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsSetupConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.SetupDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsSetupConnectionString =
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
                MessageBox.Show("Setup database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Setup";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsSetup.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.SetupDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public DataSetSetupItem.SetupGaugeInterlockRow Convert2Row(TagGaugeInterlock item)
        {
            DataSetSetupItem.SetupGaugeInterlockRow row = m_Table.NewSetupGaugeInterlockRow();
            row.Name = item.Name;
            row.Usage = item.Use;
            row.UnitType = item.Unit.Unit.ToString();
            row.LowAlarm = item.LowAlarm;
            row.LowWarning = item.LowWarning;
            row.SettingValue = item.SettingValue; // 10.10.29 minhan
            row.HighWarning = item.HighWarning;
            row.HighAlarm = item.HighAlarm;
            row._DelayTime_sec_ = item.DelayTime; // 11.01.27 minhan
            return row;
        }

        public TagGaugeInterlock Convert2Tag(DataSetSetupItem.SetupGaugeInterlockRow row)
        {
            TagGaugeInterlock item = new TagGaugeInterlock();
            item.Name = row.Name;
            item.Use = row.Usage;
            item.Unit = new TagUnit((UnitType)Enum.Parse(typeof(UnitType), row.UnitType, true));
            item.LowAlarm = row.LowAlarm;
            item.LowWarning = row.LowWarning;
            item.SettingValue = row.SettingValue; // 10.10.29 minhan
            item.HighWarning = row.HighWarning;
            item.HighAlarm = row.HighAlarm;
            item.DelayTime = row._DelayTime_sec_; // 11.01.27 minhan
            return item;
        }

        public bool IsChanged(DataSetSetupItem.SetupGaugeInterlockRow row1, DataSetSetupItem.SetupGaugeInterlockRow row2, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, row2.ItemArray, ignoreIndex);
        }

        public bool IsChanged(DataSetSetupItem.SetupGaugeInterlockRow row1, TagGaugeInterlock item, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, Convert2Row(item).ItemArray, ignoreIndex);
        }

        public bool IsChanged()
        {
            bool changed = false;
            foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    changed = true;
                }
            }
            return changed;
        }

        public void RejectChanges()
        {
            foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    row.RejectChanges();
                }
            }
        }

        public bool InitAdapter()
        {
            if (m_Initialized) return true;

            m_Table = new DataSetSetupItem.SetupGaugeInterlockDataTable();
            m_Adapter = new SetupGaugeInterlockTableAdapter();
            m_Adapter.Fill(m_Table);

            m_Initialized = true;

            return true;
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

        public bool LoadFromDB()
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                m_Table.Clear();
                m_Adapter.Fill(m_Table);

                return true;
            }
        }

        public bool LoadFromDB(SetupGaugeInterlockList list)
        {
            lock (m_LockKey)
            {
                if (true == LoadFromDB())
                {
                    list.Items.Clear();

                    foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
                    {
                        list.Items.Add(Convert2Tag(row));
                    }

                    return true;
                }

                return false;
            }
        }


        public void SaveToDB()
        {
            UpdateToDB(m_Table);
        }


        public void SaveToDB(SetupGaugeInterlockList list)
        {
            lock (m_LockKey)
            {
                m_Table.Clear();

                foreach (TagGaugeInterlock item in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(item));
                }

                UpdateToDB(m_Table);
            }
        }


        public bool InitFromDB(TagGaugeInterlock item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                bool find = false;

                foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
                {
                    if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Clone(Convert2Tag(row));
                        find = true;
                        break;
                    }
                }

                if (find == false)
                {
                    this.Add(item);
                }
            }

            return true;
        }


        public bool UpdateFromDB(string name, TagGaugeInterlock item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
                {
                    if (string.Equals(name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Clone(Convert2Tag(row));

                        return true;
                    }
                }

                return false;
            }
        }

        public void UpdateFromDB(SetupGaugeInterlockList list)
        {
            //LoadFromDB(); // 10.08.03 minhan

            foreach (TagGaugeInterlock item in list.Items)
            {
                UpdateFromDB(item.Name, item);
            }
        }


        public bool UpdateToDB(TagGaugeInterlock item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
                {
                    if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        row.BeginEdit();
                        row.ItemArray = Convert2Row(item).ItemArray;
                        row.EndEdit();

                        UpdateToDB(m_Table);
                        return true;
                    }
                }

                return true;
            }
        }


        public void UpdateToDB(DataSetSetupItem.SetupGaugeInterlockDataTable table)
        {
            try
            {
                //jemoon : Log
                WriteDataChangeLog(table);

                int rv = m_Adapter.Update(table);
                if (rv > 0)
                {
                    //table.AcceptChanges();
                }
                else
                {
                    // TODO : need recovery or rollback
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("UpdateToDB : failed - " + err.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(TagGaugeInterlock item)
        {
            lock (m_LockKey)
            {
                m_Table.Rows.Add(Convert2Row(item));

                UpdateToDB(m_Table);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Remove(TagGaugeInterlock item)
        {
            lock (m_LockKey)
            {
                foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
                {
                    if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Delete();

                        UpdateToDB(m_Table);
                        break;
                    }
                }
            }
        }

        public void DeleteGarbage(SetupGaugeInterlockList list)
        {
            //2009.05.14 eun
            //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
            //그래서 아래와 같이 역순으로 처리한다.
            int count = m_Table.Rows.Count - 1;
            for (int i = count; i > -1; i--)
            {
                DataSetSetupItem.SetupGaugeInterlockRow row = (DataSetSetupItem.SetupGaugeInterlockRow)m_Table.Rows[i];

                bool match = false;
                foreach (TagGaugeInterlock item in list.Items)
                {
                    if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        match = true;
                        break;
                    }
                }

                if (match == false)
                {
                    row.Delete();
                }
            }
            //foreach (DataSetSetupItem.SetupGaugeInterlockRow row in m_Table.Rows)
            //{
            //    bool match = false;
            //    foreach (TagGaugeInterlock item in list.Items)
            //    {
            //        if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
            //        {
            //            match = true;
            //            break;
            //        }
            //    }

            //    if (match == false)
            //    {
            //        row.Delete();
            //    }
            //}

            UpdateToDB(m_Table);
        }
        #endregion
    }
}
