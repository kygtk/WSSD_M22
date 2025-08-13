///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.21
// Author       : jemoon
// Description  : CvUnit class typeA
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Device
{
    ///////////////////////////////////////////////////////////////////////////
    // Components
    // * InSensor : 1
    // * OutSenssor : 1
    // * CvMotors : n
    // * FwDecelSensor : 1
    // * BwDecelSensor : 1
    // * BrokenFix : 1
    // * BrokenScan : 1
    // * RbtInterlockSensor : 1
    // * AlignerFront : 1
    // * AlignerRear : 1
    // * AlignerLeft : 1
    // * AlignerRight : 1
    // * UpDnLeft : 1
    // * UpDnRight : 1
    ///////////////////////////////////////////////////////////////////////////
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class CvUnitA : CvUnit
    {
        #region Fields
        private Sensor m_RbtInterlockSensor;
        private Cylinder m_AlignerFront;
        private Cylinder m_AlignerRear;
        private Cylinder m_AlignerLeft;
        private Cylinder m_AlignerRight;
        private Cylinder m_UpDnLeft;
        private Cylinder m_UpDnRight;
        private AbsodexMotor m_MotorAbsodex;

        public Alarm ALM_RbtInterlock;
        #endregion

        #region Properties
        [Category("Setting : TypeA")]
        public Sensor RbtInterlockSensor
        {
            get { return m_RbtInterlockSensor; }
            set { m_RbtInterlockSensor = value; }
        }
        [Category("Setting : TypeA")]
        public Cylinder AlignerFront
        {
            get { return m_AlignerFront; }
            set { m_AlignerFront = value; }
        }
        [Category("Setting : TypeA")]
        public Cylinder AlignerRear
        {
            get { return m_AlignerRear; }
            set { m_AlignerRear = value; }
        }
        [Category("Setting : TypeA")]
        public Cylinder AlignerLeft
        {
            get { return m_AlignerLeft; }
            set { m_AlignerLeft = value; }
        }
        [Category("Setting : TypeA")]
        public Cylinder AlignerRight
        {
            get { return m_AlignerRight; }
            set { m_AlignerRight = value; }
        }
        [Category("Setting : TypeA")]
        public Cylinder UpDnLeft
        {
            get { return m_UpDnLeft; }
            set { m_UpDnLeft = value; }
        }
        [Category("Setting : TypeA")]
        public Cylinder UpDnRight
        {
            get { return m_UpDnRight; }
            set { m_UpDnRight = value; }
        }
        [Category("Setting : TypeA")]
        public AbsodexMotor MotorAbsodex
        {
            get { return m_MotorAbsodex; }
            set { m_MotorAbsodex = value; }
        }
        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            bool ok = true;

            ok &= (base.Initialize() == DmsErrors.Success);

            if (ok)
            {
                if (m_RbtInterlockSensor != null)
                {
                    ALM_RbtInterlock = new Alarm(this.Name + " Robot Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }
            }

            m_Initialized = ok;
            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }
        #endregion
    }
}
