///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.28
// Author       : jemoon
// Description  : CstMappingUnit class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class CstMappingUnit : _DeviceAsm
    {
        #region Fields
        private int m_MaxSlotCount = 25; 
        private IoDigitalInput m_DiMapReverseDetect = new IoDigitalInput();
        private IoDigitalInput m_DiMapRotationDetect = new IoDigitalInput();
        private IoCollection<IoDigitalInput> m_MapResultInputs = new IoCollection<IoDigitalInput>();
        private ActuatorUnit m_MappingDriveUnit;
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiMapReverseDetect
        {
            get { return m_DiMapReverseDetect; }
            set { m_DiMapReverseDetect = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiMapRotationDetect
        {
            get { return m_DiMapRotationDetect; }
            set { m_DiMapRotationDetect = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoCollection<IoDigitalInput> DiMapResultInputs
        {
            get { return m_MapResultInputs; }
            set { m_MapResultInputs = value; }
        }
        [Category("DMS : Relation")]
        public ActuatorUnit MappingDriveUnit
        {
            get { return m_MappingDriveUnit; }
            set { m_MappingDriveUnit = value; }
        }
        [Category("DMS : Option")]
        public int MaxSlotCount
        {
            get { return m_MaxSlotCount; }
            set { m_MaxSlotCount = value; }
        }
        #endregion

        #region Constructor
        public CstMappingUnit()
        {
            this.Name = "Port __ Map Unit";
        }
        #endregion

        #region Methods
        protected void CreateSimulatedIoCollection()
        {
            if (m_Simul.IoMapping)
            {
                m_MapResultInputs.Clear();
                for (int i = 0; i < m_MaxSlotCount; i++)
                { 
                    IoDigitalInput io = new IoDigitalInput();
                    io.Name = "di" + XFunc.FilterigName(this.Name)+ "MapResult" + string.Format("{0}", (i+1));
                    m_MapResultInputs.Add(io);
                }
            }
        }

        public MappingStatus[] GetMappingSensorState()
        {
            MappingStatus[] mappingSensors = new MappingStatus[m_MaxSlotCount];
            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                if (DiMapResultInputs[i].GetState())
                    mappingSensors[i] = MappingStatus.On;
                else
                    mappingSensors[i] = MappingStatus.Off;
            }
            return mappingSensors;
        }
        #endregion

        #region Override
        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
        {
            
        }

        public override void UpdateTag()
        {
            
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

            CreateSimulatedIoCollection();

            bool ok = true;
            ok &= GenerateAssociatedIoDevices();


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

        public override Type FamilyType
        {
            get
            {
                return this.GetType();
            }
        }
        #endregion
    }
}
