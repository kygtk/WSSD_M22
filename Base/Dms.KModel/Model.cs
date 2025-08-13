using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.KModel
{

    public struct State
    {
        public State(string name, string comment)
        {
            Name = name;
            Comment = comment;
        }
        string Name;
        string Comment;
    }

    public struct Message
    {
        public Message(string name, string comment)
        {
            Name = name;
            Comment = comment;
        }
        string Name;
        string Comment;
    }

    public class Model
    {
        enum InMessage { IMSG1, IMSG2 }
        enum OutMessage { OMSG1, OMSG2 }

        #region Fields
        private Model m_Parent;
        protected string m_Name;
        #endregion

        #region Properties
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region Constructor
        public Model()
        {
        }
        public Model(string name)
        {
            m_Name = name;
        }
        #endregion

        #region Methods
        public void PostEvent(Model from, string msg, Model to, UInt32 time, bool simul)
        {
            if (to == null)
                to = m_Parent;
            time = time + XFunc.GetTickCount(); // It's different with original version
            EventQueue.Instance.Push( new ModelEvent(from, msg, to, time, simul));
        }

        public void PostEvent(Model from, string msg)
        {
            EventQueue.Instance.Push(new ModelEvent(from, msg, null, 0, false));
        }

        public void PostEvent(ModelEvent modelEvent)
        {
            EventQueue.Instance.Push(modelEvent);
        }

        public Model GetParent()
        {
            return m_Parent;
        }

        public void SetParent(Model parent)
        {
            m_Parent = parent;
        }

        public virtual Model GetChild(int i)
        {
            return null;
        }
        
        //public virtual bool Initialize()
        //{
        //    return false;
        //}
        public virtual int GetState()
        {
            return -1;
        }
        public virtual void AddCoupling()
        {
        }
        public virtual void AddCouplingRecursive()
        {
        }
        public virtual bool IsCoupledModel()
        {
            return false;
        }

        public virtual void ProcessEvent(ModelEvent e)
        {
        }
        #endregion
    }
}
