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
    public class ActuatorUnit : _DeviceAsm
    {
        #region Fields
        private _GenericCollection<_Actuator> m_Actuators = new _GenericCollection<_Actuator>();
        private ActuatorInitAct m_InitAct = ActuatorInitAct.NoAction;
        public Alarm ALM_ActuatorPositive = null;
        public Alarm ALM_ActuatorNegative = null;
        #endregion

        #region Properties
        [Category("Setting")]
        public _GenericCollection<_Actuator> Actuators
        {
            get { return m_Actuators; }
            set { m_Actuators = value; }
        }

        [Category("Setting")]
        public ActuatorInitAct InitAct
        {
            get { return m_InitAct; }
            set { m_InitAct = value; }
        }
        #endregion

        #region Constructor
        public ActuatorUnit()
        {
            m_Name = "__ Unit";
        }
        #endregion

        #region Methods
        public bool IsPositive()
        {
            bool fw = true;
            foreach (_Actuator actuator in m_Actuators)
            {
                fw &= actuator.IsActStatus(ActuatorAct.Pos);
            }
            return fw;
        }

        public bool IsNegative()
        {
            bool bw = true;
            foreach (_Actuator actuator in m_Actuators)
            {
                bw &= actuator.IsActStatus(ActuatorAct.Neg);
            }
            return bw;
        }

        public void SetPositiveAct()
        {
            foreach (_Actuator actuator in m_Actuators)
            {
                if (!actuator.IsActStatus(ActuatorAct.Pos)) actuator.SetAct(ActuatorAct.Pos);
            }
        }

        public void SetNegativeAct()
        {
            foreach (_Actuator actuator in m_Actuators)
            {
                if (!actuator.IsActStatus(ActuatorAct.Neg)) actuator.SetAct(ActuatorAct.Neg);
            }
        }

        public void SetStopAct()
        {
            foreach (_Actuator actuator in m_Actuators)
            {
                if (!actuator.IsActStatus(ActuatorAct.Stop)) actuator.SetAct(ActuatorAct.Stop);
            }
        }

        public ActuatorAct GetCurAct()
        {
            if (m_Actuators.Count != 0)
            {
                ActuatorAct act = m_Actuators[0].GetCurAct();
                int count = m_Actuators.Count;
                for (int i = 1; i < count; i++)
                {
                    if (act != m_Actuators[i].GetCurAct())
                    {
                        act = ActuatorAct.Noop;
                    }
                }
                return act;
            }
            else
            {
                return ActuatorAct.Noop;
            }
        }

        public ActuatorAct GetRefAct()
        {
            if (m_Actuators.Count != 0)
            {
                ActuatorAct act = m_Actuators[0].GetRefAct();
                int count = m_Actuators.Count;
                for (int i = 1; i < count; i++)
                {
                    if (act != m_Actuators[i].GetRefAct())
                    {
                        act = ActuatorAct.Noop;
                    }
                }
                return act;
            }
            else
            {
                return ActuatorAct.Noop;
            }
        }

        public int SetAct(ActuatorAct act)
        {
            switch (act)
            {
                case ActuatorAct.Pos:
                    SetPositiveAct();
                    break;
                case ActuatorAct.Neg:
                    SetNegativeAct();
                    break;
                case ActuatorAct.Stop:
                    SetStopAct();
                    break;
            }

            return 0;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ActuatorUnit); }
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
                ALM_ActuatorPositive = new Alarm(this.Name + " Positive Act Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ActuatorNegative = new Alarm(this.Name + " Negative Act Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


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
