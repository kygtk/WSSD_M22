using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: SecurityPermission(
    SecurityAction.RequestMinimum, Execution = true)]
// This class includes several Win32 interop definitions.
namespace Dms.Common
{
    internal class Win32
    {
        public static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
        public const uint FILE_MAP_WRITE = 0x02;
        public const uint PAGE_READWRITE = 0x04;
        public const uint FILE_MAP_ALL_ACCESS = 0x000F0000 | 0x0001 | 0x0002 | 0x0004 | 0x0008 | 0x0010;

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFileMapping(IntPtr hFile,
            IntPtr pAttributes, uint flProtect,
            uint dwMaximumSizeHigh, uint dwMaximumSizeLow, String pName);

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenFileMapping(uint dwDesiredAccess,
            Boolean bInheritHandle, String name);

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern Boolean CloseHandle(IntPtr handle);

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh, uint dwFileOffsetLow,
            IntPtr dwNumberOfBytesToMap);

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern Boolean UnmapViewOfFile(IntPtr address);

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern Boolean DuplicateHandle(IntPtr hSourceProcessHandle,
            IntPtr hSourceHandle,
            IntPtr hTargetProcessHandle, ref IntPtr lpTargetHandle,
            uint dwDesiredAccess, Boolean bInheritHandle, uint dwOptions);
        public const uint DUPLICATE_CLOSE_SOURCE = 0x00000001;
        public const uint DUPLICATE_SAME_ACCESS = 0x00000002;

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetCurrentProcess();
    }

    // This class wraps memory that can be simultaneously 
    // shared by multiple AppDomains and Processes.
    [Serializable]
    public class XFileMap : ISerializable, IDisposable
    {
        // The handle and string that identify 
        // the Windows file-mapping object.
        private IntPtr m_hFileMap = IntPtr.Zero;
        private String m_name;

        // The address of the memory-mapped file-mapping object.
        private IntPtr m_address;

        public unsafe Int32* Address
        {
            get { return (Int32*)m_address; }
        }

        // The constructors.

        public XFileMap()
        {
        }

        public XFileMap(Int32 size) : this(size, null) { }

        public XFileMap(Int32 size, String name)
        {
            m_hFileMap = Win32.CreateFileMapping(Win32.InvalidHandleValue, IntPtr.Zero, Win32.PAGE_READWRITE, 0, unchecked((uint)size), name);

            if (m_hFileMap == IntPtr.Zero) throw new Exception("Could not create memory-mapped file.");
            m_name = name;
            m_address = Win32.MapViewOfFile(m_hFileMap, Win32.FILE_MAP_ALL_ACCESS, 0, 0, IntPtr.Zero);
        }

        public bool CreateFileMap(Int32 size, string name)
        {
            m_hFileMap = Win32.CreateFileMapping(Win32.InvalidHandleValue, IntPtr.Zero, Win32.PAGE_READWRITE, 0, unchecked((uint)size), name);

            if (m_hFileMap == IntPtr.Zero) return false;
            m_name = name;
            m_address = Win32.MapViewOfFile(m_hFileMap, Win32.FILE_MAP_ALL_ACCESS, 0, 0, IntPtr.Zero);

            return true;
        }

        public bool OpenFileMap(String name)
        {
            m_hFileMap = Win32.OpenFileMapping(Win32.FILE_MAP_ALL_ACCESS, false, name);

            if (m_hFileMap == IntPtr.Zero) return false;

            m_address = Win32.MapViewOfFile(m_hFileMap, Win32.FILE_MAP_ALL_ACCESS, 0, 0, IntPtr.Zero);

            return true;
        }

        // The cleanup methods.
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(Boolean disposing)
        {
            Win32.UnmapViewOfFile(m_address);
            Win32.CloseHandle(m_hFileMap);
            m_address = IntPtr.Zero;
            m_hFileMap = IntPtr.Zero;
        }

        ~XFileMap()
        {
            Dispose(false);
        }

        unsafe public bool ReceiveBit(Int32 devNo)
        {
            bool rv = false;

            if (*(this.Address + devNo) == 1) rv = true;

            return rv;
        }

        unsafe public bool SendBit(Int32 devNo, bool data)
        {
            if (devNo < 0) return false;

            if (data) *(this.Address + devNo) = Convert.ToByte(true);
            else *(this.Address + devNo) = Convert.ToByte(false);

            return true;
        }

        unsafe public Int32 ReceiceWord(Int32 devNo)
        {
            Int32 rv = *(this.Address + devNo);

            return rv;
        }

        unsafe public bool SendWord(Int32 devNo, Int32 data)
        {
            if (data < 0) return false;

            *(this.Address + devNo) = data;

            return true;
        }

        unsafe public bool ReadString(Int32 devNo, ref string str, int len)
        {
            int length = len / 2 + len % 2;

            for (int i = 0; i < length; i++)
            {
                Int32 data = *(this.Address + devNo + i);

                char ch = Convert.ToChar(data & 0x00FF);

                if (ch == 0) ch = ' ';

                str += ch;

                ch = Convert.ToChar((data >> 8) & 0x00FF);
                if (ch == 0) ch = ' ';
                str += ch;
            }

            return true;
        }

        unsafe public bool WriteString(Int32 devNo, string str, int len)
        {
            int dataLength = len / 2 + len % 2;

            for (int i = 0; i < dataLength; i++)
            {
                //short size = 2;
                Int32 data;
                char ch1, ch2;

                if (str.Length < i * 2) ch1 = Convert.ToChar(0);
                else ch1 = str[i * 2];

                if (str.Length < i * 2 + 1) ch2 = Convert.ToChar(0);
                else ch2 = str[i * 2 + 1];

                if (ch1 == ' ') ch1 = Convert.ToChar(0x20);	// Space는 그대로 ...........
                if (ch2 == ' ') ch2 = Convert.ToChar(0x20);
                if (ch1 == '\r') ch1 = Convert.ToChar(0x00);	// Space는 그대로 ...........
                if (ch2 == '\r') ch2 = Convert.ToChar(0x00);

                data = (ch2 << 8) | ch1;

                SendWord(Convert.ToByte(devNo + i), data);
            }


            return true;
        }

        // Private helper methods.
        private static Boolean AllFlagsSet(Int32 flags, Int32 flagsToTest)
        {
            return (flags & flagsToTest) == flagsToTest;
        }

        private static Boolean AnyFlagsSet(Int32 flags, Int32 flagsToTest)
        {
            return (flags & flagsToTest) != 0;
        }


        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // The context's State member indicates
            // where the object will be deserialized.

            // A SharedMemory object cannot be serialized 
            // to any of the following destinations.
            const StreamingContextStates InvalidDestinations =
                      StreamingContextStates.CrossMachine |
                      StreamingContextStates.File |
                      StreamingContextStates.Other |
                      StreamingContextStates.Persistence |
                      StreamingContextStates.Remoting;
            if (AnyFlagsSet((Int32)context.State, (Int32)InvalidDestinations))
                throw new SerializationException("The SharedMemory object " +
                    "cannot be serialized to any of the following streaming contexts: " +
                    InvalidDestinations);

            const StreamingContextStates DeserializableByHandle =
                      StreamingContextStates.Clone |
                      // The same process.
                      StreamingContextStates.CrossAppDomain;
            if (AnyFlagsSet((Int32)context.State, (Int32)DeserializableByHandle))
                info.AddValue("hFileMap", m_hFileMap);

            const StreamingContextStates DeserializableByName =
                      // The same computer.
                      StreamingContextStates.CrossProcess;
            if (AnyFlagsSet((Int32)context.State, (Int32)DeserializableByName))
            {
                if (m_name == null)
                    throw new SerializationException("The SharedMemory object " +
                        "cannot be serialized CrossProcess because it was not constructed " +
                        "with a String name.");
                info.AddValue("name", m_name);
            }
        }

        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        private XFileMap(SerializationInfo info, StreamingContext context)
        {
            // The context's State member indicates 
            // where the object was serialized from.

            const StreamingContextStates InvalidSources =
                      StreamingContextStates.CrossMachine |
                      StreamingContextStates.File |
                      StreamingContextStates.Other |
                      StreamingContextStates.Persistence |
                      StreamingContextStates.Remoting;
            if (AnyFlagsSet((Int32)context.State, (Int32)InvalidSources))
                throw new SerializationException("The SharedMemory object " +
                    "cannot be deserialized from any of the following stream contexts: " +
                    InvalidSources);

            const StreamingContextStates SerializedByHandle =
                      StreamingContextStates.Clone |
                      // The same process.
                      StreamingContextStates.CrossAppDomain;
            if (AnyFlagsSet((Int32)context.State, (Int32)SerializedByHandle))
            {
                try
                {
                    Win32.DuplicateHandle(Win32.GetCurrentProcess(),
                        (IntPtr)info.GetValue("hFileMap", typeof(IntPtr)),
                        Win32.GetCurrentProcess(), ref m_hFileMap, 0, false,
                        Win32.DUPLICATE_SAME_ACCESS);
                }
                catch (SerializationException)
                {
                    throw new SerializationException("A SharedMemory was not serialized " +
                        "using any of the following streaming contexts: " +
                        SerializedByHandle);
                }
            }

            const StreamingContextStates SerializedByName =
                      // The same computer.
                      StreamingContextStates.CrossProcess;
            if (AnyFlagsSet((Int32)context.State, (Int32)SerializedByName))
            {
                try
                {
                    m_name = info.GetString("name");
                }
                catch (SerializationException)
                {
                    throw new SerializationException("A SharedMemory object was not " +
                        "serialized using any of the following streaming contexts: " +
                        SerializedByName);
                }
                m_hFileMap = Win32.OpenFileMapping(Win32.FILE_MAP_WRITE, false, m_name);
            }
            if (m_hFileMap != IntPtr.Zero)
            {
                m_address = Win32.MapViewOfFile(m_hFileMap, Win32.FILE_MAP_WRITE,
                    0, 0, IntPtr.Zero);
            }
            else
            {
                throw new SerializationException("A SharedMemory object could not " +
                    "be deserialized.");
            }
        }
    }
}
