///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : ClientManager
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;

namespace Dms.Client
{
    //[Synchronization]
    public partial class ClientManager
    {
        #region Fields
        private TagUserAccount m_CurrentUserAccount;
        private DataProvider m_DataProvider = null;
        private _GenInfoHandler m_GenInfos = null;
        private UserAccountProvider m_UserAccountProvicer = UserAccountProvider.Instance;
        private ApdItemsHandler m_ApdItemsHandler = null;
        private PartsItemsHandler m_PartsItemsHandler = null;
        private HpmjItemsHandler m_HpmjItemsHandler = null;
        #endregion

        #region Properties
        public TagUserAccount CurrentUserAccount
        {
            get { return m_CurrentUserAccount; }
            set { m_CurrentUserAccount = value; }
        }
        public UserLevels CurrentUserLevel
        {
            get { return m_CurrentUserAccount.UserLevel; }
        }
        public DataProvider DataProvider
        {
            get { return m_DataProvider; }
            set { m_DataProvider = value; }
        }
        public UserAccountProvider UserAccountProvider
        {
            get { return m_UserAccountProvicer; }
            set { m_UserAccountProvicer = value; }
        }
        public _GenInfoHandler GenInfos
        {
            get { return m_GenInfos; }
            set { m_GenInfos = value; }
        }
        public ApdItemsHandler ApdItemsHandler
        {
            get { return m_ApdItemsHandler; }
            set { m_ApdItemsHandler = value; }
        }
        public PartsItemsHandler PartsItemsHandler
        {
            get { return m_PartsItemsHandler; }
            set { m_PartsItemsHandler = value; }
        }
        public HpmjItemsHandler HpmjItemsHandler
        {
            get { return m_HpmjItemsHandler; }
            set { m_HpmjItemsHandler = value; }
        }

        public DeviceTags TagContainer
        {
            get { return m_DataProvider.TagContainer; }
        }
        public CalibrationProvider CalibrationProvider
        {
            get { return m_DataProvider.CalibrationProvider; }
        }
        public GlassDataProvider GlassDataProvider
        {
            get { return m_DataProvider.GlassDataProvider; }
        }
        public LostGlassDataProvider LostGlassDataProvider
        {
            get { return m_DataProvider.LostGlassDataProvider; }
        }
        public SetupTankLevelProvider SetupTankLevelProvider
        {
            get { return m_DataProvider.SetupTankLevel; }
        }

        public UserLevels UserLevel
        {
            get { return m_CurrentUserAccount.UserLevel; }
        }
        #endregion

        #region Methods
        public DmsErrors InitData()
        {
            return DmsErrors.Success;
        }
        #endregion
    }
}
