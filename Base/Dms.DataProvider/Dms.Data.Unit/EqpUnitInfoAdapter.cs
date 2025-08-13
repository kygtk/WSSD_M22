using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dms.Data.DataSetUnitInfoTableAdapters;
using System.Windows.Forms;

namespace Dms.Data
{
    public class CimEqpUnitInfoAdapter
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitInfo.EqpUnitInfoDataTable m_Table;
        private EqpUnitInfoTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public CimEqpUnitInfoAdapter()
        {
            this.InitAdapter();
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        private DataSetUnitInfo.EqpUnitInfoRow Convert2Row(TagEqpUnitInfo item)
        {
            DataSetUnitInfo.EqpUnitInfoRow row = m_Table.NewEqpUnitInfoRow();

            row.ID = item.Id;
            row.SUBUNITNO = item.SubUnitNo;
            row.UNITNAME = item.UnitName;
            row.DESC = item.Desc;
            row.ADDRESS = item.Address;

            return row;
        }

        private TagEqpUnitInfo Convert2Tag(DataSetUnitInfo.EqpUnitInfoRow row)
        {
            TagEqpUnitInfo info = new TagEqpUnitInfo();

            info.Id = row.ID;
            info.SubUnitNo = row.SUBUNITNO;
            info.UnitName = row.UNITNAME;
            info.Desc = row.DESC;
            info.Address = row.ADDRESS;

            return info;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUnitInfo.EqpUnitInfoDataTable();
            m_Adapter = new EqpUnitInfoTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        public void ClearDB()
        {
            lock (m_LockKey)
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
            lock (m_LockKey)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void LoadFormDB(EqpUnitInfo list)
        {
            lock (m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach (DataSetUnitInfo.EqpUnitInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitInfo.EqpUnitInfoDataTable table)
        {
            lock (m_LockKey)
            {
                try
                {
                    int rv = m_Adapter.Update(table);

                    if (rv > 0)
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

        public void SaveToDB(EqpUnitInfo list)
        {
            lock (m_LockKey)
            {
                ClearDB();

                foreach (TagEqpUnitInfo info in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(info));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }
}
