using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    [Serializable()]
    public class TagUnitInfo
    {
        private int id;
        private int groupno;
        private string name;
        private string unitid;
        private string apdname;
        private bool recipeyesno;
        private bool currentdatayesno;
        private bool initialdatayesno;
        private bool initialdatamodeyesno;
        private bool eqpnodeyesno;
        private bool eqpsetupyesno;
        private bool eqpsequenceyesno;
        private bool alarmlistyesno;
        private SubUnitInfo subunitinfos;
        private EqpUnitInfo eqpunitinfos;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int GroupNo
        {
            get { return groupno; }
            set { groupno = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string UnitId
        {
            get { return unitid; }
            set { unitid = value; }
        }

        public string ApdName
        {
            get { return apdname; }
            set { apdname = value; }
        }

        public bool RecipeYesNo
        {
            get { return recipeyesno; }
            set { recipeyesno = value; }
        }

        public bool CurrentDataYesNo
        {
            get { return currentdatayesno; }
            set { currentdatayesno = value; }
        }

        public bool InitialDataYesNo
        {
            get { return initialdatayesno; }
            set { initialdatayesno = value; }
        }

        public bool InitialDataModeYesNo
        {
            get { return initialdatamodeyesno; }
            set { initialdatamodeyesno = value; }
        }

        public bool EqpNodeYesNo
        {
            get { return eqpnodeyesno; }
            set { eqpnodeyesno = value; }
        }

        public bool EqpSetupYesNo
        {
            get { return eqpsetupyesno; }
            set { eqpsetupyesno = value; }
        }

        public bool EqpSequenceYesNo
        {
            get { return eqpsequenceyesno; }
            set { eqpsequenceyesno = value; }
        }

        public bool AlarmListYesNo
        {
            get { return alarmlistyesno; }
            set { alarmlistyesno = value; }
        }

        public SubUnitInfo SubUnitInfos
        {
            get { return subunitinfos; }
            set { subunitinfos = value; }
        }

        public EqpUnitInfo EqpUnitInfos
        {
            get { return eqpunitinfos; }
            set { eqpunitinfos = value; }
        }

        public TagUnitInfo()
        {
        }

        public TagUnitInfo(int id, int groupno, string name, string unitid, string apdname,  bool recipeyesno, bool currentdatayesno, bool initialdatayesno, bool initialdatamodeyesno, bool eqpnodeyesno, bool eqpsetupyesno, bool eqpsequenceyesno, bool alarmlistyesno, SubUnitInfo subunitinfo, EqpUnitInfo eqpunitinfo)
        {
            this.id = id;
            this.groupno = groupno;
            this.name = name;
            this.unitid = unitid;
            this.apdname = apdname;
            this.recipeyesno = recipeyesno;
            this.currentdatayesno = currentdatayesno;
            this.initialdatayesno = initialdatayesno;
            this.initialdatamodeyesno = initialdatamodeyesno;
            this.eqpnodeyesno = eqpnodeyesno;
            this.eqpsetupyesno = eqpsetupyesno;
            this.eqpsequenceyesno = eqpsequenceyesno;
            this.alarmlistyesno = alarmlistyesno;
            this.subunitinfos = subunitinfo;
            this.eqpunitinfos = eqpunitinfo;
        }

        public void Clone(TagUnitInfo tagunitinfo)
        {
            this.id = tagunitinfo.id;
            this.groupno = tagunitinfo.groupno;
            this.name = tagunitinfo.name;
            this.unitid = tagunitinfo.unitid;
            this.apdname = tagunitinfo.apdname;
            this.recipeyesno = tagunitinfo.recipeyesno;
            this.currentdatayesno = tagunitinfo.currentdatayesno;
            this.initialdatayesno = tagunitinfo.initialdatayesno;
            this.initialdatamodeyesno = tagunitinfo.initialdatamodeyesno;
            this.eqpnodeyesno = tagunitinfo.eqpnodeyesno;
            this.eqpsetupyesno = tagunitinfo.eqpsetupyesno;
            this.eqpsequenceyesno = tagunitinfo.eqpsequenceyesno;
            this.alarmlistyesno = tagunitinfo.alarmlistyesno;
            this.subunitinfos = tagunitinfo.subunitinfos;
            this.eqpunitinfos = tagunitinfo.eqpunitinfos;
        }
    }
}
