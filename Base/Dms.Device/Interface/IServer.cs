using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Util.IODefine;
using Dms.Data;

namespace Dms.Device
{
    public interface IServer
    {
        //////////////////////////////////////////////////////////////////////////////////////
        // Event Handler
        event UninitializeDelegate UninitializeDel;


        //////////////////////////////////////////////////////////////////////////////////////
        // Server self condition
        bool Initialized { get; }


        //////////////////////////////////////////////////////////////////////////////////////
        // Data Providers
        IoDefines IoDefines { get; }

        //////////////////////////////////////////////////////////////////////////////////////
        // Control object
        IoCollection<IoDigitalInput> DigitalInputs { get; }
        IoCollection<IoDigitalOutput> DigitalOutputs { get; }
        IoCollection<IoAnalogInput> AnalogInputs { get; }
        IoCollection<IoAnalogOutput> AnalogOutputs { get; }

        SlaveCollection<SlaveServo> SlaveServos { get; }
        SlaveCollection<SlaveBLDC> SlaveBLDCs { get; }
        SlaveCollection<SlaveInverter> SlaveInverters { get; }
        SlaveCollection<SlaveDigitalInput> SlaveDigitalInputs { get; }
        SlaveCollection<SlaveDigitalOutput> SlaveDigitalOutputs { get; }
        SlaveCollection<SlaveAnalogInput> SlaveAnalogInputs { get; }
        SlaveCollection<SlaveAnalogOutput> SlaveAnalogOutputs { get; }
        SlaveCollection<SlaveAP> SlaveAPs { get; }

        //////////////////////////////////////////////////////////////////////////////////////
        // Setup Parameter

        #region Methods
        DmsErrors Initialize();
        void Uninitialize();
        #endregion
    }
}
