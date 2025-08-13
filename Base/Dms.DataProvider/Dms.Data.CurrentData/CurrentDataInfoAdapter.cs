using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data.DataSetCurrentDataTableAdapters;
using Dms.Data;
using System.Data;
using System.Windows.Forms;
using Dms.Cim.Common;
using Dms.Common;

namespace Dms.Data
{
    public class CurrentDataInfoAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetCurrentData.CurrentDataDataTable m_Table;
        private CurrentDataTableAdapter m_Adapter;
        #endregion

        #region Property
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public CurrentDataInfoAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsCurrentDataConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.CurrentDataDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsCurrentDataConnectionString =
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

        #region Destructor
        #endregion

        #region Method
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
                MessageBox.Show("Current Data database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Current Data";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsCurrentData.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.CurrentDataDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DataSetCurrentData.CurrentDataRow Convert2Row(TagCurrentDataInfo item)
        {
            DataSetCurrentData.CurrentDataRow row = m_Table.NewCurrentDataRow();

            row.UNIT_NO = item.UnitNo;
            row.ID = item.Id;
            row.NAME = item.Name;
            row.DVNAME = item.DvName;
            row.TYPE = item.Type;
            row.FORMAT = item.Format;
            row.WORD_SIZE = item.WordSize;
            row.POINT = item.Point;
            row.UNIT = item.Unit;
            row.ADDRESS = item.Address;

            return row;
        }

        private TagCurrentDataInfo Convert2Tag(DataSetCurrentData.CurrentDataRow row)
        {
            TagCurrentDataInfo tagcurrentinfo = new TagCurrentDataInfo();

            tagcurrentinfo.UnitNo = row.UNIT_NO;
            tagcurrentinfo.Id = row.ID;
            tagcurrentinfo.Name = row.NAME;
            tagcurrentinfo.DvName = row.DVNAME;
            tagcurrentinfo.Type = row.TYPE;
            tagcurrentinfo.Format = row.FORMAT;
            tagcurrentinfo.WordSize = row.WORD_SIZE;
            tagcurrentinfo.Point = row.POINT;
            tagcurrentinfo.Unit = row.UNIT;
            tagcurrentinfo.Address = row.ADDRESS;

            return tagcurrentinfo;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetCurrentData.CurrentDataDataTable();
            m_Adapter = new CurrentDataTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        public void ClearDB()
        {
            lock(m_LockKey)
            {
                int count = m_Table.Count;

                if (count > 0)
                {
                    foreach (DataRow row in m_Table.Rows)
                    {
                        row.Delete();
                    }

                    UpdateToDB(m_Table);
                }
            }
        }

        public void LoadFormDB()
        {
            lock(m_LockKey)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void LoadFormDB(CurrentDataInfo list)
        {
            lock(m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach (DataSetCurrentData.CurrentDataRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetCurrentData.CurrentDataDataTable table)
        {
            lock(m_LockKey)
            {
                try
                {
                    int rv = m_Adapter.Update(table);

                    if( rv > 0 )
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

        public void SaveToDB(CurrentDataInfo list)
        {
            lock(m_LockKey)
            {
                ClearDB();

                foreach (TagCurrentDataInfo apd in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(apd));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
