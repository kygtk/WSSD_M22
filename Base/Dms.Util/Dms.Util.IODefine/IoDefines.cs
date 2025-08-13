using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Forms;
using Dms.Common;
using System.Reflection;

namespace Dms.Util.IODefine
{
    public enum Maker
    {
        DontCare,
        Unknown,
        BeckHoff,
        BR,
        Crevis,
        MitsubishiCCLink,
        CrevisCCLink,
        MitsubishiMelsecNet,
        BeckhoffPLC,
        Movensys,
        Fastech,
    }

    public enum FieldBusType
    {
        BeckhoffEtherCAT,
        BrModbusTcp,
        CrevisModbusTcp,
        MitsubishiCClink,
        CrevisCClink,
        MitsubishiMelsecNet,
        MitsubishiMelsecEtherNet,
        TwinCATPlc,
        MovensysEtherCAT,
    }

    #region XmlInclude
    [XmlInclude(typeof(IoItem))]
    [XmlInclude(typeof(IoItemDI))]
    [XmlInclude(typeof(IoNode))]
    [XmlInclude(typeof(IoTerminal))]
    [XmlInclude(typeof(BK1120))]
    [XmlInclude(typeof(KL1184))]
    [XmlInclude(typeof(KL1488))]
    [XmlInclude(typeof(KL2184))]
    [XmlInclude(typeof(KL2488))]
    [XmlInclude(typeof(KL3061))]
    [XmlInclude(typeof(KL3062))]
    [XmlInclude(typeof(KL3064))]
    [XmlInclude(typeof(KL3102))]
    [XmlInclude(typeof(KL4001))]
    [XmlInclude(typeof(KL4002))]
    [XmlInclude(typeof(KL4004))]
    [XmlInclude(typeof(KL4022))]
    [XmlInclude(typeof(KL9010))]
    [XmlInclude(typeof(IoNodeBrModbus))]
    [XmlInclude(typeof(X20AI2622))]
    [XmlInclude(typeof(X20AI4622))]
    [XmlInclude(typeof(X20AO2622))]
    [XmlInclude(typeof(X20AO4622))]
    [XmlInclude(typeof(X20BC0087))]
    [XmlInclude(typeof(X20DI9372))]
    [XmlInclude(typeof(X20DO9321))]
    [XmlInclude(typeof(X20PS2100))]
    [XmlInclude(typeof(X20PS9400))]
    [XmlInclude(typeof(CClinkMasterInfo))]
    [XmlInclude(typeof(CClinkStation))]
    [XmlInclude(typeof(CC16D))]
    [XmlInclude(typeof(CC16T))]
    [XmlInclude(typeof(CC32D))]
    [XmlInclude(typeof(CC32T))]
    [XmlInclude(typeof(CC32DT))]
    [XmlInclude(typeof(CC64AD))]
    [XmlInclude(typeof(CC62DA))]
    //[XmlInclude(typeof(CCNull08))]
    //[XmlInclude(typeof(CCNull16))]
    [XmlInclude(typeof(CCNull32))]
    [XmlInclude(typeof(CCReserved))]
    [XmlInclude(typeof(CCDummy))]
    [XmlInclude(typeof(CCSpecialDeivce))]
    [XmlInclude(typeof(CCIntelligentDeivce32DT))]
    [XmlInclude(typeof(CCXQD))]
    [XmlInclude(typeof(CCV1000))]
    [XmlInclude(typeof(IoNodeMelsec))]
    [XmlInclude(typeof(IoNodeMelsecEtherNet))]
    [XmlInclude(typeof(MelsecTerminal))]
    [XmlInclude(typeof(MelDevBitInputs))]
    [XmlInclude(typeof(MelDevBitInput08))]
    [XmlInclude(typeof(MelDevBitInput16))]
    [XmlInclude(typeof(MelDevBitInput32))]
    [XmlInclude(typeof(MelDevBitOutputs))]
    [XmlInclude(typeof(MelDevBitOutput08))]
    [XmlInclude(typeof(MelDevBitOutput16))]
    [XmlInclude(typeof(MelDevBitOutput32))]
    [XmlInclude(typeof(MelDevWordInputs))]
    [XmlInclude(typeof(MelDevWordInput08))]
    [XmlInclude(typeof(MelDevWordInput16))]
    [XmlInclude(typeof(MelDevWordInput32))]
    [XmlInclude(typeof(MelDevWordOutputs))]
    [XmlInclude(typeof(MelDevWordOutput08))]
    [XmlInclude(typeof(MelDevWordOutput16))]
    [XmlInclude(typeof(MelDevWordOutput32))]
    [XmlInclude(typeof(TwinCATPlcTerminal))]
    [XmlInclude(typeof(IoNodeTwinCATPLC))]

    [XmlInclude(typeof(NA_9131))]
    [XmlInclude(typeof(AT2_R312))]
    [XmlInclude(typeof(AT2_R321))]
    [XmlInclude(typeof(AT2_R334))]
    [XmlInclude(typeof(ST_1228))]
    [XmlInclude(typeof(ST_2318))]
    [XmlInclude(typeof(ST_3214))]
    [XmlInclude(typeof(ST_3624_1))]
    [XmlInclude(typeof(ST_4622))]

    [XmlInclude(typeof(IoNodeCrevisModbus))]
    [XmlInclude(typeof(NA_9189))]

    [XmlInclude(typeof(IoNodeMovensysEcMaster))]

    [XmlInclude(typeof(EcSlave))]
    [XmlInclude(typeof(EcSlave_BLDC))]
    [XmlInclude(typeof(EcSlave_IoType))]
    [XmlInclude(typeof(EcSlave_DI))]
    [XmlInclude(typeof(EcSlave_DIO))]
    [XmlInclude(typeof(EcSlave_DO))]
    [XmlInclude(typeof(EcSlave_AI))]
    [XmlInclude(typeof(EcSlave_AIO))]
    [XmlInclude(typeof(EcSlave_AO))]
    [XmlInclude(typeof(EcSlave_AP))]

    [XmlInclude(typeof(EcSlaveItem))]
    [XmlInclude(typeof(EcSlaveItem_Servo))]
    [XmlInclude(typeof(EcSlaveItem_BLDC))]
    [XmlInclude(typeof(EcSlaveItem_Inverter))]
    [XmlInclude(typeof(EcSlaveItem_DI))]
    [XmlInclude(typeof(EcSlaveItem_DO))]
    [XmlInclude(typeof(EcSlaveItem_AI))]
    [XmlInclude(typeof(EcSlaveItem_AO))]
    [XmlInclude(typeof(EcSlaveItem_AP))]

