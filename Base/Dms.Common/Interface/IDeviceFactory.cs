using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public interface IDeviceFactory
    {
        _Device CreateObject();
    }
}
