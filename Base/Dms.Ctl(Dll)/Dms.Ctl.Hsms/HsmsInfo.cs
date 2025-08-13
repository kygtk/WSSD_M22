using System;
using System.Collections.Generic;
using System.Text;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05
// Author       : Kim Youngsik
// Description  : HSMS Connection Information
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class HsmsInfo
    {
        #region Fields
        private bool m_HsmsConnect;
        #endregion

        #region Properties
        public bool HsmsConnect
        {
            get { return m_HsmsConnect; }
            set { m_HsmsConnect = value; }
        }
        #endregion

        //#region Singleton
        //static public readonly HsmsInfo Instance = new HsmsInfo();
        //#endregion

        #region Methods
        public HsmsInfo()
        {
            m_HsmsConnect = false;
        }

        public void Reset()
        {
            m_HsmsConnect = false;
        }
        #endregion
    }
}
