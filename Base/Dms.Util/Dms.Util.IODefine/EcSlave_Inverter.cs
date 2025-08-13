using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_Inverter : EcSlave
    {
        #region Fields
        protected int m_InverterCount;
        protected List<EcSlaveItem_Inverter> m_Inverters = new List<EcSlaveItem_Inverter>();
        #endregion

        #region Properties
        [Category("Slave Info"), ReadOnly(true), DisplayName("Inverter Count")]
        public int InverterCount
        {
            get { return m_InverterCount; }
            set { m_InverterCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Inverters")]
        public List<EcSlaveItem_Inverter> Inverters
        {
            get { return m_Inverters; }
            set { m_Inverters = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_Inverter()
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.m_Inverters.Clear();

            for (int i = 0; i < m_InverterCount; i++)
            {
                EcSlaveItem_Inverter item = new EcSlaveItem_Inverter();
                item.Name = "inverter__";
                item.Channel = i;
                m_Inverters.Add(item);
            }
        }

        protected override void UpdateNumbering()
        {
            for (int i = 0; i < m_Inverters.Count; i++)
            {
                EcSlaveItem_Inverter item = m_Inverters[i];
                item.SlaveNo = m_SlaveNo;
                item.AliasNo = m_AliasNo;
            }
        }
        #endregion
    }
}
