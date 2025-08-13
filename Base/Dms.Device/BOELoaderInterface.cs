using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class BOELoaderInterface : _DeviceAsm
    {
        #region Fields

        private IoDigitalInput m_InputSignal1 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal2 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal3 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal4 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal5 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal6 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal7 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal8 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal9 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal10 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal11 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal12 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal13 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal14 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal15 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal16 = new IoDigitalInput();
        private IoDigitalInput m_InputSignal17 = new IoDigitalInput(); // 10.12.25 minhan
        private IoDigitalInput m_InputSignal18 = new IoDigitalInput(); // 11.03.02 minhan
        private IoDigitalInput m_InputSignal19 = new IoDigitalInput(); // 11.06.10 minhan
        private IoDigitalInput m_InputSignal20 = new IoDigitalInput();  // dspcrassus - 이거는... SPT에서 SV Read에 대한 Ack를 사용할 때...

        private IoDigitalOutput m_OutputSignal1 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal2 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal3 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal4 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal5 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal6 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal7 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal8 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal9 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal10 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal11= new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal12 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal13 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal14 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal15 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal16 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal17= new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal18 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal19 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal20 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal21 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal22 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal23 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal24 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal25 = new IoDigitalOutput();
        private IoDigitalOutput m_OutputSignal26 = new IoDigitalOutput(); // 11.03.02 minhan
        private IoDigitalOutput m_OutputSignal27 = new IoDigitalOutput(); // 11.06.11 minhan
        private IoDigitalOutput m_OutputSignal28 = new IoDigitalOutput();   // dspcrassus - 이거는... SPT에서 SV Read에 대한 Request를 필요로할때...

        private IoAnalogInput m_WInputSignal1 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal2 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal3 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal4 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal5 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal6 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal7 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal8 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal9 = new IoAnalogInput(); // 10.12.25 minhan
        private IoAnalogInput m_WInputSignal10 = new IoAnalogInput(); // 11.03.02 minhan

        private IoAnalogOutput m_WOutPutSignal1 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal2 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal3 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal4 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal5 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal6 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal7 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal8 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal9 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal10 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal11 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal12 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal13 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal14 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal15 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal16 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutPutSignal17 = new IoAnalogOutput(); // 10.12.25 minhan
        private IoAnalogOutput m_WOutPutSignal18 = new IoAnalogOutput(); // 10.04.18 minhan
        private IoAnalogOutput m_WOutPutSignal19 = new IoAnalogOutput(); // 11.06.07 minhan
        private IoAnalogOutput m_WOutPutSignal20 = new IoAnalogOutput();


        #endregion
         #region Properties
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibTransfer_Readyto_Cleaner_Load_Stage
        {
            get { return m_InputSignal1; }
            set { m_InputSignal1 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibTransfer_Ready_to_Cleaner_Unload_Stage_L
        {
            get { return m_InputSignal2; }
            set { m_InputSignal2 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibRobot_Accessing_at_Cleaner_Load_Stage
        {
            get { return m_InputSignal3; }
            set { m_InputSignal3 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibCleaner_Recived_Data_Read_Request
        {
            get { return m_InputSignal4; }
            set { m_InputSignal4 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibCleaner_reported_Data_Read_Complete_L
        {
            get { return m_InputSignal5; }
            set { m_InputSignal5 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibNoSubstarate_to_Cleaner
        {
            get { return m_InputSignal6; }
            set { m_InputSignal6 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibSubstrate_ID_Data_Inside_Cleaner_Set_Request
        {
            get { return m_InputSignal7; }
            set { m_InputSignal7 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibLoader_Power_ON
        {
            get { return m_InputSignal8; }
            set { m_InputSignal8 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibCleaner_Alarm_Data_Read_Completed
        {
            get { return m_InputSignal9; }
            set { m_InputSignal9 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibRobot_Accessing_at_Cleaner_Unload_Stage
        {
            get { return m_InputSignal10; }
            set { m_InputSignal10 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibLoader_Trouble
        {
            get { return m_InputSignal11; }
            set { m_InputSignal11 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibCleaner_Next_Glass_Process_Start
        {
            get { return m_InputSignal12; }
            set { m_InputSignal12 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibTransfer_Ready_to_Cleaner_Unload_Stage_U
        {
            get { return m_InputSignal13; }
            set { m_InputSignal13 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibCleaner_Reported_Data_Read_Completed_U
        {
            get { return m_InputSignal14; }
            set { m_InputSignal14 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibRobot_Accessing_at_Cleaner_Unload_Stage_U
        {
            get { return m_InputSignal15; }
            set { m_InputSignal15 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibRecipe_Parameter_Change_Req
        {
            get { return m_InputSignal16; }
            set { m_InputSignal16 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibRecipe_Parameter_Change_Complete // 10.12.25 minhan
        {
            get { return m_InputSignal17; }
            set { m_InputSignal17 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibCrack_NG_Complete // 11.03.02 minhan
        {
            get { return m_InputSignal18; }
            set { m_InputSignal18 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibExchange_Req // 11.06.10 minhan
        {
            get { return m_InputSignal19; }
            set { m_InputSignal19 = value; }
        }
        [Category("DMS : Bit Input")]
        public IoDigitalInput mibTraceDataReadAck // 11.06.10 minhan
        {
            get { return m_InputSignal20; }
            set { m_InputSignal20 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Load_Request
        {
            get { return m_OutputSignal1; }
            set { m_OutputSignal1 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Unload_Request
        {
            get { return m_OutputSignal2; }
            set { m_OutputSignal2 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Access_Possible_Load_Stage
        {
            get { return m_OutputSignal3; }
            set { m_OutputSignal3 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Received_Data_Read_Completed
        {
            get { return m_OutputSignal4; }
            set { m_OutputSignal4 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Reported_Data_Read_Request
        {
            get { return m_OutputSignal5; }
            set { m_OutputSignal5 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobSubstrate_Present_at_Cleaner_Load_Stage
        {
            get { return m_OutputSignal6; }
            set { m_OutputSignal6 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed
        {
            get { return m_OutputSignal7; }
            set { m_OutputSignal7 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_is_Available
        {
            get { return m_OutputSignal8; }
            set { m_OutputSignal8 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Access_Possible_Unload_Stage
        {
            get { return m_OutputSignal9; }
            set { m_OutputSignal9 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobSubstrate_Present_at_Cleaner_Unload_Stage
        {
            get { return m_OutputSignal10; }
            set { m_OutputSignal10 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobUnload_Wait
        {
            get { return m_OutputSignal11; }
            set { m_OutputSignal11 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobUnload_Stage_Inter_Lock
        {
            get { return m_OutputSignal12; }
            set { m_OutputSignal12 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobIDLE
        {
            get { return m_OutputSignal13; }
            set { m_OutputSignal13 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobRUN
        {
            get { return m_OutputSignal14; }
            set { m_OutputSignal14 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobTROUBLE
        {
            get { return m_OutputSignal15; }
            set { m_OutputSignal15 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobLight_Alarm
        {
            get { return m_OutputSignal16; }
            set { m_OutputSignal16 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobSerious_Alarm
        {
            get { return m_OutputSignal17; }
            set { m_OutputSignal17 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Unload_Request_U
        {
            get { return m_OutputSignal18; }
            set { m_OutputSignal18 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Reported_Data_Read_Request_U
        {
            get { return m_OutputSignal19; }
            set { m_OutputSignal19 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleaner_Access_Possible_Unload_Stage_U
        {
            get { return m_OutputSignal20; }
            set { m_OutputSignal20 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobSubstrate_Present_at_Cleaner_Unload_Stage_U
        {
            get { return m_OutputSignal21; }
            set { m_OutputSignal21 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobUnload_Wait_U
        {
            get { return m_OutputSignal22; }
            set { m_OutputSignal22 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobUnload_Stage_Inter_lock_U
        {
            get { return m_OutputSignal23; }
            set { m_OutputSignal23 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobRecipe_Parameter_Change_Req
        {
            get { return m_OutputSignal24; }
            set { m_OutputSignal24 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobRecipe_Parameter_Change_Complete
        {
            get { return m_OutputSignal25; }
            set { m_OutputSignal25 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCrack_NG_Request // 11.03.02 minhan
        {
            get { return m_OutputSignal26; }
            set { m_OutputSignal26 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobCleanOut_Mode // 11.06.11 minhan
        {
            get { return m_OutputSignal27; }
            set { m_OutputSignal27 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput mobTraceDataReadRequest // 11.06.11 minhan
        {
            get { return m_OutputSignal28; }
            set { m_OutputSignal28 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miwPortID
        {
            get { return m_WInputSignal1; }
            set { m_WInputSignal1 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miwSlotID
        {
            get { return m_WInputSignal2; }
            set { m_WInputSignal2 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miw_Recipe_No
        {
            get { return m_WInputSignal3; }
            set { m_WInputSignal3 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miw_Lot_ID
        {
            get { return m_WInputSignal4; }
            set { m_WInputSignal4 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miw_Cst_ID
        {
            get { return m_WInputSignal5; }
            set { m_WInputSignal5 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miwGlsID
        {
            get { return m_WInputSignal6; }
            set { m_WInputSignal6 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miw_Recipe_Event // 10.12.25 minhan
        {
            get { return m_WInputSignal7; }
            set { m_WInputSignal7 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miwRecipe_Para_Form_MainEQ
        {
            get { return m_WInputSignal8; }
            set { m_WInputSignal8 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miwProcess_Data_for_MainEQ
        {
            get { return m_WInputSignal9; }
            set { m_WInputSignal9 = value; }
        }
        [Category("DMS : Word Input")]
        public IoAnalogInput miwRobot_Velocity // 11.03.02 minhan
        {
            get { return m_WInputSignal10; }
            set { m_WInputSignal10 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowAlarmID
        {
            get { return m_WOutPutSignal1; }
            set { m_WOutPutSignal1 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowAlarmCode
        {
            get { return m_WOutPutSignal2; }
            set { m_WOutPutSignal2 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowAlarmLevel
        {
            get { return m_WOutPutSignal3; }
            set { m_WOutPutSignal3 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowAlarmText
        {
            get { return m_WOutPutSignal4; }
            set { m_WOutPutSignal4 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mow_Recipe_Event // 10.12.25 minhan
        {
            get { return m_WOutPutSignal5; }
            set { m_WOutPutSignal5 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowPortID
        {
            get { return m_WOutPutSignal6; }
            set { m_WOutPutSignal6 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowSlotID
        {
            get { return m_WOutPutSignal7; }
            set { m_WOutPutSignal7 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowRecipeNo
        {
            get { return m_WOutPutSignal8; }
            set { m_WOutPutSignal8 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowLotID
        {
            get { return m_WOutPutSignal9; }
            set { m_WOutPutSignal9 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowCstID
        {
            get { return m_WOutPutSignal10; }
            set { m_WOutPutSignal10 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowGlassID
        {
            get { return m_WOutPutSignal11; }
            set { m_WOutPutSignal11 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowUnload_No1_SubstrateID_Data
        {
            get { return m_WOutPutSignal12; }
            set { m_WOutPutSignal12 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowUnload_No2_SubstrateID_Data
        {
            get { return m_WOutPutSignal13; }
            set { m_WOutPutSignal13 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowUnload_No3_SubstrateID_Data
        {
            get { return m_WOutPutSignal14; }
            set { m_WOutPutSignal14 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowUnload_No4_SubstrateID_Data
        {
            get { return m_WOutPutSignal15; }
            set { m_WOutPutSignal15 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowRecipe_parameter
        {
            get { return m_WOutPutSignal16; }
            set { m_WOutPutSignal16 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowProcess_Data_from_Cleaner
        {
            get { return m_WOutPutSignal17; }
            set { m_WOutPutSignal17 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowRecipe_DateTime // 11.04.18 minhan
        {
            get { return m_WOutPutSignal18; }
            set { m_WOutPutSignal18 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowGls_Crack // 11.06.07 minhan
        {
            get { return m_WOutPutSignal19; }
            set { m_WOutPutSignal19 = value; }
        }
        [Category("DMS : Word Output")]
        public IoAnalogOutput mowTraceDataForCleaner 
        {
            get { return m_WOutPutSignal20; }
            set { m_WOutPutSignal20 = value; }
        }
        #endregion

        #region Constructor
        public BOELoaderInterface()
        {
            this.Name = "_LoaderInterface";
        }
        #endregion
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
            //ok &= (m_DiAlarm != null);
            //ok &= (m_DiCpOn != null);


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
               // CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
               
                #endregion



                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
     
                #endregion
              


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                
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
               // m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            //m_Tag.SetValue(tagDescriptor.INPUTSIGNAL1, diLdNormalStatus.GetState());
           
        }
    }
}