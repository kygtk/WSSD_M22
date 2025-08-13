using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dms.Common
{
    public class BaseGlobalVar
    {
        public static bool StartCount;      //Tact Time Count Start
        public static bool StopCount1;
        public static bool TimeOver;        //Tact Time Time Over
        public static bool GlassOutComp1;   //UL's glass 배출여부확인
        public static bool GlassOutComp2;   //HHP CV2's glass 배출여부확인
        public static bool UlTimeOut;
        public static bool LdTimeOut;       //LD's glass 배출여부확인

        public static bool GlassDataLostReq;       //Glass Data Lost Request
        public static bool GlassDataRecoveryReq;   //Glass Data Recovery Request
        public static int LostGlassId;
        public static int RecoveryGlassPos;
        public static int RecoveryGlassNo;
        public static bool Manualcompulsion = false; // 11.02.23 minhan

        public static char EqpState;
        public static bool EqpStatusChangeReq;

        public static bool BuzzerOff;

        public static string DailyDiTotalUseKey = "DailyDiTotalUse";
        public static string DailyGlassProcessCountKey = "DailyGlassProcessCount";
        public static string DailyGlassSavedTimeKey = "DailyGlassSavedTime";//2010.06.29 kimgun
        public static int DailyDiTotalUse;
        public static int DailyGlassProcessCount;
        public static string SavedTime;

        public static bool rcvON_IF2;
        public static bool sndON_IF2;
        public static bool rcvCANCEL;
        public static bool sndEXCANCEL;
        public static bool TrMoveLoadDir;
        public static bool TrMoveUnloadDir;
        public static bool LdHandAirInterlock;
        public static bool UlHandAirInterlock;

        public static bool ExchangeReq; // 11.06.10 minhan
        public static bool LoaderReady; // 10.12.21 minhan
        public static bool NoSubstrate; // 11.02.08 minhan

        private static readonly Mutex m_MutexDailyDiTotalUse = new Mutex();
        public static void IncreaseDailyDiTotalUse()
        {
            m_MutexDailyDiTotalUse.WaitOne();
            DailyDiTotalUse++;
            GeneralData.SetIntValue("GlobalVar", DailyDiTotalUseKey, DailyDiTotalUse);
            m_MutexDailyDiTotalUse.ReleaseMutex();
        }
        public static void ResetDailyDiTotalUse()
        {
            m_MutexDailyDiTotalUse.WaitOne();
            DailyDiTotalUse = 0;
            GeneralData.SetIntValue("GlobalVar", DailyDiTotalUseKey, DailyDiTotalUse);
            m_MutexDailyDiTotalUse.ReleaseMutex();
        }

        private static readonly Mutex m_MutexDailyGlassProcessCount = new Mutex();
        public static void ResetDailyGlassProcessCount()
        {
            m_MutexDailyGlassProcessCount.WaitOne();
            DailyGlassProcessCount = 0;
            GeneralData.SetIntValue("GlobalVar", DailyGlassProcessCountKey, DailyGlassProcessCount);
            m_MutexDailyGlassProcessCount.ReleaseMutex();
        }
        public static void DailyGlassProcessCountSet()
        {
            m_MutexDailyGlassProcessCount.WaitOne();
            GeneralData.SetIntValue("GlobalVar", DailyGlassProcessCountKey, DailyGlassProcessCount);
            m_MutexDailyGlassProcessCount.ReleaseMutex();
        }
        public static void DailySavedTimeSet()
        {
            m_MutexDailyGlassProcessCount.WaitOne();
            GeneralData.SetStringValue("GlobalVar", DailyGlassSavedTimeKey, SavedTime);
            m_MutexDailyGlassProcessCount.ReleaseMutex();
        }
    }
}
