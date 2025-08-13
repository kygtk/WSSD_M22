using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dms.Common;

namespace Dms.ServerCommon
{
    public class GenInfoHandler : _GenInfoHandler
    {
        #region Additional GenInfo Items : GenInfo형식으로 만들 data(DeviceTag형태가 될 멤버들)
        public int _LoopBackTest;
        public int _OnlineMode;
        public int _OfflineMode;
        public int _NormalMode;
        public int _RecoveryMode;
        public int _ParticleMode;
        public int _GlassMoveReq;
        public int _LdInterfaceStatus;
        public int _UlInterfaceStatus;
        public int _TimeOutMargin;
        public int _HpmjInHz;// 11.01.31 minhan
        public int _HpmjCurrent; // 11.01.31 minhan
        public int _HpmjMainDI; // 11.01.31 minhan
        public int _HpmjFlow; // 11.01.31 minhan
        public int _HpmjInPress; // 11.01.31 minhan
        public int _HpmjOutPress;// 11.01.31 minhan
        public int _HpmjCo2Press; // 11.01.31 minhan
        public int _HpmjResistivity; // 11.01.31 minhan
        public int _HpmjTargetFlow;//090923 LeeChungWon
        public int _HpmjTargetPress;//090923 LeeChungWon
        public int _HpmjRunningTime; // 11.01.31 minhan
        public int _HpmjPackingTime; // 11.01.31 minhan
        public int _HpmjFilterTime; // 11.01.31 minhan
        public int _HpmjCo2BubblerTime; // 11.01.31 minhan
        public int _HpmjPackingTimeSet; // 11.01.31 minhan
        public int _HpmjFilterTimeSet; // 11.01.31 minhan
        public int _HpmjCo2BubblerTimeSet; // 11.01.31 minhan
        public int _HpmjCo2Flow; // 11.05.03 minhan
        public int _SingRunMode; // 09.10.30 minhan
        //public int _ServoStatus; // 10.12.25 minhan
        //public int _BypassCheck; // 10.12.25 minhan
        //public int _CylActTime; //10.06.16 taegoo 11.02.01 minhan
        public int _ManualIFAct;//2010.06.21 kimgun
        public int _TodayGlassCount;//2010.06.29 kimgun
        public int _CrackStatus; //2010.11.29 kang
        public int _ServoStatusTR; // 10.12.25 minhan
        public int _ServoStatusLD; // 10.12.25 minhan
        public int _ServoStatusUL; // 10.12.25 minhan
        public int _ServoStatusRB; // 10.12.25 minhan
        public int _Rb1UpGap; // 10.02.01 minhan
        public int _Rb1LoGap; // 10.02.01 minhan
        public int _Rb2UpGap; // 10.02.01 minhan
        public int _Rb2LoGap; // 10.02.01 minhan
        public int _Rb1Position; // 10.02.01 minhan
        public int _Rb2Position; // 10.02.01 minhan
        public int _Rb3Position; // 10.02.01 minhan
        public int _Rb4Position; // 10.02.01 minhan
        public int _LDCvSpeed; // 11.02.01 minhan
        public int _SwrCvSpeed; // 11.02.01 minhan
        public int _ULCvSpeed; // 11.02.01 minhan

        public int _EuvLampInt1;
        public int _EuvLampInt2;
        public int _EuvLampInt3;
        #endregion

