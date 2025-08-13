///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Pump Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Data;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RotaryPump : _Pump
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();

        public Alarm ALM_RotaryPumpAlarm;
        private TagSetupInfo m_SetupPumpUse;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupPumpUse
        {
            get { return m_SetupPumpUse; }
            set { m_SetupPumpUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsUse
        {
            get
            {
                if (m_SetupPumpUse == null) return false;
                return m_SetupPumpUse.GetValue<bool>();
            }
        }
        #endregion

        #region Constructor
        public RotaryPump()
        {
            this.Name = "__ RotaryPump";
        }
        #endregion

        #region Methods
        public bool SetPumpAct(PumpAct act)
        {
            if (!this.Initialized) return false;

            switch (act)
            {
                case PumpAct.Noop:
                    break;
                case PumpAct.Stop:
                    Stop();
                    break;
                case PumpAct.Run:
                    Run();
                    break;
            }

            return true;
        }
        #endregion

        #region Override
        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            return DiAlarm.GetState();
        }

        public override bool IsRun()
        {
            if (!this.Initialized) return false;

            return DoRun.GetState();
        }

        public override bool IsStop()
        {
            if (!this.Initialized) return false;

            bool bRun = false;

            bRun |= IsRun();

            return !bRun;
        }

        public override void Stop()
        {
            if (!this.Initialized) return;

            if (true == IsRun())
            {
                DoRun.SetState(false);
            }
        }

        public override void Run()
        {
            if (!this.Initialized) return;

            if (!IsRun())
            {
                DoRun.SetState(true);
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
            ok &= (DiAlarm != null);
            ok &= (DoRun != null);


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
                if (null != this.DiAlarm)
                {
                    ALM_RotaryPumpAlarm = new Alarm(this.Name + " Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                if (this.Name != "Rotary Pump")
                {
                    m_SetupPumpUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupPumpUse);
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
                if (m_Simul.Device)
                {
                    DiAlarm.SetState(false);
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
            m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            m_Tag.SetValue(tagDescriptor.RUN, DoRun.GetState());
            m_Tag.SetValue(tagDescriptor.STOP, !DoRun.GetState());
        }

        public override Boolean IsProcess()
        {
            return IsRun();
        }
        #endregion
    }
}
