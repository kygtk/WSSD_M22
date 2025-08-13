///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Collection of GenericTag
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Common
{
    [Serializable()]
    public class GenericTags
    {
        #region Implement IEnumerator
        // IEnumerable Interface Implementation:
        // Declaration of the GetEnumerator() method 
        // required by IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new InnerEnumerator(this);
        }

        // Inner class implements IEnumerator interface:
        private class InnerEnumerator : IEnumerator
        {
            private int m_Index = -1;
            private GenericTags m_Collection;

            public InnerEnumerator(GenericTags collection)
            {
                m_Collection = collection;
            }

            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (m_Index < m_Collection.Count - 1)
                {
                    m_Index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                m_Index = -1;
            }

            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return m_Collection.Items[m_Index];
                }
            }
        }
        #endregion

        #region Fields
        private List<GenericTag> m_Items = new List<GenericTag>();
        #endregion

        #region Properties
        public List<GenericTag> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        [XmlIgnore()]
        public int Count
        {
            get { return m_Items.Count; }
        }
        public GenericTag this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }
        #endregion

        #region Constructor
        public GenericTags()
        { 
        }
    	#endregion

        #region Methods
        public void Add(GenericTag tag)
        {
            m_Items.Add(tag);
        }
        public void Clear()
        {
            m_Items.Clear();
        }
        #endregion
    }
}
