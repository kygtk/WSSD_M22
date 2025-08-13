///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2025.06.11
// Author       : byeongmin
// Description  : ESD Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using System.Collections;

namespace Dms.Device
{
    public class ESD : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorESD tagDescriptor = new TagDescriptorESD();
        #endregion

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        protected ESD() { }
        #endregion

        #region Methods
        public virtual bool IsHigh()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsLo()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsGo()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsIonAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual short GetSurfacePotential()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(ESD); }
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
            throw new NotImplementedException();
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ESD_Ec : ESD
    {
        #region Fields
        private SlaveDigitalInput m_DiHigh = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiLo = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiGo = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiIonAlarm = new SlaveDigitalInput();
        private SlaveAnalogInput m_AiSurfacePotential = new SlaveAnalogInput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiHigh
        {
            get { return m_DiHigh; }
            set { m_DiHigh = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiLo
        {
            get { return m_DiLo; }
            set { m_DiLo = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiGo
        {
            get { return m_DiGo; }
            set { m_DiGo = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiIonAlarm
        {
            get { return m_DiIonAlarm; }
            set { m_DiIonAlarm = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveAnalogInput AiSurfacePotential
        {
            get { return m_AiSurfacePotential; }
            set { m_AiSurfacePotential = value; }
        }
        #endregion

        #region Constructor
        public ESD_Ec()
        {
            m_Name = "__ ESD";
        }
        #endregion

        #region Methods
        public override bool IsHigh()
        {
            if (!m_Initialized) return false;

            return m_DiHigh.GetState();
        }
        public override bool IsLo()
        {
            if (!m_Initialized) return false;

            return m_DiLo.GetState();
        }
        public override bool IsGo()
        {
            if (!m_Initialized) return false;

            return m_DiGo.GetState();
        }
        public override bool IsIonAlarm()
        {
            if (!m_Initialized) return false;

            return m_DiIonAlarm.GetState();
        }
        public override short GetSurfacePotential()
        {
            if (!m_Initialized) return 0;

            return m_AiSurfacePotential.GetState();
        }
        #endregion

        #region Overrides
        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.HIGH, IsHigh());
            m_Tag.SetValue(tagDescriptor.LO, IsLo());
            m_Tag.SetValue(tagDescriptor.GO, IsGo());
            m_Tag.SetValue(tagDescriptor.ION_ALM, IsIonAlarm());
            m_Tag.SetValue(tagDescriptor.SURFACE_POTENTIAL, GetSurfacePotential());
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
            ok &= m_DiHigh!=null;
            ok &= m_DiLo!=null;
            ok &= m_DiGo!=null;
            ok &= m_DiIonAlarm!=null;
            ok &= m_AiSurfacePotential!=null;


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
        #endregion
    }
}
