///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Collection of CvMotor
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.18 - jemoon : code review - GenericCollecion 상속
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Device
{
    public class MotorControl
    {
        #region Fields
        private _GenericCollection<_Motor> m_Items;
        #endregion

        #region Properties
        public _GenericCollection<_Motor> Items
        {
            get { return m_Items; }
        }
        #endregion

        #region Constructor
        public MotorControl(_GenericCollection<_Motor> collection)
        {
            m_Items = collection;
        }
        #endregion

        #region Methods
        public bool IsFw(Logic logic)
        {
            if (logic == Logic.AND)
            {   //전부다 Fw 인가?
                bool run = true;
                foreach (_Motor motor in m_Items)
                {
                    run &= motor.IsTurnCw();
                    if (!run) break;
                }

                return run;
            }
            else
            {   //하나라도 Fw 인가?
                bool run = false;
                foreach (_Motor motor in m_Items)
                {
                    run |= motor.IsTurnCw();
                    if (run) break;
                }

                return run;
            }
        }

        public bool IsBw(Logic logic)
        {
            if (logic == Logic.AND)
            {   // 전부다 Bw 인가?
                bool run = true;
                foreach (_Motor motor in m_Items)
                {
                    run &= motor.IsTurnCcw();
                    if (!run) break;
                }

                return run;
            }
            else
            {   //하나라도 Bw인가?
                bool run = false;
                foreach (_Motor motor in m_Items)
                {
                    run |= motor.IsTurnCcw();
                    if (run) break;
                }

                return run;
            }
        }

        public bool IsStop(Logic logic)
        {
            if (logic == Logic.AND)
            {   //전부다 정지인가?
                bool stop = true;
                foreach (_Motor motor in m_Items)
                {
                    stop &= motor.IsStop();
                    if (!stop) break;
                }

                return stop;
            }
            else
            {   //하나라도 정지인가?
                bool stop = false;
                foreach (_Motor motor in m_Items)
                {
                    stop |= motor.IsStop();
                    if (stop) break;
                }

                return stop;
            }
        }
        #endregion
    }
}
