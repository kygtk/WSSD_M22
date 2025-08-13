using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Dms.Common;
using Dms.Data;
using Dms.Ctl;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;
using System.Xml.Serialization;
using System.Threading;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ST540 : _Tic
    {
        private _GenericCollection<AlarmItem> m_TicAlarms = new _GenericCollection<AlarmItem>();

        private PortNo m_PortNo = PortNo.COM1;

        public Alarm ALM_TicAlarm;

        private double[] m_CurSvValue = null;
        private double[] m_CurPvValue = null;
        private int m_MonitorNo = 0;

        private XComm m_Comm;

        #region Properties
        [Category("DMS : Setting")]
        public _GenericCollection<AlarmItem> AlarmItems
        {
            get { return m_TicAlarms; }
            set { m_TicAlarms = value; }
        }
        [Category("DMS : Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Category("DMS : Setting")]
        public int MonitorNo
        {
            get { return m_MonitorNo; }
            set { m_MonitorNo = value; }
        }

        [Browsable(false), XmlIgnore()]
        public string PortName
        {
            get { return m_PortNo.ToString(); }
        }

        [Browsable(false), XmlIgnore()]
        public double[] CurTemperature
        {
            get { return m_CurPvValue; }
            set { m_CurPvValue = value; }
        }
        #endregion

        #region Constructor
        public ST540()
        {

        } 
        #endregion

        #region Methods
        public bool IsOpen()
        {
            return m_Comm.IsOpen();
        }

        public void Close()
        {
            m_Comm.Close();
        }

        // Packet Format
        // Frame : [STX][Address][Command][,][개수][,][D-Register][,][Value][CR][LF]
        // Sise  :   1     2        3      1    2   1      4       1    4     1   1 
        public void SetTemperature(int address, int temp)
        {
            string sendData = null;

            sendData = Convert.ToChar(0x02) + string.Format("{0:X2}WSD,01,0201,{1:X4}", address, temp) + "\r\n";
            m_Comm.Write(sendData);
        }

        public void SetOutputHigh(int address, int value)
        {
            string sendData = null;

            sendData = Convert.ToChar(0x02) + string.Format("{0:X2}WSD,01,0641,{1:X4}", address, value) + "\r\n";
            m_Comm.Write(sendData);
        }

        public void SetAlarmTemperature(int address, int temp)
        {
            string sendData = null;

            sendData = Convert.ToChar(0x02) + string.Format("{0:X2}WSD,03,0406,{1:X4},{2:X4},{3:X4}", address, temp, temp, temp) + "\r\n";
            m_Comm.Write(sendData);
        }

        // Packet Format
        // Frame : [STX][Address][Command][,][개수][,][D-Register][CR][LF]
        // Sise  :   1     2        3      1    2   1      4        1   1 
        public void RequsetCurTemperature(int address)
        {
            string sendData = null;
            
            sendData = Convert.ToChar(0x02) + string.Format("{0:x2}RSD,02,0001", address) + "\r\n";
            m_Comm.Write(sendData);
        }

        public void SetLog(string sLog)
        {
            m_Comm.SetLog(sLog);
        }

        // TIC로 부터의 Response는 
        // SetSvValue에 대한것과,
        // GetPvValue에 대한것 두가지다.
        // SetSvValue에 의한 Response는 단지 OK여부이므로 무시
        // GetPvValue에 의한 Response는 Heater Ready 여부를 판단하기 위해 처리한다.
        // SetSvValue에 대한 Response Packet Size : 11 Byte
        // GetPvValue에 대한 Response Packet Size : 16 Byte
        // Packet Format -> SetSvValue
        // Frame : [STX][Address][Command][,][Result][CR][LF]
        // Sise  :   1             2              3         1     2          1     1 
        // Packet Format -> GetPvValue
        // Frame : [STX][Address][Command][,][Result][,][Value][CR][LF]
        // Sise  :   1            2               3         1       2      1     4        1      1 
        public void ReceivedData(object sender)
        {
            Thread.Sleep(50);
            string recvData = m_Comm.ReadLine();
            string address = recvData.Substring(1, 2);
            string command = recvData.Substring(3, 3);
            string result = recvData.Substring(7, 2);

            if ("RSD" == command && "OK" == result)
            {
                string value = recvData.Substring(10, 4);
                m_CurPvValue[Convert.ToInt32(address)-1] = HexaString2Integer(value);

                //string msg;

                //if (Convert.ToInt32(address) == 1)
                //{
                //    msg = "Tic " + address.ToString() + " = " + m_CurPvValue[Convert.ToInt32(address) - 1].ToString();

                //    m_Server.EqpLog.TextOut(msg);

                //}
            }
            else if ("WSD" == command)
            {

            }
      
        }

        public int HexaString2Integer(string hex)
        {
            int rv = 0, v;
            int jari = hex.Length - 1;

            int count = hex.Length;
            for (int i = 0; i < count; i++)
            {
                v = hex[i];

                if (v >= 'A' && v <= 'F')
                {
                    rv += (int)Math.Pow(0x10, jari) * (v - 'A' + 0xA);
                }
                else if (v >= 'a' && v <= 'f')
                {
                    rv += (int)Math.Pow(0x10, jari) * (v - 'a' + 0xA);
                }
                else if (v >= '0' && v <= '9')
                {
                    rv += (int)((Math.Pow(0x10, jari)) * (v - '0'));
                }
                jari--;
            }
            return rv;
        } 
        #endregion

        #region Override
        public override void SetSvValue(int address, int temp)
        {
            SetTemperature(address, temp);
        }

        public override double GetPvValue(int address)
        {
            return m_CurPvValue[address];
        }

        public override bool IsAlarm()
        {
            bool result = false;

            //int count = m_TicAlarms.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    result |= m_TicAlarms[0].TicAlarms[i].DiAlarm.GetState();
            //}

            return result;
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
                m_CurSvValue = new double[MonitorNo];
                m_CurPvValue = new double[MonitorNo];

                m_Comm = new XComm();
                m_Comm.Initialize();
                m_Comm.Simulate = m_Simul.Comport;
                m_Comm.Open(PortName, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


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

        public override void SetSubscriber()
        {
            base.SetSubscriber();

            m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        } 
        #endregion
    }
}
