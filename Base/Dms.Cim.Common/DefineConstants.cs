using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Dms.Cim.Common
{
    public enum enumHostLotStatus
    {
        enumNone = 0,
        enumReady = 'R',
        enumWaitingProcess = 'W',
        enumInProcess = 'I',
        enumNormalEnd = 'T',
        enumAbnormalEnd = 'F',
        enumAbort = 'A',
        enumCanceled = 'C',
    }
    public enum enumHostGlassStatus
    {
        enumIdle = 'N',
        enumInprocess = 'I',
        enumNormalEnd = 'T',
        enumAbnormalEnd = 'F',
        enumAbort = 'A',
        enumScrap = 'S',
    }

    public enum enumLoaderInfo
    {
        enumloader = 0,
        enumUnloader = 1
    }

    public enum enumHostPortStatus
    {
        enumLoadRequest = 1,
        enumLoadComplete = 2,
        enumUnloadRequest = 3,
        enumUnloadComplete = 4,
        enumEnable = 5,
        enumDisable = 6,
    }

    public enum enumDialog
    {
        enumDialogAdd,
        enumDialogModify,
        enumDialogView,
    }

    public enum enumUnitNo
    {
        enumCIM          = 0,
        enumLOADER       = 1,
        enumPPCLN        = 2,
        enumSKYCVA       = 3,
        enumSKYCVB       = 4,
        enumSKYCVC       = 5,
        enumSKYCVD       = 6,
        enumAPCP1BUFFER  = 7,
        enumAPCP1        = 8,
        enumAPCP2BUFFER  = 9,
        enumAPCP2        = 10,
        enumCOATER1      = 11,
        enumVCD1         = 12,
        enumSHPCP1BUFFER = 13,
        enumSHPCP1       = 14,
        enumCOATER2      = 15,
        enumVCD2         = 16,
        enumSHPCP2BUFFER = 17,
        enumSHPCP2       = 18,
        enumTCU1         = 19,
        enumTCU2         = 20,
        enumDEVELOPER    = 21,
        enumSKYCVE       = 22,
        enumSKYCVF       = 23,
        enumHHPCP1BUFFER = 24,
        enumHHPCP1       = 25,
        enumHHPCP2BUFFER = 26,
        enumHHPCP2       = 27,
        enumAOI          = 28,
        enumSKYULCV      = 29,
        enumUNLOADER     = 30,
        enumEXPOSURE1    = 31,
        enumEXPOSURE2    = 32,
    }

    public class DefineConstants
    {
        public const int MaxSlotNo = 50;
        public const int LoaderPortCount = 3;

        public enum enumStageNo
        {
            enumPort1 = 1,
            enumPort2 = 2,
            enumPort3 = 3,
            enumPort4 = 4,
            enumPort5 = 5,
            enumEQStage1 = 9,
            enumEQStage2 = 10,
            enumEQStage3 = 11,
            enumEQStage4 = 12,
        }
    }

    public class LotColorInfo
    {
        private static Color[] m_LotColor = {Color.Beige, Color.Yellow, Color.HotPink, Color.SkyBlue, Color.LimeGreen, Color.DarkGoldenrod, Color.DarkCyan,
                                             Color.Cyan, Color.Lime, Color.DarkSeaGreen, Color.LightPink, Color.Violet, Color.Gold };


        public LotColorInfo()
        {

        }

        public static Color GetColor( int lotno)
        {
            int nRemainder = lotno % m_LotColor.Length;

            return m_LotColor[nRemainder];
        }

    }


}
