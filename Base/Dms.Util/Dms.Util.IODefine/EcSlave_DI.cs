using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_DI : EcSlave_IoType
    {
        #region Fields
        protected int m_InChannelCount;
        protected List<EcSlaveItem_DI> m_InChannels = new List<EcSlaveItem_DI>();
        #endregion

        #region Properties
        [Category("Slave Info"), ReadOnly(true), DisplayName("In Channel Count")]
        public int InChannelCount
        {
            get { return m_InChannelCount; }
            set { m_InChannelCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("In Channel")]
        public List<EcSlaveItem_DI> InChannels
        {
            get { return m_InChannels; }
            set { m_InChannels = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_DI()
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.m_InChannels.Clear();

            for (int i = 0; i < m_InChannelCount; i++)
            {
                EcSlaveItem_DI item = new EcSlaveItem_DI();
                item.Name = "di__";
                item.Channel = i;
                m_InChannels.Add(item);
            }
        }

        protected override void UpdateNumbering()
        {
            for (int i = 0; i < m_InChannels.Count; i++)
            {
                EcSlaveItem item = m_InChannels[i];
                item.SlaveNo = m_SlaveNo;
                item.AliasNo = m_AliasNo;
            }
        }
        #endregion
    }
}
