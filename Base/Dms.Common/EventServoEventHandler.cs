using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    #region Delegate
    public delegate void PositionChangeEventHandler(object obj, PositionChangeEventArgs e);
    public delegate void StatusChangeEventHandler(object obj, StatusChangeEventArgs e);
    #endregion

    public class PositionChangeEventArgs : EventArgs
    {
        private int m_RbtId;
        private RbtPos m_Curpos;

        public int RbtId
        {
            get { return m_RbtId; }
            set { m_RbtId = value; }
        }
        public RbtPos CurPos
        {
            get { return m_Curpos; }
            set { m_Curpos = value; }
        }

        public PositionChangeEventArgs(int rbtId, int axes)
        {
            this.RbtId = rbtId;
            this.CurPos = new RbtPos(axes);
        }
    }

    public class StatusChangeEventArgs : EventArgs
    {
        private int m_RbtId;
        private int m_AxisId;
        private AxisStatus m_Status;

        public int RbtId
        {
            get { return m_RbtId; }
            set { m_RbtId = value; }
        }
        public int AxisId
        {
            get { return m_AxisId; }
            set { m_AxisId = value; }
        }
        public AxisStatus Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        public StatusChangeEventArgs( int rbtId, AxisStatus status )
        {
            this.RbtId = rbtId;
            this.Status = status;
        }
    }
}
