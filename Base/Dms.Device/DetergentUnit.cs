using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    public class DetergentUnitItem
    {
        #region Fields
        private int m_CurMixTankId = 0;
        private int m_CurDetTankId = 0;
        private int m_ReplenishCount = 0;
        private string m_FileName = null;
        private Mutex m_Mutex = new Mutex();
        private string m_DetUnitName;
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        [Browsable(false)]
        public int CurMixTankId
        {
            get { return m_CurMixTankId; }
            set { m_CurMixTankId = value; }
        }
        [Browsable(false)]
        public int CurDetTankId
        {
            get { return m_CurDetTankId; }
            set { m_CurDetTankId = value; }
        }
        [Browsable(false)]
        public int ReplenishCount
        {
            get { return m_ReplenishCount; }
            set { m_ReplenishCount = value; }
        }
        #endregion

        #region Constructor
        private DetergentUnitItem()
        {
        }
        public DetergentUnitItem(string name)
        {
            m_DetUnitName = name;
        }
        #endregion

        #region Methods
        //public bool InitParameter()
        //{
        //    return ReadXml();
        //}
        //public void UpdateData()
        //{
        //    WriteXml();
        //}
        private void SetData(DetergentUnitItem item)
        {
            this.CurMixTankId = item.CurMixTankId;
            this.CurDetTankId = item.CurDetTankId;
            this.ReplenishCount = item.ReplenishCount;
        }
        public void Reset()
        {
            m_CurMixTankId = -1;
            m_CurDetTankId = -1;
            m_ReplenishCount = 0;
        }
        private void CheckPath()
        {
            //jemoon : 090929 - use default path option
            string folderPath = m_AppConfig.DevicePathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(folderPath);
            }

            if (Directory.Exists(folderPath) == false)
            {
                MessageBox.Show("Device Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Device Folder (for DetergentUnit)";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    folderPath = dlg.SelectedPath;
                    m_AppConfig.DevicePath.SelectedFolder = folderPath;
                    m_AppConfig.WriteXml();

                    m_FileName = string.Format("{0}\\{1}.xml", folderPath, m_DetUnitName);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.xml", folderPath, m_DetUnitName);
                m_CheckPath = 1;
            }
        }
        public bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                StreamReader sr = null;
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());

                //jemoon : 090929 - m_DetUnitName에 따라 m_FileName이 달라져야 한다.
                m_FileName = string.Format("{0}\\{1}.xml", m_AppConfig.DevicePathName, m_DetUnitName);

                try
                {
                    FileInfo fileInfo = new FileInfo(m_FileName);
                    if (fileInfo.Exists)
                    {
                        sr = new StreamReader(m_FileName);
                        DetergentUnitItem storedHandler;
                        storedHandler = xmlSer.Deserialize(sr) as DetergentUnitItem;

                        SetData(storedHandler);

                        sr.Close();
                    }
                    else
                    {
                        WriteXml();
                    }
                    return true;
                }
                catch (Exception err)
                {
                    string msg = err.ToString();
                    System.Windows.Forms.MessageBox.Show(err.ToString());

                    if (sr != null) sr.Close();

                    return false;
                }
            }
            else return false;
        }
        public void WriteXml()
        {
            if (m_CheckPath != 1 || string.IsNullOrEmpty(m_FileName)) return;

            m_Mutex.WaitOne();

            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());

            try
            {   // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(m_FileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(m_FileName + ".try");
                file.Delete();
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();
                m_Mutex.ReleaseMutex();
                return;
            }

            try
            {
                // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(m_FileName);
                if (file.Exists)
                {
                    file.CopyTo(m_FileName + ".old", true);
                }

                sw = new StreamWriter(m_FileName);
                xmlSer.Serialize(sw, this);
                sw.Close();
                m_Mutex.ReleaseMutex();
            }
            catch (Exception err)
            {
                m_Mutex.ReleaseMutex();
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());
            }
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DetergentUnit : _DeviceAsm
    {
        #region Fields
        private _GenericCollection<MixTankUnit> m_MixTanks = new _GenericCollection<MixTankUnit>();
        private _GenericCollection<DetTankUnit> m_DetTanks = new _GenericCollection<DetTankUnit>();
        private DetergentUnitItem m_Item;
        private MixTankUnit m_CurTankUnit = null;
        private DetTankUnit m_CurDetTankUnit = null;
        private TagSetupInfo m_SetupIdleUse = null;
        private TagSetupInfo m_SetupIdleRunTime = null;
        private TagSetupInfo m_SetupIdleStopTime = null;
        private bool m_ChemicalStart = false;
		private static TimeUnit m_IdleRunTimeUnit = TimeUnit.Min;
		private static TimeUnit m_IdleStopTimeUnit = TimeUnit.Min;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public _GenericCollection<MixTankUnit> MixTanks
        {
            get { return m_MixTanks; }
            set { m_MixTanks = value; }
        }
        [Category("DMS : Setting")]
        public _GenericCollection<DetTankUnit> DetTanks
        {
            get { return m_DetTanks; }
            set { m_DetTanks = value; }
        }
		[Category("DMS : Setting")]
		public static TimeUnit IdleRunTimeUnit
		{
			get { return m_IdleRunTimeUnit; }
			set { m_IdleRunTimeUnit = value; }
		}
		[Category("DMS : Setting")]
		public static TimeUnit IdleStopTimeUnit
		{
			get { return m_IdleStopTimeUnit; }
			set { m_IdleStopTimeUnit = value; }
		}
        [Browsable(false)]
		public DetergentUnitItem Items
        {
            get { return m_Item; }
            set { m_Item = value; }
        }
        [Browsable(false)]
        public bool ChemicalStart
        {
            get { return m_ChemicalStart; }
            set { m_ChemicalStart = value; }
        }
        [Browsable(false), XmlIgnore()]
        public MixTankUnit CurTankUnit
        {
            get { return m_CurTankUnit; }
            set { m_CurTankUnit = value; }
        }
        [Browsable(false), XmlIgnore()]
        public DetTankUnit CurDetTankUnit
        {
            get { return m_CurDetTankUnit; }
            set { m_CurDetTankUnit = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDetIdleUse
        {
            get { return m_SetupIdleUse; }
            set { m_SetupIdleUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDetIdleRunTime
        {
            get { return m_SetupIdleRunTime; }
            set { m_SetupIdleRunTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDetIdleStopTime
        {
            get { return m_SetupIdleStopTime; }
            set { m_SetupIdleStopTime = value; }
        }
        #endregion

        #region Constructor
        public DetergentUnit()
        {
            this.Name = "__ Detergent Unit";
        }
        #endregion

        #region Methods
        public bool InitParameter()
        {
            bool ok = m_Item.ReadXml();
            if (ok)
            {
                int count = m_MixTanks.Count;
                int DetCount = m_DetTanks.Count;
                for (int i = 0; i < count; i++)
                {
                    MixTankUnit tank = m_MixTanks[i];
                    if (tank.Id == m_Item.CurMixTankId)
                    {
                        m_CurTankUnit = tank;
                    }
                }
                for (int j = 0; j < DetCount; j++)
                {
                    DetTankUnit DetTank = m_DetTanks[j];
                    if (DetTank.Id == m_Item.CurDetTankId)
                    {
                        m_CurDetTankUnit = DetTank;
                    }
                }

            }
            return ok;
        }
        public void UpdateData()
        {
            m_Item.WriteXml();
            int count = m_MixTanks.Count;
            int DetCount = m_DetTanks.Count;
            for (int i = 0; i < count; i++)
            {
                MixTankUnit tank = m_MixTanks[i];
                if (tank.Id == m_Item.CurMixTankId)
                {
                    m_CurTankUnit = tank;
                }
            }
            for (int j = 0; j < DetCount; j++)
            {
                DetTankUnit DetTank = m_DetTanks[j];
                if ( DetTank.Id == m_Item.CurDetTankId)
                {
                    m_CurDetTankUnit = DetTank;
                }
            }
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.Name;
        }

        public override Type FamilyType
        {
            get
            {
                return typeof(DetergentUnit);
            }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            
        }

        public override void UpdateTag()
        {
            
        }

        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            //ok &= GenerateAssociatedIoDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion
            ok &= (m_MixTanks.Count != 0);


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                //CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                if (m_SetupIdleUse == null)
                {
                    m_SetupIdleUse = new TagSetupInfo(this.Name + " Idle Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    SetupIdleInfoProvider.Instance.InitFromDB(m_SetupIdleUse);
                }
                if (m_SetupIdleRunTime == null)
                {
					if (m_IdleRunTimeUnit == TimeUnit.Sec)
					{
						m_SetupIdleRunTime = new TagSetupInfo(this.Name + " Idle Run Time", OptionType.Alternative, OptionFormat.Digit, UnitType.sec, "60");
					}
					else if (m_IdleRunTimeUnit == TimeUnit.Min)
					{
						m_SetupIdleRunTime = new TagSetupInfo(this.Name + " Idle Run Time", OptionType.Alternative, OptionFormat.Digit, UnitType.min, "1");
					}
					else
					{
						m_SetupIdleRunTime = new TagSetupInfo(this.Name + " Idle Run Time", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "1");
					}
                    
					SetupIdleInfoProvider.Instance.InitFromDB(m_SetupIdleRunTime);
                }
                if (m_SetupIdleStopTime == null)
                {
					if (m_IdleRunTimeUnit == TimeUnit.Sec)
					{
						m_SetupIdleStopTime = new TagSetupInfo(this.Name + " Idle Stop Time", OptionType.Alternative, OptionFormat.Digit, UnitType.sec, "600");
					}
					else if (m_IdleRunTimeUnit == TimeUnit.Min)
					{
						m_SetupIdleStopTime = new TagSetupInfo(this.Name + " Idle Stop Time", OptionType.Alternative, OptionFormat.Digit, UnitType.min, "10");
					}
					else
					{
						m_SetupIdleStopTime = new TagSetupInfo(this.Name + " Idle Stop Time", OptionType.Alternative, OptionFormat.Digit, UnitType.min, "1");
					}
                    SetupIdleInfoProvider.Instance.InitFromDB(m_SetupIdleStopTime);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_Item = new DetergentUnitItem(this.Name + " Item");
                ok &= InitParameter();
                if (!ok)
                {
                    SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                    return DmsErrors.NotInitialized;
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                //SetSubscriber();


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }
        #endregion
    }
}
