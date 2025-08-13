///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : AutoValve Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class HmiAutoValve : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private IoDigitalOutput m_DoOpenSwitch = null;
        private IoDigitalOutput m_DoCloseSwitch = null;
        private IoDigitalInput m_DiOpenStatus = null;
        private IoAnalogOutput m_AoSelectedValveNo = null;
        private ushort m_ValveNo = 1;
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoOpenSwitch
        {
            get { return m_DoOpenSwitch; }
            set { m_DoOpenSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoCloseSwitch
        {
            get { return m_DoCloseSwitch; }
            set { m_DoCloseSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiOpenStatus
        {
            get { return m_DiOpenStatus; }
            set { m_DiOpenStatus = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoAnalogOutput AoSelectedValveNo
        {
            get { return m_AoSelectedValveNo; }
            set { m_AoSelectedValveNo = value; }
        }
        [Category("DMS : Option")]
        public ushort ValveNo
        {
            get { return m_ValveNo; }
            set { m_ValveNo = value; }
        }
        #endregion

        #region Constructor
        public HmiAutoValve()
        {
            this.Name = "__ Valve";
        }
        #endregion

        #region Methods
        public void Open()
        {
            if (!this.Initialized) return;

            m_AoSelectedValveNo.SetState(m_ValveNo);
            m_DoOpenSwitch.SetState(true);
            m_DoCloseSwitch.SetState(true);
        }

        public void Close()
        {
            if (!this.Initialized) return;
            m_DoCloseSwitch.SetState(true);
            m_DoOpenSwitch.SetState(true);
        }

        public bool IsOpen()
        {
            if (!this.Initialized) return false;

            return m_DiOpenStatus.GetState();
        }
        #endregion

        #region Override
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
            ok &= (m_DoOpenSwitch != null);
            ok &= (m_DoCloseSwitch != null);
            ok &= (m_DiOpenStatus != null);
            ok &= (m_AoSelectedValveNo != null);


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
        }
        #endregion
    }
}
