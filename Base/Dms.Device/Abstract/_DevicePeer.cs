using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dms.Common;
using Dms.Util.IODefine;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Serializable()]
    public abstract class _DevicePeer : _DeviceAsm
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        protected SlaveAP m_SlaveAP;
        protected PeerType m_PeerType;
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveAP SlaveAP
        {
            get { return m_SlaveAP; }
            set { m_SlaveAP = value; }
        }
        [Category("DMS : Setting")]
        public PeerType PeerType
        {
            get { return m_PeerType; }
        }
        #endregion

        #region Methods
        public bool IsPaired()
        {
            if (!m_Initialized) return false;

            return m_SlaveAP.GetPairingState() == PairingState.Paired;
        }
        #endregion

        #region Overrides
        #endregion
    }
}