        #region Properties for GenInfo Handle
        public bool LoopBackTest
        {
            get { return (m_InfoCollection[_LoopBackTest] == bool.TrueString); }
            set { m_InfoCollection[_LoopBackTest] = value.ToString(); }
        }
        public bool OnlineMode
        {
            get { return (m_InfoCollection[_OnlineMode] == bool.TrueString); }
            set { m_InfoCollection[_OnlineMode] = value.ToString(); }
        }
        public bool OfflineMode
        {
            get { return (m_InfoCollection[_OfflineMode] == bool.TrueString); }
            set { m_InfoCollection[_OfflineMode] = value.ToString(); }
        }
        public bool NormalMode
        {
            get { return (m_InfoCollection[_NormalMode] == bool.TrueString); }
            set { m_InfoCollection[_NormalMode] = value.ToString(); }
        }
        public bool RecoveryMode
        {
            get { return (m_InfoCollection[_RecoveryMode] == bool.TrueString); }
            set { m_InfoCollection[_RecoveryMode] = value.ToString(); }
        }
        public bool ParticleMode
        {
            get { return (m_InfoCollection[_ParticleMode] == bool.TrueString); }
            set { m_InfoCollection[_ParticleMode] = value.ToString(); }
        }
        public string GlassMoveReq
        {
            get { return m_InfoCollection[_GlassMoveReq]; }
            set { m_InfoCollection[_GlassMoveReq] = value; }
        }
        public string LdInterfaceStatus
        {
            get { return m_InfoCollection[_LdInterfaceStatus]; }
            set { m_InfoCollection[_LdInterfaceStatus] = value; }
        }
        public string UlInterfaceStatus
        {
            get { return m_InfoCollection[_UlInterfaceStatus]; }
            set { m_InfoCollection[_UlInterfaceStatus] = value; }
        }
        public string TimeOutMargin
        {
            get { return m_InfoCollection[_TimeOutMargin]; }
            set { m_InfoCollection[_TimeOutMargin] = value; }
        }
        public string HpmjInHz
        {//2009.09.21 kimgun
            get { return m_InfoCollection[_HpmjInHz]; }
            set { m_InfoCollection[_HpmjInHz] = value; }
        }
        public string HpmjCurrent
        {
            get { return m_InfoCollection[_HpmjCurrent]; }
            set { m_InfoCollection[_HpmjCurrent] = value; }
        }
        public string HpmjMainDI
        {
            get { return m_InfoCollection[_HpmjMainDI]; }
            set { m_InfoCollection[_HpmjMainDI] = value; }
        }
        public string HpmjFlow
        {
            get { return m_InfoCollection[_HpmjFlow]; }
            set { m_InfoCollection[_HpmjFlow] = value; }
        }
        public string HpmjInPress
        {
            get { return m_InfoCollection[_HpmjInPress]; }
            set { m_InfoCollection[_HpmjInPress] = value; }
        }
        public string HpmjOutPress
        {
            get { return m_InfoCollection[_HpmjOutPress]; }
            set { m_InfoCollection[_HpmjOutPress] = value; }
        }
        public string HpmjCo2Press
        {
            get { return m_InfoCollection[_HpmjCo2Press]; }
            set { m_InfoCollection[_HpmjCo2Press] = value; }
        }
        public string HpmjResistivity
        {
            get { return m_InfoCollection[_HpmjResistivity]; }
            set { m_InfoCollection[_HpmjResistivity] = value; }
        }
        public string HpmjTargetFlow
        {//090923 : LeeChungWon
            get { return m_InfoCollection[_HpmjTargetFlow]; }
            set { m_InfoCollection[_HpmjTargetFlow] = value; }
        }
        public string HpmjTargetPress
        {//090923 : LeeChungWon
            get { return m_InfoCollection[_HpmjTargetPress]; }
            set { m_InfoCollection[_HpmjTargetPress] = value; }
        }
        public string HpmjRunningTime // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjRunningTime]; }
            set { m_InfoCollection[_HpmjRunningTime] = value; }
        }
        public string HpmjPackingTime // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjPackingTime]; }
            set { m_InfoCollection[_HpmjPackingTime] = value; }
        }
        public string HpmjFilterTime // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjFilterTime]; }
            set { m_InfoCollection[_HpmjFilterTime] = value; }
        }
        public string HpmjCo2BubblerTime // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjCo2BubblerTime]; }
            set { m_InfoCollection[_HpmjCo2BubblerTime] = value; }
        }
        public string HpmjPackingTimeSet // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjPackingTimeSet]; }
            set { m_InfoCollection[_HpmjPackingTimeSet] = value; }
        }
        public string HpmjFilterTimeSet // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjFilterTimeSet]; }
            set { m_InfoCollection[_HpmjFilterTimeSet] = value; }
        }
        public string HpmjCo2BubblerTimeSet // 11.01.31 minhan
        {
            get { return m_InfoCollection[_HpmjCo2BubblerTimeSet]; }
            set { m_InfoCollection[_HpmjCo2BubblerTimeSet] = value; }
        }
        public string HpmjCo2Flow // 11.05.03 minhan
        {
            get { return m_InfoCollection[_HpmjCo2Flow]; }
            set { m_InfoCollection[_HpmjCo2Flow] = value; }
        }
        public bool SingleRunMode // 09.10.30 minhan
        {
            get { return (m_InfoCollection[_SingRunMode] == bool.TrueString); }
            set { m_InfoCollection[_SingRunMode] = value.ToString(); }
        }
        public string ServoStatusTR //10.12.25 minhan
        {
            get { return m_InfoCollection[_ServoStatusTR]; }
            set { m_InfoCollection[_ServoStatusTR] = value; }
        }
        public string ServoStatusLD //10.12.25 minhan
        {
            get { return m_InfoCollection[_ServoStatusLD]; }
            set { m_InfoCollection[_ServoStatusLD] = value; }
        }
        public string ServoStatusUL //10.12.25 minhan
        {
            get { return m_InfoCollection[_ServoStatusUL]; }
            set { m_InfoCollection[_ServoStatusUL] = value; }
        }
        public string ServoStatusRB //10.12.25 minhan
        {
            get { return m_InfoCollection[_ServoStatusRB]; }
            set { m_InfoCollection[_ServoStatusRB] = value; }
        }
        public string LDCvSpeed // 11.02.01 minhan
        {
            get { return m_InfoCollection[_LDCvSpeed]; }
            set { m_InfoCollection[_LDCvSpeed] = value; }
        }
        public string SwrCvSpeed // 11.02.01 minhan
        {
            get { return m_InfoCollection[_SwrCvSpeed]; }
            set { m_InfoCollection[_SwrCvSpeed] = value; }
        }
        public string ULCvSpeed // 11.02.01 minhan
        {
            get { return m_InfoCollection[_ULCvSpeed]; }
            set { m_InfoCollection[_ULCvSpeed] = value; }
        }
        public string EuvLampIntensity1  // dsptemp
        {
            get { return m_InfoCollection[_EuvLampInt1]; }
            set { m_InfoCollection[_EuvLampInt1] = value; }
        }
        public string EuvLampIntensity2
        {
            get { return m_InfoCollection[_EuvLampInt2]; }
            set { m_InfoCollection[_EuvLampInt2] = value; }
        }
        public string EuvLampIntensity3
        {
            get { return m_InfoCollection[_EuvLampInt3]; }
            set { m_InfoCollection[_EuvLampInt3] = value; }
        }
        //public string CylActTime //kang 11.02.01 minhan
        //{
        //    get { return m_InfoCollection[_CylActTime]; }
        //    set { m_InfoCollection[_CylActTime] = value; }
        //}
        public string CrackStatus //kang
        {
            get { return m_InfoCollection[_CrackStatus]; }
            set { m_InfoCollection[_CrackStatus] = value; }
        }
        public string ManualIFAct
        {//2010.06.21 kimgun
            get { return m_InfoCollection[_ManualIFAct]; }
            set { m_InfoCollection[_ManualIFAct] = value; }
        }
        public int TodayGlassCount
        {//2010.06.29 kimgun
            get { return Convert.ToInt32(m_InfoCollection[_TodayGlassCount]); }
            set
            {
                m_InfoCollection[_TodayGlassCount] = string.Format("{0}", value);
                BaseGlobalVar.DailyGlassProcessCount = value;
                BaseGlobalVar.DailyGlassProcessCountSet();
            }
        }
        public string Rb1UpGap // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb1UpGap]; }
            set { m_InfoCollection[_Rb1UpGap] = value; }
        }
        public string Rb1LoGap // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb1LoGap]; }
            set { m_InfoCollection[_Rb1LoGap] = value; }
        }
        public string Rb2UpGap // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb2UpGap]; }
            set { m_InfoCollection[_Rb2UpGap] = value; }
        }
        public string Rb2LoGap // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb2LoGap]; }
            set { m_InfoCollection[_Rb2LoGap] = value; }
        }
        public string Rb1Position // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb1Position]; }
            set { m_InfoCollection[_Rb1Position] = value; }
        }
        public string Rb2Position // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb2Position]; }
            set { m_InfoCollection[_Rb2Position] = value; }
        }
        public string Rb3Position // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb3Position]; }
            set { m_InfoCollection[_Rb3Position] = value; }
        }
        public string Rb4Position // 10.02.01 minhan
        {
            get { return m_InfoCollection[_Rb4Position]; }
            set { m_InfoCollection[_Rb4Position] = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly GenInfoHandler Instance = new GenInfoHandler();
        #endregion

        #region Constructor
        private GenInfoHandler()
        {
            MakeNameList();
            GenerateGenInfo();
        }
        #endregion

        #region Override
        #endregion

        #region Methods
        public void CreateTags(DeviceTags tagContainer)
        {
            m_InfoCollection.CreateTags(tagContainer);

            TotalGlassCount = m_AppData.TotalGlassCount;
            CurGlassCount = m_AppData.CurGlassCount;
            ApdReportIndex = m_AppData.ApdReportIndex;
            TodayGlassCount = BaseGlobalVar.DailyGlassProcessCount;
        }

        public void SyncTags(DeviceTags tagContainer)
        {
            m_InfoCollection.SyncTags(tagContainer);
        }
        #endregion
    }
}
