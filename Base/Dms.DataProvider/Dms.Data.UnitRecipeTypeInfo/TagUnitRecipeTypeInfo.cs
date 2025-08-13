using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class TagUnitRecipeTypeInfo
    {
        private int unitno;
        private string type;
        private int colno;
        private int rowno;
        private string title;
        private string valueindex;

        public int UnitNo
        {
            get { return unitno; }
            set { unitno = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int ColNo
        {
            get { return colno; }
            set { colno = value; }
        }

        public int RowNo
        {
            get { return rowno; }
            set { rowno = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string ValueIndex
        {
            get { return valueindex; }
            set { valueindex = value; }
        }

        public TagUnitRecipeTypeInfo()
        {}

        public TagUnitRecipeTypeInfo(int unitno, string type, int colno, int rowno, string title, string valueindex)
        {
            this.unitno = unitno;
            this.type = type;
            this.colno = colno;
            this.rowno = rowno;
            this.title = title;
            this.valueindex = valueindex;
        }

        public void Clone(TagUnitRecipeTypeInfo info)
        {
            this.unitno = info.unitno;
            this.type = info.type;
            this.colno = info.colno;
            this.rowno = info.rowno;
            this.title = info.title;
            this.valueindex = info.valueindex;
        }
    }
}
