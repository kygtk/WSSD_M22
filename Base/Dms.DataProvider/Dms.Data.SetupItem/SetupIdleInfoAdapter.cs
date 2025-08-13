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
    public class SetupIdleInfoAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private bool m_Initialized = false;
        private DataSetSetupItem.SetupIdleInfoDataTable m_Table;
        private SetupIdleInfoTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public SetupIdleInfoAdapter()
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

        private DataSetSetupItem.SetupIdleInfoRow Convert2Row(TagSetupInfo item)
        {
            DataSetSetupItem.SetupIdleInfoRow row = m_Table.NewSetupIdleInfoRow();
            row.Name = item.Name;
            row.OptionType = item.Type.ToString();
            row.OptionFormat = item.Format.ToString();
            row.UnitType = item.Unit.Unit.ToString();
            row.ItemValue = item.Val;
            row.LoTerm = item.LoTerm;
            row.HiTerm = item.HiTerm;
            return row;
        }

        private TagSetupInfo Convert2Tag(DataSetSetupItem.SetupIdleInfoRow row)
        {
            TagSetupInfo item = new TagSetupInfo();
            item.Name = row.Name;
            item.Type = (OptionType)(Enum.Parse(typeof(OptionType), row.OptionType, true));
            item.Format = (OptionFormat)(Enum.Parse(typeof(OptionFormat), row.OptionFormat, true));
            item.Unit = new TagUnit((UnitType)Enum.Parse(typeof(UnitType), row.UnitType, true));
            item.Val = row.ItemValue;
            item.LoTerm = row.LoTerm;
            item.HiTerm = row.HiTerm;
            return item;
        }

        private void UpdateDataFormat(DataSetSetupItem.SetupIdleInfoRow row, TagSetupInfo item)
        {
            row.BeginEdit();
            row.OptionType = item.Type.ToString();
            row.OptionFormat = item.Format.ToString();
            row.UnitType = item.Unit.Unit.ToString();
            row.LoTerm = item.LoTerm;
            row.HiTerm = item.HiTerm;
            row.EndEdit();
        }

        public bool IsChanged(DataSetSetupItem.SetupIdleInfoRow row1, DataSetSetupItem.SetupIdleInfoRow row2, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, row2.ItemArray, ignoreIndex);
        }

        public bool IsChanged(DataSetSetupItem.SetupIdleInfoRow row1, TagSetupInfo item, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, Convert2Row(item).ItemArray, ignoreIndex);
        }

        public bool IsChanged()
        {
            bool changed = false;
            foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
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
            foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    row.RejectChanges();
                }
            }
        }

        public bool InitAdapter()
        {
            if (m_Initialized == true) return true;

            m_Table = new DataSetSetupItem.SetupIdleInfoDataTable();
            m_Adapter = new SetupIdleInfoTableAdapter();
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

        public bool LoadFromDB(SetupIdleInfoList list)
        {
            lock (m_LockKey)
            {
                if (true == LoadFromDB())
                {
                    list.Items.Clear();

                    foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
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


        public void SaveToDB(SetupIdleInfoList list)
        {
            lock (m_LockKey)
            {
                m_Table.Clear();

                foreach (TagSetupInfo item in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(item));
                }

                UpdateToDB(m_Table);
            }
        }


        public bool InitFromDB(TagSetupInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                bool find = false;
                int[] ignore = { m_Table.ItemValueColumn.Ordinal };
                foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
                {
                    if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Format이 변경되었는지?
                        if (IsChanged(row, item, ignore))
                        {
                            UpdateDataFormat(row, item);

                            UpdateToDB(m_Table);
                        }

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


        public bool UpdateFromDB(string name, TagSetupInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
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

        public void UpdateFromDB(SetupIdleInfoList list)
        {
            //LoadFromDB(); // 10.08.03 minhan

            foreach (TagSetupInfo item in list.Items)
            {
                UpdateFromDB(item.Name, item);
            }
        }


        public bool UpdateToDB(TagSetupInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
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


        public void UpdateToDB(DataSetSetupItem.SetupIdleInfoDataTable table)
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
        public void Add(TagSetupInfo item)
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
        public void Remove(TagSetupInfo item)
        {
            lock (m_LockKey)
            {
                foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
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

        public void DeleteGarbage(SetupIdleInfoList list)
        {
            //2009.05.14 eun
            //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
            //그래서 아래와 같이 역순으로 처리한다.
            int count = m_Table.Rows.Count - 1;
            for (int i = count; i > -1; i--)
            {
                DataSetSetupItem.SetupIdleInfoRow row = (DataSetSetupItem.SetupIdleInfoRow)m_Table.Rows[i];

                bool match = false;
                foreach (TagSetupInfo item in list.Items)
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
            //foreach (DataSetSetupItem.SetupIdleInfoRow row in m_Table.Rows)
            //{
            //    bool match = false;
            //    foreach (TagSetupInfo item in list.Items)
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
