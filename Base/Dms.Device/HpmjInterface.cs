
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
    public class HpmjInterface : _DeviceAsm
    {
        #region Fields
        public  Alarm  ALM_ReadyFail =null;
        public  Alarm  ALM_AlarmHeavy=null;
        public  Alarm  ALM_AlarmLight=null;
        public Alarm ALM_RemoteFail = null;
        public Alarm ALM_HPMJ_UNIT_ESTOP = null;
        public Alarm ALM_HPMJ_LEAK_ERROR = null;
        public Alarm ALM_HPMJ_PLC_BAT_LOW = null;
        public Alarm ALM_HPMJ_INVERTER_ERROR = null;
        public Alarm ALM_HPMJ_INVERTER_ELB_TRIP = null;
        public Alarm ALM_HPMJ_INVERTER_MC_TRIP = null;
        public Alarm ALM_HPMJ_INVERTER_MC_ON_CHECK_ERROR = null;
        public Alarm ALM_HPMJ_CP_GENERATOR_POWER = null;
        public Alarm ALM_HPMJ_FILTER_IN_PRESS_UP_ERROR = null;
        public Alarm ALM_HPMJ_FILTER_IN_PRESS_UP_WARNING = null;
        public Alarm ALM_HPMJ_FILTER_IN_PRESS_LO_WARNING = null;
        public Alarm ALM_HPMJ_FILTER_IN_PRESS_LO_ERROR = null;
        public Alarm ALM_HPMJ_FILTER_OUT_PRESS_UP_ERROR = null;
        public Alarm ALM_HPMJ_FILTER_OUT_PRESS_UP_WARNING = null;
        public Alarm ALM_HPMJ_FILTER_OUT_PRESS_LO_WARNING = null;
        public Alarm ALM_HPMJ_FILTER_OUT_PRESS_LO_ERROR = null;
        public Alarm ALM_HPMJ_SHOWER_FLOW_UP_ERROR = null;
        public Alarm ALM_HPMJ_SHOWER_FLOW_UP_WARNING = null;
        public Alarm ALM_HPMJ_SHOWER_FLOW_LO_WARNING = null;
        public Alarm ALM_HPMJ_SHOWER_FLOW_LO_ERROR = null;
        public Alarm ALM_HPMJ_PUMP_CHANGE_OVER_TIME = null;
        public Alarm ALM_HPMJ_FILTER_CHANGE_OVER_TIME = null;
        public Alarm ALM_HPMJ_MAIN_DI_PRESS_UP_ERROR = null;
        public Alarm ALM_HPMJ_MAIN_DI_PRESS_UP_WARNING = null;
        public Alarm ALM_HPMJ_MAIN_DI_PRESS_LO_WARNING = null;
        public Alarm ALM_HPMJ_MAIN_DI_PRESS_LO_ERROR = null;
        public Alarm ALM_HPMJ_RESISTIVITY_UP_ERROR = null;
        public Alarm ALM_HPMJ_RESISTIVITY_UP_WARNING = null;
        public Alarm ALM_HPMJ_RESISTIVITY_LO_WARNING = null;
        public Alarm ALM_HPMJ_RESISTIVITY_LO_ERROR = null;
        public Alarm ALM_HPMJ_MELSEC_COMM_ERROR = null;
        public Alarm ALM_HPMJ_FILTER_DIFF_PRESS_UP_ERROR = null;
        public Alarm ALM_HPMJ_FILTER_DIFF_PRESS_UP_WARNING = null;
        public Alarm ALM_HPMJ_CO2_PRESS_UP_ERROR = null;
        public Alarm ALM_HPMJ_CO2_PRESS_UP_WARNING = null;
        public Alarm ALM_HPMJ_CO2_PRESS_LO_WARNING = null;
        public Alarm ALM_HPMJ_CO2_PRESS_LO_ERROR = null;
        public Alarm ALM_HPMJ_LOAD_CURRENT_UP_ERROR = null;
        public Alarm ALM_HPMJ_LOAD_CURRENT_UP_WARNING = null;
        public Alarm ALM_HPMJ_CO2_BUBBLER_CHANGE_TIME_OVER = null;
        public Alarm ALM_HPMJ_CO2_FLOW_UP_ERROR = null; // 11.05.03 minhan
        public Alarm ALM_HPMJ_CO2_FLOW_UP_WARNING = null;
        public Alarm ALM_HPMJ_CO2_FLOW_LO_WARNING = null;
        public Alarm ALM_HPMJ_CO2_FLOW_LO_ERROR = null;

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

        private IoAnalogInput m_WInputSignal1 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal2 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal3 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal4 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal5 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal6 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal7 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal8 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal9 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal10 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal11 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal12 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal13= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal14= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal15 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal16= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal17= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal18= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal19= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal20= new IoAnalogInput();
        private IoAnalogInput m_WInputSignal21 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal22 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal23 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal24 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal25 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal26 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal27 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal28 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal29 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal30 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal31 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal32 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal33 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal34 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal35 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal36 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal37 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal38 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal39 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal40 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal41 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal42 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal43 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal44 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal45 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal46 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal47 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal48 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal49 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal50 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal51 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal52 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal53 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal54 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal55 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal56 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal57 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal58 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal59 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal60 = new IoAnalogInput(); // 11.05.03 minhan
        private IoAnalogInput m_WInputSignal61 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal62 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal63 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal64 = new IoAnalogInput();
        private IoAnalogInput m_WInputSignal65 = new IoAnalogInput();
      
        private IoAnalogOutput m_WOutputSignal1 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal2 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal3 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal4 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal5 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal6 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal7 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal8 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal9 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal10 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal11 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal12 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal13 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal14 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal15 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal16 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal17 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal18 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal19 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal20 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal21 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal22 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal23 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal24 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal25 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal26 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal27 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal28 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal29 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal30 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal31 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal32 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal33 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal34 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal35 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal36 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal37 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal38 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal39 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal40 = new IoAnalogOutput(); // 11.05.03 minhan
        private IoAnalogOutput m_WOutputSignal41 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal42 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal43 = new IoAnalogOutput();
        private IoAnalogOutput m_WOutputSignal44 = new IoAnalogOutput();


        #endregion
         #region Properties
        [Category("DMS : Bit In")]
        public IoDigitalInput diReady
        {
            get { return m_InputSignal1; }
            set { m_InputSignal1 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diRemote
        {
            get { return m_InputSignal2; }
            set { m_InputSignal2 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diLocal
        {
            get { return m_InputSignal3; }
            set { m_InputSignal3 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diRun_Pump
        {
            get { return m_InputSignal4; }
            set { m_InputSignal4 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diAlarmHeavy
        {
            get { return m_InputSignal5; }
            set { m_InputSignal5 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diAlarmLight
        {
            get { return m_InputSignal6; }
            set { m_InputSignal6 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diParameter_Change_Request_Ack
        {
            get { return m_InputSignal7; }
            set { m_InputSignal7 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diPPID_Change_Request_Ack
        {
            get { return m_InputSignal8; }
            set { m_InputSignal8 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diTotal_Running_Time_Reset_Ack
        {
            get { return m_InputSignal9; }
            set { m_InputSignal9 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diPump_Packing_Change_Count_Reset_Ack
        {
            get { return m_InputSignal10; }
            set { m_InputSignal10 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diFliter_Change_Count_Reset_Ack
        {
            get { return m_InputSignal11; }
            set { m_InputSignal11 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diAlarm_Reset_Request_Ack
        {
            get { return m_InputSignal12; }
            set { m_InputSignal12 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diBuzzer_Stop_Request_Ack
        {
            get { return m_InputSignal13; }
            set { m_InputSignal13 = value; }
        }
        [Category("DMS : Bit In")]
        public IoDigitalInput diMelsecNet_OnLine
        {
            get { return m_InputSignal14; }
            set { m_InputSignal14 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doRemote_Request
        {
            get { return m_OutputSignal1; }
            set { m_OutputSignal1 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doLocal_Request
        {
            get { return m_OutputSignal2; }
            set { m_OutputSignal2 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doStart_Pump
        {
            get { return m_OutputSignal3; }
            set { m_OutputSignal3 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doParameter_Change_Request
        {
            get { return m_OutputSignal4; }
            set { m_OutputSignal4 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doPPID_Change_Request
        {
            get { return m_OutputSignal5; }
            set { m_OutputSignal5 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doTotal_Running_Time_Reset
        {
            get { return m_OutputSignal6; }
            set { m_OutputSignal6 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doPump_Packing_Change_Count_Reset
        {
            get { return m_OutputSignal7; }
            set { m_OutputSignal7 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doFilter_Change_Count_Reset
        {
            get { return m_OutputSignal8; }
            set { m_OutputSignal8 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doAlarm_Reset_Request
        {
            get { return m_OutputSignal9; }
            set { m_OutputSignal9 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doBuzzer_Stop_Request
        {
            get { return m_OutputSignal10; }
            set { m_OutputSignal10 = value; }
        }
        [Category("DMS : Bit Out")]
        public IoDigitalOutput doMelsecNet_OnLine
        {
            get { return m_OutputSignal11; }
            set { m_OutputSignal11 = value; }
        }

        [Category("DMS : Word In")]
        public IoAnalogInput miwShower_Flow
        {
            get { return m_WInputSignal1; }
            set { m_WInputSignal1 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_In_Press
        {
            get { return m_WInputSignal2; }
            set { m_WInputSignal2 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Out_Press
        {
            get { return m_WInputSignal3; }
            set { m_WInputSignal3 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_DI_Press
        {
            get { return m_WInputSignal4; }
            set { m_WInputSignal4 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwDIW_Resistivity
        {
            get { return m_WInputSignal5; }
            set { m_WInputSignal5 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwInverter_Hertz
        {
            get { return m_WInputSignal6; }
            set { m_WInputSignal6 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Press
        {
            get { return m_WInputSignal7; }
            set { m_WInputSignal7 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwInverter_Load_Current
        {
            get { return m_WInputSignal8; }
            set { m_WInputSignal8 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Diffrence_Press
        {
            get { return m_WInputSignal9; }
            set { m_WInputSignal9 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwTotal_Running_Time_Count
        {
            get { return m_WInputSignal10; }
            set { m_WInputSignal10 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwPump_Packing_Change_Count
        {
            get { return m_WInputSignal11; }
            set { m_WInputSignal11 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Change_Count
        {
            get { return m_WInputSignal12; }
            set { m_WInputSignal12 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwPump_Packing_Change_Set
        {
            get { return m_WInputSignal13; }
            set { m_WInputSignal13 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Change_Count_Set
        {
            get { return m_WInputSignal14; }
            set { m_WInputSignal14 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwAlarm_Code
        {
            get { return m_WInputSignal15; }
            set { m_WInputSignal15 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwAlarm_Code1
        {
            get { return m_WInputSignal16; }
            set { m_WInputSignal16 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwAlarm_Code2
        {
            get { return m_WInputSignal17; }
            set { m_WInputSignal17 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwAlarm_Code3
        {
            get { return m_WInputSignal18; }
            set { m_WInputSignal18 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwCO2_Bubbler_Change_Count
        {
            get { return m_WInputSignal19; }
            set { m_WInputSignal19 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwCO2_Bubbler_Change_Set
        {
            get { return m_WInputSignal20; }
            set { m_WInputSignal20 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwShower_Flow_Set // 11.02.07 minhan
        {
            get { return m_WInputSignal21; }
            set { m_WInputSignal21 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwShower_Flow_Upper_Error
        {
            get { return m_WInputSignal22; }
            set { m_WInputSignal22 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwShower_Flow_Lower_Error
        {
            get { return m_WInputSignal23; }
            set { m_WInputSignal23 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwShower_Flow_Upper_Warning
        {
            get { return m_WInputSignal24; }
            set { m_WInputSignal24 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwShower_Flow_Lower_Warning
        {
            get { return m_WInputSignal25; }
            set { m_WInputSignal25 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_In_Press_Set
        {
            get { return m_WInputSignal26; }
            set { m_WInputSignal26 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_In_Press_Upper_Error
        {
            get { return m_WInputSignal27; }
            set { m_WInputSignal27 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_In_Press_Lower_Error
        {
            get { return m_WInputSignal28; }
            set { m_WInputSignal28 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_In_Press_Upper_Warning
        {
            get { return m_WInputSignal29; }
            set { m_WInputSignal29 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_In_Press_Lower_Warning
        {
            get { return m_WInputSignal30; }
            set { m_WInputSignal30 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Out_Press_Set
        {
            get { return m_WInputSignal31; }
            set { m_WInputSignal31 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Out_Press_Upper_Error
        {
            get { return m_WInputSignal32; }
            set { m_WInputSignal32 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Out_Press_Lower_Error
        {
            get { return m_WInputSignal33; }
            set { m_WInputSignal33 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Out_Press_Upper_Warning
        {
            get { return m_WInputSignal34; }
            set { m_WInputSignal34 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Out_Press_Lower_Warning
        {
            get { return m_WInputSignal35; }
            set { m_WInputSignal35 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwResistivity_Set
        {
            get { return m_WInputSignal36; }
            set { m_WInputSignal36 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwResistivity_Upper_Error
        {
            get { return m_WInputSignal37; }
            set { m_WInputSignal37 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwResistivity_Lower_Error
        {
            get { return m_WInputSignal38; }
            set { m_WInputSignal38 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwResistivity_Upper_Warning
        {
            get { return m_WInputSignal39; }
            set { m_WInputSignal39 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwResistivity_Lower_Warning
        {
            get { return m_WInputSignal40; }
            set { m_WInputSignal40 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwDI_Press_Set
        {
            get { return m_WInputSignal41; }
            set { m_WInputSignal41 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwDI_Press_Upper_Error
        {
            get { return m_WInputSignal42; }
            set { m_WInputSignal42 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwDI_Press_Lower_Error
        {
            get { return m_WInputSignal43; }
            set { m_WInputSignal43 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwDI_Press_Upper_Warning
        {
            get { return m_WInputSignal44; }
            set { m_WInputSignal44 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwDI_Press_Lower_Warning
        {
            get { return m_WInputSignal45; }
            set { m_WInputSignal45 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Difference_Press_Set
        {
            get { return m_WInputSignal46; }
            set { m_WInputSignal46 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Difference_Press_Upper_Error
        {
            get { return m_WInputSignal47; }
            set { m_WInputSignal47 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFilter_Difference_Press_Upper_Warning
        {
            get { return m_WInputSignal48; }
            set { m_WInputSignal48 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Press_Set
        {
            get { return m_WInputSignal49; }
            set { m_WInputSignal49 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Press_Upper_Error
        {
            get { return m_WInputSignal50; }
            set { m_WInputSignal50 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Press_Lower_Error
        {
            get { return m_WInputSignal51; }
            set { m_WInputSignal51 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Press_Upper_Warning
        {
            get { return m_WInputSignal52; }
            set { m_WInputSignal52 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Press_Lower_Warning
        {
            get { return m_WInputSignal53; }
            set { m_WInputSignal53 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwInverter_Load_Current_Set
        {
            get { return m_WInputSignal54; }
            set { m_WInputSignal54 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwInveter_Load_Current_Upper_Error
        {
            get { return m_WInputSignal55; }
            set { m_WInputSignal55 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwInveter_Load_Current_Upper_Warning 
        {
            get { return m_WInputSignal56; }
            set { m_WInputSignal56 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwRunning_Mode
        {
            get { return m_WInputSignal57; }
            set { m_WInputSignal57 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwFrequency_Set
        {
            get { return m_WInputSignal58; }
            set { m_WInputSignal58 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwPressure_Set // 11.02.07 minhan
        {
            get { return m_WInputSignal59; }
            set { m_WInputSignal59 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Flow_Set // 11.05.03 minhan
        {
            get { return m_WInputSignal60; }
            set { m_WInputSignal60 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Flow_Upper_Error
        {
            get { return m_WInputSignal61; }
            set { m_WInputSignal61 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Flow_Lower_Error
        {
            get { return m_WInputSignal62; }
            set { m_WInputSignal62 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Flow_Upper_Warning
        {
            get { return m_WInputSignal63; }
            set { m_WInputSignal63 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Flow_Lower_Warning
        {
            get { return m_WInputSignal64; }
            set { m_WInputSignal64 = value; }
        }
        [Category("DMS : Word In")]
        public IoAnalogInput miwMain_CO2_Flow // 11.05.03 minhan
        {
            get { return m_WInputSignal65; }
            set { m_WInputSignal65 = value; }
        }
        
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowRunning_Mode
        {
            get { return m_WOutputSignal1; }
            set { m_WOutputSignal1 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFrequency_Set
        {
            get { return m_WOutputSignal2; }
            set { m_WOutputSignal2 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowPressure_Set
        {
            get { return m_WOutputSignal3; }
            set { m_WOutputSignal3 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowShower_Flow_Set
        {
            get { return m_WOutputSignal4; }
            set { m_WOutputSignal4 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowShower_Flow_Upper_Error
        {
            get { return m_WOutputSignal5; }
            set { m_WOutputSignal5 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowShower_Flow_Lower_Error
        {
            get { return m_WOutputSignal6; }
            set { m_WOutputSignal6 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowShower_Flow_Upper_Warning
        {
            get { return m_WOutputSignal7; }
            set { m_WOutputSignal7 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowShower_Flow_Lower_Warning
        {
            get { return m_WOutputSignal8; }
            set { m_WOutputSignal8 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_In_Press_Set
        {
            get { return m_WOutputSignal9; }
            set { m_WOutputSignal9 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_In_Press_Upper_Error
        {
            get { return m_WOutputSignal10; }
            set { m_WOutputSignal10 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_In_Press_Lower_Error
        {
            get { return m_WOutputSignal11; }
            set { m_WOutputSignal11 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_In_Press_Upper_Warning
        {
            get { return m_WOutputSignal12; }
            set { m_WOutputSignal12 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_In_Press_Lower_Warning
        {
            get { return m_WOutputSignal13; }
            set { m_WOutputSignal13 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Out_Press_Set
        {
            get { return m_WOutputSignal14; }
            set { m_WOutputSignal14 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Out_Press_Upper_Error
        {
            get { return m_WOutputSignal15; }
            set { m_WOutputSignal15 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Out_Press_Lower_Error
        {
            get { return m_WOutputSignal16; }
            set { m_WOutputSignal16 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Out_Press_Upper_Warning
        {
            get { return m_WOutputSignal17; }
            set { m_WOutputSignal17 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Out_Press_Lower_Warning
        {
            get { return m_WOutputSignal18; }
            set { m_WOutputSignal18 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowResistivity_Set
        {
            get { return m_WOutputSignal19; }
            set { m_WOutputSignal19 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowResistivity_Upper_Error
        {
            get { return m_WOutputSignal20; }
            set { m_WOutputSignal20 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowResistivity_Lower_Error
        {
            get { return m_WOutputSignal21; }
            set { m_WOutputSignal21 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowResistivity_Upper_Warning
        {
            get { return m_WOutputSignal22; }
            set { m_WOutputSignal22 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowResistivity_Lower_Warning
        {
            get { return m_WOutputSignal23; }
            set { m_WOutputSignal23 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowDI_Press_Set
        {
            get { return m_WOutputSignal24; }
            set { m_WOutputSignal24 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowDI_Press_Upper_Error
        {
            get { return m_WOutputSignal25; }
            set { m_WOutputSignal25 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowDI_Press_Lower_Error
        {
            get { return m_WOutputSignal26; }
            set { m_WOutputSignal26 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowDI_Press_Upper_Warning
        {
            get { return m_WOutputSignal27; }
            set { m_WOutputSignal27 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowDI_Press_Lower_Warning
        {
            get { return m_WOutputSignal28; }
            set { m_WOutputSignal28 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Difference_Press_Set
        {
            get { return m_WOutputSignal29; }
            set { m_WOutputSignal29 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Difference_Press_Upper_Error
        {
            get { return m_WOutputSignal30; }
            set { m_WOutputSignal30 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowFilter_Difference_Press_Upper_Warning
        {
            get { return m_WOutputSignal31; }
            set { m_WOutputSignal31 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Press_Set
        {
            get { return m_WOutputSignal32; }
            set { m_WOutputSignal32 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Press_Upper_Error
        {
            get { return m_WOutputSignal33; }
            set { m_WOutputSignal33 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Press_Lower_Error
        {
            get { return m_WOutputSignal34; }
            set { m_WOutputSignal34 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Press_Upper_Warning
        {
            get { return m_WOutputSignal35; }
            set { m_WOutputSignal35 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Press_Lower_Warning
        {
            get { return m_WOutputSignal36; }
            set { m_WOutputSignal36 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowInverter_Load_Current_Set
        {
            get { return m_WOutputSignal37; }
            set { m_WOutputSignal37 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowInveter_Load_Current_Upper_Error
        {
            get { return m_WOutputSignal38; }
            set { m_WOutputSignal38 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowInveter_Load_Current_Upper_Warning
        {
            get { return m_WOutputSignal39; }
            set { m_WOutputSignal39 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Flow_Set // 11.05.03 minhan
        {
            get { return m_WOutputSignal40; }
            set { m_WOutputSignal40 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Flow_Upper_Error
        {
            get { return m_WOutputSignal41; }
            set { m_WOutputSignal41 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Flow_Lower_Error
        {
            get { return m_WOutputSignal42; }
            set { m_WOutputSignal42 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Flow_Upper_Warning
        {
            get { return m_WOutputSignal43; }
            set { m_WOutputSignal43 = value; }
        }
        [Category("DMS : Word Out")]
        public IoAnalogOutput mowMain_CO2_Flow_Lower_Warning
        {
            get { return m_WOutputSignal44; }
            set { m_WOutputSignal44 = value; }
        }
        #endregion
 
        #region Constructor
        public HpmjInterface()
        {
            this.Name = "_HpmjInterface";
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
                ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_AlarmHeavy = new Alarm(this.Name + "Heavy Alarm status", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_AlarmLight = new Alarm(this.Name + "Light Alarm status", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_RemoteFail = new Alarm(this.Name + " Remote Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_UNIT_ESTOP	= new Alarm(this.Name + " ESTOP", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_LEAK_ERROR = new Alarm(this.Name + " LEAK_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_PLC_BAT_LOW = new Alarm(this.Name + " PLC_BAT_LOW", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_INVERTER_ERROR = new Alarm(this.Name + " INVERTER_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_INVERTER_ELB_TRIP = new Alarm(this.Name + " INVERTER_ELB_TRIP", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_INVERTER_MC_TRIP = new Alarm(this.Name + " INVERTER_MC_TRIP", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_INVERTER_MC_ON_CHECK_ERROR = new Alarm(this.Name + " INVERTER_MC_ON_CHECK_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CP_GENERATOR_POWER = new Alarm(this.Name + " CP_GENERATOR_POWER", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_IN_PRESS_UP_ERROR = new Alarm(this.Name + " FILTER_IN_PRESS_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_IN_PRESS_UP_WARNING = new Alarm(this.Name + " FILTER_IN_PRESS_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_IN_PRESS_LO_WARNING = new Alarm(this.Name + " FILTER_IN_PRESS_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_IN_PRESS_LO_ERROR = new Alarm(this.Name + " FILTER_IN_PRESS_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_OUT_PRESS_UP_ERROR = new Alarm(this.Name + " FILTER_OUT_PRESS_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_OUT_PRESS_UP_WARNING = new Alarm(this.Name + " FILTER_OUT_PRESS_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_OUT_PRESS_LO_WARNING = new Alarm(this.Name + " FILTER_OUT_PRESS_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_OUT_PRESS_LO_ERROR = new Alarm(this.Name + " FILTER_OUT_PRESS_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_SHOWER_FLOW_UP_ERROR = new Alarm(this.Name + " SHOWER_FLOW_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_SHOWER_FLOW_UP_WARNING = new Alarm(this.Name + " SHOWER_FLOW_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_SHOWER_FLOW_LO_WARNING = new Alarm(this.Name + " SHOWER_FLOW_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_SHOWER_FLOW_LO_ERROR = new Alarm(this.Name + " SHOWER_FLOW_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_PUMP_CHANGE_OVER_TIME = new Alarm(this.Name + " PUMP_CHANGE_OVER_TIME", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_CHANGE_OVER_TIME = new Alarm(this.Name + " FILTER_CHANGE_OVER_TIME", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_MAIN_DI_PRESS_UP_ERROR = new Alarm(this.Name + " MAIN_DI_PRESS_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_MAIN_DI_PRESS_UP_WARNING = new Alarm(this.Name + " MAIN_DI_PRESS_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_MAIN_DI_PRESS_LO_WARNING = new Alarm(this.Name + " MAIN_DI_PRESS_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_MAIN_DI_PRESS_LO_ERROR = new Alarm(this.Name + " MAIN_DI_PRESS_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_RESISTIVITY_UP_ERROR = new Alarm(this.Name + " RESISTIVITY_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_RESISTIVITY_UP_WARNING = new Alarm(this.Name + " RESISTIVITY_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_RESISTIVITY_LO_WARNING = new Alarm(this.Name + " RESISTIVITY_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_RESISTIVITY_LO_ERROR = new Alarm(this.Name + " RESISTIVITY_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_MELSEC_COMM_ERROR = new Alarm(this.Name + " MELSEC_COMM_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_DIFF_PRESS_UP_ERROR = new Alarm(this.Name + " FILTER_DIFF_PRESS_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_FILTER_DIFF_PRESS_UP_WARNING = new Alarm(this.Name + "FILTER_DIFF_PRESS_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_PRESS_UP_ERROR = new Alarm(this.Name + "CO2_PRESS_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_PRESS_UP_WARNING = new Alarm(this.Name + "CO2_PRESS_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_PRESS_LO_WARNING = new Alarm(this.Name + "CO2_PRESS_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_PRESS_LO_ERROR = new Alarm(this.Name + "CO2_PRESS_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_LOAD_CURRENT_UP_ERROR = new Alarm(this.Name + "LOAD_CURRENT_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HPMJ_LOAD_CURRENT_UP_WARNING = new Alarm(this.Name + "LOAD_CURRENT_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_BUBBLER_CHANGE_TIME_OVER = new Alarm(this.Name + "CO2_BUBBLER_CHANGE_TIME_OVER", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_FLOW_UP_ERROR = new Alarm(this.Name + "CO2_FLOW_UP_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.05.03 minhan
                ALM_HPMJ_CO2_FLOW_UP_WARNING = new Alarm(this.Name + "CO2_FLOW_UP_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_FLOW_LO_WARNING = new Alarm(this.Name + "CO2_FLOW_LO_WARNING", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_HPMJ_CO2_FLOW_LO_ERROR = new Alarm(this.Name + "CO2_FLOW_LO_ERROR", AlarmLevel.S, AlarmCode.EquipmentSafety);
                
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