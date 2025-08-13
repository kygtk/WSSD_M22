using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Diagnostics;

namespace Dms.Common
{
    public enum ByteOrder
    {
        LittleEndian,
        BigEndian
    }

    public enum Compatibility
    {
        Match,
        Compatible
    }

    public struct SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Millisecond;

        public static DateTime Now = DateTime.Now;

        public override string ToString()
        {
            //다음형식으로 -> 2009-12-04 14:39:48:406
            string now = string.Format("{0:d4}-{1:d2}-{2:d2} {3:d2}:{4:d2}:{5:d2}", Year, Month, Day, Hour, Minute, Second);
            return now;
        }

        public static bool CheckValidation(SystemTime time)
        {
            DateTime dateTime;
            bool valid = DateTime.TryParse(time.ToString(), out dateTime);
            return valid;
        }
    };

    public class XFunc
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "GetLocalTime", SetLastError = true)]
        public extern static void Win32GetLocalTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetLocalTime", SetLastError = true)]
        public extern static bool Win32SetLocalTime(ref SystemTime sysTime);

        public static XExceptionHandler ExceptionHandler = XExceptionHandler.Instance;

        public static string ConvertToString(short word, ByteOrder byteorder)
        {
            try
            {
                char ch1, ch2;
                string sTemp = "";

                ch1 = Convert.ToChar((word & 0x00FF));
                if (ch1 == 0) ch1 = ' ';
                ch2 = Convert.ToChar(((word >> 8) & 0x00FF));
                if (ch2 == 0) ch2 = ' ';

                if (byteorder == ByteOrder.BigEndian)
                {
                    sTemp += ch1.ToString();
                    sTemp += ch2.ToString();
                }
                else
                {
                    sTemp += ch2.ToString();
                    sTemp += ch1.ToString();
                }
                return sTemp;
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertToString(byte word, ByteOrder byteorder)
        {
            try
            {
                char ch1, ch2;
                string sTemp = "";

                ch1 = Convert.ToChar((word & 0x00FF));
                if (ch1 == 0) ch1 = ' ';
                ch2 = Convert.ToChar(((word >> 8) & 0x00FF));
                if (ch2 == 0) ch2 = ' ';

                if (byteorder == ByteOrder.BigEndian)
                {
                    sTemp += ch1.ToString();
                    sTemp += ch2.ToString();
                }
                else
                {
                    sTemp += ch2.ToString();
                    sTemp += ch1.ToString();
                }
                return sTemp;
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertToString(int[] word, int startIndex, int count, ByteOrder byteorder)
        {
            try
            {
                string stemp = "";
                for (int i = 0; i < count; i++)
                {
                    short nData = (short)word[startIndex + i];
                    stemp += ConvertToString(nData, byteorder);
                }

                return stemp;
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertToString(short[] word, int startIndex, int count, ByteOrder byteorder)
        {
            try
            {
                string stemp = "";
                for (int i = 0; i < count; i++)
                {
                    short nData = word[startIndex + i];
                    stemp += ConvertToString(nData, byteorder);
                }

                return stemp;
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertToString(byte[] word, int startIndex, int count, ByteOrder byteorder)
        {
            try
            {
                string stemp = "";
                for (int i = 0; i < count; i++)
                {
                    byte nData = word[startIndex + i];
                    stemp += ConvertToString(nData, byteorder);
                }

                return stemp;
            }
            catch
            {
                return "";
            }
        }

        public static short ConvertToWord(string sdata, ByteOrder byteorder)
        {
            try
            {
                char ch1, ch2;

                if (sdata.Length <= 0) ch1 = (char)0x20;
                else ch1 = Convert.ToChar(sdata.Substring(0, 1));

                if (sdata.Length <= 1) ch2 = (char)0x20;
                else ch2 = Convert.ToChar(sdata.Substring(1, 1));

                if (ch1 == ' ') ch1 = (char)0x20;
                if (ch2 == ' ') ch2 = (char)0x20;
                if (ch1 == '\r') ch1 = (char)0x00;
                if (ch2 == '\r') ch2 = (char)0x00;


                ushort ntemp;
                if (byteorder == ByteOrder.BigEndian)
                {
                    ntemp = (ushort)((ch2 << 8) | ch1);
                }
                else
                {
                    ntemp = (ushort)((ch1 << 8) | ch2);
                }

                return (short)ntemp;
            }
            catch
            {
                return 0;
            }
        }

        public static void ConvertToWord(string sdata, ref ushort[] stream, int startIndex, int count, ByteOrder byteorder)
        {
            try
            {
                string stemp = "";
                for (int i = 0; i < count; i++)
                {
                    if (sdata == null)
                    {
                        stemp = "";
                    }
                    else if (sdata.Length >= 2)
                    {
                        stemp = sdata.Substring(0, 2);
                        sdata = sdata.Remove(0, 2);
                    }
                    else if (sdata.Length >= 1)
                    {
                        stemp = sdata.Substring(0, 1);
                        sdata = sdata.Remove(0, 1);
                    }
                    else
                    {
                        stemp = "";
                    }

                    stream[startIndex + i] = (ushort)ConvertToWord(stemp, byteorder);
                }
            }
            catch
            {

            }
        }

        public static void ConvertToWord(string sdata, ref short[] stream, int startIndex, int count, ByteOrder byteorder)
        {
            try
            {
                string stemp = "";
                for (int i = 0; i < count; i++)
                {
                    if (sdata == null)
                    {
                        stemp = "";
                    }
                    else if (sdata.Length >= 2)
                    {
                        stemp = sdata.Substring(0, 2);
                        sdata = sdata.Remove(0, 2);
                    }
                    else if (sdata.Length >= 1)
                    {
                        stemp = sdata.Substring(0, 1);
                        sdata = sdata.Remove(0, 1);
                    }
                    else
                    {
                        stemp = "";
                    }

                    stream[startIndex + i] = ConvertToWord(stemp, byteorder);
                }
            }
            catch
            {

            }
        }

        public static void ConvertToWord(string sdata, ref byte[] stream, int startIndex, int count, ByteOrder byteorder)
        {
            try
            {
                string stemp = "";
                for (int i = 0; i < count; i++)
                {
                    if (sdata == null)
                    {
                        stemp = "";
                    }
                    else if (sdata.Length >= 2)
                    {
                        stemp = sdata.Substring(0, 2);
                        sdata = sdata.Remove(0, 2);
                    }
                    else if (sdata.Length >= 1)
                    {
                        stemp = sdata.Substring(0, 1);
                        sdata = sdata.Remove(0, 1);
                    }
                    else
                    {
                        stemp = "";
                    }

                    stream[startIndex + i] = (byte)ConvertToWord(stemp, byteorder);
                }
            }
            catch
            {

            }
        }

        public static int ConvertBcdToInt(short value)
        {
            //string sBin = ConvertToBin(value);
            //string temp = "";
            //StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < 4; i++)
            //{
            //    temp = sBin.Substring(i * 4, 4);

            //    sb.Append(Convert.ToInt32(temp, 2));
            //}

            //return sb.ToString();

            int nReturnVal = 0;

            nReturnVal = value & 0X0F;
            nReturnVal += (value >> 4 & 0X0F) * 10;
            nReturnVal += (value >> 8 & 0X0F) * 100;
            nReturnVal += (value >> 12 & 0X0F) * 1000;

            return nReturnVal;
        }

        public static short ConvertToAscii(string value)
        {
            int nReturnVal = 0;

            char[] temp;

            temp = value.ToCharArray();

            if (value.Length > 1)
            {
                nReturnVal = temp[1] << 8 | temp[0];
            }
            else nReturnVal = temp[0];

            return (short)nReturnVal;
        }

        public static short ConvertBcdToShort(string value)
        {
            string temp = "";
            string sBin = "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                temp = value.Substring(i * 1, 1);

                sBin = Convert.ToString(Convert.ToInt16(temp), 2).PadLeft(4, '0');

                sb.Append(sBin);
            }

            return Convert.ToInt16(sb.ToString(), 2);
        }

        public static short ConvertIntToBcd(int value)
        {
            int nReturnVal = 0;

            int[] nTmp = new int[4];
            nTmp[0] = value / 1000;
            nTmp[1] = (value - nTmp[0] * 1000) / 100;
            nTmp[2] = (value - nTmp[0] * 1000 - nTmp[1] * 100) / 10;
            nTmp[3] = (value - nTmp[0] * 1000 - nTmp[1] * 100 - nTmp[2] * 10);

            nReturnVal = nTmp[0] << 12 | nTmp[1] << 8 | nTmp[2] << 4 | nTmp[3];

            return (short)nReturnVal;
        }

        public static string ConvertShortToBcd(short value)
        {
            string sBin = ConvertToBin(value);
            string temp = "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                temp = sBin.Substring(i * 4, 4);

                sb.Append(Convert.ToInt32(temp, 2));
            }

            return sb.ToString();
        }

        public static int ConvertBinToDec(string sValue)
        {
            try
            {
                int nDec = 0;
                nDec = Convert.ToInt32(sValue, 2);

                return nDec;
            }
            catch
            {
                return -1;
            }
        }

        public static string ConvertToBin(short value)
        {
            string sBin = "";

            try
            {
                sBin = Convert.ToString(value, 2).PadLeft(16, '0');
            }
            catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                sBin = err.Message.ToString();
            }

            return sBin.ToUpper();
        }

        // jemoon
        // Class Type의 호환성 검사
        //
        public static bool CheckTypeCompatibility(Type type, Type target, Compatibility compatibility)
        {
            if (target == null)
            {
                return false;
            }
            else if (type == target)
            {
                return true;
            }
            else if (type.IsInterface)
            {
                Type[] types = target.GetInterfaces();
                foreach (Type typeofInterface in types)
                {
                    if (type == typeofInterface)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (compatibility == Compatibility.Match)
            {
                return false;
            }
            else
            {
                return CheckTypeCompatibility(type, target.BaseType, compatibility);
            }
        }

        // jemoon
        // Class Type의 호환성 검사
        //
        public static bool CheckTypeCompatibility_BaseOneStep(Type type, Type target)
        {
            if (target == null)
            {
                return false;
            }
            else if (type == target)
            {
                return true;
            }
            else if (type.IsInterface)
            {
                Type[] types = target.GetInterfaces();
                foreach (Type typeofInterface in types)
                {
                    if (type == typeofInterface)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return CheckTypeCompatibility(type, target.BaseType, Compatibility.Match);
            }
        }


        // jemoon
        // Class의 모든 Property정보를 반환
        //
        public static PropertyInfo[] GetProperties(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            return propertyInfos;
        }

        // jemoon
        // Class의 Property정보중에서 Type호환성이 있는 정보만 반환
        //
        public static PropertyInfo[] GetProperties(object obj, Type type, Compatibility compatibility)
        {
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            PropertyInfo[] propertyInfosAll = GetProperties(obj);
            // 해당 object의 모든 property에서 type이 호환되는것만 가져온다.
            foreach (PropertyInfo info in propertyInfosAll)
            {
                if (CheckTypeCompatibility(type, info.PropertyType, compatibility))
                {
                    propertyInfos.Add(info);
                }
            }

            return propertyInfos.ToArray();
        }

        // jemoon
        // device name에 공백, 특수문자 등을 '_'로 치환
        public static string FilterigName(string name)
        {
            char[] chars = name.ToCharArray();
            int count = chars.Length;
            for (int i = 0; i < count; i++)
            {
                if (!char.IsLetterOrDigit(chars[i]))
                {
                    chars[i] = '_';
                }
            }

            string result = new string(chars);
            return result;
        }

        //public static List<Exception> ExceptionHandler = new List<Exception>();

        // 구조체를 byte 배열로 변환해주는 함수
        public static byte[] StructureToByte(object obj)
        {
            byte[] data = null;

            try
            {
                int datasize = Marshal.SizeOf(obj);         // 구조체에 할당된 메모리의 크기를 구한다.
                data = new byte[datasize];                  // 구조체가 복사될 배열
                IntPtr buff = Marshal.AllocHGlobal(datasize);// 비관리 메모리 영역에 구조체 크기만큼의 메모리를 할당한다.
                Marshal.StructureToPtr(obj, buff, false);   // 할당된 구조체 객체의 주소를 구한다.
                Marshal.Copy(buff, data, 0, datasize);      // 구조체 객체를 배열에 복사
                Marshal.FreeHGlobal(buff);                  // 비관리 메모리 영역에 할당했던 메모리를 해제함
                //return data; // 배열을 리턴
            }
            catch (Exception err)
            {
                err.Message.ToString();
                data = null;
            }

            return data;
        }

        //byte 배열을 구조체로 변환해주는 함수
        public static object ByteToStructure(byte[] data, Type type)
        {
            object obj = null;

            try
            {
                IntPtr buff = Marshal.AllocHGlobal(data.Length);// 배열의 크기만큼 비관리 메모리 영역에 메모리를 할당한다.
                Marshal.Copy(data, 0, buff, data.Length);       // 배열에 저장된 데이터를 위에서 할당한 메모리 영역에 복사한다.
                obj = Marshal.PtrToStructure(buff, type);       // 복사된 데이터를 구조체 객체로 변환한다.
                Marshal.FreeHGlobal(buff);                      // 비관리 메모리 영역에 할당했던 메모리를 해제함
                if (Marshal.SizeOf(obj) != data.Length)         // 구조체와 원래의 데이터의 크기 비교
                    obj = null;                                 // 크기가 다르면 null 리턴
                //return obj; // 구조체 리턴
            }
            catch (Exception err)
            {
                err.Message.ToString();
                obj = null;
            }

            return obj;
        }

        public static object ByteToStructure(byte[] data, Type type, int size)
        {
            object obj = null;

            try
            {
                IntPtr buff = Marshal.AllocHGlobal(data.Length);// 배열의 크기만큼 비관리 메모리 영역에 메모리를 할당한다.
                Marshal.Copy(data, 0, buff, data.Length);       // 배열에 저장된 데이터를 위에서 할당한 메모리 영역에 복사한다.
                obj = Marshal.PtrToStructure(buff, type);       // 복사된 데이터를 구조체 객체로 변환한다.
                Marshal.FreeHGlobal(buff);                      // 비관리 메모리 영역에 할당했던 메모리를 해제함
                //if (size != data.Length)         // 구조체와 원래의 데이터의 크기 비교
                //    obj = null;                                 // 크기가 다르면 null 리턴
                //return obj; // 구조체 리턴
            }
            catch (Exception err)
            {
                err.Message.ToString();
                obj = null;
            }

            return obj;
        }

        public static IoInOutType GetInOutType(IoType ioType)
        {
            IoInOutType inoutType = IoInOutType.In;
            switch (ioType)
            {
                case IoType.DI:
                case IoType.AI:
                    {
                        inoutType = IoInOutType.In;
                    }
                    break;
                case IoType.DO:
                case IoType.AO:
                    {
                        inoutType = IoInOutType.Out;
                    }
                    break;
            }

            return inoutType;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Procedures and sample program for increasing the minimum working set area of the PC
        //The following provides measures for increasing the minimum working set area of the PC when an
        //error of error code 77 occurs due to MD function execution, and its sample program.
        //The PC board driver runs using the minimum working set area in the memory area reserved in the
        //application program. Some application program may use a large area of the minimum working set
        //area. In such a case, when the minimum working set area for the PC board driver cannot be
        //reserved, an error code 77 is returned.
        //If this situation occurs, increase the minimum working set area in the application program before
        //executing the MD function. (See the following sample program.)
        //The minimum working set area of 200KB is reserved at startup of the personal computer.
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SetProcessWorkingSet(int minMB, int maxMB)
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr iptrMinWorkingSet = minMB > 0 ? new IntPtr(minMB * 1024 * 1024) : new IntPtr(ProcessWorkingSetDefaultMin);
            IntPtr iptrMaxWorkingSet = new IntPtr(maxMB * 1024 * 1024);
            currentProcess.MinWorkingSet = iptrMinWorkingSet;
            currentProcess.MaxWorkingSet = iptrMaxWorkingSet;
        }

        private static uint ProcessWorkingSetDefaultMin = 204800; //byte
        private static uint ProcessWorkingSetDefaultMax = 1413120; //byte
        public static void SetProcessWorkingSetDefault()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr iptrMinWorkingSet = new IntPtr(ProcessWorkingSetDefaultMin); //default 
            IntPtr iptrMaxWorkingSet = new IntPtr(ProcessWorkingSetDefaultMax); //default
            currentProcess.MinWorkingSet = iptrMinWorkingSet;
            currentProcess.MaxWorkingSet = iptrMaxWorkingSet;
        }

        //문자열이 Digit인지 검사
        public static bool IsDigit(string data)
        {
            if (string.IsNullOrEmpty(data)) return false;

            char[] chars = data.ToCharArray();
            if (chars == null)
            {
                return false;
            }
            else
            {
                bool isDigit = true;
                int count = chars.Length;
                for (int i = 0; i < count; i++)
                {
                    if (!char.IsDigit(chars[i]))
                    {
                        isDigit = false;
                        break;
                    }
                }

                return isDigit;
            }
        }
    }
}

