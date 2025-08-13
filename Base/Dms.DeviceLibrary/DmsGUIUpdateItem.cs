using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.DeviceLibrary
{
    public class DmsGUIUpdateItem
    {
        #region Fields
        private string m_Name;
        private string m_Status = "garbage";
        #endregion

        #region Constructor
        public DmsGUIUpdateItem(string name, string status)
        {
            m_Name = name;
            m_Status = status;
        }
        #endregion

        #region Methods
        public string GetStatus()
        {
            return m_Status;
        }
        public string GetName()
        {
            return m_Name;
        }

        public void SetStatus(string s)
        {
            m_Status = s;
        }
        #endregion
    }
}
