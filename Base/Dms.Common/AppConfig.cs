///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : AppConfig
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading;
using EnvDTE;
using EnvDTE80;

namespace Dms.Common
{
    public enum WireNumberingMode
    {
        Editable,
        ByOct,
        ByDecimal,
        OnlyCptG6Cline,
    }

    public enum Resolution
    {
        R1024_768,
        R1280_1024,
    }

    [Editor(typeof(UIEditorSimluationConfigHelper), typeof(UITypeEditor))]
    public class Simul
    {
        #region Fields
        private bool m_IoController = true;
        private bool m_Device = true;
        private bool m_IoMapping = true;
        private bool m_Motion = true;
        private bool m_Melsec = true;
        private bool m_Interface = true;
        private bool m_Comport = true;
        private bool m_Cim = true;
        private bool m_LType = true;

        // CIM에서 사용
        // CIM하고 장비, 로더 간 통신이 Meselc인경우 로더만 시뮬레이션인 경우, 장비만 시뮬레이션인 경우에 사용
        private bool m_Loader = false;
        private bool m_Eqp = false;
        #endregion

        #region Properties
        public bool IoController
        {
            get { return m_IoController; }
            set { m_IoController = value; }
        }
        public bool Device
        {
            get { return m_Device; }
            set { m_Device = value; }
        }
        public bool IoMapping
        {
            get { return m_IoMapping; }
            set { m_IoMapping = value; }
        }
        public bool Motion
        {
            get { return m_Motion; }
            set { m_Motion = value; }
        }
        public bool Melsec
        {
            get { return m_Melsec; }
            set { m_Melsec = value; }
        }
        public bool Interface
        {
            get { return m_Interface; }
            set { m_Interface = value; }
        }
        public bool Comport
        {
            get { return m_Comport; }
            set { m_Comport = value; }
        }
        public bool Cim
        {
            get { return m_Cim; }
            set { m_Cim = value; }
        }
        public bool LType
        {
            get { return m_LType; }
            set { m_LType = value; }
        }

        public bool Loader
        {
            get { return m_Loader; }
            set { m_Loader = value; }
        }

