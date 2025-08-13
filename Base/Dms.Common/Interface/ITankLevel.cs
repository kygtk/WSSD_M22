using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum LevelConfirm
    { 
        NotUsed,
        NotConfirm,
        Confirm,
    }

    public enum LevelDetect
    {
        NotUsed,
        NotDetect,
        Detect,
    }

    public interface ITankLevel
    {
        LevelConfirm TopLevelConfirm
        {
            get;
        }
        LevelConfirm SupplyRequestConfirm
        {
            get;
        }
        LevelConfirm SupplyStopConfirm
        {
            get;
        }
        LevelConfirm BottomLevelConfirm
        {
            get;
        }
        LevelConfirm RunEnableConfirm
        {
            get;
        }


        LevelDetect TopLevelDetect
        {
            get;
        }
        LevelDetect SupplyRequestDetect
        {
            get;
        }
        LevelDetect SupplyStopDetect
        {
            get;
        }
        LevelDetect BottomLevelDetect
        {
            get;
        }
        LevelDetect RunEnableDetect
        {
            get;
        }

        event IoStateChangeEventHandler OnLevelSatateChanged;
    }
}
