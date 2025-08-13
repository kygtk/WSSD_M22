using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        public static class ServoCtl
        {
            #region Enums
            public enum ProfileType
            {
                Trapezoidal,
                SCurve,
            }
            #endregion

            #region Fields
            private static bool m_Initialized = false;

            private static CoreMotion m_CoreMotion;
            private static CoreMotionStatus m_CoreMotionStatus;
            #endregion

            #region Properties
            public static bool Initialized
            {
                get { return m_Initialized; }
            }
            #endregion

            #region Methods
            internal static void Initialize()
            {
                m_CoreMotion = new CoreMotion(m_Wmx);
                m_CoreMotionStatus = new CoreMotionStatus();

                m_Initialized = true;
            }

            internal static void Uninitialize()
            {
                m_CoreMotion.Dispose();
                m_CoreMotionStatus = null;

                m_Initialized = false;
            }

            public static int GetServoStatus(int axisNo, out CoreMotionAxisStatus cmAxisStatus)
            {
                int eCode = m_CoreMotion.GetStatus(ref m_CoreMotionStatus);
                if (eCode == 0)
                    cmAxisStatus = m_CoreMotionStatus.AxesStatus[axisNo];
                else
                    cmAxisStatus = new CoreMotionAxisStatus();

                return eCode;
            }

            public static int GetServoAxisCommandMode(int axisNo, out AxisCommandMode mode)
            {
                mode = AxisCommandMode.Position;
                int eCode = m_CoreMotion.AxisControl.GetAxisCommandMode(axisNo, ref mode);

                return eCode; ;
            }

            public static int SetServoAxsisCommandMode(int axisNo, AxisCommandMode mode)
            {
                int eCode = m_CoreMotion.AxisControl.SetAxisCommandMode(axisNo, mode);

                return eCode;
            }

            public static int ServoOn(int axisNo)
            {
                int eCode = m_CoreMotion.AxisControl.SetServoOn(axisNo, 1);
                return eCode;
            }

            public static int ServoOff(int axisNo)
            {
                int eCode = m_CoreMotion.AxisControl.SetServoOn(axisNo, 0);
                return eCode;
            }

            public static int AlarmReset(int axisNo)
            {
                int eCode = m_CoreMotion.AxisControl.ClearAmpAlarm(axisNo);
                return eCode;
            }

            public static int GetGearRatio(int axisNo, out double numerator, out double denominator)
            {
                numerator = 0;
                denominator = 0;
                int eCode = m_CoreMotion.Config.GetGearRatio(axisNo, ref numerator, ref denominator);
                return eCode;
            }

            public static int SetGearRatio(int axisNo, double numerator, double denominator)
            {
                int eCode = m_CoreMotion.Config.SetGearRatio(axisNo, numerator, denominator);
                return eCode;
            }

            public static int SetPosition(int axisNo, double pos)
            {
                int eCode = m_CoreMotion.Home.SetFeedbackPos(axisNo, pos);
                return eCode;
            }

            public static int Start_Absolute(int axisNo, int target, int vel, int acc, int dec, ProfileType type)
            {
                Motion.PosCommand pc = new Motion.PosCommand();
                pc.Profile.Type = (WMX3ApiCLR.ProfileType)type;
                pc.Axis = axisNo;
                pc.Target = target;
                pc.Profile.Velocity = vel;
                pc.Profile.Acc = acc;
                pc.Profile.Dec = dec;

                int eCode = m_CoreMotion.Motion.StartPos(pc);
                return eCode;
            }

            public static int Start_Relative(int axisNo, int target, int vel, int acc, int dec, ProfileType type)
            {
                Motion.PosCommand pc = new Motion.PosCommand();
                pc.Profile.Type = (WMX3ApiCLR.ProfileType)type;
                pc.Axis = axisNo;
                pc.Target = target;
                pc.Profile.Velocity = vel;
                pc.Profile.Acc = acc;
                pc.Profile.Dec = dec;

                int eCode = m_CoreMotion.Motion.StartMov(pc);
                return eCode;
            }

            public static int Start_Velocity(int axisNo, int vel, int acc, int dec)
            {
                Velocity.VelCommand vc = new Velocity.VelCommand();
                vc.Profile.Type = (WMX3ApiCLR.ProfileType)ProfileType.Trapezoidal;
                vc.Axis = axisNo;
                vc.Profile.Velocity = vel;
                vc.Profile.Acc = acc;
                vc.Profile.Dec = dec;

                int eCode = m_CoreMotion.Velocity.StartVel(vc);
                return eCode;
            }

            public static int Stop_P(int axisNo)
            {
                int eCode = m_CoreMotion.Motion.Stop(axisNo);
                return eCode;
            }
            public static int Stop_V(int axisNo)
            {
                int eCode = m_CoreMotion.Velocity.Stop(axisNo);
                return eCode;
            }

            public static int Stop(int axisNo, int dec)
            {
                int eCode = m_CoreMotion.Motion.Stop(axisNo, dec);
                return eCode;
            }

            public static int EStop()
            {
                int eCode = m_CoreMotion.ExecEStop(EStopLevel.Level1);
                return eCode;
            }

            public static int EStopRelease()
            {
                int eCode = m_CoreMotion.ReleaseEStop();
                return eCode;
            }

            public static int Jog(int axisNo, int vel, int acc, int dec)
            {
                Motion.JogCommand jc = new Motion.JogCommand();
                jc.Profile.Type = (WMX3ApiCLR.ProfileType)ProfileType.Trapezoidal;
                jc.Axis = axisNo;
                jc.Profile.Velocity = vel;
                jc.Profile.Acc = acc;
                jc.Profile.Dec = dec;

                int eCode = m_CoreMotion.Motion.StartJog(jc);
                return eCode;
            }

            public static int HomingParamInit(int axisNo, Config.HomeType homeType, Config.HomeDirection homeDirection,
                                             double homeFastVel, double homeFastAcc, double homeFastDec,
                                             double homeSlowVel, double homeSlowAcc, double homeSlowDec,
                                             double homeShiftPos, double homeShiftVel, double homeShiftAcc, double homeShiftDec)
            {
                Config.HomeParam homeparam = new Config.HomeParam();

                m_CoreMotion.Config.GetHomeParam(axisNo, ref homeparam);

                homeparam.HomeType = homeType;
                homeparam.HomeDirection = homeDirection;

                homeparam.HomingVelocityFast = homeFastVel;
                homeparam.HomingVelocityFastAcc = homeFastAcc;
                homeparam.HomingVelocityFastDec = homeFastDec;

                homeparam.HomingVelocitySlow = homeSlowVel;
                homeparam.HomingVelocitySlowAcc = homeSlowAcc;
                homeparam.HomingVelocitySlowDec = homeSlowDec;

                homeparam.HomeShiftDistance = homeShiftPos;
                homeparam.HomeShiftVelocity = homeShiftVel;
                homeparam.HomeShiftAcc = homeShiftAcc;
                homeparam.HomeShiftDec = homeShiftDec;

                int eCode = m_CoreMotion.Config.SetHomeParam(axisNo, homeparam);

                return eCode;
            }

            public static int Homing(int axisNo)
            {
                int eCode = m_CoreMotion.Home.StartHome(axisNo);
                return eCode;
            }

            public static int SetSync(int masterAxisNo, int slaveAxisNo)
            {
                int eCode = m_CoreMotion.Sync.SetSyncMasterSlave(masterAxisNo, slaveAxisNo);
                return eCode;
            }

            public static int SetUnsync(int slaveAxisNo)
            {
                int eCode = m_CoreMotion.Sync.ResolveSync(slaveAxisNo);
                return eCode;
            }
            #endregion
        }
    }
}