        public bool Eqp
        {
            get { return m_Eqp; }
            set { m_Eqp = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly Simul Instance = new Simul();
        #endregion

        #region Constructor
        //jemoon : Singleton 으로 구현되어야 하는데... 
        public Simul()
        {
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return "Set simulation flags";
        }
        #endregion
    }

    public class AppConfig
    {
        #region Fields
        private static bool m_Initialized = false;
        private static bool m_UseDefaultFilePath = false;
        private static readonly string[] ignores = new string[] { "Assembly", "Base", "System" };
        public static readonly string DefaultConfigFilePath = GetAppRootPath() + "Configfiles";
        public static readonly string DefaultDatabaseFilePath = GetAppRootPath() + "DataBase";
        public static readonly string DefaultLogFilePath = GetAppRootPath() + "Log";
        private static string m_AppRootPath = "";
        private static string m_File;
        private static bool m_AutoStart = true;
        private static bool m_UserControlBuild = true;
        private static ServerMode m_ServerMode = ServerMode.Local;
        private static ushort m_StorageAvailableRatio = 5;
        private static Resolution m_Resolution = Resolution.R1024_768;
        //private static bool m_IoWiringNoByOct = true; // jemoon : Only for CPT G6 C/D Line
        //private static WireNumberingMode m_IoWireNumberingMode = WireNumberingMode.ByOct; //jemoon : Bus별로 다를 수 있기에
        private static bool m_UseCpuTemperatureCheckFunction = true;
        private static ushort m_EqpLogDays = 30;
        private static ushort m_ApdLogDays = 30;
        private static ushort m_TasLogDays = 30;
        private static Simul m_Simul = Simul.Instance;
        private static Mutex m_Mutex = new Mutex();
        private static FileSelect m_SecomXmlFile = new FileSelect();    //2009.07.01 Youngsik for using SECom HSMS
        private static FolderSelect m_EqpLogPath = new FolderSelect();
        private static FolderSelect m_ApdLogPath = new FolderSelect();
        private static FolderSelect m_TasLogPath = new FolderSelect();
        private static FileSelect m_AlarmDBFile = new FileSelect();
        private static FileSelect m_GlassDataDBFile = new FileSelect();
        private static FolderSelect m_GlassDataXmlFile = new FolderSelect();
        private static FolderSelect m_LostGlassDataXmlFile = new FolderSelect();
        private static FileSelect m_RecipeDBFile = new FileSelect();
        private static FileSelect m_SetupDBFile = new FileSelect();
        private static FileSelect m_UserDBFile = new FileSelect();
        private static FolderSelect m_MelsecConfigPath = new FolderSelect();
        private static FolderSelect m_ServoPath = new FolderSelect();
        private static FolderSelect m_AppDataPath = new FolderSelect();
        private static FolderSelect m_ComponentPath = new FolderSelect();
        private static FolderSelect m_IoDefinePath = new FolderSelect();
        private static FolderSelect m_ApdDataPath = new FolderSelect();
        private static FolderSelect m_PartsItemPath = new FolderSelect();
        private static FolderSelect m_DeviceXmlPath = new FolderSelect();
        private static PcSystemType m_PcType = PcSystemType.General;
        private static FolderSelect m_ModbusConfigPath = new FolderSelect();
        private static string[] m_UniversalVariable;    //eun 20091013 : 프로젝트마다 달라질 수 있는 옵션들을 사용하기 위한 변수. SNT프로젝트에서는 어떤 CIM인지 구별하기 위해 사용
        private static FolderSelect m_MelsecDeviceInfoPath = new FolderSelect();
        private static FolderSelect m_DmsControlHmiConfigPath = new FolderSelect();
        //For CIM configuraion
        private static FileSelect m_UnitInfoDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_CimAlarmDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_ProcessDataDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_CurrentDataDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_UnitRecipeInfoDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_UnitRecipeTypeInfoDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_DosingTableDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FileSelect m_TimeTableDBFile = new FileSelect(); //Sangseo for using CIM Project
        private static FolderSelect m_RecipeXmlFile = new FolderSelect(); //Sangseo for using CIM Project
        #endregion

        #region Properties
        ///////////////////////////////////////////////////////////////////////////////
        // HMI
        [Category("HMI")]
        public bool UserControlBuild
        {
            get { return m_UserControlBuild; }
            set { m_UserControlBuild = value; }
        }
        [Category("HMI")]
        public Resolution Resolution
        {
            get { return m_Resolution; }
            set { m_Resolution = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // Server
        [Category("Option")]
        public bool AutoStart
        {
            get { return m_AutoStart; }
            set { m_AutoStart = value; }
        }
        [Category("Option")]
        public ServerMode ServerMode
        {
            get { return m_ServerMode; }
            set { m_ServerMode = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // Simulation Flags
        [Category("Simulation")]
        public Simul Simul
        {
            get { return m_Simul; }
            set { m_Simul = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // Option
        [Category("Option")]
        public ushort StorageAvailableRatio
        {
            get { return m_StorageAvailableRatio; }
            set { m_StorageAvailableRatio = value; }
        }
        //[Category("Option")]
        //public WireNumberingMode IoWireNumberingMode
        //{
        //    get { return m_IoWireNumberingMode; }
        //    set { m_IoWireNumberingMode = value; }
        //}
        [Category("Option")]
        public bool UseCpuTemperatureCheck
        {
            get { return m_UseCpuTemperatureCheckFunction; }
            set { m_UseCpuTemperatureCheckFunction = value; }
        }
        [Category("Option")]
        public PcSystemType PcType
        {
            get { return m_PcType; }
            set { m_PcType = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // FilePath : option
        [Category("FilePath")]
        public bool UseDefaultFilePath
        {
            get { return m_UseDefaultFilePath; }
            set { m_UseDefaultFilePath = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // FilePath : Log
        [Category("Option : Log")]
        public ushort EqpLogDays
        {
            get { return m_EqpLogDays; }
            set { m_EqpLogDays = value; }
        }
        [Category("Option : Log")]
        public ushort ApdLogDays
        {
            get { return m_ApdLogDays; }
            set { m_ApdLogDays = value; }
        }
        [Category("Option : Log")]
        public ushort TasLogDays
        {
            get { return m_TasLogDays; }
            set { m_TasLogDays = value; }
        }
        [Category("Option : Log")]
        public FolderSelect EqpLogPath
        {
            get { return m_EqpLogPath; }
            set { m_EqpLogPath = value; }
        }
        [Browsable(false)]
        public string EqpLogPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultLogFilePath;
                else return m_EqpLogPath.SelectedFolder;
            }
        }
        [Category("Option : Log")]
        public FolderSelect ApdLogPath
        {
            get { return m_ApdLogPath; }
            set { m_ApdLogPath = value; }
        }
        [Category("Option : Log")]
        public FolderSelect TasLogPath
        {
            get { return m_TasLogPath; }
            set { m_TasLogPath = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // FilePath : Database
        [Category("FilePath : Database")]
        public FileSelect AlarmDBFile
        {
            get { return m_AlarmDBFile; }
            set { m_AlarmDBFile = value; }
        }
        [Category("FilePath : Database"),
        Description("Select MDB or XML file")]
        public FileSelect GlassDataDBFile
        {
            get { return m_GlassDataDBFile; }
            set { m_GlassDataDBFile = value; }
        }
        [Category("FilePath : Database")]
        public FileSelect RecipeDBFile
        {
            get { return m_RecipeDBFile; }
            set { m_RecipeDBFile = value; }
        }
        [Category("FilePath : Database")]
        public FileSelect SetupDBFile
        {
            get { return m_SetupDBFile; }
            set { m_SetupDBFile = value; }
        }
        [Category("FilePath : Database")]
        public FileSelect UserDBFile
        {
            get { return m_UserDBFile; }
            set { m_UserDBFile = value; }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // FilePath : ConfigFiles
        [Category("FilePath : Configfile"),
        Description("Select MDB or XML file")]
        public FolderSelect GlassDataXmlFile
        {
            get { return m_GlassDataXmlFile; }
            set { m_GlassDataXmlFile = value; }
        }
        [Browsable(false)]
        public string GlassDataXmlFilePathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_GlassDataXmlFile.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile"),
        Description("Select MDB or XML file")]
        public FolderSelect LostGlassDataXmlFile
        {
            get { return m_LostGlassDataXmlFile; }
            set { m_LostGlassDataXmlFile = value; }
        }
        [Browsable(false)]
        public string LostGlassDataXmlFilePathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_LostGlassDataXmlFile.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile")]
        public FolderSelect ComponentContainerPath
        {
            get { return m_ComponentPath; }
            set { m_ComponentPath = value; }
        }
        [Browsable(false)]
        public string ComponentContainerPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_ComponentPath.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile")]
        public FolderSelect IoDefinePath
        {
            get { return m_IoDefinePath; }
            set { m_IoDefinePath = value; }
        }
        [Browsable(false)]
        public string IoDefinePathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_IoDefinePath.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile")]
        public FolderSelect AppDataPath
        {
            get { return m_AppDataPath; }
            set { m_AppDataPath = value; }
        }
        [Browsable(false)]
        public string AppDataPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_AppDataPath.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile")]
        public FolderSelect PartsItemPath
        {
            get { return m_PartsItemPath; }
            set { m_PartsItemPath = value; }
        }
        [Browsable(false)]
        public string PartsItemPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_PartsItemPath.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile")]
        public FolderSelect ApdDataPath
        {
            get { return m_ApdDataPath; }
            set { m_ApdDataPath = value; }
        }
        [Browsable(false)]
        public string ApdDataPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_ApdDataPath.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile"),
         Description("Specify the path for each device's xml file.(e.g., General Data, Mix Tank, Detergent Unit, Hpmj..)")]
        public FolderSelect DevicePath
        {
            get { return m_DeviceXmlPath; }
            set { m_DeviceXmlPath = value; }
        }
        [Browsable(false)]
        public string DevicePathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_DeviceXmlPath.SelectedFolder;
            }
        }
        [Category("FilePath : Configfile")]
        public FolderSelect DmsControlHmiConfigPath
        {
            get { return m_DmsControlHmiConfigPath; }
            set { m_DmsControlHmiConfigPath = value; }
        }
        [Browsable(false)]
        public string DmsControlHmiConfigPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath;
                else return m_DmsControlHmiConfigPath.SelectedFolder;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////
        // FilePath : ConfigFiles Etc
        [Category("FilePath : ETC")]
        public FolderSelect MelsecConfigurationPath
        {
            get { return m_MelsecConfigPath; }
            set { m_MelsecConfigPath = value; }
        }
        [Browsable(false)]
        public string MelsecConfigurationPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath + "\\MelsecConfig";
                else return MelsecConfigurationPath.SelectedFolder;
            }
        }
        [Category("FilePath : ETC")]
        public FolderSelect ServoParameterPath
        {
            get { return m_ServoPath; }
            set { m_ServoPath = value; }
        }
        [Browsable(false)]
        public string ServoParameterPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath + "\\ServoPara";
                else return m_ServoPath.SelectedFolder;
            }
        }
        [Category("FilePath : ETC")]
        public FolderSelect ModbusConfigurationPath
        {
            get { return m_ModbusConfigPath; }
            set { m_ModbusConfigPath = value; }
        }
        [Browsable(false)]
        public string ModbusConfigurationPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath + "\\ModbusConfig";
                else return m_ModbusConfigPath.SelectedFolder;
            }
        }
        [Category("FilePath : ETC")]
        public FileSelect SecomXmlFile
        {
            get { return m_SecomXmlFile; }
            set { m_SecomXmlFile = value; }
        }
        [Category("FilePath : ETC")]
        public FolderSelect MelsecDeviceInfoPath
        {
            get { return m_MelsecDeviceInfoPath; }
            set { m_MelsecDeviceInfoPath = value; }
        }
        [Browsable(false)]
        public string MelsecDeviceInfoPathName
        {
            get
            {
                if (m_UseDefaultFilePath) return DefaultConfigFilePath + "\\MelsecDeviceInfos";
                else return m_MelsecDeviceInfoPath.SelectedFolder;
            }
        }
        [Category("Option")]
        public string[] UniversalVariable
        {
            get { return m_UniversalVariable; }
            set { m_UniversalVariable = value; }
        }
        [Category("CIM")]
        public FileSelect UnitInfoDBFile
        {
            get { return m_UnitInfoDBFile; }
            set { m_UnitInfoDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect ProcessDataDBFile
        {
            get { return m_ProcessDataDBFile; }
            set { m_ProcessDataDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect CimAlarmDBFile
        {
            get { return m_CimAlarmDBFile; }
            set { m_CimAlarmDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect CurrentDataDBFile
        {
            get { return m_CurrentDataDBFile; }
            set { m_CurrentDataDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect UnitRecipeInfoDBFile
        {
            get { return m_UnitRecipeInfoDBFile; }
            set { m_UnitRecipeInfoDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect UnitRecipeTypeInfoDBFile
        {
            get { return m_UnitRecipeTypeInfoDBFile; }
            set { m_UnitRecipeTypeInfoDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect DosingTableDBFile
        {
            get { return m_DosingTableDBFile; }
            set { m_DosingTableDBFile = value; }
        }
        [Category("CIM")]
        public FileSelect TimeTableDBFile
        {
            get { return m_TimeTableDBFile; }
            set { m_TimeTableDBFile = value; }
        }
        [Category("CIM")]
        public FolderSelect RecipeXmlFile
        {
            get { return m_RecipeXmlFile; }
            set { m_RecipeXmlFile = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly AppConfig Instance = new AppConfig();
        #endregion

        #region Constructor
        //jemoon : Singleton 으로 구현되어야 하는데... 
        //  bm : Singlton 구현을 위해 private으로 수정 후 기존 Constructor 호출을 Instance 참조로 수정
        private AppConfig()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;
                ReadXml();
            }
        }
        #endregion

        #region Methods
        public static bool IsDesignTime()
        {
            return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");
        }

        public static string GetSolutionPath()
        {
            // Get an instance of the currently running Visual Studio IDE.
            EnvDTE80.DTE2 dte2;
            //dte2 = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.8.0");
            System.Type t = System.Type.GetTypeFromProgID("VisualStudio.DTE.8.0", true);
            dte2 = (EnvDTE80.DTE2)System.Activator.CreateInstance(t, true);

            return Path.GetDirectoryName(dte2.Solution.FullName);
        }


        public static string GetAppRootPath()
        {
            if (!string.IsNullOrEmpty(m_AppRootPath))
            {
                return m_AppRootPath;
            }

            string path;
            if (IsDesignTime())
            {
                path = GetSolutionPath();         //  BM : DTE 실행이 안돼서 임시로 주석처리 함, 배포 시 풀어서 넘길 것
                path = @"C:\Users\d\Documents\____WSSD\15HC50_BOE_G8_TFT_DHDC_(EUV)\Base\System";
            }
            else
            {
                path = Application.StartupPath;
            }

            string[] seperator = { "\\" }; //경로문자열을 분리
            if (path.Contains(seperator[0]))
            {
                string[] paths = path.Split(seperator, StringSplitOptions.None);

                path = "";
                //Ignore 폴더를 짤라냄
                int count = paths.Length;
                for (int i = 0; i < count; i++)
                {
                    bool ignored = false;
                    for (int k = 0; k < ignores.Length; k++)
                    {
                        if (paths[i].Contains(ignores[k]))
                        {
                            ignored = true;
                            break;
                        }
                    }

                    if (!ignored)
                    {
                        path += paths[i];
                        path += seperator[0];
                    }
                }
            }

            m_AppRootPath = path;

            return path;
        }

        public void ReadXml(string fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);

                if (fileInfo.Exists)
                {
                    m_File = fileName;
                }
                else
                {
                    MessageBox.Show(this.GetType().Name + " File not found" + "\r\n" + fileName);
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Title = "Select XML file : " + this.GetType().Name;
                    dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                    dlg.InitialDirectory = GetAppRootPath();

                    if (DialogResult.OK == dlg.ShowDialog())
                    {
                        m_File = dlg.FileName;
                    }
                    else
                    {
                        if (DialogResult.Yes == MessageBox.Show("Do you want create new AppConfig file?", this.GetType().Name, MessageBoxButtons.YesNo))
                        {
                            FolderBrowserDialog dlg2 = new FolderBrowserDialog();
                            dlg2.Description = "AppConfig File Folder Path to create";
                            dlg2.SelectedPath = GetAppRootPath();
                            if (DialogResult.OK == dlg2.ShowDialog())
                            {
                                m_File = string.Format("{0}\\{1}.xml", dlg2.SelectedPath, this.GetType().Name);
                                this.WriteXml();
                            }
                        }

                        return;
                    }
                }

                StreamReader sr = new StreamReader(m_File);
                XmlSerializer xmlSer = new XmlSerializer(typeof(AppConfig));
                AppConfig config = xmlSer.Deserialize(sr) as AppConfig;
                sr.Close();
            }
            catch (Exception)
            {
            }
        }

        public void ReadXml()
        {
            if (string.IsNullOrEmpty(m_File))
            {
                string dirName = AppConfig.DefaultConfigFilePath;
                string fileName = string.Format("{0}\\{1}.xml", dirName, this.GetType().Name);
                ReadXml(fileName);
            }
            else ReadXml(m_File);
        }

        public bool WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = new XmlSerializer(this.GetType());

            try
            {
                m_Mutex.WaitOne();

                // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();
                m_Mutex.ReleaseMutex();
                return false;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.CopyTo(fileName + ".old", true);
                }

                sw = new StreamWriter(fileName);
                xmlSer.Serialize(sw, this);
                sw.Close();

                m_File = fileName;
                m_Mutex.ReleaseMutex();
                return true;
            }
            catch (Exception err)
            {
                err.ToString();
                //ToDo:임시처리
                //System.Windows.Forms.MessageBox.Show(err.ToString());
                m_Mutex.ReleaseMutex();
                return false;
            }
        }

        public bool WriteXml()
        {
            m_Mutex.WaitOne();

            if (m_File == null)
            {
                string dirName = AppConfig.DefaultConfigFilePath;
                Directory.CreateDirectory(dirName);
                string fileName = string.Format("{0}\\{1}.xml", dirName, this.GetType().Name);
                m_File = fileName;
            }

            bool rv = WriteXml(m_File);

            m_Mutex.ReleaseMutex();

            return rv;
        }

        //public void Modify(string nodeName, string newValue)
        //{
        //    lock(m_LockKey)
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(m_File);
        //        XmlNode node = doc.DocumentElement;
        //        foreach (XmlNode node1 in node.ChildNodes)
        //        {
        //            if (node1.Name == nodeName)
        //            {
        //                if (nodeName.Contains("Path") || nodeName.Contains("DBFile") || nodeName.Contains("XmlFile"))
        //                {
        //                    node1.ChildNodes[0].InnerText = newValue;
        //                    break;
        //                }
        //                else
        //                {
        //                    node1.InnerText = newValue;
        //                    break;
        //                }
        //            }
        //        }
        //        doc.Save(m_File);
        //        ReadXml();
        //    }
        //}
        #endregion
    }
}
