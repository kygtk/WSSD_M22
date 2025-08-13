using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    [Serializable()]
    public class TagUnitRecipeInfo
    {
        private int unitno;
        private int id;
        private string name;
        private string dvname;
        private string type;
        private string format;
        private string lowvalue;
        private string highvalue;
        private int wordsize;
        private int point;
        private string unit;
        private string valuetype;
        private string address;
        private string checkaddress;

        public int UnitNo
        {
            get { return unitno; }
            set { unitno = value; }
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
        public string LowValue
        {
            get { return lowvalue; }
            set { lowvalue = value; }
        }
        public string HighValue
        {
            get { return highvalue; }
            set { highvalue = value; }
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

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public string ValueType
        {
            get { return valuetype; }
            set { valuetype = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string CheckAddress
        {
            get { return checkaddress; }
            set { checkaddress = value; }
        }

        public TagUnitRecipeInfo()
        {
        }

        public TagUnitRecipeInfo(int unitno, int id, string name, string dvname, string type, string format, 
                                 string lowvalue, string highvalue, int wordsize, int point, string unit, string valuetype, string address, 
                                 string checkaddress)
        {
            this.unitno = unitno;
            this.id = id;
            this.name = name;
            this.dvname = dvname;
            this.type = type;
            this.format = format;
            this.lowvalue = lowvalue;
            this.highvalue = highvalue;
            this.wordsize = wordsize;
            this.point = point;
            this.unit = unit;
            this.valuetype = valuetype;
            this.address = address;
            this.checkaddress = checkaddress;
        }

        public void Clone(TagUnitRecipeInfo info)
        {
            this.unitno = info.unitno;
            this.id = info.id;
            this.name = info.name;
            this.dvname = info.dvname;
            this.type = info.type;
            this.format = info.format;
            this.lowvalue = info.lowvalue;
            this.highvalue = info.highvalue;
            this.wordsize = info.wordsize;
            this.point = info.point;
            this.unit = info.unit;
            this.valuetype = info.valuetype;
            this.address = info.address;
            this.checkaddress = info.checkaddress;
       }
    }
}
