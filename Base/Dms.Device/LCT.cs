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
    public class LCT : _DevicePeer
    {
        #region Tag Descriptor
        TagDescriptorLCT tagDescriptor = new TagDescriptorLCT();
        #endregion

        #region Fields
        private Gauge_Ap m_GaugeLevel1;
        private Gauge_Ap m_GaugeLevel2;
        private Gauge_Ap m_GaugeConsistence;
        #endregion

        #region Properties
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugeLevel1
        {
            get { return m_GaugeLevel1; }
            set
            {
                m_GaugeLevel1 = value;
                if (m_GaugeLevel1 != null)
                {
                    m_GaugeLevel1.Peer = this;
                }
            }
        }
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugeLevel2
        {
            get { return m_GaugeLevel2; }
            set
            {
                m_GaugeLevel2 = value;
                if (m_GaugeLevel2 != null)
                {
                    m_GaugeLevel2.Peer = this;
                }
            }
        }
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugeConsistence
        {
            get { return m_GaugeConsistence; }
            set
            {
                m_GaugeConsistence = value;
                if (m_GaugeConsistence != null)
                {
                    m_GaugeConsistence.Peer = this;
                }
            }
        }
        #endregion

        #region Constructor
        public LCT()
        {
            m_Name = "__ LCT";
            m_PeerType = PeerType.LCT;
        }
        #endregion

        #region Methods
        public ushort GetLevel1Value()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.LCT_GetLevel1Value();
        }
        public ushort GetLevel2Value()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.LCT_GetLevel2Value();
        }

        public ushort GetConsistenceValue()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.LCT_GetConsistenceValue();
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
            m_Tag.SetValue(tagDescriptor.PV_LEVEL1, GetLevel1Value());
            m_Tag.SetValue(tagDescriptor.PV_LEVEL2, GetLevel2Value());
            m_Tag.SetValue(tagDescriptor.PV_CONSISTENCE, GetConsistenceValue());
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
                m_SlaveAP.SetPeer(PeerType.CDA);


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
                    //m_DiSensor.SetState(false);
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
        #endregion
    }
}
