///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.
// Author       : eun
// Description  : Servo Unit for YASKAWA MP2300
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Dms.Common;
using System.IO;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoUnitMp2300 : _ServoUnit
    {
        #region Fields
        protected static TagDescriptorServoUnit tagDescriptor = new TagDescriptorServoUnit();

        private static string m_sDirName = "ServoPara";
        private _GenericCollection<ServoMotorMp2300> m_Axis = new _GenericCollection<ServoMotorMp2300>();
        private ServoUnitInfo m_RbtInfo = new ServoUnitInfo();
        private ThreadRbtManualActMP2300 m_ThreadRbtManualAct;
        private SeqRbtEStopMP2300 m_SeqRbtEStop;
        private SeqRbtResetMP2300 m_SeqRbtReset;
        private SeqRbtMoveHomeMP2300 m_SeqRbtMoveHome;
        private SeqRbtMovePointMP2300 m_SeqRbtMovePoint;
        private SeqRbtMovePosMP2300 m_SeqRbtMovePos;
        private SeqRbtMovePointByDataMP2300 m_SeqRbtMovePointByData;
        private SeqSetHomeCompleteMP2300 m_SeqSetHomeComp;
        private SeqSetCurPositionMP2300 m_SeqSetCurPosition;
        private SeqSetCalThetaMP2300 m_SeqSetCalTheta;
        //private SeqRbtMoveS1 m_SeqRbtMoveS1;
        //private SeqRbtMoveAT1 m_SeqRbtMoveAT1;
        private int m_AxisCount;
        private string m_Message = "";
        private int m_RepeatWaitTime = 500; // 500 msec
        private static ushort[][] m_PointBuf = new ushort[2][];
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        [Category("DMS : Unit Info")]
        public _GenericCollection<ServoMotorMp2300> Axis
        {
            get { return m_Axis; }
            set { m_Axis = value; }
        }
        //[Category("DMS : Unit Info"), Description("Unit moving type")]
        //public MoveType MoveType
        //{
        //    get { return m_RbtInfo.MoveType; }
        //    set { m_RbtInfo.MoveType = value; }
        //}
        [Category("DMS : Unit Info"), Description("Teaching pointName")]
        public override string[] TeachPointName
        {
            get { return m_RbtInfo.PointName; }
            set { m_RbtInfo.PointName = value; }
        }
        //[Category("DMS : Unit Info"), Description("Sync Control")]
        //[RefreshProperties(RefreshProperties.All)]
        //public ServoSyncInfo SyncInfo
        //{
        //    get { return m_RbtInfo.SyncInfo; }
        //    set { m_RbtInfo.SyncInfo = value; }
        //}
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
        [Browsable(false), XmlIgnore()]
        public bool IsLinked
        {
            get
            {
                if (m_AxisCount == 0)
                {
                    return false;
                }
                else
                {
                    return m_Axis[0].IsCommConnected;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsTryLink
        {//
            get
            {
                if (m_AxisCount == 0)
                {
                    return false;
                }
                else
                {
                    return m_Axis[0].IsTryLink;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public static ushort[][] PointInfoBuf
        {
            get { return m_PointBuf; }
            set { m_PointBuf = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double Theta
        {
            get { return m_SeqSetCalTheta.Theta; }
        }
        #endregion

        #region Constructor
        public ServoUnitMp2300()
        {
            this.Name = "__ Mp2300 Unit";
        }
        #endregion

        #region Methods
        public void InitSequence()
        {
            m_ThreadRbtManualAct = new ThreadRbtManualActMP2300(10, this);

            m_SeqRbtEStop = new SeqRbtEStopMP2300(this);
            m_SeqRbtReset = new SeqRbtResetMP2300(this);
            m_SeqRbtMoveHome = new SeqRbtMoveHomeMP2300(this);
            m_SeqRbtMovePoint = new SeqRbtMovePointMP2300(this);
            m_SeqRbtMovePos = new SeqRbtMovePosMP2300(this);
            m_SeqRbtMovePointByData = new SeqRbtMovePointByDataMP2300(this);
            m_SeqSetHomeComp = new SeqSetHomeCompleteMP2300(this);
            m_SeqSetCurPosition = new SeqSetCurPositionMP2300(this);
            //m_SeqRbtMoveS1 = new SeqRbtMoveS1(this);
            //m_SeqRbtMoveAT1 = new SeqRbtMoveAT1(this);
            m_SeqSetCalTheta = new SeqSetCalThetaMP2300(this);
        }

        public void InitSequenceParameterforEstop()
        {
            m_SeqRbtReset.InitSeq();
            m_SeqRbtMoveHome.InitSeq();
            m_SeqRbtMovePoint.InitSeq();
            m_SeqRbtMovePos.InitSeq();
            m_SeqRbtMovePointByData.InitSeq();
            m_SeqSetHomeComp.InitSeq();
            m_SeqSetCurPosition.InitSeq();
            m_SeqSetCalTheta.InitSeq();

            int count = m_Axis.Count;
            ServoMotorMp2300 axis;
            for (int i = 0; i < count; i++)
            {
                axis = m_Axis[i];
                axis.SeqClearAxisErr.InitSeq();
                axis.SeqHome.InitSeq();
                axis.SeqSetCurPos.InitSeq();
                axis.SeqSetHomeComp.InitSeq();
            }
        }

        public void InitSequenceParameter()
        {
            m_SeqRbtEStop.InitSeq();
            m_SeqRbtReset.InitSeq();
            m_SeqRbtMoveHome.InitSeq();
            m_SeqRbtMovePoint.InitSeq();
            m_SeqRbtMovePos.InitSeq();
            m_SeqRbtMovePointByData.InitSeq();
            m_SeqSetHomeComp.InitSeq();
            m_SeqSetCurPosition.InitSeq();
            //m_SeqRbtMoveS1.InitSeq();
            //m_SeqRbtMoveAT1.InitSeq();
            m_SeqSetCalTheta.InitSeq();

            int count = m_Axis.Count;
            ServoMotorMp2300 axis;
            for (int i = 0; i < count; i++)
            {
                axis = m_Axis[i];
                axis.SeqClearAxisErr.InitSeq();
                axis.SeqHome.InitSeq();
                axis.SeqSetCurPos.InitSeq();
                axis.SeqSetHomeComp.InitSeq();
            }
        }

        public bool IsMoveOk()
        {
            for (int id = 0; id < m_AxisCount; id++)
            {
                if (!this.Axis[id].IsCmdDone()) return false;
            }

            return true;
        }

        public bool IsServoOn()
        {
            for (int id = 0; id < m_AxisCount; id++)
            {
                if (!this.Axis[id].GetServoOnState()) return false;
            }

            return true;
        }

        public bool IsRbtSensorOk()
        {
            bool bResult = true;
            for (int id = 0; id < m_AxisCount; id++)
            {
                AxisEventMP2300 axisEvent = this.Axis[id].GetAxisState();
                axisEvent &= (AxisEventMP2300)0x0F;
                if ((axisEvent & AxisEventMP2300.Warning) > 0 || (axisEvent & AxisEventMP2300.Alarm) > 0)
                {
                    return false;
                }

                AxisSourceMP2300 source = this.Axis[id].GetAxisSource();
                source &= (AxisSourceMP2300)(0x07F);
                if ((source & AxisSourceMP2300.PosLimit) > 0) bResult = false;
                if ((source & AxisSourceMP2300.NegLimit) > 0) bResult = false;
            }

            return bResult;
        }

        public bool IsRbtStatusOk()
        {
            bool bResult = true;
            for (int id = 0; id < m_AxisCount; id++)
            {
                AxisEventMP2300 axisEvent = this.Axis[id].GetAxisState();
                axisEvent &= (AxisEventMP2300)0x0F;
                if ((axisEvent & AxisEventMP2300.Warning) > 0 || (axisEvent & AxisEventMP2300.Alarm) > 0)
                {
                    return false;
                }

                AxisSourceMP2300 source = this.Axis[id].GetAxisSource();
                source &= (AxisSourceMP2300)(0x07F);
                if ((source & AxisSourceMP2300.PosLimit) > 0) bResult = false;
                if ((source & AxisSourceMP2300.NegLimit) > 0) bResult = false;
                if ((source & AxisSourceMP2300.ActReject) > 0) bResult = false;
            }

            return bResult;
        }
        //public Boolean SetSyncControl(Boolean enable)
        //{
        //    return SyncInfo.Master.SetSyncControl(SyncInfo.Slave.AxisId, enable);
        //}
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

        public int GetRbtError()
        {
            ServoMotorMp2300 motor;

            if (!IsLinked)
            {
                for (int i = 0; i < m_AxisCount; i++)
                {
                    motor = m_Axis[i];
                    motor.ClearBit(false);
                }
                return (int)MP2300Error.errLink;
            }

            int code = 0;
            for (int id = 0; id < this.AxisCount; id++)
            {
                motor = m_Axis[id];
                if (!motor.GetServoReady())
                {
                    return (int)MP2300Error.errServoReady;
                }
                else if ((code = Axis[id].GetControllerError()) > 0)
                {
                    return code;
                }
            }
            return 0;
        }
        private void PointInfo2Buf()
        {
            int dWord = 0;
            int index = 0;

            for (int i = 0; i < this.TeachPoints; i++)
            {
                if (i < 8)
                {
                    ServoMotorMp2300 motor;
                    for (int axisId = 0; axisId < m_AxisCount; axisId++)
                    {
                        motor = m_Axis[axisId];

                        index = (0 + (motor.AxisId * 10) + (i * 160));
                        dWord = motor.Len2Pulse<int>(TeachPoint[i].Pos[axisId]);
                        m_PointBuf[0][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[0][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

                        index = (2 + (motor.AxisId * 10) + (i * 160));
                        dWord = motor.Len2Pulse<int>(motor.AxisVel);
                        m_PointBuf[0][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[0][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

                        index = (4 + (motor.AxisId * 10) + (i * 160));
                        dWord = (int)motor.AxisAcc;
                        m_PointBuf[0][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[0][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

                        index = (6 + (motor.AxisId * 10) + (i * 160));
                        dWord = (int)motor.AxisDec;
                        m_PointBuf[0][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[0][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);
                    }
                }
                else
                {
                    ServoMotorMp2300 motor;
                    for (int axisId = 0; axisId < m_AxisCount; axisId++)
                    {
                        motor = m_Axis[axisId];

                        index = (0 + (motor.AxisId * 10) + ((i - 8) * 160));
                        dWord = motor.Len2Pulse<int>(TeachPoint[i].Pos[axisId]);
                        m_PointBuf[1][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[1][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

                        index = (2 + (motor.AxisId * 10) + ((i - 8) * 160));
                        dWord = motor.Len2Pulse<int>(motor.AxisVel);
                        m_PointBuf[1][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[1][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

                        index = (4 + (motor.AxisId * 10) + ((i - 8) * 160));
                        dWord = (int)motor.AxisAcc;
                        m_PointBuf[1][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[1][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

                        index = (6 + (motor.AxisId * 10) + ((i - 8) * 160));
                        dWord = (int)motor.AxisDec;
                        m_PointBuf[1][index] = (ushort)(dWord & 0x0000FFFF);
                        m_PointBuf[1][index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);
                    }
                }
            }
        }
        public int RbtMovePoint(short pointId)
        {
            return m_SeqRbtMovePointByData.Do(pointId);
        }
        public int SetCalTheta()
        {
            return m_SeqSetCalTheta.Do();
        }
        private short GetAcc(short axisId)
        {
            return this.Axis[axisId].AxisAcc;
        }
        private void SetAcc(short axisId, short acc)
        {
            this.Axis[axisId].AxisAcc = acc;
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
            get { return this.Axis.Count; }
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
            return this.Axis[axisId].Name;
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
                        }

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
                    string[] data;

                    for (short axisId = 0; axisId < m_AxisCount; axisId++)
                    {
                        readText = sreader.ReadLine();
                        double vel = 0.0;
                        short acc = 0;
                        try
                        {
                            data = readText.Split(',');
                            vel = Convert.ToDouble(data[0]);
                            if (data.Length > 1)
                                acc = Convert.ToInt16(data[1]);
                            else
                                acc = GetAcc(axisId);
                        }
                        catch
                        {
                            vel = GetDefaultVel(axisId);
                        }

                        SetDefaultVel(axisId, vel);
                        SetVel(axisId, vel);
                        SetAcc(axisId, acc);
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
                    short acc = GetAcc(axisId);
                    SetDefaultVel(axisId, vel);
                    writeText = string.Format("{0:f3}, {1} ", vel, acc.ToString());
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
            return Axis[axisId].GetPosition(ref pos);
        }

        public override int SetCurPosition(short pointId)
        {
            return m_SeqSetCurPosition.Do(pointId);
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
            return this.Axis[axisId].AxisVel;
        }
        public override double GetDefaultVel(short axisId)
        {
            return this.Axis[axisId].AxisDefaultVel;
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
            this.Axis[axisId].AxisVel = vel;
        }
        public override void SetDefaultVel(short axisId, double vel)
        {
            this.Axis[axisId].AxisDefaultVel = vel;
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
            double pulse = this.Axis[axisId].Len2Pulse(vel) * sign;
            this.Axis[axisId].StartVelMove(pulse);
        }

        public override void RbtMoveVelStop(short axisId)
        {
            this.Axis[axisId].StopVelMove();
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
        //        axis = Axis[id];
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
            return this.Axis[axisId].GetHomeSwitch();
        }

        public bool IsDetectHomeSwitchAllAxis()
        {
            bool ok = true;
            for (int i = 0; i < m_AxisCount; i++)
            {
                ok &= m_Axis[i].GetHomeSwitch();
            }
            return ok;
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
            double pulse = this.Axis[axisId].Len2Pulse(vel) * sign;
            this.Axis[axisId].AxisAcc = acc;
            this.Axis[axisId].StartVelMove(pulse);
        }

        public override int SetHomeComp()
        {
            return m_SeqSetHomeComp.Do();
        }
        #endregion

        #region Override
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

                    if (m_PointBuf[0] == null)
                    {
                        m_PointBuf[0] = new ushort[Dms.Ctl.MP2300.POINT_NUM];
                        m_PointBuf[1] = new ushort[Dms.Ctl.MP2300.POINT_NUM];
                        m_PointBuf.Initialize();
                    }
                    PointInfo2Buf();
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

            ServoMotorMp2300 m_Motor;
            for (int i = 0; i < m_AxisCount; i++)
            {
                m_Motor = m_Axis[i];

                double GetPos = GetTeachPointPos((short)RbPosId.Zero, (short)0);
                string msg = string.Format("{0:f2}", m_Motor.GetPosition() - GetPos);
                m_Tag.SetValue(tagDescriptor.refZERO, msg);//khh090807
            }
        }

        public override _ServoMotor GetServoMotor(int index)
        {
            return m_Axis[index] as _ServoMotor;
        }
        #endregion
    }
}
