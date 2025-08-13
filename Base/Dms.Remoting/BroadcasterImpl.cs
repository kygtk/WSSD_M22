using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Remoting
{
    [Serializable()]
    public class BroadcasterImpl : MarshalByRefObject, IBroadcaster
    {
        public event MessageArrivedHandler MessageArrived;

        public bool IsThereSubscribers
        {
            get
            {
                return MessageArrived != null;
            }
        }

        public void BroadcastMessage(object broadCastData)
        {
            SafeInvokeEvent(broadCastData);
        }

        private void SafeInvokeEvent(object broadCastData)
        {
            if (MessageArrived == null)
                return;

            MessageArrivedHandler mah = null;
            foreach (Delegate del in MessageArrived.GetInvocationList())
            {
                try
                {
                    mah = (MessageArrivedHandler)del;
                    mah(broadCastData);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    MessageArrived -= mah;
                }
            }
        }
    }
}
