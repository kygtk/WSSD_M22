///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.13
// Author       : Jaehee Hong
// Description  : PC System Information factory standard abstract class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Dms.Common;
using Dms.Data;

namespace Dms.PcSystemInfo
{
    abstract public class _PcSystemInfo
    {
        public abstract string GetDeviceType();
        public abstract sbyte GetCpuTemperature();
        public abstract sbyte GetBoardIoTemperature();
        public abstract sbyte GetCpuBoardTemperature();
        public abstract Int16 GetCaseFan1Speed();
        public abstract Int16 GetCaseFan2Speed();
        public abstract Int16 GetCaseFan3Speed();
        public abstract Int16 GetCpuFanSpeed();
        public abstract Int32 GetCpuVoltage();
        public abstract Int32 GetDCVoltage();
        public abstract Int32 GetStandbyVoltage();
        public abstract Int32 GetBatteryVoltage();
        public abstract Int32 GetCpu2Voltage();
        public abstract Int32 GetDC2Voltage();
        public abstract Int32 GetUpsBatteryVoltage();
        /// <summary>
        /// 0: Unknown
        /// 1: Good
        /// 2: Bad
        /// </summary>
        /// <returns></returns>
        public abstract Int32 GetCmosBatteryState();

        public abstract UInt16 GetPowerOnCycles();
        public abstract UInt16 GetPowerOnHours();
        public abstract UInt16 GetFanOnHours();

        public abstract string GetModelNo();
        public abstract string GetSerialNo();

        public FuncList<sbyte> SByteFuncList = new FuncList<sbyte>();
        public FuncList<int> IntFuncList = new FuncList<int>();
        public FuncList<ushort> UShortFuncList = new FuncList<ushort>();
        public FuncList<string> StringFuncList = new FuncList<string>();
        public FuncList<short> ShortFuncList = new FuncList<short>();

        public static readonly string entryMax = "Max";
        public static readonly string entryMin = "Min";
        public static readonly string entryAlarmEnable = "AlarmEnable";

        public void Save<T>(Function<T> func, string entryName)
        {
            if (func.MonitorEnable == false) return;

            string value = "";

            if (entryName == entryMax) value = func.Max.ToString();
            else if (entryName == entryMin) value = func.Min.ToString();
            else if (entryName == entryAlarmEnable) value = func.AlarmEnable.ToString();

            GeneralData.SetStringValue(func.Name, entryName, value);
        }

        public void SaveAll()
        {
            foreach (Function<sbyte> func in SByteFuncList)
            {
                Save<sbyte>(func, entryMax);
                Save<sbyte>(func, entryMin);
                Save<sbyte>(func, entryAlarmEnable);
            }
            foreach (Function<int> func in IntFuncList)
            {
                Save<int>(func, entryMax);
                Save<int>(func, entryMin);
                Save<int>(func, entryAlarmEnable);
            }
            foreach (Function<ushort> func in UShortFuncList)
            {
                Save<ushort>(func, entryMax);
                Save<ushort>(func, entryMin);
                Save<ushort>(func, entryAlarmEnable);
            }
            foreach (Function<string> func in StringFuncList)
            {
                Save<string>(func, entryMax);
                Save<string>(func, entryMin);
                Save<string>(func, entryAlarmEnable);
            }
            foreach (Function<short> func in ShortFuncList)
            {
                Save<short>(func, entryMax);
                Save<short>(func, entryMin);
                Save<short>(func, entryAlarmEnable);
            }
        }
    }

    [Serializable()]
    public class Function<T>
    {
        #region Fields
        private T m_Value = default(T);
        private T m_Max = default(T);
        private T m_Min = default(T);
        private bool m_AlarmEnable = false;
        private string m_Name = "";
        private GetValue GetValueMethod;
        private bool m_MonitorEnable = true;
        private UnitType m_Unit = UnitType.None;

        public Alarm ALM_Interlock;

        public delegate T GetValue();
        #endregion

        #region Properties
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public T Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public T Max
        {
            get { return m_Max; }
            set { m_Max = value; }
        }

        public T Min
        {
            get { return m_Min; }
            set { m_Min = value; }
        }

        public bool AlarmEnable
        {
            get { return m_AlarmEnable; }
            set { m_AlarmEnable = value; }
        }

        public bool MonitorEnable
        {
            get { return m_MonitorEnable; }
            set { m_MonitorEnable = value; }
        }

        public UnitType Unit
        {
            get { return m_Unit; }
            set { m_Unit = value; }
        }
        #endregion

        #region Construction
        public Function(GetValue getMethod, string name)
            : this(getMethod, name, UnitType.None, default(T), default(T))
        {
        }

        public Function(GetValue getMethod, string name, UnitType unit, T max, T min)
        {
            GetValueMethod = getMethod;
            m_Unit = unit;
            m_Name = name;
            m_Max = max;
            m_Min = min;
        }
        #endregion

        #region Methods
        public void Update()
        {
            if (GetValueMethod != null)
            {
                Value = GetValueMethod();
            }
        }
        #endregion
    }

    public class FuncList<T> : List<Function<T>>
    {
        public new void Add(Function<T> item)
        {
            base.Add(item);

            string getData = GeneralData.GetStringValue(item.Name, _PcSystemInfo.entryMax, item.Max.ToString());
            item.Max = (T)Convert.ChangeType(getData, typeof(T));

            getData = GeneralData.GetStringValue(item.Name, _PcSystemInfo.entryMin, item.Min.ToString());
            item.Min = (T)Convert.ChangeType(getData, typeof(T));

            getData = GeneralData.GetStringValue(item.Name, _PcSystemInfo.entryAlarmEnable, item.AlarmEnable.ToString());
            item.AlarmEnable = (getData == bool.TrueString);
        }
    }
}
