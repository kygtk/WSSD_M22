///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : PooDevice : partial of ServerManager Class
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.27 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Dms.Ctl;
using Dms.Common;
using Dms.Util.IODefine;
using Dms.Device;

namespace Dms.Server
{
    partial class ServerManager
    {
        #region Device Controller
        private Controllers m_IoControllers = null;
        private ICtlDevice m_IoController;
        private ICtlDevice m_EcController;
        private Mmc m_MotionController = null;
        //private RootNode m_RootNode = null; // 10.12.21 minhan
        //private FtpClient m_FtpClient = null;
        //private MP2300Ctl m_Mp2300Controller = null;
        #endregion

        #region I/O Devices
        private IoCollection<IoDigitalInput> m_DigitalInputs = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalOutput> m_DigitalOutputs = new IoCollection<IoDigitalOutput>();
        private IoCollection<IoAnalogInput> m_AnalogInputs = new IoCollection<IoAnalogInput>();
        private IoCollection<IoAnalogOutput> m_AnalogOutputs = new IoCollection<IoAnalogOutput>();

        private SlaveCollection<SlaveServo> m_SlaveServos = new SlaveCollection<SlaveServo>();
        private SlaveCollection<SlaveBLDC> m_SlaveBLDCs = new SlaveCollection<SlaveBLDC>();
        private SlaveCollection<SlaveInverter> m_SlaveInverters = new SlaveCollection<SlaveInverter>();
        private SlaveCollection<SlaveDigitalInput> m_SlaveDigitalInputs = new SlaveCollection<SlaveDigitalInput>();
        private SlaveCollection<SlaveDigitalOutput> m_SlaveDigitalOutputs = new SlaveCollection<SlaveDigitalOutput>();
        private SlaveCollection<SlaveAnalogInput> m_SlaveAnalogInputs = new SlaveCollection<SlaveAnalogInput>();
        private SlaveCollection<SlaveAnalogOutput> m_SlaveAnalogOutputs = new SlaveCollection<SlaveAnalogOutput>();
        private SlaveCollection<SlaveAP> m_SlaveAPs = new SlaveCollection<SlaveAP>();
        #endregion

        #region Properties
        public ICtlDevice IoController
        {
            get { return m_IoController; }
        }
        public ICtlDevice EcController
        {
            get { return m_EcController; }
        }
        public bool ControllerIsRun
        {
            get
            {
                return (m_IoController.DeviceState == ActiveState.Run) ||
                       (m_EcController.DeviceState == ActiveState.Run);
            }
        }
        public IMotionControl MotionController
        {
            get { return m_MotionController; }
        }
        //public MP2300Ctl Mp2300Controller
        //{
        //    get { return m_Mp2300Controller; }
        //}
        public IoCollection<IoDigitalInput> DigitalInputs
        {
            get { return m_DigitalInputs; }
            set { m_DigitalInputs = value; }
        }
        public IoCollection<IoDigitalOutput> DigitalOutputs
        {
            get { return m_DigitalOutputs; }
            set { m_DigitalOutputs = value; }
        }
        public IoCollection<IoAnalogInput> AnalogInputs
        {
            get { return m_AnalogInputs; }
            set { m_AnalogInputs = value; }
        }
        public IoCollection<IoAnalogOutput> AnalogOutputs
        {
            get { return m_AnalogOutputs; }
            set { m_AnalogOutputs = value; }
        }
        public SlaveCollection<SlaveServo> SlaveServos
        {
            get { return m_SlaveServos; }
            set { m_SlaveServos = value; }
        }
        public SlaveCollection<SlaveBLDC> SlaveBLDCs
        {
            get { return m_SlaveBLDCs; }
            set { m_SlaveBLDCs = value; }
        }
        public SlaveCollection<SlaveInverter> SlaveInverters
        {
            get { return m_SlaveInverters; }
            set { m_SlaveInverters = value; }
        }
        public SlaveCollection<SlaveDigitalInput> SlaveDigitalInputs
        {
            get { return m_SlaveDigitalInputs; }
            set { m_SlaveDigitalInputs = value; }
        }
        public SlaveCollection<SlaveDigitalOutput> SlaveDigitalOutputs
        {
            get { return m_SlaveDigitalOutputs; }
            set { m_SlaveDigitalOutputs = value; }
        }
        public SlaveCollection<SlaveAnalogInput> SlaveAnalogInputs
        {
            get { return m_SlaveAnalogInputs; }
            set { m_SlaveAnalogInputs = value; }
        }
        public SlaveCollection<SlaveAnalogOutput> SlaveAnalogOutputs
        {
            get { return m_SlaveAnalogOutputs; }
            set { m_SlaveAnalogOutputs = value; }
        }
        public SlaveCollection<SlaveAP> SlaveAPs
        {
            get { return m_SlaveAPs; }
            set { m_SlaveAPs = value; }
        }

