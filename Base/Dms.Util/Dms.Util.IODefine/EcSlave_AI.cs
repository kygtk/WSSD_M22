using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_AI : EcSlave_IoType
    {
        #region Fields
        protected int m_InChannelCount;
        protected List<EcSlaveItem_AI> m_InChannels = new List<EcSlaveItem_AI>();
        #endregion

        #region Properties
        [Category("Slave Info"), ReadOnly(true), DisplayName("In Channel Count")]
        public int InChannelCount
        {
            get { return m_InChannelCount; }
            set { m_InChannelCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("In Channel")]
        public List<EcSlaveItem_AI> InChannels
        {
            get { return m_InChannels; }
            set { m_InChannels = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_AI() { }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.m_InChannels.Clear();

            for (int i = 0; i < m_InChannelCount; i++)
            {
                EcSlaveItem_AI item = new EcSlaveItem_AI();
                item.Name = "ai__";
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
