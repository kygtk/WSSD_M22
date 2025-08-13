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
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class CstMappingUnitTakex : _CstMappingUnit
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private IoCollection<IoDigitalInput> m_DiMappingSensors = new IoCollection<IoDigitalInput>();
        private ActuatorUnit m_MappingDriveUnit;
        private SeqMapTakexMapping m_SeqMapping;
        private SeqMapTakexHoming m_SeqHoming;
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoCollection<IoDigitalInput> DiMapResultInputs
        {
            get { return m_DiMappingSensors; }
            set { m_DiMappingSensors = value; }
        }
        [Category("DMS : Relation")]
        public ActuatorUnit MappingDriveUnit
        {
            get { return m_MappingDriveUnit; }
            set { m_MappingDriveUnit = value; }
        }
        #endregion

        #region Constructor
        public CstMappingUnitTakex()
        {
            this.Name = "Port__ Mapping Unit";
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
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
            //m_MappingSensors.MaxSimulateCount = m_MaxSlotCount;

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
                    if (m_ControlMode == MuxMode.Single) m_MultiDropCount = 1;
                    
                    m_PortIds = new int[m_MultiDropCount];
                    m_MappingStatus = new MappingStatus[m_MultiDropCount][];
                    for (int i = 0; i < m_MultiDropCount; i++)
                    {
                        m_PortIds[i] = m_StartPortId + i;
                        m_MappingStatus[i] = new MappingStatus[m_MaxSlotCount];
                    }
                    m_UnitStatus = new int[m_MultiDropCount];
                    m_SeqMapping = new SeqMapTakexMapping(this);
                    m_SeqHoming = new SeqMapTakexHoming(this);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            } 
        }

        public override int MakeGlassMappingStatus(int portId)
        {
            if (m_Simul.Device)
            {
                MakeSimulMappingStatus(portId);
            }
            else
            {
                int id = FindChannelId(portId);

                //test
                bool[] states = m_DiMappingSensors.GetDiStates();

                for (int i = 0; i < m_MaxSlotCount; i++)
                {
                    //m_MappingStatus[id][i] = m_MappingSensors[i].GetState() ? MappingStatus.On : MappingStatus.Off;
                    m_MappingStatus[id][i] = m_DiMappingSensors[i].GetState() ? MappingStatus.On : MappingStatus.Off;
                }
            }

            return 0;
        }

        public override int MakeGlassMappingStatus(int portId, int slotId)
        {
            if (m_Simul.Device)
            {
                MakeSimulMappingStatus(portId, slotId);
            }
            else
            {
                int id = FindChannelId(portId);
                m_MappingStatus[id][slotId] = m_DiMappingSensors[slotId].GetState() ? MappingStatus.On : MappingStatus.Off;
            }

            return 0;
        }

        public override int Homing(int portId)
        {
            return m_SeqHoming.Do();
        }

        public override int Mapping(int portId)
        {
            return m_SeqMapping.Do();
        }

        public override int Mapping(int portId, int slotId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int CheckUnitStatus(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsMappingDriveUnitFw()
        {
            if (MappingDriveUnit.IsPositive() == true &&
                MappingDriveUnit.IsNegative() == false)
                return true;
            else return false;
        }

        public override bool IsMappingDriveUnitBw()
        {
            if (MappingDriveUnit.IsPositive() == false &&
                MappingDriveUnit.IsNegative() == true)
                return true;
            else return false;
        }
        #endregion
    }
    
    public class SeqMapTakexMapping : XSeqFunction
    {
        #region Fields
        private CstMappingUnitTakex m_MappingUnit;
        private XTimer m_TimerMapping = null;
        private int m_TimeoutMapping = 5 * 1000;
        private int m_DelayForMapping = 2 * 1000;
        #endregion

        #region Constructor
        public SeqMapTakexMapping(CstMappingUnitTakex mapUnit)
        {
            m_MappingUnit = mapUnit;
            m_TimerMapping = new XTimer(m_MappingUnit.Name + "Mapping Timer");
            m_TimeoutMapping = m_MappingUnit.TimeoutMapping;
            if(m_MappingUnit.DelayForMapping < m_TimeoutMapping)
                m_DelayForMapping = m_MappingUnit.DelayForMapping;
            else
                m_DelayForMapping = m_TimeoutMapping;
        }
        #endregion

        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    {
                        m_MappingUnit.MappingDriveUnit.SetPositiveAct();
                        m_TimerMapping.Start(m_TimeoutMapping);
                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (m_MappingUnit.MappingDriveUnit.IsPositive())
                    {
	                    m_TimerMapping.Start(m_DelayForMapping);
                        seqNo = 20;
                    }
                    else if (m_TimerMapping.Over)
                    {
                        returnValue = 1;
                        seqNo = 0;
                    }
                    break;
                case 20:
                    if (m_TimerMapping.Over)
                    {
                        m_MappingUnit.MakeGlassMappingStatus(0);                        
                        m_MappingUnit.MappingDriveUnit.SetNegativeAct();
                        m_TimerMapping.Start(m_TimeoutMapping);
                        seqNo = 30;
                    }
                    break;
                case 30:
                    if (m_MappingUnit.MappingDriveUnit.IsNegative())
                    {
                        returnValue = 0;
                        seqNo = 0;
                    }
                    else if (m_TimerMapping.Over)
                    {
                        returnValue = 1;
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
    }

    public class SeqMapTakexHoming : XSeqFunction
    {
        #region Fields
        private CstMappingUnitTakex m_MappingUnit;
        private XTimer m_TimerHoming = null;
        private int m_TimeoutHoming = 5 * 1000;
        #endregion

        #region Constructor
        public SeqMapTakexHoming(CstMappingUnitTakex mapUnit)
        {
            m_MappingUnit = mapUnit;
            m_TimerHoming = new XTimer(m_MappingUnit.Name + "Homing Timer");
            m_TimeoutHoming = m_MappingUnit.TimeoutHoming;
        }
        #endregion

        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    {
                        m_MappingUnit.MappingDriveUnit.SetNegativeAct();
                        m_TimerHoming.Start(m_TimeoutHoming);
                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (m_MappingUnit.MappingDriveUnit.IsNegative())
                    {
                        returnValue = 0;
                        seqNo = 0;
                    }
                    else if (m_TimerHoming.Over)
                    {
                        returnValue = 1;
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
    }
}
