using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Dms.Common
{
    abstract public class _GenInfoHandler
    {
        #region GenInfo Items : GenInfo형식으로 만들 data(DeviceTag형태가 될 멤버들)
        public int _Auto;
        public int _Ready;
        public int _CycleStop;
        public int _Pause;
        public int _DiStart;
        public int _DevStart;
        public int _EqpInitReq;
        public int _EqpInitComp;
        public int _CleanOut;
        public int _IdleRunning;
        public int _IdleRunningProgress;
        public int _TactTime;
        public int _TotalGlsCount;
        public int _CurGlsCount;
        public int _EQPGlsCount;
        public int _CurRecipeId;
        public int _DeviceControllerState;
        public int _CurrentCpuTemp;
        public int _EqpLog;
        public int _CommLog;
        public int _UserLevel;
        public int _HostReady;
        public int _CimMessage;
        public int _UpStreamState;
        public int _DnStreamState;
        public int _ApdReportIndex;
        public int _EuvLampOnProgress;
        public int _ChemicalStart;
        public int _ChemicalIdleRunning;
        public int _ChemicalIdleRunningProgress;
        public int _ApOnProgress;
        public int _ForceUpIfInitReq;
        public int _ForceUpIfCompReq;
        public int _ForceDnIfInitReq;
        public int _ForceDnIfCompReq;
        public int _AgingMode;
        #endregion

        #region Properties for GenInfo Handle
        public virtual bool AutoMode
        {
            get { return (m_InfoCollection[_Auto] == bool.TrueString); }
            set { m_InfoCollection[_Auto] = value.ToString(); }
        }
        public virtual bool Pause
        {
            get { return (m_InfoCollection[_Pause] == bool.TrueString); }
            set { m_InfoCollection[_Pause] = value.ToString(); }
        }
        public virtual bool CycleStop
        {
            get { return (m_InfoCollection[_CycleStop] == bool.TrueString); }
            set { m_InfoCollection[_CycleStop] = value.ToString(); }
        }
        public virtual bool DiStart
        {
            get { return (m_InfoCollection[_DiStart] == bool.TrueString); }
            set { m_InfoCollection[_DiStart] = value.ToString(); }
        }
        public virtual bool DevStart
        {
            get { return (m_InfoCollection[_DevStart] == bool.TrueString); }
            set { m_InfoCollection[_DevStart] = value.ToString(); }
        }
        public virtual bool Ready
        {
            get { return (m_InfoCollection[_Ready] == bool.TrueString); }
            set { m_InfoCollection[_Ready] = value.ToString(); }
        }
        public virtual bool EqpInitReq
        {
            get { return (m_InfoCollection[_EqpInitReq] == bool.TrueString); }
            set { m_InfoCollection[_EqpInitReq] = value.ToString(); }
        }
        public virtual bool EqpInitComp
        {
            get { return (m_InfoCollection[_EqpInitComp] == bool.TrueString); }
            set { m_InfoCollection[_EqpInitComp] = value.ToString(); }
        }
        public virtual bool CleanOut
        {
            get { return (m_InfoCollection[_CleanOut] == bool.TrueString); }
            set { m_InfoCollection[_CleanOut] = value.ToString(); }
        }
        public virtual bool IdleRunning
        {
            get { return (m_InfoCollection[_IdleRunning] == bool.TrueString); }
            set { m_InfoCollection[_IdleRunning] = value.ToString(); }
        }
        public virtual string IdleRunningProgress
        {
            get { return m_InfoCollection[_IdleRunningProgress]; }
            set { m_InfoCollection[_IdleRunningProgress] = value; }
        }
        public virtual int TactTime
        {
            get { return Convert.ToInt32(m_InfoCollection[_TactTime]); }
            set { m_InfoCollection[_TactTime] = string.Format("{0}", value); }
        }
        public virtual long TotalGlassCount
        {
            get { return Convert.ToInt64(m_InfoCollection[_TotalGlsCount]); }
            set
            {
                m_InfoCollection[_TotalGlsCount] = string.Format("{0}", value);
                m_AppData.TotalGlassCount = value;
            }
        }
        public virtual long CurGlassCount
        {
            get { return Convert.ToInt64(m_InfoCollection[_CurGlsCount]); }
            set
            {
                m_InfoCollection[_CurGlsCount] = string.Format("{0}", value);
                m_AppData.CurGlassCount = value;
                m_AppData.WriteXml();
            }
        }
        public virtual int EQPGlassCount
        {
            get { return Convert.ToInt32(m_InfoCollection[_EQPGlsCount]); }
            set { m_InfoCollection[_EQPGlsCount] = string.Format("{0}", value); }
        }
        public virtual string CurRecipeId
        {
            get { return m_InfoCollection[_CurRecipeId]; }
            set { m_InfoCollection[_CurRecipeId] = value; }
        }
        public virtual string DeviceControllerState
        {
            get { return m_InfoCollection[_DeviceControllerState]; }
            set { m_InfoCollection[_DeviceControllerState] = value; }
        }
        public virtual decimal CurCpuTemp
        {
            get { return Convert.ToDecimal(m_InfoCollection[_CurrentCpuTemp]); }
            set { m_InfoCollection[_CurrentCpuTemp] = string.Format("{0}", value); }
        }
        public virtual string EqpLog
        {
            get { return m_InfoCollection[_EqpLog]; }
            set { m_InfoCollection[_EqpLog] = value; }
        }
        public virtual string CommLog
        {
            get { return m_InfoCollection[_CommLog]; }
            set { m_InfoCollection[_CommLog] = value; }
        }

        public virtual int UserLevel
        {
            get { return Convert.ToInt32(m_InfoCollection[_UserLevel]); }
            set { m_InfoCollection[_UserLevel] = string.Format("{0}", value); }
        }
        public virtual bool HostReady
        {
            get { return (m_InfoCollection[_HostReady] == bool.TrueString); }
            set { m_InfoCollection[_HostReady] = value.ToString(); }
        }
        public virtual string CimMessage
        {
            get { return m_InfoCollection[_CimMessage]; }
            set { m_InfoCollection[_CimMessage] = value; }
        }
        public virtual string UpStreamState
        {
            get { return m_InfoCollection[_UpStreamState]; }
            set { m_InfoCollection[_UpStreamState] = value; }
        }
        public virtual string DnStreamState
        {
            get { return m_InfoCollection[_DnStreamState]; }
            set { m_InfoCollection[_DnStreamState] = value; }
        }
        public virtual short ApdReportIndex
        {
            get { return Convert.ToInt16(m_InfoCollection[_ApdReportIndex]); }
            set
            {
                m_InfoCollection[_ApdReportIndex] = string.Format("{0}", value);
                m_AppData.ApdReportIndex = value;
                m_AppData.WriteXml();
            }
        }
        public virtual string EuvLampOnProgress
        {
            get { return m_InfoCollection[_EuvLampOnProgress]; }
            set { m_InfoCollection[_EuvLampOnProgress] = value; }
        }
        public virtual bool ChemicalStart
        {
            get { return (m_InfoCollection[_ChemicalStart] == bool.TrueString); }
            set { m_InfoCollection[_ChemicalStart] = value.ToString(); }
        }

        public virtual bool ChemicalIdleRunning
        {
            get { return (m_InfoCollection[_ChemicalIdleRunning] == bool.TrueString); }
            set { m_InfoCollection[_ChemicalIdleRunning] = value.ToString(); }
        }

        public virtual string ChemicalIdleRunningProgress
        {
            get { return m_InfoCollection[_ChemicalIdleRunningProgress]; }
            set { m_InfoCollection[_ChemicalIdleRunningProgress] = value; }
        }
        public virtual string ApOnProgress
        {
            get { return m_InfoCollection[_ApOnProgress]; }
            set { m_InfoCollection[_ApOnProgress] = value; }
        }
        public virtual bool ForceUpIfInitReq
        {
            get { return (m_InfoCollection[_ForceUpIfInitReq] == bool.TrueString); }
            set { m_InfoCollection[_ForceUpIfInitReq] = value.ToString(); }
        }
        public virtual bool ForceUpIfCompReq
        {
            get { return (m_InfoCollection[_ForceUpIfCompReq] == bool.TrueString); }
            set { m_InfoCollection[_ForceUpIfCompReq] = value.ToString(); }
        }
        public virtual bool ForceDnIfInitReq
        {
            get { return (m_InfoCollection[_ForceDnIfInitReq] == bool.TrueString); }
            set { m_InfoCollection[_ForceDnIfInitReq] = value.ToString(); }
        }
        public virtual bool ForceDnIfCompReq
        {
            get { return (m_InfoCollection[_ForceDnIfCompReq] == bool.TrueString); }
            set { m_InfoCollection[_ForceDnIfCompReq] = value.ToString(); }
        }
        public virtual bool AgingMode
        {
            get { return (m_InfoCollection[_AgingMode] == bool.TrueString); }
            set { m_InfoCollection[_AgingMode] = value.ToString(); }
        }
        #endregion

        #region Fields
        protected GenInfoCollection m_InfoCollection = new GenInfoCollection();
        protected AppData m_AppData = AppData.Instance;
        protected static List<string> m_NameList = null;
        #endregion

        #region Methods
        public static string[] GetNameList()
        {
            return m_NameList.ToArray();
        }

        public void GenerateGenInfo()
        {
            try
            {
                //jemoon 상속관계에 있는 class의 FieldInfos는 역순으로 들어오므로
                //(최하위 자식 class의 field가 0, 부모class의 field는 n+1)
                //그러므로 상속관계에 있더라도 동일한 index 유지를 위해서는 reverse가 필요하다
                FieldInfo[] fieldInfos = this.GetType().GetFields();
                FieldInfo fieldInfo;
                int index = 0;
                int fieldCount = fieldInfos.Length;
                for (int i = (fieldCount - 1); i >= 0; i--)
                {
                    fieldInfo = fieldInfos[i];
                    if (fieldInfo.FieldType == typeof(int))
                    {
                        fieldInfo.SetValue(this, index++);
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }

        protected void MakeNameList()
        {
            // jemoon : GenInfoHandler 생성시 한번만
            if (m_NameList == null)
            {
                m_NameList = new List<string>();

                //jemoon 상속관계에 있는 class의 FieldInfos는 역순으로 들어오므로
                //(최하위 자식 class의 field가 0, 부모class의 field는 n+1)
                //그러므로 상속관계에 있더라도 동일한 index 유지를 위해서는 reverse가 필요하다
                FieldInfo[] fieldInfos = this.GetType().GetFields();
                FieldInfo fieldInfo;
                int fieldCount = fieldInfos.Length;
                for (int i = (fieldCount - 1); i >= 0; i--)
                {
                    fieldInfo = fieldInfos[i];
                    if (fieldInfo.FieldType == typeof(int))
                    {
                        m_NameList.Add(fieldInfo.Name.Remove(0, 1)); // remove '_'
                    }
                }
            }
        }
        #endregion
    }
}
