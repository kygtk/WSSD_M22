using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;
using System.Reflection;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class IoNode
    {
        #region Fields
        protected int m_Id;
        protected StructureType m_StructureType;
        protected List<IoTerminal> m_Terminals = new List<IoTerminal>();
        protected List<EcSlave> m_Slaves = new List<EcSlave>();
        protected IoPart m_NodeCoupler = null;
        protected IoPart m_NodeEndModule = null;
        //protected int m_DiCount = 0;
        //protected int m_DoCount = 0;
        //protected int m_AiCount = 0;
        //protected int m_AoCount = 0;
        #endregion

        #region Properties
        [Category("Basic Info"), ReadOnly(true)]
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        [Category("Node Info"), ReadOnly(true)]
        public StructureType StructureType
        {
            get { return m_StructureType; }
            set { m_StructureType = value; }
        }

        [Category("Node Info"), Browsable(true)]
        public virtual List<IoTerminal> Terminals
        {
            get { return m_Terminals; }
            set { m_Terminals = value; }
        }

        [Category("Node Info"), Browsable(false)]
        public virtual List<EcSlave> Slaves
        {
            get { return m_Slaves; }
            set { m_Slaves = value; }
        }

        [Category("Node Info")]
        public int Count
        {
            get
            {
                int cnt = 0;
                if (m_StructureType == StructureType.NodeTerminal) cnt = m_Terminals.Count;
                if (m_StructureType == StructureType.MasterSlave) cnt = m_Slaves.Count;
                return cnt;
            }
        }

        [Category("Node Info"), ReadOnly(true)]
        public IoPart NodeCoupler
        {
            get { return m_NodeCoupler; }
            set { m_NodeCoupler = value; }
        }

        [Category("Node Info"), ReadOnly(true)]
        public IoPart NodeEndModule
        {
            get { return m_NodeEndModule; }
            set { m_NodeEndModule = value; }
        }
        //[Browsable(false)]
        //public int DiCount
        //{
        //    get { return m_DiCount; }
        //}
        //[Browsable(false)]
        //public int DoCount
        //{
        //    get { return m_DoCount; }
        //}
        //[Browsable(false)]
        //public int AiCount
        //{
        //    get { return m_AiCount; }
        //}
        //[Browsable(false)]
        //public int AoCount
        //{
        //    get { return m_AoCount; }
        //}
        #endregion

        #region Constructor
        public IoNode()
        {
        }

        public IoNode(StructureType structuretype = StructureType.NodeTerminal)
        {
            m_StructureType = structuretype;
        }
        public IoNode(FieldBusType busType)
        {
            SetStructureType(busType);
            MakeNodeModule(busType);
        }
        #endregion

        #region Methods
        private void SetStructureType(FieldBusType busType)
        {
            switch (busType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                case FieldBusType.BrModbusTcp:
                case FieldBusType.CrevisModbusTcp:
                case FieldBusType.MitsubishiCClink:
                case FieldBusType.CrevisCClink:
                case FieldBusType.MitsubishiMelsecNet:
                case FieldBusType.MitsubishiMelsecEtherNet:
                case FieldBusType.TwinCATPlc:
                    m_StructureType = StructureType.NodeTerminal;
                    break;
                case FieldBusType.MovensysEtherCAT:
                    m_StructureType = StructureType.MasterSlave;
                    break;
            }
        }

        public void MakeNodeModule(FieldBusType busType)
        {
            switch (busType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                    m_NodeCoupler = new BK1120();
                    m_NodeEndModule = new KL9010();
                    break;
                case FieldBusType.BrModbusTcp:
                    m_NodeCoupler = new X20BC0087();
                    m_NodeEndModule = null;
                    break;
                case FieldBusType.CrevisModbusTcp:
                    m_NodeCoupler = new NA_9189();
                    m_NodeEndModule = null;
                    break;

            }
        }

        public static Maker[] GetBusMaker(FieldBusType busType)
        {
            Maker[] makers;//= Maker.Unknown;
            switch (busType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                    makers = new Maker[] { Maker.BeckHoff };
                    break;
                case FieldBusType.BrModbusTcp:
                    makers = new Maker[] { Maker.BR };
                    break;
                case FieldBusType.MitsubishiCClink:
                    makers = new Maker[] { Maker.MitsubishiCCLink };
                    break;
                case FieldBusType.CrevisCClink:
                    makers = new Maker[] { Maker.CrevisCCLink };
                    break;
                case FieldBusType.MitsubishiMelsecNet:
                    makers = new Maker[] { Maker.MitsubishiMelsecNet };
                    break;
                case FieldBusType.MitsubishiMelsecEtherNet:
                    makers = new Maker[] { Maker.MitsubishiMelsecNet };
                    break;
                case FieldBusType.TwinCATPlc:
                    makers = new Maker[] { Maker.BeckhoffPLC };
                    break;
                case FieldBusType.CrevisModbusTcp:
                    makers = new Maker[] { Maker.CrevisCCLink };
                    break;
                case FieldBusType.MovensysEtherCAT:
                    makers = new Maker[] { Maker.Movensys,
                                           Maker.Fastech};
                    break;
                default:
                    makers = new Maker[] { Maker.Unknown };
                    break;
            }

            return makers;
        }

        public static IoNode CreateNewNode(FieldBusType busType)
        {
            IoNode node;
            switch (busType)
            {
                case FieldBusType.BrModbusTcp:
                    node = new IoNodeBrModbus(busType);
                    break;
                case FieldBusType.CrevisModbusTcp:
                    node = new IoNodeCrevisModbus(busType);
                    break;
                case FieldBusType.MitsubishiMelsecNet:
                case FieldBusType.MitsubishiCClink:
                case FieldBusType.CrevisCClink:
                    node = new IoNodeMelsec(busType);
                    break;
                case FieldBusType.MitsubishiMelsecEtherNet:
                    node = new IoNodeMelsecEtherNet(busType);
                    break;
                case FieldBusType.TwinCATPlc:
                    node = new IoNodeTwinCATPLC(busType);
                    break;
                case FieldBusType.MovensysEtherCAT:
                    node = new IoNodeMovensysEcMaster(busType);
                    break;
                default:
                    node = new IoNode(busType);
                    break;
            }

            return node;
        }

        //private void SetPropertyAttribute()
        //{
        //    PropertyDescriptor pd_terminals = TypeDescriptor.GetProperties(this.GetType())["Terminals"];
        //    BrowsableAttribute ba_terminals = (BrowsableAttribute)pd_terminals.Attributes[typeof(BrowsableAttribute)];
        //    FieldInfo fi_terminals = ba_terminals.GetType().GetField("browsable", BindingFlags.NonPublic | BindingFlags.Instance);
        //    fi_terminals?.SetValue(ba_terminals, m_NodeType == NodeType.Node);

        //    PropertyDescriptor pd_slaves = TypeDescriptor.GetProperties(this.GetType())["Slaves"];
        //    BrowsableAttribute ba_slaves = (BrowsableAttribute)pd_slaves.Attributes[typeof(BrowsableAttribute)];
        //    FieldInfo fi_slaves = ba_slaves.GetType().GetField("browsable", BindingFlags.NonPublic | BindingFlags.Instance);
        //    fi_slaves?.SetValue(ba_slaves, m_NodeType == NodeType.EcMaster);
        //}

        //public void UpdateIoCountByType()
        //{
        //    m_DiCount = 0;
        //    m_DoCount = 0;
        //    m_AiCount = 0;
        //    m_AoCount = 0;

        //    int count = m_Terminals.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        IoTerminal terminal = m_Terminals[i];
        //        switch (terminal.IoType)
        //        { 
        //            case Dms.Common.IoType.DI:
        //                m_DiCount += terminal.ChannelCount;
        //                break;
        //            case Dms.Common.IoType.DO:
        //                m_DoCount += terminal.ChannelCount;
        //                break;
        //            case Dms.Common.IoType.AI:
        //                m_AiCount += terminal.ChannelCount;
        //                break;
        //            case Dms.Common.IoType.AO:
        //                m_AoCount += terminal.ChannelCount;
        //                break;
        //        }
        //    }
        //}
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Id + " : " + "NODE";
        }
        #endregion
    }
}
