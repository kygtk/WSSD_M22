///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.13
// Author       : Jaehee Hong
// Description  : B&R APC System Information class with ADI Library
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Text;
using BR.Adi.Interop;
using Dms.Common;

namespace Dms.PcSystemInfo
{
    public class BrPc : _PcSystemInfo
    {
        #region Fields
        private static object m_LockKey = new object();
        public Function<string> funcGetDeviceType;
        public Function<int> funcGetCmosBatteryState;
        public Function<string> funcGetModelNo;
        public Function<string> funcGetSerialNo;
        public Function<sbyte> funcGetCpuTemperature;
        public Function<sbyte> funcGetBoardIoTemperature;
        public Function<sbyte> funcGetCpuBoardTemerature;
        public Function<short> funcGetCaseFan1Speed;
        public Function<short> funcGetCaseFan2Speed;
        public Function<short> funcGetCaseFan3Speed;
        public Function<short> funcGetCpuFanSpeed;
        public Function<int> funcGetCpuVoltage;
        public Function<int> funcGetDCVoltage;
        public Function<int> funcGetStandbyVoltage;
        public Function<int> funcGetBatteryVoltage;
        public Function<int> funcGetCpu2Voltage;
        public Function<int> funcGetDC2Voltage;
        public Function<int> funcGetUpsBatteryVoltage;
        public Function<ushort> funcGetPowerOnCycles;
        public Function<ushort> funcGetPowerOnHours;
        public Function<ushort> funcGetFanOnHours;
        public Function<sbyte> funcGetBoardPowerSupplyTemperature;
        public Function<sbyte> funcGetBoardDrive1Temperature;
        public Function<sbyte> funcGetBoardDrive2Temperature;
        #endregion

