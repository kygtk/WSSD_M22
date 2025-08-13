///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Device class
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.27 - jemoon : code review
// * 2008.03.06 - jemoon : m_DeviceState/Start/Pause/Resume 은 필요없다 판단하여 제거
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Common
{
    [Serializable()]
    abstract public class _Device : IDeviceFactory
    {
        #region Fields
        protected int m_Id;
        protected string m_Name;
        protected string m_Description;
        protected bool m_Initialized;
        protected DeviceTag m_Tag = null;
        #endregion

        #region Properties
        [Category("DMS : Basic Info"), ReadOnly(true)]
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        [Category("DMS : Basic Info")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [Category("DMS : Basic Info")]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        [Category("DMS : Basic Info"), ReadOnly(true), XmlIgnore()]
        public Type DeviceType
        {
            get { return this.GetType(); }
        }

        [Category("DMS : Basic Info"), ReadOnly(true), XmlIgnore()]
        public virtual Type FamilyType
        {
            get { return typeof(_Device); }
        }

        [Browsable(false), XmlIgnore()]
        public bool Initialized
        {
            get { return m_Initialized; }
            set { m_Initialized = value; }
        }

        [Browsable(false), XmlIgnore()]
        public DeviceTag Tag
        {
            get { return m_Tag; }
        }
        #endregion

        #region Methods
        abstract public void CreateTag(DeviceTags tagContainer);
        abstract public void UpdateTag();
        abstract public DmsErrors Initialize();
        public virtual DmsErrors Uninitialize()
        {
            m_Initialized = false;
            return DmsErrors.Success;
        }
        #endregion

        #region override
        public override string ToString()
        {
            return m_Name;
        }
        #endregion

        #region IDeviceFactory Implement
        public _Device CreateObject()
        {
            return Activator.CreateInstance(this.GetType()) as _Device;
        }
        #endregion
    }
}
