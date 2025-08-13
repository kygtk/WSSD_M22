using System;
using System.Collections;
using Dms.Common;

namespace Dms.Device
{
    public interface IGenericCollection
    {
        string Name { get; }
        Type ContainedItemType { get; }
        Type CollectionType { get; }
        ModelType ContainedItemModel { get;}
        int Count { get; }
        ArrayList Items { get; set; }
        IServerManager ServerManager { get; }
        IComponentContainer ComponentContainer { get; }

        void Add(object obj);
        void Remove(object obj);
        _Device GetItem(int index);
        void SetItem(int index, object item);
        void RemoveItem(int index);
        void Clear();
        object GetCollection();
        void UpdateItemId();
        void SetComponentContainer(IComponentContainer components);
        void SetItems(ArrayList items);
        void SyncInstance(IComponentContainer components);
        bool Initialize(IServerManager server, _GenInfoHandler geninfos);
        void Uninitialize();
        
        IEnumerator GetEnumerator();
    }
}
