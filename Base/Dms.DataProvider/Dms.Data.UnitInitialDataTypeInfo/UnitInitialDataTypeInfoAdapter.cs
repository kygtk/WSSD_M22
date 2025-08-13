using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data.DataSetUnitInitialDataTypeInfoTableAdapters;
using System.Data;
using System.Windows.Forms;

namespace Dms.Data
{
    public class UnitInitialDataTypeInfoAdapter
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoDataTable m_Table;
        private InitialDataTypeInfoTableAdapter m_Adapter;
        #endregion

        #region Property
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public UnitInitialDataTypeInfoAdapter()
        {
            this.InitAdapter();
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        private DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoRow Convert2Row(TagUnitInitialDataTypeInfo item)
        {
            DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoRow row = m_Table.NewInitialDataTypeInfoRow();

            row.UnitNo = item.UnitNo;
            row.Type = item.Type;
            row.ColumnNo = item.ColNo;
            row.RowNo = item.RowNo;
            row.Title = item.Title;
            row.ValueIndex = item.ValueIndex;
            
            return row;
        }

        private TagUnitInitialDataTypeInfo Convert2Tag(DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoRow row)
        {
            TagUnitInitialDataTypeInfo info = new TagUnitInitialDataTypeInfo();

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
            m_Table = new DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoDataTable();
            m_Adapter = new InitialDataTypeInfoTableAdapter();
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

        public void LoadFormDB(UnitInitialDataTypeInfo list)
        {
            lock(m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach ( DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitInitialDataTypeInfo.InitialDataTypeInfoDataTable table)
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

        public void SaveToDB(UnitInitialDataTypeInfo list)
        {
            lock(m_LockKey)
            {
                ClearDB();

                foreach (TagUnitInitialDataTypeInfo info in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(info));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
