using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Dms.Common
{
    abstract public class _DmsDataAdaptor
    {
        #region Fields
        private XLog m_Log;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public _DmsDataAdaptor()
        {
            m_Log = new XLog(this.GetType().Name, XLog.LogStampType.UseStamp);
        }
        #endregion
        
        #region Methods
        public void WriteDataChangeLog(DataTable table)
        {
            DataTable changes = table.GetChanges();
            if (changes != null)
            {
                string message = "";
                foreach (DataRow row in changes.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        int count = row.ItemArray.Length;
                        for (int i = 0; i < count; i++)
                        {
                            message += table.Columns[i].ColumnName + " : " + row.ItemArray[i].ToString();
                            if (i < (row.ItemArray.Length - 1))
                            {
                                message += " , ";
                            }
                        }

                        m_Log.TextOut(message);
                    }
                }
            }
        } 
        #endregion
    }
}
