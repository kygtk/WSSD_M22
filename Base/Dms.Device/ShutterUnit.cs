using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ShutterUnit : _DeviceAsm
    {
        #region Fields
        protected static TagDescriptorShutter tagDescriptor = new TagDescriptorShutter();

        //private bool m_SingleAct = false;

        private Cylinder m_ShutterCylinder;

        private SeqShutterOpen m_SeqShutterOpen;
        private SeqShutterClose m_SeqShutterClose;
        private ThreadShutterManualAct m_ThreadShutterManualAct;
        #endregion

        #region Properties
        [Category("Setting")]
        public Cylinder ShutterCylinder
        {
            get { return m_ShutterCylinder; }
            set { m_ShutterCylinder = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int ManualActionCmd
        {
            get
            {
                if (m_ThreadShutterManualAct == null) return 0;
                else
                {
                    return m_ThreadShutterManualAct.ManualActionCmd;
                }
            }
            set
            {
                if (m_ThreadShutterManualAct == null) return;
                else
                {
                    m_ThreadShutterManualAct.ManualActionCmd = value;
                }
            }
        }
        #endregion

        #region Constructor
        public ShutterUnit()
        {
            this.Name = "__ Shutter Unit";
        }
        #endregion

        #region Methods
        public void InitSequence()
        {
            m_ThreadShutterManualAct = new ThreadShutterManualAct(10, this);

            m_SeqShutterOpen = new SeqShutterOpen(this);
            m_SeqShutterClose = new SeqShutterClose(this);
        }

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

            log = string.Format("Cylinder\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        public ShutterAct GetAct()
        {
            ShutterAct act = ShutterAct.Noop;

            if (IsOpen() && !IsClose()) act = ShutterAct.Open;
            if (!IsOpen() && IsClose()) act = ShutterAct.Close;

            return act;
        }

        public bool IsOpen()
        {
            return m_ShutterCylinder.IsActStatus(ActuatorAct.Pos);
        }

        public bool IsClose()
        {
            return m_ShutterCylinder.IsActStatus(ActuatorAct.Neg);
        }

        public int Open()
        {
            return m_SeqShutterOpen.Do();
        }

        public int Close()
        {
            return m_SeqShutterClose.Do();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ShutterUnit); }
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
                //ALM_CylinderFw = new Alarm(this.Name + " FW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_CylinderBw = new Alarm(this.Name + " BW Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


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
                    InitSequence();

                    m_ThreadShutterManualAct.Start();
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
            m_Tag.SetValue(tagDescriptor.Open, IsOpen());
            m_Tag.SetValue(tagDescriptor.Close, IsClose());
        }
        #endregion
    }
}
