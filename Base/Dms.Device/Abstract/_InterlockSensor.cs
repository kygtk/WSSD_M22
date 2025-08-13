///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.01.09
// Author       : jemoon
// Description  : DoorSensor Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    abstract public class _InterlockSensor : Sensor
    {
        #region Fields
        protected TagSetupSenSorInterlock m_SetupSensorIntr = null;
        protected bool m_IsAlarm = false;
        public Alarm ALM_Interlock = null;
        protected SensorInterlockType m_InterlockType;
        protected string m_InterlockTypeName;
        protected bool m_UseInterlock = true;
        protected bool m_UseHeavyInterlock = true;
        protected bool m_UseHardInterlock = false;
        protected AlarmLevel m_AlarmLevel = AlarmLevel.S;
        #endregion

        #region Properties
        [Category("DMS : Option")]
        [Description("Check/NoCheck option 사용 : option에 따라 Alarm)")]
        public bool UseInterlock
        {
            get { return m_UseInterlock; }
            set { m_UseInterlock = value; }
        }
        [Category("DMS : Option")]
        [Description("Check/NoCheck option 사용하지 않음 : 감지시 무조건 Alarm)")]
        public bool UseHardInterlock
        {
            get { return m_UseHardInterlock; }
            set { m_UseHardInterlock = value; }
        }
        [Category("DMS : Option")]
        [Description("Alarm시 HeavyInterlock으로 처리)")]
        public bool UseHeavyInterlock
        {
            get { return m_UseHeavyInterlock; }
            set { m_UseHeavyInterlock = value; }
        }
        [Category("DMS : Option")]
        [Description("Alarm Level)")]
        public AlarmLevel AlarmLevel
        {
            get { return m_AlarmLevel; }
            set { m_AlarmLevel = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupInterlock
        {
            get { return m_SetupSensorIntr; }
            set { m_SetupSensorIntr = value; }
        }
        #endregion

        #region Constructor
        public _InterlockSensor()
        {
        }
        #endregion

        #region Methods
        public bool IsDetectedInterlock()
        {
            bool detected = IsDetected();
            if (m_UseHardInterlock)
            {
                return detected;
            }
            else if (m_UseInterlock)
            {
                return detected && m_SetupSensorIntr.Use;
            }
            else
            {
                return false;
            }
        }

        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("LeakUnit\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(_InterlockSensor); }
        }

        public override DmsErrors Initialize()
        {
            bool ok = true;
            ok &= (SubInitialize() == DmsErrors.Success);

            if (ok)
            {
                ALM_Interlock = new Alarm(this.Name.Replace("Sensor", "") + " Alarm", m_AlarmLevel, AlarmCode.EquipmentSafety);
                if (m_UseInterlock && !m_UseHardInterlock)
                {
                    m_SetupSensorIntr = new TagSetupSenSorInterlock(this.Name, false, m_InterlockType);
                    SetupSensorInterlockProvider.Instance.InitFromDB(m_SetupSensorIntr);
                }
            }

            m_Initialized = ok;
            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }
        public abstract DmsErrors SubInitialize();

        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.DETECT, IsDetected());
        }
        #endregion
    }
}
