using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    #region Enum
    public enum PortStatus
    {
        None,
        LoadRequest,
        PreLoad,
        CstOn,
        LoadComplete,
        Reject,
        Wait,
        CancelEnable,
        InProcess,
        ProcessEnd,
        Aborting,
        Abort,
        UnloadRequest,
        UnloadComplete
    }

    public enum PortUsage
    {
        NoUse,
        Use
    }

    public enum LoaderCommandReply
    {
        Nak,
        Ack
    }

    public enum LoaderStatus
    {
        Down,
        DoingInit,
        DoingRobotHome,
        Ready,
    }

    public enum LoaderEqpState
    {
        None,
        Run,
        Idle,
        Down,
    }

    public enum LoaderMode
    {
        Auto,
        Manual,
    }

    public enum LoaderControlMode
    {
        Offline,
        Online,
    }

    public enum MappingStatus
    {
        Off,
        On,
    }

    public enum SelectStatus
    {
        Selected,
        NotSelected
    }

    public enum GlassStatus
    {
        Empty,      // Silver
        Exist,      // LightCyan    // dspcrassus
        Selected,   // Green
        Wait,       // Green
        Started,    // Green
        Proc,       // Yellow
        Aborted,    // Gray
        Canceled,   // Gray
        Ok,         // Blue
        Ng,         // Orange
        Scraped     // Red
    }

    public enum PortTransferMode
    {
        None,
        AGV,
        MGV
    }

    public enum PortCommand
    {
        None,
        CstIdReadRequest,
        MappingRequest,
        ReChuckingRequest,
        LoadRequest,
        UnloadRequest,
        Clamping,
        GlassThickness,
    }

    public enum SolarPortCommand
    {
        None,
        PortStart,
        PortReject,
        PortAbort,
        PortCancel,
        PortPause,
        PortResume,
        MappingOn_Off,
        RcpReject,
        CstIdReadRequest,
        MappingRequest,
    }

    /// <summary>
    /// Glass 취출시 1번 Slot 부터 뽑을 것인지 25번(max slot)부터 뽑을 것인지
    /// 선택하기 위하여 사용함
    /// Increase : 1번부터 뽑는 방법을 의미
    /// Decrease : 25번부터 뽑는 방법을 의미
    /// </summary>
    public enum CstSlotOrder
    {
        Increase,
        Decrease,
    }

    /// <summary>
    /// GUI상에 1번 Slot이 위에 위치하는지 아래에 위치하는지를 의미한다.
    /// </summary>
    public enum CstSlot1Position
    {
        Top,
        Bottom,
    }

    public enum LoaderRobotHand
    {
        upperHand = 1,
        lowerHand = 2
    }

    public enum RobotCommandResult
    {
        accept,
        deny,
    }

    public enum RobotActionType
    {
        None = 0,
        GetStandBy = 1,
        Get = 2,
        PutStandBy = 3,
        Put = 4,
        Y_Align = 5,
        VcrRead = 6,
        ReturnHome = 7,
        GetAbort = 8,
        //KindDataRead = 8,
    }

    #endregion

    public class LoaderGlass
    {
        #region Fields
        private int m_OrgPortNo;
        private int m_OrgSlotNo;
        private int m_TargetPortNo;
        private int m_TargetSlotNo;
		private string m_RecipeId = "";
		private MappingStatus m_GlassMappingStatus = MappingStatus.Off;
        private SelectStatus m_GlassSelectStatus = SelectStatus.NotSelected;
        private GlassStatus m_Status;
        #endregion

        #region Properties
        public GlassStatus Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        public MappingStatus GlassMappingStatus
        {
            get { return m_GlassMappingStatus; }
            set { m_GlassMappingStatus = value; }
        }

        public SelectStatus GlassSelectStatus
        {
            get { return m_GlassSelectStatus; }
            set { m_GlassSelectStatus = value; }
        }

        public int OrgPortNo
        {
            get { return m_OrgPortNo; }
            set { m_OrgPortNo = value; }
        }

        public int OrgSlotNo
        {
            get { return m_OrgSlotNo; }
            set { m_OrgSlotNo = value; }
        }

        public int TargetPortNo
        {
            get { return m_TargetPortNo; }
            set { m_TargetPortNo = value; }
        }

        public int TargetSlotNo
        {
            get { return m_TargetSlotNo; }
            set { m_TargetSlotNo = value; }
        }
		public string RecipeId
		{
			get { return m_RecipeId; }
			set { m_RecipeId = value; }
		}
        #endregion

        #region Constructor
        public LoaderGlass()
        {
        }
        #endregion

        #region Methods
        #endregion
    }

    public class LoaderCst : IDisposable
    {
        #region Fields
        private int m_SlotCount;
        private string m_CstID;
        private string m_KeyInCstID;
        private CstSlotOrder m_SlotOrder;
        private CstSlot1Position m_Slot1Position;
        private string m_CstType;
        private LoaderGlass [] m_Glasses;
        #endregion

        #region Properties
        public int SlotCount
        {
            get { return m_SlotCount; }
        }
        public string CstID
        {
            get { return m_CstID; }
            set { m_CstID = value; }
        }
        public string KeyInCstID
        {
            get { return m_KeyInCstID; }
            set { m_KeyInCstID = value; }
        }
        public string CstType
        {
            get { return m_CstType; }
            set { m_CstType = value; }
        }
        public LoaderGlass[] Glasses
        {
            get { return m_Glasses; }
            set { m_Glasses = value; }
        } 
        public CstSlotOrder SlotOrder
        {
            get { return m_SlotOrder; }
            set { m_SlotOrder = value; }
        }
        public CstSlot1Position Slot1Position
        {
            get { return m_Slot1Position; }
            set { m_Slot1Position = value; }
        }
        #endregion

        #region Constructor
        public LoaderCst(int slotCount)
        {
            m_Glasses = new LoaderGlass[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                m_Glasses[i] = new LoaderGlass();
            }
            m_SlotCount = slotCount;
        }
        #endregion

        #region Methods
        public bool SetSelectStatus(int slotId, SelectStatus status)
        {
            if (slotId >= m_SlotCount || slotId < 0) return false;

            if (m_Glasses[slotId].GlassMappingStatus == MappingStatus.Off)
            {
                return false;
            }
            
            m_Glasses[slotId].GlassSelectStatus = status; 
            
            return true;
        }

        public bool SetGlassStatus(int slotId, GlassStatus status)
        {
            if (slotId >= m_SlotCount || slotId < 0)
            {
                return false;
            }

            m_Glasses[slotId].Status = status;

            return true;
        }

        public GlassStatus GetGlassStatus(int slotId)
        {
            if (slotId >= m_SlotCount || slotId < 0) return GlassStatus.Empty;
            else
            {
                return m_Glasses[slotId].Status;
            }
        }
        
        public bool SetMappingStatus(MappingStatus[] status)
        {
            for (int i = 0; i < m_SlotCount; i++)
            {
                m_Glasses[i].GlassMappingStatus = status[i];
            }
            return true;
        }

		public void SetRecipeId(int slotId, string recipeId)
		{
			m_Glasses[slotId].RecipeId = recipeId;
		}

		public string GetRecipeId(int slotId)
		{
			return m_Glasses[slotId].RecipeId;
		}

		public void ClearInfomation()
        {
            m_CstID = "";
            for (int i = 0; i < m_SlotCount; i++)
            {
                m_Glasses[i].GlassMappingStatus = MappingStatus.Off;
                m_Glasses[i].GlassSelectStatus = SelectStatus.NotSelected;
                m_Glasses[i].Status = GlassStatus.Empty;
				m_Glasses[i].RecipeId = "";
            }
        }
        #endregion

        #region IDisposable 멤버

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }


    //public delegate void GlassStatusEventHandler(EventArgs args);
    public delegate void RobotEventHandler(int portId, int slotId, RobotActionType action);

    public interface ILoaderSlave
    {
        event RobotEventHandler OnRobotEvent;

        GlassStatus GetGlassStatus(int portId, int slotId);
        GlassStatus[] GetPortGlassStatus(int portId);

        PortStatus GetPortStatus(int portId);
        
        LoaderStatus GetLoaderStatus();
        LoaderEqpState GetLoaderEqpState();
        LoaderControlMode GetLoaderControlMode();
        MappingStatus[] GetMappingData(int portId);
        LoaderCommandReply SetGlassSelectData(int portId, SelectStatus[] status);
        LoaderCommandReply SetGlassRecipeId(int portId, string[] recipe);

        string GetCstId(int portId);
        void SetCstId(int portId, string cstId);

        bool IsCassetteExist(int portId);
        bool IsPortPassable(int portId);

        bool IsGlassOkInputToCst(int portId, int slotId);

        bool IsPortEnable(int portId);
        bool IsPortError(int portId);

        bool IsAreaSensorDetected(int portId);
        
        bool IsDoorOpen();

        PortTransferMode GetTransferMode(int portId);
        LoaderCommandReply SetTransferMode(int portId, PortTransferMode mode);
        LoaderCommandReply SetPortEnable(int portId, PortUsage use);

        bool IsLoaderAutoMode();
        LoaderCommandReply SetLoaderReady();
        LoaderCommandReply SetGlassStatus(int portId, int slotId, GlassStatus status);
        LoaderCommandReply CstIdReadRequest(int portId);
        LoaderCommandReply MappingRequest(int portId);
        LoaderCommandReply ReChuckingRequest(int portId);
        LoaderCommandReply LotStartRequest(int portId);
        LoaderCommandReply LotEndRequest(int portId);
        LoaderCommandReply LotCancelRequest(int portId);
        LoaderCommandReply LotAbortRequest(int portId);
        LoaderCommandReply AbortConfirm(int portId);

        PortCommand GetPortCommand(int portId);

        /// <summary>
        /// Robot에서 호출한다. Loader Port로 부터 꺼내야 할 Glass객체를 반환받는다.
        /// </summary>
        /// <returns>LoaderGlass</returns>
        LoaderGlass GetNextOutputGlass();
        /// <summary>
        /// EQP에서 Unloading하는 Glass의 Port No, Slot No등의 정보를 이용하여 
        /// Loader에 존재하는 LoaderGlass객체들 중 이것과 맞는 객체를 찾아 반환한다.
        /// </summary>
        /// <returns>LoaderGlass</returns>
        LoaderGlass GetLoaderGlass(int portId, int slotId);
		string GetRecipeId(int portId, int slotId);

        void SetHostControlMode(HostControlMode mode);
        HostControlMode GetHostControlMode();

        int GetPortCount();

        void SetOpCallState(bool call);
        bool GetOpCallState();
    }
}
