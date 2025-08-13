using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data.DataSetUnitRecipeInfoTableAdapters;
using System.Data;
using System.Windows.Forms;
using Dms.Cim.Common;
using Dms.Common;

namespace Dms.Data
{
    public class UnitRecipeInfoAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitRecipeInfo.UnitRecipeBodyInfoDataTable m_Table;
        private UnitRecipeBodyInfoTableAdapter m_Adapter;
        #endregion

        #region Property
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public UnitRecipeInfoAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsUnitRecipeInfoConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.UnitRecipeInfoDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsUnitRecipeInfoConnectionString =
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
                MessageBox.Show("Unit Recipe Info database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Unit Recipe Info";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsUnitRecipeInfo.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.UnitRecipeInfoDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DataSetUnitRecipeInfo.UnitRecipeBodyInfoRow Convert2Row(TagUnitRecipeInfo item)
        {
            DataSetUnitRecipeInfo.UnitRecipeBodyInfoRow row = m_Table.NewUnitRecipeBodyInfoRow();

            row.UNIT_NO = item.UnitNo;
            row.ID = item.Id;
            row.NAME = item.Name;
            row.DVNAME = item.DvName;
            row.TYPE = item.Type;
            row.FORMAT = item.Format;
            row.LOW_VALUE = item.LowValue;
            row.HIGH_VALUE = item.HighValue;
            row.WORD_SIZE = item.WordSize;
            row.POINT = item.Point;
            row.UNIT = item.Unit;
            row.ValueType = item.ValueType;
            row.ADDRESS = item.Address;
            row.CHECKADDRESS = item.CheckAddress;

            return row;
        }

        private TagUnitRecipeInfo Convert2Tag(DataSetUnitRecipeInfo.UnitRecipeBodyInfoRow row)
        {
            TagUnitRecipeInfo info = new TagUnitRecipeInfo();

            info.UnitNo = row.UNIT_NO;
            info.Id = row.ID;
            info.Name = row.NAME;
            info.DvName = row.DVNAME;
            info.Type = row.TYPE;
            info.Format = row.FORMAT;
            info.LowValue = row.LOW_VALUE;
            info.HighValue = row.HIGH_VALUE;
            info.WordSize = row.WORD_SIZE;
            info.Point = row.POINT;
            info.Unit = row.UNIT;
            info.ValueType = row.ValueType;
            info.Address = row.ADDRESS;
            info.CheckAddress = row.CHECKADDRESS;

            return info;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUnitRecipeInfo.UnitRecipeBodyInfoDataTable();
            m_Adapter = new UnitRecipeBodyInfoTableAdapter();
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

        public void LoadFormDB(UnitRecipeInfo list)
        {
            lock(m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach (DataSetUnitRecipeInfo.UnitRecipeBodyInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitRecipeInfo.UnitRecipeBodyInfoDataTable table)
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

        public void SaveToDB(UnitRecipeInfo list)
        {
            lock(m_LockKey)
            {
                ClearDB();

                foreach (TagUnitRecipeInfo info in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(info));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
