using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data.DataSetUnitInitialDataInfoTableAdapters;
using System.Data;
using System.Windows.Forms;

namespace Dms.Data
{
    public class UnitInitialDataInfoAdapter
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitInitialDataInfo.UnitInitialDataInfoDataTable m_Table;
        private UnitInitialDataInfoTableAdapter m_Adapter;
        #endregion

        #region Property
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public UnitInitialDataInfoAdapter()
        {
            this.InitAdapter();
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        private DataSetUnitInitialDataInfo.UnitInitialDataInfoRow Convert2Row(TagUnitInitialDataInfo item)
        {
            DataSetUnitInitialDataInfo.UnitInitialDataInfoRow row = m_Table.NewUnitInitialDataInfoRow();

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
            row.CHECK_ADDRESS = item.CheckAddress;
            
            return row;
        }

        private TagUnitInitialDataInfo Convert2Tag(DataSetUnitInitialDataInfo.UnitInitialDataInfoRow row)
        {
            TagUnitInitialDataInfo info = new TagUnitInitialDataInfo();

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
            info.CheckAddress = row.CHECK_ADDRESS;

            return info;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUnitInitialDataInfo.UnitInitialDataInfoDataTable();
            m_Adapter = new UnitInitialDataInfoTableAdapter();
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

        public void LoadFormDB(UnitInitialDataInfo list)
        {
            lock(m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach ( DataSetUnitInitialDataInfo.UnitInitialDataInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitInitialDataInfo.UnitInitialDataInfoDataTable table)
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

        public void SaveToDB(UnitInitialDataInfo list)
        {
            lock(m_LockKey)
            {
                ClearDB();

                foreach (TagUnitInitialDataInfo info in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(info));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
