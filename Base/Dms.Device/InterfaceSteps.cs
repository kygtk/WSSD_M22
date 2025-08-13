using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    public class InterfaceSteps : _GenericCollection<InterfaceStep>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Methods
        public void Clone(InterfaceSteps interfaceStemps)
        {
            if (interfaceStemps == null) return;

            this.m_Items.Clear();
            foreach (InterfaceStep item in interfaceStemps)
            {
                this.Items.Add(item.Clone());
            }
        }

        public bool IsChanged(InterfaceSteps interfaceStemps)
        {
            bool changed = false;

            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                changed = (this[i].StepNo != interfaceStemps[i].StepNo);
                if (changed) break;
            }

            return changed;
        }
        #endregion

        #region Override
        #endregion
    }
}
