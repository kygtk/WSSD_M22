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
    public class PowerJointAll : PowerJoint
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        //2009.11.25 kimgun joint의 갯수에 따라 아래 아이오도 같이 추가 되므로 joint에 해댱 아이오 필요하다.
        private Cylinder m_AlignCylinder1 = null;
        private Cylinder m_AlignCylinder2 = null;
        private Cylinder m_TurnCylinder = null;
        private Cylinder m_TurnStopCylinder = null;
        private BrokenDetectFixedType m_BrokenSensor = null;
        // private GlsSensor m_GlassExistSensor = null;
        private Sensor m_GlassExistSensor = null;
        ////////////////////////////////////////////////////////////////////////////////////////////////////// 
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public BrokenDetectFixedType FixedBrokenSensor
        {
            get { return m_BrokenSensor; }
            set { m_BrokenSensor = value; }
        }
        [Category("DMS : Relation")]
        public Sensor GlassExistSensor
        {
            get { return m_GlassExistSensor; }
            set { m_GlassExistSensor = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder Align1
        {
            get { return m_AlignCylinder1; }
            set { m_AlignCylinder1 = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder Align2
        {
            get { return m_AlignCylinder2; }
            set { m_AlignCylinder2 = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder TurnCylinder
        {
            get { return m_TurnCylinder; }
            set { m_TurnCylinder = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder TurnStopCylinder
        {
            get { return m_TurnStopCylinder; }
            set { m_TurnStopCylinder = value; }
        }
        #endregion

        #region Constructor
        public PowerJointAll()
        {
            this.Name = "Tr __ Joint";
        }
        #endregion

        #region Methods

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
