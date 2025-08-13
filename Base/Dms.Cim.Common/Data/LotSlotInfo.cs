using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Dms.Cim.Common
{
    public enum ProcessResult
    {
        NormalEnd = 'T',
        AbnormalEnd = 'F',
        Abort = 'A'
    }

    [Serializable]
    public class LotSlotInfo
    {
        private string m_SlotId;
        private string m_GlassId;
        private string m_ProcessFlag;
        private string m_GlassRecipeId;
        private string m_OldGlassRecipeId;
        private string m_GlassRecipeVer;
        private string m_SheetJudge;
        private string m_PanelJudge;
        private string m_TestFlag;
        private string m_GotoChamber;
        private enumHostGlassStatus m_GlassStatus = enumHostGlassStatus.enumIdle;

        private string m_Thickness = "";

        private string[] m_PreChamber = new string[3];
        private ApdDatas m_ApdDatas = new ApdDatas();

        private int m_AccCnt = 0;

        public string SlotId
        {
            get { return m_SlotId; }
            set { m_SlotId = value; }
        }

        public string GlassId
        {
            get { return m_GlassId; }
            set { m_GlassId = value; }
        }

        public string ProcessFlag
        {
            get { return m_ProcessFlag; }
            set { m_ProcessFlag = value; }
        }

        public string GlassRecipeId
        {
            get { return m_GlassRecipeId; }
            set { m_GlassRecipeId = value; }
        }

        public string OldGlassRecipeId
        {
            get { return m_OldGlassRecipeId; }
            set { m_OldGlassRecipeId = value; }
        }

        public string GlassRecipeVer
        {
            get { return m_GlassRecipeVer; }
            set { m_GlassRecipeVer = value; }
        }

        public string SheetJudge
        {
            get { return m_SheetJudge; }
            set { m_SheetJudge = value; }
        }
        public string PanelJudge
        {
            get { return m_PanelJudge; }
            set { m_PanelJudge = value; }
        }

        public string TestFlag
        {
            get { return m_TestFlag; }
            set { m_TestFlag = value; }
        }

        public string GotoChamber
        {
            get { return m_GotoChamber; }
            set { m_GotoChamber = value; }
        }

        public enumHostGlassStatus GlassStatus
        {
            get { return m_GlassStatus; }
            set { m_GlassStatus = value; }
        }

        public string[] PreChamber
        {
            get { return m_PreChamber; }
            set { m_PreChamber = value; }
        }

        public int AccCnt
        {
            get { return m_AccCnt; }
            set { m_AccCnt = value; }
        }


        [XmlIgnore()]
        public ApdDatas ApdDatas
        {
            get { return m_ApdDatas; }
            set { m_ApdDatas = value; }
        }

        public string Thickness
        {
            get { return m_Thickness; }
            set { m_Thickness = value; }
        }

        public LotSlotInfo()
        {
        }

        public void Initialize()
        {
        }

        public void SetAccCnt(int value)
        {
            m_AccCnt = value;
        }
    }
}
