using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_AP : EcSlave_IoType
    {
        #region Fields
        protected int m_ChannelCount;
        protected List<EcSlaveItem_AP> m_Channels = new List<EcSlaveItem_AP>();
        #endregion

        #region Properties
        [Category("Slave Info"), ReadOnly(true), DisplayName("Channel Count")]
        public int ChannelCount
        {
            get { return m_ChannelCount; }
            set { m_ChannelCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Channel")]
        public List<EcSlaveItem_AP> Channels
        {
            get { return m_Channels; }
            set { m_Channels = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_AP() { }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.m_Channels.Clear();

            for (int i = 0; i < m_ChannelCount; i++)
            {
                EcSlaveItem_AP item = new EcSlaveItem_AP();
                item.Name = "ap__";
                item.Channel = i;
                m_Channels.Add(item);
            }
        }

        protected override void UpdateNumbering()
        {
            for (int i = 0; i < m_Channels.Count; i++)
            {
                EcSlaveItem item = m_Channels[i];
                item.SlaveNo = m_SlaveNo;
                item.AliasNo = m_AliasNo;
            }
        }
        #endregion
    }
}
