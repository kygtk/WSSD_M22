///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.07.16
// Author       : EUN
// Description  : RPS(Remote Plasma System) - for cleaning chamber. (mks)
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RPS : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorRPS tagDescriptor = new TagDescriptorRPS();
        #endregion

        #region Fields
        private IoDigitalInput m_DiReady;
        private IoDigitalInput m_DiPlasmaOk;
        private IoDigitalInput m_DiAcOk;
        private IoDigitalOutput m_DoPlasmaOn;

        private TagSetupInfo m_SetupPlasmaOnWaitTime;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiReady
        {
            get { return m_DiReady; }
            set { m_DiReady = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiPlasmaOk
        {
            get { return m_DiPlasmaOk; }
            set { m_DiPlasmaOk = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiAcOk
        {
            get { return m_DiAcOk; }
            set { m_DiAcOk = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoPlasmaOn
        {
            get { return m_DoPlasmaOn; }
            set { m_DoPlasmaOn = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupPlasmaOnWaitTime
        {
            get { return m_SetupPlasmaOnWaitTime; }
            set { m_SetupPlasmaOnWaitTime = value; }
        }
        #endregion

        #region Constructor
        public RPS()
        {
            this.Name = "__ RPS";
        }
        #endregion

        #region Methods
        public bool IsPlasmaOn()
        {
            return (!this.Initialized) ? false : m_DoPlasmaOn.GetState();
        }

        public void PlasmaOn()
        {
            if (!this.Initialized) return;

            if (IsPlasmaOn() == false)
            {
                m_DoPlasmaOn.SetState(true);
                if (Simul.Device) m_DiPlasmaOk.SetState(true);
            }
        }

        public bool IsReady()
        {
            return (!this.Initialized) ? false : m_DiReady.GetState();
        }

        public bool IsAcOk()
        {
            return (!this.Initialized) ? false : m_DiAcOk.GetState();
        }

        public bool IsPlasmaOk()
        {
            return (!this.Initialized) ? false : m_DiPlasmaOk.GetState();
        }
        #endregion

        #region Override
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
            ok &= (m_DiReady != null);
            ok &= (m_DiPlasmaOk != null);
            ok &= (m_DiAcOk != null);
            ok &= (m_DoPlasmaOn != null);


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
                m_SetupPlasmaOnWaitTime = new TagSetupInfo(this.Name + " Plasma On Wait Time", OptionType.None, OptionFormat.Binary, UnitType.sec, "40");
                SetupGenInfoProvider.Instance.InitFromDB(m_SetupPlasmaOnWaitTime);


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
                if (m_Simul.Device)
                {
                    m_DiReady.SetState(true);
                    m_DiAcOk.SetState(true);
                }


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.READY, IsReady());
            m_Tag.SetValue(tagDescriptor.PLASMAOK, IsPlasmaOk());
            m_Tag.SetValue(tagDescriptor.ACOK, IsAcOk());
            m_Tag.SetValue(tagDescriptor.PLASMAON, IsPlasmaOn());
        }
        #endregion
    }
}
