using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Reflection;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectConfig), typeof(UITypeEditor))]
    public class _GenericCollection<T> : IGenericCollection, ICollectionFactory
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
            private _GenericCollection<T> m_Collection;

            public InnerEnumerator(_GenericCollection<T> collection)
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

        #region Consts
        private readonly Type[] ATOMIC_DEVICE_TYPES = {typeof(AlarmResetSwitch),
                                                       typeof(Sensor),
                                                       typeof(ETCOutput),
                                                       typeof(GlsSensor),
                                                       typeof(LevelSensor),
                                                       typeof(EmoSensor),
                                                       typeof(LeakSensor),
                                                       typeof(CoverSensor),
                                                       typeof(DoorSensor),
                                                       typeof(Buzzer),
                                                       typeof(Fan),
                                                       typeof(BrokenDetectFixedType),
                                                       typeof(BrokenDetectScanType),
                                                       typeof(SignalTower),
                                                       typeof(HepaFilter),
                                                       typeof(Ionizer),
                                                       typeof(AutoValve),
                                                       typeof(BLDCMotor),
                                                       typeof(RbMotor),
                                                       typeof(EuvLamp),
                                                       typeof(ESD),
                                                       typeof(DoorLockSensor),
                                                       typeof(PartsItem)};
        #endregion

        #region Fields
        protected IServerManager m_Server;
        protected _GenInfoHandler m_GenInfo;
        protected IComponentContainer m_ComponentContainer;
        protected Type m_ContainedItemType = null;
        protected ArrayList m_Items = new ArrayList();
        protected Simul m_Simul = null;
        //protected XSequence m_Thread = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public IServerManager ServerManager
        {
            get { return m_Server; }
        }
        [Browsable(false), XmlIgnore()]
        public IComponentContainer ComponentContainer
        {
            get { return m_ComponentContainer; }
        }
        [Category("DMS : Config")]
        [Editor(typeof(UIEditorGenericCollection), typeof(UITypeEditor))]
        public ArrayList Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public int Count
        {
            get { return m_Items.Count; }
        }
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public virtual Type ContainedItemType
        {
            get { return m_ContainedItemType; }
        }
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public virtual Type CollectionType
        {
            get { return this.GetType(); }
        }
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public virtual ModelType ContainedItemModel
        {
            get
            {
                if (Array.IndexOf(ATOMIC_DEVICE_TYPES, m_ContainedItemType) >= 0)
                    return ModelType.Atomic;

                _DeviceAsm obj = this.CreateObject() as _DeviceAsm;
                if (obj == null)
                {
                    return ModelType.Coupled;
                }
                else
                {
                    return obj.ModelType;
                }
            }
        }
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public string Name
        {
            get { return m_ContainedItemType.Name + "s"; }
        }
        public T this[int index]
        {
            get { return (T)(m_Items[index]); }
            set { m_Items[index] = value; }
        }
        public T this[string name]
        {
            get
            {
                foreach (T item in m_Items)
                {
                    _Device device = (item as _Device);

                    if (device != null)
                    {
                        if (name == device.Name)
                        {
                            return item;
                        }
                    }
                }

                return default(T);
            }
        }
        #endregion

        #region Constructor
        public _GenericCollection()
        {
            m_ContainedItemType = typeof(T);
        }
        #endregion

        #region Methods
        public void Clear()
        {
            m_Items.Clear();
        }

        public virtual void Add(object obj)
        {
            m_Items.Add((T)obj);
        }

        public virtual void Remove(object obj)
        {
            m_Items.Remove((T)obj);
        }

        public _Device GetItem(int index)
        {
            return this[index] as _Device;
        }

        public void SetItem(int index, object item)
        {
            this[index] = (T)item;
        }

        public void RemoveItem(int index)
        {
            m_Items.RemoveAt(index);
        }

        public object GetCollection()
        {
            return this;
        }

        public void SetComponentContainer(IComponentContainer componentContainer)
        {
            m_ComponentContainer = componentContainer;

            foreach (_DeviceAsm obj in m_Items)
            {
                obj.SetComponentContainer(m_ComponentContainer);
            }
        }

        public virtual bool Initialize(IServerManager server, _GenInfoHandler geninfos)
        {
            m_Server = server;
            m_Simul = AppConfig.Instance.Simul;
            m_GenInfo = geninfos;

            server.UninitializeDel += new UninitializeDelegate(this.Uninitialize);

            return this.Initialize();
        }

        protected virtual bool Initialize()
        {
            foreach (T obj in m_Items)
            {
                if (XFunc.CheckTypeCompatibility(typeof(_DeviceAsm), obj.GetType(), Compatibility.Compatible))
                {
                    (obj as _DeviceAsm).Initialize(m_Server, m_GenInfo);
                }
                else
                {
                    (obj as _Device).Initialize();
                }

                if (!(obj as _Device).Initialized)
                {
                    MessageBox.Show(string.Format("[{0}] is Not Initialized!", (obj as _Device).Name));
                    //Application.Exit();
                    return false;
                }
            }

            return true;
        }

        public virtual void Uninitialize()
        {
            foreach (T obj in m_Items)
            {
                (obj as _Device).Uninitialize();
            }
        }

        public virtual void SyncInstance(IComponentContainer components)
        {
            try
            {
                // CollectionÀÇ containeditem type ÀÌ _DeviceAsm °è¿­ ÀÎ°¡?
                bool ok = XFunc.CheckTypeCompatibility(typeof(_DeviceAsm), this.ContainedItemType, Compatibility.Compatible);
                if (ok)
                {
                    foreach (T device in m_Items)
                    {
                        (device as _DeviceAsm).SyncInstance(components);
                    }
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public void UpdateItemId()
        {
            int count = m_Items.Count;
            for (int i = 0; i < count; i++)
            {
                ((_DeviceAsm)m_Items[i]).Id = i;
            }
        }

        public void SetItems(ArrayList items)
        {
            m_Items.Clear();
            m_Items = items;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            if (this.Count == 0)
            {
                return "(Empty) " + this.Name;
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

        #region ICollectionFactory ¸â¹ö

        public IGenericCollection CreateCollection()
        {
            return new _GenericCollection<T>();
        }

        #endregion

        #region IDeviceFactory ¸â¹ö

        public _Device CreateObject()
        {
            if (m_ContainedItemType.IsAbstract || m_ContainedItemType.IsInterface || m_ContainedItemType.GetConstructors().Length == 0)
            {
                return null;
            }
            else
            {
                return Activator.CreateInstance(this.ContainedItemType) as _Device;
            }
        }

        #endregion
    }
}
