using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dms.Common;

namespace Dms.Cim.Common
{
    public class DmsUnitLogAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DmsUnitLogAdapter()
        {
        }
        #endregion
        
        #region Methods
        public void SetLog(string unitname, int id)
        {
            lock(m_LockKey)
            {
                m_Log.TextOut("--------------------------------------------------------------------------------");
                string message = string.Format("{0}{1}", "Unit : ",unitname);
                m_Log.TextOut(message);
                m_Log.TextOut("State : Delete");
                m_Log.TextOut("ID : " + id.ToString().PadLeft(2, '0'));
                m_Log.TextOut("--------------------------------------------------------------------------------");
            }
        }

        public void SetLog(string unitname, DataTable table)
        {
            lock(m_LockKey)
            {
                DataTable changes = table.GetChanges();

                if (changes != null)
                {
                    int rowCounts = changes.Rows.Count;

                    for (int i = 0; i < rowCounts; i++)
                    {
                        DataRow row = changes.Rows[i];
                        DataRowState rowState = row.RowState;

                        if (rowState == DataRowState.Added ||
                            rowState == DataRowState.Modified)
                        {
                            int rowItemCounts = row.ItemArray.Length;

                            string message = "";

                            m_Log.TextOut("--------------------------------------------------------------------------------");

                            message = string.Format("{0}{1}", "Unit : ", unitname);
                            m_Log.TextOut(message);
                            message = string.Format("State - {0}", rowState.ToString());
                            m_Log.TextOut(message);

                            for (int itemIndex = 0; itemIndex < rowItemCounts; itemIndex++)
                            {
                                message = string.Format("{0}{1}{2}", table.Columns[itemIndex].ColumnName, " : ", row.ItemArray[itemIndex].ToString());
                                m_Log.TextOut(message);
                            }

                            m_Log.TextOut("--------------------------------------------------------------------------------");
                        }
                    }
                }
            }
        }
        #endregion

    }
}
