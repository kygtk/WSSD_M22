
using System.Collections.Generic;
namespace Dms.Device
{    
    #region Enum
    public enum TactTimeAct
    {
        ttCOUNT_START, ttCOUNT_END
    }
    public enum TactTimeMode
    {
        Impossible, Normal, Delay, None
    }
    public enum RecipeCheck
    {
        Ng, Ok, None
    }
    #endregion

    public interface IJobCondition
    {
        HeavyInterlock HeavyInterlock { get; }
        TagInterlock Interlock { get; set; }
        bool ProcessMode { get; }
        bool SetupIdleUse { get; }
        int SetupIdleRunTime { get; }
        int SetupIdleStopTime { get; }
        Dms.Data.TagRecipe CurrentRecipe { get; set; }
        int CvProcessSpeed(int id);
        bool GetGaugeCond(Dms.Device.CvUnit cv);
        bool RbDirection(Dms.Device.RbUnit unit);
        double RbProcessSpeed(Dms.Device.RbUnit unit);
        double RbGap(Dms.Device.RbUnit unit);
        bool RbUse(Dms.Device.RbUnit unit);
        bool DBDirection(Dms.Device.DBUnit unit);
        double DBProcessSpeed(Dms.Device.DBUnit unit);
        double DBGap(Dms.Device.DBUnit unit);
        bool DBUse(Dms.Device.DBUnit unit);
        void SetRecipe2JobCond(string recipeId);
        void SetTactTime(TactTimeAct act);
        int TactTime { get; }
        void SetRecipeReceiveFromHost();
        bool EuvLampUse(Dms.Device.EuvLamp unit);
        bool EuvUse(Dms.Device._EuvUnit unit);
        bool ApUse(Dms.Device._Plasma unit);
        double ApVoltage(Dms.Device._Plasma unit);
        double ApN2Flow(Dms.Device._Plasma unit);
        double ApCDAFlow(Dms.Device._Plasma unit);
        bool HpmjUse(Dms.Device.Hpmj unit);
        int SetupHpmjRunTime { get; } //lkl 150929
        int SetupHpmjStopTime { get; } //lkl 150929
        int HpmjPressure(Dms.Device.Hpmj unit);
		int GetGlassCountInEqp();//장비 모든 Unit의 Glass count;
		int GetGlassCountInProcess();//장비 Unit중 Process Unit의 Glass coount, ex)HDC는 TR, Hand 제외한 LD(in) ~ UL(in)까지만
		int GetGlassCountMaxCapability();//장비가 한번에 처리할 수 있는 총 매수
		int GetGlassCountPutIntoPossible();//장비에 투입가능한 매수 ex) PutIntoPossible = MaxCapability - GlassCountInEqp
		List<int> GetCurrentAlarmIds();
		List<int> GetCurrnetWarningIds();
        double BasePressure();     //Base Pressure of Process Chamber
        double ProcessPressure(); //Process Pressure of Process Chamber
    }
}
