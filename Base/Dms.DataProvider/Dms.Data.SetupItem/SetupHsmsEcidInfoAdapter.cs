using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data.DataSetSetupItemTableAdapters;

namespace Dms.Data
{
    public class SetupHsmsEcidInfoAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private bool m_Initialized = false;
        private DataSetSetupItem.SetupHsmsEcidInfoDataTable m_Table;
        private SetupHsmsEcidInfoTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public SetupHsmsEcidInfoAdapter()
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
                dlg.Title = "Select mdb file : Setup";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
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

        private DataSetSetupItem.SetupHsmsEcidInfoRow Convert2Row(TagHsmsSetupInfo item)
        {
            DataSetSetupItem.SetupHsmsEcidInfoRow row = m_Table.NewSetupHsmsEcidInfoRow();
            row.Id = item.ID;
            row.Name = item.Name;
            row.OptionType = item.Type.ToString();
            row.OptionFormat = item.Format.ToString();
            row.UnitType = item.Unit.Unit.ToString();
            row.ItemValue = item.Val;
            row.LoTerm = item.LoTerm;
            row.HiTerm = item.HiTerm;
            row.Mode = item.Mode.ToString();
            return row;
        }

        private TagHsmsSetupInfo Convert2Tag(DataSetSetupItem.SetupHsmsEcidInfoRow row)
        {
            TagHsmsSetupInfo item = new TagHsmsSetupInfo();
            item.ID = row.Id;
            item.Name = row.Name;
            item.Type = (OptionType)(Enum.Parse(typeof(OptionType), row.OptionType, true));
            item.Format = (OptionFormat)(Enum.Parse(typeof(OptionFormat), row.OptionFormat, true));
            item.Unit = new TagUnit((UnitType)Enum.Parse(typeof(UnitType), row.UnitType, true));
            item.Val = row.ItemValue;
            item.LoTerm = row.LoTerm;
            item.HiTerm = row.HiTerm;
            item.Mode = (GridViewMode)(Enum.Parse(typeof(GridViewMode), row.Mode, true));
            return item;
        }

        private void UpdateDataFormat(DataSetSetupItem.SetupHsmsEcidInfoRow row, TagHsmsSetupInfo item)
        {
            row.BeginEdit();
            row.OptionType = item.Type.ToString();
            row.OptionFormat = item.Format.ToString();
            row.UnitType = item.Unit.Unit.ToString();
            row.LoTerm = item.LoTerm;
            row.HiTerm = item.HiTerm;
            row.EndEdit();
        }

        public bool IsChanged(DataSetSetupItem.SetupHsmsEcidInfoRow row1, DataSetSetupItem.SetupHsmsEcidInfoRow row2, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, row2.ItemArray, ignoreIndex);
        }

        public bool IsChanged(DataSetSetupItem.SetupHsmsEcidInfoRow row1, TagHsmsSetupInfo item, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, Convert2Row(item).ItemArray, ignoreIndex);
        }

        public bool IsChanged()
        {
            bool changed = false;
            foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
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
            foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
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

            m_Table = new DataSetSetupItem.SetupHsmsEcidInfoDataTable();
            m_Adapter = new SetupHsmsEcidInfoTableAdapter();
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

        public bool LoadFromDB(SetupHsmsEcidInfoList list)
        {
            lock (m_LockKey)
            {
                if (true == LoadFromDB())
                {
                    list.Items.Clear();

                    foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
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


        public void SaveToDB(SetupHsmsEcidInfoList list)
        {
            lock (m_LockKey)
            {
                m_Table.Clear();

                foreach (TagHsmsSetupInfo item in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(item));
                }

                UpdateToDB(m_Table);
            }
        }

        public void Save(List<string> list, List<string> value)
        {
            int count = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                int id = Convert.ToInt32(list[i]);
                foreach (DataRow row in Table.Rows)
                {
                    if (row["ID"].ToString() == id.ToString())
                    {
                        row["ItemValue"] = value[i];
                    }
                }
            }
        }

        public bool InitFromDB(TagHsmsSetupInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                bool find = false;
                int[] ignore = { m_Table.ItemValueColumn.Ordinal };

                foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
                {
                    if (string.Equals(item.Name, row.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        // format이 변경되었는가?
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


        public bool UpdateFromDB(string name, TagHsmsSetupInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
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

        public void UpdateFromDB(SetupHsmsEcidInfoList list)
        {
            //LoadFromDB(); // 10.08.03 minhan

            foreach (TagHsmsSetupInfo item in list.Items)
            {
                UpdateFromDB(item.Name, item);
            }
        }


        public bool UpdateToDB(TagHsmsSetupInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
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


        public void UpdateToDB(DataSetSetupItem.SetupHsmsEcidInfoDataTable table)
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
        public void Add(TagHsmsSetupInfo item)
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
        public void Remove(TagHsmsSetupInfo item)
        {
            lock (m_LockKey)
            {
                foreach (DataSetSetupItem.SetupHsmsEcidInfoRow row in m_Table.Rows)
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

        public void DeleteGarbage(SetupHsmsEcidInfoList list)
        {
            //2009.05.14 eun
            //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
            //그래서 아래와 같이 역순으로 처리한다.
            int count = m_Table.Rows.Count - 1;
            for (int i = count; i > -1; i--)
            {
                DataSetSetupItem.SetupHsmsEcidInfoRow row = (DataSetSetupItem.SetupHsmsEcidInfoRow)m_Table.Rows[i];

                bool match = false;
                foreach (TagHsmsSetupInfo item in list.Items)
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
            //foreach (DataSetSetupItem.SetupHsmsEqpInfoRow row in m_Table.Rows)
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
