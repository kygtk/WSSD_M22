using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;

namespace Dms.Server
{
    public unsafe class SeqSharedMem : XSequence
    {
        #region struct
        unsafe struct CMapData
        {
            public bool bAlive;	// Program Start
            public UInt32 dwCpu_Share;
            public UInt32 dwMemory_Share;
        }
        #endregion

        #region Fields
        private ServerManager m_Server;
        private SharedMemory ShareMem = null;
        private CMapData* Mapdata;
        private PcStatus m_PcStatus;
        public bool m_mappingSuccess;
        private int HandleCnt = 0;
        private CheckUnitCondForm m_CheckCondView;
        #endregion

        #region Construct
        public SeqSharedMem(int scanTime, ServerManager server)            
        {
            m_Server = server;
            m_mappingSuccess = false;
            InitSharedMem();
            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqMemoryOverDetector(this, m_Server));
            RegisterSequence(new SeqMemoryOverDetectorAlive(this));
        }
        #endregion

        #region Override
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                foreach (XSeqFunction seq in SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Methods
        public unsafe void InitSharedMem()
        {
            if(!SetMapping())
            {
                //Partsmanager start
                Process MappingProcess = new Process();
                string FilePath = Environment.CurrentDirectory;
                MappingProcess.StartInfo.FileName = FilePath + "\\HDCPartsManager";
                MappingProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                MappingProcess.StartInfo.CreateNoWindow = true;                   
                MappingProcess.Start();
                
                Thread.Sleep(1000);
                if(!SetMapping())
                {
                    MessageBox.Show("Mapping이 실패하였습니다.Memory Detector기능이 작동되지 않습니다.", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    m_mappingSuccess = false;
                }
            }
            //process handler 갯수를 구하자
            Process[] m_Process = System.Diagnostics.Process.GetProcessesByName("HDCPartsManager");
            if (m_Process.Length != 0)
            {
                HandleCnt = m_Process[0].HandleCount;
            }
            //if (m_CheckCondView == null || m_CheckCondView.IsDisposed)
            //{
            //    m_CheckCondView = new CheckUnitCondForm();
            //   // m_CheckCondView.Initialize(m_Control);
            //    m_CheckCondView.Show();
            //}
        }
        public unsafe uint GetMemory()
        {
            uint uRv = 0;
            try
            {
                uRv = Mapdata->dwMemory_Share;
            }
            catch
            {
                uRv = 0;
            }
            return uRv;
        }
        public unsafe uint GetCpu()
        {
            uint uRv = 0;
            try
            {
                uRv = Mapdata->dwCpu_Share;
            }
            catch
            {
                uRv = 0;
            }
            return uRv;
        }
        public unsafe bool GetAlive()
        {
            bool bRv = false;
            
            try
            {
                bRv = Mapdata->bAlive;
            }
            catch
            {
                bRv = false;
            }
            return bRv;
        }
        public unsafe bool IsRun()
        {
            bool brun = false;
            brun = ShareMem.Root != IntPtr.Zero ? true : false;
            return brun;
        }
        public bool IsProcessrun()
        {
            try
            {
                int nHandleCnt;
                Process[] m_Process = System.Diagnostics.Process.GetProcessesByName("HDCPartsManager");
             
                if (m_Process.Length != 0)
                    nHandleCnt = m_Process[0].HandleCount;
                else
                    return false;

                if (m_Process[0].MainModule.EntryPointAddress == IntPtr.Zero)
                    return false;
                //if (nHandleCnt < HandleCnt)
                //{
                ////    m_Process[0].Kill();
                //    return false;
                //}
                if (//nHandleCnt >= HandleCnt &&
                    m_Process[0].Responding == true)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }

        }
        public void SetForceKill()
        {
            Process[] m_Process = System.Diagnostics.Process.GetProcessesByName("HDCPartsManager");
            if (m_Process.Length != 0)
                m_Process[0].Kill();
        }
        public unsafe void SetUnmapping()
        {
            m_mappingSuccess = false;
            if (m_PcStatus != null)
                m_PcStatus.Hide();
            //if (m_PcStatus != null)
            //{
            //    m_PcStatus.Close();
            //    m_PcStatus = null;
            //}
            if (ShareMem != null && ShareMem.Root != IntPtr.Zero)
                ShareMem.Dispose();

        }
        public unsafe bool SetMapping()
        {
            ShareMem = new SharedMemory("CPCState", true, 0);
            if (ShareMem.Root != IntPtr.Zero)
            {
                void* root = ShareMem.Root.ToPointer();
                Mapdata = (CMapData*)root;
                int Maxmem = m_Server.SetupMemoryLImit.GetValue<int>();
                int Maxcpu = 100;
                if (m_PcStatus == null)
                {
                    m_PcStatus = new PcStatus(Maxmem, Maxcpu);

                    m_PcStatus.SetProgressMem(Mapdata->dwMemory_Share);
                    m_PcStatus.SetProgressCpu(Mapdata->dwCpu_Share);
                }
                m_PcStatus.Show();


                m_mappingSuccess = true;
            }
            else
                m_mappingSuccess = false;
            return m_mappingSuccess;
        }
        public unsafe void UpdateData()
        {
            Process[] m_Process = System.Diagnostics.Process.GetProcessesByName("HDCPartsManager");
            if (m_Process.Length != 0)
          //  if (ShareMem.Root != IntPtr.Zero)
            {
                m_PcStatus.SetProgressMem(Mapdata->dwMemory_Share);
                m_PcStatus.SetProgressCpu(Mapdata->dwCpu_Share);
                m_PcStatus.MaxMemory = m_Server.SetupMemoryLImit.GetValue<int>();
            }
        }
        #endregion
    }

    public class SeqMemoryOverDetector : XSeqFunction
    {
        #region Fields
        private static IEqpManager m_Eqp;
        private static ServerManager m_Server;
        private GenInfoHandler m_GenInfo;
        private SeqSharedMem m_Share;
        private Alarm m_AlarmPcMemoryOver;
        private string msg;
        #endregion

        #region Constructor
        public SeqMemoryOverDetector(SeqSharedMem control, ServerManager server)
        {
            m_Server = server;
            m_Share = control;
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            m_Eqp = m_Server.EqpStateManager;
            m_AlarmPcMemoryOver = new Alarm("PC MEMORY Over Detect", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            SeqFunName = "SeqMemoryOverDetector";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_Share.m_mappingSuccess) return -1;
            m_Share.UpdateData();
            uint MemoryLimit = m_Server.SetupMemoryLImit.GetValue<uint>();
            uint Curmemory = m_Share.GetMemory();
           // uint Curmemory = 0;
            int seqNo = SeqNo;
            switch (seqNo)
            {
                case 0:
                    if (Curmemory >= MemoryLimit)
                    {
                        StartTicks = Common.XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (Curmemory < MemoryLimit)
                    {
                        seqNo = 0;
                    }
                    else if (GetElapsedTicks() > 1000)
                    {
                        AlarmId = m_AlarmPcMemoryOver.Id;
                        m_Eqp.SetAlarm(AlarmId);
                        // Log
                        msg = string.Format("Memory: %d kB", Curmemory);
                        m_Server.Log(msg);
                        seqNo = 1000;
                    }
                    break;

                case 1000:
                    if (Curmemory < MemoryLimit &&
                        m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        // Log
                        m_Server.Log("Recovery Memory Reset");
                        seqNo = 0;
                    }
                    break;
            }
            SeqNo = seqNo;
            return -1;
        }
        #endregion
    }

    public class SeqMemoryOverDetectorAlive : XSeqFunction
    {
        #region Fields
        private SeqSharedMem m_Share;
        private int nTryNO = 0;
        #endregion

        #region Constructor
        public SeqMemoryOverDetectorAlive(SeqSharedMem control)//, ServerManager server)
        {
            m_Share = control;
            SeqFunName = "SeqMemoryOverDetectorAlive";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //return -1;
            int seqNo = SeqNo;
           // if (!m_Share.IsProcessrun()) return -1;
            switch (seqNo)
            {
                case 0:
                    if (m_Share.GetAlive())
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    else
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    break;

                case 10:
                    if (!m_Share.GetAlive())
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_Share.SetUnmapping();
                        seqNo = 100;
                    }
                    break;
                case 20:
                    if (m_Share.GetAlive())
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_Share.SetUnmapping();
                        seqNo = 100;
                    }
                    break;
                case 100:
                    if (m_Share.IsProcessrun())//Process 살아 있는지 확인
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 110;
                    }
                    break;
                case 110:
                    if (GetElapsedTicks() > 2000)
                    {
                        if (m_Share.SetMapping())
                        {
                            nTryNO = 0;
                            StartTicks = XFunc.GetTickCount();
                            seqNo = 130;
                        }
                        else if (nTryNO > 3)//반복해도 안 되면 재 시도 말자.
                        {
                            seqNo = 120;
                            nTryNO = 0;
                        }
                        nTryNO++;
                    }
                    break;
                case 120:
                    {
                        seqNo = 120;
                    }
                    break;
                case 130:
                    //정말 새로 살았는지 아니면 좀비 Process인지 한번더 체크
                    //아~~정말 이런 무식한 방법밖엔 없는겐가??
                    if (m_Share.GetAlive())
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 200;
                    }
                    else if (!m_Share.GetAlive())
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 300;
                    }
                    break;
                case 200:
                    if (!m_Share.GetAlive())
                    { //정말 살았다.                    
                        seqNo = 0;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {//좀비 process로 판단
                        m_Share.SetForceKill();
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 210;
                    }
                    break;
                case 210:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Share.SetUnmapping();
                       // m_Share.SetForceKill();
                        seqNo = 100;
                    }
                    break;
                case 300:
                    if (m_Share.GetAlive())
                    { //정말 살았다.                    
                        seqNo = 0;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {//좀비 process로 판단
                        m_Share.SetForceKill();
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 310;
                    }
                    break;
                case 310:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Share.SetUnmapping();
                        //m_Share.SetForceKill();
                        seqNo = 100;
                    }
                    break;
            }
            SeqNo = seqNo;
            return -1;
        }
        #endregion
    }
}
