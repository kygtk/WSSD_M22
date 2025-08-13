///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2011.03.07
// Author       : minhan
// Description  : FFU Control Class
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;
using Dms.Common;
using Dms.Ctl;
using Dms.Data;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class FanFilterControl : _DeviceAsm
    {
        #region Enum
        enum ControlCode // 11.05.06 minhan
        {
            ccSTX = 0x02,		   // Start Byte
            ccSetMode1 = 0x8A,     // Mode1 
            ccSetMode2 = 0x87,     // Mode2
            ccSpeedMode1 = 0x89,   // Speed Mode1
            ccSpeedMode2 = 0x84,   // Speed Mode2
            ccStartBL_ID = 0x81,   // Start BL_ID
            ccETX = 0x03,		   // End Byte
        };
        #endregion

        #region Tag Descriptor
        protected static TagDescriptorFanFilterControl tagDescriptor = new TagDescriptorFanFilterControl();
        #endregion

        #region Fields
        private XComm m_Comm;
        private PortNo m_PortNo = PortNo.COM1;
        private TagSetupInfo m_SetupFfuControlIntr;
        private int m_Lv32No = 1;
        private const byte m_DpuId = 0x9F;
        private int m_Max_FFU_No = 32;
        private int m_Checksum = 0;
        private byte[] m_SendData = new byte[10]; // 11.05.06 minhan
        private byte[,] m_RecvData = new byte [32,4];
        private int m_DataSize = 0;
        private bool m_bRv = false;
        private bool m_IsAlarm = false;
        public delegate void GetFfuControlReceivedData(object sender);
        public event GetFfuControlReceivedData FfuDataReceived;
        public Alarm ALM_OverCurAlarm;
        public Alarm ALM_MotorAlarm;
        public Alarm ALM_NoConnect;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Category("DMS : Setting")]
        public int Lv32No
        {
            get { return m_Lv32No; }
            set { m_Lv32No = value; }
        }
        [Category("DMS : Setting")]
        public int Max_FFU_No
        {
            get { return m_Max_FFU_No; }
            set { m_Max_FFU_No = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string PortName
        {
            get { return m_PortNo.ToString(); }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupFfuControlIntr
        {
            get { return m_SetupFfuControlIntr; }
            set { m_SetupFfuControlIntr = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        #endregion

        #region Constructor
        public FanFilterControl()
        {
            this.Name = "__ Unit Fan Filter Control";
        }
        #endregion

        #region Methods
        public bool SendCommand()
        {
            m_Checksum = 0;

            m_SendData[0] = (byte)ControlCode.ccSTX;
            m_SendData[1] = (byte)ControlCode.ccSetMode1;
            m_SendData[2] = (byte)ControlCode.ccSetMode2;
            m_SendData[3] = (byte)(m_Lv32No + 0x80);
            m_SendData[4] = m_DpuId;
            m_SendData[5] = (byte)ControlCode.ccStartBL_ID;
            m_SendData[6] = (byte)(0x80 + m_Max_FFU_No);

            for(int i = 1; i < 7; i++)
            {
                m_Checksum = m_Checksum + m_SendData[i]; 
            }

            m_SendData[7] = (byte)(m_Checksum & 0xFF);
            m_SendData[8] = (byte)ControlCode.ccETX;
           
            m_bRv = m_Comm.Write(m_SendData, 0, 9);
            return m_bRv;
        }

        public bool SendMotorSpeed(int speed) // 11.05.06 minhan
        {
            m_Checksum = 0;

            m_SendData[0] = (byte)ControlCode.ccSTX;
            m_SendData[1] = (byte)ControlCode.ccSpeedMode1;
            m_SendData[2] = (byte)ControlCode.ccSpeedMode2;
            m_SendData[3] = (byte)(m_Lv32No + 0x80);
            m_SendData[4] = m_DpuId;
            m_SendData[5] = (byte)ControlCode.ccStartBL_ID;
            m_SendData[6] = (byte)(0x80 + m_Max_FFU_No);
            m_SendData[7] = (byte)(speed / 10);

            for (int i = 1; i < 8; i++)
            {
                m_Checksum = m_Checksum + m_SendData[i];
            }

            m_SendData[8] = (byte)(m_Checksum & 0xFF);
            m_SendData[9] = (byte)ControlCode.ccETX;

            m_bRv = m_Comm.Write(m_SendData, 0, 10);
            return m_bRv;
        }

        private void ReceivedData(object sender)
        {
            try
            {
                Thread.Sleep(1000); // 11.04.11 minhan
                m_DataSize = (m_Max_FFU_No * 4) + 7;
                byte[] m_buf = new byte[m_DataSize];

                int len = m_Comm.Read(ref m_buf, 0, m_DataSize); // Test 해봐야한다. ref를 쓰지 않는데, 일단 수정 하였음.

                if (m_buf[0] == (byte)ControlCode.ccSTX)
                {
                    if ((len == m_DataSize) &&
                        (m_buf[1] == (byte)ControlCode.ccSetMode1) &&
                        (m_buf[2] == (byte)ControlCode.ccSetMode2) &&
                        (m_buf[3] == (byte)(m_Lv32No + 0x80)) &&
                        (m_buf[4] == m_DpuId) &&
                        (m_buf[m_DataSize - 1] == (byte)ControlCode.ccETX))
                    {
                        int i = 5;

                        for (int j = 0; j < m_Max_FFU_No; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                m_RecvData[j, k] = m_buf[i];
                                i++;
                            }
                        }

                        if (FfuDataReceived != null)
                        {
                            FfuDataReceived(this);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public bool IsCurrentAlarm(int index)
        {
            if (m_RecvData[index,2] == 0xA0) return true;
            else return false;
        }
        public bool IsMotorAlarm(int index)
        {
            if (m_RecvData[index,2] == 0xC0) return true;
            else return false;
        }
        public bool IsNoConnect(int index)
        {
            if (m_RecvData[index,2] == 0x00) return true;
            else return false;
        }
        public int IsCurMotorSpeed(int index)
        {
            if (m_RecvData[index,1] != 0x00)
            {
                return (int)m_RecvData[index, 1] * 10; // 11.04.11 minhan
            }
            else
            {
                return 0;
            }
        }
        public int IsSetMotorSpeed(int index)
        {
            if (m_RecvData[index, 3] != 0x00)
            {
                return (int)m_RecvData[index, 3] * 10; // 11.05.06 minhan
            }
            else
            {
                return 0;
            }
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

            log = string.Format("FFU Control    \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(FanFilter); }
        }
        public override string ToString()
        {
            return this.Name;
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
                //CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                ALM_OverCurAlarm = new Alarm(this.Name + " Over Current Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_MotorAlarm = new Alarm(this.Name + " Motor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_NoConnect = new Alarm(this.Name + " No Connect", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion
               

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성    
                #region Example
                m_SetupFfuControlIntr = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                SetupGenInfoProvider.Instance.InitFromDB(m_SetupFfuControlIntr);
                #endregion
                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
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
        public override void SetSubscriber()
        {
            base.SetSubscriber();

            m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
        }
        public override void UpdateTag() // 특별히 사용할게 없음.
        {
            
        }
        //public override DmsErrors Uninitialize() // 11.06.01 minhan
        //{
        //    //if (m_Comm.IsOpen()) // 11.05.06 minhan
        //    //{
        //        m_Comm.Close();
        //    //}
        //    return base.Uninitialize();
        //}
        #endregion
    }
}
