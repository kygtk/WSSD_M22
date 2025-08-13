using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

namespace Dms.Cim.Common
{
    [Serializable]
    public class LotInfo : ICloneable
    {
        public LotInfo()
        {
        }

        private short m_LotNo;
        private int m_PortNo;
        private string m_PCode;
        private string m_RCode;
        private string m_CstId;
        private string m_LotId;
        private string m_OpId;
        private string m_RecipeId;
        private string m_OldRecipeId;
        private string m_RecipeIdVer;
        private string m_OpNo;
        private string m_ProcId;
        private string m_ModelNo;

        // Not Use //////////////////////////////////////////////////
        private string m_RwkCnt;
        private string m_RwkFlag;
        private string m_MqcFlag;
        private string m_MpFlag;
        private string ReaderFlag;  //2DREADERFLAG
        // Not Use //////////////////////////////////////////////////

        private string m_Ssr_Flag;
        private string m_PreTool;
        private string m_PreRecipeId;
        private string m_PreRecipeName;
        private string m_PreDateTime1;
        private string m_PreDateTime2;
        private string m_PreOpeNo;
        private string m_PreMaskName;

        private string m_CstType;
        private string m_ShtCnt;
        private string m_ProcPriority;

        private enumHostLotStatus m_LotStatus = enumHostLotStatus.enumNone;

        private bool[] m_HostMapping = new bool[DefineConstants.MaxSlotNo];
        private bool[] m_ProcessFlagMapping = new bool[DefineConstants.MaxSlotNo];
        private bool[] m_Mapping = new bool[DefineConstants.MaxSlotNo];

        private int m_InNum = 0;
        private int m_OutNum = 0;
        private int m_UnloaderPortNo = 0;

        [XmlIgnore()]
        public LotDatas m_LotApdData = new LotDatas();

        private LotSlotInfos m_LotSlotInfos = new LotSlotInfos();

        public short LotNo
        {
            get { return m_LotNo; }
            set { m_LotNo = value; }
        }

        public int PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }

        public string PCode
        {
            get { return m_PCode; }
            set { m_PCode = value; }
        }

        public string RCode
        {
            get { return m_RCode; }
            set { m_RCode = value; }
        }

        public string CSTID
        {
            get { return m_CstId; }
            set { m_CstId = value; }
        }

        public string LOTID
        {
            get { return m_LotId; }
            set { m_LotId = value; }
        }

        public string OPID
        {
            get { return m_OpId; }
            set { m_OpId = value; }
        }

        public string RecipeId
        {
            get { return m_RecipeId; }
            set { m_RecipeId = value; }
        }

        public string OldRecipeId
        {
            get { return m_OldRecipeId; }
            set { m_OldRecipeId = value; }
        }

        public string RecipeIdVer
        {
            get { return m_RecipeIdVer; }
            set { m_RecipeIdVer = value; }
        }

        public string OPNO
        {
            get { return m_OpNo; }
            set { m_OpNo = value; }
        }

        public string PROCID
        {
            get { return m_ProcId; }
            set { m_ProcId = value; }
        }

        public string MODELNO
        {
            get { return m_ModelNo; }
            set { m_ModelNo = value; }
        }

        // NOT USE //////////////////////////////////////////////////////////////////////////////////////////////
        public string RWKCNT
        {
            get { return m_RwkCnt; }
            set { m_RwkCnt = value; }
        }

        public String RWKFLAG
        {
            get { return m_RwkFlag; }
            set { m_RwkFlag = value; }
        }

        public String MQCFLAG
        {
            get { return m_MqcFlag; }
            set { m_MqcFlag = value; }
        }

        public String MPFLAG
        {
            get { return m_MpFlag; }
            set { m_MpFlag = value; }
        }

        public string READERFLAG
        {
            get { return ReaderFlag; }
            set { ReaderFlag = value; }
        }
        // //////////////////////////////////////////////////////////////////////////////////////////////////////

        public string SSR_FLAG
        {
            get { return m_Ssr_Flag; }
            set { m_Ssr_Flag = value; }
        }

        public string PRETOOL
        {
            get { return m_PreTool; }
            set { m_PreTool = value; }
        }

        public string PRERECIPEID
        {
            get { return m_PreRecipeId; }
            set { m_PreRecipeId = value; }
        }

        public string PRERECIPENAME
        {
            get { return m_PreRecipeName; }
            set { m_PreRecipeName = value; }
        }

        public string PREDATETIME1
        {
            get { return m_PreDateTime1; }
            set { m_PreDateTime1 = value; }
        }

        public string PREDATETIME2
        {
            get { return m_PreDateTime2; }
            set { m_PreDateTime2 = value; }
        }

        public string PREOPENO
        {
            get { return m_PreOpeNo; }
            set { m_PreOpeNo = value; }
        }

        public string PREMASKNAME
        {
            get { return m_PreMaskName; }
            set { m_PreMaskName = value; }
        }

        public string CSTTYPE
        {
            get { return m_CstType; }
            set { m_CstType = value; }
        }

        public string PROCPRIORITY
        {
            get { return m_ProcPriority; }
            set { m_ProcPriority = value; }
        }

