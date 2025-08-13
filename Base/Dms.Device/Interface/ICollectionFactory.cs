using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public interface ICollectionFactory : IDeviceFactory
    {
        IGenericCollection CreateCollection();
        void Add(object item);
    }
}
