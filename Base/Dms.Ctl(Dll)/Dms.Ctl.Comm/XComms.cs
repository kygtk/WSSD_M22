using System;
using System.Text;
using System.IO.Ports;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Dms.Ctl
{
    public class XComms
    {
        private List<XComm> m_Comms = new List<XComm>();
        
        public List<XComm> Comms
        {
            get { return m_Comms; }
        }

        public XComm this[int index]
        {
            get { return m_Comms[index]; }
        }

        public int Count
        {
            get { return m_Comms.Count; }
        }

        public XComms()
        {
            
        }

 
        public void Initialize(int CommNo)
        {
            for (int i = 0; i < CommNo; i++)
            {
                string PortName = string.Format("COM{0}", i);

                XComm Comm = new XComm(PortName);

                m_Comms.Add(Comm);
            }
        }
    }
}
