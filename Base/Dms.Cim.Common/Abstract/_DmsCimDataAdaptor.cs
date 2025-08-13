using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dms.Common;

namespace Dms.Cim.Common
{
    //Data별로 Log를 생성하지 않고 참조로 설정 가능 하도록 
    abstract public class _DmsCimDataAdaptor : _DmsDataAdaptor
    {
        #region Fields
        #endregion

        #region Properties
        public XLog Log
        {
            get { return m_Log; }
            set { m_Log = value; }
        }
        #endregion

        #region Constructor
        public _DmsCimDataAdaptor() 
            : base(false)
        {
        }

        public _DmsCimDataAdaptor(bool createLogfile)
            : base(createLogfile)
        { 
        }
        #endregion
        
        #region Methods
        #endregion
    }
}
