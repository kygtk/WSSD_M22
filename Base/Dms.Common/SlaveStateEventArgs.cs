using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dms.Common
{
    #region Delegate
    public delegate void SlaveStateChangeEventHandler(object obj, SlaveStateEventArgs e);
    #endregion

    [Serializable()]
   public class SlaveStateEventArgs:EventArgs
    {
        #region Fields
        private EcSlaveItemType m_Type;
        private int m_Id;
        #endregion

        #region Properties
        public EcSlaveItemType Type
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
        public SlaveStateEventArgs()
        {

        }

        public SlaveStateEventArgs(EcSlaveItemType type, int id)
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
