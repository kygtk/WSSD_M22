using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_DIO : EcSlave_IoType
    {
        #region Fields
        protected int m_InChannelCount;
        protected List<EcSlaveItem_DI> m_InChannels = new List<EcSlaveItem_DI>();

        protected int m_OutChannelCount;
        protected List<EcSlaveItem_DO> m_OutChannels = new List<EcSlaveItem_DO>();
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

        [Category("Slave Info"), ReadOnly(true), DisplayName("Out Channel Count")]
        public int OutChannelCount
        {
            get { return m_OutChannelCount; }
            set { m_OutChannelCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Out Channel")]
        public List<EcSlaveItem_DO> OutChannels
        {
            get { return m_OutChannels; }
            set { m_OutChannels = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_DIO() { }
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

            this.m_OutChannels.Clear();

            for (int i = 0; i < m_OutChannelCount; i++)
            {
                EcSlaveItem_DO item = new EcSlaveItem_DO();
                item.Name = "do__";
                item.Channel = i;
                m_OutChannels.Add(item);
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

            for (int i = 0; i < m_OutChannels.Count; i++)
            {
                EcSlaveItem item = m_OutChannels[i];
                item.SlaveNo = m_SlaveNo;
                item.AliasNo = m_AliasNo;
            }
        }
        #endregion
    }
}
