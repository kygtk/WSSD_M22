using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Util.IODefine
{
    public interface ITerminalFactory
    {
        IoTerminal CreateObject();
    }

    public interface IEcSlaveFactory
    {
        EcSlave CreateObject();
    }
}
