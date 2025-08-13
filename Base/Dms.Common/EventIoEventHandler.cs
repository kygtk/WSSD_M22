///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Io State Change Event handler
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    #region Delegate
    public delegate void IoStateChangeEventHandler(object obj, IoStateEventArgs e);
    #endregion

    [Serializable()]
    public class IoStateEventArgs : EventArgs
    {
        #region Fields
        private IoType m_Type;
        private int m_Id;
        #endregion

        #region Properties
        public IoType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }   
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region Constructor
        public IoStateEventArgs()
        { 
        
        }

        public IoStateEventArgs(IoType type, int id)
        {
            this.m_Type = type;
            this.m_Id = id;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Type.ToString() + " - " + m_Id.ToString();
        }
        #endregion
    }
}
