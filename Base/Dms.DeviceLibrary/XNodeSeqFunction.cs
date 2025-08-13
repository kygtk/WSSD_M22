///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 
// Author       : Koosangseo
// Description  : XNodeSeqFunction Class
//-------------------------------------------------------------------------
// Revison History
// * 2009.10.22 : jemoon - 상호참조 문제 해결을 위해 위치 이동
//                Dms.Common -> Dms.DeviceLibrary
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Dms.Common;

namespace Dms.DeviceLibrary
{
    public class XNodeSeqFunction : DmsNode
    {
        protected bool m_IsUse = false;
        protected int m_SeqNo = 0;
        protected int m_ReturnSeqNo = 0;
        protected int m_AlarmId = 0;
        protected string m_SeqFunName;
        protected UInt32 m_StartTicks = XFunc.GetTickCount();
        protected List<UInt32> m_ExtraStartTicks = new List<UInt32>();
        
        public int SeqNo
        {
            get { return m_SeqNo; }
            set { m_SeqNo = value; }
        }

        [Browsable(false), XmlIgnore()]
        public int ReturnSeqNo
        {
            get { return m_ReturnSeqNo; }
            set { m_ReturnSeqNo = value; }
        }

        [Browsable(false), XmlIgnore()]
        public int AlarmId
        {
            get { return m_AlarmId; }
            set { m_AlarmId = value; }
        }

        public string SeqFunName
        {
            get { return m_SeqFunName; }
            set { m_SeqFunName = value; }
        }

        public bool IsUse
        {
            get { return m_IsUse; }
            set { m_IsUse = value; }
        }

        //public DateTime StartTime
        //{
        //    get { return m_StartTime; }
        //    set { m_StartTime = value; }
        //}

        [Browsable(false), XmlIgnore()]
        public UInt32 StartTicks
        {
            get { return m_StartTicks; }
            set { m_StartTicks = value; }
        }

        [Browsable(false), XmlIgnore()]
        public List<UInt32> StartTicksExtra
        {
            get { return m_ExtraStartTicks; }
            set { m_ExtraStartTicks = value; }
        }

        public XNodeSeqFunction()
        {
            InitSeq();
        }

        public virtual void InitSeq()
        {
            this.SeqNo = 0;
        }

        public virtual int Do()
        {
            int result = -1;
            int nSeqNo = this.SeqNo;        

            switch(nSeqNo)
            {
                case 0:
                    break;
            }

            this.SeqNo = nSeqNo;
            
            return result;
        }

        //public double GetElapsedTicks()
        //{
        //    TimeSpan diff = DateTime.Now - StartTime;
        //    return diff.TotalMilliseconds;
        //}

        public UInt32 GetElapsedTicks()
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

        public UInt32 GetElapsedTicks(int id)
        {
            return XFunc.GetTickCount() - m_ExtraStartTicks[id];
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteConfiguration()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
