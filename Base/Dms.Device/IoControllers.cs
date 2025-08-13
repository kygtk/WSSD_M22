using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Util.IODefine;
using Dms.Ctl;
using System.Windows.Forms;

namespace Dms.Device
{
    public class Controllers
    {
        #region Singleton Code...
        public static readonly Controllers Instance = new Controllers();
        #endregion

        #region Fields
        private IServer m_ServerManager = null;
        private List<IoDefines> m_IoDefineList = new List<IoDefines>();
        private List<ICtlDevice> m_IoControllers = new List<ICtlDevice>();
        private List<ICtlDevice> m_EcControllers = new List<ICtlDevice>();
        private bool m_Initialized = false;
        #endregion

        #region Properties
        public List<IoDefines> IoDefineList
        {
            get { return m_IoDefineList; }
        }
        public List<ICtlDevice> IoControllers
        {
            get { return m_IoControllers; }
        }
        public List<ICtlDevice> EcControllers
        {
            get { return m_EcControllers; }
        }
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        #endregion

        #region Constructor
        private Controllers()
        {
        }
        #endregion

        public DmsErrors Initialize(IServer serverManager)
        {
            m_ServerManager = serverManager;
            AppConfig appConfig = AppConfig.Instance;
            if (appConfig.Simul.IoMapping == false)
            {
                m_IoDefineList = IoDefines.ReadIodefineList();
            }
            else
            {
                //jemoon : io mapping simulaion이면 모든 i/o를 ethercat으로 잡는다.
                m_IoDefineList = IoDefines.CreateIoDefineList();
                m_IoDefineList[0] = m_ServerManager.IoDefines;
            }

            m_Initialized = true;

            return DmsErrors.Success;
        }

        #region Methods
        public DmsErrors CreateControllers()
        {
            DmsErrors IoResult = CreateIoControllers(m_IoDefineList);
            DmsErrors EcResult = CreateEcControllers(m_IoDefineList);

            if (IoResult == DmsErrors.Success && EcResult == DmsErrors.Success)
                return DmsErrors.Success;
            else
                return DmsErrors.InternalError;
        }

