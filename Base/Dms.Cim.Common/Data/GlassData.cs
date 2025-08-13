using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dms.Cim.Common
{
    [Serializable()]
    public class EqpGlassData
    {
        public int UnitNo;
        public int PortNo;
        public int SlotNo;
        public string RecipeId;
        public string CarrierId;

        public void Clone(EqpGlassData data)
        {
            this.UnitNo = data.UnitNo;
            this.PortNo = data.PortNo;
            this.SlotNo = data.SlotNo;
            this.RecipeId = data.RecipeId;
            this.CarrierId = data.CarrierId;
        }
    }

    [Serializable()]
    public class EqpGlassDataList
    {
        public List<EqpGlassData> List;
        public EqpGlassDataList(List<EqpGlassData> list)
        {
            List = new List<EqpGlassData>();
            List = list;
        }
    }
}
