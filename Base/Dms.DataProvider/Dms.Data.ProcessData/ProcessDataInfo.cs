using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    [Serializable()]
    public class TagGlassApdInfo
    {
        private int unitno;
        private string apdunitname;
        private int id;
        private string name;
        private string dvname;
        private string type;
        private string format;
        private int wordsize;
        private int point;
        private string address;
        private string unit;
        private bool usedata;
        private bool hostreport;
//        private bool lotapd;

        public int UnitNo
        {
            get { return unitno; }
            set { unitno = value; }
        }

        public string ApdUnitName
        {
            get { return apdunitname; }
            set { apdunitname = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string DvName
        {
            get { return dvname; }
            set { dvname = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        public int WordSize
        {
            get { return wordsize; }
            set { wordsize = value; }
        }
        public int Point
        {
            get { return point; }
            set { point = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public bool UseData
        {
            get { return usedata; }
            set { usedata = value; }
        }
        public bool HostReport
        {
            get { return hostreport; }
            set { hostreport = value; }
        }
/*
        public bool LotApd
        {
            get { return lotapd; }
            set { lotapd = value; }
        }
*/
        public TagGlassApdInfo()
        {
        }

//        public TagGlassApdInfo(int unitno, string apdunitname, int id, string name, string dvname, string type, string format, int wordsize, int point, string address, string unit, bool usedata, bool hostreport, bool lotapd)
        public TagGlassApdInfo(int unitno, string apdunitname, int id, string name, string dvname, string type, string format, int wordsize, int point, string address, string unit, bool usedata, bool hostreport)
        {
            this.unitno = unitno;
            this.apdunitname = apdunitname;
            this.id = id;
            this.name = name;
            this.dvname = dvname;
            this.type = type;
            this.format = format;
            this.wordsize = wordsize;
            this.point = point;
            this.address = address;
            this.unit = unit;
            this.usedata = usedata;
            this.hostreport = hostreport;
//            this.lotapd = lotapd;
        }

        public void Clone(TagGlassApdInfo info)
        {
            this.unitno = info.unitno;
            this.apdunitname = info.apdunitname;
            this.id = info.id;
            this.name = info.name;
            this.dvname = info.dvname;
            this.type = info.type;
            this.format = info.format;
            this.wordsize = info.wordsize;
            this.point = info.point;
            this.address = info.address;
            this.unit = info.unit;
            this.usedata = info.usedata;
            this.hostreport = info.hostreport;
//            this.lotapd = info.lotapd;
        }
    }

    [Serializable()]
    public class TagLotApdInfo
    {
        private int id;
        private string name;
        private string type;
        private string format;
        private bool useapd;
        private bool hostreport;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        public bool UseApd
        {
            get { return useapd; }
            set { useapd = value; }
        }

        public bool HostReport
        {
            get { return hostreport; }
            set { hostreport = value; }
        }

        public TagLotApdInfo()
        {
        }

        public TagLotApdInfo(int id, string name, string type, string format, bool useapd, bool hostreport)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.format = format;
            this.useapd = useapd;
            this.hostreport = hostreport;
        }

        public void Clone(TagLotApdInfo taglotapdinfo)
        {
            this.id = taglotapdinfo.id;
            this.name = taglotapdinfo.name;
            this.type = taglotapdinfo.type;
            this.format = taglotapdinfo.format;
            this.useapd = taglotapdinfo.useapd;
            this.hostreport = taglotapdinfo.hostreport;
        }
    }

    [Serializable()]
    public class TagTraceDataInfo
    {
        private int unitno;
        private string traceunitname;
        private int id;
        private string name;
        private string dvname;
        private string type;
        private string format;
        private int wordsize;
        private int point;
        private string address;
        private string unit;
        private bool usedata;
        private bool hostreport;

        public int UnitNo
        {
            get { return unitno; }
            set { unitno = value; }
        }

        public string TraceUnitName
        {
            get { return traceunitname; }
            set { traceunitname = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string DvName
        {
            get { return dvname; }
            set { dvname = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        public int WordSize
        {
            get { return wordsize; }
            set { wordsize = value; }
        }
        public int Point
        {
            get { return point; }
            set { point = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public bool UseData
        {
            get { return usedata; }
            set { usedata = value; }
        }
        public bool HostReport
        {
            get { return hostreport; }
            set { hostreport = value; }
        }

        public TagTraceDataInfo()
        {
        }

        public TagTraceDataInfo(int unitno, string traceunitname, int id, string name, string dvname, string type, string format, int wordsize, int point, string address, string unit, bool usedata, bool hostreport)
        {
            this.unitno = unitno;
            this.traceunitname = traceunitname;
            this.id = id;
            this.name = name;
            this.dvname = dvname;
            this.type = type;
            this.format = format;
            this.wordsize = wordsize;
            this.point = point;
            this.address = address;
            this.unit = unit;
            this.usedata = usedata;
            this.hostreport = hostreport;
        }

        public void Clone(TagTraceDataInfo info)
        {
            this.unitno = info.unitno;
            this.traceunitname = info.traceunitname;
            this.id = info.id;
            this.name = info.name;
            this.dvname = info.dvname;
            this.type = info.type;
            this.format = info.format;
            this.wordsize = info.wordsize;
            this.point = info.point;
            this.address = info.address;
            this.unit = info.unit;
            this.usedata = info.usedata;
            this.hostreport = info.hostreport;
        }
    }

    [Serializable()]
    public class TagRecipeItemInfo
    {
        private int unitno;
        private string recipeunitname;
        private int id;
        private string name;
        private string pparmname;
        private string type;
        private string format;
        private string minvalue;
        private string maxvalue;
        private int wordsize;
        private int point;
        private string eqp_address;
        private string cim_address;
        private string unit;
        private bool usedata;
        private bool hostreport;

        public int UnitNo
        {
            get { return unitno; }
            set { unitno = value; }
        }

        public string RecipeUnitName
        {
            get { return recipeunitname; }
            set { recipeunitname = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string PparmName
        {
            get { return pparmname; }
            set { pparmname = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        public string MinValue
        {
            get { return minvalue; }
            set { minvalue = value; }
        }
        public string MaxValue
        {
            get { return maxvalue; }
            set { maxvalue = value; }
        }

        public int WordSize
        {
            get { return wordsize; }
            set { wordsize = value; }
        }
        public int Point
        {
            get { return point; }
            set { point = value; }
        }

        public string EqpAddress
        {
            get { return eqp_address; }
            set { eqp_address = value; }
        }

        public string CimAddress
        {
            get { return cim_address; }
            set { cim_address = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public bool UseData
        {
            get { return usedata; }
            set { usedata = value; }
        }
        public bool HostReport
        {
            get { return hostreport; }
            set { hostreport = value; }
        }
        public TagRecipeItemInfo()
        {
        }

        public TagRecipeItemInfo(int unitno, string recipeunitname, int id, string name, string pparmname, string type, string format, string minvalue, string maxvalue, int wordsize, int point, string eqp_address, string cim_address, string unit, bool usedata, bool hostreport)
        {
            this.unitno = unitno;
            this.recipeunitname = recipeunitname;
            this.id = id;
            this.name = name;
            this.pparmname = pparmname;
            this.type = type;
            this.format = format;
            this.minvalue = minvalue;
            this.maxvalue = maxvalue;
            this.wordsize = wordsize;
            this.point = point;
            this.eqp_address = eqp_address;
            this.cim_address = cim_address;
            this.unit = unit;
            this.usedata = usedata;
            this.hostreport = hostreport;
        }

        public void Clone(TagRecipeItemInfo info)
        {
            this.unitno = info.unitno;
            this.recipeunitname = info.recipeunitname;
            this.id = info.id;
            this.name = info.name;
            this.pparmname = info.pparmname;
            this.type = info.type;
            this.format = info.format;
            this.minvalue = info.minvalue;
            this.maxvalue = info.maxvalue;
            this.wordsize = info.wordsize;
            this.point = info.point;
            this.eqp_address = info.eqp_address;
            this.cim_address = info.cim_address;
            this.unit = info.unit;
            this.usedata = info.usedata;
            this.hostreport = info.hostreport;
        }
    }
}
