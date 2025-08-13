using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;
using Dms.Ctl;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class TagEuvIfFlag
    {
        #region Fields
        private bool m_bLampAlarm;
        private bool m_bLampPause;
        private bool m_bLampOnComp;
        private bool m_bLampOnFail;
        private bool m_bEuvResetReq;
        private bool m_bEuvResetComp;
        private bool m_bEuvUtilAlarm;
        private int m_nLampControl;
        private UshioEuvUnit m_Parent;
        #endregion

        #region Properties
        public bool bLampAlarm
        {
            get { return m_bLampAlarm; }
            set { m_bLampAlarm = value; }
        }
        public bool bLampPause
        {
            get { return m_bLampPause; }
            set { m_bLampPause = value; }
        }
        public bool bLampOnComp
        {
            get { return m_bLampOnComp; }
            set { m_bLampOnComp = value; }
        }
        public bool bLampOnFail
        {
            get { return m_bLampOnFail; }
            set { m_bLampOnFail = value; }
        }
        public bool bEuvUtilAlarm
        {
            get { return m_bEuvUtilAlarm; }
            set { m_bEuvUtilAlarm = value; }
        }
        public bool bEuvResetReq
        {
            get { return m_bEuvResetReq; }
            set { m_bEuvResetReq = value; }
        }
        public bool bEuvResetComp
        {
            get { return m_bEuvResetComp; }
            set { m_bEuvResetComp = value; }
        }
        public int nLampControl
        {
            get { return m_nLampControl; }
            set { m_nLampControl = value; }
        }

        #endregion

        #region Constructor
        public TagEuvIfFlag()
        {
        }

        public TagEuvIfFlag(UshioEuvUnit euv)
        {
            m_Parent = euv;
        }
        #endregion

        #region Methods
        public void ClearInfo()
        {
            m_bLampAlarm = false;
            m_bLampOnComp = false;
            m_bLampPause = false;
            m_bLampOnFail = false;
            m_bEuvUtilAlarm = false;
            m_nLampControl = -1;
        }
        #endregion
    };

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class UshioEuvUnit : _EuvUnit
    {
        #region Fields
        protected TagEuvIfFlag m_IfFlag = null;
        protected _GenericCollection<EuvLamp> m_Lamps = new _GenericCollection<EuvLamp>();
        protected CvUnit m_Cv = null;
        private EuvUnitComm m_Comm = null;
        private bool m_IsAlarm;
        protected bool m_HasHumidifier = false;

        #region euv unit auto valve
        protected AutoValve m_N2Valve = null;
        private AutoValve m_PCWInValve = null;
        private AutoValve m_PCWOutValve = null;
        protected AutoValve m_CdaValve = null;
        #endregion

        #region euv unit House up/dn
        protected ActuatorUnit m_ActuatorUnit;
        #endregion

        #region euv interface
        //setup info
        public TagSetupInfo m_SetupEuvUse = null;
        //Alarm List
        public Alarm ALM_PowerNotReady = null;
        public Alarm ALM_EUVlamptimeover = null;
        public Alarm ALM_EUVlamponfail = null;
        public Alarm ALM_EUVprotectivefunc = null;
        public Alarm ALM_EUVelectriccircuiterr = null;
        public Alarm ALM_EUVn2pressover = null;
        public Alarm ALM_EUVlamphouseoverheat = null;
        public Alarm ALM_EUVhouseopen = null;
        public Alarm ALM_EUVtransformeroverheat = null;
        public Alarm ALM_EUVn2pressdrop = null;
        public Alarm ALM_EUVelectriccircuitcoveropen = null;
        public Alarm ALM_EUVcontrolsystemerr = null;
        public Alarm ALM_EUVunitemo = null;
        public Alarm ALM_EUVhumidifiererr = null;
        public Alarm ALM_EUVnotready = null;
        public Alarm ALM_EUVmanual = null;
        public Alarm ALM_HUMIDIFIERnotready = null;
        public Alarm ALM_EUVcommopenerror = null;
        public Alarm ALM_EUVhouseclose = null;
        public Alarm ALM_EUVUNITn2Highlevel = null;
        public Alarm ALM_EUVUNITpcwHighlevel = null;
        public Alarm ALM_EUVUNITcdaHighlevel = null;
        public Alarm ALM_EUVUNITn2lowlevel = null;
        public Alarm ALM_EUVUNITpcwlowlevel = null;
        public Alarm ALM_EUVUNITcdalowlevel = null;
        #endregion

        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public _GenericCollection<EuvLamp> Lamps
        {
            get { return m_Lamps; }
            set { m_Lamps = value; }
        }
        [Category("DMS : Relation")]
        public EuvUnitComm Comm
        {
            get { return m_Comm; }
            set { m_Comm = value; }
        }
        [Category("DMS : Relation")]
        public CvUnit Cv
        {
            get { return m_Cv; }
            set { m_Cv = value; }
        }
        [Category("DMS : Relation")]
        public bool HasHumidifier
        {
            get { return m_HasHumidifier; }
            set { m_HasHumidifier = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupEuvUse
        {
            get { return m_SetupEuvUse; }
            set { m_SetupEuvUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagEuvIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }

        #region Setting
        [Category("Setting")]
        public AutoValve N2Valve
        {
            get { return m_N2Valve; }
            set { m_N2Valve = value; }
        }
        [Category("Setting")]
        public AutoValve PCWInValve
        {
            get { return m_PCWInValve; }
            set { m_PCWInValve = value; }
        }
        [Category("Setting")]
        public AutoValve PCWOutValve
        {
            get { return m_PCWOutValve; }
            set { m_PCWOutValve = value; }
        }
        [Category("Setting")]
        public AutoValve CdaValve
        {
            get { return m_CdaValve; }
            set { m_CdaValve = value; }
        }
        [Category("Setting")]
        public ActuatorUnit ActuatorUnit
        {
            get { return m_ActuatorUnit; }
            set { m_ActuatorUnit = value; }
        }
        #endregion

        #endregion

        #region Constructor
        protected UshioEuvUnit() { }
        #endregion

        #region Methods
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

            log = string.Format("EuvUnit  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        public void Utcontrol(AutoValveAct act)
        {
            m_N2Valve.SetAutoValveAct(act);
            m_CdaValve.SetAutoValveAct(act);
            //m_PCWInValve.SetAutoValveAct(act);
            //m_PCWOutValve.SetAutoValveAct(act);
        }

        public int GetHumStatus()
        {
            int nStatus = 0;
            if (IsHumReady()) nStatus |= (int)humidifierSTATUS.humREADY;
            return nStatus;
        }
        public bool GetHouseCloseState()
        {
            bool close = true;
            close &= m_ActuatorUnit.IsNegative();
            //close &= DiHouseClose.GetState();
            //close &= !m_DiUWLO.GetState();
            return close;
        }
        public bool GetUtState()
        {
            bool bOpen = true;
            bOpen &= m_N2Valve.IsOpen();
            bOpen &= m_CdaValve.IsOpen();
            //bOpen &= m_PCWInValve.IsOpen();
            //bOpen &= m_PCWOutValve.IsOpen();
            return bOpen;
        }

        protected bool IsEuvUse()
        {
            return m_Server.JobCond.EuvUse(this);
        }
        public void LampOnSelect()
        {
            bool bOn = IsEuvUse();
            int count = m_Lamps.Count;
            EuvLamp lamp;
            for (int i = 0; i < count; i++)
            {
                lamp = m_Lamps[i];
                lamp.LampOnSelect(bOn & lamp.IsUse());
            }
        }
        #endregion

        #region Virtuals
        public virtual void EmoReset(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsEmoReset()
        {
            throw new NotImplementedException();
        }
        public virtual void LampUseTimeReset(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsLampUseTimeReset()
        {
            throw new NotImplementedException();
        }

        public virtual void LampEmergency(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsLampEmergency()
        {
            throw new NotImplementedException();
        }
        public virtual bool LampRemote(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsLampRemote()
        {
            throw new NotImplementedException();
        }
        public virtual bool LampLocalLock(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsLampLocalLock()
        {
            throw new NotImplementedException();
        }
        public virtual void LampControl(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual bool HumidifierReset(bool bOn)
        {
            throw new NotImplementedException();
        }
        public virtual void SetInitialCount(bool bInitialCount)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsSetInitialCount()
        {
            throw new NotImplementedException();
        }


        public virtual int GetEuvAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsEuvEMO()
        {
            throw new NotImplementedException();
        }
        public virtual int GetHumAlarm()
        {
            throw new NotImplementedException();
        }
        public virtual bool GetInitialCount()
        {
            throw new NotImplementedException();
        }
        public virtual bool GetLampOnState()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsEuvReady()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsHumReady()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsRemoteMode()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsLevelAlarmExist()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsN2LevelHHSensed()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsN2LevelLLSensed()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(UshioEuvUnit); }
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);

                if (m_Simul.Device)
                {
                    MessageBox.Show(msg);
                }
            }
        }

        public override void UpdateTag()
        {
            if (m_Tag != null)
            {
                m_Tag.SetValue(tagDescriptor.ALARM, m_IsAlarm);
                m_Tag.SetValue(tagDescriptor.USE, IsEuvUse());
            }
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class UshioEuvUnit_Io : UshioEuvUnit
    {
        #region Fields

        #region euv unit level sensor
        //euv unit level sensor
        private IoDigitalInput m_DiN2LevelHH = null;
        private IoDigitalInput m_DiN2LevelLL = null;
        private IoDigitalInput m_DiPCWLevelHH = null;
        private IoDigitalInput m_DiPCWLevelLL = null;
        private IoDigitalInput m_DiCDALevelHH = null;
        private IoDigitalInput m_DiCDALevelLL = null;
        #endregion

        #region euv unit House up/dn
        private IoDigitalInput m_DiHouseClose = new IoDigitalInput();
        #endregion

        #region euv interface
        //CN01
        private IoDigitalOutput m_DoUDLC = new IoDigitalOutput();
        private IoDigitalOutput m_DoUTRE = new IoDigitalOutput();
        private IoDigitalOutput m_DoULON = new IoDigitalOutput();
        private IoDigitalInput m_DiUNRE = new IoDigitalInput();
        private IoDigitalInput m_DiUCRE = new IoDigitalInput();
        private IoDigitalInput m_DiULAC = new IoDigitalInput();
        private IoDigitalInput m_DiURDY = new IoDigitalInput();
        private IoDigitalInput m_DiUWLL = new IoDigitalInput();
        private IoDigitalInput m_DiUEUV = new IoDigitalInput();
        private IoDigitalInput m_DiUESF = new IoDigitalInput();
        private IoDigitalInput m_DiUEEL = new IoDigitalInput();
        //CN02A1
        private IoDigitalOutput m_DoUICR = new IoDigitalOutput();
        private IoDigitalOutput m_DoULST = new IoDigitalOutput();
        //CN02A2
        private IoDigitalInput m_DiUICA = new IoDigitalInput();
        private IoDigitalInput m_DiUFLT = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW1 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW2 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW3 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW4 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW5 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW6 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW7 = new IoDigitalInput();
        private IoDigitalInput m_DiUPOW8 = new IoDigitalInput();
        //CN03
        private IoDigitalOutput m_DoUEMG = new IoDigitalOutput();
        private IoDigitalOutput m_DoUERT = new IoDigitalOutput();
        private IoDigitalInput m_DiUWNH = new IoDigitalInput();
        private IoDigitalInput m_DiUWLT = new IoDigitalInput();
        private IoDigitalInput m_DiUWLO = new IoDigitalInput();
        private IoDigitalInput m_DiUWTT = new IoDigitalInput();
        private IoDigitalInput m_DiUWNL = new IoDigitalInput();
        private IoDigitalInput m_DiUECO = new IoDigitalInput();
        private IoDigitalInput m_DiUOPT = new IoDigitalInput();
        private IoDigitalInput m_DiUCPU = new IoDigitalInput();
        private IoDigitalInput m_DiUEMO = new IoDigitalInput();
        //humidifier
        private IoDigitalInput m_DiUHRY = new IoDigitalInput();
        private IoDigitalInput m_DiUHEM = new IoDigitalInput();
        private IoDigitalOutput m_DoUHER = new IoDigitalOutput();
        #endregion

        #endregion

        #region Properties
        #region Setting
        [Category("Setting")]
        public IoDigitalInput DiN2LevelHH
        {
            get { return m_DiN2LevelHH; }
            set { m_DiN2LevelHH = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiN2LevelLL
        {
            get { return m_DiN2LevelLL; }
            set { m_DiN2LevelLL = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiPCWLevelHH
        {
            get { return m_DiPCWLevelHH; }
            set { m_DiPCWLevelHH = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiPCWLevelLL
        {
            get { return m_DiPCWLevelLL; }
            set { m_DiPCWLevelLL = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiCDALevelHH
        {
            get { return m_DiCDALevelHH; }
            set { m_DiCDALevelHH = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiCDALevelLL
        {
            get { return m_DiCDALevelLL; }
            set { m_DiCDALevelLL = value; }
        }
        //[Category("Setting")]
        //public IoDigitalInput DiHouseClose
        //{
        //    get { return m_DiHouseClose; }
        //    set { m_DiHouseClose = value; }
        //}
        [Category("Setting")]
        public IoDigitalInput DiUNRE
        {
            get { return m_DiUNRE; }
            set { m_DiUNRE = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUCRE
        {
            get { return m_DiUCRE; }
            set { m_DiUCRE = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiULAC
        {
            get { return m_DiULAC; }
            set { m_DiULAC = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiURDY
        {
            get { return m_DiURDY; }
            set { m_DiURDY = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUWLL
        {
            get { return m_DiUWLL; }
            set { m_DiUWLL = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUEUV
        {
            get { return m_DiUEUV; }
            set { m_DiUEUV = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUESF
        {
            get { return m_DiUESF; }
            set { m_DiUESF = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUEEL
        {
            get { return m_DiUEEL; }
            set { m_DiUEEL = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUICA
        {
            get { return m_DiUICA; }
            set { m_DiUICA = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUFLT
        {
            get { return m_DiUFLT; }
            set { m_DiUFLT = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW1
        {
            get { return m_DiUPOW1; }
            set { m_DiUPOW1 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW2
        {
            get { return m_DiUPOW2; }
            set { m_DiUPOW2 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW3
        {
            get { return m_DiUPOW3; }
            set { m_DiUPOW3 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW4
        {
            get { return m_DiUPOW4; }
            set { m_DiUPOW4 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW5
        {
            get { return m_DiUPOW5; }
            set { m_DiUPOW5 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW6
        {
            get { return m_DiUPOW6; }
            set { m_DiUPOW6 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW7
        {
            get { return m_DiUPOW7; }
            set { m_DiUPOW7 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUPOW8
        {
            get { return m_DiUPOW8; }
            set { m_DiUPOW8 = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUWNH
        {
            get { return m_DiUWNH; }
            set { m_DiUWNH = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUWLT
        {
            get { return m_DiUWLT; }
            set { m_DiUWLT = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUWLO
        {
            get { return m_DiUWLO; }
            set { m_DiUWLO = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUWTT
        {
            get { return m_DiUWTT; }
            set { m_DiUWTT = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUWNL
        {
            get { return m_DiUWNL; }
            set { m_DiUWNL = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUECO
        {
            get { return m_DiUECO; }
            set { m_DiUECO = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUOPT
        {
            get { return m_DiUOPT; }
            set { m_DiUOPT = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUCPU
        {
            get { return m_DiUCPU; }
            set { m_DiUCPU = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUEMO
        {
            get { return m_DiUEMO; }
            set { m_DiUEMO = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUHRY
        {
            get { return m_DiUHRY; }
            set { m_DiUHRY = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiUHEM
        {
            get { return m_DiUHEM; }
            set { m_DiUHEM = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoUDLC
        {
            get { return m_DoUDLC; }
            set { m_DoUDLC = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoUTRE
        {
            get { return m_DoUTRE; }
            set { m_DoUTRE = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoULON
        {
            get { return m_DoULON; }
            set { m_DoULON = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoUICR
        {
            get { return m_DoUICR; }
            set { m_DoUICR = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoULST
        {
            get { return m_DoULST; }
            set { m_DoULST = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoUEMG
        {
            get { return m_DoUEMG; }
            set { m_DoUEMG = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoUERT
        {
            get { return m_DoUERT; }
            set { m_DoUERT = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoUHER
        {
            get { return m_DoUHER; }
            set { m_DoUHER = value; }
        }
        #endregion
        #endregion

        #region Constructor
        public UshioEuvUnit_Io()
        {
            this.Name = "Ushio EUV Unit __";
        }
        #endregion

        #region Methods
        #endregion

        #region UshioEuvUnit Overrides
        public override void EmoReset(bool bOn)
        {
            m_DoUERT.SetState(bOn);
        }
        public override bool IsEmoReset()
        {
            return m_DoUERT.GetState();
        }
        public override void LampUseTimeReset(bool bOn)
        {
            m_DoULST.SetState(bOn);
        }
        public override bool IsLampUseTimeReset()
        {
            return m_DoULST.GetState();
        }

        public override void LampEmergency(bool bOn)
        {
            m_DoUEMG.SetState(bOn);
        }
        public override bool IsLampEmergency()
        {
            return m_DoUEMG.GetState();
        }
        public override bool LampRemote(bool bOn)
        {
            if (bOn)
            {
                if (m_DiUCRE.GetState()) m_DoUTRE.SetState(bOn);
                else return false;
            }
            else m_DoUTRE.SetState(bOn);
            return true;
        }
        public override bool IsLampRemote()
        {
            return m_DoUTRE.GetState();
        }
        public override bool LampLocalLock(bool bOn)
        {
            m_DoUDLC.SetState(bOn);
            return true;
        }
        public override bool IsLampLocalLock()
        {
            return m_DoUDLC.GetState();
        }
        public override void LampControl(bool bOn)
        {
            m_DoULON.SetState(bOn);
            foreach (EuvLamp LampUnit in m_Lamps)
            {
                if (bOn) LampUnit.On(); //view용
                else LampUnit.Off();    //view용
            }
        }
        public override bool HumidifierReset(bool bOn)
        {
            m_DoUHER.SetState(bOn);
            return true;
        }
        public override void SetInitialCount(bool bInitialCount)
        {
            m_DoUICR.SetState(bInitialCount);
        }
        public override bool IsSetInitialCount()
        {
            return m_DoUICR.GetState();
        }

        public override int GetEuvAlarm()
        {
            euvalarmIndex nRv = 0;

            if (m_DiUWLL.GetState()) nRv |= euvalarmIndex.euvAlmLampUseTimeExcess;		// Lamp Used Time Over
            if (m_DiUEUV.GetState()) nRv |= euvalarmIndex.euvAlmLampOnFail;			    // Lamp On Fail
            if (m_DiUESF.GetState()) nRv |= euvalarmIndex.euvAlmProtectiveFunction;	    // Protective Function Appearance
            if (m_DiUEEL.GetState()) nRv |= euvalarmIndex.euvAlmElectricCircuit;		// Electric Circuit Error
            if (m_DiUWLT.GetState()) nRv |= euvalarmIndex.euvAlmLampHouseTemp;			// Lamp House Temperature Error
            if (m_DiUWLO.GetState()) nRv |= euvalarmIndex.euvAlmLampHouseOpen;			// Lamp House Open
            if (m_DiUWTT.GetState()) nRv |= euvalarmIndex.euvAlmTransformerTemp;		// Step-up Transformer Temperature Error
            if (m_DiUWNL.GetState()) nRv |= euvalarmIndex.euvAlmN2PressDrop;			// Lamp House N2 Pressure Drop
            if (m_DiUECO.GetState()) nRv |= euvalarmIndex.euvAlmElectricCompCover;		// Electric Component Cover Open 
            if (m_DiUCPU.GetState()) nRv |= euvalarmIndex.euvAlmControlSystem;			// Control System Error
            if (m_DiUEMO.GetState()) nRv |= euvalarmIndex.euvAlmEMO;                    // EMO

            return (int)nRv;
        }
        public override bool IsEuvEMO()
        {
            return m_DiUEMO.GetState();
        }
        public override int GetHumAlarm()
        {
            int nRv = 0;
            if (m_HasHumidifier)
            {
                if (m_DiUHEM.GetState()) nRv |= (int)humidifierSTATUS.humABNORMAL;
            }
            return nRv;
        }
        public override bool GetInitialCount()
        {
            if (IsEuvUse())
            {
                return m_DiUICA.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool GetLampOnState()
        {
            bool bOn;
            bOn = m_DoULON.GetState();
            return bOn;
        }

        public override bool IsEuvReady()
        {
            bool bReady = false;
            if (!IsEuvUse())
            {
                bReady = true;
            }
            else
            {
                bReady = m_DiURDY.GetState();
            }
            return bReady;
        }
        public override bool IsHumReady()
        {
            bool bReady = false;
            if (!IsEuvUse() || !m_HasHumidifier)
            {
                bReady = true;
            }
            else
            {
                bReady = m_DiUHRY.GetState();
            }
            return bReady;
        }
        public override bool IsRemoteMode()
        {
            bool bMode;
            bMode = (m_DiUNRE.GetState());
            return bMode;
        }

        public override bool IsLevelAlarmExist()
        {
            bool bErr = false;
            bErr |= m_DiN2LevelHH.GetState();
            bErr |= m_DiN2LevelLL.GetState();
            bErr |= m_DiPCWLevelHH.GetState();
            bErr |= m_DiPCWLevelLL.GetState();
            bErr |= m_DiCDALevelHH.GetState();
            bErr |= m_DiCDALevelLL.GetState();
            return bErr;
        }
        public override bool IsN2LevelHHSensed()
        {
            return m_DiN2LevelHH.GetState();
        }
        public override bool IsN2LevelLLSensed()
        {
            return m_DiN2LevelLL.GetState();
        }
        #endregion

        #region Override

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
            ok &= (m_DoUDLC != null);
            ok &= (m_DoUTRE != null);
            ok &= (m_DoULON != null);
            ok &= (m_DiUNRE != null);
            ok &= (m_DiUCRE != null);
            ok &= (m_DiULAC != null);
            ok &= (m_DiURDY != null);
            ok &= (m_DiUWLL != null);
            ok &= (m_DiUEUV != null);
            ok &= (m_DiUESF != null);
            ok &= (m_DiUEEL != null);
            ok &= (m_DoUICR != null);
            ok &= (m_DoULST != null);
            ok &= (m_DiUICA != null);
            ok &= (m_DiUFLT != null);
            ok &= (m_DoUEMG != null);
            ok &= (m_DoUERT != null);
            ok &= (m_DiUWNH != null);
            ok &= (m_DiUWLT != null);
            ok &= (m_DiUWLO != null);
            ok &= (m_DiUWTT != null);
            ok &= (m_DiUWNL != null);
            ok &= (m_DiUECO != null);
            ok &= (m_DiUOPT != null);
            ok &= (m_DiUCPU != null);
            ok &= (m_DiUEMO != null);

            if (m_HasHumidifier)
            {
                ok &= (m_DiUHRY != null);
                ok &= (m_DiUHEM != null);
                ok &= (m_DoUHER != null);
            }

            //ok &= (m_DiHouseClose != null);
            ok &= (m_Cv != null);
            ok &= (m_ActuatorUnit != null);
            ok &= (m_N2Valve != null);
            ok &= (m_CdaValve != null);



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
                ALM_EUVlamptimeover = new Alarm(this.Name + " Lamp Used Time Over Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVlamponfail = new Alarm(this.Name + " Lamp On Fail Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVprotectivefunc = new Alarm(this.Name + " Protective Function Appearance Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVelectriccircuiterr = new Alarm(this.Name + " Electric Circuit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVn2pressover = new Alarm(this.Name + " N2 Pressure Over Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVlamphouseoverheat = new Alarm(this.Name + " Lamp House Overheat Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVhouseopen = new Alarm(this.Name + " Lamp House Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVtransformeroverheat = new Alarm(this.Name + " Step-up Transformer Overheat Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVn2pressdrop = new Alarm(this.Name + " N2 Pressure Drop Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVelectriccircuitcoveropen = new Alarm(this.Name + " Electric Circuit Cover Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVcontrolsystemerr = new Alarm(this.Name + " Control System Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVunitemo = new Alarm(this.Name + " EMO", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVhumidifiererr = new Alarm(this.Name + " Humidifier Abnormal State Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVnotready = new Alarm(this.Name + " Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVmanual = new Alarm(this.Name + " Local Mode Alarm ", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HUMIDIFIERnotready = new Alarm(this.Name + " Humidifier Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVcommopenerror = new Alarm(this.Name + " COM Port Open Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVhouseclose = new Alarm(this.Name + " House not closed Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITn2Highlevel = new Alarm(this.Name + " N2 high Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITpcwHighlevel = new Alarm(this.Name + " PCW high Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITcdaHighlevel = new Alarm(this.Name + " CDA high Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITn2lowlevel = new Alarm(this.Name + " N2 Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITpcwlowlevel = new Alarm(this.Name + " PCW Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITcdalowlevel = new Alarm(this.Name + " CDA Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupEuvUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                setupGenInfoProvider.InitFromDB(this.m_SetupEuvUse);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagEuvIfFlag(this);
                m_IfFlag.ClearInfo();

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
                    m_ActuatorUnit.SetNegativeAct();
                    //m_DiHouseClose.SetState(true);
                    m_DiUNRE.SetState(true);
                    m_DiURDY.SetState(true);
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

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class UshioEuvUnit_Ec : UshioEuvUnit
    {
        #region Fields

        #region euv unit level sensor
        //euv unit level sensor
        private SlaveDigitalInput m_DiN2LevelHH = null;
        private SlaveDigitalInput m_DiN2LevelLL = null;
        private SlaveDigitalInput m_DiPCWLevelHH = null;
        private SlaveDigitalInput m_DiPCWLevelLL = null;
        private SlaveDigitalInput m_DiCDALevelHH = null;
        private SlaveDigitalInput m_DiCDALevelLL = null;
        #endregion

        #region euv unit House up/dn
        private SlaveDigitalInput m_DiHouseClose = new SlaveDigitalInput();
        #endregion

        #region euv interface
        //CN01
        private SlaveDigitalOutput m_DoUDLC = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoUTRE = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoULON = new SlaveDigitalOutput();
        private SlaveDigitalInput m_DiUNRE = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUCRE = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiULAC = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiURDY = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUWLL = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUEUV = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUESF = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUEEL = new SlaveDigitalInput();
        //CN02A1
        private SlaveDigitalOutput m_DoUICR = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoULST = new SlaveDigitalOutput();
        //CN02A2
        private SlaveDigitalInput m_DiUICA = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUFLT = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW1 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW2 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW3 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW4 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW5 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW6 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW7 = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUPOW8 = new SlaveDigitalInput();
        //CN03
        private SlaveDigitalOutput m_DoUEMG = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoUERT = new SlaveDigitalOutput();
        private SlaveDigitalInput m_DiUWNH = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUWLT = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUWLO = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUWTT = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUWNL = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUECO = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUOPT = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUCPU = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUEMO = new SlaveDigitalInput();
        //humidifier
        private SlaveDigitalInput m_DiUHRY = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiUHEM = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoUHER = new SlaveDigitalOutput();
        #endregion

        #endregion

        #region Properties
        #region Setting
        [Category("Setting")]
        public SlaveDigitalInput DiN2LevelHH
        {
            get { return m_DiN2LevelHH; }
            set { m_DiN2LevelHH = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiN2LevelLL
        {
            get { return m_DiN2LevelLL; }
            set { m_DiN2LevelLL = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiPCWLevelHH
        {
            get { return m_DiPCWLevelHH; }
            set { m_DiPCWLevelHH = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiPCWLevelLL
        {
            get { return m_DiPCWLevelLL; }
            set { m_DiPCWLevelLL = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiCDALevelHH
        {
            get { return m_DiCDALevelHH; }
            set { m_DiCDALevelHH = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiCDALevelLL
        {
            get { return m_DiCDALevelLL; }
            set { m_DiCDALevelLL = value; }
        }
        //[Category("Setting")]
        //public IoDigitalInput DiHouseClose
        //{
        //    get { return m_DiHouseClose; }
        //    set { m_DiHouseClose = value; }
        //}
        [Category("Setting")]
        public SlaveDigitalInput DiUNRE
        {
            get { return m_DiUNRE; }
            set { m_DiUNRE = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUCRE
        {
            get { return m_DiUCRE; }
            set { m_DiUCRE = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiULAC
        {
            get { return m_DiULAC; }
            set { m_DiULAC = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiURDY
        {
            get { return m_DiURDY; }
            set { m_DiURDY = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUWLL
        {
            get { return m_DiUWLL; }
            set { m_DiUWLL = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUEUV
        {
            get { return m_DiUEUV; }
            set { m_DiUEUV = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUESF
        {
            get { return m_DiUESF; }
            set { m_DiUESF = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUEEL
        {
            get { return m_DiUEEL; }
            set { m_DiUEEL = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUICA
        {
            get { return m_DiUICA; }
            set { m_DiUICA = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUFLT
        {
            get { return m_DiUFLT; }
            set { m_DiUFLT = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW1
        {
            get { return m_DiUPOW1; }
            set { m_DiUPOW1 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW2
        {
            get { return m_DiUPOW2; }
            set { m_DiUPOW2 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW3
        {
            get { return m_DiUPOW3; }
            set { m_DiUPOW3 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW4
        {
            get { return m_DiUPOW4; }
            set { m_DiUPOW4 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW5
        {
            get { return m_DiUPOW5; }
            set { m_DiUPOW5 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW6
        {
            get { return m_DiUPOW6; }
            set { m_DiUPOW6 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW7
        {
            get { return m_DiUPOW7; }
            set { m_DiUPOW7 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUPOW8
        {
            get { return m_DiUPOW8; }
            set { m_DiUPOW8 = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUWNH
        {
            get { return m_DiUWNH; }
            set { m_DiUWNH = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUWLT
        {
            get { return m_DiUWLT; }
            set { m_DiUWLT = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUWLO
        {
            get { return m_DiUWLO; }
            set { m_DiUWLO = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUWTT
        {
            get { return m_DiUWTT; }
            set { m_DiUWTT = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUWNL
        {
            get { return m_DiUWNL; }
            set { m_DiUWNL = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUECO
        {
            get { return m_DiUECO; }
            set { m_DiUECO = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUOPT
        {
            get { return m_DiUOPT; }
            set { m_DiUOPT = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUCPU
        {
            get { return m_DiUCPU; }
            set { m_DiUCPU = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUEMO
        {
            get { return m_DiUEMO; }
            set { m_DiUEMO = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUHRY
        {
            get { return m_DiUHRY; }
            set { m_DiUHRY = value; }
        }
        [Category("Setting")]
        public SlaveDigitalInput DiUHEM
        {
            get { return m_DiUHEM; }
            set { m_DiUHEM = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoUDLC
        {
            get { return m_DoUDLC; }
            set { m_DoUDLC = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoUTRE
        {
            get { return m_DoUTRE; }
            set { m_DoUTRE = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoULON
        {
            get { return m_DoULON; }
            set { m_DoULON = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoUICR
        {
            get { return m_DoUICR; }
            set { m_DoUICR = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoULST
        {
            get { return m_DoULST; }
            set { m_DoULST = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoUEMG
        {
            get { return m_DoUEMG; }
            set { m_DoUEMG = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoUERT
        {
            get { return m_DoUERT; }
            set { m_DoUERT = value; }
        }
        [Category("Setting")]
        public SlaveDigitalOutput DoUHER
        {
            get { return m_DoUHER; }
            set { m_DoUHER = value; }
        }
        #endregion
        #endregion

        #region Constructor
        public UshioEuvUnit_Ec()
        {
            this.Name = "Ushio EUV Unit __";
        }
        #endregion

        #region Methods
        #endregion

        #region UshioEuvUnit Overrides
        public override void EmoReset(bool bOn)
        {
            m_DoUERT.SetState(bOn);
        }
        public override bool IsEmoReset()
        {
            return m_DoUERT.GetState();
        }
        public override void LampUseTimeReset(bool bOn)
        {
            m_DoULST.SetState(bOn);
        }
        public override bool IsLampUseTimeReset()
        {
            return m_DoULST.GetState();
        }

        public override void LampEmergency(bool bOn)
        {
            m_DoUEMG.SetState(bOn);
        }
        public override bool IsLampEmergency()
        {
            return m_DoUEMG.GetState();
        }
        public override bool LampRemote(bool bOn)
        {
            if (bOn)
            {
                if (m_DiUCRE.GetState()) m_DoUTRE.SetState(bOn);
                else return false;
            }
            else m_DoUTRE.SetState(bOn);
            return true;
        }
        public override bool IsLampRemote()
        {
            return m_DoUTRE.GetState();
        }
        public override bool LampLocalLock(bool bOn)
        {
            m_DoUDLC.SetState(bOn);
            return true;
        }
        public override bool IsLampLocalLock()
        {
            return m_DoUDLC.GetState();
        }
        public override void LampControl(bool bOn)
        {
            m_DoULON.SetState(bOn);
            foreach (EuvLamp LampUnit in m_Lamps)
            {
                if (bOn) LampUnit.On(); //view용
                else LampUnit.Off();    //view용
            }
        }
        public override bool HumidifierReset(bool bOn)
        {
            m_DoUHER.SetState(bOn);
            return true;
        }
        public override void SetInitialCount(bool bInitialCount)
        {
            m_DoUICR.SetState(bInitialCount);
        }
        public override bool IsSetInitialCount()
        {
            return m_DoUICR.GetState();
        }

        public override int GetEuvAlarm()
        {
            euvalarmIndex nRv = 0;

            if (m_DiUWLL.GetState()) nRv |= euvalarmIndex.euvAlmLampUseTimeExcess;		// Lamp Used Time Over
            if (m_DiUEUV.GetState()) nRv |= euvalarmIndex.euvAlmLampOnFail;			    // Lamp On Fail
            if (m_DiUESF.GetState()) nRv |= euvalarmIndex.euvAlmProtectiveFunction;	    // Protective Function Appearance
            if (m_DiUEEL.GetState()) nRv |= euvalarmIndex.euvAlmElectricCircuit;		// Electric Circuit Error
            if (m_DiUWLT.GetState()) nRv |= euvalarmIndex.euvAlmLampHouseTemp;			// Lamp House Temperature Error
            if (m_DiUWLO.GetState()) nRv |= euvalarmIndex.euvAlmLampHouseOpen;			// Lamp House Open
            if (m_DiUWTT.GetState()) nRv |= euvalarmIndex.euvAlmTransformerTemp;		// Step-up Transformer Temperature Error
            if (m_DiUWNL.GetState()) nRv |= euvalarmIndex.euvAlmN2PressDrop;			// Lamp House N2 Pressure Drop
            if (m_DiUECO.GetState()) nRv |= euvalarmIndex.euvAlmElectricCompCover;		// Electric Component Cover Open 
            if (m_DiUCPU.GetState()) nRv |= euvalarmIndex.euvAlmControlSystem;			// Control System Error
            if (m_DiUEMO.GetState()) nRv |= euvalarmIndex.euvAlmEMO;                    // EMO

            return (int)nRv;
        }
        public override bool IsEuvEMO()
        {
            return m_DiUEMO.GetState();
        }
        public override int GetHumAlarm()
        {
            int nRv = 0;
            if (m_HasHumidifier)
            {
                if (m_DiUHEM.GetState()) nRv |= (int)humidifierSTATUS.humABNORMAL;
            }
            return nRv;
        }
        public override bool GetInitialCount()
        {
            if (IsEuvUse())
            {
                return m_DiUICA.GetState();
            }
            else
            {
                return false;
            }
        }
        public override bool GetLampOnState()
        {
            bool bOn;
            bOn = m_DoULON.GetState();
            return bOn;
        }

        public override bool IsEuvReady()
        {
            bool bReady = false;
            if (!IsEuvUse())
            {
                bReady = true;
            }
            else
            {
                bReady = m_DiURDY.GetState();
            }
            return bReady;
        }
        public override bool IsHumReady()
        {
            bool bReady = false;
            if (!IsEuvUse() || !m_HasHumidifier)
            {
                bReady = true;
            }
            else
            {
                bReady = m_DiUHRY.GetState();
            }
            return bReady;
        }
        public override bool IsRemoteMode()
        {
            bool bMode;
            bMode = (m_DiUNRE.GetState());
            return bMode;
        }

        public override bool IsLevelAlarmExist()
        {
            bool bErr = false;
            bErr |= m_DiN2LevelHH.GetState();
            bErr |= m_DiN2LevelLL.GetState();
            bErr |= m_DiPCWLevelHH.GetState();
            bErr |= m_DiPCWLevelLL.GetState();
            bErr |= m_DiCDALevelHH.GetState();
            bErr |= m_DiCDALevelLL.GetState();
            return bErr;
        }
        public override bool IsN2LevelHHSensed()
        {
            return m_DiN2LevelHH.GetState();
        }
        public override bool IsN2LevelLLSensed()
        {
            return m_DiN2LevelLL.GetState();
        }
        #endregion

        #region Override
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
            ok &= (m_DoUDLC != null);
            ok &= (m_DoUTRE != null);
            ok &= (m_DoULON != null);
            ok &= (m_DiUNRE != null);
            ok &= (m_DiUCRE != null);
            ok &= (m_DiULAC != null);
            ok &= (m_DiURDY != null);
            ok &= (m_DiUWLL != null);
            ok &= (m_DiUEUV != null);
            ok &= (m_DiUESF != null);
            ok &= (m_DiUEEL != null);
            ok &= (m_DoUICR != null);
            ok &= (m_DoULST != null);
            ok &= (m_DiUICA != null);
            ok &= (m_DiUFLT != null);
            ok &= (m_DoUEMG != null);
            ok &= (m_DoUERT != null);
            ok &= (m_DiUWNH != null);
            ok &= (m_DiUWLT != null);
            ok &= (m_DiUWLO != null);
            ok &= (m_DiUWTT != null);
            ok &= (m_DiUWNL != null);
            ok &= (m_DiUECO != null);
            ok &= (m_DiUOPT != null);
            ok &= (m_DiUCPU != null);
            ok &= (m_DiUEMO != null);

            if (m_HasHumidifier)
            {
                ok &= (m_DiUHRY != null);
                ok &= (m_DiUHEM != null);
                ok &= (m_DoUHER != null);
            }

            //ok &= (m_DiHouseClose != null);
            ok &= (m_Cv != null);
            ok &= (m_ActuatorUnit != null);
            ok &= (m_N2Valve != null);
            ok &= (m_CdaValve != null);



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
                ALM_EUVlamptimeover = new Alarm(this.Name + " Lamp Used Time Over Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVlamponfail = new Alarm(this.Name + " Lamp On Fail Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVprotectivefunc = new Alarm(this.Name + " Protective Function Appearance Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVelectriccircuiterr = new Alarm(this.Name + " Electric Circuit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVn2pressover = new Alarm(this.Name + " N2 Pressure Over Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVlamphouseoverheat = new Alarm(this.Name + " Lamp House Overheat Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVhouseopen = new Alarm(this.Name + " Lamp House Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVtransformeroverheat = new Alarm(this.Name + " Step-up Transformer Overheat Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVn2pressdrop = new Alarm(this.Name + " N2 Pressure Drop Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVelectriccircuitcoveropen = new Alarm(this.Name + " Electric Circuit Cover Open Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVcontrolsystemerr = new Alarm(this.Name + " Control System Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVunitemo = new Alarm(this.Name + " EMO", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVhumidifiererr = new Alarm(this.Name + " Humidifier Abnormal State Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVnotready = new Alarm(this.Name + " Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVmanual = new Alarm(this.Name + " Local Mode Alarm ", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HUMIDIFIERnotready = new Alarm(this.Name + " Humidifier Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVcommopenerror = new Alarm(this.Name + " COM Port Open Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVhouseclose = new Alarm(this.Name + " House not closed Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITn2Highlevel = new Alarm(this.Name + " N2 high Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITpcwHighlevel = new Alarm(this.Name + " PCW high Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITcdaHighlevel = new Alarm(this.Name + " CDA high Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITn2lowlevel = new Alarm(this.Name + " N2 Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITpcwlowlevel = new Alarm(this.Name + " PCW Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVUNITcdalowlevel = new Alarm(this.Name + " CDA Low Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupEuvUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                setupGenInfoProvider.InitFromDB(this.m_SetupEuvUse);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagEuvIfFlag(this);
                m_IfFlag.ClearInfo();

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
                    m_ActuatorUnit.SetNegativeAct();
                    //m_DiHouseClose.SetState(true);
                    m_DiUNRE.SetState(true);
                    m_DiURDY.SetState(true);
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
