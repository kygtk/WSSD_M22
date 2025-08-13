using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class InterfaceStep : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorInterfaceStep tagDescriptor = new TagDescriptorInterfaceStep();
        #endregion

        #region Fields
        private int m_StepNo;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public int StepNo
        {
            get { return m_StepNo; }
            set 
            { 
                m_StepNo = value; 
                //if(m_Tag != null) 
                //    UpdateTag();
            }
        }
        #endregion

        #region Constructor
        public InterfaceStep() : this("__")
        {  
        }

        public InterfaceStep(string name) 
        {
            this.Name = name;
        }
        #endregion

        #region Method
        public InterfaceStep Clone(InterfaceStep originItem)
        {
            InterfaceStep newItem = new InterfaceStep();
            newItem.Id = originItem.Id;
            newItem.Name = originItem.Name;
            newItem.StepNo = originItem.StepNo;
            return newItem;
        }

        public InterfaceStep Clone()
        {
            InterfaceStep newItem = new InterfaceStep();

            newItem.Id = this.Id;
            newItem.Name = this.Name;
            newItem.StepNo = this.StepNo;
            
            return newItem;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.Name;
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
            m_Tag.SetValue(tagDescriptor.STEP, m_StepNo);
            
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

        public override Type FamilyType
        {
            get
            {
                return typeof(InterfaceStep);
            }
        }
        #endregion
    }
}
