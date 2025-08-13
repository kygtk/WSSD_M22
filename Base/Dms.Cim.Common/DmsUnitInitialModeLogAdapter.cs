using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Dms.Cim.Common
{
    public class DmsUnitInitialModeLogAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DmsUnitInitialModeLogAdapter()
        {
        }
        #endregion
        
        #region Methods
        public void SetLog(string unitname, DataTable table)
        {
            lock(m_LockKey)
            {
                int rowCounts = table.Rows.Count;

                string message = "";

                m_Log.TextOut("--------------------------------------------------------------------------------");

                message = string.Format("{0}{1}", "Unit : ", unitname);
                m_Log.TextOut(message);
                message = string.Format("State - {0}", "Modify");
                m_Log.TextOut(message);

                for (int i = 0; i < rowCounts; i++)
                {
                    DataRow row = table.Rows[i];

                    int rowItemCounts = row.ItemArray.Length;

                    StringBuilder sb = new StringBuilder();

                    for (int itemIndex = 0; itemIndex < rowItemCounts; itemIndex++)
                    {
                        sb.Append(row.ItemArray[itemIndex].ToString());

                        if( (rowItemCounts - 1) > itemIndex ) sb.Append(" : ");
                    }

                    m_Log.TextOut(sb.ToString());
                }

                m_Log.TextOut("--------------------------------------------------------------------------------");
            }
        }
        #endregion
    }
}
