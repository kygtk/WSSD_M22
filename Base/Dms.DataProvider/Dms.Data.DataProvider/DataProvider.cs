using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dms.Util.IODefine;
using Dms.Common;
using System.Windows.Forms;

namespace Dms.Data
{
    public class DataProvider
    {
        #region Fields
        private IoDefines m_IoDefines;
        private DeviceTags m_DeviceTagContainer;
        private RecipeProvider m_RecipeProvider;
        private SetupGenInfoProvider m_SetupGenInfoProvider;
        private SetupIdleInfoProvider m_SetupIdleInfoProvider;
        private SetupGaugeInterlockProvider m_SetupGaugeInterlockProvider;
        private SetupCvDistanceProvider m_SetupCvDistanceProvider;
        private SetupCvInfoProvider m_SetupCvInfoProvider;
        private SetupSensorTimeoutProvider m_SetupSensorTimeoutProvider;
        private SetupSensorInterlockProvider m_SetupSensorIntrProvider;
        private SetupTankLevelProvider m_SetupTankLevelProvider;
        private SetupDevTankLevelProvider m_SetupDevTankLevelProvider;
        private SetupHpmjInfoProvider m_SetupHpmjInfoProvider;
        // private SetupInterfaceProvider m_SetupInterfaceInfoProvider; // 11.02.01 minhan
        //private SetupHsmsInfoProvider m_SetupHsmsInfoProvider;
        //private SetupHsmsEqpInfoProvider m_SetupHsmsEqpInfoProvider;
        //private SetupFtpInfoProvider m_SetupFtpInfoProvider;
        private AlarmListProvider m_AlarmListProvider;
        private AlarmHistoryProvider m_AlarmHistoryProvider;
        private CurrentAlarmsProvider m_CurrentAlarmsProvider;
        private CalibrationProvider m_CalibrationProvider;
        private GlassDataProvider m_GlassDataProvider;
        private SetupInterfaceProvider m_SetupInterfaceProvider;
        private LostGlassDataProvider m_LostGlassDataProvider;

        // Mr.kang 2010.12.27 History 만들려구
        private GlassApdInfoProvider m_GlassApdInfoProvider;
        private GlassApdHistoryProvider m_GlassApdHistoryProvider;

        public bool Created = false;
        #endregion

        #region Properties
        public IoDefines IoDefines
        {
            get { return m_IoDefines; }
        }
        public DeviceTags TagContainer
        {
            get { return m_DeviceTagContainer; }
        }
        public AlarmListProvider AlarmList
        {
            get { return m_AlarmListProvider; }
        }
        public AlarmHistoryProvider AlarmHistory
        {
            get { return m_AlarmHistoryProvider; }
        }
        public CurrentAlarmsProvider CurrentAlarms
        {
            get { return m_CurrentAlarmsProvider; }
        }
        public SetupGenInfoProvider SetupGenInfo
        {
            get { return m_SetupGenInfoProvider; }
        }
        public SetupIdleInfoProvider SetupIdleInfo
        {
            get { return m_SetupIdleInfoProvider; }
        }
        public SetupSensorInterlockProvider SetupSensorIntr
        {
            get { return m_SetupSensorIntrProvider; }
        }
        public SetupCvDistanceProvider SetupCvDistance
        {
            get { return m_SetupCvDistanceProvider; }
        }
        public SetupSensorTimeoutProvider SetupSensorTimeout
        {
            get { return m_SetupSensorTimeoutProvider; }
        }
        public SetupCvInfoProvider SetupCvInfo
        {
            get { return m_SetupCvInfoProvider; }
        }
        public SetupGaugeInterlockProvider SetupGaugeInterlock
        {
            get { return m_SetupGaugeInterlockProvider; }
        }
        public SetupTankLevelProvider SetupTankLevel
        {
            get { return m_SetupTankLevelProvider; }
        }
        public SetupDevTankLevelProvider SetupDevTankLevel
        {
            get { return m_SetupDevTankLevelProvider; }
        }
        public SetupHpmjInfoProvider SetupHpmjInfo
        {
            get { return m_SetupHpmjInfoProvider; }
        }
        //public SetupHsmsInfoProvider SetupHsmsInfo // 11.02.01 minhan
        //{
        //    get { return m_SetupHsmsInfoProvider; }
        //}
        //public SetupHsmsEqpInfoProvider SetupHsmsEqpInfo
        //{
        //    get { return m_SetupHsmsEqpInfoProvider; }
        //}
        //public SetupFtpInfoProvider SetupFtpInfo
        //{
        //    get { return m_SetupFtpInfoProvider; }
        //}
        public CalibrationProvider CalibrationProvider
        {
            get { return m_CalibrationProvider; }
        }
        public RecipeProvider RecipeProvider
        {
            get { return m_RecipeProvider; }
        }
        public GlassDataProvider GlassDataProvider
        {
            get { return m_GlassDataProvider; }
        }
        public SetupInterfaceProvider SetupInterface
        {
            get { return m_SetupInterfaceProvider; }
        }
        public LostGlassDataProvider LostGlassDataProvider
        {
            get { return m_LostGlassDataProvider; }
        }
        public GlassApdInfoProvider GlassApdInfoProvider
        {
            get { return m_GlassApdInfoProvider; }
        }
        public GlassApdHistoryProvider GlassApdDataHistoryProvider
        {
            get { return m_GlassApdHistoryProvider; }
        }
        //public CimUnitInfoProvider UnitInfoProvider
        //{
        //    get { return m_UnitInfoProvider; }
        //}

