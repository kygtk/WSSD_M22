using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadHandControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<FishHand> m_FishHands;
        protected static int m_UnitCount;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {

        }
        #endregion

        #region Constructor

        public ThreadHandControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_FishHands = DmsComponents.Instance.ComponentContainer.GetCollection<FishHand>();
            m_UnitCount = m_FishHands.Count;

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
                if (!m_Server.ControllerIsRun) return;

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
        public static _GenericCollection<FishHand> Units
        {
            get
            {
                if (m_FishHands == null) m_FishHands = new _GenericCollection<FishHand>();
                return m_FishHands;
            }
        }
        #endregion   

        #region Virtual Methods
        public virtual void InitParameter()
        {


        }
        #endregion

    }
}
