using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    abstract public class _Motor : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor(); 
        #endregion

        #region Fields
        protected bool m_MotorReverseAct = false;
        protected bool m_ReverseMotorUI = false;
        #endregion

        #region Properties
        [Category("DMS : Parameter Setting")]
        public bool MotorReverseAct
		{
			get { return m_MotorReverseAct; }
            set { m_MotorReverseAct = value; }
		}
		[Browsable(false), XmlIgnore()]
		public bool IsMotorReverseUI
		{
			get { return m_ReverseMotorUI; }
		}
		#endregion

		#region Methods
		abstract public void Stop();
        abstract public void TurnCw();
        abstract public void TurnCcw();
        abstract public bool IsStop();
        abstract public bool IsTurnCw();
        abstract public bool IsTurnCcw();
        abstract public bool IsAlarm();
        abstract public bool IsCpOn();
        abstract public bool SetMotorAct(int act); 
        abstract public void SetSpeed(ushort speed);
        abstract public void SetAcc(short acc);
        abstract public int GetCurSpeed();
        abstract public int GetMaxSpeed();
        abstract public int GetMinSpeed();
        abstract public Alarm GetDriverAlarm();
        abstract public Alarm GetCpAlarm(); 
        public void SetReverseMotorAct(bool enable)
        {
            m_MotorReverseAct = enable;
        }
        public void SetReverseMotorUI(bool enable)
        {
            m_ReverseMotorUI = enable;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(_Motor); }
        } 
        #endregion
    }
}
