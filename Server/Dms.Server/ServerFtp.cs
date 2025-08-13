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
        private TagSetupInfo m_SetupTpdEqpName;
        private TagSetupInfo m_SetupTpdFtpAddress;
        private TagSetupInfo m_SetupTpdFtpPortNo;
        private TagSetupInfo m_SetupTpdFtpUserId;
        private TagSetupInfo m_SetupTpdFtpPassword;
        private TagSetupInfo m_SetupTpdSamplingTime;
        private TagSetupInfo m_SetupTpdTransferTime;
        private TagSetupInfo m_SetupTpdTransferPath;
        private TagSetupInfo m_SetupTasEqpName;
        private TagSetupInfo m_SetupTasFtpAddress;
        private TagSetupInfo m_SetupTasFtpPortNo;
        private TagSetupInfo m_SetupTasFtpUserId;
        private TagSetupInfo m_SetupTasFtpPassword;
        private TagSetupInfo m_SetupTasTransferTime;
        private TagSetupInfo m_SetupTasTransferPath;

        public Alarm ALM_TpdConnect;
        public Alarm ALM_TpdTransfer;
        public Alarm ALM_TpdPath;
        public Alarm ALM_TasConnect;
        public Alarm ALM_TasTransfer;
        public Alarm ALM_TasPath;
        private XLog m_FtpLog = new XLog("FTP Log", XLog.LogStampType.UseStamp);//2010.06.18 kimgun
        #endregion

        #region Properties
        public TagSetupInfo SetupTpdEqpName
        {
            get { return m_SetupTpdEqpName; }
            set { m_SetupTpdEqpName = value; }
        }
        public TagSetupInfo SetupTpdFtpAddress
        {
            get { return m_SetupTpdFtpAddress; }
            set { m_SetupTpdFtpAddress = value; }
        }
        public TagSetupInfo SetupTpdFtpPortNo
        {
            get { return m_SetupTpdFtpPortNo; }
            set { m_SetupTpdFtpPortNo = value; }
        }
        public TagSetupInfo SetupTpdFtpUserId
        {
            get { return m_SetupTpdFtpUserId; }
            set { m_SetupTpdFtpUserId = value; }
        }
        public TagSetupInfo SetupTpdFtpPassword
        {
            get { return m_SetupTpdFtpPassword; }
            set { m_SetupTpdFtpPassword = value; }
        }
        public TagSetupInfo SetupTpdSamplingTime
        {
            get { return m_SetupTpdSamplingTime; }
            set { m_SetupTpdSamplingTime = value; }
        }
        public TagSetupInfo SetupTpdTransferTime
        {
            get { return m_SetupTpdTransferTime; }
            set { m_SetupTpdTransferTime = value; }
        }
        public TagSetupInfo SetupTpdTransferPath
        {
            get { return m_SetupTpdTransferPath; }
            set { m_SetupTpdTransferPath = value; }
        }
        public TagSetupInfo SetupTasEqpName
        {
            get { return m_SetupTasEqpName; }
            set { m_SetupTasEqpName = value; }
        }
        public TagSetupInfo SetupTasFtpAddress
        {
            get { return m_SetupTasFtpAddress; }
            set { m_SetupTasFtpAddress = value; }
        }
        public TagSetupInfo SetupTasFtpPortNo
        {
            get { return m_SetupTasFtpPortNo; }
            set { m_SetupTasFtpPortNo = value; }
        }
        public TagSetupInfo SetupTasFtpUserId
        {
            get { return m_SetupTasFtpUserId; }
            set { m_SetupTasFtpUserId = value; }
        }
        public TagSetupInfo SetupTasFtpPassword
        {
            get { return m_SetupTasFtpPassword; }
            set { m_SetupTasFtpPassword = value; }
        }
        public TagSetupInfo SetupTasTransferTime
        {
            get { return m_SetupTasTransferTime; }
            set { m_SetupTasTransferTime = value; }
        }
        public TagSetupInfo SetupTasTransferPath
        {
            get { return m_SetupTasTransferPath; }
            set { m_SetupTasTransferPath = value; }
        }
        public XLog FtpLog
        {
            get { return m_FtpLog; }
        }
        #endregion

        #region Methods
        public bool InitializeServerFtp()
        {
            m_SetupTpdEqpName = new TagSetupInfo("TPD : Equipment Name", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "HDC");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdEqpName);
            m_SetupTpdFtpAddress = new TagSetupInfo("TPD : FTP Server IP Address", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "127.0.0.1");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdFtpAddress);
            m_SetupTpdFtpPortNo = new TagSetupInfo("TPD : FTP Server Port Number", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "5000");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdFtpPortNo);
            m_SetupTpdFtpUserId = new TagSetupInfo("TPD : FTP Server User ID", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "dms");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdFtpUserId);
            m_SetupTpdFtpPassword = new TagSetupInfo("TPD : FTP Server Password", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "dms");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdFtpPassword);
            m_SetupTpdSamplingTime = new TagSetupInfo("TPD : TPD Sampling Interval", OptionType.None, OptionFormat.String, Dms.Common.UnitType.sec, "10");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdSamplingTime);
            m_SetupTpdTransferTime = new TagSetupInfo("TPD : TPD Transfer Interval", OptionType.None, OptionFormat.String, Dms.Common.UnitType.sec, "10");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdTransferTime);
            m_SetupTpdTransferPath = new TagSetupInfo("TPD : FTP Transfer Path", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "\\tpd");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTpdTransferPath);

            m_SetupTasEqpName = new TagSetupInfo("TAS : Equipment Name", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "HDC");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasEqpName);
            m_SetupTasFtpAddress = new TagSetupInfo("TAS : FTP Server IP Address", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "127.0.0.1");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasFtpAddress);
            m_SetupTasFtpPortNo = new TagSetupInfo("TAS : FTP Server Port Number", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "5000");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasFtpPortNo);
            m_SetupTasFtpUserId = new TagSetupInfo("TAS : FTP Server User ID", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "dms");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasFtpUserId);
            m_SetupTasFtpPassword = new TagSetupInfo("TAS : FTP Server Password", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "dms");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasFtpPassword);
            m_SetupTasTransferTime = new TagSetupInfo("TAS : TAS Transfer Interval", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.min, "1");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasTransferTime);
            m_SetupTasTransferPath = new TagSetupInfo("TAS : FTP Transfer Path", OptionType.None, OptionFormat.String, Dms.Common.UnitType.None, "\\tpd");
            m_DataProvider.SetupFtpInfo.InitFromDB(m_SetupTasTransferPath);

            ALM_TpdConnect = new Alarm("TPD Ftp Connection Fail.", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_TpdTransfer = new Alarm("TPD File Transfer Fail.", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_TpdPath = new Alarm("Incorrect TPD Transfer Path.", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);

            ALM_TasConnect = new Alarm("TAS Ftp Connection Fail.", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_TasTransfer = new Alarm("TAS File Transfer Fail.", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_TasPath = new Alarm("Incorrect TAS Transfer Path.", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);

            AppConfig app = new AppConfig();

            return true;
        }

        private void UninitializeServerFtp()
        {

        }
        public void SetFtpLog(string msg)//string seqName, int portNo, int slotNo, string message)
        {
            //string portName;
            //string slotName;
            //string log;

            //if (portNo <= 0) portName = "";
            //else
            //{
            //    portName = portNo.ToString();
            //}

            //if (slotNo <= 0) slotName = "";
            //else
            //{
            //    slotName = slotNo.ToString();
            //}

            //log = string.Format("FTP\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_FtpLog.TextOut(msg);
         //   log = string.Format("{0}", msg);
            this.m_GenInfos.EqpLog = msg;
        }
        #endregion
    }
}
