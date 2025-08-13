using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PowerJoint : _DeviceAsm
    {
        #region Tag Descriptor
        //protected static TagDescriptorPowerJoint tagDescriptor = new TagDescriptorPowerJoint();
        #endregion

        #region Fields
        private _GenericCollection<Cylinder> m_JointCylinder = new _GenericCollection<Cylinder>();
        private _GenericCollection<Cylinder> m_JointLock = new _GenericCollection<Cylinder>();
        private IoDigitalInput m_JointConfirm = null;
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public IoDigitalInput JointConfirm
        {
            get { return m_JointConfirm; }
            set { m_JointConfirm = value; }
        }
        [Category("DMS : Relation")]
        public _GenericCollection<Cylinder> JointCylinder
        {
            get { return m_JointCylinder; }
            set { m_JointCylinder = value; }
        }
        [Category("DMS : Relation")]
        public _GenericCollection<Cylinder> JointLock
        {
            get { return m_JointLock; }
            set { m_JointLock = value; }
        }
        #endregion

        #region Constructor
        public PowerJoint()
        {
            this.Name = "Tr __ Joint";
        }
        #endregion

        #region Methods
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

            log = string.Format("PowerJoint\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        //Joint Up/Down
        public bool IsUp()
        {
            bool Up = true;
            foreach (Cylinder JointCylinder in m_JointCylinder)
            {
                Up &= JointCylinder.IsActStatus(ActuatorAct.Pos);
            }
            return Up;
        }
        public bool IsDown()
        {
            bool Down = true;
            foreach (Cylinder JointCylinder in m_JointCylinder)
            {
                Down &= JointCylinder.IsActStatus(ActuatorAct.Neg);
            }
            return Down;
        }
        public void SetUp()
        {
            foreach (Cylinder JointCylinder in m_JointCylinder)
            {
                if (!JointCylinder.IsActStatus(ActuatorAct.Pos))
                {
                    JointCylinder.SetAct(ActuatorAct.Pos);
                }
            }
        }
        public void SetDown()
        {
            foreach (Cylinder JointCylinder in m_JointCylinder)
            {
                if (!JointCylinder.IsActStatus(ActuatorAct.Neg))
                {
                    JointCylinder.SetAct(ActuatorAct.Neg);
                }
            }
        }
        //Joint Lock/UnLock
        public bool IsLock()
        {
            bool Lock = true;
            foreach (Cylinder JointLock in m_JointLock)
            {
                Lock &= JointLock.IsActStatus(ActuatorAct.Pos);
            }
            return Lock;
        }
        public bool IsUnLock()
        {
            bool UnLock = true;
            foreach (Cylinder JointLock in m_JointLock)
            {
                UnLock &= JointLock.IsActStatus(ActuatorAct.Neg);
            }
            return UnLock;
        }
        public void SetLock()
        {
            foreach (Cylinder JointLock in m_JointLock)
            {
                if (!JointLock.IsActStatus(ActuatorAct.Pos))
                {
                    JointLock.SetAct(ActuatorAct.Pos);
                }
            }
        }
        public void SetUnLock()
        {
            foreach (Cylinder JointLock in m_JointLock)
            {
                if (!JointLock.IsActStatus(ActuatorAct.Neg))
                {
                    JointLock.SetAct(ActuatorAct.Neg);
                }
            }
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(PowerJoint); }
        }

        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        }
        #endregion
    }
}
