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
    public class AutoValve : _Valve
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        protected AutoValve() { }
        #endregion

        #region Methods
        public bool SetAutoValveAct(AutoValveAct act)
        {
            if (this.Initialized == false) return false;

            switch (act)
            {
                case AutoValveAct.Noop:
                    break;
                case AutoValveAct.Close:
                    Close();
                    break;
                case AutoValveAct.Open:
                    Open();
                    break;
            }

            return true;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(AutoValve); }
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
            m_Tag.SetValue(tagDescriptor.RUN, IsOpen());
        }

        public override bool IsProcess()
        {
            return IsOpen();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override bool IsOpen()
        {
            throw new NotImplementedException();
        }

        public override bool IsClose()
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
    public class AutoValve_Io : AutoValve
    {
        #region Fields
        private IoDigitalOutput m_DoOpen = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoOpen
        {
            get { return m_DoOpen; }
            set { m_DoOpen = value; }
        }
        #endregion

        #region Constructor
        public AutoValve_Io()
        {
            this.Name = "__ Valve";
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void Open()
        {
            if (!this.Initialized) return;

            if (!IsOpen())
            {
                //DoOpen.SetState(true);
                DoOpen.SetState(m_TypeNormalClose);
            }
        }

        public override void Close()
        {
            if (!this.Initialized) return;

            if (!IsClose())
            {
                //DoOpen.SetState(false);
                DoOpen.SetState(!m_TypeNormalClose);
            }
        }

        public override bool IsOpen()
        {
            if (!this.Initialized) return false;

            if (m_TypeNormalClose)
                return DoOpen.GetState();
            else
                return !DoOpen.GetState();
        }

        public override bool IsClose()
        {
            if (!this.Initialized) return false;

            bool bRun = false;
            bRun |= IsOpen();

            return !bRun;
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
            ok &= (m_DoOpen != null);


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

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AutoValve_Ec : AutoValve
    {
        #region Fields
        private SlaveDigitalOutput m_DoOpen = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoOpen
        {
            get { return m_DoOpen; }
            set { m_DoOpen = value; }
        }
        #endregion

        #region Constructor
        public AutoValve_Ec()
        {
            this.Name = "__ Valve";
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void Open()
        {
            if (!this.Initialized) return;

            if (!IsOpen())
            {
                //DoOpen.SetState(true);
                DoOpen.SetState(m_TypeNormalClose);
            }
        }

        public override void Close()
        {
            if (!this.Initialized) return;

            if (!IsClose())
            {
                //DoOpen.SetState(false);
                DoOpen.SetState(!m_TypeNormalClose);
            }
        }

        public override bool IsOpen()
        {
            if (!this.Initialized) return false;

            if (m_TypeNormalClose)
                return DoOpen.GetState();
            else
                return !DoOpen.GetState();
        }

        public override bool IsClose()
        {
            if (!this.Initialized) return false;

            bool bRun = false;
            bRun |= IsOpen();

            return !bRun;
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
            ok &= (m_DoOpen != null);


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
