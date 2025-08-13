using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Cim.Common
{
    public class PortInfo
    {
        #region Fields
        private string m_TrsMode;
        private string m_Enable;
        private string m_CstId;
        private string m_LotId;
        private string m_PortStatus;
        private string m_RecipeId;
        private string m_LotStatus;
        private string m_SlotInfo;
        private string m_PortType;
        private int m_InNum;
        private int m_OutNum;
        private int m_TotalNum;
        #endregion

        #region Property
        public string TrsMode
        {
            get { return m_TrsMode; }
            set { m_TrsMode = value; }
        }

        public string Enable
        {
            get { return m_Enable; }
            set { m_Enable = value; }
        }

        public string CstId
        {
            get { return m_CstId; }
            set { m_CstId = value; }
        }

        public string LotId
        {
            get { return m_LotId; }
            set { m_LotId = value; }
        }

        public string PortStatus
        {
            get { return m_PortStatus; }
            set { m_PortStatus = value; }
        }

        public string RecipeId
        {
            get { return m_RecipeId; }
            set { m_RecipeId = value; }
        }

        public string LotStatus
        {
            get { return m_LotStatus; }
            set { m_LotStatus = value; }
        }

        public string SlotInfo
        {
            get { return m_SlotInfo; }
            set { m_SlotInfo = value; }
        }

        public string PortType
        {
            get { return m_PortType; }
            set { m_PortType = value; }
        }

        public int InNum
        {
            get { return m_InNum; }
            set { m_InNum = value; }
        }

        public int OutNum
        {
            get { return m_OutNum; }
            set { m_OutNum = value; }
        }

        public int TotalNum
        {
            get { return m_TotalNum; }
            set { m_TotalNum = value; }
        }
        #endregion

        public PortInfo()
        {
            m_TrsMode = "";
            m_Enable = "";
            m_CstId = "";
            m_LotId = "";
            m_PortStatus = "";
            m_RecipeId = "";
            m_LotStatus = "";
            m_SlotInfo = "";
            m_PortType = "";
            m_InNum = 0;
            m_OutNum = 0;
            m_TotalNum = 0;
        }
    }
}
