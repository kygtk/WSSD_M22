///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.28
// Author       : jemoon
// Description  : CstMappingUnit class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Threading;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class CstMappingUnit_BoeHF_Theragen : _CstMappingUnit
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        protected Mutex m_Mutex = new Mutex();
        private bool m_InProcess = false;
        private SeqMapBoeHF_Mapping[] m_SeqMapping;
        #endregion

        #region Properties
        private int m_InterfaceTimeout = 10000;
        [Category("DMS : General Setting"), Description("Melsec Interface Mapping Timeout Setting(msec)")]
        public int InterfaceTimeout
        {
            get { return m_InterfaceTimeout; }
            set { m_InterfaceTimeout = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool InProcess
        {
            get { return m_InProcess; }
            set
            {
                m_Mutex.WaitOne();
                m_InProcess = value;
                m_Mutex.ReleaseMutex();
            }
        }
        #endregion

        #region Melsec Device
        private IoDigitalInput m_mibLoaderOffline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLoaderOffline
        {
            get { return m_mibLoaderOffline; }
            set { m_mibLoaderOffline = value; }
        }

        private IoDigitalInput m_mibPortCommandReadComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortCommandReadComplete
        {
            get { return m_mibPortCommandReadComplete; }
            set { m_mibPortCommandReadComplete = value; }
        }
        private IoDigitalInput m_mibPortCommandEndReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortCommandEndReport
        {
            get { return m_mibPortCommandEndReport; }
            set { m_mibPortCommandEndReport = value; }
        }

        private IoDigitalOutput m_mobPortCommandReadRequest = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortCommandReadRequest
        {
            get { return m_mobPortCommandReadRequest; }
            set { m_mobPortCommandReadRequest = value; }
        }
        private IoDigitalOutput m_mobPortCommandEndConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortCommandEndConfirm
        {
            get { return m_mobPortCommandEndConfirm; }
            set { m_mobPortCommandEndConfirm = value; }
        }

        private IoAnalogInput m_miwEndCommandPortNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwEndCommandPortNo
        {
            get { return m_miwEndCommandPortNo; }
            set { m_miwEndCommandPortNo = value; }
        }
        private IoAnalogInput m_miwEndCommandCode = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwEndCommandCode
        {
            get { return m_miwEndCommandCode; }
            set { m_miwEndCommandCode = value; }
        }
        private IoAnalogInput m_miwEndCommandResult = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwEndCommandResult
        {
            get { return m_miwEndCommandResult; }
            set { m_miwEndCommandResult = value; }
        }
        private IoCollection<IoAnalogInput> m_miwPortMapInfos = new IoCollection<IoAnalogInput>();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoCollection<IoAnalogInput> miwPortMapInfos
        {
            get { return m_miwPortMapInfos; }
            set { m_miwPortMapInfos = value; }
        }

        private IoAnalogOutput m_mowCommandPortNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandPortNo
        {
            get { return m_mowCommandPortNo; }
            set { m_mowCommandPortNo = value; }
        }
        private IoAnalogOutput m_mowCommandCode = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandCode
        {
            get { return m_mowCommandCode; }
            set { m_mowCommandCode = value; }
        }
        #endregion

        #region Constructor
        public CstMappingUnit_BoeHF_Theragen()
        {
            this.Name = "Port__ Mapping Unit";
        }
        #endregion

        #region Methods
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
            //m_MappingSensors.MaxSimulateCount = m_MaxSlotCount;

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
                    if (m_ControlMode == MuxMode.Single) m_MultiDropCount = 1;

                    m_PortIds = new int[m_MultiDropCount];
                    m_MappingStatus = new MappingStatus[m_MultiDropCount][];
                    m_SeqMapping = new SeqMapBoeHF_Mapping[m_MultiDropCount];
                    for (int i = 0; i < m_MultiDropCount; i++)
                    {
                        m_PortIds[i] = m_StartPortId + i;
                        m_MappingStatus[i] = new MappingStatus[m_MaxSlotCount];
                        m_SeqMapping[i] = new SeqMapBoeHF_Mapping(i, this);
                    }
                    m_UnitStatus = new int[m_MultiDropCount];
                    //m_SeqMapping = new SeqMapBoeHF_Mapping(this);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override int MakeGlassMappingStatus(int portId)
        {
            if (m_Simul.Loader)
            {
                MakeSimulMappingStatus(portId);
            }
            else
            {
                int id = FindChannelId(portId);

                short[] states = m_miwPortMapInfos[portId * 2].GetStates(2);

                for (int i = 0; i < m_MaxSlotCount; i++)
                {
                    int word = (int)(i / 16);
                    int bit = (int)(i % 16);
                    //                    m_MappingStatus[id][i] = (states[word] >> bit) == 1 ? MappingStatus.On : MappingStatus.Off;

                    if ((states[word] >> bit & 0x01) == 1)
                    {
                        m_MappingStatus[id][i] = MappingStatus.On;
                    }
                    else m_MappingStatus[id][i] = MappingStatus.Off;
                }
            }

            return 0;
        }

        public override int MakeGlassMappingStatus(int portId, int slotId)
        {
            if (m_Simul.Loader)
            {
                MakeSimulMappingStatus(portId, slotId);
            }
            else
            {
                int id = FindChannelId(portId);
                short[] states = m_miwPortMapInfos[portId * 2].GetStates(2);
                int word = (int)(slotId / 16);
                int bit = (int)(slotId % 16);

                //                m_MappingStatus[id][slotId] = (states[word] >> bit) == 1 ? MappingStatus.On : MappingStatus.Off;
                if ((states[word] >> bit & 0x01) == 1)
                {
                    m_MappingStatus[id][slotId] = MappingStatus.On;
                }
                else m_MappingStatus[id][slotId] = MappingStatus.Off;
            }

            return 0;
        }

        public override int Homing(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int Mapping(int portId)
        {
            int id = FindChannelId(portId);
            return m_SeqMapping[id].Do();
        }

        public override int Mapping(int portId, int slotId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int CheckUnitStatus(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsMappingDriveUnitFw()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsMappingDriveUnitBw()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }

    public class SeqMapBoeHF_Mapping : XSeqFunction
    {
        #region Fields
        private CstMappingUnit_BoeHF_Theragen m_MappingUnit;
        private XTimer m_TimerMapping = null;
        private int m_TimeoutMapping = 5 * 1000;
        private int m_PortId = 0;
        private int m_ChannelId = 0;
        private int m_CommandResult = 0;
        #endregion

        #region Constructor
        public SeqMapBoeHF_Mapping(int channelId, CstMappingUnit_BoeHF_Theragen mapUnit)
        {
            m_ChannelId = channelId;
            m_MappingUnit = mapUnit;
            m_PortId = m_MappingUnit.FindPortId(channelId);
            m_TimerMapping = new XTimer(m_MappingUnit.Name + "Mapping Timer");
            m_TimeoutMapping = m_MappingUnit.InterfaceTimeout;

            this.m_SeqFunName = "MAPPING";
        }
        #endregion

        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    if (!m_MappingUnit.InProcess)
                    {
                        //작업중임을 알리는 flag를 set하고
                        m_MappingUnit.InProcess = true;

                        m_MappingUnit.mowCommandPortNo.SetState((ushort)(m_PortId + 1));
                        m_MappingUnit.mowCommandCode.SetState((ushort)PortCommandCode.Mapping);
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Mapping Request");

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 300)
                    {
                        m_MappingUnit.mobPortCommandReadRequest.SetState(true);
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Request ON");

                        if (m_MappingUnit.Simul.Loader)
                        {
                            //melsec simulation mode이면 바로 ok로 처리
                            m_MappingUnit.mibPortCommandReadComplete.SetState(true);
                            m_TimerMapping.Start(m_TimeoutMapping);
                        }
                        seqNo = 20;
                    }
                    break;

                case 20:
                    if (m_MappingUnit.mibPortCommandReadComplete.GetState() == true)
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Complete ON");

                        m_MappingUnit.mobPortCommandReadRequest.SetState(false);
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Request OFF");

                        m_MappingUnit.mowCommandPortNo.SetState(0);
                        m_MappingUnit.mowCommandCode.SetState(0);

                        if (m_MappingUnit.Simul.Loader)
                        {
                            m_MappingUnit.mibPortCommandReadComplete.SetState(false);
                        }

                        seqNo = 30;
                    }
                    else if (m_MappingUnit.mibLoaderOffline.GetState())
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Loader Control Mode change Offline");

                        returnValue = 10;
                        m_MappingUnit.InProcess = false;
                        seqNo = 0;
                    }
                    break;

                case 30:
                    if (m_MappingUnit.mibPortCommandReadComplete.GetState() == false)
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command Read Complete OFF");

                        if (m_MappingUnit.Simul.Loader)
                        {
                            m_MappingUnit.mibPortCommandEndReport.SetState(true);
                            m_MappingUnit.miwEndCommandCode.SetState((short)PortCommandCode.Mapping);
                            m_MappingUnit.miwEndCommandPortNo.SetState((short)(m_PortId + 1));
                            m_MappingUnit.miwEndCommandResult.SetState(1);
                        }

                        m_TimerMapping.Start(m_TimeoutMapping);

                        seqNo = 40;
                    }
                    else if (m_MappingUnit.mibLoaderOffline.GetState())
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Loader Control Mode change Offline");

                        returnValue = 10;
                        m_MappingUnit.InProcess = false;
                        seqNo = 0;
                    }
                    break;

                case 40:
                    if (m_MappingUnit.mibPortCommandEndReport.GetState() == true)
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Report ON");

                        if ((m_MappingUnit.miwEndCommandCode.GetState() == (short)PortCommandCode.Mapping) &&
                           (m_MappingUnit.miwEndCommandPortNo.GetState() == ((short)m_PortId + 1)))
                        {
                            m_CommandResult = m_MappingUnit.miwEndCommandResult.GetState();

                            if (m_CommandResult == 1)
                            {
                                m_MappingUnit.MakeGlassMappingStatus(m_PortId);
                            }
                            //else
                            //{
                            //    m_MappingUnit.MakeGlassMappingStatus(m_PortId);
                            //}

                            m_MappingUnit.mobPortCommandEndConfirm.SetState(true);
                            m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Confirm ON");

                            if (m_MappingUnit.Simul.Loader)
                            {
                                m_MappingUnit.mibPortCommandEndReport.SetState(false);
                            }

                            seqNo = 50;
                        }
                    }
                    else if (m_MappingUnit.mibLoaderOffline.GetState())
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Loader Control Mode change Offline");

                        returnValue = 10;
                        m_MappingUnit.InProcess = false;
                        seqNo = 0;
                    }
                    else if (m_TimerMapping.Over)
                    {
                        m_MappingUnit.InProcess = false;
                        returnValue = 100;
                        seqNo = 0;
                    }
                    break;
                case 50:
                    if (m_MappingUnit.mibPortCommandEndReport.GetState() == false)
                    {
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Report OFF");

                        m_MappingUnit.mobPortCommandEndConfirm.SetState(false);
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, "Port Command End Confirm OFF");

                        if (m_CommandResult == 1)
                        {
                            returnValue = 0;
                        }
                        else if (m_CommandResult == 2)
                        {
                            returnValue = 2;
                        }
                        else if (m_CommandResult == 3)
                        {
                            returnValue = 3;
                        }
                        else
                        {
                            returnValue = 4;
                        }

                        string log;
                        log = string.Format("Port Command Result : {0}", m_CommandResult);
                        m_MappingUnit.SetLog("Mapping", this.m_SeqFunName, m_PortId + 1, 0, log);

                        m_MappingUnit.InProcess = false;
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
    }
}
