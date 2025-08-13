//=========================================================
// Copyright    : DMS Co., Ltd
// Create Date  : 2025.04.16
// Author       : byeongmin
// Description  : 기존 ICtlDevice로 호환이 불가능한 Module(AP, Servo 등)을 제어하기 위한 Interface
//---------------------------------------------------------
// Revison History
// * 2025.04.16 : Create
//=========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Common
{
    public interface ICtlDevice_Adv : ICtlDevice
    {
        #region Properties
        int ServoCount { get; }
        int BldcCount { get; }
        int InverterCount { get; }
        int ApCount { get; }
        #endregion

        #region Methods
        #region Servo Control
        int ServoOn(int index);
        int ServoOff(int index);

        bool ServoIsOn(int index);
        bool ServoIsDone(int index);
        bool ServoGetHomeSwitch(int index);
        bool ServoGetLimitPSwitch(int index);
        bool ServoGetLimitMSwitch(int index);
        AxisEvent ServoGetAxisState(int index);

        int ServoMove_R(int index, double dist, double vel, double acc);
        int ServoMove_S(int index, double pos, double vel, double acc);
        int ServoMove_T(int index, double pos, double vel, double acc, double dec);
        int ServoMove_V(int index, double spd, double acc, double dec);

        int ServoHoming(int index);

        int ServoStop(int index);
        int ServoEStop(int index);
        int ServoEStopRelease(int index);

        bool ServoIsAlarm(int index);
        int ServoAlarmReset(int index);

        int ServoSetPosition(int index, double pos);
        double ServoGetPosition(int index);
        double ServoGetVelocity(int index);

        int ServoSetAxisCommandMode(int index, int mode);

        int ServoSetSync(int index, int slaveindex);
        int ServoUnsync(int slaveindex);
        #endregion

        #region BLDC Control
        double BldcGetLoadFactor(int index);
        short BldcGetCurrentRPM(int index);
        bool BldcGetIsTurnFw(int index);
        bool BldcGetIsTurnBw(int index);
        bool BldcGetIsAlarm(int index);
        void BldcSetRotateFw(int index, bool op);
        void BldcSetRotateBw(int index, bool op);
        void BldcSetAlarmReset(int index, bool op);
        void BldcSetAccTime(int index, ushort val);
        void BldcSetDecTime(int index, ushort val);
        void BldcSetTargetRPM(int index, ushort val);
        #endregion

        #region Inverter Control
        bool InverterGetIsAlarm(int index);
        bool InverterGetIsRun(int index);
        double InverterGetCurrentFrequency(int index);

        void InverterSetAlarmReset(int index, bool op);
        void InverterSetRun(int index, bool op);
        void InverterSetTargetFrequency(int index, double val);
        #endregion

        #region AP Control
        void ApSetPeer(int index, int channel, PeerType type, int peerid);

        PairingState ApGetPairingState(int index);

        #region DIW
        short ApDiwReadFlowValue(int index);
        short ApDiwReadPressValue(int index);
        #endregion

        #region CDA
        short ApCdaReadFlowValue(int index);
        short ApCdaReadPressValue(int index);
        #endregion

        #region SmartDamper
        SmartDamperMode ApSmartDamperReadSmartDamperMode(int index);
        ushort ApSmartDamperReadTargetPressure(int index);
        ushort ApSmartDamperReadTargetPressureHysteresis(int index);
        ushort ApSmartDamperReadCurrentValveAngle(int index);
        ushort ApSmartDamperReadCurrentPressure(int index);
        ushort ApSmartDamperReadAlarmCode(int index);
        bool ApSmartDamperWriteSmartDamperMode(int index, SmartDamperMode mode);
        bool ApSmartDamperWriteTargetPressure(int index, ushort value);
        bool ApSmartDamperWriteTargetPressureHysteresis(int index, ushort value);
        bool ApSmartDamperWriteTargetValveAngle(int index, ushort value);
        #endregion

        #region D40A
        ushort ApD40AReadState(int index);
        #endregion

        #region D4SL
        bool ApD4SLIsOpened(int index, int channel);
        bool ApD4SLIsLocked(int index, int channel);
        void ApD4SLSetLock(int index, int channel, bool op);
        void ApD4SLSetLockAll(int index, bool op);
        #endregion

        #region LFC
        ushort ApLfcReadFlowValue(int index);
        ushort ApLfcReadPressValue(int index);
        ushort ApLfcReadCurrentOpenRate(int index);
        bool ApLfcWriteTargetOpenRate(int index, ushort value);
        #endregion

        #region Manometer
        short ApManometerReadExhaustValue(int index);
        #endregion

        #region LCT
        ushort ApLctReadLevel1Value(int index);
        ushort ApLctReadLevel2Value(int index);
        ushort ApLctReadConsistenceValue(int index);
        #endregion
        #endregion
        #endregion
    }
}
