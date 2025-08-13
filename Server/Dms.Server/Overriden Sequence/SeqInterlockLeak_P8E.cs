using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.Sequence;

namespace Dms.Server
{
    public class ThreadLeakInterlock_P8E : ThreadLeakInterlock
    {
        #region Fields
        //protected static IServerManager m_Server;
        //protected static _GenericCollection<LeakSensor> m_Units;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (LeakSensor device in m_Units)
            {
                RegisterSequence(new SeqLeakInterlock(this, device));
            }
        }

        #endregion

        #region Constructor
        public ThreadLeakInterlock_P8E(int scanTime, IServerManager server)
            : base(scanTime,server)
        {
            //m_Server = server;
            //m_Units = m_Server.ComponentContainer.GetCollection<LeakSensor>();

            //RegisterSequences();
        } 
        #endregion

        #region Sequence
        //public override void Sequence()
        //{
        //    try
        //    {
        //        Thread.Sleep(m_ScanTime);

        //        if (m_Server.State != ActiveState.Run) return;

        //        foreach (XSeqFunction seq in SeqFunctions)
        //        {
        //            seq.Do();
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        XFunc.ExceptionHandler.Add(err);
        //    }
        //} 
        #endregion

        #region Static Methods
        //public static _GenericCollection<LeakSensor> Units
        //{
        //    get
        //    {
        //        if (m_Units == null) m_Units = new _GenericCollection<LeakSensor>();
        //        return m_Units;
        //    }
        //}
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqLeakInterlock : XSeqFunction
    {
        #region Fields
        protected LeakSensor Leak;
        protected static IEqpManager m_EqpManager;
        protected static ThreadLeakInterlock m_Control;
        protected static int UnitLeakCnt = 0;
        #endregion

        #region Constructor
        public SeqLeakInterlock(ThreadLeakInterlock control, LeakSensor leak)
        {
            Leak = leak;
            m_EqpManager = Leak.ServerManager.EqpStateManager;
            m_Control = control;

            SeqFunName = string.Format("{0} INTR", Leak.Name);
        } 
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool detected = true;
            int cnt = 0;
            detected &= Leak.IsDetectedInterlock();
            

            if (detected && !Leak.IsAlarm)
            {
                //Leak.SetInterlockCondtion(true);
                Leak.IsAlarm = true;
                AlarmId = Leak.ALM_Interlock.Id;
                m_EqpManager.SetAlarm(AlarmId);
                log = string.Format("Alarm Set : {0} Detect", Leak.Name);
                Leak.SetLog(SeqFunName, 0, 0, log);
            }
            else if (Leak.IsAlarm && !detected && m_EqpManager.AlarmResetSwitchPushed)
            {
             //   Leak.SetInterlockCondtion(false);
                Leak.IsAlarm = false;
                m_EqpManager.ResetAlarm(AlarmId);
                AlarmId = 0;
                log = string.Format("Alarm Reset : {0} Detect", Leak.Name);
                Leak.SetLog(SeqFunName, 0, 0, log);
            }
                if (detected)
                    UnitLeakCnt = (UnitLeakCnt | (0x0001 << (Leak.Id % 3)));
           
            if ((Leak.Id % 3) == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if ((UnitLeakCnt & (0x001 << i)) > 0)
                        cnt++;
                }
                if (cnt > 1)
                    Leak.SetInterlockCondtion(true);
                else
                    Leak.SetInterlockCondtion(false);
                UnitLeakCnt = 0;
            }
            return -1;
        } 
        #endregion
    }
}
