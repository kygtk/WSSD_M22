using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave_Servo : EcSlave
    {
        #region Fields
        protected int m_ServoCount;
        protected List<EcSlaveItem_Servo> m_Servos = new List<EcSlaveItem_Servo>();

        protected int m_AxisIndex;
        #endregion

        #region Properties
        [Category("Slave Info"), ReadOnly(true), DisplayName("Servo Count")]
        public int ServoCount
        {
            get { return m_ServoCount; }
            set { m_ServoCount = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Servos")]
        public List<EcSlaveItem_Servo> Servos
        {
            get { return m_Servos; }
            set { m_Servos = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Axis Index")]
        public int AxisIndex
        {
            get { return m_AxisIndex; }
            set { m_AxisIndex = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_Servo()
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.m_Servos.Clear();

            for (int i = 0; i < m_ServoCount; i++)
            {
                EcSlaveItem_Servo item = new EcSlaveItem_Servo();
                item.Name = "servo__";
                item.Channel = i;
                m_Servos.Add(item);
            }
        }

        protected override void UpdateNumbering()
        {
            for (int i = 0; i < m_Servos.Count; i++)
            {
                EcSlaveItem_Servo item = m_Servos[i];
                item.SlaveNo = m_SlaveNo;
                item.AliasNo = m_AliasNo;
                item.AxisIndex = m_AxisIndex;
            }
        }
        #endregion
    }
}
