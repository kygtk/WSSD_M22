using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using Dms.Data;
using Dms.Data.DataSetProcessDataTableAdapters;
using Dms.Common;
using Dms.Cim.Common;

namespace Dms.Data
{
    public class GlassApdInfoAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private DataSetProcessData.GlassApdDataTable m_Table;
        private GlassApdTableAdapter m_Adapter;
        #endregion

        #region Property
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public GlassApdInfoAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsProcessDataConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.ProcessDataDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsProcessDataConnectionString =
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
                MessageBox.Show("Process Data database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Process Data";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsProcessData.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.ProcessDataDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private DataSetProcessData.GlassApdRow Convert2Row(TagGlassApdInfo item)
        {
            DataSetProcessData.GlassApdRow row = m_Table.NewGlassApdRow();

            row.UNITNO = item.UnitNo;
            row.APDUNITNAME = item.ApdUnitName;
            row.ID = item.Id;
            row.NAME = item.Name;
            row.DVNAME = item.DvName;
            row.TYPE = item.Type;
            row.FORMAT = item.Format;
            row.WORDSIZE = item.WordSize;
            row.POINT = item.Point;
            row.ADDRESS = item.Address;
            row.UNIT = item.Unit;
            row.USEDATA = item.UseData;
            row.HOSTREPORT = item.HostReport;
//            row.LOTAPD = item.LotApd;

            return row;
        }

        private TagGlassApdInfo Convert2Tag(DataSetProcessData.GlassApdRow row)
        {
            TagGlassApdInfo info = new TagGlassApdInfo();

            info.UnitNo = row.UNITNO;
            info.ApdUnitName = row.APDUNITNAME;
            info.Id = row.ID;
            info.Name = row.NAME;
            info.DvName = row.DVNAME;
            info.Type = row.TYPE;
            info.Format = row.FORMAT;
            info.WordSize = row.WORDSIZE;
            info.Point = row.POINT;
            info.Address = row.ADDRESS;
            info.Unit = row.UNIT;
            info.UseData = row.USEDATA;
            info.HostReport = row.HOSTREPORT;
//            info.LotApd = row.LOTAPD;

            return info;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetProcessData.GlassApdDataTable();
            m_Adapter = new GlassApdTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        public void ClearDB()
        {
            lock (this)
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
            lock (this)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void LoadFormDB(GlassApdInfo list)
        {
            lock (this)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach (DataSetProcessData.GlassApdRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetProcessData.GlassApdDataTable table)
        {
            lock (this)
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

        public void SaveToDB(GlassApdInfo list)
        {
            lock (this)
            {
                ClearDB();

                foreach (TagGlassApdInfo apd in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(apd));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
