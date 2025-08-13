using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Ctl;

namespace Dms.Ctl
{
    public class clsThreadMelsec : XSequence
    {
        public clsThreadMelsec(Melsec melsec)
        {
            m_Melsec = melsec;
        }
        private Melsec m_Melsec;

        public override void Sequence()
        {
            try
            {
                m_Melsec.Monitor(m_Melsec.NetworkNo, m_Melsec.StationNo);
//                m_Melsec.Scan();

                Thread.Sleep(10);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
    }
}
