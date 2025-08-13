using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Data;
using System.Xml.Serialization;
using Dms.Util.IODefine;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class LFC : _DevicePeer
    {
        #region Tag Descriptor
        TagDescriptorLFC tagDescriptor = new TagDescriptorLFC();
        #endregion

        #region Fields
        private Gauge_Ap m_GaugeFlow;
        private Gauge_Ap m_GaugePress;
        private Gauge_Ap m_GaugeOpenRate;

        private ushort m_SV_OpenRate = 0;
        #endregion

        #region Properties
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugeFlow
        {
            get { return m_GaugeFlow; }
            set
            {
                m_GaugeFlow = value;
                if (m_GaugeFlow != null)
                {
                    m_GaugeFlow.Peer = this;
                }
            }
        }
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugePress
        {
            get { return m_GaugePress; }
            set
            {
                m_GaugePress = value;
                if (m_GaugePress != null)
                {
                    m_GaugePress.Peer = this;
                }
            }
        }
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugeOpenRate
        {
            get { return m_GaugeOpenRate; }
            set
            {
                m_GaugeOpenRate = value;
                if (m_GaugeOpenRate != null)
                {
                    m_GaugeOpenRate.Peer = this;
                }
            }
        }
        #endregion

        #region Constructor
        public LFC()
        {
            m_Name = "__ LFC";
            m_PeerType = PeerType.LFC;
        }
        #endregion

        #region Methods
        public ushort GetFlowValue()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.LFC_GetFlowValue();
        }

        public ushort GetPressValue()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.LFC_GetPressValue();
        }

        public ushort GetCurrentOpenRate()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.LFC_GetCurrentOpenRate();
        }

        public void SetTargetOpenRate(ushort value)
        {
            if (!m_Initialized) return;

            if (m_SlaveAP.LFC_SetTargetOpenRate(value))
                m_SV_OpenRate = value;
        }
        #endregion

        #region Overrides
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
            m_Tag.SetValue(tagDescriptor.PAIRING, IsPaired());

            m_Tag.SetValue(tagDescriptor.PV_FLOW, GetFlowValue());
            m_Tag.SetValue(tagDescriptor.PV_PRESS, GetPressValue());
            m_Tag.SetValue(tagDescriptor.PV_CUR_OPENRATE, GetCurrentOpenRate());

            m_Tag.SetValue(tagDescriptor.SV_TGT_OPENRATE, m_SV_OpenRate);
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
            ok &= m_SlaveAP != null;


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
                m_SlaveAP.SetAdvController();
                m_SlaveAP.SetPeer(PeerType.LFC);


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
