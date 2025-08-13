using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.KModel
{
    public class AtomicModel : Model
    {
        #region Fields
        protected int m_State;
        #endregion

        #region Constructor
        public AtomicModel()
        {
        }

        public AtomicModel(string name)
        {
            m_Name = name;
        }
        #endregion

        #region Methods
        public override void ProcessEvent(ModelEvent e)
        {
            StateTransitionAllTheWay(e);
        }

        public void StateTransitionAllTheWay(ModelEvent e)
        {
            while (true)
            {
                if (!StateTransition(e)) break;
            }
        }

        public void ScheduleEvent(Model from, string msg, Model to, UInt32 t, bool simul)
        {
            EventScheduler.Instance.ScheduleEvent(from, msg, to, t, simul);
        }

        public virtual bool StateTransition(ModelEvent e)
        {
            return false;
        }

        public override bool IsCoupledModel()
        {
            return false;
        }

        public override int GetState()
        {
            return m_State;
        }
        #endregion
    }
}
