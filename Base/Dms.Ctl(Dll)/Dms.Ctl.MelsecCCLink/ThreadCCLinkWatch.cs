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
using System.Diagnostics;

namespace Dms.Ctl
{
    public class ThreadCCLinkWatch : XSequence
    {
        private MelsecCCLink m_Control;

        public ThreadCCLinkWatch(int scanTime, MelsecCCLink control)
        {
            m_ScanTime = scanTime;
            m_Control = control;
        }

        bool _tarce = false;
        uint m_temp = 0;
        public override void Sequence()
        {   
            try
            {
                if (_tarce) m_temp = XFunc.GetTickCount();

                Thread.Sleep(m_ScanTime);

                if (m_Control.Initialized && !m_Control.Simul.IoController)
                {

                    if (m_Control.ModuleConfig.BusType == Dms.Util.IODefine.FieldBusType.MitsubishiCClink) // 11.02.19 minhan 이 타입이면 기존 방식 그대로.
                    {
                        if (m_Control.DeviceState == ActiveState.Run) 
                        {
                            m_Control.CheckLinkState();
                            m_Control.SeqInitStation();
                            m_Control.UpdateIoStatus();
                        }
                    }
                    else // Crevis
                    {
                        m_Control.CheckCrevisLinkState(); // 여기는 isopen을 보고 하였슴.
                        m_Control.ActiveStateMonitor();

                        if (m_Control.DeviceState != ActiveState.Stop) // 11.02.23 minhan
                            // stop이라면 connect가 완전히 나간 상황이라서 아예 할필요가 없을 것 같네.
                        {
                            m_Control.UpdateCrevisIoStatus();
                        }
                    }

                    if (_tarce)
                    {
                        m_temp = XFunc.GetTickCount() - m_temp;
                        string tarce = string.Format("{0}", m_temp.ToString());
                        Trace.WriteLine(tarce);
                    }
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
    }
}
