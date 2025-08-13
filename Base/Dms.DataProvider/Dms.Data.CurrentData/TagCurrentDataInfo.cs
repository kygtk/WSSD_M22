using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    [Serializable()]
    public class TagCurrentDataInfo
    {
        private int unitno;
        private int id;
        private string name;
        private string dvname;
        private string type;
        private string format;
        private int wordsize;
        private int point;
        private string unit;
        private string address;

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

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public TagCurrentDataInfo()
        {
        }

        public TagCurrentDataInfo(int unitno, int id, string name, string dvname, string type, string format, int wordsize, int point, string unit, string address)
        {
            this.unitno = unitno;
            this.id = id;
            this.name = name;
            this.dvname = dvname;
            this.type = type;
            this.format = format;
            this.wordsize = wordsize;
            this.point = point;
            this.unit = unit;
            this.address = address;
        }

        public void Clone(TagCurrentDataInfo tagcurrentdatainfo)
        {
            this.unitno = tagcurrentdatainfo.unitno;
            this.id = tagcurrentdatainfo.id;
            this.name = tagcurrentdatainfo.name;
            this.dvname = tagcurrentdatainfo.dvname;
            this.type = tagcurrentdatainfo.type;
            this.format = tagcurrentdatainfo.format;
            this.wordsize = tagcurrentdatainfo.wordsize;
            this.point = tagcurrentdatainfo.point;
            this.unit = tagcurrentdatainfo.unit;
            this.address = tagcurrentdatainfo.address;
        }
    }
}
