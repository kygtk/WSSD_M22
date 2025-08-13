using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using Dms.Device;
using Dms.Mitsubishi;

namespace Dms.Server
{
    partial class ServerManager
    {
        #region Fields
        private TagSetupInfo m_SetupHsmsT3Timeout;
        private TagSetupInfo m_SetupHsmsT5Timeout;
        private TagSetupInfo m_SetupHsmsT6Timeout;
        private TagSetupInfo m_SetupHsmsT7Timeout;
        private TagSetupInfo m_SetupHsmsT8Timeout;
        private TagSetupInfo m_SetupHsmsT9Timeout;
        private TagSetupInfo m_SetupHsmsLinkTest;
        private TagSetupInfo m_SetupHsmsDeviceId;
        private TagSetupInfo m_SetupHsmsConnectionMode;
        private TagSetupInfo m_SetupHsmsHostIp;
        private TagSetupInfo m_SetupHsmsHostPortNo;
        private TagSetupInfo m_SetupHsmsBackupHostIp;
        private TagSetupInfo m_SetupHsmsBackupHostPortNo;

        private TagSetupInfo m_SetupHsmsDriverName;
        private TagSetupInfo m_SetupHsmsIniFilePath;
        private TagSetupInfo m_SetupHsmsEqpName;
        private TagSetupInfo m_SetupHsmsEqpId;
        private TagSetupInfo m_SetupHsmsControlId;
        private TagSetupInfo m_SetupHsmsEqpMdln;
        private TagSetupInfo m_SetupHsmsEqpSoftRev;

        //private XLog m_InterfaceLog = new XLog("InterfaceLog", XLog.LogStampType.UseStamp);
        //private TransferData m_RecvData = new TransferData();
        //private TransferData m_SendData = new TransferData();
        //private TagRecipe m_RecvRecipe = new TagRecipe();

        public Alarm ALM_HsmsT3Timeout;
        public Alarm ALM_HsmsT5Timeout;
        public Alarm ALM_HsmsT6Timeout;
        public Alarm ALM_HsmsT7Timeout;
        public Alarm ALM_HsmsT8Timeout;
        public Alarm ALM_HsmsT9Timeout;
        #endregion

        #region Properties
        public TagSetupInfo SetupHsmsT3Timeout
        {
            get { return m_SetupHsmsT3Timeout; }
            set { m_SetupHsmsT3Timeout = value; }
        }
        public TagSetupInfo SetupHsmsT5Timeout
        {
            get { return m_SetupHsmsT5Timeout; }
            set { m_SetupHsmsT5Timeout = value; }
        }
        public TagSetupInfo SetupHsmsT6Timeout
        {
            get { return m_SetupHsmsT6Timeout; }
            set { m_SetupHsmsT6Timeout = value; }
        }
        public TagSetupInfo SetupHsmsT7Timeout
        {
            get { return m_SetupHsmsT7Timeout; }
            set { m_SetupHsmsT7Timeout = value; }
        }
        public TagSetupInfo SetupHsmsT8Timeout
        {
            get { return m_SetupHsmsT8Timeout; }
            set { m_SetupHsmsT8Timeout = value; }
        }
        public TagSetupInfo SetupHsmsT9Timeout
        {
            get { return m_SetupHsmsT9Timeout; }
            set { m_SetupHsmsT9Timeout = value; }
        }
        public TagSetupInfo SetupHsmsLinkTest
        {
            get { return m_SetupHsmsLinkTest; }
            set { m_SetupHsmsLinkTest = value; }
        }
        public TagSetupInfo SetupHsmsDeviceId
        {
            get { return m_SetupHsmsDeviceId; }
            set { m_SetupHsmsDeviceId = value; }
        }
        public TagSetupInfo SetupHsmsConnectionMode
        {
            get { return m_SetupHsmsConnectionMode; }
            set { m_SetupHsmsConnectionMode = value; }
        }
        public TagSetupInfo SetupHsmsHostIp
        {
            get { return m_SetupHsmsHostIp; }
            set { m_SetupHsmsHostIp = value; }
        }
        public TagSetupInfo SetupHsmsHostPortNo
        {
            get { return m_SetupHsmsHostPortNo; }
            set { m_SetupHsmsHostPortNo = value; }
        }
        public TagSetupInfo SetupHsmsBackupHostIp
        {
            get { return m_SetupHsmsBackupHostIp; }
            set { m_SetupHsmsBackupHostIp = value; }
        }
        public TagSetupInfo SetupHsmsBackupHostPortNo
        {
            get { return m_SetupHsmsBackupHostPortNo; }
            set { m_SetupHsmsBackupHostPortNo = value; }
        }

