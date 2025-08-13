using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;
using Dms.Data;

namespace Dms.Sequence
{
    public class ThreadEmoInterlock : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<EmoSensor> m_Units;
        protected static _GenericCollection<ServoUnit> m_ServoUnits;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public ThreadEmoInterlock(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<EmoSensor>();
            m_ServoUnits = DmsComponents.Instance.ComponentContainer.GetCollection<ServoUnit>();

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (EmoSensor device in m_Units)
            {
                RegisterSequence(new SeqEmoInterlock(this, device));
            }
            foreach (ServoUnit servo in m_ServoUnits)
            {
                if (servo.Interlocks.Count >= 1)
                    RegisterSequence(new SeqEmoServoStop(this, servo));
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

        #region Static Methods
        public static _GenericCollection<EmoSensor> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<EmoSensor>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods

        #endregion

        #region General Methods

        #endregion
    }

    public class SeqEmoInterlock : XSeqFunction
    {
        #region Fields
        private EmoSensor m_Emo;
        protected static IEqpManager m_EqpManager;
        protected static ThreadEmoInterlock m_Control;
        #endregion

        #region Constructor
        public SeqEmoInterlock(ThreadEmoInterlock control, EmoSensor emo)
        {
            m_Emo = emo;
            m_EqpManager = m_Emo.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", m_Emo.Name);
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
                        //StartTime = DateTime.Now;
                        m_StartTicks = Common.XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 3000)
                    {
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        string log;
                        bool detected = m_Emo.IsDetectedInterlock();

                        if (detected && !m_Emo.IsAlarm)
                        {
                            m_Emo.SetInterlockCondtion(true);
                            m_Emo.IsAlarm = true;
                            m_AlarmId = m_Emo.ALM_Interlock.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            log = string.Format("Alarm Set : {0} Detect", m_Emo.Name);
                            m_Emo.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        else if (m_Emo.IsAlarm && !detected && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_Emo.SetInterlockCondtion(false);
                            m_Emo.IsAlarm = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            log = string.Format("Alarm Reset : {0} Detect", m_Emo.Name);
                            m_Emo.SetLog(m_SeqFunName, 0, 0, log);
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEmoServoStop : XSeqFunction
    {
        #region Fields
        protected ServoUnit m_ServoUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadEmoInterlock m_Control;
        private Alarm EstopAlarm = null;
        #endregion

        #region Constructor
        public SeqEmoServoStop(ThreadEmoInterlock control, ServoUnit servo)
        {
            m_ServoUnit = servo;
            m_Server = servo.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            EstopAlarm = new Alarm($"{m_ServoUnit.Name} Servo Estop fial", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_SeqFunName = "Servo Stop";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        bool InterlockAlarm = false;
                        bool ServoMove = false;

                        foreach (_InterlockSensor interlock in m_ServoUnit.Interlocks)
                        {
                            InterlockAlarm |= interlock.IsAlarm;
                        }

                        foreach (ServoMotor servo in m_ServoUnit.Axis)
                        {
                            ServoMove |= servo.GetInMotion();
                        }

                        if (InterlockAlarm && ServoMove)
                        {
                            m_Server.Log($"Servo Stop Interlock Detected!!!(Case 0) : {m_ServoUnit.Name}");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        bool EstopCheck = true;

                        EstopCheck = m_ServoUnit.RbtEStop();

                        if (EstopCheck)
                        {
                            m_Server.Log($"Servo Stop Servo Motor E-Stop!!!(Case 10) : {m_ServoUnit.Name}");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 4000)
                        {
                            m_AlarmId = EstopAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Server.Log($"Servo Stop Servo Motor E-Stop Fail : {m_ServoUnit.Name}");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.Log($"Servo Stop Alarm Reset : {m_ServoUnit.Name}");
                        nSeqNo = 0;
                    }
                    break;
            }
            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}
