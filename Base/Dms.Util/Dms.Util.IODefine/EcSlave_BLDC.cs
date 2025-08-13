using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_BLDC : EcSlave_IoType
    {
        #region Fields
        protected int m_BLDCCount;
        protected List<EcSlaveItem_BLDC> m_BLDCs = new List<EcSlaveItem_BLDC>();
        #endregion

        #region Properties
        [Category("Slave Info"), ReadOnly(true), DisplayName("BLDC Count")]
        public int BLDCCount
        {
            get { return m_BLDCCount; }
            set { m_BLDCCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("BLDCs")]
        public List<EcSlaveItem_BLDC> BLDCs
        {
            get { return m_BLDCs; }
            set { m_BLDCs = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_BLDC()
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.m_BLDCs.Clear();

            for (int i = 0; i < m_BLDCCount; i++)
            {
                EcSlaveItem_BLDC item = new EcSlaveItem_BLDC();
                item.Name = "bldc__";
                item.Channel = i;
                m_BLDCs.Add(item);
            }
        }

        protected override void UpdateNumbering()
        {
            for (int i = 0; i < m_BLDCs.Count; i++)
            {
                EcSlaveItem_BLDC item = m_BLDCs[i];
                item.SlaveNo = m_SlaveNo;
                item.AliasNo = m_AliasNo;
            }
        }
        #endregion
    }
}
