///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.15
// Author       : jemoon
// Description  : EqpUnit Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class EqpUnit : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorEqpUnit tagDescriptor = new TagDescriptorEqpUnit();
        #endregion

        #region Fields
        private EqpState m_EqpState;
        private ProcessState m_ProcessState;
        //private EqpState m_EqpOldState = EqpState.UnKnown;
        //private ProcessState m_ProcessOldState = ProcessState.UnKnown;
        private EqpUnit m_Parent = null;
        private EqpUnits m_EqpUnits = new EqpUnits();
        private string m_version = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public EqpState EqpState
        {
            get { return m_EqpState; }
            set 
            { 
                m_EqpState = value;
                //if (m_EqpOldState != m_EqpState)
                //{
                //    UpdateTag();
                //    m_EqpOldState = m_EqpState;
                //}
            }
        }
        [Browsable(false), XmlIgnore()]
        public ProcessState ProcessState
        {
            get { return m_ProcessState; }
            set
            {
                m_ProcessState = value;
                //if (m_ProcessOldState != m_ProcessState)
                //{
                //    UpdateTag();
                //    m_ProcessOldState = m_ProcessState;
                //}
            }
        }
        [Category("DMS : Relation")]
        public EqpUnits EqpUnits
        {
            get { return m_EqpUnits; }
            set { m_EqpUnits = value; }
        }
        [Category("DMS : Relation")]
        public string version
        {
            get { return m_version; }
            set { m_version = value; }
        }
        [Category("DMS : Relation"), ReadOnly(true)]
        public EqpUnit Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
        #endregion

        #region Constructor
        public EqpUnit()
        {
            this.Name = "__";
        }
        public EqpUnit(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(EqpUnit); }
        }

        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.


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
            m_Tag.SetValue(tagDescriptor.EQP_STATE, m_EqpState);
            m_Tag.SetValue(tagDescriptor.PROCESS_STATE, m_ProcessState);
        } 
        #endregion
    }
}
