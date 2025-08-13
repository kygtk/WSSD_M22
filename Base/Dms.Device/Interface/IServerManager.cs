using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Util.IODefine;
using Dms.Data;

namespace Dms.Device
{
    public interface IServerManager : IServer
    {
        //////////////////////////////////////////////////////////////////////////////////////
        // Event Handler

        //////////////////////////////////////////////////////////////////////////////////////
        // Server self condition
        ActiveState State { get; }

        //////////////////////////////////////////////////////////////////////////////////////
        // General Variables
        XLog EqpLog { get; }

        //////////////////////////////////////////////////////////////////////////////////////
        // Data Providers
        IComponentContainer ComponentContainer { get; }
        DeviceTags TagContainer { get; }
        IJobCondition JobCond { get; }
        IGlassDataHandler GlassData { get; }
        //AlarmListProvider AlarmList { get; }
        //SetupGenInfoProvider SetupGenInfo { get; }
        //SetupIdleInfoProvider SetupIdleInfo { get; }
        //SetupCvDistanceProvider SetupCvDistance { get; }
        //SetupCvInfoProvider SetupCvInfo { get; }
        //SetupSensorTimeoutProvider SetupSensorTimeout { get; }
        //SetupSensorInterlockProvider SetupSensorIntr { get; }
        //SetupTankLevelProvider SetupTankLevel { get; }
        //SetupDevTankLevelProvider SetupDevTankLevel { get; }
        //SetupGaugeInterlockProvider SetupGaugeInterlock { get; }
        //SetupHpmjInfoProvider SetupHpmjInfo { get; }
        //CalibrationProvider CalibrationProvider { get; }
        //ApdItemsHandler ApdItemsHandler { get; }


        //////////////////////////////////////////////////////////////////////////////////////
        // Control object
        ICtlDevice IoController { get; }
        ICtlDevice EcController { get; }
        bool ControllerIsRun { get; }
        IMotionControl MotionController { get; }
        IEqpManager EqpStateManager { get; }

        //////////////////////////////////////////////////////////////////////////////////////
        // Setup Parameter
        TagSetupInfo SetupGlassSize { get; }
        TagSetupInfo SetupIdleWaitTime { get; }

        DataProvider DataProvider { get; }

        int GetSetupMaxGlassNo { get; }
        int GetSetupCvStopSpeed { get; }
        bool IsSingleMode { get; }

        bool Created { get; }

        #region Methods
        void UninitializeByException();
        void Start();
        void Stop();
        void CommandProc(Command cmd, params Object[] para);
        void WriteExceptionLog(string log);
        void Log(string log);
        void AddSeqInitFunction(XSeqInitFunction func);
        #endregion
    }
}
