using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;


namespace Dms.Device
{
    public delegate bool ServoInterlockConditionDelegate(_ServoUnit unit, int tagetPosition);

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoUnit : _ServoUnit
    {
        #region Fields
        protected static TagDescriptorServoUnit tagDescriptor = new TagDescriptorServoUnit();

        private static string m_sDirName = "ServoPara";
        private _GenericCollection<ServoMotor> m_Axis = new _GenericCollection<ServoMotor>();
        private _GenericCollection<_InterlockSensor> m_InterlockSensors = new _GenericCollection<_InterlockSensor>();
        private ServoUnitInfo m_RbtInfo = new ServoUnitInfo();
        private ThreadRbtManualAct m_ThreadRbtManualAct;
        //private ThreadRbtUpdateCondition m_ThreadRbtUpdateCondition;
        private SeqRbtEStop m_SeqRbtEStop;
        private SeqRbtReset m_SeqRbtReset;
        private SeqRbtMoveHome m_SeqRbtMoveHome;
        private SeqRbtMovePoint m_SeqRbtMovePoint;
        private SeqRbtMovePos m_SeqRbtMovePos;
        private SeqRbtMoveS1 m_SeqRbtMoveS1;
        private SeqRbtMoveAT1 m_SeqRbtMoveAT1;
        private int m_AxisCount;
        private string m_Message = "";
        private int m_RepeatWaitTime = 500; // 500 msec
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        [Category("DMS : Unit Info")]
        public _GenericCollection<ServoMotor> Axis
        {
            get { return m_Axis; }
            set { m_Axis = value; }
        }
        [Category("DMS : Unit Info"), DisplayName("Interlock Sensors"), Description("Interlock sensors that will stop the motor when detected.")]
        public _GenericCollection<_InterlockSensor> Interlocks  //  BM
        {
            get { return m_InterlockSensors; }
            set { m_InterlockSensors = value; }
        }
        [Category("DMS : Unit Info"), Description("Unit moving type")]
        public MoveType MoveType
        {
            get { return m_RbtInfo.MoveType; }
            set { m_RbtInfo.MoveType = value; }
        }
        [Category("DMS : Unit Info"), Description("Teaching pointName")]
        public override string[] TeachPointName
        {
            get { return m_RbtInfo.PointName; }
            set { m_RbtInfo.PointName = value; }
        }
        [Category("DMS : Unit Info"), Description("Sync Control")]
        [RefreshProperties(RefreshProperties.All)]
        public ServoSyncInfo SyncInfo
        {
            get { return m_RbtInfo.SyncInfo; }
            set { m_RbtInfo.SyncInfo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos[] TeachPoint
        {
            get { return m_RbtInfo.TeachPoint; }
            set { m_RbtInfo.TeachPoint = value; }
        }
        [Browsable(false), XmlIgnore()]
        public short[] HomeOrder
        {
            get { return m_RbtInfo.HomeOrder; }
            set { m_RbtInfo.HomeOrder = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string DirName
        {
            get { return m_sDirName; }
            set { m_sDirName = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool ManualMoving
        {
            get { return m_ThreadRbtManualAct.ManualActionCmd > 0; }
        }
        [Browsable(false), XmlIgnore()]
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int RepeatWaitTime
        {
            get
            {
                return m_RepeatWaitTime;
            }
            set
            {
                m_RepeatWaitTime = value;
            }
        }
        #endregion

        //#region Event
        //public event PositionChangeEventHandler OnPositionChange;
        //public event StatusChangeEventHandler OnStatusChange; 
        //#endregion

        #region Constructor
        public ServoUnit()
        {
            this.Name = "__ Unit";
        }
        #endregion

        #region Methods
        public void InitSequence()
        {
            m_ThreadRbtManualAct = new ThreadRbtManualAct(10, this);
            //m_ThreadRbtUpdateCondition = new ThreadRbtUpdateCondition(50, this);

            m_SeqRbtEStop = new SeqRbtEStop(this);
            m_SeqRbtReset = new SeqRbtReset(this);
            m_SeqRbtMoveHome = new SeqRbtMoveHome(this);
            m_SeqRbtMovePoint = new SeqRbtMovePoint(this);
            m_SeqRbtMovePos = new SeqRbtMovePos(this);
            m_SeqRbtMoveS1 = new SeqRbtMoveS1(this);
            m_SeqRbtMoveAT1 = new SeqRbtMoveAT1(this);
        }

        public void InitSequenceParameter()
        {
            m_SeqRbtEStop.InitSeq();
            m_SeqRbtReset.InitSeq();
            m_SeqRbtMoveHome.InitSeq();
            m_SeqRbtMovePoint.InitSeq();
            m_SeqRbtMovePos.InitSeq();
            m_SeqRbtMoveS1.InitSeq();
            m_SeqRbtMoveAT1.InitSeq();

            int count = m_Axis.Count;
            for (int i = 0; i < count; i++)
            {
                ServoMotor axis = m_Axis[i];
                if (axis.GetType() == typeof(ServoMotor_Mc))
                {
                    ((ServoMotor_Mc)axis).SeqClearAxisErr.InitSeq();
                    //axis.SeqHomeAndIndex.InitSeq();
                    ((ServoMotor_Mc)axis).SeqHomeOnly.InitSeq();
                }
            }
        }

        public Boolean IsMoveOk()
        {
            for (int id = 0; id < m_AxisCount; id++)
            {
                if (!m_Axis[id].IsCmdDone()) return false;
            }

            return true;
        }

        public Boolean IsRbtSensorOk()
        {
            Boolean bResult = true;
            for (int id = 0; id < m_AxisCount; id++)
            {
                AxisEvent axisEvent = m_Axis[id].GetAxisState();
                if ((axisEvent == AxisEvent.EStopEvent) ||
                    (axisEvent == AxisEvent.AbortEvent))
                {
                    return false;
                }

                bResult = m_Axis[id].CheckAxisState();
                if (!bResult) return false;

                //AxisSource source = m_Axis[id].GetAxisSource();
                //source &= (AxisSource)(0x1FFF);
                //if ((source & AxisSource.StPosLimit) > 0) bResult = false;
                //if ((source & AxisSource.StNegLimit) > 0) bResult = false;
                //if ((source & AxisSource.StAmpFault) > 0) bResult = false;
                //if ((source & AxisSource.StAmpPowerOnOff) > 0) bResult = false;
                //if ((source & AxisSource.StOutofFrames) > 0) bResult = false;
            }

            return bResult;

        }

        public Boolean IsRbtStatusOk()
        {
            Boolean bResult = true;
            for (int id = 0; id < m_AxisCount; id++)
            {
                AxisEvent axisEvent = m_Axis[id].GetAxisState();
                if ((axisEvent == AxisEvent.StopEvent) ||
                    (axisEvent == AxisEvent.EStopEvent) ||
                    (axisEvent == AxisEvent.AbortEvent))
                {
                    return false;
                }

                bResult = m_Axis[id].CheckAxisState();
                if (!bResult) return false;

                //AxisSource source = m_Axis[id].GetAxisSource();
                ////source &= 0x1FFF;
                //if ((source & AxisSource.StPosLimit) > 0) bResult = false;
                //if ((source & AxisSource.StNegLimit) > 0) bResult = false;
                //if ((source & AxisSource.StAmpFault) > 0) bResult = false;
                //if ((source & AxisSource.StAmpPowerOnOff) > 0) bResult = false;
                //if ((source & AxisSource.StOutofFrames) > 0) bResult = false;
            }

            return bResult;

        }

        public Boolean SetSyncControl(Boolean enable)
        {
            return SyncInfo.Master.SetSyncControl(SyncInfo.Slave.AxisId, enable);
        }

        public void InitTeachPoint()
        {
            int no = this.TeachPoints;
            this.TeachPoint = new RbtPos[no];
            for (int id = 0; id < no; id++)
            {
                this.TeachPoint[id] = new RbtPos(this.AxisCount);
            }

            ReadTeachPosFromFile();
        }

        public int RbtMoveS1(RbtPos rbtPos, RbtVel rbtVel)
        {
            return m_SeqRbtMoveS1.Do(rbtPos, rbtVel);
        }

        public int RbtMoveAT1(RbtPos rbtPos, RbtVel rbtVel)
        {
            return m_SeqRbtMoveAT1.Do(rbtPos, rbtVel);
        }

        // Move to _ServoUnit
        //public static void SetInterlockScenario(ServoInterlockConditionDelegate scenario)
        //{
        //    m_IsServoInterlockCondition = scenario;
        //}

        public bool IsInterlockCondition()
        {
            // -1 : 특정 포인트가 아닌 일반 인터락 조건
            return IsInterlockCondition(-1);
        }

        public bool IsInterlockCondition(int targetPosition)
        {
            if (m_IsServoInterlockCondition == null)
            {
                return false;
            }
            else
            {
                return m_IsServoInterlockCondition(this, targetPosition);
            }
        }
        #endregion

        #region implement IServoUnit
        private bool m_HomeComp;
        private bool m_bReady;
        [Browsable(false), XmlIgnore()]
        [Description("Home complete")]
        public override bool HomeComp
        {
            get { return m_HomeComp; }
            set { m_HomeComp = value; }
        }
        [Browsable(false), XmlIgnore()]
        [Description("Servo On(Reset)")]
        public override bool Ready
        {
            get { return m_bReady; }
            set { m_bReady = value; }
        }
        [Category("DMS : Unit Info"), XmlIgnore()]
        public override int AxisCount
        {
            get { return m_Axis.Count; }
        }
        [Category("DMS : Unit Info"), XmlIgnore()]
        public override int TeachPoints
        {
            get
            {
                if (this.TeachPointName == null) return 0;
                else
                {
                    return this.TeachPointName.Length;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public override bool Sync
        {
            get { return m_RbtInfo.SyncInfo.Sync; }
            set { m_RbtInfo.SyncInfo.Sync = value; }
        }
        //[Browsable(false)]
        //public override short MasterAxis
        //{
        //    get { return m_RbtInfo.SyncInfo.Master.AxisId; }
        //}
        //[Browsable(false)]
        //public override short SlaveAxis
        //{
        //    get { return m_RbtInfo.SyncInfo.Slave.AxisId; }
        //}
        [Browsable(false), XmlIgnore()]
        public override double VelRatio
        {
            get { return m_RbtInfo.VelRatio; }
            set { m_RbtInfo.VelRatio = value; }
        }
        //[Browsable(false), XmlIgnore()]
        //public override RbtPos CurPos
        //{
        //    get { return m_RbtInfo.CurPos; }
        //    set { m_RbtInfo.CurPos = value; }
        //}
        //[Browsable(false), XmlIgnore()]
        //public override AxisStatus[] CurStatus
        //{
        //    get { return m_RbtInfo.CurStatus; }
        //    set { m_RbtInfo.CurStatus = value; }
        //}
        [Browsable(false), XmlIgnore()]
        public override int ManualActionCmd
        {
            get
            {
                if (m_ThreadRbtManualAct == null) return 0;
                else
                {
                    return m_ThreadRbtManualAct.ManualActionCmd;
                }
            }
            set
            {
                if (m_ThreadRbtManualAct == null) return;
                else
                {
                    m_ThreadRbtManualAct.ManualActionCmd = value;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public override short SelectedPointId
        {
            get
            {
                if (m_ThreadRbtManualAct == null) return 0;
                {
                    return m_ThreadRbtManualAct.SelectedPointId;
                }
            }
            set
            {
                if (m_ThreadRbtManualAct == null) return;
                {
                    m_ThreadRbtManualAct.SelectedPointId = value;
                }
            }
        }

        public override string GetAxisName(short axisId)
        {
            return m_Axis[axisId].Name;
        }

        public override string GetPointName(short pointId)
        {
            if (pointId < 0 || pointId > this.TeachPoints) return "?";
            else return this.TeachPointName[pointId];
        }

        public override Boolean RbtEStop()
        {
            return m_SeqRbtEStop.Do() == 0;
        }

        public override Boolean RbtReset()
        {
            return m_SeqRbtReset.Do() == 0;
        }

        public override int RbtMoveHome()
        {
            return m_SeqRbtMoveHome.Do();
        }

        public override bool SaveTeachPosToFile()
        {
            if (m_CheckPath != 1) return false;

            string dirName = m_AppConfig.ServoParameterPathName;
            string fileName = string.Format("{0}\\{1}.pos", dirName, this.Name);

            try
            {
                if (m_AppConfig.UseDefaultFilePath)
                {
                    Directory.CreateDirectory(dirName);
                }

                StreamWriter swriter = File.CreateText(fileName);

                int id = 0;
                string writeText;
                foreach (RbtPos point in this.TeachPoint)
                {
                    id++;
                    writeText = string.Format("{0:d3} ", id);
                    foreach (double pos in point.Pos)
                    {
                        writeText += string.Format("{0:f3} ", pos);
                    }
                    swriter.WriteLine(writeText);
                }

                swriter.Close();
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);

                if (AppConfig.Instance.Simul.Device)
                {
                    MessageBox.Show(msg);
                }
            }

            return true;
        }

        private void CheckPath()
        {
            string filePath = m_AppConfig.ServoParameterPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("Servo Parameter Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Servo Parameter Folder";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.ServoParameterPath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    //m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                //m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                m_CheckPath = 1;
            }
        }

        public override bool ReadTeachPosFromFile()
        {
            string dirName = m_AppConfig.ServoParameterPathName;
            string fileName = string.Format("{0}\\{1}.pos", dirName, this.Name);

            if (File.Exists(fileName) == false)
            {
                return false;
            }
            else
            {
                using (StreamReader sreader = new StreamReader(fileName))
                {
                    string readText;
                    int pointCount = this.TeachPoints;
                    for (short pointId = 0; pointId < pointCount; pointId++)
                    {
                        readText = sreader.ReadLine();
                        string[] splitText = null;
                        if (readText != null)
                        {
                            splitText = readText.Split(new char[] { ' ' });

                            for (short axisId = 0; axisId < m_AxisCount; axisId++)
                            {
                                try
                                {
                                    double pos = Convert.ToDouble(splitText[axisId + 1]);
                                    SetTeachPointPos(pointId, axisId, pos);
                                }
                                catch
                                {
                                    SetTeachPointPos(pointId, axisId, 0.0);
                                }
                            }
                        }
                    }
                }
            }

            return false;

        }

        public override void ReadVelFromFile()
        {
            string dirName = m_AppConfig.ServoParameterPathName;
            string fileName = string.Format("{0}\\{1}.vel", dirName, this.Name);

            if (File.Exists(fileName) == false)
            {
                for (short axisId = 0; axisId < m_AxisCount; axisId++)
                {
                    SetVel(axisId, GetDefaultVel(axisId));
                }

                SaveVelToFile();
            }
            else
            {
                using (StreamReader sreader = new StreamReader(fileName))
                {
                    string readText;

                    for (short axisId = 0; axisId < m_AxisCount; axisId++)
                    {
                        readText = sreader.ReadLine();
                        double vel;
                        try
                        {
                            vel = Convert.ToDouble(readText);
                        }
                        catch
                        {
                            vel = GetDefaultVel(axisId);
                        }

                        SetDefaultVel(axisId, vel);
                        SetVel(axisId, vel);
                    }
                }
            }
        }

        public override void SaveVelToFile()
        {
            if (m_CheckPath != 1) return;

            string dirName = m_AppConfig.ServoParameterPathName;
            string fileName = string.Format("{0}\\{1}.vel", dirName, this.Name);

            try
            {
                if (m_AppConfig.UseDefaultFilePath)
                {
                    Directory.CreateDirectory(dirName);
                }

                StreamWriter swriter = File.CreateText(fileName);
                string writeText = "";

                for (short axisId = 0; axisId < m_AxisCount; axisId++)
                {
                    double vel = GetVel(axisId);
                    SetDefaultVel(axisId, vel);
                    writeText = string.Format("{0:f3} ", vel);
                    swriter.WriteLine(writeText);
                }

                swriter.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public override bool GetCurPosition(ref RbtPos rbtPos)
        {
            for (short axisId = 0; axisId < m_AxisCount; axisId++)
            {
                if (!GetCurPosition(axisId, ref rbtPos.Pos[axisId]))
                {
                    return false;
                }
            }

            return true;
        }
        public override bool GetCurPosition(short axisId, ref double pos)
        {
            return m_Axis[axisId].GetPosition(ref pos);
        }

        public override int SetCurPosition(short pointId)
        {
            for (short id = 0; id < m_AxisCount; id++)
            {
                double pos = GetTeachPointPos(pointId, id);

                SetCurPosition(id, pos);
            }
            return 0;
        }
        private void SetCurPosition(short axisId, double pos)
        {
            m_Axis[axisId].ServoOn(false);

            if (!m_Axis[axisId].GetServoOnState())
            {
                if (0 != m_Axis[axisId].GetEncoderDir())
                {
                    pos *= -1.0;
                }

                pos = m_Axis[axisId].Len2Pulse(pos);
                m_Axis[axisId].SetPosition(pos);
            }
        }

        public override RbtVel GetVel()
        {
            RbtVel rbtVel = new RbtVel(this.AxisCount);

            for (short axisId = 0; axisId < m_AxisCount; axisId++)
            {
                rbtVel.Vel[axisId] = GetVel(axisId);
            }

            return rbtVel;
        }
        public override double GetVel(short axisId)
        {
            return m_Axis[axisId].AxisVel;
        }
        public override double GetDefaultVel(short axisId)
        {
            return m_Axis[axisId].AxisDefaultVel;
        }

        public override void SetVel(RbtVel rbtVel)
        {
            for (short axisId = 0; axisId < m_AxisCount; axisId++)
            {
                SetVel(axisId, rbtVel.Vel[axisId]);
            }
        }
        public override void SetVel(short axisId, double vel)
        {
            m_Axis[axisId].AxisVel = vel;
        }
        public override void SetDefaultVel(short axisId, double vel)
        {
            m_Axis[axisId].AxisDefaultVel = vel;
        }

        public override RbtPos GetTeachPointPos(short pointId)
        {
            RbtPos rbtPos = new RbtPos(this.AxisCount);

            for (short id = 0; id < m_AxisCount; id++)
            {
                rbtPos.Pos[id] = GetTeachPointPos(pointId, id);
            }

            return rbtPos;

        }
        public override double GetTeachPointPos(short pointId, short axisId)
        {
            return this.TeachPoint[pointId].Pos[axisId];
        }

        public override void SetTeachPointPos(short pointId, RbtPos rbtPos)
        {
            this.TeachPoint[pointId] = rbtPos;
        }
        public override void SetTeachPointPos(short pointId, short axisId, double pos)
        {
            this.TeachPoint[pointId].Pos[axisId] = pos;
        }

        public override int RbtMovePos(short pointId)
        {
            return m_SeqRbtMovePoint.Do(pointId);
        }

        public override int RbtMovePos(RbtPos rbtPos)
        {
            return m_SeqRbtMovePos.Do(rbtPos);
        }

        public override int RbtMovePos(string pointName)
        {
            short pointId = -1;
            for (short id = 0; id < this.TeachPoints; id++)
            {
                if (pointName == GetPointName(id))
                {
                    pointId = id;
                    break;
                }
            }

            if (pointId != -1)
            {
                return m_SeqRbtMovePoint.Do(pointId);
            }
            else
            {
                return 1000;
            }
        }

        public override void RbtMoveVelStart(short axisId, short sign, double vel)
        {
            double pulse = m_Axis[axisId].Len2Pulse(vel) * sign;
            m_Axis[axisId].StartVelMove(pulse);
        }

        public override void RbtMoveVelStop(short axisId)
        {
            m_Axis[axisId].StopVelMove();
        }

        public override void SetVelRatio(double ratio)
        {
            this.VelRatio = ratio;
        }

        //public override AxisStatus[] GetRbtStatus()
        //{
        //    AxisStatus[] status = new AxisStatus[this.AxisCount];

        //    ServoMotor axis;
        //    for (short id = 0; id < m_AxisCount; id++)
        //    {
        //        axis = m_Axis[id];
        //        status[id].Limit.Negative = axis.GetNegSwitch();
        //        status[id].Limit.Home = axis.GetHomeSwitch();
        //        status[id].Limit.Positive = axis.GetPosSwitch();
        //        status[id].Source = axis.GetAxisSource();
        //        status[id].State = axis.GetAxisState();
        //    }

        //    return status;
        //}

        public override Boolean IsDetectHomeSwitch(short axisId)
        {
            return m_Axis[axisId].GetHomeSwitch();
        }

        public override int GetCurPointId()
        {
            RbtPos absPos = new RbtPos(m_AxisCount);
            RbtPos curPos = new RbtPos(m_AxisCount);

            GetCurPosition(ref curPos);

            int pointCount = this.TeachPoints;
            for (short id = 0; id < pointCount; id++)
            {
                bool match = true;
                for (short axisId = 0; axisId < m_AxisCount; axisId++)
                {
                    absPos.Pos[axisId] = curPos.Pos[axisId] - GetTeachPointPos(id, axisId);
                    match &= (Math.Abs(absPos.Pos[axisId]) < (double)InPosition.Margin);
                }

                if (match)
                {
                    return id;
                }
            }

            return -1;
        }

        public override void RbtMoveVelStart(short axisId, short sign, double vel, short acc)
        {
            double pulse = m_Axis[axisId].Len2Pulse(vel) * sign;
            m_Axis[axisId].AxisAcc = acc;
            m_Axis[axisId].StartVelMove(pulse);
        }

        public override int SetHomeComp()
        {
            m_HomeComp = true;
            return 0;
        }
        #endregion

        #region Override
        //public override Type FamilyType
        //{
        //    get { return typeof(ServoUnit); }
        //}

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
                this.VelRatio = 1.0;
                //this.CurPos = new RbtPos(this.AxisCount);
                //this.CurStatus = new AxisStatus[this.AxisCount];
                this.HomeOrder = new short[this.AxisCount];
                int count = this.HomeOrder.Length;
                for (short i = 0; i < count; i++)
                {
                    if (!this.Sync)
                    {
                        this.HomeOrder[i] = i;
                    }
                }
                m_AxisCount = this.AxisCount;

                if (m_CheckPath == -1) CheckPath();

                ok &= (m_CheckPath == 1);

                if (ok)
                {
                    InitTeachPoint();

                    ReadVelFromFile();

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
                    InitSequence();

                    m_ThreadRbtManualAct.Start();
                    //m_ThreadRbtUpdateCondition.Start();
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override DmsErrors Uninitialize()
        {
            if (m_Initialized == false) return base.Uninitialize();

            if (m_ThreadRbtManualAct != null)
            {
                m_ThreadRbtManualAct.Pause();
            }
            //m_ThreadRbtUpdateCondition.Pause();

            XTimer watch = new XTimer(this.Name);
            watch.Start(2000);

            bool todo = true;
            while (todo)
            {
                todo &= !this.RbtEStop();
                todo &= !watch.Over;
            }

            return base.Uninitialize();
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
        private enum RbPosId
        {
            Wait = 0,
            Zero = 1
        }
        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.Id, this.Id);
            m_Tag.SetValue(tagDescriptor.CURPOSNAME, this.GetPointName((short)(this.GetCurPointId())));
            //kang 2010.12.13 
            ServoMotor m_Motor;
            for (int i = 0; i < m_AxisCount; i++)
            {
                m_Motor = m_Axis[i];

                double GetPos = GetTeachPointPos((short)RbPosId.Zero, (short)0);
                string msg = string.Format("{0:f2}", m_Motor.GetPosition() - GetPos);
                m_Tag.SetValue(tagDescriptor.refZERO, msg);
            }
        }

        public override _ServoMotor GetServoMotor(int index)
        {
            return m_Axis[index] as _ServoMotor;
        }

        #endregion
    }
}