        #endregion


        public DataProvider()
        {
            try
            {

                m_IoDefines = new IoDefines();
                m_DeviceTagContainer = new DeviceTags();
                m_RecipeProvider = RecipeProvider.Instance;
                m_SetupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupIdleInfoProvider = SetupIdleInfoProvider.Instance;
                m_SetupGaugeInterlockProvider = SetupGaugeInterlockProvider.Instance;
                m_SetupCvDistanceProvider = SetupCvDistanceProvider.Instance;
                m_SetupCvInfoProvider = SetupCvInfoProvider.Instance;
                m_SetupSensorTimeoutProvider = SetupSensorTimeoutProvider.Instance;
                m_SetupSensorIntrProvider = SetupSensorInterlockProvider.Instance;
                m_SetupTankLevelProvider = SetupTankLevelProvider.Instance;
                m_SetupDevTankLevelProvider = SetupDevTankLevelProvider.Instance;
                m_SetupHpmjInfoProvider = SetupHpmjInfoProvider.Instance;
                //m_SetupHsmsInfoProvider = SetupHsmsInfoProvider.Instance; // 11.02.01 minhan
                //m_SetupHsmsEqpInfoProvider = SetupHsmsEqpInfoProvider.Instance;
                //m_SetupFtpInfoProvider = SetupFtpInfoProvider.Instance;
                m_AlarmListProvider = AlarmListProvider.Instance;
                m_AlarmHistoryProvider = AlarmHistoryProvider.Instance;
                m_CurrentAlarmsProvider = CurrentAlarmsProvider.Instance;
                m_CalibrationProvider = CalibrationProvider.Instance;
                m_GlassDataProvider = GlassDataProvider.Instance;
                m_SetupInterfaceProvider = SetupInterfaceProvider.Instance;
                m_LostGlassDataProvider = LostGlassDataProvider.Instance;
                //Mr.kang

                m_GlassApdHistoryProvider = GlassApdHistoryProvider.Instance;
                m_GlassApdInfoProvider = GlassApdInfoProvider.Instance;



                Created = _DmsDataAdaptor.Created;

                if (Created)
                {
                    Created &= Initialize();
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                MessageBox.Show(msg);
                Created = false;
            }
        }

        public DataProvider(DataProvider provider)
        {
            m_IoDefines = provider.IoDefines;
            m_DeviceTagContainer = provider.TagContainer;
            m_RecipeProvider = provider.RecipeProvider;
            m_SetupGenInfoProvider = provider.SetupGenInfo;
            m_SetupIdleInfoProvider = provider.SetupIdleInfo;
            m_SetupGaugeInterlockProvider = provider.SetupGaugeInterlock;
            m_SetupCvDistanceProvider = provider.SetupCvDistance;
            m_SetupCvInfoProvider = provider.SetupCvInfo;
            m_SetupSensorTimeoutProvider = provider.SetupSensorTimeout;
            m_SetupSensorIntrProvider = provider.SetupSensorIntr;
            m_SetupTankLevelProvider = provider.SetupTankLevel;
            m_SetupDevTankLevelProvider = provider.SetupDevTankLevel;
            m_SetupHpmjInfoProvider = provider.SetupHpmjInfo;
            //m_SetupHsmsInfoProvider = provider.SetupHsmsInfo; // 11.02.01 minhan
            //m_SetupHsmsEqpInfoProvider = provider.SetupHsmsEqpInfo;
            //m_SetupFtpInfoProvider = provider.SetupFtpInfo;
            m_AlarmListProvider = provider.AlarmList;
            m_AlarmHistoryProvider = provider.AlarmHistory;
            m_CurrentAlarmsProvider = provider.CurrentAlarms;
            m_CalibrationProvider = provider.CalibrationProvider;
            m_GlassDataProvider = provider.GlassDataProvider;
            m_SetupInterfaceProvider = provider.SetupInterface;
            m_LostGlassDataProvider = provider.LostGlassDataProvider;
        }

        public bool Initialize()
        {
            bool ok = true;

            AppConfig appConfig = AppConfig.Instance;
            AppData appData = AppData.Instance;

            /////////////////////////////////////////////////////////////////
            // Io List 생성방법 변경
            //if (ok && appConfig.Simul.IoMapping == false)
            //{
            //    ok &= m_IoDefines.ReadXml();
            //}
            /////////////////////////////////////////////////////////////////

            m_RecipeProvider.Create();
            m_AlarmListProvider.Create();
            m_CurrentAlarmsProvider.ResetAllAlarm();
            //Mr.Kang 2010.12.27

            m_GlassApdInfoProvider.Create();
            m_GlassApdHistoryProvider.Create(m_GlassApdInfoProvider.List, m_GlassApdInfoProvider.NameList.Count, null);

            return ok;
        }

        public void DeleteGarbage()
        {
            m_SetupGenInfoProvider.DeleteDbGarbage();
            m_SetupIdleInfoProvider.DeleteDbGarbage();
            m_SetupGaugeInterlockProvider.DeleteDbGarbage();
            m_SetupCvDistanceProvider.DeleteDbGarbage();
            m_SetupSensorTimeoutProvider.DeleteDbGarbage();
            m_SetupCvInfoProvider.DeleteDbGarbage();
            m_SetupSensorIntrProvider.DeleteDbGarbage();
            m_SetupHpmjInfoProvider.DeleteDbGarbage();
            //m_SetupHsmsInfoProvider.DeleteDbGarbage(); // 11.02.01 minhan
            //m_SetupHsmsEqpInfoProvider.DeleteDbGarbage();
            //m_SetupFtpInfoProvider.DeleteDbGarbage();
            m_CalibrationProvider.DeleteDbGarbage();
            m_SetupTankLevelProvider.DeleteDbGarbage();
            m_SetupDevTankLevelProvider.DeleteDbGarbage();
            m_SetupInterfaceProvider.DeleteDbGarbage();
            m_LostGlassDataProvider.DeleteDbGarbage(100);
        }

        public void InitFromDataBase()
        {   // for Remote HMI

            //m_AlarmListProvider.LoadFromDB();
            //m_AlarmHistoryProvider.LoadFromDB();
            //m_CurrentAlarmsProvider.LoadFromDB();
            //m_SetupGenInfoProvider.LoadFromDB();
            //m_SetupIdleInfoProvider.LoadFromDB();
            //m_SetupCvInfoProvider.LoadFromDB();
            //m_SensorIntrProvider.LoadFromDB();
            //m_SetupTankLevelProvider.LoadFromDB();
            //m_SetupGaugeInterlockProvider.LoadFromDB();
            //m_SetupCvDistanceProvider.LoadFromDB();
            //m_SetupSensorTimeoutProvider.LoadFromDB();
            //m_GlassDataProvider.LoadFromDB();
            //m_CalibrationProvider.LoadFromDB();
        }

        public void SetEditPermission(UserLevels curUserLevel)
        {
            m_RecipeProvider.SetEditPermission(curUserLevel);
            m_SetupGenInfoProvider.SetEditPermission(curUserLevel);
            m_SetupIdleInfoProvider.SetEditPermission(curUserLevel);
            m_SetupGaugeInterlockProvider.SetEditPermission(curUserLevel);
            m_SetupCvDistanceProvider.SetEditPermission(curUserLevel);
            m_SetupSensorTimeoutProvider.SetEditPermission(curUserLevel);
            m_SetupCvInfoProvider.SetEditPermission(curUserLevel);
            m_SetupSensorIntrProvider.SetEditPermission(curUserLevel);
            m_SetupTankLevelProvider.SetEditPermission(curUserLevel);
            m_SetupDevTankLevelProvider.SetEditPermission(curUserLevel);
            m_SetupHpmjInfoProvider.SetEditPermission(curUserLevel);
            m_SetupInterfaceProvider.SetEditPermission(curUserLevel);
            //m_SetupHsmsInfoProvider.SetEditPermission(curUserLevel); // 11.02.01 minhan
            //m_SetupHsmsEqpInfoProvider.SetEditPermission(curUserLevel);
            //m_SetupFtpInfoProvider.SetEditPermission(curUserLevel);
        }

        public bool IsSetupInfoChanged()
        {
            bool changed = false;
            changed |= m_SetupGenInfoProvider.Adapter.IsChanged();
            changed |= m_SetupIdleInfoProvider.Adapter.IsChanged();
            changed |= m_SetupTankLevelProvider.Adapter.IsChanged();
            changed |= m_SetupDevTankLevelProvider.Adapter.IsChanged();
            changed |= m_SetupGaugeInterlockProvider.Adapter.IsChanged();
            changed |= m_SetupSensorIntrProvider.Adapter.IsChanged();
            changed |= m_SetupCvDistanceProvider.Adapter.IsChanged();
            changed |= m_SetupCvInfoProvider.Adapter.IsChanged();
            changed |= m_SetupSensorTimeoutProvider.Adapter.IsChanged();
            changed |= m_SetupHpmjInfoProvider.Adapter.IsChanged();
            changed |= m_SetupInterfaceProvider.Adapter.IsChanged();
            //changed |= m_SetupHsmsInfoProvider.Adapter.IsChanged(); // 11.02.01 minhan
            //changed |= m_SetupHsmsEqpInfoProvider.Adapter.IsChanged();
            //changed |= m_SetupFtpInfoProvider.Adapter.IsChanged();
            return changed;
        }

        public void RejectChangeSetupInfos()
        {
            m_SetupGenInfoProvider.Adapter.RejectChanges();
            m_SetupIdleInfoProvider.Adapter.RejectChanges();
            m_SetupTankLevelProvider.Adapter.RejectChanges();
            m_SetupDevTankLevelProvider.Adapter.RejectChanges();
            m_SetupGaugeInterlockProvider.Adapter.RejectChanges();
            m_SetupSensorIntrProvider.Adapter.RejectChanges();
            m_SetupCvDistanceProvider.Adapter.RejectChanges();
            m_SetupCvInfoProvider.Adapter.RejectChanges();
            m_SetupSensorTimeoutProvider.Adapter.RejectChanges();
            m_SetupHpmjInfoProvider.Adapter.RejectChanges();
            m_SetupInterfaceProvider.Adapter.RejectChanges();
            //m_SetupHsmsInfoProvider.Adapter.RejectChanges(); // 11.02.01 minhan
            //m_SetupHsmsEqpInfoProvider.Adapter.RejectChanges();
            //m_SetupFtpInfoProvider.Adapter.RejectChanges();
        }

        public void SaveSetupInfos()
        {
            GlobalVar.SetupChangeReportReq = m_SetupGenInfoProvider.Adapter.IsChanged();
            m_SetupGenInfoProvider.UpdateToDB();
            m_SetupIdleInfoProvider.UpdateToDB();
            m_SetupTankLevelProvider.UpdateToDB();
            m_SetupDevTankLevelProvider.UpdateToDB();
            m_SetupGaugeInterlockProvider.UpdateToDB();
            m_SetupSensorIntrProvider.UpdateToDB();
            m_SetupCvDistanceProvider.UpdateToDB();
            m_SetupCvInfoProvider.UpdateToDB();
            m_SetupSensorTimeoutProvider.UpdateToDB();
            m_SetupHpmjInfoProvider.UpdateToDB();
            m_SetupInterfaceProvider.UpdateToDB();
            //m_SetupHsmsInfoProvider.UpdateToDB(); // 11.02.01 minhan
            //m_SetupHsmsEqpInfoProvider.UpdateToDB();
            //m_SetupFtpInfoProvider.UpdateToDB();
        }
    }
}
