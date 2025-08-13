using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    public class HpmjItem : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorHpmjItem tagDescriptor = new TagDescriptorHpmjItem();
        #endregion

        #region Fields
        private DeviceTags m_TagContainer = null;
        private int m_MaxUsedTime = 1000;
        private int m_CurUsedTime;
        private bool m_LifeTimeOver;   //현재값이 최대값보다 클경우 true
        public Alarm ALM_LifeTimeOver;
        #endregion

        #region Properties
        [Browsable(false)]
        public int MaxUsedTime
        {
            get { return m_MaxUsedTime; }
            set { m_MaxUsedTime = value; }
        }
        [Browsable(false)]
        public int CurUsedTime
        {
            get { return m_CurUsedTime; }
            set 
            { 
                m_CurUsedTime = value; 
                if(m_Tag != null)
                    UpdateTag();
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool LifeTimeOver
        {
            get { return m_LifeTimeOver; }
            set { m_LifeTimeOver = value; }
        }
        #endregion

        #region Constructor
        public HpmjItem() : this("__")
        {
            //this.Name = "__";
        }

        public HpmjItem(string name) 
        {
            this.Name = name;
        }
        #endregion

        #region Methods
        public HpmjItem Clone()
        {
            HpmjItem newItem = new HpmjItem();
            newItem.Id = this.Id;
            newItem.CurUsedTime = this.CurUsedTime;
            newItem.MaxUsedTime = this.MaxUsedTime;
            newItem.LifeTimeOver = this.LifeTimeOver;
            newItem.Name = this.Name;
            newItem.m_Tag = this.m_Tag;
            newItem.ALM_LifeTimeOver = this.ALM_LifeTimeOver;
            return newItem;
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
            m_Tag[tagDescriptor.CUR_TIME].Value = m_CurUsedTime.ToString();
        }

        public override DmsErrors Initialize()
        {
            if (Initialized == true) return DmsErrors.Success;

            m_TagContainer = m_Server.TagContainer;

            CreateTag(m_TagContainer);

            ALM_LifeTimeOver = new Alarm(this.Name + " Used Time Over Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);

            this.Initialized = true;

            return DmsErrors.Success;
        }

        public override Type FamilyType
        {
            get
            {
                return this.GetType();
            }
        }
        #endregion
    }
}