        private DmsErrors CreateIoControllers(List<IoDefines> ioList)
        {
            bool ok = true;

            foreach (IoDefines iodefine in ioList)
            {
                if (!iodefine.IsDefined && !AppConfig.Instance.Simul.IoMapping)
                {
                    m_IoControllers.Add(null);
                }
                else
                {
                    switch (iodefine.BusType)
                    {
                        case FieldBusType.BeckhoffEtherCAT:
                            {
                                EtherCAT controller = EtherCAT.Instance;
                                m_IoControllers.Add(controller);
                                ok &= controller.Initialize(IoUpdateMode.Polling, 301, 302) == DmsErrors.Success;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.BrModbusTcp:
                            {
                                BrModbusTcpIo controller = BrModbusTcpIo.Instance;
                                m_IoControllers.Add(controller);
                                controller.WatchUpdateCycle = 10;
                                controller.WatchUpdateMode = WatchMode.Thread;
                                ok &= controller.Initialize(iodefine) == DmsErrors.Success;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.MitsubishiCClink:
                            {
                                MelsecCCLink controller = MelsecCCLink.InstanceM;

                                m_IoControllers.Add(controller);
                                controller.WatchUpdateCycle = 10;
                                controller.WatchUpdateMode = WatchMode.Thread;
                                ok &= controller.Initialize(iodefine) == DmsErrors.Success;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.CrevisCClink:
                            {
                                MelsecCCLink controller = MelsecCCLink.InstanceC;
                                controller.ModuleConfig.BusType = FieldBusType.CrevisCClink;
                                m_IoControllers.Add(controller);
                                controller.WatchUpdateCycle = 10;
                                controller.WatchUpdateMode = WatchMode.Thread;
                                ok &= controller.Initialize(iodefine) == DmsErrors.Success;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.MitsubishiMelsecNet:
                            {
                                MelsecNet controller = MelsecNet.Instance;
                                m_IoControllers.Add(controller);
                                controller.WatchUpdateCycle = 10;
                                controller.WatchUpdateMode = WatchMode.Thread;
                                ok &= controller.Initialize(iodefine) == DmsErrors.Success;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.MitsubishiMelsecEtherNet:
                            {
                                MelsecEtherNet controller = MelsecEtherNet.Instance;
                                m_IoControllers.Add(controller);
                                controller.WatchUpdateCycle = 10;
                                controller.WatchUpdateMode = WatchMode.Thread;
                                ok &= controller.Initialize(iodefine) == DmsErrors.Success;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.TwinCATPlc:
                            {
                                TwinCATPlc controller = TwinCATPlc.Instance;
                                m_IoControllers.Add(controller);
                                ok &= controller.CreateMasterInfo(iodefine);
                                ok &= controller.Initialize(IoUpdateMode.Polling, 801) == DmsErrors.Success;
                                //controller.DiCount = iodefine.DigitalInputs.Count;
                                //controller.DoCount = iodefine.DigitalOutputs.Count;
                                //controller.AiCount = iodefine.AnalogInputs.Count;
                                //controller.AoCount = iodefine.AnalogOutputs.Count;
                                ok &= CompareIoSize(controller, iodefine);
                                ok &= InitializeIO(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                        case FieldBusType.MovensysEtherCAT:
                            {
                                m_IoControllers.Add(null);
                            }
                            break;
                    }

                    if (AppConfig.Instance.Simul.IoMapping) break; // io mapping simulation mode 이면 하나만 생성한다.
                }
            }

            return ok ? DmsErrors.Success : DmsErrors.InternalError;
        }

        private DmsErrors CreateEcControllers(List<IoDefines> ioList)
        {
            bool ok = true;

            foreach (IoDefines iodefine in ioList)
            {
                if (!iodefine.IsDefined && !AppConfig.Instance.Simul.IoMapping)
                {
                    m_EcControllers.Add(null);
                }
                else
                {
                    switch (iodefine.BusType)
                    {
                        case FieldBusType.BeckhoffEtherCAT:
                        case FieldBusType.BrModbusTcp:
                        case FieldBusType.MitsubishiCClink:
                        case FieldBusType.CrevisCClink:
                        case FieldBusType.MitsubishiMelsecNet:
                        case FieldBusType.MitsubishiMelsecEtherNet:
                        case FieldBusType.TwinCATPlc:
                            m_EcControllers.Add(null);
                            break;
                        case FieldBusType.MovensysEtherCAT:
                            {
                                WMX3 controller = WMX3.Instance;
                                m_EcControllers.Add(controller);
                                ok &= controller.Initialize(iodefine) == DmsErrors.Success;
                                //ok &= CompareSlaveSize(controller, iodefine);
                                ok &= InitializeSlave(controller, iodefine);
                                m_ServerManager.UninitializeDel += new UninitializeDelegate(controller.Uninitialize);
                            }
                            break;
                    }

                    if (AppConfig.Instance.Simul.IoMapping) break; // io mapping simulation mode 이면 하나만 생성한다.
                }
            }

            return ok ? DmsErrors.Success : DmsErrors.InternalError;
        }

        public ICtlDevice GetMainIoController()
        {
            ICtlDevice iocontroller = null;
            foreach (ICtlDevice controller in m_IoControllers)
            {
                if (controller != null && controller.GetType().Name != "WMX3")
                {
                    iocontroller = controller;
                    break;
                }
            }

            return iocontroller;
        }

        public ICtlDevice GetMainEcController()
        {
            ICtlDevice eccontroller = null;
            foreach (ICtlDevice controller in m_EcControllers)
            {
                if (controller != null && controller.GetType().Name == "WMX3")
                {
                    eccontroller = controller;
                    break;
                }
            }

            return eccontroller;
        }

        public ICtlDevice GetController(FieldBusType busType)
        {
            ICtlDevice controller = null;
            switch (busType)
            {
                case FieldBusType.BeckhoffEtherCAT:
                    {
                        controller = EtherCAT.Instance;
                    }
                    break;
                case FieldBusType.BrModbusTcp:
                    {
                        controller = BrModbusTcpIo.Instance;
                    }
                    break;
                case FieldBusType.MitsubishiCClink:
                    {
                        controller = MelsecCCLink.InstanceM;
                    }
                    break;
                case FieldBusType.CrevisCClink:
                    {
                        controller = MelsecCCLink.InstanceC;
                    }
                    break;
                case FieldBusType.MitsubishiMelsecNet:
                    {
                        controller = MelsecNet.Instance;
                    }
                    break;
                case FieldBusType.MitsubishiMelsecEtherNet:
                    {
                        controller = MelsecEtherNet.Instance;
                    }
                    break;
            }

            return controller;
        }

        private bool CompareIoSize(ICtlDevice controller, IoDefines iodefine)
        {
            //jemoon : Io size를 Check
            bool ok = true;
            //if (AppConfig.Instance.Simul.IoMapping)
            {
                ok &= (controller.DiCount >= iodefine.DigitalInputs.Count);
                ok &= (controller.DoCount >= iodefine.DigitalOutputs.Count);
                ok &= (controller.AiCount >= iodefine.AnalogInputs.Count);
                ok &= (controller.AoCount >= iodefine.AnalogOutputs.Count);
            }
            //else
            //{
            //    ok &= (controller.DiCount == iodefine.DigitalInputs.Count);
            //    ok &= (controller.DoCount == iodefine.DigitalOutputs.Count);
            //    ok &= (controller.AiCount == iodefine.AnalogInputs.Count);
            //    ok &= (controller.AoCount == iodefine.AnalogOutputs.Count);
            //}

            if (!ok)
            {
                MessageBox.Show("Can not Start Program : I/O size is not match - " + controller.GetType().Name);
            }

            return ok;
        }

        private bool CompareSlaveSize(ICtlDevice controller, IoDefines iodefine)
        {
            bool ok = true;

            ok &= controller.DiCount >= iodefine.SlaveDigitalInputs.Count;
            ok &= controller.DoCount >= iodefine.SlaveDigitalOutputs.Count;
            ok &= controller.AiCount >= iodefine.SlaveAnalogInputs.Count;
            ok &= controller.AoCount >= iodefine.SlaveAnalogOutputs.Count;

            if (controller.IsAdvDevice)
            {
                ICtlDevice_Adv controller_adv = controller as ICtlDevice_Adv;
                ok &= controller_adv.InverterCount >= iodefine.SlaveInverters.Count;
                ok &= controller_adv.ServoCount >= iodefine.SlaveServos.Count;
                ok &= controller_adv.BldcCount >= iodefine.SlaveBLDCs.Count;
                ok &= controller_adv.ApCount >= iodefine.SlaveAPs.Count;
            }

            if (!ok)
            {
                MessageBox.Show("Can not Start Program : Slave size is not match - " + controller.GetType().Name);
            }

            return ok;
        }

        private int m_ServoIndex = 0;
        private int m_BLDCIndex = 0;
        private int m_InverterIndex = 0;
        private int m_DiIndex = 0;
        private int m_DoIndex = 0;
        private int m_AiIndex = 0;
        private int m_AoIndex = 0;
        private int m_ApIndex = 0;
        private bool InitializeIO(ICtlDevice controller, IoDefines ioDefines)
        {
            //
            // Digital Input
            //
            int count = controller.DiCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    IoDigitalInput io = new IoDigitalInput(m_DiIndex++, controller);
                    m_ServerManager.DigitalInputs.Add(io);
                }
                else if (i < ioDefines.DigitalInputs.Count)
                {
                    IoItemDI ioItem = ioDefines.DigitalInputs[i] as IoItemDI;
                    IoDigitalInput io = new IoDigitalInput(ioItem, controller);
                    m_ServerManager.DigitalInputs.Add(io);
                }
            }
            //
            // Digital Output
            //
            count = controller.DoCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    IoDigitalOutput io = new IoDigitalOutput(m_DoIndex++, controller);
                    m_ServerManager.DigitalOutputs.Add(io);
                }
                else if (i < ioDefines.DigitalOutputs.Count)
                {
                    IoItem ioItem = ioDefines.DigitalOutputs[i];
                    IoDigitalOutput io = new IoDigitalOutput(ioItem, controller);
                    m_ServerManager.DigitalOutputs.Add(io);
                }
            }
            //
            // Analog Input
            //
            count = controller.AiCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    IoAnalogInput io = new IoAnalogInput(m_AiIndex++, controller);
                    m_ServerManager.AnalogInputs.Add(io);
                }
                else if (i < ioDefines.AnalogInputs.Count)
                {
                    IoItem ioItem = ioDefines.AnalogInputs[i];
                    IoAnalogInput io = new IoAnalogInput(ioItem, controller);
                    m_ServerManager.AnalogInputs.Add(io);
                }
            }
            //
            // Analog Output
            //
            count = controller.AoCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    IoAnalogOutput io = new IoAnalogOutput(m_AoIndex++, controller);
                    m_ServerManager.AnalogOutputs.Add(io);
                }
                else if (i < ioDefines.AnalogOutputs.Count)
                {
                    IoItem ioItem = ioDefines.AnalogOutputs[i];
                    IoAnalogOutput io = new IoAnalogOutput(ioItem, controller);
                    m_ServerManager.AnalogOutputs.Add(io);
                }
            }

            return true;
        }

