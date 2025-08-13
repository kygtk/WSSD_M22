using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Dms.Device
{
    public class IoDigitalInputs
    {
        #region Fields
        private List<IoDigitalInput> m_Items = new List<IoDigitalInput>();
        private int m_SimuateCount; // jemoon  : only for automatically generate for io mapping simulation mode
        #endregion

        #region Properties
        public List<IoDigitalInput> Items
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
        public IoDigitalInput this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }        
        }
        public IoDigitalInput this[string name]
        {
            get
            {
                if (name != "" && name != null)
                {
                    foreach (IoDigitalInput item in m_Items)
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
                foreach (IoDigitalInput item in m_Items)
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
        public void Add(IoDigitalInput item)
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
