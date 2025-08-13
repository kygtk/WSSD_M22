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
    public class D40A : _DevicePeer
    {
        #region Tag Descriptor
        TagDescriptorD40A tagDescriptor = new TagDescriptorD40A();
        #endregion

        #region Fields
        private readonly DoorSensor_Ap[] m_DoorSensors = new DoorSensor_Ap[6];
        #endregion

        #region Properties
        [Category("DMS : Sensors")]
        public DoorSensor_Ap DoorSensor0
        {
            get { return m_DoorSensors[0]; }
            set
            {
                m_DoorSensors[0] = value;
                if (m_DoorSensors[0] != null)
                {
                    m_DoorSensors[0].D40A = this;
                    m_DoorSensors[0].Channel = 0;
                }
            }
        }
        [Category("DMS : Sensors")]
        public DoorSensor_Ap DoorSensor1
        {
            get { return m_DoorSensors[1]; }
            set
            {
                m_DoorSensors[1] = value;
                if (m_DoorSensors[1] != null)
                {
                    m_DoorSensors[1].D40A = this;
                    m_DoorSensors[1].Channel = 1;
                }
            }
        }
        [Category("DMS : Sensors")]
        public DoorSensor_Ap DoorSensor2
        {
            get { return m_DoorSensors[2]; }
            set
            {
                m_DoorSensors[2] = value;
                if (m_DoorSensors[2] != null)
                {
                    m_DoorSensors[2].D40A = this;
                    m_DoorSensors[2].Channel = 2;
                }
            }
        }
        [Category("DMS : Sensors")]
        public DoorSensor_Ap DoorSensor3
        {
            get { return m_DoorSensors[3]; }
            set
            {
                m_DoorSensors[3] = value;
                if (m_DoorSensors[3] != null)
                {
                    m_DoorSensors[3].D40A = this;
                    m_DoorSensors[3].Channel = 3;
                }
            }
        }
        [Category("DMS : Sensors")]
        public DoorSensor_Ap DoorSensor4
        {
            get { return m_DoorSensors[4]; }
            set
            {
                m_DoorSensors[4] = value;
                if (m_DoorSensors[4] != null)
                {
                    m_DoorSensors[4].D40A = this;
                    m_DoorSensors[4].Channel = 4;
                }
            }
        }
        [Category("DMS : Sensors")]
        public DoorSensor_Ap DoorSensor5
        {
            get { return m_DoorSensors[5]; }
            set
            {
                m_DoorSensors[5] = value;
                if (m_DoorSensors[5] != null)
                {
                    m_DoorSensors[5].D40A = this;
                    m_DoorSensors[5].Channel = 5;
                }
            }
        }
        #endregion

        #region Constructor
        public D40A()
        {
            m_Name = "__ D40A";
            m_PeerType = PeerType.D40A;
        }
        #endregion

        #region Methods
        public DoorSensor GetDoorSensor(int channel)
        {
            return m_DoorSensors[channel];
        }

        public bool IsDetected(int channel)
        {
            if (!m_Initialized) return false;

            ushort state = m_SlaveAP.D40A_GetState();
            return (state & (1 << channel)) != 0;
        }

        private string GetState(int channel)
        {
            DoorSensor_Ap door = m_DoorSensors[channel];

            if (door == null) return "NULL";
            else
            {
                bool open = IsDetected(channel);

                if (open)
                    return "OPENED";
                else
                    return "CLOSED";
            }
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

            m_Tag.SetValue(tagDescriptor.DOOR0_STATE, GetState(0));
            m_Tag.SetValue(tagDescriptor.DOOR1_STATE, GetState(1));
            m_Tag.SetValue(tagDescriptor.DOOR2_STATE, GetState(2));
            m_Tag.SetValue(tagDescriptor.DOOR3_STATE, GetState(3));
            m_Tag.SetValue(tagDescriptor.DOOR4_STATE, GetState(4));
            m_Tag.SetValue(tagDescriptor.DOOR5_STATE, GetState(5));
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
                m_SlaveAP.SetPeer(PeerType.D40A);


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
