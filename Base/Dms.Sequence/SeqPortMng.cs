using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Device;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadPort : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<PortUnit> m_PortUnits;
        protected static int m_UnitCount;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ThreadPort(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_PortUnits = DmsComponents.Instance.ComponentContainer.GetCollection<PortUnit>();
            m_UnitCount = m_PortUnits.Count;
            m_GenInfos = GenInfoHandler.Instance;
            RegisterSequences();
        }
        #endregion

        #region Override
        protected override void RegisterSequences()
        {
            base.RegisterSequences();
        }

        public override void Sequence()
        {
            base.Sequence();
        }
        #endregion
    }

    public class SeqPortMng : XSeqFunction
    {
        #region Fields
        protected IServerManager m_ServerManager;
        protected PortUnit m_Port;
        private Simul m_Simul;
        #endregion

        #region constructor
        public SeqPortMng()
        {
        }
        public SeqPortMng(PortUnit port, IServerManager server)
        {
            m_Port = port;
            m_ServerManager = server;
            m_Simul = m_Port.Simul;

            //m_SeqStockerInfc = new SeqStokerLoad(port, server);
        }
        #endregion

        #region Override
        public override int Do()
        {
            SeqCst();
            SeqManualPortCommand();
            return -1;
        }
        #endregion

        #region SeqCst
        protected int m_SeqCstNo = 0;
        public virtual int SeqCst()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;

            SetPortSubUnitStatus(m_Port);

            int seqNo = m_SeqCstNo;
            switch (seqNo)
            {
                case 0:
                    {
                        // Check LoadCondition
                        bool loadCondition = true;
                        loadCondition &= m_Port.IsCstLoadCondition();
                        loadCondition &= m_Port.PortEnable == PortUsage.Use;

                        if (m_Port.TransferMode == PortTransferMode.AGV)
                        {
                            if (loadCondition)
                            {
                                seqNo = 100;
                            }
                            else
                            {
                                seqNo = 10;
                            }
                        }
                        else if (m_Port.TransferMode == PortTransferMode.MGV)
                        {
                            if (loadCondition || m_Port.InputOkSwPushConfirm)
                                seqNo = 100;
                            else
                                seqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        //Check Unload Condition
                        bool unloadContition = true;
                        unloadContition &= m_Port.IsCstUnloadCondition();
                        unloadContition &= m_Port.PortEnable == PortUsage.Use;

                        bool forceEnd = m_Port.ForceEndSwPushConfirm;

                        if (!unloadContition && !forceEnd)
                        {
                            seqNo = 0;
                        }
                        else if (unloadContition && !forceEnd)
                        {
                            seqNo = 200;
                        }
                        else if (forceEnd)
                        {
                            seqNo = 300;
                        }
                    }
                    break;
                case 100:
                    if (SeqCstLoad(m_Port) == 0)
                    {
                        seqNo = 10;
                    }
                    break;

                case 200:
                    if (SeqCstUnload(m_Port) == 0)
                    {
                        m_Port.ClearInfomation();
                        seqNo = 0;
                    }
                    break;
                case 300:
                    if (SeqCstForceEnd(m_Port) == 0)
                    {
                        seqNo = 10;
                    }
                    break;
            }
            m_SeqCstNo = seqNo;
            return -1;
        }
        #endregion

        #region CstLoad
        public virtual int SeqCstLoad(PortUnit port)
        {
            return -1;
        }

        #endregion

        #region CstUnload
        public virtual int SeqCstUnload(PortUnit port)
        {
            return -1;
        }
        #endregion

        #region CstForceEnd
        public virtual int SeqCstForceEnd(PortUnit port)
        {
            return -1;
        }
        #endregion

        public virtual void SetPortSubUnitStatus(PortUnit port)
        {
        }

        public virtual int SeqManualPortCommand()
        {
            return -1;
        }

    }
}
