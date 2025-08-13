using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Cim.Common
{
    public class CTTimeInfo
    {
        int m_No;
        private string m_WaitSF;
        private XTimer m_Timer;

        public CTTimeInfo(int number)
        {
            m_No = number;
            m_WaitSF = "";
            m_Timer = new XTimer("Timer " + m_No.ToString() + ": CT Timer");
        }

        public string WaitSF
        {
            get { return m_WaitSF; }
            set { m_WaitSF = value; }
        }

        public void SetCTTime(int Time)
        {
            m_Timer.Start(Time * 1000);
        }

        public void Pause()
        {
            m_Timer.Pause();
        }

        public bool Over()
        {
            return m_Timer.Over;
        }
    }
}