        #region Constructor
        public BrPc()
        {
            funcGetDeviceType = new Function<string>(GetDeviceType, "Device Type");
            funcGetCmosBatteryState = new Function<int>(GetCmosBatteryState, "CMOS Battery State", UnitType.None, 1, 1);
            funcGetModelNo = new Function<string>(GetModelNo, "Model No");
            funcGetSerialNo = new Function<string>(GetSerialNo, "Serial No");
            funcGetCpuTemperature = new Function<sbyte>(GetCpuTemperature, "CPU Temperature", UnitType.Celsius, 80, 0);
            funcGetBoardIoTemperature = new Function<sbyte>(GetBoardIoTemperature, "Board IO Temperature", UnitType.Celsius, 60, 0);
            funcGetCpuBoardTemerature = new Function<sbyte>(GetCpuBoardTemperature, "CPU Board Temperature", UnitType.Celsius, 60, 0);
            funcGetCaseFan1Speed = new Function<short>(GetCaseFan1Speed, "Case Fan1 Speed", UnitType.rpm, short.MaxValue, 1000);
            funcGetCaseFan2Speed = new Function<short>(GetCaseFan2Speed, "Case Fan2 Speed", UnitType.rpm, short.MaxValue, 1000);
            funcGetCaseFan3Speed = new Function<short>(GetCaseFan3Speed, "Case Fan3 Speed", UnitType.rpm, short.MaxValue, 1000);
            funcGetCpuFanSpeed = new Function<short>(GetCpuFanSpeed, "CPU Fan Speed", UnitType.rpm, short.MaxValue, 1000);
            funcGetCpuVoltage = new Function<int>(GetCpuVoltage, "CPU Voltage", UnitType.V, 5000, 1000);
            funcGetDCVoltage = new Function<int>(GetDCVoltage, "DC Voltage", UnitType.V, 5000, 1000);
            funcGetStandbyVoltage = new Function<int>(GetStandbyVoltage, "Standby Voltage", UnitType.V, 5000, 1000);
            funcGetBatteryVoltage = new Function<int>(GetBatteryVoltage, "Battery Voltage", UnitType.V, 5000, 1000);
            funcGetCpu2Voltage = new Function<int>(GetCpu2Voltage, "CPU2 Voltage", UnitType.V, 5000, 1000);
            funcGetDC2Voltage = new Function<int>(GetDC2Voltage, "DC2 Voltage", UnitType.V, 5100, 1000);
            funcGetUpsBatteryVoltage = new Function<int>(GetUpsBatteryVoltage, "UPS Battery Voltage", UnitType.V, 5000, 1000);
            funcGetPowerOnCycles = new Function<ushort>(GetPowerOnCycles, "Power On Cycles");
            funcGetPowerOnHours = new Function<ushort>(GetPowerOnHours, "Power On Hours");
            funcGetFanOnHours = new Function<ushort>(GetFanOnHours, "Fan On Hours");
            
            //run sequence but don't make Dms.Data.Alarm
            funcGetPowerOnCycles.MonitorEnable = false;
            funcGetPowerOnHours.MonitorEnable = false;
            funcGetFanOnHours.MonitorEnable = false;

            funcGetBoardPowerSupplyTemperature = new Function<sbyte>(GetPowerSupplyTemperature, "Board Power Supply Temperature", UnitType.Celsius, 60, 0);
            funcGetBoardDrive1Temperature = new Function<sbyte>(GetDrive1Temperature, "Board Drive1 Temperature", UnitType.Celsius, 60, 0);
            funcGetBoardDrive2Temperature = new Function<sbyte>(GetDrive2Temperature, "Board Drive2 Temperature", UnitType.Celsius, 60, 0);

            //인터락을 체크할 필요 없고 한번만 실행하면 되는 함수들은 FuncList에 넣지 말고 그냥 한번 실행만 하자.
            //FuncList에 Add를 하면 GeneralData.GetData할때 InterlockValue가 null이라서 Exception발생
            //StringFuncList.Add(funcGetDeviceType);
            //StringFuncList.Add(funcGetModelNo);
            //StringFuncList.Add(funcGetSerialNo);

            funcGetDeviceType.Update();
            funcGetModelNo.Update();
            funcGetSerialNo.Update();

            SByteFuncList.Add(funcGetCpuTemperature);
            SByteFuncList.Add(funcGetBoardIoTemperature);
            SByteFuncList.Add(funcGetCpuBoardTemerature);
            SByteFuncList.Add(funcGetBoardPowerSupplyTemperature);
            SByteFuncList.Add(funcGetBoardDrive1Temperature);
            SByteFuncList.Add(funcGetBoardDrive2Temperature);

            ShortFuncList.Add(funcGetCaseFan1Speed);
            ShortFuncList.Add(funcGetCaseFan2Speed);
            ShortFuncList.Add(funcGetCaseFan3Speed);
            ShortFuncList.Add(funcGetCpuFanSpeed);

            IntFuncList.Add(funcGetCpuVoltage);
            IntFuncList.Add(funcGetDCVoltage);
            IntFuncList.Add(funcGetStandbyVoltage);
            IntFuncList.Add(funcGetBatteryVoltage);
            IntFuncList.Add(funcGetCpu2Voltage);
            IntFuncList.Add(funcGetDC2Voltage);
            IntFuncList.Add(funcGetUpsBatteryVoltage);
            IntFuncList.Add(funcGetCmosBatteryState);

            UShortFuncList.Add(funcGetPowerOnCycles);
            UShortFuncList.Add(funcGetPowerOnHours);
            UShortFuncList.Add(funcGetFanOnHours);
        }
        #endregion

