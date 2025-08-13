using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    abstract public class _Valve : _DeviceAsm, ICtlPiping
    {
        #region Tag Descriptor
        protected TagDescriptorValve tagDescriptor = new TagDescriptorValve();
        #endregion

        #region Fields
        protected bool m_TypeNormalClose = true;
        #endregion

        #region Properties
        [Category("DMS : Option")]
        [Description("Default : Set Valve Normal Close Type")]
        public bool TypeNormalClose
        {
            get { return m_TypeNormalClose; }
            set { m_TypeNormalClose = value; }
        }
        #endregion

        #region Methods
        abstract public void Open();
        abstract public void Close();
        abstract public bool IsOpen();
        abstract public bool IsClose();
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion

        #region ICtlPiping ¸â¹ö
        abstract public bool IsProcess();
        #endregion
    }
}