        //public RootNode RootNode // 10.12.21 minhan
        //{
        //    get { return m_RootNode; }
        //}
        #endregion

        #region Methods
        private DmsErrors InitializeServerDevice()
        {
            //jemoon 
            //Initialize 되는 순서데로 UninitializeDelegate에 추가해서
            //Uninitialize 될때 자동으로 역순 Uninitialize 한다.

            try
            {
                bool ok = true;

                #region Initialize IO Controller
                if (AppConfig.Instance.Simul.Device)
                {
                    MessageBox.Show("System run in Device Simulation mode!!");
                }
                if (AppConfig.Instance.Simul.IoMapping)
                {
                    MessageBox.Show("System run in IO Mapping Simulation mode!!");
                }

                if (ok)
                {
                    m_IoControllers = Controllers.Instance;
                    m_IoControllers.Initialize(this);
                    ok &= m_IoControllers.CreateControllers() == DmsErrors.Success;
                    m_IoController = m_IoControllers.GetMainIoController();
                    m_EcController = m_IoControllers.GetMainEcController();
                }
                #endregion

                #region Initialize MMC
                if (ok)
                {
                    m_MotionController = new Mmc(1, 8); // 카드는 8축인데... 10.12.21 minhan
                    ok &= m_MotionController.Initialize() == 0;
                    this.UninitializeDel += new UninitializeDelegate(m_MotionController.Uninitialize);
                }
                //if (ok)
                //{
                //    m_Mp2300Controller = MP2300Ctl.Instance;
                //    ok &= m_Mp2300Controller.Initialize() == 0;
                //    this.UninitializeDel += new UninitializeDelegate(m_Mp2300Controller.UnInitialize);
                //}
                #endregion

                #region Initialize RootNode for Melsec Net
                //if (ok)
                //{
                //    m_RootNode = new RootNode();
                //    m_RootNode.MelsecBoardControl.SetCreateMode(this.Simul.Melsec);
                //    m_RootNode.MelsecBoardControl.SetSimuateAddressMode(this.Simul.Melsec);
                //    ok &= m_RootNode.AddNode(null, 0, "RootNode").Initialize();
                //    this.UninitializeDel += new UninitializeDelegate(m_RootNode.Uninitialize);
                //}
                #endregion

                #region Initialize Melsec Interface device
                if (ok)
                {
                    ok &= this.InitializeServerInterface();
                    this.UninitializeDel += new UninitializeDelegate(UninitializeServerInterface);

                    ////ECS : HSMS // 10.12.21 minhan
                    //ok &= this.InitializeServerHsms();
                    //this.UninitializeDel += new UninitializeDelegate(UninitializeServerHsms);

                    ////ECS : MELSEC
                    ////ok &= this.InitializeServerEcsMelsec();
                    ////this.UninitializeDel += new UninitializeDelegate(UninitializeServerEcsMelsec);

                    //ok &= this.InitializeServerFtp();
                    //this.UninitializeDel += new UninitializeDelegate(UninitializeServerFtp);
                }
                #endregion

                #region Initialize FtpClient
                //if (ok) // 10.12.21 minhan
                //{
                //    m_FtpClient = new FtpClient(50, this);
                //    m_FtpClient.Start();
                //}
                #endregion

                return ok ? DmsErrors.Success : DmsErrors.InternalError;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                WriteExceptionLog(msg);
                MessageBox.Show(msg);
                //Application.Exit();            

                return DmsErrors.InternalError;
            }
        }
        #endregion
    }
}
