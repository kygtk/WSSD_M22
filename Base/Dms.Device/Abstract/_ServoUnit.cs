using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class _ServoUnit : _DeviceAsm, IServoUnit
    {
        //public void FireEventUpdatePosition(RbtPos rbtPos)
        //{
        //    PositionChangeEventHandler eHandle = this.OnPositionChange;
        //    if (eHandle != null)
        //    {
        //        PositionChangeEventArgs args = new PositionChangeEventArgs(this.Id, this.AxisCount);
        //        args.CurPos = rbtPos;
        //        eHandle(this, args);
        //    }
        //}

        //public void FireEventUpdateStatus(AxisStatus status)
        //{
        //    StatusChangeEventHandler eHandle = this.OnStatusChange;
        //    if (eHandle != null)
        //    {
        //        for (short id = 0; id < this.AxisCount; id++)
        //        {
        //            StatusChangeEventArgs args = new StatusChangeEventArgs(this.Id, status);
        //            eHandle(this, args);
        //        }
        //    }
        //}


        #region Fields
        protected static ServoInterlockConditionDelegate m_IsServoInterlockCondition;
        #endregion

        #region Properties
        public virtual string[] TeachPointName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        #endregion

        #region Methods
        public virtual _ServoMotor GetServoMotor(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public static void SetInterlockScenario(ServoInterlockConditionDelegate scenario)
        {
            m_IsServoInterlockCondition = scenario;
        }
        #endregion

        #region IServoUnit 멤버

        //public int Id
        //{
        //    get { throw new Exception("The method or operation is not implemented."); }
        //}

        //public string Name
        //{
        //    get { throw new Exception("The method or operation is not implemented."); }
        //}

        public virtual int AxisCount
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual int TeachPoints
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual bool HomeComp
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual bool Ready
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual bool Sync
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual double VelRatio
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        //public virtual RbtPos CurPos
        //{
        //    get { throw new Exception("The method or operation is not implemented."); }
        //    set { throw new Exception("The method or operation is not implemented."); }
        //}

        //public virtual AxisStatus[] CurStatus
        //{
        //    get { throw new Exception("The method or operation is not implemented."); }
        //    set { throw new Exception("The method or operation is not implemented."); }
        //}

        public virtual int ManualActionCmd
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual short SelectedPointId
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        //public event PositionChangeEventHandler OnPositionChange;

        //public event StatusChangeEventHandler OnStatusChange;

        public virtual bool RbtEStop()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool RbtReset()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int RbtMoveHome()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual string GetAxisName(short axisId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual string GetPointName(short pointId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual RbtPos GetTeachPointPos(short pointId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double GetTeachPointPos(short pointId, short axisId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetTeachPointPos(short pointId, RbtPos rbtPos)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetTeachPointPos(short pointId, short axisId, double pos)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool SaveTeachPosToFile()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool ReadTeachPosFromFile()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SaveVelToFile()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void ReadVelFromFile()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual RbtVel GetVel()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double GetVel(short axisId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double GetDefaultVel(short axisId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetVel(RbtVel rbtVel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetVel(short axisId, double vel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetDefaultVel(short axisId, double vel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int GetCurPointId()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool GetCurPosition(ref RbtPos rbtPos)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool GetCurPosition(short axisId, ref double pos)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int SetCurPosition(short pointId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //public virtual void SetCurPosition(short axisId, double pos)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public virtual int RbtMovePos(short posId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int RbtMovePos(RbtPos rbtPos)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int RbtMovePos(string posName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void RbtMoveVelStart(short axisId, short sign, double vel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void RbtMoveVelStart(short axisId, short sign, double vel, short acc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void RbtMoveVelStop(short axisId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetVelRatio(double ratio)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool IsDetectHomeSwitch(short axisId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //public virtual AxisStatus[] GetRbtStatus()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public virtual int SetHomeComp()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        public override Type FamilyType
        {
            get { return typeof(_ServoUnit); }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void UpdateTag()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override DmsErrors Initialize()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
