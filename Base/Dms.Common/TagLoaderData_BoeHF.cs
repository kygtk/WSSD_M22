using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum boeRbtCommandCode
    {
        NONE = 0,
        RESET = 1,
        GET_WAIT = 2,
        PUT_WAIT = 3,
        GET = 4,
        PUT = 5,
        EXCHANGE = 6,
    }

    public enum boeRbtCommandArm
    {
        UPPER_HAND = 1,
        LOWER_HAND = 2,
    }

    public enum boeRbtCommandTarget
    {
        PORT = 1,
        EQP = 2,
        VCR = 3,
    }

    //public enum boePortCommand
    //{
    //    LOAD_REQUEST = 1,
    //    UNLOAD_REQUEST = 2,
    //    CLAMPING = 3,
    //    CSTID_READ = 4,
    //    MAPPING = 5,
    //}

    public enum PortCommandCode
    {
        None = 0,
        LoadRequest = 1,
        UnloadRequest = 2,
        Clamping = 3,
        CSTIDRead = 4,
        Mapping = 5,
        GlassThick = 6, // Auto에서는 사용하지 않고 Manual일때만 사용 (Glass Thick Data Send라는 시나리오 따로 있음)
    }

    public enum ThicknessKind
    {
        T040 = 1,
        T050 = 2,
        T063 = 3,
        T070 = 4,
        T110 = 5,
    }

    public enum boeRbtCommandResult
    {
        NORMAL_END = 1,
        ABNORMAL_END = 2,
        ILLEGAL_COMMAND = 3,
        TIMEOUT_ERROR = 4,
        SCRAP = 10,
        RETRY = 11,
    }

    public enum boeRbtOpDirection
    {
        PORT1 = 1,
        PORT2 = 2,
        PORT3 = 3,
        PORT4 = 4,
        PORT5 = 5,
        PORT6 = 6,
        PORT7 = 7,
        PORT8 = 8,
        STAGE1 = 9,
        STAGE2 = 10,
        STAGE3 = 11,
        STAGE4 = 12,
        VCR = 13,
    }
}
