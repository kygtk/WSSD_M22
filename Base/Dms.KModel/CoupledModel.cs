using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.KModel
{
    public class Link
    {
        public Model m_From;
        public Model m_To;
        public string m_InMsg;
        public string m_OutMsg;

        public Link(Model from, string outMsg, Model to, string inMsg)
        {
            this.m_From = from;
            this.m_OutMsg = outMsg;
            this.m_InMsg = inMsg;
            this.m_To = to;
        }
    }

    public class CoupledModel : Model
    {
        #region Fields
        private List<Model> m_Children = new List<Model>();
        protected List<Link> m_DispatchTable = new List<Link>();
        #endregion

        #region Properties
        public List<Model> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }
        #endregion

        #region Constructor

        public CoupledModel()
        {
        }

        public CoupledModel(string name)
            : base(name)
        {
        }

        #endregion

        #region Methods

        public override int GetState()
        {
            return m_Children[0].GetState();
        }

        public void AddChild(Model child, int index)
        {
            m_Children.Add(child);
            child.SetParent(this);
        }

        public void AddCoupling(Model from, string outMsg, Model to, string inMsg)
		{
            m_DispatchTable.Add(new Link(from, outMsg, to, inMsg)); 
        }

        public virtual void AddAllCoupling()
        {
        }

        public void AddAllCouplingRecursive()
        {
            AddAllCoupling();

            int count = m_Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_Children[i] != null && m_Children[i].IsCoupledModel())
                {
                    m_Children[i].AddCouplingRecursive();
                }
            }
        }

        public void DispatchEvent(ModelEvent e)
        {
            int count = m_DispatchTable.Count;
            for (int i = 0; i < count; i++)
            {
                if (e.From.Equals(m_DispatchTable[i].m_From) &&
                    e.Message.Equals(m_DispatchTable[i].m_OutMsg))
                {
                    PostEvent(this, m_DispatchTable[i].m_InMsg, m_DispatchTable[i].m_To, 0, false);
                    //PostEvent(this, dispatchTable[i].inMsg, dispatchTable[i].to, e.GetTime(), e.IsSimulationEvent());
                }
            }
        }

        public override void AddCouplingRecursive()
        {
            base.AddCouplingRecursive();
        }

        public override Model GetChild(int i)
        {
            return base.GetChild(i);
        }

        //public virtual bool Initialize()
        //{
        //    return base.Initialize();
        //}

        public override bool IsCoupledModel()
        {
            return true;
        }

        public override void ProcessEvent(ModelEvent e)
        {
            base.ProcessEvent(e);
        }

        #endregion
    }
}
