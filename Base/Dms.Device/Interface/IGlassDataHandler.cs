using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    public interface IGlassDataHandler
    {
        int Count { get; }
        void Create(Dms.Data.TagGlassData data);
        void Delete(int positionId);
        void GetAllPositionId(out int[] ids);
        bool GetData(int position, ref Dms.Data.TagGlassData data);
        ulong GetPositionFlag();
        bool IsExist(int position);
        void Move(int fromPositionId, int toPositionId);
        void Update(int position, Dms.Data.TagGlassData data);

        void GetPortNo(int Position, ref int PortNo, ref int SlotNo);
        bool IsProcessed(int Position);
    }
}
