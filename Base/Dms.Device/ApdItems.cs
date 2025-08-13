using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    public class ApdItems
    {
        #region Fields
        private int m_PositionId;
        private _GenericCollection<ApdItem> m_Items;
        #endregion

        #region Properties
        [Browsable(false)]
        public int PositionId
        {
            get { return m_PositionId; }
            set { m_PositionId = value; }
        }
        public _GenericCollection<ApdItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        public ApdItem this[int id]
        {
            get { return m_Items[id]; }
        }
        #endregion

        #region Constructor
        public ApdItems()
        {
            m_Items = new _GenericCollection<ApdItem>();
        }
        public ApdItems(_GenericCollection<ApdItem> collection)
        {
            m_Items = new _GenericCollection<ApdItem>();
            m_Items = collection;
        }
        #endregion

        #region Methods
        public void Add(ApdItem item)
        {
            m_Items.Add(item);
        }

        public void Clone(ApdItems apdItems)
        {
            if (apdItems == null) return;

            this.Items.Clear();
            foreach (ApdItem item in apdItems.Items)
            {
                this.Add(item.Clone());
            }
        }

        public void Clone(_GenericCollection<ApdItem> collection)
        {
            if (collection == null) return;

            this.Items.Clear();
            foreach (ApdItem item in collection)
            {
                this.Add(item.Clone());
            }        
        }
        
        public bool IsChanged(ApdItems apdItems)
        { 
            bool changed = false;
            changed = (apdItems.PositionId != m_PositionId);

            if (this.Count != apdItems.Count) return false;

            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                changed = (this[i].Value != apdItems[i].Value);
                if (changed) break;
            }

            return changed;
        }

        public void SyncRefDeviceTag(DeviceTags tagContainer)
        {
            foreach (ApdItem item in m_Items)
            {
                item.SyncRefDeviceTag(tagContainer);
            }
        }
        #endregion
    }
}
