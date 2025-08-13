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
    public class CalibrationListAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private bool m_Initialized = false;
        private DataSetSetupItem.CalibrationDataTable m_Table;
        private CalibrationTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public CalibrationListAdapter()
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

        public bool InitAdapter()
        {
            if (m_Initialized) return true;

            m_Table = new DataSetSetupItem.CalibrationDataTable();
            m_Adapter = new CalibrationTableAdapter();
            m_Adapter.Fill(m_Table);

            m_Initialized = true;

            return true;
        }

        private DataSetSetupItem.CalibrationRow Convert2Row(TagCalibrationInfo item)
        {
            DataSetSetupItem.CalibrationRow row = m_Table.NewCalibrationRow();
            row.Name = item.Name;
            row.Type = item.Type.ToString();
            row.Scale = item.Scale.ToString();
            row.Unit = item.Unit.Unit.ToString();
            row.Gain = item.FilterGain;
            row.AdcMax = item.AdcMax;
            row.AdcMin = item.AdcMin;
            row.RealMax = item.RealMax;
            row.RealMin = item.RealMin;
            return row;
        }


        private TagCalibrationInfo Convert2Tag(DataSetSetupItem.CalibrationRow row)
        {
            TagCalibrationInfo item = new TagCalibrationInfo();
            item.Name = row.Name;
            item.Type = (GaugeType)Enum.Parse(typeof(GaugeType), row.Type, true);
            item.Scale = (ScaleType)Enum.Parse(typeof(ScaleType), row.Scale, true);
            item.Unit = new TagUnit((UnitType)Enum.Parse(typeof(UnitType), row.Unit, true));
            item.FilterGain = row.Gain;
            item.AdcMax = (short)row.AdcMax;
            item.AdcMin = (short)row.AdcMin;
            item.RealMax = row.RealMax;
            item.RealMin = row.RealMin;
            return item;
        }

        private void UpdateDataFormat(DataSetSetupItem.CalibrationRow row, TagCalibrationInfo item)
        {
            row.BeginEdit();
            row.Type = item.Type.ToString();
            row.Unit = item.Unit.Unit.ToString();
            row.EndEdit();
        }

        public bool IsChanged(DataSetSetupItem.CalibrationRow row1, DataSetSetupItem.CalibrationRow row2, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, row2.ItemArray, ignoreIndex);
        }

        public bool IsChanged(DataSetSetupItem.CalibrationRow row1, TagCalibrationInfo item, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, Convert2Row(item).ItemArray, ignoreIndex);
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

        public void SaveToDB()
        {
            UpdateToDB(m_Table);
        }


        public void SaveToDB(CalibrationList list)
        {
            lock (m_LockKey)
            {
                m_Table.Clear();

                foreach (TagCalibrationInfo item in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(item));
                }

                UpdateToDB(m_Table);
            }
        }


        public bool InitFromDB(TagCalibrationInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                bool find = false;
                int[] ignore = { m_Table.NameColumn.Ordinal,
                                m_Table.ScaleColumn.Ordinal,
                                m_Table.GainColumn.Ordinal,
                                m_Table.AdcMaxColumn.Ordinal,
                                m_Table.AdcMinColumn.Ordinal,
                                m_Table.RealMaxColumn.Ordinal,
                                m_Table.RealMinColumn.Ordinal };

                foreach (DataSetSetupItem.CalibrationRow row in m_Table.Rows)
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


        public bool UpdateFromDB(string name, TagCalibrationInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.CalibrationRow row in m_Table.Rows)
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

        public void UpdateFromDB(CalibrationList list)
        {
            //LoadFromDB(); // 10.08.03 minhan

            foreach (TagCalibrationInfo item in list.Items)
            {
                UpdateFromDB(item.Name, item);
            }
        }


        public bool UpdateToDB(TagCalibrationInfo item)
        {
            lock (m_LockKey)
            {
                if (!m_Initialized)
                {
                    return false;
                }

                foreach (DataSetSetupItem.CalibrationRow row in m_Table.Rows)
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


        public void UpdateToDB(DataSetSetupItem.CalibrationDataTable table)
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
        public void Add(TagCalibrationInfo item)
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
        public void Remove(TagCalibrationInfo item)
        {
            lock (m_LockKey)
            {

                foreach (DataSetSetupItem.CalibrationRow row in m_Table.Rows)
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

        public void DeleteGarbage(CalibrationList list)
        {
            //2009.05.14 eun
            //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
            //그래서 아래와 같이 역순으로 처리한다.
            int count = m_Table.Rows.Count - 1;
            for (int i = count; i > -1; i--)
            {
                DataSetSetupItem.CalibrationRow row = (DataSetSetupItem.CalibrationRow)m_Table.Rows[i];

                bool match = false;
                foreach (TagCalibrationInfo item in list.Items)
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
            //foreach (DataSetSetupItem.CalibrationRow row in m_Table.Rows)
            //{
            //    bool match = false;
            //    foreach (TagCalibrationInfo item in list.Items)
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
