using System;
using System.Collections.Generic;
using System.Text;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05
// Author       : Kim Youngsik
// Description  : HSMS Parameters
// Revison History
// * 2009.07.01 Add definition of parameter
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    #region Definition of Parameter
    public class Parameter
    {
        #region Fields
        private string m_Key = "";
        private string m_Value = "";
        private bool m_SecomParam = false;
        #endregion

        #region Properties
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public bool SecomParameter
        {
            get { return m_SecomParam; }
            set { m_SecomParam = value; }
        }
        #endregion

        #region Constuctors
        public Parameter()
        {
            m_Key = "";
            m_Value = "";
            m_SecomParam = false;
        }

        public Parameter(string key, string val, bool secomParm)
        {
            m_Key = key;
            m_Value = val;
            m_SecomParam = secomParm;
        }
        #endregion

        #region Methods
        #endregion
    }
    #endregion

    public class HsmsParameter
    {
        #region Fields
        private Parameter m_EqpID = new Parameter();
        private Parameter m_SoftVer = new Parameter();
        private Parameter m_SecomEqpId = new Parameter();
        private Parameter m_RemoteIP = new Parameter();
        private Parameter m_RemotePort = new Parameter();
        private Parameter m_ConnectMode = new Parameter();
        private Parameter m_DeviceID = new Parameter();

        private Parameter m_T3Timeout = new Parameter();
        private Parameter m_T5Timeout = new Parameter();
        private Parameter m_T6Timeout = new Parameter();
        private Parameter m_T7Timeout = new Parameter();
        private Parameter m_T8Timeout = new Parameter();
        private Parameter m_T9Timeout = new Parameter();

        private Parameter m_LinkTest = new Parameter();
        private Parameter m_RetryCount = new Parameter();
        private Parameter m_T3RetryCount = new Parameter();

        private List<Parameter> m_Parameters = new List<Parameter>();
        #endregion

        #region Properties
        public Parameter EqpID
        {
            get { return m_EqpID; }
            set { m_EqpID = value; }
        }

        public Parameter SoftVer
        {
            get { return m_SoftVer; }
            set { m_SoftVer = value; }
        }

        public Parameter SecomEqpId
        {
            get { return m_SecomEqpId; }
            set { m_SecomEqpId = value; }
        }

        public Parameter RemoteIP
        {
            get { return m_RemoteIP; }
            set { m_RemoteIP = value; }
        }

        public Parameter RemotePort
        {
            get { return m_RemotePort; }
            set { m_RemotePort = value; }
        }

        public Parameter ConnectMode
        {
            get { return m_ConnectMode; }
            set { m_ConnectMode = value; }
        }

        public Parameter DeviceID
        {
            get { return m_DeviceID; }
            set { m_DeviceID = value; }
        }

        public Parameter T3
        {
            get { return m_T3Timeout; }
            set { m_T3Timeout = value; }
        }

        public Parameter T5
        {
            get { return m_T5Timeout; }
            set { m_T5Timeout = value; }
        }

        public Parameter T6
        {
            get { return m_T6Timeout; }
            set { m_T6Timeout = value; }
        }

        public Parameter T7
        {
            get { return m_T7Timeout; }
            set { m_T7Timeout = value; }
        }

        public Parameter T8
        {
            get { return m_T8Timeout; }
            set { m_T8Timeout = value; }
        }

        public Parameter T9
        {
            get { return m_T9Timeout; }
            set { m_T9Timeout = value; }
        }

        public Parameter LinkTest
        {
            get { return m_LinkTest; }
            set { m_LinkTest = value; }
        }

        public Parameter RetryCount
        {
            get { return m_RetryCount; }
            set { m_RetryCount = value; }
        }

        public Parameter T3RetryCount
        {
            get { return m_T3RetryCount; }
            set { m_T3RetryCount = value; }
        }

        public List<Parameter> Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }
        #endregion

        #region Singleton
        public static readonly HsmsParameter Instance = new HsmsParameter();
        #endregion

        #region Constructor
        public HsmsParameter()
        {
            m_Parameters.Clear();

            AddParameter(ref m_EqpID, "EQPID", "0", false);
            AddParameter(ref m_SoftVer, "SOFTREV", "DMSHDC", false);

            AddParameter(ref m_SecomEqpId, "SECOMID", "HDC", false);
            AddParameter(ref m_RemoteIP, "REMOTEIP", "127.0.0.1", true);
            AddParameter(ref m_RemotePort, "REMOTEPORT", "5000", true);
            AddParameter(ref m_ConnectMode, "HSMSMODE", "Active", true);
            AddParameter(ref m_DeviceID, "DEVICEID", "0", true);

            AddParameter(ref m_T3Timeout, "T3", "60", true);
            AddParameter(ref m_T5Timeout, "T5", "60", true);
            AddParameter(ref m_T6Timeout, "T6", "60", true);
            AddParameter(ref m_T7Timeout, "T7", "60", true);
            AddParameter(ref m_T8Timeout, "T8", "60", true);
            AddParameter(ref m_T9Timeout, "T9", "60", true);

            AddParameter(ref m_LinkTest, "LINKTEST", "60", true);
            AddParameter(ref m_RetryCount, "RETRYCOUNT", "3", false);
            AddParameter(ref m_T3RetryCount, "T3RETRYCOUNT", "3", false);
        }

        public void AddParameter(ref Parameter parm, string key, string val, bool secomParm)
        {
            parm.Key = key;
            parm.Value = val;
            parm.SecomParameter = secomParm;

            m_Parameters.Add(parm);
        }
        #endregion
    }
}
