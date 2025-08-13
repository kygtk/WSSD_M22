using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading;
#region Description
//Write by Kimgun
//date: 2010.06.22
//유량계(lpm)를 사용하는 gauge의 토탈 적산 유량을 표시.
#endregion
namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class FlowAccumulate : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorAccumulate tagDescriptor = new TagDescriptorAccumulate();
        #endregion

        #region Fields
        private _GenericCollection<_Gauge> m_Accumulates = new _GenericCollection<_Gauge>();
        private FlowAccuItem m_Item = null;
        // private _Gauge m_gauge = null;
        private uint m_StartTicks = 0;
        private string m_Accumulate = "";//등록된 유량계의 토탈 적산 유량
        private string m_OldAccumulate = "";
        private int m_HoldTime = 1;//적산 data를 기억하는 시간 Hour 
        private int seqNo = 0;
        private bool m_DailyCehck = true;
        private XTimer m_Timer = null;
        private System.Threading.Timer m_ThreadingTimer = null;
        private int kmtest = 0;
        #endregion

        #region Properties
        [Category("Group")]
        [Description("적산량을 수집할 유량계들을 등록")]
        public _GenericCollection<_Gauge> Accumulates
        {
            get { return m_Accumulates; }
            set { m_Accumulates = value; }
        }
        [Category("Set")]
        [Description("적산량을 유지할 시간을 표시")]
        public int HoldTime
        {
            get { return m_HoldTime; }
            set { m_HoldTime = value; }
        }
        [Category("Set")]
        [Description("true면 holdTime 단위가 day로 바뀐다.(true:day,false:Hour)")]
        //hour는 저장 시간 기준이기때문에 저장 시간부터 설정시간까지다.
        //day는 무조건 날짜만 보기때문에 저장 시점이 24시간이 되지 않아도 날짜만 바뀌면 인정된다.
        public bool DailyCehck
        {
            get { return m_DailyCehck; }
            set { m_DailyCehck = value; }
        }
        #endregion

        #region Constructor
        public FlowAccumulate()
        {
            this.Name = "__ Unit";
        }
        #endregion

        #region Methods
        public void SeqFlowAccumulate(Object stateInfo)
        {
            int nSeqNo = seqNo;
            switch (nSeqNo)
            {
                case 0:
                    if (!m_Item.HoldTimeCheck(HoldTime, DailyCehck))
                    {
                        foreach (_Gauge gauge in m_Accumulates)
                        {
                            double CurVal = gauge.CurValue / 60;
                            m_Item.TotalFlow += CurVal;
                        }
                        m_Item.UpdateData();
                        m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
                        nSeqNo = 10;
                    }
                    else
                    {
                        foreach (_Gauge gauge in m_Accumulates)
                        {
                            double CurVal = gauge.CurValue / 60;
                            m_Item.TotalFlow += CurVal;
                        }
                        m_Item.OldTotalFlow = m_Item.TotalFlow;
                        m_OldAccumulate = string.Format("{0:F2}", m_Item.OldTotalFlow);
                        m_Item.TotalFlow = 0;
                        m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        m_Timer.Start(60 * 1000);
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (XFunc.GetTickCount() - m_StartTicks > 1000)
                    {
                        foreach (_Gauge gauge in m_Accumulates)
                        {
                            double CurVal = gauge.CurValue / 60;
                            m_Item.TotalFlow += CurVal;
                        }
                        m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
                        m_StartTicks = XFunc.GetTickCount();
                        kmtest++;
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (m_Timer.Over) nSeqNo = 40;
                    else if (XFunc.GetTickCount() - m_StartTicks > 1000)
                    {
                        foreach (_Gauge gauge in m_Accumulates)
                        {
                            double CurVal = gauge.CurValue / 60;
                            m_Item.TotalFlow += CurVal;
                        }
                        m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
                        m_StartTicks = XFunc.GetTickCount();
                        kmtest++;
                        nSeqNo = 20;
                    }
                    break;
                case 40:
                    if (!m_Item.HoldTimeCheck(HoldTime, DailyCehck))
                    {
                        foreach (_Gauge gauge in m_Accumulates)
                        {
                            double CurVal = gauge.CurValue / 60;
                            m_Item.TotalFlow += CurVal;
                        }
                        m_Item.UpdateData();
                        m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
                        m_StartTicks = XFunc.GetTickCount();
                        kmtest++;
                        nSeqNo = 10;
                    }
                    else
                    {
                        foreach (_Gauge gauge in m_Accumulates)
                        {
                            double CurVal = gauge.CurValue / 60;
                            m_Item.TotalFlow += CurVal;
                        }
                        m_Item.OldTotalFlow = m_Item.TotalFlow;
                        m_OldAccumulate = string.Format("{0:F2}", m_Item.OldTotalFlow);
                        m_Item.TotalFlow = 0;
                        m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
                        nSeqNo = 10;
                    }
                    break;
            }
            seqNo = nSeqNo;
        }
        public void FlowReset()
        {
            m_Item.TotalFlow = 0;
            m_Accumulate = string.Format("{0:F2}", m_Item.TotalFlow);
        }
        #endregion

        #region Override
        //public override Type FamilyType
        //{
        //    get { return typeof(ActuatorUnit); }
        //}
        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                //Tag 생성전에 먼저 초기화를 해야 된다.
                m_Item = new FlowAccuItem(this.Name, HoldTime);
                m_Item.InitParameter();
                m_OldAccumulate = string.Format("{0:F2}", m_Item.OldTotalFlow);
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();
                m_Timer = new XTimer("DayCheckTimer");
                m_ThreadingTimer = new System.Threading.Timer(new TimerCallback(SeqFlowAccumulate), null, 0, 100);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {

                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)
            {
                //XFunc.ExceptionHandler.Add(err, ExceptionLevel.Shutdown);
            }
        }

        public override void UpdateTag()
        {
            // SeqFlowAccumulate();
            m_Tag.SetValue(tagDescriptor.CurValue, m_Accumulate);
            m_Tag.SetValue(tagDescriptor.OldValue, m_OldAccumulate);
        }
        #endregion
    }
    #region 제거
    //public class FlowAccumulate : _DeviceAsm
    //{
    //    #region Tag Descriptor
    //    protected static TagDescriptorAccumulate tagDescriptor = new TagDescriptorAccumulate();
    //    #endregion
    //    #region enum
    //    public enum UtilGroup
    //    {
    //        Group1,
    //        Group2,
    //        Group3,
    //        Group4,
    //        Group5,
    //        Group6,
    //        Group7,
    //        Group8,
    //        Group9,
    //        Group10,
    //    }
    //    #endregion
    //    #region Fields
    //    private DeviceTags m_TagContainer = null;
    //    private AutoValve m_AutoValve = null;
    //    private Pump m_Pump = null;
    //    private Gauge m_gauge = null;
    //    private ushort m_PointCount = 1;//소수점 자리수 default로 소수점 한자리 사용.
    //    private uint m_StartTicks = 0;
    //    private double m_AccumulateValue = 0;
    //    private string m_Accumulate = "";//등록된 유량계의 각각의 적산 유량
    //    private string m_TotalAccumulate = "";//등록된 유량계의 토탈 적산 유량
    //    private UtilGroup m_UtilGrop = UtilGroup.Group1;
    //    private int m_HoldTime = 1;//적산 data를 기억하는 시간 Hour 
    //    private int seqNo = 0;
    //    private static FlowAccuItem m_FlowAccuItem = null;
    //    private static XTimer m_Timer = null;



    //    #endregion

    //    #region Properties
    //    [Category("Condition Setting")]
    //    [Description("유량계에 부착된 auto Valve")]
    //    public AutoValve AutoValve
    //    {
    //        get { return m_AutoValve; }
    //        set { m_AutoValve = value; }
    //    }
    //    [Category("Condition Setting")]
    //    public Pump pump
    //    {
    //        get { return m_Pump; }
    //        set { m_Pump = value; }
    //    }
    //    [Category("Setting")]
    //    public Gauge gauge
    //    {
    //        get { return m_gauge; }
    //        set { m_gauge = value; }
    //    }

    //    [Category("Setting")]
    //    public ushort PointCount
    //    {
    //        get { return m_PointCount; }
    //        set { m_PointCount = value; }
    //    }
    //    [Category("Setting")]
    //    public UtilGroup group
    //    {
    //        get { return m_UtilGrop; }
    //        set { m_UtilGrop = value; }
    //    }
    //    [Category("Setting")]
    //    public int HoldTime
    //    {
    //        get { return m_HoldTime; }
    //        set { m_HoldTime = value; }
    //    }

    //    #endregion

    //    #region Constructor
    //    public FlowAccumulate() : this("__")
    //    {
    //        //this.Name = "__";
    //        //m_RefTagInfo = new DeviceTagInfo(this.GetType().Name);
    //    }

    //    public FlowAccumulate(string name) 
    //    {
    //        this.Name = name;
    //       // m_RefTagInfo = new DeviceTagInfo(this.GetType().Name);
    //    }
    //    #endregion

    //    #region Method
    //    public void SeqFlowAccumulate()
    //    {
    //        int nSeqNo = seqNo;
    //        bool checkCond = true;

    //        checkCond &= AutoValve == null || AutoValve.DoOpen.GetState();
    //        checkCond &= pump == null || pump.IsRun();
    //        checkCond &= m_gauge != null;
    //       //checkCond &= hpmj == null || hpmj.Pump.IsRun();
    //        switch (nSeqNo)
    //        {

    //            case 0:
    //                if (checkCond)
    //                {
    //                    m_StartTicks = XFunc.GetTickCount();
    //                    m_Timer.Start(60 * 1000);
    //                    nSeqNo = 10;
    //                }
    //                break;
    //            case 10:
    //                if (XFunc.GetTickCount() - m_StartTicks > 1000)
    //                {
    //                    double CurVal = m_gauge.CurValue / 60;
    //                    //등록된 device total 유량용
    //                    m_TotalAccumulate = string.Format("{0:02}", m_FlowAccuItem.AccumulateUpdate(CurVal, (int)group));
    //                    //개별 유량용
    //                    m_AccumulateValue += CurVal;
    //                    m_Accumulate = string.Format("{0:02f}", m_AccumulateValue);
    //                    m_StartTicks = XFunc.GetTickCount();
    //                    nSeqNo = 20;
    //                }
    //                break;
    //            case 20:
    //                if (m_Timer.Over) nSeqNo = 30;
    //                else
    //                {
    //                    if (XFunc.GetTickCount() - m_StartTicks > 1000)
    //                    {
    //                        double CurVal = m_gauge.CurValue / 60;
    //                        //등록된 device total 유량용
    //                        m_TotalAccumulate = string.Format("{0:02}", m_FlowAccuItem.AccumulateUpdate(CurVal, (int)group));
    //                        //개별 유량용
    //                        m_AccumulateValue += CurVal;
    //                        m_Accumulate = string.Format("{0:02f}", m_AccumulateValue);
    //                        m_StartTicks = XFunc.GetTickCount();
    //                        nSeqNo = 10;
    //                    }
    //                }
    //                break;
    //            case 30:
    //                if (!m_FlowAccuItem.HoldTimeCheck(HoldTime))
    //                {
    //                    double CurVal = m_gauge.CurValue / 60;
    //                    //등록된 device total 유량용
    //                    m_TotalAccumulate = string.Format("{0:02}", m_FlowAccuItem.AccumulateUpdate(CurVal, (int)group));
    //                    //개별 유량용
    //                    m_AccumulateValue += CurVal;
    //                    m_Accumulate = string.Format("{0:02f}", m_AccumulateValue);
    //                    nSeqNo = 0;
    //                }
    //                else
    //                {
    //                    m_AccumulateValue = 0;
    //                    m_Accumulate = string.Format("{0:02f}", m_AccumulateValue);
    //                    nSeqNo = 0;
    //                }
    //                break;
    //        }
    //        seqNo = nSeqNo;
    //    }
    //    #endregion

    //    #region Override
    //    public override string ToString()
    //    {
    //        return this.Name;
    //    }
    //    public override void CreateTag(DeviceTags tagContainer)
    //    {
    //        try
    //        {
    //            m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
    //        }
    //        catch (Exception err)   
    //        {
    //            XFunc.ExceptionHandler.Add(err, ExceptionLevel.Shutdown);
    //        }
    //    }

    //    public override void UpdateTag()
    //    {
    //        SeqFlowAccumulate();
    //        m_Tag.SetValue(tagDescriptor.SingleValue, m_Accumulate + "lpm");
    //        m_Tag.SetValue(tagDescriptor.TotalValue, m_Accumulate + "lpm");
    //    }

    //    public override DmsErrors Initialize()
    //    {
    //        ////////////////////////////////////////////////////////////////////////////////////////
    //        // 초기화 순서는 아래의 Flow를 따라야 한다.
    //        ////////////////////////////////////////////////////////////////////////////////////////


    //        ////////////////////////////////////////////////////////////////////////////////////////
    //        // 1. 이미 초기화완료 되었는지 Check
    //        if (Initialized == true) return DmsErrors.Success;


    //        ////////////////////////////////////////////////////////////////////////////////////////
    //        // 2. DeviceI/O 등록
    //        bool ok = true;
    //        ok &= GenerateAssociatedIoDevices();


    //        ////////////////////////////////////////////////////////////////////////////////////////
    //        // 3. 필수 I/O 들이 등록되어 있는지 Check
    //        #region Example
    //        //ok &= (m_DiAlarm != null);
    //        ok &= m_gauge != null;
    //        #endregion


    //        ////////////////////////////////////////////////////////////////////////////////////////
    //        if (!ok)
    //        {
    //            SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
    //            return DmsErrors.NotInitialized;
    //        }
    //        else
    //        {
    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 4. Tag 생성
    //            m_TagContainer = m_Server.TagContainer;
    //            CreateTag(m_TagContainer);

    //            //if (!SyncRefDeviceTag(m_TagContainer))
    //            //{
    //            //    return DmsErrors.InternalError;
    //            //}


    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 5. Alarm Item 생성
    //            #region Example
    //            //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
    //            #endregion


    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 6. SetupItem 생성
    //            #region Example
    //            //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
    //            //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
    //            #endregion


    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 7. 기타 초기화 조건
    //            if(m_Timer == null)
    //                m_Timer = new XTimer("DayCheckTimer ");

    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 8. Tag Update Timer 등록
    //            SetSubscriber();

    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 9. I/O 초기값 설정, Simulation code
    //            #region Example
    //            //if (m_Simul.Device)
    //            //{
    //            //    m_DiReady.SetState(true);
    //            //}
    //            #endregion


    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 10. Set Flag
    //            m_Initialized = ok;


    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 11. 초기화완료 확인이후 수행 조건
    //            if (m_Initialized)
    //            {
    //                if (m_FlowAccuItem == null)
    //                {
    //                    m_FlowAccuItem = new FlowAccuItem(this.Name,sizeof(UtilKinds));
    //                    m_FlowAccuItem.InitParameter();
    //                }
    //            }


    //            ////////////////////////////////////////////////////////////////////////////////////////
    //            // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
    //            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
    //        }
    //    }

    //    //public override Type FamilyType 필요한지는 조금 있다가 판단
    //    //{
    //    //    get
    //    //    {
    //    //        return typeof(ApdItem);
    //    //    }
    //    //}
    //    #endregion
    //}
    #endregion
}
