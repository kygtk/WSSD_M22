using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class AlarmHistory
    {
        #region Fields
        private List<AlarmHistory> m_Items = new List<AlarmHistory>();
        #endregion

        #region Properties
        public List<AlarmHistory> Items
        {
            get { return m_Items; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion    

        #region Constructor
        public AlarmHistory()
        {
        }
        #endregion

        #region Methods

        #endregion
    }
}
