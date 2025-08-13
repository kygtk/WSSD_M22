///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05.
// Author       : chungwon
// Description  : TransAsm class basic
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Device
{
    public class TagTransferIfFlag
    {
        #region Fields
        private bool inReady;
        private bool inComp;
        private bool outReady;
        private bool outComp;
        private bool inError;
        private bool outError;
        private TransferUnit m_Parent;
        #endregion

        #region Properties
        public bool InReady
        {
            get { return inReady; }
            set { inReady = value; }
        }
        public bool InComp
        {
            get { return inComp; }
            set { inComp = value; }
        }
        public bool OutReady
        {
            get { return outReady; }
            set { outReady = value; }
        }
        public bool OutComp
        {
            get { return outComp; }
            set { outComp = value; }
        }
        public bool InError
        {
            get { return inError; }
            set { inError = value; }
        }
        public bool OutError
        {
            get { return outError; }
            set { outError = value; }
        }
        #endregion

        #region Constructor
        public TagTransferIfFlag()
        {
        }

        public TagTransferIfFlag(TransferUnit unit)
        {
            m_Parent = unit;
        }
        #endregion

        #region Methods
        public virtual void Reset()
        {
            InReady = false;
            InComp = true;
            OutReady = false;
            OutComp = true;
            InError = false;
            OutError = false;
        }
        #endregion
    }

    public class TagTrayTransferIfFlag
    {
        #region Fields
        private bool recvReady;
        private bool recvComp;
        private bool sendReady;
        private bool sendComp;
        private bool recvError;
        private bool sendError;
        private bool revRecvReady;
        private bool revRecvComp;
        private bool revSendReady;
        private bool revSendComp;
        private bool revRecvError;
        private bool revSendError;
        private TransferUnit m_Parent;
        #endregion

        #region Properties
        public bool RecvReady
        {
            get { return recvReady; }
            set { recvReady = value; }
        }
        public bool RecvComp
        {
            get { return recvComp; }
            set { recvComp = value; }
        }
        public bool SendReady
        {
            get { return sendReady; }
            set { sendReady = value; }
        }
        public bool SendComp
        {
            get { return sendComp; }
            set { sendComp = value; }
        }
        public bool RecvError
        {
            get { return recvError; }
            set { recvError = value; }
        }
        public bool SendError
        {
            get { return sendError; }
            set { sendError = value; }
        }
        public bool RevRecvReady
        {
            get { return revRecvReady; }
            set { revRecvReady = value; }
        }
        public bool RevRecvComp
        {
            get { return revRecvComp; }
            set { revRecvComp = value; }
        }
        public bool RevSendReady
        {
            get { return revSendReady; }
            set { revSendReady = value; }
        }
        public bool RevSendComp
        {
            get { return revSendComp; }
            set { revSendComp = value; }
        }
        public bool RevRecvError
        {
            get { return revRecvError; }
            set { revRecvError = value; }
        }
        public bool RevSendError
        {
            get { return revSendError; }
            set { revSendError = value; }
        }
        #endregion

        #region Constructor
        public TagTrayTransferIfFlag()
        {
        }

        public TagTrayTransferIfFlag(TransferUnit unit)
        {
            m_Parent = unit;
        }
        #endregion

        #region Methods
        public virtual void Reset()
        {
            recvReady = false;
            recvComp = false;
            sendReady = false;
            sendComp = true;
            recvError = false;
            sendError = false;
            revRecvReady = false;
            revRecvComp = false;
            revSendReady = false;
            revSendComp = true;
            revRecvError = false;
            revSendError = false;
        }
        #endregion
    }

    public class GlassDataIDHandler
    {
        #region Fields
        private int m_PositionID;
        private static int m_SerialKeyCalculator = 0;
        #endregion

        #region Properties
        public int PositionID
        {
            get { return m_PositionID; }
        }
        public static int MaxPositionCount
        {
            get { return m_SerialKeyCalculator; }
        }
        #endregion

        #region Constructor
        public GlassDataIDHandler()
        {
            m_PositionID = m_SerialKeyCalculator;
            m_SerialKeyCalculator++;
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    abstract public class TransferUnit : _DeviceAsm
    {
        #region Fields
        private TransferUnit m_PrevUnit;
        private TransferUnit m_NextUnit;
        //protected TagIfFlag m_IfFlag = null;
        protected short m_GlassDataCount = 1;
        protected List<GlassDataIDHandler> m_GlassDataList = new List<GlassDataIDHandler>();
        public XSeqFunction[] Sequence = null;
        #endregion

        #region Properties
        //[Browsable(false), XmlIgnore()]
        //public TagIfFlag IfFlag
        //{
        //    get { return m_IfFlag; }
        //    set { m_IfFlag = value; }
        //}
        [Category("DMS : Relation"), Browsable(false), XmlIgnore()]
        public TransferUnit PrevUnit
        {
            get { return m_PrevUnit; }
            set { m_PrevUnit = value; }
        }
        [Category("DMS : Relation"), Browsable(false), XmlIgnore()]
        public TransferUnit NextUnit
        {
            get { return m_NextUnit; }
            set { m_NextUnit = value; }
        }
        [Category("DMS : Relation"), Browsable(false), XmlIgnore()]
        public short GlassDataCount
        {
            get { return m_GlassDataCount; }
            set { m_GlassDataCount = value; }
        }
        #endregion

        #region Constructor
        public TransferUnit()
        {

        }

        //public TransferUnit(int glassDataCount)
        //{
        //    for (int i = 0; i < m_GlassDataCount; i++)
        //    {
        //        m_GlassDataList.Add(new GlassDataIDHandler());
        //    }
        //}
        #endregion

        #region _DeviceAsm override
        public override void CreateTag(DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        }

        public override DmsErrors Initialize()
        {
            return DmsErrors.Success;
        }
        #endregion

        #region Method

        public void GenerateDataKey()
        {
            for (int i = 0; i < m_GlassDataCount; i++)
            {
                m_GlassDataList.Add(new GlassDataIDHandler());
            }
        }

        public int DataMatchingKey(short localIndex)
        {
            try
            {
                return m_GlassDataList[localIndex].PositionID;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return -1;
            }
        }

        //unit에 존재하는 GlassDataExistCount를 반환
        public virtual int GetGlassDataExistCountInUnit()
        {
            int count = 0;
            for (short i = 0; i < m_GlassDataCount; i++)
            {
                if (IsGlassDataExist(i))
                {
                    count++;
                }
            }
            return count;
        }

        //unit의 지정된 지점에 GlassData가 존재하는지 여부를 반환
        public virtual bool IsGlassDataExist(short index)
        {
            return m_Server.GlassData.IsExist(DataMatchingKey(index));
        }
        #endregion
    }
}
