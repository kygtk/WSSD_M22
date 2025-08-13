using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    abstract public class _EuvUnit : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorEuvUnit tagDescriptor = new TagDescriptorEuvUnit();
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion
    }
}
