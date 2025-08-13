using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public interface ICtlDevice
    {
        #region Properties
        bool Initialized { get; set; }
        bool Uninitializing { get; set; }
        ActiveState DeviceState { get; set; }
        string ControllerState { get; }
        int DiCount { get; }
        int DoCount { get; }
        int AiCount { get; }
        int AoCount { get; }

        bool IsAdvDevice { get; }
        #endregion

        #region Methods
        #region DI
        bool ReadDiSync(int index);
        bool ReadDiAsync(int index);
        void WriteDiSync(int index, bool val);
        #endregion

        #region DO
        bool ReadDoSync(int index);
        bool ReadDoAsync(int index);
        void WriteDoSync(int index, bool val);
        void WriteDoAsync(int index, bool val);
        #endregion

        #region AI
        short ReadAiSync(int index);
        short ReadAiAsync(int index);
        void WriteAiSync(int index, short val);
        #endregion

        #region AO
        ushort ReadAoSync(int index);
        ushort ReadAoAsync(int index);
        void WriteAoSync(int index, ushort val);
        void WriteAoAsync(int index, ushort val);
        #endregion

        //object Read(int index, int type);
        object Read(int index, int group, int dataType, int node);                //IoAnalogInput, IoDigitalInput의 GetStateAs()에서 호출됨
        object Read(int index, IoType type, int group, int dataType, int node); //ViewIOEdit에서 호출됨

        void WriteSync(int index, object val, IoType type);         //사용안함
        void Write(int index, object val, int group, int dataType, int node);                 //IoAnalogOutput, IoDigitalOutput의 SetStateSync()에서 호출됨
        void Write(int index, object val, IoType type, int group, int dataType, int node); //ViewIOEdit에서 호출됨

        void Uninitialize();
        #endregion

        #region Events
        event IoStateChangeEventHandler OnIoStateChange;
        #endregion
    }
}
