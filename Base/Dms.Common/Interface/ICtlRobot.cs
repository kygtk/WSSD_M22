using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    
    public interface IServoUnit
    {
        int Id
        {
            get;
        }
        string Name
        {
            get;
        }
        int AxisCount
        {
            get;
        }
        int TeachPoints
        {
            get;
        }
        bool HomeComp
        {
            get;
        }
        bool Ready
        {
            get;
        }
        bool Sync
        {
            get;
        }
        //short MasterAxis
        //{
        //    get;
        //}
        //short SlaveAxis
        //{
        //    get;
        //}
        double VelRatio
        {
            get;
            set;
        }
        RbtPos CurPos
        {
            get;
        }

        AxisStatus[] CurStatus
        {
            get;
        }
        
        int ManualActionCmd
        {
            get;
            set;
        }
        short SelectedPointId
        {
            set;
        }

        event PositionChangeEventHandler OnPositionChange;
        event StatusChangeEventHandler OnStatusChange;

        Boolean RbtEStop();
        Boolean RbtReset();
        int RbtMoveHome();
        string GetAxisName(short axisId);
        string GetPointName(short pointId);
        RbtPos GetTeachPointPos(short pointId);
        double GetTeachPointPos(short pointId, short axisId);
        void SetTeachPointPos(short pointId, RbtPos rbtPos);
        void SetTeachPointPos(short pointId, short axisId, double pos);
        Boolean SaveTeachPosToFile();
        Boolean ReadTeachPosFromFile();
        void SaveVelToFile();
        void ReadVelFromFile();
        RbtVel GetVel();
        double GetVel(short axisId);
        double GetDefaultVel(short axisId);
        void SetVel(RbtVel rbtVel);
        void SetVel(short axisId, double vel);
        void SetDefaultVel(short axisId, double vel);
        int GetCurPointId();
        Boolean GetCurPosition(ref RbtPos rbtPos);
        Boolean GetCurPosition(short axisId, ref double pos);
        void SetCurPosition(short pointId);
        void SetCurPosition(short axisId, double pos);
        int RbtMovePos(short posId);
        int RbtMovePos(RbtPos rbtPos);
        int RbtMovePos(string posName);
        void RbtMoveVelStart(short axisId, short sign, double vel);
        void RbtMoveVelStart(short axisId, short sign, double vel, short acc);
        void RbtMoveVelStop(short axisId);
        void SetVelRatio(double ratio);
        Boolean IsDetectHomeSwitch(short axisId);
        AxisStatus[] GetRbtStatus();
    }
}
