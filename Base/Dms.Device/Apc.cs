///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.10.22
// Author       : eun
// Description  : Mfc for PSM's AP Plasma
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using System.IO.Ports;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using Dms.Ctl;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Apc : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorApc tagDescriptor = new TagDescriptorApc();
        #endregion

        #region Enum
        public enum Point
        {
            A = 0, 
            B, 
            C,
            D, 
            E, 
            MaxNo
        }
        #endregion
        
        #region Fields
        private static object m_LockKey = new object();
        private int m_Remote;
        private int m_SelectedPoint;

        private double m_GetPressure = 0.0;
        private double[] m_SetPointValues;

        private PortNo m_PortNo = PortNo.COM1;
        private XComm m_Comm;
        private string m_Command = "Noop";

        private TagSetupInfo m_ApcFullScale; 
        #endregion

        #region Properties
        [Category("DMS :Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public XComm Comm
        {
            get { return m_Comm; }
            set { m_Comm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string PortName
        {
            get { return m_PortNo.ToString(); }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupApcFullScale
        {
            get { return m_ApcFullScale; }
            set { m_ApcFullScale = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double Pressure
        {
            get { return m_GetPressure; }
            set { m_GetPressure = value; }
        }
        #endregion

        #region Constructor
        public Apc()
        {
            this.Name = "__ Apc";
        }
        #endregion

        #region Methods
        public int GetSelectedPoint()
        {
            return m_SelectedPoint;
        }

        public double GetSetPointValue(Point point)
        {
            return m_SetPointValues[Convert.ToInt32(point)];
        }

        public double GetPressure()
        {
            return m_GetPressure;
        }

        public bool IsRemote()
        {
            bool result = false;

            if (m_Remote == 1) result = true;

            return result;
        }

        //ch1(A) : Base Pressure(torr), ch2(B) : Process Pressure(torrr), ch5(E) : Full Open(100%)
        public void ReceivedData(object sender)
        {
            string recvData = m_Comm.ReadLine();
            int length = recvData.Length;
            
            if (length < 3) return;

            string ch1 = recvData.Substring(0, 1);
            string ch2 = recvData.Substring(1, 1);

            string value1 = recvData.Substring(1, length - 1);
            string value2 = recvData.Substring(2, length - 2);

            switch (ch1)
            {
                // Select Point
                case "S" :
                    {
                        if (ch1 == "1")
                            m_SetPointValues[0] = (Convert.ToDouble(value2) / 100.0) * SetupApcFullScale.GetValue<double>();
                        else if (ch1 == "2")
                            m_SetPointValues[1] = (Convert.ToDouble(value2) / 100.0) * SetupApcFullScale.GetValue<double>();
                        else if (ch1 == "3")
                            m_SetPointValues[2] = (Convert.ToDouble(value2) / 100.0) * SetupApcFullScale.GetValue<double>();
                        else if (ch1 == "4")
                            m_SetPointValues[3] = Convert.ToDouble(value2);
                        else if (ch1 == "5")
                            m_SetPointValues[4] = Convert.ToDouble(value2);
                    }
                    break;

                // Pvalue
                // %of Sensor full scale
                case "P":
                    {
                        m_GetPressure = (Convert.ToDouble(value1) / 100) * SetupApcFullScale.GetValue<double>();
                    }
                    break;

                // Mxyz : x ---> 0(Local), 1(Remote)
                //		  y ---> Not used
                //		  z ---> 0(Open), 1(Close), 2(Stop), 3(A), 4(B), 5(C), 6(D), 7(E), 8(Analog Set Point)
                case "M":
                    {
                        if (length < 4) return;

                        m_Remote = Convert.ToInt32(ch2);
                        m_SelectedPoint = Convert.ToInt32(recvData.Substring(3, 1));                        
                    }
                    break;
            }
        }

        public bool IsBasePressureSensed(double pressure)
        {
            if ((m_GetPressure) <= pressure) return true;

            return false;

        }

        public bool IsProcessPressureSensed()
        {
            if ((m_GetPressure >= (m_SetPointValues[1] - (0.01 * SetupApcFullScale.GetValue<double>())) &&
                (m_GetPressure <= (m_SetPointValues[1] + (0.01 * SetupApcFullScale.GetValue<double>()))))) return true;

            return false;
        }

        public void SetPointPressure(Point point, double pressure)
        {
            lock(m_LockKey)
            {
                string sendData;
                int pointNo = Convert.ToInt32(point);
                m_SetPointValues[pointNo] = pressure;

                sendData = string.Format("S{0}{1:f3}", pointNo + 1, pressure * 100) + Environment.NewLine;

                m_Comm.Write(sendData);
            }
        }

        public void GetPointPressure(Point point)
        {
            lock(m_LockKey)
            {
                string sendData;
                int pointNo = Convert.ToInt32(point) + 1;

                if (point == Point.E) pointNo = 10;

                sendData = string.Format("R{0}", pointNo);

                m_Comm.Write(sendData);
            }
        }

        public void SelectPoint(Point point)
        {
            lock(m_LockKey)
            {
                string sendData;
                int pointNo = Convert.ToInt32(point) + 1;

                sendData = string.Format("D{0}", pointNo) + Environment.NewLine;
                m_Comm.Write(sendData);
                m_Command = string.Format("SELECTED_POINT_{0}", Convert.ToChar(64 + pointNo));
            }
        }

        public void ReqCurPressure()
        {
            lock(m_LockKey)
            {
                string msg = "R5" + Environment.NewLine;
                m_Comm.Write(msg);
            }
        }

        public void ReqApcState()
        {
            lock(m_LockKey)
            {
                string msg = "R37" + Environment.NewLine;
                m_Comm.Write(msg);
            }
        }

        public void SetValveOpen()
        {
            lock(m_LockKey)
            {
                string msg = "O" + Environment.NewLine;
                m_Comm.Write(msg);
                m_Command = "OPEN";
            }
        }

        public void SetValveClose()
        {
            lock(m_LockKey)
            {
                string msg = "C" + Environment.NewLine;
                m_Comm.Write(msg);
                m_Command = "CLOSE";
            }
        }

        public void SetValveHold()
        {
            lock(m_LockKey)
            {
                string msg = "H" + Environment.NewLine;
                m_Comm.Write(msg);
                m_Command = "HOLD";
            }
        }
        #endregion

        #region Override
        public override void SetSubscriber()
        {
            base.SetSubscriber();

            m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
        }

        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
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
            m_Tag.SetValue(tagDescriptor.VALUE, m_Command);

            string pressure = string.Format("{0:f3} Torr", m_GetPressure);

            m_Tag.SetValue(tagDescriptor.PRESSURE, pressure);
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
            //ok &= (m_AoGaugeFlowSet != null);
            //ok &= (m_Gauge != null);

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
                m_ApcFullScale = new TagSetupInfo(this.Name + " Full Scale", OptionType.None, OptionFormat.Float, UnitType.Torr, "10");
                SetupGenInfoProvider.Instance.InitFromDB(m_ApcFullScale);
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_SetPointValues = new double[Convert.ToInt32(Point.MaxNo)];

                m_Comm = new XComm();
                m_Comm.Initialize();
                m_Comm.Simulate = m_Simul.Comport;
                m_Comm.Open(PortName, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);



                if (m_Simul.Device)
                {
                    //m_GetPressure = m_Server.JobCond.CurrentRecipe.BaseVacuumPress;
                }


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
