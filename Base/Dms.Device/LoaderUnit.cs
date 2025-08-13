using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class LoaderUnit : _DeviceAsm, ILoaderSlave
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private LampSwitch m_LampSwitchSetup = new LampSwitch();
        [Category("DMS : Relation")]
        public LampSwitch LampSwitchSetup
        {
            get { return m_LampSwitchSetup; }
            set { m_LampSwitchSetup = value; }
        }
        private LampSwitch m_LampSwitchStart = new LampSwitch();
        [Category("DMS : Relation")]
        public LampSwitch LampSwitchStart
        {
            get { return m_LampSwitchStart; }
            set { m_LampSwitchStart = value; }
        }
        private LampSwitch m_LampSwitchStop = new LampSwitch();
        [Category("DMS : Relation")]
        public LampSwitch LampSwitchStop
        {
            get { return m_LampSwitchStop; }
            set { m_LampSwitchStop = value; }
        }
        private DoorSensor m_LoaderDoorSensor;
        [Category("DMS : Relation")]
        public DoorSensor LoaderDoorSensor
        {
            get { return m_LoaderDoorSensor; }
            set { m_LoaderDoorSensor = value; }
        }
        private _GenericCollection<PortUnitA> m_Ports = new _GenericCollection<PortUnitA>();
        [Category("DMS : Relation")]
        public _GenericCollection<PortUnitA> Ports
        {
            get { return m_Ports; }
            set { m_Ports = value; }
        }
        private YaskawaNX100 m_Robot = new YaskawaNX100();
        [Category("DMS : Relation")]
        public YaskawaNX100 Robot
        {
            get { return m_Robot; }
            set { m_Robot = value; }
        }

        private LoaderStatus m_Status;
        [Browsable(false), XmlIgnore()]
        public LoaderStatus Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        private LoaderEqpState m_EqpState;
        [Browsable(false), XmlIgnore()]
        public LoaderEqpState EqpState
        {
            get { return m_EqpState; }
            set { m_EqpState = value; }
        }

        private Queue m_QueueLotStart = new Queue();
        [Browsable(false), XmlIgnore()]
        public Queue QueueLotStart
        {
            get { return m_QueueLotStart; }
            set { m_QueueLotStart = value; }
        }

        private LoaderMode m_Mode = LoaderMode.Auto;
        [Browsable(false), XmlIgnore()]
        public LoaderMode Mode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        private HostControlMode m_HostControlMode = HostControlMode.Offline;
        private LoaderControlMode m_LoaderControlMode = LoaderControlMode.Offline;
        private bool m_OpCallState = false;
        #endregion

        #region Constructor
        public LoaderUnit()
        {
            this.Name = "__ Loader";
        }
        #endregion

        #region Methods
        public int GetFirstWaitingPort()
        {
            if (m_QueueLotStart.Count > 0)
                return (int)m_QueueLotStart.Peek();
            return -1;
        }
        #endregion

        #region Override
        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {

        }

        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                    m_Robot.OnRobotTransferEvent += new RobotTransferEventHandler(OnRobotTransferEvent);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override Type FamilyType
        {
            get
            {
                return this.GetType();
            }
        }
        #endregion

        #region ILoaderSlave 멤버
        public string GetRecipeId(int portId, int slotId)
        {
            return m_Ports[portId].Cst.GetRecipeId(slotId);
        }

        public LoaderCommandReply SetGlassRecipeId(int portId, string[] recipe)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            int slotCount = m_Ports[portId].Cst.SlotCount;
            if (portId >= m_Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }
            if (recipe.Length != slotCount)
            {
                text = "BUG!! Slot No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            for (int i = 0; i < slotCount; i++)
            {
                m_Ports[portId].Cst.SetRecipeId(i, recipe[i]);
            }

            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply AbortConfirm(int portNo)
        {
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply SetPortEnable(int portId, PortUsage use)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= m_Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].PortEnable = use;

            return LoaderCommandReply.Ack;
        }

        public int GetPortCount()
        {
            return m_Ports.Count;
        }

        public string GetCstId(int portId)
        {
            return m_Ports[portId].Cst.CstID;
        }

        public GlassStatus GetGlassStatus(int portId, int slotId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return GlassStatus.Empty;
            }
            if (slotId >= Ports[portId].Cst.SlotCount || slotId < 0)
            {
                text = "BUG!! Slot No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return GlassStatus.Empty;
            }
            return m_Ports[portId].Cst.GetGlassStatus(slotId);
        }

        public LoaderGlass GetLoaderGlass(int portId, int slotId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return null;
            }
            if (slotId >= Ports[portId].Cst.Glasses.Length || slotId < 0)
            {
                text = "BUG!! Slot No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return null;
            }

            return Ports[portId].Cst.Glasses[slotId];
        }

        public LoaderStatus GetLoaderStatus()
        {
            return m_Status;
        }

        public LoaderEqpState GetLoaderEqpState()
        {
            if (m_Server.EqpStateManager.EqpUnit.EqpState == Common.EqpState.Fault)
            {
                if (m_EqpState != LoaderEqpState.Down)
                {
                    m_EqpState = LoaderEqpState.Down;
                }
            }
            else
            {
                if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Excute)
                {
                    if (m_EqpState != LoaderEqpState.Run)
                    {
                        m_EqpState = LoaderEqpState.Run;
                    }
                }
                else
                {
                    if (m_EqpState != LoaderEqpState.Idle)
                    {
                        m_EqpState = LoaderEqpState.Idle;
                    }
                }
            }

            return m_EqpState;
        }


        public MappingStatus[] GetMappingData(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            MappingStatus[] status = new MappingStatus[m_Ports[portId].Cst.SlotCount];
            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return status;
            }
            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                status[i] = m_Ports[portId].Cst.Glasses[i].GlassMappingStatus;
            }
            return status;
        }

        public LoaderGlass GetNextOutputGlass()
        {
            //if (m_QueueLotStart.Count > 0 && m_QueueLotStart.Peek() != null)
            if (GetFirstWaitingPort() >= 0)
            {
                int portId = (int)m_QueueLotStart.Peek();
                PortUnitA port = m_Ports[portId];
                int slotCnt = port.Cst.SlotCount;
                if (port.Cst.SlotOrder == CstSlotOrder.Increase)
                {
                    for (int i = 0; i < slotCnt; i++)
                    {
                        if (port.Cst.Glasses[i].Status == GlassStatus.Wait)
                        {
                            return port.Cst.Glasses[i];
                        }
                    }
                }
                else
                {
                    for (int i = slotCnt - 1; i >= 0; i--)
                    {
                        if (port.Cst.Glasses[i].Status == GlassStatus.Wait)
                        {
                            return port.Cst.Glasses[i];
                        }
                    }
                }

                //Wait glass가 더이상 없다면
                m_QueueLotStart.Dequeue();
                return null;
            }
            else return null;
        }

        public GlassStatus[] GetPortGlassStatus(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            int slotCnt = m_Ports[portId].Cst.SlotCount;
            GlassStatus[] status = new GlassStatus[slotCnt];
            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return status;
            }
            PortUnitA port = m_Ports[portId];
            for (int i = 0; i < slotCnt; i++)
            {
                status[i] = port.Cst.Glasses[i].Status;
            }
            return status;
        }

        public PortStatus GetPortStatus(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return PortStatus.None;
            }
            return m_Ports[portId].PortStatus;
        }

        public PortTransferMode GetTransferMode(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return PortTransferMode.None;
            }
            return m_Ports[portId].TransferMode;
        }

        public PortCommand GetPortCommand(int portId)
        {
            return m_Ports[portId].PortCommand;
        }

        public bool IsAreaSensorDetected(int portId)
        {
            return m_Ports[portId].AreaInterlock.IsAlarm;
        }

        public bool IsCassetteExist(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return false;
            }
            return Ports[portId].IsCstExist();
        }

        public bool IsPortPassable(int portId)
        {
            bool rv = Ports[portId].IsCstClampPos();
            rv &= Ports[portId].FloatUnit.IsPositive();
            rv &= Ports[portId].MappingUnit.IsMappingDriveUnitBw();
            rv &= !Ports[portId].DiCstOppositeDetect.GetState();
            rv |= !Ports[portId].IsCstExist() && !m_GenInfos.AutoMode;

            return rv;
        }

        public bool IsDoorOpen()
        {
            return m_LoaderDoorSensor.IsDetected();
        }

        public bool IsGlassOkInputToCst(int portId, int slotId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return false;
            }

            if (m_Ports[portId].Cst.Glasses[slotId].Status == GlassStatus.Proc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLoaderAutoMode()
        {
            if (m_Mode == LoaderMode.Auto) return true;
            else return false;
        }

        public bool IsPortEnable(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return false;
            }
            if (m_Ports[portId].PortEnable == PortUsage.Use) return true;
            else return false;
        }

        public bool IsPortError(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LoaderCommandReply LotAbortRequest(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }
            m_Ports[portId].LotAbortSet();
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply LotCancelRequest(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }
            m_Ports[portId].LotCancelSet();
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply LotEndRequest(int portId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            if (m_Ports[portId].PortStatus == PortStatus.InProcess ||
                m_Ports[portId].PortStatus == PortStatus.Aborting) { }  // Abort일때는 Lot End 명령을 받을 수 있어야 함.
            else return LoaderCommandReply.Nak;

            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                if (m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Proc ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Wait ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Selected ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Started ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Canceled)
                    return LoaderCommandReply.Nak;
            }

            if (m_Ports[portId].PortStatus == PortStatus.Aborting) m_Ports[portId].PortStatus = PortStatus.Abort;
            else m_Ports[portId].PortStatus = PortStatus.ProcessEnd;
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply LotStartRequest(int portId)
        {
            // 먼저 해당 Port에 cst가 있는지, 그 안의 glass들이 준비가 되어 있는지 Check한다.
            // 그 다음 Queue에 port order를 정하고 glass 상태들을 wait로 변경한다.
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }
            PortUnitA port = Ports[portId];
            if (port.IsCstExist() == false ||
                port.Cst == null) return LoaderCommandReply.Nak;

            if (m_QueueLotStart.Contains(portId)) return LoaderCommandReply.Nak;
            for (int i = 0; i < port.Cst.SlotCount; i++)
            {
                if (port.Cst.Glasses[i].Status == GlassStatus.Aborted ||
                    port.Cst.Glasses[i].Status == GlassStatus.Canceled ||
                    port.Cst.Glasses[i].Status == GlassStatus.Proc ||
                    port.Cst.Glasses[i].Status == GlassStatus.Scraped)
                    return LoaderCommandReply.Nak;
            }

            port.LotStartSet();

            m_QueueLotStart.Enqueue(portId);

            return LoaderCommandReply.Ack;
        }

        #region Port Command
        public LoaderCommandReply CstIdReadRequest(int portNo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LoaderCommandReply MappingRequest(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LoaderCommandReply ReChuckingRequest(int portId)
        {
            if (m_Ports[portId].PortStatus != PortStatus.UnloadRequest ||
                m_Ports[portId].TransferMode == PortTransferMode.AGV ||
                m_HostControlMode != HostControlMode.Offline)
            {
                m_Ports[portId].PortCommand = PortCommand.None;

                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].PortCommand = PortCommand.ReChuckingRequest;

            return LoaderCommandReply.Ack;
        }
        #endregion

        public void SetCstId(int portId, string cstId)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return;
            }
            m_Ports[portId].Cst.CstID = cstId;
        }

        public LoaderCommandReply SetGlassSelectData(int portId, SelectStatus[] status)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }
            if (status.Length != Ports[portId].Cst.Glasses.Length)
            {
                text = "BUG!! Slot No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                if (m_Ports[portId].Cst.Glasses[i].GlassMappingStatus == MappingStatus.Off &&
                    status[i] == SelectStatus.Selected)
                    return LoaderCommandReply.Nak;
            }

            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                m_Ports[portId].Cst.Glasses[i].GlassSelectStatus = status[i];

                if (status[i] == SelectStatus.Selected)
                {
                    m_Ports[portId].Cst.Glasses[i].Status = GlassStatus.Selected;
                    m_Ports[portId].Cst.Glasses[i].OrgPortNo = portId + 1;
                    m_Ports[portId].Cst.Glasses[i].OrgSlotNo = i + 1;
                    m_Ports[portId].Cst.Glasses[i].TargetPortNo = portId + 1;
                    m_Ports[portId].Cst.Glasses[i].TargetSlotNo = i + 1;
                }
                else
                {
                    if (m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Exist)
                    {
                        m_Ports[portId].Cst.Glasses[i].OrgPortNo = portId + 1;
                        m_Ports[portId].Cst.Glasses[i].OrgSlotNo = i + 1;
                        m_Ports[portId].Cst.Glasses[i].TargetPortNo = portId + 1;
                        m_Ports[portId].Cst.Glasses[i].TargetSlotNo = i + 1;
                    }
                    else if (m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Selected)
                    {
                        m_Ports[portId].Cst.Glasses[i].Status = GlassStatus.Exist;
                    }
                    else
                        m_Ports[portId].Cst.Glasses[i].Status = GlassStatus.Empty;
                }

            }

            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply SetGlassStatus(int portId, int slotId, GlassStatus status)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }
            if (slotId >= Ports[portId].Cst.SlotCount || slotId < 0)
            {
                text = "BUG!! Slot No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].Cst.SetGlassStatus(slotId, status);
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply SetLoaderReady()
        {
            throw new Exception("The method or operation is not implemented.");

            // Port와 Robot을 초기화 하여야 한다. 
            // 11.09 이함수는 사용안하기로 협의 홍/구
        }

        public LoaderCommandReply SetTransferMode(int portId, PortTransferMode mode)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].TransferMode = mode; // /////////////////////////////////////////// < Need to modify
            return LoaderCommandReply.Nak;
        }

        public event RobotEventHandler OnRobotEvent;

        public void FireRobotEvent(int portId, int slotId, RobotActionType action)
        {
            if (OnRobotEvent != null)
            {
                OnRobotEvent(portId, slotId, action);
            }
        }

        public void OnRobotTransferEvent(RobotActionType act, int portId, int slotId)
        {
            switch (act)
            {
                case RobotActionType.Get:
                    {
                        bool fromCst = portId < m_Ports.Count;
                        if (fromCst)
                        {
                            if (m_Ports[portId].PortStatus != PortStatus.Aborting) m_Ports[portId].PortStatus = PortStatus.InProcess;

                            //Event Fire
                            FireRobotEvent(portId, slotId, act);
                        }
                    }
                    break;
                case RobotActionType.Put:
                    {
                        bool intoCst = portId < m_Ports.Count;
                        if (intoCst)
                        {
                            //Event Fire
                            FireRobotEvent(portId, slotId, act);
                        }
                    }
                    break;
            }
        }

        public void SetHostControlMode(HostControlMode mode)
        {
            m_HostControlMode = mode;
        }

        public HostControlMode GetHostControlMode()
        {
            return m_HostControlMode;
        }

        public LoaderControlMode GetLoaderControlMode()
        {
            return m_LoaderControlMode;
        }

        public void SetOpCallState(bool call)
        {
            m_OpCallState = call;
        }

        public bool GetOpCallState()
        {
            return m_OpCallState;
        }
        #endregion
    }
}
