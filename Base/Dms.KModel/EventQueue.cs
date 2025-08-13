using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.KModel
{
    public class EventQueue
    {
        #region Fields
        private static Queue<ModelEvent> m_Queue = new Queue<ModelEvent>();
        #endregion

        #region Singletone
        public static readonly EventQueue Instance = new EventQueue();
        #endregion

        #region Constructor
        private EventQueue()
        {
        }

        public void Push(ModelEvent e)
        {
            m_Queue.Enqueue(e);
        }

        public ModelEvent Pop()
        {
            return m_Queue.Dequeue();
        }

        public bool Empty()
        {
            if (m_Queue.Count == 0) return true;
            
            return false;
        }

        public ModelEvent Peek()
        {
            return m_Queue.Peek();
        }
        #endregion
    }
}
