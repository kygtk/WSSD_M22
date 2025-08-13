///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.13
// Author       : Jaehee Hong
// Description  : PC System Information factory class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.PcSystemInfo
{
    public class PcSystemFactory
    {
        private static _PcSystemInfo m_PcSystemInfo = null;
        
        #region Singleton
        public static readonly PcSystemFactory Instance = new PcSystemFactory();
        #endregion

        #region Constructor
        private PcSystemFactory()
        {
        }
        #endregion

        #region Methods
        public _PcSystemInfo GetSystem(PcSystemType pc)
        {
            if (m_PcSystemInfo == null)
            {
                switch (pc)
                {
                    case PcSystemType.APC:
                        m_PcSystemInfo = new BrPc();
                        break;

                    case PcSystemType.General:
                        m_PcSystemInfo = new GeneralPc();
                        break;
                }
            }
            return m_PcSystemInfo;
        }
        #endregion
    }
}