        private bool InitializeSlave(ICtlDevice controller, IoDefines ioDefines)
        {
            int count;

            if (controller.IsAdvDevice)
            {
                ICtlDevice_Adv controller_adv = controller as ICtlDevice_Adv;

                //
                // Servo
                //
                count = controller_adv.ServoCount;
                for (int i = 0; i < count; i++)
                {
                    if (AppConfig.Instance.Simul.IoMapping)
                    {
                        SlaveServo slave = new SlaveServo(m_ServoIndex++, controller);
                        m_ServerManager.SlaveServos.Add(slave);
                    }
                    else if (i < ioDefines.SlaveServos.Count)
                    {
                        EcSlaveItem_Servo slaveitem_servo = ioDefines.SlaveServos[i];
                        SlaveServo slave = new SlaveServo(slaveitem_servo, controller);
                        m_ServerManager.SlaveServos.Add(slave);
                    }
                }
                //
                // BLDC
                //
                count = controller_adv.BldcCount;
                for (int i = 0; i < count; i++)
                {
                    if (AppConfig.Instance.Simul.IoMapping)
                    {
                        SlaveBLDC slave = new SlaveBLDC(m_BLDCIndex++, controller);
                        m_ServerManager.SlaveBLDCs.Add(slave);
                    }
                    else if (i < ioDefines.SlaveBLDCs.Count)
                    {
                        EcSlaveItem_BLDC slaveitem_bldc = ioDefines.SlaveBLDCs[i];
                        SlaveBLDC slave = new SlaveBLDC(slaveitem_bldc, controller);
                        m_ServerManager.SlaveBLDCs.Add(slave);
                    }
                }
                //
                // Inverter
                //
                count = controller_adv.InverterCount;
                for (int i = 0; i < count; i++)
                {
                    if (AppConfig.Instance.Simul.IoMapping)
                    {
                        SlaveInverter slave = new SlaveInverter(m_InverterIndex++, controller);
                        m_ServerManager.SlaveInverters.Add(slave);
                    }
                    else if (i < ioDefines.SlaveInverters.Count)
                    {
                        EcSlaveItem_Inverter slaveitem_inverter = ioDefines.SlaveInverters[i];
                        SlaveInverter slave = new SlaveInverter(slaveitem_inverter, controller);
                        m_ServerManager.SlaveInverters.Add(slave);
                    }
                }
                //
                // AP
                //
                count = controller_adv.ApCount;
                for (int i = 0; i < count; i++)
                {
                    if (AppConfig.Instance.Simul.IoMapping)
                    {
                        SlaveAP slave = new SlaveAP(m_ApIndex++, controller);
                        m_ServerManager.SlaveAPs.Add(slave);
                    }
                    else if (i < ioDefines.SlaveAPs.Count)
                    {
                        EcSlaveItem_AP slaveitem_ap = ioDefines.SlaveAPs[i];
                        SlaveAP slave = new SlaveAP(slaveitem_ap, controller);
                        m_ServerManager.SlaveAPs.Add(slave);
                    }
                }
            }
            //
            // Digital Input
            //
            count = controller.DiCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    SlaveDigitalInput slave = new SlaveDigitalInput(m_DiIndex++, controller);
                    m_ServerManager.SlaveDigitalInputs.Add(slave);
                }
                else if (i < ioDefines.SlaveDigitalInputs.Count)
                {
                    EcSlaveItem_DI slaveitem_di = ioDefines.SlaveDigitalInputs[i];
                    SlaveDigitalInput slave = new SlaveDigitalInput(slaveitem_di, controller);
                    m_ServerManager.SlaveDigitalInputs.Add(slave);
                }
            }
            //
            // Digital Output
            //
            count = controller.DoCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    SlaveDigitalOutput slave = new SlaveDigitalOutput(m_DoIndex++, controller);
                    m_ServerManager.SlaveDigitalOutputs.Add(slave);
                }
                else if (i < ioDefines.SlaveDigitalOutputs.Count)
                {
                    EcSlaveItem_DO slaveitem_do = ioDefines.SlaveDigitalOutputs[i];
                    SlaveDigitalOutput slave = new SlaveDigitalOutput(slaveitem_do, controller);
                    m_ServerManager.SlaveDigitalOutputs.Add(slave);
                }
            }
            //
            // Analog Input
            //
            count = controller.AiCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    SlaveAnalogInput slave = new SlaveAnalogInput(m_AiIndex++, controller);
                    m_ServerManager.SlaveAnalogInputs.Add(slave);
                }
                else if (i < ioDefines.SlaveAnalogInputs.Count)
                {
                    EcSlaveItem_AI slaveitem_ai = ioDefines.SlaveAnalogInputs[i];
                    SlaveAnalogInput slave = new SlaveAnalogInput(slaveitem_ai, controller);
                    m_ServerManager.SlaveAnalogInputs.Add(slave);
                }
            }
            //
            // Analog Output
            //
            count = controller.AoCount;
            for (int i = 0; i < count; i++)
            {
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    SlaveAnalogOutput slave = new SlaveAnalogOutput(m_AoIndex++, controller);
                    m_ServerManager.SlaveAnalogOutputs.Add(slave);
                }
                else if (i < ioDefines.SlaveAnalogOutputs.Count)
                {
                    EcSlaveItem_AO slaveitem_ao = ioDefines.SlaveAnalogOutputs[i];
                    SlaveAnalogOutput slave = new SlaveAnalogOutput(slaveitem_ao, controller);
                    m_ServerManager.SlaveAnalogOutputs.Add(slave);
                }
            }

            return true;
        }
        #endregion
    }
}
