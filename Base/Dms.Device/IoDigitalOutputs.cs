using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Dms.Device
{
    public class IoDigitalOutputs
    {
        #region Fields
        private List<IoDigitalOutput> m_Items = new List<IoDigitalOutput>();
        private int m_SimuateCount; // jemoon  : only for automatically generate for io mapping simulation mode
        #endregion

        #region Properties
        public List<IoDigitalOutput> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        [Browsable(false)]
        public int SimulateCount
        {
            get { return m_SimuateCount; }
            set { m_SimuateCount = value; }
        }
        public IoDigitalOutput this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }        
        }
        public IoDigitalOutput this[string name]
        {
            get 
            {
                if (name != "" && name != null)
                {
                    foreach (IoDigitalOutput item in m_Items)
                    {
                        if (name == item.Name)
                        {
                            return item;
                        }
                    }
                    MessageBox.Show("Io Name [" + name + "] is not found!");
                }
                return null; 
            }
            set 
            {
                foreach (IoDigitalOutput item in m_Items)
                {
                    if (name == item.Name)
                    {
                        m_Items[item.Id] = value;
                    }
                }
            }
        }
        #endregion

        #region Methods
        public void Add(IoDigitalOutput item)
        {
            m_Items.Add(item);
        }

        public void RemoveAt(int index)
        {
            m_Items.RemoveAt(index);
        }
        #endregion
    }
}
