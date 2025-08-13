using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections;

namespace Dms.Device
{   
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class SRU : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorSafetyRelayUnit tagDescriptor = new TagDescriptorSafetyRelayUnit();
        #endregion

        #region Fields
        private IoDigitalInput m_SruAct = new IoDigitalInput();
        private IoDigitalOutput m_SruByPass = new IoDigitalOutput();
        private bool m_ByPassUse = false;
        private TagSetupInfo m_SetupByPass = null;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        [Description("On이되면 Hardware Interlock이 동작)")]
        public IoDigitalInput SruAct
        {
            get { return m_SruAct; }
            set { m_SruAct = value; }
        }
        [Category("DMS : Setting")]
        [Description("On이되면 Hardware Interlock 무시)")]
        public IoDigitalOutput SruByPass
        {
            get { return m_SruByPass; }
            set { m_SruByPass = value; }
        }
        [Category("DMS : Setting")]
        [Description("Use/NoUse option 사용 : option에 따라 ByPass사용 미사용)")]
        public bool ByPassUse
        {
            get { return m_ByPassUse; }
            set { m_ByPassUse = value; }
        }
        #endregion
        
        #region Constructor
        public SRU()
        {
            this.Name = "__ SRU";
        }
        #endregion

        #region Methods
        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("SRU\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        private void ByPass(bool bOn)
        {
            m_SruByPass.SetState(bOn);
        }
        #endregion
        
        #region Override
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
            ok &= (m_SruAct != null);
            if(ByPassUse)
                ok &= (m_SruByPass != null);
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
                if (ByPassUse)
                {
                    SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                    m_SetupByPass = new TagSetupInfo(this.Name + " Bypass", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.NoUse.ToString());
                    setupGenInfoProvider.InitFromDB(this.m_SetupByPass);
                }

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
             if (m_Tag != null)
            {
                m_Tag.SetValue(tagDescriptor.SRUACT, this.SruAct);
                if (ByPassUse)
                {
                    ByPass(m_SetupByPass.GetValue<bool>());
                 //   m_Tag.SetValue(tagDescriptor.BYPASS, ByPass(m_SetupByPass.GetValue<bool>()));
                }
            }
        }
        #endregion       
    }
}
