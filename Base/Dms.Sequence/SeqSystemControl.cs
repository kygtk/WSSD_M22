using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using Dms.PcSystemInfo;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadSystemControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        #endregion

        #region Constructor
        public ThreadSystemControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqCheckDeviceControllerStatus(this, m_Server));
            //RegisterSequence(new SeqCheckSystemStatus(this, m_Server));
            RegisterSequence(new SeqCheckAvailableLogStorage(this, m_Server));

            _PcSystemInfo pcSystem = PcSystemFactory.Instance.GetSystem(AppConfig.Instance.PcType);

            if (pcSystem != null)
            {
                //foreach (Function<string> func in pcSystem.StringFuncList)
                //{
                //    RegisterSequence(new SeqPcSystemFactorySetting(m_Server, func));
                //}
                foreach (Function<sbyte> func in pcSystem.SByteFuncList)
                {
                    RegisterSequence(new SeqPcSystem<sbyte>(m_Server, func));
                }
                foreach (Function<int> func in pcSystem.IntFuncList)
                {
                    RegisterSequence(new SeqPcSystem<int>(m_Server, func));
                }
                foreach (Function<short> func in pcSystem.ShortFuncList)
                {
                    RegisterSequence(new SeqPcSystem<short>(m_Server, func));
                }
                foreach (Function<ushort> func in pcSystem.UShortFuncList)
                {
                    RegisterSequence(new SeqPcSystem<ushort>(m_Server, func));
                }
            }
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion
    }

    public class SeqCheckDeviceControllerStatus : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadSystemControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected Alarm m_AlarmDeviceStatusError;
        #endregion

        #region Constructor
        public SeqCheckDeviceControllerStatus(ThreadSystemControl control, IServerManager server)
        {
            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "Device Controller";
            m_AlarmDeviceStatusError = new Alarm("Device Controller Abnormal Status Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            m_GenInfos.DeviceControllerState = m_Server.IoController.ControllerState;

            int nSeqNo = m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    {
                        //  Base Scenario
                        if (!m_Server.ControllerIsRun)
                        {
                            m_AlarmId = m_AlarmDeviceStatusError.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            nSeqNo = 10;
                        }
                        // Crevis Scenario
                        else if (BaseGlobalVar.Manualcompulsion) 
                        {
                            m_AlarmId = m_AlarmDeviceStatusError.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 10:
                    {
                        if (m_Server.ControllerIsRun)
                        {
                            if (m_AlarmId != 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId);
                                m_AlarmId = 0;
                                nSeqNo = 0;
                            }
                        }
                    }
                    break;
                case 100:
                    {
                        if (m_GenInfos.AutoMode)
                        {
                            m_Server.CommandProc(Command.Manual); // 메뉴얼 강제 전환 
                        }
                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    {
                        if (!BaseGlobalVar.Manualcompulsion) // 11.03.07 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCheckSystemStatus : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server = null;
        protected static ThreadSystemControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqCheckSystemStatus(ThreadSystemControl control, IServerManager server)
        {
            m_Server = server;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "System";
        }
        #endregion

        public override int Do()
        {
            try
            {
                if (AppConfig.Instance.UseCpuTemperatureCheck)
                {
                    ManagementObjectSearcher searcher =
                       new ManagementObjectSearcher("root\\WMI",
                       "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        decimal curTemp = Convert.ToDecimal(queryObj["CurrentTemperature"]);
                        curTemp = curTemp / 10 - 273.15m;
                        m_GenInfos.CurCpuTemp = curTemp;
                        break;
                    }
                }

            }
            catch (ManagementException err)
            {
                //XFunc.ExceptionHandler.Add(err as Exception);
                MessageBox.Show("An error occurred while querying for WMI data: " + err.Message);

                AppConfig.Instance.UseCpuTemperatureCheck = false;
                AppConfig.Instance.WriteXml();
            }

            return -1;
        }
    }

    public class SeqCheckAvailableLogStorage : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server = null;
        protected static IEqpManager m_EqpManager = null;
        protected static ThreadSystemControl m_Control;
        private Alarm ALM_NotEnoughStorage;
        #endregion

        #region Constructor
        public SeqCheckAvailableLogStorage(ThreadSystemControl control, IServerManager server)
        {
            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            ALM_NotEnoughStorage = new Alarm("Not Enough Storage Available to write log", AlarmLevel.L, AlarmCode.AttentionFlags);

            this.m_SeqFunName = "Storage";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        nSeqNo = 100;
                    }
                    break;
                case 100:
                    if (XLog.AvailableLogStorage == false)
                    {
                        m_AlarmId = ALM_NotEnoughStorage.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Server.Log("Not Enough Storage Warning : Set");
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (XLog.AvailableLogStorage == true)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.Log("Not Enough Storage Warning : Reset");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqPcSystem<T> : XSeqFunction
    {
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager = null;
        protected Function<T> m_Func;

        public SeqPcSystem(IServerManager server, Function<T> func)
        {
            if (m_Server == null) m_Server = server;
            if (m_EqpManager == null) m_EqpManager = m_Server.EqpStateManager;
            m_Func = func;
            if (m_Func.MonitorEnable) m_Func.ALM_Interlock = new Alarm(m_Func.Name + " Alarm", AlarmLevel.L, AlarmCode.AttentionFlags);
            m_SeqFunName = "Seq" + m_Func.Name.Trim();
        }

        private bool IsAlarm()
        {
            int current = Convert.ToInt32(m_Func.Value);
            int max = Convert.ToInt32(m_Func.Max);
            int min = Convert.ToInt32(m_Func.Min);

            bool alarm = false;
            alarm |= (current < min);
            alarm |= (current > max);
            return alarm;
        }

        public override int Do()
        {
            if (!m_Server.Initialized || !AppConfig.Instance.UseCpuTemperatureCheck) return -1;

            int nSeqNo = this.m_SeqNo;

            m_Func.Update();

            if (m_Func.MonitorEnable == false) return -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        //m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_Func.AlarmEnable && IsAlarm())
                        {
                            m_AlarmId = m_Func.ALM_Interlock.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            nSeqNo = 20;
                        }
                        //else if (GetElapsedTicks() > 5000)
                        //{
                        //    nSeqNo = 0;
                        //}
                    }
                    break;
                case 20:
                    {
                        if (m_EqpManager.AlarmResetSwitchPushed && (!m_Func.AlarmEnable || !IsAlarm()))
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
    }

    //public class SeqPcSystemFactorySetting : XSeqFunction
    //{
    //    protected static IServerManager m_Server = null;
    //    protected Function<string> m_Func;
    //    protected bool m_IsUpdate = false;

    //    public SeqPcSystemFactorySetting(IServerManager server, Function<string> func)
    //    {
    //        if (m_Server == null) m_Server = server;
    //        m_Func = func;
    //        SeqFunName = "Seq" + m_Func.Name.Trim();
    //    }

    //    public override int Do()
    //    {
    //        if (m_IsUpdate == false)
    //        {
    //            m_Func.Update();
    //            m_IsUpdate = true;
    //        }

    //        return -1;
    //    }
    //}
}
