///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : GenericCollection of items
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.20 - jemoon : code review - GenericCollecion 상속
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using Dms.Common;

namespace Dms.Device
{
    class ThreadLevelSensorConfirm : XSequence
    {
        private IServerManager m_Server;
        private _GenericCollection<LevelSensor> m_LevelSensors;
        private new List<XSeqFunction> m_SeqFunctions = new List<XSeqFunction>();

        public ThreadLevelSensorConfirm(int scanTime, _GenericCollection<LevelSensor> levelsensor)
            : base(scanTime)
        {
            m_Server = levelsensor.ServerManager;
            m_LevelSensors = levelsensor;
            foreach (LevelSensor sensor in m_LevelSensors)
            {
                m_SeqFunctions.Add(new SeqLevelSensorConfirm(sensor));
            }
        }

        ~ThreadLevelSensorConfirm()
        {
        }

        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
    }

    public class SeqLevelSensorConfirm : XSeqFunction
    {
        private IServerManager m_Server;
        private LevelSensor LevelSensor;
        private bool m_TimerInit;

        public SeqLevelSensorConfirm(LevelSensor levelsensor)
        {
            LevelSensor = levelsensor;
            m_Server = LevelSensor.ServerManager;

            string funName = string.Format("LEVEL{0}", LevelSensor.Id);
            m_SeqFunName = funName;
        }

        public override int Do()
        {
            if (m_Server.State != ActiveState.Run) return -1;

            int setupConfirmTime = LevelSensor.SetupLevelSensorConfirmTime.GetValue<int>() * 1000;

            bool sensorDetected = LevelSensor.IsDetected();
            bool sensorConfirmed = LevelSensor.IsConfirmed();

            // jemoon : confirm 상태가 아닐때 센서가 첫방 감지되면 startTick을 초기화한다.
            if (sensorDetected && !m_TimerInit)
            {
                m_TimerInit = true;
                m_StartTicks = XFunc.GetTickCount();
            }

            if (sensorDetected && !sensorConfirmed && (GetElapsedTicks() > setupConfirmTime))
            {
                LevelSensor.Confirmed = true;
            }
            else if (!sensorDetected && sensorConfirmed)
            {
                LevelSensor.Confirmed = false;
                m_TimerInit = false;
            }

            return -1;
        }
    }
}
