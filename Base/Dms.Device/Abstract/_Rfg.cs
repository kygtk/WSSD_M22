using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Ctl;

namespace Dms.Device
{
    abstract public class _Rfg : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorRfg tagDescriptor = new TagDescriptorRfg(); 
        #endregion

        #region Fields
        protected bool m_RfgOnState;
        protected double m_SetRfPower;
        protected double m_ForwardPowerValue;
        protected double m_ReflectedPowerValue;
        
        protected CommType m_CommType;
        private PortNo m_PortNo = PortNo.COM2;
        private XComm m_Comm;

        private static TagSetupInfo m_SetupForwardInterlockRange = null;
        private static TagSetupInfo m_SetupReflectedInterlockRange = null;

        public Alarm ALM_ForwardInterlockAlarm;
        public Alarm ALM_ReflectedInterlockAlarm;

        #endregion

		#region Properties
        [Category("DMS :Comm Setting")]
        public CommType CommType
        {
            get { return m_CommType; }
            set { m_CommType = value; }
        }
        [Category("DMS :Comm Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public XComm Comm
        {
            get { return m_Comm; }
            set { m_Comm = value; }

        }
        [Browsable(false), XmlIgnore()]
        public double RfPower
        {
            get { return m_SetRfPower; }
            set { m_SetRfPower = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupForwardInterlockRange
        {
            get { return m_SetupForwardInterlockRange; }
            set { m_SetupForwardInterlockRange = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupReflectedInterlockRange
        {
            get { return m_SetupReflectedInterlockRange; }
            set { m_SetupReflectedInterlockRange = value; }
        }
        
		#endregion

		#region Methods
        abstract public void SetRfPower(double power);
        abstract public double GetForwardRfPower();
        abstract public double GetReflectedRfPower();
        abstract public void RfPowerOn(bool state);
        abstract public bool IsRfPowerOn();
        abstract public void SetInterlock(bool state);

        abstract public bool IsForwardInterlock(double range);
        abstract public bool IsReflectedInterlock(double range);



        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(_Rfg); }
        } 
        #endregion
    }
}