        #region Methods
        public sbyte GetTemperature(TemperatureValue valueId)
        {
            sbyte value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetTemperature(valueId, out value, sizeof(sbyte)) == false)
                {
                    return sbyte.MaxValue;
                }
                else
                    return value;
            }
        }

        public sbyte GetPowerSupplyTemperature()
        {
            return GetTemperature(TemperatureValue.PowerSupply);
        }

        public sbyte GetDrive1Temperature()
        {
            return GetTemperature(TemperatureValue.Drive1);
        }

        public sbyte GetDrive2Temperature()
        {
            return GetTemperature(TemperatureValue.Drive2);
        }
        #endregion

        #region Override
        /// <summary>
        /// Get PC Name
        /// </summary>
        /// <returns>string vlaue of PC name</returns>
        public override string GetDeviceType()
        {
            string returnValue;
            DeviceType deviceType;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetDeviceType(out deviceType) == false)
                {
                    int lastError = Marshal.GetLastWin32Error();
                    returnValue = "Error " + lastError;
                }
                else
                    returnValue = deviceType.ToString();
            return returnValue;
            }
        }

        public override Int32 GetCmosBatteryState()
        {
            Int32 state;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetHardwareValue(HardwareValue.BatteryState, out state, sizeof(Int32)) == false)
                {
                    int lastError = Marshal.GetLastWin32Error();
                    return lastError;
                }
                else
                    return state;
            }
        }

        public override string GetModelNo()
        {
            StringBuilder value;
            value = new StringBuilder(40);
            if (NativeMethods.AdiGetFactoryValue(FactoryValue.ModelNumber, value, 40) == false)
            {
                int lastError = Marshal.GetLastWin32Error();
                return "Error" + lastError.ToString();
            }
            else
                return value.ToString();
        }

        public override string GetSerialNo()
        {
            StringBuilder value;
            value = new StringBuilder(40);
            if (NativeMethods.AdiGetFactoryValue(FactoryValue.SerialNumber, value, 40) == false)
            {
                int lastError = Marshal.GetLastWin32Error();
                return "Error" + lastError.ToString();
            }
            else
                return value.ToString();
        }

        public override sbyte GetCpuTemperature()
        {
            sbyte value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetTemperature(TemperatureValue.CpuInternal, out value, sizeof(sbyte)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override sbyte GetBoardIoTemperature()
        {
            sbyte value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetTemperature(TemperatureValue.BoardIO, out value, sizeof(sbyte)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override sbyte GetCpuBoardTemperature()
        {
            sbyte value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetTemperature(TemperatureValue.CpuBoard, out value, sizeof(sbyte)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override short GetCaseFan1Speed()
        {
            Int16 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetHardwareValue(HardwareValue.CaseFan1Speed, out value, sizeof(Int16)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override short GetCaseFan2Speed()
        {
            Int16 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetHardwareValue(HardwareValue.CaseFan2Speed, out value, sizeof(Int16)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override short GetCaseFan3Speed()
        {
            Int16 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetHardwareValue(HardwareValue.CaseFan3Speed, out value, sizeof(Int16)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override short GetCpuFanSpeed()
        {
            Int16 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetHardwareValue(HardwareValue.CpuFanSpeed, out value, sizeof(Int16)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetCpuVoltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.Cpu, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetDCVoltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.DC, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetStandbyVoltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.Standby, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetBatteryVoltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.Battery, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetCpu2Voltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.Cpu2, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetDC2Voltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.DC2, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override int GetUpsBatteryVoltage()
        {
            Int32 value;
            lock(m_LockKey)
            {
                if (NativeMethods.AdiGetVoltage(VoltageValue.UpsBattery, out value, sizeof(Int32)) == false)
                {
                    return 0;
                }
                else
                    return value;
            }
        }

        public override ushort GetPowerOnCycles()
        {
            UInt16 value;
            if (NativeMethods.AdiGetStatisticsValue(StatisticsValue.PowerOnCycles, out value, sizeof(UInt16)) == false)
            {
                return 0;
            }
            else
                return value;
        }

        public override ushort GetPowerOnHours()
        {
            UInt16 value;
            if (NativeMethods.AdiGetStatisticsValue(StatisticsValue.PowerOnHours, out value, sizeof(UInt16)) == false)
            {
                return 0;
            }
            else
                return value;
        }

        public override ushort GetFanOnHours()
        {
            UInt16 value;
            if (NativeMethods.AdiGetStatisticsValue(StatisticsValue.FanOnHours, out value, sizeof(UInt16)) == false)
            {
                return 0;
            }
            else
                return value;
        }
        #endregion
    }
}
