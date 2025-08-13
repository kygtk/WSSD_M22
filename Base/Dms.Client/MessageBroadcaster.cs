///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : EventSubscriber
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Client
{
    public class MessageBroadcaster : IBroadcaster
    {
        #region Fields
        private IBroadcaster m_bcaster = null;
        private BroadcastEventHelper m_eventHelper = null;
        #endregion

        #region Event
        public event MessageArrivedHandler MessageArrived;
        #endregion

        #region Constructor
        public MessageBroadcaster()
        {
            InitializeObjects();
        }
        #endregion

        #region Methods
        public void BroadcastMessage(object broadCastData)
        {
            if (m_bcaster == null)
                throw new ServerNotAliveException();

            m_bcaster.BroadcastMessage(broadCastData);
        }

        private void HandleMessage(object broadCastData)
        {
            if (MessageArrived != null)
                MessageArrived(broadCastData);
        }

        private void InitializeObjects()
        {
            m_bcaster = (IBroadcaster)RemotingHelper.GetObject(typeof(IBroadcaster));
            m_eventHelper = new BroadcastEventHelper();

            m_bcaster.MessageArrived += new MessageArrivedHandler(m_eventHelper.HandleMessageLocally);
            m_eventHelper.MessageArrivedLocally += new MessageArrivedHandler(HandleMessage);
        }
        #endregion
    }
}
