using System;


namespace Dms.Common
{

    public interface IEventHelper
    {
        event MessageArrivedHandler MessageArrivedLocally;
        void HandleMessageLocally(object broadCastData);
    }

    [Serializable()]
    public class BroadcastEventHelper : MarshalByRefObject, IEventHelper
    {
        public event MessageArrivedHandler MessageArrivedLocally;

        public void HandleMessageLocally(object broadCastData)
        {
            if (MessageArrivedLocally != null)
                MessageArrivedLocally(broadCastData);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
