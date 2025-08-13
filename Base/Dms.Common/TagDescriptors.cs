using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Common
{
    [Serializable()]
    [Editor(typeof(UIEditorTagDescriptorSelect), typeof(UITypeEditor))]
    public class TagDescriptor
    {
        #region Fields
        private int m_Id = 0;
        private string m_Key = "";
        #endregion

        #region Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        #endregion

        #region Constructor
        public TagDescriptor()
        {
        }

        public TagDescriptor(int id, string key)
        {
            m_Id = id;
            m_Key = key;
        }
        #endregion

        #region override
        public override string ToString()
        {
            return m_Id + " : " + m_Key;
        }
        #endregion
    }

    public class TagDescriptors
    {
        #region Implement IEnumerator
        // IEnumerable Interface Implementation:
        // Declaration of the GetEnumerator() method 
        // required by IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new InnerEnumerator(this);
        }

        // Inner class implements IEnumerator interface:
        private class InnerEnumerator : IEnumerator
        {
            private int m_Index = -1;
            private TagDescriptors m_Collection;

            public InnerEnumerator(TagDescriptors collection)
            {
                m_Collection = collection;
            }

            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (m_Index < m_Collection.Count - 1)
                {
                    m_Index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                m_Index = -1;
            }

            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return m_Collection.Items[m_Index];
                }
            }
        }
        #endregion

        #region Fields
        protected List<TagDescriptor> m_Items = new List<TagDescriptor>();
        #endregion

        #region Properties
        public List<TagDescriptor> Items
        {
            get { return m_Items; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        public TagDescriptor this[int index]
        {
            get { return m_Items[index]; }
        }
        #endregion

        #region Constructor
        public TagDescriptors()
        {
            GenerateDescriptor();
        }
        #endregion

        #region Methods
        protected void GenerateDescriptor()
        {
            try
            {
                //FieldInfo[] fieldInfos = this.GetType().GetFields();

                //foreach (FieldInfo fieldInfo in fieldInfos)
                //{
                //    if (fieldInfo.FieldType == typeof(int))
                //    {
                //        fieldInfo.SetValue(this, m_Items.Count);
                //        TagDescriptor tagDescriptor = new TagDescriptor(m_Items.Count, fieldInfo.Name);
                //        m_Items.Add(tagDescriptor);
                //    }
                //}

                //jemoon 상속관계에 있는 class의 FieldInfos는 역순으로 들어오므로
                //(최하위 자식 class의 field가 0, 부모class의 field는 n+1)
                //그러므로 상속관계에 있더라도 동일한 index 유지를 위해서는 reverse가 필요하다
                FieldInfo[] fieldInfos = this.GetType().GetFields();
                FieldInfo fieldInfo;
                int fieldCount = fieldInfos.Length;
                for (int i = (fieldCount - 1); i >= 0; i--)
                {
                    fieldInfo = fieldInfos[i];
                    if (fieldInfo.FieldType == typeof(int))
                    {
                        fieldInfo.SetValue(this, m_Items.Count);
                        TagDescriptor tagDescriptor = new TagDescriptor(m_Items.Count, fieldInfo.Name);
                        m_Items.Add(tagDescriptor);
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }
        #endregion
    }

    public class TagDescriptorGenInfo : TagDescriptors
    {
        public int VALUE;
    }

    public class TagDescriptorSensor : TagDescriptors
    {
        public int DETECT;
    }

    public class TagDescriptorDoorLock : TagDescriptorSensor
    {
        public int LOCKED;
    }

    public class TagDescriptorCoil : TagDescriptors
    {
        public int ON;
    }

    public class TagDescriptiorRbtHandInterlock : TagDescriptorSensor
    {
        public int DETECT_HAND;
    }

    public class TagDescriptorLevelSensor : TagDescriptorSensor
    {
        public int CONFIRM;
    }

    public class TagDescriptorGlsSensor : TagDescriptorSensor
    {
        public int SICK;
    }

    public class TagDescriptorDualGlsSensor : TagDescriptorGlsSensor
    {
        public int DETECTOP;
        public int SICKOP;
        public int USE;
        public int USEOP;
    }

    public class TagDescriptorProcessTime : TagDescriptors
    {
        public int PROCESSTIME;
    }

    public class TagDescriptorEuvUnit : TagDescriptors
    {
        public int WARNING;
        public int ALARM;
        public int USE;
        public int NOUSE;
    }

    public class TagDescriptorPartsItem : TagDescriptors
    {
        public int CURGLS;
        public int CURTIME;
    }

    public class TagDescriptorLamp : TagDescriptors
    {
        public int NOUSE;
        public int ON;
        public int OFF;
    }

    public class TagDescriptorEqpUnit : TagDescriptors
    {
        public int EQP_STATE;
        public int PROCESS_STATE;
    }

    public class TagDescriptorSignalTower : TagDescriptors
    {
        public int LAMP1;
        public int LAMP2;
        public int LAMP3;
        public int LAMP4;
    }

    public class TagDescriptorLampSwitch : TagDescriptors
    {
        public int LAMP;
    }

    public class TagDescriptorValve : TagDescriptors
    {
        public int RUN;
    }

    public class TagDescriptorActuator : TagDescriptors
    {
        public int ACT_COMMAND;
        public int ACT_STATUS;
        public int POS_SENSOR;
        public int NEG_SENSOR;
        public int POS_INTR_SENSOR;
        public int NEG_INTR_SENSOR;
        public int ALARM;
        public int CYLINDERTIME;
    }

    public class TagDescriptorActuatorTurn : TagDescriptorActuator
    {
        public int REF_POS;
        public int CUR_POS;
        public int POSITION_LIST;
        public int SENSOR_LIST;
    }

    public class TagDescriptorBrokenScan : TagDescriptors
    {
        public int START;
        public int END;
        public int ERROR;
        public int RESET;
    }

    public class TagDescriptorBrokenFixed : TagDescriptors
    {
        public int FRONT_1_DETECT;
        public int FRONT_2_DETECT;
        public int REAR_1_DETECT;
        public int REAR_2_DETECT;
    }

    public class TagDescriptorApdItem : TagDescriptors
    {
        public int VALUE;
    }

    public class TagDescriptorLpdItem : TagDescriptors
    {
        public int VALUE;
    }

    public class TagDescriptorTpdItem : TagDescriptors
    {
        public int VALUE;
    }

    public class TagDescriptorHepaFilter : TagDescriptors
    {
        public int ALARM;
        public int CPON;
    }

    public class TagDescriptorFanFilter : TagDescriptors
    {
        public int ALARM;
        public int CPON;
    }

    public class TagDescriptorFanFilterControl : TagDescriptors // 11.03.07 minhan
    {
        public int CurALARM;
        public int MotorALARM;
        public int NOCONNECT;
    }

    public class TagDescriptorLoaderInterlock : TagDescriptors
    {
        public int INPUTSIGNAL1;
        public int INPUTSIGNAL2;
        public int INPUTSIGNAL3;
        public int INPUTSIGNAL4;
        public int INPUTSIGNAL5;
        public int INPUTSIGNAL6;
        public int INPUTSIGNAL7;
        public int INPUTSIGNAL8;
        public int INPUTSIGNAL9;
        public int INPUTSIGNAL10;
        public int INPUTSIGNAL11;
        public int OUTPUTSIGNAL1;
        public int OUTPUTSIGNAL2;
        public int OUTPUTSIGNAL3;
        public int OUTPUTSIGNAL4;
        public int OUTPUTSIGNAL5;
        public int OUTPUTSIGNAL6;
        public int OUTPUTSIGNAL7;
        public int OUTPUTSIGNAL8;
    }

    public class TagDescriptorHeaterUnit : TagDescriptors
    {
        public int STATUS;
        public int TEMP_LIST;
    }

    public class TagDescriptorHotWireUnit : TagDescriptors
    {
        public int STATUS;
        public int TEMP_LIST;
    }

    public class TagDescriptorIonizer : TagDescriptors
    {
        public int LEVEL_ALARM;
        public int COND_ALARM;
        public int CONTROL_ALARM;
        public int RUN_ALARM; // 09.11.26 minhan
        public int RUN;
    }

    public class TagDescriptorHeater : TagDescriptors
    {
        public int ALARM;
        public int ON;
        public int OFF;
    }

    public class TagDescriptorBuzzer : TagDescriptors
    {
        public int MELODY1;
        public int MELODY2;
        public int MELODY3;
        public int MELODY4;
        public int LAMP;
    }

    public class TagDescriptorPiping : TagDescriptors
    {
        public int RUN;
    }

    public class TagDescriptorPump : TagDescriptorPiping
    {
        public int ALARM;
        public int STOP;
        public int TYPE;
        //public int RUN;
    }

    public class TagDescriptorDryPump : TagDescriptorPiping
    {
        public int ALARM;
        public int STOP;
        public int TYPE;
        public int RUNNING;
        public int WARNING;
        //public int PCWFLOWOK;
        //public int N2FLOWWARNING;
        //public int EXTWARNING;
        //public int RUN;
    }

    public class TagDescriptorTankUnit : TagDescriptors
    {
        public int TANKLEVELNAME;
        public int LEVELS;
        public int TOPLEVEL;
        public int SUPPLYSTOP;
        public int SUPPLYREQUEST;
        public int RUNENABLE;
        public int BOTTOMLEVEL;
    }

    public class TagDescriptorMeasureTank : TagDescriptors
    {
        public int TANKLEVELNAME;
        public int LEVELS;
        public int TOPLEVEL;
        public int SUPPLYSTOP;
        public int SUPPLYREQUEST;
        public int RUNENABLE;
        public int BOTTOMLEVEL;
    }

    public class TagDescriptorDevTankUnit : TagDescriptors
    {
        public int TANKLEVELNAME;
        public int LEVELS;
        public int TOPLEVEL;
        public int SUPPLYSTOP;
        public int SUPPLYREQUEST;
        public int DEVSUPPLYSTOP;
        public int DEVSUPPLYREQUEST;
        public int RUNENABLE;
        public int BOTTOMLEVEL;
    }

    public class TagDescriptorInverter : TagDescriptors
    {
        public int ALARM;
        public int FREQUENCY;
    }

    public class TagDescriptorMotor : TagDescriptors
    {
        public int ALARM;
        public int CPON;
        public int FW;
        public int BW;
        public int CW;
        public int CCW;
        public int STOP;
        public int SPEED;
        public int MAXSPEED;
        public int MINSPEED;
    }

    public class TagDescriptorGauge : TagDescriptors
    {
        public int CURVAL;
        public int CURADC;
    }

    public class TagDescriptorShinkoDryCleaner : TagDescriptors
    {
        public int PREFILTER_ALARM;
        public int HEPAFILTER_ALARM;
        public int INVERTER_ALARM;
        //public int BLOWER1_ALARM;
        //public int BLOWER2_ALARM;
        public int WATERLEAK_ALARM;
        public int TEMP_ALARM;
        public int PRES_ALARM;
        public int EMO_ALARM;
        public int POWERON;
        public int POWEROFF;
        public int RUN;
        public int STOP;
        public int ESTOP;
    }

    public class TagDescriptorHpmj : TagDescriptors
    {
        public int EMO;
        public int POWER;
        public int LEAK;
        public int MCTRIP;
        public int ELBTRIP;
        public int CO2GENPOWER;
        public int INVERTER_FRQ;
    }

    public class TagDescriptorHpmjItem : TagDescriptors
    {
        public int CUR_TIME;
    }

    public class TagDescriptorConiferIDReader : TagDescriptors
    {
        public int VALUE;
    }

    public class TagDescriptorPlasma : TagDescriptors
    {
        public int ALARM;
        public int USE;
        public int ON;
    }

    public class TagDescriptorAlarm : TagDescriptors
    {
        public int ALARM;
    }

    public class TagDescriptorBS7200 : TagDescriptors
    {
        public int VALUE;
    }

    public class TagDescriptorInterfaceStep : TagDescriptors
    {
        public int STEP;
    }
    public class TagDescriptorServoUnit : TagDescriptors
    {
        public int CURPOSNAME;
        public int Id;
        public int refZERO;
        public int LoadRatio;//100617 LeeChungWon
    }
    //2010.06.18 kang add
    public class TagDesciptorCylinerActTime : TagDescriptors
    {
        public int FwActTime;
        public int BwActTime;
    }
    public class TagDesciptorCrackStatus : TagDescriptors
    {
        public int CrackStatus;
    }
    public class TagDescriptorServoMp2300 : TagDescriptors
    {
        public int CURRENTPOS;
        public int DETECT;			//Home Sensor의 감지유무를 나타냄. Dms.Control.Sensor를 사용하기 위해 0번째 위치에, DETECT라는 이름을 사용해야 함.
    }

    public class TagDescriptorShutter : TagDescriptors
    {
        public int Open;
        public int Close;
        public int Push;
        public int Pull;
        public int Up;
        public int Down;
        public int SingleAct;
    }

    public class TagDescriptorMixTank : TagDescriptorTankUnit
    {
        public int SHWMODE;
        public int USEDTIME;
        public int GLSCOUNT;
        public int RECYCLECOUNT;
        public int DRAINCOUNT;
        public int STANDBYTIME;
        public int PREVUSEDTIME;
        public int TANKSTATUS;
        public int SETUPDRAINCOUNT;
        public int SETUPRECYCLECOUNT;
        //public int LEVELS;
        //public int TOPLEVEL;
        //public int SUPPLYSTOP;
        //public int SUPPLYREQUEST;
        //public int RUNENABLE;
        //public int BOTTOMLEVEL;
    }

    public class TagDescriptorApc : TagDescriptors
    {
        public int VALUE;
        public int PRESSURE;
    }

    public class TagDescriptorRfUnit : TagDescriptors
    {
        public int ONSTATE;
        public int SETRFPOWER;
        public int FORWARDPOWER;
        public int REFLECTEDPOWER;
        public int TUNEPOS;
        public int LOADPOS;
    }

    public class TagDescriptorRfg : TagDescriptors
    {
        public int ONSTATE;
        public int OVERTEMP;
        public int SETRFPOWER;
        public int FORWARDPOWER;
        public int REFLECTEDPOWER;
    }

    public class TagDescriptorRfTuner : TagDescriptors
    {
        //public int POWERSENSED;
        //public int POWERTUNED;
        //public int TUNERFAULT;

        //public int DCBIAS;
        public int TUNEPOS;
        public int LOADPOS;

    }
    public class TagDescriptorMfc : TagDescriptors
    {
        public int ON;
        public int OFF;
        public int SETVAL;
        public int CURVAL;
    }

    public class TagDescriptorGasControl : TagDescriptors
    {
        public int OPEN;
        public int CLOSE;
    }

    public class TagDescriptorPmChamber : TagDescriptors
    {
        public int PRESSURE;
        public int TRAYEXIST;
        public int RUNSTATE;
        public int PROCESS;
        public int RFONSTATE;
        public int SHOWERHEADPOSITION;
        public int CHAMBERMESSAGE;
        public int PROCESSPRESSURE;
        public int ISBASEPRESSURE;
        public int ISPROCESSPRESSURE;
        public int PROCESSSTEP;
    }
    public class TagDescriptorPmChamber_HWCVD : TagDescriptors
    {
        public int PRESSURE;
        public int TRAYEXIST;
        public int RUNSTATE;
        public int PROCESS;
        public int TRAYTRANSFERPOSITION;
        public int CHAMBERMESSAGE;
        public int PROCESSPRESSURE;
        public int ISBASEPRESSURE;
        public int ISPROCESSPRESSURE;
    }

    public class TagDescriptorLockUnit : TagDescriptors
    {
        public int PRESSURE;
        public int TRAYEXIST;
        public int RUNSTATE;
        public int PROCESS;
        public int HANDPOSITION;
    }
    public class TagDescriptorLockChamber : TagDescriptors
    {
        public int PRESSURE;
        public int TRAYEXIST;
        public int RUNSTATE;
        public int PROCESS;
        public int CHAMBERMESSAGE;
        public int SHUTTERSTATE;
        public int STOPPERSTATE;
        public int TRAYTRANSFERPOS;
        public int TRAYTRANSFERRELEASE;
    }

    public class TagDescriptorBufferUnit : TagDescriptors
    {
        public int TRAYEXIST;
    }

    public class TagDescriptorHeatExchanger : TagDescriptors
    {
        public int ON;
        public int OFF;
        public int ALARM;
        public int WARNING;

        public int CH1READY;
        public int CH2READY;
        public int CH3READY;

        public int POWERONLED;
        public int ALARMLED;

        public int SETTEMP1;
        public int SETTEMP2;
        public int CURTEMP1;
        public int CURTEMP2;
    }

    public class TagDescriptorIntegratingFlowMeter : TagDescriptors
    {
        public int CURFLOW;
        public int INTEGRALFLOW;
        public int FLOWMETERRESET;
    }
    public class TagDescriptorSafetyRelayUnit : TagDescriptors
    {
        public int SRUACT;
        public int BYPASS;
    }

    public class TagDescriptorLoaderRobot : TagDescriptors
    {
        public int PatternOperationPerform;		// [NX->PLC] Pattern of Motion Reading Complete I/F Bit
        public int InterfereTarget;         // Target Unit for Current Pattern of Operation which is used for UserControl to move
        public int OperationHand;
        public int GetOperation;
        public int PutOperation;
        public int PrepareOperation;
        public int UpHandGlassDetect;
        public int LoHandGlassDetect;
        public int MaxPortNo;
        public int MaxStageNo;
        public int PatternNo;	// [PLC->NX] Pattern of Operation List No.
        public int UpperHandPortNo;
        public int LowerHandPortNo;
        public int UpperHandSlotNo;
        public int LowerHandSlotNo;
    }

    public class TagDescriptorCimUnit : TagDescriptors
    {
        public int EQP_STATE;
        //        public int PROCESS_STATE;
    }

    public class TagDescriptorMotorLoadFactor : TagDescriptors
    {
        public int MOTOR1;
        public int MOTOR2;
        public int MOTOR3;
        public int MOTOR4;
        public int MOTOR5;
        public int MOTOR6;
        public int MOTOR7;
        public int MOTOR8;
        public int MOTOR9;
        public int MOTOR10;
        public int MOTOR11;
        public int MOTOR12;
        public int MOTOR13;
        public int MOTOR14;
        public int MOTOR15;
        public int MOTOR16;
        public int MOTOR17;
        public int MOTOR18;
        public int MOTOR19;
        public int MOTOR20;
    }

    public class TagDescriptorRPS : TagDescriptors
    {
        public int READY;
        public int PLASMAOK;
        public int ACOK;
        public int PLASMAON;
    }
    public class TagDescriptorForcedExhaust : TagDescriptors
    {
        public int MC_ON;
        public int ELB_ON;
        public int RUN;
    }
    public class TagDescriptorAccumulate : TagDescriptors
    {//2010.06.21 kimgun util의 사용 누적량을 표시
        public int CurValue;//현재 누적량
        public int OldValue;//이전 누적량
    }

    public class TagDescriptorPeer : TagDescriptors
    {
        public int PAIRING;
    }

    public class TagDescriptorFlowMeter : TagDescriptorPeer
    {
        public int PV_FLOW;
        public int PV_PRESS;
    }

    public class TagDescriptorSmartDamper : TagDescriptorPeer
    {
        public int PV_MODE;
        public int PV_TGT_PRESSURE;
        public int PV_TGT_HYSTERESIS;
        public int PV_CUR_VALVEANGLE;
        public int PV_CUR_PRESSURE;
        public int PV_ALARMCODE;
        public int SV_MODE;
        public int SV_TGT_PRESSURE;
        public int SV_TGT_HYSTERESIS;
        public int SV_TGT_VALVEANGLE;
    }

    public class TagDescriptorD40A : TagDescriptorPeer
    {
        public int DOOR0_STATE;
        public int DOOR1_STATE;
        public int DOOR2_STATE;
        public int DOOR3_STATE;
        public int DOOR4_STATE;
        public int DOOR5_STATE;
    }

    public class TagDescriptorD4SL : TagDescriptorPeer
    {
        public int DOOR0_STATE;
        public int DOOR1_STATE;
        public int DOOR2_STATE;
        public int DOOR3_STATE;
        public int DOOR4_STATE;
        public int DOOR5_STATE;
        public int DOOR6_STATE;
        public int DOOR7_STATE;
    }

    public class TagDescriptorLFC : TagDescriptorPeer
    {
        public int PV_FLOW;
        public int PV_PRESS;
        public int PV_CUR_OPENRATE;
        public int SV_TGT_OPENRATE;
    }

    public class TagDescriptorManometer : TagDescriptorPeer
    {
        public int PV_EXHAUST;
    }

    public class TagDescriptorLCT : TagDescriptorPeer
    {
        public int PV_LEVEL1;
        public int PV_LEVEL2;
        public int PV_CONSISTENCE;
    }

    public class TagDescriptorESD : TagDescriptors
    {
        public int HIGH;
        public int LO;
        public int GO;
        public int ION_ALM;
        public int SURFACE_POTENTIAL;
    }

    public class TagDescriptorFan : TagDescriptors
    {
        public int CP_ON;
        public int RUN;
    }

    public class TagDescriptorEtcOutput : TagDescriptors
    {
        public int ON;
    }
}
