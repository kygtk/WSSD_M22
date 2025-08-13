using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Collections;
using System.Reflection;
using Dms.Util.IODefine;
using System.Threading;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    abstract public class _DeviceAsm : _Device
    {
        #region Fields
        //protected ArrayList m_AssociatedIoDevices = null;
        protected static IServerManager m_Server = null;
        protected static IComponentContainer m_ComponentContainer = null;
        protected static readonly Simul m_Simul = AppConfig.Instance.Simul;
        protected static _GenInfoHandler m_GenInfos = null;
        private System.Threading.Timer m_ThreadingTimer = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public IServerManager ServerManager
        {
            get { return m_Server; }
        }
        [Browsable(false), XmlIgnore()]
        public Simul Simul
        {
            get { return m_Simul; }
        }
        [Browsable(false), XmlIgnore()]
        public _GenInfoHandler GenInfos { get { return m_GenInfos; } }
        [Browsable(false), XmlIgnore()]
        public IComponentContainer ComponentContainer
        {
            get { return m_ComponentContainer; }
        }
        [Browsable(false), XmlIgnore()]
        public ModelType ModelType
        {
            get
            {
                // Property의 Type이 _DeviceAsm 호환인 것의 Count를 가져온다.
                PropertyInfo[] propertyInfos1 = XFunc.GetProperties(this, typeof(_DeviceAsm), Compatibility.Compatible);
                PropertyInfo[] propertyInfos2 = XFunc.GetProperties(this, typeof(IGenericCollection), Compatibility.Compatible);
                return (propertyInfos1.Length + propertyInfos2.Length > 0) ? ModelType.Coupled : ModelType.Atomic;
            }
        }
        #endregion

        #region Methods
        public void SetComponentContainer(IComponentContainer componentContainer)
        {
            if (m_ComponentContainer == null)
            {
                m_ComponentContainer = componentContainer;
            }
        }
        public virtual DmsErrors Initialize(IServerManager server, _GenInfoHandler geninfos)
        {
            m_Server = server;
            m_GenInfos = geninfos;

            return Initialize();
        }
        public virtual void HandleEvent(object sender, IoStateEventArgs e)
        {
            if (!this.Initialized) return;

            //jemoon : 아래의 코드는 threading timer polling으로 대체함
            //bool matched = false;
            //if (m_AssociatedIoDevices != null)
            //{
            //    foreach (_DeviceIo io in m_AssociatedIoDevices)
            //    {
            //        if (io != null)
            //        {
            //            Dms.Util.IODefine.IoItem ioInfo = io.GetIoInfo();
            //            if (ioInfo.IoType == e.Type && ioInfo.Id == e.Id)
            //            {
            //                matched = true;
            //                break;
            //            }
            //        }
            //    }
            //}

            //if (matched)
            //{
            //    UpdateTag();
            //}
        }

        //Timer polling으로 Tag Update 수행
        public virtual void SetSubscriber()
        {
            //jemoon : event 쓰면 과도한 점유율 유발
            //ICtlDevice ctlDevice = m_Server.IoController;
            //ctlDevice.OnIoStateChange += new IoStateChangeEventHandler(HandleEvent);
            if (m_ThreadingTimer == null)
            {
                m_ThreadingTimer = new System.Threading.Timer(new TimerCallback(CheckState), null, 0, 300);
            }

            //UpdateTag();
        }

        public void CheckState(Object stateInfo)
        {
            if (m_Initialized)
            {
                UpdateTag();
            }
        }

        // Class가 가지는 Io들을 생성/동기화, 등록한다.
        protected bool GenerateAssociatedDevices()
        {
            try
            {
                bool ok = true;
                // Io Simulation Mode이면
                if (m_Simul.IoMapping)
                {
                    ok &= CreateAssociatedDevices();
                    ok &= CreateAssociatedCollection();
                }
                else
                {
                    ok &= SyncAssociatedDevices();
                    ok &= SyncAssociatedCollection();
                }

                //ok &= RegisterAssociatedIoDevices();

                return ok;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        protected ArrayList GetAssociatedIoDevices()
        {
            ArrayList arrayAssociatedIoDevices = new ArrayList();

            try
            {
                // Property의 Type이 _DeviceIo 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceIo), Compatibility.Compatible);
                PropertyInfo[] propertyIoCollections = XFunc.GetProperties(this, typeof(iIoCollection), Compatibility.Compatible);

                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get메소드 통해 object를 가져온다
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    object io = getMethodInfo.Invoke(this, null);
                    arrayAssociatedIoDevices.Add(io);
                }

                foreach (PropertyInfo info in propertyIoCollections)
                {
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    iIoCollection collection = getMethodInfo.Invoke(this, null) as iIoCollection;
                    int count = collection.Count;
                    for (int i = 0; i < count; i++)
                    {
                        arrayAssociatedIoDevices.Add(collection.GetItem(i));
                    }
                }
            }
            catch (Exception err) //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }

            return arrayAssociatedIoDevices;
        }

        public ArrayList GetAssociatedIoDevices(Type type)
        {
            if (type == null) return null;

            ArrayList deviceIoAll = GetAssociatedIoDevices();
            ArrayList deviceIo = new ArrayList();
            int count = deviceIoAll.Count;
            for (int i = 0; i < count; i++)
            {
                if (deviceIoAll[i].GetType() == type)
                {
                    deviceIo.Add(deviceIoAll[i]);
                }
            }

            return deviceIo;
        }

        public ArrayList GetAssociatedIoDevices(IoType ioType)
        {
            Type type = null;

            switch (ioType)
            {
                case IoType.DI:
                    type = typeof(IoDigitalInput);
                    break;
                case IoType.DO:
                    type = typeof(IoDigitalOutput);
                    break;
                case IoType.AI:
                    type = typeof(IoAnalogInput);
                    break;
                case IoType.AO:
                    type = typeof(IoAnalogOutput);
                    break;
            }

            return GetAssociatedIoDevices(type);
        }
        //Class가 가지는 IoCollection 계열을 Io Instance에서 가져와서 동기화 시킨다.. 
        private bool SyncAssociatedCollection()
        {
            bool ok = true;
            ok &= SyncAssociatedIoCollection();
            ok &= SyncAssociatedSlaveCollection();
            return ok;
        }

        private bool SyncAssociatedIoCollection()
        {
            // Property의 Type이 IioCollection 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(iIoCollection), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get, set 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();
                MethodInfo setMethodInfo = info.GetSetMethod();
                iIoCollection collection = getMethodInfo.Invoke(this, null) as iIoCollection;

                // Server에 생성되어진 Instance 를 가져와서
                if (collection == null)
                {
                    collection = Activator.CreateInstance(info.PropertyType) as iIoCollection;
                }
                else
                {
                    int count = collection.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        _DeviceIo io = collection.GetItem(i);
                        if (io != null)
                        {
                            Type type = io.GetType();
                            if (type == typeof(IoDigitalInput))
                            {
                                io = m_Server.DigitalInputs[io.Name];
                            }
                            else if (type == typeof(IoDigitalOutput))
                            {
                                io = m_Server.DigitalOutputs[io.Name];
                            }
                            else if (type == typeof(IoAnalogInput))
                            {
                                io = m_Server.AnalogInputs[io.Name];
                            }
                            else if (type == typeof(IoAnalogOutput))
                            {
                                io = m_Server.AnalogOutputs[io.Name];
                            }

                            if (io != null)
                            {
                                collection.SetItem(i, io);
                            }
                        }

                        if (io == null)
                        {
                            collection.RemoveItem(i);
                        }
                    }
                }

                // 가져온 instance로 set
                object[] parameters = new object[] { collection };
                setMethodInfo.Invoke(this, parameters);
            }

            return true;
        }

        private bool SyncAssociatedSlaveCollection()
        {
            // Property의 Type이 ISlaveCollection 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(ISlaveCollection), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get, set 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();
                MethodInfo setMethodInfo = info.GetSetMethod();
                ISlaveCollection collection = getMethodInfo.Invoke(this, null) as ISlaveCollection;

                // Server에 생성되어진 Instance 를 가져와서
                if (collection == null)
                {
                    collection = Activator.CreateInstance(info.PropertyType) as ISlaveCollection;
                }
                else
                {
                    int count = collection.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        _DeviceSlave slave = collection.GetItem(i);
                        if (slave != null)
                        {
                            Type type = slave.GetType();
                            if (type == typeof(SlaveServo))
                            {
                                slave = m_Server.SlaveServos[slave.Name];
                            }
                            if (type == typeof(SlaveBLDC))
                            {
                                slave = m_Server.SlaveBLDCs[slave.Name];
                            }
                            if (type == typeof(SlaveInverter))
                            {
                                slave = m_Server.SlaveInverters[slave.Name];
                            }
                            if (type == typeof(SlaveDigitalInput))
                            {
                                slave = m_Server.SlaveDigitalInputs[slave.Name];
                            }
                            else if (type == typeof(SlaveDigitalOutput))
                            {
                                slave = m_Server.SlaveDigitalOutputs[slave.Name];
                            }
                            else if (type == typeof(SlaveAnalogInput))
                            {
                                slave = m_Server.SlaveAnalogInputs[slave.Name];
                            }
                            else if (type == typeof(SlaveAnalogOutput))
                            {
                                slave = m_Server.SlaveAnalogOutputs[slave.Name];
                            }
                            else if (type == typeof(SlaveAP))
                            {
                                slave = m_Server.SlaveAPs[slave.Name];
                            }

                            if (slave != null)
                            {
                                collection.SetItem(i, slave);
                            }
                        }

                        if (slave == null)
                        {
                            collection.RemoveItem(i);
                        }
                    }
                }

                // 가져온 instance로 set
                object[] parameters = new object[] { collection };
                setMethodInfo.Invoke(this, parameters);
            }

            return true;
        }

        //Class가 가지는 IoCollection 계열을 simulation 할수 있도록 생성한다. 
        //m_Simul.IoMapping == true : IoMapping simulation mode인 경우에만 Call
        private bool CreateAssociatedCollection()
        {
            bool ok = true;
            ok &= CreateAssociatedIoCollection();
            ok &= CreateAssociatedSlaveCollection();
            return ok;
        }

        private bool CreateAssociatedIoCollection()
        {
            try
            {
                // Io Simulation Mode이면
                if (!m_Simul.IoMapping) return false;

                // Property의 Type이 IioCollection 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(iIoCollection), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();
                    iIoCollection collection = getMethodInfo.Invoke(this, null) as iIoCollection;

                    if (collection == null)
                    {
                        collection = Activator.CreateInstance(info.PropertyType) as iIoCollection;
                    }
                    else
                    {
                        collection.Clear();

                        // simulation할 Io 생성
                        _DeviceIo instance = null;
                        IoItem ioItem = null;
                        int count = collection.MaxSimulateCount;
                        Type type = collection.ContainedItemType;

                        for (int i = 0; i < count; i++)
                        {
                            if (type == typeof(IoDigitalInput))
                            {
                                if (m_Server.DigitalInputs.CurSimulateCount >= m_Server.DigitalInputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Digital Input size is not match");
                                    return false;
                                }

                                instance = m_Server.DigitalInputs[m_Server.DigitalInputs.CurSimulateCount++];
                                ioItem = instance.GetIoInfo();
                                ioItem.Id = instance.Id;
                                ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                ioItem.Name = "di" + ioItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = ioItem.Name;
                            }
                            else if (type == typeof(IoDigitalOutput))
                            {
                                if (m_Server.DigitalOutputs.CurSimulateCount >= m_Server.DigitalOutputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Digital Output size is not match");
                                    return false;
                                }

                                instance = m_Server.DigitalOutputs[m_Server.DigitalOutputs.CurSimulateCount++];
                                ioItem = instance.GetIoInfo();
                                ioItem.Id = instance.Id;
                                ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                ioItem.Name = "do" + ioItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = ioItem.Name;
                            }
                            else if (type == typeof(IoAnalogInput))
                            {
                                if (m_Server.AnalogInputs.CurSimulateCount >= m_Server.AnalogInputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Analog Input size is not match");
                                    return false;
                                }

                                instance = m_Server.AnalogInputs[m_Server.AnalogInputs.CurSimulateCount++];
                                ioItem = instance.GetIoInfo();
                                ioItem.Id = instance.Id;
                                ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                ioItem.Name = "ai" + ioItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = ioItem.Name;
                            }
                            else if (type == typeof(IoAnalogOutput))
                            {
                                if (m_Server.AnalogOutputs.CurSimulateCount >= m_Server.AnalogOutputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Analog Output size is not match");
                                    return false;
                                }

                                instance = m_Server.AnalogOutputs[m_Server.AnalogOutputs.CurSimulateCount++];
                                ioItem = instance.GetIoInfo();
                                ioItem.Id = instance.Id;
                                ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                ioItem.Name = "ao" + ioItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = ioItem.Name;
                            }

                            // 생성한 io instance를 IoDefines에 추가하고
                            m_Server.IoDefines.AddIOCollection(ioItem);

                            // 가져온 io instance로 set
                            collection.Add(instance);
                        }
                    }
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        private bool CreateAssociatedSlaveCollection()
        {
            try
            {
                // Io Simulation Mode이면
                if (!m_Simul.IoMapping) return false;

                // Property의 Type이 ISlaveCollection 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(ISlaveCollection), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();
                    ISlaveCollection collection = getMethodInfo.Invoke(this, null) as ISlaveCollection;

                    if (collection == null)
                    {
                        collection = Activator.CreateInstance(info.PropertyType) as ISlaveCollection;
                    }
                    else
                    {
                        collection.Clear();

                        // simulation할 Io 생성
                        _DeviceSlave instance = null;
                        EcSlaveItem slaveItem = null;
                        int count = collection.MaxSimulateCount;
                        Type type = collection.ContainedItemType;

                        for (int i = 0; i < count; i++)
                        {
                            if (type == typeof(SlaveServo))
                            {
                                if (m_Server.SlaveServos.CurSimulateCount >= m_Server.SlaveServos.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave Servo size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveServos[m_Server.SlaveServos.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "servo" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveBLDC))
                            {
                                if (m_Server.SlaveBLDCs.CurSimulateCount >= m_Server.SlaveBLDCs.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave BLDC size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveBLDCs[m_Server.SlaveBLDCs.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "bldc" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveInverter))
                            {
                                if (m_Server.SlaveInverters.CurSimulateCount >= m_Server.SlaveInverters.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave Inverter size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveInverters[m_Server.SlaveInverters.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "bldc" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveDigitalInput))
                            {
                                if (m_Server.SlaveDigitalInputs.CurSimulateCount >= m_Server.SlaveDigitalInputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave Digital Input size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveDigitalInputs[m_Server.SlaveDigitalInputs.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "di" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveDigitalOutput))
                            {
                                if (m_Server.SlaveDigitalOutputs.CurSimulateCount >= m_Server.SlaveDigitalOutputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave Digital Output size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveDigitalOutputs[m_Server.SlaveDigitalOutputs.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "do" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveAnalogInput))
                            {
                                if (m_Server.SlaveAnalogInputs.CurSimulateCount >= m_Server.SlaveAnalogInputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave Analog Input size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveAnalogInputs[m_Server.SlaveAnalogInputs.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "ai" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveAnalogOutput))
                            {
                                if (m_Server.SlaveAnalogOutputs.CurSimulateCount >= m_Server.SlaveAnalogOutputs.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave Analog Output size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveAnalogOutputs[m_Server.SlaveAnalogOutputs.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "ao" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }
                            else if (type == typeof(SlaveAP))
                            {
                                if (m_Server.SlaveAPs.CurSimulateCount >= m_Server.SlaveAPs.Count)
                                {
                                    MessageBox.Show("Can not start program : Slave AP size is not match");
                                    return false;
                                }

                                instance = m_Server.SlaveAPs[m_Server.SlaveAPs.CurSimulateCount++];
                                slaveItem = instance.GetSlaveInfo();
                                slaveItem.Id = instance.Id;
                                slaveItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                                slaveItem.Name = "ap" + slaveItem.Name.ToUpper() + (i + 1).ToString();
                                instance.Name = slaveItem.Name;
                            }

                            // 생성한 io instance를 IoDefines에 추가하고
                            m_Server.IoDefines.AddSlaveCollection(slaveItem);

                            // 가져온 io instance로 set
                            collection.Add(instance);
                        }
                    }
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        // jemoon : GenerateAssociatedIoDevices() 기능으로 대체됨으로 필요없음
        // Class가 가지는 Io들을 m_AssociatedIoDevices ArrayList에 등록한다.
        //private bool RegisterAssociatedIoDevices()
        //{
        //    try
        //    {
        //        if (m_AssociatedIoDevices == null) m_AssociatedIoDevices = new ArrayList();

        //        // Property의 Type이 _DeviceIo 호환인 것만 가져온다.
        //        PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceIo), Compatibility.Compatible);
        //        foreach (PropertyInfo info in propertyInfos)
        //        {
        //            // Property의 get메소드 통해 object를 가져온다
        //            MethodInfo getMethodInfo = info.GetGetMethod();
        //            object io = getMethodInfo.Invoke(this, null);
        //            m_AssociatedIoDevices.Add(io);
        //        }

        //        return true;
        //    }
        //    catch (Exception err) //Don't Use XFunc.ExceptionHandler.Add(err);
        //    {
        //        string msg = err.ToString();
        //        m_Server.WriteExceptionLog(msg);
        //        MessageBox.Show(msg);

        //        return false;
        //    }
        //}

        //Class가 가지는 Io들을 Server의 Io Instance에서 가져와서 동기화 시킨다. 
        //m_Simul.IoMapping == false : IoMapping simulation mode가 아닌경우에만 Call
        private bool SyncAssociatedDevices()
        {
            bool ok = true;
            ok &= SyncAssociatedIoDevices();
            ok &= SyncAssociatedSlaveDevices();
            return ok;
        }

        private bool SyncAssociatedIoDevices()
        {
            try
            {
                // Property의 Type이 _DeviceIo 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceIo), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();
                    _DeviceIo io = getMethodInfo.Invoke(this, null) as _DeviceIo;
                    object instance = null;

                    // Server에 생성되어진 Io Instance 를 가져와서
                    if (io != null)
                    {
                        if (info.PropertyType == typeof(IoDigitalInput))
                        {
                            instance = m_Server.DigitalInputs[io.Name];
                        }
                        else if (info.PropertyType == typeof(IoDigitalOutput))
                        {
                            instance = m_Server.DigitalOutputs[io.Name];
                        }
                        else if (info.PropertyType == typeof(IoAnalogInput))
                        {
                            instance = m_Server.AnalogInputs[io.Name];
                        }
                        else if (info.PropertyType == typeof(IoAnalogOutput))
                        {
                            instance = m_Server.AnalogOutputs[io.Name];
                        }
                    }

                    // 가져온 io instance로 set
                    object[] parameters = new object[] { instance };
                    setMethodInfo.Invoke(this, parameters);
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        private bool SyncAssociatedSlaveDevices()
        {
            try
            {
                // Property의 Type이 _DeviceSlave 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceSlave), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();
                    _DeviceSlave slave = getMethodInfo.Invoke(this, null) as _DeviceSlave;
                    object instance = null;

                    // Server에 생성되어진 Io Instance 를 가져와서
                    if (slave != null)
                    {
                        if (info.PropertyType == typeof(SlaveServo))
                        {
                            instance = m_Server.SlaveServos[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveBLDC))
                        {
                            instance = m_Server.SlaveBLDCs[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveInverter))
                        {
                            instance = m_Server.SlaveInverters[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveDigitalInput))
                        {
                            instance = m_Server.SlaveDigitalInputs[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveDigitalOutput))
                        {
                            instance = m_Server.SlaveDigitalOutputs[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveAnalogInput))
                        {
                            instance = m_Server.SlaveAnalogInputs[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveAnalogOutput))
                        {
                            instance = m_Server.SlaveAnalogOutputs[slave.Name];
                        }
                        else if (info.PropertyType == typeof(SlaveAP))
                        {
                            instance = m_Server.SlaveAPs[slave.Name];
                        }
                    }

                    // 가져온 io instance로 set
                    object[] parameters = new object[] { instance };
                    setMethodInfo.Invoke(this, parameters);
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        //Class가 가지는 Io들을 simulation 할수 있도록 생성한다. 
        //m_Simul.IoMapping == true : IoMapping simulation mode인 경우에만 Call

        private bool CreateAssociatedDevices()
        {
            bool ok = true;
            ok &= CreateAssociatedIoDevices();
            ok &= CreateAssociatedSlaveDevices();
            return ok;
        }

        private bool CreateAssociatedIoDevices()
        {
            try
            {
                // Io Simulation Mode이면
                if (!m_Simul.IoMapping) return false;

                // Property의 Type이 _DeviceIo 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceIo), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();

                    // simulation할 Io 생성
                    _DeviceIo instance = null;
                    IoItem ioItem = null;
                    if (info.PropertyType == typeof(IoDigitalInput))
                    {
                        if (m_Server.DigitalInputs.CurSimulateCount >= m_Server.DigitalInputs.Count)
                        {
                            MessageBox.Show("Can not start program : Digital Input size is not match");
                            return false;
                        }

                        instance = m_Server.DigitalInputs[m_Server.DigitalInputs.CurSimulateCount++];
                        ioItem = instance.GetIoInfo();
                        ioItem.Id = instance.Id;
                        ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        ioItem.Name = "di" + ioItem.Name.ToUpper();
                        instance.Name = ioItem.Name;
                    }
                    else if (info.PropertyType == typeof(IoDigitalOutput))
                    {
                        if (m_Server.DigitalOutputs.CurSimulateCount >= m_Server.DigitalOutputs.Count)
                        {
                            MessageBox.Show("Can not start program : Digital Output size is not match");
                            return false;
                        }

                        instance = m_Server.DigitalOutputs[m_Server.DigitalOutputs.CurSimulateCount++];
                        ioItem = instance.GetIoInfo();
                        ioItem.Id = instance.Id;
                        ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        ioItem.Name = "do" + ioItem.Name.ToUpper();
                        instance.Name = ioItem.Name;
                    }
                    else if (info.PropertyType == typeof(IoAnalogInput))
                    {
                        if (m_Server.AnalogInputs.CurSimulateCount >= m_Server.AnalogInputs.Count)
                        {
                            MessageBox.Show("Can not start program : Analog Input size is not match");
                            return false;
                        }

                        instance = m_Server.AnalogInputs[m_Server.AnalogInputs.CurSimulateCount++];
                        ioItem = instance.GetIoInfo();
                        ioItem.Id = instance.Id;
                        ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        ioItem.Name = "ai" + ioItem.Name.ToUpper();
                        instance.Name = ioItem.Name;
                    }
                    else if (info.PropertyType == typeof(IoAnalogOutput))
                    {
                        if (m_Server.AnalogOutputs.CurSimulateCount >= m_Server.AnalogOutputs.Count)
                        {
                            MessageBox.Show("Can not start program : Analog Output size is not match");
                            return false;
                        }

                        instance = m_Server.AnalogOutputs[m_Server.AnalogOutputs.CurSimulateCount++];
                        ioItem = instance.GetIoInfo();
                        ioItem.Id = instance.Id;
                        ioItem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        ioItem.Name = "ao" + ioItem.Name.ToUpper();
                        instance.Name = ioItem.Name;
                    }

                    // 생성한 io instance를 IoDefines에 추가하고
                    m_Server.IoDefines.AddIOCollection(ioItem);

                    // 가져온 io instance로 set
                    object[] parameters = new object[] { instance };
                    setMethodInfo.Invoke(this, parameters);
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        private bool CreateAssociatedSlaveDevices()
        {
            try
            {
                // Io Simulation Mode이면
                if (!m_Simul.IoMapping) return false;

                // Property의 Type이 _DeviceSlave 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceSlave), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();

                    // simulation할 Io 생성
                    _DeviceSlave instance = null;
                    EcSlaveItem slaveitem = null;
                    if (info.PropertyType == typeof(SlaveServo))
                    {
                        if (m_Server.SlaveServos.CurSimulateCount >= m_Server.SlaveServos.Count)
                        {
                            MessageBox.Show("Can not start program : Slave Servo size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveServos[m_Server.SlaveServos.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "servo" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveBLDC))
                    {
                        if (m_Server.SlaveBLDCs.CurSimulateCount >= m_Server.SlaveBLDCs.Count)
                        {
                            MessageBox.Show("Can not start program : Slave BLDC size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveBLDCs[m_Server.SlaveBLDCs.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "bldc" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveInverter))
                    {
                        if (m_Server.SlaveInverters.CurSimulateCount >= m_Server.SlaveInverters.Count)
                        {
                            MessageBox.Show("Can not start program : Slave Inverter size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveInverters[m_Server.SlaveInverters.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "inverter" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveDigitalInput))
                    {
                        if (m_Server.SlaveDigitalInputs.CurSimulateCount >= m_Server.SlaveDigitalInputs.Count)
                        {
                            MessageBox.Show("Can not start program : Slave Digital Input size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveDigitalInputs[m_Server.SlaveDigitalInputs.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "di" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveDigitalOutput))
                    {
                        if (m_Server.SlaveDigitalOutputs.CurSimulateCount >= m_Server.SlaveDigitalOutputs.Count)
                        {
                            MessageBox.Show("Can not start program : Slave Digital Output size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveDigitalOutputs[m_Server.SlaveDigitalOutputs.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "do" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveAnalogInput))
                    {
                        if (m_Server.SlaveAnalogInputs.CurSimulateCount >= m_Server.SlaveAnalogInputs.Count)
                        {
                            MessageBox.Show("Can not start program : Slave Analog Input size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveAnalogInputs[m_Server.SlaveAnalogInputs.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "ai" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveAnalogOutput))
                    {
                        if (m_Server.SlaveAnalogOutputs.CurSimulateCount >= m_Server.SlaveAnalogOutputs.Count)
                        {
                            MessageBox.Show("Can not start program : Analog Output size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveAnalogOutputs[m_Server.SlaveAnalogOutputs.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "ao" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }
                    else if (info.PropertyType == typeof(SlaveAP))
                    {
                        if (m_Server.SlaveAPs.CurSimulateCount >= m_Server.SlaveAPs.Count)
                        {
                            MessageBox.Show("Can not start program : AP size is not match");
                            return false;
                        }

                        instance = m_Server.SlaveAPs[m_Server.SlaveAPs.CurSimulateCount++];
                        slaveitem = instance.GetSlaveInfo();
                        slaveitem.Id = instance.Id;
                        slaveitem.Name = XFunc.FilterigName(Name) + "_" + info.Name.Substring(2);
                        slaveitem.Name = "ap" + slaveitem.Name.ToUpper();
                        instance.Name = slaveitem.Name;
                    }

                    // 생성한 io instance를 IoDefines에 추가하고
                    m_Server.IoDefines.AddSlaveCollection(slaveitem);

                    // 가져온 io instance로 set
                    object[] parameters = new object[] { instance };
                    setMethodInfo.Invoke(this, parameters);
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        // Class가 가지는 _DeviceAsm와 _GenericCollection을 Server instance로 부터 동기화 한다.
        public void SyncInstance(IComponentContainer components)
        {
            SyncAssociatedDeviceAsm(components);
            SyncAssociatedDeviceCollection(components);
        }

        //Class가 가지는 _GenericCollection 계열을 DmsComponents Instance에서 가져와서 동기화 시킨다. 
        private void SyncAssociatedDeviceCollection(IComponentContainer components)
        {
            // Property의 Type이 _GeneralCollection 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(IGenericCollection), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get, set 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();
                MethodInfo setMethodInfo = info.GetSetMethod();
                IGenericCollection collection = getMethodInfo.Invoke(this, null) as IGenericCollection;

                // Server에 생성되어진 Instance 를 가져와서
                if (collection == null)
                {
                    collection = Activator.CreateInstance(info.PropertyType) as IGenericCollection;
                }
                else
                {
                    int count = collection.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        _Device device = collection.GetItem(i);

                        if (device != null)
                        {
                            string deviceName = device.Name;
                            device = components[deviceName] as _Device;

                            if (device != null)
                            {
                                collection.SetItem(i, device);
                            }
                        }

                        if (device == null)
                        {
                            collection.RemoveItem(i);
                        }
                    }
                }

                // 가져온 instance로 set
                object[] parameters = new object[] { collection };
                setMethodInfo.Invoke(this, parameters);
            }
        }

        //Class가 가지는 _DeviceAsm 계열을 DmsComponents Instance에서 가져와서 동기화 시킨다. 
        private void SyncAssociatedDeviceAsm(IComponentContainer components)
        {
            // Property의 Type이 _DeviceIo 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(_DeviceAsm), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get, set 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();
                MethodInfo setMethodInfo = info.GetSetMethod();
                _DeviceAsm device = getMethodInfo.Invoke(this, null) as _DeviceAsm;
                object instance = null;

                // Server에 생성되어진 Instance 를 가져와서
                if (device != null)
                {
                    instance = components[device.Name];
                }

                // 가져온 instance로 set
                if (setMethodInfo != null)
                {
                    object[] parameters = new object[] { instance };
                    setMethodInfo.Invoke(this, parameters);
                }
            }
        }

        public void SetLog(string unitName, string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", unitName, seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(_DeviceAsm); }
        }
        #endregion
    }
}
