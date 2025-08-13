using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class EqpGroupInfo
    {
        #region Fields
        private int m_EqpGroupNo;
        private eqpSTATUS m_EqpGroupStatus;
        private List<EqpInfo> m_GroupInfo = new List<EqpInfo>();
        #endregion

        #region Properties
        public int EqpGroupNo
        {
            get { return m_EqpGroupNo; }
            set { m_EqpGroupNo = value; }
        }
        public eqpSTATUS EqpGroupStatus
        {
            get { return m_EqpGroupStatus; }
            set { m_EqpGroupStatus = value; }
        }
        public List<EqpInfo> GroupInfo
        {
            get { return m_GroupInfo; }
            set { m_GroupInfo = value; }
        }
        #endregion

        public EqpGroupInfo()
        {
            m_EqpGroupStatus = eqpSTATUS.eqpIdle;
        }

        #region Method
        public void Add(EqpInfo info)
        {
            m_GroupInfo.Add(info);
        }

        #endregion

    }
}
