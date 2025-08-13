using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;

namespace Dms.Device
{
    public delegate bool PlasmaPowerOnInterlockConditionDelegate(_Plasma unit);

    abstract public class _Plasma : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorPlasma tagDescriptor = new TagDescriptorPlasma();
        #endregion

        #region Fields
        protected Mfc m_MfcN2 = new Mfc();
        protected Mfc m_MfcCDA = new Mfc();
        protected Mfc m_MfcVoltage = new Mfc();
        protected ActuatorUnit m_ActuatorUnit;
        protected static PlasmaPowerOnInterlockConditionDelegate m_IsPowerOnInterlockCondition;
		protected ProcessControlBy m_ControlBy = ProcessControlBy.Recipe;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public Mfc MfcN2
        {
            get { return m_MfcN2; }
            set { m_MfcN2 = value; }
        }
        [Category("DMS : Setting")]
        public Mfc MfcCDA
        {
            get { return m_MfcCDA; }
            set { m_MfcCDA = value; }
        }
        [Category("DMS : Setting")]
        public Mfc MfcVoltage
        {
            get { return m_MfcVoltage; }
            set { m_MfcVoltage = value; }
        }
        [Category("DMS : Setting")]
        public ActuatorUnit ActuatorUnit
        {
            get { return m_ActuatorUnit; }
            set { m_ActuatorUnit = value; }
        }
		[Category("DMS : Option"), Description("Control by Setup or Recipe")]
		public ProcessControlBy ControlBy
		{
			get { return m_ControlBy; }
			set { m_ControlBy = value; }
		}
        #endregion

        #region Methods
        abstract public void PowerOn();
        abstract public void PowerOff();
        abstract public bool IsPowerOn();
        abstract public bool IsPowerOff();
        abstract public void SetVoltage(double voltage);
        abstract public void SetN2Flow(double flow);
        abstract public void SetCDAFlow(double flow);
        public static void SetInterlockScenario(PlasmaPowerOnInterlockConditionDelegate scenario)
        {
            m_IsPowerOnInterlockCondition = scenario;
        }
        abstract public bool IsInterlockCondition();
		abstract public bool IsUse();
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return this.GetType(); }
        }
        #endregion

    }
}
