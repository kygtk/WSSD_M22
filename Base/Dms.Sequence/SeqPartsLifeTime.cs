using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public delegate bool GetPartsCheckConditionDelegate(int partsNo);

    public class ThreadPartsItems : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static PartsItemsHandler m_Handler;
        public GetPartsCheckConditionDelegate GetPartsCheckCondition;
        #endregion

        #region Properties
        public IServerManager ServerManager
        {
            get { return m_Server; }
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            int count = m_Handler.GetItems().Count;
            for (int i = 0; i < count; i++)
            {
                RegisterSequence(new SeqPartsUseTime(this, m_Handler, i));
                RegisterSequence(new SeqPartsCondition(this, m_Handler, i));
            }
        }
        #endregion

        #region Constructor
        public ThreadPartsItems(int scanTime, IServerManager server, PartsItemsHandler handler, GetPartsCheckConditionDelegate checkCondition)
            : base(scanTime)
        {
            m_Server = server;
            m_Handler = handler;
            GetPartsCheckCondition = checkCondition;

            RegisterSequences();
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
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqPartsUseTime : XSeqFunction
    {
        #region Fields
        protected static PartsItemsHandler m_Handler;
        protected static ThreadPartsItems m_Control;
        protected int m_PartsNo;
        protected uint m_Period = 60 * 1000;   //1min
        protected int m_Time;
        #endregion

        #region Constructor
        public SeqPartsUseTime(ThreadPartsItems control, PartsItemsHandler handler, int partNo)
        {
            m_Control = control;
            m_Handler = handler;
            m_PartsNo = partNo;
            m_SeqFunName = "PARTUSEDTIME";
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1;
                    }
                    break;
                case 1:
                    if (GetElapsedTicks() > m_Period)
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_Control.GetPartsCheckCondition(m_PartsNo))
                        {
                            m_Time++;
                            if (m_Time * m_Period > 60 * 60 * 1000)
                            {
                                m_Handler.IncreaseTime(m_PartsNo);
                                m_Time = 0;
                            }
                        }

                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqPartsCondition : XSeqFunction
    {
        #region Fields
        protected static ThreadPartsItems m_Control;
        protected static PartsItemsHandler m_Handler;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static _GenInfoHandler m_GenInfos;
        protected int m_PartsNo;
        #endregion

        #region Constructor
        public SeqPartsCondition(ThreadPartsItems control, PartsItemsHandler handler, int partNo)
        {
            m_Control = control;
            m_Server = m_Control.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_GenInfos = GenInfoHandler.Instance;
            m_Handler = handler;
            m_PartsNo = partNo;
            m_SeqFunName = "PARTCONDITION";
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1;
                    }
                    break;
                case 1:
                    if (GetElapsedTicks() > 1000)
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (!m_Handler.GetLifeTimeOver(m_PartsNo))
                        {
                            nSeqNo = 0;
                        }
                        else
                        {
                            m_AlarmId = m_Handler.Items[m_PartsNo].ALM_LifeTimeOver.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            //m_GenInfos.CycleStop = true;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        if (m_Handler.GetLifeTimeOver(m_PartsNo))
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            //m_GenInfos.CycleStop = true;
                            nSeqNo = 1100;
                        }
                        else
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 1100:
                    if (GetElapsedTicks() > 2000)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}
