using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.KModel
{
    public class EventScheduler
    {
        #region Singleton 
        public static readonly EventScheduler Instance = new EventScheduler();
        #endregion

        #region Constructor
        private EventScheduler()
        {
        }
        #endregion

        #region Methods
        public void ScheduleEvent(Model from, string msg, Model to, UInt32 time, bool simul)
        {
            ModelEvent ev = new ModelEvent(from, msg, to, time, simul);
            EventQueue.Instance.Push(ev);
        }
        #endregion
    }
}
