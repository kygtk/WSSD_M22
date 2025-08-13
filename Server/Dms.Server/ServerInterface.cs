using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.Windows.Forms;
using System.Collections;
using Dms.Device;
using System.Reflection;
using Dms.Mitsubishi;

namespace Dms.Server
{
    partial class ServerManager // 10.12.21 minhan
    {
        #region Fields
        private TagSetupInfo m_SetupIfTimeoutL3;
        private TagSetupInfo m_SetupIfTimeoutL4;
        private TagSetupInfo m_SetupIfTimeoutU1;
        private TagSetupInfo m_SetupIfTimeoutU3;
        private TagSetupInfo m_SetupIfTimeoutU4;
        //private TagSetupInfo m_SetupIfTimeoutE1; // 11.06.10 minhan
        //private TagSetupInfo m_SetupIfTimeoutE3; // 11.06.10 minhan
        //private TagSetupInfo m_SetupIfTimeoutE4; // 11.06.10 minhan
        private TagSetupInfo m_DataCompTimeout; // 10.12.21 minhan데이터가 정상적으로 주고 받고 완료되는 시간의 Time Out 
   
        private XLog m_InterfaceLog = new XLog("InterfaceLog", XLog.LogStampType.UseStamp);

        private TransferData m_RecvData = new TransferData();
        private TransferData m_SendData = new TransferData();
        private TagRecipe m_RecvRecipe = new TagRecipe();

        public Alarm ALM_IfTimeoutL3;
        public Alarm ALM_IfTimeoutL4;
        public Alarm ALM_IfTimeoutU1;
        public Alarm ALM_IfTimeoutU3;
        public Alarm ALM_IfTimeoutU4;
        //public Alarm ALM_IfTimeoutE1; // 11.06.08 minhan
        //public Alarm ALM_IfTimeoutE3; // 11.06.08 minhan
        //public Alarm ALM_IfTimeoutE4; // 11.06.08 minhan
        public Alarm ALM_IfDataCompTimeout; // 10.12.27 minhan
        
        #endregion

        #region Properties
        public TagSetupInfo SetupIfTimeoutL3
        {
            get { return m_SetupIfTimeoutL3; }
            set { m_SetupIfTimeoutL3 = value; }
        }
        public TagSetupInfo SetupIfTimeoutL4
        {
            get { return m_SetupIfTimeoutL4; }
            set { m_SetupIfTimeoutL4 = value; }
        }
        public TagSetupInfo SetupIfTimeoutU1
        {
            get { return m_SetupIfTimeoutU1; }
            set { m_SetupIfTimeoutU1 = value; }
        }
        public TagSetupInfo SetupIfTimeoutU3
        {
            get { return m_SetupIfTimeoutU3; }
            set { m_SetupIfTimeoutU3 = value; }
        }
        public TagSetupInfo SetupIfTimeoutU4
        {
            get { return m_SetupIfTimeoutU4; }
            set { m_SetupIfTimeoutU4 = value; }
        }
        //public TagSetupInfo SetupIfTimeoutE1 // 11.06.08 minhan
        //{
        //    get { return m_SetupIfTimeoutE1; }
        //    set { m_SetupIfTimeoutE1 = value; }
        //}
        //public TagSetupInfo SetupIfTimeoutE3 // 11.06.08 minhan
        //{
        //    get { return m_SetupIfTimeoutE3; }
        //    set { m_SetupIfTimeoutE3 = value; }
        //}
        //public TagSetupInfo SetupIfTimeoutE4 // 11.06.08 minhan
        //{
        //    get { return m_SetupIfTimeoutE4; }
        //    set { m_SetupIfTimeoutE4 = value; }
        //}
        public TagSetupInfo DataCompTimeout
        {
            get { return m_DataCompTimeout; }
            set { m_DataCompTimeout = value; }
        }
        public XLog InterfaceLog
        {
            get { return m_InterfaceLog; }
        }
        public TransferData RecvData
        {
            get { return m_RecvData; }
            set { m_RecvData = value; }
        }
        public TransferData SendData
        {
            get { return m_SendData; }
            set { m_SendData = value; }
        }
        public TagRecipe RecvRecipeBody
        {
            get { return m_RecvRecipe; }
            set { m_RecvRecipe = value; }
        }
        #endregion