        public string SHTCNT
        {
            get { return m_ShtCnt; }
            set { m_ShtCnt = value; }
        }

        public bool[] HostMapping
        {
            get { return m_HostMapping; }
            set { m_HostMapping = value; }
        }

        public bool[] ProcessFlagMap
        {
            get { return m_ProcessFlagMapping; }
            set { m_ProcessFlagMapping = value; }
        }
        
        public bool[] Mapping
        {
            get { return m_Mapping; }
            set { m_Mapping = value; }
        }

        public enumHostLotStatus LotStatus
        {
            get { return m_LotStatus; }
            set { m_LotStatus = value; }
        }

        public int InNum
        {
            get { return m_InNum; }
            set { m_InNum = value; }
        }

        public int OutNum
        {
            get { return m_OutNum; }
            set { m_OutNum = value; }
        }
        public int UnloaderPortNo
        {
            get { return m_UnloaderPortNo; }
            set { m_UnloaderPortNo = value; }
        }

        public LotDatas LotApdData
        {
            get { return m_LotApdData; }
        }

        public LotSlotInfos SlotInfos
        {
            get { return m_LotSlotInfos; }
            set { m_LotSlotInfos = value; }
        }

        public object Clone()
        {
            LotInfo lot = new LotInfo();

            lot.LotNo = LotNo;
            lot.PortNo = PortNo;
            lot.PCode = PCode;
            lot.RCode = RCode;

            lot.CSTID = CSTID;
            lot.LOTID = LOTID;
            lot.OPID = OPID;
            lot.RecipeId = RecipeId;
            lot.RecipeIdVer = RecipeIdVer;
            lot.OPNO = OPNO;
            lot.PROCID = PROCID;
            lot.MODELNO = MODELNO;

            // Not Use //////////////////////////////////////////////////
            lot.RWKCNT = RWKCNT;
            lot.RWKFLAG = RWKFLAG;
            lot.MQCFLAG = MQCFLAG;
            lot.MPFLAG = MPFLAG;
            lot.READERFLAG = READERFLAG;
            // Not Use //////////////////////////////////////////////////

            lot.SSR_FLAG = SSR_FLAG;
            lot.PRETOOL = PRETOOL;
            lot.PRERECIPEID = PRERECIPEID;
            lot.PRERECIPENAME = PRERECIPENAME;
            lot.PREDATETIME1 = PREDATETIME1;
            lot.PREDATETIME2 = PREDATETIME2;
            lot.PREOPENO = PREOPENO;
            lot.PREMASKNAME = PREMASKNAME;

            lot.CSTTYPE = CSTTYPE;
            lot.SHTCNT = SHTCNT;
            lot.PROCPRIORITY = PROCPRIORITY;

            lot.UnloaderPortNo = UnloaderPortNo;
//            lot.LotApd = LotApd;
            lot.SlotInfos = SlotInfos;

            return lot;
        }

        public string GetPathName()
        {
            Directory.SetCurrentDirectory(Application.StartupPath);
            string dirName = @"..\..\..\SettingFile\Data\LotInfo";
            string path = string.Format("{0}\\{1}.xml", dirName, m_LotId.Trim());
            return path;
        }

        public void WriteXml()
        {
            try
            {
                string path = GetPathName();
                StreamWriter sw = new StreamWriter(path);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());
                xmlSer.Serialize(sw, this);
                sw.Close();
            }
            catch
            {
                //ToDo:임시처리
                //System.Windows.Forms.MessageBox.Show(err.ToString());
            }
        }

        public bool IsSlotInfo(int slotno)
        {
            bool bRv = false;

            foreach (LotSlotInfo slotinfo in SlotInfos.Items)
            {
                if (Convert.ToInt32(slotinfo.SlotId) == slotno)
                {
                    bRv = true;
                }
            }

            return bRv;
        }

        public LotSlotInfo GetSlotInfo(int slotno)
        {
            LotSlotInfo slotinfo = null;

            foreach (LotSlotInfo info in SlotInfos.Items)
            {
                if (Convert.ToInt32(info.SlotId) == slotno)
                {
                    slotinfo = info;
                    break;
                }
            }

            return slotinfo;
        }


        public bool SetApdData(int slotno, List<TagApdItem> apddata)
        {
            bool bRv = false;

            try
            {
                if (SlotInfos.Count > 0 && SlotInfos.Count >= slotno )
                {
                    SlotInfos.SetApdData(slotno, apddata);

                    bRv = true;
                }
            }
            catch //(System.Exception e)
            {
            }
            finally
            {
            }

            return bRv;
        }


        public bool IsLotSlotExistAsStatusOf(enumHostGlassStatus status)
        {
            foreach (LotSlotInfo info in m_LotSlotInfos.Items)
            {
                if (info.GlassStatus == status) return true;
            }
            return false;
        }

        public void IncreaseInNum()
        {
            m_InNum++;
        }

        public void ResetInNum()
        {
            m_InNum = 0;
        }

        public void IncreaseOutNum()
        {
            m_OutNum++;
        }

        public void ResetOutNum()
        {
            m_OutNum = 0;
        }
    }
}
