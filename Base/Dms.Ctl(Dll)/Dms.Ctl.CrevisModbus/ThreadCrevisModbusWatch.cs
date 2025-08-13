///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.09
// Author       : jemoon
// Description  : BrModbusTcpIo
//-------------------------------------------------------------------------
// Revison History
// * 2010.01.12 : jemoon - master별로 Thread 별도 생성하도록 수정
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Ctl
{
    public class ThreadCrevisModbusWatch : XSequence
    {
        private CrevisModbusTcpIo m_Control;
		private int m_MasterId;

        public ThreadCrevisModbusWatch(int scanTime, CrevisModbusTcpIo control, int masterId)
        {
            m_ScanTime = scanTime;
            m_Control = control;
			m_MasterId = masterId;
        }

        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

				//m_Control.ActiveStateMonitor(m_MasterId);

				bool run = true;
				run &= m_Control.Initialized;
				run &= m_Control.DeviceState == ActiveState.Run;
				run &= !m_Control.Simul.IoController;
                if (run)
                {
                    SeqPolling(m_MasterId);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }

        #region Sequence    
        public void SeqPolling(int masterId)
        {
            // Update 속도 개선을 위해 여러번 수행함
            if (m_Control.DeviceState == ActiveState.Run)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (m_Control.DeviceState == ActiveState.Run)
                    {
						m_Control.UpdateIoStatus(masterId);
                    }
                }
            }
        }
        #endregion
    }
}