    [XmlInclude(typeof(MADLN05BE))]
    [XmlInclude(typeof(MADLT15BF))]
    [XmlInclude(typeof(MCDLN35BE))]
    [XmlInclude(typeof(MFDLNB3BE))]
    [XmlInclude(typeof(ESD_EC_120_C))]
    [XmlInclude(typeof(YCS_BMC_XBD1))]
    [XmlInclude(typeof(S100))]
    [XmlInclude(typeof(BHY_IO2_EC_DI16NT))]
    [XmlInclude(typeof(BHY_IO2_EC_DI32NT))]
    [XmlInclude(typeof(BHY_IO2_EC_DO16NT))]
    [XmlInclude(typeof(BHY_IO2_EC_DO32NT))]
    [XmlInclude(typeof(BHY_IO2_EC_MD8NT))]
    [XmlInclude(typeof(BHY_IO2_EC_MD16NT))]
    [XmlInclude(typeof(Ezi_IO_EC_AD08_T))]
    [XmlInclude(typeof(DMS_AP))]
    #endregion
    [Serializable()]
    public class IoDefines
    {
        #region Singleton code...
        //public static readonly IoDefines Instance = new IoDefines();
        #endregion

        #region Fields
        private List<IoNode> m_Container = new List<IoNode>();
        private List<IoItemDI> m_DigitalInputs = new List<IoItemDI>();
        private List<IoItem> m_DigitalOutputs = new List<IoItem>();
        private List<IoItem> m_AnalogInputs = new List<IoItem>();
        private List<IoItem> m_AnalogOutputs = new List<IoItem>();
        private List<EcSlaveItem_Servo> m_SlaveServos = new List<EcSlaveItem_Servo>();
        private List<EcSlaveItem_BLDC> m_SlaveBLDCs = new List<EcSlaveItem_BLDC>();
        private List<EcSlaveItem_Inverter> m_SlaveInverters = new List<EcSlaveItem_Inverter>();
        private List<EcSlaveItem_DI> m_SlaveDigitalInputs = new List<EcSlaveItem_DI>();
        private List<EcSlaveItem_DO> m_SlaveDigitalOutputs = new List<EcSlaveItem_DO>();
        private List<EcSlaveItem_AI> m_SlaveAnalogInputs = new List<EcSlaveItem_AI>();
        private List<EcSlaveItem_AO> m_SlaveAnalogOutputs = new List<EcSlaveItem_AO>();
        private List<EcSlaveItem_AP> m_SlaveAPs = new List<EcSlaveItem_AP>();
        private string m_FileName = "";
        private static string m_FilePath = "";	//jemoon : 모든 Bustype의 저장경로는 같아야 한다.
        private int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        private FieldBusType m_BusType = FieldBusType.BeckhoffEtherCAT;
        private WireNumberingMode m_WireNumberingMode = WireNumberingMode.ByOct;
        private bool m_SimulateController = false;
        #endregion

        #region Properties
        [Category("Basic Info")]
        public WireNumberingMode WireNumberMode
        {
            get { return m_WireNumberingMode; }
            set { m_WireNumberingMode = value; }
        }

        [Category("Basic Info")]
        public bool SimulateController
        {
            get { return m_SimulateController; }
            set { m_SimulateController = value; }
        }

        [Category("Node Info")]
        public List<IoNode> Container
        {
            get { return m_Container; }
            set { m_Container = value; }
        }

        [Category("Node Info"), ReadOnly(true)]
        public FieldBusType BusType
        {
            get { return m_BusType; }
            set { m_BusType = value; }
        }

        [Category("Node Info")]
        public int Count
        {
            get { return m_Container.Count; }
        }

        [Category("I/O Info"), XmlIgnore()]
        public List<IoItemDI> DigitalInputs
        {
            get { return m_DigitalInputs; }
            set { m_DigitalInputs = value; }
        }

        [Category("I/O Info"), XmlIgnore()]
        public List<IoItem> DigitalOutputs
        {
            get { return m_DigitalOutputs; }
            set { m_DigitalOutputs = value; }
        }

        [Category("I/O Info"), XmlIgnore()]
        public List<IoItem> AnalogInputs
        {
            get { return m_AnalogInputs; }
            set { m_AnalogInputs = value; }
        }

