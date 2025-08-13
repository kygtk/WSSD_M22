using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public interface IDMSComm
    {
        void GetInitialData();
        bool SendCommand(String clientId, Command cmd, params Object[] obj);
    }
}
