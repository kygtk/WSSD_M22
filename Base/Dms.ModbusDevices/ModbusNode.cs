using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dms.DeviceLibrary;
using Dms.Ctl;
using Dms.Common;

namespace Dms.ModbusDevices
{
    public class ModbusNode : DmsModbusNode
    {
        #region Fields
        protected ModbusCommDevice m_Modbus = null;
        #endregion

        #region Constructor
        public ModbusNode()
        {
            m_Modbus = ModbusCommDevice.Instance;
        }
        #endregion

        #region Methods
        private void SyncInstance(object configration)
        {
            // Property의 Type이 MelsecDevice 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(ModbusDevice), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get, set 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();
                MethodInfo setMethodInfo = info.GetSetMethod();

                // Configration file에서 현재 class로 set
                object obj = getMethodInfo.Invoke(configration, null);
                object[] parameters = new object[] { obj };
                setMethodInfo.Invoke(this, parameters);
            }
        }

        public void SetModbus(ModbusCommDevice modbus)
        {
            // Property의 Type이 MelsecDevice 호환인 것만 가져온다.
            PropertyInfo[] propertyInfos = XFunc.GetProperties(this, typeof(ModbusDevice), Compatibility.Compatible);
            foreach (PropertyInfo info in propertyInfos)
            {
                // Property의 get 메소드를 가져온다.
                MethodInfo getMethodInfo = info.GetGetMethod();

                // 현재 Class의 object를 가져온다.
                ModbusDevice mod = getMethodInfo.Invoke(this, null) as ModbusDevice;
                //mod.SetModbus(modbus);
            }
        }

        public override bool Initialize()
        {
            //if (!m_CreateMode) ReadConfiguration(m_Config, this.GetPathName());
            ReadConfiguration(m_Config, this.GetPathName());

            //SetModbus(m_Modbus);

            return base.Initialize();
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            object obj = new object();
            if (dss.ReadXml(ref obj, this.GetType(), filename))
            {
                SyncInstance(obj);
            }
        }

        public override void WriteConfiguration()
        {
            DmsSerializingService dss = new DmsSerializingService();
            dss.WriteXml(this, this.GetType(), GetPathName());
        }
        #endregion
    }
}
