using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.DeviceLibrary
{
    [Serializable()]
    public class DmsGUIUpdateItems
    {
        #region Fields
        private List<DmsGUIUpdateItem> m_Items = new List<DmsGUIUpdateItem>();
        #endregion

        #region Properties
        public List<DmsGUIUpdateItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public DmsGUIUpdateItem this[string name]
        {
            get
            {
                foreach (DmsGUIUpdateItem dgui in m_Items)
                {
                    if (name == dgui.GetName())
                        return dgui;
                }
                return null;
            }
        }
        #endregion

        #region Constructor
        public DmsGUIUpdateItems()
        {
        }
        #endregion

        #region Methods
        public void Add(DmsGUIUpdateItem item)
        {
            m_Items.Add(item);
        }

        public void Clear()
        {
            m_Items.Clear();
        }
        #endregion
    }
}
