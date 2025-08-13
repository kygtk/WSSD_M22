using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Ctl;

namespace Dms.Cim.Common
{
    public enum CommunicationState
    {
        NOT_COMMUNICATING,
        COMMUNICATING,
    }

    public enum hostMODE
    {
        hostNone = 0,
        hostOffline = 'O',
        hostOnlineMonitor = 'M',
        hostOnlineControl = 'C',
    }

    public enum trsMODE
    {
        trsMGV = 'M',
        trsAGV = 'A',
    }

    public class HostInfo : HsmsInfo
    {
        #region Fields
//        private HsmsInfo m_HsmsInfo;

        private int m_UnitNo = 0;

        private CommunicationState m_CommState = CommunicationState.NOT_COMMUNICATING;

        private hostMODE m_Mode = hostMODE.hostOffline;
        private hostMODE m_RequestMode;
        private hostMODE m_BeforeMode;

        private eqpSTATUS m_EqpStatus;
        private eqpSUBSTATUS m_EqpSubStatus;

        private trsMODE m_LoaderTrsMode;
        private trsMODE m_UnloaderTrsMode;

        private bool m_LoaderConnect;
        private bool m_EqpConnect;

        private bool m_AlarmExist;

        private bool m_OnlineModeChanging;

        private string m_RecipeId;
        private string m_ReqRecipeBodyId;

        private string m_LoopBack;

        private UserLevels m_UserLevel;

        private bool m_TimeSetReq;
        #endregion

        #region Property
//        public HsmsInfo HsmsInfo
//        {
//            get { return m_HsmsInfo; }
//        }
        public int UnitNo
        {
            get { return m_UnitNo; }
            set { m_UnitNo = value; }
        }

        public CommunicationState CommState
        {
            get { return m_CommState; }
            set { m_CommState = value; }
        }

        public hostMODE Mode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        public hostMODE ReqMode
        {
            get { return m_RequestMode; }
            set { m_RequestMode = value; }
        }

        public hostMODE BeforeMode
        {
            get { return m_BeforeMode; }
            set { m_BeforeMode = value; }
        }

        public trsMODE LoaderTrsMode
        {
            get { return m_LoaderTrsMode; }
            set { m_LoaderTrsMode = value; }
        }

        public trsMODE UnloaderTrsMode
        {
            get { return m_UnloaderTrsMode; }
            set { m_UnloaderTrsMode = value; }
        }

        public bool LoaderConnect
        {
            get { return m_LoaderConnect; }
            set { m_LoaderConnect = value; }
        }

        public bool EqpConnect
        {
            get { return m_EqpConnect; }
            set { m_EqpConnect = value; }
        }

        public bool ModeChanging
        {
            get { return m_OnlineModeChanging; }
            set { m_OnlineModeChanging = value; }
        }

        public bool AlarmExist
        {
            get { return m_AlarmExist; }
            set { m_AlarmExist = value; }
        }

        public string Recipe
        {
            get { return m_RecipeId; }
            set { m_RecipeId = value; }
        }

        public string ReqRecipeBodyId
        {
            get { return m_ReqRecipeBodyId; }
            set { m_ReqRecipeBodyId = value; }
        }

        public string LoopBack
        {
            get { return m_LoopBack; }
            set { m_LoopBack = value; }
        }

        public eqpSTATUS EqpStatus
        {
            get { return m_EqpStatus; }
            set { m_EqpStatus = value; }
        }

        public eqpSUBSTATUS EqpSubStatus
        {
            get { return m_EqpSubStatus; }
            set { m_EqpSubStatus = value; }
        }

        public UserLevels UserLevel
        {
            get { return m_UserLevel; }
            set { m_UserLevel = value; }
        }

        public bool TimeSetReq
        {
            get { return m_TimeSetReq; }
            set { m_TimeSetReq = value; }
        }
        #endregion

        public HostInfo()
        {
//            m_HsmsInfo = HsmsInfo.Instance;
        }

        public void DataReset()
        {
            m_Mode = hostMODE.hostOffline;
            m_RequestMode = hostMODE.hostOffline;
            m_BeforeMode = hostMODE.hostOffline;

            m_EqpStatus = eqpSTATUS.eqpIdle;
            m_EqpSubStatus = eqpSUBSTATUS.eqpsubNone;

            m_LoaderTrsMode = trsMODE.trsAGV;
            m_UnloaderTrsMode = trsMODE.trsAGV;

            m_LoaderConnect = false;
            m_EqpConnect = false;

            m_AlarmExist = false;

            m_OnlineModeChanging = false;
            m_TimeSetReq = false;

            m_RecipeId = "001";
            m_LoopBack = "";

        }
    }
}
