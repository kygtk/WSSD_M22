using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class EcSlaveItem
    {
        #region Fields
        protected int m_Id = -1;
        protected int m_SlaveNo = -1;
        protected int m_AliasNo;
        protected int m_Channel = 0;
        protected string m_Name = "";
        protected string m_Description = "";
        protected string m_State = "";
        protected EcSlaveItemType m_SlaveItemType;
        #endregion

        #region Event
        #endregion

        #region Properties
        [Category("Address"), ReadOnly(true)]
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        [Category("Address"), ReadOnly(true), DisplayName("Slave No")]
        public int SlaveNo
        {
            get { return m_SlaveNo; }
            set { m_SlaveNo = value; }
        }

        [Category("Address"), ReadOnly(true), DisplayName("Alias No")]
        public int AliasNo
        {
            get { return m_AliasNo; }
            set { m_AliasNo = value; }
        }

        [Category("Address"), ReadOnly(true)]
        public int Channel
        {
            get { return m_Channel; }
            set { m_Channel = value; }
        }

        [Category("Slave Info")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [Category("Slave Info")]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        [Category("Slave Info"), ReadOnly(true), XmlIgnore()]
        public string State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Slave Item Type")]
        public EcSlaveItemType SlaveItemType
        {
            get { return m_SlaveItemType; }
            set { m_SlaveItemType = value; }
        }

        [Category("Address")]
        public virtual string Address
        {
            get { return ""; }
        }
        #endregion

        #region Constructor
        public EcSlaveItem()
        {
        }

        public EcSlaveItem(EcSlaveItemType type)
        {
            m_SlaveItemType = type;
        }

        public EcSlaveItem(EcSlaveItemType type, int id)
        {
            m_SlaveItemType = type;
            m_Id = id;
        }
        #endregion

        #region Methods
        public virtual EcSlaveItem Clone()
        {
            EcSlaveItem item = new EcSlaveItem
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType
            };

            return item;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Name))
                return this.GetType().Name;
            else
                return this.Name;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_Servo : EcSlaveItem
    {
        #region Feilds
        private int m_AxisIndex;
        #endregion

        #region Properties
        public int AxisIndex
        {
            get { return m_AxisIndex; }
            set { m_AxisIndex = value; }
        }

        public override string Address
        {
            get { return string.Format("{0:X2}SERVO{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_Servo() : base(EcSlaveItemType.Servo)
        {
        }
        #endregion

        #region Override
        public new EcSlaveItem_Servo Clone()
        {
            EcSlaveItem_Servo item = new EcSlaveItem_Servo
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType,

                AxisIndex = m_AxisIndex
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_BLDC : EcSlaveItem
    {
        #region Properties
        public override string Address
        {
            get { return string.Format("{0:X2}BLDC{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_BLDC() : base(EcSlaveItemType.BLDC)
        {
        }
        #endregion

        #region Override
        public new EcSlaveItem_BLDC Clone()
        {
            EcSlaveItem_BLDC item = new EcSlaveItem_BLDC
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_Inverter : EcSlaveItem
    {
        #region Properties
        public override string Address
        {
            get { return string.Format("{0:X2}IVT{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_Inverter() : base(EcSlaveItemType.Inverter)
        {
        }
        #endregion

        #region Override
        public new EcSlaveItem_Inverter Clone()
        {
            EcSlaveItem_Inverter item = new EcSlaveItem_Inverter
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_DI : EcSlaveItem
    {
        #region Fields
        private ActiveType m_ActiveType = ActiveType.A;
        #endregion

        #region Properties
        [Category("I/O Info")]
        public ActiveType ActiveType
        {
            get { return m_ActiveType; }
            set { m_ActiveType = value; }
        }

        public override string Address
        {
            get { return string.Format("{0:X2}X{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_DI() : base(EcSlaveItemType.DI)
        {
        }

        //public EcSlaveItem_DI(SlaveType type) : base(type)
        //{
        //}
        #endregion

        #region Override
        public new EcSlaveItem_DI Clone()
        {
            EcSlaveItem_DI item = new EcSlaveItem_DI
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType,
                ActiveType = m_ActiveType
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_DO : EcSlaveItem
    {
        #region Properties
        public override string Address
        {
            get { return string.Format("{0:X2}Y{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_DO() : base(EcSlaveItemType.DO)
        {
        }

        //public EcSlaveItem_DO(SlaveType type) : base(type)
        //{
        //}
        #endregion

        #region Override
        public new EcSlaveItem_DO Clone()
        {
            EcSlaveItem_DO item = new EcSlaveItem_DO
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_AI : EcSlaveItem
    {
        #region Properties
        public override string Address
        {
            get { return string.Format("{0:X2}AI{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_AI() : base(EcSlaveItemType.AI)
        {
        }

        //public EcSlaveItem_AI(SlaveType type) : base(type)
        //{
        //}
        #endregion

        #region Override
        public new EcSlaveItem_AI Clone()
        {
            EcSlaveItem_AI item = new EcSlaveItem_AI
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_AO : EcSlaveItem
    {
        #region Properties
        public override string Address
        {
            get { return string.Format("{0:X2}AO{1:X2}", m_AliasNo, m_Channel); }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_AO() : base(EcSlaveItemType.AO)
        {
        }

        //public EcSlaveItem_AO(SlaveType type) : base(type)
        //{
        //}
        #endregion

        #region Override
        public new EcSlaveItem_AO Clone()
        {
            EcSlaveItem_AO item = new EcSlaveItem_AO
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType
            };

            return item;
        }
        #endregion
    }

    [Serializable()]
    public class EcSlaveItem_AP : EcSlaveItem
    {
        #region Fields
        protected PeerType m_PeerType;
        protected int m_PeerId;
        #endregion

        #region Properties
        public override string Address
        {
            get { return string.Format("{0:X2}AP{1:X2}", m_AliasNo, m_Channel); }
        }

        [Category("Peer Info"), DisplayName("Peer Type")]
        public PeerType PeerType
        {
            get { return m_PeerType; }
            set { m_PeerType = value; }
        }

        [Category("Peer Info"), DisplayName("Peer Id")]
        public int PeerId
        {
            get { return m_PeerId; }
            set { m_PeerId = value; }
        }
        #endregion

        #region Constructor
        public EcSlaveItem_AP() : base(EcSlaveItemType.AP)
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public new EcSlaveItem_AP Clone()
        {
            EcSlaveItem_AP item = new EcSlaveItem_AP
            {
                Id = m_Id,
                SlaveNo = m_SlaveNo,
                AliasNo = m_AliasNo,
                Channel = m_Channel,
                Name = m_Name,
                Description = m_Description,
                State = m_State,
                SlaveItemType = m_SlaveItemType,

                PeerType = m_PeerType,
                PeerId = m_PeerId,
            };

            return item;
        }
        #endregion
    }
}