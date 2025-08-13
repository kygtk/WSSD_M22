///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.19
// Author       : jemoon
// Description  : Abstract for BCR
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Common;
using System.Threading;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    abstract public class _BCR : _DeviceAsm
    {
        #region Fields
        protected int m_DataLength = 6;
        protected string[] m_Data = null;
        protected int[] m_UnitStatus = null;        
        protected int m_TimeoutRead = 2 * 1000; //2000 msec
        //for mux mode
        protected MuxMode m_ControlMode = MuxMode.Single;
        protected ushort m_MultiDropCount = 1;
        protected int m_StartPortId = 0;
        protected int[] m_PortIds;
        private string m_FixedSimulateId = "";
        #endregion

        #region Properties
        [Category("DMS : Option"), Description("Data Length")]
        public int DataLength
        {
            get { return m_DataLength; }
            set { m_DataLength = value; }
        }
        [Category("DMS : Option"), Description("msec")]
        public int TimeoutRead
        {
            get { return m_TimeoutRead; }
            set { m_TimeoutRead = value; }
        }
        [Category("DMS : Option"), Description("1:1 or 1:N")]
        public MuxMode ControlMode
        {
            get { return m_ControlMode; }
            set
            {
                m_ControlMode = value;
                if (m_ControlMode == MuxMode.Single)
                {
                    this.MultiDropCount = 1;
                }
            }
        }
        [Category("DMS : Option"), Description("1:1 or 1:N")]
        public ushort MultiDropCount
        {
            get { return m_MultiDropCount; }
            set
            {
                if (m_ControlMode == MuxMode.Single) value = 1;
                if (value <= 0) value = 1;
                else m_MultiDropCount = value;
            }
        }
        [Category("DMS : Option")]
        public int StartPortId
        {
            get { return m_StartPortId; }
            set { m_StartPortId = value; }
        }
        [Category("DMS : Option"), Description("Fixed simulate ID")]
        public string FixedSimulateId
        {
            get { return m_FixedSimulateId; }
            set { m_FixedSimulateId = value; }
        }
        #endregion

        #region Methods
        //485 multi drop으로 구성된 경우, portId에 매칭되는 channel id를 return
        public int FindChannelId(int portId)
        {
            if (m_ControlMode == MuxMode.Single) return 0;

            int id = -1;    // -1 : not found
            for (int i = 0; i < m_MultiDropCount; i++)
            {
                if (m_PortIds[i] == portId)
                {
                    id = i;
                    break;
                }
            }

            return id;
        }
        //485 multi drop으로 구성된 경우, channel 에 매칭되는 port id를 return
        public int FindPortId(int channelId)
        {
            return m_PortIds[channelId];
        }

        protected string MakeSimuateData(int portId)
        {
			if (!string.IsNullOrEmpty(m_FixedSimulateId))
			{
				return m_FixedSimulateId + string.Format("{0:d2}", portId + 1);
			}
			else
			{
				Random rand = new Random();
				return string.Format("{0:d3}", rand.Next(1, 100));
			}
        }

        public void MakeData(int portId, string data)
        {
            int id = FindChannelId(portId);
            if (m_Simul.Device)
            {
                m_Data[id] = MakeSimuateData(portId);
            }
            else
            {
                m_Data[id] = data;
            }
        }

        public void SetData(int portId, string data)
        {
            int id = FindChannelId(portId);
            m_Data[id] = data;
        }

        public string GetData(int portId)
        {
            int id = FindChannelId(portId);
            return m_Data[id];
        }

        public int GetUnitStatus(int portId)
        {
            int id = FindChannelId(portId);
            return m_UnitStatus[id];
        } 
        #endregion

        #region Abstract Methods
        abstract public int Reading(int portId);
        abstract public void DirectReading(int portId);
        #endregion
    }
}