        [Category("I/O Info"), XmlIgnore()]
        public List<IoItem> AnalogOutputs
        {
            get { return m_AnalogOutputs; }
            set { m_AnalogOutputs = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_Servo> SlaveServos
        {
            get { return m_SlaveServos; }
            set { m_SlaveServos = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_BLDC> SlaveBLDCs
        {
            get { return m_SlaveBLDCs; }
            set { m_SlaveBLDCs = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_Inverter> SlaveInverters
        {
            get { return m_SlaveInverters; }
            set { m_SlaveInverters = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_DI> SlaveDigitalInputs
        {
            get { return m_SlaveDigitalInputs; }
            set { m_SlaveDigitalInputs = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_DO> SlaveDigitalOutputs
        {
            get { return m_SlaveDigitalOutputs; }
            set { m_SlaveDigitalOutputs = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_AI> SlaveAnalogInputs
        {
            get { return m_SlaveAnalogInputs; }
            set { m_SlaveAnalogInputs = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_AO> SlaveAnalogOutputs
        {
            get { return m_SlaveAnalogOutputs; }
            set { m_SlaveAnalogOutputs = value; }
        }

        [Category("Slave Info"), XmlIgnore()]
        public List<EcSlaveItem_AP> SlaveAPs
        {
            get { return m_SlaveAPs; }
            set { m_SlaveAPs = value; }
        }

        [Browsable(false), XmlIgnore()]
        public string FileName
        {
            get { return m_FileName; }
        }

        public bool IsDefined
        {
            get
            {
                bool ok = false;
                ok |= m_DigitalInputs.Count > 0;
                ok |= m_DigitalOutputs.Count > 0;
                ok |= m_AnalogInputs.Count > 0;
                ok |= m_AnalogOutputs.Count > 0;

                ok |= m_SlaveServos.Count > 0;
                ok |= m_SlaveBLDCs.Count > 0;
                ok |= m_SlaveInverters.Count > 0;
                ok |= m_SlaveDigitalInputs.Count > 0;
                ok |= m_SlaveDigitalOutputs.Count > 0;
                ok |= m_SlaveAnalogInputs.Count > 0;
                ok |= m_SlaveAnalogOutputs.Count > 0;
                ok |= m_SlaveAPs.Count > 0;
                //jemoon - 100128 : 이걸 내가 넣었다고 하는데 왜 넣었는지 기억 안남
                //오늘생각해 보니 필요없을것 같아서 주석처리 함
                //나중에 필요한 일이 있으면 생각 나겠지
                //ok &= m_Container.Count > 0; 
                return ok;
            }
        }
        #endregion

        #region Constructor
        public IoDefines()
        {
        }
        public IoDefines(FieldBusType busType)
        {
            m_BusType = busType;
        }
        #endregion

        #region Methods
        public void Clone(IoDefines iodefines)
        {
            m_Container = iodefines.Container;
            m_DigitalInputs = iodefines.DigitalInputs;
            m_DigitalOutputs = iodefines.DigitalOutputs;
            m_AnalogInputs = iodefines.AnalogInputs;
            m_AnalogOutputs = iodefines.AnalogOutputs;
            m_SlaveServos = iodefines.SlaveServos;
            m_SlaveBLDCs = iodefines.SlaveBLDCs;
            m_SlaveInverters = iodefines.SlaveInverters;
            m_SlaveDigitalInputs = iodefines.SlaveDigitalInputs;
            m_SlaveDigitalOutputs = iodefines.SlaveDigitalOutputs;
            m_SlaveAnalogInputs = iodefines.SlaveAnalogInputs;
            m_SlaveAnalogOutputs = iodefines.SlaveAnalogOutputs;
            m_SlaveAPs = iodefines.SlaveAPs;
        }

        public void ClearCollection()
        {
            ClearIOCollection();
            ClearSlaveCollection();
        }

        private void ClearIOCollection()
        {
            m_DigitalInputs.Clear();
            m_DigitalOutputs.Clear();
            m_AnalogInputs.Clear();
            m_AnalogOutputs.Clear();
        }

        private void ClearSlaveCollection()
        {
            m_SlaveServos.Clear();
            m_SlaveBLDCs.Clear();
            m_SlaveInverters.Clear();
            m_SlaveDigitalInputs.Clear();
            m_SlaveDigitalOutputs.Clear();
            m_SlaveAnalogInputs.Clear();
            m_SlaveAnalogOutputs.Clear();
            m_SlaveAPs.Clear();
        }

        public void AddIOCollection(IoItem item)
        {
            if (item.IoType == IoType.DI)
            {
                m_DigitalInputs.Add(item as IoItemDI);
            }
            else if (item.IoType == IoType.DO)
            {
                m_DigitalOutputs.Add(item);
            }
            else if (item.IoType == IoType.AI)
            {
                m_AnalogInputs.Add(item);
            }
            else if (item.IoType == IoType.AO)
            {
                m_AnalogOutputs.Add(item);
            }
        }

        public void AddSlaveCollection(EcSlaveItem item)
        {
            switch (item.SlaveItemType)
            {
                case EcSlaveItemType.Servo:
                    m_SlaveServos.Add(item as EcSlaveItem_Servo);
                    break;
                case EcSlaveItemType.BLDC:
                    m_SlaveBLDCs.Add(item as EcSlaveItem_BLDC);
                    break;
                case EcSlaveItemType.Inverter:
                    m_SlaveInverters.Add(item as EcSlaveItem_Inverter);
                    break;
                case EcSlaveItemType.DI:
                    m_SlaveDigitalInputs.Add(item as EcSlaveItem_DI);
                    break;
                case EcSlaveItemType.DO:
                    m_SlaveDigitalOutputs.Add(item as EcSlaveItem_DO);
                    break;
                case EcSlaveItemType.AI:
                    m_SlaveAnalogInputs.Add(item as EcSlaveItem_AI);
                    break;
                case EcSlaveItemType.AO:
                    m_SlaveAnalogOutputs.Add(item as EcSlaveItem_AO);
                    break;
                case EcSlaveItemType.AP:
                    m_SlaveAPs.Add(item as EcSlaveItem_AP);
                    break;
            }
        }

        public void InitializeIOCollection()
        {
            ClearCollection();
            int nodeCount = m_Container.Count;
            int terminalCount;
            int channelCount;
            int slavecount;
            for (int nodeid = 0; nodeid < nodeCount; nodeid++)
            {
                IoNode ioNode = m_Container[nodeid];
                terminalCount = ioNode.Terminals.Count;
                for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                {
                    IoTerminal terminal = ioNode.Terminals[terminalid];
                    channelCount = terminal.ChannelCount;
                    for (int channelid = 0; channelid < channelCount; channelid++)
                    {
                        IoItem item = terminal.Channels[channelid];
                        AddIOCollection(item);
                    }
                }

                slavecount = ioNode.Slaves.Count;
                for (int slaveid = 0; slaveid < slavecount; slaveid++)
                {
                    EcSlave slave = ioNode.Slaves[slaveid];
                    switch (slave.SlaveType)
                    {
                        case SlaveType.Servo:
                            {
                                channelCount = ((EcSlave_Servo)slave).ServoCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_Servo)slave).Servos[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.BLDC:
                            {
                                channelCount = ((EcSlave_BLDC)slave).BLDCCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_BLDC)slave).BLDCs[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.Inverter:
                            {
                                channelCount = ((EcSlave_Inverter)slave).InverterCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_Inverter)slave).Inverters[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.DI:
                            {
                                channelCount = ((EcSlave_DI)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DI)slave).InChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.DIO:
                            {
                                channelCount = ((EcSlave_DIO)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DIO)slave).InChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                                channelCount = ((EcSlave_DIO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DIO)slave).OutChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.DO:
                            {
                                channelCount = ((EcSlave_DO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DO)slave).OutChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.AI:
                            {
                                channelCount = ((EcSlave_AI)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AI)slave).InChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.AIO:
                            {
                                channelCount = ((EcSlave_AIO)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AIO)slave).InChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                                channelCount = ((EcSlave_AIO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AIO)slave).OutChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.AO:
                            {
                                channelCount = ((EcSlave_AO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AO)slave).OutChannels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                        case SlaveType.AP:
                            {
                                channelCount = ((EcSlave_AP)slave).ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AP)slave).Channels[channelid];
                                    AddSlaveCollection(item);
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());

            try
            {   // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();

                return;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.CopyTo(fileName + ".old", true);
                }

                sw = new StreamWriter(fileName);
                xmlSer.Serialize(sw, this);
                sw.Close();

                m_FileName = fileName;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                System.Windows.Forms.MessageBox.Show(err.ToString());
            }
        }

        public void WriteXml()
        {
            if (string.IsNullOrEmpty(m_FileName))
            {
                string dirName = AppConfig.DefaultConfigFilePath;
                Directory.CreateDirectory(dirName);

                m_FileName = string.Format("{0}\\{1}.xml", dirName, GetDefaultFileName());
            }

            WriteXml(m_FileName);
        }

        public bool ReadXml(string fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    m_FileName = fileName;
                }
                else
                {
                    //MessageBox.Show("File not found");
                    //OpenFileDialog dlg = new OpenFileDialog();
                    //dlg.Title = "Select XML file : " + this.ToString();
                    //dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    //if (DialogResult.OK == dlg.ShowDialog())
                    //{
                    //    m_FileName = dlg.FileName;
                    //}

                    //jemoon : 100107 - 지정경로에 해당 file이 없다면 해당 Bustype은 Nouse로 처리 -> MessageBox를 띄우지 않아야한다.
                    //string name = string.Format("{0}.xml", GetDefaultFileName());
                    //MessageBox.Show(name + " file does not exist in the specified location. Check the file and try again.");

                    return false;
                }

                m_Container.Clear();

                StreamReader sr = new StreamReader(m_FileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(IoDefines));
                IoDefines pool = new IoDefines();
                pool = xmlSer.Deserialize(sr) as IoDefines;
                sr.Close();

                m_Container = pool.Container;
                m_BusType = pool.BusType;
                m_WireNumberingMode = pool.WireNumberMode;
                m_SimulateController = pool.SimulateController;

                InitializeIOCollection();

                //MitsubishiMelsecEtherNet은 ioItem 이 text로 정의된다.
                if (m_BusType == FieldBusType.MitsubishiMelsecEtherNet)
                {
                    if (m_Container.Count > 0)
                    {   //Node가 한개 이상 등록되어 있다면
                        MelsecDeviceInfoProvider provider = new MelsecDeviceInfoProvider();
                        provider.ReadFromStorage();
                        provider.GetIoItems(this);
                        UpdateWiringNo();
                    }
                }


                if (m_BusType == FieldBusType.CrevisCClink)
                {
                    if (m_Container.Count > 0)
                    {
                        UpdateWiringNo();

                    }

                }


                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string[] split = fileName.Split('\\');
                MessageBox.Show(err.Message, split[split.Length - 1]);
                return false;
            }
        }

        public bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                return ReadXml(m_FileName);
            }
            else return false;
        }

        private void CheckPath()
        {
            //jemoon : 090929 - use default path option
            string filePath = m_FilePath;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = m_AppConfig.IoDefinePathName;
            }

            if (m_AppConfig.UseDefaultFilePath)
            {
                //Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("IoDefine Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "IoDefine File Folder";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.IoDefinePath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    m_FilePath = filePath;
                    m_FileName = string.Format("{0}\\{1}.xml", filePath, GetDefaultFileName());
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FilePath = filePath;
                m_FileName = string.Format("{0}\\{1}.xml", filePath, GetDefaultFileName());
                m_CheckPath = 1;
            }
        }

        public void WriteText(string fileName)
        {
            try
            {
                StreamWriter sw = File.CreateText(fileName);
                sw.AutoFlush = true;

                sw.WriteLine("===================== Digital Input =====================");
                sw.WriteLine("ID\tWire\tName");
                sw.WriteLine("=======================================================");
                string txt = "";
                foreach (IoItem io in this.DigitalInputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", io.Id, io.WiringNo, io.Name);
                    sw.WriteLine(txt);
                }

                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("===================== Digital Output ====================");
                sw.WriteLine("ID\tWire\tName");
                sw.WriteLine("=======================================================");
                foreach (IoItem io in this.DigitalOutputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", io.Id, io.WiringNo, io.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("===================== Analog Input ======================");
                sw.WriteLine("ID\tWire\tName");
                sw.WriteLine("=======================================================");
                foreach (IoItem io in this.AnalogInputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", io.Id, io.WiringNo, io.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("===================== Analog Output =====================");
                sw.WriteLine("ID\tWire\tName");
                sw.WriteLine("=======================================================");
                foreach (IoItem io in this.AnalogOutputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", io.Id, io.WiringNo, io.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("===================== Slave Servo ====================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveServos)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("====================== Slave BLDC =====================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveBLDCs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("==================== Slave Inverter ===================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveInverters)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("================= Slave Digital Input =================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveDigitalInputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }

                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("================= Slave Digital Output ================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveDigitalOutputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("================= Slave Analog Input ==================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveAnalogInputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }


                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("================= Slave Analog Output =================");
                sw.WriteLine("ID\tAddress\tName");
                sw.WriteLine("=======================================================");
                foreach (EcSlaveItem slave in this.SlaveAnalogOutputs)
                {
                    txt = string.Format("{0:d3}\t{1}\t{2}", slave.Id, slave.Address, slave.Name);
                    sw.WriteLine(txt);
                }

                sw.Close();
            }
            catch (Exception err) //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.Message);
            }
        }

        public void WriteText()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save As...";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = GetDefaultFileName() + ".txt";
            dlg.DefaultExt = "txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    this.WriteText(dlg.FileName);
                }
                catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private List<IoTerminal> m_TerminalTypes = null;
        private void MakeTerminalTypes()
        {
            if (m_TerminalTypes == null)
            {
                m_TerminalTypes = GetTerminalTypes();
            }
        }

        public static List<IoTerminal> GetTerminalTypes()
        {
            return GetTerminalTypes(Maker.DontCare);
        }

        public static List<IoTerminal> GetTerminalTypes(Maker productMaker)
        {
            List<IoTerminal> terminalTypes = new List<IoTerminal>();
            Assembly asm = Assembly.Load((typeof(IoDefines)).Namespace);
            Type[] types = asm.GetTypes();
            foreach (Type type in types)
            {
                if (XFunc.CheckTypeCompatibility(typeof(IoTerminal), type, Compatibility.Compatible))
                {
                    if (!type.IsAbstract)
                    {
                        IoTerminal terminal = Activator.CreateInstance(type) as IoTerminal;
                        // DontCare == 모든 Maker
                        if (productMaker == Maker.DontCare || terminal.ProductMaker == productMaker)
                        {
                            terminalTypes.Add(terminal);
                        }
                    }
                }
            }

            return terminalTypes;
        }

        public static List<IoTerminal> GetTerminalTypes(Maker[] productMakers)
        {
            List<IoTerminal> terminalTypes = new List<IoTerminal>();
            Assembly asm = Assembly.Load((typeof(IoDefines)).Namespace);
            Type[] types = asm.GetTypes();
            foreach (Type type in types)
            {
                if (XFunc.CheckTypeCompatibility(typeof(IoTerminal), type, Compatibility.Compatible))
                {
                    if (!type.IsAbstract)
                    {
                        IoTerminal terminal = Activator.CreateInstance(type) as IoTerminal;
                        // DontCare == 모든 Maker
                        foreach (Maker productMaker in productMakers)
                        {
                            if (productMaker == Maker.DontCare || terminal.ProductMaker == productMaker)
                            {
                                terminalTypes.Add(terminal);
                                break;
                            }
                        }
                    }
                }
            }

            return terminalTypes;
        }

        public static List<IoTerminal> GetTerminalTypes(Maker productMaker, IoType ioType)
        {
            List<IoTerminal> terminalTypes = new List<IoTerminal>();
            terminalTypes = GetTerminalTypes(productMaker);
            int count = terminalTypes.Count;
            for (int i = (count - 1); i >= 0; i--)
            {
                if (terminalTypes[i].IoType != ioType)
                {
                    terminalTypes.RemoveAt(i);
                }
            }

            return terminalTypes;
        }

        public static List<IoTerminal> GetTerminalTypes(Maker[] productMakers, IoType ioType)
        {
            List<IoTerminal> terminalTypes = new List<IoTerminal>();

            foreach (Maker productMaker in productMakers)
            {
                terminalTypes.AddRange(GetTerminalTypes(productMaker));
            }

            int count = terminalTypes.Count;
            for (int i = (count - 1); i >= 0; i--)
            {
                if (terminalTypes[i].IoType != ioType)
                {
                    terminalTypes.RemoveAt(i);
                }
            }
            return terminalTypes;
        }

        public static List<EcSlave> GetSlaveTypes()
        {
            List<EcSlave> slaveTypes = new List<EcSlave>();
            Assembly asm = Assembly.Load(typeof(IoDefines).Namespace);
            Type[] types = asm.GetTypes();
            foreach (Type type in types)
            {
                if (XFunc.CheckTypeCompatibility(typeof(EcSlave), type, Compatibility.Compatible))
                {
                    if (!type.IsAbstract)
                    {
                        EcSlave slave = Activator.CreateInstance(type) as EcSlave;
                        // Slave는 Maker와 관계 없으므로 전부 추가
                        slaveTypes.Add(slave);
                    }
                }
            }

            return slaveTypes;
        }
        public static List<EcSlave> GetSlaveTypes(SlaveType slaveType)
        {
            List<EcSlave> slaveTypes = new List<EcSlave>();

            slaveTypes.AddRange(GetSlaveTypes());

            int count = slaveTypes.Count;
            for (int i = (count - 1); i >= 0; i--)
            {
                if (slaveTypes[i].SlaveType != slaveType)
                {
                    slaveTypes.RemoveAt(i);
                }
            }
            return slaveTypes;
        }


        public bool IsExist(List<IoPart> items, IoPart part)
        {
            bool find = false;
            if (part != null)
            {
                Type type = part.GetType();
                foreach (IoPart io in items)
                {
                    if (io.GetType() == type)
                    {
                        find = true;
                        break;
                    }
                }
            }

            return find;
        }

        public List<IoPart> GetContainedPartsTypes()
        {
            MakeTerminalTypes();

            List<IoPart> result = new List<IoPart>();

            foreach (IoNode node in m_Container)
            {
                IoPart coupler = node.NodeCoupler;
                IoPart end = node.NodeEndModule;

                if (coupler != null && !IsExist(result, coupler))
                {
                    result.Add(coupler);
                }

                if (end != null && !IsExist(result, end))
                {
                    result.Add(end);
                }
            }

            foreach (IoTerminal source in m_TerminalTypes)
            {
                bool find = false;
                foreach (IoNode node in m_Container)
                {
                    List<IoTerminal> terminals = node.Terminals;
                    foreach (IoTerminal terminal in terminals)
                    {
                        if (source.GetType() == terminal.GetType())
                        {
                            find = true;
                            result.Add(source);
                            break;
                        }
                    }

                    if (find)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public List<IoPart> GetContainedParts(Type type)
        {
            List<IoPart> result = new List<IoPart>();

            foreach (IoNode node in m_Container)
            {
                IoPart coupler = node.NodeCoupler;
                IoPart end = node.NodeEndModule;

                if (coupler != null && type == coupler.GetType())
                {
                    result.Add(coupler);
                }
                if (end != null && type == end.GetType())
                {
                    result.Add(end);
                }
            }

            foreach (IoNode node in m_Container)
            {
                List<IoTerminal> terminals = node.Terminals;
                foreach (IoTerminal terminal in terminals)
                {
                    if (type == terminal.GetType())
                    {
                        result.Add(terminal);
                    }
                }
            }

            return result;
        }

        public string GetDefaultFileName()
        {
            string fileName = "";

            switch (m_BusType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                    {
                        fileName = this.GetType().Name;
                    }
                    break;
                case FieldBusType.BrModbusTcp:
                case FieldBusType.CrevisModbusTcp:
                case FieldBusType.MitsubishiCClink:
                case FieldBusType.CrevisCClink:
                case FieldBusType.MitsubishiMelsecNet:
                case FieldBusType.MitsubishiMelsecEtherNet:
                case FieldBusType.TwinCATPlc:
                case FieldBusType.MovensysEtherCAT:
                    {
                        fileName = this.GetType().Name + m_BusType.ToString();
                    }
                    break;
            }

            return fileName;
        }

        public void UpdateModuleIndex()
        {
            switch (m_BusType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                case FieldBusType.BrModbusTcp:
                case FieldBusType.MitsubishiMelsecNet:
                    {
                        int diCount = 0;
                        int doCount = 0;
                        int aiCount = 0;
                        int aoCount = 0;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            // Update Node Id
                            IoNode ioNode = m_Container[nodeid];
                            ioNode.Id = nodeid;

                            int diTerminalCountByNode = 0;
                            int doTerminalCountByNode = 0;
                            int aiTerminalCountByNode = 0;
                            int aoTerminalCountByNode = 0;

                            // Update Terminal Id
                            int terminalCount = ioNode.Terminals.Count;
                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                IoTerminal terminal = ioNode.Terminals[terminalid];

                                int ioTypeIdByNode = 0;
                                if (terminal.IoType == IoType.DI) ioTypeIdByNode = diTerminalCountByNode++;
                                else if (terminal.IoType == IoType.DO) ioTypeIdByNode = doTerminalCountByNode++;
                                else if (terminal.IoType == IoType.AI) ioTypeIdByNode = aiTerminalCountByNode++;
                                else if (terminal.IoType == IoType.AO) ioTypeIdByNode = aoTerminalCountByNode++;

                                terminal.Id = terminalid;
                                terminal.IdByIoType = ioTypeIdByNode;

                                //Update channel Id
                                int channelCount = terminal.ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    int ioId = 0;
                                    if (terminal.IoType == IoType.DI) ioId = diCount++;
                                    else if (terminal.IoType == IoType.DO) ioId = doCount++;
                                    else if (terminal.IoType == IoType.AI) ioId = aiCount++;
                                    else if (terminal.IoType == IoType.AO) ioId = aoCount++;

                                    IoItem item = terminal.Channels[channelid];
                                    item.Id = ioId;
                                    item.Node = nodeid;
                                    item.Terminal = ioTypeIdByNode;
                                    item.Channel = channelid;
                                    item.IoType = terminal.IoType;
                                }
                            }

                            //ioNode.UpdateIoCountByType();
                        }
                    }
                    break;
                case FieldBusType.MitsubishiCClink:
                    {
                        int diCount = 0;
                        int doCount = 0;
                        int aiCount = 0;
                        int aoCount = 0;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            // Update Node Id
                            IoNode ioNode = m_Container[nodeid];
                            ioNode.Id = nodeid;

                            int diTerminalCountByNode = 0;
                            int doTerminalCountByNode = 0;
                            int aiTerminalCountByNode = 0;
                            int aoTerminalCountByNode = 0;

                            // Update Terminal Id
                            int terminalCount = ioNode.Terminals.Count;
                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                IoTerminal terminal = ioNode.Terminals[terminalid];

                                int ioTypeIdByNode = 0;
                                if (terminal.IoType == IoType.DI) ioTypeIdByNode = diTerminalCountByNode++;
                                else if (terminal.IoType == IoType.DO) ioTypeIdByNode = doTerminalCountByNode++;
                                else if (terminal.IoType == IoType.AI) ioTypeIdByNode = aiTerminalCountByNode++;
                                else if (terminal.IoType == IoType.AO) ioTypeIdByNode = aoTerminalCountByNode++;

                                terminal.Id = terminalid;
                                terminal.IdByIoType = ioTypeIdByNode;

                                //Update channel Id
                                int channelCount = terminal.ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    IoItem item = terminal.Channels[channelid];
                                    int ioId = 0;
                                    if (item.IoType == IoType.DI) ioId = diCount++;
                                    else if (item.IoType == IoType.DO) ioId = doCount++;
                                    else if (item.IoType == IoType.AI) ioId = aiCount++;
                                    else if (item.IoType == IoType.AO) ioId = aoCount++;

                                    item.Id = ioId;
                                    item.Node = nodeid;
                                    item.Terminal = terminalid;
                                    //item.Channel = channelid;
                                    //item.IoType = terminal.IoType;
                                }
                            }

                            //ioNode.UpdateIoCountByType();
                        }
                    }
                    break;

                case FieldBusType.CrevisCClink:
                    {
                        int diCount = 0;
                        int doCount = 0;
                        int aiCount = 0;
                        int aoCount = 0;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            // Update Node Id
                            IoNode ioNode = m_Container[nodeid];
                            ioNode.Id = nodeid;

                            int diTerminalCountByNode = 0;
                            int doTerminalCountByNode = 0;
                            int aiTerminalCountByNode = 0;
                            int aoTerminalCountByNode = 0;

                            // Update Terminal Id
                            int terminalCount = ioNode.Terminals.Count;
                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                IoTerminal terminal = ioNode.Terminals[terminalid];

                                int ioTypeIdByNode = 0;
                                if (terminal.IoType == IoType.DI) ioTypeIdByNode = diTerminalCountByNode++;
                                else if (terminal.IoType == IoType.DO) ioTypeIdByNode = doTerminalCountByNode++;
                                else if (terminal.IoType == IoType.AI) ioTypeIdByNode = aiTerminalCountByNode++;
                                else if (terminal.IoType == IoType.AO) ioTypeIdByNode = aoTerminalCountByNode++;

                                terminal.Id = terminalid;
                                terminal.IdByIoType = ioTypeIdByNode;

                                //Update channel Id
                                int channelCount = terminal.ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    IoItem item = terminal.Channels[channelid];
                                    int ioId = 0;
                                    if (item.IoType == IoType.DI) ioId = diCount++;
                                    else if (item.IoType == IoType.DO) ioId = doCount++;
                                    else if (item.IoType == IoType.AI) ioId = aiCount++;
                                    else if (item.IoType == IoType.AO) ioId = aoCount++;

                                    item.Id = ioId;
                                    item.Node = nodeid;
                                    item.Terminal = ioTypeIdByNode;
                                    item.Channel = channelid;
                                    //item.IoType = terminal.IoType;
                                }
                            }

                            //ioNode.UpdateIoCountByType();
                        }
                    }
                    break;

                case FieldBusType.CrevisModbusTcp:
                    {
                        int diCount = 0;
                        int doCount = 0;
                        int aiCount = 0;
                        int aoCount = 0;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            // Update Node Id
                            IoNode ioNode = m_Container[nodeid];
                            ioNode.Id = nodeid;

                            int inputCountByNode = 0;
                            int outputCountByNode = 0;

                            int nextInputCount = 0;
                            int nextoutputCont = 0;

                            // Update Terminal Id
                            int terminalCount = ioNode.Terminals.Count;
                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                CClinkStation terminal = ioNode.Terminals[terminalid] as CClinkStation;

                                int ioTypeIdByNode = 0;
                                if (terminal.IoType == IoType.DI)
                                {
                                    ioTypeIdByNode = nextInputCount;
                                    nextInputCount = nextInputCount + terminal.Info.Points / 8;
                                }
                                else if (terminal.IoType == IoType.AI)
                                {
                                    ioTypeIdByNode = nextInputCount;
                                    nextInputCount = nextInputCount + terminal.Info.Points / 8;
                                }
                                else if (terminal.IoType == IoType.DO)
                                {
                                    ioTypeIdByNode = nextoutputCont;
                                    nextoutputCont = nextoutputCont + terminal.Info.Points / 8;
                                }
                                else if (terminal.IoType == IoType.AO)
                                {
                                    ioTypeIdByNode = nextoutputCont;
                                    nextoutputCont = nextoutputCont + terminal.Info.Points / 8;
                                }

                                terminal.Id = terminalid;
                                terminal.IdByIoType = ioTypeIdByNode;

                                //Update channel Id
                                int channelCount = terminal.ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    IoItem item = terminal.Channels[channelid];
                                    int ioId = 0;
                                    if (item.IoType == IoType.DI) ioId = diCount++;
                                    else if (item.IoType == IoType.DO) ioId = doCount++;
                                    else if (item.IoType == IoType.AI) ioId = aiCount++;
                                    else if (item.IoType == IoType.AO) ioId = aoCount++;

                                    item.Id = ioId;
                                    item.Node = nodeid;
                                    item.Terminal = ioTypeIdByNode;
                                    item.Channel = channelid;
                                    //item.IoType = terminal.IoType;
                                }
                            }

                            //ioNode.UpdateIoCountByType();
                        }
                    }
                    break;

                case FieldBusType.MovensysEtherCAT:
                    {
                        int servoCount = 0;
                        int bldcCount = 0;
                        int inverterCount = 0;
                        int diCount = 0;
                        int doCount = 0;
                        int aiCount = 0;
                        int aoCount = 0;
                        int apCount = 0;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            // Update Node Id
                            IoNode ioNode = m_Container[nodeid];
                            ioNode.Id = nodeid;

                            // Update Channel Id
                            int slaveCount = ioNode.Slaves.Count;
                            for (int slaveid = 0; slaveid < slaveCount; slaveid++)
                            {
                                EcSlave slave = ioNode.Slaves[slaveid];

                                switch (slave.SlaveType)
                                {
                                    case SlaveType.Servo:
                                        {
                                            EcSlave_Servo _servo = slave as EcSlave_Servo;
                                            for (int ch = 0; ch < _servo.ServoCount; ch++)
                                                _servo.Servos[ch].Id = servoCount++;
                                        }
                                        break;
                                    case SlaveType.BLDC:
                                        {
                                            EcSlave_BLDC _bldc = slave as EcSlave_BLDC;
                                            for (int ch = 0; ch < _bldc.BLDCCount; ch++)
                                                _bldc.BLDCs[ch].Id = bldcCount++;
                                        }
                                        break;
                                    case SlaveType.Inverter:
                                        {
                                            EcSlave_Inverter _inverter = slave as EcSlave_Inverter;
                                            for (int ch = 0; ch < _inverter.InverterCount; ch++)
                                                _inverter.Inverters[ch].Id = inverterCount++;
                                        }
                                        break;
                                    case SlaveType.DI:
                                        {
                                            EcSlave_DI _di = slave as EcSlave_DI;
                                            for (int ch = 0; ch < _di.InChannelCount; ch++)
                                                _di.InChannels[ch].Id = diCount++;
                                        }
                                        break;
                                    case SlaveType.DIO:
                                        {
                                            EcSlave_DIO _dio = slave as EcSlave_DIO;
                                            for (int ch = 0; ch < _dio.InChannelCount; ch++)
                                                _dio.InChannels[ch].Id = diCount++;
                                            for (int ch = 0; ch < _dio.OutChannelCount; ch++)
                                                _dio.OutChannels[ch].Id = doCount++;
                                        }
                                        break;
                                    case SlaveType.DO:
                                        {
                                            EcSlave_DO _do = slave as EcSlave_DO;
                                            for (int ch = 0; ch < _do.OutChannelCount; ch++)
                                                _do.OutChannels[ch].Id = doCount++;
                                        }
                                        break;
                                    case SlaveType.AI:
                                        {
                                            EcSlave_AI _ai = slave as EcSlave_AI;
                                            for (int ch = 0; ch < _ai.InChannelCount; ch++)
                                                _ai.InChannels[ch].Id = aiCount++;
                                        }
                                        break;
                                    case SlaveType.AIO:
                                        {
                                            EcSlave_AIO _aio = slave as EcSlave_AIO;
                                            for (int ch = 0; ch < _aio.InChannelCount; ch++)
                                                _aio.InChannels[ch].Id = aiCount++;
                                            for (int ch = 0; ch < _aio.OutChannelCount; ch++)
                                                _aio.OutChannels[ch].Id = aoCount++;
                                        }
                                        break;
                                    case SlaveType.AO:
                                        {
                                            EcSlave_AO _ao = slave as EcSlave_AO;
                                            for (int ch = 0; ch < _ao.OutChannelCount; ch++)
                                                _ao.OutChannels[ch].Id = aoCount++;
                                        }
                                        break;
                                    case SlaveType.AP:
                                        {
                                            EcSlave_AP _ap = slave as EcSlave_AP;
                                            for (int ch = 0; ch < _ap.ChannelCount; ch++)
                                                _ap.Channels[ch].Id = apCount++;
                                        }
                                        break;
                                }
                            }

                            //ioNode.UpdateIoCountByType();
                        }
                    }
                    break;
            }
        }

        public void UpdateWiringNo()
        {
            //jemoon : Bus별로 Mode가 다를 수 있기에 
            //m_WireNumberingMode = m_AppConfig.IoWireNumberingMode;

            switch (m_BusType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                case FieldBusType.BrModbusTcp:
                case FieldBusType.CrevisModbusTcp:
                case FieldBusType.MitsubishiCClink:
                    {
                        //jemoon : 수동편집모드 추가
                        if (m_WireNumberingMode == WireNumberingMode.Editable) return;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            IoNode ioNode = m_Container[nodeid];

                            int diCountByNode = 0;
                            int doCountByNode = 0;
                            int aiCountByNode = 0;
                            int aoCountByNode = 0;

                            int terminalCount = ioNode.Terminals.Count;
                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                IoTerminal terminal = ioNode.Terminals[terminalid];

                                int channelCount = terminal.ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    IoItem item = terminal.Channels[channelid];

                                    int ioIdByNode = 0;
                                    string wiringCategory = "";
                                    if (item.IoType == IoType.DI)
                                    {
                                        ioIdByNode = diCountByNode++;
                                        wiringCategory = "X";
                                    }
                                    else if (item.IoType == IoType.DO)
                                    {
                                        ioIdByNode = doCountByNode++;
                                        wiringCategory = "Y";
                                    }
                                    else if (item.IoType == IoType.AI)
                                    {
                                        ioIdByNode = aiCountByNode++;
                                        wiringCategory = "AI";
                                    }
                                    else if (item.IoType == IoType.AO)
                                    {
                                        ioIdByNode = aoCountByNode++;
                                        wiringCategory = "AO";
                                    }

                                    string wireNo = "";
                                    switch (m_WireNumberingMode)
                                    {
                                        case WireNumberingMode.ByOct:
                                            wireNo = string.Format("{0:d3}", Convert.ToInt32(Convert.ToString(ioIdByNode, 8)));
                                            item.WiringNo = (nodeid + 1).ToString() + wiringCategory + wireNo;
                                            break;
                                        case WireNumberingMode.ByDecimal:
                                            wireNo = string.Format("{0:d2}", item.Terminal + 1) + string.Format("{0:d2}", channelid + 1);
                                            item.WiringNo = (nodeid + 1).ToString() + wiringCategory + wireNo;
                                            break;
                                        case WireNumberingMode.OnlyCptG6Cline:
                                            wireNo = string.Format("{0:d2}", item.Terminal) + string.Format("{0:X}", channelid);
                                            item.WiringNo = (nodeid + 1).ToString() + wiringCategory + wireNo;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case FieldBusType.CrevisCClink:
                    {
                        //jemoon : 수동편집모드 추가
                        if (m_WireNumberingMode == WireNumberingMode.Editable) return;

                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            IoNode ioNode = m_Container[nodeid];

                            int diCountByNode = 0;
                            int doCountByNode = 0;
                            int aiCountByNode = 0;
                            int aoCountByNode = 0;

                            int terminalCount = ioNode.Terminals.Count;
                            int nodeNo = 0;


                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                CClinkStation terminal = ioNode.Terminals[terminalid] as CClinkStation;


                                if (terminal.PartName.StartsWith("AT") || terminal.PartName.StartsWith("NA"))
                                {
                                    nodeNo += 1;
                                    diCountByNode = 0;
                                    doCountByNode = 0;
                                    aiCountByNode = 0;
                                    aoCountByNode = 0;
                                }


                                string wiringCategory = "";

                                //if (terminal.IoType == IoType.DI) ;
                                //else if (terminal.IoType == IoType.DO) wiringCategory = "Y";
                                //else if (terminal.IoType == IoType.AI) wiringCategory = "AI";
                                //else if (terminal.IoType == IoType.AO) wiringCategory = "AO";

                                int channelCount = terminal.ChannelCount;



                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    IoItem item = terminal.Channels[channelid];

                                    int ioIdByNode = 0;




                                    if (item.IoType == IoType.DI)
                                    {
                                        wiringCategory = "X";
                                        ioIdByNode = diCountByNode++;
                                    }
                                    else if (item.IoType == IoType.DO)
                                    {
                                        wiringCategory = "Y";
                                        ioIdByNode = doCountByNode++;
                                    }
                                    else if (item.IoType == IoType.AI)
                                    {
                                        wiringCategory = "AI";
                                        ioIdByNode = aiCountByNode++;
                                    }
                                    else if (item.IoType == IoType.AO)
                                    {
                                        wiringCategory = "AO";
                                        ioIdByNode = aoCountByNode++;
                                    }

                                    string wireNo = "";
                                    switch (m_WireNumberingMode)
                                    {
                                        case WireNumberingMode.ByOct:
                                            wireNo = string.Format("{0:d3}", Convert.ToInt32(Convert.ToString(ioIdByNode, 8)));
                                            item.WiringNo = (nodeNo).ToString() + wiringCategory + wireNo;
                                            break;
                                        case WireNumberingMode.ByDecimal:
                                            wireNo = string.Format("{0:d2}", item.Terminal + 1) + string.Format("{0:d2}", channelid + 1);
                                            item.WiringNo = (nodeNo).ToString() + wiringCategory + wireNo;
                                            break;
                                        case WireNumberingMode.OnlyCptG6Cline:
                                            wireNo = string.Format("{0:d2}", item.Terminal) + string.Format("{0:X}", channelid);
                                            item.WiringNo = (nodeNo).ToString() + wiringCategory + wireNo;
                                            break;
                                    }

                                }
                            }
                        }
                    }
                    break;
                case FieldBusType.MitsubishiMelsecNet:
                    {
                        int nodeCount = m_Container.Count;
                        for (int nodeid = 0; nodeid < nodeCount; nodeid++)
                        {
                            IoNode ioNode = m_Container[nodeid];

                            int terminalCount = ioNode.Terminals.Count;
                            for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                            {
                                MelsecTerminal terminal = (MelsecTerminal)ioNode.Terminals[terminalid];

                                int channelCount = terminal.ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    IoItem item = terminal.Channels[channelid];
                                    string wiringCategory = MelsecTerminal.GetDevTypeCategoryName(terminal.DeviceType);

                                    //if (terminal.IoType == IoType.DI) wiringCategory = "B";
                                    //else if (terminal.IoType == IoType.DO) wiringCategory = "B";
                                    //else if (terminal.IoType == IoType.AI) wiringCategory = "W";
                                    //else if (terminal.IoType == IoType.AO) wiringCategory = "W";

                                    string wireNo = "";
                                    MelsecTerminal.AddressingType addressType = MelsecTerminal.GetAddressingType(terminal.DeviceType);
                                    if (addressType == MelsecTerminal.AddressingType.HEX)
                                    {
                                        wireNo = string.Format("{0:X4}", terminal.OffsetDec + item.Channel);
                                    }
                                    else
                                    {
                                        wireNo = string.Format("{0:D4}", terminal.OffsetDec + item.Channel);
                                    }

                                    item.WiringNo = wiringCategory + wireNo;
                                }
                            }
                        }
                    }
                    break;
                case FieldBusType.MitsubishiMelsecEtherNet:
                    {
                        MelsecDeviceInfoProvider provider = new MelsecDeviceInfoProvider();
                        provider.ReadFromStorage();

                        int count = m_DigitalInputs.Count;
                        string wiringCategory = "";
                        string wireNo = "";
                        MelsecTerminal.AddressingType addressType;
                        IoItem ioItem;

                        //Digital Inputs
                        List<MelsecDeviceInfo> items = provider.DiAddressMap.Items;
                        for (int i = 0; i < count; i++)
                        {
                            wiringCategory = MelsecTerminal.GetDevTypeCategoryName(items[i].DeviceType);
                            addressType = MelsecTerminal.GetAddressingType(items[i].DeviceType);
                            if (addressType == MelsecTerminal.AddressingType.HEX)
                            {
                                wireNo = items[i].AddressHex;
                            }
                            else
                            {
                                wireNo = string.Format("{0:D4}", items[i].AddressDec);
                            }

                            ioItem = m_DigitalInputs[i];
                            ioItem.WiringNo = wiringCategory + wireNo;
                        }

                        //Digital Outputs
                        count = m_DigitalOutputs.Count;
                        items = provider.DoAddressMap.Items;
                        for (int i = 0; i < count; i++)
                        {
                            wiringCategory = MelsecTerminal.GetDevTypeCategoryName(items[i].DeviceType);
                            addressType = MelsecTerminal.GetAddressingType(items[i].DeviceType);
                            if (addressType == MelsecTerminal.AddressingType.HEX)
                            {
                                wireNo = items[i].AddressHex;
                            }
                            else
                            {
                                wireNo = string.Format("{0:D4}", items[i].AddressDec);
                            }

                            ioItem = m_DigitalOutputs[i];
                            ioItem.WiringNo = wiringCategory + wireNo;
                        }

                        //Analog Inputs
                        count = m_AnalogInputs.Count;
                        items = provider.AiAddressMap.Items;
                        for (int i = 0; i < count; i++)
                        {
                            wiringCategory = MelsecTerminal.GetDevTypeCategoryName(items[i].DeviceType);
                            addressType = MelsecTerminal.GetAddressingType(items[i].DeviceType);
                            if (addressType == MelsecTerminal.AddressingType.HEX)
                            {
                                wireNo = items[i].AddressHex;
                            }
                            else
                            {
                                wireNo = string.Format("{0:D4}", items[i].AddressDec);
                            }

                            ioItem = m_AnalogInputs[i];
                            ioItem.WiringNo = wiringCategory + wireNo;
                        }

                        //Analog Outputs
                        count = m_AnalogOutputs.Count;
                        items = provider.AoAddressMap.Items;
                        for (int i = 0; i < count; i++)
                        {
                            wiringCategory = MelsecTerminal.GetDevTypeCategoryName(items[i].DeviceType);
                            addressType = MelsecTerminal.GetAddressingType(items[i].DeviceType);
                            if (addressType == MelsecTerminal.AddressingType.HEX)
                            {
                                wireNo = items[i].AddressHex;
                            }
                            else
                            {
                                wireNo = string.Format("{0:D4}", items[i].AddressDec);
                            }

                            ioItem = m_AnalogOutputs[i];
                            ioItem.WiringNo = wiringCategory + wireNo;
                        }
                    }
                    break;
            }
        }

        public static List<IoDefines> CreateIoDefineList()
        {
            List<IoDefines> ioDefineList = new List<IoDefines>();
            Array busTypes = Enum.GetValues(typeof(FieldBusType));
            foreach (object obj in busTypes)
            {
                IoDefines iodefines = new IoDefines((FieldBusType)obj);
                ioDefineList.Add(iodefines);
            }

            return ioDefineList;
        }

        public static List<IoDefines> ReadIodefineList()
        {
            List<IoDefines> ioDefineList = CreateIoDefineList();
            foreach (IoDefines iodefines in ioDefineList)
            {
                iodefines.ReadXml();
            }

            return ioDefineList;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.GetType().Name;
        }
        #endregion
    }
}
