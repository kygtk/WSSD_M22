///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.28
// Author       : jemoon
// Description  : General PortUnit
///////////////////////////////////////////////////////////////////////////
// *Revision History
// 2009.11.12 - jemoon : Abstract·Î º¯°æ


using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    abstract public class PortUnit : _DeviceAsm
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private bool m_InputOkSwPushConfirm;
        private bool m_InputEndSwPushConfirm;
        private bool m_OutputOkSwPushConfirm;
        private bool m_OutputEndSwPushConfirm;
        private bool m_ForceEndSwPushConfirm;

        protected SetupLoaderInfoProvider m_SetupLoaderInfoProvider = null;
        protected TagSetupInfo[] m_SetupPortEnable = new TagSetupInfo[(int)(nxOP_CUBE.STAGE1-1)];

        [Browsable(false), XmlIgnore()]
        public bool InputOkSwPushConfirm
        {
            get { return m_InputOkSwPushConfirm; }
            set { m_InputOkSwPushConfirm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool InputEndSwPushConfirm
        {
            get { return m_InputEndSwPushConfirm; }
            set { m_InputEndSwPushConfirm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool OutputOkSwPushConfirm
        {
            get { return m_OutputOkSwPushConfirm; }
            set { m_OutputOkSwPushConfirm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool OutputEndSwPushConfirm
        {
            get { return m_OutputEndSwPushConfirm; }
            set { m_OutputEndSwPushConfirm = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool ForceEndSwPushConfirm
        {
            get { return m_ForceEndSwPushConfirm; }
            set { m_ForceEndSwPushConfirm = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo[] SetupPortEnable
        {
            get { return m_SetupPortEnable; }
            set { m_SetupPortEnable = value; }
        }
        #endregion

        #region Properties
        protected PortStatus m_PortStatus;
        [Browsable(false), XmlIgnore()]
        public PortStatus PortStatus
        {
            get { return m_PortStatus; }
            set { m_PortStatus = value; }
        }

        //protected bool m_LotStartReq;
        //[Browsable(false), XmlIgnore()]
        //public bool LotStartReq
        //{
        //    get { return m_LotStartReq; }
        //    set { m_LotStartReq = value; }
        //}

        //protected bool m_LotAbortReq;
        //[Browsable(false), XmlIgnore()]
        //public bool LotAbortReq
        //{
        //    get { return m_LotAbortReq; }
        //    set { m_LotAbortReq = value; }
        //}

        //protected bool m_LotCancelReq;
        //[Browsable(false), XmlIgnore()]
        //public bool LotCancelReq
        //{
        //    get { return m_LotCancelReq; }
        //    set { m_LotCancelReq = value; }
        //}

        //protected bool m_LotEndReq;
        //[Browsable(false), XmlIgnore()]
        //public bool LotEndReq
        //{
        //    get { return m_LotEndReq; }
        //    set { m_LotEndReq = value; }
        //}

        protected bool m_PortError;
        [Browsable(false), XmlIgnore()]
        public bool PortError
        {
            get { return m_PortError; }
            set { m_PortError = value; }
        }

        protected LoaderCst m_Cst;
        [Browsable(false), XmlIgnore()]
        public LoaderCst Cst
        {
            get { return m_Cst; }
            set { m_Cst = value; }
        }

        protected PortTransferMode m_TransferMode = PortTransferMode.AGV;
        [Browsable(false), XmlIgnore()]
        public PortTransferMode TransferMode
        {
            get { return m_TransferMode; }
            set { m_TransferMode = value; }
        }

        protected PortUsage m_PortEnable = PortUsage.Use;
        [Browsable(false), XmlIgnore()]
        public PortUsage PortEnable
        {
            get { return m_PortEnable; }
            set { m_PortEnable = value; }
        }

        protected string m_CassetteID;
        [Browsable(false), XmlIgnore()]
        public string CassetteID
        {
            get { return m_CassetteID; }
            set { m_CassetteID = value; }
        }

        protected PortCommand m_PortCommand;
        [Browsable(false), XmlIgnore()]
        public PortCommand PortCommand
        {
            get { return m_PortCommand; }
            set { m_PortCommand = value; }
        }

        #region LampSwitch Setting Abstract Register
        abstract public LampSwitch LampInputOk
        {
            get;
            set;
        }

        abstract public LampSwitch LampInputEnd
        {
            get;
            set;
        }

        abstract public LampSwitch LampOutputOk
        {
            get;
            set;
        }

        abstract public LampSwitch LampOutputEnd
        {
            get;
            set;
        }

        abstract public LampSwitch LampForceEnd
        {
            get;
            set;
        }
        #endregion

        abstract public _CstMappingUnit MappingUnit
        {
            get;
            set;
        }

        abstract public _BCR BCR
        {
            get;
            set;
        }
        #endregion

        #region Methods
        abstract public CstSlotOrder GetSlotOrder();
        abstract public bool IsCstInProcessing();
        abstract public bool IsCstUnloadCondition();
        abstract public bool IsCstExist();
        abstract public bool IsSafeCstLoading();
        abstract public bool IsCstClampPos();
        abstract public bool IsCstClampNeg();
        abstract public bool IsCstFloatPos();
        abstract public bool IsCstFloatNeg();
        abstract public bool IsCstLoadCondition();
        abstract public bool IsCstOpposite();
        abstract public void LotAbortSet();
        abstract public void LotCancelSet();
        abstract public void LotStartSet();
        abstract public void SetIonizerRun(bool on);
        abstract public int SeqPortInit();
        public void ClearInfomation()
        {
            m_Cst.ClearInfomation();
        }
        #endregion

        #region Override
        #endregion
    }
}