        public bool InitializeServerInterface()
        {
            m_SetupIfTimeoutL3 = new TagSetupInfo("LD :Robot Accessing to Cleaner On Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "10");
            m_DataProvider.SetupInterface.InitFromDB(m_SetupIfTimeoutL3);
            m_SetupIfTimeoutL4 = new TagSetupInfo("LD :Robot Accessing to Cleaner Off Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "30");
            m_DataProvider.SetupInterface.InitFromDB(m_SetupIfTimeoutL4);
            SetupIfTimeoutU1 = new TagSetupInfo("UL :Transfer Ready to Cleaner On Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "90");
            m_DataProvider.SetupInterface.InitFromDB(SetupIfTimeoutU1);
            SetupIfTimeoutU3 = new TagSetupInfo("UL :Robot Accessing to Cleaner On Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "10");
            m_DataProvider.SetupInterface.InitFromDB(SetupIfTimeoutU3);
            SetupIfTimeoutU4 = new TagSetupInfo("UL :Robot Accessing to Cleaner Off Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "30");
            m_DataProvider.SetupInterface.InitFromDB(SetupIfTimeoutU4);
            //SetupIfTimeoutE1 = new TagSetupInfo("EX :Transfer Ready to Cleaner On Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "30"); // 11.06.08 minhan
            //m_DataProvider.SetupInterface.InitFromDB(SetupIfTimeoutE1);
            //SetupIfTimeoutE3 = new TagSetupInfo("EX :Robot Accessing to Cleaner On Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "30"); // 11.06.08 minhan
            //m_DataProvider.SetupInterface.InitFromDB(SetupIfTimeoutE3);
            //SetupIfTimeoutE4 = new TagSetupInfo("EX :Robot Accessing to Cleaner Off Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "30"); // 11.06.08 minhan
            //m_DataProvider.SetupInterface.InitFromDB(SetupIfTimeoutE4);
            m_DataCompTimeout = new TagSetupInfo("Glass Data Interface Complete Timeout", OptionType.None, OptionFormat.Digit, Dms.Common.UnitType.sec, "10"); // 10.12.21 minhan
            m_DataProvider.SetupInterface.InitFromDB(m_DataCompTimeout);

            ALM_IfTimeoutL3 = new Alarm("Glass Recv T3 Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning);
            ALM_IfTimeoutL4 = new Alarm("Glass Recv T4 Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning);
            ALM_IfTimeoutU1 = new Alarm("Glass Send T1 Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_IfTimeoutU3 = new Alarm("Glass Send T3 Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning);
            ALM_IfTimeoutU4 = new Alarm("Glass Send T4 Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning);
            //ALM_IfTimeoutE1 = new Alarm("Glass Exchange T1 Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.06.10 minhan
            //ALM_IfTimeoutE3 = new Alarm("Glass Exchange T3 Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning); // 11.06.10 minhan
            //ALM_IfTimeoutE4 = new Alarm("Glass Exchange T4 Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning); // 11.06.10 minhan
            ALM_IfDataCompTimeout = new Alarm("Glass Data Comp Timeout", AlarmLevel.S, AlarmCode.EquipmentStatusWarning); // 10.12.21 minhan

            //AppConfig app = AppConfig.Instance;

            //if (app.Simul.Melsec)
            //{
            //    if (m_RootNode != null)
            //    {
            //        m_RootNode.IfLoaderSend.MibUpstreamReady.SetStatus(true);
            //        m_RootNode.IfLoaderRecv.MibDownstreamReady.SetStatus(true);
            //    }
            //}

            return true;
        }

        private void UninitializeServerInterface()
        {
            //if(m_RootNode != null)
            //    ResetSignal();
        }

        private void ResetSignal()
        {
            //ArrayList items1 = new ArrayList();
            //GetCollection(m_RootNode.GetNode(), ref items1, typeof(MelsecBitOutput));
            //foreach (MelsecBitOutput melDevice in items1)
            //{
            //    melDevice.SetStatus(false);
            //}

            //ArrayList items4 = new ArrayList();
            //GetCollection(m_RootNode.GetNode(), ref items4, typeof(MelsecWordOutput));
            //foreach (MelsecWordOutput melDevice in items4)
            //{
            //    short[] zero = new short[melDevice.Size];
            //    melDevice.SetValues(zero);
            //}
        }

        private void GetCollection(TreeNode currentNode, ref ArrayList items, Type type)
        {
            object currentObject = currentNode.Tag;

            if (currentObject.GetType() == type)
            {
                items.Add(currentObject);
            }

            foreach (TreeNode childNode in currentNode.Nodes)
            {
                GetCollection(childNode, ref items, type);
            }

            PropertyInfo[] propertyInfos = currentObject.GetType().GetProperties();
            foreach (PropertyInfo info in propertyInfos)
            {
                if (type == info.PropertyType)
                {
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MelsecDevice mel = getMethodInfo.Invoke(currentObject, null) as MelsecDevice;

                    //if (m_Melsec.SimulateAddress)
                    //{
                    //    mel.Name = (currentObject as DmsNode).GetOriginalName() + " : " + info.Name;
                    //}

                    items.Add(mel);
                }
            }
        }

        public void SetInterfaceLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("Interface\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_InterfaceLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            this.m_GenInfos.CommLog = log;
            //m_Server.GenInfos.EqpLog = log;
        }
    }
}
