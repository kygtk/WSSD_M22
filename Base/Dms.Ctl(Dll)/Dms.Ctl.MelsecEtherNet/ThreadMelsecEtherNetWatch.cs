///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.18
// Author       : jemoon
// Description  : ThreadCCLinkWatch
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Ctl
{
    public class ThreadMelsecEtherNetWatch : XSequence
    {
        private MelsecEtherNet m_Control;
		private int m_MasterId;

        public ThreadMelsecEtherNetWatch(int scanTime, MelsecEtherNet control, int masterId)
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

                if (m_Control.Initialized && !m_Control.Simul.IoController)
                {
                    if (m_Control.DeviceState == ActiveState.Run)
                    {
						m_Control.UpdateIoStatus(m_MasterId);
                    }

					m_Control.ActiveStateMonitor(m_MasterId);
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
    }
}
