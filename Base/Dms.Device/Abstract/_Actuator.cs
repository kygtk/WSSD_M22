using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using Dms.Data;
using System.Threading;
using System.Windows.Forms;

namespace Dms.Device
{
    abstract public class _Actuator : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion
        
        #region Fields
        protected ActuatorAct m_RefAct;
        protected ActuatorAct m_CurAct;
        protected int m_MoveTimeout = 5;
        protected int m_HomeTimeout = 20;
        protected _GenericCollection<Sensor> m_PositiveInterlockSensors = new _GenericCollection<Sensor>();
        protected _GenericCollection<Sensor> m_NegativeInterlockSensors = new _GenericCollection<Sensor>();
        #endregion

        #region Properties
        public override Type FamilyType
        {
            get { return typeof(_Actuator); }
        }
        [Category("DMS : Option - Interlock")]
        public _GenericCollection<Sensor> PosInterlockSensors
        {
            get { return m_PositiveInterlockSensors; }
            set { m_PositiveInterlockSensors = value; }
        }
        [Category("DMS : Option - Interlock")]
        public _GenericCollection<Sensor> NegInterlockSensors
        {
            get { return m_NegativeInterlockSensors; }
            set { m_NegativeInterlockSensors = value; }
        }
        [Category("DMS : Option - Setup")]
        public int MoveTimeout
        {
            get { return m_MoveTimeout; }
            set { m_MoveTimeout = value; }
        }
        [Category("DMS : Option - Setup")]
        public int HomeTimeout
        {
            get { return m_HomeTimeout; }
            set { m_HomeTimeout = value; }
        }
        #endregion

        #region Methods
        abstract public int SetAct(ActuatorAct act);
        abstract public ActuatorAct GetCurAct();
        abstract public ActuatorAct GetRefAct();
        abstract public bool IsActStatus(ActuatorAct act);
        abstract public bool IsAlarm();
        abstract public AlarmList GetAlarmList();
        public bool IsInterlockCondition(ActuatorAct act)
        {
            _GenericCollection<Sensor> interlockSensors = null;
            //jemoon 추가적으로 필요한게 있으면 아래 케이스에 추가필요함
            if (act == ActuatorAct.Pos)
            {
                interlockSensors = m_PositiveInterlockSensors;
            }
            else if(act == ActuatorAct.Neg)
            {
                interlockSensors = m_NegativeInterlockSensors;
            }

            bool interlock = false;
            if (interlockSensors != null)
            {
                int count = interlockSensors.Count;
                for (int i = 0; i < count; i++)
                {
                    interlock |= interlockSensors[i].IsDetected();

                    if (interlock) break;
                }
            }

            return interlock;
        }

        //Thread m_NoticeThread;
        public void DisplayNotice()
        {
            //if (m_NoticeThread != null)
            //{
            //    if (m_NoticeThread.IsAlive)
            //    {
            //        //이미 MessageBox가 떠 있다면
            //        m_NoticeThread.Abort();
            //    }
            //}
            //m_NoticeThread = new Thread(delegate() { Notice(); });
            //m_NoticeThread.Start();

            MessageBox.Show("Interlock condition!\nPlease check interlock sensor", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //FormMessageBox m_MessageBox;
        //protected void Notice()
        //{
        //    if (m_MessageBox != null)
        //    {
        //        m_MessageBox.Close();
        //    }

        //    m_MessageBox = new FormMessageBox(this.Name, "Interlock condition!\nPlease check interlock sensor");
        //    m_MessageBox.ShowDialog();
        //}
        #endregion
    }
}
