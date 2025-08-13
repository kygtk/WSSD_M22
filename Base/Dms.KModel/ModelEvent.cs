using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.KModel
{
    public class ModelEvent
    {
        #region Fields
        private Model m_From;
        private Model m_To;
        private UInt32 m_Time;
        private string m_Message;
        private bool m_Simul;
        #endregion

        #region Properties
        public Model From
        {
            get { return m_From; }
            set { m_From = value; }
        }

        public Model To
        {
            get { return m_To; }
            set { m_To = value; }
        }

        public UInt32 Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public bool Simul
        {
            get { return m_Simul; }
            set { m_Simul = value; }
        }
        #endregion

        #region Constructor
        public ModelEvent(Model from, string msg, Model to, UInt32 time, bool simul)
        {
            m_From = from;
            m_Message = msg;
            m_To = to;
            m_Time = time;
            m_Simul = simul;
        }
        #endregion

        #region Methods
        public void Set(Model from, Model to, string msg)
        {
            m_From = from; 
            m_To = to;
            m_Message = msg;
        }

        public UInt32 GetTime()
        {
            return m_Time;
        }

        #region Operator
        public static bool operator > (ModelEvent a, ModelEvent b)
        {
            if (a.m_Time > b.m_Time) return true;
            else return false;
        }

        public static bool operator < (ModelEvent a, ModelEvent b)
        {
            if (a.m_Time < b.m_Time) return true;
            else return false;
        }

        public static bool operator == (ModelEvent a, ModelEvent b)
        {
            if (a.m_Time == b.m_Time && a.m_From == b.m_From && a.m_Message == b.m_Message)
                return true;
            else
                return false;
        }

        public static bool operator != (ModelEvent a, ModelEvent b)
        {
            if (a.m_Time != b.m_Time || a.m_From != b.m_From || a.m_Message == b.m_Message)
                return true;
            else
                return false;
        }
        #endregion

        #endregion

        #region Override

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        #endregion
    }
}
