using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;

namespace Dms.ServerCommon
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class EquipmentType : _DeviceAsm
    {
        public enum CimProtocolType
        {
            Hsms,
            Melsec,
        }

        public enum TraceTransferType
        {
            TCPIP,
            MELSEC,
        }

        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private CimProtocolType m_CimProtocolType;
        private TraceTransferType m_TraceTransferMethod = TraceTransferType.TCPIP;
        private GlassFlowDirection m_FlowDirection;

        #endregion

        #region Properties
        [Description("Select Equipment type")]
        public CimProtocolType CimProtocol
        {
            get { return m_CimProtocolType; }
            set { m_CimProtocolType = value; }
        }
        [Description("Select Trace Data Transfer Method")]
        public TraceTransferType TraceTransferMethod
        {
            get { return m_TraceTransferMethod; }
            set { m_TraceTransferMethod = value; }
        }
        [Description("Select Glass flow direction")]
        public GlassFlowDirection GlassFlowDirection
        {
            get { return m_FlowDirection; }
            set { m_FlowDirection = value; }
        }
        #endregion

        #region Constructor
        public EquipmentType()
        {
            this.Name = "Equipment Type";
        }
        #endregion

        #region Methods
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


        public override void UpdateTag()
        {
        }

        public override void SetSubscriber()
        {
        }

        public override DmsErrors Initialize()
        {
            Initialized = true;

            return DmsErrors.Success;

            //Device의 기능이 필요없으므로 아래 초기화 순서는 생략            
            /*
                        ////////////////////////////////////////////////////////////////////////////////////////
                        // 초기화 순서는 아래의 Flow를 따라야 한다.

                        ////////////////////////////////////////////////////////////////////////////////////////
                        // 1. 이미 초기화완료 되었는지 Check
                        Initialized = true;

                        if (Initialized == true) return DmsErrors.Success;

                        //Device의 기능이 필요없으므로 아래 초기화 순서는 생략

                        ////////////////////////////////////////////////////////////////////////////////////////
                        // 2. DeviceI/O 등록
                        bool ok = true;
                        ok &= GenerateAssociatedIoDevices();


                        ////////////////////////////////////////////////////////////////////////////////////////
                        // 3. 필수 I/O 들이 등록되어 있는지 Check
                        #region Example
                        //ok &= (m_DiAlarm != null);
                        #endregion
                        //ok &= (m_DiAlive != null);
                        //ok &= (m_AiCurrentRecipeId != null);


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
             */
        }
        #endregion
    }
}
