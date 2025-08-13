using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    abstract public class _Gauge : _DeviceAsm, ICtlGauge
    {
        #region Tag Descriptor
        protected static TagDescriptorGauge tagDescriptor = new TagDescriptorGauge();
        #endregion

        #region Fields
        protected short m_CurAdc = 0;
        protected double m_CurValue = 0.0;
        protected double m_OldValue = -1.0;
        protected double m_SimulCurValue = 0.0;
        protected bool m_UseSimulator = false;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public bool UseSimulator
        {
            get { return m_UseSimulator; }
            set { m_UseSimulator = value; }
        } 
        [Browsable(false), XmlIgnore()]
        public short CurAdc
        {
            get { return m_CurAdc; }
            set { m_CurAdc = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double CurValue
        {
            get { return m_CurValue; }
            set { m_CurValue = value; }
        } 
        [Browsable(false), XmlIgnore()]
        public double SimulCurValue
        {
            get { return m_SimulCurValue; }
            set { m_SimulCurValue = value; }
        } 
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion

    }
}
