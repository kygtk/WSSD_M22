using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum eqpMODE
    {
        eqpRemote,
        eqpLocal,
        eqpManual,
    }

    public enum eqpProcessMode
    {
        eqpAuto,
        eqpManual,
    }

    public enum eqpSTATUS
    {
        eqpNone,
        eqpIdle = 'I',
        eqpRun = 'R',
        eqpPause = 'P',
        eqpDown = 'D',
        eqpAssistWait = 'A',
        eqpMaint = 'M',
    }

    public enum eqpSUBSTATUS
    {
        eqpsubNone = ' ',
        eqpsubStartAssist = 'S',
        eqpsubEndAssist = 'E',
    }

    public enum eqpDETOUR_MODE
    {
        eqpNormal = 1,
        eqpDetour = 2,
    }

    public enum eqpTACT_TIME_CONTROL_MODE
    {
        eqpImpossible = 0,
        eqpNormal = 1,
        eqpDelay = 2,
    }

    public enum eqpRecoveryType
    {
        eqpRECOVERY = 1,
        epqRESTORATION = 2,
    }

    public enum eqpRECOVERY_STATUS
    {
        eqpNone = 0,
        eqpNormal = 1,
        eqpPrepare = 2,
        eqpReady = 3,
        eqpRecovery = 4,
        eqpRestoration = 5,
        eqpFail = 6,
    }

    public enum eqpCommand
    {
        OriginPoint,
        Prepare,
        InitializeGlassData,
        BuzzerStop,
        AlarmReset,
    }

    [Flags]
    public enum eqpSETUP_STATUS
    {
        None = 0,
        OriginComplete = 1 << 0,
        PrepareComplete = 1 << 1,
        GlassDataInitComplete = 1 << 2,
        All = Int16.MaxValue,
    }
}
