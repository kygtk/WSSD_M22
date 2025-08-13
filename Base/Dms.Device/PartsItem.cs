using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    public class PartsItem : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorPartsItem tagDescriptor = new TagDescriptorPartsItem();
        #endregion

        #region Fields
        //private DeviceTags m_TagContainer = null;
        private int m_MaxGlsCount = 1000;
        private int m_MaxUsedTime = 1000;
        private int m_CurGlsCount;
        private int m_CurUsedTime;
        private bool m_LifeTimeOver;   //현재값이 최대값보다 클경우 true
        [XmlIgnore()]
        public Alarm ALM_LifeTimeOver;

        private _DeviceAsm m_ConnectedDevice;
        #endregion

        #region Properties
        [Browsable(false)]
        public int MaxGlsCount
        {
            get { return m_MaxGlsCount; }
            set { m_MaxGlsCount = value; }
        }
        [Browsable(false)]
        public int MaxUsedTime
        {
            get { return m_MaxUsedTime; }
            set { m_MaxUsedTime = value; }
        }
        [Browsable(false)]
        public int CurGlsCount
        {
            get { return m_CurGlsCount; }
            set
            {
                m_CurGlsCount = value;
                //if(m_Tag != null)
                //    UpdateTag();
            }
        }
        [Browsable(false)]
        public int CurUsedTime
        {
            get { return m_CurUsedTime; }
            set
            {
                m_CurUsedTime = value;
                //if(m_Tag != null)
                //    UpdateTag();
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool LifeTimeOver
        {
            get { return m_LifeTimeOver; }
            set { m_LifeTimeOver = value; }
        }


        [Category("DMS : Setting"), Description("Device for checking usage time")]
        public _DeviceAsm ConnectedDevice
        {
            get { return m_ConnectedDevice; }
            set { m_ConnectedDevice = value; }
        }
        #endregion

        #region Constructor
        public PartsItem() : this("__")
        {
            //this.Name = "__";
        }

        public PartsItem(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Methods
        public PartsItem Clone()
        {
            PartsItem newItem = new PartsItem();
            newItem.m_Id = m_Id;
            newItem.m_CurGlsCount = m_CurGlsCount;
            newItem.m_MaxGlsCount = m_MaxGlsCount;
            newItem.m_CurUsedTime = m_CurUsedTime;
            newItem.m_MaxUsedTime = m_MaxUsedTime;
            newItem.m_LifeTimeOver = m_LifeTimeOver;
            newItem.m_Name = m_Name;
            newItem.m_Tag = m_Tag;
            newItem.ALM_LifeTimeOver = ALM_LifeTimeOver;
            return newItem;
        }

        public bool CheckCondition()
        {
            bool check = true;

            //  연결 Device가 없으면 true return
            if (m_ConnectedDevice == null) return check;

            if (m_ConnectedDevice.FamilyType == typeof(RbMotor))
            {
                check &= ((RbMotor)m_ConnectedDevice).IsTurnCw() || ((RbMotor)m_ConnectedDevice).IsTurnCcw();
            }
            if (m_ConnectedDevice.FamilyType == typeof(AutoValve))
            {
                check &= ((AutoValve)m_ConnectedDevice).IsOpen();
            }
            if (m_ConnectedDevice.FamilyType == typeof(Pump))
            {
                check &= ((Pump)m_ConnectedDevice).IsRun();
            }
            if (m_ConnectedDevice.FamilyType == typeof(EuvLamp))
            {
                check &= ((EuvLamp)m_ConnectedDevice).IsOn();
            }

            return check;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Name;
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
            m_Tag[tagDescriptor.CURGLS].Value = m_CurGlsCount.ToString();
            m_Tag[tagDescriptor.CURTIME].Value = m_CurUsedTime.ToString();
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
                ALM_LifeTimeOver = new Alarm(this.Name + " Life Time Over Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);


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
