///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.19
// Author       : jemoon
// Description  : Abstract for Cassette mapping unit
//-------------------------------------------------------------------------
// Revison History
// * 2009.10.20 : jemoon - add mux mode
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Device
{
    public enum MappingSimulateMode
    { 
        Empty,
        Full,
        Random,
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    abstract public class _CstMappingUnit : _DeviceAsm
    {
        #region Fields
        protected int m_MaxSlotCount = 25;
        protected MappingStatus[][] m_MappingStatus;
        protected int m_TimeoutMapping = 5 * 1000;  //msec
        protected int m_TimeoutHoming = 5 * 1000;  //msec
        private int m_DelayForMapping = 2 * 1000;
        protected MappingSimulateMode m_MapSimulateMode = MappingSimulateMode.Empty;
        protected int[] m_UnitStatus;
        //for mux mode
        protected MuxMode m_ControlMode = MuxMode.Single;
        protected ushort m_MultiDropCount = 1;
        protected int m_StartPortId = 0;
        protected int[] m_PortIds;
        #endregion

        #region Properties
        [Category("DMS : Option")]
        public int MaxSlotCount
        {
            get { return m_MaxSlotCount; }
            set { m_MaxSlotCount = value; }
        }
        [Category("DMS : Option")]
        public MappingSimulateMode MapSimulateMode
        {
            get { return m_MapSimulateMode; }
            set { m_MapSimulateMode = value; }
        }
        [Category("DMS : Option"), Description("timeout for mapping(msec)")]
        public int TimeoutMapping
        {
            get { return m_TimeoutMapping; }
            set { m_TimeoutMapping = value; }
        }
        [Category("DMS : Option"), Description("timeout for homing(msec)")]
        public int TimeoutHoming
        {
            get { return m_TimeoutHoming; }
            set { m_TimeoutHoming = value; }
        }
        [Category("DMS : Option"), Description("Delay Time for Mapping(msec)")]
        public int DelayForMapping
        {
            get { return m_DelayForMapping; }
            set { m_DelayForMapping = value; }
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
        #endregion

        #region Methods
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

        public int FindPortId(int channelId)
        {
            return m_PortIds[channelId];
        }

        public int GetUnitStatus(int portId)
        {
            int id = FindChannelId(portId);
            return m_UnitStatus[id];
        }

        public MappingStatus[] GetGlassMappingStatus(int portId)
        {
            int id = FindChannelId(portId);
            return m_MappingStatus[id];
        }

        public MappingStatus GetGlassMappingStatus(int portId, int slotId)
        {
            int id = FindChannelId(portId);
            return m_MappingStatus[id][slotId];
        }

        protected void MakeSimulMappingStatus(int portId)
        {
            if (m_Simul.Device)
            {
                int id = FindChannelId(portId);
                switch (m_MapSimulateMode)
                {
                    case MappingSimulateMode.Empty:
                        {
                            for (int i = 0; i < m_MaxSlotCount; i++)
                            {
                                m_MappingStatus[id][i] = MappingStatus.Off;
                            }
                        }
                        break;
                    case MappingSimulateMode.Full:
                        {
                            for (int i = 0; i < m_MaxSlotCount; i++)
                            {
                                m_MappingStatus[id][i] = MappingStatus.On;
                            }
                        }
                        break;
                    case MappingSimulateMode.Random:
                        {
                            Random autoRan = new Random();
                            for (int i = 0; i < m_MaxSlotCount; i++)
                            {
                                int randomValue = autoRan.Next();
                                m_MappingStatus[id][i] = (randomValue % 3) > 1 ? MappingStatus.On : MappingStatus.Off;
                            }
                        }
                        break;

                }
            }
        }

        protected void MakeSimulMappingStatus(int portId, int slotId)
        {
            if (m_Simul.Device)
            {
                int id = FindChannelId(portId);
                switch (m_MapSimulateMode)
                {
                    case MappingSimulateMode.Empty:
                        {
                            m_MappingStatus[id][slotId] = MappingStatus.Off;
                        }
                        break;
                    case MappingSimulateMode.Full:
                        {
                            m_MappingStatus[id][slotId] = MappingStatus.On;
                        }
                        break;
                    case MappingSimulateMode.Random:
                        {
                            Random autoRan = new Random();
                            m_MappingStatus[id][slotId] = autoRan.Next(0, 1) > 0 ? MappingStatus.On : MappingStatus.Off;
                        }
                        break;
                }
            }
        } 
        #endregion

        #region Abstract Methods
        abstract public int Homing(int portId);
        abstract public int Mapping(int portId); // 전체 mapping
        abstract public int Mapping(int portId, int slotId); // 특정 Slot mapping
        abstract public int CheckUnitStatus(int portId);
        abstract public int MakeGlassMappingStatus(int portId);   //Make all Mapping data
        abstract public int MakeGlassMappingStatus(int portId, int slotId);   //Make slot Mapping data 
        abstract public bool IsMappingDriveUnitFw();
        abstract public bool IsMappingDriveUnitBw();
        #endregion
    }
}
