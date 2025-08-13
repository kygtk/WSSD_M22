using System;
using System.Collections;
using Dms.Common;

namespace Dms.Device
{
    public interface iIoCollection
    {
        IEnumerator GetEnumerator();
        string Name { get; }
        int Count { get; }
        int MaxSimulateCount { get; set;}
        Type ContainedItemType { get; }
        IoType ContainedIoType { get; }
        _DeviceIo CreateNewItem();
        void Add(object item);
        void Clear();
        _DeviceIo GetItem(int index);
        void SetItem(int index, object item);
        void RemoveItem(int index);
        iIoCollection CreateCollection();
        int IndexOf(object item);
        void InsertItem(int index, object item);
    }
}
