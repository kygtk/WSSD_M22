using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dms.Device
{
    public class PartsItems
    {
        #region Fields
        private _GenericCollection<PartsItem> m_Items;
        #endregion

        #region Properties
        public _GenericCollection<PartsItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        public PartsItem this[int id]
        {
            get { return m_Items[id]; }
        }
        public PartsItem this[string name]
        {
            get { return m_Items[name]; }
        }
        #endregion

        #region Constructor
        public PartsItems()
        {
            m_Items = new _GenericCollection<PartsItem>();
        }
        public PartsItems(_GenericCollection<PartsItem> collection)
        {
            m_Items = new _GenericCollection<PartsItem>();
            this.Clone(collection);
        }
        #endregion

        #region Methods
        public void Add(PartsItem item)
        {
            m_Items.Add(item);
        }

        public void Clone(PartsItems partsItems)
        {
            if (partsItems == null) return;

            this.Items.Clear();
            foreach (PartsItem item in partsItems.Items)
            {
                this.Add(item.Clone());
            }

        }

        public void Clone(_GenericCollection<PartsItem> collection)
        {
            if (collection == null) return;

            this.Items.Clear();

            foreach (PartsItem item in collection)
            {
                this.Add(item.Clone());
            }
        }

        public bool IsChanged(PartsItems partsItems)
        {
            bool changed = false;

            if (this.Count != partsItems.Count) return false;

            int count = this.Count;
            PartsItem self;
            PartsItem target;
            for (int i = 0; i < count; i++)
            {
                self = this[i];
                target = partsItems[i];
                changed = (self.CurGlsCount != target.CurGlsCount ||
                            self.CurUsedTime != target.CurUsedTime ||
                            self.MaxGlsCount != target.MaxGlsCount ||
                            self.MaxUsedTime != target.MaxUsedTime ||
                            self.LifeTimeOver != target.LifeTimeOver);
                if (changed) break;
            }

            return changed;
        }
        #endregion
    }
}
