///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.11.11
// Author       : sangseo
// Description  : CimUnit Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
//using Dms.Cim.Common;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class CimUnit : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorCimUnit tagDescriptor = new TagDescriptorCimUnit();
        #endregion

        #region Fields
        private eqpSTATUS m_EqpState;
        private CimUnit m_Parent = null;
        private CimUnits m_CimUnits = new CimUnits();

        private int m_GroupNo = 0;
        private string m_UnitId = "UNITID";
        private bool m_UseRecipe = false;
        private bool m_UseCurrentData = false;
        private bool m_UseInitialData = false;
        private bool m_UseInitialMode = false;
        private bool m_ExistAlarmList = false;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public eqpSTATUS EqpState
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

        [Category("DMS : Relation")]
        public CimUnits CimUnits
        {
            get { return m_CimUnits; }
            set { m_CimUnits = value; }
        }
        [Category("DMS : Relation"), ReadOnly(true)]
        public CimUnit Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

        [Category("DMS : Setting"), Description("Group No for Unit")]
        public int GroupNo
        {
            get { return m_GroupNo; }
            set { m_GroupNo = value; }
        }

        [Category("DMS : Setting"), Description("Unit ID for host report")]
        public string UnitID
        {
            get { return m_UnitId; }
            set { m_UnitId = value; }
        }

        [Category("DMS : Setting"), Description("Use EQP Recipe for Unit")]
        public bool UseRecipe
        {
            get { return m_UseRecipe; }
            set { m_UseRecipe = value; }
        }
        [Category("DMS : Setting"), Description("Use Current Data for Unit")]
        public bool UseCurrentData
        {
            get { return m_UseCurrentData; }
            set { m_UseCurrentData = value; }
        }
        [Category("DMS : Setting"), Description("Use Initial Data for Unit")]
        public bool UseInitialData
        {
            get { return m_UseInitialData; }
            set { m_UseInitialData = value; }
        }
        [Category("DMS : Setting"), Description("Use Initial Mode for Unit")]
        public bool UseInitialMode
        {
            get { return m_UseInitialMode; }
            set { m_UseInitialMode = value; }
        }
        [Category("DMS : Setting"), Description("Exist Alarm List for Unit")]
        public bool ExistAlarmList
        {
            get { return m_ExistAlarmList; }
            set { m_ExistAlarmList = value; }
        }
        #endregion

        #region Constructor
        public CimUnit()
        {
            this.Name = "__";
        }
        public CimUnit(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(CimUnit); }
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
        }
        #endregion
    }
}
