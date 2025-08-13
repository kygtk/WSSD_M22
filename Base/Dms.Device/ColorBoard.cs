///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.10.22
// Author       : eun
// Description  : Color Board for PSM's AP Plasma
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
    public class ColorBoard : _DeviceAsm
    {
        #region Fields
        private IoDigitalOutput m_DoOn = new IoDigitalOutput();
        private Gauge m_Red;
        private Gauge m_Green;
        private Gauge m_Blue;
        private bool m_IsAlarm = false;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalOutput DoPowerOn
        {
            get { return m_DoOn; }
            set { m_DoOn = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugeRed
        {
            get { return m_Red; }
            set { m_Red = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugeGreen
        {
            get { return m_Green; }
            set { m_Green = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugeBlue
        {
            get { return m_Blue; }
            set { m_Blue = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get
            {
                m_IsAlarm = m_Red.IsAlarm || m_Green.IsAlarm || m_Blue.IsAlarm;
                return m_IsAlarm;
            }
        }
        #endregion

        #region Constructor
        public ColorBoard()
        {
            this.Name = "__ Color Board";
        }
        #endregion

        #region Methods
        public void On()
        {
            m_DoOn.SetState(true);
        }

        public void Off()
        {
            m_DoOn.SetState(false);
        }

        public bool IsOn()
        {
            return m_DoOn.GetState();
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
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion
            ok &= (m_DoOn != null);
            ok &= (m_Red != null);
            ok &= (m_Green != null);
            ok &= (m_Blue != null);


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
