///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.13
// Author       : eun
// Description  : Glass Broken Detect Scan Type Class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BrokenDetectScanType : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorBrokenScan tagDescriptor = new TagDescriptorBrokenScan();
        #endregion

        #region Fields
        public Alarm ALM_BrokenDetect = null;
        protected TagSetupSenSorInterlock m_SetupBrokenIntr = null;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupBrokenInterlock
        {
            get { return m_SetupBrokenIntr; }
            set { m_SetupBrokenIntr = value; }
        }
        #endregion

        #region Constructor
        protected BrokenDetectScanType() { }
        #endregion

        #region Methods
        public virtual void Reset(bool state)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsError()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(BrokenDetectScanType); }
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
    public class BrokenDetectScanType_Io : BrokenDetectScanType
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalInput m_DiError = new IoDigitalInput();
        private IoDigitalOutput m_DoReset = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiError
        {
            get { return m_DiError; }
            set { m_DiError = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoReset
        {
            get { return m_DoReset; }
            set { m_DoReset = value; }
        }
        #endregion

        #region Constructor
        public BrokenDetectScanType_Io()
        {
            this.Name = "__ Unit Broken Detect ScanType";
        }
        #endregion

        #region Methods
        public override void Reset(bool state)
        {
            m_DoReset.SetState(state);
        }
        public override bool IsError()
        {
            return m_DiError.GetState();
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
            //ok &= (m_DiStart != null);
            //ok &= (m_DiEnd != null);
            ok &= (m_DiError != null);
            ok &= (m_DoReset != null);


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
                ALM_BrokenDetect = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupBrokenIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.Broken);
                SetupSensorInterlockProvider.Instance.InitFromDB(this.m_SetupBrokenIntr);


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ERROR, DiError.GetState());
            m_Tag.SetValue(tagDescriptor.RESET, DoReset.GetState());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BrokenDetectScanType_Ec : BrokenDetectScanType
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalInput m_DiError = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoReset = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public SlaveDigitalInput DiError
        {
            get { return m_DiError; }
            set { m_DiError = value; }
        }
        [Category("DMS : Setting")]
        public SlaveDigitalOutput DoReset
        {
            get { return m_DoReset; }
            set { m_DoReset = value; }
        }
        #endregion

        #region Constructor
        public BrokenDetectScanType_Ec()
        {
            this.Name = "__ Unit Broken Detect ScanType";
        }
        #endregion

        #region Methods
        public override void Reset(bool state)
        {
            m_DoReset.SetState(state);
        }
        public override bool IsError()
        {
            return m_DiError.GetState();
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
            //ok &= (m_DiStart != null);
            //ok &= (m_DiEnd != null);
            ok &= (m_DiError != null);
            ok &= (m_DoReset != null);


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
                ALM_BrokenDetect = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupBrokenIntr = new TagSetupSenSorInterlock(this.Name, false, SensorInterlockType.Broken);
                SetupSensorInterlockProvider.Instance.InitFromDB(this.m_SetupBrokenIntr);


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ERROR, DiError.GetState());
            m_Tag.SetValue(tagDescriptor.RESET, DoReset.GetState());
        }
        #endregion
    }
}
