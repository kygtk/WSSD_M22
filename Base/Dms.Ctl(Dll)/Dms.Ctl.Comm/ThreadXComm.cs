using System;
using Dms.Common;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Dms.Ctl.Comm
{
    class ThreadXComm : XSequence
    {
        public ThreadXComm(XComm xComm)
        {
            m_XComm = xComm;
        }

        private XComm m_XComm;

        public override void Sequence()
        {
            try
            {
                Thread.Sleep(10);
                m_XComm.WriteDo();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
