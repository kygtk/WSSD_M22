using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public class XSeqFunction
    {
        #region Feilds
        protected int m_SeqNo = 0;
        protected int m_ReturnSeqNo = 0;
        protected int m_AlarmId = 0;
        protected string m_SeqFunName;
        protected uint m_StartTicks = XFunc.GetTickCount();
        protected List<uint> m_ExtraStartTicks = new List<uint>();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public XSeqFunction()
        {
            InitSeq();
        }
        #endregion

        #region Virtual Sequence Function Methods
        public virtual void InitSeq()
        {
            m_SeqNo = 0;
        }

        public virtual int Do()
        {
            int result = -1;
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    break;
            }

            m_SeqNo = nSeqNo;

            return result;
        }
        #endregion

        #region Methods
        public uint GetElapsedTicks()
        {
            return XFunc.GetTickCount() - m_StartTicks;
        }

        public void SetExtraStartTicks(int count)
        {
            for (int i = 0; i < count; i++)
            {
                m_ExtraStartTicks.Add(XFunc.GetTickCount());
            }
        }

        public uint GetElapsedTicks(int id)
        {
            return XFunc.GetTickCount() - m_ExtraStartTicks[id];
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return this.GetType().Name;
        }
        #endregion
    }
}
