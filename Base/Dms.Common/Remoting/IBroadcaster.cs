using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public delegate void MessageArrivedHandler(object broadCastData);

    public interface IBroadcaster
    {
        void BroadcastMessage(object broadCastData);

        event MessageArrivedHandler MessageArrived;
    }
}
