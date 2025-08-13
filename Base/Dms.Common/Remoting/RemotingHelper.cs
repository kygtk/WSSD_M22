using System;
using System.Collections;
using System.Text;
using System.Runtime.Remoting;

namespace Dms.Common
{
    public class RemotingHelper
    {
        internal static bool m_isInit = false;
        private static IDictionary m_wellKnownTypes;

        public static object GetObject(Type type)
        {
            if (!m_isInit)
                InitTypeCache();
            WellKnownClientTypeEntry entr = (WellKnownClientTypeEntry)m_wellKnownTypes[type];
            if (entr == null)
                throw new RemotingException("Type not found");
            return Activator.GetObject(entr.ObjectType, entr.ObjectUrl);
        }

        private static void InitTypeCache()
        {
            m_wellKnownTypes = new Hashtable();
            foreach (WellKnownClientTypeEntry entr in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
                m_wellKnownTypes.Add(entr.ObjectType, entr);
            m_isInit = true;
        }
    }
}