        public TagSetupInfo SetupHsmsIniFilePath
        {
            get { return m_SetupHsmsIniFilePath; }
            set { m_SetupHsmsIniFilePath = value; }
        }
        public TagSetupInfo SetupHsmsEqpName
        {
            get { return m_SetupHsmsEqpName; }
            set { m_SetupHsmsEqpName = value; }
        }
        public TagSetupInfo SetupHsmsEqpID
        {
            get { return m_SetupHsmsEqpId; }
            set { m_SetupHsmsEqpId = value; }
        }
        public TagSetupInfo SetupHsmsControlID
        {
            get { return m_SetupHsmsControlId; }
            set { m_SetupHsmsControlId = value; }
        }
        public TagSetupInfo SetupHsmsEqpMDLN
        {
            get { return m_SetupHsmsEqpMdln; }
            set { m_SetupHsmsEqpMdln = value; }
        }
        public TagSetupInfo SetupHsmsEqpSoftRev
        {
            get { return m_SetupHsmsEqpSoftRev; }
            set { m_SetupHsmsEqpSoftRev = value; }
        }

        //public XLog InterfaceLog
        //{
        //    get { return m_InterfaceLog; }
        //}
        //public TransferData RecvData
        //{
        //    get { return m_RecvData; }
        //    set { m_RecvData = value; }
        //}
        //public TransferData SendData
        //{
        //    get { return m_SendData; }
        //    set { m_SendData = value; }
        //}
        //public TagRecipe RecvRecipeBody
        //{
        //    get { return m_RecvRecipe; }
        //    set { m_RecvRecipe = value; }
        //}
        #endregion

        #region Methods
        public bool InitializeServerHsms()
        {
            m_SetupHsmsDriverName = new TagSetupInfo("SECom Driver Name", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "SECOM");
            m_DataProvider.SetupHsmsEqpInfo.InitFromDB(m_SetupHsmsDriverName);
            m_SetupHsmsEqpName = new TagSetupInfo("Equipment Name", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "HDC");
            m_DataProvider.SetupHsmsEqpInfo.InitFromDB(m_SetupHsmsEqpName);
            m_SetupHsmsEqpId = new TagSetupInfo("Equipment ID", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "202");
            m_DataProvider.SetupHsmsEqpInfo.InitFromDB(m_SetupHsmsEqpId);
            m_SetupHsmsControlId = new TagSetupInfo("Controller ID", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "200");
            m_DataProvider.SetupHsmsEqpInfo.InitFromDB(m_SetupHsmsControlId);
            m_SetupHsmsEqpMdln = new TagSetupInfo("MDLN", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "CLN");
            m_DataProvider.SetupHsmsEqpInfo.InitFromDB(m_SetupHsmsEqpMdln);
            m_SetupHsmsEqpSoftRev = new TagSetupInfo("Software Revision", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "V001");
            m_DataProvider.SetupHsmsEqpInfo.InitFromDB(m_SetupHsmsEqpSoftRev);

            SetupHsmsInfoXml secomXml = m_DataProvider.SetupHsmsInfo.SecomXml;
            //secomXml.FilePath = SecomFile.SelectedFile;

            m_SetupHsmsT3Timeout = new TagSetupInfo(secomXml.T3.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsT3Timeout);
            m_SetupHsmsT5Timeout = new TagSetupInfo(secomXml.T5.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsT5Timeout);
            m_SetupHsmsT6Timeout = new TagSetupInfo(secomXml.T6.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsT6Timeout);
            m_SetupHsmsT7Timeout = new TagSetupInfo(secomXml.T7.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsT7Timeout);
            m_SetupHsmsT8Timeout = new TagSetupInfo(secomXml.T8.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsT8Timeout);
            m_SetupHsmsT9Timeout = new TagSetupInfo(secomXml.T9.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsT9Timeout);
            m_SetupHsmsLinkTest = new TagSetupInfo(secomXml.LinkTest.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "60");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsLinkTest);
            m_SetupHsmsDeviceId = new TagSetupInfo(secomXml.DeviceID.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.None, "0");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsDeviceId);
            m_SetupHsmsConnectionMode = new TagSetupInfo(secomXml.ConnectMode.Description, OptionType.None, OptionFormat.HsmsConnectionMode, Dms.Common.UnitType.None, Dms.Common.HsmsConnectionMode.Active.ToString());
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsConnectionMode);
            m_SetupHsmsHostIp = new TagSetupInfo(secomXml.RemoteIP.Description, OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "127.0.0.1");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsHostIp);
            m_SetupHsmsHostPortNo = new TagSetupInfo(secomXml.RemotePort.Description, OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.None, "5000");
            m_DataProvider.SetupHsmsInfo.InitFromDB(m_SetupHsmsHostPortNo);

            if (m_DataProvider.SetupHsmsInfo.LoadFromXml(m_SetupHsmsDriverName.Val) == true)
            {
                m_DataProvider.SetupHsmsInfo.SyncFromXml();
            }
            else
            {
                if (MessageBox.Show("Secom xml file does not exist or driver name is invalid. Do you want to continue to run?", "ServerHsms", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }
            }

            ALM_HsmsT3Timeout = new Alarm("T3 Timeout Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_HsmsT5Timeout = new Alarm("T5 Timeout Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_HsmsT6Timeout = new Alarm("T6 Timeout Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_HsmsT7Timeout = new Alarm("T7 Timeout Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_HsmsT8Timeout = new Alarm("T8 Timeout Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_HsmsT9Timeout = new Alarm("T9 Timeout Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);

            return true;
        }

        private void UninitializeServerHsms()
        {

        }
        #endregion
    }
}
