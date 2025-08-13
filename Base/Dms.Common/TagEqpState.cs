///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.15
// Author       : jemoon
// Description  : enum for EqpState
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    [Serializable()]
    public enum EqpState
    { 
        UnKnown,
        Normal,
        Fault,
        PM,
    }

    [Serializable()]
    public enum ProcessState
    {
        //UnKnown,    //처음엔 UnKnown이었다가 바로 Init으로 변경
        //Init,       //Initialize하기 전까지
        //Idle,       //Initialize완료 후
        ////Setup,    //Recipe가 바뀌어서 LD에서 출발하지 않고 해당 Recipe를 적용중일 때
        ////Ready,    //Glass가 해당 unit을 빠져나간 후?
        //Excute,     //==Run
        //Pause,

        UnKnown,
        Init,
        Idle,
        Excute,
        Pause,
        Setup,
    }
}
