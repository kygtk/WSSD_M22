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
    public class CylinderUnit : _DeviceAsm
    {
        #region Fields
        private _GenericCollection<Cylinder> m_Cylinders = new _GenericCollection<Cylinder>();
        public Alarm ALM_CylinderFw = null;
        public Alarm ALM_CylinderBw = null;
        #endregion

        #region Properties
        [Category("Setting")]
        public _GenericCollection<Cylinder> Cylinders
        {
            get { return m_Cylinders; }
            set { m_Cylinders = value; }
        }
        #endregion

        #region Constructor
        public CylinderUnit()
        {
            this.Name = "__ Cylinder Unit";
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

            log = string.Format("Cylinder\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        public bool IsFw()
        {
            bool fw = true;
            foreach (Cylinder cylinder in m_Cylinders)
            {
                fw &= cylinder.IsActStatus(ActuatorAct.Pos);
            }
            return fw;
        }

        public bool IsBw()
        {
            bool bw = true;
            foreach (Cylinder cylinder in m_Cylinders)
            {
                bw &= cylinder.IsActStatus(ActuatorAct.Neg);
            }
            return bw;
        }

        public void SetFw()
        {
            foreach (Cylinder cylinder in m_Cylinders)
            {
                if (!cylinder.IsActStatus(ActuatorAct.Pos)) cylinder.SetAct(ActuatorAct.Pos);
            }
        }

        public void SetBw()
        {
            foreach (Cylinder cylinder in m_Cylinders)
            {
                if (!cylinder.IsActStatus(ActuatorAct.Neg)) cylinder.SetAct(ActuatorAct.Neg);
            }
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(CylinderUnit); }
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
                ALM_CylinderFw = new Alarm(this.Name + " FW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_CylinderBw = new Alarm(this.Name + " BW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


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
