using System;
using System.Collections;
using System.Text;

namespace Dms.Remoting
{
    public class RemoteSingletonObjectsList
    {
        private static RemoteSingletonObjectsList m_instance = new RemoteSingletonObjectsList();
        public static RemoteSingletonObjectsList Instance
        {
            get
            {
                return m_instance;
            }
        }
        private RemoteSingletonObjectsList()
        {
        }

        private Hashtable m_objects = new Hashtable();

        public void RegisterRemoteSingletonObject(object obj)
        {
            m_objects[obj.GetType()] = obj;
        }

        public object GetRemoteSingletonObject(Type type)
        {
            return m_objects[type];
        }
    }
}
