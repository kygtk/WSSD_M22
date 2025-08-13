using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Dms.Common;

namespace Dms.Device
{
    public interface IComponentContainer
    {
        ArrayList Items
        {
            get;
            set;
        }
        object this[int index]
        {
            get;
        }
        object this[string name]
        {
            get;
        }
        ArrayList this[Type type]
        {
            get;
        }
        //object GetCollection(Type type);
        _GenericCollection<T> GetCollection<T>();
		_GenericCollection<T> GetCollection<T>(Compatibility compatibility);
		ArrayList GetCollectionArray(Type type, Compatibility compatibility);
    }
}
