using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Device
{
    public class HpmjItems
    {
        #region Fields
        private _GenericCollection<HpmjItem> m_Items;
        #endregion

        #region Properties
        public _GenericCollection<HpmjItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        public HpmjItem this[int id]
        {
            get { return m_Items[id]; }
        }
        public HpmjItem this[string name]
        {
            get { return m_Items[name]; }
        }
        #endregion

        #region Constructor
        public HpmjItems()
        {
            m_Items = new _GenericCollection<HpmjItem>();
        }
        public HpmjItems(_GenericCollection<HpmjItem> collection)
        {
            m_Items = new _GenericCollection<HpmjItem>();
            this.Clone(collection);
        }
        #endregion

        #region Methods
        public void Add(HpmjItem item)
        {
            m_Items.Add(item);
        }

        public void Clone(HpmjItems hpmjItems)
        {
            if (hpmjItems == null) return;

            this.Items.Clear();
            foreach (HpmjItem item in hpmjItems.Items)
            {
                this.Add(item.Clone());
            }

        }

        public void Clone(_GenericCollection<HpmjItem> collection)
        {
            if (collection == null) return;

            this.Items.Clear();

            foreach (HpmjItem item in collection)
            {
                this.Add(item.Clone());
            }
        }

        public bool IsChanged(HpmjItems hpmjItems)
        {
            bool changed = false;

            if (this.Count != hpmjItems.Count) return false;

            int count = this.Count;
            HpmjItem self;
            HpmjItem target;
            for (int i = 0; i < count; i++)
            {
                self = this[i];
                target = hpmjItems[i];
                changed = (self.CurUsedTime != target.CurUsedTime ||
                            self.MaxUsedTime != target.MaxUsedTime ||
                            self.LifeTimeOver != target.LifeTimeOver);
                if (changed) break;
            }

            return changed;
        }
        #endregion
    }
}
