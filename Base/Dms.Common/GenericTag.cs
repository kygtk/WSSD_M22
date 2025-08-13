///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Dms Tag Class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Dms.Common
{

    /// <summary>
    /// Class : GenericTag
    /// </summary>
    [Serializable()]
    public class GenericTag
    {
        #region Fields
        private string m_Key = "";
        private string m_Value = "";
        #endregion

        #region Properties
        [ReadOnly(true)]
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        [ReadOnly(true)]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        } 
        #endregion

        #region Constructor
        public GenericTag()
        {
        }

        /// <summary>
        /// key, value의 초기값을 받아 생성함. - jemoon : 080109
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
		public GenericTag(string key, string value)
        {
            m_Key = key;
            m_Value = value;
        } 
    	#endregion

        #region override
        public override string ToString()
        {
            return m_Key + " : " + m_Value;
        }
        #endregion
    }
}
