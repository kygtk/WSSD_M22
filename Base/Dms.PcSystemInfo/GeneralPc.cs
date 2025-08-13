///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.13
// Author       : Jaehee Hong
// Description  : General IPC System Information class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Text;
using System.Management;
using System;
using Dms.Common;

namespace Dms.PcSystemInfo
{
    public class GeneralPc : _PcSystemInfo
    {
        public Function<sbyte> funcGetCpuTemperature;
        public Function<short> funcGetCpuFan;
        public Function<string> funcGetDeviceType;
        public Function<string> funcGetModelNo;
        public Function<string> funcGetSerialNo;

        #region Constructor
        public GeneralPc()
        {
            funcGetCpuTemperature = new Function<sbyte>(GetCpuTemperature, "CPU Temperature", UnitType.Celsius, 80, 0);

            funcGetCpuFan = new Function<short>(GetCpuFanSpeed, "CPU Fan Speed", UnitType.rpm, short.MaxValue, 1000);

            funcGetDeviceType = new Function<string>(GetDeviceType, "Device Type");
            funcGetModelNo = new Function<string>(GetModelNo, "Model No");
            funcGetSerialNo = new Function<string>(GetSerialNo, "Serial No");

            funcGetDeviceType.Update();
            funcGetModelNo.Update();
            funcGetSerialNo.Update();

            SByteFuncList.Add(funcGetCpuTemperature);

            //ShortFuncList.Add(funcGetCpuFan);
        }
        #endregion

        #region override
        public override string GetDeviceType()
        {
            //return "General type";
            string returnValue = "";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystemProduct");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    returnValue = queryObj["Version"].ToString();
                }
            }
            catch (ManagementException e)
            {
                returnValue = e.Message;
            }
            return returnValue;
        }

        public override sbyte GetCpuTemperature()
        {
            sbyte value = 0;
            try
            {
                ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\WMI",
                   "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    decimal curTemp = Convert.ToDecimal(queryObj["CurrentTemperature"]);
                    curTemp = curTemp / 10 - 273.15m;
                    value = (sbyte)curTemp;
                    break;
                }
            }
            catch (ManagementException)
            {
                //XFunc.ExceptionHandler.Add(err as Exception);
                value = 0;
            }
            return value;
        }

        public override sbyte GetBoardIoTemperature()
        {
            sbyte value = 0;
            value = sbyte.MaxValue;
            return value;
        }

        public override sbyte GetCpuBoardTemperature()
        {
            sbyte value = 0;
            value = sbyte.MaxValue;
            return value;
        }

        public override short GetCaseFan1Speed()
        {
            short value = 0;
            value = short.MaxValue;
            return value;
        }

        public override short GetCaseFan2Speed()
        {
            short value = 0;
            value = short.MaxValue;
            return value;
        }

        public override short GetCaseFan3Speed()
        {
            short value = 0;
            value = short.MaxValue;
            return value;
        }

        public override short GetCpuFanSpeed()
        {
            short value = 0;
            value = short.MaxValue;
            return value;

            //short value = 0;
            //try
            //{
            //    ManagementObjectSearcher searcher =
            //        new ManagementObjectSearcher("root\\CIMV2",
            //        "SELECT * FROM Win32_Fan");

            //    foreach (ManagementObject queryObj in searcher.Get())
            //    {
            //        string str = queryObj["Status"].ToString();
            //    }
            //}
            //catch (ManagementException e)
            //{
            //    //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            //    value = 0;
            //}
            //return value;
        }

        public override int GetCpuVoltage()
        {
            return 0;
        }

        public override int GetDCVoltage()
        {
            return 0;
        }

        public override int GetStandbyVoltage()
        {
            return 0;
        }

        public override int GetBatteryVoltage()
        {
            return 0;
        }

        public override int GetCpu2Voltage()
        {
            return 0;
        }

        public override int GetDC2Voltage()
        {
            return 0;
        }

        public override int GetUpsBatteryVoltage()
        {
            return 0;
        }

        public override int GetCmosBatteryState()
        {
            return 1;
        }

        public override ushort GetPowerOnCycles()
        {
            return 0;
        }

        public override ushort GetPowerOnHours()
        {
            return 0;
        }

        public override ushort GetFanOnHours()
        {
            return 0;
        }

        public override string GetModelNo()
        {
            //return "IPC_____";
            string returnValue = "";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    returnValue = queryObj["Model"].ToString();
                    break;
                }
            }
            catch (ManagementException e)
            {
                returnValue = e.Message;
            }
            return returnValue;
        }

        public override string GetSerialNo()
        {
            //return "012345678901";
            string returnValue = "";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystemProduct");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    returnValue = queryObj["IdentifyingNumber"].ToString();
                    break;
                }
            }
            catch (ManagementException e)
            {
                returnValue = e.Message;
            }
            return returnValue;
        }
        #endregion
    }
}
