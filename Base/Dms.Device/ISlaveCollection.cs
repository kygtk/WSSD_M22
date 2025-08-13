using System;
using System.Collections;
using Dms.Common;

namespace Dms.Device
{
    public interface ISlaveCollection
    {
        IEnumerator GetEnumerator();
        string Name { get; }
        int Count { get; }
        int MaxSimulateCount { get; set; }
        Type ContainedItemType { get; }
        EcSlaveItemType ContainedSlaveItemType { get; }
        _DeviceSlave CreateNewItem();
        void Add(object item);
        void Clear();
        _DeviceSlave GetItem(int index);
        void SetItem(int index, object item);
        void RemoveItem(int index);
        ISlaveCollection CreateCollection();
        int IndexOf(object item);
        void InsertItem(int index, object item);
    }
}
