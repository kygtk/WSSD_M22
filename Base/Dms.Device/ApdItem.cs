using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class ApdItem : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorApdItem tagDescriptor = new TagDescriptorApdItem();
        #endregion

        #region Fields
        protected DeviceTag m_RefTag = null;        //이 APD Item이 참조해야 할 Tag, 없는 경우도 있겠지?
        private DeviceTagInfo m_RefTagInfo = null;  //Designer에서 참조해야 할 Tag를 선택해주기 위한 DeviceTagInfo
        //private string m_RefTagDescriptor = "";   //참조해야 할 Tag의 Descriptor중에서 선택
        private TagDescriptor m_RefTagDescriptor;   //참조해야 할 Tag의 Descriptor중에서 선택
        private DeviceTags m_TagContainer = null;
        private string m_Value = "";
        private CvUnit m_OwnerUnit = null;
        private string m_OwnerUnitName = "";
        private UnitType m_ItemUnit = UnitType.None;
        private int m_WordCount = 1;
        private int m_StartAddress = 1; // 11.02.09 minhan
        private int m_StartTpdAddress = 1;
        private double m_Rate = 1.0;
        private Format m_Format = Format.NUMBER;
        private bool m_ReportEnable = true;
        private bool m_LpdReportEnable = false;  //-2   //jemoon : default false로
        private bool m_TpdReportEnable = false;  //-3   //jemoon : default false로
        #endregion

        #region Properties
        [Category("Setting")]
        public bool ReportEnable
        {
            get { return m_ReportEnable; }
            set { m_ReportEnable = value; }
        }
        [Category("Setting")]
        public bool LpdReportEnable
        {
            get { return m_LpdReportEnable; }
            set { m_LpdReportEnable = value; }
        }
        [Category("Setting")]
        public bool TpdReportEnable
        {
            get { return m_TpdReportEnable; }
            set { m_TpdReportEnable = value; }
        }

        [Category("Setting")]
        public DeviceTagInfo ReferenceTag
        {
            get { return m_RefTagInfo; }
            set { m_RefTagInfo = value; }
        }
        [Category("Setting")]
        public TagDescriptor ReferenceTagDescriptor
        {
            get { return m_RefTagDescriptor; }
            set { m_RefTagDescriptor = value; }
        }
        [Category("Setting")]
        public CvUnit OwnerUnit
        {
            get { return m_OwnerUnit; }
            set { m_OwnerUnit = value; }
        }
        [Category("Setting")]
        public UnitType ItemUnit
        {
            get { return m_ItemUnit; }
            set { m_ItemUnit = value; }
        }
        [Category("Setting"), Description("PLC Memory Word Count")]
        public int WordCount
        {
            get { return m_WordCount; }
            set { m_WordCount = value; }
        }
        [Category("Setting"), Description("Start Address")]
        public int StartAddress
        {
            get { return m_StartAddress; }
            set { m_StartAddress = value; }
        }
        [Category("Setting"), Description("TPD Start Address")]
        public int StartTpdAddress
        {
            get { return m_StartTpdAddress; }
            set { m_StartTpdAddress = value; }
        }
        [Category("Setting")]
        public double Rate
        {
            get { return m_Rate; }
            set { m_Rate = value; }
        }
        [Category("Setting")]
        public Format Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }
        [Browsable(false)]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        [Category("Setting"), Browsable(false), XmlIgnore()]
        public string OwnerUnitName
        {
            get
            {
                if (m_OwnerUnit != null) return m_OwnerUnit.Name;
                else return m_OwnerUnitName;
            }
            set { m_OwnerUnitName = value; }
        }
        #endregion

        #region Constructor
        public ApdItem() : this("__")
        {
            //this.Name = "__";
            //m_RefTagInfo = new DeviceTagInfo(this.GetType().Name);
        }

        public ApdItem(string name)
        {
            this.Name = name;
            m_RefTagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Method
        // Reference Device Tag를 찾아온다.
        public bool SyncRefDeviceTag(DeviceTags tagContainer)
        {
            if (m_TagContainer == null) m_TagContainer = tagContainer;

            // Ref Tag Info 가 Setting 된경우에만
            if (m_RefTagInfo != null)
            {
                m_RefTag = m_TagContainer[m_RefTagInfo.DeviceName];

                // RefTag가 존재한다면
                if (m_RefTag != null)
                {
                    bool ok = true;

                    if (m_RefTagDescriptor == null)
                    {   // jemoon : RefTag는 선택되어 져 있는데 TagDescriptor가 선택되지 않았다면 오류
                        ok = false;
                    }
                    else if (m_RefTag.Items.Count <= m_RefTagDescriptor.Id)
                    {   // jemoon : RefTag가 바뀌었는데 TagDescriptor가 갱신되지 않았다면 오류
                        ok = false;
                    }
                    else if (m_RefTag[m_RefTagDescriptor.Id] == null)
                    {   // jemoon : RefTag가 바뀌었는데 TagDescriptor가 갱신되지 않았다면 오류
                        ok = false;
                    }

                    if (!ok)
                    {
                        MessageBox.Show("APD Item Reference mismatched!");
                        return false;
                    }
                }
            }

            return true;
        }

        //public ApdItem Clone(ApdItem originItem)
        //{
        //    ApdItem newItem = new ApdItem();
        //    newItem.Id = originItem.Id;
        //    newItem.Value = originItem.Value;
        //    newItem.m_RefTag = originItem.m_RefTag;
        //    newItem.m_RefTagDescriptor = originItem.m_RefTagDescriptor;
        //    newItem.m_RefTagInfo = originItem.m_RefTagInfo;
        //    newItem.m_Tag = originItem.m_Tag;
        //    newItem.Name = originItem.Name;
        //    newItem.m_OwerUnitName = originItem.m_OwerUnitName;
        //    newItem.m_ItemUnit = originItem.m_ItemUnit;
        //    return newItem;
        //}

        public ApdItem Clone()
        {
            ApdItem newItem = new ApdItem();
            newItem.m_Id = this.m_Id;
            newItem.m_Value = this.m_Value;
            newItem.m_RefTag = this.m_RefTag;
            newItem.m_RefTagDescriptor = this.m_RefTagDescriptor;
            newItem.m_RefTagInfo = this.m_RefTagInfo;
            newItem.m_Tag = this.m_Tag;
            newItem.m_Name = this.m_Name;
            //Only m_OwnerUnitName 만이 필요하다
            //불필요한 Xml Serialize를 피하기 위해 OwnerUnit는 복사하지 않음
            //newItem.OwnerUnit = this.OwnerUnit;
            newItem.m_OwnerUnitName = this.OwnerUnitName;
            newItem.m_ItemUnit = this.m_ItemUnit;
            newItem.m_WordCount = this.m_WordCount;
            newItem.m_StartAddress = this.m_StartAddress; // 11.02.09 minhan
            newItem.m_StartTpdAddress = this.m_StartTpdAddress;
            newItem.m_Rate = this.m_Rate;
            newItem.m_Format = this.m_Format;
            newItem.m_ReportEnable = this.m_ReportEnable;
            newItem.m_LpdReportEnable = this.m_LpdReportEnable;
            newItem.m_TpdReportEnable = this.m_TpdReportEnable;
            return newItem;
        }

        public bool IsMatchedProperties(ApdItem item)
        {
            bool matched = true;
            matched &= this.m_RefTagInfo.DeviceName == item.m_RefTagInfo.DeviceName;
            matched &= this.m_RefTagDescriptor == item.m_RefTagDescriptor;
            matched &= this.OwnerUnitName == item.OwnerUnitName;
            matched &= this.m_ItemUnit == item.m_ItemUnit;
            matched &= this.m_WordCount == item.m_WordCount;
            matched &= this.m_StartAddress == item.m_StartAddress; // 11.02.09 minhan
            matched &= this.m_StartTpdAddress == item.m_StartTpdAddress;
            matched &= this.m_Rate == item.m_Rate;
            matched &= this.m_Format == item.m_Format;

            return matched;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.Name;
        }
        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            // Ref Tag Info 가 Setting 된경우에만
            if (m_RefTag != null)
            {
                //jemoon : 아래부분은 string 말고 int로 indexing할수 있도록 수정필요함
                //jemoon : 수정하였음
                //m_Value = m_RefTag[m_RefTagDescriptor].Value;
                m_Value = m_RefTag[m_RefTagDescriptor.Id].Value;
            }
        }

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
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                m_TagContainer = m_Server.TagContainer;
                CreateTag(m_TagContainer);

                if (!SyncRefDeviceTag(m_TagContainer))
                {
                    return DmsErrors.InternalError;
                }


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

        public override Type FamilyType
        {
            get
            {
                return typeof(ApdItem);
            }
        }
        #endregion
    }
}
