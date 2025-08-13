using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;
using Dms.Common;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorIOConfig), typeof(UITypeEditor))]
    [Serializable()]
    public class IoCollection<T> : iIoCollection
    {
        #region Implement IEnumerator
        // IEnumerable Interface Implementation:
        // Declaration of the GetEnumerator() method 
        // required by IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new InnerEnumerator(this);
        }

        // Inner class implements IEnumerator interface:
        private class InnerEnumerator : IEnumerator
        {
            private int m_Index = -1;
            private IoCollection<T> m_Collection;

            public InnerEnumerator(IoCollection<T> collection)
            {
                m_Collection = collection;
            }

            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (m_Index < m_Collection.Count - 1)
                {
                    m_Index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                m_Index = -1;
            }

            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return m_Collection.Items[m_Index];
                }
            }
        }
        #endregion

        #region Fields
        private List<T> m_Items = new List<T>();
        private int m_CurSimuateCount; // jemoon  : automatically create IoDevice in Server sider
        private int m_MaxSimulateCount = 1; //jemoon : automatically create IoDevice in IoCollecion at _DeviceAsm
        private Type m_ContainedItemType;
        private IoType m_ContainedIoType;
        #endregion

        #region Properties
        public Type ContainedItemType
        {
            get { return m_ContainedItemType; }
        }
        public IoType ContainedIoType
        {
            get { return m_ContainedIoType; }
        }
        public List<T> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        [Browsable(false), XmlIgnore()]
        public int CurSimulateCount
        {
            get { return m_CurSimuateCount; }
            set { m_CurSimuateCount = value; }
        }
        public int MaxSimulateCount
        {
            get { return m_MaxSimulateCount; }
            set { m_MaxSimulateCount = value; }
        }
        public T this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }        
        }
        public T this[string name]
        {
            get 
            {
                if (name != "" && name != null)
                {
                    foreach (T item in m_Items)
                    {
                        _DeviceIo io = item as _DeviceIo;
                        if (name == io.Name)
                        {
                            return item;
                        }
                    }
                    
                    MessageBox.Show("Io Name [" + name + "] is not found!");
                }
                return default(T); 
            }
            set 
            {
                foreach (T item in m_Items)
                {
                    _DeviceIo io = item as _DeviceIo;
                    if (io != null)
                    {
                        if (name == io.Name)
                        {
                            m_Items[io.Id] = value;
                        }
                    }
                }
            }
        }
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public string Name
        {
            get { return m_ContainedItemType.Name + "s"; }
        }
        #endregion

        #region Construct
        public IoCollection()
        {
            m_ContainedItemType = typeof(T);
            _DeviceIo io = (_DeviceIo)Activator.CreateInstance(m_ContainedItemType);
            m_ContainedIoType = io.GetIoInfo().IoType;
        }
        #endregion

        #region Methods
        public virtual void Add(object obj)
        {
            m_Items.Add((T)obj);
        }

        public void Add(T item)
        {
            m_Items.Add(item);
        }

        public void RemoveAt(int index)
        {
            m_Items.RemoveAt(index);
        }

        public void Clear()
        {
            m_Items.Clear();
        }

        public iIoCollection CreateCollection()
        {
            return new IoCollection<T>();
        }

        public _DeviceIo CreateNewItem()
        {
            return (_DeviceIo)Activator.CreateInstance(m_ContainedItemType);
        }

        public _DeviceIo GetItem(int index)
        {
            return this[index] as _DeviceIo;
        }

        public void SetItem(int index, object item)
        {
            this[index] = (T)item;
        }

        public void RemoveItem(int index)
        {
            m_Items.RemoveAt(index);
        }

        public bool[] GetDiStates()
        {
            int count = m_Items.Count;
            if (count == 0 || (m_ContainedIoType != IoType.DI))
            {
                return null;
            }
            else
            {
                bool[] values = new bool[count];
                for (int i = 0; i < count; i++)
                {
                    IoDigitalInput io = m_Items[i] as IoDigitalInput;
                    values[i] = io.GetState();
                }

                return values;
            }
        }

        public void SetDiStates(bool[] values)
        {
            int itemCount = m_Items.Count;
            int vlaueCount = values.Length;
            if (itemCount < vlaueCount || (m_ContainedIoType != IoType.DI))
            {
                return;
            }
            else
            {
                for (int i = 0; i < vlaueCount; i++)
                {
                    IoDigitalInput io = m_Items[i] as IoDigitalInput;
                    io.SetState(values[i]);
                }
            }
        }

        public bool[] GetDoStates()
        {
            int count = m_Items.Count;
            if (count == 0 || (m_ContainedIoType != IoType.DO))
            {
                return null;
            }
            else
            {
                bool[] values = new bool[count];
                for (int i = 0; i < count; i++)
                {
                    IoDigitalOutput io = m_Items[i] as IoDigitalOutput;
                    values[i] = io.GetState();
                }

                return values;
            }
        }

        public void SetDoStates(bool[] values)
        {
            int itemCount = m_Items.Count;
            int vlaueCount = values.Length;
            if (itemCount < vlaueCount || (m_ContainedIoType != IoType.DO))
            {
                return;
            }
            else
            {
                for (int i = 0; i < vlaueCount; i++)
                {
                    IoDigitalOutput io = m_Items[i] as IoDigitalOutput;
                    io.SetState(values[i]);
                }
            }
        }


        public short[] GetAiStates()
        {
            int count = m_Items.Count;
            if (count == 0 || (m_ContainedIoType != IoType.AI))
            {
                return null;
            }
            else 
            {
                short[] values = new short[count];
                for (int i = 0; i < count; i++)
                {
                    IoAnalogInput io = m_Items[i] as IoAnalogInput;
                    values[i] = io.GetState();
                }

                return values;
            }
        }

        public void SetAiStates(short[] values)
        {
            int itemCount = m_Items.Count;
            int vlaueCount = values.Length;
            if (itemCount < vlaueCount  || (m_ContainedIoType != IoType.AI))
            {
                return;
            }
            else
            {
                for (int i = 0; i < vlaueCount; i++)
                {
                    IoAnalogInput io = m_Items[i] as IoAnalogInput;
                    io.SetState(values[i]);
                }
            }
        }

        public ushort[] GetAoStates()
        {
            int count = m_Items.Count;
            if (count == 0 || (m_ContainedIoType != IoType.AO))
            {
                return null;
            }
            else
            {
                ushort[] values = new ushort[count];
                for (int i = 0; i < count; i++)
                {
                    IoAnalogOutput io = m_Items[i] as IoAnalogOutput;
                    values[i] = io.GetState();
                }

                return values;
            }
        }

        public void SetAoStates(ushort[] values)
        {
            int itemCount = m_Items.Count;
            int vlaueCount = values.Length;
            if (itemCount < vlaueCount || (m_ContainedIoType != IoType.AO))
            {
                return;
            }
            else
            {
                for (int i = 0; i < vlaueCount; i++)
                {
                    IoAnalogOutput io = m_Items[i] as IoAnalogOutput;
                    io.SetState(values[i]);
                }
            }
        }

        public int IndexOf(object item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                _DeviceIo io = GetItem(i);
                if ((item as _Device).Name == io.Name)
                {
                    return i;
                }
            }

            return -1;
        }

        public void InsertItem(int i, object item)
        {
            m_Items.Insert(i, (T)item);
        }
        #endregion

        #region Override
        public override string ToString()
        {
            if (this.Count == 0)
            {
                return this.Name;
            }
            else
            {
                int index = 0;
                string names = "";
                foreach (T item in m_Items)
                {
                    index++;
                    _Device device = (item as _Device);
                    if (device != null)
                    {
                        names += device.Name;
                        if (index < m_Items.Count)
                        {
                            names += ", ";
                        }
                    }
                }

                return names;
            }
        }
        #endregion
    }
}
