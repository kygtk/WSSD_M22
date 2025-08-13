using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data.DataSetUnitRecipeTypeInfoTableAdapters;
using System.Data;
using System.Windows.Forms;
using Dms.Cim.Common;
using Dms.Common;

namespace Dms.Data
{
    public class UnitRecipeTypeInfoAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoDataTable m_Table;
        private UnitRecipeTypeInfoTableAdapter m_Adapter;
        #endregion

        #region Property
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public UnitRecipeTypeInfoAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsUnitRecipeTypeInfoConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.UnitRecipeTypeInfoDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsUnitRecipeTypeInfoConnectionString =
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
                MessageBox.Show("Unit Recipe Type Info database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Unit Recipe Type Info Alarm";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsUnitRecipeTypeInfo.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.UnitRecipeTypeInfoDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoRow Convert2Row(TagUnitRecipeTypeInfo item)
        {
            DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoRow row = m_Table.NewUnitRecipeTypeInfoRow();

            row.UnitNo = item.UnitNo;
            row.Type = item.Type;
            row.ColumnNo = item.ColNo;
            row.RowNo = item.RowNo;
            row.Title = item.Title;
            row.ValueIndex = item.ValueIndex;
            
            return row;
        }

        private TagUnitRecipeTypeInfo Convert2Tag(DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoRow row)
        {
            TagUnitRecipeTypeInfo info = new TagUnitRecipeTypeInfo();

            info.UnitNo = row.UnitNo;
            info.Type = row.Type;
            info.ColNo = row.ColumnNo;
            info.RowNo = row.RowNo;
            info.Title = row.Title;
            info.ValueIndex = row.ValueIndex;

            return info;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoDataTable();
            m_Adapter = new UnitRecipeTypeInfoTableAdapter();
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

        public void LoadFormDB(UnitRecipeTypeInfo list)
        {
            lock(m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach ( DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitRecipeTypeInfo.UnitRecipeTypeInfoDataTable table)
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

        public void SaveToDB(UnitRecipeTypeInfo list)
        {
            lock(m_LockKey)
            {
                ClearDB();

                foreach (TagUnitRecipeTypeInfo info in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(info));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion

    }
}
