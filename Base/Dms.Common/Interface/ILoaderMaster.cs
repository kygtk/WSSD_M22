using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public interface ILoaderMaster
    {
        string GetRecipeOf(int portId, int slotId);
    }
}
