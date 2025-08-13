using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum DmsErrors
    {
        Success = 0,
        UnknownError = 1000,
        NotImplemented,
        InternalError,
        ClientNotRegistered,
        NotInitialized,
        DBNotInitialized,
        DBAlreadyCreated,
        DBConnectionFail,
        DBLoadFail,
        ConfigFileNotFound,
        InitDatabaseNotFound,
        DataUpdationNotCompleted,
        HMINotRegistered,
        UnAuthorisedClient
    }    
}
