///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.03
// Author       : Y.S.Lee
// Description  : Mfc control for On/OFF function
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class GasControl : _DeviceAsm
    {
        public enum GasControlType
        {
            InValve, MFC, OutValve
        }


        #region Tag Descriptor
        protected static TagDescriptorGasControl tagDescriptor = new TagDescriptorGasControl();
        #endregion

        #region Fields
        private AutoValveAct m_Act = AutoValveAct.Noop;
        private PMChamber_HWCVD m_Chamber;
        private GasControlType m_Type;


        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public PMChamber_HWCVD Chamber
        {
            get { return m_Chamber; }
            set { m_Chamber = value; }
        }
        [Category("DMS : Setting")]
        public GasControlType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        #endregion

        #region Constructor
        public GasControl()
        {
            this.Name = "__ Gas Control";
        }
        #endregion

        #region Methods


        public bool SetOn(AutoValveAct act)
        {
            if (this.Initialized == false) return false;

            m_Act = act;

            switch (act)
            {
                case AutoValveAct.Close:

                    break;
                case AutoValveAct.Open:

                    break;
            }

            return true;
        }

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
            //bool on = (m_Act == AutoValveAct.Open) ;

            //m_Tag.SetValue(tagDescriptor.OPEN, on);
            //m_Tag.SetValue(tagDescriptor.CLOSE, !on);

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
            //ok &= (m_AoGaugeFlowSet != null);
            //ok &= (m_Gauge != null);
            //ok &= (m_DoMfcOn != null);


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
                //m_SetupMfcFullScale = new TagSetupInfo(this.Name + " Full Scale", OptionType.None, OptionFormat.Float, UnitType.SCCM, "10");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupMfcFullScale);
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                //m_CalibrationProvider = CalibrationProvider.Instance;
                //m_Info = new TagCalibrationInfo(GaugeType.MfcFlow, this.Name);
                //m_CalibrationProvider.InitFromDB(m_Info);

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
        #endregion
    }
}
