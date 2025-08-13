///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.14
// Author       : jemoon
// Description  : Absodex DD Motor Class
///////////////////////////////////////////////////////////////////////////
// * Revision History
//

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Windows.Forms;

namespace Dms.Device
{
    public enum AnswerMode
    {
        NoUse,
        MoveConfirm,
        MCode,
        Both
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AbsodexMotor : _ActuatorTurn
    {
        #region Fields
        private IoDigitalInput m_DiMCode0 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode1 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode2 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode3 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode4 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode5 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode6 = new IoDigitalInput();
        private IoDigitalInput m_DiMCode7 = new IoDigitalInput();
        private IoDigitalInput m_DiMoveConfirm = new IoDigitalInput();
        private IoDigitalInput m_DiReady = new IoDigitalInput();
        private IoDigitalInput m_DiAlarmCode1 = new IoDigitalInput();   //B Contact
        private IoDigitalInput m_DiAlarmCode2 = new IoDigitalInput();   //B Contact
        private IoDigitalOutput m_DoPosSelect0 = new IoDigitalOutput();
        private IoDigitalOutput m_DoPosSelect1 = new IoDigitalOutput();
        private IoDigitalOutput m_DoPosSelect2 = new IoDigitalOutput();
        private IoDigitalOutput m_DoPosSelect3 = new IoDigitalOutput();
        private IoDigitalOutput m_DoPosSelectSet = new IoDigitalOutput();
        private IoDigitalOutput m_DoMove = new IoDigitalOutput();
        private IoDigitalOutput m_DoStop = new IoDigitalOutput();
        private IoDigitalOutput m_DoAnswer = new IoDigitalOutput();
        private IoDigitalOutput m_DoEStop = new IoDigitalOutput();      // BContact
        private IoDigitalOutput m_DoReset = new IoDigitalOutput();
        private IoCollection<IoDigitalInput> m_DiMCodes = null;
        private IoCollection<IoDigitalOutput> m_DoPosSelects = null;
        private _GenericCollection<Sensor> m_PositionSensors = new _GenericCollection<Sensor>();
        protected bool m_UsePositionSensor = false;
        protected AnswerMode m_AnswerMode = AnswerMode.NoUse;
        public Alarm ALM_ReadyFail = null;
        public Alarm ALM_MoveFail = null;
        public Alarm ALM_AlarmResetFail = null;
        protected XSeqFunction m_SeqMove = null;
        protected XSeqFunction m_SeqReset = null;
        #endregion

        #region Properties
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode0
        {
            get { return m_DiMCode0; }
            set { m_DiMCode0 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode1
        {
            get { return m_DiMCode1; }
            set { m_DiMCode1 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode2
        {
            get { return m_DiMCode2; }
            set { m_DiMCode2 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode3
        {
            get { return m_DiMCode3; }
            set { m_DiMCode3 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode4
        {
            get { return m_DiMCode4; }
            set { m_DiMCode4 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode5
        {
            get { return m_DiMCode5; }
            set { m_DiMCode5 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode6
        {
            get { return m_DiMCode6; }
            set { m_DiMCode6 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMCode7
        {
            get { return m_DiMCode7; }
            set { m_DiMCode7 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiMoveConfirm
        {
            get { return m_DiMoveConfirm; }
            set { m_DiMoveConfirm = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiReady
        {
            get { return m_DiReady; }
            set { m_DiReady = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiAlarmCode1
        {
            get { return m_DiAlarmCode1; }
            set { m_DiAlarmCode1 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalInput DiAlarmCode2
        {
            get { return m_DiAlarmCode2; }
            set { m_DiAlarmCode2 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoPosSelect0
        {
            get { return m_DoPosSelect0; }
            set { m_DoPosSelect0 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoPosSelect1
        {
            get { return m_DoPosSelect1; }
            set { m_DoPosSelect1 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoPosSelect2
        {
            get { return m_DoPosSelect2; }
            set { m_DoPosSelect2 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoPosSelect3
        {
            get { return m_DoPosSelect3; }
            set { m_DoPosSelect3 = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoPosSelectSet
        {
            get { return m_DoPosSelectSet; }
            set { m_DoPosSelectSet = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoMove
        {
            get { return m_DoMove; }
            set { m_DoMove = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoStop
        {
            get { return m_DoStop; }
            set { m_DoStop = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoEStop
        {
            get { return m_DoEStop; }
            set { m_DoEStop = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoAnswer
        {
            get { return m_DoAnswer; }
            set { m_DoAnswer = value; }
        }
        [Category("DMS : Setting - I/F")]
        public IoDigitalOutput DoReset
        {
            get { return m_DoReset; }
            set { m_DoReset = value; }
        }
        [Category("DMS : Setting - Setup")]
        public AnswerMode AnswerMode
        {
            get { return m_AnswerMode; }
            set { m_AnswerMode = value; }
        }
        [Category("DMS : Setting - Position")]
        public bool UsePositionSensor
        {
            get { return m_UsePositionSensor; }
            set { m_UsePositionSensor = value; }
        }
        [Category("DMS : Setting - Position")]
        public _GenericCollection<Sensor> PositionSensors
        {
            get { return m_PositionSensors; }
            set { m_PositionSensors = value; }
        }
        #endregion

        #region Constructor
        public AbsodexMotor()
        {
            this.Name = "__ Turn Motor";
        }
        #endregion

        #region Methods
        public bool IsReady()
        {
            return m_DiReady.GetState();
        }
        public void PosSelect(int pointId)
        {
            //jemoon : AbsodexMotor program Number는 pointId + 1
            int programNo = pointId + 1;
            for (int i = 0; i < 4; i++)
            {
                bool flag = ((programNo >> i) & 0x01) > 0;
                m_DoPosSelects[i].SetState(flag);
            }
        }
        public void PosSelectSet()
        {
            m_DoPosSelectSet.SetPulse(true, 100);
        }
        public void Move()
        {
            m_DoMove.SetPulse(true, 100);
        }
        public void Answer()
        {
            m_DoAnswer.SetPulse(true, 100);
        }
        public void AlarmReset()
        {
            m_DoReset.SetPulse(true, 100);
        }
        public void EStop()
        {
            m_DoEStop.SetPulse(false, 100);
        }
        public void Stop()
        {
            m_DoStop.SetPulse(true, 100);
        }
        public bool IsConfirm()
        {
            bool ok = m_DiMoveConfirm.GetState();
            return ok;
        }

        public int MotorAct()
        {
            int result = -1;

            // 이미수행완료
            if (m_CurAct == m_RefAct) return 0;

            switch (m_RefAct)
            {
                case ActuatorAct.EStop:
                    {
                        EStop();
                        result = 0;

                        m_SeqMove.InitSeq();
                        m_SeqReset.InitSeq();
                    }
                    break;
                case ActuatorAct.AlarmReset:
                    {
                        result = m_SeqReset.Do();

                        m_SeqMove.InitSeq();
                    }
                    break;
                case ActuatorAct.Stop:
                    {
                        Stop();
                        result = 0;

                        m_SeqMove.InitSeq();
                        m_SeqReset.InitSeq();
                    }
                    break;
                case ActuatorAct.Pos:
                    {
                        result = m_SeqMove.Do();
                    }
                    break;
            }

            // 명령수행 완료
            if (result == 0)
            {
                m_CurAct = m_RefAct;

                UpdateTag();
            }

            return result;
        }

        public int GetMcode()
        {
            int mcode = 0;
            for (int i = 0; i < 8; i++)
            {
                mcode |= ((m_DiMCodes[i].GetState() ? 1 : 0) << i);
            }

            return mcode;
        }

        //jemoon : Only for simulation
        public void SetMcode(int pointId)
        {
            //jemoon : AbsodexMotor program Number는 pointId + 1
            int programNo = pointId;
            for (int i = 0; i < 8; i++)
            {
                bool flag = ((programNo >> i) & 0x01) > 0;
                m_DiMCodes[i].SetState(flag);
            }
        }
        #endregion

        #region Override
        public override int SetAct(ActuatorAct act)
        {
            if (!this.Initialized) return -1;

            // Alarm, emo 상태에서는 Move 명령 수행 금지토록

            if (act == ActuatorAct.Neg) act = ActuatorAct.Pos;
            if (act == ActuatorAct.Pos)
            {
                if (IsAlarm() || IsActStatus(ActuatorAct.EStop)) return -1;
                m_CurPoint = -1;
            }

            if (IsInterlockCondition(act))
            {
                if (!m_GenInfos.AutoMode)
                {
                    DisplayNotice();
                }

                return 0;
            }

            m_RefAct = act;
            m_CurAct = ActuatorAct.Noop;

            UpdateTag();

            return 0;
        }

        public override int SetRefPoint(int pointId)
        {
            if (!this.Initialized) return -1;

            // Move중이거나, Alarm, Emo 일경우에는 수행금지

            //if (!IsReady() || IsAlarm()) return -1;

            m_RefPoint = pointId;

            UpdateTag();

            return 0;
        }

        public override bool IsActStatus(ActuatorAct act)
        {
            return (m_CurAct == act);
        }

        public override bool IsAlarm()
        {
            bool alarm = false;
            if (m_DiAlarmCode1 != null)
            {
                alarm |= m_DiAlarmCode1.GetState();
            }
            if (m_DiAlarmCode2 != null)
            {
                alarm |= m_DiAlarmCode2.GetState();
            }

            return alarm;
        }

        public override AlarmList GetAlarmList()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ActuatorAct GetCurAct()
        {
            return m_CurAct;
        }

        public override ActuatorAct GetRefAct()
        {
            return m_RefAct;
        }
        public override int GetRefPoint()
        {
            return m_RefPoint;
        }

        public override int GetCurPoint()
        {
            return m_CurPoint;
        }

        public override void SetCurPoint(int pointId)
        {
            m_CurPoint = pointId;
        }

        public override bool GetPositionSensorState(int pointId, bool defaultValue)
        {
            if (m_PositionSensors.Count <= pointId)
            {
                return defaultValue;
            }
            else
            {
                if (m_PositionSensors[pointId] == null) return defaultValue;
                else return m_PositionSensors[pointId].IsDetected();
            }
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
            ok &= (m_DiMCode0 != null);
            ok &= (m_DiMCode1 != null);
            ok &= (m_DiMCode2 != null);
            ok &= (m_DiMCode3 != null);
            ok &= (m_DiMCode4 != null);
            ok &= (m_DiMCode5 != null);
            ok &= (m_DiMCode6 != null);
            ok &= (m_DiMCode7 != null);
            ok &= (m_DiMoveConfirm != null);
            ok &= (m_DiReady != null);
            ok &= (m_DiAlarmCode1 != null);
            ok &= (m_DiAlarmCode2 != null);
            ok &= (m_DoPosSelect0 != null);
            ok &= (m_DoPosSelect1 != null);
            ok &= (m_DoPosSelect2 != null);
            ok &= (m_DoPosSelect3 != null);
            ok &= (m_DoPosSelectSet != null);
            ok &= (m_DoMove != null);
            ok &= (m_DoStop != null);
            ok &= ((m_DoAnswer != null) || (m_AnswerMode == AnswerMode.NoUse));
            ok &= (m_DoEStop != null);
            ok &= (m_DoReset != null);


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
                ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_MoveFail = new Alarm(this.Name + " Move Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_AlarmResetFail = new Alarm(this.Name + " Alarm Reset Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_DiMCodes = new IoCollection<IoDigitalInput>();
                m_DiMCodes.Add(m_DiMCode0);
                m_DiMCodes.Add(m_DiMCode1);
                m_DiMCodes.Add(m_DiMCode2);
                m_DiMCodes.Add(m_DiMCode3);
                m_DiMCodes.Add(m_DiMCode4);
                m_DiMCodes.Add(m_DiMCode5);
                m_DiMCodes.Add(m_DiMCode6);
                m_DiMCodes.Add(m_DiMCode7);
                m_DoPosSelects = new IoCollection<IoDigitalOutput>();
                m_DoPosSelects.Add(m_DoPosSelect0);
                m_DoPosSelects.Add(m_DoPosSelect1);
                m_DoPosSelects.Add(m_DoPosSelect2);
                m_DoPosSelects.Add(m_DoPosSelect3);

                m_SeqMove = new SeqAbsodexMove(this);
                m_SeqReset = new SeqAbsodexReset(this);


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
                m_DoEStop.SetState(true);
                if (m_Simul.Device)
                {
                    m_DiReady.SetState(true);
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

                string postionList = "";
                string posSensorList = "";
                if (m_PointList != null)
                {
                    foreach (string pos in m_PointList)
                    {
                        postionList += (pos + "*");
                        posSensorList += (false.ToString() + "*");
                    }
                    m_Tag.SetValue(tagDescriptor.POSITION_LIST, postionList);
                    m_Tag.SetValue(tagDescriptor.SENSOR_LIST, posSensorList);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ACT_STATUS, GetCurAct());
            m_Tag.SetValue(tagDescriptor.ACT_COMMAND, GetRefAct());
            m_Tag.SetValue(tagDescriptor.REF_POS, GetRefPoint());
            m_Tag.SetValue(tagDescriptor.CUR_POS, GetCurPoint());
            m_Tag.SetValue(tagDescriptor.ALARM, IsAlarm());

            string posSensorList = "";
            if (m_PositionSensors != null)
            {
                foreach (Sensor sensor in m_PositionSensors)
                {
                    if (sensor == null) posSensorList += (false.ToString() + "*");
                    else posSensorList += (sensor.IsDetected().ToString() + "*");
                }

                m_Tag.SetValue(tagDescriptor.SENSOR_LIST, posSensorList);
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

            log = string.Format("\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion
    }

    public class SeqAbsodexMove : XSeqFunction
    {
        #region Fields
        private AbsodexMotor m_Motor = null;
        private Simul m_Simul = null;
        private int moveTimeout = 0;
        #endregion

        #region Constructor
        public SeqAbsodexMove()
        {
        }

        public SeqAbsodexMove(AbsodexMotor motor)
        {
            m_Simul = AppConfig.Instance.Simul;
            m_Motor = motor;
        }
        #endregion

        #region override
        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Motor.IsReady())
                    {
                        m_Motor.PosSelect(m_Motor.GetRefPoint());
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        result = m_Motor.ALM_ReadyFail.Id;
                        nSeqNo = 0;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 50)
                    {
                        m_Motor.PosSelectSet();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (!m_Motor.DoPosSelectSet.GetState())
                    {
                        nSeqNo = 100;
                    }
                    break;
                case 100:
                    if (m_Motor.IsReady())
                    {
                        m_Motor.Move();

                        if (m_Simul.Device)
                        {
                            moveTimeout = 2 * 1000;
                        }
                        else
                        {
                            //jemoon : Absodex Motor는 0번이 Home
                            if (m_Motor.GetRefPoint() == 0)
                            {
                                moveTimeout = m_Motor.HomeTimeout * 1000;
                            }
                            else
                            {
                                moveTimeout = m_Motor.MoveTimeout * 1000;
                            }
                        }


                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 200;
                    }
                    else
                    {
                        result = m_Motor.ALM_ReadyFail.Id;
                        nSeqNo = 0;
                    }
                    break;
                case 200:
                    if (m_Motor.IsConfirm())
                    {

                        if (m_Motor.AnswerMode == AnswerMode.MoveConfirm ||
                            m_Motor.AnswerMode == AnswerMode.Both)
                        {
                            m_Motor.Answer();
                        }

                        nSeqNo = 210;
                    }
                    else if (m_Motor.IsAlarm())
                    {
                        result = m_Motor.ALM_MoveFail.Id;
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > moveTimeout)
                    {
                        if (m_Simul.Device)
                        {
                            m_Motor.DiMoveConfirm.SetState(true);
                        }
                        else
                        {
                            result = m_Motor.ALM_MoveFail.Id;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 210:
                    if (!m_Motor.IsConfirm())
                    {
                        //result = 0;
                        //nSeqNo = 0;
                        nSeqNo = 220;
                    }
                    else if (m_Simul.Device)
                    {
                        m_Motor.DiMoveConfirm.SetState(false);
                    }
                    break;
                case 220:
                    if (m_Motor.GetMcode() > 0)
                    {
                        if (m_Motor.AnswerMode == AnswerMode.MCode ||
                            m_Motor.AnswerMode == AnswerMode.Both)
                        {
                            m_Motor.Answer();
                        }

                        if (m_Motor.GetMcode() == (m_Motor.GetRefPoint() + 1))
                        {
                            nSeqNo = 300;
                        }
                        else
                        {
                            result = m_Motor.ALM_MoveFail.Id;
                            nSeqNo = 0;
                        }
                    }
                    else if (GetElapsedTicks() > moveTimeout)
                    {
                        if (m_Simul.Device)
                        {
                            m_Motor.SetMcode(m_Motor.GetRefPoint() + 1);
                        }
                        else
                        {
                            result = m_Motor.ALM_MoveFail.Id;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 300:
                    if (m_Motor.GetMcode() == 0)
                    {
                        m_Motor.SetCurPoint(m_Motor.GetRefPoint());
                        result = 0;
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > moveTimeout)
                    {
                        if (m_Simul.Device)
                        {
                            m_Motor.SetMcode(0);
                        }
                        else
                        {
                            result = m_Motor.ALM_MoveFail.Id;
                            nSeqNo = 0;
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;
            return result;
        }
        #endregion
    }

    public class SeqAbsodexReset : XSeqFunction
    {
        #region Fields
        private IServerManager m_Server = null;
        private AbsodexMotor m_Motor = null;
        #endregion

        #region Constructor
        public SeqAbsodexReset()
        {
        }

        public SeqAbsodexReset(AbsodexMotor motor)
        {
            m_Motor = motor;
            m_Server = motor.ServerManager;
        }
        #endregion

        #region override
        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_Motor.AlarmReset();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_Motor.IsAlarm() && m_Motor.IsReady())
                    {
                        result = 0;
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 2 * 1000)
                    {
                        result = m_Motor.ALM_AlarmResetFail.Id;
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;
            return result;
        }
        #endregion
    }
}
