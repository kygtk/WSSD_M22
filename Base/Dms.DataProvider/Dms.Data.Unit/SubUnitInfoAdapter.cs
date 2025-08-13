using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dms.Data.DataSetUnitInfoTableAdapters;
using System.Windows.Forms;
using Dms.Cim.Common;

namespace Dms.Data
{
    public class SubUnitInfoAdapter
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitInfo.SubUnitInfoDataTable m_Table;
        private SubUnitInfoTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public SubUnitInfoAdapter()
        {
            this.InitAdapter();
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        private DataSetUnitInfo.SubUnitInfoRow Convert2Row(TagSubUnitInfo item)
        {
            DataSetUnitInfo.SubUnitInfoRow row = m_Table.NewSubUnitInfoRow();

            row.ID = item.Id;
            row.SUBUNITNO = item.SubUnitNo;
            row.UNITNAME = item.UnitName;
            row.DESC = item.Desc;
            row.ADDRESS = item.Address;

            return row;
        }

        private TagSubUnitInfo Convert2Tag(DataSetUnitInfo.SubUnitInfoRow row)
        {
            TagSubUnitInfo info = new TagSubUnitInfo();

            info.Id = row.ID;
            info.SubUnitNo = row.SUBUNITNO;
            info.UnitName = row.UNITNAME;
            info.Desc = row.DESC;
            info.Address = row.ADDRESS;

            return info;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUnitInfo.SubUnitInfoDataTable();
            m_Adapter = new SubUnitInfoTableAdapter();
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

        public void LoadFormDB(SubUnitInfo list)
        {
            lock (m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach (DataSetUnitInfo.SubUnitInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitInfo.SubUnitInfoDataTable table)
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

        public void SaveToDB(SubUnitInfo list)
        {
            lock (m_LockKey)
            {
                ClearDB();

                foreach (TagSubUnitInfo info in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(info));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion

    }
}
